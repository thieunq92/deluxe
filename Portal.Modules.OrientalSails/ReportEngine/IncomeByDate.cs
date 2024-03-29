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
    [Obsolete("User report engine interface")]
    public class IncomeByDate
    {
        public static void Emotion(IList list, SailsAdminBasePage Page, HttpResponse Response, string templatePath)
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
                        sheet.Cells[currentRow + rows, 7].Value = roomDic[key];                        
                        sheet.Cells[currentRow + rows, 8].Value = key.Substring(0, key.IndexOf("|"));
                        if (Page.CustomPriceForRoom)
                        {
                            sheet.Cells[currentRow + rows, 9].Value = valueDic[key]/roomDic[key];
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
                sheet.Cells[currentRow, 11].Value = booking.SpecialRequest;
                sheet.Cells[currentRow, 11].Value = booking.PickupAddress;
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
    }
}