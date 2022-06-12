<%@ Page Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true"
    CodeBehind="BookingReport.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.BookingReport" %>

<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<%@ Import Namespace="Portal.Modules.OrientalSails" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Booking by date</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="sticky" style="z-index: 999; background-color: #ffffff">
        <div class="row">
            <div class="col-xs-12">
                <table class="table table-borderless" style="margin-bottom: -5px">
                    <tr>
                        <td style="width: 7%; padding-left: 0">Date to view
                        </td>
                        <td style="width: 15%; padding-left: 0">
                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control undisabled" data-control="datetimepicker"></asp:TextBox>
                        </td>
                        <td style="width: 6%; padding-left: 0">
                            <asp:Button ID="btnDisplay" runat="server" Text="Display" OnClick="btnDisplay_Click"
                                CssClass="btn btn-primary undisabled" />
                        </td>
                        <td style="padding-left: 0;width:10%">
                            <asp:TextBox runat="server" ID="txtSearchCode" placeholder="Booking code or TA code" OnTextChanged="txtSearchCode_TextChanged" CssClass="form-control --width-auto"></asp:TextBox></td>
                        <td>
                            <asp:Repeater ID="rptOrganization" runat="server" OnItemDataBound="rptOrganization_ItemDataBound">
                                <HeaderTemplate>
                                    <asp:HyperLink ID="hplOrganization" runat="server" class="btn btn-default">
                                All
                                    </asp:HyperLink>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="btn-group">
                                        <asp:HyperLink ID="hplOrganization" runat="server" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
                                <span class="caret"></span>
                                        </asp:HyperLink>
                                        <ul class="dropdown-menu">
                                            <asp:Repeater ID="rptTrips" runat="server" OnItemDataBound="rptTrips_ItemDataBound">
                                                <HeaderTemplate>
                                                    <li id="liMenu" runat="server">
                                                        <a class="btn btn-default has-customer" href="BookingReport.aspx?NodeId=1&SectionId=15&Date=<%= (Request.QueryString["Date"] != null ? DateTime.FromOADate(Convert.ToDouble(Request.QueryString["Date"])) : DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).ToOADate()%>&orgid=<%# ((Portal.Modules.OrientalSails.Domain.Organization)((RepeaterItem)Container.Parent.Parent).DataItem).Id%>">All</a>
                                                    </li>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <li id="liMenu" runat="server">
                                                        <asp:HyperLink ID="hplTrip" runat="server"></asp:HyperLink>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-12 btn-grid">
                    <asp:Repeater ID="rptTrips" runat="server" OnItemDataBound="rptTrips_ItemDataBound">
                        <HeaderTemplate>
                            <a class="btn btn-default <%= Request.QueryString["tripid"] == null ? "active" :"" %>" href="BookingReport.aspx?NodeId=1&SectionId=15&Date=<%= (Request.QueryString["Date"] != null ? DateTime.FromOADate(Convert.ToDouble(Request.QueryString["Date"])) : DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)).ToOADate()%>">All</a>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:HyperLink ID="hplTrip" runat="server" CssClass="btn btn-default" style="border:none"></asp:HyperLink>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-12">
                <table class="table table-bordered table-common">
                    <asp:Repeater ID="rptBookingList" runat="server" OnItemDataBound="rptBookingList_ItemDataBound"
                        OnItemCommand="rptBookingList_ItemCommand">
                        <HeaderTemplate>
                            <tr class="active">
                                <th rowspan="2">No.
                                </th>
                                <th rowspan="2">Name of pax</th>
                                <th rowspan="2" style="width:4%">Group
                                </th>
                                <th colspan="3">Number of pax
                                </th>
                                <th rowspan="2" id="thTrip" runat="server">Trip
                                </th>
                                <th rowspan="2" id="thTime" runat="server">Time
                                </th>
                                <th rowspan="2">Pickup address
                                </th>
                                <th rowspan="2" id="thDropoffAddress" runat="server">Dropoff Address
                                </th>
                                <th rowspan="2" id="thFlightDetails" runat="server">Flight Details
                                </th>
                                <th rowspan="2" id="thCarRequirements" runat="server">Car Type
                                </th>
                                <th rowspan="2" id="thSpecialRequest" runat="server">Special request
                                </th>
                                <asp:PlaceHolder ID="plhHidden" runat="server" Visible="false">
                                    <th colspan="2">
                                        <%# base.GetText("textNumberOfCabin") %>
                                    </th>
                                    <th colspan="2"></th>
                                </asp:PlaceHolder>
                                <th rowspan="2">Agency
                                </th>
                                <th rowspan="2">Booking code
                                </th>
                                <th rowspan="2">Total
                                </th>
                                <th rowspan="2">Feedback</th>
                            </tr>
                            <tr class="active">
                                <th>Adult
                                </th>
                                <th>Child
                                </th>
                                <th>Baby
                                </th>
                                <asp:PlaceHolder ID="plhHidden2" runat="server" Visible="false">
                                    <th>Double
                                    </th>
                                    <th>Twin
                                    </th>
                                    <th>Adult
                                    </th>
                                    <th>Child
                                    </th>
                                </asp:PlaceHolder>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="item" id="trItem" runat="server">
                                <td>
                                    <asp:Literal ID="litIndex" runat="server"></asp:Literal>
                                    <asp:HiddenField ID="hiddenId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                </td>
                                <td class="--text-left">
                                    <asp:Label ID="label_NameOfPax" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlGroup" runat="server" CssClass="form-control" Style="width:auto">
                                    </asp:DropDownList>
                                    <asp:Literal ID="litGroup" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Label ID="label_NoOfAdult" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="label_NoOfChild" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="label_NoOfBaby" runat="server"></asp:Label>
                                </td>
                                <td id="tdTrip" runat="server">
                                    <asp:Label ID="labelItinerary" runat="server"></asp:Label>
                                </td>
                                <td id="tdTime" runat="server">
                                    <asp:Literal ID="ltrTime" runat="server" />
                                </td>
                                <td class="--text-left">
                                    <asp:Label ID="labelPuAddress" runat="server"></asp:Label>
                                </td>
                                <td id="tdDropoffAddress" runat="server">
                                    <asp:Literal ID="ltrDropoffAddress" runat="server"></asp:Literal>
                                </td>
                                <td id="tdFlightDetails" runat="server">
                                    <asp:Literal ID="ltrFlightDetails" runat="server" />
                                </td>
                                <td id="tdCarRequirements" runat="server">
                                    <asp:Literal ID="ltrCarRequirements" runat="server" />
                                </td>
                                <td id="tdSpecialRequest" runat="server" class="--text-left">
                                    <asp:Label ID="labelSpecialRequest" runat="server"></asp:Label>
                                </td>

                                <td id="hidden1" runat="server" visible="false">
                                    <asp:Label ID="label_NoOfDoubleCabin" runat="server"></asp:Label>
                                </td>
                                <td id="hidden2" runat="server" visible="false">
                                    <asp:Label ID="label_NoOfTwinCabin" runat="server"></asp:Label>
                                </td>
                                <td id="hidden3" runat="server" visible="false">
                                    <asp:Label ID="label_NoOfTransferAdult" runat="server"></asp:Label>
                                </td>
                                <td id="hidden4" runat="server" visible="false">
                                    <asp:Label ID="label_NoOfTransferChild" runat="server"></asp:Label>
                                </td>
                                <td class="--text-left">
                                    <asp:HyperLink ID="hyperLink_Partner" runat="server"></asp:HyperLink>
                                </td>
                                <td>
                                    <asp:HyperLink ID="hplCode" runat="server"></asp:HyperLink>
                                </td>
                                <td class="--text-right">
                                    <asp:Label ID="label_TotalPrice" runat="server"></asp:Label>
                                </td>
                                <td id="tdFeedback" runat="server">
                                    <a id="anchorFeedback" runat="server" style="cursor: pointer;">Feedback</a>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr class="item">
                                <td colspan="3">GRAND TOTAL
                                </td>
                                <td>
                                    <strong>
                                        <asp:Label ID="label_NoOfAdult" runat="server"></asp:Label></strong>
                                </td>
                                <td>
                                    <strong>
                                        <asp:Label ID="label_NoOfChild" runat="server"></asp:Label></strong>
                                </td>
                                <td>
                                    <strong>
                                        <asp:Label ID="label_NoOfBaby" runat="server"></asp:Label></strong>
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <asp:PlaceHolder ID="plhHidden" runat="server" Visible="false">
                                    <td>
                                        <strong>
                                            <asp:Label ID="label_NoOfDoubleCabin" runat="server"></asp:Label></strong>
                                    </td>
                                    <td>
                                        <strong>
                                            <asp:Label ID="label_NoOfTwinCabin" runat="server"></asp:Label></strong>
                                    </td>
                                    <td>
                                        <strong>
                                            <asp:Label ID="label_NoOfTransferAdult" runat="server"></asp:Label></strong>
                                    </td>
                                    <td>
                                        <strong>
                                            <asp:Label ID="label_NoOfTransferChild" runat="server"></asp:Label></strong>
                                    </td>
                                </asp:PlaceHolder>
                                <td></td>
                                <td></td>
                                <td>
                                    <strong>
                                        <asp:Label ID="label_TotalPrice" runat="server"></asp:Label></strong>
                                </td>
                            </tr>
                        </FooterTemplate>
                    </asp:Repeater>
                    <asp:Repeater runat="server" ID="rptShadows" OnItemDataBound="rptBookingList_ItemDataBound">
                        <HeaderTemplate>
                            <tr>
                                <td colspan="100%" style="background-color: #FF7F7F; color: #ffffff">Booking moved (to another date) or cancelled need attention (within 2 days)
                                </td>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="item" id="trItem" runat="server">
                                <td>
                                    <asp:Literal ID="litIndex" runat="server"></asp:Literal>
                                    <asp:HiddenField ID="hiddenId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                </td>
                                <td class="--text-left">
                                    <asp:Label ID="label_NameOfPax" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlGroup" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:Literal ID="litGroup" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Label ID="label_NoOfAdult" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="label_NoOfChild" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="label_NoOfBaby" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="labelItinerary" runat="server"></asp:Label>
                                </td>
                                <td class="--text-left">
                                    <asp:Label ID="labelPuAddress" runat="server"></asp:Label>
                                </td>
                                <td class="--text-left">
                                    <asp:Label ID="labelSpecialRequest" runat="server"></asp:Label>
                                </td>
                                <td id="hidden1" runat="server" visible="false">
                                    <asp:Label ID="label_NoOfDoubleCabin" runat="server"></asp:Label>
                                </td>
                                <td id="hidden2" runat="server" visible="false">
                                    <asp:Label ID="label_NoOfTwinCabin" runat="server"></asp:Label>
                                </td>
                                <td id="hidden3" runat="server" visible="false">
                                    <asp:Label ID="label_NoOfTransferAdult" runat="server"></asp:Label>
                                </td>
                                <td id="hidden4" runat="server" visible="false">
                                    <asp:Label ID="label_NoOfTransferChild" runat="server"></asp:Label>
                                </td>
                                <td class="--text-left">
                                    <asp:HyperLink ID="hyperLink_Partner" runat="server"></asp:HyperLink>
                                </td>
                                <td>
                                    <asp:HyperLink ID="hplCode" runat="server"></asp:HyperLink>
                                </td>
                                <td class="--text-right">
                                    <asp:Label ID="label_TotalPrice" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <a id="anchorFeedback" runat="server" style="cursor: pointer;">Feedback</a>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <asp:PlaceHolder ID="plhDailyExpenses" runat="server">
                <table class="table table-borderless table-common">
                    <asp:PlaceHolder ID="plhOperator" runat="server">
                        <tr>
                            <td colspan="2">Điều hành
                            </td>
                            <td colspan="6">Tên
                                <asp:DropDownList ID="ddlOperators" runat="server" CssClass="form-control" Style="display: inline-block; width: 18%">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtOperator" runat="server" ReadOnly="true" CssClass="form-control" Style="display: inline-block; width: auto"></asp:TextBox>
                                Điện thoại
                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" Style="display: inline-block; width: auto"></asp:TextBox>
                                Sale in charge
                                <asp:DropDownList ID="ddlSaleInCharge" runat="server" CssClass="form-control" Style="display: inline-block; width: auto">
                                </asp:DropDownList>
                                Tel. No.
                                <asp:TextBox ID="txtSalePhone" runat="server" ReadOnly="true" CssClass="form-control" Style="display: inline-block; width: auto"></asp:TextBox>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <tr>
                        <td></td>
                        <td></td>
                        <td class="--text-bold">Supplier
                        </td>
                        <td class="--text-bold">Name
                        </td>
                        <td class="--text-bold">Phone
                        </td>
                        <td class="--text-bold">Cost
                        </td>
                        <td></td>
                    </tr>
                    <asp:Repeater ID="rptCruiseExpense" runat="server" OnItemDataBound="rptCruiseExpense_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td colspan="8" class="--text-left">
                                    <asp:HiddenField ID="hiddenId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
                                    <h5 class="--text-bold">
                                        <%# DataBinder.Eval(Container.DataItem,"Name") %></h5>
                                </td>
                            </tr>
                            <asp:Repeater ID="rptServices" runat="server" OnItemDataBound="rptServices_ItemDataBound">
                                <ItemTemplate>
                                    <tr id="seperator" runat="server" class="seperator" visible="false">
                                        <td colspan="8" style="border-bottom: solid 1px black;">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td class="--text-left --no-padding-left">
                                            <asp:HiddenField ID="hiddenId" runat="server" />
                                            <asp:HiddenField ID="hiddenType" runat="server" />
                                            <asp:Literal ID="litType" runat="server"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSuppliers" runat="server" data-permission="caneditexpense" data-locktype="disabled" CssClass="form-control">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 30%">
                                            <asp:TextBox ID="txtName" runat="server" data-permission="caneditexpense" data-locktype="disabled" CssClass="form-control"
                                                placeholder="Name"></asp:TextBox>
                                            <asp:DropDownList ID="ddlGuides" runat="server" data-permission="caneditexpense" data-locktype="disabled" CssClass="form-control">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPhone" runat="server" data-permission="caneditexpense" data-locktype="readonly" CssClass="form-control"
                                                data-control="phoneinputmask" placeholder="Phone"></asp:TextBox>
                                        </td>
                                        <td style="width: 10%">
                                            <asp:TextBox ID="txtCost" runat="server" data-permission="caneditexpense" data-locktype="readonly" CssClass="form-control"
                                                data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true"></asp:TextBox>
                                        </td>
                                        <td style="width: 6%">
                                            <asp:DropDownList ID="ddlGroups" runat="server" Visible="true" data-permission="caneditexpense" data-locktype="disabled" CssClass="form-control">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width:15%">
                                            <asp:Button ID="btnRemove" runat="server" Text='Remove'
                                                CssClass="btn btn-primary" OnClick="btnRemove_Click" data-permission="candeleteexpense" data-locktype="disabled" />
                                            <asp:Button runat="server" CssClass="btn btn-primary" ID="btnExpenseHistory" Text="History"
                                                OnClientClick='<%# Eval("Id","window.open(\"ExpenseHistory.aspx?NodeId=1&SectionId=15&ei={0}\",\"Expense History\",\"width=1000,height=1000\");return false;") %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <tr>
                                <td colspan="100%" class="--text-left">
                                    <asp:Button ID="btnAddServiceBlock" Text="Add block" CssClass="btn btn-primary" runat="server"
                                        OnClick="btnAddServiceBlock_Click" data-permission="canaddexpense" data-locktype="disabled" />
                                    <asp:Repeater ID="rptAddServices" runat="server" OnItemDataBound="rtpAddServices_ItemDataBound"
                                        Visible="true">
                                        <ItemTemplate>
                                            <asp:Button ID="btnAddService" runat="server" OnClick="btnAddService_Click" CssClass="btn btn-primary"
                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' data-permission="canaddexpense" data-locktype="disabled" />
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr>
                                <td colspan="5" class="--text-left">Tổng
                                </td>
                                <td class="--text-right">
                                    <strong>
                                        <asp:Literal ID="litTotal" runat="server"></asp:Literal></strong>
                                </td>
                            </tr>
                        </FooterTemplate>
                    </asp:Repeater>
                </table>
            </asp:PlaceHolder>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <asp:Button ID="btnSaveExpenses" runat="server" Text="Save without export" OnClick="btnSaveExpenses_Click"
                CssClass="btn btn-primary" />
            <asp:Button ID="btnExport" runat="server" Text="Export tour" OnClick="btnExport_Click"
                CssClass="btn btn-primary undisabled" />
            <asp:Button ID="btnIncomeDate" runat="server" Text="Xuất doanh thu" OnClick="btnIncomeDate_Click"
                Visible="false" CssClass="btn btn-primary undisabled" />
            <asp:Button ID="btnExportRoom" runat="server" Text="Export room list" OnClick="btnExportRoom_Click"
                Visible="false" CssClass="btn btn-primary undisabled" />
            <asp:Button ID="btnExportWelcome" runat="server" Text="Export Welcome board" OnClick="btnExportWelcome_Click"
                CssClass="btn btn-primary undisabled" />
            <asp:Button ID="btnExcel" runat="server" Text="Export customer data" OnClick="btnExcel_Click"
                CssClass="btn btn-primary undisabled" />
            <asp:Button ID="btnProvisional" runat="server" Text="Export provisional register"
                Visible="false" OnClick="btnProvisional_Click" CssClass="btn btn-primary undisabled" />
            <%if (LockOrUnlock == "Unlock")
              {%>
            <asp:Button ID="btnLockDate" runat="server" Text="Lock Date" OnClick="btnLockDate_Click"
                CssClass="btn btn-primary undisabled" />
            <%}%>
            <%if (LockOrUnlock == "Lock")
              {%>
            <asp:Button ID="btnUnlockDate" runat="server" Text="Unlock Date" OnClick="btnUnlockDate_Click"
                CssClass="btn btn-primary undisabled" />
            <%}%>
        </div>
    </div>
    <fieldset>
        <svc:Popup ID="popupManager" runat="server">
        </svc:Popup>
        <input type="button" id="btnComment" runat="server" value="View Comment" class="button"
            visible="false" />
        <div class="data_table">
            <div class="data_grid">
            </div>
        </div>
        <div class="basicinfo">
        </div>

    </fieldset>
</asp:Content>
<asp:Content ID="Scripts" ContentPlaceHolderID="Scripts" runat="server">
    <script type="text/javascript">
        function addCommas(nStr) {
            nStr = nStr.replace(',', '');
            nStr += '';
            x = nStr.split('.');
            x1 = x[0];
            x2 = x.length > 1 ? '.' + x[1] : '';
            var rgx = /(\d+)(\d{3})/;
            while (rgx.test(x1)) {
                x1 = x1.replace(rgx, '$1' + ',' + '$2');
            }
            return x1 + x2;
        }
    </script>
    <%if (!Module.PermissionCheck(Permission.AllowLockDate, UserIdentity))
      {%>
    <script>
        $("#<%= btnLockDate.ClientID %>").attr({ "disabled": "disabled", "title": "You don't have allow lock date permission" });
    </script>
    <%}%>
    <%if (!Module.PermissionCheck(Permission.AllowUnlockDate, UserIdentity))
      {%>
    <script>
        $("#<%= btnUnlockDate.ClientID %>").attr({ "disabled": "disabled", "title": "You don't have allow unlock date permission" });
    </script>
    <% } %>
    <%if (!CanAddExpense)
      {%>
    <script>
        $('[data-permission = "canaddexpense"][data-locktype="disabled"]').attr({ "disabled": "disabled", "title": "You don't have allow add daily expense permission" })
        $('[data-permission = "canaddexpense"][data-locktype="readonly"]').attr({ "readonly": "readonly", "title": "You don't have allow add daily expense permission" })
    </script>
    <% } %>
    <%if (!CanEditExpense)
      {%>
    <script>
        $('[data-permission = "caneditexpense"][data-locktype="disabled"]').attr({ "disabled": "disabled", "title": "You don't have allow edit daily expense permission" })
        $('[data-permission = "caneditexpense"][data-locktype="readonly"]').attr({ "readonly": "readonly", "title": "You don't have allow edit daily expense permission" })
    </script>
    <% } %>
    <%if (!CanDeleteExpense)
      {%>
    <script>
        $('[data-permission = "candeleteexpense"][data-locktype="disabled"]').attr({ "disabled": "disabled", "title": "You don't have allow delete daily expense permission" })
        $('[data-permission = "candeleteexpense"][data-locktype="readonly"]').attr({ "readonly": "readonly", "title": "You don't have allow delete daily expense permission" })
    </script>
    <% } %>
</asp:Content>
