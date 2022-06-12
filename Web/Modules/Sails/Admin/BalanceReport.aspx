<%@ Page Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true"
    CodeBehind="BalanceReport.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.BalanceReport" %>

<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Balance report</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="form-group">
        <div class="row">
            <div class="col-xs-12">
                <asp:Repeater ID="rptCruises" runat="server" OnItemDataBound="rptCruises_ItemDataBound">
                    <HeaderTemplate>
                        <asp:HyperLink ID="hplCruises" runat="server" Text="All" CssClass="btn btn-default"></asp:HyperLink>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:HyperLink ID="hplCruises" runat="server" CssClass="btn btn-default"></asp:HyperLink>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-1" style="width:4%">
                Month
            </div>
            <div class="col-xs-1">
                <asp:DropDownList ID="ddlMonths" runat="server" CssClass="form-control">
                    <asp:ListItem>-- All months --</asp:ListItem>
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                    <asp:ListItem>6</asp:ListItem>
                    <asp:ListItem>7</asp:ListItem>
                    <asp:ListItem>8</asp:ListItem>
                    <asp:ListItem>9</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>11</asp:ListItem>
                    <asp:ListItem>12</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-xs-1" style="width:3%">
                Year
            </div>
            <div class="col-xs-1 --no-padding-right">
                <asp:TextBox ID="txtYear" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-xs-1">
                <asp:Button ID="btnDisplay" runat="server" OnClick="btnDisplay_Click" Text="Display"
                    CssClass="btn btn-primary" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <table class="table table-bordered table-common">
                <asp:Repeater ID="rptBalance" runat="server" OnItemDataBound="rptBalance_ItemDataBound">
                    <HeaderTemplate>
                        <tr class="active">
                            <th style="width:10%">Date</th>
                            <th>Income</th>
                            <!--<th>Receivable</th>-->
                            <th>Expense</th>
                            <!--<th>Payable</th>-->
                            <th>Balance</th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Literal ID="litDate" runat="server"></asp:Literal></td>
                            <td class="--text-right">
                                <asp:Literal ID="litIncome" runat="server"></asp:Literal></td>
                            <!--<td><asp:Literal ID="litReceivable" runat="server"></asp:Literal></td>-->
                            <td class="--text-right">
                                <asp:Literal ID="litExpense" runat="server"></asp:Literal></td>
                            <!--<td><asp:Literal ID="litPayable" runat="server"></asp:Literal></td>-->
                            <td class="--text-right">
                                <asp:Literal ID="litBalance" runat="server"></asp:Literal></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <tr>
                            <td>GRAND TOTAL</td>
                            <td>
                                <asp:Literal ID="litIncome" runat="server"></asp:Literal></td>
                            <!--<td><asp:Literal ID="litReceivable" runat="server"></asp:Literal></td>-->
                            <td>
                                <asp:Literal ID="litExpense" runat="server"></asp:Literal></td>
                            <!--<td><asp:Literal ID="litPayable" runat="server"></asp:Literal></td>-->
                            <td>
                                <asp:Literal ID="litBalance" runat="server"></asp:Literal></td>
                        </tr>
                    </FooterTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <fieldset>

        <div class="data_table">
            <div class="data_grid">
            </div>
        </div>
    </fieldset>
</asp:Content>
