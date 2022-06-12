<%@ Control Language="c#" AutoEventWireup="False" Codebehind="View.ascx.cs" Inherits="CMS.Modules.Gallery.Web.View" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<h1><%= this.Album.Title %></h1>
<asp:UpdatePanel id="updatePanelAlbum" runat="server">
<ContentTemplate>
<div class="view">
	
  <div class="buttonbar">
  	  <asp:hyperlink id="hplGalleryImage1" runat="Server"></asp:hyperlink>&nbsp;&nbsp;
      <asp:ImageButton id="FirstImageButton1" runat="server" AlternateText="First Photo" />&nbsp;&nbsp;
      <asp:ImageButton id="PreviousImageButton1" runat="server" AlternateText="Previous Photo" />&nbsp;&nbsp;
      <asp:ImageButton id="NextImageButton1" runat="server" AlternateText="Next Photo" />&nbsp;&nbsp;
      <asp:ImageButton id="LastImageButton1" runat="server" AlternateText="Last Photo" />
  </div>
                            	
	<asp:Repeater id="PhotoDetailsRepeater" runat="server">
	    <ItemTemplate>
	        <asp:literal id="litImage" runat="server"></asp:literal>
	        <div class="clear"></div>
	        <asp:literal id="litTitle" runat="server"></asp:literal>
	        <br />
	        <asp:HyperLink ID="hplDownload" runat="server" Text='<%# base.GetText("hplDownload") %>'></asp:HyperLink>
	    </ItemTemplate>
	</asp:Repeater>
	
  <div class="buttonbar" style="display:none">
      <asp:hyperlink id="hplGalleryImage2" runat="Server"></asp:hyperlink>&nbsp;&nbsp;
      <asp:ImageButton id="FirstImageButton2" runat="server" AlternateText="First Photo" />&nbsp;&nbsp;
      <asp:ImageButton id="PreviousImageButton2" runat="server" AlternateText="Previous Photo" />&nbsp;&nbsp;
      <asp:ImageButton id="NextImageButton2" runat="server" AlternateText="Next Photo" />&nbsp;&nbsp;
      <asp:ImageButton id="LastImageButton2" runat="server" AlternateText="Last Photo" />
  </div>

	<div class="pager">
		<asp:hyperlink id="hplBack" runat="server"></asp:hyperlink>
	</div>

</div>
</ContentTemplate>
</asp:UpdatePanel>