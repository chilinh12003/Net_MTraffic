<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Ad_CheckInfo.aspx.cs" Inherits="MyAdmin.Admin_CCare.Ad_CheckInfo" %>

<%@ Register Src="../Admin_Control/Admin_Paging.ascx" TagName="Admin_Paging" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Header" runat="server">
    <link href="../CSS/CheckInfo.css" rel="stylesheet" type="text/css" />
    <link href="../Calendar/dhtmlgoodies_calendar/dhtmlgoodies_calendar.css" rel="stylesheet" type="text/css" />
    <script src="../Calendar/dhtmlgoodies_calendar/dhtmlgoodies_calendar.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Tools" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_ToolBox" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_Search" runat="server">
    <label>
        Số điện thoại
    </label>
    <input type="text" runat="server" id="tbx_MSISDN" />
    <asp:Button runat="server" ID="tbx_Search" Text="Tra cứu" OnClick="tbx_Search_Click" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph_Content" runat="server">
    <div class="check-info">
        <div class="left">
            <fieldset>
                <legend>Dịch vụ đã đăng ký</legend>
                <div class="data">
                    <asp:Repeater runat="server" ID="rpt_Sub">
                        <ItemTemplate>
                            <ul>
                                <li><span class="title">Dịch vụ:</span><span class="value"><%#Eval("ServiceName") %></span></li>
                                <li><span class="title">Ngày đăng ký:</span><span class="value"><%# ((DateTime)Eval("EffectiveDate")).ToString(MyUtility.MyConfig.LongDateFormat)%></span></li>
                                <li><span class="title">Ngày Hết hạn:</span><span class="value"><%# ((DateTime)Eval("ExpiryDate")).ToString(MyUtility.MyConfig.LongDateFormat)%></span></li>
                                <li><span class="title">Kênh đăng ký:</span><span class="value"><%#Eval("ChannelTypeName") %></span></li>
                                <li><span class="title">Tổng tin đã nhận:</span><span class="value"><%# ((int)Eval("TotalMT")).ToString(MyUtility.MyConfig.IntFormat)%></span></li>
                                <li><span class="title">Số tin trong ngày:</span><span class="value"><%#((int)Eval("TotalMTByDay")).ToString(MyUtility.MyConfig.IntFormat)%></span></li>
                            </ul>
                            <div class="line">
                            </div>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <ul>
                                <li><span class="title">Dịch vụ:</span><span class="value"><%#Eval("ServiceName") %></span></li>
                                <li><span class="title">Ngày đăng ký:</span><span class="value"><%# ((DateTime)Eval("EffectiveDate")).ToString(MyUtility.MyConfig.LongDateFormat)%></span></li>
                                <li><span class="title">Ngày Hết hạn:</span><span class="value"><%# ((DateTime)Eval("ExpiryDate")).ToString(MyUtility.MyConfig.LongDateFormat)%></span></li>
                                <li><span class="title">Kênh đăng ký:</span><span class="value"><%#Eval("ChannelTypeName") %></span></li>
                                <li><span class="title">Tổng tin đã nhận:</span><span class="value"><%# ((int)Eval("TotalMT")).ToString(MyUtility.MyConfig.IntFormat)%></span></li>
                                <li><span class="title">Số tin trong ngày:</span><span class="value"><%#((int)Eval("TotalMTByDay")).ToString(MyUtility.MyConfig.IntFormat)%></span></li>
                            </ul>
                            <div class="line">
                            </div>
                        </AlternatingItemTemplate>
                    </asp:Repeater>
                </div>
            </fieldset>
        </div>
        <div class="right">
        </div>
        <fieldset>
            <legend>Lịch sử hoạt động</legend>
            <label>
                Từ ngày:</label>
            <input type="text" runat="server" id="tbx_FromDate" style="width: 70px;" />
            <input type="button" value="..." onclick="displayCalendar(document.getElementById('<%=tbx_FromDate.ClientID %>'),'dd/mm/yyyy',this)" />
            <label>
                Đến ngày:</label>
            <input type="text" runat="server" id="tbx_ToDate" style="width: 70px;" />
            <input type="button" value="..." onclick="displayCalendar(document.getElementById('<%=tbx_ToDate.ClientID %>'),'dd/mm/yyyy',this)" />
            <select runat="server" id="sel_Service">
            </select>
            <asp:Button runat="server" ID="btn_PushMT" OnClick="btn_PushMT_Click" Text="LS MO/MT" />
            <asp:Button runat="server" ID="btn_MOlog" OnClick="btn_MOlog_Click" Text="LS Đăng ký/Hủy" />
            <asp:Button runat="server" ID="btn_ChargeLog" OnClick="btn_ChargeLog_Click" Text="LS gia hạn" />
            <asp:Button runat="server" ID="btn_Action" OnClick="btn_Action_Click" Text="LS tương tác" />
            <div class="NoiDung">
                <div runat="server" id="div_1">
                    <table class="Data" border="0" cellpadding="0" cellspacing="0">
                        <tbody>
                            <tr class="Table_Header">
                                <th class="Table_TL">
                                </th>
                                <th width="10">
                                    STT
                                </th>
                                <th>
                                    Dịch vụ
                                </th>
                                <th>
                                    Số điện thoại
                                </th>
                                <th>
                                    MO
                                </th>
                                <th style="width: 30%;">
                                    MT
                                </th>
                                <th>
                                    Thời gian
                                </th>
                                <th class="Table_TR">
                                </th>
                            </tr>
                            <asp:Repeater runat="server" ID="rpt_Data_1">
                                <ItemTemplate>
                                    <tr class="Table_Row_1">
                                        <td class="Table_ML_1"></td>
                                        <td>
                                            <%#(Container.ItemIndex + PageIndex).ToString()%>
                                        </td>
                                        <td>
                                            <%#Eval("ServiceName")%>
                                        </td>
                                        <td>
                                            <%#Eval("MSISDN")%>
                                        </td>
                                        <td>
                                            <%#Eval("MO")%>
                                        </td>
                                        <td>
                                            <%#Eval("MT")%>
                                        </td>
                                        <td>
                                            <%#Eval("LogDate") == DBNull.Value ? string.Empty : ((DateTime)Eval("LogDate")).ToString(MyUtility.MyConfig.LongDateFormat)%>
                                        </td>
                                        <td class="Table_MR_1"></td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="Table_Row_2">
                                        <td class="Table_ML_2"></td>
                                        <td>
                                            <%#(Container.ItemIndex + PageIndex).ToString()%>
                                        </td>
                                        <td>
                                            <%#Eval("ServiceName")%>
                                        </td>
                                        <td>
                                            <%#Eval("MSISDN")%>
                                        </td>
                                        <td>
                                            <%#Eval("MO")%>
                                        </td>
                                        <td>
                                            <%#Eval("MT")%>
                                        </td>
                                        <td>
                                            <%#Eval("LogDate") == DBNull.Value ? string.Empty : ((DateTime)Eval("LogDate")).ToString(MyUtility.MyConfig.LongDateFormat)%>
                                        </td>
                                        <td class="Table_MR_2"></td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
                <div runat="server" id="div_2">
                    <table class="Data" border="0" cellpadding="0" cellspacing="0">
                        <tbody>
                            <tr class="Table_Header">
                                <th class="Table_TL">
                                </th>
                                <th width="10">
                                    STT
                                </th>
                                <th>
                                    Dịch vụ
                                </th>
                                <th>
                                    Số điện thoại
                                </th>
                                <th>
                                    Loại giao dịch
                                </th>
                                <th>
                                    Kênh
                                </th>
                                <th>
                                    Thời gian
                                </th>
                                <th width="7%">
                                    Ứng dụng
                                </th>
                                <th width="7%">
                                    UserName
                                </th>
                                <th width="7%">
                                    UserIP
                                </th>
                                <th class="Table_TR">
                                </th>
                            </tr>
                            <asp:Repeater runat="server" ID="rpt_Data_2">
                                <ItemTemplate>
                                    <tr class="Table_Row_1">
                                        <td class="Table_ML_1"></td>
                                        <td>
                                            <%#(Container.ItemIndex + PageIndex).ToString()%>
                                        </td>
                                        <td>
                                            <%#Eval("ServiceName")%>
                                        </td>
                                        <td>
                                            <%#Eval("MSISDN")%>
                                        </td>
                                        <td>
                                            <%#Eval("ActionName")%>
                                        </td>
                                        <td>
                                            <%#Eval("ChannelTypeName")%>
                                        </td>
                                        <td>
                                            <%#Eval("ChargeDate") == DBNull.Value ? string.Empty : ((DateTime)Eval("ChargeDate")).ToString(MyUtility.MyConfig.LongDateFormat)%>
                                        </td>
                                        <td>
                                            <%#Eval("AppName")%>
                                        </td>
                                        <td>
                                            <%#Eval("UserName")%>
                                        </td>
                                        <td>
                                            <%#Eval("IP")%>
                                        </td>
                                        <td class="Table_MR_1"></td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="Table_Row_2">
                                        <td class="Table_ML_2"></td>
                                        <td>
                                            <%#(Container.ItemIndex + PageIndex).ToString()%>
                                        </td>
                                        <td>
                                            <%#Eval("ServiceName")%>
                                        </td>
                                        <td>
                                            <%#Eval("MSISDN")%>
                                        </td>
                                        <td>
                                            <%#Eval("ActionName")%>
                                        </td>
                                        <td>
                                            <%#Eval("ChannelTypeName")%>
                                        </td>
                                        <td>
                                            <%#Eval("ChargeDate") == DBNull.Value ? string.Empty : ((DateTime)Eval("ChargeDate")).ToString(MyUtility.MyConfig.LongDateFormat)%>
                                        </td>
                                        <td>
                                            <%#Eval("AppName")%>
                                        </td>
                                        <td>
                                            <%#Eval("UserName")%>
                                        </td>
                                        <td>
                                            <%#Eval("IP")%>
                                        </td>
                                        <td class="Table_MR_2"></td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
                <div runat="server" id="div_3">
                    <table class="Data" border="0" cellpadding="0" cellspacing="0">
                        <tbody>
                            <tr class="Table_Header">
                                <th class="Table_TL">
                                </th>
                                <th width="10">
                                    STT
                                </th>
                                <th>
                                    Dịch vụ
                                </th>
                                <th>
                                    Số điện thoại
                                </th>
                                <th>
                                    Giá
                                </th>
                                <th>
                                    Hình thức
                                </th>
                                <th>
                                    Tình trạng
                                </th>
                                <th>
                                    Kênh
                                </th>
                                <th>
                                    Thời gian
                                </th>
                                <th class="Table_TR">
                                </th>
                            </tr>
                            <asp:Repeater runat="server" ID="rpt_Data_3">
                                <ItemTemplate>
                                    <tr class="Table_Row_1">
                                        <td class="Table_ML_1"></td>
                                        <td>
                                            <%#(Container.ItemIndex + PageIndex).ToString()%>
                                        </td>
                                        <td>
                                            <%#Eval("ServiceName")%>
                                        </td>
                                        <td>
                                            <%#Eval("MSISDN")%>
                                        </td>
                                        <td>
                                            <%#Eval("Price")%>
                                        </td>
                                        <td>
                                            <%#Eval("ChargeTypeName")%>
                                        </td>
                                        <td>
                                            <%#Eval("ChargeStatusName")%>
                                        </td>
                                        <td>
                                            <%#Eval("ChannelTypeName")%>
                                        </td>
                                        <td>
                                            <%#Eval("ChargeDate") == DBNull.Value ? string.Empty : ((DateTime)Eval("ChargeDate")).ToString(MyUtility.MyConfig.LongDateFormat)%>
                                        </td>
                                        <td class="Table_MR_1"></td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="Table_Row_2">
                                        <td class="Table_ML_2"></td>
                                        <td>
                                            <%#(Container.ItemIndex + PageIndex).ToString()%>
                                        </td>
                                        <td>
                                            <%#Eval("ServiceName")%>
                                        </td>
                                        <td>
                                            <%#Eval("MSISDN")%>
                                        </td>
                                        <td>
                                            <%#Eval("Price")%>
                                        </td>
                                        <td>
                                            <%#Eval("ChargeTypeName")%>
                                        </td>
                                        <td>
                                            <%#Eval("ChargeStatusName")%>
                                        </td>
                                        <td>
                                            <%#Eval("ChannelTypeName")%>
                                        </td>
                                        <td>
                                            <%#Eval("ChargeDate") == DBNull.Value ? string.Empty : ((DateTime)Eval("ChargeDate")).ToString(MyUtility.MyConfig.LongDateFormat)%>
                                        </td>
                                        <td class="Table_MR_2"></td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
                 <div runat="server" id="div_4">
                    <table class="Data" border="0" cellpadding="0" cellspacing="0">
                        <tbody>
                            <tr class="Table_Header">
                                <th class="Table_TL">
                                </th>
                                <th width="10">
                                    STT
                                </th>
                                <th>
                                    Dịch vụ
                                </th>
                                <th>
                                    Số điện thoại
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
                                <th class="Table_TR">
                                </th>
                            </tr>
                            <asp:Repeater runat="server" ID="rpt_Data_4">
                                <ItemTemplate>
                                    <tr class="Table_Row_1">
                                        <td class="Table_ML_1"></td>
                                        <td>
                                            <%#(Container.ItemIndex + PageIndex).ToString()%>
                                        </td>
                                        <td>
                                            <%#Eval("ServiceName")%>
                                        </td>
                                        <td>
                                            <%#Eval("MSISDN")%>
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
                                        <td class="Table_MR_1"></td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="Table_Row_2">
                                        <td class="Table_ML_2"></td>
                                         <td>
                                            <%#(Container.ItemIndex + PageIndex).ToString()%>
                                        </td>
                                        <td>
                                            <%#Eval("ServiceName")%>
                                        </td>
                                        <td>
                                            <%#Eval("MSISDN")%>
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
                                        <td class="Table_MR_2"></td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
                <div class="Table_Footer">
                    <div class="Table_BL">
                        <uc1:Admin_Paging ID="Admin_Paging1" runat="server" />
                        <uc1:Admin_Paging ID="Admin_Paging2" runat="server" />
                        <uc1:Admin_Paging ID="Admin_Paging3" runat="server" />
                        <uc1:Admin_Paging ID="Admin_Paging4" runat="server" />
                    </div>
                    <div class="Table_BR">
                    </div>
                </div>
            </div>
        </fieldset>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cph_Javascript" runat="server">
</asp:Content>
