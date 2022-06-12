<%@ Page Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true"
    CodeBehind="CruiseConfig.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.CruiseConfig" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Fixed expenses</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="row">
        <div class="col-xs-12">
            <table class="table table-bordered table-common">
                <tr class="active">
                    <th>Valid from
                    </th>
                    <th>Valid to
                    </th>
                    <th>Trip
                    </th>
                    <th></th>
                </tr>
                <asp:Repeater ID="rptCruiseTables" runat="server" OnItemDataBound="rptCruiseTables_ItemDataBound"
                    OnItemCommand="rptCruiseTables_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Literal ID="litValidFrom" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litValidTo" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litCruise" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:HyperLink ID="hyperLinkEdit" runat="server">
                                    <i class="fa fa-edit fa-lg" aria-hidden="true" data-toggle="tooltip" data-placement="top" title="Edit"></i>
                                </asp:HyperLink>
                                <asp:LinkButton runat="server" ID="imageButtonDelete" ToolTip='Delete'
                                    CommandName="delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' OnClientClick="return confirm('Are you sure?')">
                                <i class="fa fa-trash fa-lg text-danger" aria-hidden="true" title="" data-toggle="tooltip" data-placement="top" data-original-title="Delete"></i>
                                </asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <input type="button" value="New table" id="inputNew" runat="server" class="btn btn-primary" />
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-xs-1 --width-auto">
            Valid from
        </div>
        <div class="col-xs-2 --width-auto --no-padding-left">
            <asp:TextBox ID="txtValidFrom" runat="server" CssClass="form-control" data-control="datetimepicker"></asp:TextBox>
        </div>
        <div class="col-xs-1 --width-auto --no-padding-left">
            Valid to
        </div>
        <div class="col-xs-2 --width-auto --no-padding-left">
            <asp:TextBox ID="txtValidTo" runat="server" CssClass="form-control" data-control="datetimepicker"></asp:TextBox>
        </div>
        <div class="col-xs-1 --width-auto --no-padding-left">
            Trip
        </div>
        <div class="col-xs-2 --width-auto --no-padding-left">
            <asp:DropDownList ID="ddlCruises" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-xs-12" style="width: 42%">
            <table class="table table-borderless table-common">
                <tr>
                    <th>Customer from</th>
                    <th>Customer to</th>
                    <th>TA price</th>
                    <th>Public price</th>
                </tr>
                <asp:Repeater ID="rptCruiseExpenses" runat="server" OnItemDataBound="rptCruiseExpenses_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td style="padding-left: 0!important">
                                <asp:HiddenField ID="hiddenId" runat="server" />
                                <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control" placeholder="Customer from" data-inputmask="'alias': 'integer', 'groupSeparator': ',', 'autoGroup': true, 'rightAlign': false"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="txtTo" runat="server" CssClass="form-control" placeholder="Customer to" data-inputmask="'alias': 'integer', 'groupSeparator': ',', 'autoGroup': true, 'rightAlign':false"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="txtCost" runat="server" CssClass="form-control" placeholder="TA price" data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="txtPublicPrice" runat="server" CssClass="form-control" placeholder="Public price" data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true"></asp:TextBox></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" CssClass="btn btn-primary" />
            <asp:Button ID="btnAddRow" runat="server" OnClick="btnAddRow_Click" CssClass="btn btn-primary"
                Text="Add row" />
        </div>
    </div>
</asp:Content>
