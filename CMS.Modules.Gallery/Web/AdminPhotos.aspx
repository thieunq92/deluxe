<%@ Page language="c#" Codebehind="AdminPhotos.aspx.cs" AutoEventWireup="false" Inherits="CMS.Modules.Gallery.Web.AdminPhotos" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>Edit Photos</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<div id="moduleadminpane">
				<h1>Photos management</h1>
				<p>
					<table class="tbl">
						<asp:repeater id="rptPhotos" runat="server" OnItemCommand="rptPhotos_ItemCommand">
							<headertemplate>
								<tr>
									<th>Thumbnail</th>
									<th>Title</th>
									<th>Filename</th>
									<th>Size (bytes)</th>
									<th>Published by</th>
									<th>Number of views</th>
									<th>Date modified</th>
									<th colspan="3"></th>
								</tr>
							</headertemplate>
							<itemtemplate>
								<tr>								
									<td><asp:literal runat="Server" id="litThumb" />
									<asp:HiddenField ID="hiddenOrder" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Order") %>' />
									<asp:HiddenField ID="hiddenId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' /></td>
									<td><%# DataBinder.Eval(Container.DataItem, "Title") %></td>
									<td><%# DataBinder.Eval(Container.DataItem, "FileName") %></td>
									<td><%# DataBinder.Eval(Container.DataItem, "Size") %></td>
									<td><%# DataBinder.Eval(Container.DataItem, "CreatedBy.FullName") %></td>
									<td><%# DataBinder.Eval(Container.DataItem, "NrOfViews") %></td>
									<td><asp:literal id="litDateModified" runat="server"></asp:literal></td>
									<td><asp:hyperlink id="hplEdit" runat="server">Edit</asp:hyperlink></td>
									<td><asp:LinkButton ID="lbtUp" runat="server" CommandName="Up" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'>Up</asp:LinkButton></td>
									<td><asp:LinkButton ID="lbtDown" runat="server" CommandName="Down" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'>Down</asp:LinkButton></td>
								</tr>
							</itemtemplate>
						</asp:repeater>
					</table>
				</p>
				<br/>
				<input id="btnNew" type="button" value="Add Photo" runat="server" name="btnNew">
				<br/><br/>
				<asp:button id="btnBack" runat="server" text="Back" onclick="btnBack_Click"></asp:button>
			</div>
		</form>
	</body>
</html>
