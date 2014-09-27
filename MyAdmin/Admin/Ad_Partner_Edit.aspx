<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Ad_Partner_Edit.aspx.cs" Inherits="MyAdmin.Admin.Ad_Partner_Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_Header" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Tools" runat="server">
    <a href="Ad_Member.aspx" runat="server" id="link_Cancel">
        <span class="Cancel"></span>
        Hủy </a>
    <asp:LinkButton runat="server" ID="lbtn_Save" OnClick="lbtn_Save_Click" OnClientClick="return CheckAll();">
     <span class="Save"></span>
            Lưu
    </asp:LinkButton>
    <asp:LinkButton runat="server" ID="lbtn_Accept" OnClick="lbtn_Apply_Click" OnClientClick="return CheckAll();">
     <span class="Accept"></span>
            Apply
    </asp:LinkButton>
    <a href="Ad_Partner_Edit.aspx" runat="server" id="link_Add">
        <span class="Add"></span>
        Thêm </a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_ToolBox" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_Search" runat="server">
    <div class="Edit_Left">
        <div class="Edit_Title">
            Nhóm đối tác:</div>
        <div class="Edit_Control">
            <select runat="server" id="sel_PartnerType">
            </select>
        </div>
        
        <div class="Edit_Title">
            UserName:</div>
        <div class="Edit_Control">
            <input type="text" runat="server" id="tbx_PartnerUserName" />
        </div>
        <div class="Edit_Title">
            Password:</div>
        <div class="Edit_Control">
            <input type="text" runat="server" id="tbx_PartnerPassword" />
        </div>
        <div class="Edit_Title">
            Tên đối tác:</div>
        <div class="Edit_Control">
            <input type="text" runat="server" id="tbx_PartnerName" />
        </div>
        <div class="Edit_Title">
            Điện thoại:</div>
        <div class="Edit_Control">
            <input type="text" runat="server" id="tbx_Phone" />
        </div>     
        <div class="Edit_Title">
            Email:</div>
        <div class="Edit_Control">
            <input type="text" runat="server" id="tbx_Email" />
        </div>
        <div class="Edit_Title">
            Địa chỉ:</div>
        <div class="Edit_Control">
            <input type="text" runat="server" id="tbx_Address" />
        </div>
        <div class="Edit_Title">
            Website:</div>
        <div class="Edit_Control">
            <input type="text" runat="server" id="tbx_Website" />
        </div>       
        <div class="Edit_Title">
            Kích hoạt:</div>
        <div class="Edit_Control">
            <input type="checkbox" runat="server" checked="checked" id="chk_Active" />
        </div>
    </div>
    <div class="Edit_Right">
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph_Content" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cph_Javascript" runat="server">

    <script language="javascript" type="text/javascript">

        
    </script>

</asp:Content>
