using System;
using System.Collections;
using System.Web.UI.WebControls;
using CMS.Modules.TourManagement.DataAccess;
using CMS.Modules.TourManagement.Domain;

namespace CMS.Modules.TourManagement.Util
{
    public class LocationHelper
    {
        public static void BindDropDownList(DropDownList dropdownlist, Location currentLocation, ITourDao tourDao,
                                            ImageButton buttonUp, ImageButton buttonDown, Label lblNotFound)
        {
            dropdownlist.DataTextField = "Name";
            dropdownlist.DataValueField = "Id";
            if (currentLocation != null)
            {
                // Nếu địa điểm hiện tại có ông, tức là nó là nút cấp 3 trở lên
                // Danh sách khi ấy hiển thị toàn bộ nút con của nút ông
                if (currentLocation.Parent != null)
                {
                    Location parent = currentLocation.Parent;
                    dropdownlist.DataSource = parent.Children;
                    dropdownlist.DataBind();
                    lblNotFound.Text = String.Empty;
                    buttonUp.Visible = true;
                }
                    //Nếu không, danh sách hiển thị toàn bộ nút cấp 1
                    // (vì nút hiện tại là nút cấp 2)
                else
                {
                    BindParentRoot(dropdownlist,tourDao);
                    dropdownlist.SelectedValue = currentLocation.Id.ToString();
                }
            }
            else // Nếu là nút gốc, đơn giản gọi hàm nút gốc.
            {
                BindParentRoot(dropdownlist, tourDao);
            }
        }

        private static void BindParentRoot(ListControl dropdownlist, ITourDao tourDao)
        {
            IList list = tourDao.LocationGetRoot();
            dropdownlist.DataSource = list;
            dropdownlist.DataBind();
            dropdownlist.SelectedIndex = list.Count - 1;
        }
    }
}