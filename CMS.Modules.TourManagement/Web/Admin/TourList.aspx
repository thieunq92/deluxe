<%@ Page Language="C#" MasterPageFile="TourMaster.Master" AutoEventWireup="true"
    Codebehind="TourList.aspx.cs" Inherits="CMS.Modules.TourManagement.Web.Admin.TourList"
    Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">

    <script type="text/javascript">
function confirm_delete()
{
  if (confirm("Are you sure you want to delete?")==true)
    return true;
  else
    return false;
}
    </script>

    <fieldset>
        <legend>
            <asp:Label CssClass="label" ID="labelTourList" runat="server" Text="Quản lý khách hàng"></asp:Label></legend>
        <div class="settinglist">
            <div class="search_panel">
                <table>
                    <tr>
                        <td>
                            <%= base.GetText("labelCityStart") %>
                        </td>
                        <td>
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
                        </td>
                        <td>
                            <%= base.GetText("labelCityEnd") %>
                        </td>
                        <td>
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
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Number of day >
                        </td>
                        <td>
                            <asp:TextBox ID="textBoxNumberOfDayGt" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            Number of day <
                        </td>
                        <td>
                            <asp:TextBox ID="textBoxNumberOfDayLt" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <!--<tr>
                            <td>
                                Length >
                            </td>
                            <td>
                                <asp:TextBox ID="textBoxLengthGt" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Length <
                            </td>
                            <td>
                                <asp:TextBox ID="textBoxLengthLt" runat="server"></asp:TextBox>
                            </td>
                        </tr>-->
                                <tr>
                                    <td>Tour name</td>
                                    <td><asp:TextBox ID="txtTourName" runat="server"></asp:TextBox></td>
                                    <td>Exact duration (0 for 1/2 days)</td>
                                </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTourTypes" runat="server">
                            </asp:DropDownList></td>
                        <td>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTourRegion" runat="server">
                            </asp:DropDownList></td>
                    </tr>
                </table>
            </div>
            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                CssClass="button" />
            <div class="data_table">
                <div class="panel_result" id="panelResult" runat="server" visible="false">
                    <asp:Label ID="labelResults" runat="server"></asp:Label>
                </div>
                <div class="pager">
                    <svc:Mirror ID="mirrorPager" runat="server" ControlIDToMirror="pagerTours" />
                </div>
                <div class="data_grid">
                    <table cellpadding="0" cellspacing="0">
                        <asp:Repeater ID="rptTours" runat="server" OnItemDataBound="rptTours_ItemDataBound"
                            OnItemCommand="rptTours_ItemCommand">
                            <HeaderTemplate>
                                <tr class="header">
                                    <th style="width: 20%;">
                                        <asp:HyperLink runat="server" ID="hplName" Text='<%# base.GetText("LabelName") %>'></asp:HyperLink>
                                        <asp:Image runat="server" ID="imgNameOrder" Visible="false" />
                                    </th>
                                    <th style="width: 15%;">
                                        <%# base.GetText("labelCityStart")%>
                                    </th>
                                    <th style="width: 15%;">
                                        <%# base.GetText("labelCityEnd")%>
                                    </th>
                                    <th style="width: 10%;">
                                        <asp:HyperLink runat="server" ID="hplTourType" Text=' <%# base.GetText("labelTourType")%>'></asp:HyperLink>
                                        <asp:Image runat="server" ID="imgTourTypeOrder" Visible="false" />
                                    </th>
                                    <th style="width: 10%;">
                                        <asp:HyperLink runat="server" ID="hplNumberOfDay" Text='Time'></asp:HyperLink>
                                        <asp:Image runat="server" ID="imgNumberOfDayOrder" Visible="false" />
                                    </th>
                                    <th style="width: 10%;">
                                        <asp:HyperLink runat="server" ID="hplLengthTrip" Text='Length'></asp:HyperLink>
                                        <asp:Image runat="server" ID="imgLengthTripOrder" Visible="false" />
                                    </th>
                                    <th>
                                    </th>
                                    <th style="10%">
                                        <%# base.GetText("labelAction") %>
                                    </th>
                                </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="width: 125px;">
                                        <asp:HyperLink ID="hyperLinkView" runat="server"></asp:HyperLink>
                                    </td>
                                    <td style="width: 125px;">
                                        <asp:Label ID="labelCityStart" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 125px;">
                                        <asp:Label ID="labelCityEnd" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 100px;">
                                        <asp:Label ID="labelTourType" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 70px;">
                                        <asp:Label ID="labelNumberOfDays" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 70px;">
                                        <asp:Label ID="labelTripLength" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        Featured:
                                        <%# DataBinder.Eval(Container.DataItem,"Featured") %>
                                    </td>
                                    <td>
                                        <%--<asp:HyperLink runat="server" ID="hplView" ToolTip='View' NavigateUrl="#"><img class="image_button16" align="absmiddle" src="/Admin/AdminModuleImages/image.png" alt='<%# base.GetText("hplView") %>'/></asp:HyperLink>--%>
                                        <asp:HyperLink runat="server" ID="hplEdit" ToolTip='Edit' NavigateUrl="#">
                                            <asp:Image ID="imageEdit" runat="server" CssClass="image_button16" ImageUrl="/Admin/AdminModuleImages/edit.gif"
                                                ImageAlign="AbsMiddle" /></asp:HyperLink>
                                        <asp:ImageButton runat="server" ID="btnDelete" ToolTip='Delete' ImageUrl="/Admin/AdminModuleImages/delete_file.gif"
                                            AlternateText='<%# base.GetText("ibtDelete") %>' ImageAlign="AbsMiddle" CssClass="image_button16"
                                            CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'
                                            OnClientClick="return confirm_delete();" />
                                        <asp:ImageButton runat="server" ID="btnFeature" ToolTip='Feature this' ImageUrl="../Images/arrow_up.png"
                                            AlternateText='<%# base.GetText("ibtFeature") %>' ImageAlign="AbsMiddle" CssClass="image_button16"
                                            CommandName="FeatureUp" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
                                        <asp:ImageButton runat="server" ID="btnFeatureDown" ToolTip='Unfeature this' ImageUrl="../Images/arrow_down.png"
                                            AlternateText='<%# base.GetText("ibtFeature") %>' ImageAlign="AbsMiddle" CssClass="image_button16"
                                            CommandName="FeatureDown" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
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
                    <svc:Pager ID="pagerTours" runat="server" OnCacheEmpty="pagerTours_CacheEmpty" OnPageChanged="pagerTours_PageChanged"
                        ControlToPage="rptTours" PageSize="20" />
                </div>
            </div>
        </div>
    </fieldset>
</asp:Content>
