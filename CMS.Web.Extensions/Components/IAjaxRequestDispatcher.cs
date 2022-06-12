using System.Collections.Generic;

namespace CMS.Web.Extensions.Components
{
	/// <summary>
	/// Defines an interface which dispatches ajaxrequest tot the right IAjaxService
	/// </summary>
	public interface IAjaxRequestDispatcher
	{
		/// <summary>
		/// A dictionary with all the handlers which can perform an ajaxaction
		/// </summary>
		IDictionary<string, IAjaxService> Handlers { get; }

		/// <summary>
		/// Add an IAjaxService to the dispatchter
		/// </summary>
		/// <param name="service">IAjaxService to add</param>
		void AddHandler(IAjaxService service);

		/// <summary>
		/// Remove IAjaxService from the dispatcher
		/// </summary>
		/// <param name="service">IAjaxService to remove</param>
		void RemoveHandler(IAjaxService service);

		/// <summary>
		/// Handle an ajaxrequest
		/// </summary>
		/// <param name="requestparams">Requestparameters</param>
		/// <returns>the result of the ajax-operation</returns>
		string Handle(IRequestParameters requestparams);
	}
}