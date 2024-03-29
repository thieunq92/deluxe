using System;
using System.Collections;
using System.Data.SqlTypes;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.ServerControls;
//using NHibernate.Expression;

namespace CMS.Web.Util
{
    public class ValueBinder
    {
        private static string GetValue(object obj)
        {
            if (obj == null) return string.Empty;
            if (obj is INullable)
            {
                if (((INullable)obj).IsNull)
                {
                    return string.Empty;
                }
            }
            if (obj is DateTime)
            {
                return ((DateTime)obj).ToString("dd/MM/yyyy");
            }

            return obj.ToString();
        }

        #region -- Bind --
        public static void BindTextBox(RepeaterItem item, string control, object value)
        {
            TextBox lit = item.FindControl(control) as TextBox;
            if (lit != null && value != null)
            {
                lit.Text = GetValue(value);
            }
        }

        public static void BindHiddenField(RepeaterItem item, string control, object value)
        {
            HiddenField lit = item.FindControl(control) as HiddenField;
            if (lit != null && value != null)
            {
                lit.Value = GetValue(value);
            }
        }

        public static void BindLiteral(RepeaterItem item, string control, object value)
        {
            Literal lit = item.FindControl(control) as Literal;
            if (lit != null && value != null)
            {
                lit.Text = GetValue(value);
            }
        }

        public static void BindLabel(RepeaterItem item, string control, object value)
        {
            Label label = item.FindControl(control) as Label;
            if (label != null && value != null)
            {
                label.Text = GetValue(value);
            }
        }

        public static void BindDropdownList(RepeaterItem item, string control, IEnumerable list, object value, bool isNullable)
        {
            BindDropdownList(item, control, list, value, isNullable, "Name", "Id");
        }

        public static void BindDropdownList(RepeaterItem item, string control, IEnumerable list, object value, bool isNullable, string textfield, string valuefield)
        {
            DropDownList ddl = item.FindControl(control) as DropDownList;
            if (ddl == null) return;
            ddl.DataSource = list;
            ddl.DataTextField = textfield;
            ddl.DataValueField = valuefield;
            ddl.DataBind();

            if (isNullable)
            {
                ddl.Items.Insert(0, "-- Lựa chọn --");
            }

            if (value != null)
            {
                ListItem selected = ddl.Items.FindByValue(value.ToString());
                if (selected != null)
                {
                    selected.Selected = true;
                }
            }
        }

        public static void BindButton(RepeaterItem item, string control, object value)
        {
            IButtonControl lit = item.FindControl(control) as IButtonControl;
            if (lit != null && value != null)
            {
                lit.Text = GetValue(value);
            }
        }

        public static void BindButton(RepeaterItem item, string control, object value, object commandArgument)
        {
            IButtonControl lit = item.FindControl(control) as IButtonControl;
            if (lit != null && value != null)
            {
                lit.Text = GetValue(value);
                lit.CommandArgument = GetValue(commandArgument);
            }
        }
        #endregion

        #region -- Get --
        public static string GetStringValue(RepeaterItem item, string textbox)
        {
            TextBox txt = item.FindControl(textbox) as TextBox;
            if (txt != null)
            {
                return txt.Text;
            }
            return string.Empty;
        }

        public static DateTime? GetDateTimeValue(RepeaterItem item, string textbox)
        {
            TextBox txt = item.FindControl(textbox) as TextBox;
            if (txt != null)
            {
                DateTime date;
                if (DateTime.TryParseExact(txt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                                           out date))
                {
                    return date;
                }
            }
            return null;
        }

        public static string GetSelectedValue(RepeaterItem item, string dropdownlist, bool isNullable)
        {
            DropDownList ddl = item.FindControl(dropdownlist) as DropDownList;
            if (ddl != null)
            {
                if (isNullable && ddl.SelectedIndex == 0) return null;
                return ddl.SelectedValue;
            }
            return null;
        }

        public static string GetSelectedValue(RepeaterItem item, string dropdownlist)
        {
            return GetSelectedValue(item, dropdownlist, true);
        }
        #endregion        

        #region -- Control binder helper --
        public static void BindTextBox(TextBox textBox, object value)
        {
            if (value!=null)
            {
                textBox.Text = GetValue(value);
            }
        }

        public static void BindDropdownList(DropDownList dropDownList, object value)
        {
            if (value!=null)
            {
                ListItem item = dropDownList.Items.FindByValue(GetValue(value));
                if (item!=null)
                {
                    item.Selected = true;
                }
            }
        }
        #endregion

        #region -- Show hide --
        public static void ShowControl(Control item, string id)
        {
            Control ctl = item.FindControl(id);
            ctl.Visible = true;
        }

        public static void HideControl(Control item, string id)
        {
            Control ctl = item.FindControl(id);
            ctl.Visible = false;
        }
        #endregion
    }

    public class TryConvert
    {
        public static Int32 ToInt32(string str)
        {
            int result;
            if (!Int32.TryParse(str, out result))
            {
                result = 0;
            }
            return result;
        }

        public static double ToDouble(string str)
        {
            double result;
            if (!double.TryParse(str, out result))
            {
                result = 0;
            }
            return result;
        }
    }
}
