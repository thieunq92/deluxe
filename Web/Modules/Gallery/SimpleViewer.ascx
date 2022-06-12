<%@ Control Language="C#" AutoEventWireup="true" Codebehind="SimpleViewer.ascx.cs"
    Inherits="CMS.Modules.Gallery.Web.SimpleViewer" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>

<script type="text/javascript" src="/js/swfobject.js"></script>

<div id="flashcontent">
    <p>
        AutoViewer requires JavaScript and the Flash Player. <a href="http://www.macromedia.com/go/getflashplayer/">
            Get Flash here.</a>
    </p>
</div>
<asp:UpdatePanel ID="updateReview" runat="server">
    <contenttemplate>
                <asp:Repeater ID="rptReviews" runat="server" OnItemDataBound="rptReviews_ItemDataBound">
                    <ItemTemplate>
                        <asp:Label ID="labelAuthor" runat="server" CssClass="author"></asp:Label><br />
                        <asp:Label ID="labelContent" runat="server" CssClass="comment_content"></asp:Label><br />
                    </ItemTemplate>
                </asp:Repeater>
                <div class = "comment_form">                    
                    <asp:TextBox ID="textBoxAuthor" runat="server"></asp:TextBox>
                    <ajax:TextBoxWatermarkExtender ID="textBoxAuthorEx" runat="server" WatermarkCssClass="water_marked" TargetControlID="textBoxAuthor" WatermarkText="Your name"></ajax:TextBoxWatermarkExtender><br />
                    <asp:TextBox ID="textBoxEmail" runat="server"></asp:TextBox>
                    <ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtenderEmail" runat="server" WatermarkCssClass="water_marked" TargetControlID="textBoxEmail" WatermarkText="Your email"></ajax:TextBoxWatermarkExtender><br />
                    <asp:TextBox ID="textBoxComment" runat="server" TextMode="MultiLine" Height="100"></asp:TextBox><br />
                    <asp:CheckBox ID="chkAgree" runat="server" Text="I agree" CssClass="checkbox"/><br />
                    <svc:ImageVerifier ID="imageVerifier" runat="server"></svc:ImageVerifier><br />
                    <asp:Label ID="labelFailedVerified" runat="server" Text="You must input the verify code in the picture<br/>" Visible="false" CssClass="verify_error" style="color:Red"></asp:Label>
                    <asp:TextBox ID="textBoxVerifierCode" runat="server"></asp:TextBox><ajax:TextBoxWatermarkExtender ID="waterMarkVerifyCode" runat="server" WatermarkCssClass="water_marked" TargetControlID="textBoxVerifierCode" WatermarkText="Type in the verify code above"></ajax:TextBoxWatermarkExtender><br />
                    <asp:Button ID="buttonSubmit" runat="server" Text="Submit" OnClick="buttonSubmit_Click" CssClass="button"/>
                </div>
                </contenttemplate>
</asp:UpdatePanel>

<script type="text/javascript">
		var fo = new SWFObject("/support/viewer.swf", "simpleviewer", "550", "700", "8", "#ffffff");						
		//Optional Configuration
		fo.addVariable("xmlDataPath", "<%= GetXmlPath() %>");
		fo.addVariable("langOpenImage", "Open Image in New Window");
		fo.addVariable("langAbout", "About");	
		fo.addVariable("preloaderColor", "0x999999");		
		fo.write("flashcontent");
</script>

