using System;
using CMS.Core.DataAccess;
using CMS.Core.Domain;
using CMS.Core.Communication;

namespace CMS.Modules.User
{
	/// <summary>
	/// The UserModule provides services for user management, specifically non-admin users.
	/// <remark>
	/// Note that the core user functionality is not in this module but in CMS.Core and
	/// CMS.Web.Util.AuthenticationModule. This class is needed because every module requires 
	/// a corresponding class. Perhaps functionality like 'forgot password?' will be added here in the future.
	/// 
	/// MBO, 20041229: User functionality implemented in core. This module stays pretty empty.
	/// </remark>
	/// </summary>
	public class UserModule : ModuleBase, IActionProvider
	{
		private ActionCollection _outboundActions;
	    private readonly ICommonDao _commonDao;
	    public ICommonDao CommonDao
	    {
            get { return _commonDao; }
	    }

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="section"></param>
		public UserModule(ICommonDao commonDao)
		{
		    _commonDao = commonDao;
			InitOutboundActions();
		}

		private void InitOutboundActions()
		{
			this._outboundActions = new ActionCollection();
			this._outboundActions.Add(new CMS.Core.Communication.Action("ViewProfile", new string[1] {"UserId"}));
			this._outboundActions.Add(new CMS.Core.Communication.Action("EditProfile", null));
			this._outboundActions.Add(new CMS.Core.Communication.Action("Register", null));
			this._outboundActions.Add(new CMS.Core.Communication.Action("ResetPassword", null));
		}

		#region IActionProvider Members

		public ActionCollection GetOutboundActions()
		{
			return this._outboundActions;
		}

		#endregion
	}
}
