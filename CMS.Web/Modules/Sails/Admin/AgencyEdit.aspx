<%@ Page Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true"
    CodeBehind="AgencyEdit.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.AgencyEdit" ValidateRequest="false" %>

<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <title>Agency add/edit</title>
</asp:Content>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="row">
        <div class="col-xs-4 --no-padding-left">
            <div class="form-group">
                <div class="row">
                    <div class="col-xs-3">
                        Name
                    </div>
                    <div class="col-xs-9">
                        <asp:TextBox ID="textBoxName" runat="server" placeholder="Name" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="row">
                    <div class="col-xs-3">
                        Phone
                    </div>
                    <div class="col-xs-9">
                        <asp:TextBox ID="txtPhone" runat="server" placeholder="Phone" CssClass="form-control" data-control="phoneinputmask"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="row">
                    <div class="col-xs-3">
                        Fax
                    </div>
                    <div class="col-xs-9">
                        <asp:TextBox ID="txtFax" runat="server" placeholder="Fax" CssClass="form-control" data-control="phoneinputmask"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="row">
                    <div class="col-xs-3">
                        Email
                    </div>
                    <div class="col-xs-9">
                        <asp:TextBox ID="txtEmail" runat="server" placeholder="Email" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="row">
                    <div class="col-xs-3">
                        Tax code
                    </div>
                    <div class="col-xs-9">
                        <asp:TextBox ID="txtTaxCode" runat="server" placeholder="Tax code" CssClass="form-control" data-control="phoneinputmask"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="row">
                    <div class="col-xs-3 --no-padding-right">
                        Sales in charge
                    </div>
                    <div class="col-xs-9">
                        <asp:DropDownList ID="ddlSales" runat="server" CssClass="form-control --width-auto">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="row">
                    <div class="col-xs-3">
                        Region
                    </div>
                    <div class="col-xs-9">
                        <asp:DropDownList ID="ddlOrganizations" runat="server" CssClass="form-control --width-auto">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="row">
                    <div class="col-xs-3">
                        Other info
                    </div>
                    <div class="col-xs-9">
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="form-control" placeholder="Other info"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-4 --no-padding-right">
            <div class="form-group">
                <div class="row">
                    <div class="col-xs-3">
                        Address
                    </div>
                    <div class="col-xs-9">
                        <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" placeholder="Address" CssClass="form-control">
                        </asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="row">
                    <div class="col-xs-3">
                        Role
                    </div>
                    <div class="col-xs-9">
                        <asp:DropDownList ID="ddlRoles" runat="server" CssClass="form-control --width-auto">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="row">
                    <div class="col-xs-3">
                        Level
                    </div>
                    <div class="col-xs-9">
                        <asp:DropDownList ID="ddlLevel" runat="server" CssClass="form-control --width-auto">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="row">
                    <div class="col-xs-3 --no-padding-right">
                        Contract status
                    </div>
                    <div class="col-xs-9">
                        <asp:DropDownList ID="ddlContractStatus" runat="server" CssClass="form-control --width-auto">
                            <asp:ListItem Value="0">No</asp:ListItem>
                            <asp:ListItem Value="1">Pending</asp:ListItem>
                            <asp:ListItem Value="2">Yes</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="row">
                    <div class="col-xs-3">
                        Accountant
                    </div>
                    <div class="col-xs-9">
                        <asp:TextBox ID="txtAccountant" runat="server" CssClass="form-control" placeholder="Accountant"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="row">
                    <div class="col-xs-3 --no-padding-right">
                        Payment period
                    </div>
                    <div class="col-xs-9">
                        <asp:DropDownList ID="ddlPaymentPeriod" runat="server" CssClass="form-control --width-auto">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <asp:Literal ID="litCreated" runat="server"></asp:Literal>
            <asp:Literal ID="litModified" runat="server"></asp:Literal>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <asp:Button ID="buttonSave" runat="server" OnClick="buttonSave_Click" CssClass="btn btn-primary" />
        </div>
    </div>
</asp:Content>
