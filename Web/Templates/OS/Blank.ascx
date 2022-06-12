<%@ Control Language="c#" AutoEventWireup="false" Inherits="CMS.Web.Extensions.UI.AjaxBaseTemplate" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc1" TagName="navigation" Src="~/Controls/Navigation/NavigationPath.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HierarchicalMenu" Src="~/Controls/Navigation/HierarchicalMenu.ascx" %>
<%@ Register Assembly="CMS.Web" Namespace="CMS.Web.Controls.UserOnlineCounter" TagPrefix="control" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title><asp:literal id="PageTitle" runat="server"></asp:literal></title>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<asp:literal id="MetaTags" runat="server" />
	<asp:literal id="Stylesheets" runat="server" />
	<asp:literal id="JavaScripts" runat="server" />
	<link rel="shortcut icon" href="/favicon.ico" />	
<script src="/js/equalcolumns.js" type="text/javascript"></script> 	
<script src="/js/menuTab.js" type="text/javascript"></script>

<script type="text/javascript" src="/js/mootools.js"></script>
<script type="text/javascript" src="/js/calendar.rc4.js"></script>

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
<center>
<!--Oriental Sails Content-->
<div id="container">
	<table width="100%" border="0" cellpadding="0" cellspacing="0" height="auto">
      <tr>
        <td width="249" align="left" valign="top">
         <div class="box-left">
         	<div class="content-left">
         	    
	        </div> 
          </div>
        </td>
        <td width="689" align="left" valign="top" >
      <div class="box-right">
              <div id="content">
              	<asp:placeholder id="maincontent" runat="server"></asp:placeholder>
              </div>
      </div>        </td>
      </tr>
    </table>

</div>

</center>
</form>
</body>
</html>
