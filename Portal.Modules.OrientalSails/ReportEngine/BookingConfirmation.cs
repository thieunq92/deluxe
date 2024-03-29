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
    [Obsolete("Use report engine interface")]
    public class BookingConfirmation
    {
        public static void Emotion(Booking booking, SailsAdminBasePage Page, HttpResponse Response, string templatePath)
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
                        sheet.Cells[currentRow + rows, 4].Value = valueDic[key]/roomDic[key];
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
                    sheet.Cells[currentRow + rows, 5].Value = service.UnitPrice*service.Quantity;
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
    }
}