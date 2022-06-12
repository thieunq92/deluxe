<%@ Page Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true"
    CodeBehind="BookingList.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.BookingList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>

<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Booking manager</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="row">
        <div class="col-xs-12">
            <table class="table table-borderless table-common">
                <tr>
                    <td class="--text-left">Booking code
                    </td>
                    <td>
                        <asp:TextBox ID="txtBookingId" runat="server" CssClass="form-control" Style="width: 30%" placeholder="Booking code"></asp:TextBox></td>
                    <td class="--text-left">Customer name
                    </td>
                    <td>
                        <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" placeholder="Customer name"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="--text-left">Trip
                    </td>
                    <td class="--text-left">
                        <asp:DropDownList ID="ddlOrgs" runat="server" CssClass="form-control" Style="display: inline-block; width: auto"></asp:DropDownList>
                        <svc:CascadingDropDown runat="server" ID="cddlTrips" CssClass="form-control" Style="display: inline-block; width: auto" />
                    </td>
                    <td class="--text-left">Start date
                    </td>
                    <td>
                        <asp:TextBox ID="textBoxStartDate" runat="server" CssClass="form-control" data-control="datetimepicker" placeholder="Start date(dd/mm/yyyy)"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="--text-left">Status
                    </td>
                    <td class="--text-left">
                        <input type="button" class="btn btn-default" value="All" id="btnAll" runat="server" />
                        <input type="button" class="btn btn-default --approved" value="Approved"
                            id="btnApproved" runat="server" />
                        <input type="button" class="btn btn-default --cancelled" value="Rejected"
                            id="btnRejected" runat="server" />
                        <input type="button" class="btn btn-default" value="Cancelled"
                            id="btnCancelled" runat="server" />
                        <input type="button" class="btn btn-default --pending" value="Pending"
                            id="btnPending" runat="server" />

                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <em>Lưu ý: Số lượng ghi trong dấu "()" là số lượng booking khởi hành trong tương lai
                                (ngày xuất phát lớn hơn ngày hiện tại), không bao gồm các điều kiện lọc khác (theo
                                tên, theo ngày khởi hành...)</em>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <asp:Button ID="buttonSearch" runat="server" OnClick="buttonSearch_Click" ValidationGroup="date"
                CssClass="btn btn-primary" />
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-xs-12">
            <table class="table table-bordered table-common">
                <asp:Repeater ID="rptBookingList" runat="server" OnItemDataBound="rptBookingList_ItemDataBound"
                    OnItemCommand="rptBookingList_ItemCommand">
                    <HeaderTemplate>
                        <tr class="active">
                            <th>Booking code
                            </th>
                            <th>Trip
                            </th>
                            <th>Number of pax
                            </th>
                            <th id="columnCustomerName" runat="server">Customer name
                            </th>
                            <th>Partner
                            </th>
                            <th>TA Code
                            </th>
                            <th>Status
                            </th>
                            <th>Confirm by
                            </th>
                            <th>Start date
                            </th>
                            <th style="width:4%">Action
                            </th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id="trItem" runat="server" class="item">
                            <td>
                                <asp:Panel CssClass="hover_content" ID="PopupMenu" runat="server">
                                    <asp:Literal ID="litNote" runat="server"></asp:Literal>
                                </asp:Panel>
                                <asp:HyperLink ID="hplCode" runat="server"></asp:HyperLink>
                            </td>
                            <td class="--text-left">
                                <asp:HyperLink ID="hyperLink_Trip" runat="server"></asp:HyperLink>
                            </td>
                            <td>
                                <asp:Literal ID="litPax" runat="server"></asp:Literal></td>
                            <td id="columnCustomerName" runat="server" class="--text-left">
                                <asp:Label ID="labelName" runat="server"></asp:Label></td>
                            <td class="--text-left">
                                <asp:HyperLink ID="hplAgency" runat="server"></asp:HyperLink>
                            </td>
                            <td>
                                <asp:Literal ID="litAgencyCode" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Label ID="label_Status" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="labelConfirmBy" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="label_startDate" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:HyperLink ID="hyperLinkView" runat="server">
                                    <i class="fa fa-edit fa-lg" aria-hidden="true" data-toggle="tooltip" data-placement="top" title="Edit"></i>
                                </asp:HyperLink>
                                <i class="fa fa-info-circle fa-lg" aria-hidden="true" data-toggle="tooltip" data-placement="top" 
                                    title="<%# ((Portal.Modules.OrientalSails.Domain.Booking)Container.DataItem).Note %>"></i>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <div class="pager">
                <svc:Pager ID="pagerBookings" runat="server" HideWhenOnePage="true" ControlToPage="rptBookingList"
                    OnPageChanged="pagerBookings_PageChanged" PageSize="20" />
            </div>
        </div>
    </div>
</asp:Content>
