<%@ Control Language="C#" AutoEventWireup="true" Codebehind="Tours.ascx.cs" Inherits="CMS.Modules.TourManagement.Web.Tours" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<div>
    <div class="tour_list">
        <ul>
            <li class="pager"></li>
            <asp:Repeater ID="rptTours" runat="server" OnItemDataBound="rptTours_ItemDataBound">
                <ItemTemplate>
                    <li class="list_item">
                        <h4>
                            <asp:Image ID="imageListItem" runat="server" />
                            <asp:HyperLink ID="hyperLinkTourName" runat="server"></asp:HyperLink>
                        </h4>
                        <asp:Image ID="imageTour" runat="server" CssClass="tour_list_image" ImageAlign="left" />
                        <table style="width:400px">
                            <tr>
                                <td><asp:Label ID="labelLocation" runat="server"></asp:Label></td>
                                <td><asp:Label ID="labelDuration" runat="server"></asp:Label></td>
                            </tr>                            
                            <tr>
                                <td><asp:Label ID="labelTourType" runat="server"></asp:Label></td>
                                <td><asp:Label ID="labelTourGrade" runat="server"></asp:Label></td>
                            </tr>
                        </table>
                        <asp:Label ID="labelDescription" runat="server"></asp:Label><br />
                        <div style="clear: left;">
                        </div>
                    </li>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <li class="list_item_alternate">
                        <h4>
                            <asp:Image ID="imageListItem" runat="server" />
                            <asp:HyperLink ID="hyperLinkTourName" runat="server"></asp:HyperLink>
                        </h4>
                        <asp:Image ID="imageTour" runat="server" CssClass="tour_list_image" ImageAlign="left" />
                        <table style="width:400px">
                            <tr>
                                <td><asp:Label ID="labelLocation" runat="server"></asp:Label></td>
                                <td><asp:Label ID="labelDuration" runat="server"></asp:Label></td>
                            </tr>                            
                            <tr>
                                <td><asp:Label ID="labelTourType" runat="server"></asp:Label></td>
                                <td><asp:Label ID="labelTourGrade" runat="server"></asp:Label></td>
                            </tr>
                        </table>
                        <asp:Label ID="labelDescription" runat="server"></asp:Label><br />
                        <div style="clear: left;">
                        </div>
                    </li>
                </AlternatingItemTemplate>
            </asp:Repeater>
            <li>
                <svc:Pager ID="pagerTours" runat="server" HideWhenOnePage="false" PageSize="10" OnPageChanged="pagerTours_PageChanged"
                    ControlToPage="rptTours" />
            </li>
        </ul>
    </div>
</div>
