<%@ Page Language="C#" MasterPageFile="TourMaster.Master" AutoEventWireup="true"
    Codebehind="AddBooking.aspx.cs" Inherits="CMS.Modules.TourManagement.Web.Admin.AddBooking"
    Title="Untitled Page" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Assembly="CMS.Modules.TourManagement" Namespace="CMS.Modules.TourManagement.Web.Controls"
    TagPrefix="tsc" %>
<%@ Register Src="/Modules/TourHotel/Controls/HotelSelectorProxy.ascx" TagPrefix="asp"
    TagName="HotelSelector" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">

    <script type="text/javascript">
    function toggleVisible(id)
    {
        item = document.getElementById(id);
        if (item.style.display=="")
        {
            item.style.display="none";
        }
        else
        {
            item.style.display=""
        }
    }
    </script>

    <fieldset>
        <legend>Add booking tour</legend>
        <asp:PlaceHolder ID="stepInfo" runat="server" Visible="false">
            <div class="basicinfo">
                <table>
                    <tr>
                        <td>
                            Code</td>
                        <td>
                            <asp:Literal ID="litCode" runat="server"></asp:Literal>
                        </td>
                        <td>
                            Customer</td>
                        <td>
                            <asp:TextBox ID="txtCustomers" runat="server"></asp:TextBox></td>
                        <td>
                            Number of pax</td>
                        <td>
                            <asp:TextBox ID="txtNumberOfPax" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                            Start date</td>
                        <td>
                            <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                            <ajax:CalendarExtender ID="calendarStartDate" runat="server" Format="dd/MM/yyyy"
                                TargetControlID="txtStartDate">
                            </ajax:CalendarExtender>
                        </td>
                        <td>
                            Start location</td>
                        <td>
                            <svc:CascadingDropDown ID="cddlCountries" runat="server">
                            </svc:CascadingDropDown><br />
                            <svc:CascadingDropDown ID="cddlRegions" runat="server">
                            </svc:CascadingDropDown><br />
                            <svc:CascadingDropDown ID="cddlCities" runat="server">
                            </svc:CascadingDropDown>
                        </td>
                        <td>
                            Number of days</td>
                        <td>
                            <asp:TextBox ID="txtNumberOfDays" runat="server"></asp:TextBox></td>
                    </tr>
                </table>
            </div>
            <asp:Button ID="btnStepInfo" runat="server" Text="Save" CssClass="button" OnClick="btnStepInfo_Click" />
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="stepSetup" runat="server" Visible="false">
            <div>
                <asp:UpdatePanel ID="udpBookingDays" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <table style="width: 100%;">
                            <asp:Repeater ID="rptBookingDays" runat="server" OnItemDataBound="rptBookingDays_ItemDataBound">
                                <HeaderTemplate>
                                    <tr>
                                        <th style="width: 45px;">
                                            Day</th>
                                        <th style="width: 145px;">
                                            Date</th>
                                        <th colspan="3">
                                            Itinerary</th>
                                        <th style="width: 125px;">
                                            Rate</th>
                                        <th colspan="2" style="width: 70px;">
                                            Add</th>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td colspan="8">
                                            <asp:ImageButton runat="server" ID="btnInsertBefore" ToolTip='Insert before' ImageUrl="../Images/up_plus.gif"
                                                OnClick="btnInsertBefore_Click" AlternateText='Insert before' ImageAlign="AbsMiddle"
                                                CssClass="image_button16" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
                                            <asp:ImageButton runat="server" ID="btnInsertAfter" ToolTip='Insert after' ImageUrl="../Images/down_plus.gif"
                                                OnClick="btnInsertAfter_Click" AlternateText='Insert after' ImageAlign="AbsMiddle"
                                                CssClass="image_button16" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
                                            <asp:ImageButton runat="server" ID="btnRemoveDay" ToolTip='Remove day' ImageUrl="../Images/delete.png"
                                                OnClick="btnRemoveDay_Click" AlternateText='Remove day' ImageAlign="AbsMiddle"
                                                CssClass="image_button16" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
                                            <ajax:ConfirmButtonExtender ID="confirmRemoveDay" runat="server" ConfirmText="All associated data cann't be recovered, are you sure?"
                                                TargetControlID="btnRemoveDay">
                                            </ajax:ConfirmButtonExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HiddenField ID="hiddenId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                            <asp:HiddenField ID="hiddenIndex" runat="server" />
                                            <asp:Literal ID="litTitle" runat="server"></asp:Literal></td>
                                        <td>
                                            <asp:Literal ID="litDate" runat="server"></asp:Literal>
                                        </td>
                                        <tsc:TourSelector ID="tourSelector" runat="server" TourOnly="true" AutoPostBack="true"
                                            OnSelect="tourSelector_Select" WrapInTag="td" SelectImageUrl="../Images/folder.png"
                                            RemoveImageUrl="../Images/delete.png"></tsc:TourSelector>
                                        <asp:LinkButton ID="btnTourSelector" runat="server" Style="display: none" OnClick="btnTourSelector_Select" />
                                        <td>
                                            <asp:TextBox ID="txtRate" runat="server"></asp:TextBox></td>
                                        <td>
                                            <asp:Button ID="btnAddTour" runat="server" Text="Tour" CssClass="button" OnClick="btnAddTour_Click" /></td>
                                        <td>
                                            <asp:Button ID="btnAddService" runat="server" Text="Misc" CssClass="button" OnClick="btnAddService_Click" /></td>
                                    </tr>
                                    <asp:Repeater ID="rptServices" runat="server" OnItemDataBound="rptServices_ItemDataBound">
                                        <ItemTemplate>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:HiddenField ID="hiddenId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtName" runat="server" Width="300"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRate" runat="server"></asp:TextBox></td>
                                                <td colspan="2"></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <tr id="trHotel" runat="server">
                                        <td colspan="2">
                                        Hotel
                                        </td>
                                        <td>
                                            <asp:HiddenField ID="hiddenParameters" runat="server" />
                                            <asp:DropDownList ID="ddlHotels" runat="server">
                                            </asp:DropDownList>
                                            <asp:Literal ID="litHotel" runat="server"></asp:Literal>
                                        </td>
                                        <td>
                                            <input type="image" id="btnChangeHotel" runat="server" src="/Admin/AdminModuleImages/reorder.gif"
                                                alt="Change hotel" title="Change hotel" value="Change" class="image_button16" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="btnRemoveHotel" runat="server" ImageUrl="../Images/delete.png"
                                                AlternateText="Remove hotel" ToolTip="Remove hotel" CssClass="image_button16"
                                                OnClick="btnRemoveHotel_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'/>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHotelRate" runat="server"></asp:TextBox>
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>
                                    <asp:Repeater ID="rptTours" runat="server" OnItemDataBound="rptTours_ItemDataBound">
                                        <ItemTemplate>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:HiddenField ID="hiddenId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                                    Connecting tour
                                                </td>
                                                <tsc:TourSelector ID="tourSelector" runat="server" TourOnly="true" WrapInTag="td"
                                                    SelectImageUrl="../Images/folder.png" RemoveImageUrl="../Images/delete.png" AutoPostBack="true"
                                                    OnSelect="connectedTour_Select"></tsc:TourSelector>
                                                <asp:LinkButton ID="btnTourSelector" runat="server" Style="display: none" OnClick="btnConnectedTour_Select" />
                                                <td>
                                                    <asp:TextBox ID="txtRate" runat="server"></asp:TextBox></td>
                                                <td colspan="2"></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <tr style="background: #ABC3D7; height: 2px;">
                                        <td colspan="8">
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <tr>
                                <td colspan="5">
                                    TOTAL</td>
                                <td>
                                    <asp:TextBox ID="txtTotal" runat="server"></asp:TextBox></td>
                                <td colspan="2">
                                    <asp:Button ID="btnCalculate" runat="server" OnClick="btnCalculate_Click" Text="Calculate"
                                        CssClass="button" /></td>
                            </tr>
                        </table>
                        <asp:Panel ID="panelPopup" runat="server" Visible="false">
                            <div class="window" style="width:300px;">
                                <div class="titlebar">Warning</div>
                                <div class="content" style="height:auto;">
                                You have select a tour with more days than expected. Be sure to<br />
                                1. Override will override next tour(s) in conflict (by delete them for new tour
                                to take place<br />
                                2. Insert day(s) before next tour to ensure that next tour will not be delete<br />
                                3. Cancel and select another tour<br />
                                <center>
                                    <asp:Button ID="btnOverride" runat="server" Text="Override tour" CssClass="button"
                                        OnClick="btnOverride_Click" />
                                    <asp:Button ID="btnInsertAbstract" runat="server" Text="Insert days" CssClass="button"
                                        OnClick="btnInsertAbstract_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click" />
                                </center>
                                </div>
                            </div>
                        </asp:Panel>
                        <ajax:AlwaysVisibleControlExtender ID="avcPopup" runat="server" HorizontalSide="Center"
                            VerticalSide="Middle" TargetControlID="panelPopup">
                        </ajax:AlwaysVisibleControlExtender>
                        <asp:Panel ID="popupAbstractDay" runat="server" Visible="false">
                            <div class="window" style="width:300px;">
                                <div class="titlebar">Warning</div>
                                <div class="content" style="height:auto;">
                                You have select a tour with more days than expected. You can expand the booking to fit new timeline. Do you want to expand booking or continue to design?
                                <center>
                                    <asp:Button ID="btnAbstractConfirm" runat="server" Text="Sure" CssClass="button"
                                        OnClick="btnAddBookingDays_Click" />
                                    <asp:Button ID="btnAbstractCancel" runat="server" Text="Continue" CssClass="button" OnClick="btnCancel_Click"/>
                                </center>
                                </div>
                            </div>
                        </asp:Panel>
                        <ajax:AlwaysVisibleControlExtender ID="avcAbstractDay" runat="server" HorizontalSide="Center"
                            VerticalSide="Middle" TargetControlID="popupAbstractDay">
                        </ajax:AlwaysVisibleControlExtender>
                        <asp:Button ID="btnAddBookingDays" runat="server" Text="Add abstract day" CssClass="button"
                            OnClick="btnAddBookingDays_Click" Visible="false" />
                        <asp:Button ID="btnAddBookingDay" runat="server" Text="Add day" CssClass="button"
                            OnClick="btnAddBookingDay_Click" />
                        <asp:Button ID="btnStepSetup" runat="server" Text="Save" CssClass="button" OnClick="btnSetSetup_Click" />
                        <svc:PopupWin ID="popupError" runat="server" Visible="False" ColorStyle="Blue" Width="230px"
                            Height="100px" DockMode="BottomRight" WindowScroll="False" WindowSize="300, 200">
                        </svc:PopupWin>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnStepSetup" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </asp:PlaceHolder>
    </fieldset>
</asp:Content>
