<%@ Page Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true"
    CodeBehind="BookingReportPeriod.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.BookingReportPeriod" %>

<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Booking by period</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="row">
        <div class="col-xs-1 --no-padding-right" style="width: 3%">
            From
        </div>
        <div class="col-xs-2">
            <asp:TextBox ID="txtFrom" runat="server" data-control="datetimepicker" CssClass="form-control" placeholder="From(dd/mm/yyyy)"></asp:TextBox>
        </div>
        <div class="col-xs-1 --no-padding-right" style="width: 2%">
            To
        </div>
        <div class="col-xs-2 --no-padding-right">
            <asp:TextBox ID="txtTo" runat="server" data-control="datetimepicker" CssClass="form-control" placeholder="To(dd/mm/yyyy)"></asp:TextBox>
        </div>
        <div class="col-xs-1">
            <asp:Button ID="btnDisplay" runat="server" Text="Display" OnClick="btnDisplay_Click"
                CssClass="btn btn-primary" />
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-xs-12">
            <div class="table-responsive">
                <table class="table table-bordered table-common">
                    <asp:Repeater ID="rptBookingList" runat="server" OnItemDataBound="rptBookingList_ItemDataBound"
                        OnItemCommand="rptBookingList_ItemCommand">
                        <HeaderTemplate>
                            <tr class="active">
                                <th rowspan="2" style="width: 80px;">
                                    <%= base.GetText("textDate") %>
                                </th>
                                <th colspan="4" style="width: 100px;">
                                    <%= base.GetText("textNumberOfPax") %>
                                </th>
                                <asp:Repeater ID="rptTrips" runat="server">
                                    <ItemTemplate>
                                        <th rowspan="2">
                                            <%# DataBinder.Eval(Container.DataItem, "TripCode") %>
                                        </th>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <asp:Repeater ID="rptRooms" runat="server" OnItemDataBound="rptRooms_ItemDataBound">
                                    <ItemTemplate>
                                        <th colspan="2">
                                            <asp:Literal ID="litRoomName" runat="server"></asp:Literal>
                                        </th>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <th rowspan="2">Tổng
                                </th>
                                <th rowspan="2"></th>
                            </tr>
                            <tr class="active">
                                <th>
                                    <%= base.GetText("textAdult") %>
                                </th>
                                <th>
                                    <%= base.GetText("textChild") %>
                                </th>
                                <th>
                                    <%= base.GetText("textBaby") %>
                                </th>
                                <th>
                                    <%= base.GetText("textTotalPax") %>
                                </th>
                                <asp:Repeater ID="rptRoomAvail" runat="server">
                                    <ItemTemplate>
                                        <th>Total</th>
                                        <th>Avail</th>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litTr" runat="server"></asp:Literal>
                            <td>
                                <asp:Panel CssClass="hover_content" ID="PopupMenu" runat="server">
                                    <asp:Literal ID="litNote" runat="server"></asp:Literal>
                                </asp:Panel>
                                <asp:HyperLink ID="hplDate" runat="server"></asp:HyperLink>
                            </td>
                            <td>
                                <asp:Literal ID="litAdult" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litChild" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litBaby" runat="server"></asp:Literal></td>
                            <td>
                                <asp:Literal ID="litTotalPax" runat="server"></asp:Literal>
                            </td>
                            <asp:Repeater ID="rptTrips" runat="server" OnItemDataBound="rptTrips_ItemDataBound">
                                <ItemTemplate>
                                    <td>
                                        <asp:Literal ID="litPax" runat="server"></asp:Literal>
                                    </td>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:Repeater ID="rptRoomAvail" runat="server" OnItemDataBound="rptRooms_ItemDataBound">
                                <ItemTemplate>
                                    <td>
                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal></td>
                                    <td id="tdAvail" runat="server">
                                        <asp:Literal ID="litAvail" runat="server"></asp:Literal></td>
                                </ItemTemplate>
                            </asp:Repeater>
                            <td>
                                <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:LinkButton ID="lbtLock" runat="server" CommandName='lock'></asp:LinkButton>
                                <asp:Image ImageUrl="../Images/info.png" ID="imgNote" runat="server" />
                                <ajax:HoverMenuExtender ID="hmeNote" runat="Server" HoverCssClass="popupHover" PopupControlID="PopupMenu"
                                    PopupPosition="Left" TargetControlID="imgNote" PopDelay="25" />
                            </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
            <br/>
            <asp:Button ID="btnExcel" runat="server" Text="Export" OnClick="btnExcel_Click" CssClass="btn btn-primary" />
        </div>
    </div>
</asp:Content>
