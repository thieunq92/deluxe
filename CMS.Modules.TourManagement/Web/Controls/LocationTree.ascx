<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LocationTree.ascx.cs" Inherits="CMS.Modules.TourManagement.Web.Controls.LocationTree" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
    <asp:TreeView ID="treeViewLocation" runat="server" ExpandDepth="0" ImageSet="Simple" OnTreeNodePopulate="treeViewLocation_TreeNodePopulate" PopulateNodesFromClient="False">
    </asp:TreeView>
</ContentTemplate>
<Triggers>
    <asp:AsyncPostBackTrigger ControlID="treeViewLocation" EventName="TreeNodePopulate" />
</Triggers>
</asp:UpdatePanel>