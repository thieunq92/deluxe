using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using GemBox.Spreadsheet;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;

namespace Portal.Modules.OrientalSails.ReportEngine
{
    public class IndochinaJunk : IReportEngine
    {
        #region Implementation of IReportEngine

        public void CustomerDetails(IList customers, DateTime startDate, string tplPath, HttpResponse response)
        {
            ExcelFile excelFile = new ExcelFile();
            excelFile.LoadXls(tplPath);

            ExcelWorksheet sheet = excelFile.Worksheets[0];
            // Dòng dữ liệu đầu tiên
            const int firstrow = 7;
            int crow = firstrow;

            IList checkinCustomers = new ArrayList();
            foreach (Customer customer in customers)
            {
                if (customer.Booking.StartDate == startDate)
                {
                    checkinCustomers.Add(customer);
                }
            }

            sheet.Rows[crow].InsertCopy(checkinCustomers.Count - 1, sheet.Rows[firstrow]);

            sheet.Cells["F2"].Value = checkinCustomers.Count;
            IList countedRoom = new ArrayList();
            foreach (Customer customer in checkinCustomers)
            {
                sheet.Cells[crow, 0].Value = crow - firstrow + 1;
                sheet.Cells[crow, 1].Value = customer.Fullname;
                if (customer.IsMale.HasValue)
                {
                    if (customer.IsMale.Value)
                    {
                        sheet.Cells[crow, 2].Value = "Nam";
                    }
                    else
                    {
                        sheet.Cells[crow, 2].Value = "Nữ";
                    }
                }

                if (customer.Birthday.HasValue)
                {
                    sheet.Cells[crow, 3].Value = customer.Birthday.Value;
                }

                if (customer.Nationality != null)
                {
                    sheet.Cells[crow, 4].Value = customer.Nationality.Code;
                }
                sheet.Cells[crow, 5].Value = customer.Passport;
                sheet.Cells[crow, 6].Value = customer.StayTerm;

                sheet.Cells[crow, 7].Value = customer.Booking.StartDate;
                sheet.Cells[crow, 8].Value = customer.Booking.EndDate;
                sheet.Cells[crow, 9].Value = (customer.Booking.EndDate - customer.Booking.StartDate).Days;

                sheet.Cells[crow, 10].Value = string.Format("{0} {1}", customer.BookingRoom.RoomClass.Name,
                                                           customer.BookingRoom.RoomType.Name);
                if (customer.BookingRoom.Room != null)
                {
                    sheet.Cells[crow, 11].Value = customer.BookingRoom.Room.Name;
                }

                sheet.Cells[crow, 12].Value = customer.Booking.CustomBookingId;

                if (customer.Booking.Agency != null)
                {
                    sheet.Cells[crow, 13].Value = customer.Booking.Agency.Name;
                }
                sheet.Cells[crow, 14].Value = customer.StayIn;

                if (customer.Birthday.HasValue)
                {
                    if (customer.Booking.StartDate.DayOfYear <= customer.Birthday.Value.DayOfYear && customer.Booking.EndDate.DayOfYear >= customer.Birthday.Value.DayOfYear)
                    {
                        sheet.Cells[crow, 15].Value = "BIRTHDAY";
                    }
                }

                crow += 1;
                if (!countedRoom.Contains(customer.BookingRoom))
                {
                    countedRoom.Add(customer.BookingRoom);
                }
            }

            sheet.Cells["H2"].Value = countedRoom.Count;
            sheet.Cells["O1"].Value = startDate;
            response.Clear();
            response.Buffer = true;
            response.ContentType = "application/vnd.ms-excel";
            response.AppendHeader("content-disposition",
                                  "attachment; filename=" + string.Format("customer{0:dd_MMM}", startDate));

            MemoryStream m = new MemoryStream();

            excelFile.SaveXls(m);

            response.OutputStream.Write(m.GetBuffer(), 0, m.GetBuffer().Length);
            response.OutputStream.Flush();
            response.OutputStream.Close();

            m.Close();
            response.End();
        }

        public void BookingConfirmation(Booking booking, SailsAdminBasePage Page, HttpResponse Response,
                                        string templatePath)
        {
            IList bkServices = Page.Module.ExtraOptionGetBooking();
            IList customerServices = Page.Module.ExtraOptionGetCustomer();

            // Bắt đầu thao tác export
            ExcelFile excelFile = new ExcelFile();
            excelFile.LoadXls(templatePath);
            // Mở sheet 0
            ExcelWorksheet sheet = excelFile.Worksheets[0];

            #region -- Thông tin booking --

            if (booking.Agency != null)
            {
                if (booking.Booker != null)
                {
                    sheet.Cells["E8"].Value = booking.Booker.Name;
                    sheet.Cells["B10"].Value = "Dear " + booking.Booker.Name;
                }
                else
                {
                    sheet.Cells["B10"].Value = "Dear " + booking.Agency.Name;
                }
                sheet.Cells["E9"].Value = booking.Agency.Name;
                sheet.Cells["E10"].Value = booking.Agency.Phone;
                sheet.Cells["E11"].Value = booking.Agency.Fax;
            }

            if (!string.IsNullOrEmpty(booking.AgencyCode))
            {
                sheet.Cells["F13"].Value = booking.AgencyCode;
            }

            if (Page.UseCustomBookingId)
            {
                sheet.Cells["C16"].Value = string.Format(Page.BookingFormat, booking.CustomBookingId);
            }
            else
            {
                sheet.Cells["C16"].Value = string.Format(Page.BookingFormat, booking.Id);
            }

            sheet.Cells["C17"].Value = booking.Trip.NumberOfDay - 1;
            sheet.Cells["C20"].Value = booking.CreatedDate;
            sheet.Cells["F17"].Value = booking.StartDate;
            sheet.Cells["F18"].Value = booking.EndDate;
            sheet.Cells["F19"].Value = booking.Adult + booking.Child;

            const int currentRow = 24;
            int rows = 0;
            if (booking.IsCharter)
            {
                // Charter thì chỉ tốn đúng 1 dòng và 1 dịch vụ duy nhất: charter, đơn giá = tổng giá luôn
                sheet.Cells[currentRow, 3].Value = 1;
                sheet.Cells[currentRow, 2].Value = "Charter";
                sheet.Cells[currentRow, 4].Value = booking.Total;
                sheet.Cells[currentRow, 5].Value = booking.Total;
            }
            else
            {
                // Các dịch vụ

                double total = 0;

                #region -- Thuê phòng --

                Dictionary<string, int> roomDic = new Dictionary<string, int>();
                Dictionary<string, double> valueDic = new Dictionary<string, double>();
                foreach (BookingRoom bkroom in booking.BookingRooms)
                {
                    string key = string.Format("{0}-{1}|{2}", bkroom.RoomClass.Name, bkroom.RoomType.Name, bkroom.Total);
                    //TODO: Tính đơn giá kể cả trong trường hợp không tính giá thủ công
                    if (roomDic.ContainsKey(key))
                    {
                        roomDic[key]++;
                        if (Page.CustomPriceForRoom)
                        {
                            valueDic[key] += bkroom.Total;
                        }
                    }
                    else
                    {
                        roomDic.Add(key, 1);
                        if (Page.CustomPriceForRoom)
                        {
                            valueDic.Add(key, bkroom.Total);
                        }
                    }
                }

                if (roomDic.Keys.Count > 1)
                {
                    sheet.Rows[currentRow + 1].InsertCopy(roomDic.Keys.Count - 1, sheet.Rows[currentRow + 1]);
                    // Chèn thêm n-1 bản sao của dòng tiếp theo vào trước dòng tiếp theo (tức là sau dòng hiện tại)
                }
                // Mỗi loại phòng vào 1 dịch vụ
                foreach (string key in roomDic.Keys)
                {
                    sheet.Cells[currentRow + rows, 3].Value = roomDic[key];
                    sheet.Cells[currentRow + rows, 2].Value = key.Substring(0, key.IndexOf("|"));
                    if (Page.CustomPriceForRoom)
                    {
                        sheet.Cells[currentRow + rows, 4].Value = valueDic[key] / roomDic[key];
                        sheet.Cells[currentRow + rows, 5].Value = valueDic[key];
                        total += valueDic[key];
                    }
                    rows++;
                }

                #endregion

                #region -- Dịch vụ thêm --

                Dictionary<ExtraOption, int> extraDic = new Dictionary<ExtraOption, int>();
                Dictionary<ExtraOption, double> extraPrices = new Dictionary<ExtraOption, double>();
                IList customPrices = Page.Module.ServicePriceGetByBooking(booking);
                foreach (ExtraOption option in bkServices)
                {
                    if (booking.ExtraServices.Contains(option))
                    {
                        extraDic.Add(option, booking.Adult + booking.Child);
                        foreach (BookingServicePrice price in customPrices)
                        {
                            if (price.ExtraOption == option)
                            {
                                extraPrices.Add(option, price.UnitPrice);
                            }
                        }

                        if (!extraPrices.ContainsKey(option))
                        {
                            extraPrices.Add(option, option.Price);
                        }
                    }
                }

                foreach (ExtraOption option in customerServices)
                {
                    int count = Page.Module.CustomerServiceCountByBooking(option, booking);
                    if (count > 0)
                    {
                        extraDic.Add(option, count);
                        foreach (BookingServicePrice price in customPrices)
                        {
                            if (price.ExtraOption == option)
                            {
                                extraPrices.Add(option, price.UnitPrice);
                            }
                        }

                        if (!extraPrices.ContainsKey(option))
                        {
                            extraPrices.Add(option, option.Price);
                        }
                    }
                }

                if (extraDic.Keys.Count > 0)
                {
                    sheet.Rows[currentRow + rows].InsertCopy(extraDic.Keys.Count, sheet.Rows[currentRow + rows]);
                    // Chèn thêm n bản sao của dòng tiếp theo vào trước dòng tiếp theo (tức là sau dòng hiện tại)
                }
                foreach (ExtraOption key in extraDic.Keys)
                {
                    sheet.Cells[currentRow + rows, 3].Value = extraDic[key];
                    sheet.Cells[currentRow + rows, 2].Value = key.Name;
                    if (Page.CustomPriceForRoom)
                    {
                        sheet.Cells[currentRow + rows, 4].Value = extraPrices[key];
                        sheet.Cells[currentRow + rows, 5].Value = extraPrices[key] * extraDic[key];
                        total += extraPrices[key] * extraDic[key];
                    }
                    rows++;
                }

                foreach (BookingService service in booking.Services)
                {
                    sheet.Cells[currentRow + rows, 2].Value = service.Name;
                    sheet.Cells[currentRow + rows, 3].Value = service.Quantity;
                    sheet.Cells[currentRow + rows, 4].Value = service.UnitPrice;
                    sheet.Cells[currentRow + rows, 5].Value = service.UnitPrice * service.Quantity;
                    total += service.UnitPrice * service.Quantity;
                    rows++;
                }

                if (booking.Total != total)
                {
                    // Không bằng nhau thì xóa hai cột đơn giá và thành tiền
                    for (int ii = currentRow; ii < currentRow + rows; ii++)
                    {
                        sheet.Cells[ii, 4].Value = string.Empty;
                        sheet.Cells[ii, 5].Value = string.Empty;
                    }
                }

                #endregion
            }

            sheet.Cells["F34"].Value = booking.Total;
            if (booking.CreatedBy != null)
            {
                sheet.Cells["B48"].Value = booking.CreatedBy.FullName;
            }

            #endregion

            #region -- Trả dữ liệu về cho người dùng --

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("content-disposition",
                                  "attachment; filename=" + string.Format("Confirm{0}.xls", booking.Id));

            MemoryStream m = new MemoryStream();

            excelFile.SaveXls(m);

            Response.OutputStream.Write(m.GetBuffer(), 0, m.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();

            m.Close();
            Response.End();

            #endregion
        }

        public void IncomeByDate(IList list, SailsAdminBasePage Page, HttpResponse Response, string templatePath)
        {
            IList bkServices = Page.Module.ExtraOptionGetBooking();
            IList customerServices = Page.Module.ExtraOptionGetCustomer();

            // Bắt đầu thao tác export
            ExcelFile excelFile = new ExcelFile();
            excelFile.LoadXls(templatePath);
            // Mở sheet 0
            ExcelWorksheet sheet = excelFile.Worksheets[0];

            DateTime? date = null;

            const int firstCruise = 11;
            const int firstrow = 12;
            int curr = firstrow;

            #region -- Chuyển từ bk sang room --

            IList rooms = new ArrayList();
            foreach (Booking booking in list)
            {
                foreach (BookingRoom bkroom in booking.BookingRooms)
                {
                    rooms.Add(bkroom);
                }
            }
            sheet.Rows[firstrow].InsertCopy(rooms.Count - 1, sheet.Rows[firstrow]);

            #endregion

            int firstCRow = firstCruise; // Dòng đầu tiên của một tàu
            Cruise cruise = null;
            foreach (BookingRoom bkroom in rooms)
            {
                if (bkroom.Book.Cruise!=cruise)
                {
                    int cRow = firstCruise;
                    if (cruise != null) // Là null chứng tỏ mới chỉ là tàu đầu tiên
                    {
                        sheet.Rows[curr].InsertCopy(1, sheet.Rows[firstCruise]);
                        cRow = curr;
                    }
                    cruise = bkroom.Book.Cruise;
                    
                    sheet.Cells[cRow, 0].Value = cruise.Name;

                    if (cRow == curr)
                    {
                        curr += 1;
                    }
                    firstCRow = cRow;
                }

                sheet.Cells[curr, 0].Value = curr - firstCRow;
                if (bkroom.Book.Agency!=null)
                {
                    sheet.Cells[curr, 1].Value = bkroom.Book.Agency.Name;
                    sheet.Cells[curr, 2].Value = bkroom.Book.AgencyCode;
                }
                sheet.Cells[curr, 3].Value = bkroom.Book.Trip.TripCode;
                sheet.Cells[curr, 4].Value = bkroom.RealCustomers.Count;
                sheet.Cells[curr, 5].Value = bkroom.Total;
                sheet.Cells[curr, 6].Value = 1;
                sheet.Cells[curr, 7].Value = bkroom.Total;
                sheet.Cells[curr, 8].Value = 1;
                sheet.Cells[curr, 13].Value = 1;

                if (bkroom.Book.Agency!=null)
                {
                    if (bkroom.Book.Agency.PaymentPeriod == PaymentPeriod.Monthly)
                    {
                        sheet.Cells[curr, 16].Value = "Yes";
                    }
                    if (bkroom.Book.Agency.PaymentPeriod == PaymentPeriod.MonthlyCK)
                    {
                        sheet.Cells[curr, 17].Value = "Yes";
                    }
                }

                if (bkroom.Book.HasInvoice)
                {
                    sheet.Cells[curr, 18].Value = "Yes";
                }
                else
                {
                    sheet.Cells[curr, 18].Value = "No";
                }

                sheet.Cells[curr, 20].Value = bkroom.Book.PickupAddress;

                curr += 1;
            }

            #region -- Trả dữ liệu về cho người dùng --

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("content-disposition",
                                  "attachment; filename=" + string.Format("DoanhThuNgay{0:dd_MMM_yyyy}.xls", date));

            MemoryStream m = new MemoryStream();

            excelFile.SaveXls(m);

            Response.OutputStream.Write(m.GetBuffer(), 0, m.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();

            m.Close();
            Response.End();

            #endregion
        }

        public void Provisional(IList list, DateTime date, Purpose defaultPurpose, SailsAdminBasePage Page, HttpResponse Response, string templatePath)
        {
            Emotion emo = new Emotion();
            emo.Provisional(list, date, defaultPurpose, Page, Response, templatePath);
            //// Bắt đầu thao tác export
            //ExcelFile excelFile = new ExcelFile();
            //excelFile.LoadXls(templatePath);
            //// Mở sheet 0
            //ExcelWorksheet sheet = excelFile.Worksheets[0];

            //#region -- Xuất dữ liệu --

            //sheet.Cells["E3"].Value = string.Format("Ngày {0} tháng {1} năm {2}", date.Day, date.Month, date.Year);

            //const int firstrow = 8;            
            //int curr = firstrow;
            //foreach (Booking booking in list)
            //{
            //    foreach (BookingRoom room in booking.BookingRooms)
            //    {
            //        foreach (Customer customer in room.Customers)
            //        {
            //            sheet.Cells[curr, 0].Value = curr - firstrow + 1;
            //            if (!string.IsNullOrEmpty(customer.Fullname))
            //            {
            //                sheet.Cells[curr, 1].Value = customer.Fullname;
            //            }
            //            else
            //            {
            //                continue; // Bỏ qua customer không tên này
            //            }
                        
            //            if (customer.IsMale.HasValue)
            //            {
            //                int col = 3;
            //                if (customer.IsMale.Value)
            //                {
            //                    col = 2;
            //                }
            //                if (customer.Birthday.HasValue)
            //                {
            //                    sheet.Cells[curr, col].Value = customer.Birthday;
            //                }
            //            }

            //            if (customer.Nationality!=null)
            //            {
            //                sheet.Cells[curr, 5].Value = customer.Nationality.Name;
            //            }
            //            sheet.Cells[curr, 6].Value = customer.Passport;
            //            sheet.Cells[curr, 7].Value = customer.StayTerm;
            //            if (booking.Agency!=null)
            //            {
            //                sheet.Cells[curr, 9].Value = booking.Agency.Name;
            //            }
            //            curr++;
            //        }
            //    }
            //}

            //#endregion

            //#region -- Trả dữ liệu về cho người dùng --

            //Response.Clear();
            //Response.Buffer = true;
            //Response.ContentType = "application/vnd.ms-excel";
            //Response.AppendHeader("content-disposition",
            //                      "attachment; filename=" + string.Format("TamTru{0:dd_MMM}", date));

            //MemoryStream m = new MemoryStream();

            //excelFile.SaveXls(m);

            //Response.OutputStream.Write(m.GetBuffer(), 0, m.GetBuffer().Length);
            //Response.OutputStream.Flush();
            //Response.OutputStream.Close();

            //m.Close();
            //Response.End();

            //#endregion
        }

        public void CustomerComment(DateTime date)
        {
            
        }

        #endregion
    }
}