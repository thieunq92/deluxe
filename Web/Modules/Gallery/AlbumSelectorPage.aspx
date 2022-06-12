<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlbumSelectorPage.aspx.cs" Inherits="CMS.Modules.Gallery.Web.AlbumSelectorPage" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>User selector</title>        
</head>
<body>
    <form id="form2" runat="server"> 
        <link id="linkCss" rel="stylesheet" href="/Admin/Css/Style.css" type="text/css" />                   
        <asp:ScriptManager Id="scriptManager" runat="server">
        </asp:ScriptManager>
        <fieldset>
            <div class="bitcorp" style="width: 800px; font-size: 12px; font-family: Arial;">
                <div class="settinglist" style="width: 700px;">
                    <div class="search_panel">
                        <table>
                            <td>
                                <span style="font-size: 12px;">Name</span></td>
                            <td>
                                <asp:TextBox ID="textBoxName" runat="server"></asp:TextBox></td>
                        </table>
                    </div>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                        CssClass="button" />
                    <div class="data_table">
                        <asp:UpdatePanel ID="updatePanelUsers" runat="server">
                            <ContentTemplate>
                            <div class="data_grid">
                                <div class="pager">
                                <svc:Mirror ID="mirrorPager" runat="server" ControlIDToMirror="pagerUsers" />
                                </div>
                                <table>
                                <asp:Repeater ID="rptAlbums" runat="server" OnItemDataBound="rptAlbums_ItemDataBound">
                                    <HeaderTemplate>
                                        <tr>
                                            <th>User name</th>
                                            <th>Full name</th>
                                        </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td><a id="processData" runat="server" href="#"><%# DataBinder.Eval(Container.DataItem,"Title") %></a></td>                                            
                                            <td><%# DataBinder.Eval(Container.DataItem,"PhotoCount") %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                </table>
                                <div class="pager">
                                <svc:Pager ID="pagerAlbums" runat="server" ControlToPage="rptAlbums" OnPageChanged="pagerAlbums_PageChanged"></svc:Pager>
                                </div>
                            </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="pager">
                    </div>
                </div>
            </div>
        </fieldset>
    </form>
</body>
</html>