<%@ Page Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true"
    CodeBehind="PayableList.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.PayableList" %>

<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Payables</title>
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
            <div class="col-xs-1 --no-padding-right --width-auto">
                Supplier
            </div>
            <div class="col-xs-2">
                <asp:DropDownList ID="ddlSupplier" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
            <div class="col-xs-1 --no-padding-right --width-auto">
                Service
            </div>
            <div class="col-xs-2">
                <asp:DropDownList ID="ddlCostTypes" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
            <div class="col-xs-1 --no-padding-right --width-auto">
                Trip code
            </div>
            <div class="col-xs-2 --no-padding-right --width-auto">
                <asp:TextBox ID="txtTripCode" runat="server" CssClass="form-control" placeholder="Trip code">
                </asp:TextBox>
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-12">
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
    <div class="row">
        <div class="col-xs-6">
            <asp:Repeater ID="rptServices" runat="server" OnItemDataBound="rptServices_ItemDataBound">
                <HeaderTemplate>
                    <asp:HyperLink ID="hplCostType" runat="server" Text="All" CssClass="btn btn-default"></asp:HyperLink>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:HyperLink ID="hplCostType" runat="server" CssClass="btn btn-default"></asp:HyperLink>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:HyperLink ID="hplCostType" runat="server" Text="Theo số khách" CssClass="btn btn-default"></asp:HyperLink>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <div class="col-xs-6 --text-right">
            Payment status:  
                <asp:HyperLink ID="hplAllPaid" runat="server" CssClass="btn btn-default">All</asp:HyperLink>
            <asp:HyperLink ID="hplNotPaid" runat="server" CssClass="btn btn-default">Not paid</asp:HyperLink>
            <asp:HyperLink ID="hplPaid" runat="server" CssClass="btn btn-default">Paid</asp:HyperLink>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <div id="tabs" style="border:none">
                <ul style="border:none; border-bottom:1px solid #ddd; background:none">
                    <li><a href="#tabs-1">Deluxe Group Tour</a></li>
                    <li><a href="#tabs-2">Hue Street Food</a></li>
                </ul>
                <div id="tabs-1">
                    <div class="data_table">
                        <%--<h4>Tạm thời không hiển thị data để tối ưu hệ thống, vẫn có thể xuất sang excel bình thường</h4>--%>
                        <div class="data_grid">
                            <table class="table table-bordered table-common">
                                <tr class="active">
                                    <th>Date</th>
                                    <th>Trip code
                                    </th>
                                    <th>Partner</th>
                                    <th>Service</th>
                                    <th>Total</th>
                                    <th>Paid</th>
                                    <th>Payable</th>
                                    <th style="width: 180px;"></th>
                                    <th>Paid on</th>
                                </tr>
                                <asp:Repeater ID="rptExpenseServices" runat="server" OnItemDataBound="rptExpenseServices_ItemDataBound"
                                    OnItemCommand="rptExpenseServices_ItemCommand">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:HyperLink ID="hplDate" runat="server"></asp:HyperLink></td>
                                            <td>
                                                <asp:HyperLink ID="hplTripCode" runat="server"></asp:HyperLink>
                                            </td>
                                            <td class="--text-left">
                                                <asp:HyperLink ID="hplPartner" runat="server"></asp:HyperLink></td>
                                            <td>
                                                <asp:HyperLink ID="hplService" runat="server"></asp:HyperLink></td>
                                            <td class="--text-right">
                                                <asp:Literal ID="litTotal" runat="server"></asp:Literal></td>
                                            <td class="--text-right">
                                                <asp:Literal ID="litPaid" runat="server"></asp:Literal></td>
                                            <td class="--text-right">
                                                <asp:Literal ID="litPayable" runat="server"></asp:Literal></td>
                                            <td>
                                                <asp:TextBox ID="txtPay" runat="server" CssClass="form-control" style="display:inline-block;width:70%" data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true"></asp:TextBox>
                                                <asp:Button ID="btnPay" runat="server" Text="Pay" CssClass="btn btn-primary" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' style="display:inline-block"/></td>
                                            <td>
                                                <asp:Literal ID="litPaidOn" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <tr class="--text-bold">
                                            <td colspan="4">Subtotal</td>
                                            <td class="--text-right">
                                                <asp:Literal ID="litTotal" runat="server"></asp:Literal></td>
                                            <td class="--text-right">
                                                <asp:Literal ID="litPaid" runat="server"></asp:Literal></td>
                                            <td class="--text-right">
                                                <asp:Literal ID="litPayable" runat="server"></asp:Literal></td>
                                            <td>
                                                <asp:Button ID="btnPay" runat="server" Text="Pay all" CssClass="btn btn-primary" />
                                                <ajax:ConfirmButtonExtender ID="ConfirmButtonExtenderDelete" runat="server" TargetControlID="btnPay"
                                                    ConfirmText='Are you sure you have pay all payable in the list? This action can not be undone.'>
                                                </ajax:ConfirmButtonExtender>
                                            </td>
                                        </tr>
                                    </FooterTemplate>
                                </asp:Repeater>
                                <%--<tr>
                        <th>
                            GRAND TOTAL</th>
                        <th>
                            <asp:Literal ID="litGrandTotal" runat="server"></asp:Literal></th>
                        <td>
                            <asp:Literal ID="litGrandPaid" runat="server"></asp:Literal></td>
                        <td>
                            <asp:Literal ID="litGrandPayable" runat="server"></asp:Literal></td>
                        <td>
                            <asp:Button ID="btnPay" runat="server" Text="Pay all" CssClass="button" />
                            <ajax:ConfirmButtonExtender ID="ConfirmButtonExtenderDelete" runat="server" TargetControlID="btnPay"
                                ConfirmText='Are you sure you have pay all payable in the list (and all pages)? This action can not be undone.'>
                            </ajax:ConfirmButtonExtender>
                        </td>
                    </tr>--%>
                            </table>
                        </div>
                        <div class="pager">
                            <svc:Pager ID="pagerServices" PagerLinkMode="HyperLinkQueryString" runat="server"
                                HideWhenOnePage="true" ControlToPage="rptExpenseServices" PageSize="50" />
                        </div>
                    </div>
                </div>
                <div id="tabs-2">
                    <div class="data_table">
                        <%--<h4>Tạm thời không hiển thị data để tối ưu hệ thống, vẫn có thể xuất sang excel bình thường</h4>--%>
                        <div class="data_grid">
                            <table class="table table-bordered table-common">
                                <tr class="active">
                                    <th>Date</th>
                                    <th>Trip code
                                    </th>
                                    <th>Partner</th>
                                    <th>Service</th>
                                    <th>Total</th>
                                    <th>Paid</th>
                                    <th>Payable</th>
                                    <th style="width: 180px;"></th>
                                    <th>Paid on</th>
                                </tr>
                                <asp:Repeater ID="rptExpenseServicesHsf" runat="server" OnItemDataBound="rptExpenseServicesHsf_ItemDataBound"
                                    OnItemCommand="rptExpenseServicesHsf_ItemCommand">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:HyperLink ID="hplDate" runat="server"></asp:HyperLink></td>
                                            <td>
                                                <asp:HyperLink ID="hplTripCode" runat="server"></asp:HyperLink>
                                            </td>
                                            <td class="--text-left">
                                                <asp:HyperLink ID="hplPartner" runat="server"></asp:HyperLink></td>
                                            <td>
                                                <asp:HyperLink ID="hplService" runat="server"></asp:HyperLink></td>
                                            <td class="--text-right">
                                                <asp:Literal ID="litTotal" runat="server"></asp:Literal></td>
                                            <td class="--text-right">
                                                <asp:Literal ID="litPaid" runat="server"></asp:Literal></td>
                                            <td class="--text-right">
                                                <asp:Literal ID="litPayable" runat="server"></asp:Literal></td>
                                            <td>
                                                <asp:TextBox ID="txtPay" runat="server" CssClass="form-control" style="display:inline-block;width:70%" data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true"></asp:TextBox>
                                                <asp:Button ID="btnPay" runat="server" Text="Pay" CssClass="btn btn-primary" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' 
                                                    style="display:inline-block"/></td>
                                            <td>
                                                <asp:Literal ID="litPaidOn" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <tr class="--text-bold">
                                            <td colspan="4">Subtotal</td>
                                            <td class="--text-right">
                                                <asp:Literal ID="litTotal" runat="server"></asp:Literal></td>
                                            <td class="--text-right">
                                                <asp:Literal ID="litPaid" runat="server"></asp:Literal></td>
                                            <td class="--text-right">
                                                <asp:Literal ID="litPayable" runat="server"></asp:Literal></td>
                                            <td>
                                                <asp:Button ID="btnPay" runat="server" Text="Pay all" CssClass="btn btn-primary" />
                                                <ajax:ConfirmButtonExtender ID="ConfirmButtonExtenderDelete" runat="server" TargetControlID="btnPay"
                                                    ConfirmText='Are you sure you have pay all payable in the list? This action can not be undone.'>
                                                </ajax:ConfirmButtonExtender>
                                            </td>
                                        </tr>
                                    </FooterTemplate>
                                </asp:Repeater>
                                <%--<tr>
                        <th>
                            GRAND TOTAL</th>
                        <th>
                            <asp:Literal ID="litGrandTotal" runat="server"></asp:Literal></th>
                        <td>
                            <asp:Literal ID="litGrandPaid" runat="server"></asp:Literal></td>
                        <td>
                            <asp:Literal ID="litGrandPayable" runat="server"></asp:Literal></td>
                        <td>
                            <asp:Button ID="btnPay" runat="server" Text="Pay all" CssClass="button" />
                            <ajax:ConfirmButtonExtender ID="ConfirmButtonExtenderDelete" runat="server" TargetControlID="btnPay"
                                ConfirmText='Are you sure you have pay all payable in the list (and all pages)? This action can not be undone.'>
                            </ajax:ConfirmButtonExtender>
                        </td>
                    </tr>--%>
                            </table>
                        </div>
                        <div class="pager">
                            <svc:Pager ID="pagerServicesHsf" PagerLinkMode="HyperLinkQueryString" runat="server"
                                HideWhenOnePage="true" ControlToPage="rptExpenseServicesHsf" PageSize="50" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <asp:Button ID="btnExportPayables" runat="server" Text="Export all payables" CssClass="btn btn-primary" OnClick="btnExportPayables_Click" />
            <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="btn btn-primary" OnClick="btnExport_Click" />
            <asp:Button ID="btnExportOneSheet" runat="server" Text="Xuất 1 sheet" CssClass="btn btn-primary" OnClick="btnExportOneSheet_Click" />
            <asp:Button ID="btnExportGuide" runat="server" Text="Xuất báo cáo guide" CssClass="btn btn-primary" OnClick="btnExportGuide_Click" />
        </div>
    </div>
    <asp:HiddenField ID="hdnSelectedTab" runat="server" Value="0" />
</asp:Content>
<asp:Content runat="server" ID="Scripts" ContentPlaceHolderID="Scripts">
    <script>
        $(function () {
            $("#tabs").tabs({ 
                activate: function() {
                    var selectedTab = $('#tabs').tabs('option', 'active');
                    $("#<%= hdnSelectedTab.ClientID %>").val(selectedTab);
                },
                active: <%= hdnSelectedTab.Value %>
                });
        });
    </script>
</asp:Content>
