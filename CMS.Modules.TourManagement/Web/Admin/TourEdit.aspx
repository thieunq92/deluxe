<%@ Page Language="C#" MasterPageFile="TourMaster.Master" AutoEventWireup="true"
    Codebehind="TourEdit.aspx.cs" Inherits="CMS.Modules.TourManagement.Web.Admin.TourEdit"
    Title="Untitled Page" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls.FileUpload"
    TagPrefix="cc2" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <asp:Panel ID="panelContent" runat="server">
        <fieldset>
            <legend>
                <asp:Label ID="labelTourEdit" runat="server"></asp:Label></legend>
            <div class="settinglist">
                <svc:Mirror ID="mirrorSubmit" runat="server" ControlIDToMirror="ButtonSubmit" />
                <svc:Mirror ID="mirrorCancel" runat="server" ControlIDToMirror="buttonCancel" />
                <div class="basicinfo">
                    <table width="100%" cellspacing="5">
                        <tr valign="top">
                            <td width="12%">
                                <%= base.GetText("LabelName") %>
                            </td>
                            <td width="38%">
                                <asp:TextBox ID="textBoxName" runat="server"></asp:TextBox>
                            </td>
                            <td width="12%">
                                <%= base.GetText("labelTourType") %>
                            </td>
                            <td width="38%">
                                <asp:DropDownList ID="ddlTourType" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Local operator
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlProviders" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <%= base.GetText("labelNumberOfDays") %>
                            </td>
                            <td>
                                <asp:TextBox ID="textBoxNumberOfDays" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Package type</td>
                            <td>
                                <asp:DropDownList ID="ddlPackageType" runat="server">
                                </asp:DropDownList></td>
                            <td>Is half a day</td>
                            <td><asp:CheckBox ID="chkHalf" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>
                                <%= base.GetText("labelStartAt") %>
                            </td>
                            <td>
                                <span>
                                    <asp:DropDownList ID="dropDownCountry" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dropDownCountry_SelectedIndexChanged"
                                        Width="150">
                                    </asp:DropDownList><br />
                                    <!-- Area -->
                                    <asp:UpdatePanel ID="UpdatePanelArea" runat="server" UpdateMode="Conditional" RenderMode="inline">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="dropDownArea" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dropDownArea_SelectedIndexChanged"
                                                Enabled="false" Width="150">
                                            </asp:DropDownList><br />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="dropDownCountry" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <!-- City -->
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="inline">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="dropDownCity" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dropDownCity_SelectedIndexChanged"
                                                Enabled="false" Width="150">
                                            </asp:DropDownList>
                                            <br />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="dropDownCountry" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="dropDownArea" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <!-- City -->
                                    <asp:UpdatePanel ID="UpdatePanelSmallThanCity" runat="server" UpdateMode="Conditional"
                                        RenderMode="inline">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="dropdownListLocation" runat="server" Enabled="false" Width="150">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="dropDownCountry" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="dropDownArea" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="dropDownCity" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </span>
                            </td>
                            <td>
                                <%= base.GetText("labelEndIn") %>
                            </td>
                            <td>
                                <span>
                                    <asp:DropDownList ID="DropDownListCountryEnd" runat="server" AutoPostBack="true"
                                        OnSelectedIndexChanged="DropDownListCountryEnd_SelectedIndexChanged" Width="150">
                                    </asp:DropDownList><br />
                                    <!-- Area -->
                                    <asp:UpdatePanel ID="UpdatePanelRegionEnd" runat="server" UpdateMode="Conditional"
                                        RenderMode="inline">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="DropDownListRegionEnd" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListRegionEnd_SelectedIndexChanged"
                                                Enabled="false" Width="150">
                                            </asp:DropDownList><br />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="DropDownListCountryEnd" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <!-- City -->
                                    <asp:UpdatePanel ID="UpdatePanelCityEnd" runat="server" UpdateMode="Conditional"
                                        RenderMode="inline">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="DropDownListCityEnd" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListCityEnd_SelectedIndexChanged"
                                                Enabled="false" Width="150">
                                            </asp:DropDownList>
                                            <br />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="DropDownListCountryEnd" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="DropDownListRegionEnd" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <!-- City -->
                                    <asp:UpdatePanel ID="UpdatePanelLocationEnd" runat="server" UpdateMode="Conditional"
                                        RenderMode="inline">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="DropDownListLocationEnd" runat="server" Enabled="false" Width="150">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="DropDownListCountryEnd" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="DropDownListRegionEnd" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="DropDownListCityEnd" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%= base.GetText("labelGrade") %>
                            </td>
                            <td>
                                <asp:TextBox ID="textBoxGrade" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <%= base.GetText("labelTourRegion") %>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTourRegions" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="basicinfo">
                    <table>
                        <tr>
                            <td style="width: 12%">
                                <asp:Label ID="LabelAbtractImage" runat="server"></asp:Label></td>
                            <td style="width: 37%">
                                <cc2:FileUploaderAJAX runat="server" ID="FileUploaderImage" />
                                <asp:TextBox ID="txtHiddenImage" runat="server" Style="display: none;"></asp:TextBox>
                                <div id="divImage" style="overflow: auto">
                                </div>
                            </td>
                            <td style="width: 12%">
                                <asp:Label ID="labelMap" runat="server" Text="Ảnh đại diện"></asp:Label></td>
                            <td style="width: 37%">
                                <cc2:FileUploaderAJAX runat="server" ID="FileUploaderMap" />
                                <asp:TextBox ID="txtHiddenMap" runat="server" Style="display: none;"></asp:TextBox>
                                <div id="divMap" style="overflow: auto">
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="advancedinfo">
                    <h4>
                        <%= base.GetText("labelSummary") %>
                    </h4>
                    <FCKeditorV2:FCKeditor ID="fckEditorSummary" runat="server" Width="100%" Height="200"
                        BasePath="~/support/fckeditor/" ToolbarSet="Basic">
                    </FCKeditorV2:FCKeditor>
                </div>
                <div class="advancedinfo">
                    <h4>
                        <%= base.GetText("labelActivities") %>
                    </h4>
                    <FCKeditorV2:FCKeditor ID="fckEditorActivities" runat="server" Width="100%" Height="200"
                        BasePath="~/support/fckeditor/" ToolbarSet="Basic">
                    </FCKeditorV2:FCKeditor>
                </div>
                <div class="advancedinfo">
                    <h4>
                        <%= base.GetText("labelTripHighLight") %>
                    </h4>
                    <FCKeditorV2:FCKeditor ID="fckEditorTripHighLight" runat="server" Width="100%" Height="200"
                        BasePath="~/support/fckeditor/" ToolbarSet="Basic">
                    </FCKeditorV2:FCKeditor>
                </div>
                <div class="advancedinfo">
                    <h4>
                        <%= base.GetText("labelDetailsIniterary") %>
                    </h4>
                    <FCKeditorV2:FCKeditor ID="fckEditorDetailsIniterary" runat="server" Width="100%"
                        Height="200" BasePath="~/support/fckeditor/" ToolbarSet="Basic">
                    </FCKeditorV2:FCKeditor>
                </div>
                <div class="advancedinfo">
                    <h4>
                        <%= base.GetText("labelInclusion") %>
                    </h4>
                    <FCKeditorV2:FCKeditor ID="fckEditorInclusion" runat="server" Width="100%" Height="200"
                        BasePath="~/support/fckeditor/" ToolbarSet="Basic">
                    </FCKeditorV2:FCKeditor>
                </div>
                <div class="advancedinfo">
                    <h4>
                        <%= base.GetText("labelExclusion") %>
                    </h4>
                    <FCKeditorV2:FCKeditor ID="fckEditorExclusion" runat="server" Width="100%" Height="200"
                        BasePath="~/support/fckeditor/" ToolbarSet="Basic">
                    </FCKeditorV2:FCKeditor>
                </div>
                <div class="advancedinfo">
                    <h4>
                        <%= base.GetText("labelWhatToTake") %>
                    </h4>
                    <FCKeditorV2:FCKeditor ID="fckEditorWhatToTake" runat="server" Width="100%" Height="200"
                        BasePath="~/support/fckeditor/" ToolbarSet="Basic">
                    </FCKeditorV2:FCKeditor>
                </div>
                <div class="advancedinfo">
                    <h4>
                        <%= base.GetText("labelTourInstruction") %>
                    </h4>
                    <FCKeditorV2:FCKeditor ID="fckEditorTourInstruction" runat="server" Width="100%"
                        Height="200" BasePath="~/support/fckeditor/" ToolbarSet="Basic">
                    </FCKeditorV2:FCKeditor>
                </div>
                <div class="advancedinfo">
                    <h4>
                        <%= base.GetText("labelNoteForOperator") %>
                    </h4>
                    <FCKeditorV2:FCKeditor ID="fckEditorNoteForOperator" runat="server" Width="100%"
                        Height="200" BasePath="~/support/fckeditor/" ToolbarSet="Basic">
                    </FCKeditorV2:FCKeditor>
                </div>
                <div class="advancedinfo">
                    <h4>
                        <%= base.GetText("labelNoteForSale") %>
                    </h4>
                    <FCKeditorV2:FCKeditor ID="fckEditorNoteForSale" runat="server" Width="100%" Height="200"
                        BasePath="~/support/fckeditor/" ToolbarSet="Basic">
                    </FCKeditorV2:FCKeditor>
                </div>
            </div>
            <asp:Button ID="ButtonSubmit" runat="server" Text="Submit" CssClass="button" OnClick="ButtonSubmit_Click">
            </asp:Button>
        </fieldset>
    </asp:Panel>
</asp:Content>
