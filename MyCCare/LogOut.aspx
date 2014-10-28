<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogOut.aspx.cs" Inherits="MyCCare.LogOut" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="JS/jquery-1.8.3.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <script type="text/javascript">

            $(function ()
            {
                // log user out from SSO service
                $.get('http//10.211.0.250:8080/SSO/SSOService.svc/user/Logout?callback=?', {},
                    function (ssodata)
                    {
                        // client's no longer logged in, redirect to logon page
                        // giá trị trả về dạng json
                        //?( {"LogoutResult":true} );
                        document.location = 'http://192.168.39.226:9090/default.aspx';
                    }, 'jsonp');
            });
        </script>

    </form>
</body>
</html>
