<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="Main.Master"
    CodeBehind="ViewMeetings.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.ViewMeetings" %>

<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Portal.Modules.OrientalSails.Domain" %>
<%@ Import Namespace="System.Linq" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>View meeting</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="form-group">
        <div class="row">
            <div class="col-xs-12">
                <span>From</span>
                <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control" data-control="datetimepicker" Style="display: inline-block; width: auto"></asp:TextBox>
                <span>To</span>
                <asp:TextBox ID="txtTo" runat="server" CssClass="form-control" data-control="datetimepicker" Style="display: inline-block; width: auto">
                </asp:TextBox>
                <asp:PlaceHolder runat="server" ID="plhSales">
                    <span style="font-size: 12px">Sales</span>
                    <asp:DropDownList runat="server" ID="ddlSales" CssClass="form-control" Style="display: inline-block; width: auto" />
                </asp:PlaceHolder>
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-12">
                <asp:Button runat="server" ID="btnView" CssClass="btn btn-primary"
                    Text="View" OnClick="btnView_OnClick"></asp:Button>
                <asp:TextBox ID="txtPageSize" runat="server" CssClass="form-control" Style="display: inline-block; width: 4%">
                </asp:TextBox>
                <span>meetings/page</span>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <table class="table table-bordered table-common">
                <asp:Repeater ID="rptMeetings" runat="server" OnItemDataBound="rptMeetings_OnItemDataBound">
                    <HeaderTemplate>
                        <tr class="header">
                            <th width="7%">
                                <asp:LinkButton runat="server" ID="lbtUpdateTime" OnClick="lbtUpdateTime_OnClick">
                                    Update time</asp:LinkButton>
                                <asp:Image runat="server" ID="imgSortUtStatus" Width="8px" Visible="False" />
                            </th>
                            <th width="8%">
                                <asp:LinkButton runat="server" ID="lbtDateMeeting" OnClick="lbtDateMeeting_OnClick">
                                    Date meeting</asp:LinkButton>
                                <asp:Image runat="server" ID="imgSortDmStatus" Width="8px" Visible="False" />
                            </th>
                            <th width="10%" runat="server" id="thSales">Sales
                            </th>
                            <th width="10%">Meeting with
                            </th>
                            <th width="7%">Position
                            </th>
                            <th width="20%">Agency
                            </th>
                            <th>Note
                            </th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Literal runat="server" ID="ltrUpdateTime" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="ltrDateMeeting" />
                            </td>
                            <td runat="server" id="tdSales">
                                <asp:Literal runat="server" ID="ltrSale" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="ltrName" />
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="ltrPosition" />
                            </td>
                            <td class="--text-left">
                                <asp:Literal runat="server" ID="ltrAgency" />
                            </td>
                            <td class="--text-left">
                                <asp:Literal runat="server" ID="ltrNote" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <%if (((IEnumerable<Activity>)rptMeetings.DataSource).Count() <= 0)
                  {
                %>
                    <tr>
                        <td colspan="100%">No record found</td>
                    </tr>
                <%}%>
            </table>
            <asp:Button runat="server" ID="btnExportMeetings" CssClass="btn btn-primary"
                Text="Export"
                OnClick="btnExportMeetings_OnClick"></asp:Button>
            <div class="pager">
                <svc:Pager ID="pagerMeetings" runat="server" HideWhenOnePage="True" ShowTotalPages="True"
                    ControlToPage="rptMeetings" OnPageChanged="pagerMeetings_OnPageChanged" PageSize="20" />
            </div>
        </div>
    </div>
    <fieldset>


        <div class="basicinfo" style="width: 100%; border: none">
        </div>
    </fieldset>

</asp:Content>
