using System.Web;
using System.Web.SessionState;
using CMS.Web.Components;
using CMS.Web.Util;

namespace CMS.Web.Extensions.Components
{
	/// <summary>
	/// Handles any ajax-request and returns a response depending on the definition in the service
	/// </summary>
	public class AjaxRequestHandler : IHttpHandler, IReadOnlySessionState
	{
		#region Private members

		private IAjaxRequestDispatcher _dispatcher;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an HttpHandler for AjaxRequests and resolves the dispatchter from the IoC container
		/// </summary>
		public AjaxRequestHandler()
		{
			PortalContainer container = ContainerAccessorUtil.GetContainer();
			this._dispatcher = container.Resolve<IAjaxRequestDispatcher>();
		}

		/// <summary>
		/// Creates an HttpHandler with an exisiting dispatcher
		/// </summary>
		/// <param name="dispatcher">Implementation of an IAjaxRequestDispatcher</param>
		public AjaxRequestHandler(IAjaxRequestDispatcher dispatcher)
		{
			this._dispatcher = dispatcher;
		}

		#endregion

		#region Methods

		public void ProcessRequest(HttpContext context)
		{
			HttpRequest req = context.Request;
			//create new requestparams from the httpcontext
			IRequestParameters requestparams = new RequestParameters(context);

			//handle the parameters and write to the client
			string response = this._dispatcher.Handle(requestparams);
			context.Response.ContentType = "text/plain";
			context.Response.Write(response);
		}

		#endregion

		#region Properties

		public bool IsReusable
		{
			get { return false; }
		}

		#endregion
	}
}