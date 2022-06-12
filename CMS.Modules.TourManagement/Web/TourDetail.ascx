<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TourDetail.ascx.cs" Inherits="CMS.Modules.TourManagement.Web.TourDetail" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit"  TagPrefix="ajax" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="svc" %>
<script type="text/javascript">
function PopupCenterEmail() 
{

var targetWin = window.open ("/SendEmail.aspx?url="+location.href, "Send_Email_to_Your_Friend", "toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=500, height=500, top=200, left=200");
targetWin.focus();
}
</script>

<div class="tour_view_detail">
<!--       		<h4>
		    <asp:Label runat="server" ID="labelDescription"></asp:Label>		    
		    </h4>-->		    
		    <asp:Image ID="imageTour" runat="server" ImageAlign="left" style="padding-right: 10px; padding-top: 10px; max-width:200px;"/>
		    <div class="send_email">
                <a href="javascript:PopupCenterEmail();" >
                <img src="/Images/iconemail.gif" alt="Send Email to Your Friend" align="absmiddle"/> Email
                </a>
		    </div>
		    <asp:Label ID="labelLocation" runat="server"></asp:Label><br />
		    <asp:Label ID="labelTripLength" runat="server"></asp:Label><br />
		    <asp:Label ID="labelTime" runat="server"></asp:Label><br />
		    <p runat="server" id="Description"></p>
		    <div style="clear:both"></div>		    
    <ajax:TabContainer ID="tabContainerTour" runat="server" CssClass="technorati">
        <ajax:TabPanel ID="panelDetailsIniterary" runat="server" HeaderText="Initerary" CssClass="tour_description">
            <ContentTemplate>
            <p runat="server" id="pDetailsIniterary"></p>
            </ContentTemplate>
        </ajax:TabPanel>    
        <ajax:TabPanel ID="panelActivities" runat="server" HeaderText="Activities & Highlight" CssClass="tour_description">
            <ContentTemplate>
            <p runat="server" id="pActivities"></p>
            <p runat="server" id="pHighLight"></p>
            </ContentTemplate>
        </ajax:TabPanel>
        <ajax:TabPanel ID="panelInclusion" runat="server" HeaderText="Inclusion" CssClass="tour_description" Visible="false">
            <ContentTemplate>
            <p runat="server" id="pInclusion"></p>
            </ContentTemplate>
        </ajax:TabPanel>  
        <ajax:TabPanel ID="panelExclusion" runat="server" HeaderText="Exclusion" CssClass="tour_description" Visible="false">
            <ContentTemplate>
            <p runat="server" id="pExclusion"></p>
            </ContentTemplate>
        </ajax:TabPanel>      
        <ajax:TabPanel ID="panelWhatToTake" runat="server" HeaderText="What To Take" CssClass="tour_description" Visible="false">
            <ContentTemplate>
            <p runat="server" id="pWhatToTake"></p>
            </ContentTemplate>
        </ajax:TabPanel>            
        <ajax:TabPanel ID="panelTourInstruction" runat="server" HeaderText="Instruction" CssClass="tour_description">
            <ContentTemplate>
            <p runat="server" id="pTourInstruction"></p>
            </ContentTemplate>
        </ajax:TabPanel>
        <ajax:TabPanel ID="panelAlbum" runat="server" HeaderText="Album" CssClass="tour_description">
            <ContentTemplate>
            </ContentTemplate>
        </ajax:TabPanel>  
        <ajax:TabPanel ID="panelMap" runat="server" HeaderText="Map" CssClass="tour_description">
            <ContentTemplate>
            </ContentTemplate>
        </ajax:TabPanel>
        <ajax:TabPanel ID="panelReview" runat="server" HeaderText="Review" CssClass="tour_description">
            <ContentTemplate>
                <asp:UpdatePanel ID="updateReview" runat="server">
                    <ContentTemplate>
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
                </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </ajax:TabPanel>        
        <ajax:TabPanel ID="panelPrice" runat="server" HeaderText="Price">
            <ContentTemplate>
                <asp:HyperLink ID="hplOrder" runat="server"></asp:HyperLink>
            </ContentTemplate>
        </ajax:TabPanel>
    </ajax:TabContainer>
</div>