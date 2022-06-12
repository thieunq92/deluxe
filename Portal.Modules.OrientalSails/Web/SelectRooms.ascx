<%@ Control Language="C#" AutoEventWireup="true" Codebehind="SelectRooms.ascx.cs"
    Inherits="Portal.Modules.OrientalSails.Web.SelectRooms" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
    <table style="width: 100%;">
        <asp:Repeater ID="rptRoomClass" runat="server" OnItemDataBound="rptRoomClass_ItemDataBound">
            <HeaderTemplate>
                <tr>
                    <th>
                        <%#base.GetText("labelRoomClass") %>
                    </th>
                    <th>
                        <%#base.GetText("labelRoomType")%>
                    </th>
                    <th>
                        <%#base.GetText("labelAvailable")%>
                    </th>
                    <th>
                        <%#base.GetText("labelSelect")%>
                    </th>
                </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label ID="labelRoomClassId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Id") %>'
                    Style="display: none"></asp:Label>
                <asp:Repeater ID="rptRoomType" runat="server" OnItemDataBound="rptRoomType_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Label ID="label_RoomClass" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="label_RoomType" runat="server"></asp:Label>
                                <asp:Label ID="labelRoomTypeId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Id") %>'
                                    Style="display: none"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="label_Avaliable" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSelect" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    <asp:Repeater ID="rptExtraOption" runat="server" OnItemDataBound="rptExtraOption_ItemDataBound">
        <ItemTemplate>
                <asp:CheckBox ID="checkBoxExtra" runat="server" Checked="true"/>
                <asp:HiddenField ID="hiddenId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"Id") %>' />            
        </ItemTemplate>
    </asp:Repeater>
    <br/>
    <center>
    <asp:Button ID="buttonSubmit" runat="server" OnClick="buttonSubmit_Click" />
    <asp:Button ID="buttonSelectRoom" runat="server" OnClick="buttonSelectRoom_Click"
        Text="Room" />
    </center>
