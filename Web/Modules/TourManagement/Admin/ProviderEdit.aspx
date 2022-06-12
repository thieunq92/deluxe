<%@ Page Language="C#" MasterPageFile="TourMaster.Master" AutoEventWireup="true"
    Codebehind="ProviderEdit.aspx.cs" Inherits="CMS.Modules.TourManagement.Web.Admin.ProviderEdit"
    Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls.FileUpload"
    TagPrefix="cc2" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <asp:Panel ID="panelContent" runat="server">
        <fieldset>
            <legend>                
                <asp:Label CssClass="label" ID="titleProviderEdit" runat="server" Text="admin"></asp:Label></legend>
            <div class="settinglist">
                <div class="basicinfo">
                    <asp:Label ID="labelUpdateStatus" ForeColor="blue" runat="server" Visible="false"></asp:Label>
                </div>
                <div class="basicinfo">
                    <table>
                        <tr>
                            <td style="width: 12%">
                                <asp:Label ID="labelLocation" runat="server"></asp:Label></td>
                            <td colspan="3">
                                <%--Country--%>
                                <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"
                                    Width="150">
                                </asp:DropDownList>
                                <%--Region--%>
                                <asp:UpdatePanel ID="UpdatePanelRegion" runat="server" UpdateMode="Conditional" RenderMode="inline">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlRegion" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged"
                                            Width="150">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlCountry" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <%--City--%>
                                <asp:UpdatePanel ID="UpdatePanelCity" runat="server" UpdateMode="Conditional" RenderMode="inline">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlCity" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged"
                                            Width="150">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlCountry" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlRegion" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <%--Location--%>
                                <asp:UpdatePanel ID="UpdatePanelLocation" runat="server" UpdateMode="Conditional"
                                    RenderMode="inline">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlLocation" runat="server" Width="150">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlCountry" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlRegion" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlCity" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <br />
                                <asp:Label ID="labelValidLocation" runat="server" Visible="false"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LabelName" runat="server"></asp:Label></td>
                            <td style="width: 37%">
                                <asp:TextBox ID="textBoxName" runat="server"></asp:TextBox></td>
                            <td style="width: 12%">
                                <asp:Label ID="labelAddress" runat="server"></asp:Label></td>
                            <td style="width: 37%">
                                <asp:TextBox ID="textBoxAddress" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="labelWebsite" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="textBoxWebsite" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="labelTel" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="textBoxTel" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="labelFax" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="textBoxFax" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="labelMobile" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="textBoxMobile" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Provider Type</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlTypes">
                                </asp:DropDownList></td>
                        </tr>
                    </table>
                </div>
                <div class="basicinfo">
                    <table>
                        <asp:Repeater ID="rptCategory" runat="server" OnItemDataBound="rptCategory_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkCategory" runat="server" /><asp:HiddenField ID="hiddenId" runat="server"
                                            Value='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
                <div class="advancedinfo">
                    <table>
                        <tr>
                            <td style="width: 12%;">
                                <asp:Label ID="labelImage" runat="server" Text="image"></asp:Label></td>
                            <td style="width: 37%;">
                                <cc2:FileUploaderAJAX runat="server" ID="FileUploaderImage" />
                                <asp:TextBox ID="textBoxHiddenImage" runat="server" Style="display: none;"></asp:TextBox>
                                <div id="divImage" style="overflow: auto">
                                </div>
                            </td>
                            <td style="width: 12%;">
                                <asp:Label ID="labelMap" runat="server" Text="Bản đồ"></asp:Label></td>
                            <td style="width: 37%;">
                                <cc2:FileUploaderAJAX runat="server" ID="FileUploaderMap" />
                                <asp:TextBox ID="textBoxHiddenMap" runat="server" Style="display: none;"></asp:TextBox>
                                <div id="divMap" style="overflow: auto">
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="advancedinfo">
                    <h4>
                        <asp:Label ID="labelDescription" runat="server" Text="Thông tin mô tả"></asp:Label></h4>
                    <FCKeditorV2:FCKeditor ID="FCKDescription" runat="server" Width="100%" Height="400"
                        BasePath="~/support/fckeditor/">
                    </FCKeditorV2:FCKeditor>
                </div>
                <asp:Button ID="buttonSubmit" runat="server" OnClick="buttonSubmit_OnClick" CssClass="button" />
                <asp:Button ID="buttonCancel" runat="server" OnClick="buttonCancel_OnClick" CssClass="button" />
            </div>
        </fieldset>
    </asp:Panel>
</asp:Content>
