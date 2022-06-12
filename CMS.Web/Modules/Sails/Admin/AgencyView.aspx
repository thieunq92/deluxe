<%@ Page Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true"
    CodeBehind="AgencyView.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.AgencyView" %>

<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Agency view</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="row">
        <div class="col-xs-12">
            <h3 class="page-header">
                <asp:Literal runat="server" ID="litName1"></asp:Literal></h3>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-1">
                    <span class="--text-bold">Name</span>
                </div>
                <div class="col-xs-11">
                    <asp:Literal runat="server" ID="litName"></asp:Literal>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-1">
                    <span class="--text-bold">Role</span>
                </div>
                <div class="col-xs-11">
                    <asp:Literal runat="server" ID="litRole"></asp:Literal>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-1">
                    <span class="--text-bold">Level</span>
                </div>
                <div class="col-xs-11">
                    <asp:Literal runat="server" ID="litLevel"></asp:Literal>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-1">
                    <span class="--text-bold">Address</span>
                </div>
                <div class="col-xs-11">
                    <asp:Literal runat="server" ID="litAddress"></asp:Literal>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-1">
                    <span class="--text-bold">Phone</span>
                </div>
                <div class="col-xs-11">
                    <asp:Literal runat="server" ID="litPhone"></asp:Literal>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-1">
                    <span class="--text-bold">Fax</span>
                </div>
                <div class="col-xs-11">
                    <asp:Literal runat="server" ID="litFax"></asp:Literal>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-1">
                    <span class="--text-bold">Email</span>
                </div>
                <div class="col-xs-11">
                    <asp:HyperLink runat="server" ID="hplEmail"></asp:HyperLink>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-1 --no-padding-right">
                    <span class="--text-bold">Sale in charge</span>
                </div>
                <div class="col-xs-11">
                    <asp:Literal runat="server" ID="litSale"></asp:Literal>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-1">
                    <span class="--text-bold">Tax code</span>
                </div>
                <div class="col-xs-11">
                    <asp:Literal runat="server" ID="litTax"></asp:Literal>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-1">
                    <span class="--text-bold">Region</span>
                </div>
                <div class="col-xs-11">
                    <asp:Literal runat="server" ID="litLocation"></asp:Literal>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-1">
                    <span class="--text-bold">Accountant</span>
                </div>
                <div class="col-xs-11">
                    <asp:Literal runat="server" ID="litAccountant"></asp:Literal>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-1 --no-padding-right">
                    <span class="--text-bold">Payment period</span>
                </div>
                <div class="col-xs-11">
                    <asp:Literal runat="server" ID="litPayment"></asp:Literal>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-1">
                    <span class="--text-bold">Contract status</span>
                </div>
                <div class="col-xs-11">
                    <asp:Literal runat="server" ID="litContractstatus"></asp:Literal>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-xs-1">
                    <span class="--text-bold">Other info</span>
                </div>
                <div class="col-xs-11">
                    <asp:Literal runat="server" ID="litNote"></asp:Literal>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <asp:HyperLink runat="server" ID="hplEditAgency" CssClass="btn btn-primary">Edit this agency</asp:HyperLink>
                <asp:HyperLink runat="server" ID="hplBookingList" CssClass="btn btn-primary">Booking by this agency</asp:HyperLink>
                <div id="disableInform" style="display: none">
                    You don't have permission to use this function. If you want to use this function please contact administrator
                </div>
                <asp:HyperLink runat="server" ID="hplReceivable"
                    CssClass="btn btn-primary">Receivables (last 3 months)</asp:HyperLink>
                <asp:HyperLink runat="server" ID="hplTripManager"
                    CssClass="btn btn-primary">Trip Manager</asp:HyperLink>
                <asp:HyperLink runat="server" ID="hplGuideCollects"
                    CssClass="btn btn-primary">Guide Collects</asp:HyperLink>
                <asp:HyperLink runat="server" ID="hplDriverCollects"
                    CssClass="btn btn-primary">Driver Collects</asp:HyperLink>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-xs-12">
                <h5 class="--text-bold">CONTACTS</h5>
                <asp:PlaceHolder runat="server" ID="plhContacts">
                    <table class="table table-bordered table-common">
                        <tr class="active">
                            <th>Name
                            </th>
                            <th>Position
                            </th>
                            <th>Booker
                            </th>
                            <th>Phone
                            </th>
                            <th>Email
                            </th>
                            <th>Birthday
                            </th>
                            <th>Note
                            </th>
                            <th style="width: 6%">Add meeting
                            </th>
                            <th style="width: 3%">Edit
                            </th>
                            <th style="width: 4%">Delete
                            </th>
                        </tr>
                        <asp:Repeater runat="server" ID="rptContacts" OnItemDataBound="rptContacts_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Literal runat="server" ID="ltrName"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal runat="server" ID="litPosition"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal runat="server" ID="litBooker"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal runat="server" ID="litPhone"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:HyperLink runat="server" ID="hplEmail"></asp:HyperLink>
                                    </td>
                                    <td>
                                        <%# ((DateTime?)Eval("Birthday"))==null?"" : ((DateTime?)Eval("Birthday")).Value.ToString("dd/MM/yyyy")%>
                                    </td>
                                    <td>
                                        <%# DataBinder.Eval(Container.DataItem, "Note") %>
                                    </td>
                                    <td>
                                        <asp:HyperLink runat="server" ID="hplCreateMeeting" ToolTip="Add a meeting with this contact"><img src="https://cdn1.iconfinder.com/data/icons/IconsLandVistaPeopleIconsDemo/128/Group_Meeting_Light.png" width="17px" height="17px" /></asp:HyperLink>
                                    </td>
                                    <td>
                                        <asp:HyperLink runat="server" ID="hplName" ToolTip="Edit this contact"><img src="https://cdn1.iconfinder.com/data/icons/CrystalClear/128x128/actions/edit.png" width="17px" height="17px" /></asp:HyperLink>
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lbtDelete" OnClick="lbtDelete_Click" CommandArgument='<%#Eval("Id")%>'
                                            OnClientClick="return confirm('Are you sure?')" ToolTip="Delete this contact"><img src="https://cdn3.iconfinder.com/data/icons/softwaredemo/PNG/128x128/DeleteRed.png" width="17px" height="17px" /></asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                    <asp:HyperLink runat="server" ID="hplAddContact" CssClass="btn btn-primary">New contact</asp:HyperLink>
                </asp:PlaceHolder>
                <asp:Label runat="server" ID="lblContacts" Text="You don't have permission to use this function. If you want to use this function please contact administrator"
                    Visible="False" />
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-xs-12">
                <h5 class="--text-bold">RECENT ACTIVITIES</h5>
                <asp:PlaceHolder runat="server" ID="plhActivities">
                    <table class="table table-bordered table-common">
                        <asp:Repeater runat="server" ID="rptActivities" OnItemDataBound="rptActivities_ItemDataBound">
                            <HeaderTemplate>
                                <tr class="active">
                                    <th style="width: 7%">Date meeting
                                    </th>
                                    <th style="width: 10%">Sale
                                    </th>
                                    <th style="width: 13%">Meeting with
                                    </th>
                                    <th style="width: 13%">Position
                                    </th>
                                    <th>Note
                                    </th>
                                    <th style="width: 3%">Edit
                                    </th>
                                    <th style="width: 4%">Delete
                                    </th>
                                </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Literal runat="server" ID="ltrDateMeeting" />
                                    </td>
                                    <td>
                                        <asp:Literal runat="server" ID="ltrSale" />
                                    </td>
                                    <td>
                                        <asp:Literal runat="server" ID="ltrName" />
                                    </td>
                                    <td>
                                        <asp:Literal runat="server" ID="ltrPosition" />
                                    </td>
                                    <td>
                                        <asp:Literal runat="server" ID="ltrNote" />
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lbtEditActivity" ToolTip="Edit this meeting"><img src="https://cdn1.iconfinder.com/data/icons/CrystalClear/128x128/actions/edit.png" width="17px" height="17px" /></asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton runat="server" ID="lbtDeleteActivity" OnClick="lbtDeleteActivity_Click"
                                            CommandArgument='<%#Eval("Id")%>' OnClientClick="return confirm('Are you sure?')"
                                            ToolTip="Delete this meeting"><img src="https://cdn3.iconfinder.com/data/icons/softwaredemo/PNG/128x128/DeleteRed.png" width="17px" height="17px" /></asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </asp:PlaceHolder>
                <asp:Label runat="server" ID="lblActivities" Text="You don't have permission to use this function. If you want to use this function please contact administrator"
                    Visible="False" />
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-xs-12">
                <asp:PlaceHolder runat="server" ID="plhContracts">
                    <table class="table table-bordered table-common">
                        <tr class="active">
                            <th style="width: 15%">Created Date</th>
                            <th>Name
                            </th>
                            <th style="width: 15%">Expired on
                            </th>
                            <th style="width: 5%">Received</th>
                            <th style="width: 20%">Download
                            </th>
                            <th style="width: 3%">Edit
                            </th>
                        </tr>
                        <asp:Repeater runat="server" ID="rptContracts" OnItemDataBound="rptContracts_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Literal runat="server" ID="litCreatedDate" />
                                    </td>
                                    <td>
                                        <asp:Literal runat="server" ID="litName"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal runat="server" ID="litExpired"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal runat="server" ID="litReceived"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:HyperLink runat="server" ID="hplDownload" ToolTip="Download this contract file"><img src="https://cdn2.iconfinder.com/data/icons/freecns-cumulus/16/519672-178_Download-128.png" width="17px" height="17px" /></asp:HyperLink>
                                    </td>
                                    <td>
                                        <asp:HyperLink runat="server" ID="hplEdit" ToolTip="Edit this contract"><img src="https://cdn1.iconfinder.com/data/icons/CrystalClear/128x128/actions/edit.png" width="17px" height="17px" /></asp:HyperLink>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                    <asp:HyperLink runat="server" ID="hplAddContract" CssClass="btn btn-primary">New contract</asp:HyperLink>
                </asp:PlaceHolder>
                <asp:Label runat="server" ID="lblContracts" Text="You don't have permission to use this function. If you want to use this function please contact administrator"
                    Visible="False" />
            </div>
        </div>
    </div>
    <svc:Popup ID="popupManager" runat="server">
    </svc:Popup>
</asp:Content>
