<%@ Page Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true"
    CodeBehind="Ad_Member_Edit.aspx.cs" Inherits="Administrator.Admin.Ad_Member_Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_Header" runat="server">
    <style type="text/css">
        .Edit_Control input[type=text], .Edit_Control input[type=password]
        {
            width: 200px;
        }
    </style>
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
    <a href="Ad_Member_Edit.aspx" runat="server" id="link_Add">
        <span class="Add"></span>
        Thêm </a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_ToolBox" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_Search" runat="server">
    <div class="Edit_Left">
        <div class="Edit_Title">
            Nhóm thành viên:</div>
        <div class="Edit_Control">
            <select runat="server" id="sel_MemberGroup">
            </select>
        </div>
        <div class="Edit_Title">
            Họ và Tên:</div>
        <div class="Edit_Control">
            <input type="text" runat="server" id="tbx_MemberName" />
        </div>
        <div class="Edit_Title">
            Tên đăng nhập:</div>
        <div class="Edit_Control">
            <input type="text" runat="server" id="tbx_LoginName" maxlength="50" />
        </div>
        <div class="Edit_Title">
            Mật khẩu:</div>
        <div class="Edit_Control">
            <input type="password" runat="server" id="tbx_Pass" maxlength="20" />
        </div>
        <div class="Edit_Title">
            Nhắc lại mật khẩu:</div>
        <div class="Edit_Control">
            <input type="password" runat="server" id="tbx_RepeatPass" maxlength="20"/>
        </div>
        <div class="Edit_Title">
            Email:</div>
        <div class="Edit_Control">
            <input type="text" runat="server" id="tbx_Email" />
        </div>
        <div class="Edit_Title">
            Điện thoại:</div>
        <div class="Edit_Control">
            <input type="text" runat="server" id="tbx_Phone" />
        </div>
        <div class="Edit_Title">
            Địa chỉ:</div>
        <div class="Edit_Control">
            <input type="text" runat="server" id="tbx_Address" />
        </div>
        <div class="Edit_Title">
            Giới tính:</div>
        <div class="Edit_Control">
            <input id="rad_Male" type="radio" name="Sex" runat="server" checked="true" /><label>Nam</label>
            <input id="rad_Female" type="radio" name="Sex" runat="server" /><label>Nữ</label>
        </div>
        <div class="Edit_Title">
            Ngày sinh:</div>
        <div class="Edit_Control">
            <input type="text" runat="server" id="tbx_Birthday" />
        </div>
        <div class="Edit_Title" style="height: 40px;">
            Ảnh đại diện<br />(Width:Auto)</div>
        <div class="Edit_Control" style="height: 50px;">
            <div class="Upload">
                <input type="file" runat="server" id="file_UploadImage" />
                <input type="text" runat="server" id="tbx_UploadImage" />
            </div>
            <div class="UploadImage">
                <img runat="server" id="img_Upload" src="" style="float: left;
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

        var tbx_LoginName = document.getElementById("<%=tbx_LoginName.ClientID %>");
        var tbx_Pass = document.getElementById("<%=tbx_Pass.ClientID %>");
        var tbx_RepeatPass = document.getElementById("<%=tbx_RepeatPass.ClientID %>");
        
        function CheckAll()
        {
            if (tbx_LoginName.value.length < 1)
            {
                alert("Xin hãy nhập tên đăng nhập!");
                tbx_LoginName.focus();
                return false;
            }
            if (tbx_Pass.value.length < 1)
            {
                alert("Xin hãy nhập mật khẩu!");
                tbx_Pass.focus();
                return false;
            }
            if (tbx_RepeatPass.value.length < 1)
            {
                alert("Xin hãy nhập Nhắc lại mẩt khẩu!");
                tbx_RepeatPass.focus();
                return false;
            }
            if (tbx_RepeatPass.value != tbx_Pass.value)
            {
                alert("Mật khẩu và Nhắc lại mật khẩu là không giống nhau, xin hãy xem lại!");
                tbx_RepeatPass.focus();
                return false;
            }

            return true;
        }

        tbx_Pass.value = '<%= ViewState["Pass"] == null?"":ViewState["Pass"].ToString().Substring(0,10)%>';
        tbx_RepeatPass.value = '<%= ViewState["Pass"] == null?"":ViewState["Pass"].ToString().Substring(0,10)%>';
    </script>

</asp:Content>
