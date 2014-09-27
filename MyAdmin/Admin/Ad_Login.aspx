<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Ad_Login.aspx.cs" Inherits="MyAdmin.Admin.Ad_Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title> .:: MTraffic CMS - Login ::. </title>
    <link href="../CSS/ForAll.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/Frames.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/Admin_Login.css" rel="stylesheet" type="text/css" />    
    <script src="../Javascript/jquery.js" type="text/javascript"></script>
    <script src="../Javascript/ForAll.js" type="text/javascript"></script>
    <script src="../Javascript/Frames.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
     <div class="NewLinde_Pad">
        <div style="margin: 0 auto; width: 440px; padding-top:70px;">
            <div class="LoGo">
               
            </div>
            <div class="Frame_1" id="Frame_1" style="padding-top:40px;">
                <div class="DangKy">
                    <div class="NewLine_Pad" style="padding-top: 10px;">
                        <div class="DangKy_Title">
                            Tên đăng nhập:</div>
                        <div class="DangKy_Control">
                            <input type="text" runat="server" id="tbx_LoginName" />
                        </div>
                    </div>
                     <div class="NewLine_Pad" style="padding-top: 10px;">
                        <div class="DangKy_Title">
                            Mật khẩu:</div>
                        <div class="DangKy_Control">
                            <input type="password" runat="server" id="tbx_Password" />
                        </div>
                    
                    </div>         
                    <div class="NewLine_Pad">
                    <div class="DangKy_Title">
                            &nbsp;</div>
                        <div class="DangKy_Control">
                        <img id="img_captcha" src="../Captcha.ashx" style="vertical-align:middle;width:150px; height:50px; margin-left:2px;" />
                        <a style="vertical-align:middle;" title="Đỗi mã khác" href="javascript:void(0);" onclick="refresh_captcha('img_captcha');"><img src="../Images/Buttons/Refresh.png" /></a>
                        </div>
                    </div>
                    <div class="NewLine_Pad">
                         <div class="DangKy_Title">
                            Nhập mã xác thực:</div>
                        <div class="DangKy_Control">
                            <input type="text" runat="server" autocomplete="off" id="tbx_Captcha" />
                            
                            
                        </div>
                    </div>           
                    <div class="DangKy_Button">
                        <asp:Button runat="server" ID="btn_Login" Text="Đăng nhập" OnClientClick="return CheckAll();"
                            OnClick="btn_Login_Click" />
                    </div>
                     <div class="NewLine" runat="server" id="div_Loi" visible="false">
                        <div class="NewLine_Pad DangKy_Error">
                            <strong>Lỗi!</strong>
                        </div>
                        <div class="NewLine_Pad DangKy_Error" style="font-size: 11px;">
                           <%=AlertMessage %>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <script language="javascript" type="text/javascript">
        Create_Frame(1, "Frame_1", "Đăng nhập");

        function CheckAll()
        {
            var tbx_LoginName = document.getElementById('<%=tbx_LoginName.ClientID %>');
            if (tbx_LoginName.value == "")
            {
                alert("Xin hãy nhập Tên đăng nhập!");
                tbx_LoginName.focus();
                return false;
            }
            var tbx_Password = document.getElementById('<%=tbx_Password.ClientID %>');
            if (tbx_Password.value == "")
            {
                alert("Xin hãy nhập Mật khẩu!");
                tbx_Password.focus();
                return false;
            }
            return true;
        }

        function refresh_captcha(ctr_id) 
        {
            var src = "../captcha.ashx"+ "?time=" + (new Date()).getTime();
            $("#" + ctr_id).attr("src", src);
        }

    </script>
    </form>
</body>
</html>
