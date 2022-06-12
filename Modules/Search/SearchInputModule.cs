using System;

using CMS.Core.Domain;
using CMS.Core.Communication;

namespace CMS.Modules.Search
{
	/// <summary>
	/// Summary description for SearchInputModule.
	/// </summary>
	public class SearchInputModule : ModuleBase, IActionProvider
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="section"></param>
		public SearchInputModule()
		{
		}

		#region IActionProvider Members

		public ActionCollection GetOutboundActions()
		{
			ActionCollection ac = new ActionCollection();
			ac.Add(new CMS.Core.Communication.Action("Search", new string[0]));
			return ac;
		}

		#endregion
	}
}
