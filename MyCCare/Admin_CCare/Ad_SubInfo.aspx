<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Ad_SubInfo.aspx.cs" Inherits="MyCCare.Admin_CCare.Ad_SubInfo" %>


<%@ Register Src="../Admin_Control/Admin_Paging.ascx" TagName="Admin_Paging" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Content" runat="server">
    <div id="menutabs1" class='mt10'>
        <a class="selected" href="Ad_SubInfo.aspx">
            <img class='icon1' src='../images/icon1.png'><span>Tra cứu thuê bao</span></a>
        <a class="" href="Ad_HistoryRegDereg.aspx">
            <img class='icon2' src='../images/icon2.png'><span>Tra cứu sử dụng dịch vụ</span></a>
        <a class="" href="Ad_RegDereg.aspx">
            <img class='icon3' src='../images/icon3.png'><span>Cài đặt dịch vụ</span></a>
        <a class="" href="Ad_ServiceInfo.aspx">
            <img class='icon4' src='../images/icon4.png'><span>Thông tin dịch vụ</span></a>
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
        </table>
    </div>
    <table class='tbl_style'>
        <thead>
          <tr>
					<th>Dịch vụ</th>
					<th>Gói cước</th>
					<th>Trạng thái</th>
					<th>Ngày đăng ký</th>
					<th>Hiệu lực gói cước</th>
				</tr>
        </thead>
        <asp:Repeater runat="server" ID="rpt_Data">
            <ItemTemplate>
                <tr>
                    <td align='center'>MTraffic</td>
                    <td align='center'><%#Eval("ServiceName")%></td>
                    <td align='center'><%#Eval("StatusName")%></td>
                    <td align='center'><%#Eval("EffectiveDate") == DBNull.Value ? string.Empty : ((DateTime)Eval("EffectiveDate")).ToString(MyUtility.MyConfig.LongDateFormat)%></td>
                    <td align='center'><%#Eval("ExpiryDate") == DBNull.Value ? string.Empty : ((DateTime)Eval("ExpiryDate")).ToString(MyUtility.MyConfig.LongDateFormat)%></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>

    <uc1:Admin_Paging ID="Admin_Paging1" runat="server" />

</asp:Content>
