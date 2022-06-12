<%@ Control Language="c#" AutoEventWireup="false" Inherits="CMS.Web.Extensions.UI.AjaxBaseTemplate" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register TagPrefix="uc1" TagName="navigation" Src="~/Controls/Navigation/NavigationLevelZeroOne.ascx" %>
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
<script src="/js/equalcolumns.js" type="text/javascript"></script> 	

<style type="text/css">
div
{
	display: inline;
	background-image : url(transparent.gif);
}
#northwest .out
{
	background-image : url(northwest.jpg);	
}

#northeast .out
{
	background-image : url(northeast.jpg);
}

#hanoi .out
{
	background-image : url(hanoi.jpg);	
}

#northcentral .out
{
	background-image : url(northcentral.jpg);
}

#centralhighland .out
{
	background-image : url(centralhighland.jpg);
}

#southcentral .out
{
	background-image : url(southcentral.jpg);
}

#mekong .out
{
	background-image : url(mekong.jpg);
}

.hover
{
filter:alpha(opacity=50);
opacity: 0.5;	
background-color: red;
}
.out
{
background-color: transperent;
}
</style>

<script>
      function OnMouseHover(element)
      {      
      	this.className = "hover";
      }
      
      function OnMouseOut(element)
      {
      	this.className = "out";
      }

</script>
	</head>
<body>
<form id="t" method="post" runat="server">
<div id="outercontainer">
<div id="container">

<div id="header">
<div style="float:right;">
<asp:PlaceHolder runat="server" ID="LoginArea"></asp:PlaceHolder>
</div>
        <h1>HOTEL MANAGEMENT</h1>
        <uc1:HierarchicalMenu ID="nav" runat="server" />
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
<div style="position: absolute;">
<img src="http://localhost:1988/support/map/map.jpg" style="position: absolute; top:8px;left:8px;"/>


<div id="hanoi">
<div style="position: absolute; top: 146px;left: 278px; z-index: 10; width: 62px; height: 25px;"
onmouseover="this.className='hover'" onmouseout="this.className='out'" class="out" onclick="window.location='http://localhost:1988/default.aspx?LocationId=5'">
</div>
</div>

<div id="northwest">
<div style="position: absolute; top: 31px;left: 117px; z-index: 1; width: 198px; height: 159px;"
onmouseover="this.className='hover'" onmouseout="this.className='out'" class="out" onclick="window.location='http://localhost:1988/default.aspx?LocationId=498'">
</div>
</div>

<div id="northeast">
<div style="position: absolute; top: 50px;left: 310px; z-index: 2; width: 169px; height: 147px;"
onmouseover="this.className='hover'" onmouseout="this.className='out'" class="out" onclick="window.location='http://localhost:1988/default.aspx?LocationId=4'">
</div>
</div>

<div id="northcentral">
<div style="position: absolute; top: 191px;left: 211px; z-index: 3; width: 180px; height: 177px;"
onmouseover="this.className='hover'" onmouseout="this.className='out'" class="out" onclick="window.location='http://localhost:1988/default.aspx?LocationId=499'">
</div>
</div>

<div style="position: absolute; top: 373px;left: 332px; z-index: 2; width: 215px; height: 110px;"
onmouseover="this.className='hover'" onmouseout="this.className='out'" class="out" onclick="window.location='http://localhost:1988/default.aspx?LocationId=7'">
</div>

<div id="centralhighland">
<div style="position: absolute; top: 487px;left: 397px; z-index: 5;width: 74px; height: 213px;"
onmouseover="this.className='hover'" onmouseout="this.className='out'" class="out" onclick="window.location='http://localhost:1988/default.aspx?LocationId=500'">
</div>
</div>

<div id="southcentral">
<div style="position: absolute; top: 489px;left: 442px; z-index: 4; width: 104px; height: 259px;"
onmouseover="this.className='hover'" onmouseout="this.className='out'" class="out" onclick="window.location='http://localhost:1988/default.aspx?LocationId=501'">
</div>
</div>

<div style="position: absolute; top: 694px;left: 331px; z-index: 1; width: 148px; height: 90px;"
onmouseover="this.className='hover'" onmouseout="this.className='out'" onclick="window.location='http://localhost:1988/default.aspx?LocationId=494'">
</div>

<div id="mekong">
<div style="position: absolute; top: 720px;left: 207px; z-index: 4; width: 155px; height: 147px;"
onmouseover="this.className='hover'" onmouseout="this.className='out'" class="out" onclick="window.location='http://localhost:1988/default.aspx?LocationId=6'">
</div>
</div>
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
