<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Ad_Article_Edit.aspx.cs" Inherits="MyAdmin.Admin.Ad_Article_Edit" %>
<%@ Register Assembly="CuteEditor" Namespace="CuteEditor" TagPrefix="CE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Tools" runat="server">
    <a href="Ad_Article.aspx" runat="server" id="link_Cancel"><span class="Cancel"></span>
        Hủy </a>
    <asp:LinkButton runat="server" ID="lbtn_Save" OnClick="lbtn_Save_Click" OnClientClick="return CheckAll();">
     <span class="Save"></span>
            Lưu
    </asp:LinkButton>
    <asp:LinkButton runat="server" ID="lbtn_Accept" OnClick="lbtn_Apply_Click" OnClientClick="return CheckAll();">
     <span class="Accept"></span>
            Apply
    </asp:LinkButton>
    <a href="Ad_Article_Edit.aspx" runat="server" id="link_Add"><span class="Add"></span>
        Thêm </a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_ToolBox" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_Search" runat="server">
    <div class="Edit_Left">
        <div class="Edit_Title">
            Thể loại mức 1</div>
        <div class="Edit_Control">
            <asp:DropDownList runat="server" ID="ddl_CateID_1" OnSelectedIndexChanged="ddl_CateID_1_SelectedIndexChanged"
                AutoPostBack="true">
            </asp:DropDownList>
        </div>
        <div class="Edit_Title">
            Thể loại mức 2</div>
        <div class="Edit_Control">
            <asp:DropDownList runat="server" ID="ddl_CateID_2" OnSelectedIndexChanged="ddl_CateID_2_SelectedIndexChanged"
                AutoPostBack="true">
            </asp:DropDownList>
        </div>
        <div class="Edit_Title">
            Thể loại mức 3</div>
        <div class="Edit_Control">
            <select runat="server" id="sel_CateID_3">
                <option value="0">- - Thể loại mức 3 - - </option>
            </select>
        </div>
        <div class="Edit_Title" style="height: 40px;">
            Ảnh đại diện 1<br />
            (Width:50px)</div>
        <div class="Edit_Control" style="height: 50px;">
            <div class="Upload">
                <input type="file" runat="server" id="file_UploadImage_1" />
                <input type="text" runat="server" id="tbx_UploadImage_1" />
            </div>
            <div class="UploadImage">
                <img runat="server" id="img_Upload_1" src="" style="float: left; height: 50px; margin-left: 10px;" />
            </div>
        </div>
        <div class="Edit_Title" style="height: 40px;">
            Ảnh đại diện 2<br />
            (Width:100px)</div>
        <div class="Edit_Control" style="height: 50px;">
            <div class="Upload">
                <input type="file" runat="server" id="file_UploadImage_2" />
                <input type="text" runat="server" id="tbx_UploadImage_2" />
            </div>
            <div class="UploadImage">
                <img runat="server" id="img_Upload_2" src="" style="float: left; height: 50px; margin-left: 10px;" />
            </div>
        </div>       
        <div class="Edit_Title">
            &nbsp;
        </div>
        <div class="Edit_Control">
            <asp:Button runat="server" ID="btn_UploadImage" Text="Upload" ToolTip="Upload Image"
                OnClick="btn_UploadImage_Click" />
        </div>
        <div class="NewLine">
             <div class="Edit_Title">
                Tiêu đề:</div>
            <div class="Edit_Control_Editor">
                 <input type="text" runat="server" id="tbx_ArticleName" style="width: 100%;" />
            </div>
        </div>
        <div class="NewLine">
            <div class="Edit_Title" style="height: 50px;">
                Mô tả:</div>
            <div class="Edit_Control_Editor">
                <textarea runat="server" id="tbx_Description" style="width: 100%; height: 50px;"></textarea>
            </div>
        </div>
        <div class="NewLine">
            <div class="Edit_Title" style="height: 500px;">
                Nội dung:</div>
            <div class="Edit_Control_Editor">
                <CE:Editor ID="tbx_Content" runat="server" Width="100%" Height="502px" ThemeType="Office2007"
                    RemoveServerNamesFromUrl="true" ConfigurationPath="~/CuteSoft_Client/CuteEditor/Configuration/AutoConfigure/My.config">
                </CE:Editor>
            </div>
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
                Độ ưu tiên</div>
            <div class="Properties_Control">
                <input type="text" runat="server" id="tbx_Priority" value="0" onkeypress="return isNumberKey_int(event);" />
            </div>
            <div class="Properties_Title">
                Kích hoạt</div>
            <div class="Properties_Control">
                <input type="checkbox" runat="server" id="chk_Active" checked="checked" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph_Content" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cph_Javascript" runat="server">
</asp:Content>

