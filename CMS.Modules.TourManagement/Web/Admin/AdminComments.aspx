<%@ Page Language="C#" MasterPageFile="TourMaster.Master" AutoEventWireup="true"
    Codebehind="AdminComments.aspx.cs" Inherits="CMS.Modules.TourManagement.Web.Admin.AdminComments"
    Title="Untitled Page" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="/Admin/Controls/UserSelector.ascx" TagPrefix="asp" TagName="UserSelector" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <fieldset>
        <legend>
            <%-- <img src="../Images/landscape.gif" align="absMiddle" alt="landscape" /> --%>
            <%= base.GetText("labelCommentManagement")%>
        </legend>
        <div class="settinglist">
            <div class="advancedinfo" id="divHotelView" runat="server">
                <%--<cc1:HotelView runat="server" ID="HotelInfo" />--%>
            </div>
            <div class="search_panel">
                <table>
                    <tr>
                        <td style="width: 10%">
                            <%= base.GetText("labelPostedUser") %>
                        </td>
                        <td style="width: 23%">
                            <asp:UserSelector ID="userPosted" runat="server">
                            </asp:UserSelector>
                        </td>
                        <td style="width: 10%">
                            <%= base.GetText("PostedAfter") %>
                        </td>
                        <td style="width: 24%">
                            <asp:TextBox ID="txtPostedAfter" runat="server"></asp:TextBox>
                            <ajax:CalendarExtender ID="calendarPostedAfter" runat="server" TargetControlID="txtPostedAfter"
                                Format="dd/MM/yyyy">
                            </ajax:CalendarExtender>
                        </td>
                        <td style="width: 10%">
                            <%= base.GetText("PostedBefore") %>
                        </td>
                        <td style="width: 23%">
                            <asp:TextBox ID="txtPostedBefore" runat="server"></asp:TextBox>
                            <ajax:CalendarExtender ID="calendarPostedBefore" runat="server" TargetControlID="txtPostedBefore"
                                Format="dd/MM/yyyy">
                            </ajax:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= base.GetText("labelInContent") %>
                        </td>
                        <td>
                            <asp:TextBox ID="txtInContent" runat="server"></asp:TextBox></td>
                        <td>
                            <%= base.GetText("labelIP") %>
                        </td>
                        <td>
                            <asp:TextBox ID="txtIP" runat="server"></asp:TextBox></td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_click" CssClass="button" />
            <div class="data_table">
                <asp:UpdatePanel ID="updatePanelHotels" runat="server">
                    <ContentTemplate>
                        <div class="data_grid">
                            <div class="pager">
                            </div>
                            <table cellspacing="0" cellpadding="0">
                                <asp:Repeater ID="rptComments" runat="server" OnItemCommand="rptComments_ItemCommand"
                                    OnItemDataBound="rptComments_ItemDataBound">
                                    <HeaderTemplate>
                                        <tr class="header">
                                            <th style="width: 10%;">
                                                <%# base.GetText("labelAuthorAccount") %>
                                            </th>
                                            <th style="width: 15%;">
                                                <%# base.GetText("labelAuthor") %>
                                            </th>
                                            <th style="width: 15%;">
                                                <%# base.GetText("labelContent") %>
                                            </th>
                                            <th style="width: 15%;">
                                                <%# base.GetText("labelEmail") %>
                                            </th>
                                            <th style="width: 15%;">
                                                <%# base.GetText("labelIP") %>
                                            </th>
                                            <th style="width: 15%;">
                                                <%# base.GetText("labelTour")%>
                                            </th>
                                            <th style="width: 10%;">
                                                <%# base.GetText("labelPosted") %>
                                            </th>
                                            <th>
                                            </th>
                                        </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class="item">
                                            <td>
                                                <asp:HyperLink ID="labelUserName" runat="server"></asp:HyperLink></td>
                                            <td>
                                                <asp:Label ID="labelFullName" runat="server"></asp:Label></td>
                                            <td>
                                                <asp:Label ID="labelOnHover" runat="server"></asp:Label>
                                                <div id="labelContent" runat="server">
                                                </div>
                                            </td>
                                            <td>
                                                <asp:HyperLink ID="labelEmail" runat="server"></asp:HyperLink></td>
                                            <td>
                                                <asp:HyperLink ID="labelIP" runat="server"></asp:HyperLink></td>
                                            <td>
                                                <asp:HyperLink ID="labelLandscape" runat="server"></asp:HyperLink></td>
                                            <td>
                                                <asp:HyperLink ID="labelPosted" runat="server"></asp:HyperLink></td>
                                            <td>
                                                <asp:ImageButton runat="server" ID="btnDelete" ToolTip='Delete' ImageUrl="../Images/delete_file.gif"
                                                    AlternateText='<%# base.GetText("ibtDelete") %>' ImageAlign="AbsMiddle" CssClass="image_button16"
                                                    CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' /><ajax:ConfirmButtonExtender
                                                        ID="ConfirmButtonExtenderDelete" runat="server" TargetControlID="btnDelete" ConfirmText='Are you sure want to delete?'>
                                                    </ajax:ConfirmButtonExtender>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                            <div class="pager">
                                <svc:Pager ID="pagerComments" runat="server" ControlToPage="rptComments" OnPageChanged="pagerComments_PageChanged" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </fieldset>
</asp:Content>
