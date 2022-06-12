<%@ Page Language="C#" AutoEventWireup="true" Codebehind="AlbumSelector.aspx.cs"
    Inherits="CMS.Modules.Gallery.Web.AlbumSelector" %>

<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Album selector</title>
</head>
<body>
    <form id="form1" runat="server">
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
                    <div>
                        <table class="mytable" style="width: 100%; font-size: 12px;">
                            <asp:Repeater ID="rptAlbums" runat="server">
                                <HeaderTemplate>
                                    <tr>
                                        <th>
                                            Album Id
                                        </th>
                                        <th>
                                            Name
                                        </th>
                                        <th>
                                            Photo count
                                        </th>
                                        <th>
                                            Action
                                        </th>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="lblId" Text='<%# DataBinder.Eval(Container.DataItem,"Id") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <a href="#" onclick="window.opener.ProcessModalData('<%# DataBinder.Eval(Container.DataItem,"Id") %>','<%# DataBinder.Eval(Container.DataItem,"Title") %>')">
                                                <%# DataBinder.Eval(Container.DataItem,"Title") %>
                                            </a>
                                        </td>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem,"PhotoCount") %>
                                        </td>
                                        <td>
                                            <asp:HyperLink runat="server" ID="hplView" Target="_blank" Text='View album'></asp:HyperLink>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                    <div class="pager">
                        <svc:Pager ID="pagerAlbums" runat="server" ControlToPage="rptAlbums" OnPageChanged="pagerAlbums_PageChanged"
                            PageSize="10" />
                    </div>
                </div>
            </div>
        </fieldset>
    </form>
</body>
</html>
