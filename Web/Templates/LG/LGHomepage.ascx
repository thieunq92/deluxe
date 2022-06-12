<%@ Control Language="c#" AutoEventWireup="false" Inherits="CMS.Web.Extensions.UI.AjaxBaseTemplate" %>
<%@ Register TagPrefix="uc1" TagName="navigation" Src="~/Controls/Navigation/NavigationLevelZeroOne.ascx" %>
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
<div id="container">

    <!-- Border div -->
    <!-- TOP DIV -->
    <div style="height:10px; width: auto; background-color:#FFFFFF;">
	    <div class="border-left">    
	    <div class="top-right">
	    </div>
	    </div>
	    <div class="top-border">
	    </div>	 
	</div>
	<!-- TOP DIV -->
	
	<!-- CENTER DIV -->
	
	<!-- TOP FLASH HERE -->
    <div class="left-border">
    <div class="right-border">			
	    <div class="boxholder">
	        <div>
	        <img src="Images/vtvlg.png" width=700px" height="200px"/>
		    </div>
	    </div>
    </div>	
	</div>
	<!-- TOP FLASH HERE -->
	
	<!-- NAVIGATION BAR -->
    <div class="left-nav">
    <div class="right-nav">
	    <div class="navholder">
            <div id="mainmenu">
            
            </div>
	    </div>
    </div>	
	</div>	
	<!-- NAVIGATION BAR -->
	
	<div class="left-border">
    <div class="right-border">			
	    <div class="boxholder">
	        <div>
		    something
		    <img src="Images/topflash.png"/>
		    </div>
	    </div>
    </div>	
	</div>
	
	<!-- CENTER DIV -->

    <!-- BOTTOM DIV -->

	<div class="bottom-left">    
	<div class="bottom-right">
	</div>
	</div>
    <div style="height:10px; width: auto; background-color:#FFFFFF;	margin-left: 11px;margin-right: 9px; margin-top:-18px;">
    </div>	 
	<!-- BOTTOM DIV -->
	
	<div class="bottom-shadow" style="">
	</div>
	<img src="Images/vtvlg.png" alt="VTV3 - LG"/>
</div>
</form>
</body>
</html>
