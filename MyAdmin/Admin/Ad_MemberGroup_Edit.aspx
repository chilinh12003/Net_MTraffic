<%@ Page Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Ad_MemberGroup_Edit.aspx.cs" Inherits="MyAdmin.Admin.Ad_MemberGroup_Edit" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Tools" runat="server">
    <a href="Ad_MemberGroup.aspx" runat="server" id="link_Cancel">
        <span class="Cancel"></span>
        Hủy </a>
    <asp:LinkButton runat="server" ID="lbtn_Save" OnClick="lbtn_Save_Click">
     <span class="Save"></span>
            Lưu
    </asp:LinkButton>
    <asp:LinkButton runat="server" ID="lbtn_Accept" OnClick="lbtn_Apply_Click">
     <span class="Accept"></span>
            Apply
    </asp:LinkButton>
    <a href="Ad_MemberGroup_Edit.aspx"  runat="server" id="link_Add">
        <span class="Add"></span>
        Thêm </a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_ToolBox" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_Search" runat="server">
    <div class="Edit_Left">
          <div class="Edit_Title">
            Mã nhóm:</div>
        <div class="Edit_Control">
            <input type="text" runat="server" id="tbx_MemberGroupID" style="width:50px;" />
        </div>
        <div class="Edit_Title">
            Tên nhóm:</div>
        <div class="Edit_Control">
            <input type="text" runat="server" id="tbx_MemberGroupName" />
        </div>
        <div class="Edit_Title">
            Đối tác:</div>
        <div class="Edit_Control">
            <select runat="server" id="sel_Parnter"></select>
        </div>
    </div>
    <div class="Edit_Right">
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph_Content" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cph_Javascript" runat="server">
</asp:Content>
