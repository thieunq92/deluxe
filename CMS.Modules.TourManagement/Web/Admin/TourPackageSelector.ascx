<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TourPackageSelector.ascx.cs" Inherits="CMS.Modules.TourManagement.Web.Admin.TourPackageSelector" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:UpdatePanel ID="updatePanel1" runat="server">
    <ContentTemplate>
        <ul>
            <li>
                <table>
                    <asp:Repeater ID="rptTourConfigs" runat="server" OnItemDataBound="rptTourConfigs_ItemDataBound" OnItemCommand="rptTourConfigs_ItemCommand">
                        <HeaderTemplate>
                            <tr>
                                <th style="width:120px;">Tour name</th>
                                <th style="width:120px;">Provider</th>
                                <th><%#base.GetText("labelAction") %></th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="width:120px;">
                                    <asp:Label ID="labelName" runat="server"></asp:Label>
                                </td>
                                <td style="width:120px;">
                                    <asp:Label ID="labelProvider" runat="server"></asp:Label>
                                </td>
                                <td style="width:120px;">
                                    <asp:Label ID="labelTime" runat="server"></asp:Label>
                                </td>
                                <td style="width:120px;">
                                    <asp:Label ID="labelNetPrice" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:ImageButton runat="server" ID="imageButtonDelete" ToolTip='Delete' ImageUrl="/Admin/AdminModuleImages/delete_file.gif" AlternateText='<%# base.GetText("ibtDelete") %>' ImageAlign="AbsMiddle" CssClass="image_button16" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'/>
                                    <ajax:ConfirmButtonExtender ID="ConfirmButtonExtenderDelete" runat="server" TargetControlID="imageButtonDelete" ConfirmText='<%# base.GetText("messageConfirmDelete") %>'>
                                    </ajax:ConfirmButtonExtender>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </li>
        </ul>
    </ContentTemplate>
</asp:UpdatePanel>

<ul>
    <li>
        <table>
        <asp:Repeater ID="rptTourPackages" runat="server" OnItemCommand="rptTourPackages_ItemCommand">
            <ItemTemplate>
                <tr>
                    <td>
                <%# DataBinder.Eval(Container.DataItem,"Name") %>
                </td>
                <td><asp:Button ID="btnSelect" runat="server" CommandName="Select" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' Text="Select"/></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        </table>
    </li>
</ul>