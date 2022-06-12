<%@ Register TagPrefix="cc1" Namespace="CMS.ServerControls" Assembly="CMS.ServerControls" %>
<%@ Page language="c#" Codebehind="AdminPhoto.aspx.cs" AutoEventWireup="false" Inherits="CMS.Modules.Gallery.Web.AdminPhoto" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>Edit Photo</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<div id="moduleadminpane">
				<h1>Edit Photo</h1>
				<div class="group">
					<h4>Photo properties</h4>
					<table>
						<tr>
							<td rowspan="3"><asp:image runat="Server" id="imThumb" /></td>
						</tr>
						<tr>
							<td style="WIDTH: 100px">File</td>
							<td>
								<asp:panel id="pnlFileName" runat="server" visible="True">
									<asp:textbox id="txtFile" runat="server" width="300px" enabled="False"></asp:textbox>
									<asp:requiredfieldvalidator id="rfvFile" runat="server" errormessage="File is required" display="Dynamic" cssclass="validator" controltovalidate="txtFile" enableclientscript="False"></asp:requiredfieldvalidator>
								</asp:panel>
								<input id="filUpload" style="WIDTH: 300px" type="file" runat="server">
								<asp:button id="btnUpload" runat="server" causesvalidation="False" text="Upload"></asp:button>
							</td>
						</tr>
						<tr>
							<td style="WIDTH: 100px">Title (optional)</td>
							<td><asp:textbox id="txtTitle" runat="server" width="300px"></asp:textbox></td>
						</tr>
					</table>
				</div>
				<p><asp:button id="btnSave" runat="server" text="Save"></asp:button><asp:button id="btnDelete" runat="server" visible="False" causesvalidation="False" text="Delete"></asp:button><asp:button id="btnCancel" runat="server" causesvalidation="False" text="Cancel"></asp:button></p>
			</div>
		</form>
	</body>
</html>
