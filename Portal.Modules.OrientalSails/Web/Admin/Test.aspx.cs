using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            if (fileUploadImport.HasFile)
            {
                int count = 0;
                string content = System.Text.Encoding.Unicode.GetString(fileUploadImport.FileBytes);
                content = content.Replace("\r\n", "■");
                string[] items = content.Split('■');
                // Bỏ qua dòng thứ nhất
                items[0] = string.Empty;

                // Tách các dòng
                foreach (string item in items)
                {
                    if (string.IsNullOrEmpty(item)) continue;
                    string[] info = item.Split(',');
                    //if (info.Length < LENGTH)
                    //{
                    //    labelResult.Text += string.Format("<br/>ERROR: Cấu trúc file không đúng: {0}", item);
                    //    continue;
                    //}

                    #region -- thông tin sản phẩm --
                    //Product product = Module.ProductGetByToken(info[SERIAL].Trim());
                    //if (product != null)
                    //{
                    //    labelResult.Text += string.Format("<br/>ERROR: Đã tồn tại thiết bị với số serial:{0}", info[SERIAL].Trim());
                    //    continue;
                    //}
                    //product = new Product();

                    //product.eToken = info[SERIAL].Trim();
                    //product.Capicity = Convert.ToInt32(info[DUNGLUONG].Trim());
                    //product.UnitPrice = Convert.ToDouble(info[DONGIA].Trim());

                    //foreach (Product pro in list)
                    //{
                    //    if (pro.eToken == product.eToken)
                    //    {
                    //        continue;
                    //    }
                    //}

                    //foreach (Product pro in imported)
                    //{
                    //    if (pro.eToken == product.eToken)
                    //    {
                    //        continue;
                    //    }
                    //}

                    ////Module.SaveOrUpdate(product, Page.User.Identity as User, group.Date, true);
                    //count++;
                    //imported.Add(product);

                    #endregion
                }

                //foreach (Product pro in imported)
                //{
                //    list.Add(pro);
                //}
                //rptProducts.DataSource = list;
                //rptProducts.DataBind();
                //labelResult.Text += string.Format("<br/>Đã nhập {0} thiết bị thành công", count);
            }
        }
    }
}