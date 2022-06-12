<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FeaturedTours.ascx.cs" Inherits="CMS.Modules.TourManagement.Web.FeaturedTours" %>
<div class="featured_tour">
<ul>
<asp:Repeater ID="rptTours" runat="server" OnItemDataBound="rptTours_ItemDataBound">
    <ItemTemplate>
    <li>
        <asp:HyperLink ID="hplTour" runat="server"><%# DataBinder.Eval(Container.DataItem,"Name") %></asp:HyperLink><br />
    </li>    
    </ItemTemplate>
</asp:Repeater>
    <li id="liNeedASection" runat="server" visible="false">
        If you are administrator, please config section connection for this
    </li>
</ul>
</div>