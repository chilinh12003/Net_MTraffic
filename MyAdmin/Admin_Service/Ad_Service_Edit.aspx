<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Ad_Service_Edit.aspx.cs" Inherits="MyAdmin.Admin_Service.Ad_Service_Edit" %>

<%@ Register Assembly="CuteEditor" Namespace="CuteEditor" TagPrefix="CE" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Header" runat="server">
    <link href="../CSS/autocomplete.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Tools" runat="server">
    <a href="Ad_Service.aspx" runat="server" id="link_Cancel"><span class="Cancel"></span>Hủy </a>
    <asp:LinkButton runat="server" ID="lbtn_Save" OnClick="lbtn_Save_Click" OnClientClick="return CheckAll();">
     <span class="Save"></span>
            Lưu
    </asp:LinkButton>
    <asp:LinkButton runat="server" ID="lbtn_Accept" OnClick="lbtn_Apply_Click" OnClientClick="return CheckAll();">
     <span class="Accept"></span>
            Apply
    </asp:LinkButton>
    <a href="Ad_Service_Edit.aspx" runat="server" id="link_Add"><span class="Add"></span>Thêm </a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_ToolBox" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_Search" runat="server">
    <div class="Edit_Left">
        <div class="Edit_Title">
            Nhóm dịch vụ
        </div>
        <div class="Edit_Control">
            <select runat="server" id="sel_ServiceGroup">
            </select>
        </div>
          <div class="Edit_Title">
            Loại dịch vụ
        </div>
        <div class="Edit_Control">
            <select runat="server" id="sel_ServiceType">
            </select>
        </div>         
        <div class="Edit_Title">
            Tên dịch vụ
        </div>
        <div class="Edit_Control">
            <input type="text" runat="server" id="tbx_ServiceName" />
        </div>
        <div class="Edit_Title" style="height: 40px;">
            Ảnh đại diện 1</div>
        <div class="Edit_Control" style="height: 50px;">
            <div class="Upload">
                <input type="file" runat="server" id="file_UploadImage_1" />
                <input type="text" runat="server" id="tbx_UploadImage_1" />
            </div>
            <div class="UploadImage">
                <img runat="server" id="img_Upload_1" src="../Images/Images/NoImage.jpg" style="float: left; height: 50px; margin-left: 10px;" />
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
                <img runat="server" id="img_Upload_2" src="../Images/Images/NoImage.jpg" style="float: left; height: 50px; margin-left: 10px;" />
            </div>
        </div>
        <div class="Edit_Title">
            &nbsp;
        </div>
        <div class="Edit_Control">
            <asp:Button runat="server" ID="btn_UploadImage" Text="Upload" ToolTip="Upload Image" OnClick="btn_UploadImage_Click" />
        </div>
        <div class="NewLine">
            <div class="Edit_Title" style="height: 300px;">
                Mô tả:</div>
            <div class="Edit_Control_Editor">
                <CE:Editor ID="tbx_Description" runat="server" Width="100%" Height="302px" ThemeType="Office2007" RemoveServerNamesFromUrl="true" ConfigurationPath="~/CuteSoft_Client/CuteEditor/Configuration/AutoConfigure/MySimple.config">
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
                TableName</div>
            <div class="Properties_Control">
                <input type="text" runat="server" id="tbx_TableName" />
            </div>
            <div class="Properties_Title">
                Thời gian Push MT</div>
            <div class="Properties_Control">
                <input type="text" runat="server" id="tbx_PushTime" />
            </div>
            <div class="Properties_Title">
                Số lượng MT</div>
            <div class="Properties_Control">
                <input type="text" runat="server" id="tbx_MTNumber" />
            </div>
             <div class="Properties_Title">
                Cấu trúc Đăng ký</div>
            <div class="Properties_Control">
                <input type="text" runat="server" id="tbx_RegKeyword" />
            </div>
             <div class="Properties_Title">
                Cấu trúc Hủy</div>
            <div class="Properties_Control">
                <input type="text" runat="server" id="tbx_DeregKeyword" />
            </div>
            <div class="Properties_Title">
                Đối tác</div>
            <div class="Properties_Control">
                <select runat="server" id="sel_Partner">
                </select>
            </div>
            <div class="Properties_Title">
                Giá</div>
            <div class="Properties_Control">
                <input type="text" runat="server" id="tbx_Price" />
            </div>
            <div class="Properties_Title">
                Ưu tiên:</div>
            <div class="Properties_Control">
                <input type="text" runat="server" id="tbx_Priority" value="0" onkeypress="return isNumberKey_int(event);" />
            </div>
            <div class="Properties_Title">
                Kích hoạt:</div>
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
