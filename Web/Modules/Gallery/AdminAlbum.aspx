<%@ Page language="c#" Codebehind="AdminAlbum.aspx.cs" AutoEventWireup="True" Inherits="CMS.Modules.Gallery.Web.AdminAlbum" ValidateRequest="false" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls.FileUpload" TagPrefix="cc1" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>Edit Album</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<div id="moduleadminpane">
				<h1>Edit Album</h1>
				<div class="group">
					<h4>Album</h4>
					<table>
						<tr>
							<td style="WIDTH: 100px">Title</td>
							<td><asp:textbox id="txtTitle" runat="server" width="650px"></asp:textbox><asp:requiredfieldvalidator id="rfvTitle" runat="server" cssclass="validator" display="Dynamic" errormessage="The title is required" enableclientscript="False" controltovalidate="txtTitle"></asp:requiredfieldvalidator></td>
						</tr>
						<tr>
							<td style="WIDTH: 100px">Description</td>
							<td><FCKeditorV2:FCKeditor ID="fckEditorDescription" runat="server" Width="600" Height="400" BasePath="~/support/fckeditor/">
                                    </FCKeditorV2:FCKeditor></td>
						</tr>
						<tr>
							<td style="WIDTH: 100px">Active</td>
							<td><asp:checkbox id="chkActive" runat="server" checked="True"></asp:checkbox></td>
						</tr>
						<tr>
						    <td colspan="2">
                                <cc1:FileUploaderAJAX ID="FileUploaderAJAXPhotos" runat="server" MaxFiles="20" />
                                <asp:TextBox runat="server" id="txtAlbumId" style="display:none;"></asp:TextBox>
						    </td>						    
						</tr>
					</table>
				</div>
				
				<div class="group" runat="server" id="divFastImport" visible="false">
					<h4>Fast import</h4>
					<table>
					  <tr>
					    <td>
					      You have unmapped images in your album - do you like to import them?<br />
					      
					      <asp:Button runat="server" Text="Start Import"  ID="btnMassImport" />
					    </td>
					  </tr>
					</table>
				</div>				
				
				<p><asp:button id="btnSave" runat="server" text="Save" onclick="btnSave_Click"></asp:button><asp:button id="btnDelete" runat="server" text="Delete" visible="False" onclick="btnDelete_Click"></asp:button><input id="btnCancel" type="button" value="Cancel" runat="server"></p>
			</div>
		</form>
	</body>
</html>
