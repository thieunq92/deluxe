<%@ Page Language="C#" MasterPageFile="TourMaster.Master" AutoEventWireup="true" CodeBehind="ProviderCategories.aspx.cs" Inherits="CMS.Modules.TourManagement.Web.Admin.ProviderCategories" Title="Untitled Page" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls"
    TagPrefix="svc" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>    
<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
<table width="100%" border="0" cellpadding="0" cellspacing="10" bgcolor="#eeeeee" align="center">
<tr>
<td style="border:none;">
    <fieldset>
		<legend>		
		<asp:Label ID="labelBoatCategoryManagement" runat="server"></asp:Label>
		</legend>
		<div class="settinglist">
		    <asp:UpdatePanel id="updatePanelService" runat="server">
		    <ContentTemplate>
		    <div class="listbox">
		        <ul>
		            <asp:Repeater ID="repeaterServices" runat="server" OnItemDataBound="repeaterServices_ItemDataBound" OnItemCommand="repeaterServices_ItemCommand">
		            <ItemTemplate>
		            <li>
                          <asp:LinkButton ID='linkButtonServiceEdit' runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name")%>' CommandName="edit">
                          </asp:LinkButton>
                    </li>
		            </ItemTemplate>
		            </asp:Repeater>		            
		        </ul>
		        <asp:Button ID="btnAddNewRegion" runat="server" OnClick="btnAddNew_Click" />
		    </div>		    
		    <div class="wbox">
		        <div class="wbox_title">
		        <asp:Label ID="labelFormTitle" runat="server" Text="Title"></asp:Label>
		        </div>
		        <div class="wbox_content">
		        <div class="group">
		        <h4><asp:Label ID="labelBoatCategoryInformation" runat="server"></asp:Label></h4>
		        <table>
		            <tr>
		                <td style="width:100px;">
		                <asp:Label ID="labelRegionName" runat="server" Text="Type Of Service"></asp:Label>		                
		                </td>
		                <td>
		                <asp:TextBox ID="textBoxServiceName" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvTypeOfService" runat="server" ControlToValidate="textBoxServiceName" Text="Type is required!" ValidationGroup="service"></asp:RequiredFieldValidator>
		                </td>
		            </tr>
		            <tr>
		                <td style="width:100px;">
		                <asp:Label ID="labelOrderPriority" runat="server" Text="Type Of Service"></asp:Label>		                
		                </td>
		                <td>
		                <asp:TextBox ID="textBoxOrder" runat="server"></asp:TextBox>
		                </td>
		            </tr>		            		            
		       </table>
		        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="service" />
		        <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" />
		        <ajax:ConfirmButtonExtender ID="ConfirmButtonExtenderDelete" runat="server" TargetControlID="btnDelete" ConfirmText='Are you sure want to delete?'>
                                            </ajax:ConfirmButtonExtender>
		        </div>
		        </div>
		    </div>
		    </ContentTemplate>
		    </asp:UpdatePanel>
		</div>    
    </fieldset>
</td>
</tr>   
</table>
</asp:Content>