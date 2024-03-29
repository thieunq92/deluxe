using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using CMS.Core.Domain;
using CMS.Modules.Gallery.Domain;
using CMS.Modules.Gallery.Web;
using CMS.Modules.TourManagement.Domain;
using CMS.Modules.TourManagement.Web.UI;
using CMS.Web.Util;

namespace CMS.Modules.TourManagement.Web.Admin
{
    public partial class PositionEdit : TourAdminBasePage
    {
        #region -- PRIVATE METHODS --
        private Location _location;
        protected AlbumSelectorControl _albumSelector;
        #endregion

        #region -- PAGE EVENTS --
        protected void Page_Load(object sender, EventArgs e)
        {
            _albumSelector = (AlbumSelectorControl)albumSelector;
            if (Module.LocationGallery != null)
            {
                _albumSelector.SectionId = Module.LocationGallery.Id;
                _albumSelector.NodeId = Module.LocationGallery.Node.Id;
            }
            else
            {
                _albumSelector.Visible = false;
            }
            Title = Resources.titleAdminPositionEdit;
            if (!UserIdentity.HasPermission(AccessLevel.Administrator))
            {
                ShowError(Resources.textAccessDenied);
                panelContent.Visible = false;
                return;
            }
            ddlParent.DataTextField = "Name";
            ddlParent.DataValueField = "Id";
            if (!IsPostBack)
            {
                if (Request.QueryString["LocationId"] != null && Convert.ToInt32(Request.QueryString["LocationId"]) > 0)
                {
                    _location = Module.LocationGetById(Convert.ToInt32(Request.QueryString["LocationId"]));
                    TextBoxName.Text = _location.Name;
                    if (!string.IsNullOrEmpty(_location.Image))
                    {
                        imgLocationView.ImageUrl = _location.Image;
                        imgLocationView.Visible = true;
                    }
                    // Nếu địa điểm hiện tại có cha, tức là nó không phải là nút gốc
                    if (_location.Parent != null)
                    {
                        // Nếu địa điểm hiện tại có ông, tức là nó là nút cấp 3 trở lên
                        // Danh sách khi ấy hiển thị toàn bộ nút con của nút ông
                        if (_location.Parent.Parent != null)
                        {
                            Location parent = _location.Parent.Parent;
                            ddlParent.DataSource = parent.Children;
                            ddlParent.DataBind();
                            ddlParent.SelectedValue = _location.Parent.Id.ToString();
                            lblNotFound.Text = String.Empty;
                            ibnUp.Visible = true;
                        }
                            //Nếu không, danh sách hiển thị toàn bộ nút cấp 1
                            // (vì nút hiện tại là nút cấp 2)
                        else
                        {
                            BindParentRoot();
                            ddlParent.SelectedValue = _location.Parent.Id.ToString();
                        }
                    }
                    else // Nếu là nút gốc, đơn giản gọi hàm nút gốc.
                    {
                        BindParentRoot();
                    }
                    // Đã xét hết các trường hợp

                    FCKeditor1.Value = _location.Description;
                }
                else
                {
                    BindParentRoot();
                }
            }
            if (_location == null)
            {
                _location = new Location();
            }
        }
        #endregion

        #region -- PRIVATE METHODS --
        private void BindParentRoot()
        {
            IList list = Module.LocationGetRoot();
            Location noParent = new Location(Resources.textRoot, null, 2, "", "vi-VN");
            list.Add(noParent);
            ddlParent.DataSource = list;
            ddlParent.DataBind();
            ddlParent.SelectedIndex = list.Count - 1;
        }
        #endregion

        #region -- CONTROLS EVENTS --

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["LocationId"] != null && Convert.ToInt32(Request.QueryString["LocationId"]) > 0)
            {
                _location = Module.LocationGetById(Convert.ToInt32(Request.QueryString["LocationId"]));
            }
            else
            {
                _location = new Location();
            }

            #region -- Location data --

            _location.Name = TextBoxName.Text;
            if (Convert.ToInt32(ddlParent.SelectedValue) > 0)
            {
                _location.Parent = Module.LocationGetById(Convert.ToInt32(ddlParent.SelectedValue));
                _location.Level = _location.Parent.Level + 1;
            }
            else
            {
                _location.Parent = null;
                // WARNING: level = 3 is default for conntry!
                _location.Level = 3;
            }
            _location.Description = FCKeditor1.Value;
            _location.LanguageKey = "vi-VN";
            if (FileUploadAbtractImage.HasFile)
            {
                _location.Image = FileHelper.Upload(FileUploadAbtractImage, "Image\\Locations\\");
            }

            try
            {
                if (_albumSelector.SelectedAlbumId > 0)
                {
                    _location.Album = Module.AlbumGetById(_albumSelector.SelectedAlbumId);
                }
            }
            catch
            {
                _location.Album = null;
            }

            #endregion

            if (_location.Id > 0)
            {
                Module.Update(_location);
            }
            else
            {
                Module.Save(_location);
                _location.Order = _location.Id;
                Module.Update(_location);
            }


            // Test: Automatic add all country
            //foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            //{
            //    Location location = new Location(ci.DisplayName, null, 3, "", "vi-VN");
            //    location.Image = string.Empty;
            //    Module.Save(location);
            //}

            if (_location.Parent != null)
            {
                PageRedirect(string.Format("PositionList.aspx?NodeId={0}&SectionId={1}&LocationId={2}",
                                                Request.QueryString["NodeId"], Request.QueryString["SectionId"],
                                                _location.Parent.Id));
            }
            else
            {
                PageRedirect(string.Format("PositionList.aspx?NodeId={0}&SectionId={1}",
                                                Request.QueryString["NodeId"], Request.QueryString["SectionId"]));
            }
        }

        #region -- Parent Location --

        protected void ibnDown_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlParent.SelectedIndex >= 0 && Convert.ToInt32(ddlParent.SelectedValue) > 0)
            {
                Location parent = Module.LocationGetById(Convert.ToInt32(ddlParent.SelectedValue));
                if (parent.Children.Count > 0)
                {
                    ddlParent.DataSource = parent.Children;
                    ddlParent.DataBind();
                    lblNotFound.Text = String.Empty;
                }
                else
                {
                    lblNotFound.Text = Resources.messageNotFoundAnyLocation;
                }
                if (parent.Parent != null)
                {
                    ibnUp.Visible = true;
                }
            }
        }

        protected void ibnUp_Click(object sender, ImageClickEventArgs e)
        {
            // Lấy địa điểm hiện tại
            Location current = Module.LocationGetById(Convert.ToInt32(ddlParent.Items[0].Value));

            // Nếu địa điểm này có ông, lấy tất cả các con của ông
            if (current.Parent.Parent != null)
            {
                ddlParent.DataSource = current.Parent.Parent.Children;
                ibnUp.Visible = true;
            }
                // Nếu không, lấy tất cả các địa điểm root (không cha)
            else
            {
                ddlParent.DataSource = Module.LocationGetRoot();
                ibnUp.Visible = false;
            }
            ddlParent.DataBind();
            lblNotFound.Text = String.Empty;
        }

        #endregion

        #region -- ALBUM --
        #endregion
        #endregion
    }
}