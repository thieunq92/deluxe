<%@ Page Language="C#" MasterPageFile="SailsMaster.Master" AutoEventWireup="true" CodeBehind="Organizations.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.Organizations" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <fieldset>
        <legend>
            <asp:Label ID="labelBoatCategoryManagement" runat="server"></asp:Label>
        </legend>
        <div class="settinglist">
            <div class="listbox">
                <ul>
                    <asp:Repeater ID="repeaterServices" runat="server" OnItemDataBound="repeaterServices_ItemDataBound"
                        OnItemCommand="repeaterServices_ItemCommand">
                        <ItemTemplate>
                            <li>
                                <asp:LinkButton ID='linkButtonServiceEdit' runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name")%>'
                                    CommandName="edit">
                                </asp:LinkButton>
                                <ul>
                                    <asp:Repeater ID="repeaterSubCategories" runat="server" OnItemCommand="repeaterServices_ItemCommand"
                                        OnItemDataBound="repeaterServices_ItemDataBound">
                                        <ItemTemplate>
                                            <li>
                                                <asp:LinkButton ID='linkButtonServiceEdit' runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name")%>'
                                                    CommandName="edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>'></asp:LinkButton>
                                                <ul>
                                                    <asp:Repeater ID="repeaterSubCategories" runat="server" OnItemCommand="repeaterServices_ItemCommand">
                                                        <ItemTemplate>
                                                            <li>
                                                                <asp:LinkButton ID='linkButtonServiceEdit' runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name")%>'
                                                                    CommandName="edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>'></asp:LinkButton></li>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </ul>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                <asp:Button ID="btnAddNewRegion" runat="server" OnClick="btnAddNew_Click" CssClass="button" Text="New region" />
            </div>
            <div class="wbox">
                <div class="wbox_title">
                    <asp:Label ID="labelFormTitle" runat="server" Text="Title"></asp:Label>
                </div>
                <div class="wbox_content">
                    <div class="group">
                        <h4>
                            <asp:Label ID="labelBoatCategoryInformation" runat="server"></asp:Label></h4>
                        <table>
                            <tr>
                                <td style="width: 100px;">
                                    <asp:Label ID="labelRegionName" runat="server" Text="Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="textBoxServiceName" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvTypeOfService" runat="server" ControlToValidate="textBoxServiceName"
                                        Text="Type is required!" ValidationGroup="service"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr style="display: none;">
                                <td style="width: 100px;">
                                    <asp:Label ID="labelParent" runat="server" Text="Type Of Service"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlParent" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="display: none;">
                                <td style="width: 100px;">
                                    <asp:Label ID="labelOrderPriority" runat="server" Text="Type Of Service"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="textBoxOrder" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="display: none;">
                                <td>
                                    <asp:Label ID="labelDescription" runat="server" Text="Install Fee"></asp:Label>
                                </td>
                                <td>
                                    <FCKeditorV2:FCKeditor ID="fckEditorDescription" runat="server" Width="100%" Height="200"
                                        BasePath="~/support/fckeditor/" ToolbarSet="FontOnly">
                                    </FCKeditorV2:FCKeditor>
                                </td>
                            </tr>
                        </table>
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="service"
                            CssClass="button" />
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click"
                            CssClass="button" />
                        <ajax:ConfirmButtonExtender ID="ConfirmButtonExtenderDelete" runat="server" TargetControlID="btnDelete"
                            ConfirmText='Are you sure want to delete?'>
                        </ajax:ConfirmButtonExtender>
                    </div>
                </div>
            </div>
        </div>
    </fieldset>
</asp:Content>
