<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Admin_Gridview.ascx.cs"
    Inherits="MyAdmin.Admin_Control.Admin_Gridview" %>
<%@ Register Src="../Admin_Control/Admin_Paging.ascx" TagName="Admin_Paging" TagPrefix="uc1" %>
<table class="Data" border="0" cellpadding="0" cellspacing="0">
    <tbody>
        <tr class="Table_Header">
            <th class="Table_TL"></th>
            <%=CreateHeader()%>
            <th class="Table_TR"></th>
        </tr>
        <%=CreateRow()%>
    </tbody>
</table>
<div class="Table_Footer">
    <div class="Table_BL">
        <uc1:Admin_Paging ID="Admin_Paging1" runat="server" />
    </div>
    <div class="Table_BR">
    </div>
</div>
