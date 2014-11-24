<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Ad_RP_Sub.aspx.cs" Inherits="MyAdmin.Admin_Report.Ad_RP_Sub" %>

<%@ Register Src="../Admin_Control/Admin_Paging.ascx" TagName="Admin_Paging" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Header" runat="server">
    <link href="../Calendar/dhtmlgoodies_calendar/dhtmlgoodies_calendar.css" rel="stylesheet" type="text/css" />
    <script src="../Calendar/dhtmlgoodies_calendar/dhtmlgoodies_calendar.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Tools" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_ToolBox" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_Search" runat="server">
    <label>
        Từ ngày:</label>
    <input type="text" runat="server" id="tbx_FromDate" style="width: 70px;" />
    <input type="button" value="..." onclick="displayCalendar(document.getElementById('<%=tbx_FromDate.ClientID %>'),'dd/mm/yyyy',this)" />
    <label>
        Đến ngày:</label>
    <input type="text" runat="server" id="tbx_ToDate" style="width: 70px;" />
    <input type="button" value="..." onclick="displayCalendar(document.getElementById('<%=tbx_ToDate.ClientID %>'),'dd/mm/yyyy',this)" />
    <asp:Button runat="server" ID="btn_Search" Text="Tìm kiếm" OnClick="btn_Search_Click" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph_Content" runat="server">
     <table class="Data" border="0" cellpadding="0" cellspacing="0">
        <tbody>
            <tr class="Table_Header">
                <th class="Table_TL border-none"></th>
                <th class="last " rowspan="2">STT</th>
                <th class="last" rowspan="2">Ngày</th>
                <th class="last" rowspan="2">Dịch vụ</th>
                <th colspan="5">Đăng ký</th>
                <th  colspan="5">Hủy</th>
                <th  colspan="4">Gia hạn</th>
                <th colspan="4">Doanh thu</th>
                <th class="Table_TR border-none"></th>
            </tr>
       <tr class="Table_Header">

        <th class="Table_TL_Repeat last border-none"></th>
            
            <th class="last">Tổng</th>
            <th class="last">Mới</th>
            <th class="last">SMS</th>
            <th class="last">WAP</th>
            <th class="last">Other</th>
            <th class="last">Tổng</th>
            <th class="last">Mới</th>
            <th class="last">Tự hủy</th>
            <th class="last">MaxRetry</th>
            <th class="last">Other</th>
            <th class="last">Tổng</th>
            <th class="last">Thành Công</th>
            <th class="last">Thất bại</th>
            <th class="last">Tỷ lệ (%)</th>
            <th class="last">Đăng ký</th>
            <th class="last">Gia hạn</th>
            <th class="last">Tổng</th>
            <th class="last" style="color:Red;" >TỔNG TIỀN</th>
            <th class="Table_TR_Repeat last border-none"></th>
        </tr>
           <asp:Repeater runat="server" ID="rpt_Data">
                <ItemTemplate>
                 <tr class="Table_Row_1">
                         <td  class="Table_ML_1 border-none">
                        </td>
                     <td><%#(Container.ItemIndex + PageIndex).ToString()%></td>
                    <%#this.GetReport_HTML((DateTime)Eval("ReportDay"))%>
                     <td><%# Eval("ServiceName").ToString()%></td>
                     <td><%#((double)Eval("SubTotal")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("SubNew")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("SubSMS")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("SubWAP")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("SubOther")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("UnsubTotal")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("UnsubNew")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("UnsubSelf")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("UnsubExtend")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("UnsubOther")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("RenewTotal")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("RenewSuccess")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("RenewFail")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("RenewRate")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("SaleReg")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("SaleRenew")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("SaleReg") + (double)Eval("SaleRenew") ).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <%#this.GetTotalMoneyByDay_HTML((DateTime)Eval("ReportDay"))%>
                  <td  class="Table_MR_1 border-none">
                        </td>
                    </tr>
         </ItemTemplate>
                <AlternatingItemTemplate>
                 <tr class="Table_Row_2">
                         <td class="Table_ML_2 border-none">
                        </td>
                    <td><%#(Container.ItemIndex + PageIndex).ToString()%></td>
                    <%#this.GetReport_HTML((DateTime)Eval("ReportDay"))%>
                     <td><%# Eval("ServiceName").ToString()%></td>
                     <td><%#((double)Eval("SubTotal")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("SubNew")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("SubSMS")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("SubWAP")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("SubOther")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("UnsubTotal")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("UnsubNew")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("UnsubSelf")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("UnsubExtend")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("UnsubOther")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("RenewTotal")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("RenewSuccess")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("RenewFail")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("RenewRate")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("SaleReg")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("SaleRenew")).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <td><%#((double)Eval("SaleReg") + (double)Eval("SaleRenew") ).ToString(MyUtility.MyConfig.IntFormat)%></td>
                     <%#this.GetTotalMoneyByDay_HTML((DateTime)Eval("ReportDay"))%>
                 <td  class="Table_MR_2 border-none">
                        </td>
                    </tr>
         </AlternatingItemTemplate>
         </asp:Repeater>
    </table>
     <div class="Table_Footer">
        <div class="Table_BL">
            <uc1:Admin_Paging ID="Admin_Paging1" runat="server" />
        </div>
        <div class="Table_BR">
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cph_Javascript" runat="server">
</asp:Content>
