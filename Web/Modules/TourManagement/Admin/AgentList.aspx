<%@ Page Language="C#" MasterPageFile="TourMaster.Master" AutoEventWireup="true"
    Codebehind="AgentList.aspx.cs" Inherits="CMS.Modules.TourManagement.Web.Admin.AgentList"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <asp:Panel ID="panelContent" runat="server">
        <fieldset>
            <legend>
                <asp:Label ID="labelStatus" runat="server" Text="Agency"></asp:Label></legend>
            <div class="settinglist">
                <div class="basicinfo">
                    <asp:Label ID="labelUserCount" runat="server"></asp:Label>
                </div>
                <div class="data_table">
                    <div class="data_grid">
                        <table cellspacing="0" cellpadding="0">
                            <asp:Repeater ID="RepeaterArticles" runat="server" OnItemCommand="RepeaterArticles_ItemCommand"
                                OnItemDataBound="RepeaterArticles_ItemDataBound">
                                <HeaderTemplate>
                                    <tr>
                                        <th style="width: 70%;">
                                            <%# base.GetText("textAgencyType") %>
                                        </th>
                                        <th style="width: 20%;">
                                            <%# base.GetText("textCount") %>
                                        </th>
                                        <th>
                                        </th>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:HyperLink ID='AgentViewLink' runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name")%>'>
                                            </asp:HyperLink>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblUserNumber"></asp:Label></td>
                                        <td>
                                            <asp:HyperLink runat="server" ID="hplEdit" ToolTip='Edit'><img class="image_button16" align="absmiddle" src="../Images/edit.gif" alt="Edit"/></asp:HyperLink></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                </div>
            </div>
        </fieldset>
    </asp:Panel>
</asp:Content>
