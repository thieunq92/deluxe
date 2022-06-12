<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="Main.Master" CodeBehind="DashBoard.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.DashBoard" %>

<%@ Import Namespace="Portal.Modules.OrientalSails.Domain" %>
<%@ Import Namespace="Portal.Modules.OrientalSails.Web.Admin.Utility" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Collections.Generic" %>

<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Dash board</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="row">
        <div class="col-xs-12">
            <h2 class="--text-bold">Xin chào <%= CurrentUser.FullName %>, chúc bạn một ngày làm việc đầy năng lượng</h2>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-6">
            <h4 class="--text-bold">Your today bookings</h4>
            <table class="table table-bordered table-common table__total">
                <tbody>
                    <tr class="header active">
                        <th style="width: 13%">Code</th>
                        <th style="width: 9%">Trip</th>
                        <th style="width: 8%">NoP</th>
                        <th style="width: 17%">Revenue</th>
                        <th>Agency</th>
                        <th style="width: 8%">View</th>
                    </tr>
                    <asp:Repeater runat="server" ID="rptTodayBookings">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <a href="BookingView.aspx?NodeId=1&SectionId=15&bookingid=<%#((Booking)Container.DataItem).Id%>">
                                        <%# BookingUtil.GetBookingCode(((Booking)Container.DataItem).Id)%>
                                </td>
                                <td><%# ((Booking)Container.DataItem).Trip != null ? ((Booking)Container.DataItem).Trip.TripCode : "" %></td>
                                <td><%# ((Booking)Container.DataItem).Pax %></td>
                                <td class="--text-right"><%# BookingUtil.GetTotalAsString((Booking)Container.DataItem)%>₫</td>
                                <td class="--text-left"><a href="AgencyView.aspx?NodeId=1&SectionId=15&agencyid=<%# ((Booking)Container.DataItem).Agency != null ? ((Booking)Container.DataItem).Agency.Id : 0 %> ">
                                    <%# ((Booking)Container.DataItem).Agency != null ? ((Booking)Container.DataItem).Agency.Name : "" %></a></td>
                                <td><a href="" data-toggle="tooltip" title="<%# ((Booking)Container.DataItem).SpecialRequest%>">View</a></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <% 
                        if (((IEnumerable<Booking>)rptTodayBookings.DataSource).Count() <= 0)
                        {
                    %>
                    <tr>
                        <td colspan="100%">No records found</td>
                    </tr>
                    <%
                        }
                    %>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="2"></td>
                        <td class="td__total --text-bold">Total</td>
                        <td class="td__total --text-right"><%= BookingUtil.GetTotalOfBookings((IEnumerable<Booking>)rptTodayBookings.DataSource)%>₫</td>
                    </tr>
                </tfoot>
            </table>
        </div>
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="col-xs-offset-3 col-xs-3">
                    <h4 class="--text-bold" style="display: inline-block; max-width: 50%">Month summary</h4>
                    <asp:DropDownList runat="server" ID="ddlMonthSearching" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlMonthSearching_SelectedIndexChanged" Style="width: 21%; padding: 0; display: inline-block; position: relative; bottom: 7px">
                    </asp:DropDownList>
                    <asp:DropDownList runat="server" ID="ddlYearSearching" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlYearSearching_SelectedIndexChanged" Style="width: 24%; padding: 0; display: inline-block; position: relative; bottom: 7px">
                    </asp:DropDownList>
                    <table class="table table-borderless table__archivement">
                        <tr>
                            <td>Number of pax:</td>
                            <td>
                                <asp:Label runat="server" ID="lblNumberOfPax" /></td>
                        </tr>
                        <tr>
                            <td>Number of bookings:</td>
                            <td>
                                <asp:Label runat="server" ID="lblNumberOfBookings" /></td>
                        </tr>
                        <tr>
                            <td>Total revenue:</td>
                            <td>
                                <asp:Label runat="server" ID="lblTotalRevenue" />₫</td>
                        </tr>
                        <tr>
                            <td>Agencies visited:</td>
                            <td>
                                <asp:Label runat="server" ID="lblAgenciesVisited" /></td>
                        </tr>
                        <tr>
                            <td>Meeting reports:</td>
                            <td>
                                <asp:Label runat="server" ID="lblMeetingReports" /></td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="col-xs-8 ">
            <h4 class="--text-bold">Your new bookings received today/Status changed today</h4>
            <table class="table table-bordered table-common table__total">
                <tbody>
                    <tr class="header active">
                        <th style="width: 10%">Code</th>
                        <th style="width: 8%">Trip</th>
                        <th style="width: 6%">NoP</th>
                        <th style="width: 15%">Revenue</th>
                        <th style="width: 20%">Start date</th>
                        <th style="width: 8%">View</th>
                        <th>Created by</th>
                    </tr>
                    <asp:Repeater runat="server" ID="rptNewBookings">
                        <ItemTemplate>
                            <tr class="<%# ((Booking)Container.DataItem).Status == Portal.Modules.OrientalSails.Web.Util.StatusType.Cancelled ? "--cancelled":""%>
                                <%# ((Booking)Container.DataItem).Status == Portal.Modules.OrientalSails.Web.Util.StatusType.Pending ? "--pending":""%>">
                                <td><a href="BookingView.aspx?NodeId=1&SectionId=15&bookingid=<%#((Booking)Container.DataItem).Id%>"><%# BookingUtil.GetBookingCode(((Booking)Container.DataItem).Id)%></a></td>
                                <td><%# ((Booking)Container.DataItem).Trip != null ? ((Booking)Container.DataItem).Trip.TripCode : "" %></td>
                                <td><%# ((Booking)Container.DataItem).Pax %></td>
                                <td class="--text-right"><%# BookingUtil.GetTotalAsString((Booking)Container.DataItem)%>₫</td>
                                <td><%# ((Booking)Container.DataItem).StartDate.ToString("dd/MM/yyyy") %></td>
                                <td><a href="" data-toggle="tooltip" title="<%# ((Booking)Container.DataItem).SpecialRequest%>">View</a></td>
                                <td><%# ((Booking)Container.DataItem).CreatedBy != null ? ((Booking)Container.DataItem).CreatedBy.FullName : ""%></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Repeater runat="server" ID="rptChangedState">
                        <ItemTemplate>
                            <tr class="--changed-state">
                                <td><a href="BookingView.aspx?NodeId=1&SectionId=15&bookingid=<%#((Booking)Container.DataItem).Id%>"><%# BookingUtil.GetBookingCode(((Booking)Container.DataItem).Id)%></a></td>
                                <td><%# ((Booking)Container.DataItem).Trip != null ? ((Booking)Container.DataItem).Trip.TripCode : "" %></td>
                                <td><%# ((Booking)Container.DataItem).Pax %></td>
                                <td class="--text-right"><%# BookingUtil.GetTotalAsString((Booking)Container.DataItem)%>₫</td>
                                <td><%# ((Booking)Container.DataItem).StartDate.ToString("dd/MM/yyyy") %></td>
                                <td><a href="" data-toggle="tooltip" title="<%# ((Booking)Container.DataItem).SpecialRequest%>">View</a></td>
                                <td><%# ((Booking)Container.DataItem).CreatedBy != null ? ((Booking)Container.DataItem).CreatedBy.FullName : ""%></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <% 
                        if (((IEnumerable<Booking>)rptNewBookings.DataSource).Count() <= 0 && ((IEnumerable<Booking>)rptChangedState.DataSource).Count() <= 0)
                        {
                    %>
                    <tr>
                        <td colspan="100%">No records found</td>
                    </tr>
                    <%
                        }
                    %>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="2"></td>
                        <td class="td__total --text-bold">Total</td>
                        <td class="td__total --text-right"><%= BookingUtil.GetTotalOfBookings((IEnumerable<Booking>)rptNewBookings.DataSource)%>₫</td>
                    </tr>
                </tfoot>
            </table>
        </div>
        <div class="col-xs-offset-1 col-xs-3">
            <h4 class="--text-bold">Top 10 partners in - <%= DateTime.Now.ToString("MMMM").Substring(0,3)%> </h4>
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
        </div>
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="col-xs-9">
                    <h4 class="--text-bold">Most recent meetings&nbsp&nbsp&nbsp<a href="" data-toggle="modal" data-target="#addMeetingModal" onclick="clearFormMeeting();"><i class="fas fa-plus"></i></a></h4>
                    <div class="form-group area__title-control-group" style="position:relative">
                        <div class="row">
                            <div class="col-xs-12 --no-padding-right" style="text-align: right; position: absolute; top: -26px; right: 0; width: 77%">
                                <asp:TextBox runat="server" ID="txtFromRecentMeetingSearch" CssClass="form-control --width-auto" placeholder="From (dd/MM/yyyy)" data-control="datetimepicker"
                                    Style="display: inline-block" />
                                <asp:TextBox runat="server" ID="txtToRecentMeetingSearch" CssClass="form-control --width-auto" placeholder="To (dd/MM/yyyy)" data-control="datetimepicker"
                                    Style="display: inline-block" />
                                <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="Search" CssClass="btn btn-primary" />
                                <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" CssClass="btn btn-primary"></asp:Button>
                            </div>
                        </div>
                    </div>
                    <table class="table table-bordered table-common">
                        <tbody>
                            <tr class="header active">
                                <th style="width: 8%">Date</th>
                                <th style="width: 25%">Agency</th>
                                <th style="width: 10%">Contact</th>
                                <th style="width: 46%">View meetings</th>
                                <th style="width: auto"></th>
                            </tr>
                            <asp:Repeater ID="rptRecentMeetings" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <%# ((Activity)Container.DataItem).DateMeeting != null ? ((Activity)Container.DataItem).DateMeeting.ToString("dd/MM/yyyy"):""  %></td>
                                        <td class="--text-left"><a href="AgencyView.aspx?NodeId=1&SectionId=15&agencyid=<%#((Activity)Container.DataItem).Params %>">
                                            <%# AgencyGetById(((Activity)Container.DataItem).Params)!= null ? AgencyGetById(((Activity)Container.DataItem).Params).Name : "" %></td>
                                        <td><%# AgencyContactGetById(((Activity)Container.DataItem).ObjectId) != null ? AgencyContactGetById(((Activity)Container.DataItem).ObjectId).Name : ""  %></td>
                                        <td class="--text-left"><article><%# ((Activity)Container.DataItem).Note%></article></td>
                                        <td class="--text-right">
                                            <div class="button-group">
                                            </div>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <%     
                                if (((IEnumerable<Activity>)rptRecentMeetings.DataSource).Count() <= 0)
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
                    <h4 class="--text-bold --text-italic"><a href="ViewMeetings.aspx?NodeId=1&SectionId=15&sales=<%= CurrentUser.Id %>">View more meetings</a></h4>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="col-xs-3">
            <h4 class="--text-bold">Agencies send no bookings last 3 months</h4>
            <table class="table table-borderless table__agencies-no-bookings">
                <asp:Repeater ID="rptAgenciesSendNoBookings" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td><%# Container.ItemIndex + 1%></td>
                            <td><a href="AgencyView.aspx?NodeId=1&SectionId=15&agencyid=<%# Eval("Agency.Id")%>"><%# Eval("Agency.Name")%></td>
                            <td><%# ((DateTime)Eval("CreatedDate")).ToString("dd/MM/yyyy")%></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <%     
                    if (((IEnumerable<Booking>)rptAgenciesSendNoBookings.DataSource).Count() <= 0)
                    {
                %>
                <tr>
                    <td colspan="100%">No records found</td>
                </tr>
                <%
                    }
                %>
            </table>
        </div>
        <div class="col-xs-5">
            <h4 class="--text-bold">Agencies not visited / updated last 2 months</h4>
            <table class="table table-bordered table-common">
                <asp:Repeater runat="server" ID="rptAgencyNotVisited">
                    <ItemTemplate>
                        <tr>
                            <td><%# Container.ItemIndex %></td>
                            <td style="width: 27%" class="--text-left"><a href="AgencyView.aspx?NodeId=1&SectionId=15&agencyid=<%# Eval("AgencyId") %>"><%# Eval("Name")%></td>
                            <td><%# Eval("LastMeeting")%></td>
                            <td class="--text-left"><%# Eval("Note")%></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <%     
                    if (((IEnumerable<object>)rptAgencyNotVisited.DataSource).Count() <= 0)
                    {
                %>
                <tr>
                    <td colspan="100%">No records found</td>
                </tr>
                <%
                    }
                %>
            </table>
        </div>
    </div>
    <div class="modal fade meeting-modal" id="addMeetingModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog meeting-modal__modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title --text-bold" id="myModalLabel">Add meeting</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="row" data-hidactivityidclientid="<%= hidActivityId.ClientID %>" id="hidActivityIdClientId">
                            <asp:HiddenField runat="server" ID="hidActivityId" />
                            <div class="col-xs-2 --no-padding-leftright">
                                Agency
                            </div>
                            <div class="col-xs-10 --no-padding-leftright">
                                <input type="text" data-id="hidGuideId" style="display: none" name="txtAgencyId" />
                                <input type="text" id="txtAgency" placeholder="Select agency" readonly class="form-control" data-toggle="modal" data-target=".modal-selectGuide"
                                    data-url="AgencySelectorPage.aspx?NodeId=1&SectionId=15" onclick="setTxtGuideClicked(this)"
                                    data-id="txtName" name="txtAgency"/>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2 --no-padding-leftright">
                                Contact
                            </div>
                            <div class="col-xs-4 --no-padding-leftright">
                                <select class="form-control" name="ddlContact" data-id="ddlContact">
                                    <option value="0">-- Contact --</option>
                                </select>
                            </div>

                            <div class="col-xs-2 --no-padding-left --text-right">
                                Position
                            </div>
                            <div class="col-xs-4 --no-padding-leftright">
                                <asp:TextBox runat="server" ID="txtPosition" CssClass="form-control" placeholder="Position" disabled="disabled" data-id="txtPosition" />
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2 --no-padding-leftright">
                                Date meeting
                            </div>
                            <div class="col-xs-4 --no-padding-leftright">
                                <asp:TextBox runat="server" ID="txtDateMeeting" CssClass="form-control" placeholder="Date meeting" data-control="datetimepicker" data-id="txtDateMeeting" autocomplete="off" />
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-xs-2 --no-padding-leftright">
                                Note
                            </div>
                            <div class="col-xs-10 --no-padding-leftright">
                                <asp:TextBox runat="server" ID="txtNote" CssClass="form-control" TextMode="MultiLine" Rows="12" placeholder="Note" Text="" data-id="txtNote" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" Text="Save" CssClass="btn btn-primary" OnClientClick="return checkDouble(this)" />
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade modal-selectGuide" tabindex="-1" role="dialog" aria-labelledby="gridSystemModalLabel">
        <div class="modal-dialog" role="document" style="width: 1230px">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h3 class="modal-title">Select agency</h3>
                </div>
                <div class="modal-body">
                    <iframe frameborder="0" width="1200" scrolling="no" onload="resizeIframe(this)" src=""></iframe>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Scripts" ContentPlaceHolderID="Scripts" runat="server">
    <script>
        $('.modal-selectGuide').on('shown.bs.modal', function () {
            $(".modal-selectGuide iframe").attr('src', 'AgencySelectorPage1.aspx?NodeId=1&SectionId=15')
        })

        var txtGuideClicked = null;
        var rowGuideSelected = null;
        var txtGuideNameSelected = null;
        var txtPhoneSelected = null;
        var hidGuideIdSelected = null;
        function setTxtGuideClicked(txtGuide) {
            txtGuideClicked = txtGuide;
            if (typeof (txtGuideClicked) != "undefined") {
                rowGuideSelected = $(txtGuideClicked).closest(".row");
            }
        }

        var selectGuideIframe = $(".modal-selectGuide iframe");
        selectGuideIframe.on("load", function () {
            //giữ vị trí của scroll khi sang trang mới -- chức năng của phần selectguide
            if (window.name.search('^' + location.hostname + '_(\\d+)_(\\d+)_') == 0) {
                var name = window.name.split('_');
                $(".modal-selectGuide").scrollLeft(name[1]);
                $(".modal-selectGuide").scrollTop(name[2]);
                window.name = name.slice(3).join('_');
            }
            $(".pager a", selectGuideIframe.contents()).click(function () {
                window.name = location.hostname + "_" + $(".modal-selectGuide").scrollLeft() + "_" + $(".modal-selectGuide").scrollTop() + "_";
            })
            //--

            //chức năng select agency bằng popup
            $("[data-id = 'txtName']", selectGuideIframe.contents()).click(function () {
                if (typeof (txtGuideClicked) != "undefined") {
                    $(txtGuideClicked).val($(this).text())
                }
                if (typeof (rowGuideSelected) != "undefined") {
                    txtGuideNameSelected = $(rowGuideSelected).find("[data-id='txtName']");
                    txtPhoneSelected = $(rowGuideSelected).find("[data-id='txtPhone']");
                    hidGuideIdSelected = $(rowGuideSelected).find("[data-id='hidGuideId']");
                }
                if (typeof (txtPhoneSelected) != "undefined") {
                    $(txtPhoneSelected).val($(this).attr("data-phone"))
                }
                if (typeof (hidGuideIdSelected) != "undefined") {
                    $(hidGuideIdSelected).val($(this).attr("data-agencyid"));
                }
                if (typeof (txtGuideNameSelected) != "undefined") {
                    $(txtGuideNameSelected).val($(this).text())
                }
                $('.modal-selectGuide').modal('hide')
                $(hidGuideIdSelected).trigger('input');
                $(hidGuideIdSelected).trigger('change');
                $(txtGuideNameSelected).trigger('input');
                $(txtGuideNameSelected).trigger('change');
            });
            //--
        })
    </script>
    <script>
        $("[name = txtAgencyId]").change(function () {
            $('[name = ddlContact]').find('option:not(:first)').remove();
            $.ajax({
                type: 'POST',
                url: 'WebService/DashBoardWebService.asmx/AgencyContactGetAllByAgencyId',
                data: "{ 'ai': '" + $(this).val() + "'}",
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
            }).done(function (data) {
                var agencyContacts = JSON.parse(data.d);
                $.each(agencyContacts, function (i, agencyContact) {
                    $('[name = ddlContact]')
                        .append($('<option>', {
                            value: agencyContact.Id,
                            text: agencyContact.Name,
                            _position: agencyContact.Position,
                        }));
                });
            })
        })
        $('[name = ddlContact]').change(function () {
            $('#<%= txtPosition.ClientID %>').val($(this).find('option:selected').attr('_position'));
        })
    </script>
    <script>
        function clearFormMeeting() {
            clearForm($('#addMeetingModal .modal-content'));
        }
    </script>
    <script>
        $(document).ready(function () {
            $("#aspnetForm").validate({
                rules: {
                    txtAgency: "required",
                    <%= txtDateMeeting.UniqueID%>: "required",
                    <%= txtNote.UniqueID%> : "required",
                },
                messages: {
                    txtAgency: "Yêu cầu chọn một Agency",
                    <%= txtDateMeeting.UniqueID%>: "Yêu cầu chọn ngày",
                    <%= txtNote.UniqueID%>:"Yêu cầu điền Note",
                },
                errorElement: "em",
                errorPlacement: function (error, element) {
                    error.addClass("help-block");

                    if (element.prop("type") === "checkbox") {
                        error.insertAfter(element.parent("label"));
                    } else {
                        error.insertAfter(element);
                    }

                    if (element.siblings("span").prop("class") === "input-group-addon") {
                        error.insertAfter(element.parent()).css({ color: "#a94442" });
                    }
                },
                highlight: function (element, errorClass, validClass) {
                    $(element).closest("div").addClass("has-error").removeClass("has-success");
                },
                unhighlight: function (element, errorClass, validClass) {
                    $(element).closest("div").removeClass("has-error");
                }
            })

        })
    </script>
</asp:Content>
