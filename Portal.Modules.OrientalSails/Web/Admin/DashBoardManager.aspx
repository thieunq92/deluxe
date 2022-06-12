<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="Main.Master" CodeBehind="DashBoardManager.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.DashBoardManager" %>

<%@ Import Namespace="Portal.Modules.OrientalSails.DataTransferObject.DashBoardManager" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Portal.Modules.OrientalSails.Domain" %>
<%@ Import Namespace="Portal.Modules.OrientalSails.Web.Util" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Dash Board Mananger</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="row">
        <div class="col-xs-12">
            <h2 class="--text-bold">Xin chào <%= CurrentUser.FullName %>, chúc bạn một ngày làm việc đầy năng lượng</h2>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <a href="GoldenDayCreateCampaign.aspx?NodeId=1&SectionId=15" class="btn btn-primary">Create golden days</a>
            <a href="GoldenDayListCampaign.aspx?NodeId=1&SectionId=15" class="btn btn-primary">View all campaigns</a>
            <a href="DashBoardOperation.aspx?NodeId=1&SectionId=15" class="btn btn-primary">Dashboard Operation</a>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-10">
            <div class="col-xs-12 --no-padding-left --no-padding-right">
                <div class="row">
                    <div class="col-xs-12 --no-padding-left">
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-xs-4">
                                        <h4 class="--text-bold" style="margin-bottom: 0">Month summary 
                        <asp:DropDownList runat="server" ID="ddlMonthSearching" AutoPostBack="true" CssClass="form-control --dropdown-inline">
                        </asp:DropDownList>
                                            <asp:DropDownList runat="server" ID="ddlYearSearching" AutoPostBack="true" CssClass="form-control --dropdown-inline">
                                            </asp:DropDownList></h4>
                                    </div>
                                    <div class="col-xs-2">
                                        <asp:DropDownList runat="server" ID="ddlRegion" AutoPostBack="true" CssClass="form-control" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="table-responsive" style="width: 86%; display: inline-block; float: left">
                                    <table class="table table-bordered table-common table__total ">
                                        <tbody>
                                            <tr>
                                                <td></td>
                                                <asp:Repeater ID="rptSales" runat="server">
                                                    <ItemTemplate>
                                                        <td class="active --text-bold"><a href="SalesPerformanceAnalysis.aspx?NodeId=1&SectionId=15&SalesId=<%#((CMS.Core.Domain.User)Container.DataItem).Id%>"><%# ((CMS.Core.Domain.User)Container.DataItem).UserName %></td>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tr>
                                            <tr>
                                                <td class="active --text-bold">NoP</td>
                                                <asp:Repeater ID="rptSalesNoOfPax" runat="server" OnItemDataBound="rptSalesNoOfPax_ItemDataBound">
                                                    <ItemTemplate>
                                                        <td class="--text-right">
                                                            <asp:Literal runat="server" ID="ltrNumberOfPax"></asp:Literal></td>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tr>
                                            <tr>
                                                <td class="active --text-bold">NoB</td>
                                                <asp:Repeater ID="rptSalesNoOfBookings" runat="server" OnItemDataBound="rptSalesNoOfBookings_ItemDataBound">
                                                    <ItemTemplate>
                                                        <td class="--text-right">
                                                            <asp:Literal runat="server" ID="ltrNumberOfBooking"></asp:Literal></td>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tr>
                                            <tr>
                                                <td class="active --text-bold">Revenue</td>
                                                <asp:Repeater ID="rptSalesRevenueInUSD" runat="server" OnItemDataBound="rptSalesRevenueInUSD_ItemDataBound">
                                                    <ItemTemplate>
                                                        <td class="--text-right">
                                                            <asp:Literal runat="server" ID="ltrRevenue"></asp:Literal></td>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tr>
                                            <tr>
                                                <td class="active --text-bold">Reports</td>
                                                <asp:Repeater ID="rptSalesMeetingReports" runat="server" OnItemDataBound="rptSalesMeetingReports_ItemDataBound">
                                                    <ItemTemplate>
                                                        <td class="--text-right">
                                                            <asp:Literal runat="server" ID="ltrNumberOfReport"></asp:Literal>
                                                        </td>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div style="display: inline-block; float: left; width: 14%">
                                    <table class="table table-bordered table-common table__total ">
                                        <tbody>
                                            <tr>
                                                <td class="active --text-bold">Total</td>
                                                <td class="active --text-bold"><%= YearSearching - 1 %></td>
                                            </tr>
                                            <tr>
                                                <td class="--text-right">
                                                    <asp:Literal runat="server" ID="ltrTotalPax"></asp:Literal></td>
                                                <td class="--text-right">
                                                    <asp:Literal runat="server" ID="ltrPaxLastOneYear"></asp:Literal></td>
                                            </tr>
                                            <tr>
                                                <td class="--text-right">
                                                    <asp:Literal runat="server" ID="ltrTotalBooking"></asp:Literal></td>
                                                <td class="--text-right">
                                                    <asp:Literal runat="server" ID="ltrBookingLastOneYear"></asp:Literal></td>
                                            </tr>
                                            <tr>
                                                <td class="--text-right">
                                                    <asp:Literal runat="server" ID="ltrTotalRevenue"></asp:Literal></td>
                                                <td class="--text-right">
                                                    <asp:Literal runat="server" ID="ltrRevenueLastOneYear"></asp:Literal></td>
                                            </tr>
                                            <tr>
                                                <td class="--text-right">
                                                    <asp:Literal runat="server" ID="ltrTotalReport"></asp:Literal></td>
                                                <td class="--text-right">
                                                    <asp:Literal runat="server" ID="ltrReportLastOneYear"></asp:Literal></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="col-xs-6 --no-padding-left">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <h4 class="--text-bold" style="margin-bottom: 0">New bookings received Today
                                  <asp:DropDownList runat="server" ID="ddlSales" AutoPostBack="true" CssClass="form-control --dropdown-inline" Style="float: right" AppendDataBoundItems="true">
                                      <asp:ListItem Value="0">-- All sales --</asp:ListItem>
                                  </asp:DropDownList>
                        </h4>
                        <table class="table table-bordered table-common table__total ">
                            <tbody>
                                <tr class="active">
                                    <th>Code</th>
                                    <th>Trip</th>
                                    <th>No of pax</th>
                                    <th>Revenue</th>
                                    <th>Start date</th>
                                    <th>Agency</th>
                                    <th>View</th>
                                </tr>
                                <asp:Repeater runat="server" ID="rptNewBookings">
                                    <ItemTemplate>
                                        <tr class="<%# ((Booking)Container.DataItem).Status == StatusType.Approved ? "--approved":""  %>
                                               <%# ((Booking)Container.DataItem).Status == StatusType.Cancelled ? "--cancelled":""  %>
                                               <%# ((Booking)Container.DataItem).Status == StatusType.Pending ? "--pending":""  %>
                                        ">
                                            <td><a href="BookingView.aspx?NodeId=1&SectionId=15&bookingid=<%# ((Booking)Container.DataItem).Id %>"><%# ((Booking)Container.DataItem).BookingCode %></td>
                                            <td><%# ((Booking)Container.DataItem).Trip != null ? ((Booking)Container.DataItem).Trip.TripCode : "" %></td>
                                            <td><%# ((Booking)Container.DataItem).Pax %></td>
                                            <td class="--text-right"><%# GetTotalAsString((Booking)Container.DataItem)%></td>
                                            <td><%# ((Booking)Container.DataItem).StartDate.ToString("dd/MM/yyyy") %></td>
                                            <td class="--text-left"><a href="AgencyView.aspx?NodeId=1&SectionId=15&agencyid=<%# ((Booking)Container.DataItem).Agency != null ? ((Booking)Container.DataItem).Agency.Id : 0%>">
                                                <%# ((Booking)Container.DataItem).Agency != null ? ((Booking)Container.DataItem).Agency.Name : ""%></a></td>
                                            <td><a href="javascript:void(0)" data-toggle="tooltip" title="<%# ((Booking)Container.DataItem).SpecialRequest%>">View</a></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <%                                 
                                    if (((System.Collections.Generic.List<Booking>)rptNewBookings.DataSource).Count <= 0)
                                    {                                      
                                %>
                                <tr>
                                    <td colspan="100%">No records found</td>
                                </tr>
                                <%
                                    }
                                %>
                            </tbody>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="col-xs-6 --no-padding-left --no-padding-right">
                <div class="row">
                    <div class="col-xs-12 --no-padding-leftright">
                        <h4 class="--text-bold" style="margin-bottom: 5px">Today running groups</h4>
                        <table class="table table-bordered table-common ">
                            <tbody>
                                <tr class="active">
                                    <th>No</th>
                                    <th style="width: 20%">Trip code</th>
                                    <th>No of pax</th>
                                    <th>Guide</th>
                                    <th>Transport</th>
                                </tr>
                                <asp:Repeater ID="rptTodayRunningGroups" runat="server" OnItemDataBound="rptTodayRunningGroups_ItemDataBound">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Container.ItemIndex + 1 %></td>
                                            <td class="--text-right">
                                                <asp:Literal runat="server" ID="ltrTripCode"></asp:Literal></td>
                                            <td>
                                                <asp:Literal runat="server" ID="ltrNoOfPax"></asp:Literal></td>
                                            <td class="--text-left">
                                                <asp:Literal runat="server" ID="ltrGuide"></asp:Literal></td>
                                            <td class="--text-left">
                                                <asp:Literal runat="server" ID="ltrTransport"></asp:Literal></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <tr class="active">
                                            <td colspan="2" class="--text-bold">Total</td>
                                            <td class="--text-bold">
                                                <asp:Literal runat="server" ID="ltrTotalPax"></asp:Literal></td>
                                            <td colspan="100%"></td>
                                        </tr>
                                    </FooterTemplate>
                                </asp:Repeater>
                                <tr class="active">
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="col-xs-12 --no-padding-leftright">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <h4 class="--text-bold" style="margin-bottom: 0">Availability
                                <span style="padding-left: 28%">View</span>
                            <asp:DropDownList runat="server" ID="ddlView" AutoPostBack="true" CssClass="form-control --dropdown-inline">
                                <asp:ListItem Value="7">7 days</asp:ListItem>
                                <asp:ListItem Value="15">15 days</asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox runat="server" ID="txtAvaibilityDateSearching" CssClass="form-control --float-right" Style="width: 30%; margin-top: -7px" AutoPostBack="true"
                                data-control="datetimepicker"
                                placeholder="Date(dd/MM/yyyy)" />
                        </h4>
                        <div class="table-responsive">
                            <table class="table table-bordered table-common table__total ">
                                <tbody>
                                    <tr class="active">
                                        <th>Date</th>
                                        <asp:Repeater runat="server" ID="rptTrips">
                                            <ItemTemplate>
                                                <th>
                                                    <%# ((SailsTrip)Container.DataItem).TripCode %>
                                                </th>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tr>
                                    <asp:Repeater runat="server" ID="rptDates" OnItemDataBound="rptDates_ItemDataBound">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <%# ((DateTime)Container.DataItem).ToString("dd/MM/yyyy") %>
                                                </td>
                                                <asp:Repeater ID="rptDateTrips" runat="server" OnItemDataBound="rptDateTrips_ItemDataBound">
                                                    <ItemTemplate>
                                                        <td>
                                                            <asp:Literal runat="server" ID="ltrNumberOfPax" /></td>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="col-xs-2 --no-padding-left" style="padding-right: 13px">
            <div class="row">
                <div class="col-xs-12 col__golden-day col__golden-day-dashboard-manager --no-padding-leftright">
                    <h4 class="golden-day__header --text-bold --no-margin-bottom">Golden days
                       
                    </h4>
                    <asp:DropDownList runat="server" ID="ddlCampaign" AppendDataBoundItems="true" class="form-control">
                    </asp:DropDownList>
                    <input type="hidden" id="golden-day" />
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col__golden-day col__golden-day-dashboard-manager --no-padding-leftright">
                    <input type="hidden" id="golden-day-nextmonth" />
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 --no-padding-leftright">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <h4 class="--text-bold">Top 10 partners in - <%= DateTime.Now.ToString("MMMM").Substring(0,3)%> </h4>
                            <asp:DropDownList runat="server" ID="ddlMonthTopPartner" AutoPostBack="true" CssClass="form-control --text-bold --dropdown-inline">
                            </asp:DropDownList>
                            <asp:DropDownList runat="server" ID="ddlYearTopPartner" AutoPostBack="true" CssClass="form-control --text-bold --dropdown-inline">
                            </asp:DropDownList>
                            <table class="table table-borderless table__top10partner">
                                <asp:Repeater runat="server" ID="rptTop10Partner">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Container.ItemIndex + 1 %></td>
                                            <td><a href="AgencyView.aspx?NodeId=1&SectionId=15&agencyid=<%# Eval("AgencyId") %>"><%# Eval("AgencyName")%></td>
                                            <td><%# Eval("NumberOfPax")%></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <%     
                                    if (((IList)rptTop10Partner.DataSource).Count <= 0)
                                    {
                                %>
                                <tr>
                                    <td colspan="100%">No records found</td>
                                </tr>
                                <%
                                    }
                                %>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Scripts" ContentPlaceHolderID="Scripts" runat="server">
    <script>
        var goldenDayControl = null;
        var goldenDayNextMonthControl = null;
        $(function () {
            $('#golden-day').datetimepicker({
                timepicker: false,
                format: 'd/m/Y',
                inline: true,
                value: '<%=DateTime.Today.ToString("dd/MM/yyyy")%>',
                onGenerate: function (ct, $i) {
                    goldenDayControl = this;
                    var isGenerated = false;
                    hightlightedDates(ct, $i, this, isGenerated);
                    isGenerated = true;
                },
                onChangeMonth: function (ct, $i) {
                    var gddtCurrent = ct;
                    var gddtNextMonth = new Date(gddtCurrent.getFullYear(), gddtCurrent.getMonth() + 1, gddtCurrent.getDate());
                    $('#golden-day-nextmonth').datetimepicker('setOptions', { value: gddtNextMonth.format("dd/MM/yyyy") });
                    var isGenerated = false;
                    hightlightedDates(ct, $i, this, isGenerated);
                    hightlightedDates(gddtNextMonth, $i, goldenDayNextMonthControl, isGenerated);
                    $('.tooltip').remove();
                }
            });
            $('#golden-day-nextmonth').datetimepicker({
                timepicker: false,
                format: 'd/m/Y',
                inline: true,
                value: '<%=DateTime.Today.AddMonths(1).ToString("dd/MM/yyyy")%>',
                onGenerate: function (ct, $i) {
                    goldenDayNextMonthControl = this;
                    var isGenerated = false;
                    hightlightedDates(ct, $i, this, isGenerated);
                    isGenerated = true;
                },
                onChangeMonth: function (ct, $i) {
                    var gddtCurrent = ct;
                    var gddtPreviousMonth = new Date(gddtCurrent.getFullYear(), gddtCurrent.getMonth() - 1, gddtCurrent.getDate());
                    $('#golden-day').datetimepicker('setOptions', { value: gddtPreviousMonth.format("dd/MM/yyyy") });
                    var isGenerated = false;
                    hightlightedDates(ct, $i, this, isGenerated);
                    hightlightedDates(gddtPreviousMonth, $i, goldenDayControl, isGenerated);
                    $('.tooltip').remove();

                }
            });
        });
        function hightlightedDates(ct, $i, control, isGenerated) {

            if (isGenerated) {
                return;
            }
            $(".col__golden-day .xdsoft_date").removeClass('xdsoft_current').removeClass('xdsoft_today');
            var dd = ct.getDate();
            var MM = ct.getMonth() + 1;
            var yyyy = ct.getFullYear();
            var date = MM + '/' + dd + '/' + yyyy;
            $.ajax({
                type: 'POST',
                url: 'WebService/DashBoardWebService.asmx/GoldenDayGetAllInMonthByDate',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: '{"date":"' + date + '"}',
            }).done(function (data) {
                var goldenDays = JSON.parse(data.d);
                var highlightedDates = new Array();
                for (var i = 0; i < goldenDays.length; i++) {
                    var date = new Date(goldenDays[i].Date);
                    var dateAsString = date.getFullYear() + '/' + (date.getMonth() + 1) + '/' + date.getDate();
                    var policy = goldenDays[i].Policy === null ? '' : goldenDays[i].Policy;
                    var highlightedDate = dateAsString + ',' + policy.replace(new RegExp(',', 'g'), '\u201A') + ',hightlight__golden-day';
                    highlightedDates.push(highlightedDate);
                }
                control.setOptions({
                    highlightedDates: highlightedDates,
                    value: ct,
                    onGenerate: function (ct) {
                        $('.col__golden-day .xdsoft_date').removeClass('xdsoft_current').removeClass('xdsoft_today');
                        goldenDayGenerateTooltip();
                        $('.xdsoft_date').click(function () {
                            $('.tooltip').remove();
                        });
                        $('.xdsoft_date').mouseleave(function () {
                            $('.tooltip').remove();
                        });
                    }
                });
            })
        }
    </script>
    <script>
        function goldenDayGenerateTooltip() {
            $('.col__golden-day .hightlight__golden-day').tooltip({
                container: 'body',
                placement: 'left',
                html: true,
            });
        }
    </script>
    <script>
        $(function () {
            $('#<%= ddlCampaign.ClientID%>').click(function () {
                var selectedDate = $('#<%= ddlCampaign.ClientID%> :selected').val();
                var dateParts = selectedDate.split("/");
                var gddtCurrent = new Date(+dateParts[2], dateParts[1] - 1, +dateParts[0]);
                var gddtNextMonth = new Date(gddtCurrent.getFullYear(), gddtCurrent.getMonth() + 1, gddtCurrent.getDate())
                $('#golden-day').datetimepicker('setOptions', { value: gddtCurrent.format("dd/MM/yyyy") });
                hightlightedDates(gddtCurrent, null, goldenDayControl, false);
                hightlightedDates(gddtNextMonth, null, goldenDayNextMonthControl, false);
            })
        });
    </script>
</asp:Content>
