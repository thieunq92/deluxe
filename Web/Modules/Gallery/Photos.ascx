<%@ Control Language="c#" AutoEventWireup="True" Codebehind="Photos.ascx.cs" Inherits="CMS.Modules.Gallery.Web.Photos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ register assembly="CMS.ServerControls" namespace="CMS.ServerControls"	tagprefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>

<div class="gallery">

<h1><%= this.Album.Title %></h1>
<asp:HyperLink ID="hplBackToSection" runat="server" Text="Return" style="float:right;"></asp:HyperLink>
<asp:Label ID="labelAlbumDescription" runat="server"></asp:Label>

    <asp:UpdatePanel Id="updatePanelPhotos" runat="server">
    <ContentTemplate>
	<asp:DataList id="PhotoDataList" runat="server" onitemdatabound="PhotoDataList_ItemDataBound" repeatdirection="Horizontal" repeatColumns="2" cssclass="view" enableviewstate="False" CellSpacing="5">
		<itemtemplate>
		    <div>
		    <asp:hyperlink id="hplFile" runat="server"><asp:literal id="litImage" runat="server"></asp:literal></asp:hyperlink>
		    <div class="clear"></div>
		    <asp:literal id="litImageLabel" runat="server" />
		    </div>
		</itemtemplate>
	</asp:DataList>

	<div class="pager">
		<cc1:pager id="pgrPhotos" runat="server" 
		    controltopage="PhotoDataList" 		   
		    hidewhenonepage="true" 
		    onpagechanged="pgrPhoto_PageChanged" 
		     />
	</div>

	<div class="pager">
		<asp:hyperlink id="hplBack" runat="server"></asp:hyperlink>
	</div>
	</ContentTemplate>
	</asp:UpdatePanel>    
</div>