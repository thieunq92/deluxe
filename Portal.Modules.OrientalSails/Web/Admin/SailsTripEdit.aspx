<%@ Page Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true"
    CodeBehind="SailsTripEdit.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.SailsTripEdit"
    Title="Untitled Page" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls.FileUpload"
    TagPrefix="cc2" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="cc1" %>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="row">
        <div class="col-xs-12">
            <table class="table table-borderless table-common" style="width: 66%">
                <tr>
                    <td class="--text-left">
                        <asp:Label ID="labelName" runat="server"></asp:Label>
                    </td>
                    <td class="--text-left">
                        <asp:TextBox ID="textBoxName" runat="server" CssClass="form-control"></asp:TextBox>
                    </td>
                    <td class="--text-left">
                        <%= base.GetText("textTripCode") %>
                    </td>
                    <td class="--text-left">
                        <asp:TextBox ID="txtTripCode" runat="server" MaxLength="5" CssClass="form-control"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="--text-left">
                        <asp:Label ID="labelNumberOfDay" runat="server"></asp:Label>
                    </td>
                    <td class="--text-left">
                        <asp:TextBox ID="textBoxNumberOfDay" runat="server" CssClass="form-control"></asp:TextBox>
                    </td>
                    <td class="--text-left">
                        <asp:Label ID="labelNumberOfOptions" runat="server"></asp:Label>
                    </td>
                    <td class="--text-left">
                        <asp:DropDownList ID="ddlNumberOfOptions" runat="server" CssClass="form-control">
                            <asp:ListItem Text="0" Value="0"></asp:ListItem>
                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="--text-left">Org</td>
                    <td class="--text-left">
                        <asp:DropDownList ID="ddlOrganizations" runat="server" CssClass="form-control"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="--text-left">Số khách tối thiểu</td>
                    <td class="--text-left">
                        <asp:TextBox ID="txtNumberCustomerMin" runat="server" CssClass="form-control"></asp:TextBox>
                    </td>
                    <td class="--text-left">Thời gian đặt trước tối thiểu (giờ)</td>
                    <td class="--text-left">
                        <asp:TextBox ID="txtTimeCreateBookingMin" runat="server" CssClass="form-control"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <h3>Các dịch vụ sử dụng trên tour</h3>
            <table>
                <asp:Repeater ID="rptCostTypes" runat="server" OnItemDataBound="rptCostTypes_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td style="padding: 0 0 0 50px">
                                <asp:HiddenField ID="hiddenId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                <asp:CheckBox ID="chkCostType" runat="server" CssClass="checkbox" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="button" />
        </div>
    </div>
</asp:Content>
