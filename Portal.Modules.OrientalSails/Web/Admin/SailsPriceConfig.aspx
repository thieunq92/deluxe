<%@ Page Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true"
    CodeBehind="SailsPriceConfig.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.SailsPriceConfig"
    Title="Untitled Page" %>

<%@ Import Namespace="Portal.Modules.OrientalSails.Domain" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Price config</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="row">
        <div class="col-xs-12">
            <h4 class="page-header --text-bold">
                <asp:Label ID="titleSailsPriceConfig" runat="server"></asp:Label></h4>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <table class="table table-bordered table-common" style="width:30%">
                <tr class="active">
                    <th>Valid From
                    </th>
                    <th></th>
                </tr>
                <asp:Repeater runat="server" ID="rptValidFrom" OnItemDataBound="rptValidFrom_OnItemDataBound"
                    OnItemCommand="rptValidFrom_OnItemCommand">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblValidFrom" Text='<%#((DateTime)Eval("ValidFrom")).ToString("dd/MM/yyyy")%>' />
                            </td>
                            <td>
                                <asp:LinkButton runat="server" ID="lbtnEdit" CommandName="Edit"></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="lbtnDelete" CommandName="Delete" CommandArgument='<%#((DateTime)Eval("ValidFrom")).ToString("dd/MM/yyyy")%>'></asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <%if (((IList)rptValidFrom.DataSource).Count <= 0)
                  {
                %>
                <tr>
                    <td colspan="100%">No records found</td>
                </tr>
                <%}%>
            </table>
            <div class="pager" style="text-align: left; margin-top: 9px; margin-bottom: -20px;">
                <svc:Pager runat="server" ID="pgValidFrom" ControlToPage="rptValidFrom" HideWhenOnePage="true"
                    OnPageChanged="pgValidFrom_OnPageChanged" PageSize="5"></svc:Pager>
            </div>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-xs-1 --width-auto">
            Valid froms
        </div>
        <div class="col-xs-2">
            <asp:TextBox ID="textBoxStartDate" runat="server" CssClass="form-control" data-control="datetimepicker"></asp:TextBox>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-xs-12">
            <table class="table table-borderless table-common" style="width: auto">
                <tr>
                    <th width="10%">Public Price
                    </th>
                    <th width="10%">USD
                    </th>
                    <th width="10%">VND
                    </th>
                    <th></th>
                </tr>
                <tr>
                    <td class="--text-left">Adult
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtPriceAdultUSD" class="form-control"
                             data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'placeholder':'0'"/>
                        <asp:HiddenField runat="server" ID="hidPriceAdultUSD" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtPriceAdultVND" runat="server" class="form-control" 
                            data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'placeholder':'0'"/>
                        <asp:HiddenField runat="server" ID="hidPriceAdultVND" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td class="--text-left">Child
                    </td>
                    <td>
                        <asp:TextBox ID="txtPriceChildUSD" runat="server" class="form-control" 
                            data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'placeholder':'0'"/>
                        <asp:HiddenField runat="server" ID="hidPriceChildUSD" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtPriceChildVND" runat="server" class="form-control" 
                            data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'placeholder':'0'"/>
                        <asp:HiddenField runat="server" ID="hidPriceChildVND" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td class="--text-left">Baby
                    </td>
                    <td>
                        <asp:TextBox ID="txtPriceBabyUSD" runat="server" class="form-control" 
                            data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'placeholder':'0'" />
                        <asp:HiddenField runat="server" ID="hidPriceBabyUSD" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtPriceBabyVND" runat="server" class="form-control" 
                            data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'placeholder':'0'"/>
                        <asp:HiddenField runat="server" ID="hidPriceBabyVND" />
                    </td>
                    <td></td>
                </tr>
            </table>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-xs-12">
            <table class="table table-borderless table-common">
                <tr>
                    <th rowspan="2" style="width: 12%">Comission
                    </th>
                    <th colspan="2" style="text-align: center">Adult
                    </th>
                    <th colspan="2" style="text-align: center">Child
                    </th>
                    <th colspan="2" style="text-align: center">Baby
                    </th>
                </tr>
                <tr>
                    <th style="text-align: center">USD
                    </th>
                    <th style="text-align: center">VND
                    </th>
                    <th style="text-align: center">USD
                    </th>
                    <th style="text-align: center">VND
                    </th>
                    <th style="text-align: center">USD
                    </th>
                    <th style="text-align: center">VND
                    </th>
                </tr>
                <asp:Repeater runat="server" ID="rptCommission" OnItemDataBound="rptCommission_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <asp:Label runat="server" ID="AgencyLevelId" Text='<%# DataBinder.Eval(Container.DataItem,"Id") %>'
                                Style="display: none;"></asp:Label>
                            <asp:Label runat="server" ID="AgencyCommissionId" Style="display: none;"></asp:Label>
                            <td class="--text-left">
                                <asp:Label runat="server" ID="AgencyLevelName" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtAdultCommissionUSD" CssClass="form-control" 
                                    data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'placeholder':'0'"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hidAdultCommissionUSD"></asp:HiddenField>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtAdultCommissionVND" CssClass="form-control" 
                                    data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'placeholder':'0'"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hidAdultCommissionVND"></asp:HiddenField>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtChildCommissionUSD" CssClass="form-control" 
                                    data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'placeholder':'0'"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hidChildCommissionUSD"></asp:HiddenField>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtChildCommissionVND" CssClass="form-control" 
                                    data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'placeholder':'0'"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hidChildCommissionVND"></asp:HiddenField>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBabyCommissionUSD" CssClass="form-control" 
                                    data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'placeholder':'0'"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hidBabyCommissionUSD"></asp:HiddenField>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBabyCommissionVND" CssClass="form-control" 
                                    data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true, 'placeholder':'0'"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hidBabyCommissionVND"></asp:HiddenField>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <asp:Button ID="buttonSubmit" runat="server" CssClass="btn btn-primary" OnClick="buttonSubmit_Click" />
                <asp:Button ID="buttonCancel" runat="server" CssClass="btn btn-primary" OnClick="buttonCancel_Click" />
            </div>
        </div>
    </div>
</asp:Content>
