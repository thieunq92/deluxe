using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using CMS.Web.Extensions.Util;
using CMS.Web.UI;

namespace CMS.Web.Extensions.UI
{
	public class AjaxBaseTemplate : BaseTemplate
	{

		/// <summary>
		/// In the addedcontrol method we hook up to the init-proces, otherwise
		/// the stuff won't work (oninit is too late)
		/// </summary>
		/// <param name="control"></param>
		/// <param name="index"></param>
		protected override void AddedControl(Control control, int index)
		{
			if (control is HtmlForm)
			{
				ScriptManagerUtil.AddScriptmanagerToForm(control as HtmlForm);
			}
			base.AddedControl(control, index);
		} 

		/// <summary>
		/// Should be public to add stuff to the services of the scriptmanager
		/// </summary>
		public ScriptManager ScriptManager
		{
			get { return ScriptManagerUtil.GetScriptmanagerFromForm((HtmlForm)this.Form); }
		}
	}
}