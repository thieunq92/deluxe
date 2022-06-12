<%@ Page Title="" Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="EventList.aspx.cs"
    Inherits="Portal.Modules.OrientalSails.Web.Admin.EventList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Event list</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="form-group">
        <div class="row">
            <div class="col-xs-1 --no-padding-right --width-auto">
                From
            </div>
            <div class="col-xs-2">
                <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control" data-control="datetimepicker"></asp:TextBox>
            </div>
            <div class="col-xs-1 --no-padding-right --width-auto">
                To
            </div>
            <div class="col-xs-2">
                <asp:TextBox ID="txtTo" runat="server" CssClass="form-control" data-control="datetimepicker"></asp:TextBox>
            </div>
            <div class="col-xs-6">
                <asp:Button ID="buttonSearch" runat="server" OnClick="buttonSearch_Click"
                    CssClass="btn btn-primary" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <table class="table table-bordered table-common">
                <asp:Repeater ID="rptEvents" runat="server" OnItemDataBound="rptEvents_ItemDataBound">
                    <HeaderTemplate>
                        <tr class="active">
                            <th rowspan="2">Event code</th>
                            <th rowspan="2">Date</th>
                            <th rowspan="2">Trip</th>
                            <th rowspan="2" style="width:5%">Number of pax</th>
                            <th colspan="3">Revenue</th>
                            <th colspan="3">Expenses</th>
                            <th rowspan="2">Profit</th>
                        </tr>
                        <tr class="active">
                            <th>Paid</th>
                            <th>Receivable</th>
                            <th>Total</th>
                            <th>Paid</th>
                            <th>Payable</th>
                            <th>Total</th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Repeater ID="rptGroups" runat="server" OnItemDataBound="rptGroups_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:HyperLink ID="hplEventCode" runat="server"></asp:HyperLink></td>
                                    <td>
                                        <asp:HyperLink ID="hplDate" runat="server"></asp:HyperLink></td>
                                    <td>
                                        <asp:HyperLink ID="hplTrip" runat="server"></asp:HyperLink></td>
                                    <td>
                                        <asp:Literal ID="litPax" runat="server"></asp:Literal></td>
                                    <td style="text-align: right;">
                                        <asp:Literal ID="litRevenuePaid" runat="server"></asp:Literal></td>
                                    <td style="text-align: right;">
                                        <asp:Literal ID="litReceivable" runat="server"></asp:Literal></td>
                                    <td style="text-align: right;">
                                        <asp:Literal ID="litRevenue" runat="server"></asp:Literal></td>
                                    <td style="text-align: right;">
                                        <asp:Literal ID="litExpensePaid" runat="server"></asp:Literal></td>
                                    <td style="text-align: right;">
                                        <asp:Literal ID="litPayable" runat="server"></asp:Literal></td>
                                    <td style="text-align: right;">
                                        <asp:Literal ID="litExpense" runat="server"></asp:Literal></td>
                                    <td style="text-align: right;">
                                        <asp:Literal ID="litProfit" runat="server"></asp:Literal></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <div class="pager">
                <svc:Pager ID="pagerEvents" runat="server" HideWhenOnePage="true" ControlToPage="rptEvents" PageSize="100" PagerLinkMode="HyperLinkQueryString" />
            </div>
        </div>
    </div>
</asp:Content>

