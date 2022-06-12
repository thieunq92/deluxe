<%@ Import namespace="CMS.Core.Domain"%>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="Albums.ascx.cs" Inherits="CMS.Modules.Gallery.Web.Albums" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ register assembly="CMS.ServerControls" namespace="CMS.ServerControls"	tagprefix="cc1" %>


<h1><%= Module.Section.Title %></h1>

<asp:panel id="pnlAlbumList" runat="server" CssClass="gallery">
		<asp:DataList ID="AlbumDataList" runat="Server" onItemDataBound="AlbumDataList_ItemDataBound" cssclass="view" repeatColumns="2" repeatdirection="Horizontal" CellSpacing="10">
		  <ItemStyle cssClass="item"/>
		  <ItemTemplate>		    
		    <div class="div_item" onmouseover="this.className='div_hover';" onmouseout="this.className='div_item';">
		    <asp:hyperlink id="hplImage" runat="server"><asp:literal id="litImage" runat="server"></asp:literal></asp:hyperlink>
		    <h4><asp:hyperlink id="hplAlbum" runat="server"><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem,"Title").ToString()) %></asp:hyperlink></h4>		      
		    </div>
		  </ItemTemplate>
		</asp:DataList>
                                
		<div class="pager">
			<cc1:pager id="pgrAlbums" runat="server" controltopage="AlbumDataList"  
			    hidewhenonepage="true" onpagechanged="pgrAlbums_PageChanged" 
			    pagerlinkmode="HyperLinkPathInfo" />
		</div>		
</asp:panel>