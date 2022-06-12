<%@ Page Title="Agency user" Language="C#" MasterPageFile="Main.Master" AutoEventWireup="true" CodeBehind="AgencyUserList.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.AgencyUserList" %>

<%@ Register TagPrefix="svc" Namespace="CMS.ServerControls" Assembly="CMS.ServerControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="AdminContent" runat="server">
    <div class="data_table">
        <h4>User</h4>
        <div class="page-header">
            <h3>Danh mục</h3>
        </div>
        <div class="search-panel">
            <%--<div class="form-group">
                <div class="row">
                    <div class="col-xs-1">
                        Tên
                    </div>
                    <div class="col-xs-3">
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" placeholder="UserName"></asp:TextBox>
                    </div>
                    <div class="col-xs-1">
                        Danh mục cha
                    </div>
                    <div class="col-xs-3">
                        <asp:DropDownList ID="ddlAgency" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>--%>
            <div class="form-group">
                <div class="row">
                    <div class="col-xs-12">
                        <asp:Button runat="server" ID="btnSearch" CssClass="btn btn-primary"
                            OnClick="btnSearch_Click" Text="Tìm kiếm"></asp:Button>
                        <asp:Button runat="server" ID="btnAddNew" CssClass="btn btn-primary" OnClientClick="return addUser();"
                            Text="Thêm mới user"></asp:Button>
                    </div>
                </div>
            </div>
        </div>
        <div class="pager">
            <svc:Pager ID="pagerUser" runat="server" ControlToPage="rptUsers" PagerLinkMode="HyperLinkQueryString" />
        </div>
        <div class="col-xs-4">
            <table class="table table-bordered table-common">
                <tr class="active">
                    <th>UserName</th>
                    <th></th>
                   <%-- <th></th>--%>
                </tr>
                <asp:Repeater ID="rptUsers" runat="server" OnItemCommand="rptUsers_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            <td class="item">
                                <%#Eval("UserName") %></td>
                            <td>
                                <a href="javascript:;" onclick='editUser(<%#Eval("Id") %>)'><i class="fa fa-edit fa-lg" aria-hidden="true" data-toggle="tooltip" data-placement="top" title="Edit"></i>
                                </a>
                            </td>
                           <%-- <td>
                                <asp:LinkButton ID="lbtDelete" runat="server" CommandArgument='<%#Eval("Id") %>' CommandName="delete" >
                                    <i class="fa fa-trash fa-lg text-danger" aria-hidden="true" title="" data-toggle="tooltip" data-placement="top" data-original-title="Delete"></i>
                                </asp:LinkButton></td>--%>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="modal fade" id="osModal" tabindex="-1" role="dialog" aria-labelledby="gridSystemModalLabel" data-backdrop="static" data-keyboard="true">
        <div class="modal-dialog" role="document" style="width: 60vw; height: 50vh">
            <div class="modal-content">
                <div class="modal-header">
                    <%--                    <span>Add booking</span>--%>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                </div>
                <div class="modal-body">
                    <iframe frameborder="0" width="90%" style="height: 60vh" scrolling="no"></iframe>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script>
        function addUser() {
            var src = "/Modules/Sails/Admin/AgencyUserEdit.aspx?NodeId=1&SectionId=15&UserId=-1";
            $("#osModal iframe").attr('src', src);
            $("#osModal").modal();
            return false;
        }
        function editUser(id) {
            var src = "/Modules/Sails/Admin/AgencyUserEdit.aspx?NodeId=1&SectionId=15&UserId=" + id;
            $("#osModal iframe").attr('src', src);
            $("#osModal").modal();
            return false;
        }
        function closePoup(refesh) {
            $("#osModal").modal('hide');
            if (refesh === 1) {
                window.location.href = window.location.href;
            }
        }
    </script>
</asp:Content>
