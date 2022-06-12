<%@ Page language="c#" Codebehind="AdminGallery.aspx.cs" AutoEventWireup="True" Inherits="CMS.Modules.Gallery.Web.AdminGallery" %>
<%@ Register TagPrefix="cc1" Namespace="CMS.ServerControls" Assembly="CMS.ServerControls" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>Gallery management</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="C#" name="CODE_LANGUAGE" />
		<meta content="JavaScript" name="vs_defaultClientScript" />
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<div id="moduleadminpane">
				<h1>Gallery management</h1>
				<div>
				    <asp:Label ID="labelAlbumName" runat="server" Text="Title">
				    </asp:Label>
				    <asp:TextBox ID="textBoxTitle" runat="server">
				    </asp:TextBox>
				    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
				</div>
					<table class="tbl">
						<asp:repeater id="rptAlbums" runat="server" OnItemCommand="rptAlbums_ItemCommand">
							<headertemplate>
								<tr>
									<th>Title</th>
									<th>Description</th>
									<th># Photos</th>
									<th>Active</th>
									<th>Created by</th>
									<th>Modified by</th>
									<th>Date updated</th>
									<th></th>
									<th></th>
									<th></th>
								</tr>
							</headertemplate>
							<itemtemplate>
								<tr>
									<td><%# DataBinder.Eval(Container.DataItem, "Title") %></td>
									<td><%# DataBinder.Eval(Container.DataItem, "Description") %></td>
									<td><%# DataBinder.Eval(Container.DataItem, "PhotoCount") %></td>
									<td><asp:checkbox id="chkActive" runat="server" checked="True"></asp:checkbox></td>
									<td><%# DataBinder.Eval(Container.DataItem, "CreatedBy.Username") %></td>
									<td><%# DataBinder.Eval(Container.DataItem, "ModifiedBy.Username") %></td>
									<td><asp:literal id="litDateModified" runat="server"></asp:literal></td>
									<td><asp:hyperlink id="hplEdit" runat="server">Edit</asp:hyperlink></td>
									<td><asp:hyperlink id="hplPhotos" runat="server">Photos</asp:hyperlink></td>
									<td><asp:LinkButton ID="lbtConvert" runat="server" CommandName="upgrade" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'>Use Simple Viewer</asp:LinkButton></td>
								</tr>
							</itemtemplate>
						</asp:repeater>
					</table>
				<div class="pager">
					<cc1:pager id="pgrAlbums" runat="server" controltopage="rptAlbums" cachedatasource="True" hidewhenonepage="true" pagesize="10" cacheduration="30" cachevarybyparams="SectionId" oncacheempty="pgrAlbums_CacheEmpty"></cc1:pager>
				</div>				
				<br/>
				<input id="btnNew" type="button" value="New Album" runat="server" />
				<asp:Button ID="btnConvertGallery" runat="server" OnClick="btnConvertGallery_Click" />
			</div>
		</form>
	</body>
</html>
