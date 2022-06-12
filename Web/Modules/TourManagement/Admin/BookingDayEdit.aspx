<%@ Page Language="C#" MasterPageFile="PopUp.Master" AutoEventWireup="true" Codebehind="BookingDayEdit.aspx.cs"
    Inherits="CMS.Modules.TourManagement.Web.Admin.BookingDayEdit" Title="Untitled Page" %>

<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <div>
        <div class="basicinfo">
            <table>
                <tr>
                    <td>
                        Day title</td>
                    <td>
                        <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <FCKeditorV2:FCKeditor ID="fckContent" runat="server" Width="100%" Height="200" BasePath="~/support/fckeditor/"
                            ToolbarSet="Basic">
                        </FCKeditorV2:FCKeditor>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" CssClass="button" />
    </div>
</asp:Content>
