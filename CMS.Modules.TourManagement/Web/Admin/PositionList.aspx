<%@ Page Language="C#" MasterPageFile="TourMaster.Master" AutoEventWireup="true"
    Codebehind="PositionList.aspx.cs" Inherits="CMS.Modules.TourManagement.Web.Admin.PositionList"
    Title="Untitled Page" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <asp:Panel ID="panelContent" runat="server">
        <fieldset>
            <legend>
                <img src="../Images/location.gif" align="absMiddle" /><asp:Label ID="labelPositionList"
                    runat="server"></asp:Label></legend>
            <div class="settinglist">
                <div class="basicinfo">
                    <asp:Label runat="server" ID="lblParentLocation"></asp:Label>
                    <asp:HyperLink runat="server" ID="hplParentLocation"></asp:HyperLink><br />
                    <asp:Label runat="server" ID="lblError" Visible="false" Text="This location has some hotels in it, you must move the hotels or delete them first!"
                        Style="color: red;"></asp:Label>
                </div>
                <div class="data_grid">
                    <asp:UpdatePanel ID="updatePanelLocation" runat="server">
                        <ContentTemplate>
                            <cc2:ReorderList ID="reorderListLocation" runat="server" DragHandleAlignment="Left"
                                ItemInsertLocation="Beginning" DataKeyField="Id" SortOrderField="" CssClass="reorderlist"
                                AllowReorder="true" OnItemDataBound="reorderListLocation_ItemDataBound" OnItemCommand="reorderListLocation_ItemCommand"
                                OnItemReorder="reorderListLocation_ItemReorder" PostBackOnReorder="true">
                                <ItemTemplate>
                                    <span style="float: right;">
                                        <asp:ImageButton runat="server" ID="btnView" ToolTip='View' ImageUrl="../Images/image.png"
                                            AlternateText="Xem" ImageAlign="AbsMiddle" CssClass="image_button16" CommandName="View"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
                                        <asp:HyperLink runat="server" ID="hplEdit" ToolTip='Edit' NavigateUrl="#"><img class="image_button16" align="absmiddle" src="../Images/edit.gif" alt="Sửa"/></asp:HyperLink>
                                        <asp:ImageButton runat="server" ID="btnDelete" ToolTip='Delete' ImageUrl="../Images/delete_file.gif"
                                            AlternateText="Xóa" ImageAlign="AbsMiddle" CssClass="image_button16" CommandName="Delete"
                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>' />
                                        <asp:CheckBox runat="server" ID="chkSelected" />
                                        <cc2:ConfirmButtonExtender ID="ConfirmButtonExtenderDelete" runat="server" TargetControlID="btnDelete"
                                            ConfirmText='<%# base.GetText("messageConfirmDeleteLocation") %>'>
                                        </cc2:ConfirmButtonExtender>
                                    </span><span>
                                        <div style="width: 200px; float: left;">
                                            <asp:HyperLink runat="server" ID="hplLocation" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'>
                                            </asp:HyperLink>
                                            <asp:Label runat="server" ID="lblSubLocation"></asp:Label>
                                        </div>
                                        <strong>
                                            <asp:Label runat="server" ID="lblID"></asp:Label></strong>
                                        <br />
                                    </span>
                                </ItemTemplate>
                                <ReorderTemplate>
                                </ReorderTemplate>
                                <DragHandleTemplate>
                                    <div class="dragHandle">
                                    </div>
                                </DragHandleTemplate>
                            </cc2:ReorderList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="settings_grouplist">
                <ul>
                    <li>
                        <asp:Label ID="lblNotFound" runat="server"></asp:Label>
                        <cc1:Pager ID="pagerLocation" runat="server" ControlToPage="reorderListLocation"
                            HideWhenOnePage="True" OnCacheEmpty="pagerLocation_CacheEmpty" OnPageChanged="pagerLocation_PageChanged"
                            PageSize="100" />
                    </li>
                </ul>
            </div>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="PanelPopup" runat="server" Style="display: none;" CssClass="modalPopup">
        <asp:Panel ID="PanelTitle" runat="server" Style="cursor: move; background-color: #DDDDDD;
            border: solid 1px Gray; color: Black">
            <div style="min-height: 20px;">
                <div style="text-align: right;">
                    <table>
                        <tr>
                            <td style="width: 90%;">
                                <center>
                                    <asp:Label runat="server" ID="lblLocationName"></asp:Label></center>
                            </td>
                            <td style="width: 10%;">
                                <a id="hideModalPopupViaClientButton" href="#">[X]</a>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </asp:Panel>
        <div style="padding-top: 10px;">
            <asp:Image runat="server" ID="imgLocationView" />
        </div>
    </asp:Panel>
    <asp:Button runat="server" ID="hiddenButtonPopup" Style="display: none" />

    <script type="text/javascript">       
        // Add click handlers for buttons to show and hide modal popup on pageLoad
        function pageLoad() {
            $addHandler($get("hideModalPopupViaClientButton"), 'click', hideModalPopupViaClient);
        }
              
        function hideModalPopupViaClient(ev) {
            ev.preventDefault();        
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.hide();
        }
    </script>

    <cc2:ModalPopupExtender ID="ModalPopupExtender" runat="server" BehaviorID="programmaticModalPopupBehavior"
        TargetControlID="hiddenButtonPopup" PopupControlID="PanelPopup" BackgroundCssClass="modalBackground"
        DropShadow="true" PopupDragHandleControlID="PanelTitle" />
</asp:Content>
