using System.Collections.Generic;
using CMS.Core.Security;

namespace CMS.Web.Extensions.Components
{
	public interface IRequestParameters
	{
		/// <summary>
		/// ServiceName of the request
		/// </summary>
		string ServiceName { get; }

		/// <summary>
		/// Get another parameter from the request
		/// </summary>
		/// <param name="parametername">name of the parameter</param>
		/// <returns>the requested parametervalue</returns>
		string GetParameter(string parametername);

		/// <summary>
		/// Get all parameters from the request
		/// </summary>
		/// <returns>A dictionary with all parameters from the request</returns>
		IDictionary<string, string> GetAllParameters();

		/// <summary>
		/// Get the user which performed the request
		/// </summary>
		BitPortalPrincipal User { get; }
	}
}