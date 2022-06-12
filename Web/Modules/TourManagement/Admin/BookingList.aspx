<%@ Page Language="C#" MasterPageFile="TourMaster.Master" AutoEventWireup="true"
    Codebehind="BookingList.aspx.cs" Inherits="CMS.Modules.TourManagement.Web.Admin.BookingList"
    Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <svc:Popup ID="popupManager" runat="server"></svc:Popup>
    <fieldset>
        <legend>Booking management</legend>
        <div class="search_panel">
            <table>
                <tr>
                    <td>
                        Code</td>
                    <td>
                        <asp:TextBox ID="txtBkCode" runat="server"></asp:TextBox></td>
                    <td>
                        Name</td>
                    <td>
                        <asp:TextBox ID="txtBkName" runat="server"></asp:TextBox></td>
                    <td>
                        Start date</td>
                    <td>
                        <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox><ajax:CalendarExtender
                            ID="calendarStartDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtStartDate">
                        </ajax:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        Status</td>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server">
                            <asp:ListItem>-- All status --</asp:ListItem>
                            <asp:ListItem>Proposal</asp:ListItem>
                            <asp:ListItem>Not run yet</asp:ListItem>
                            <asp:ListItem>Running</asp:ListItem>
                            <asp:ListItem>Completed</asp:ListItem>
                        </asp:DropDownList></td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                        Running on</td>
                    <td>
                        <asp:TextBox ID="txtRunningOn" runat="server"></asp:TextBox>
                        <ajax:CalendarExtender ID="calendarRunning" runat="server" Format="dd/MM/yyyy" TargetControlID="txtRunningOn">
                        </ajax:CalendarExtender>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button" OnClick="btnSearch_Click" />
        <div class="data_table">
            <div class="pager">
                <svc:Mirror ID="mirrorPager" runat="server" ControlIDToMirror="pagerBookings" />
            </div>
            <div class="data_grid">
                <table>
                    <asp:Repeater ID="rptBookings" runat="server" OnItemDataBound="rptBookings_ItemDataBound">
                        <HeaderTemplate>
                            <tr>
                                <th>
                                    Code
                                </th>
                                <th>
                                    Customer name
                                </th>
                                <th>
                                    Number of pax
                                </th>
                                <th>
                                    Start date
                                </th>
                                <th>
                                    Status
                                </th>
                                <th>
                                    Sale
                                </th>
                                <th>
                                    Brief
                                </th>
                                <th>
                                    Total
                                </th>
                                <th>
                                    Paid
                                </th>
                                <th>
                                    Remain
                                </th>
                                <th>
                                    Action
                                </th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Panel CssClass="hover_content" ID="PopupMenu" runat="server">
                                        <asp:Literal ID="litDetails" runat="server"></asp:Literal>
                                    </asp:Panel>
                                    <asp:HyperLink ID="hplCode" runat="server"></asp:HyperLink>
                                </td>
                                <td>
                                    <asp:Literal ID="litCustomers" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="litCustomerCount" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="litStartDate" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Literal ID="litSale" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Label ID="labelBookingContent" runat="server">Brief</asp:Label>
                                    <ajax:HoverMenuExtender ID="hmeNote" runat="Server" HoverCssClass="popupHover" PopupControlID="PopupMenu"
                                        PopupPosition="Left" TargetControlID="labelBookingContent" PopDelay="25" />
                                </td>
                                <td><asp:Literal ID="litTotal" runat="server"></asp:Literal></td>
                                <td><asp:Literal ID="litPaid" runat="server"></asp:Literal></td>
                                <td><asp:Literal ID="litRemain" runat="server"></asp:Literal></td>
                                <td><a id="aPayment" runat="server" style="cursor:pointer;">Payment</a></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
            <div class="pager">
                <svc:Pager ID="pagerBookings" runat="server" ControlToPage="rptBookings" OnPageChanged="pagerBookings_PageChanged" />
            </div>
        </div>
    </fieldset>
</asp:Content>
