<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MyCCare.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="JS/jquery-1.8.3.min.js" type="text/javascript"></script>
</head>
<body>
    
    <form id="form1" runat="server">

        <script type="text/javascript">

            $(document).ready(function ()
            {
                $(function ()
                {
                    $.get('<%=MyCCare.Login1.SSOLink %>/SSO/SSOService.svc/user/RequestToken?callback=?', {},
                    function (ssodata)
                    {
                        // get url to logon page in case this operation fails
                        var logonPage = '<%=MyCCare.Login1.SSOLink %>/SSO/Login.aspx?keyid=10020&URL=<%=MyUtility.MyConfig.Domain %>/Login.aspx';
                        var IndexCP = '<%=MyUtility.MyConfig.Domain %>/Default.aspx'; //trang của dịch vụ
                        if (ssodata.Status == 'SUCCESS')
                        {
                            //verify the token is genuine
                            $.ajax({
                                type: "GET",
                                url: "<%=MyUtility.MyConfig.Domain %>" + "/Login.ashx",
                                data: { type: "ValidateToken", token: ssodata.Token },
                                contentType: "application/json; charset=utf-8",
                                dataType: "text",
                                success: function (data)
                                {
                                    if (data == 0)
                                    {
                                        alert("Không có quyền đăng nhập vào hệ thống")
                                        //document.location = CCOSIndexPage;
                                    }
                                    else
                                    {
                                        alert("Đăng nhập thành công điều hướng về trang của CP")
                                        document.location = IndexCP;
                                    }
                                },
                                error: function (data)
                                {
                                    document.location.reload();
                                    alert('Lỗi đăng nhập');
                                }
                            });

                        } else
                        {
                            // user needs to logon to SSO service
                            document.location = logonPage;
                        }
                        // tell jQuery to use JSONP 
                    }, 'jsonp');
                });
            });
        </script>
    </form>
</body>
</html>
