<%@ Page Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true"
    CodeBehind="BookingHistories.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.BookingHistories" %>

<%@ Import Namespace="Portal.Modules.OrientalSails.Domain" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Booking histories</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="row">
        <div class="col-xs-12">
            <h4 class="--text-bold">Date</h4>
            <table class="table table-bordered table-common">
                <tr class="active">
                    <th style="width: 120px;">Time</th>
                    <th style="width: 150px;">User</th>
                    <th style="width: 300px;">From</th>
                    <th style="width: 300px;">To</th>
                </tr>
                <asp:Repeater runat="server" ID="rptDates" OnItemDataBound="rptDates_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Literal runat="server" ID="litTime"></asp:Literal></td>
                            <td>
                                <asp:Literal runat="server" ID="litUser"></asp:Literal></td>
                            <td>
                                <asp:Literal runat="server" ID="litFrom"></asp:Literal></td>
                            <td>
                                <asp:Literal runat="server" ID="litTo"></asp:Literal></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <h4 class="--text-bold">Trip</h4>
            <table class="table table-bordered table-common">
                <tr class="active">
                    <th style="width: 120px;">Time</th>
                    <th style="width: 150px;">User</th>
                    <th style="width: 300px;">From</th>
                    <th style="width: 300px;">To</th>
                </tr>
                <asp:Repeater runat="server" ID="rptTrips" OnItemDataBound="rptTrips_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Literal runat="server" ID="litTime"></asp:Literal></td>
                            <td>
                                <asp:Literal runat="server" ID="litUser"></asp:Literal></td>
                            <td>
                                <asp:Literal runat="server" ID="litFrom"></asp:Literal></td>
                            <td>
                                <asp:Literal runat="server" ID="litTo"></asp:Literal></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <h4 class="--text-bold">Total value</h4>
            <table class="table table-bordered table-common">
                <tr class="active">
                    <th style="width: 120px;">Time</th>
                    <th style="width: 150px;">User</th>
                    <th style="width: 300px;">From</th>
                    <th style="width: 300px;">To</th>
                </tr>
                <asp:Repeater runat="server" ID="rptTotals" OnItemDataBound="rptTotals_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Literal runat="server" ID="litTime"></asp:Literal></td>
                            <td>
                                <asp:Literal runat="server" ID="litUser"></asp:Literal></td>
                            <td>
                                <asp:Literal runat="server" ID="litFrom"></asp:Literal></td>
                            <td>
                                <asp:Literal runat="server" ID="litTo"></asp:Literal></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <h4 class="--text-bold">Status</h4>
            <table class="table table-bordered table-common">
                <tr class="active">
                    <th style="width: 120px;">Time</th>
                    <th style="width: 150px;">User</th>
                    <th style="width: 300px;">From</th>
                    <th style="width: 300px;">To</th>
                </tr>
                <asp:Repeater runat="server" ID="rptStatus" OnItemDataBound="rptStatus_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Literal runat="server" ID="litTime"></asp:Literal></td>
                            <td>
                                <asp:Literal runat="server" ID="litUser"></asp:Literal></td>
                            <td>
                                <asp:Literal runat="server" ID="litFrom"></asp:Literal></td>
                            <td>
                                <asp:Literal runat="server" ID="litTo"></asp:Literal></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <h4 class="--text-bold">Agencies</h4>
            <table class="table table-bordered table-common">
                <tr class="active">
                    <th style="width: 120px;">Time</th>
                    <th style="width: 150px;">User</th>
                    <th style="width: 300px;">From</th>
                    <th style="width: 300px;">To</th>
                </tr>
                <asp:Repeater runat="server" ID="rptAgencies" OnItemDataBound="rptAgencies_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Literal runat="server" ID="litTime"></asp:Literal></td>
                            <td>
                                <asp:Literal runat="server" ID="litUser"></asp:Literal></td>
                            <td>
                                <asp:Literal runat="server" ID="litFrom"></asp:Literal></td>
                            <td>
                                <asp:Literal runat="server" ID="litTo"></asp:Literal></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <h4 class="--text-bold">Special Request</h4>
            <table class="table table-bordered table-common">
                <tr class="active">
                    <th style="width: 120px;">Time</th>
                    <th style="width: 150px;">User</th>
                    <th style="width: 300px;">From</th>
                    <th style="width: 300px;">To</th>
                </tr>
                <asp:Repeater runat="server" ID="rptSpecialRequest" OnItemDataBound="rptSpecialRequest_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Literal runat="server" ID="litTime"></asp:Literal></td>
                            <td>
                                <asp:Literal runat="server" ID="litUser"></asp:Literal></td>
                            <td>
                                <asp:Literal runat="server" ID="litFrom"></asp:Literal></td>
                            <td>
                                <asp:Literal runat="server" ID="litTo"></asp:Literal></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <h4 class="--text-bold">Customer info</h4>
            <table class="table table-bordered table-common">
                <tr class="active">
                    <th style="width: 120px;">Time</th>
                    <th style="width: 150px;">User</th>
                    <th style="width: 300px;">From</th>
                    <th style="width: 300px;">To</th>
                </tr>
                <asp:Repeater runat="server" ID="rptCustomerInfo" OnItemDataBound="rptCustomerInfo_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Literal runat="server" ID="litTime"></asp:Literal></td>
                            <td>
                                <asp:Literal runat="server" ID="litUser"></asp:Literal></td>
                            <td>
                                <asp:Literal runat="server" ID="litFrom"></asp:Literal></td>
                            <td>
                                <asp:Literal runat="server" ID="litTo"></asp:Literal></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <h4 class="--text-bold">NumberOfPax</h4>
            <table class="table table-bordered table-common">
                <tr class="active">
                    <th style="width: 120px;">Time</th>
                    <th style="width: 150px;">User</th>
                    <th style="width: 300px;">From</th>
                    <th style="width: 300px;">To</th>
                </tr>
                <asp:Repeater runat="server" ID="rptNumberOfPax" OnItemDataBound="rptNumberOfPax_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Literal runat="server" ID="litTime"></asp:Literal></td>
                            <td>
                                <asp:Literal runat="server" ID="litUser"></asp:Literal></td>
                            <td>
                                <asp:Literal runat="server" ID="litFrom"></asp:Literal></td>
                            <td>
                                <asp:Literal runat="server" ID="litTo"></asp:Literal></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>

</asp:Content>
