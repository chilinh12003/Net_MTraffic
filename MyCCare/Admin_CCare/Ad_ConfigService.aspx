<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Ad_ConfigService.aspx.cs" Inherits="MyCCare.Admin_CCare.Ad_ConfigService" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Content" runat="server">
    <div id="menutabs1" class='mt10'>
        <a class="" href="Ad_SubInfo.aspx">
            <img class='icon1' src='../images/icon1.png'><span>Tra cứu thuê bao</span></a>
        <a class="" href="Ad_HistoryRegDereg.aspx">
            <img class='icon2' src='../images/icon2.png'><span>Tra cứu sử dụng dịch vụ</span></a>
        <a class="selected" href="Ad_RegDereg.aspx">
            <img class='icon3' src='../images/icon3.png'><span>Cài đặt dịch vụ</span></a>
        <a class="" href="Ad_ServiceInfo.aspx">
            <img class='icon4' src='../images/icon4.png'><span>Thông tin dịch vụ</span></a>
    </div>
    <div class='tabtracuusddv'>
        <ul>
            <li><a href='Ad_RegDereg.aspx'>Đăng ký / Hủy / Reset</a></li>
            <li><a href='Ad_ResendMT.aspx'>Bù nội dung</a></li>
            <li><a href='Ad_ConfigService.aspx' class='select'>Cài đặt dịch vụ</a></li>
        </ul>
    </div>
    <div class='fillterarea'>
        <table cellspacing="5" cellpadding="5">
            <tbody>
                <tr>
                    <td width="100">Số thuê bao:</td>
                    <td colspan="5">
                        <input style='width: 147px' type='text' class='textbox' runat="server" id="tbx_MSISDN" /></td>
                    <td>Gói cước:</td>
                    <td>
                        <select style='width: 150px' class="dropdownlist" runat="server" id="sel_Service">
                            <option>-- Chọn gói cước --</option>
                        </select></td>
                    <td width="200" align="right" rowspan="2">
                        <asp:Button CssClass="btn_search" runat="server" ID="btn_Search" Text="Tra cứu" OnClick="btn_Search_Click" /></td>
                </tr>
            </tbody>
        </table>
    </div>
    <%if(IsShow()){ %>
    <div class='p10 bor'>
        <table class="tbl_style center bumaduthuong" style='width: auto'>
            <tbody>
                <tr>
                    <th>Nội dung</th>
                    <th width='160'>Giá trị</th>
                </tr>
                <tr>
                    <td>Nhận thông báo về thông tin trận đấu</td>
                   <td><select runat="server" id="sel_IsNotify">
                       <option value="0">Nhận thông báo</option>
                       <option value="1">Không nhận thông báo</option>
                       </select></td>
                </tr>
                <tr>
                    <td colspan='2' align='right'>
                        <asp:Button runat="server" ID="btn_Save" Text="Thực hiện" OnClick="btn_Save_Click" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <%} %>

</asp:Content>
