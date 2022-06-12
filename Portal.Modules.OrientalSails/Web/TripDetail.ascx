<%@ Control Language="C#" AutoEventWireup="true" Codebehind="TripDetail.ascx.cs"
    Inherits="Portal.Modules.OrientalSails.Web.TripDetail" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<div class="trip_detail">
    <h3>
        <asp:Label ID="label_Name" runat="server" CssClass="trip_detail_name"></asp:Label></h3>
    <br />
    <asp:Image ID="image_Trip" runat="server" CssClass="trip_detail_image" />
    <asp:Label ID="label_NumberOfDays" runat="server" CssClass="trip_detail_NoOfDay"></asp:Label>
    <br />
    <asp:DropDownList ID="ddlOption" runat="server">
    </asp:DropDownList>
    <asp:TextBox ID="textBoxStartDate" runat="server"></asp:TextBox>
    <asp:Image ID="imagecalenderStartDate" ImageUrl="/Images/calendar.gif" runat="server"
        CssClass="image_button16" ImageAlign="AbsMiddle" />
    <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ValidationGroup="date"
        ControlToValidate="textBoxStartDate" ErrorMessage="Required"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ValidationGroup="date" ID="revStartDate" runat="server"
        ControlToValidate="textBoxStartDate" EnableClientScript="false" ErrorMessage="Date is not valid"
        ValidationExpression="(?n:^(?=\d)((?<day>31(?!(.0?[2469]|11))|30(?!.0?2)|29(?(.0?2)(?=.{3,4}(1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(16|[2468][048]|[3579][26])00))|0?[1-9]|1\d|2[0-8])(?<sep>[/.-])(?<month>0?[1-9]|1[012])\2(?<year>(1[6-9]|[2-9]\d)\d{2})(?:(?=\x20\d)\x20|$))?(?<time>((0?[1-9]|1[012])(:[0-5]\d){0,2}(?i:\ [AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$)"></asp:RegularExpressionValidator>
    <ajax:CalendarExtender ID="calenderStartDate" runat="server" Format="dd/MM/yyyy"
        TargetControlID="textBoxStartDate" PopupButtonID="imagecalenderStartDate">
    </ajax:CalendarExtender>
    <asp:ImageButton ID="imageButtonBook" ValidationGroup="date" runat="server" ImageAlign="AbsMiddle"
        ImageUrl="/Modules/Sails/Images/book_now_icon.gif" AlternateText="Book now" ToolTip="Book now"
        OnClick="imageButtonBook_Click" />
    <asp:Label ID="labelError" runat="server"></asp:Label>
    <p id="Description" runat="server" class="trip_detail_advanceinfo">
    </p>
    <div style="clear:both"></div>
    <ajax:TabContainer ID="tabTrip" runat="server">
        <ajax:TabPanel ID="tabActivities" runat="server" HeaderText="Activities">
            <ContentTemplate>
                <p id="Activities" runat="server" class="trip_detail_advanceinfo">
                </p>
            </ContentTemplate>
        </ajax:TabPanel>
        <ajax:TabPanel ID="tabInclusions" runat="server" HeaderText="Inclusions">
            <ContentTemplate>
                <p id="Inclusions" runat="server" class="trip_detail_advanceinfo">
                </p>
            </ContentTemplate>
        </ajax:TabPanel>
        <ajax:TabPanel ID="tabExclusions" runat="server" HeaderText="Exclusions">
            <ContentTemplate>
                <p id="Exclusions" runat="server" class="trip_detail_advanceinfo">
                </p>
            </ContentTemplate>
        </ajax:TabPanel>
        <ajax:TabPanel ID="tabWhatToTake" runat="server" HeaderText="Instructions">
            <ContentTemplate>
                <p id="WhatToTake" runat="server" class="trip_detail_advanceinfo">
                </p>
            </ContentTemplate>
        </ajax:TabPanel>
        <ajax:TabPanel ID="tabPrice" runat="server" HeaderText="Price">
            <ContentTemplate>
                <asp:Repeater ID="rptTripOption" runat="server" OnItemDataBound="rptTripOption_ItemDataBound">
                    <ItemTemplate>
                    <h4><asp:Literal ID="litOption" runat="server"></asp:Literal></h4>
                        <table>
                            <asp:Repeater ID="rptRoomClasses" runat="server" OnItemDataBound="rptRoomClasses_ItemDataBound">
                                <HeaderTemplate>
                                    <tr>
                                        <th>
                                            Room</th>
                                        <th>
                                            Price</th>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Repeater ID="rptRoomTypes" runat="server" OnItemDataBound="rptRoomTypes_ItemDataBound">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:Literal ID="litRoomName" runat="server"></asp:Literal></td>
                                                <td>
                                                    <asp:Literal ID="litPrice" runat="server"></asp:Literal></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </ItemTemplate>
                </asp:Repeater>
            </ContentTemplate>
        </ajax:TabPanel>
    </ajax:TabContainer>
</div>
