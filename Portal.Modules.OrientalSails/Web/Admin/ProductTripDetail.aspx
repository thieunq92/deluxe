<%@ Page Title="" Language="C#" MasterPageFile="Popup.Master" AutoEventWireup="true" CodeBehind="ProductTripDetail.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.ProductTripDetail" %>

<asp:Content ID="Content2" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="row">
        <div class="col-xs-1">
        </div>
        <div class="col-xs-11">
            <asp:Button ID="btnCancel" Visible="False" Text="Cancel" runat="server" OnClick="buttonCancel_Clicl"
                CssClass="btn btn-primary" />
            <button id="btn-print" onclick="printHTML();">Print</button>
            <button id="btn-export" onclick="exportHTML();">
                Export to word
                doc</button>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-xs-1">
            <asp:HiddenField runat="server" ID="hidTripName" />
        </div>
        <div class="col-xs-11" id="source-html">
            <table class="table table-borderless">
                <tr>
                    <td>
                        <asp:Label ID="labelName" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="textBoxName" runat="server"></asp:Label>
                    </td>
                    <td>
                        <%= base.GetText("textTripCode") %>
                    </td>
                    <td>
                        <asp:Label ID="txtTripCode" runat="server" maxlength="5"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="labelNumberOfDay" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="textBoxNumberOfDay" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="labelNumberOfOptions" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Literal ID="litNumberOfOptions" runat="server">
                        </asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td>Org</td>
                    <td>
                        <asp:Literal ID="litOrganizations" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td>Số khách tối thiểu</td>
                    <td>
                        <asp:Label ID="txtNumberCustomerMin" runat="server"></asp:Label>
                    </td>
                    <td>Thời gian đặt trước tối thiểu (giờ)</td>
                    <td>
                        <asp:Label ID="txtTimeCreateBookingMin" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>

            <div class="advancedinfo">
                <h4>
                    <asp:Label ID="labelDescription" runat="server"></asp:Label>
                </h4>
                <asp:Literal runat="server" ID="litDescription"></asp:Literal>
            </div>
            <div class="advancedinfo">
                <h4>
                    <asp:Label ID="labelItinerary" runat="server"></asp:Label>
                </h4>
                <asp:Literal runat="server" ID="litItinerary"></asp:Literal>

            </div>
            <div class="advancedinfo">
                <h4>
                    <asp:Label ID="labelInclusions" runat="server"></asp:Label>
                </h4>
                <asp:Literal runat="server" ID="litInclusions"></asp:Literal>
            </div>
            <div class="advancedinfo">
                <h4>
                    <asp:Label ID="labelExclusions" runat="server"></asp:Label>
                </h4>
                <asp:Literal runat="server" ID="litExclusions"></asp:Literal>
            </div>
            <div class="advancedinfo">
                <h4>
                    <asp:Label ID="labelWhatToTake" runat="server"></asp:Label>
                </h4>
                <asp:Literal runat="server" ID="litWhatToTake"></asp:Literal>
            </div>
            <div class="advancedinfo">
                <h4>Trip image
                </h4>
                <asp:Image runat="server" Width="150px" ID="imgTripImage" />
            </div>
            <div class="basicinfo">
                <h4>Các dịch vụ sử dụng trên tour</h4>
                <table>
                    <asp:Repeater ID="rptCostTypes" runat="server" OnItemDataBound="rptCostTypes_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hiddenId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                    <asp:Label ID="labelCostType" runat="server" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
    </div>
    <script>
        function exportHTML() {
            var header = "<html xmlns:o='urn:schemas-microsoft-com:office:office' " +
                "xmlns:w='urn:schemas-microsoft-com:office:word' " +
                "xmlns='http://www.w3.org/TR/REC-html40'>" +
                "<head><meta charset='utf-8'><title>Export HTML to Word Document with JavaScript</title></head><body>";
            var footer = "</body></html>";
            var sourceHtml = header + document.getElementById("source-html").innerHTML + footer;

            var source = 'data:application/vnd.ms-word;charset=utf-8,' + encodeURIComponent(sourceHtml);
            var fileDownload = document.createElement("a");
            document.body.appendChild(fileDownload);
            fileDownload.href = source;
            fileDownload.download = document.getElementById('<%=hidTripName.ClientID%>').value +'.doc';
            fileDownload.click();
            document.body.removeChild(fileDownload);
        }

        function printHTML() {
            window.print();
        }
    </script>
</asp:Content>
