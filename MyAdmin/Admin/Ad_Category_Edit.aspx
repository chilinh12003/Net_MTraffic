<%@ Page Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true"
    CodeBehind="Ad_Category_Edit.aspx.cs" Inherits="MyAdmin.Admin.Ad_Category_Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Tools" runat="server">
    <a href="Ad_Category.aspx" runat="server" id="link_Cancel"><span class="Cancel"></span>Hủy </a>
    <asp:LinkButton runat="server" ID="lbtn_Save" OnClick="lbtn_Save_Click">
     <span class="Save"></span>
            Lưu
    </asp:LinkButton>
    <asp:LinkButton runat="server" ID="lbtn_Accept" OnClick="lbtn_Apply_Click">
     <span class="Accept"></span>
            Apply
    </asp:LinkButton>
    <a href="Ad_Category_Edit.aspx" runat="server" id="link_Add"><span class="Add"></span>Thêm </a>
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
            <asp:DropDownList runat="server" ID="ddl_CateID_1" AutoPostBack="true" OnSelectedIndexChanged="ddl_CateID_1_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
         <div class="Edit_Title">
            Thể loại mức 2:</div>
        <div class="Edit_Control">
            <asp:DropDownList runat="server" ID="ddl_CateID_2" AutoPostBack="true" OnSelectedIndexChanged="ddl_CateID_2_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        <div class="Edit_Title">
            Thể loại mức 3:</div>
        <div class="Edit_Control">
            <select runat="server" id="sel_CateID_3">
            </select>
        </div>       
        <div class="Edit_Title" style="height: 40px;">
            Ảnh đại diện 1</div>
        <div class="Edit_Control" style="height: 50px;">
            <div class="Upload">
                <input type="file" runat="server" id="file_UploadImage_1" />
                <input type="text" runat="server" id="tbx_UploadImage_1" />
            </div>
            <div class="UploadImage">
                <img runat="server" id="img_Upload_1" src="../Images/Images/NoImage.jpg" style="float: left;
                    height: 50px; margin-left: 10px;" />
            </div>
        </div>
        <div class="Edit_Title" style="height: 40px;">
            Ảnh đại diện 2</div>
        <div class="Edit_Control" style="height: 50px;">
            <div class="Upload">
                <input type="file" runat="server" id="file_UploadImage_2" />
                <input type="text" runat="server" id="tbx_UploadImage_2" />
            </div>
            <div class="UploadImage">
                <img runat="server" id="img_Upload_2" src="../Images/Images/NoImage.jpg" style="float: left;
                    height: 50px; margin-left: 10px;" />
            </div>
        </div>
         <div class="Edit_Title" style="height: 40px;">
            Ảnh đại diện 3</div>
        <div class="Edit_Control" style="height: 50px;">
             <div class="Upload">
                <input type="file" runat="server" id="file_UploadImage_3" />
                <input type="text" runat="server" id="tbx_UploadImage_3" />
            </div>
            <div class="UploadImage">
                <img runat="server" id="img_Upload_3" src="../Images/Images/NoImage.jpg" style="float: left;
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
                Tên thể loại:</div>
            <div class="Properties_Control">
                <input type="text" runat="server" id="tbx_CateName" />
            </div>
            <div class="Properties_Title">
                Tiền tố code:</div>
            <div class="Properties_Control">
                <input type="text" runat="server" id="tbx_PrefixCode" />
            </div>
             <div class="Properties_Title" style="height:50px;">
                Mô tả:</div>
            <div class="Properties_Control" style="height:60px;">
                <textarea runat="server" id="tbx_Description" style="height:50px; "></textarea>
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
