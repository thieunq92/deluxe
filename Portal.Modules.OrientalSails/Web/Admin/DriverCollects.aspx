<%@ Page Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true"
    CodeBehind="DriverCollects.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.DriverCollects" %>

<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Driver collect</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="form-group">
        <div class="row">
            <div class="col-xs-1">
                From
            </div>
            <div class="col-xs-2">
                <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control" placeholder="From(dd/mm/yyyy)" data-control="datetimepicker"></asp:TextBox>
            </div>
            <div class="col-xs-1">
                To
            </div>
            <div class="col-xs-2">
                <asp:TextBox ID="txtTo" runat="server" CssClass="form-control" placeholder="To(dd/mm/yyyy)" data-control="datetimepicker"></asp:TextBox>
            </div>
            <div class="col-xs-1">
                Driver
            </div>
            <div class="col-xs-2">
                <asp:TextBox ID="txtDriver" runat="server" CssClass="form-control" placeholder="Driver"></asp:TextBox>
            </div>
            <div class="col-xs-1">
                Paid on
            </div>
            <div class="col-xs-2">
                <asp:TextBox ID="txtPaidOn" runat="server" CssClass="form-control" placeholder="Paid on(dd/mm/yyyy)" data-control="datetimepicker"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-1">
                Booking code
            </div>
            <div class="col-xs-2">
                <asp:TextBox ID="txtBookingCode" runat="server" CssClass="form-control" placeholder="Booking code"></asp:TextBox>
            </div>
            <div class="col-xs-1 --no-padding-right">
                Sales in charge
            </div>
            <div class="col-xs-2">
                <asp:DropDownList ID="ddlSales" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
            <div class="col-xs-1">
                Services
            </div>
            <div class="col-xs-2">
                <asp:DropDownList ID="ddlTrips" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-12">
                <asp:Button ID="btnDisplay" runat="server" Text="Display" OnClick="btnDisplay_Click"
                    CssClass="btn btn-primary" />
                <asp:Button ID="btnExportDriverCollect" runat="server" Text="Export DriverCollect"
                    OnClick="btnExportDriverCollect_Click" CssClass="btn btn-primary" />
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-6">
                <asp:Repeater ID="rptOrganization" runat="server" OnItemDataBound="rptOrganization_ItemDataBound">
                    <HeaderTemplate>
                        <asp:HyperLink ID="hplOrganization" runat="server" Text="Your regions" CssClass="btn btn-default"></asp:HyperLink>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:HyperLink ID="hplOrganization" runat="server" CssClass="btn btn-default"></asp:HyperLink>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <div class="col-xs-6 --text-right">
                <span>Payment status:</span>
                <asp:HyperLink ID="hplAllPaid" runat="server" CssClass="btn btn-default">All</asp:HyperLink>
                <asp:HyperLink ID="hplNotPaid" runat="server" CssClass="btn btn-default">Not paid</asp:HyperLink>
                <asp:HyperLink ID="hplPaid" runat="server" CssClass="btn btn-default">Paid</asp:HyperLink>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <table class="table table-bordered table-common">
                <asp:Repeater ID="rptBookingList" runat="server" OnItemDataBound="rptBookingList_ItemDataBound">
                    <HeaderTemplate>
                        <tr class="active">
                            <th rowspan="2">Booking code
                            </th>
                            <th rowspan="2">Driver
                            </th>
                            <th rowspan="2">Sale in charge
                            </th>
                            <th rowspan="2" style="width: 130px;">Partner
                            </th>
                            <%--                                <th rowspan="2">
                                    Cruise
                                </th>--%>
                            <th rowspan="2">Service
                            </th>
                            <th rowspan="2">Date
                            </th>
                            <th colspan="3">No of pax
                            </th>
                            <th colspan="2">Driver Collect
                            </th>
                            <th colspan="2">Paid
                            </th>
                            <th rowspan="2">Applied rate
                            </th>
                            <th rowspan="2">Receivables
                            </th>
                            <th rowspan="2">Action
                            </th>
                            <th rowspan="2">Paid on
                            </th>
                        </tr>
                        <tr class="active">
                            <th>Adult
                            </th>
                            <th>Child
                            </th>
                            <th>Infant
                            </th>
                            <th>USD
                            </th>
                            <th>VND
                            </th>
                            <th>USD
                            </th>
                            <th>VND
                            </th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id="trItem" runat="server" class="item">
                            <td>
                                <asp:HyperLink ID="hplCode" runat="server"></asp:HyperLink>
                            </td>
                            <td>
                                <asp:Literal ID="litTACode" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litSaleInCharge" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:HyperLink ID="hyperLink_Partner" runat="server"></asp:HyperLink>
                            </td>
                            <%--                                <td>
                                    <asp:HyperLink ID="hplCruise" runat="server"></asp:HyperLink>
                                </td>--%>
                            <td>
                                <asp:Literal ID="litService" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litDate" runat="server"></asp:Literal><asp:HiddenField ID="hiddenId"
                                    runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="label_NoOfAdult" runat="server"></asp:Label>
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="label_NoOfChild" runat="server"></asp:Label>
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="label_NoOfBaby" runat="server"></asp:Label>
                            </td>
                            <td style="text-align: right;">
                                <asp:Literal ID="litDriverCollect" runat="server"></asp:Literal>
                            </td>
                            <td style="text-align: right;">
                                <asp:Literal ID="litDriverCollectVND" runat="server"></asp:Literal>
                            </td>
                            <td style="text-align: right;">
                                <asp:Literal ID="litPaid" runat="server"></asp:Literal>
                            </td>
                            <td style="text-align: right;">
                                <asp:Literal ID="litPaidBase" runat="server"></asp:Literal>
                            </td>
                            <td style="text-align: right;">
                                <asp:Literal ID="litCurrentRate" runat="server"></asp:Literal>
                            </td>
                            <td style="text-align: right;">
                                <asp:Literal ID="litReceivable" runat="server"></asp:Literal>
                            </td>
                            <td style="text-align: right;">
                                <a id="aPayment" runat="server" style="cursor: pointer;">Payment</a>
                            </td>
                            <td>
                                <asp:Literal ID="litPaidOn" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <tr class="item">
                            <td colspan="6">GRAND TOTAL
                            </td>
                            <td style="text-align: right;">
                                <strong>
                                    <asp:Label ID="label_NoOfAdult" runat="server"></asp:Label></strong>
                            </td>
                            <td style="text-align: right;">
                                <strong>
                                    <asp:Label ID="label_NoOfChild" runat="server"></asp:Label></strong>
                            </td>
                            <td style="text-align: right;">
                                <strong>
                                    <asp:Label ID="label_NoOfBaby" runat="server"></asp:Label></strong>
                            </td>
                            <td style="text-align: right;">
                                <strong>
                                    <asp:Label ID="label_TotalPrice" runat="server"></asp:Label></strong>
                            </td>
                            <td style="text-align: right;">
                                <strong>
                                    <asp:Label ID="label_TotalPriceVND" runat="server"></asp:Label></strong>
                            </td>
                            <td style="text-align: right;">
                                <strong>
                                    <asp:Literal ID="litPaid" runat="server"></asp:Literal></strong>
                            </td>
                            <td style="text-align: right;">
                                <strong>
                                    <asp:Literal ID="litPaidBase" runat="server"></asp:Literal></strong>
                            </td>
                            <td></td>
                            <td style="text-align: right;">
                                <strong>
                                    <asp:Literal ID="litReceivable" runat="server"></asp:Literal></strong>
                            </td>
                            <td style="text-align: right;">
                                <a id="aPayment" runat="server" style="cursor: pointer;">Pay all</a>
                            </td>
                        </tr>
                    </FooterTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <svc:Popup ID="popupManager" runat="server">
    </svc:Popup>
</asp:Content>
<asp:Content ID="Scripts" ContentPlaceHolderID="Scripts" runat="server">
    <script type="text/javascript">
        function toggleVisible(id) {
            item = document.getElementById(id);
            if (item.style.display == "") {
                item.style.display = "none";
            }
            else {
                item.style.display = "";
            }
        }
    </script>
</asp:Content>
