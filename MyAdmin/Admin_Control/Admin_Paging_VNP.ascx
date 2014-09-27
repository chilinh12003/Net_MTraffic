<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Admin_Paging_VNP.ascx.cs" Inherits="MyAdmin.Admin_Control.Admin_Paging_VNP" %>
<div class="pagination pagination-right">
    <ul>
        <li>
            <asp:LinkButton runat="server" ID="lbtn_SlidePrev" OnClick="lbtn_SlidePrev_Click">«</asp:LinkButton></li>
        <li>
            <asp:LinkButton runat="server" ID="lbtn_1" OnClick="lbtn_1_Click">1</asp:LinkButton>
        </li>
        <li>
            <asp:LinkButton runat="server" ID="lbtn_2" OnClick="lbtn_2_Click">2</asp:LinkButton>
        </li>
        <li>
            <asp:LinkButton runat="server" ID="lbtn_3" OnClick="lbtn_3_Click">3</asp:LinkButton>
        </li>
        <li>
            <asp:LinkButton runat="server" ID="lbtn_SlideNext" OnClick="lbtn_SlideNext_Click">»</asp:LinkButton>
    </ul>
</div>
