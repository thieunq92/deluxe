using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CMS.Web.Extensions.Components
{
	/// <summary>
	/// Implementation of IAjaxRequestDispatcher, collects all AjaxServices and provides functionality to call
	/// them
	/// </summary>
	public class AjaxRequestDispatcher : IAjaxRequestDispatcher
	{
		#region Private members

		private readonly IDictionary<string, IAjaxService> handlers = new Dictionary<string, IAjaxService>();

		#endregion

		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public AjaxRequestDispatcher()
		{
		}

		/// <summary>
		/// Constructor for an initial set of IAjaxServices
		/// </summary>
		/// <param name="services">Array of IAjaxService which can be requested</param>
		public AjaxRequestDispatcher(IAjaxService[] services)
		{
			foreach (IAjaxService service in services)
			{
				this.AddHandler(service);
			}
		}

		#endregion

		#region IAjaxRequestDispatcher members

		public IDictionary<string, IAjaxService> Handlers
		{
			get { return handlers; }
		}

		public void AddHandler(IAjaxService service)
		{
			try
			{
				if (!handlers.ContainsKey(service.ServiceName))
				{
					handlers[service.ServiceName] = service;
				}
				else
				{
					throw new CuyahogaAjaxException("Cannot add ajaxservice to dispatcher");
				}
			}
			catch (Exception ex)
			{
				throw new CuyahogaAjaxException("Can not add ajaxservice to dispacther", ex);
			}
		}

		public void RemoveHandler(IAjaxService service)
		{
			handlers.Remove(service.ServiceName);
		}

		public virtual string Handle(IRequestParameters requestparams)
		{
			try
			{
				string action = requestparams.ServiceName;
				//get the IAjaxService and  perform the action
				if (Handlers.ContainsKey(action))
				{
					IAjaxService service = Handlers[action];
					//TODO: Add a security check here
					object responseobject = service.Handle(requestparams);
					if (responseobject != null)
					{
						return TransformResponse(responseobject, service.OutputType);
					}
					else
					{
						return null;
					}
				}
				else
					throw new CuyahogaAjaxException("Couln't find any handler for the action: " + action);
			}
			catch (Exception ex)
			{
				return String.Concat(ex.Message, " stacktrace:", ex.StackTrace);
			}
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Transforms an responseojbect
		/// </summary>
		/// <param name="responseobject">the object to transform</param>
		/// <param name="type">OutputType to transform to</param>
		/// <returns>string with transformed response</returns>
		private string TransformResponse(object responseobject, OutputType type)
		{
			if (type == OutputType.JSon)
			{
				return ToJson(responseobject);
			}
			else if (type == OutputType.Text)
			{
				return responseobject.ToString();
			}
			else
			{
				return responseobject.ToString();
			}

		}

		/// <summary>
		/// Transforms an object (or hierarchy) to JSon with Newtonsoft.Json
		/// </summary>
		/// <param name="objecttoserialize"></param>
		/// <returns></returns>
		private string ToJson(object objecttoserialize)
		{
			return JavaScriptConvert.SerializeObject(objecttoserialize);
		}

		#endregion
	}

	public class CuyahogaAjaxException : ApplicationException
	{
		internal CuyahogaAjaxException(string message)
			: base(message)
		{
		}

		internal CuyahogaAjaxException(string message, Exception ex)
			: base(message, ex)
		{
		}
	}
}