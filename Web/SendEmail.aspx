<%@ Page Language="C#" AutoEventWireup="true" Codebehind="SendEmail.aspx.cs" Inherits="CMS.Web.SendEmail" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Send Email</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="send_email">
            <asp:Label ID="labelSendStatus" runat="server"></asp:Label>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="labelYourName" runat="server">Tên của bạn</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="textBoxYourName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="labelYourEmail" runat="server">Email của bạn</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="textBoxYourEmail" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="labelSendTo" runat="server">Gửi đến</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="textBoxSendTo" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="labelSendToCC" runat="server">Đồng gửi đến</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="textBoxSendToCC" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="labelSubject" runat="server">Tiêu đề</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="textBoxSubject" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="labelMessage" runat="server">Thông điệp</asp:Label>
                    </td>
                    <td rowspan="2">
                        <asp:TextBox ID="textBoxMessage" runat="server" TextMode="MultiLine" Height="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="buttonSendEmail" runat="server" OnClick="buttonSendEmail_Click" Text="Gửi" />
                        <asp:Button ID="buttonClose" runat="server" Text="Đóng" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
