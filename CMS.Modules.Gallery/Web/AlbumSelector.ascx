<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AlbumSelector.ascx.cs" Inherits="CMS.Modules.Gallery.Web.AlbumSelectorControl" %>
<asp:TextBox ID="textBoxAlbumName" runat="server" Width="120"></asp:TextBox><asp:Label ID="labelAlbumName" runat="server"></asp:Label>
<asp:HiddenField ID="hiddenId" runat="server" />
<input type="button" id="btnSelect" runat="server" class="button" value="Select" />
<input type="button" id="btnRemove" runat="server" value="Remove" class="button" />