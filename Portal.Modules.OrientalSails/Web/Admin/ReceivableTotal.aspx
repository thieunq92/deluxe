<%@ Page Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true"
    CodeBehind="ReceivableTotal.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.ReceivableTotal" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Receivable/Payable summary
    </title>
</asp:Content>
<asp:Content ID="AdminContainer" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="row">
        <div class="col-xs-12">
            <div id="tabs" style="border: none">
                <ul style="border: none; border-bottom: 1px solid #ddd; background: none">
                    <li><a href="#tabs-1">Deluxe Group Tour</a></li>
                    <li><a href="#tabs-2">Hue Street Food</a></li>
                </ul>
                <div id="tabs-1">
                    <div class="data_grid">
                        <table class="table table-bordered table-common">
                            <tr class="active">
                                <th>No.</th>
                                <th>Agency name</th>
                                <th>Total receivable</th>
                            </tr>
                            <asp:Repeater ID="rptAgencies" runat="server" OnItemDataBound="rptAgencies_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:Literal ID="litNo" runat="server"></asp:Literal></td>
                                        <td class="--text-left">
                                            <asp:HyperLink ID="hplName" runat="server"></asp:HyperLink></td>
                                        <td class="--text-right">
                                            <asp:Literal ID="litTotal" runat="server"></asp:Literal></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <tr>
                                        <td colspan="2">GRAND TOTAL
                                        </td>
                                        <td class="--text-right">
                                            <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                </FooterTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                </div>
                <div id="tabs-2">
                    <div class="data_grid">
                        <table class="table table-bordered table-common">
                            <tr class="active">
                                <th>No.</th>
                                <th>Agency name</th>
                                <th>Total payable</th>
                            </tr>
                            <asp:Repeater ID="rptPayables" runat="server" OnItemDataBound="rptPayables_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:Literal ID="litNo" runat="server"></asp:Literal></td>
                                        <td class="--text-left">
                                            <asp:HyperLink ID="hplName" runat="server"></asp:HyperLink></td>
                                        <td class="--text-right">
                                            <asp:Literal ID="litTotal" runat="server"></asp:Literal></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <tr>
                                        <td colspan="2">GRAND TOTAL
                                        </td>
                                        <td class="--text-right">
                                            <asp:Literal ID="litTotal" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                </FooterTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnSelectedTab" runat="server" Value="0" />
</asp:Content>
<asp:Content runat="server" ID="Scripts" ContentPlaceHolderID="Scripts">
    <script>
        $(function () {
            $("#tabs").tabs({ 
                activate: function() {
                    var selectedTab = $('#tabs').tabs('option', 'active');
                    $("#<%= hdnSelectedTab.ClientID %>").val(selectedTab);
                },
                active: <%= hdnSelectedTab.Value %>
                });
        });
    </script>
</asp:Content>
