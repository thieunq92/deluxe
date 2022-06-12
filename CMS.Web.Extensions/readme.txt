CMS.Web.Extensions

The goal of the extensions-stuff is to make the use of the ASP.Net Ajax stuff from Microsoft, without touching the other parts of Cuyahoga (core, web)

* Enable MS Asp.Net Ajax support

To enable ASP.Net Ajax support (updatepanel, timer, control-toolkit) a ScriptManager should be added to the page. Because Cuyahoga contains modules which can be added more than once to one webpage. It is not possible to add multiple instances of the ScriptManager on one page, so the only possibility is to add it to a Template. That brings us with another problem: every page gets this massive amount of Javascript from ASP.Net Ajax, even when you don't even need it.

CMS.Web.Extensions will solve this problem by dynamically adding a ScriptManager to the page and make sure that only one ScriptManager resides on the page.

To use Updatepanels, Timers or other stuff from ASP.Net Ajax or the Ajax Control Toolkit you only should inherit your Template or Control (in case of a module) from an element from the CMS.Web.Extensions.UI namespace.

To use the stuff in modules, inherit from the AjaxBaseModuleControl:

public partial class AjaxTest1 : CMS.Web.Extensions.UI.AjaxBaseModuleControl
{
	protected void Page_Load(object sender, EventArgs e)
	{
		this.ScriptManager.EnablePageMethods = true;
		
	}
}

As you can see, the AjaxBaseModuleControl gives access to the ScriptManager, which resides on the Control

To use the stuff through your website (in the Template), you can add a ScriptManager to your template. Another way is to let the Template-control inherit from AjaxBaseTemplate. This one also adds the ScriptManager to a page (an the Extensions stuff also checks tath modules won't add another ScriptManager to the page)

It can added to the page in the markup of a template .ascx file:


<%@ Control Language="c#" AutoEventWireup="false" Inherits="CMS.Web.Extensions.UI.AjaxBaseTemplate" %>

To avoid circular references, just add it to the Inherits-attribute and ignore the warnings from Visual Studio, the assembly will be resolved runtime and it just works.

To enable ASP.Net Ajax to work you'll need to change your web.config file (it will be twice as big as before), a sample web.config file in included in the trunk. 

CMS.Web.Extensions addresses also some url-rewriting issues by adding a form.browser file and two classes who perform rewriting of the form-tag in ASP.Net


AjaxRequestHandler/AjaxRequestDispatcher

Because not everyone wants to use the ASP.Net Ajax stuff and the Control Toolkit, Cuyahoga adds another way of Ajax-support to your
website. With the alternative way, you can use all JavaScript-libraries you want and it is a more pure way of using Ajax (much cleaner though).
With this stuff it will be possible to create a site containing only static-html files (for example) and add all kinds of dynamic stuff with Ajax-services based on the Cuyahoga-infrastructure. You can transport data using text or JSon and handle it in your own way, so you keep controlling everything what is happening.

Of course you can perform requests to all separate .aspx files or .ashx HttpHandlers to make a call to the server. The CMS.Web.Extensions assembly provides one single-entry point for every Ajax-request you want to make. Based on the action specified in the querystring or post-parameter the right Service will be resolved and you action will be performed.

You can configure this entrypoint by adding the following line to the web.config file (is added in the sample web.config) in the HttpHandlers section:

<add path="*.CMS.ashx" verb="*" type="CMS.Web.Extensions.Components.AjaxRequestHandler, CMS.Web.Extensions" />

For example (from www.svalmen.nl, using JQuery to perform an Ajax-call) a call to get the latest thumbnails from the gallerymodule:

$.get("/slideshow.CMS.ashx?galleryaction=getlatestthumbnails", 
            function(result){
                var photos = eval('(' + result + ')');  
                setSlideShowThumbs(photos);
            }
        );

The part in the url before '.CMS.ashx' defines the servicename (before this was the action parameter in the querystring)
The galleryaction parameter is an optional/additional parameter from the galleryservice which can provide some additional information.

This call (as you can see with $.get, but it also works wit $.post) calls the central entrypoint for all Ajax-services, the AjaxRequestHandler, which uses the AjaxRequestDispatcher to resolve the right implementation of IAjaxService.

To add Ajax-services (IAjaxService) you only have to implement the IAjaxService interface and register the implementation in the service.config file. The IAjaxService is a simple interface which has the following signature:

public interface IAjaxService
{
	/// <summary>Gets the name of the service used to map client calls to the appropriate service.</summary>
	string ServiceName { get; }

	/// <summary>
	/// Defines the output type for the IAjaxService (JSON/Text)
	/// </summary>
	OutputType OutputType { get; }

	/// <summary>Processes the request and returns a response.</summary>
	/// <param name="requestparams">The request to handle, usually the query string collection.</param>
	/// <returns>A response collection that should make sense to the client.</returns>
	object Handle(IRequestParameters requestparams);
}

The ServiceName must correspond with the part for '.CMS.ashx', this will be called the servicename

The OutputType specifies the OutputType for the Ajax-call, sometimes it will be enough to return plain-text, another way is to encode an object-graph with JSON, this will be done by Newtonsoft.Json.

Handle is the method which will be called by de Dispatcher when the service is picked and it should perform an action, it should return an object (or simple string), the real action will be done here.

To add implementations of IAjaxService to the RequestDispatcher/Handler you should add the following (or similar) lines to the services.config file in the config-folder of the Web-project:


    <!--Extensions-->
    <component
      id="extensions.ajaxdispatcher"
      service="CMS.Web.Extensions.Components.IAjaxRequestDispatcher, CMS.Web.Extensions"
      type="CMS.Web.Extensions.Components.AjaxRequestDispatcher, CMS.Web.Extensions">
      <parameters>
        <services>
          <array type="CMS.Web.Extensions.Components.IAjaxService, CMS.Web.Extensions">
            <item>${samples.getalbumservice}</item>
          </array>
        </services>
      </parameters>
    </component>

    <component
      id="samples.getalbumservice"
      service="CMS.Web.Extensions.Components.IAjaxService, CMS.Web.Extensions"
      type="CMS.Modules.JQuerySlideshow.Ajax.GalleryService, CMS.Modules.JQuerySlideshow">
    </component>

To add any additional services, add a component to the service.config (as samples.getalbumservice) and add the component to the services-array of the IAjaxRequestDispatcher component.


This information should be enough to work with the extensions project, there are a lot of ideas for future development and also to refactor some stuff.

I've used both solutions a few times (ASP.Net ajax and the IAjaxRequestDispatcher), and when you know something about JavaScript and use some cool libraries (Scriptaculous/Prototype, JQuery, ExtJS etc.) the second solution is far more powerful than the solution from Microsoft. I've built a demo with an auto-completion textbox, with the Control Toolkit and one with Scriptaculous, with the Control Toolkit it took me about 2 hours (some weird exceptions were thrown by the System.Web.Extensions assembly). With Scriptaculous about 10 minutes, configure the service add 6 lines of html and javascript, including the libraries and the result is exactly the same as with the Control Toolkit.


Sugestions, additional info, bugfixes, etc, please feel free to mail me at: erwin.berends (at) xs4all.nl






















