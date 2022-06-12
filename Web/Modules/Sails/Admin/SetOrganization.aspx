<%@ Page Language="C#" MasterPageFile="SailsMaster.Master" AutoEventWireup="true" CodeBehind="SetOrganization.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.SetOrganization" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <fieldset>
        <legend><asp:Literal ID="litTitle" runat="server"></asp:Literal></legend>
        <div>
            <ul style="list-style:none; width: 100%;" class="permission_list">
            <asp:Repeater ID="rptOrganizations" runat="server" OnItemDataBound="rptOrganizations_ItemDataBound">
                <ItemTemplate>
                    <li id="liClear" class="p_header" runat="server" style="clear:both;" visible="false">
                    </li>
                    <li style="float:left; width: 300px;display:block;">
                        <asp:HiddenField ID="hiddenId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>'/>
                        <asp:CheckBox ID="chkPermission" runat="server" />
                    </li>
                </ItemTemplate>
            </asp:Repeater>
            </ul>
        </div>
        <asp:Button ID="btnSave" runat="server" CssClass="button" OnClick="btnSave_Click"/>
    </fieldset>
</asp:Content>
