<%@ Page Language="c#" AutoEventWireup="false" Inherits="CMS.Web.UI.PageEngine" EnableEventValidation="false" ValidateRequest="false" %>

<!--  -->
<script runat="server">
    protected override void OnLoad(EventArgs e)
    {
        Response.Redirect("Modules/Sails/Admin/BookingList.aspx?NodeId=1&SectionId=15");
    }
</script>