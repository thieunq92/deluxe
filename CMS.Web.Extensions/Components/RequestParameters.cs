using System;
using System.Collections.Generic;
using System.Web;
using CMS.Core.Security;

namespace CMS.Web.Extensions.Components
{
	/// <summary>
	/// Implementation of the IRequestParameters interface
	/// </summary>
	public class RequestParameters : IRequestParameters
	{

		#region Private members

		private BitPortalPrincipal _user;
		private string _serviceName;
		private Dictionary<string, string> _otherParameters = new Dictionary<string, string>();

		#endregion

		#region Constructor

		/// <summary>
		/// Create requestparameters from an HttpContext
		/// </summary>
		/// <param name="context">HttpContext from the request</param>
		public RequestParameters(HttpContext context)
		{
			//Get the user, null when not logged in
			this._user = context.User as BitPortalPrincipal;
			//Get the action from the params
			//Add all other params to the dictionary, these are also the server variables etc.
			Uri url = context.Request.Url;
			//Let's get the name of the service from the last part of the url
			string lastpart = url.LocalPath.Substring(url.LocalPath.LastIndexOf("/") + 1);
			//Exclude .CMS.ashx from the servicename
			this._serviceName = lastpart.Replace(".CMS.ashx", String.Empty);
			foreach (string param in context.Request.Params.Keys)
			{
				if (param != "action")
				{
					_otherParameters.Add(param, context.Request.Params[param]);
				}
			}
		}

		#endregion

		#region IRequestParameters Members

		public string GetParameter(string parametername)
		{
			if (this._otherParameters.ContainsKey(parametername))
			{
				return this._otherParameters[parametername];
			}
			else
			{
				throw new CuyahogaAjaxException("Requested parameter not found");
			}
		}

		public BitPortalPrincipal User
		{
			get { return _user; }
		}


		public IDictionary<string, string> GetAllParameters()
		{
			return _otherParameters;
		}

		public string ServiceName
		{
			get { return _serviceName; }
		}

		#endregion
	}
}