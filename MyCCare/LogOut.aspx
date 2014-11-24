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
                $.get('<%=MyCCare.Login1.SSOLink %>/SSO/SSOService.svc/user/Logout?callback=?', {},
                    function (ssodata)
                    {
                        // client's no longer logged in, redirect to logon page
                        // giá trị trả về dạng json
                        //?( {"LogoutResult":true} );
                        document.location = '<%=MyUtility.MyConfig.Domain %>/cskh/login.aspx';
                    }, 'jsonp');
            });
        </script>

    </form>
</body>
</html>
