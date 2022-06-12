<%@ Page Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="SailsTripList.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.SailsTripList" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="row">
        <div class="col-xs-12">
            <h3><asp:Label runat="server" ID="titleSailsTripList"/></h3>
            <table class="table table-bordered table-common">
                <asp:Repeater ID="rptTripList" runat="server" OnItemDataBound="rptTripList_ItemDataBound" OnItemCommand="rptTripList_ItemCommand">
                    <HeaderTemplate>
                        <tr class="active">
                            <th style="width: 200px;">
                                <%#base.GetText("labelName") %>
                            </th>
                            <th style="width: 100px;">
                                <%#base.GetText("labelNumberOfDay") %>
                            </th>
                            <th style="width: 100px;">
                                <%#base.GetText("labelNumberOfOptions")%>
                            </th>
                            <th>
                                <%#base.GetText("textPriceConfig")%>
                            </th>
                            <th style="width: 100px;"></th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="item">
                            <td class="--text-left">
                                <asp:HyperLink ID="hyperLink_Name" runat="server"></asp:HyperLink>
                            </td>
                            <td>
                                <asp:Label ID="label_NumberOfDays" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="label_NumberofOption" runat="server"></asp:Label>
                            </td>
                            <td>
                                <table style="width: auto;">
                                    <asp:Repeater ID="rptOptions" runat="server" OnItemDataBound="rptOptions_ItemDataBound">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:Literal ID="litOption" runat="server"></asp:Literal></td>
                                                <asp:Repeater ID="rptCruises" runat="server" OnItemDataBound="rptCruises_ItemDataBound">
                                                    <ItemTemplate>
                                                        <td>
                                                            <asp:HyperLink ID="hplCruise" runat="server"></asp:HyperLink></td>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <td>
                                                            <asp:HyperLink ID="hplCruise" runat="server"></asp:HyperLink></td>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                                <!--<asp:DropDownList ID="ddlOption" runat="server"></asp:DropDownList>
                                    <asp:ImageButton ID="imageButtonPrice" runat="server" ToolTip="Price" AlternateText="Price" ImageAlign="AbsMiddle" CssClass="image_button16" CommandName="Price" ImageUrl="../Images/price.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'/>-->
                            </td>
                            <td>
                                <asp:HyperLink ID="hyperLinkEdit" runat="server">
                                    <asp:Image ID="imageEdit" runat="server" ImageAlign="AbsMiddle" AlternateText="Edit" CssClass="image_button16" ImageUrl="../Images/edit.gif" />
                                </asp:HyperLink>
                                <asp:ImageButton runat="server" ID="imageButtonDelete" ToolTip='Delete' ImageUrl="../Images/delete_file.gif"
                                    AlternateText='Delete' ImageAlign="AbsMiddle" CssClass="image_button16" CommandName="Delete"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
                                <ajax:ConfirmButtonExtender ID="ConfirmButtonExtenderDelete" runat="server" TargetControlID="imageButtonDelete"
                                    ConfirmText='<%# base.GetText("messageConfirmDelete") %>'>
                                </ajax:ConfirmButtonExtender>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
</asp:Content>
