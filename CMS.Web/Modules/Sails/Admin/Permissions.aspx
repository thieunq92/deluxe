<%@ Page Language="C#" MasterPageFile="SailsMaster.Master" AutoEventWireup="true" CodeBehind="Permissions.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.Permissions" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <fieldset>
        <legend><asp:Literal ID="litTitle" runat="server"></asp:Literal></legend>
        <div>
            <ul style="list-style:none; width: 100%;" class="permission_list">
            <asp:Repeater ID="rptPermissions" runat="server" OnItemDataBound="rptPermissions_ItemDataBound">
                <ItemTemplate>
                    <li id="liClear" class="p_header" runat="server" style="clear:both;" visible="false">
                    </li>
                    <li style="float:left; width: 300px;display:block;">
                        <asp:HiddenField ID="hiddenName" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Name") %>'/>
                        <asp:CheckBox ID="chkPermission" runat="server" />
                    </li>
                </ItemTemplate>
            </asp:Repeater>
            </ul>
        </div>
        <asp:Button ID="btnSave" runat="server" CssClass="button" OnClick="btnSave_Click"/>
    </fieldset>
</asp:Content>
