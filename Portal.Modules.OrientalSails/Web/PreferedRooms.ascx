<%@ Control Language="C#" AutoEventWireup="true" Codebehind="PreferedRooms.ascx.cs"
    Inherits="Portal.Modules.OrientalSails.Web.PreferedRooms" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
Please select:
<ul>
    <asp:Repeater ID="rptRooms" runat="server">
        <ItemTemplate>
            <li>
                <asp:Label ID="labelRoom" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem") %>'></asp:Label></li>
        </ItemTemplate>
    </asp:Repeater>
</ul>
<asp:Panel ID="panelWarning" runat="server" Visible="false">
    <strong><em>Warning: the person stay in shared room can not choose his room. We will locate him with someone else.</em></strong>
</asp:Panel>
<div id="roomMap">
    <asp:UpdatePanel ID="udpRoomMap" runat="server">
        <ContentTemplate>
            <asp:Repeater ID="rptRoomList" runat="server" OnItemCommand="rptRoomList_itemCommand"
                OnItemDataBound="rptRoomList_ItemDataBound">
                <ItemTemplate>
                    <div id='room<%#DataBinder.Eval(Container.DataItem,"Id")%>'>
                        <asp:ImageButton ID="imageRoom" runat="server" CommandName="choose" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"Id") %>' />
                        <asp:Literal ID="litRoom" runat="server" Visible="false"></asp:Literal>
                        <div id="roomcontent" runat="server" visible="false">
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="updatePanelSelectedRoom" runat="server">
    <ContentTemplate>
        <asp:ListBox ID="listSelectedRooms" runat="server" Visible="false"></asp:ListBox>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:Button ID="buttonSubmit" runat="server" OnClick="buttonSubmit_Click" />