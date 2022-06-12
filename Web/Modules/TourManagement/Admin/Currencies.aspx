<%@ Page Language="C#" MasterPageFile="TourMaster.Master" AutoEventWireup="true"
    Codebehind="Currencies.aspx.cs" Inherits="CMS.Modules.TourManagement.Web.Admin.Currencies"
    Title="Untitled Page" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <fieldset>
        <legend>
            <img src="../Images/location.gif" align="absMiddle" />
            <asp:Label ID="labelCurrencies" runat="server" Text="Danh sách địa điểm"></asp:Label>
        </legend>
        <div class="settinglist">
            <asp:UpdatePanel ID="updatePanelCurrencies" runat="server">
                <ContentTemplate>
                    <div class="listbox">
                        <ul>
                            <asp:Repeater ID="repeaterCurrencies" runat="server" OnItemDataBound="repeaterCurrencies_ItemDataBound"
                                OnItemCommand="repeaterCurrencies_ItemCommand">
                                <ItemTemplate>
                                    <li>
                                        <asp:LinkButton ID='linkButtonCurrencyEdit' runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name")%>'
                                            CommandName="edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id")%>'>
                                        </asp:LinkButton>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                        <asp:Button ID="btnAddNew" runat="server" Text="New Service" OnClick="btnAddNew_Click" CssClass="button"/>
                    </div>
                    <div class="wbox">
                        <div class="wbox_title">
                            <asp:Label ID="labelFormTitle" runat="server" Text="Title"></asp:Label>
                        </div>
                        <div class="wbox_content">
                            <div class="group">
                                <h4>
                                    <asp:Label ID="labelGeneral" runat="server" Text="General"></asp:Label></h4>
                                <table>
                                    <tr>
                                        <td style="width: 100px;">
                                            <asp:Label ID="lblName" runat="server" Text="Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="textBoxName" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="labelCulture" runat="server" Text="Culture"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="dropDownListCulture" runat="server" Width="250" AutoPostBack="true"
                                                OnSelectedIndexChanged="dropDownListCulture_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="labelCustomCultureSetting" runat="server" Text="Custom setting"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="checkBoxCustomSetting" runat="server" Enabled="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="labelConversationRate" runat="server" Text="Conversation rate"></asp:Label>
                                            <br /><i>(From USD)</i>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="textBoxConversationRate" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="group">
                                <h4>
                                    <asp:Label ID="labelCultureInfo" runat="server" Text="Culture Info"></asp:Label></h4>
                                <table>
                                    <tr>
                                        <td style="width: 100px;">
                                            <asp:Label ID="labelSymbol" runat="server" Text="Symbol"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="textBoxSymbol" runat="server" MaxLength="1" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="labelFormat" runat="server" Text="Format"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="textBoxFormat" runat="server" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="service" CssClass="button"/>
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" CssClass="button"/>
                                <ajax:ConfirmButtonExtender ID="ConfirmButtonExtenderDelete" runat="server" TargetControlID="btnDelete"
                                    ConfirmText='Are you sure want to delete?'>
                                </ajax:ConfirmButtonExtender>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </fieldset>
</asp:Content>
