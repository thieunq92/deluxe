<%@ Page Language="C#" MasterPageFile="SailsMaster.Master" AutoEventWireup="true" CodeBehind="CommissionEdit.aspx.cs" Inherits="Portal.Modules.OrientalSails.CommissionEdit" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="basicinfo">
        <table>
            <asp:PlaceHolder ID="plhOneBooking" runat="server">
            <tr>
                <td style="width:30%">Booking ID</td>
                <td style="width:70%"><asp:Literal ID="litBookingCode" runat="server"></asp:Literal></td>                
            </tr>
            <tr>
                <td>Agency</td>
                <td><asp:Literal ID="litAgency" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td>Start date</td>
                <td><asp:Literal ID="litStartDate" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td>Service</td>
                <td><asp:Literal ID="litService" runat="server"></asp:Literal></td>
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
                <td style="width:30%">Total</td>
                <td style="width:70%"><asp:Literal ID="litTotal" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td>Applied rate</td>
                <td><asp:TextBox ID="txtAppliedRate" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>USD paid</td>
                <td><asp:TextBox ID="txtPaid" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>VND paid</td>
                <td><asp:TextBox ID="txtPaidBase" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Mark as paid</td>
                <td><asp:CheckBox ID="chkPaid" runat="server" /></td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:center;">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" OnClick="btnSave_Click" />
                <asp:Button ID="btnConfirm" runat="server" Text="Confirm" CssClass="button" OnClick="btnConfirm_Click" />
                <input id="reset" type="button" value="Reset" class="button" onclick="window.location = window.location" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
