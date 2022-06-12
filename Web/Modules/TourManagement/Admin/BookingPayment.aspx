<%@ Page Language="C#" MasterPageFile="PopUp.Master" AutoEventWireup="true" CodeBehind="BookingPayment.aspx.cs" Inherits="CMS.Modules.TourManagement.Web.Admin.BookingPayment" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">    
    <div class="basicinfo">
        <table>
            <tr>
                <td>Total</td>
                <td><asp:Label ID="lblTotal" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>Paid</td>
                <td><asp:TextBox ID="txtPaid" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Accepted (auto if paid > 0)</td>
                <td><asp:CheckBox ID="chkPaid" runat="server" /></td>
            </tr>
        </table>
    </div>
    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" OnClick="btnSave_Click" />
</asp:Content>
