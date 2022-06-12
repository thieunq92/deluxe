<%@ Page Language="C#" MasterPageFile="SailsMaster.Master" AutoEventWireup="true"
    CodeBehind="TripManager.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.TripManager"
    Title="Untitled" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <fieldset>
        <legend>
            <img alt="Room" src="../Images/sails.gif" align="absMiddle" />
            Trip Manager
        </legend>
        <div class="search_panel">
            <table>
                <tr>
                    <td>From</td>
                    <td>
                        <asp:TextBox ID="txtFrom" runat="server"></asp:TextBox>
                        <ajax:CalendarExtender ID="calendarFrom" runat="server" TargetControlID="txtFrom"
                            Format="dd/MM/yyyy">
                        </ajax:CalendarExtender>
                    </td>
                    <td>To</td>
                    <td>
                        <asp:TextBox ID="txtTo" runat="server"></asp:TextBox>
                        <ajax:CalendarExtender ID="calendarTo" runat="server" TargetControlID="txtTo" Format="dd/MM/yyyy">
                        </ajax:CalendarExtender>
                    </td>
                    <td>Trip Code</td>
                    <td>
                        <asp:TextBox ID="txtTripCode" runat="server">
                        </asp:TextBox></td>
                    <td>Status
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnDisplay" runat="server" Text="Display" OnClick="btnDisplay_Click"
            CssClass="button" />
        <div class="data_table">
            <div class="data_grid">
                <table cellspacing="1">
                    <tr class="header">
                        <th>STT
                        </th>
                        <th>Date
                        </th>
                        <th>TripCode
                        </th>
                        <th>Cost
                        </th>
                        <th>Status
                        </th>
                    </tr>
                    <asp:Repeater ID="rptTripManager" runat="server" OnItemDataBound="rptTripManager_ItemDataBound">
                        <ItemTemplate>
                            <tr id="trItem" runat="server" class="item">
                                <td>
                                    <asp:Literal ID="ltrIndex" runat="server"></asp:Literal></td>
                                <td>
                                    <asp:Literal ID="ltrDate" runat="server"></asp:Literal></td>
                                <td>
                                    <asp:Literal ID="ltrTripCode" runat="server"></asp:Literal>
                                </td>
                                <td><%# Eval("Cost")%></td>
                                <td>
                                    <asp:Literal ID="ltrStatus" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:HyperLink ID="hplPay" runat="server">Pay</asp:HyperLink>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
            <div class="pager">
                <svc:Pager ID="pagerTripManager" runat="server" HideWhenOnePage="true" ControlToPage="rptTripManager"
                    OnPageChanged="pagerTripManager_PageChanged" PageSize="20" />
            </div>
        </div>
    </fieldset>
</asp:Content>

