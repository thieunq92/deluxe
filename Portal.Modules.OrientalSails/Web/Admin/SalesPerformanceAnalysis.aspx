<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="Main.Master" CodeBehind="SalesPerformanceAnalysis.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.SalesPerformanceAnalysis" %>

<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Sales Performance Report - <%= Sales.FullName %></title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="row">
        <div class="col-xs-9">
            <h3>SALES PERFORMANCE REPORT - <%= Sales.FullName %>
                <asp:DropDownList runat="server" ID="ddlYear" CssClass="form-control" Style="float: right; width: 10%" AutoPostBack="true"></asp:DropDownList>
                <asp:DropDownList runat="server" ID="ddlMonth" CssClass="form-control" Style="float: right; width: 10%" AutoPostBack="true"></asp:DropDownList>
            </h3>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-9">
            <div class="row">
                <div class="col-xs-12 --no-padding-leftright">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <h4 class="--text-bold">General information</h4>
                            <table class="table table-bordered table-common">
                                <tr class="active">
                                    <th class="--text-left">Details</th>
                                    <asp:Repeater runat="server" ID="rptMonths" OnItemDataBound="rptMonths_ItemDataBound">
                                        <ItemTemplate>
                                            <th>
                                                <asp:Literal runat="server" ID="ltrMonth" /></th>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                                <tr>
                                    <td class="--text-left">No of pax</td>
                                    <asp:Repeater runat="server" ID="rptNoOfPax" OnItemDataBound="rptNoOfPax_ItemDataBound">
                                        <ItemTemplate>
                                            <td>
                                                <asp:Literal runat="server" ID="ltrNoOfPax" /></td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                                <tr>
                                    <td class="--text-left">No of bookings</td>
                                    <asp:Repeater runat="server" ID="rptNoOfBooking" OnItemDataBound="rptNoOfBooking_ItemDataBound">
                                        <ItemTemplate>
                                            <td>
                                                <asp:Literal runat="server" ID="ltrNoOfBooking" /></td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                                <tr>
                                    <td class="--text-left">Revenue in USD</td>
                                    <asp:Repeater runat="server" ID="rptRevenueInMonth" OnItemDataBound="rptRevenueInMonth_ItemDataBound">
                                        <ItemTemplate>
                                            <td>
                                                <asp:Literal runat="server" ID="ltrRevenueInMonth" /></td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                                <tr>
                                    <td class="--text-left">Meeting Reports</td>
                                    <asp:Repeater runat="server" ID="rptMeetingReport" OnItemDataBound="rptMeetingReport_ItemDataBound">
                                        <ItemTemplate>
                                            <td>
                                                <asp:Literal runat="server" ID="ltrMeetingReport" /></td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                                <tr>
                                    <td class="--text-left">% out of Total</td>
                                    <asp:Repeater runat="server" ID="rptPercentOutOfTotal" OnItemDataBound="rptPercentOutOfTotal_ItemDataBound">
                                        <ItemTemplate>
                                            <td>
                                                <asp:Literal runat="server" ID="ltrPercentOutOfTotal" /></td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                                <tr>
                                    <td class="--text-left">Top 10 percentage</td>
                                    <asp:Repeater runat="server" ID="rptTopPercentage" OnItemDataBound="rptTopPercentage_ItemDataBound">
                                        <ItemTemplate>
                                            <td>
                                                <asp:Literal runat="server" ID="ltrTopPercentage" /></td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlMonth" />
                            <asp:AsyncPostBackTrigger ControlID="ddlYear" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 --no-padding-leftright">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <h4 class="--text-bold">Top 10 partners analysis</h4>
                            <table class="table table-bordered table-common">
                                <tr class="active">
                                    <asp:Repeater runat="server" ID="rptTopPartnerMonth" OnItemDataBound="rptTopPartnerMonth_ItemDataBound">
                                        <ItemTemplate>
                                            <th colspan="2">
                                                <asp:Literal runat="server" ID="ltrMonth" /></th>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                                <tr class="active">
                                    <asp:Repeater runat="server" ID="rptTopPartnerMonthSub">
                                        <ItemTemplate>
                                            <th>Name</th>
                                            <th>No of pax</th>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                                <asp:Repeater runat="server" ID="rptTopPartnerIndex" OnItemDataBound="rptTopPartnerIndex_ItemDataBound">
                                    <ItemTemplate>
                                        <tr>
                                            <asp:Repeater runat="server" ID="rptTopPartnerData" OnItemDataBound="rptTopPartnerData_ItemDataBound">
                                                <ItemTemplate>
                                                    <td class="--text-left" style="height:24px">
                                                        <asp:Literal runat="server" ID="ltrName" /></td>
                                                    <td>
                                                        <asp:Literal runat="server" ID="ltrNumberOfPax" /></td>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <tr>
                                            <asp:Repeater runat="server" ID="rptTopPartnerData" OnItemDataBound="rptTopPartnerData_ItemDataBound">
                                                <ItemTemplate>
                                                    <td class="--text-left --text-bold">Total top 10</td>
                                                    <td class="--text-bold">
                                                        <asp:Literal runat="server" ID="ltrNumberOfPax" /></td>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tr>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlMonth" />
                            <asp:AsyncPostBackTrigger ControlID="ddlYear" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 --no-padding-leftright">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <h4 class="--text-bold">New partners </h4>
                            <table class="table table-bordered table-common">
                                <tr class="active">
                                    <th>Name</th>
                                    <th>Most recent meeting</th>
                                    <th style="width: 100px">Last booking</th>
                                </tr>
                                <asp:Repeater runat="server" ID="rptNewPartner" OnItemDataBound="rptNewPartner_ItemDataBound">
                                    <ItemTemplate>
                                        <tr>
                                            <td class="--text-left">
                                                <asp:Literal ID="ltrName" runat="server" />
                                            </td>
                                            <td class="--text-left">
                                                <asp:Literal ID="ltrMostRecentMeeting" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Literal ID="ltrLastBooking" runat="server" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlMonth" />
                            <asp:AsyncPostBackTrigger ControlID="ddlYear" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 --no-padding-leftright">
                    <h4 class="--text-bold">Partner in charge</h4>
                    <table class="table table-bordered table-common">
                        <tr class="active">
                            <th rowspan="2">No</th>
                            <th rowspan="2">Name</th>
                            <th rowspan="2">Contract status</th>
                            <th rowspan="2">Role</th>
                            <th rowspan="2">Last booking</th>
                            <th colspan="2">Last meeting</th>
                        </tr>
                        <tr class="active">
                            <th>Date</th>
                            <th>Details</th>
                        </tr>
                        <asp:Repeater ID="rptPartnerInCharge" runat="server" OnItemDataBound="rptPartnerInCharge_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%# Container.ItemIndex + 1 %>
                                    </td>
                                    <td class="--text-left">
                                        <asp:Literal runat="server" ID="ltrName" />
                                    </td>
                                    <td>
                                        <asp:Literal runat="server" ID="ltrContractStatus" />
                                    </td>
                                    <td style="width: 100px">
                                        <asp:Literal runat="server" ID="ltrRole" />
                                    </td>
                                    <td>
                                        <asp:Literal runat="server" ID="ltrLastBooking" />
                                    </td>
                                    <td>
                                        <asp:Literal runat="server" ID="ltrLastMeeting" />
                                    </td>
                                    <td class="--text-left">
                                        <asp:Literal runat="server" ID="ltrDetails" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </div>
        </div>
        <div class="col-xs-3 --no-padding-left">
            <div class="row">
                <div class="col-xs-12 --no-padding-leftright">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <h4 class="--text-bold">Top 10 partner  
                        <asp:DropDownList runat="server" ID="ddlTopPartnerMonth" CssClass="form-control" Style="float: right; width: 30%" AutoPostBack="true"></asp:DropDownList>
                                <asp:DropDownList runat="server" ID="ddlTopPartnerYear" CssClass="form-control" Style="float: right; width: 30%" AutoPostBack="true"></asp:DropDownList></h4>
                            <table class="table table-borderless">
                                <asp:Repeater ID="rptTopPartner" runat="server" OnItemDataBound="rptTopPartner_ItemDataBound">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Container.ItemIndex + 1 %></td>
                                            <td>
                                                <asp:Literal runat="server" ID="ltrAgencyName" /></td>
                                            <td>
                                                <asp:Literal runat="server" ID="ltrNumberOfPax" /></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlTopPartnerMonth" />
                            <asp:AsyncPostBackTrigger ControlID="ddlTopPartnerYear" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Scripts" ContentPlaceHolderID="Scripts" runat="server">
</asp:Content>
