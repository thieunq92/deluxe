<%@ Page Title="" Language="C#" MasterPageFile="MainAgency.Master" AutoEventWireup="true" CodeBehind="ProductTrip.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.ProductTrip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="AdminContent" runat="server">

    <div class="row">
        <div class="col-xs-1">
        </div>
        <div class="col-xs-11">
            <table class="table table-borderless">
                <asp:repeater id="rptTripList" runat="server" onitemdatabound="rptTripList_ItemDataBound" onitemcommand="rptTripList_ItemCommand">
                    <HeaderTemplate>
                        <tr class="header">
                            <th style="width: 200px;">
                                <%#base.GetText("labelName") %>
                            </th>
                            <th style="width: 100px;">
                                <%#base.GetText("labelNumberOfDay") %>
                            </th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="item">
                            <td>
                                <asp:HyperLink ID="hyperLink_Name" runat="server"></asp:HyperLink>
                            </td>
                            <td>
                                <asp:Label ID="label_NumberOfDays" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:repeater>
            </table>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script>
        function openPopup(pageUrl, title, h, w) {
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            var targetWin = window.open(pageUrl, title, 'toolbar=no,scrollbars=yes, location=no, directories=no, status=no, menubar=no, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
            targetWin.focus();
            return targetWin;
        }
    </script>
</asp:Content>
