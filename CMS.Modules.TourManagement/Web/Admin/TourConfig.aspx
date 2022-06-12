<%@ Page Language="C#" MasterPageFile="TourMaster.Master" AutoEventWireup="true" Codebehind="TourConfig.aspx.cs"
    Inherits="CMS.Modules.TourManagement.Web.Admin.TourHotelConfig" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <fieldset>
        <legend>
            <img src="../Images/location.gif" align="absMiddle" />
            <asp:Label ID="labelTourConfiguration" runat="server"></asp:Label></legend>            
        <div class="settinglist">
        <div class="basicinfo">
        <asp:Button ID="buttonOverview" runat="server" OnClick="buttonOverview_Click" CssClass="button"/>
        <asp:Button ID="buttonHotelConfig" runat="server" OnClick="buttonHotelConfig_Click" CssClass="button"/>
        <asp:Button ID="buttonRestaurantConfig" runat="server" OnClick="buttonRestaurantConfig_Click" CssClass="button"/>
        <asp:Button ID="buttonGuideConfig" runat="server" Text="Guide" OnClick="buttonGuideConfig_Click" CssClass="button"/>
        <asp:Button ID="buttonTransportConfig" runat="server" OnClick="buttonTransportConfig_Click" CssClass="button"/>
        <asp:Button ID="buttonLandscapeConfig" runat="server" OnClick="buttonLandscapeConfig_Click" CssClass="button"/>
        <asp:Button ID="buttonBoatConfig" runat="server" OnClick="buttonBoatConfig_Click" CssClass="button"/>
        <asp:Button ID="buttonTourPackageConfig" runat="server" OnClick="buttonTourPackageConfig_Click" Text="Package config" CssClass="button"/>
        <asp:Button ID="buttonMiscellaneous" runat="server" OnClick="buttonMiscellaneous_Click" CssClass="button" Text="Miscellaneous"/>
        <asp:Button ID="buttonRelated" runat="server" OnClick="buttonRelated_Click" CssClass="button" Text="Related" />
        </div>
        
        <div class="advancedinfo">
        <asp:PlaceHolder ID="plhConfig" runat="server"></asp:PlaceHolder>
        </div>
        <asp:Panel ID="panelExport" runat="server" Visible="false">
        <ul>
        <li>
            <asp:Button ID="btnCalculate" runat="server" Text="Calculate" OnClick="btnCalculate_Click" />
            <asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click" />
        </li>
        </ul>
        </asp:Panel>
        </div>
    </fieldset>
</asp:Content>
