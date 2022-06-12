<%@ Page Language="C#" MasterPageFile="Popup.Master" AutoEventWireup="true" CodeBehind="BookingPayment.aspx.cs"
    Inherits="Portal.Modules.OrientalSails.Web.Admin.BookingPayment" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="basicinfo">
        <table>
            <asp:PlaceHolder ID="plhOneBooking" runat="server">
                <tr>
                    <td style="width: 30%">
                        Booking ID
                    </td>
                    <td style="width: 70%">
                        <asp:Literal ID="litBookingCode" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td>
                        Agency
                    </td>
                    <td>
                        <asp:Literal ID="litAgency" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td>
                        Start date
                    </td>
                    <td>
                        <asp:Literal ID="litStartDate" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td>
                        Service
                    </td>
                    <td>
                        <asp:Literal ID="litService" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td>
                        Revenue
                    </td>
                    <td>
                        <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                        USD
                    </td>
                </tr>
                <tr>
                    <td>
                        Guide collect
                    </td>
                    <td>
                        <asp:Literal ID="litGuideCollect" runat="server"></asp:Literal>
                        USD
                    </td>
                </tr>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="plhMultiAgency" runat="server" Visible="false">
                <tr>
                    <td colspan="2">
                        <asp:Literal ID="litBookings" runat="server"></asp:Literal><br />
                        <!--<i>Leave applied rate blank if you don't want to change all applied rates</i>-->
                    </td>
                </tr>
            </asp:PlaceHolder>
            <tr>
                <td colspan="2">
                    <ajax:TabContainer runat="server" ID="tabContainer">
                        <ajax:TabPanel runat="server" ID="tabBooking" HeaderText="Partner receivable">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 30%">
                                            Remained
                                        </td>
                                        <td style="width: 70%">
                                            <asp:Literal ID="litRemainUSD" runat="server"></asp:Literal>
                                            USD (<asp:Literal ID="litRemainVND" runat="server"></asp:Literal>
                                            VND)
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Applied rate
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAppliedRate" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
<%--                                    <tr>
                                        <td style="width: 30%">
                                            Receivable:
                                        </td>
                                        <td style="width: 70%">
                                            <asp:Literal ID="litAgencyReceivable" runat="server"></asp:Literal>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td>
                                            USD paid
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPaid" runat="server">0</asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            VND paid
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPaidBase" runat="server">0</asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trNote" runat="server">
                                        <td>Note</td>
                                        <td><asp:TextBox ID="txtNote" runat="server"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Mark as paid
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkPaid" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: center;">
                                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" OnClick="btnSave_Click" />
                                            <asp:Button ID="btnConfirm" runat="server" Text="Confirm" CssClass="button" OnClick="btnConfirm_Click" />
                                            <input id="reset" type="button" value="Reset" class="button" onclick="window.location = window.location" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </ajax:TabPanel>
                        <ajax:TabPanel runat="server" ID="tabGuide" HeaderText="Guide collect">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 30%">
                                            Remained
                                        </td>
                                        <td style="width: 70%">
                                            <asp:Literal ID="litGuideUSD" runat="server"></asp:Literal>
                                            USD (<asp:Literal ID="litGuideVND" runat="server"></asp:Literal>
                                            VND)
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Applied rate
                                        </td>
                                        <td>
                                            <asp:Literal ID="litRate" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            USD paid
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtGuideUSDPaid" runat="server">0</asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            VND paid
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtGuideVNDPaid" runat="server">0</asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Mark as paid
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkGuidePaid" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: center;">
                                            <asp:Button ID="btnSaveGuide" runat="server" Text="Save" CssClass="button" OnClick="btnSaveGuide_Click" />
                                            <asp:Button ID="btnConfirmGuide" runat="server" Text="Confirm" CssClass="button" OnClick="btnConfirmGuide_Click" />
                                            <input id="btnResetGuide" type="button" value="Reset" class="button" onclick="window.location = window.location" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </ajax:TabPanel>
                        <ajax:TabPanel runat="server" ID="tabDriver" HeaderText="Driver collect">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 30%">
                                            Remained
                                        </td>
                                        <td style="width: 70%">
                                            <asp:Literal ID="litDriverUSD" runat="server"></asp:Literal>
                                            USD (<asp:Literal ID="litDriverVND" runat="server"></asp:Literal>
                                            VND)
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Applied rate
                                        </td>
                                        <td>
                                            <asp:Literal ID="litDriverRate" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            USD paid
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDriverUSDPaid" runat="server">0</asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            VND paid
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDriverVNDPaid" runat="server">0</asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Mark as paid
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkDriverPaid" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: center;">
                                            <asp:Button ID="btnSaveDriver" runat="server" Text="Save" CssClass="button" OnClick="btnSaveDriver_Click" />
                                            <input id="btnResetDriver" type="button" value="Reset" class="button" onclick="window.location = window.location" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </ajax:TabPanel>
                    </ajax:TabContainer>
                </td>
            </tr>
        </table>
        <asp:PlaceHolder ID="plhHistory" runat="server">
        <h4>Payment history</h4>
        <table> 
        <tr>
            <th rowspan="2">Time</th>
            <th rowspan="2">Pay by</th>            
            <th colspan="2">Amount</th>
            <th rowspan="2">Created by</th>
            <th rowspan="2">Note</th>
        </tr>  
        <tr>
            <th>USD</th>
            <th>VND</th>
        </tr>     
        <asp:Repeater ID="rptPaymentHistory" runat="server" OnItemDataBound="rptPaymentHistory_ItemDataBound">
            <ItemTemplate>
                <tr>
                    <td><asp:Literal ID="litTime" runat="server"></asp:Literal></td>
                    <td><asp:Literal ID="litPayBy" runat="server"></asp:Literal></td>
                    <td><asp:Literal ID="litAmountUSD" runat="server"></asp:Literal></td>
                    <td><asp:Literal ID="litAmountVND" runat="server"></asp:Literal></td>
                    <td><asp:Literal ID="litCreatedBy" runat="server"></asp:Literal></td>
                    <td><asp:Literal ID="litNote" runat="server"></asp:Literal></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                <tr>
                    <th colspan="2">Total</th>
                    <th><asp:Literal ID="litTotalUSD" runat="server"></asp:Literal></th>
                    <th><asp:Literal ID="litTotalVND" runat="server"></asp:Literal></th>
                </tr>
            </FooterTemplate>
        </asp:Repeater>
        </table>
        </asp:PlaceHolder>
    </div>
</asp:Content>
