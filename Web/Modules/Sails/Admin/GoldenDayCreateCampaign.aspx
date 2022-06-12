<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="Main.Master"
    CodeBehind="GoldenDayCreateCampaign.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.GoldenDayCreateCampaign" %>

<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>GoldenDayCreateCampaign</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="wrapper wrapper__body wrapper__goldenday-create-campaign">
        <div id="createcampaign-panel" ng-controller="createCampaignController" ng-if="$root.campaign == null && $root.goldenDay == null">
            <fieldset
                ng-init="
                month = '<%= DateTime.Today.AddMonths(1).Month.ToString() %>';
                year = '<%= DateTime.Today.Year %>'
            ">
                <legend>Create campaign </legend>
                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-1 col__month-label --no-padding-left">
                            <label>Month</label>
                        </div>
                        <div class="col-xs-2 --no-padding-left col__month">
                            <asp:DropDownList runat="server" ID="ddlMonth" CssClass="form-control" ng-model="month">
                            </asp:DropDownList>
                        </div>
                        <div class="col-xs-1 col__year-label --no-padding-left">
                            <label>Year</label>
                        </div>
                        <div class="col-xs-2 --no-padding-left col__year">
                            <asp:DropDownList runat="server" ID="ddlYear" CssClass="form-control" ng-model="year">
                            </asp:DropDownList>
                        </div>
                        <div class="col-xs-1 --no-padding-left">
                            <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-primary" ng-click="save()" ng-disabled="buttonSaveDisabled" OnClick="btnSave_Click"
                                data-id="btnSave" data-uniqueid="<%# btnSave.UniqueID %>" data-clientid="<%# btnSave.ClientID%>" />
                        </div>
                    </div>
                </div>
            </fieldset>
            <input type="hidden" value="{{month}}" name="month" />
            <input type="hidden" value="{{year}}" name="year" />
        </div>

        <div id="createdate-panel" ng-controller="createPolicyController" ng-if="$root.campaign != null || $root.goldenDay != null">
            <input type="hidden" value="" name="month" ng-value="$root.campaign.Month" />
            <input type="hidden" value="" name="year" ng-value="$root.campaign.Year" />
            <input type="hidden" data-clientid="<%= btnSave.ClientID%>" data-uniqueid="<%= btnSave.UniqueID %>" data-id="btnSave">
            <fieldset>
                <legend>{{$root.campaign.Name}}</legend>
                <div class="row">
                    <div class="col-xs-3 --no-padding-left" style="border-right: 1px solid #ccc;">
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div id="upCruiseAvailable">
                                    <table class="table table-bordered table-common" style="border-right: none; border-top: none; border-bottom: none">
                                        <tr class="header active">
                                            <th>Date</th>
                                            <th>Total pax</th>
                                        </tr>
                                        <asp:Repeater ID="rptTotalPaxByDate" runat="server" OnItemDataBound="rptTotalPaxByDate_ItemDataBound">
                                            <ItemTemplate>
                                                <tr>
                                                    <td ng-class="{'--goldenday-select':isSelected('<%# ((DateTime)Container.DataItem).ToString("dd/MM/yyyy")%>')}"><%# ((DateTime)Container.DataItem).ToString("dd/MM/yyyy")%></td>
                                                    <td ng-class="{'--goldenday-select':isSelected('<%# ((DateTime)Container.DataItem).ToString("dd/MM/yyyy")%>')}">
                                                        <asp:Literal runat="server" ID="ltrTotalPax"></asp:Literal></td>
                                                    <td style='width: 10%; border: none'>
                                                        <button type="button" class="btn --goldenday-select" ng-click="addGoldenDay('<%# ((DateTime)Container.DataItem).ToString("dd/MM/yyyy")%>',null)"
                                                            ng-hide="isSelected('<%# ((DateTime)Container.DataItem).ToString("dd/MM/yyyy")%>')">
                                                            Select</button>
                                                        <button type="button" class="btn btn-default" ng-click="removeGoldenDay('<%# ((DateTime)Container.DataItem).ToString("dd/MM/yyyy")%>',null)"
                                                            ng-hide="!isSelected('<%# ((DateTime)Container.DataItem).ToString("dd/MM/yyyy")%>')">
                                                            UnSelect</button>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="col-xs-9">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-xs-12 --no-padding-leftright">
                                    <h4 class="--text-bold">Policy campaign</h4>
                                </div>
                            </div>
                        </div>
                        <button type="button" ng-click="add()" class="btn btn-primary" ng-show="$root.campaign.Policies.length <= 0">Add Policy </button>
                        <div ng-show="$root.campaign.Policies.length > 0">
                            <div class="form-group" ng-repeat="policy in $root.campaign.Policies">
                                <div class="row">
                                    <div class="col-xs-1 --no-padding-leftright" style="width: 2%">Trip</div>
                                    <div class="col-xs-5 --no-padding-right">
                                        <asp:DropDownList ID="ddlTrip" runat="server" CssClass="form-control" AppendDataBoundItems="true" ng-model="policy.Trip.Id"
                                            convert-to-string>
                                            <asp:ListItem Value="0" Text="-- Trip --"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-xs-1 --no-padding-right" style="width: 6%">
                                        Adult
                                    </div>
                                    <div class="col-xs-2 --no-padding-right">
                                        <asp:TextBox ID="txtAdultPrice" runat="server" CssClass="form-control" placeholder="Adult" data-control="inputmask"
                                            data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true"
                                            ng-model="policy.Adult"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-1 --no-padding-right" style="width: 6%">
                                        Child
                                    </div>
                                    <div class="col-xs-2 --no-padding-right">
                                        <asp:TextBox ID="txtChildPrice" runat="server" CssClass="form-control" placeholder="Child" data-control="inputmask"
                                            data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true"
                                            ng-model="policy.Child"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-2" style="width: auto">
                                        <a href="" ng-click="delete($index)">
                                            <i class="fa fa-lg fa-trash text-danger"
                                                data-toggle="tooltip" data-placement="top" title="Delete policy"></i>
                                        </a>
                                        <a href="" ng-click="add()">
                                            <i class="fa fa-lg fa-plus-circle text-success"
                                                data-toggle="tooltip" data-placement="top" title="Add policy"></i></a>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <button type="button" ng-click="addPolicyGoldenDay()" class="btn btn-primary" ng-show="$root.policies != null && $root.policies.length <= 0">Add Policy </button>
                        <div ng-show="$root.policies != null && $root.policies.length > 0">
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-xs-12 --no-padding-leftright">
                                        <h4 class="--text-bold">Policy {{$root.goldenDay.Date | date:'dd/MM/yyyy'}}</h4>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group" ng-repeat="policy in $root.policies">
                                <div class="row">
                                    <div class="col-xs-1 --no-padding-leftright" style="width: 2%">Trip</div>
                                    <div class="col-xs-5 --no-padding-right">
                                        <asp:DropDownList ID="ddlTripGoldenDay" runat="server" CssClass="form-control" AppendDataBoundItems="true" ng-model="policy.Trip.Id"
                                            convert-to-string>
                                            <asp:ListItem Value="0" Text="-- Trip --"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-xs-1 --no-padding-right" style="width: 6%">
                                        Adult
                                    </div>
                                    <div class="col-xs-2 --no-padding-right">
                                        <asp:TextBox ID="txtAdultGoldenDay" runat="server" CssClass="form-control" placeholder="Adult" data-control="inputmask"
                                            data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true"
                                            ng-model="policy.Adult"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-1 --no-padding-right" style="width: 6%">
                                        Child
                                    </div>
                                    <div class="col-xs-2 --no-padding-right">
                                        <asp:TextBox ID="txtChildGoldenDay" runat="server" CssClass="form-control" placeholder="Child" data-control="inputmask"
                                            data-inputmask="'alias': 'decimal', 'groupSeparator': ',', 'autoGroup': true"
                                            ng-model="policy.Child"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-2" style="width: auto">
                                        <a href="" ng-click="deletePolicyGoldenDay($index)">
                                            <i class="fa fa-lg fa-trash text-danger"
                                                data-toggle="tooltip" data-placement="top" title="Delete policy"></i>
                                        </a>
                                        <a href="" ng-click="addPolicyGoldenDay()">
                                            <i class="fa fa-lg fa-plus-circle text-success"
                                                data-toggle="tooltip" data-placement="top" title="Add policy"></i></a>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group" ng-show="$root.policies.length > 0 || $root.campaign.Policies.length > 0">
                            <div class="row">
                                <div class="col-xs-12 col__button-save --no-padding-left --text-right">
                                    <button type="button" class="btn btn-primary" ng-click="save()" ng-disabled="buttonSaveDisabled">Save</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Scripts" runat="server" ContentPlaceHolderID="Scripts">
    <script type="text/javascript" src="/modules/sails/admin/goldendaycreatecampaigncontroller.js"></script>
    <script>

    </script>
</asp:Content>

