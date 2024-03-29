using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using GemBox.Spreadsheet;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails.ReportEngine
{
    public class TourCommand
    {
        public static void Export(IList list, int count, IList expenseList, DateTime _date, string bookingFormat, HttpResponse Response, string templatePath, SailsTrip trip, SailsAdminBasePage page)
        {
            SailsModule module = page.Module;
            // Bắt đầu thao tác export

            ExcelFile excelFile = new ExcelFile();
            excelFile.LoadXls(templatePath);

            // Số lệnh điều tua = số lượng group
            int numberOfGroup = -1;

            foreach (Booking booking in list)
            {
                if (booking.Group > numberOfGroup)
                {
                    numberOfGroup = booking.Group;
                }
            }

            foreach (ExpenseService service in expenseList)
            {
                if (service.Group > numberOfGroup)
                {
                    numberOfGroup = service.Group;
                }
            }

            if (numberOfGroup <= 0)
            {
                numberOfGroup = 1; // Tối thiểu là 1 group
            }

            #region -- sheet 0: Lệnh điều tour --
            for (int ii = 0; ii <= numberOfGroup; ii++)
            {
                // Mở sheet 0
                ExcelWorksheet originSheet = excelFile.Worksheets[0];

                int numberOfBooking = 0;
                foreach (Booking booking in list)
                {
                    if (booking.Group == ii)
                    {
                        numberOfBooking++;
                    }
                }

                if (numberOfBooking == 0 && ii == 0) continue;

                ExcelWorksheet sheet = excelFile.Worksheets.AddCopy("LenhDieuTour" + ii, originSheet);

                #region -- Thông tin chung --

                if (expenseList != null)
                {
                    // Các thông tin chèn thêm
                    foreach (ExpenseService service in expenseList)
                    {
                        if (service.Group != ii) continue;

                        switch (service.Type.Id)
                        {
                            case SailsModule.GUIDE_COST:
                                if (service.Type.IsCustomType || service.Supplier == null)
                                {
                                    if (string.IsNullOrEmpty(service.Name) && service.Supplier != null)
                                    {
                                        sheet.Cells["C3"].Value = service.Supplier.Name;
                                    }
                                    else
                                    {
                                        sheet.Cells["C3"].Value = service.Name;
                                    }
                                    if (string.IsNullOrEmpty(service.Name) && service.Supplier != null)
                                    {
                                        sheet.Cells["F3"].Value = service.Supplier.Phone;
                                    }
                                    else
                                    {
                                        sheet.Cells["F3"].Value = service.Phone;
                                    }
                                }
                                else
                                {
                                    sheet.Cells["C3"].Value = service.Supplier.Name;
                                    sheet.Cells["f3"].Value = service.Supplier.Phone;
                                }
                                break;
                            case SailsModule.TRANSPORT:
                                if (service.Type.IsCustomType || service.Supplier == null)
                                {
                                    if (string.IsNullOrEmpty(service.Name) && service.Supplier != null)
                                    {
                                        sheet.Cells["C4"].Value = service.Supplier.Name;
                                    }
                                    else
                                    {
                                        sheet.Cells["C4"].Value = service.Name;
                                    }
                                    if (string.IsNullOrEmpty(service.Name) && service.Supplier != null)
                                    {
                                        sheet.Cells["F4"].Value = service.Supplier.Phone;
                                    }
                                    else
                                    {
                                        sheet.Cells["F4"].Value = service.Phone;
                                    }
                                }
                                else
                                {
                                    sheet.Cells["C4"].Value = service.Supplier.Name;
                                    sheet.Cells["f4"].Value = service.Supplier.Phone;
                                }
                                break;
                            case SailsModule.HOTEL:
                                if (service.Supplier != null)
                                {
                                    sheet.Cells["C7"].Value = service.Supplier.Name;
                                }
                                else
                                {
                                    sheet.Cells["C7"].Value = service.Name;
                                }
                                break;
                        }
                    }
                }

                sheet.Cells["D1"].Value = _date;

                // Tính tổng pax
                int adult = 0;
                int child = 0;
                int baby = 0;

                int pAdult = 0;
                int pChild = 0;
                int pBaby = 0;

                foreach (Booking booking in list)
                {

                    if (booking.StartDate == _date && booking.Group == ii)
                    {
                        adult += booking.Adult;
                        child += booking.Child;
                        baby += booking.Baby;
                    }
                    else if (booking.Group == ii)
                    {
                        pAdult += booking.Adult;
                        pChild += booking.Child;
                        pBaby += booking.Baby;
                    }
                }

                int pax = adult + child + pAdult + pChild;

                sheet.Cells["C5"].Value = page.UserIdentity.FullName;
                sheet.Cells["C6"].Value = pax;
                sheet.Cells["C12"].Value = adult;
                sheet.Cells["D12"].Value = child;
                sheet.Cells["E12"].Value = baby;

                sheet.Cells["C18"].Value = pAdult;
                sheet.Cells["D18"].Value = pChild;
                sheet.Cells["E18"].Value = pBaby;

                #endregion

                #region -- Sao chép các dòng --

                // Sao chép dòng đầu theo số lượng booking
                // Dòng đầu tiên là dòng 11
                const int firstrow = 10;

                // Đếm số book trong ngày
                int curr = 0;
                foreach (Booking booking in list)
                {
                    if (booking.StartDate == _date && booking.Group == ii)
                    {
                        curr += 1;
                    }
                }

                sheet.Rows[16].InsertCopy(count - curr - 1, sheet.Rows[firstrow]);
                if (curr > 0)
                {
                    sheet.Rows[firstrow].InsertCopy(curr - 1, sheet.Rows[firstrow]);
                }
                int firstProw = 16 + curr;

                #endregion

                #region -- Thông tin từng booking --

                // Ghi vào file excel theo từng dòng
                int crow = firstrow;
                int cProw = firstProw - 1;
                foreach (Booking booking in list)
                {
                    if (booking.Group != ii) continue;
                    sheet.Cells["D2"].Value = string.Format("{0}{1:ddMMyy}-{2:00}", booking.Trip.TripCode, booking.StartDate, booking.Group);
                    int current;
                    int index;
                    if (booking.StartDate != _date)
                    {
                        // Dòng previous hiện tại
                        current = cProw;
                        // Index = cột previous hiện tại - previous đầu tiên
                        index = cProw - firstProw + 2;
                        cProw++;
                    }
                    else
                    {
                        current = crow;
                        index = crow - firstrow + 1;
                        crow++;
                    }
                    sheet.Cells[current, 0].Value = index; // Cột index
                    string name = booking.CustomerName.Replace("<br/>", "\n");
                    if (name.Length > 0)
                    {
                        name = name.Remove(name.Length - 1);
                    }
                    sheet.Cells[current, 1].Value = name; // Cột name
                    sheet.Cells[current, 2].Value = booking.Adult; // Cột adult
                    sheet.Cells[current, 3].Value = booking.Child; // Cột child
                    sheet.Cells[current, 4].Value = booking.Baby; // Cột baby
                    sheet.Cells[current, 5].Value = booking.Trip.TripCode; // Cột trip code
                    sheet.Cells[current, 6].Value = booking.PickupAddress; // Cột pickup address
                    sheet.Cells[current, 7].Value = booking.SpecialRequest; // Cột special request

                    sheet.Cells[current, 9].Style.Borders.SetBorders(MultipleBorders.Outside, System.Drawing.Color.Black, LineStyle.Thin);

                    if (booking.Agency != null)
                    {
                        sheet.Cells[current, 8].Value = booking.Agency.Name; // Cột agency
                        sheet.Cells[current, 9].Value = string.Format(bookingFormat, booking.Id) + "\r\n" + booking.AgencyCode;
                    }
                    else
                    {
                        sheet.Cells[current, 8].Value = SailsModule.NOAGENCY; // Hiển thị oriental nếu ko có agency

                        if (booking.CustomBookingId > 0)
                        {
                            sheet.Cells[current, 9].Value = string.Format(bookingFormat, booking.CustomBookingId);

                        }
                        else
                        {
                            sheet.Cells[current, 9].Value = string.Format(bookingFormat, booking.Id);

                        }

                    }

                    if (booking.PickupTime != null)
                    {
                        sheet.Cells[current, 10].Value = booking.PickupTime.Value.ToString("HH:mm");
                    }

                    if (booking.SeeoffTime != null)
                    {
                        sheet.Cells[current, 11].Value = booking.SeeoffTime.Value.ToString("HH:mm");
                    }

                    if (trip.Name.ToLower() != "airport transfer")
                    {
                        sheet.Columns[11].Delete();
                    }

                    if (trip.Name.ToLower() != "airport transfer")
                    {
                        sheet.Columns[10].Delete();
                    }
                }


                #endregion

                // Mở sheet 1
                originSheet = excelFile.Worksheets[1];
                ExcelWorksheet budget = excelFile.Worksheets.AddCopy("Budget" + ii, originSheet);

                budget.Cells["C3"].Value = sheet.Cells["C3"].Value;
                budget.Cells["C4"].Value = sheet.Cells["C4"].Value;
                budget.Cells["A1"].Value = sheet.Cells["D1"].Value;
                budget.Cells["C5"].Value = page.UserIdentity.FullName;
                budget.Cells["C6"].Value = sheet.Cells["C6"].Value;

                budget.Cells["A1"].Value = string.Format("{0}{1:ddMMyyyy}", trip.TripCode, _date);

                // Các chi phí xuất ra bao gồm chi phí theo ngày và chi phí theo khách
                ExpenseCalculator calculator = new ExpenseCalculator(module, true);
                SailExpense expense = module.ExpenseGetByDate(trip, _date);
                IList expenses = module.ExpenseServiceGet(trip, _date, _date, null, null, null, null, false, null, 0, "");


                CostingTable cost = calculator.GetCurrentTable(_date, trip, TripOption.Option1);
                IList costTypes = new ArrayList();
                foreach (KeyValuePair<CostType, double> entry in module.CostTypeGetAutoDailyBase())
                {
                    costTypes.Add(entry.Key);
                }
                Dictionary<CostType, Costing> unitPrices = cost.GetCostMap(costTypes);


                const int first = 8;
                curr = first;

                foreach (ExpenseService entry in expenses)
                {
                    if (entry.Group != ii) continue;

                    if (entry.Type.IsPayNow)
                    {
                        if (entry.Type.IsDailyInput)// Nếu là chi phí nhập tay
                        {
                            if (entry.Cost == 0) continue;
                            budget.Rows[curr].InsertCopy(1, budget.Rows[curr]);
                            budget.Cells[curr, 0].Value = curr - first + 1;
                            budget.Cells[curr, 1].Value = _date;
                            if (entry.Type.Name.ToLower().Contains("others"))
                            {
                                budget.Cells[curr, 2].Value = entry.Name;
                            }
                            else
                            {
                                budget.Cells[curr, 2].Value = entry.Type.Name;
                            }
                            budget.Cells[curr, 3].Value = entry.Cost; // chi phí dành cho cả đoàn
                            budget.Cells[curr, 4].Value = entry.Cost;
                            curr++;
                        }
                        else if (!entry.Type.IsMonthly && !entry.Type.IsYearly)
                        {
                            double value = unitPrices[entry.Type].Adult * adult;

                            if (value > 0)
                            {
                                budget.Rows[curr].InsertCopy(1, budget.Rows[curr]);
                                budget.Cells[curr, 0].Value = curr - first + 1;
                                budget.Cells[curr, 1].Value = _date;
                                budget.Cells[curr, 2].Value = entry.Type.Name + " (adult)";
                                budget.Cells[curr, 3].Value = unitPrices[entry.Type].Adult; // giá adult                    
                                budget.Cells[curr, 4].Value = value;
                                curr++;
                            }

                            value = unitPrices[entry.Type].Child * child;

                            if (value > 0)
                            {
                                budget.Rows[curr].InsertCopy(1, budget.Rows[curr]);
                                budget.Cells[curr, 0].Value = curr - first + 1;
                                budget.Cells[curr, 1].Value = _date;
                                budget.Cells[curr, 2].Value = entry.Type.Name + " (child)";
                                budget.Cells[curr, 3].Value = unitPrices[entry.Type].Child; // giá adult                    
                                budget.Cells[curr, 4].Value = value;
                                curr++;
                            }

                            value = unitPrices[entry.Type].Baby * baby;

                            if (value > 0)
                            {
                                budget.Rows[curr].InsertCopy(1, budget.Rows[curr]);
                                budget.Cells[curr, 0].Value = curr - first + 1;
                                budget.Cells[curr, 1].Value = _date;
                                budget.Cells[curr, 2].Value = entry.Type.Name + " (baby)";
                                budget.Cells[curr, 3].Value = unitPrices[entry.Type].Baby; // giá adult                    
                                budget.Cells[curr, 4].Value = value;
                                curr++;
                            }
                        }
                    }
                }
            }

            #endregion

            #region -- sheet 1:
        //if (excelFile.Worksheets.Count == 1)
        //{
        //    goto end;
        //}
        //ExcelWorksheet budget = excelFile.Worksheets[1];            
            #endregion

            #region -- Trả dữ liệu về cho người dùng --

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("content-disposition",
                                  "attachment; filename=" + string.Format("Lenhdieutour{0:dd_MMM}.xls", _date));

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

    public class BookingChanges
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

            const int firstRow = 7;
            int currentRow = firstRow;

            #region  Danh sách dữ liệu là danh sách tracking, chuyển về thành danh sách booking

            IList bookings = new ArrayList();
            Dictionary<Booking, double> changes = new Dictionary<Booking, double>();
            foreach (BookingTrack track in list)
            {
                if (!bookings.Contains(track.Booking))
                {
                    bookings.Add(track.Booking);
                    // Thêm mới sự thay đổi
                    changes.Add(track.Booking, 0);
                }
                // dù mới hay cũ đều cộng thêm sự thay đổi
                double total = 0;
                foreach (BookingChanged change in track.Changes)
                {
                    switch (change.Action)
                    {
                        case BookingAction.ChangeTotal:
                            total += Convert.ToDouble(change.Parameter);
                            break;
                        case BookingAction.Approved:
                            total += Convert.ToDouble(change.Parameter);
                            break;
                        case BookingAction.Cancelled:
                            total += -Convert.ToDouble(change.Parameter);
                            break;
                        case BookingAction.Transfer:
                            total += Convert.ToDouble(change.Parameter);
                            break;
                        case BookingAction.Untransfer:
                            total += Convert.ToDouble(change.Parameter);
                            break;
                        case BookingAction.ChangeTransfer:
                            total += Convert.ToDouble(change.Parameter);
                            break;
                    }
                }
                changes[track.Booking] += total; // Cộng thêm giá trị thay đổi
            }
            #endregion

            int pax = 0;
            int rooms = 0;
            double grandTotal = 0;

            foreach (double value in changes.Values)
            {
                grandTotal += value;
            }

            #region -- Tạo file --

            int index = 0;
            foreach (Booking booking in bookings)
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
                    sheet.Cells[currentRow, 5].Value = booking.Agency.Name; // tên đối tác
                }

                if (booking.IsTransferred)
                {
                    sheet.Cells[currentRow, 6].Value = booking.TransferAdult + booking.TransferChildren + booking.TransferBaby; // Số khách
                    pax += booking.TransferAdult + booking.TransferChildren + booking.TransferBaby; // Số khách
                }
                else
                {
                    sheet.Cells[currentRow, 6].Value = booking.Adult + booking.Child + booking.Baby; // Số khách
                    pax += booking.Adult + booking.Child + booking.Baby; // Số khách
                }

                sheet.Cells[currentRow, 7].Value = booking.StartDate; // Ngày check-in               
                sheet.Cells[currentRow, 12].Value = booking.Total; // Tổng tiền booking
                if (booking.Amended > 0)
                {
                    sheet.Cells[currentRow, 13].Value = "Amend"; // Amend
                }
                else
                {
                    sheet.Cells[currentRow, 13].Value = "New"; // New
                }

                sheet.Cells[currentRow, 15].Value = changes[booking]; // Thay đổi về doanh thu

                //grandTotal += booking.Total;
                rooms += booking.BookingRooms.Count;

                // Dịch vụ sử dụng xuất sau vì có thể tốn thêm dòng
                int rows = 0;
                if (booking.IsCharter)
                {
                    // Charter thì chỉ tốn đúng 1 dòng và 1 dịch vụ duy nhất: charter, đơn giá = tổng giá luôn
                    sheet.Cells[currentRow, 8].Value = 1;
                    sheet.Cells[currentRow, 9].Value = "Charter";
                    sheet.Cells[currentRow, 10].Value = booking.Total;
                    sheet.Cells[currentRow, 11].Value = booking.Total;
                    rows = 1;
                }
                else
                {
                    // Các dịch vụ

                    // Lưu lại giá tổng các dịch vụ để so với giá tổng thực tế
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
                        sheet.Cells[currentRow + rows, 8].Value = roomDic[key];
                        sheet.Cells[currentRow + rows, 9].Value = key.Substring(0, key.IndexOf("|"));
                        if (Page.CustomPriceForRoom)
                        {
                            sheet.Cells[currentRow + rows, 10].Value = valueDic[key] / roomDic[key];
                            sheet.Cells[currentRow + rows, 11].Value = valueDic[key];
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
                        sheet.Cells[currentRow + rows, 8].Value = extraDic[key];
                        sheet.Cells[currentRow + rows, 9].Value = key.Name;
                        if (Page.CustomPriceForRoom)
                        {
                            sheet.Cells[currentRow + rows, 10].Value = extraPrices[key];
                            sheet.Cells[currentRow + rows, 11].Value = extraPrices[key] * extraDic[key];
                            total += extraPrices[key] * extraDic[key];
                        }
                        rows++;
                    }
                    #endregion

                    // Lấy giá tổng thực tế so sánh với tổng dịch vụ
                    if (booking.Total != total)
                    {
                        // Không bằng nhau thì xóa hai cột đơn giá và thành tiền
                        for (int ii = currentRow; ii < currentRow + rows; ii++)
                        {
                            sheet.Cells[ii, 10].Value = string.Empty;
                            sheet.Cells[ii, 11].Value = string.Empty;
                        }
                    }

                    if (rows == 0)
                    {
                        rows = 1;
                    }
                }
                currentRow += rows;
            }

            sheet.Cells["E4"].Value = pax;
            sheet.Cells["E5"].Value = rooms;
            sheet.Cells["J5"].Value = grandTotal;
            #endregion

            #region -- Trả dữ liệu về cho người dùng --

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("content-disposition",
                                  "attachment; filename=" + string.Format("BaoCaoBKNhanDuoc.xls"));

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
