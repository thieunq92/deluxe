using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NHibernate.Criterion;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.Admin;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails
{
    public partial class CommissionEdit : SailsAdminBasePage
    {
        protected IList _agencies;
        private Booking _booking;
        private int _mode;
        private double left;
        private double paid;
        private double paidBase;
        private double total;

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
                #region -- một booking --

                if (_mode == 1)
                {
                    btnConfirm.Visible = false;
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

                    if (_booking.IsCommissionVND)
                        litTotal.Text = _booking.CommissionVND.ToString("#,0.#") + " VND";
                    else
                        litTotal.Text = _booking.Commission.ToString("#,0.#")+" USD";
                    
                    if (_booking.CurrencyRate > 0)
                    {
                        txtAppliedRate.Text = _booking.CurrencyRate.ToString("#,0.#");
                    }
                    else
                    {
                        txtAppliedRate.Text = Module.ExchangeGetByDate(DateTime.Now).Rate.ToString("#,0.#");
                    }

                    txtPaid.Text = _booking.Paid.ToString("#,0.#");
                    txtPaidBase.Text = _booking.PaidBase.ToString("#,0.#");

                    total = _booking.Total;
                    paid = _booking.Paid;
                    paidBase = _booking.PaidBase;
                    left = _booking.TotalReceivable;

                    chkPaid.Checked = _booking.ComPaid;
                }

                #endregion

                #region -- nhiều booking --

                if (_mode == 2)
                {
                    plhOneBooking.Visible = false;
                    plhMultiAgency.Visible = true;

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

                    double rate = Module.ExchangeGetByDate(DateTime.Now).Rate;
                    double backrate = 0;
                    bool rateChanged = false;
                    IList bookings = GetData();
                    foreach (Booking booking in bookings)
                    {
                        if (booking.Commission <= 0) continue;
                        paid += booking.ComUSDpayed;
                        paidBase += booking.ComVNDpayed;
                        if (booking.ComRate <= 0)
                        {
                            booking.ComRate = rate;
                        }

                        if (backrate == 0)
                        {
                            backrate = booking.ComRate;
                        }

                        if (backrate != booking.ComRate)
                        {
                            rateChanged = true;
                        }

                        left += booking.CommissionLeft;
                        total += booking.CommissionLeft / backrate;
                    }

                    litTotal.Text = total.ToString("#,0.#");
                    txtPaid.Text = "0";
                    txtPaidBase.Text = left.ToString("#,0.#");

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

            // Valid tổng tiền đã trả

            //Số tiền dư tính bằng tiền tệ gốc
            if (_mode == 1)
            {
                left = (_booking.Total - Convert.ToDouble(txtPaid.Text)) * appliedRate - Convert.ToDouble(txtPaidBase.Text);
            }
            if (_mode == 2)
            {
                IList bookings = GetData();
                foreach (Booking booking in bookings)
                {
                    if (appliedRate == 0)
                    {
                        if (booking.ComRate <= 0)
                        {
                            booking.ComRate = currentRate;
                        }
                    }
                    else
                    {
                        booking.ComRate = appliedRate;
                    }

                    left += booking.CommissionLeft;
                }
                left = left - Convert.ToDouble(txtPaid.Text) * appliedRate - Convert.ToDouble(txtPaidBase.Text);
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

            if (left < 5000 && !chkPaid.Checked)
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
                _booking.ComUSDpayed = Convert.ToDouble(txtPaid.Text);
                _booking.ComVNDpayed = Convert.ToDouble(txtPaidBase.Text);
                _booking.ComPaid = chkPaid.Checked;

                if (!_booking.ComPaidDate.HasValue)
                    _booking.ComPaidDate = DateTime.Today;

                Module.SaveOrUpdate(_booking);
            }

            if (_mode == 2)
            {
                double rate = Convert.ToDouble(txtAppliedRate.Text);
                paid = Convert.ToDouble(txtPaid.Text);
                paidBase = Convert.ToDouble(txtPaidBase.Text);
                IList list = GetData();
                for (int ii = 0; ii < list.Count; ii++)
                {
                    Booking booking = (Booking)list[ii];
                    booking.ComRate = rate;
                    booking.ComPaid = false;
                    // Nếu còn dư USD thì paid all bằng USD
                    if (booking.CommissionLeft / booking.ComRate < paid)
                    {
                        booking.ComUSDpayed = booking.Commission;
                        booking.ComVNDpayed = 0;
                        paid -= booking.CommissionLeft;
                    }
                    else
                    {
                        // Nếu không thì trả hết USD còn lại, thêm một ít VNĐ
                        // Đúng luôn cả với trường hợp USD = 0
                        booking.ComUSDpayed = paid;
                        booking.ComVNDpayed = 0;
                        paid = 0;

                        if (paidBase > booking.CommissionLeft)
                        {
                            booking.PaidBase = booking.CommissionLeft;
                            paidBase -= booking.CommissionLeft;
                        }
                        else
                        {
                            booking.PaidBase = paidBase;
                            paidBase = 0;
                        }
                    }
                    booking.ComPaid = true;
                    if (!booking.ComPaidDate.HasValue)
                        booking.ComPaidDate = DateTime.Today;

                    Module.SaveOrUpdate(booking);
                }
            }
            Page.ClientScript.RegisterClientScriptBlock(typeof(CommissionEdit), "close",
                                                        "window.opener.location = window.opener.location;window.close();",
                                                        true);
        }

        protected IList GetData()
        {
            int count;
            ICriterion criterion = Expression.Eq(Booking.DELETED, false);
            criterion = Expression.And(criterion, Expression.Eq(Booking.STATUS, StatusType.Approved));
            //TODO: uncomment
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
