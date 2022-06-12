using System.Web.UI;
using System.Web.UI.Adapters;
using CMS.Web.Extensions.UI;
using CMS.Web.Extensions.Util;

namespace CMS.Web.Extensions.Components
{
	public class FormRewriterControlAdapter : ControlAdapter
	{
		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(new RewriteFormHtmlTextWriter(writer));
		}
	}

}