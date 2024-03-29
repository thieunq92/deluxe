﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExpenseHistory.aspx.cs" Inherits="Portal.Modules.OrientalSails.Web.Admin.ExpenseHistory" MasterPageFile="NewPopup.master" %>

<asp:Content ID="AdminContent" runat="server" ContentPlaceHolderID="AdminContent">
    <h4 class="page-header">Supplier</h4>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-2">
                <strong>Sửa bởi</strong>
            </div>
            <div class="col-xs-2">
                <strong>Thời gian</strong>
            </div>
            <div class="col-xs-4">
                <strong>Từ</strong>
            </div>
            <div class="col-xs-4">
                <strong>Chuyển sang</strong>
            </div>
        </div>
    </div>
    <asp:Repeater runat="server" ID="rptSupplierHistory">
        <ItemTemplate>
            <div class="form-group">
                <div class="row">
                    <div class="col-xs-2">
                        <%# Eval("CreatedBy.FullName") %>
                    </div>
                    <div class="col-xs-2">
                        <%# ((DateTime?)Eval("CreatedDate")).Value.ToString("dd/MM/yyyy HH:mm:ss")%>
                    </div>
                    <div class="col-xs-4">
                        <%# GetSupplier(Eval("OldValue").ToString())%>
                    </div>
                    <div class="col-xs-4">
                        <%# GetSupplier(Eval("NewValue").ToString())%>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <h4 class="page-header">Name</h4>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-2">
                <strong>Sửa bởi</strong>
            </div>
            <div class="col-xs-2">
                <strong>Thời gian</strong>
            </div>
            <div class="col-xs-4">
                <strong>Từ</strong>
            </div>
            <div class="col-xs-4">
                <strong>Chuyển sang</strong>
            </div>
        </div>
    </div>
    <asp:Repeater runat="server" ID="rptNameHistory">
        <ItemTemplate>
            <div class="form-group">
                <div class="row">
                    <div class="col-xs-2">
                        <%# Eval("CreatedBy.FullName") %>
                    </div>
                    <div class="col-xs-2">
                        <%# ((DateTime?)Eval("CreatedDate")).Value.ToString("dd/MM/yyyy HH:mm:ss")%>
                    </div>
                    <div class="col-xs-4">
                        <%# Eval("OldValue").ToString()%>
                    </div>
                    <div class="col-xs-4">
                        <%# Eval("NewValue").ToString()%>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <h4 class="page-header">Phone</h4>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-2">
                <strong>Sửa bởi</strong>
            </div>
            <div class="col-xs-2">
                <strong>Thời gian</strong>
            </div>
            <div class="col-xs-4">
                <strong>Từ</strong>
            </div>
            <div class="col-xs-4">
                <strong>Chuyển sang</strong>
            </div>
        </div>
    </div>
    <asp:Repeater runat="server" ID="rptPhoneHistory">
        <ItemTemplate>
            <div class="form-group">
                <div class="row">
                    <div class="col-xs-2">
                        <%# Eval("CreatedBy.FullName") %>
                    </div>
                    <div class="col-xs-2">
                        <%# ((DateTime?)Eval("CreatedDate")).Value.ToString("dd/MM/yyyy HH:mm:ss")%>
                    </div>
                    <div class="col-xs-4">
                        <span class="phone"><%# Eval("OldValue").ToString()%></span>
                    </div>
                    <div class="col-xs-4">
                        <span class="phone"><%# Eval("NewValue").ToString()%></span>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <h4 class="page-header">Cost</h4>
    <div class="form-group">
        <div class="row">
            <div class="col-xs-2">
                <strong>Sửa bởi</strong>
            </div>
            <div class="col-xs-2">
                <strong>Thời gian</strong>
            </div>
            <div class="col-xs-4">
                <strong>Từ</strong>
            </div>
            <div class="col-xs-4">
                <strong>Chuyển sang</strong>
            </div>
        </div>
    </div>
    <asp:Repeater runat="server" ID="rptCostHistory">
        <ItemTemplate>
            <div class="form-group">
                <div class="row">
                    <div class="col-xs-2">
                        <%# Eval("CreatedBy.FullName") %>
                    </div>
                    <div class="col-xs-2">
                        <%# ((DateTime?)Eval("CreatedDate")).Value.ToString("dd/MM/yyyy HH:mm:ss")%>
                    </div>
                    <div class="col-xs-4">
                        <%# GetCost(Eval("OldValue").ToString())%>
                    </div>
                    <div class="col-xs-4">
                        <%# GetCost(Eval("NewValue").ToString())%>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</asp:Content>
<asp:Content ID="Scripts" runat="server" ContentPlaceHolderID="Scripts">
</asp:Content>
