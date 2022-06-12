using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace CMS.Web.Extensions.Util
{
	public class ScriptManagerUtil
	{
		private const string scriptmanagerindicator = "isscriptmanageradded";

		/// <summary>
		/// Add a scriptmanager to an htmlform
		/// </summary>
		/// <param name="form">The htmlform which should contain the scriptmanager</param>
		public static void AddScriptmanagerToForm(HtmlForm form)
		{
			if (!(HttpContext.Current.Items.Contains(scriptmanagerindicator)))
			{
				HttpContext.Current.Items.Add(scriptmanagerindicator, true);
				form.Controls.AddAt(0, new ScriptManager());
			}
		}

		/// <summary>
		/// Gets the scriptmanager from the form
		/// </summary>
		/// <param name="form"></param>
		/// <returns></returns>
		public static ScriptManager GetScriptmanagerFromForm(HtmlForm form)
		{
			//Always on 0-index, if not, just return null
			return form.Controls[0] as ScriptManager;
		}
	}
}
