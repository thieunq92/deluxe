<%@ Page Language="C#" MasterPageFile="SailsMaster.Master" AutoEventWireup="true"
    Codebehind="SetPermission.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.SetPermission"
    Title="Untitled Page" %>

<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <fieldset>
        <legend>Set permission</legend>
        <div class="data_table">
            <h4>
                Roles</h4>
            <div class="pager">
                <svc:Pager ID="pagerRoles" runat="server" ControlToPage="rptRoles" OnPageChanged="pagerRoles_PageChanged" />
            </div>
            <div class="data_grid">
                <table>
                    <asp:Repeater ID="rptRoles" runat="server" OnItemDataBound="rptRoles_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Literal ID="litName" runat="server"></asp:Literal></td>
                                <td>
                                    <asp:HyperLink ID="hplSetPermission" runat="server">Set permission</asp:HyperLink></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
        <div class="data_table">
            <h4>
                User</h4>
            <div class="pager">
                <svc:Pager ID="pagerUser" runat="server" ControlToPage="rptUsers" OnPageChanged="pagerUser_PageChanged" />
            </div>
            <div class="data_grid">
                <table>
                    <asp:Repeater ID="rptUsers" runat="server"  OnItemDataBound="rptUsers_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Literal ID="litName" runat="server"></asp:Literal></td>
                                <td>
                                    <asp:HyperLink ID="hplSetPermission" runat="server">Set permission</asp:HyperLink></td>
                                <td>
                                    <asp:HyperLink ID="hplSetOrganization" runat="server">Set region</asp:HyperLink>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
    </fieldset>
</asp:Content>
