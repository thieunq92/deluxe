<%@ Page Language="C#" MasterPageFile="TourMaster.Master" AutoEventWireup="true"
    Codebehind="ProviderList.aspx.cs" Inherits="CMS.Modules.TourManagement.Web.Admin.ProviderList"
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
                <asp:Label CssClass="label" ID="titleProviderList" runat="server"></asp:Label></legend>
            <div class="settinglist">
                <div class="search_panel">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="LabelName" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="textBoxName" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="labelLocation" runat="server"></asp:Label>
                            </td>
                            <td>
                                <%--Country--%>
                                <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"
                                    Width="150">
                                </asp:DropDownList><br />
                                <%--Region--%>
                                <br />
                                <asp:UpdatePanel ID="UpdatePanelRegion" runat="server" UpdateMode="Conditional" RenderMode="inline">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlRegion" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged"
                                            Width="150">
                                        </asp:DropDownList><br />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlCountry" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <%--City--%>
                                <br />
                                <asp:UpdatePanel ID="UpdatePanelCity" runat="server" UpdateMode="Conditional" RenderMode="inline">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlCity" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged"
                                            Width="150">
                                        </asp:DropDownList><br />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlCountry" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlRegion" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <%--Location--%>
                                <br />
                                <asp:UpdatePanel ID="UpdatePanelLocation" runat="server" UpdateMode="Conditional"
                                    RenderMode="inline">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlLocation" runat="server" Width="150">
                                        </asp:DropDownList><br />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlCountry" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlRegion" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlCity" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Button ID="butonSearch" runat="server" OnClick="butonSearch_OnClick" CssClass="button" />
                <div class="data_table">
                    <div class="panel_result" id="panelResult" runat="server" visible="false">
                        <asp:Label ID="labelResults" runat="server" ForeColor="blue"></asp:Label>
                    </div>
                    <div class="pager">
                        <cc1:Mirror ID="mirrorPager" runat="server" ControlIDToMirror="pagerProvider" />
                    </div>
                    <div class="data_grid">
                        <table cellpadding="0" cellspacing="0">
                            <asp:Repeater ID="rptProvider" runat="server" OnItemDataBound="rptProvider_OnItemDataBound"
                                OnItemCommand="rptProvider_OnItemCommand">
                                <HeaderTemplate>
                                    <tr class="header">
                                        <th style="width: 20%;">
                                            <asp:HyperLink ID="hplName" runat="server" Text='<%# base.GetText("LabelName") %>'></asp:HyperLink>
                                            <asp:Image runat="server" ID="imgNameOrder" Visible="false" />
                                        </th>
                                        <th style="width: 25%;">
                                            <asp:HyperLink runat="server" ID="hplAddress" Text='<%# base.GetText("labelAddress") %>'></asp:HyperLink>
                                            <asp:Image runat="server" ID="imgAddressOrder" Visible="false" />
                                        </th>
                                        <th style="width: 30%;">
                                            <asp:HyperLink runat="server" ID="hplLocation" Text='<%# base.GetText("labelLocation") %>'></asp:HyperLink>
                                            <asp:Image runat="server" ID="imgLocationOrder" Visible="false" />
                                        </th>
                                        <th style="width: 10%;">
                                            <asp:HyperLink runat="server" ID="hplIsFeatured" Text='<%# base.GetText("labelFeatured") %>'></asp:HyperLink>
                                            <asp:Image runat="server" ID="imgIsFeaturedOrder" Visible="false" />
                                        </th>
                                        <th style="width: 15%;">
                                            <%# base.GetText("labelAction") %>
                                        </th>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="item">
                                        <td>
                                            <asp:HyperLink ID="hyperLinkName" runat="server"></asp:HyperLink>
                                        </td>
                                        <td>
                                            <asp:Label ID="labelProviderAddress" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="labelProviderLocation" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="checkBoxFeatured" runat="server" />
                                            <asp:Label ID="labelProviderFeatured" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:HyperLink runat="server" ID="hyperLinkView" ToolTip='View' NavigateUrl="#"><img class="image_button16" align="absmiddle" src="../Images/image.png" alt='<%# base.GetText("hyperLinkView") %>'/></asp:HyperLink>
                                            <asp:HyperLink runat="server" ID="hyperLinkEdit" ToolTip='Edit' NavigateUrl="#"><img class="image_button16" align="absmiddle" src="../Images/edit.gif" alt='<%# base.GetText("hyperLinkEdit") %>'/></asp:HyperLink>
                                            <asp:ImageButton runat="server" ID="imageButtonDelete" ToolTip='Delete' ImageUrl="../Images/delete_file.gif"
                                                AlternateText='<%# base.GetText("imageButtonDelete") %>' ImageAlign="AbsMiddle"
                                                CssClass="image_button16" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
                                            <asp:ImageButton runat="server" ID="imageButtonFeature" ToolTip='Feature this' ImageUrl="../Images/arrow_up.png"
                                                AlternateText='<%# base.GetText("imageButtonFeature") %>' ImageAlign="AbsMiddle"
                                                CssClass="image_button16" CommandName="FeatureUp" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
                                            <asp:ImageButton runat="server" ID="imageButtonFeatureDown" ToolTip='Unfeature this'
                                                ImageUrl="../Images/arrow_down.png" AlternateText='<%# base.GetText("imageButtonFeatureDown") %>'
                                                ImageAlign="AbsMiddle" CssClass="image_button16" CommandName="FeatureDown" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
                                            <ajax:ConfirmButtonExtender ID="ConfirmButtonExtenderDelete" runat="server" TargetControlID="imageButtonDelete"
                                                ConfirmText='<%# base.GetText("messageConfirmDelete") %>'>
                                            </ajax:ConfirmButtonExtender>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                    <div class="pager">
                        <asp:DropDownList ID="ddlPageSize" runat="server" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged"
                            AutoPostBack="true">
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem Selected="True">20</asp:ListItem>
                            <asp:ListItem>50</asp:ListItem>
                            <asp:ListItem>100</asp:ListItem>
                            <asp:ListItem>200</asp:ListItem>
                        </asp:DropDownList>
                        <cc1:Pager ID="pagerProvider" runat="server" OnCacheEmpty="pagerProvider_CacheEmpty"
                            OnPageChanged="pagerProvider_PageChanged" ControlToPage="rptProvider" PageSize="20" />
                    </div>
                </div>
            </div>
        </fieldset>
    </asp:Panel>
</asp:Content>
