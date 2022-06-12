<%@  Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="BookingByOperator.aspx.cs"
    Inherits="Portal.Modules.OrientalSails.Web.Admin.BookingByOperator" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Booking by operator</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="row">
        <div class="col-xs-1 --no-padding-right" style="width:3%">
            From
        </div>
        <div class="col-xs-2">
            <asp:TextBox ID="txtFrom" runat="server" data-control="datetimepicker" CssClass="form-control" placeholder="From(dd/mm/yyyy)"></asp:TextBox>
        </div>
        <div class="col-xs-1 --no-padding-right" style="width:2%">
            To
        </div>
        <div class="col-xs-2 --no-padding-right">
            <asp:TextBox ID="txtTo" runat="server" data-control="datetimepicker" CssClass="form-control" placeholder="To(dd/mm/yyyy)"></asp:TextBox>
        </div>
        <div class="col-xs-1">
            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Report" CssClass="btn btn-primary" />
        </div>
    </div>
    <br/>
    <div class="row">
        <div class="col-xs-12">
            <table class="table table-bordered table-common">
                <asp:Repeater ID="rptDates" runat="server" OnItemDataBound="rptDates_ItemDataBound">
                    <HeaderTemplate>
                        <tr class="active">
                            <th></th>
                            <asp:Repeater ID="rptOperators" runat="server" OnItemDataBound="rptOperators_ItemDataBound">
                                <ItemTemplate>
                                    <th>
                                        <asp:Literal ID="litName" runat="server"></asp:Literal>
                                    </th>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <th>
                                <asp:Literal ID="litDate" runat="server"></asp:Literal></th>
                            <asp:Repeater ID="rptOperators" runat="server" OnItemDataBound="rptOperators_ItemDataBound">
                                <ItemTemplate>
                                    <td style="text-align: right;">
                                        <asp:Literal ID="litCount" runat="server"></asp:Literal>
                                    </td>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <tr>
                            <th>Total</th>
                            <asp:Repeater ID="rptOperators" runat="server" OnItemDataBound="rptOperators_ItemDataBound">
                                <ItemTemplate>
                                    <td style="text-align: right;">
                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                    </td>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tr>
                    </FooterTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
</asp:Content>
