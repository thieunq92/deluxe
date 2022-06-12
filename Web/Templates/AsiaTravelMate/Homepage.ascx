<%@ Control Language="c#" AutoEventWireup="false" Inherits="CMS.Web.Extensions.UI.AjaxBaseTemplate" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc1" TagName="navigation" Src="~/Controls/Navigation/NavigationPath.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HierarchicalMenu" Src="~/Controls/Navigation/HierarchicalMenu.ascx" %>
<%@ Register Assembly="CMS.Web" Namespace="CMS.Web.Controls.UserOnlineCounter" TagPrefix="control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title><asp:literal id="PageTitle" runat="server"></asp:literal></title>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<asp:literal id="MetaTags" runat="server" />
	<asp:literal id="Stylesheets" runat="server" />
	<asp:literal id="JavaScripts" runat="server" />
	<link rel="shortcut icon" href="/favicon.ico" />
<!--<script src="/js/equalcolumns.js" type="text/javascript"></script>-->
<script type="text/javascript" src="/js/menueffect.js">

			/***********************************************
			* Blm Multi-level Effect menu- By Brady Mulhollem at http://www.bradyontheweb.com/
			* Script featured on DynamicDrive.com
			* Visit Dynamic Drive at http://www.dynamicdrive.com/ for this script and more
			***********************************************/

</script>
	</head>
<body>
<form id="t" method="post" runat="server">
        <ajaxToolkit:AlwaysVisibleControlExtender ID="ace" runat="server"
    TargetControlID="updateProcessHotel"         
    VerticalSide="Middle"
    HorizontalSide="Center"
    ScrollEffectDuration=".1"/>
        <asp:UpdateProgress ID="updateProcessHotel" runat="server" >
        <ProgressTemplate>
        <img src="/Images/loading.gif" alt="loading" />
        </ProgressTemplate>
        </asp:UpdateProgress>
<div id="outercontainer">
<div id="container">

<div id="header">
<div style="float:right;">
<asp:PlaceHolder runat="server" ID="LoginArea"></asp:PlaceHolder>
</div>
        <h1>HOTEL MANAGEMENT
        </h1>
        <div class="mlmenu horizontal inaccesible">
        <uc1:HierarchicalMenu ID="nav" runat="server" />
        </div>
</div>

<div id="content">
    <!-- left -->
    <div id = "content_left">
    
        <!-- Search Area -->
        <div id = "search_area">
        <div id = "search_area_content">
        <div id = "search_area_bottom">
        <asp:PlaceHolder ID="SearchArea" runat="server">
        </asp:PlaceHolder>
        </div>
        </div>
        </div>
        <!-- End Search Area -->
        
    <!-- Menu -->
        <div class = "box" style="float:right; width:175px;">
        <div class = "box_top">
        <div class = "box_bottom" style="padding-left:20px;">        
        <strong>MENU</strong>
        </div>
        </div>
        </div>

        <div id="location_menu" style="float:right; width:180px;">
        <asp:PlaceHolder ID="LocationMenu" runat="server">
        </asp:PlaceHolder>        
        </div>       
    <!-- End Menu -->
    
    </div>
    <div id = "content_right">
        <div id="side_content">
        <asp:PlaceHolder ID="SideContent" runat="server"></asp:PlaceHolder>
        <asp:PlaceHolder ID="SideContent1" runat="server"></asp:PlaceHolder>
        <asp:PlaceHolder ID="SideContent2" runat="server"></asp:PlaceHolder>
        <asp:PlaceHolder ID="SideContent3" runat="server"></asp:PlaceHolder>        
        </div>
        <div id="main_content">
        <uc1:navigation ID="NavigationPath" runat="server" />
        <asp:PlaceHolder ID="MainContent" runat="server"></asp:PlaceHolder>
        </div>        
    </div>
</div>

<div id="footer">
    <div style="float:left">    
<img src="/Templates/AsiaTravelMate/Images/footer.jpg" />
    </div>
    <span style="float:right; width: 250px;">
    <strong>Asiana Travel Mate <br /></strong>
Vietnam Tours<br />
International Tour Operation License No: 0604/2006/TCDL-GP LHQT.<br />

<strong>Head Office<br /></strong>
Suite 301 Eden Mall, 4 Le Loi Str., Dist. 1, HCMC, Vietnam<br />
T (84-8) 38227458 - 38275169 / F (84-8) 38241384<br />
Email: <a href="mailto:info@asianatravelmate.com">info@asianatravelmate.com</a><br />
Website: <a href="http://www.asianatravelmate.com">www.asianatravelmate.com</a>    <br />
<strong>POWERED BY <a href="http://bitcorp.vn">B.I.T</a></strong>
    </span>
</div>

</div>
</div>
</form>
</body>
</html>
