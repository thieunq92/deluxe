<%@ Page Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="ExtraOptionEdit.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.ExtraOptionEdit" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <div style="width: 600px; float: right;">
        <asp:UpdatePanel ID="updatePanelEdit" runat="server">
            <ContentTemplate>
                <asp:Label ID="labelStatus" runat="server"></asp:Label>
                <table class="table table-borderless" style="width:67%">
                    <tr>
                        <td>
                            <asp:Label ID="labelName" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="textBoxName" runat="server" CssClass="form-control"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="labelPrice" runat="server">Price</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="textBoxPrice" runat="server" CssClass="form-control"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td><%= base.GetText("textIncludedInRoomPrice") %></td>
                        <td style="padding-left:22px; padding-bottom:20px">
                            <asp:CheckBox ID="chkIncluded" runat="server" CssClass="checkbox" /></td>
                    </tr>
                    <tr>
                        <td><%= base.GetText("textTargetApply") %></td>
                        <td>
                            <asp:DropDownList ID="ddlTargets" runat="server" CssClass="form-control">
                                <asp:ListItem>All</asp:ListItem>
                                <asp:ListItem>Customer</asp:ListItem>
                                <asp:ListItem>Booking</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="labelDescription" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="textBoxDescription" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <asp:Label ID="labelNote" runat="server"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Button ID="buttonSubmit" runat="server" ValidationGroup="date" CssClass="btn btn-primary"
            OnClick="buttonSubmit_Click" />
        <asp:Button ID="buttonAdd" runat="server" CssClass="btn btn-primary" OnClick="buttonAdd_Click" />
    </div>
    <div style="width: 320px; float: left; padding-left:20px">
        <asp:UpdatePanel ID="updatePanelList" runat="server">
            <ContentTemplate>
                <table class="table table-bordered table-common">
                    <asp:Repeater ID="rptExtraOption" runat="server" OnItemDataBound="rptExtraOption_ItemDataBound"
                        OnItemCommand="rptExtraOption_ItemCommand">
                        <HeaderTemplate>
                            <tr class="active">
                                <th style="width: 150px;">
                                    Name
                                </th>
                                <th style="width: 80px;">
                                    Price
                                </th>
                                <th></th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="item">
                                <td class="--text-left">
                                    <asp:Label ID="label_Name" runat="server"></asp:Label>
                                </td>
                                <td class="--text-right">
                                    <asp:Label ID="label_Price" runat="server"></asp:Label>
                                </td>

                                <td>
                                    <asp:ImageButton runat="server" ID="imageButtonEdit" ToolTip='Edit' ImageUrl="../Images/edit.gif"
                                        AlternateText='Edit' ImageAlign="AbsMiddle" CssClass="image_button16" CommandName="Edit"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
                                    <asp:ImageButton runat="server" ID="imageButtonDelete" ToolTip='Delete' ImageUrl="../Images/delete_file.gif"
                                        AlternateText='Delete' ImageAlign="AbsMiddle" CssClass="image_button16" CommandName="Delete"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
                                    <ajax:ConfirmButtonExtender ID="ConfirmButtonExtenderDelete" runat="server" TargetControlID="imageButtonDelete"
                                        ConfirmText='<%# base.GetText("messageConfirmDelete") %>'>
                                    </ajax:ConfirmButtonExtender>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

