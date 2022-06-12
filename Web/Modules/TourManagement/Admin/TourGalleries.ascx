<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TourGalleries.ascx.cs" Inherits="CMS.Modules.TourManagement.Web.Admin.TourGalleries" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<ul>
    <li>
        <table>
<asp:Repeater ID="rptRelatedTours" runat="server" OnItemDataBound="rptRelatedTours_ItemDataBound">
    <ItemTemplate>
        <tr>
            <td><asp:Label ID="labelTourName" runat="server"></asp:Label></td>
            <td><asp:ImageButton runat="server" ID="imageButtonDelete" ToolTip='Delete' ImageUrl="/Admin/AdminModuleImages/delete_file.gif" AlternateText='Delete' ImageAlign="AbsMiddle" CssClass="image_button16" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'/>
                <ajax:ConfirmButtonExtender ID="ConfirmButtonExtenderDelete" runat="server" TargetControlID="imageButtonDelete" ConfirmText='Are you sure want to delete this?'>
                </ajax:ConfirmButtonExtender>
            </td>
        </tr>
    </ItemTemplate>
</asp:Repeater>
        </table>
    </li>
    <li>
        <table>
        <asp:Repeater ID="rptTours" runat="server" OnItemDataBound="rptTours_ItemDataBound">
            <ItemTemplate>
            <tr>
                <td><asp:CheckBox ID="chkTour" runat="server" /><asp:HiddenField ID="hiddenId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"Id") %>' /></td>
            </tr>
            </ItemTemplate>
        </asp:Repeater>        
        <tr>
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save"/>
        </tr>
        </table>
    </li>
    <li class="pager">
        <svc:Pager ID="pagerTours" runat="server" ControlToPage="rptTours" OnPageChanged="pagerTours_PageChanged" />
    </li>
</ul>