<%@ Page Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true"
    CodeBehind="IncomeReport.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.IncomeReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
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
                <asp:Button ID="btnDisplay" runat="server" Text="Display" OnClick="btnDisplay_Click"
                    CssClass="btn btn-primary" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <table class="table table-bordered table-common">
                <asp:Repeater ID="rptBookingList" runat="server" OnItemDataBound="rptBookingList_ItemDataBound"
                    OnItemCommand="rptBookingList_ItemCommand">
                    <HeaderTemplate>
                        <tr class="active">
                            <th>
                                <%= base.GetText("textDate") %>
                            </th>
                            <th>
                                <%= base.GetText("textCheckInPax") %>
                            </th>
                            <asp:Repeater ID="rptTrip" runat="server">
                                <ItemTemplate>
                                    <th>
                                        <%# DataBinder.Eval(Container.DataItem,"TripCode") %>
                                    </th>
                                </ItemTemplate>
                            </asp:Repeater>
                            <th>Total
                            </th>
                            <!--<th>
                                        Doanh thu quầy bar</th>-->
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="item">
                            <td>
                                <asp:HyperLink ID="hplDate" runat="server"></asp:HyperLink>
                            </td>
                            <td>
                                <asp:Literal ID="litTotalPax" runat="server"></asp:Literal>
                            </td>
                            <asp:Repeater ID="rptTrip" runat="server" OnItemDataBound="rptItemTrip_ItemDataBound">
                                <ItemTemplate>
                                    <td>
                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal></td>
                                </ItemTemplate>
                            </asp:Repeater>
                            <td>
                                <asp:Literal ID="litTotal" runat="server"></asp:Literal></td>
                            <!--<td>
                                        <asp:Literal ID="litBar" runat="server"></asp:Literal></td>-->
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="item" style="background-color: #E9E9E9">
                            <td>
                                <asp:HyperLink ID="hplDate" runat="server"></asp:HyperLink>
                            </td>
                            <td>
                                <asp:Literal ID="litTotalPax" runat="server"></asp:Literal>
                            </td>
                            <asp:Repeater ID="rptTrip" runat="server" OnItemDataBound="rptItemTrip_ItemDataBound">
                                <ItemTemplate>
                                    <td>
                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal></td>
                                </ItemTemplate>
                            </asp:Repeater>
                            <td>
                                <asp:Literal ID="litTotal" runat="server"></asp:Literal></td>
                            <!--<td>
                                        <asp:Literal ID="litBar" runat="server"></asp:Literal></td>-->
                        </tr>
                    </AlternatingItemTemplate>
                    <FooterTemplate>
                        <tr>
                            <th>GRAND TOTAL</th>
                            <th>
                                <asp:Literal ID="litTotalPax" runat="server"></asp:Literal>
                            </th>
                            <asp:Repeater ID="rptTrip" runat="server" OnItemDataBound="rptFooterTrip_ItemDataBound">
                                <ItemTemplate>
                                    <th>
                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal></th>
                                </ItemTemplate>
                            </asp:Repeater>
                            <th>
                                <asp:Literal ID="litTotal" runat="server"></asp:Literal></th>
                            <!--<th>
                                        <asp:Literal ID="litBar" runat="server"></asp:Literal></th>-->
                        </tr>
                    </FooterTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <fieldset>
        <div>
            <div class="data_table">
                <div class="data_grid">
                </div>
            </div>
        </div>
    </fieldset>
</asp:Content>
