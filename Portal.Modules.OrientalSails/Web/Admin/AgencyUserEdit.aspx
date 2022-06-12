<%@ Page Title="Agency user edit" Language="C#" MasterPageFile="NewPopup.Master" AutoEventWireup="true" CodeBehind="AgencyUserEdit.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.AgencyUserEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="AdminContent" runat="server">
    <script>
        function RefreshParentPage() {
            window.parent.closePoup(1);
        }
        function closePoup() {
            window.parent.closePoup(0);
        }
    </script>
    <div class="group">
        <h4>Cấu hình cơ bản</h4>
        <table>
            <tr>
                <td style="width: 200px">Agency</td>
                <td>

                    <asp:DropDownList ID="ddlAgencies" runat="server" CssClass="form-control" Style="display: inline-block; width: 65%">
                    </asp:DropDownList>


                </td>
            </tr>
            <tr>
                <td style="width: 200px">Tên đăng nhập</td>
                <td>
                    <asp:TextBox ID="txtUsername" CssClass="form-control" runat="server" Width="200px"></asp:TextBox><asp:Label ID="lblUsername" runat="server" Visible="False"></asp:Label><asp:RequiredFieldValidator ID="rfvUsername" runat="server" ErrorMessage="Phải có tên đăng nhập" CssClass="validator"
                        Display="Dynamic" EnableClientScript="False" ControlToValidate="txtUsername"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td>Họ</td>
                <td>
                    <asp:TextBox ID="txtFirstname" CssClass="form-control" runat="server" Width="200px"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Tên</td>
                <td>
                    <asp:TextBox ID="txtLastname" CssClass="form-control" runat="server" Width="200px"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Email</td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Width="200px"></asp:TextBox><asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" EnableClientScript="False"
                        Display="Dynamic" CssClass="validator" ErrorMessage="Phải có email"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" EnableClientScript="False"
                            Display="Dynamic" CssClass="validator" ErrorMessage="Email không hợp lệ" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td>Website</td>
                <td>
                    <asp:TextBox ID="txtWebsite" runat="server" CssClass="form-control" Width="200px"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Hoạt động</td>
                <td>
                    <asp:CheckBox ID="chkActive" runat="server"></asp:CheckBox></td>
            </tr>
            <tr>
                <td>Múi giờ</td>
                <td>
                    <asp:DropDownList ID="ddlTimeZone" CssClass="form-control" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Mật khẩu</td>
                <td>
                    <asp:TextBox ID="txtPassword1" runat="server" CssClass="form-control" Width="200px" TextMode="Password"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Nhập lại mật khẩu</td>
                <td>
                    <asp:TextBox ID="txtPassword2" runat="server" CssClass="form-control" Width="200px" TextMode="Password"></asp:TextBox><asp:CompareValidator ID="covPassword" runat="server" ControlToValidate="txtPassword1" EnableClientScript="False"
                        Display="Dynamic" CssClass="validator" ErrorMessage="Hai ô mật khẩu không trùng nhau" ControlToCompare="txtPassword2"></asp:CompareValidator></td>
            </tr>
        </table>
    </div>
    <div class="group">
        <h4>Vai trò</h4>
        <table class="tbl">
            <asp:Repeater ID="rptRoles" runat="server" OnItemDataBound="rptRoles_ItemDataBound">
                <HeaderTemplate>
                    <tr>
                        <th>Vai trò</th>
                        <th></th>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
                        <td style="text-align: center">
                            <asp:CheckBox ID="chkRole" runat="server"></asp:CheckBox></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    <div>
        <asp:Button ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-primary" runat="server" Text="Lưu"></asp:Button>
        <asp:Button ID="btnCancel" runat="server" Text="Đóng" CausesValidation="False" OnClientClick="closePoup();return false;" CssClass="btn btn-default" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
</asp:Content>
