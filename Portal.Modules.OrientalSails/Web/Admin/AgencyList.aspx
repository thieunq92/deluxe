<%@ Page Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true"
    CodeBehind="AgencyList.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.AgencyList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Partners manager</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="form-group">
        <div class="row">
            <div class="col-xs-1 --width-auto">
                Name
            </div>
            <div class="col-xs-2 --no-padding-left">
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Name"></asp:TextBox>
            </div>
            <div class="col-xs-1 --width-auto --no-padding-left">
                Role
            </div>
            <div class="col-xs-2 --no-padding-left">
                <asp:DropDownList ID="ddlRoles" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
            <div class="col-xs-1 --width-auto --no-padding-left">
                Sales in charges
            </div>
            <div class="col-xs-2 --no-padding-left">
                <asp:DropDownList ID="ddlSales" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-12">
                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                    CssClass="btn btn-primary" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <table class="table table-bordered table-common">
                <asp:Repeater ID="rptAgencies" runat="server" OnItemDataBound="rptAgencies_ItemDataBound"
                    OnItemCommand="rptAgencies_ItemCommand">
                    <HeaderTemplate>
                        <tr class="active">
                            <th colspan="2">Name
                            </th>
                            <th>Phone
                            </th>
                            <th>Fax
                            </th>
                            <th>Email
                            </th>
                            <th>Contract
                            </th>
                            <th>Sales in charge
                            </th>
                            <th>Role
                            </th>
                            <th>Last booking
                            </th>
                            <th>Price
                            </th>
                            <th>
                                <%= base.GetText("textAction") %>
                            </th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id="trItem" runat="server" class="item">
                            <td>
                                <asp:Literal ID="litIndex" runat="server"></asp:Literal></td>
                            <td class="--text-left">
                                <asp:HyperLink ID="hplName" runat="server"></asp:HyperLink>
                            </td>
                            <td class="--text-left">
                                <%# DataBinder.Eval(Container.DataItem,"Phone") %>
                            </td>
                            <td class="--text-left">
                                <%# DataBinder.Eval(Container.DataItem,"Fax") %>
                            </td>
                            <td class="--text-left">
                                <%# DataBinder.Eval(Container.DataItem,"Email") %>
                            </td>
                            <td>
                                <asp:Literal ID="litContract" runat="server"></asp:Literal>
                                <asp:HyperLink ID="hplContract" runat="server"></asp:HyperLink>
                            </td>
                            <td>
                                <asp:Literal ID="litSale" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="litRole" runat="server"></asp:Literal></td>
                            <td id="tdLastBooking" runat="server"></td>
                            <td>
                                <asp:HyperLink ID="hplPriceSetting" runat="server">Setting
                                </asp:HyperLink>
                            </td>
                            <td>
                                <asp:HyperLink ID="hplEdit" runat="server">
                                    <i class="fa fa-edit fa-lg" aria-hidden="true" data-toggle="tooltip" data-placement="top" title="Edit"></i>
                                </asp:HyperLink>
                                <asp:LinkButton runat="server" ID="imageButtonDelete" CommandName="Delete"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' OnClientClick="return confirm('Are you sure?')">
                                     <i class="fa fa-trash fa-lg text-danger" aria-hidden="true" title="" data-toggle="tooltip" data-placement="top" data-original-title="Delete"></i>
                                </asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <div class="pager">
                <svc:Pager ID="pagerBookings" runat="server" HideWhenOnePage="true" ControlToPage="rptAgencies"
                    OnPageChanged="pagerBookings_PageChanged" PageSize="20" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <asp:Button ID="btnExportAgencyList" runat="server" Text="Export" OnClick="btnExport_Click"
                CssClass="btn btn-primary" />
        </div>
    </div>
</asp:Content>
