<%@ Page Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true"
    CodeBehind="BookingReportPeriodAll.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.BookingReportPeriodAll" %>

<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Booking by period</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="row">
        <div class="col-xs-1 --no-padding-right" style="width: 3%">
            From
        </div>
        <div class="col-xs-2">
            <asp:TextBox ID="txtFrom" runat="server" data-control="datetimepicker" CssClass="form-control" placeholder="From(dd/mm/yyyy)"></asp:TextBox>
        </div>
        <div class="col-xs-1 --no-padding-right" style="width: 2%">
            To
        </div>
        <div class="col-xs-2 --no-padding-right">
            <asp:TextBox ID="txtTo" runat="server" data-control="datetimepicker" CssClass="form-control" placeholder="To(dd/mm/yyyy)"></asp:TextBox>
        </div>
        <div class="col-xs-1">
            <asp:Button ID="btnDisplay" runat="server" Text="Display" OnClick="btnDisplay_Click"
                CssClass="btn btn-primary" />
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-xs-12">
            <table class="table table-bordered table-common">
                <asp:Repeater ID="rptBookingList" runat="server" OnItemDataBound="rptBookingList_ItemDataBound"
                    OnItemCommand="rptBookingList_ItemCommand">
                    <HeaderTemplate>
                        <tr class="active">
                            <th style="width: 80px;">
                                <%= base.GetText("textDate") %>
                            </th>
                            <asp:Repeater ID="rptTripRow" runat="server" OnItemDataBound="rptTripRow_ItemDataBound">
                                <ItemTemplate>
                                    <th id="thTrip" runat="server" colspan="2"></th>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Literal ID="litTr" runat="server"></asp:Literal>
                        <td>
                            <asp:HyperLink ID="hplDate" runat="server"></asp:HyperLink>
                        </td>
                        <asp:Repeater ID="rptTripCustomer" runat="server" OnItemDataBound="rptTripCustomer_ItemDataBound">
                            <ItemTemplate>
                                <td>
                                    <asp:Literal ID="litGroup" runat="server"></asp:Literal></td>
                                <td>
                                    <asp:Literal ID="litCustomer" runat="server"></asp:Literal></td>
                            </ItemTemplate>
                        </asp:Repeater>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <asp:Button ID="btnExcel" runat="server" Text="Export" OnClick="btnExcel_Click" CssClass="btn btn-primary" />
        </div>
    </div>
</asp:Content>
