<%@ Page Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true"
    CodeBehind="ExpenseReport.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.ExpenseReport" %>

<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Expense report</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="form-group">
        <div class="row">
            <div class="col-xs-1 --no-padding-right" style="width: 3%">From</div>
            <div class="col-xs-2">
                <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control" data-control="datetimepicker" placeholder="From(dd/mm/yyyy)"></asp:TextBox>
            </div>
            <div class="col-xs-1 --no-padding-right" style="width: 2%">To</div>
            <div class="col-xs-2 --no-padding-right">
                <asp:TextBox ID="txtTo" runat="server" CssClass="form-control" data-control="datetimepicker" placeholder="To(dd/mm/yyyy)"></asp:TextBox>
            </div>
            <div class="col-xs-1">
                <asp:Button ID="btnDisplay" runat="server" Text="Display" OnClick="btnDisplay_Click"
                    CssClass="btn btn-primary" />
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-12">
                <asp:Repeater ID="rptOrganization" runat="server" OnItemDataBound="rptOrganization_ItemDataBound">
                    <HeaderTemplate>
                        <asp:HyperLink ID="hplOrganization" runat="server" Text="All regions" CssClass="btn btn-default"></asp:HyperLink>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:HyperLink ID="hplOrganization" runat="server" CssClass="btn btn-default"></asp:HyperLink>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
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
            <div class="col-xs-12">
                <table class="table table-bordered table-common">
                    <asp:Repeater ID="rptBookingList" runat="server" OnItemDataBound="rptBookingList_ItemDataBound"
                        OnItemCommand="rptBookingList_ItemCommand">
                        <HeaderTemplate>
                            <tr class="active">
                                <th style="width: 100px">Date</th>
                                <asp:Repeater ID="rptServices" runat="server">
                                    <ItemTemplate>
                                        <th style="width: 100px">
                                            <%# DataBinder.Eval(Container.DataItem,"Name") %>
                                        </th>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <asp:PlaceHolder ID="plhPeriodExpense" runat="server" Visible="false">
                                    <th>Chi phí tháng
                                    </th>
                                    <th>Chi phí năm
                                    </th>
                                </asp:PlaceHolder>
                                <th style="width: 100px">Total
                                </th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="item">
                                <td>
                                    <asp:HyperLink ID="hplDate" runat="server"></asp:HyperLink></td>
                                <asp:Repeater ID="rptServices" runat="server" OnItemDataBound="rptServices_ItemDataBound">
                                    <ItemTemplate>
                                        <td class="--text-right">
                                            <asp:Literal ID="litCost" runat="server"></asp:Literal>
                                        </td>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <asp:PlaceHolder ID="plhPeriodExpense" runat="server" Visible="false">
                                    <td>
                                        <asp:Literal ID="litMonth" runat="server"></asp:Literal></td>
                                    <td>
                                        <asp:Literal ID="litYear" runat="server"></asp:Literal></td>
                                </asp:PlaceHolder>
                                <td class="--text-right">
                                    <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="item" style="background-color: #E9E9E9">
                                <td>
                                    <asp:HyperLink ID="hplDate" runat="server"></asp:HyperLink></td>
                                <asp:Repeater ID="rptServices" runat="server" OnItemDataBound="rptServices_ItemDataBound">
                                    <ItemTemplate>
                                        <td class="--text-right"> 
                                            <asp:Literal ID="litCost" runat="server"></asp:Literal>
                                        </td>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <asp:PlaceHolder ID="plhPeriodExpense" runat="server" Visible="false">
                                    <td>
                                        <asp:Literal ID="litMonth" runat="server"></asp:Literal></td>
                                    <td>
                                        <asp:Literal ID="litYear" runat="server"></asp:Literal></td>
                                </asp:PlaceHolder>
                                <td class="--text-right">
                                    <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                        <FooterTemplate>
                            <tr>
                                <td>TOTAL</td>
                                <asp:Repeater ID="rptServices" runat="server" OnItemDataBound="rptServiceTotal_ItemDataBound">
                                    <ItemTemplate>
                                        <td class="--text-right">
                                            <asp:Literal ID="litCost" runat="server"></asp:Literal>
                                        </td>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <asp:PlaceHolder ID="plhPeriodExpense" runat="server" Visible="false">
                                    <td>
                                        <asp:Literal ID="litMonth" runat="server"></asp:Literal></td>
                                    <td>
                                        <asp:Literal ID="litYear" runat="server"></asp:Literal></td>
                                </asp:PlaceHolder>
                                <td class="--text-right">
                                    <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </FooterTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
