<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubInfo.aspx.cs" Inherits="MyAdmin.Admin_CCare.SubInfo" %>

<%@ Register Src="../Admin_Control/Admin_Paging_VNP.ascx" TagName="Admin_Paging_VNP" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <title>VAS151</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">
    <!-- Le styles -->
    <link href="../CSS/bootstrap.css" rel="stylesheet" type="text/css" />
    <style>
        body
        {
            padding-top: 60px; /* 60px to make the container go all the way to the bottom of the topbar */
        }
    </style>
    <link href="http://vinabox.vinaphone.com.vn/static/css/bootstrap-responsive.css" rel="stylesheet">
    <style type="text/css">
        .sidebar-nav
        {
            padding: 9px 0;
        }
        .navbar .brand
        {
            padding: 5px 20px !important;
        }
        .hero-unit
        {
            padding: 9px !important;
        }
        li.active, p.vas-header
        {
            background-color: #0088CC;
            color: #FFFFFF;
        }
        p.vas-header
        {
            padding-left: 5px;
        }
        .hero-unit1
        {
            padding: 9px !important;
        }
        .local-active
        {
            background-color: #F5F5F5;
            color: #C8A999;
        }
        .local-disable
        {
            background-color: #F5F5F5;
            color: #C8A999;
        }
    </style>
    <!-- Le HTML5 shim, for IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>      <script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>    <![endif]-->
</head>
<body>
    <form id="form1" runat="server">
    <div class="navbar navbar-inverse navbar-fixed-top">
        <div class="navbar-inner">
            <div class="container-fluid">
                <a data-target=".nav-collapse" data-toggle="collapse" class="btn btn-navbar"><span class="icon-bar"></span><span class="icon-bar"></span><span class="icon-bar"></span></a><a class="brand" href="javascript:void(0);">
                    <img src="http://vinabox.vinaphone.com.vn/static/web/images/vinabox_logo.png" alt="VINABOX" width="41" height="33" />CSKH DỊCH VỤ MTRAFFIC</a>
            </div>
        </div>
    </div>
    <div class="container-fluid">
        <div class="row-fluid">
            <div class="span12">
                <div class="alert alert-error" style="display: none">
                </div>
                <div class="hero-unit">
                    <p class="vas-header">
                        Thông tin thuê bao
                        <%=MSISDN %></p>
                    <table class="table table-striped">
                        <tr>
                            <th>
                                Số điện Thoại
                            </th>
                            <th>
                                Tình trạng
                            </th>
                            <th>
                                Gói dịch vụ
                            </th>
                            <th>
                                Ngày đăng ký
                            </th>
                            <th>
                                Ngày hết hạn
                            </th>
                        </tr>
                        <asp:Repeater runat="server" ID="rpt_Sub">
                        <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("MSISDN")%>
                            </td>
                            <td>
                                <%# MyUtility.MyEnum.StringValueOf((MyMTraffic.Sub.Subscriber.Status)int.Parse(Eval("StatusID").ToString())) %>
                            </td>
                            <td>
                                <%#Eval("ServiceName")%>
                            </td>
                            <td>
                                <%#Eval("EffectiveDate")%>
                            </td>
                            <td>
                                <%#Eval("ExpiryDate")%>
                            </td>
                        </tr>
                        </ItemTemplate>
                        </asp:Repeater>
                        
                    </table>
                </div>
                <div class="hero-unit">
                    <p class="vas-header">
                        Lịch sử đăng ký/huỷ dịch vụ của thuê bao
                        <%=MSISDN %></p>
                    <table class="table table-striped">
                        <tr>
                            <th width="6%">
                                STT
                            </th>
                            <th width="20%">
                                Thời gian giao dịch</td>
                                <th width="18%">
                                    Loại giao dịch
                                </th>
                                <th width="14%">
                                    Tên gói cước
                                </th>
                                <th width="11%">
                                    Trạng thái
                                </th>
                                <th width="7%">
                                    Ứng dụng
                                </th>
                                <th width="13%">
                                    Kênh thực hiện
                                </th>
                                <th width="7%">
                                    UserName
                                </th>
                                <th width="7%">
                                    UserIP
                                </th>
                                <th width="11%">
                                    Cước phí
                                </th>
                        </tr>
                        <asp:Repeater runat="server" ID="rpt_Reg">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%#(Container.ItemIndex + PageIndex_1).ToString()%>
                                    </td>
                                    <td>
                                        <%#Eval("ChargeDate") == DBNull.Value ? string.Empty : ((DateTime)Eval("ChargeDate")).ToString(MyUtility.MyConfig.LongDateFormat)%>
                                    </td>
                                    <td>
                                        <%#Eval("ActionName")%>
                                    </td>
                                    <td>
                                        <%#Eval("ServiceName")%>
                                    </td>
                                    <td>
                                        <%#Eval("ChargeStatusName")%>
                                    </td>
                                    <td>
                                    <%#Eval("AppName")%>
                                    </td>
                                    <td>
                                        <%#Eval("ChannelTypeName")%>
                                    </td>
                                    <td>
                                    <%#Eval("UserName")%>
                                    </td>
                                    <td>
                                    <%#Eval("IP")%>
                                    </td>
                                    <td>
                                        <%#Eval("Price")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                    <uc1:Admin_Paging_VNP ID="Admin_Paging_VNP1" runat="server" />
                </div>
                <div class="hero-unit">
                    <p class="vas-header">
                        Lịch sử gia hạn dịch vụ của thuê bao
                        <%=MSISDN %></p>
                    <table class="table table-striped">
                        <tr>
                            <th>
                                STT
                            </th>
                            <th>
                                Thời gian giao dịch</td>
                                <th>
                                    Loại giao dịch
                                </th>
                                <th>
                                    Tên gói cước
                                </th>
                                <th>
                                    Trạng thái
                                </th>
                                <th>
                                    Kênh thực hiện
                                </th>
                                <th>
                                    Cước phí
                                </th>
                        </tr>
                        <asp:Repeater runat="server" ID="rpt_Renew">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%#(Container.ItemIndex + PageIndex_2).ToString()%>
                                    </td>
                                    <td>
                                        <%#Eval("ChargeDate") == DBNull.Value ? string.Empty : ((DateTime)Eval("ChargeDate")).ToString(MyUtility.MyConfig.LongDateFormat)%>
                                    </td>
                                    <td>
                                        Gia hạn gói dịch vụ
                                    </td>
                                    <td>
                                        <%#Eval("ServiceName")%>
                                    </td>
                                    <td>
                                        <%#Eval("ChargeStatusName")%>
                                    </td>
                                    <td>
                                        <%#Eval("ChannelTypeName")%>
                                    </td>
                                    <td>
                                        <%#Eval("Price")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                    <uc1:Admin_Paging_VNP ID="Admin_Paging_VNP2" runat="server" />
                </div>
                <div class="hero-unit">
                    <p class="vas-header">
                        Lịch sử MO/MT của thuê bao
                        <%=MSISDN %></p>
                    <table class="table table-striped">
                        <tr>
                            <th>
                                STT
                            </th>
                            <th>
                                Thời gian nhận
                                <th>
                                    MO
                                </th>
                                <th>
                                    Trạng thái
                                </th>
                                <th>
                                    Đầu số
                                </th>
                                <th>
                                    Thời gian gửi
                                </th>
                                <th width="40%">
                                    MT
                                </th>
                                <th>
                                    Trạng thái
                                </th>
                                <th>
                                    Cước phí
                                </th>
                        </tr>
                        <asp:Repeater runat="server" ID="rpt_MOLog">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%#(Container.ItemIndex + PageIndex_3).ToString()%>
                                    </td>
                                    <td>
                                        <%#Eval("ReceiveDate") == DBNull.Value ? string.Empty : ((DateTime)Eval("ReceiveDate")).ToString(MyUtility.MyConfig.LongDateFormat)%>
                                    </td>
                                    <td>
                                        <%#Eval("MO")%>
                                    </td>
                                    <td>
                                        Đã xử lý
                                    </td>
                                    <td>
                                        1546
                                    </td>
                                    <td>
                                        <%#Eval("LogDate") == DBNull.Value ? string.Empty : ((DateTime)Eval("LogDate")).ToString(MyUtility.MyConfig.LongDateFormat)%>
                                    </td>
                                    <td>
                                        <%#Eval("MT")%>
                                    </td>
                                    <td>
                                        Đã gửi
                                    </td>
                                    <td>
                                        0
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                    <uc1:Admin_Paging_VNP ID="Admin_Paging_VNP3" runat="server" />
                    <p>
                        &nbsp;</p>
                </div>
                 <div class="hero-unit">
                    <p class="vas-header">
                        Lịch sử tương tác, sử dụng dịch vụ của thuê bao
                        <%=MSISDN %></p>
                    <table class="table table-striped">
                        <tr>
                            <th>
                                STT
                            </th>
                            <th>
                                Thời gian tương tác
                            </th>
                            <th>
                                Tên gói cước
                            </th>
                            <th>
                                Thao tác
                            </th>
                            <th>
                                Mô tả
                            </th>
                            <th>
                                Kênh tương tác
                            </th>
                        </tr>
                        <asp:Repeater runat="server" ID="rpt_Action">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%#(Container.ItemIndex + PageIndex_3).ToString()%>
                                    </td>
                                    <td>
                                        <%#Eval("LogDate") == DBNull.Value ? string.Empty : ((DateTime)Eval("LogDate")).ToString(MyUtility.MyConfig.LongDateFormat)%>
                                    </td>
                                    <td>
                                        <%#Eval("ServiceName")%>
                                    </td>
                                    <td>
                                        <%#Eval("ActionName")%>
                                    </td>
                                    <td>
                                         <%#Eval("Description")%>
                                    </td>
                                  
                                    <td>
                                        <%#Eval("ChannelTypeName")%>
                                    </td>                                   
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                    <uc1:Admin_Paging_VNP ID="Admin_Paging_VNP_Action" runat="server" />
                    <p>
                        &nbsp;</p>
                </div>
                <!--/span-->
            </div>
            <p>
                <!--/row-->
            </p>
            <footer>        <p>&copy; HB 2013</p>      </footer>
        </div>
        <!--/.fluid-container-->
        <!-- Le javascript    ================================================== -->
        <script src="http://vinabox.vinaphone.com.vn/static/js/jquery-1.7.2.min.js"></script>
        <script src="http://vinabox.vinaphone.com.vn/static/js/bootstrap.min.js"></script>
    </form>
</body>
</html>
