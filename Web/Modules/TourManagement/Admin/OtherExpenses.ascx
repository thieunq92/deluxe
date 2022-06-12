<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OtherExpenses.ascx.cs" Inherits="CMS.Modules.TourManagement.Web.Admin.OtherExpenses" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="CMS.ServerControls" Namespace="CMS.ServerControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:UpdatePanel ID="updatePanel1" runat="server">
    <ContentTemplate>
        <ul>
            <li>
                <table>
                    <asp:Repeater ID="rptAddedExpenses" runat="server" OnItemDataBound="rptAddedExpenses_ItemDataBound" OnItemCommand="rptAddedExpenses_ItemCommand">
                        <HeaderTemplate>
                            <tr>
                                <th style="width:120px;">Name</th>
                                <th>Time</th>
                                <th><%#base.GetText("labelAction") %></th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="width:120px;">
                                    <%# DataBinder.Eval(Container.DataItem,"Name") %>
                                </td>
                                <td>Day <%#DataBinder.Eval(Container.DataItem,"DayFrom")%> - Day <%#DataBinder.Eval(Container.DataItem,"DayTo")%></td>
                                <td>
                                    <asp:ImageButton runat="server" ID="imageButtonDelete" ToolTip='Delete' ImageUrl="/Admin/AdminModuleImages/delete_file.gif" AlternateText='<%# base.GetText("ibtDelete") %>' ImageAlign="AbsMiddle" CssClass="image_button16" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Id") %>'/>
                                    <ajax:ConfirmButtonExtender ID="ConfirmButtonExtenderDelete" runat="server" TargetControlID="imageButtonDelete" ConfirmText='<%# base.GetText("messageConfirmDelete") %>'>
                                    </ajax:ConfirmButtonExtender>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </li>
            <li>
                Expense name: <asp:TextBox ID="txtBoxName" runat="server"></asp:TextBox>
                From day <asp:DropDownList ID="ddlDayFrom" runat="server"></asp:DropDownList>
                to day <asp:DropDownList ID="ddlDayTo" runat="server"></asp:DropDownList>
                <table>
                    <tr>
                    <asp:Repeater ID="rptExpensePrice" runat="server">
                        <ItemTemplate>                            
                            <td><asp:HiddenField ID="hiddenNumberOfCustomer" runat="server" Value='<%# DataBinder.Eval(Container,"DataItem") %>' /><asp:TextBox ID="txtPrice" runat="server"></asp:TextBox></td>
                        </ItemTemplate>
                    </asp:Repeater>
                    </tr>
                </table>
                <asp:Button ID="buttonAdd" runat="server" OnClick="buttonAdd_Click" Text="Add" />
            </li>
        </ul>
    </ContentTemplate>
</asp:UpdatePanel>