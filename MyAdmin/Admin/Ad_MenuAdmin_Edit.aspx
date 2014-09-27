<%@ Page Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true"
    CodeBehind="Ad_MenuAdmin_Edit.aspx.cs" Inherits="MyAdmin.Admin.Ad_MenuAdmin_Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Tools" runat="server">
    <a href="Ad_MenuAdmin.aspx" runat="server" id="link_Cancel"><span class="Cancel"></span>
        Hủy </a>
    <asp:LinkButton runat="server" ID="lbtn_Save" OnClick="lbtn_Save_Click">
     <span class="Save"></span>
            Lưu
    </asp:LinkButton>
    <asp:LinkButton runat="server" ID="lbtn_Accept" OnClick="lbtn_Apply_Click">
     <span class="Accept"></span>
            Apply
    </asp:LinkButton>
    <a href="Ad_MenuAdmin_Edit.aspx" runat="server" id="link_Add"><span class="Add"></span>
        Thêm </a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_ToolBox" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_Search" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph_Content" runat="server">
    <div class="Edit_Left">
        <div class="Edit_Title">
            Thể loại mức 1:</div>
        <div class="Edit_Control">
            <asp:DropDownList runat="server" ID="ddl_MenuID_1" AutoPostBack="true" OnSelectedIndexChanged="ddl_MenuID_1_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        <div class="Edit_Title">
            Thể loại mức 2:</div>
        <div class="Edit_Control">
            <asp:DropDownList runat="server" ID="ddl_MenuID_2" AutoPostBack="true" OnSelectedIndexChanged="ddl_MenuID_2_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        <div class="Edit_Title">
            Thể loại mức 3:</div>
        <div class="Edit_Control">
            <select runat="server" id="sel_MenuID_3">
            </select>
        </div>
        <div class="Edit_Title" style="height: 40px;">
            Ảnh đại diện</div>
        <div class="Edit_Control" style="height: 50px;">
            <div class="Upload">
                <input type="file" runat="server" id="file_UploadImage" />
                <input type="text" runat="server" id="tbx_UploadImage" />
            </div>
            <div class="UploadImage">
                <img runat="server" id="img_Upload" src="../Images/Images/NoImage.jpg" style="float: left;
                    height: 50px; margin-left: 10px;" />
            </div>
        </div>
        <div class="Edit_Title">
            &nbsp;
        </div>
        <div class="Edit_Control">
            <asp:Button runat="server" ID="btn_UploadImage" Text="Upload" ToolTip="Upload Image"
                OnClick="btn_UploadImage_Click" />
        </div>
    </div>
    <div class="Edit_Right">
        <div class="Properties_Header">
            <div class="Properties_Header_In">
                Thông tin chi tiết khác
            </div>
        </div>
        <div class="Properties">
            <div class="Properties_Title">
                Tên menu:</div>
            <div class="Properties_Control">
                <input type="text" runat="server" id="tbx_CateName" />
            </div>
            <div class="Properties_Title">
                Mã trang:</div>
            <div class="Properties_Control">
                <input type="text" runat="server" id="tbx_PageCode" />
            </div>
            <div class="Properties_Title">
                Link tới trang:</div>
            <div class="Properties_Control">
                <input type="text" runat="server" id="tbx_Link" />
            </div>
            <div class="Properties_Title">
                Độ ưu tiên:</div>
            <div class="Properties_Control">
                <input type="text" runat="server" id="tbx_Priority" value="0" style="width: 50px;"
                    onkeypress="" />
            </div>
            <div class="Properties_Title">
                Kích hoạt:</div>
            <div class="Properties_Control">
                <input type="checkbox" runat="server" checked="checked" id="chk_Active" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cph_Javascript" runat="server">
</asp:Content>
