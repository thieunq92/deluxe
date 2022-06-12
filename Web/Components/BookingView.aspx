<%@ Page Language="C#" MasterPageFile="SailsMaster.Master" AutoEventWireup="True"
    CodeBehind="BookingView.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.BookingView"
    Title="Untitled Page" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="uc" TagName="customer" Src="../Controls/CustomerInfoRowInput.ascx" %>
<%@ Register Assembly="Portal.Modules.OrientalSails" Namespace="Portal.Modules.OrientalSails.Web.Controls"
    TagPrefix="orc" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <script type="text/javascript">

        function getDropValue(controlid) {
            ddl = document.getElementById(controlid);
            return ddl.options[ddl.selectedIndex].value;
        }

        function Validate() {
            document.getElementById('ajaxloader').style.visibility = "visible";
            UseCallback();
        }
        function GetBookerPhone(result, context) {
            document.getElementById('<%= labelTelephone.ClientID %>').innerHTML = result;
            document.getElementById('ajaxloader').style.visibility = "hidden";
        }

        function toggleVisible(id) {
            item = document.getElementById(id);
            if (item.style.display == "") {
                item.style.display = "none";
            }
            else {
                item.style.display = "";
            }
        }

        function setVisible(id, visible) {
            control = document.getElementById(id);
            if (visible)
            { control.style.display = ""; }
            else {
                control.style.display = "none";
            }
        }

        function ddltype_changed(id, optionid, vids) {
            ddltype = document.getElementById(id);
            if (vids.indexOf('#' + ddltype.options[ddltype.selectedIndex].value + '#') >= 0) {
                setVisible(optionid, true);
            }
            else {
                setVisible(optionid, false);
            }
        }

        function ddlagency_changed(id, codeid) {
            ddltype = document.getElementById(id);
            switch (ddltype.selectedIndex) {
                case 0:
                    setVisible(codeid, false);
                    break;
                default:
                    setVisible(codeid, true);
                    break;
            }
        }

        function setDefault(chkid, txtid, customers) {
            chkCustomer = document.getElementById(chkid);
            txtQuantity = document.getElementById(txtid);
            if (chkCustomer.checked == 1) {
                txtQuantity.value = customers;
            }
            else {
                txtQuantity.value = "1";
            }
        }

        function serviceChanged(ddlid, chkid, txtid, customers, list) {
            ddlService = document.getElementById(ddlid);
            chkCustomer = document.getElementById(chkid);
            txtQuantity = document.getElementById(txtid);
            if (list.indexOf('#' + ddlService.options[ddlService.selectedIndex].value + '#') > 0) {
                chkCustomer.checked = 1;
                txtQuantity.value = customers;
            }
            else {
                chkCustomer.checked = 0;
                txtQuantity.value = "1";
            }
        }
    </script>
    <script>
        $(document).ready(function () {
            $(":input").inputmask();
        })
    </script>
    <script>
        $(document).ready(function () {
            $("#<%= txtPickupTime.ClientID %>").datetimepicker({ format: "d/m/Y H:i" });
            $("#<%= txtSeeoffTime.ClientID %>").datetimepicker({ format: "d/m/Y H:i" });
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
    <div>
        <fieldset>
            <legend>
                <img alt="Room" src="../Images/sails.gif" align="absMiddle" />
                <asp:Label ID="titleBookingView" runat="server"></asp:Label>
            </legend>
            <div class="settinglist">
                <table>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkSendMail" runat="server" Checked="false" CssClass="checkbox"
                                Visible="False" />
                        </td>
                        <td>
                            <asp:Button ID="buttonSubmit" runat="server" CssClass="button" OnClick="buttonSubmit_Click" />
                        </td>
                        <td>
                            <asp:Button ID="buttonCancel" runat="server" CssClass="button" OnClick="buttonCancel_Click" />
                        </td>
                        <td>
                            <input type="button" class="button" id="btnEmail" runat="server" value="Send mail" />
                        </td>
                        <td>
                            <input type="button" class="button" id="btnViewHistory" runat="server" value="View history" />
                        </td>
                    </tr>
                </table>
                <div class="search_panel">
                    <table>
                        <tr>
                            <td style="width: 12%">
                                <asp:Label ID="labelBookingId" runat="server"></asp:Label>
                            </td>
                            <td style="width: 26%">
                                <asp:Label ID="label_BookingId" runat="server"></asp:Label>
                                <asp:TextBox ID="txtBookingId" runat="server"></asp:TextBox>
                            </td>
                            <td style="width: 12%">
                                <asp:Label ID="labelTripName" runat="server"></asp:Label>
                            </td>
                            <td style="width: 26%">
                                <asp:DropDownList ID="ddlOrgs" runat="server" Width="80">
                                </asp:DropDownList>
                                <svc:CascadingDropDown runat="server" ID="cddlTrips" />
                                <%--                            <asp:DropDownList ID="ddlTrips" runat="server">
                            </asp:DropDownList>--%>
                                <asp:DropDownList ID="ddlOptions" runat="server" Width="100">
                                    <asp:ListItem>Option 1</asp:ListItem>
                                    <asp:ListItem>Option 2</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 12%">
                            </td>
                            <td style="width: 12%">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="labelStartDate" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                                <ajax:CalendarExtender ID="calendarDate" runat="server" TargetControlID="txtStartDate"
                                    Format="dd/MM/yyyy">
                                </ajax:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="labelStatus" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatusType" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <%= base.GetText("textNumberOfPax") %>
                            </td>
                            <td>
                                <asp:Literal ID="litPax" runat="server"></asp:Literal>
                                <asp:Image ImageUrl="../Images/info.png" ID="imgPax" runat="server" ImageAlign="AbsMiddle" />
                                <ajax:HoverMenuExtender ID="hmeNote" runat="Server" HoverCssClass="popupHover" PopupControlID="PopupMenu"
                                    PopupPosition="Right" TargetControlID="imgPax" PopDelay="25" />
                                <asp:Panel CssClass="hover_content" ID="PopupMenu" runat="server" Style="width: auto">
                                    <asp:Literal ID="litPaxDetail" runat="server"></asp:Literal>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%= base.GetText("textAgency") %>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlAgencies" runat="server" Width="120">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtAgencyCode" runat="server" Width="75"></asp:TextBox>
                                <ajax:TextBoxWatermarkExtender ID="waterAgency" runat="server" TargetControlID="txtAgencyCode"
                                    WatermarkText="TA Code">
                                </ajax:TextBoxWatermarkExtender>
                            </td>
                            <td>
                                <%= base.GetText("textBooker") %>
                            </td>
                            <td>
                                <svc:CascadingDropDown ID="cddlBooker" runat="server">
                                </svc:CascadingDropDown>
                            </td>
                            <td>
                                <img id="ajaxloader" src="/images/loading.gif" alt="loading" style="visibility: hidden;
                                    height: 12px;" />
                                <asp:Label ID="labelTelephone" runat="server"></asp:Label>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%= base.GetText("textCreated") %>
                            </td>
                            <td>
                                <asp:Literal ID="litCreated" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <%= base.GetText("textBookingDate")%>
                            </td>
                            <td>
                                <asp:HyperLink ID="hplBookingDate" runat="server"></asp:HyperLink>
                            </td>
                            <td>
                                <%= base.GetText("textApprovedBy") %>
                            </td>
                            <td>
                                <asp:Literal ID="litApprovedBy" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%= base.GetText("textTotal") %>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="udpTotal" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtTotal" runat="server" data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true"></asp:TextBox>
                                        <asp:DropDownList ID="ddlCurrency" runat="server" Width="60">
                                            <asp:ListItem Value="true">VND</asp:ListItem>
                                            <asp:ListItem Value="false">USD</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:LinkButton ID="lbtCalculate" runat="server" OnClick="lbtCalculate_Click"></asp:LinkButton>
                                        <asp:Button runat="server" ID="btnLockIncome" CssClass="button" Text="Lock booking"
                                            OnClick="btnLockIncome_Click" />
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
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="lbtCalculate" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                Guide collect:
                            </td>
                            <td>
                                <asp:TextBox ID="txtGuideCollect" runat="server" data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true"></asp:TextBox>
                                <asp:DropDownList ID="ddlCurrencyGuideCollect" runat="server" Width="60">
                                    <asp:ListItem Value="true">VND</asp:ListItem>
                                    <asp:ListItem Value="false">USD</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <%= base.GetText("textPaymentNeeded") %>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsPaymentNeeded" runat="server" CssClass="checkbox" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Commission
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtCommission" runat="server" data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true"></asp:TextBox>
                                        <asp:DropDownList ID="ddlCurrencyComission" runat="server" Width="60">
                                            <asp:ListItem Value="true">VND</asp:ListItem>
                                            <asp:ListItem Value="false">USD</asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="lbtCalculate" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <!--<td>Invoice</td>
                        <td><asp:CheckBox ID="chkInvoice" runat="server" /></td>                        -->
                            <td>
                                Cancel penalty
                            </td>
                            <td>
                                <asp:TextBox ID="txtPenalty" runat="server" Text="0" data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true"></asp:TextBox>
                                <asp:DropDownList ID="ddlCurrencyCPenalty" runat="server" Width="60">
                                    <asp:ListItem Value="true">VND</asp:ListItem>
                                    <asp:ListItem Value="false">USD</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%= base.GetText("textExtraService") %>
                            </td>
                            <td colspan="3">
                                <asp:PlaceHolder ID="plhDetailService" runat="server">
                                    <asp:Repeater ID="rptExtraServices" runat="server" OnItemDataBound="rptExtraServices_ItemDataBound">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hiddenId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"Id") %>' />
                                            <asp:CheckBox ID="chkService" runat="server" CssClass="checkbox" />
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <a id="aServices" runat="server">[Nh?p giá]</a> </asp:PlaceHolder>
                            </td>
                            <asp:PlaceHolder ID="plhAccountStatus" runat="server">
                                <td>
                                    <%= base.GetText("textAccountingStatus") %>
                                </td>
                                <td>
                                    <asp:Button ID="btnAccountingUpdate" runat="server" CssClass="button" OnClick="btnAccountingUpdate_Click" />
                                </td>
                            </asp:PlaceHolder>
                        </tr>
                        <asp:PlaceHolder ID="plhServices" runat="server">
                            <tr>
                                <td colspan="4">
                                    <asp:UpdatePanel ID="udpServices" runat="server">
                                        <ContentTemplate>
                                            <table style="width: auto;">
                                                <tr>
                                                    <td>
                                                        Tên d?ch v?
                                                    </td>
                                                    <td>
                                                        Theo khách
                                                    </td>
                                                    <td>
                                                        ??n giá
                                                    </td>
                                                    <td>
                                                        S? l??ng
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
                                                                <%--<asp:TextBox ID="txtName" runat="server"></asp:TextBox>--%>
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
                            </tr>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="plhCharter" runat="server">
                            <tr>
                                <td>
                                    <%= base.GetText("textBookingCharter") %>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkCharter" runat="server" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%= base.GetText("textIsTransferred") %>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIsTransferred" runat="server" />
                                </td>
                                <td>
                                    <%= base.GetText("textTransferTo") %>
                                </td>
                                <td>
                                    <orc:AgencySelector ID="agencyTransferTo" runat="server" />
                                </td>
                                <td>
                                    <%= base.GetText("textTransferredCost") %>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTransferCost" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trTransferCustomer" runat="server">
                                <td>
                                    <%= base.GetText("textTransferAdult") %>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTransferAdult" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <%= base.GetText("textTransferChildren") %>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTransferChildren" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <%= base.GetText("textTransferBaby") %>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTransferBaby" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </asp:PlaceHolder>
                        <tr>
                            <td colspan="6">
                                <asp:Literal ID="litCreatedTime" runat="server"></asp:Literal>
                                <asp:Literal ID="litLastEdited" runat="server"></asp:Literal>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="pnlAirportTransfer">
                    <br />
                    <span style="font-size: 12px">Airport Transfer Information</span>
                    <div class="basicinfo">
                        <table>
                            <tr>
                                <td>
                                    Pickup Time
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPickupTime" runat="server" />
                                </td>
                                <td rowspan="2">
                                    Flight Details
                                </td>
                                <td rowspan="2">
                                    <asp:TextBox ID="txtFlightDetails" runat="server" TextMode="MultiLine" />
                                </td>
                                <td rowspan="2">
                                    Car Type
                                </td>
                                <td rowspan="2">
                                    <asp:TextBox ID="txtCartype" runat="server" TextMode="MultiLine" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Seeoff Time
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSeeoffTime" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                </div>
                <div class="basicinfo">
                    <table cellspacing="3">
                        <tr>
                            <td style="width: 30%">
                                <%= base.GetText("textPickupAddress") %>
                            </td>
                            <td style="width: 20%">
                                <%= base.GetText("textSpecialRequest") %>
                            </td>
                            <td style="width: 50%">
                                <%= base.GetText("textCustomerInfo") %>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtPickup" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSpecialRequest" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCustomerInfo" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <asp:Button ID="btnInvoice" runat="server" Text="Invoice" CssClass="button" OnClick="btnInvoice_Click" />
            <asp:PlaceHolder ID="plhEmo" runat="server">
                <input class="button" type="button" id="btnProvisionalDetail" runat="server" />
                <asp:Button ID="btnConfirmation" runat="server" Text="Confirmation export" CssClass="button"
                    OnClick="btnConfirmation_Click" />
            </asp:PlaceHolder>
            <div class="basicinfo">
                <table>
                    <asp:Repeater ID="rptCustomers" runat="server" OnItemDataBound="rptCustomers_ItemDataBound">
                        <HeaderTemplate>
                            <tr>
                                <th>
                                    Name
                                </th>
                                <th>
                                    Sex
                                </th>
                                <th>
                                    Type
                                </th>
                                <th>
                                    Birthday
                                </th>
                                <th>
                                </th>
                                <th>
                                    Nationality
                                </th>
                                <th>
                                    Visa No.
                                </th>
                                <th>
                                    Passport No.
                                </th>
                                <th>
                                    Visa Expired
                                </th>
                                <th>
                                    Viet Kieu
                                </th>
                                <th>
                                </th>
                                <th>
                                </th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <uc:customer ID="customerData" runat="server"></uc:customer>
                                <td>
                                    <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" CssClass="button"
                                        Text="Delete" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
            <asp:PlaceHolder ID="plhAddRoom" runat="server">
                <asp:DropDownList ID="ddlRoomTypes" runat="server" Visible="false">
                </asp:DropDownList>
                <asp:Button ID="btnAddRoom" runat="server" OnClick="btnAddRoom_Click" Text="Add"
                    CssClass="button" />
            </asp:PlaceHolder>
        </fieldset>
    </div>
</asp:Content>
