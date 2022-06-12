<%@ Page Language="C#" MasterPageFile="Main.Master" AutoEventWireup="True"
    CodeBehind="BookingView.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.BookingView"
    Title="Untitled Page" %>

<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="uc" TagName="customer" Src="../Controls/CustomerInfoRowInput.ascx" %>
<%@ Register Assembly="Portal.Modules.OrientalSails" Namespace="Portal.Modules.OrientalSails.Web.Controls"
    TagPrefix="orc" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Booking view</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="form-group">
        <div class="row">
            <div class="col-xs-12">
                <asp:CheckBox ID="chkSendMail" runat="server" Checked="false" CssClass="checkbox"
                    Visible="False" />
                <asp:Button ID="buttonSubmit" runat="server" CssClass="btn btn-primary" OnClick="buttonSubmit_Click" />
                <asp:Button ID="buttonCancel" runat="server" CssClass="btn btn-primary" OnClick="buttonCancel_Click" />
                <input type="button" class="btn btn-primary" id="btnEmail" runat="server" value="Send mail" />
                <input type="button" class="btn btn-primary" id="btnViewHistory" runat="server" value="View history" />
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-1">
                Booking code
            </div>
            <div class="col-xs-3 --no-padding-left">
                <asp:Label ID="label_BookingId" runat="server"></asp:Label>
                <asp:TextBox ID="txtBookingId" runat="server"></asp:TextBox>
            </div>
            <div class="col-xs-1">
                Trip
            </div>
            <div class="col-xs-7 --no-padding-left">
                <asp:DropDownList ID="ddlOrgs" runat="server" CssClass="form-control" Style="display: inline-block; width: 14%; padding-right: 3px" AutoPostBack="true">
                </asp:DropDownList>
                <svc:CascadingDropDown runat="server" ID="cddlTrips" CssClass="form-control" Style="display: inline-block; width: 35%" />
                <asp:DropDownList ID="ddlOptions" runat="server" CssClass="form-control" Style="display: inline-block; width: 14%">
                    <asp:ListItem>Option 1</asp:ListItem>
                    <asp:ListItem>Option 2</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-1">
                Start date
            </div>
            <div class="col-xs-3 --no-padding-left">
                <asp:TextBox ID="txtStartDate" runat="server" placeholder="Start date(dd/MM/yyyy)" CssClass="form-control" data-control="datetimepicker" Style="width: 43%"
                    AutoPostBack="true"></asp:TextBox>
            </div>
            <div class="col-xs-1">
                Status
            </div>
            <div class="col-xs-4 --no-padding-left">
                <asp:DropDownList ID="ddlStatusType" runat="server" CssClass="form-control" Style="width: 25%">
                </asp:DropDownList>
            </div>
            <div class="col-xs-1 --no-padding-right --no-padding-left">
                Number of pax
            </div>
            <div class="col-xs-2 --no-padding-left">
                <asp:Literal ID="litPax" runat="server"></asp:Literal>
                <i class="fa fa-info-circle fa-lg" aria-hidden="true" data-toggle="tooltip" data-placement="right" title="<%= PaxGetDetails() %>"></i>
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-1">
                Agency
            </div>
            <div class="col-xs-3 --no-padding-left">
                <asp:DropDownList ID="ddlAgencies" runat="server" CssClass="form-control" Style="display: inline-block; width: 65%" AutoPostBack="true">
                </asp:DropDownList>
                <asp:TextBox ID="txtAgencyCode" CssClass="form-control" placeholder="TA Code" runat="server" data-toggle="tooltip" title="TA code" data-placement="top"
                    Style="width: 33%; display: inline-block; padding: 1px"></asp:TextBox>
            </div>
            <div class="col-xs-1">
                Booker
            </div>
            <div class="col-xs-4 --no-padding-left">
                <svc:CascadingDropDown ID="cddlBooker" runat="server" CssClass="form-control" Style="width: 53%">
                </svc:CascadingDropDown>
                <div style="display: none">
                    <img id="ajaxloader" src="/images/loading.gif" alt="loading" style="visibility: hidden; height: 12px;" />
                    <asp:Label ID="labelTelephone" runat="server"></asp:Label>
                </div>
            </div>
            <div class="col-xs-1 --no-padding-right --no-padding-left">
                Approved by
            </div>
            <div class="col-xs-2 --no-padding-left">
                <asp:Literal ID="litApprovedBy" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-1">
                Created by
            </div>
            <div class="col-xs-3 --no-padding-left">
                <asp:Literal ID="litCreated" runat="server"></asp:Literal>
            </div>
            <div class="col-xs-1">
                Created date
            </div>
            <div class="col-xs-4 --no-padding-left">
                <asp:HyperLink ID="hplBookingDate" runat="server"></asp:HyperLink>
            </div>
            <div class="col-xs-1 --no-padding-right --no-padding-left">
                Pay before tour
            </div>
            <div class="col-xs-2 --no-padding-left">
                <asp:CheckBox ID="chkIsPaymentNeeded" runat="server" CssClass="" />
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-1">
                Public price
            </div>
            <div class="col-xs-1 --no-padding-left">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <%= GetPublicPrice().ToString("#,0.##") %> <span style="font-size: 9px">VND</span> <span class="--float-right">‒</span>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="cddlTrips" />
                        <asp:AsyncPostBackTrigger ControlID="ddlOrgs" />
                        <asp:AsyncPostBackTrigger ControlID="txtStartDate" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="col-xs-1 --no-padding-right">
                TA Commission
            </div>
            <div class="col-xs-1 --no-padding-left">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <%= GetTACommission().ToString("#,0.##")%> <span style="font-size: 9px">VND</span> <span class="--float-right">=</span>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtCommission" />
                        <asp:AsyncPostBackTrigger ControlID="ddlAgencies" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="col-xs-1 --no-padding-right">
                Net Income
            </div>
            <div class="col-xs-1 --no-padding-left">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <%= GetReceivable().ToString("#,0.##")%> <span style="font-size: 9px">VND</span>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="col-xs-2" style="width: 11%">| Actual receivable</div>
            <div class="col-xs-1 --no-padding-left">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <%= GetActualReceivable().ToString("#,0.##")%> <span style="font-size: 9px">VND</span>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtTotal" />
                        <asp:AsyncPostBackTrigger ControlID="txtDiscountPercent" />
                        <asp:AsyncPostBackTrigger ControlID="txtDiscountAmount" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-1">
                Total
            </div>
            <div class="col-xs-3 --no-padding-left">
                <asp:UpdatePanel runat="server" RenderMode="Inline">
                    <ContentTemplate>
                        <asp:TextBox ID="txtTotal" runat="server" data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true"
                            data-permission="canedittotal" data-locktype="readonly" CssClass="form-control" Style="display: inline-block; width: 39%" OnLoad="txtTotal_Load">
                        </asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtTotal" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:DropDownList ID="ddlCurrency" runat="server" CssClass="form-control" Style="display: inline-block; width: 25%">
                    <asp:ListItem Value="true">VND</asp:ListItem>
                    <asp:ListItem Value="false">USD</asp:ListItem>
                </asp:DropDownList>
                <asp:Button runat="server" ID="btnLockIncome" CssClass="btn btn-primary" Text="Lock booking"
                    OnClick="btnLockIncome_Click" data-permission="canlockincome" data-locktype="disabled" />
                <asp:Button runat="server" ID="btnUnlockIncome" Visible="False" CssClass="button"
                    Text="Unlock" OnClick="btnUnlockIncome_Click" />
                <asp:Image ImageUrl="../Images/info.png" ID="imgLockIncome" runat="server" ImageAlign="AbsMiddle"
                    Visible="False" />
                <ajax:HoverMenuExtender ID="hmeLockIncome" runat="Server" HoverCssClass="popupHover"
                    PopupControlID="pLockIncome" PopupPosition="Right" TargetControlID="imgLockIncome"
                    PopDelay="25" />
                <asp:Panel CssClass="hover_content" ID="pLockIncome" runat="server" Style="width: auto">
                    <asp:Literal runat="server" ID="litLockIncome" Visible="False"></asp:Literal>
                </asp:Panel>
            </div>
            <div class="col-xs-4">
                <div class="col-xs-6 --no-padding-left" style="width: 41%; padding-right: 5px;">
                    Discount percent<asp:TextBox ID="txtDiscountPercent" runat="server" CssClass="form-control" Style="display: inline-block; width: 30%" OnLoad="txtPercentDiscount_Load"
                        data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'rightAlign':'false'"></asp:TextBox>%
                </div>
                <div class="col-xs-6 --no-padding-leftright" style="width: 59%">
                    Discount
                <asp:TextBox ID="txtDiscountAmount" runat="server" CssClass="form-control" Style="display: inline-block; width: 37.5%; margin-left: 5px"
                    data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true" placeholder="Discount" Text="0" OnLoad="txtDiscountAmount_Load"></asp:TextBox>
                    <asp:DropDownList ID="ddlDiscountCurrencyType" runat="server" CssClass="form-control" Style="display: inline-block; width: 35%">
                        <asp:ListItem Value="VND">VND</asp:ListItem>
                        <asp:ListItem Value="USD">USD</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-1">
                Driver collect
            </div>
            <div class="col-xs-3 --no-padding-left">
                <asp:TextBox ID="txtDriverCollect" runat="server" data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true" CssClass="form-control"
                    Style="display: inline-block; width: 71%"></asp:TextBox>
                <asp:DropDownList ID="ddlCurrencyDriverCollect" runat="server" CssClass="form-control" Style="display: inline-block; width: 27%">
                    <asp:ListItem Value="true">VND</asp:ListItem>
                    <asp:ListItem Value="false">USD</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-1">
                Comission
            </div>
            <div class="col-xs-3 --no-padding-left">
                <asp:TextBox ID="txtCommission" runat="server" data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true"
                    CssClass="form-control" Style="display: inline-block; width: 39%"></asp:TextBox>
                <asp:DropDownList ID="ddlCurrencyComission" runat="server" CssClass="form-control" Style="display: inline-block; width: 25%">
                    <asp:ListItem Value="true">VND</asp:ListItem>
                    <asp:ListItem Value="false">USD</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-xs-1 --no-padding-right">
                Cancel penalty
            </div>
            <div class="col-xs-3 --no-padding-left">
                <asp:TextBox ID="txtPenalty" runat="server" Text="0" data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true"
                    CssClass="form-control" Style="display: inline-block; width: 71%"></asp:TextBox>
                <asp:DropDownList ID="ddlCurrencyCPenalty" runat="server" CssClass="form-control" Style="display: inline-block; width: 27%">
                    <asp:ListItem Value="true">VND</asp:ListItem>
                    <asp:ListItem Value="false">USD</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-xs-1">
                Guide collect
            </div>
            <div class="col-xs-3 --no-padding-left">
                <asp:TextBox ID="txtGuideCollect" runat="server" data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true" CssClass="form-control"
                    Style="display: inline-block; width: 71%"></asp:TextBox>
                <asp:DropDownList ID="ddlCurrencyGuideCollect" runat="server" CssClass="form-control"
                    Style="display: inline-block; width: 27%">
                    <asp:ListItem Value="true">VND</asp:ListItem>
                    <asp:ListItem Value="false">USD</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-1 --no-padding-right">
                Extra services
            </div>
            <div class="col-xs-11">
                <asp:PlaceHolder ID="plhDetailService" runat="server">
                    <asp:Repeater ID="rptExtraServices" runat="server" OnItemDataBound="rptExtraServices_ItemDataBound">
                        <ItemTemplate>
                            <asp:HiddenField ID="hiddenId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"Id") %>' />
                            <asp:CheckBox ID="chkService" runat="server" CssClass="checkbox" />
                        </ItemTemplate>
                    </asp:Repeater>
                    <a id="aServices" runat="server">[Nh?p giá]</a>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="plhAccountStatus" runat="server">
                    <%= base.GetText("textAccountingStatus") %>
                    <asp:Button ID="btnAccountingUpdate" runat="server" CssClass="button" OnClick="btnAccountingUpdate_Click" />
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="plhServices" runat="server">
                    <td colspan="4">
                        <asp:UpdatePanel ID="udpServices" runat="server">
                            <ContentTemplate>
                                <table style="width: auto;">
                                    <tr>
                                        <td>Tên d?ch v?
                                        </td>
                                        <td>Theo khách
                                        </td>
                                        <td>??n giá
                                        </td>
                                        <td>S? l??ng
                                        </td>
                                    </tr>
                                    <asp:Repeater ID="rptServices" runat="server" OnItemDataBound="rptServices_ItemDataBound"
                                        OnItemCommand="rptServices_ItemCommand">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:HiddenField ID="hiddenId" runat="server" />
                                                    <asp:DropDownList ID="ddlService" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkByCustomer" runat="server" CssClass="checkbox" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtUnitPrice" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtQuantity" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnRemove" runat="server" Text="Remove" CommandName="remove" CssClass="button" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAddService" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <asp:Button ID="btnAddService" runat="server" Text="Thêm" CssClass="button" OnClick="btnAddService_Click" />
                    </td>
                </asp:PlaceHolder>
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-8">
                <asp:Literal ID="litCreatedTime" runat="server"></asp:Literal>
                <asp:Literal ID="litLastEdited" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div id="pnlAirportTransfer">
                <br />
                <span style="font-size: 12px">Airport Transfer Information</span>
                <div class="basicinfo">
                    <table>
                        <tr>
                            <td>Pickup Time
                            </td>
                            <td>
                                <asp:TextBox ID="txtPickupTime" runat="server" />
                            </td>
                            <td>Pickup Address
                            </td>
                            <td>
                                <asp:TextBox ID="txtPUPickupAddress" runat="server" TextMode="MultiLine" />
                            </td>
                            <td>Dropoff Address
                            </td>
                            <td>
                                <asp:TextBox ID="txtPUDropoffAddress" runat="server" TextMode="MultiLine" />
                            </td>
                            <td>Flight Details
                            </td>
                            <td>
                                <asp:TextBox ID="txtPUFlightDetails" runat="server" TextMode="MultiLine" />
                            </td>
                            <td>Car Requirements
                            </td>
                            <td>
                                <asp:TextBox ID="txtPUCarRequirements" runat="server" TextMode="MultiLine" />
                            </td>
                        </tr>
                        <tr>
                            <td>Seeoff Time
                            </td>
                            <td>
                                <asp:TextBox ID="txtSeeoffTime" runat="server" />
                            </td>
                            <td>Pickup Address
                            </td>
                            <td>
                                <asp:TextBox ID="txtSOPickupAddress" runat="server" TextMode="MultiLine" />
                            </td>
                            <td>Dropoff Address
                            </td>
                            <td>
                                <asp:TextBox ID="txtSODropoffAddress" runat="server" TextMode="MultiLine" />
                            </td>
                            <td>Flight Details
                            </td>
                            <td>
                                <asp:TextBox ID="txtSOFlightDetails" runat="server" TextMode="MultiLine" />
                            </td>
                            <td>Car Requirements
                            </td>
                            <td>
                                <asp:TextBox ID="txtSOCarRequirements" runat="server" TextMode="MultiLine" />
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-4">
                Pickup address
            </div>
            <div class="col-xs-4">
                Special request
            </div>
            <div class="col-xs-4">
                Customer info
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-4">
                <asp:TextBox ID="txtPickup" runat="server" TextMode="MultiLine" CssClass="form-control" placeholder="Pickup address"></asp:TextBox>
            </div>
            <div class="col-xs-4">
                <asp:TextBox ID="txtSpecialRequest" runat="server" TextMode="MultiLine" CssClass="form-control" placeholder="Special request"></asp:TextBox>
            </div>
            <div class="col-xs-4">
                <asp:TextBox ID="txtCustomerInfo" runat="server" TextMode="MultiLine" CssClass="form-control" placeholder="Customer info"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-12">
                <asp:Button ID="btnInvoice" runat="server" Text="Invoice" CssClass="btn btn-primary" OnClick="btnInvoice_Click" />
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-12">
                <table class="table table-borderless table-common" style="margin-bottom: 0">
                    <asp:Repeater ID="rptCustomers" runat="server" OnItemDataBound="rptCustomers_ItemDataBound">
                        <HeaderTemplate>
                            <tr>
                                <th>Name
                                </th>
                                <th>Gender
                                </th>
                                <th>Type
                                </th>
                                <th>Birthday
                                </th>
                                <th>Nationality
                                </th>
                                <th>Visa No.
                                </th>
                                <th>Passport No.
                                </th>
                                <th>Visa Expired
                                </th>
                                <th style="width: 5%">Viet Kieu
                                </th>
                                <th></th>
                                <th></th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <uc:customer ID="customerData" runat="server"></uc:customer>
                                <td>
                                    <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" CssClass="btn btn-primary"
                                        Text="Delete" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-12">
                <asp:PlaceHolder ID="plhAddRoom" runat="server">
                    <asp:DropDownList ID="ddlRoomTypes" runat="server" Visible="false">
                    </asp:DropDownList>
                    <asp:Button ID="btnAddRoom" runat="server" OnClick="btnAddRoom_Click" Text="Add"
                        CssClass="btn btn-primary" />
                </asp:PlaceHolder>
            </div>
        </div>
    </div>
    <div>
        <fieldset>
            <div class="settinglist">
                <div class="basicinfo">
                </div>
            </div>

            <asp:PlaceHolder ID="plhEmo" runat="server">
                <input class="button" type="button" id="btnProvisionalDetail" runat="server" />
                <asp:Button ID="btnConfirmation" runat="server" Text="Confirmation export" CssClass="button"
                    OnClick="btnConfirmation_Click" />
            </asp:PlaceHolder>
            <div class="basicinfo">
                <table>
                </table>
            </div>

        </fieldset>
    </div>
</asp:Content>
<asp:Content runat="server" ID="Scripts" ContentPlaceHolderID="Scripts">
    <script>
        $(document).ready(function () {
            $("#<%= txtPickupTime.ClientID %>").datetimepicker({ datepicker: true, format: "d/m/Y H:i" });
            $("#<%= txtSeeoffTime.ClientID %>").datetimepicker({ datepicker: true, format: "d/m/Y H:i" });
        });
    </script>
    <script>
        $(document).ready(function () {
            if ($("#<%= cddlTrips.ClientID%> option:selected").text().toLowerCase() === 'airport transfer') {
                $("#pnlAirportTransfer").css({ "display": "block" });
            } else {
                $("#pnlAirportTransfer").css({ "display": "none" });
            }

            $("#<%= cddlTrips.ClientID%>").change(function () {
                if ($("#<%= cddlTrips.ClientID%> option:selected").text().toLowerCase() === 'airport transfer') {
                    $("#pnlAirportTransfer").css({ "display": "block" });
                } else {
                    $("#pnlAirportTransfer").css({ "display": "none" });
                }
            });
        })
    </script>
    <%if (Booking.LockStatus == "Locked")
      {%>
    <script>
        $("button:not(.undisabled)").attr("disabled", "true");
        $("input[type='text']:not(.undisabled)").attr("disabled", "true");
        $("input[type='checkbox']").attr("disabled", "true");
        $("input[type='radio']").attr("disabled", "true");
        $("input[type='submit']:not(.undisabled)").attr("disabled", "true");
        $("select").attr("disabled", "true");
        $("textArea").attr("disabled", "true");
    </script>
    <%}%>
    <%if (!CanEditTotal)
      {%>
    <script>
        $('[data-permission = "canedittotal"][data-locktype="disabled"]').attr({ "disabled": "disabled", "title": "You don't have allow add daily expense permission" })
        $('[data-permission = "canedittotal"][data-locktype="readonly"]').attr({ "readonly": "readonly", "title": "You don't have allow add daily expense permission" })
    </script>
    <% } %>
    <%if (!CanLockIncome)
      {%>
    <script>
        $('[data-permission = "canlockincome"][data-locktype="disabled"]').attr({ "disabled": "disabled", "title": "You don't have allow edit daily expense permission" })
        $('[data-permission = "canlockincome"][data-locktype="readonly"]').attr({ "readonly": "readonly", "title": "You don't have allow edit daily expense permission" })
    </script>
    <% } %>
    <script>
        $("#<%=cddlTrips.ClientID%>").change(function () {
            __doPostBack('<%= cddlTrips.ClientID %>')
        })
    </script>
    <script>
        $("#<%=txtDiscountAmount.ClientID%>").keyup(function () {
            __doPostBack('<%= txtDiscountAmount.ClientID %>');
        });
    </script>
    <script>
        $("#<%=txtDiscountPercent.ClientID%>").keyup(function () {
            __doPostBack('<%= txtDiscountPercent.ClientID %>');
         });
    </script>
    <script>
        $("#<%=txtTotal.ClientID%>").blur(function () {
            __doPostBack('<%= txtTotal.ClientID %>');
         });
    </script>
    <script>
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtTotal.ClientID%>").blur(function () {
                __doPostBack('<%= txtTotal.ClientID %>');
            });
            $("#<%=txtTotal.ClientID%>").inputmask({
                'alias': 'decimal',
                'groupSeperator': ',',
                'autoGroup': 'true',
            })
        });
    </script>
</asp:Content>
