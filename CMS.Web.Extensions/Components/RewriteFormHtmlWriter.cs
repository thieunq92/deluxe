using System.IO;
using System.Web;
using System.Web.UI;

public class RewriteFormHtmlTextWriter : HtmlTextWriter
{
	public RewriteFormHtmlTextWriter(HtmlTextWriter writer)
		: base(writer)
	{
		this.InnerWriter = writer.InnerWriter;
	}

	public RewriteFormHtmlTextWriter(TextWriter writer)
		: base(writer)
	{
		base.InnerWriter = writer;
	}

	public override void WriteAttribute(string name, string value, bool encode)
	{
		if (name == "action")
		{
			HttpContext context = HttpContext.Current;
			if (context.Items["AttributeAlreadyWritten"] == null)
			{
				value = context.Request.RawUrl;
				context.Items["AttributeAlreadyWritten"] = true;
			}
		}
		base.WriteAttribute(name, value, encode);
	}
}