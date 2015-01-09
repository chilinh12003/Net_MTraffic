<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Ad_ResendMT.aspx.cs" Inherits="MyCCare.Admin_CCare.Ad_ResendMT" %>


<%@ Register Src="../Admin_Control/Admin_Paging.ascx" TagName="Admin_Paging" TagPrefix="uc1" %>

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
        <li><a href='Ad_RegDereg.aspx' >Đăng ký / Hủy / Reset</a></li>
        <li><a href='Ad_ResendMT.aspx' class='select'>Bù nội dung</a></li>
            </ul>
    </div>
    <div class='fillterarea'>
        <table>
            <tr>
                <td width='100'>Số thuê bao:</td>
                <td colspan="5">
                    <input style='width: 147px' type='text' class='textbox' runat="server" id="tbx_MSISDN" /></td>
                <td rowspan='2' align='right' width='200'>
                   <asp:Button runat="server" CssClass="btn_search" ID="btn_Search" Text="Tra cứu" OnClick="btn_Search_Click" /></td>
            </tr>
            <tr>
                <td>Gói cước:</td>
                <td>
                    <select style='width: 150px' class="dropdownlist" runat="server" id="sel_Service">
                    </select></td>
                <td>Thời gian bắt đầu:</td>
                <td>
                    <input type="text" class="datetime" runat="server" id="tbx_FromDate" onclick="displayCalendar(this, 'dd/mm/yyyy', this);" /></td>
                
                <td>Thời gian kết thúc</td>
                <td>
                    <input type="text" class="datetime" runat="server" id="tbx_ToDate" onclick="displayCalendar(this, 'dd/mm/yyyy', this);" /></td>
            </tr>
        </table>
    </div>
    <table class='tbl_style'>
        <thead>
            <tr>
                <th>Gói cước</th>
           <th>Thời gian</th>
			<th style="width:400px;">Nội dung MT</th>
			<th>Tình trạng</th>
			<th>Hành động</th>
                </tr>
        </thead>
        <asp:Repeater runat="server" ID="rpt_Data">
            <ItemTemplate>
                <tr>
                    <td align='center'><%#Eval("ServiceName")%></td>
                    <td align='center'><%#Eval("LogDate") == DBNull.Value ? string.Empty : ((DateTime)Eval("LogDate")).ToString(MyUtility.MyConfig.LongDateFormat)%></td>
                    <td align='center'><%#Eval("MT")%></td>
                    <td align='center'>Đã gửi</td>
                    <td align='center'><asp:Button CssClass="resend"  Enabled="<%#MyCCare.Login1.IsAdmin() %>" runat="server" ID="btn_Resend" Text="Gửi lại" CommandArgument='<%# Eval("LogID") %>' OnClick="btn_Resend_Click" OnClientClick="return confirm('Bạn muộn gửi lại MT cho số thuê bao này?');"></asp:Button></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>

    <uc1:Admin_Paging ID="Admin_Paging1" runat="server" />

</asp:Content>
