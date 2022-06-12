<%@ Page Language="C#" MasterPageFile="TourMaster.Master" AutoEventWireup="true" Codebehind="PositionEdit.aspx.cs"
    Inherits="CMS.Modules.TourManagement.Web.Admin.PositionEdit" Title="Location Edit"
    ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Src="/Modules/Gallery/AlbumSelector.ascx" TagPrefix="asp" TagName="AlbumSelector" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
<asp:Panel ID="panelContent" runat="server">
    <fieldset>
        <legend>
            <asp:Label ID="labelLocationEdit" runat="server" Text="Location"></asp:Label></legend>
        <div class="settinglist">
			<svc:Mirror ID="mirrorSubmit" runat="server" ControlIDToMirror="ButtonSubmit"></svc:Mirror>
        	<div class="basicinfo">
        		<table>
        		    <tr>
        			<td style="width: 12%"><asp:Label ID="LabelName" runat="server" Text="Tên địa điểm"></asp:Label></td>
        			<td style="width: 37%"><asp:TextBox ID="TextBoxName" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="RfvName" runat="server" ErrorMessage="Field is required" ControlToValidate="TextBoxName" Display="Dynamic"></asp:RequiredFieldValidator></td>
        			<td style="width: 12%"><asp:Label ID="labelParent" runat="server" Text="Nằm trong địa điểm:"></asp:Label></td>
        			<td style="width: 37%"> <asp:UpdatePanel runat="server" ID="updatePanelParent">
                        <ContentTemplate><asp:DropDownList ID="ddlParent" runat="server">
                                </asp:DropDownList>
                                <asp:ImageButton ID="ibnDown" OnClick="ibnDown_Click" runat="server" ToolTip='<%# base.GetText("ibnDown") %>'
                                    ImageUrl="/Admin/Images/downred.gif" CssClass="button" AlternateText="Down">
                                </asp:ImageButton>
                                <asp:Label runat="server" ID="lblNotFound"></asp:Label>
                                <asp:ImageButton ID="ibnUp" OnClick="ibnUp_Click" runat="server" ToolTip='<%# base.GetText("ibnUp") %>'
                                    ImageUrl="/Admin/Images/upred.gif" CssClass="button" AlternateText="Up"
                                    Visible="false"></asp:ImageButton></ContentTemplate>
                    </asp:UpdatePanel></td>
                    </tr>
                    <tr>
                        <td>Album</td>
                        <td><asp:AlbumSelector ID="albumSelector" runat="server"></asp:AlbumSelector></td>
                        <td></td>
                        <td></td>                        
                    </tr>
        		</table>
        	</div>
        	
        	<div class="advancedinfo">
        	<h4><asp:Label ID="LabelAbtractImage" runat="server"></asp:Label></h4>
                    <asp:RegularExpressionValidator ID="RevImage" runat="server" ErrorMessage="Định dạng file không hỗ trợ"
                        ValidationExpression=".*.(jpg|jpeg|png|bmp|gif)" ControlToValidate="FileUploadAbtractImage">
                    </asp:RegularExpressionValidator><br />
                    <asp:FileUpload ID="FileUploadAbtractImage" runat="server" />
                    <br />
                    <asp:Image runat="server" ID="imgLocationView" Visible="false" Width="400" />        	
        	</div>
        	
        	<div class="advancedinfo">
        	<h4><asp:Label ID="lblDescription" runat="server" Text="Thông tin mô tả"></asp:Label></h4>
                    <FCKeditorV2:FCKeditor ID="FCKeditor1" runat="server" Width="100%" Height="200" BasePath="~/support/fckeditor/"
                        ToolbarSet="Default">
                    </FCKeditorV2:FCKeditor>
        	</div>
        	<asp:Button ID="ButtonSubmit" runat="server" Text="Submit" CssClass="button"
                            OnClick="ButtonSubmit_Click"></asp:Button>
        </div>
    </fieldset>
    </asp:Panel>
</asp:Content>
