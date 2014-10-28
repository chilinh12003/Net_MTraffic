<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Admin_Paging.ascx.cs"
    Inherits="MyAdmin.Admin_Control.Admin_Paging" %>
<link href="../CSS/Admin_Paging.css" rel="stylesheet" type="text/css" />
<div class="Paging">
    
    <asp:LinkButton runat="server" ID="lbtn_First" OnClick="lbtn_First_Click"><span>Đầu</span></asp:LinkButton>
    <asp:LinkButton runat="server" ID="lbtn_Prevous" OnClick="lbtn_Prevous_Click"><span>Trước</span></asp:LinkButton>
    <asp:LinkButton runat="server" ID="lbtn_SlidePrev" OnClick="lbtn_SlidePrev_Click"><span>...</span></asp:LinkButton>
    <asp:LinkButton runat="server" ID="lbtn_1" OnClick="lbtn_1_Click"><span>1</span></asp:LinkButton>
    <asp:LinkButton runat="server" ID="lbtn_2" OnClick="lbtn_2_Click"><span>2</span></asp:LinkButton>
    <asp:LinkButton runat="server" ID="lbtn_3" OnClick="lbtn_3_Click"><span>3</span></asp:LinkButton>
    <asp:LinkButton runat="server" ID="lbtn_SlideNext" OnClick="lbtn_SlideNext_Click"><span>...</span></asp:LinkButton>
    <asp:LinkButton runat="server" ID="lbtn_Next" OnClick="lbtn_Next_Click"><span>Sau</span></asp:LinkButton>
    <asp:LinkButton runat="server" ID="lbtn_Last" OnClick="lbtn_Last_Click"><span>Cuối</span></asp:LinkButton>
<asp:DropDownList runat="server" ID="ddl_PageSize" AutoPostBack="true" OnSelectedIndexChanged="ddl_PageSize_SelectedIndexChanged">
        <asp:ListItem Value="5" Text="5"></asp:ListItem>
        <asp:ListItem Value="10" Text="10" Selected="True"></asp:ListItem>
        <asp:ListItem Value="15" Text="15"></asp:ListItem>
        <asp:ListItem Value="30" Text="30"></asp:ListItem>
        <asp:ListItem Value="50" Text="50"></asp:ListItem>
        <asp:ListItem Value="100" Text="100"></asp:ListItem>
        <asp:ListItem Value="500" Text="500"></asp:ListItem>
    </asp:DropDownList>
</div>
