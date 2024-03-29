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
    public class Emotion: IReportEngine
    {
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

        public void BookingConfirmation(Booking booking, SailsAdminBasePage Page, HttpResponse Response, string templatePath)
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
            sheet.Cells["C21"].Value = booking.Note;

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
                    string key;
                    if (bkroom.IsSingle)
                    {
                        key = string.Format("{0}-{1}|{2}", bkroom.RoomClass.Name, "Single", bkroom.Total);
                    }
                    else
                    {
                        key = string.Format("{0}-{1}|{2}", bkroom.RoomClass.Name, bkroom.RoomType.Name, bkroom.Total);
                    }
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

            const int firstRow = 7;
            int currentRow = firstRow;

            int pax = 0;
            int rooms = 0;
            double grandTotal = 0;

            #region -- Tạo file --

            int index = 0;
            foreach (Booking booking in list)
            {
                // Mỗi track ứng với một dòng
                // Nếu là dòng đầu không cần chèn thêm dòng
                if (currentRow != firstRow)
                {
                    // Chèn thêm dòng
                    sheet.Rows[currentRow].InsertCopy(1, sheet.Rows[currentRow]);
                }

                // Điền các thông tin của dòng
                index++;
                sheet.Cells[currentRow, 0].Value = index; // Số thứ tự
                int id;
                if (Page.UseCustomBookingId)
                {
                    id = booking.CustomBookingId;
                }
                else
                {
                    id = booking.Id;
                }
                sheet.Cells[currentRow, 1].Value = string.Format(Page.BookingFormat, id); // Booking code
                sheet.Cells[currentRow, 2].Value = booking.CreatedDate; // Ngày nhận
                sheet.Cells[currentRow, 3].Value = booking.CreatedBy.FullName; // Người nhận
                if (booking.Agency != null)
                {
                    sheet.Cells[currentRow, 4].Value = booking.Agency.Name; // tên đối tác
                }

                if (booking.IsTransferred)
                {
                    sheet.Cells[currentRow, 5].Value = booking.TransferAdult + booking.TransferChildren +
                                                       booking.TransferBaby; // Số khách
                    pax += booking.TransferAdult + booking.TransferChildren + booking.TransferBaby; // Số khách
                }
                else
                {
                    sheet.Cells[currentRow, 5].Value = booking.Adult + booking.Child + booking.Baby; // Số khách
                    pax += booking.Adult + booking.Child + booking.Baby; // Số khách
                }

                sheet.Cells[currentRow, 6].Value = booking.StartDate; // Ngày check-in
                if (!date.HasValue)
                {
                    date = booking.StartDate;
                }
                sheet.Cells[currentRow, 11].Value = booking.Total; // Tổng tiền booking

                grandTotal += booking.Total;
                rooms += booking.BookingRooms.Count;

                // Dịch vụ sử dụng xuất sau vì có thể tốn thêm dòng
                int rows = 0;
                if (booking.IsCharter)
                {
                    // Charter thì chỉ tốn đúng 1 dòng và 1 dịch vụ duy nhất: charter, đơn giá = tổng giá luôn
                    sheet.Cells[currentRow, 7].Value = 1;
                    sheet.Cells[currentRow, 8].Value = "Charter";
                    sheet.Cells[currentRow, 9].Value = booking.Total;
                    sheet.Cells[currentRow, 10].Value = booking.Total;
                    rows = 1;
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
                        string key;
                        if (bkroom.IsSingle)
                        {
                            key = string.Format("{0}-{1}|{2}", bkroom.RoomClass.Name, "Single", bkroom.Total);
                        }
                        else
                        {
                            key = string.Format("{0}-{1}|{2}", bkroom.RoomClass.Name, bkroom.RoomType.Name, bkroom.Total);
                        }
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
                        sheet.Cells[currentRow + rows, 7].Value = roomDic[key];
                        sheet.Cells[currentRow + rows, 8].Value = key.Substring(0, key.IndexOf("|"));
                        if (Page.CustomPriceForRoom)
                        {
                            sheet.Cells[currentRow + rows, 9].Value = valueDic[key] / roomDic[key];
                            sheet.Cells[currentRow + rows, 10].Value = valueDic[key];
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
                        sheet.Cells[currentRow + rows, 7].Value = extraDic[key];
                        sheet.Cells[currentRow + rows, 8].Value = key.Name;
                        if (Page.CustomPriceForRoom)
                        {
                            sheet.Cells[currentRow + rows, 9].Value = extraPrices[key];
                            sheet.Cells[currentRow + rows, 10].Value = extraPrices[key] * extraDic[key];
                            total += extraPrices[key] * extraDic[key];
                        }
                        rows++;
                    }

                    foreach (BookingService service in booking.Services)
                    {
                        sheet.Cells[currentRow + rows, 8].Value = service.Name;
                        sheet.Cells[currentRow + rows, 7].Value = service.Quantity;
                        sheet.Cells[currentRow + rows, 9].Value = service.UnitPrice;
                        sheet.Cells[currentRow + rows, 10].Value = service.UnitPrice * service.Quantity;
                        total += service.UnitPrice * service.Quantity;
                        rows++;
                    }

                    #endregion

                    // Lấy giá tổng thực tế so sánh với tổng dịch vụ
                    if (booking.Total != total)
                    {
                        // Không bằng nhau thì xóa hai cột đơn giá và thành tiền
                        for (int ii = currentRow; ii < currentRow + rows; ii++)
                        {
                            sheet.Cells[ii, 9].Value = string.Empty;
                            sheet.Cells[ii, 10].Value = string.Empty;
                        }
                    }

                    if (rows == 0)
                    {
                        rows = 1;
                    }
                }
                sheet.Cells[currentRow, 12].Value = booking.SpecialRequest;
                sheet.Cells[currentRow, 13].Value = booking.PickupAddress;
                currentRow += rows;
            }

            sheet.Cells["E4"].Value = pax;
            sheet.Cells["E5"].Value = rooms;
            sheet.Cells["K5"].Value = grandTotal;
            sheet.Cells["I5"].Value = date;

            #endregion

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

        public void Provisional(IList list, DateTime date, Purpose defaultPurpose , SailsAdminBasePage Page, HttpResponse Response, string templatePath)
        {
            // Bắt đầu thao tác export
            ExcelFile excelFile = new ExcelFile();
            excelFile.LoadXls(templatePath);
            // Mở sheet 0
            ExcelWorksheet sheet = excelFile.Worksheets[0];

            #region -- Xuất dữ liệu --

            sheet.Cells["F8"].Value = date;

            const int firstrow = 11;
            //sheet.Rows[firstrow].InsertCopy(totalRows - 1, sheet.Rows[firstrow]);
            int curr = firstrow;
            foreach (Booking booking in list)
            {
                foreach (BookingRoom room in booking.BookingRooms)
                {
                    foreach (Customer customer in room.Customers)
                    {
                        if (room.Room != null)
                        {
                            sheet.Cells[curr, 0].Value = room.Room.Name;
                        }
                        if (!string.IsNullOrEmpty(customer.Fullname))
                        {
                            sheet.Cells[curr, 1].Value = customer.Fullname;
                        }
                        else
                        {
                            continue; // Bỏ qua customer không tên này
                        }

                        if (customer.IsMale.HasValue && !customer.IsMale.Value)
                        {
                            sheet.Cells[curr, 2].Value = "Nữ";
                        }
                        else
                        {
                            sheet.Cells[curr, 2].Value = "Nam";
                        }

                        if (customer.Birthday.HasValue)
                        {
                            sheet.Cells[curr, 3].Value = customer.Birthday.Value;
                        }

                        //sheet.Cells[curr, 4].Value = customer.Country;
                        if (customer.Nationality != null)
                        {
                            sheet.Cells[curr, 4].Value = customer.Nationality.Code;
                        }
                        else
                        {
                            sheet.Cells[curr, 4].Value = "KHONG RO";
                        }
                        sheet.Cells[curr, 5].Value = customer.Passport;

                        if (customer.IsVietKieu)
                        {
                            sheet.Cells[curr, 6].Value = "VK | Việt Kiều";
                        }
                        else if (customer.Nationality != null && customer.Nationality.Code.ToLower() == "vietnam")
                        {
                            sheet.Cells[curr, 6].Value = "VN | Việt Nam";
                        }
                        else
                        {
                            sheet.Cells[curr, 6].Value = "NN | Nước ngoài";
                        }

                        //sheet.Cells[curr, 7].Value = customer.Purpose;
                        if (customer.StayPurpose != null)
                        {
                            sheet.Cells[curr, 7].Value = customer.StayPurpose.Code;
                        }
                        else
                        {
                            sheet.Cells[curr, 7].Value = defaultPurpose.Code;
                        }
                        sheet.Cells[curr, 8].Value = customer.Booking.StartDate;
                        sheet.Cells[curr, 9].Value = customer.Booking.EndDate;
                        sheet.Cells[curr, 10].Value = customer.StayTerm;
                        if (booking.Agency!=null)
                        {
                            sheet.Cells[curr, 11].Value = booking.Agency.Name;
                        }
                        //if (string.IsNullOrEmpty(customer.StayIn))
                        //{
                        //    if (booking.Agency != null)
                        //    {
                        //        sheet.Cells[curr, 11].Value = booking.Agency.Name;
                        //    }
                        //}
                        //else
                        //{
                        //    sheet.Cells[curr, 11].Value = customer.StayIn;
                        //}
                        curr++;
                    }
                }
            }

            #endregion

            #region -- Trả dữ liệu về cho người dùng --

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("content-disposition",
                                  "attachment; filename=" + string.Format("TamTru{0:dd_MMM}", date));

            MemoryStream m = new MemoryStream();

            excelFile.SaveXls(m);

            Response.OutputStream.Write(m.GetBuffer(), 0, m.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();

            m.Close();
            Response.End();

            #endregion
        }
    }
}
