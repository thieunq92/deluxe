<%@ Page Language="C#" MasterPageFile="TourMaster.Master" AutoEventWireup="true" Codebehind="TourPackagePriceConfig.aspx.cs"
    Inherits="CMS.Modules.TourManagement.Web.Admin.TourPackagePriceConfig" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <fieldset>
        <legend>
            <asp:Label ID="labelTourEdit" runat="server"></asp:Label></legend>
        
        <div class="settinglist">
        	<div class="basicinfo">
        		<asp:Label ID="labelTourName" runat="server">
                    </asp:Label><br />
                    <asp:Label ID="labelProvider" runat="server"></asp:Label>
        	</div>
        	
        	<div class="advancedinfo">
        		<table>
        		    <tr>
        		        <td>Rate quoted on</td>
        		        <td><asp:DropDownList ID="ddlCurrencies" runat="server"></asp:DropDownList></td>
        		    </tr>
                    <tr>
                    <asp:Repeater ID="rptCustomers" runat="server">
                    	<HeaderTemplate>
                    		<td>Number of people</td>
                    	</HeaderTemplate>
                        <ItemTemplate>
                            <td><%# DataBinder.Eval(Container,"DataItem") %></td>
                        </ItemTemplate>
                    </asp:Repeater>
                    </tr>
                    <tr>
                    <asp:Repeater ID="rptPrices" runat="server" OnItemDataBound="rptPrices_ItemDataBound">
                    	<HeaderTemplate>
                    		<td>Total price</td>
                    	</HeaderTemplate>                    
                        <ItemTemplate>
                            <asp:HiddenField ID="hiddenCustomer" runat="server" Value='<%# DataBinder.Eval(Container,"DataItem") %>' />
                            <td><asp:TextBox ID="textBoxPrice" runat="server"></asp:TextBox></td>
                        </ItemTemplate>
                    </asp:Repeater>
                    </tr>                    
                    </table>
        	</div>
        	
        	<asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="button"/>
        </div>        
    </fieldset>
</asp:Content>
