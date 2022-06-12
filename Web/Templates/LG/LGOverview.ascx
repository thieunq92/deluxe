<%@ Control Language="c#" AutoEventWireup="false" Inherits="CMS.Web.Extensions.UI.AjaxBaseTemplate" %>
<%@ Register Assembly="CMS.Web" Namespace="CMS.Web.Controls.Hitcounter" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="navigation" Src="~/Controls/Navigation/NavigationLevelZeroOne.ascx" %>
<%@ Register TagPrefix="uc2" TagName="subnavigation" Src="~/Controls/Navigation/HierarchicalMenuLevel2.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">	<head runat="server">
		<title><asp:literal id="PageTitle" runat="server"></asp:literal></title>
		<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
		<asp:literal id="MetaTags" runat="server" />
		<asp:literal id="Stylesheets" runat="server" />
		<asp:literal id="JavaScripts" runat="server" />
	    <comment>
	        <style type="text/css">
	            .boxholder
	                {
	                    height: 1200px;
	                }
	        </style>
	    </comment>  		
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
	
	<!-- NAVIGATION BAR -->
    <div class="left-nav180">
    <div class="right-nav180">
	    <div class="navholder-inner">
	        <div>
	        <img id="Img1" src="/Images/logo.png" alt="Olympia logo" runat="server"/>	        
            <div class="search" style="margin-left:250px; margin-top: -170px; height:70px; text-align: right;">
            Liên hệ - Sitemap - Đăng nhập<br /><br />
            <asp:PlaceHolder ID="SearchArea" runat="server"></asp:PlaceHolder>
            </div>	        
            <div id="mainmenu" style="margin-left:250px; margin-top:-3px; #margin-top: -5px;">
            	        <center>
            <uc1:navigation id="Nav1" runat="server"></uc1:navigation>            
            	        </center>
            </div>
		    </div>
	    </div>
    </div>	
	</div>	
	<!-- NAVIGATION BAR -->
	
	<div class="left-border" style="#margin-top: 11px;">
    <div class="right-border">
    
        <!-- LEFT CONTENT -->
	    <div class="boxholder">
	        <div id="leftcontent" style="float:left; width: 250px; margin-left: 1px;">
	        <div id="leftmenu">
	        <uc2:subnavigation id="Nav2" runat="server"/>
	        </div>
	        <br />
	        <asp:PlaceHolder ID="LeftContent" runat="server"></asp:PlaceHolder>
	        <!--<img src="Images/HD2008.png"/>-->
	        <br /><br /><br />
	        <center>
	        Số lượt truy cập <br /><br />
	        <cc1:HitCounter ID="HitCounter1" runat="server" BackColor="Transparent" BorderColor="White"
        ForeColor="Red" TextFileName="counter.config" Padding="7" />    
            </center>
            <br />
	        </div>	    
	    
	    <!-- MIDDLE CONTENT -->
	    	    
	    <div style="float:left; width :490px; margin-left: 20px;">
	        <asp:PlaceHolder ID="MainContent" runat="server"></asp:PlaceHolder>
	        Main conetent
	    </div>
	    
	    <div style="float: left; width :175px; margin-right:-100px;">
	        <asp:PlaceHolder ID="RightContent" runat="server"></asp:PlaceHolder>
	        <br />
	        <asp:PlaceHolder ID="RightContent2" runat="server"></asp:PlaceHolder>
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
	<img src="/Images/vtvlg.png" alt="VTV3 - LG" runat="server"/>
</div>
		</form>
	</body>
</html>
