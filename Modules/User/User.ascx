<%@ Control Language="c#" AutoEventWireup="True" Codebehind="User.ascx.cs" Inherits="CMS.Modules.User.User" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<div class="login">
<asp:panel id="pnlLogin" runat="server" CssClass="login_panel">
<asp:label id="lblUsername" runat="server"></asp:label><br/>
<asp:textbox id="txtUsername" runat="server" width="120px"></asp:textbox><br/>
            <ajax:TextBoxWatermarkExtender ID="textBoxHotelNameEx" runat="server" WatermarkText='Username' WatermarkCssClass="water_marked"
            TargetControlID="txtUsername">
            </ajax:TextBoxWatermarkExtender>
<asp:label id="lblPassword" runat="server"></asp:label><br/>
<asp:textbox id="txtPassword" runat="server" width="120px" textmode="Password"></asp:textbox>
            <%--<ajax:TextBoxWatermarkExtender ID="txtPasswordEx" runat="server" WatermarkText='Password' WatermarkCssClass="water_marked"
            TargetControlID="txtPassword">
            </ajax:TextBoxWatermarkExtender>--%><br/>
<asp:checkbox id="chkPersistLogin" runat="server" CssClass="checkbox"></asp:checkbox><br/>
<asp:label id="lblLoginError" runat="server" enableviewstate="False" visible="False" cssclass="error"></asp:label><br/>
<asp:button id="btnLogin" runat="server" onclick="btnLogin_Click"></asp:button><br/><br/>
<asp:hyperlink id="hplRegister" runat="server"></asp:hyperlink>&nbsp;&nbsp; 
<asp:hyperlink id="hplResetPassword" runat="server" CssClass="resetpassword"></asp:hyperlink>
</asp:panel>
<asp:panel id="pnlUserInfo" runat="server" visible="False" CssClass="logout_panel">
	<asp:label id="lblLoggedInText" runat="server"></asp:label>
	<asp:label id="lblLoggedInUser" runat="server"></asp:label>
	<br/>
	<asp:button id="btnLogout" runat="server" onclick="btnLogout_Click"></asp:button>
	<br/>
	<br/>
	<asp:hyperlink id="hplEdit" runat="server"></asp:hyperlink>
</asp:panel>
</div>