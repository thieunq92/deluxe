<%@ Page Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="EventEdit.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.EventEdit" %>

<%@ Import Namespace="Portal.Modules.OrientalSails.Domain" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Assembly="Portal.Modules.OrientalSails" Namespace="Portal.Modules.OrientalSails.Web.Controls" TagPrefix="orc" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Event edit</title>
    <style>
        span.icon__edit {
            position: relative;
        }

            span.icon__edit:before {
                font-weight: 900;
                font-family: "Font Awesome 5 Free";
                font-size: 16px !important;
                line-height: normal;
                vertical-align: -.0667em;
                content: "\f044";
                position: absolute;
                z-index: -10;
                top: 1px;
                color: #337ab7;
            }

        span.icon-delete {
            position: relative;
        }

            span.icon-delete:before {
                font-weight: 900;
                font-family: "Font Awesome 5 Free";
                font-size: 16px !important;
                line-height: normal;
                vertical-align: -.0667em;
                content: "\f1f8";
                position: absolute;
                z-index: -10;
                top: 1px;
                color: #d44950;
            }

        span.icon-save {
            position: relative;
        }

            span.icon-save:before {
                font-weight: 900;
                font-family: "Font Awesome 5 Free";
                font-size: 16px !important;
                line-height: normal;
                vertical-align: -.0667em;
                content: "\f00c";
                position: absolute;
                z-index: -10;
                top: 1px;
                color: #74b816;
            }

        span.icon-cancel {
            position: relative;
        }

            span.icon-cancel:before {
                font-weight: 900;
                font-family: "Font Awesome 5 Free";
                font-size: 16px !important;
                line-height: normal;
                vertical-align: -.0667em;
                content: "\f05e";
                position: absolute;
                z-index: -10;
                top: 1px;
                color: #d44950;
            }
    </style>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <svc:Popup ID="popupManager" runat="server">
    </svc:Popup>
    <div class="row">
        <div class="col-xs-12">
            <h3 class="page-header"><%= string.Format("{0}{1:ddMMyy}-{2:00}", SailExpense.Trip.TripCode, SailExpense.Date, Group) %></h3>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <h4 class="--text-normal">Summary</h4>
            <table class="table table-bordered table-common">
                <tr class="active">
                    <th rowspan="2" style="text-align: center;">Date</th>
                    <th rowspan="2" style="text-align: center;">Trip</th>
                    <th rowspan="2" style="text-align: center;">Pax</th>
                    <th colspan="3" style="text-align: center;">Revenue</th>
                    <th colspan="3" style="text-align: center;">Expense</th>
                    <th colspan="2" style="text-align: center;">Profit</th>
                </tr>
                <tr class="active">
                    <th style="text-align: center;">Total</th>
                    <th style="text-align: center;">Paid</th>
                    <th style="text-align: center;">Receivable</th>
                    <th style="text-align: center;">Total</th>
                    <th style="text-align: center;">Paid</th>
                    <th style="text-align: center;">Payable</th>
                    <th style="text-align: center;">Real Cash</th>
                    <th style="text-align: center;">Expected</th>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="litDate" runat="server"></asp:Literal></td>
                    <td>
                        <asp:Literal ID="litTrip" runat="server"></asp:Literal></td>
                    <td>
                        <asp:Literal ID="litPax" runat="server"></asp:Literal></td>
                    <td style="text-align: right;">
                        <asp:Literal ID="litRevenueTotal" runat="server"></asp:Literal></td>
                    <td style="text-align: right;">
                        <asp:Literal ID="litRevenuePaid" runat="server"></asp:Literal></td>
                    <td style="text-align: right;">
                        <asp:Literal ID="litReceivable" runat="server"></asp:Literal></td>
                    <td style="text-align: right;">
                        <asp:Literal ID="litPayableTotal" runat="server"></asp:Literal></td>
                    <td style="text-align: right;">
                        <asp:Literal ID="litPayablePaid" runat="server"></asp:Literal></td>
                    <td style="text-align: right;">
                        <asp:Literal ID="litPayable" runat="server"></asp:Literal></td>
                    <td style="text-align: right;">
                        <asp:Literal ID="litRealCash" runat="server"></asp:Literal></td>
                    <td style="text-align: right;">
                        <asp:Literal ID="litExpected" runat="server"></asp:Literal></td>
                </tr>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <h4>Bookings</h4>
            <table class="table table-bordered table-common table__total">
                <asp:Repeater ID="rptBookingList" runat="server" OnItemDataBound="rptBookingList_ItemDataBound">
                    <HeaderTemplate>
                        <tr class="active">
                            <th rowspan="2">Booking code
                            </th>
                            <th rowspan="2">TA Code
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
                            <th rowspan="2">Guide Collect</th>
                            <th colspan="2">Total
                            </th>
                            <th colspan="2">Paid</th>
                            <th rowspan="2">Applied rate</th>
                            <th rowspan="2">Receivables</th>
                            <th rowspan="2">Action</th>
                            <th rowspan="2">Paid on
                            </th>
                        </tr>
                        <tr class="active">
                            <th>Adult</th>
                            <th>Child</th>
                            <th>Infant</th>
                            <th>USD</th>
                            <th>VND</th>
                            <th>USD</th>
                            <th>VND</th>
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
                                <asp:HyperLink ID="hplPartner" runat="server"></asp:HyperLink>
                            </td>
                            <%--                                <td>
                                    <asp:HyperLink ID="hplCruise" runat="server"></asp:HyperLink>
                                </td>--%>
                            <td>
                                <asp:Literal ID="litService" runat="server"></asp:Literal></td>
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
                                <asp:Literal ID="litGuideCollect" runat="server"></asp:Literal>
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="label_TotalPrice" runat="server">
                                    <asp:TextBox ID="txtTotal" runat="server" Style="display: none;"></asp:TextBox></asp:Label>
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="label_TotalPriceVND" runat="server">
                                </asp:Label>
                            </td>
                            <td style="text-align: right;">
                                <asp:Literal ID="litPaid" runat="server"></asp:Literal></td>
                            <td style="text-align: right;">
                                <asp:Literal ID="litPaidBase" runat="server"></asp:Literal></td>
                            <td style="text-align: right;">
                                <asp:Literal ID="litCurrentRate" runat="server"></asp:Literal></td>
                            <td style="text-align: right;">
                                <asp:Literal ID="litReceivable" runat="server"></asp:Literal></td>
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
                            <td colspan="6" class="--text-bold">GRAND TOTAL
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
                            <td></td>
                            <td style="text-align: right;">
                                <strong>
                                    <asp:Label ID="label_TotalPrice" runat="server"></asp:Label></strong>
                            </td>
                            <td style="text-align: right;">
                                <strong>
                                    <asp:Literal ID="litPaid" runat="server"></asp:Literal></strong></td>
                            <td style="text-align: right;"></td>
                            <td><strong>
                                <asp:Literal ID="litPaidBase" runat="server"></asp:Literal></strong></td>
                            <td style="text-align: right;"></td>
                            <td style="text-align: right;">
                                <strong>
                                    <asp:Literal ID="litReceivable" runat="server"></asp:Literal></strong>

                            </td>
                            <td><a id="aPayment" runat="server" style="cursor: pointer;">Pay all</a></td>
                            <td></td>
                        </tr>
                    </FooterTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <asp:UpdatePanel ID="upExpenseService" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <h4>Expenses</h4>
                    <table class="table table-bordered table-common table__total">
                        <tr class="active">
                            <th>Date</th>
                            <th>Trip code
                            </th>
                            <th style="width:180px">Partner</th>
                            <th>Service</th>
                            <th style="width:137px">Total</th>
                            <th>Paid</th>
                            <th>Payable</th>
                            <th style="width: 173px"></th>
                            <th>Paid on</th>
                            <th></th>
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
                                    <td class="--text-left">
                                        <asp:HyperLink ID="hplService" runat="server"></asp:HyperLink></td>


                                    <td class="--text-right">
                                        <asp:Panel runat="server" ID="pnlTotalView">
                                            <asp:Literal ID="litTotal" runat="server" Text='<%# ((ExpenseService)(Container.DataItem)).Cost.ToString("#,0.##") %>'></asp:Literal>
                                            <span class="icon icon__edit" title="Edit">
                                                <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-primary" Text="Edit" CommandName="Edit"></asp:Button>
                                            </span>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="pnlTotalEdit" Visible="False">
                                            <asp:TextBox ID="txtTotal" runat="server" Text='<%# ((Literal)Container.FindControl("litTotal")).Text %>' CssClass="form-control" style="width: 89px;display:inline-block"></asp:TextBox>
                                            <span class="icon icon-save">
                                                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" CommandName="Save" CommandArgument="<%# ((ExpenseService)Container.DataItem).Id %>"></asp:Button>
                                            </span>
                                            <span class="icon icon-cancel">
                                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary" Text="Cancel" CommandName="Cancel"></asp:Button>
                                            </span>
                                        </asp:Panel>
                                    </td>
                                    <td class="--text-right">
                                        <asp:Literal ID="litPaid" runat="server"></asp:Literal></td>
                                    <td class="--text-right">
                                        <asp:Literal ID="litPayable" runat="server"></asp:Literal></td>
                                    <td>
                                        <asp:TextBox ID="txtPay" runat="server" CssClass="form-control" Style="display: inline-block; width: 110px"></asp:TextBox>
                                        <asp:Button ID="btnPay" runat="server" Text="Pay" CssClass="btn btn-primary" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' CommandName="Pay" /></td>
                                    <td>
                                        <asp:Literal ID="litPaidOn" runat="server"></asp:Literal>
                                    </td>
                                    <td class="control">
                                        <span class="icon icon__edit" title="Edit">
                                            <orc:AgencySelector ID="agencySelector" runat="server" CssClass="form-control" NodeId="1" SectionId="15" />
                                        </span>
                                        <asp:Button runat="server" CommandArgument="<%#((ExpenseService)(Container.DataItem)).Id%>" CommandName="EditPartner" Text="Edit" data-control="send" Style="display: none"></asp:Button>
                                        <span class="icon icon-delete" title="Delete">
                                            <asp:Button runat="server" ToolTip="Delete" CommandArgument="<%#((ExpenseService)(Container.DataItem)).Id%>" CommandName="Delete"
                                                Text="Delete" OnClientClick="return confirm('Bạn chắc chắn chứ ?')"></asp:Button>
                                        </span>
                                        <a href="" title="History" data-toggle="modal" data-target=".modal-expenseHistory"
                                            data-url="ExpenseHistory.aspx?NodeId=1&SectionId=15&ei=<%# ((ExpenseService)(Container.DataItem)).Id %>">
                                            <i class="fa fa-lg fa-clock" style="color: #f59f00"></i>
                                        </a>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                <tr>
                                    <td colspan="4" class="--text-bold">SUBTOTAL</td>
                                    <td class="--text-bold --text-right">
                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal></td>
                                    <td class="--text-bold --text-right">
                                        <asp:Literal ID="litPaid" runat="server"></asp:Literal></td>
                                    <td class="--text-bold --text-right">
                                        <asp:Literal ID="litPayable" runat="server"></asp:Literal></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </FooterTemplate>
                        </asp:Repeater>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <fieldset>
    </fieldset>
    <div class="modal fade modal-expenseHistory" tabindex="-1" role="dialog" aria-labelledby="gridSystemModalLabel">
        <div class="modal-dialog" role="document" style="width: 1230px">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title">Expense history</h3>
                </div>
                <div class="modal-body">
                    <iframe frameborder="0" width="1200" scrolling="no" onload="resizeIframe(this)" src=""></iframe>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content runat="server" ID="server" ContentPlaceHolderID="Scripts">
    <script>
        $(document).ready(function () {
            $('a[data-target = ".modal-expenseHistory"]').click(function () {
                $(".modal-expenseHistory iframe").attr('src', $(this).attr('data-url'));
            });
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                $('a[data-target = ".modal-expenseHistory"]').click(function () {
                    $(".modal-expenseHistory iframe").attr('src', $(this).attr('data-url'));
                });
            });
        });
    </script>
    <script>
        $(document).ready(function () {
            $('td.control').find("[type='text']").hide();
            $('.icon').find("[type='button'],[type='submit']").css({
                'background-color': 'transparent',
                'color': 'transparent',
                'border': 'none',
                'width': '16px',
                'padding': 0,
                'margin': 0,
                'box-shadow': 'none',
                'outline': 'none'
            });
        });
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(
            function () {
                $('td.control').find("[type='text']").hide();
                $('.icon').find("[type='button'],[type='submit']").css({
                    'background-color': 'transparent',
                    'color': 'transparent',
                    'border': 'none',
                    'width': '16px',
                    'padding': 0,
                    'margin': 0
                });
            });
    </script>
</asp:Content>
