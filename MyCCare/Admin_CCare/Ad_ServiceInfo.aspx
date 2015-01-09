<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Ad_ServiceInfo.aspx.cs" Inherits="MyCCare.Admin_CCare.Ad_ServiceInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_Header" runat="server">

    <style type="text/css">
        table.MsoNormalTable {
            line-height: 150%;
            font-size: 14.0pt;
            font-family: "Times New Roman","serif";
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Content" runat="server">
    <div id="menutabs1" class='mt10'>
        <a class="" href="Ad_SubInfo.aspx">
            <img class='icon1' src='../images/icon1.png'><span>Tra cứu thuê bao</span></a>
        <a class="" href="Ad_HistoryRegDereg.aspx">
            <img class='icon2' src='../images/icon2.png'><span>Tra cứu sử dụng dịch vụ</span></a>
        <a class="" href="Ad_RegDereg.aspx">
            <img class='icon3' src='../images/icon3.png'><span>Cài đặt dịch vụ</span></a>
        <a class="selected" href="Ad_ServiceInfo.aspx">
            <img class='icon4' src='../images/icon4.png'><span>Thông tin dịch vụ</span></a>
    </div>
    <div class='p10 bor'>
        <h4 class='titlecheck'>Mô tả dịch vụ:</h4>
        <p>
            -	Giới thiệu dịch vụ: m-Traffic là dịch vụ cung cấp thông tin và tình hình giao thông trên kênh FM 91Mhz – VOV giao thông tới người dùng dưới dạng bản tin thông qua kết nối trên điện thoại di động.<br />
            -	Khi khách hàng đăng ký thành công dịch vụ mTraffic, hàng ngày khách hàng sẽ nhận được các bản tin về dịch vụ mTraffic (khoảng 2 bản tin dài/ ngày).<br />
            -	Đầu số dịch vụ: 1546<br />
            -   Website của dịch vụ: http://mtraffic.vn.<br />
            -   Dịch vụ được cung cấp trên các kênh: Kênh SMS, Web, Wap.<br />
            -   Hình thức triển khai: SMS Subscription.<br />
        </p>
        <br />
        <h4 class='titlecheck'>Cách sử dụng dịch vụ:</h4>
        <p>Để đăng ký dịch vụ và có cơ hội tham gia chương trình khuyến mại, khách hàng soạn tin DK gửi 1546 (cước thuê bao: 1.000đ/ngày, miễn phí ngày đầu tiên cho các khách hàng đăng ký dịch vụ lần đầu)</p>
        <br />
        <h4 class='titlecheck'>Giá cước:</h4>
        <p>
            1.000 đồng/ngày/ 1 gói dịch vụ
        </p>
        <br />
        <h4 class='titlecheck'>Đăng ký/Hủy:</h4>
        <div>
            <table border="1" cellpadding="10" cellspacing="1">
                <tr>
                    <td>TT
                    </td>
                    <td>Dịch vụ
                    </td>
                    <td>Cú pháp Đăng ký
                    </td>
                    <td>Cú pháp Hủy dịch vụ
                    </td>
                </tr>
                <tr>
                    <td>1</td>
                    <td>Tin giao thông tổng hợp</td>
                    <td>DK</td>
                    <td>HUY</td>
                </tr>
                 <tr>
                    <td>2</td>
                    <td>Khám phá</td>
                    <td>DK KP</td>
                    <td>HUY KP</td>
                </tr>
                 <tr>
                    <td>3</td>
                    <td>Luật giao thông </td>
                    <td>DK LGT</td>
                    <td>HUY LGT</td>
                </tr>
                 <tr>
                    <td>4</td>
                    <td>Tư vấn </td>
                    <td>DK TV</td>
                    <td>HUY TV</td>
                </tr>
                <tr>
                    <td>5</td>
                    <td>Hình sự </td>
                    <td>DK HS</td>
                    <td>HUY HS</td>
                </tr>

            </table>
        </div>
    </div>
</asp:Content>
