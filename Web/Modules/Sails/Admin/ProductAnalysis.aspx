<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductAnalysis.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.ProductAnalysis"
    MasterPageFile="Main.Master" %>

<%@ Import Namespace="System.Linq" %>
<asp:Content runat="server" ID="Head" ContentPlaceHolderID="Head">
    <title>Product Analysis</title>
</asp:Content>
<asp:Content runat="server" ID="AdminContent" ContentPlaceHolderID="AdminContent">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-xs-9 --no-padding-left">
                    <div class="row">
                        <div class="col-xs-5 --no-padding-right">
                            <asp:DropDownList ID="ddlTrip" runat="server" CssClass="form-control" AppendDataBoundItems="true" AutoPostBack="true">
                                <asp:ListItem Value="-1" Text="-- Select product --"></asp:ListItem>
                                <asp:ListItem Value="0" Text="-- All products --"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-xs-3">
                            <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control" AppendDataBoundItems="true" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                        <div class="col-xs-3">
                            <asp:HyperLink runat="server" ID="hplPriceConfig">Price config</asp:HyperLink>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12">
                            <h4 class="--text-bold">Summary</h4>
                            <table class="table table-bordered table-common" style="width: 60%">
                                <tr class="active">
                                    <td style="width: 20%"></td>
                                    <asp:Repeater runat="server" ID="rptMonths">
                                        <ItemTemplate>
                                            <th><%# ConvertToMonthAndYearString(((DateTime)Container.DataItem)) %></th>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                                <tr>
                                    <th class="--text-left active">Total pax</th>
                                    <asp:Repeater runat="server" ID="rptTotalPax" OnItemDataBound="rptTotalPax_ItemDataBound">
                                        <ItemTemplate>
                                            <td class="--text-right">
                                                <asp:Literal runat="server" ID="ltrTotalPax" /></td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                                <tr>
                                    <th class="--text-left active">Total revenue</th>
                                    <asp:Repeater runat="server" OnItemDataBound="rptTotalRevenue_ItemDataBound" ID="rptTotalRevenue">
                                        <ItemTemplate>
                                            <td class="--text-right">
                                                <asp:Literal runat="server" ID="ltrTotalRevenue"></asp:Literal></td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                                <tr>
                                    <th class="--text-left active">Total expense</th>
                                    <asp:Repeater runat="server" OnItemDataBound="rptExpense_ItemDataBound" ID="rptExpense">
                                        <ItemTemplate>
                                            <td class="--text-right">
                                                <asp:Literal runat="server" ID="ltrExpense"></asp:Literal></td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                                <tr>
                                    <th class="--text-left active">Profit & loss</th>
                                    <asp:Repeater runat="server" OnItemDataBound="rptProfitAndLoss_ItemDataBound" ID="rptProfitAndLoss">
                                        <ItemTemplate>
                                            <td class="--text-right">
                                                <asp:Literal runat="server" ID="ltrProfitAndLoss"></asp:Literal></td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12">
                            <h4 class="--text-bold">
                                <asp:Literal ID="ltrMonth" runat="server"></asp:Literal></h4>
                            <table class="table table-bordered table-common">
                                <tr class="active">
                                    <th>No</th>
                                    <th>Trip code</th>
                                    <th>Revenue</th>
                                    <th>Expense</th>
                                    <th>Profit & Loss</th>
                                </tr>
                                <asp:Repeater ID="rptTrips" OnItemDataBound="rptTrips_ItemDataBound" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Container.ItemIndex + 1 %></td>
                                            <td>
                                                <asp:Literal runat="server" ID="ltrTripCode"></asp:Literal></td>
                                            <td id="tdRevenue" runat="server" class="--text-right">
                                                <asp:Literal runat="server" ID="ltrRevenue"></asp:Literal></td>
                                            <td id="tdExpense" runat="server" class="--text-right">
                                                <asp:Literal runat="server" ID="ltrExpense"></asp:Literal></td>
                                            <td class="--text-right">
                                                <asp:Literal runat="server" ID="ltrProfitAndLoss"></asp:Literal></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <%    
                                            if (((System.Collections.Generic.IEnumerable<Portal.Modules.OrientalSails.Web.Admin.EventCode>)rptTrips.DataSource).Count() <= 0)
                                            {
                                        %>
                                        <tr>
                                            <td colspan="100%">No records found</td>
                                        </tr>
                                        <%
                                            }
                                        %>
                                        <tr>
                                            <th colspan="2">Total</th>
                                            <th class="--text-right"><asp:Literal runat="server" ID="ltrTotalRevenue"></asp:Literal></th>
                                            <th class="--text-right"><asp:Literal runat="server" ID="ltrTotalExpense"></asp:Literal></th>
                                            <th class="--text-right"><asp:Literal runat="server" ID="ltrTotalProfitAndLoss"></asp:Literal></th>
                                        </tr>
                                    </FooterTemplate>
                                </asp:Repeater>

                            </table>
                        </div>
                    </div>
                </div>
                <div class="col-xs-3">
                    <h4 class="--text-bold">Top 10 partner in
                <asp:Literal ID="ltrMonth1" runat="server"></asp:Literal></h4>
                    <table class="table table-borderless table-common">
                        <asp:Repeater runat="server" ID="rptTop10Partner">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Container.ItemIndex + 1 %></td>
                                    <td class="--text-left"><a href="AgencyView.aspx?NodeId=1&SectionId=15&agencyid=<%# Eval("AgencyId") %>"><%# Eval("AgencyName")%></td>
                                    <td><%# Eval("NumberOfPax")%></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <%    
                            if (((IList)rptTop10Partner.DataSource).Count <= 0)
                            {
                        %>
                        <tr>
                            <td colspan="100%">No records found</td>
                        </tr>
                        <%
                            }
                        %>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content runat="server" ID="Scripts" ContentPlaceHolderID="Scripts">
    <script>

    </script>
</asp:Content>
