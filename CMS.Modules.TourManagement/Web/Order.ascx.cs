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
using CMS.Modules.TourManagement.Domain;
using CMS.Modules.TourManagement.Web.UI;

namespace CMS.Modules.TourManagement.Web
{
    public partial class Order : TourControlBase
    {
        private Tour _tour;

        protected void Page_Load(object sender, EventArgs e)
        {
            _tour = Module.TourGetById(Module.TourId);
            IList listPeople = new ArrayList();
            for (int ii=1; ii<=15; ii++)
            {
                listPeople.Add(ii + " people");
            }
            ddlNumberOfPeople.DataSource = listPeople;
            ddlNumberOfPeople.DataBind();

            IList listInfant = new ArrayList();
            for (int ii=1; ii<=5; ii ++)
            {
                listInfant.Add(ii + " infants");
            }
            ddlInfants.DataSource = listInfant;
            ddlInfants.DataBind();

            IList listChildren = new ArrayList();
            for (int ii = 1; ii <= 5; ii++)
            {
                listChildren.Add(ii + " children");
            }
            ddlChildren.DataSource = listChildren;
            ddlChildren.DataBind();
        }

        protected void buttonSubmit_Click(object sender, EventArgs e)
        {
            PageEngine.PageRedirect(Request.RawUrl.Replace("order", "tour"));
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Module.Section.Title = string.Format("Order for {0}", _tour.Name);
        }
    }
}