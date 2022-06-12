<%@ Page Title="" Language="C#" MasterPageFile="SailsMaster.Master" AutoEventWireup="true"
    CodeBehind="PaymentChecking.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.PaymentChecking" %>

<%@ Register TagPrefix="svc" Namespace="CMS.ServerControls" Assembly="CMS.ServerControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <fieldset>
        <legend></legend>
        <svc:Popup ID="popupManager" runat="server">
        </svc:Popup>
        <div class="data_grid">
            <table>
                <tr>
                    <th>
                        Booking code
                    </th>
                    <th>
                        Date
                    </th>
                    <th>
                        Sale in charge
                    </th>
                    <th>
                        Partner
                    </th>
                    <th>
                        Service
                    </th>
                    <th>
                        <asp:HyperLink runat="server" ID="hplGuide"></asp:HyperLink>
                        Guide remaining
                    </th>
                    <th>
                        Partner remaining
                    </th>
                    <th>
                        Action
                    </th>
                </tr>
                <asp:Repeater ID="rptBookings" runat="server" OnItemDataBound="rptBookings_ItemDataBound">
                    <ItemTemplate>
                        <tr id="trRow" runat="server">
                            <td>
                                <asp:HyperLink runat="server" ID="hplBookingCode"></asp:HyperLink>
                            </td>
                            <td>
                                <asp:HyperLink runat="server" ID="hplDate"></asp:HyperLink>
                            </td>
                            <td>
                                <asp:HyperLink runat="server" ID="hplSale"></asp:HyperLink>
                            </td>
                            <td>
                                <asp:HyperLink runat="server" ID="hplPartner"></asp:HyperLink>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="litService"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="litGuideRemain"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="litPartnerRemain"></asp:Literal>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" ID="lbtGuideConfirm" Visible="False" OnClick="lbtGuideConfirm_Click"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'>[Guide Confirm]</asp:LinkButton>
                                <asp:LinkButton runat="server" ID="lbtAgencyConfirm" Visible="False" OnClick="lbtAgencyConfirm_Click"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'>[Partner Confirm]</asp:LinkButton>
                                <a id="aPayment" runat="server" style="cursor: pointer;">[Payment]</a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
        <div class="pager">
            <svc:Pager ID="pagerBookings" runat="server" HideWhenOnePage="true" ControlToPage="rptBookings"
                PageSize="100" PagerLinkMode="HyperLinkQueryString" />
        </div>
    </fieldset>
</asp:Content>
