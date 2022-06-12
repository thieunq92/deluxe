<%@ Control Language="c#" AutoEventWireup="false" Inherits="CMS.Web.Extensions.UI.AjaxBaseTemplate" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register TagPrefix="uc1" TagName="navigation" Src="~/Controls/Navigation/NavigationLevelZeroOne.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HierarchicalMenu" Src="~/Controls/Navigation/HierarchicalMenu.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title><asp:literal id="PageTitle" runat="server"></asp:literal></title>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<asp:literal id="MetaTags" runat="server" />
	<asp:literal id="Stylesheets" runat="server" />
	<asp:literal id="JavaScripts" runat="server" />
	</head>
<body>
<form id="t" method="post" runat="server">
<div id="outercontainer">
<div id="container" style="width: 400px; min-height: 300px;">
<asp:PlaceHolder ID="plhContent" runat="server">
</asp:PlaceHolder>
</div>
</div>
</form>
</body>
</html>
