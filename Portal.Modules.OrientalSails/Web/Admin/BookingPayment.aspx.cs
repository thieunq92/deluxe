using System;
using System.Collections;
using System.Web.UI.WebControls;
using CMS.Web.Util;
using NHibernate.Criterion;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class BookingPayment : SailsAdminBase
    {

        #region -- PRIVATE MEMBERS --
        protected IList _agencies;
        private Booking _booking;
        private int _mode;
        private double left;
        private double paid;
        private double paidBase;
        private double total;

        /// <summary>
        /// Biến toàn cục lưu tổng số tiền thanh toán bằng USD của lịch sử giao dịch
        /// </summary>
        private double _totalUSD;

        /// <summary>
        /// Biến toàn cục lưu tổng số tiền thanh toán bằng VND của lịch sử giao dịch
        /// </summary>
        private double _totalVND;
        #endregion

        #region -- PAGE EVENTS --
        protected void Page_Load(object sender, EventArgs e)
        {
            HideError();
            if (Request.QueryString["bookingid"] != null)
            {
                _booking = Module.BookingGetById(Convert.ToInt32(Request.QueryString["bookingid"]));
                _mode = 1;
            }
            else
            {
                _mode = 2;
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["mode"] == "guide")
                {
                    tabContainer.ActiveTab = tabGuide;
                }
                if (Request.QueryString["mode"] == "driver")
                {
                    tabContainer.ActiveTab = tabDriver;
                }

                #region -- một booking --

                if (_mode == 1)
                {
                    btnConfirm.Visible = false;
                    btnConfirmGuide.Visible = false;
                    litBookingCode.Text = string.Format("ATM{0:00000}", _booking.Id);
                    if (_booking.Agency != null)
                    {
                        litAgency.Text = _booking.Agency.Name;
                    }
                    litStartDate.Text = _booking.StartDate.ToString("dd/MM/yyyy");
                    litService.Text = _booking.Trip.Name;
                    if (_booking.Trip.NumberOfOptions > 1)
                    {
                        litService.Text += _booking.TripOption.ToString();
                    }

                    litGuideCollect.Text = _booking.GuideCollect.ToString("#,0.#");
                    litTotal.Text = _booking.Total.ToString("#,0.#");

                    #region -- Partner receivable --
                    // Truyền vào các tham số liên quan đến khoản phải thu của booking cho partner
                    //litAgencyReceivable.Text = _booking.AgencyReceivable.ToString("#,0.#");                    

                    if (_booking.CurrencyRate > 0) // Tính rate áp dụng trước khi quy đổi tiền ở bước dưới
                    {
                        txtAppliedRate.Text = _booking.CurrencyRate.ToString("#,0.#");
                    }
                    else
                    {
                        _booking.CurrencyRate = Module.ExchangeGetByDate(DateTime.Now).Rate;
                        txtAppliedRate.Text = _booking.CurrencyRate.ToString("#,0.#");
                    }

                    litRemainVND.Text = _booking.AgencyReceivable.ToString("#,0.#");
                    litRemainUSD.Text = (_booking.AgencyReceivable / _booking.CurrencyRate).ToString("#,0.##");
                    #endregion

                    #region -- Guide collect --
                    // Chỉ hiển thị tab guide collect nếu có guide collect
                    if (_booking.GuideCollect > 0)
                    {
                        //litGuideReceivable.Text = _booking.GuideCollectReceivable.ToString("#,0.#");
                        litRate.Text = _booking.CurrencyRate.ToString("#,0.#"); // Luôn lấy rate theo booking, đã set ở bước partner

                        litGuideVND.Text = _booking.GuideCollectReceivable.ToString("#,0.#");
                        litGuideUSD.Text = (_booking.GuideCollectReceivable / _booking.CurrencyRate).ToString("#,0.##");
                    }

                    #endregion

                    #region -- Driver collect --
                    // Chỉ hiển thị tab guide collect nếu có guide collect
                    if (_booking.DriverCollect > 0)
                    {
                        //litGuideReceivable.Text = _booking.GuideCollectReceivable.ToString("#,0.#");
                        litDriverRate.Text = _booking.CurrencyRate.ToString("#,0.#"); // Luôn lấy rate theo booking, đã set ở bước partner

                        litGuideVND.Text = _booking.DriverCollectReceivable.ToString("#,0.#");
                        litGuideUSD.Text = (_booking.DriverCollectReceivable / _booking.CurrencyRate).ToString("#,0.##");
                    }

                    #endregion

                    total = _booking.Total;
                    paid = _booking.Paid;
                    paidBase = _booking.PaidBase;
                    left = _booking.TotalReceivable;

                    chkPaid.Checked = _booking.IsPaid;
                    chkGuidePaid.Checked = _booking.GuideCollected;

                    _totalUSD = 0;
                    _totalVND = 0;
                    rptPaymentHistory.DataSource = _booking.Transactions;
                    rptPaymentHistory.DataBind();
                }

                #endregion

                #region -- nhiều booking --

                if (_mode == 2)
                {
                    trNote.Visible = false;
                    plhHistory.Visible = false; // nhiều booking thì ko lấy lịch sử
                    plhOneBooking.Visible = false;
                    plhMultiAgency.Visible = true;

                    tabGuide.Visible = false;

                    #region -- Danh sách booking --
                    // Thông tin booking
                    DateTime from = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["from"]));
                    DateTime to = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["to"]));
                    if (GetAgencies().Count == 0)
                    {
                        litBookings.Text = string.Format("All bookings from {0:dd/MM/yyyy} to {1:dd/MM/yyyy}", from, to);
                    }
                    else
                    {
                        string agencies = string.Empty;
                        foreach (Agency agency in GetAgencies())
                        {
                            agencies += agency.Name + ", ";
                        }
                        if (agencies.Length > 2)
                        {
                            agencies = agencies.Remove(agencies.Length - 2);
                        }
                        litBookings.Text = string.Format("All bookings from {0:dd/MM/yyyy} to {1:dd/MM/yyyy} of {2}",
                                                         from, to, agencies);
                    }

                    #endregion

                    #region -- Tính toán --
                    double rate = Module.ExchangeGetByDate(DateTime.Now).Rate;
                    double backrate = 0;
                    bool rateChanged = false;
                    IList bookings = GetData();

                    double remainedUSD = 0;
                    double remainedVND = 0;

                    foreach (Booking booking in bookings)
                    {
                        total += booking.Total;
                        paid += booking.Paid;
                        paidBase += booking.PaidBase;
                        if (booking.CurrencyRate <= 0)
                        {
                            booking.CurrencyRate = rate;
                        }

                        if (backrate == 0)
                        {
                            backrate = booking.CurrencyRate;
                        }

                        if (backrate != booking.CurrencyRate)
                        {
                            rateChanged = true;
                        }

                        remainedVND += booking.AgencyReceivable;
                        left += booking.TotalReceivable;
                    }
                    #endregion

                    #region -- Partner receivable --
                    // Truyền vào các tham số liên quan đến khoản phải thu của booking cho partner
                    //litAgencyReceivable.Text = _booking.AgencyReceivable.ToString("#,0.#");                    

                    litRemainVND.Text = remainedVND.ToString("#,0.##");
                    litRemainUSD.Text = (remainedVND / rate).ToString("#,0.##");

                    #endregion

                    litTotal.Text = total.ToString("#,0.#");
                    txtPaid.Text = paid.ToString("#,0.#");
                    txtPaidBase.Text = paidBase.ToString("#,0.#");

                    if (rateChanged)
                    {
                        txtAppliedRate.Text = "";
                    }
                    else
                    {
                        txtAppliedRate.Text = backrate.ToString("#,0.#");
                    }
                    chkPaid.Checked = true;
                    chkPaid.Enabled = false;
                }

                #endregion
            }
        }

        #endregion

        #region -- CONTROL EVENTS --
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Valid tỷ giá
            double appliedRate;
            double currentRate = Module.ExchangeGetByDate(DateTime.Now).Rate;
            if (_mode != 2 || txtAppliedRate.Text != string.Empty)
            {
                appliedRate = Convert.ToDouble(txtAppliedRate.Text);
                if (appliedRate / currentRate > 1.1f || appliedRate / currentRate < 0.9f)
                {
                    ShowError(
                        string.Format(
                            "Warning: applied rate and current rate is quite diffirent ({0} with {1}), please confirm",
                            currentRate, appliedRate));
                    btnConfirm.Visible = true;
                    return;
                }
            }
            else
            {
                appliedRate = 0;
            }

            //Số tiền dư tính bằng tiền tệ gốc
            if (_mode == 1) // Tình huống pay cho 1 booking
            {
                // Tổng tiền đã thanh toán = khoản đã thanh toán + khoản
                double usdPaid = Convert.ToDouble(txtPaid.Text);
                double vndPaid = Convert.ToDouble(txtPaidBase.Text);

                usdPaid += _booking.Paid;
                vndPaid += _booking.PaidBase;
                left = (_booking.Total - usdPaid) * appliedRate - vndPaid;
            }
            if (_mode == 2) // Tình huống pay cho nhiều booking
            {
                IList bookings = GetData();
                foreach (Booking booking in bookings)
                {
                    if (appliedRate == 0)
                    {
                        if (booking.CurrencyRate <= 0)
                        {
                            booking.CurrencyRate = currentRate;
                        }
                    }
                    else
                    {
                        booking.CurrencyRate = appliedRate;
                    }

                    left += booking.Total;
                }
                left = (left - Convert.ToDouble(txtPaid.Text)) * appliedRate - Convert.ToDouble(txtPaidBase.Text);
            }
            left = Math.Abs(left);

            if (left > 5000 && chkPaid.Checked)
            {
                ShowError(
                    string.Format(
                        "Warning: The diffirent with paid money and total value is high ({0} VND), please confirm that this booking is paid",
                        left));
                btnConfirm.Visible = true;
                return;
            }

            if (left != 0 && left < 5000 && !chkPaid.Checked) // Chỉ cảnh báo khi gần = 0 chứ không cảnh báo khi đúng = 0
            {
                ShowError(
                    string.Format(
                        "Warning: The diffirent with paid money and total value is near zero ({0} VND), please confirm that this booking is not paid",
                        left));
                btnConfirm.Visible = true;
                return;
            }

            btnConfirm_Click(sender, e);
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (_mode == 1)
            {
                _booking.CurrencyRate = Convert.ToDouble(txtAppliedRate.Text);
                double paidUSD = Convert.ToDouble(txtPaid.Text);
                double paidVND = Convert.ToDouble(txtPaidBase.Text);

                double totalPaidUSD = 0;
                double totalPaidVND = 0;
                // Tính toán lại giữa lịch sử giao dịch và con số lưu
                foreach (var t in _booking.Transactions)
                {
                    if (t.TransactionType == Transaction.BOOKING)
                    {
                        totalPaidUSD += t.USDAmount;
                        totalPaidVND += t.VNDAmount;
                    }
                }

                // Lưu giao dịch
                var transaction = new Transaction();
                transaction.Agency = _booking.Agency;
                transaction.Booking = _booking;
                transaction.CreatedBy = UserIdentity;
                transaction.CreatedDate = DateTime.Now;
                transaction.IsExpense = false;
                transaction.TransactionType = Transaction.BOOKING;
                transaction.USDAmount = paidUSD;
                transaction.VNDAmount = paidVND;
                transaction.Note = txtNote.Text;
                Module.SaveOrUpdate(transaction);

                // Sau đó tính toán lại booking (thêm vào transaction mới)
                totalPaidUSD += transaction.USDAmount;
                totalPaidVND += transaction.VNDAmount;

                _booking.Paid = totalPaidUSD;
                _booking.PaidBase = totalPaidVND;

                // Nếu money left đúng = 0 thì không có lý do để not paid
                bool temp;
                temp = _booking.IsPaid;
                _booking.IsPaid = false;
                if (_booking.AgencyReceivable == 0)
                {
                    chkPaid.Checked = true;
                }
                _booking.IsPaid = temp;

                if (_booking.IsPaid != chkPaid.Checked) // Nếu có sự thay đổi trạng thái
                {
                    _booking.IsPaid = chkPaid.Checked;
                    if (_booking.IsPaid) // và là thay đổi thành đã paid, thì ghi nhận ngày hiện tại là ngày paid
                    {
                        _booking.PaidDate = DateTime.Now;
                    }
                }
                _booking.AgencyConfirmed = false;
                Module.SaveOrUpdate(_booking);
            }

            if (_mode == 2)
            {
                double rate = Convert.ToDouble(txtAppliedRate.Text); // tỉ giá áp dụng
                paid = Convert.ToDouble(txtPaid.Text); // số tiền bằng USD
                paidBase = Convert.ToDouble(txtPaidBase.Text); // số tiền bằng VND
                IList list = GetData();
                for (int ii = 0; ii < list.Count; ii++)
                {
                    Booking booking = (Booking)list[ii];
                    booking.CurrencyRate = rate;
                    booking.IsPaid = false;

                    var transaction = new Transaction();
                    transaction.Agency = booking.Agency;
                    transaction.Booking = booking;
                    transaction.CreatedBy = UserIdentity;
                    transaction.CreatedDate = DateTime.Now;
                    transaction.IsExpense = false;
                    transaction.TransactionType = Transaction.BOOKING;

                    // Nếu còn dư USD thì paid all bằng USD
                    if (booking.Total < paid)
                    {
                        booking.Paid = booking.Total;
                        booking.PaidBase = 0;
                        paid -= booking.Paid;

                        transaction.USDAmount = booking.Total;
                    }
                    else
                    {
                        // Nếu không thì trả hết USD còn lại, thêm một ít VNĐ
                        // Đúng luôn cả với trường hợp USD = 0
                        booking.Paid = paid;
                        transaction.USDAmount = paid;
                        booking.PaidBase = 0;
                        paid = 0;

                        if (paidBase > booking.TotalReceivable)
                        {
                            booking.PaidBase = booking.TotalReceivable;
                            transaction.VNDAmount = booking.PaidBase;
                            paidBase -= booking.PaidBase;
                        }
                        else
                        {
                            transaction.VNDAmount = paidBase;
                            booking.PaidBase = paidBase;
                            paidBase = 0;
                        }
                    }

                    if (!booking.IsPaid) // Nếu có sự thay đổi trạng thái thì ghi nhận ngày paid là ngày hiện tại
                    {
                        booking.IsPaid = true;
                        booking.PaidDate = DateTime.Now;
                    }

                    booking.AgencyConfirmed = false;
                    Module.SaveOrUpdate(transaction);
                    Module.SaveOrUpdate(booking);
                }
            }

            if (_mode == 1 && !_booking.IsPaid) // trong tình huống thanh toán 1 booking và chưa thanh toán xong
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(BookingPayment), "close", "window.location = window.location;",
                                                        true);
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(BookingPayment), "close",
                                                        "window.opener.location = window.opener.location;window.close();",
                                                        true);
            }
        }

        protected void rptPaymentHistory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is Transaction)
            {
                var transaction = (Transaction)e.Item.DataItem;
                ValueBinder.BindLiteral(e.Item, "litTime", transaction.CreatedDate.ToString("dd/MM/yyyy HH:mm"));
                ValueBinder.BindLiteral(e.Item, "litPayBy", transaction.AgencyName);
                ValueBinder.BindLiteral(e.Item, "litAmountUSD", transaction.USDAmount.ToString("#,0.#"));
                ValueBinder.BindLiteral(e.Item, "litAmountVND", transaction.VNDAmount.ToString("#,0.#"));
                ValueBinder.BindLiteral(e.Item, "litCreatedBy", transaction.CreatedBy.UserName);
                ValueBinder.BindLiteral(e.Item, "litNote", transaction.Note);

                _totalUSD += transaction.USDAmount;
                _totalVND += transaction.VNDAmount;
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                ValueBinder.BindLiteral(e.Item, "litTotalUSD", _totalUSD.ToString("#,0.#"));
                ValueBinder.BindLiteral(e.Item, "litTotalVND", _totalVND.ToString("#,0.#"));
            }
        }

        #region -- Guide tab --
        protected void btnSaveGuide_Click(object sender, EventArgs e)
        {
            // Valid tỷ giá
            double appliedRate;
            double currentRate = Module.ExchangeGetByDate(DateTime.Now).Rate;
            if (_mode != 2 && _booking.CurrencyRate > 0) // chế độ một booking
            {
                appliedRate = _booking.CurrencyRate;
            }
            else
            {
                appliedRate = currentRate;
            }

            // Valid tổng tiền đã trả

            // Tổng tiền đã thanh toán = khoản đã thanh toán + khoản
            double usdPaid = Convert.ToDouble(txtGuideUSDPaid.Text);
            double vndPaid = Convert.ToDouble(txtGuideVNDPaid.Text);

            usdPaid += _booking.GuideCollectedUSD;
            vndPaid += _booking.GuideCollectedVND;

            //Số tiền dư tính bằng tiền tệ gốc
            if (_mode == 1) // Tình huống pay cho 1 booking
            {
                if (_booking.IsGuideCollectVND)
                    left = _booking.GuideCollectVND - usdPaid * appliedRate - vndPaid;
                else
                    left = (_booking.GuideCollect - usdPaid) * appliedRate - vndPaid;
            }
            //if (_mode == 2) // Tình huống pay cho nhiều booking
            //{
            //    IList bookings = GetData();
            //    foreach (Booking booking in bookings)
            //    {
            //        if (appliedRate == 0)
            //        {
            //            if (booking.CurrencyRate <= 0)
            //            {
            //                booking.CurrencyRate = currentRate;
            //            }
            //        }
            //        else
            //        {
            //            booking.CurrencyRate = appliedRate;
            //        }

            //        left += booking.Total;
            //    }
            //    left = (left - Convert.ToDouble(txtPaid.Text)) * appliedRate - Convert.ToDouble(txtPaidBase.Text);
            //}
            left = Math.Abs(left);

            if (left > 5000 && chkGuidePaid.Checked)
            {
                ShowError(
                    string.Format(
                        "Warning: The diffirent with paid money and total value is high ({0} VND), please confirm that this booking is paid",
                        left));
                btnConfirmGuide.Visible = true;
                return;
            }

            if (left != 0 && left < 5000 && !chkGuidePaid.Checked) // Chỉ cảnh báo khi gần = 0 chứ không cảnh báo khi đúng = 0
            {
                ShowError(
                    string.Format(
                        "Warning: The diffirent with paid money and total value is near zero ({0} VND), please confirm that this booking is not paid",
                        left));
                btnConfirmGuide.Visible = true;
                return;
            }

            btnConfirmGuide_Click(sender, e);
        }
        protected void btnSaveDriver_Click(object sender, EventArgs e)
        {
            // Valid tỷ giá
            double appliedRate;
            double currentRate = Module.ExchangeGetByDate(DateTime.Now).Rate;
            if (_mode != 2 && _booking.CurrencyRate > 0) // chế độ một booking
            {
                appliedRate = _booking.CurrencyRate;
            }
            else
            {
                appliedRate = currentRate;
            }

            // Valid tổng tiền đã trả

            // Tổng tiền đã thanh toán = khoản đã thanh toán + khoản
            double usdPaid = Convert.ToDouble(txtDriverUSDPaid.Text);
            double vndPaid = Convert.ToDouble(txtDriverVNDPaid.Text);

            usdPaid += _booking.DriverCollectedUSD;
            vndPaid += _booking.DriverCollectedVND;

            //Số tiền dư tính bằng tiền tệ gốc
            if (_mode == 1) // Tình huống pay cho 1 booking
            {
                if (_booking.IsDriverCollectVND)
                    left = _booking.DriverCollectVND - usdPaid * appliedRate - vndPaid;
                else
                    left = (_booking.DriverCollect - usdPaid) * appliedRate - vndPaid;
            }
            //if (_mode == 2) // Tình huống pay cho nhiều booking
            //{
            //    IList bookings = GetData();
            //    foreach (Booking booking in bookings)
            //    {
            //        if (appliedRate == 0)
            //        {
            //            if (booking.CurrencyRate <= 0)
            //            {
            //                booking.CurrencyRate = currentRate;
            //            }
            //        }
            //        else
            //        {
            //            booking.CurrencyRate = appliedRate;
            //        }

            //        left += booking.Total;
            //    }
            //    left = (left - Convert.ToDouble(txtPaid.Text)) * appliedRate - Convert.ToDouble(txtPaidBase.Text);
            //}
            left = Math.Abs(left);

            if (left > 5000 && chkDriverPaid.Checked)
            {
                ShowError(
                    string.Format(
                        "Warning: The diffirent with paid money and total value is high ({0} VND), please confirm that this booking is paid",
                        left));
                btnConfirm.Visible = true;
                return;
            }

            if (left != 0 && left < 5000 && !chkDriverPaid.Checked) // Chỉ cảnh báo khi gần = 0 chứ không cảnh báo khi đúng = 0
            {
                ShowError(
                    string.Format(
                        "Warning: The diffirent with paid money and total value is near zero ({0} VND), please confirm that this booking is not paid",
                        left));
                btnConfirm.Visible = true;
                return;
            }

            btnConfirmDriver_Click(sender, e);
        }

        protected void btnConfirmGuide_Click(object sender, EventArgs e)
        {
            if (_mode == 1)
            {
                double paidUSD = Convert.ToDouble(txtGuideUSDPaid.Text);
                double paidVND = Convert.ToDouble(txtGuideVNDPaid.Text);

                double totalPaidUSD = 0;
                double totalPaidVND = 0;
                // Tính toán lại giữa lịch sử giao dịch và con số lưu
                foreach (var t in _booking.Transactions)
                {
                    if (t.TransactionType == Transaction.GUIDECOLLECT)
                    {
                        totalPaidUSD += t.USDAmount;
                        totalPaidVND += t.VNDAmount;
                    }
                }

                // Lưu giao dịch
                var transaction = new Transaction();

                // Tìm guide của booking này
                var sailDate = Module.ExpenseGetByDate(_booking.Trip, _booking.StartDate);
                ExpenseService guide = null;
                foreach (ExpenseService service in sailDate.Services)
                {
                    if (service.Type.Name.ToUpper() == "GUIDE" && service.Group == _booking.Group)
                    {
                        guide = service;
                    }
                }

                if (guide == null)
                {
                    ShowError("Could not determine guide for this booking, please recheck booking by date");
                    return;
                }


                transaction.Agency = guide.Supplier;
                transaction.Booking = _booking;
                transaction.CreatedBy = UserIdentity;
                transaction.CreatedDate = DateTime.Now;
                transaction.IsExpense = false;
                transaction.TransactionType = Transaction.GUIDECOLLECT;
                transaction.USDAmount = paidUSD;
                transaction.VNDAmount = paidVND;
                Module.SaveOrUpdate(transaction);

                // Sau đó tính toán lại booking (thêm vào transaction mới)
                totalPaidUSD += transaction.USDAmount;
                totalPaidVND += transaction.VNDAmount;

                // Nếu đúng = 0 thì paid

                _booking.GuideCollectedUSD = totalPaidUSD;
                _booking.GuideCollectedVND = totalPaidVND;
                _booking.GuideCollected = chkGuidePaid.Checked || _booking.GuideCollectReceivable == 0;
                // để collected = true nếu thu đúng bằng số lượng guide collect
                _booking.GuideConfirmed = false;
                _booking.PaidDate = DateTime.Now;
                Module.SaveOrUpdate(_booking);
            }

            if (_mode == 1 && !_booking.GuideCollected) // trong tình huống thanh toán 1 booking và chưa thanh toán xong
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(BookingPayment), "close", "window.location = window.location;",
                                                        true);
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(BookingPayment), "close",
                                                        "window.opener.location = window.opener.location;window.close();",
                                                        true);
            }
        }

        protected void btnConfirmDriver_Click(object sender, EventArgs e)
        {
            if (_mode == 1)
            {
                double paidUSD = Convert.ToDouble(txtDriverUSDPaid.Text);
                double paidVND = Convert.ToDouble(txtDriverVNDPaid.Text);

                double totalPaidUSD = 0;
                double totalPaidVND = 0;
                // Tính toán lại giữa lịch sử giao dịch và con số lưu
                foreach (var t in _booking.Transactions)
                {
                    if (t.TransactionType == Transaction.DRIVERCOLLECT)
                    {
                        totalPaidUSD += t.USDAmount;
                        totalPaidVND += t.VNDAmount;
                    }
                }

                // Lưu giao dịch
                var transaction = new Transaction();

                // Tìm guide của booking này
                var sailDate = Module.ExpenseGetByDate(_booking.Trip, _booking.StartDate);
                ExpenseService driver = null;
                foreach (ExpenseService service in sailDate.Services)
                {
                    if (service.Type.Name.ToUpper() == "TRANSPORT" && service.Group == _booking.Group)
                    {
                        driver = service;
                    }
                }

                if (driver == null)
                {
                    ShowError("Could not determine driver for this booking, please recheck booking by date");
                    return;
                }


                transaction.Agency = driver.Supplier;
                transaction.Booking = _booking;
                transaction.CreatedBy = UserIdentity;
                transaction.CreatedDate = DateTime.Now;
                transaction.IsExpense = false;
                transaction.TransactionType = Transaction.DRIVERCOLLECT;
                transaction.USDAmount = paidUSD;
                transaction.VNDAmount = paidVND;
                Module.SaveOrUpdate(transaction);

                // Sau đó tính toán lại booking (thêm vào transaction mới)
                totalPaidUSD += transaction.USDAmount;
                totalPaidVND += transaction.VNDAmount;

                // Nếu đúng = 0 thì paid

                _booking.DriverCollectedUSD = totalPaidUSD;
                _booking.DriverCollectedVND = totalPaidVND;
                _booking.DriverCollected = chkDriverPaid.Checked || _booking.DriverCollectReceivable == 0;
                // để collected = true nếu thu đúng bằng số lượng guide collect
                _booking.PaidDate = DateTime.Now;
                Module.SaveOrUpdate(_booking);
            }

            if (_mode == 1 && !_booking.DriverCollected) // trong tình huống thanh toán 1 booking và chưa thanh toán xong
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(BookingPayment), "close", "window.location = window.location;",
                                                        true);
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(BookingPayment), "close",
                                                        "window.opener.location = window.opener.location;window.close();",
                                                        true);
            }
        }
        #endregion

        #endregion
        protected IList GetData()
        {
            int count;
            ICriterion criterion = Expression.And(Expression.Eq(Booking.DELETED, false), Expression.Eq("IsPaid", false));
            criterion = Expression.And(criterion, Expression.Eq(Booking.STATUS, StatusType.Approved));
            DateTime from = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["from"]));
            DateTime to = DateTime.FromOADate(Convert.ToDouble(Request.QueryString["to"]));
            criterion = Expression.And(criterion,
                                       Expression.And(Expression.Ge(Booking.STARTDATE, from),
                                                      Expression.Le(Booking.STARTDATE, to)));

            ICriterion agencyCrit = PaymentReport.SetCriterion(GetAgencies());
            if (agencyCrit != null)
            {
                criterion = Expression.And(criterion, agencyCrit);
            }
            return Module.BookingGetByCriterion(criterion, Order.Asc(Booking.STARTDATE), out count, 0, 0, false, UserIdentity);
        }

        public IList GetAgencies()
        {
            if (_agencies == null)
            {
                if (Request.QueryString["agencyid"] != null)
                {
                    _agencies = new ArrayList();
                    int agencyid = Convert.ToInt32(Request.QueryString["agencyid"]);
                    if (agencyid > 0)
                    {
                        _agencies.Add(Module.AgencyGetById(agencyid));
                    }
                    else
                    {
                        _agencies.Add(new Agency());
                    }
                    return _agencies;
                }

                if (!string.IsNullOrEmpty(Request.QueryString["agencyname"]))
                {
                    _agencies = Module.AgencyGetByName(Request.QueryString["agencyname"]);
                }
                else
                {
                    _agencies = new ArrayList();
                }
            }
            return _agencies;
        }
    }
}