<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LocationTree.ascx.cs" Inherits="CMS.Modules.TourManagement.Web.LocationTree" EnableViewState="true" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<div>
<asp:UpdatePanel ID="UpdatePanelLocation" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
<ContentTemplate>
<asp:TreeView ID="treeViewLocation" runat="server" ExpandDepth="0" ImageSet="Simple" OnTreeNodePopulate="treeViewLocation_TreeNodePopulate" PopulateNodesFromClient="False" ShowExpandCollapse="false" RootNodeStyle-Width="100%">
    </asp:TreeView>
</ContentTemplate>
<Triggers>
    <asp:AsyncPostBackTrigger ControlID="treeViewLocation" EventName="TreeNodePopulate" />
</Triggers>
</asp:UpdatePanel>
</div>