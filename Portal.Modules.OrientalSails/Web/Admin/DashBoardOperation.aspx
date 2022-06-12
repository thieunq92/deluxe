<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="Main.Master" CodeBehind="DashBoardOperation.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.DashBoardOperation" %>
<%@ Import Namespace="Portal.Modules.OrientalSails.Web.Admin" %>

<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Dash Board Operation</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="row">
        <div class="col-xs-12">
            <h2 class="--text-bold">Xin chào <%= CurrentUser.FullName %>, chúc bạn một ngày làm việc đầy năng lượng</h2>
        </div>
    </div>
    <br />
    <asp:UpdatePanel ID="udpDashBoard" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div class="col-xs-4">
                    <h3>Today running</h3>
                </div>
                <div class="col-xs-2">
                    <h3>
                        <asp:TextBox runat="server" ID="txtDateSearching" CssClass="form-control --float-right" AutoPostBack="true"
                            data-control="datetimepicker"
                            placeholder="Date(dd/MM/yyyy)" />
                    </h3>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <table class="table table-bordered table-common table__total ">
                        <tr class="active">
                            <th rowspan="2">No</th>
                            <th rowspan="2" style="width: 10%">Trip code</th>
                            <th rowspan="2" style="width: 3%">No of pax</th>
                            <th rowspan="2" style="width: 3%">No of bk</th>
                            <th rowspan="2" style="width: 10%">Guide</th>
                            <th rowspan="2" style="width: 10%">Transport</th>
                            <th rowspan="2">Special request</th>
                            <th colspan="5">Expenses</th>
                            <th rowspan="2" style="width: 17%">Note</th>
                        </tr>
                        <tr class="active">
                            <th>Guide</th>
                            <th>Transport</th>
                            <th>Boat</th>
                            <th>Other</th>
                            <th>Total</th>
                        </tr>
                        <asp:Repeater ID="rptTodayRunningGroups" runat="server" OnItemDataBound="rptTodayRunningGroups_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Container.ItemIndex + 1 %></td>
                                    <td class="--text-right">
                                        <asp:Literal runat="server" ID="ltrTripCode"></asp:Literal></td>
                                    <td>
                                        <asp:Literal runat="server" ID="ltrNoOfPax"></asp:Literal></td>
                                    <td>
                                        <asp:Literal runat="server" ID="ltrNoOfBooking"></asp:Literal></td>
                                    <td class="--text-left">
                                        <asp:Literal runat="server" ID="ltrGuide"></asp:Literal></td>
                                    <td class="--text-left">
                                        <asp:Literal runat="server" ID="ltrTransport"></asp:Literal></td>
                                    <td class="--text-left">
                                        <asp:Literal runat="server" ID="ltrSpecialRequest"></asp:Literal></td>
                                    <td class="--text-right">
                                        <asp:Literal runat="server" ID="ltrGuideExpense"></asp:Literal></td>
                                    <td class="--text-right">
                                        <asp:Literal runat="server" ID="ltrTransportExpense"></asp:Literal></td>
                                    <td class="--text-right">
                                        <asp:Literal runat="server" ID="ltrBoatExpense"></asp:Literal></td>
                                    <td class="--text-right">
                                        <asp:Literal runat="server" ID="ltrOtherExpense"></asp:Literal></td>
                                    <td class="--text-right">
                                        <asp:Literal runat="server" ID="ltrTotalExpense"></asp:Literal></td>
                                    <td class="--text-left">
                                        <asp:Repeater runat="server" ID="rptNotes" OnItemDataBound="rptNotes_ItemDataBound">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="ltrNote"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <a href="javascript:void(0)" data-toggle="modal" data-target="#addNoteModal"
                                            onclick="$('#addNoteModal .modal-title').html('Add note trip code <%# GetTripCode((EventCode)Container.DataItem) %>')
                                        ;clearFormAddNote()
                                        ;$('#<%=hidTripCode.ClientID %>').val('<%# GetTripCode((EventCode)Container.DataItem) %>')">Add note</a></td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                <%                                 
                                    if (
                                        ((System.Collections.Generic.List<Portal.Modules.OrientalSails.Web.Admin.EventCode>)
                                        rptTodayRunningGroups.DataSource).Count <= 0)
                                    {                                      
                                %>
                                <tr>
                                    <td colspan="100%">No records found</td>
                                </tr>
                                <%
                                    }
                                %>
                                <tr class="active">
                                    <td colspan="2" class="--text-bold">Total</td>
                                    <td class="--text-bold">
                                        <asp:Literal runat="server" ID="ltrTotalPax"></asp:Literal></td>
                                    <td class="--text-bold">
                                        <asp:Literal runat="server" ID="ltrTotalBooking"></asp:Literal></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td class="--text-bold --text-right">
                                        <asp:Literal runat="server" ID="ltrTotalGuideExpense"></asp:Literal></td>
                                    <td class="--text-bold --text-right">
                                        <asp:Literal runat="server" ID="ltrTotalTransportExpense"></asp:Literal></td>
                                    <td class="--text-bold --text-right">
                                        <asp:Literal runat="server" ID="ltrTotalBoatExpense"></asp:Literal></td>
                                    <td class="--text-bold --text-right">
                                        <asp:Literal runat="server" ID="ltrTotalOtherExpense"></asp:Literal></td>
                                    <td class="--text-bold --text-right">
                                        <asp:Literal runat="server" ID="ltrTotalOfTotalExpense"></asp:Literal></td>
                                    <td colspan="100%"></td>
                                </tr>
                            </FooterTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnAddNoteSave" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="modal fade note-modal" id="addNoteModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog note-modal__modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title --text-bold" id="myModalLabel">Add note</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="row">
                            <asp:HiddenField runat="server" ID="hidTripCode" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2 --no-padding-leftright">
                                To
                            </div>
                            <div class="col-xs-2 --no-padding-leftright">
                                <asp:DropDownList runat="server" ID="ddlForRole" AppendDataBoundItems="true" CssClass="form-control">
                                    <asp:ListItem Text="-- Role --" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2 --no-padding-leftright">
                                Note
                            </div>
                            <div class="col-xs-10 --no-padding-leftright">
                                <asp:TextBox runat="server" ID="txtNote" CssClass="form-control" TextMode="MultiLine" Rows="12" placeholder="Note" Text="" data-id="txtAgencyNotesNote" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    <asp:Button runat="server" ID="btnAddNoteSave" OnClick="btnAddNoteSave_Click" Text="Save" CssClass="btn btn-primary" OnClientClick="$('#addNoteModal').modal('toggle')" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Scripts" runat="server" ContentPlaceHolderID="Scripts">
    <script>
        function clearFormAddNote() {
            clearForm($('#addNoteModal .modal-content'));
        }
    </script>
</asp:Content>
