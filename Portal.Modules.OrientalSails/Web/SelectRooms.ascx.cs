using System;
using System.Globalization;
using System.Web.UI.WebControls;
using CMS.Web.UI;
using CMS.Web.Util;
using log4net;
using Portal.Modules.OrientalSails.Domain;

namespace Portal.Modules.OrientalSails.Web
{
    public partial class SelectRooms : BaseModuleControl
    {
        #region -- Private Member --

        private readonly CultureInfo _cultureInfo = new CultureInfo("vi-VN");
        private readonly ILog _logger = LogManager.GetLogger(typeof (SelectRooms));
        private DateTime _startDate;
        private SailsTrip _trip;

        protected new SailsModule Module
        {
            get { return base.Module as SailsModule; }
        }

        #endregion

        #region -- Page Event --

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Lấy start date từ trong session
                _startDate = DateTime.ParseExact(Session["StartDate"].ToString(), "dd/MM/yyyy",
                                                         _cultureInfo.DateTimeFormat);
                _trip = Module.TripGetById(Convert.ToInt32(Session["TripId"]));
                if (!IsPostBack)
                {
                    // Lấy danh sách phòng
                    rptRoomClass.DataSource = Module.RoomClassGetAll();
                    rptRoomClass.DataBind();

                    // Lấy danh sách dịch vụ 
                    rptExtraOption.DataSource = Module.ExtraOptionGetBooking();
                    rptExtraOption.DataBind();
                    LocalizeControls();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error when Page_Load in SelectRooms ", ex);
                throw;
            }
        }

        #endregion

        #region -- Private Method --

        #endregion

        #region -- Control events --

        protected void rptRoomClass_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is RoomClass)
            {
                RoomClass item = e.Item.DataItem as RoomClass;
                if (item != null)
                {
                    Repeater rptRoomType = e.Item.FindControl("rptRoomType") as Repeater;
                    if (rptRoomType != null)
                    {
                        rptRoomType.DataSource = Module.RoomTypexGetAll();
                        rptRoomType.DataBind();
                    }
                }
            }
        }

        protected void rptRoomType_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is RoomTypex)
            {
                RoomTypex rtype = e.Item.DataItem as RoomTypex;
                RoomClass rclass = ((RepeaterItem) ((e.Item.Parent).Parent)).DataItem as RoomClass;
                Label label_RoomClass = e.Item.FindControl("label_RoomClass") as Label;
                Label label_RoomType = e.Item.FindControl("label_RoomType") as Label;
                Label label_Avaliable = e.Item.FindControl("label_Avaliable") as Label;
                DropDownList ddlSelect = e.Item.FindControl("ddlSelect") as DropDownList;
                if (rclass != null && rtype != null && label_Avaliable != null && label_RoomClass != null &&
                    label_RoomType != null && ddlSelect != null)
                {
                    label_RoomClass.Text = rclass.Name;
                    label_RoomType.Text = rtype.Name;
                    int roomCount = Module.RoomCount(rclass, rtype,null, _startDate, _trip.NumberOfDay);
                    if (roomCount < 0)
                    {
                        e.Item.Visible = false;
                        return;
                    }
                    if (roomCount == 0)
                    {
                        ddlSelect.Visible = false;
                    }

                    if (rtype.Id != SailsModule.TWIN)
                    {
                        label_Avaliable.Text = string.Format("{0} room(s)", roomCount);
                        for (int i = 0; i <= roomCount; i++)
                        {
                            ddlSelect.Items.Add(new ListItem(string.Format("{0} room", i),i.ToString()));
                        }
                    }
                    else
                    {
                        label_Avaliable.Text = string.Format("{0} room(s) ({1} person)", roomCount /2 , roomCount);
                        for (int i = 0; i <= roomCount; i++)
                        {
                            ddlSelect.Items.Add(new ListItem(string.Format("{0} person", i), i.ToString()));
                        }
                    }
                }
            }
        }

        protected void buttonSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (PageEngine.IsValid)
                {
                    if (!SaveData())
                    {
                        return;
                    }

                    // Chuyển sang trang nhập thông tin khách hàng
                    PageEngine.PageRedirect(string.Format("{0}/{1}{2}", UrlHelper.GetUrlFromSection(Module.Section),
                                                          SailsModule.ACTION_CUSTOMER_INFO_PARAM,
                                                          UrlHelper.EXTENSION));
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error when buttonSubmit_Click in SelectRoom", ex);
                throw;
            }
        }

        private bool SaveData()
        {
            //Luu vao session theo dang roomclass - roomtype - so luong phong
            #region -- Lưu thông tin phòng đã chọn --
            const string confStr = "{0},{1},{2}";
            int confInt = 0;
            int totalRooms = 0;
            foreach (RepeaterItem classItem in rptRoomClass.Items)
            {
                Label labelRoomClassId = (Label) classItem.FindControl("labelRoomClassId");
                RoomClass roomClass = Module.RoomClassGetById(Convert.ToInt32(labelRoomClassId.Text));
                Repeater rptRoomType = (Repeater) classItem.FindControl("rptRoomType");

                foreach (RepeaterItem typeItem in rptRoomType.Items)
                {
                    Label labelRoomTypeId = (Label) typeItem.FindControl("labelRoomTypeId");
                    DropDownList ddlSelect = (DropDownList) typeItem.FindControl("ddlSelect");

                    RoomTypex roomTypex = Module.RoomTypexGetById(Convert.ToInt32(labelRoomTypeId.Text));
                    if (ddlSelect.SelectedIndex > 0)
                    {
                        confInt++;
                        Session.Add("Config" + confInt,
                                    string.Format(confStr, roomClass.Id, roomTypex.Id, ddlSelect.SelectedValue));
                        totalRooms += Convert.ToInt32(ddlSelect.SelectedValue);
                    }
                }
            }
            if (confInt == 0)
            {
                return false;
            }
            Session.Add("ConfigCount", confInt);
            Session.Add("RoomCount", totalRooms);
            #endregion

            #region -- Lưu thông tin extra service --
            foreach (RepeaterItem service in rptExtraOption.Items)
            {
                string services = string.Empty;
                CheckBox checkBoxExtra = (CheckBox) service.FindControl("checkBoxExtra");
                HiddenField hiddenId = (HiddenField) service.FindControl("hiddenId");
                if (checkBoxExtra.Checked)
                {
                    if (string.IsNullOrEmpty(services))
                    {
                        services = hiddenId.Value;
                    }
                    else
                    {
                        services += "," + hiddenId.Value;
                    }
                }

                if (!string.IsNullOrEmpty(services))
                {
                    Session.Add("ExtraService", services);
                }
            }
            #endregion
            return true;
        }

        #endregion

        protected void buttonSelectRoom_Click(object sender, EventArgs e)
        {
            if (PageEngine.IsValid)
            {
                if (!SaveData())
                {
                    return;
                }

                // Chuyển sang trang nhập thông tin khách hàng
                PageEngine.PageRedirect(string.Format("{0}/{1}{2}", UrlHelper.GetUrlFromSection(Module.Section),
                                                      SailsModule.ACTION_PREFERED_ROOM_PARAM,
                                                      UrlHelper.EXTENSION));
            }
        }

        protected void rptExtraOption_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ExtraOption item = e.Item.DataItem as ExtraOption;
            if (item != null)
            {
                CheckBox checkBoxExtra = e.Item.FindControl("checkBoxExtra") as CheckBox;
                if (checkBoxExtra != null)
                {
                    checkBoxExtra.Text = string.Format("{0}", item.Name, item.Price);
                }
            }
        }
    }
}