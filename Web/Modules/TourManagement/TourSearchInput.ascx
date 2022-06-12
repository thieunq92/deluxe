<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TourSearchInput.ascx.cs" Inherits="CMS.Modules.TourManagement.Web.TourSearchInput" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<div class="hotel_search_input">
<ul style="list-style:none; padding-right: 10px; margin-left:5px;">
    <li>
    <div style="float:right">
    <asp:DropDownList ID="ddlTourRegions" runat="server" Width="152">
    </asp:DropDownList>
    </div>
    <br /><br />
    </li>
    <li>
    <div style="float:right">
    <asp:DropDownList ID="ddlTourTypes" runat="server" Width="152">
    </asp:DropDownList>
    </div>    
    <br /><br />
    </li>
    <li>
    <div style="float:right">
    <asp:TextBox ID="textBoxNumberOfDays" runat="server" Width="150"></asp:TextBox>
    <ajax:TextBoxWatermarkExtender ID="textBoxNumberOfDaysEx" runat="server" WatermarkText='Number of days' WatermarkCssClass="water_marked"
            TargetControlID="textBoxNumberOfDays">
            </ajax:TextBoxWatermarkExtender>
    </div>    
    <br /><br />
    </li>
    <li>
    <div style="float:right">
    <asp:TextBox ID="textBoxName" runat="server" Width="150"></asp:TextBox>
    <ajax:TextBoxWatermarkExtender ID="textBoxNameEx" runat="server" WatermarkText='Name' WatermarkCssClass="water_marked"
            TargetControlID="textBoxName">
            </ajax:TextBoxWatermarkExtender>    
    </div>    
    <br /><br />
    </li>    
    <li style="text-align: right;height:10px;">
    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"/>
    </li>    
</ul>
</div>

