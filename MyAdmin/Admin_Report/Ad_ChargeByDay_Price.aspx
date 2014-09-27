<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Ad_ChargeByDay_Price.aspx.cs" Inherits="MyAdmin.Admin_Report.Ad_ChargeByDay_Price" %>

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
     <select runat="server" id="sel_Service">
        <option value="0">- - Tìm theo tất cả - - </option>
    </select>
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
                <th class="Table_TL">
                </th>
                <th width="10">
                    STT
                </th>
                <th>Dịch vụ</th>
                <th>
                    Ngày
                </th>
                <th>
                    Giá
                </th>
                <th>
                    Số lượng
                </th>
                <th>
                    Tổng tiền
                </th>
                <th class="Table_TR">
                </th>
            </tr>
            <asp:Repeater runat="server" ID="rpt_Data">
                <ItemTemplate>
                    <tr class="Table_Row_1">
                        <td class="Table_ML_1">
                        </td>
                        
                        <td>
                            <%#(Container.ItemIndex + PageIndex).ToString()%>
                        </td>     
                        <td><%#Eval("ServiceName") %></td>                  
                        <td>
                            <%#Eval("ChargeDate") == DBNull.Value ? string.Empty : ((DateTime)Eval("ChargeDate")).ToString(MyUtility.MyConfig.ShortDateFormat)%>
                        </td>
                       
                        <td>
                            <%# int.Parse( Eval("Price").ToString()).ToString(MyUtility.MyConfig.IntFormat)%>
                        </td>
                        <td>
                            <%# int.Parse( Eval("TotalCount").ToString()).ToString(MyUtility.MyConfig.IntFormat)%>
                        </td>
                         <td>
                            <%# int.Parse( Eval("Totalprice").ToString()).ToString(MyUtility.MyConfig.IntFormat)%>
                        </td>           
                        <td class="Table_MR_1">
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="Table_Row_2">
                        <td class="Table_ML_2">
                        </td>
                          <td>
                            <%#(Container.ItemIndex + PageIndex).ToString()%>
                        </td>       
                        <td><%#Eval("ServiceName") %></td>                
                        <td>
                            <%#Eval("ChargeDate") == DBNull.Value ? string.Empty : ((DateTime)Eval("ChargeDate")).ToString(MyUtility.MyConfig.ShortDateFormat)%>
                        </td>
                       
                        <td>
                            <%# int.Parse( Eval("Price").ToString()).ToString(MyUtility.MyConfig.IntFormat)%>
                        </td>
                        <td>
                            <%# int.Parse( Eval("TotalCount").ToString()).ToString(MyUtility.MyConfig.IntFormat)%>
                        </td>
                         <td>
                            <%# int.Parse( Eval("Totalprice").ToString()).ToString(MyUtility.MyConfig.IntFormat)%>
                        </td>          
                        <td class="Table_MR_2">
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
    <div class="Table_Footer">
        <div class="Table_BL">
            <uc1:Admin_Paging ID="Admin_Paging1" runat="server" />
        </div>
        <div class="Table_BR">
        </div>
    </div>
    <div class="Div_Hidden">
        <input type="hidden" runat="server" id="hid_ListCheckAll" />
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cph_Javascript" runat="server">
</asp:Content>

