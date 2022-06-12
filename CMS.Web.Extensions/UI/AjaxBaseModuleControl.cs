using System.Web;
using System.Web.UI;
using CMS.Web.UI;

namespace CMS.Web.Extensions.UI
{
	public class AjaxBaseModuleControl : BaseModuleControl
	{
		#region Constants

		private const string scriptmanagerkey = "isscriptmanageradded";

		#endregion

		#region Constructor

		public AjaxBaseModuleControl()
			: base()
		{
			//Check whether the scriptmanager indicator does exist, can be added 
			//in a template or in a earlier loaded event
			if (!(HttpContext.Current.Items.Contains(scriptmanagerkey)))
			{
				//it doesn't exist, add it to the current context, so after the
				//request is processed, it will be deleted
				this.LoadScriptManager();
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// Scriptmanager class from the ASP.Net Ajax Toolkit
		/// </summary>
		public ScriptManager ScriptManager
		{
			get { return HttpContext.Current.Items[scriptmanagerkey] as ScriptManager; }
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Load standard scriptmanager, and the scriptloader
		/// </summary>
		private void LoadScriptManager()
		{
			if (this.ScriptManager == null)
			{
				//Scriptmanager isn't added yet, add it to the controls collection
				ScriptManager sm = new ScriptManager();
				HttpContext.Current.Items.Add(scriptmanagerkey, sm);
				this.Controls.Add(sm);
			}
		}


		#endregion
	}
}