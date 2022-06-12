<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TourRegionMenu.ascx.cs" Inherits="CMS.Modules.TourManagement.Web.TourRegionMenu" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<div class="region_menu">
<asp:UpdatePanel ID="UpdatePanelLocation" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
<ContentTemplate>
<asp:TreeView ID="treeViewTourRegion" runat="server" ExpandDepth="0" ImageSet="Simple" OnTreeNodePopulate="treeViewTourRegion_TreeNodePopulate" PopulateNodesFromClient="False" ShowExpandCollapse="false" RootNodeStyle-Width="100%">
    </asp:TreeView>
</ContentTemplate>
<Triggers>
    <asp:AsyncPostBackTrigger ControlID="treeViewTourRegion" EventName="TreeNodePopulate" />
</Triggers>
</asp:UpdatePanel>
</div>