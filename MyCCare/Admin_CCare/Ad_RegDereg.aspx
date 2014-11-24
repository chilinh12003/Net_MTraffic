<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Ad_RegDereg.aspx.cs" Inherits="MyCCare.Admin_CCare.Ad_RegDereg" %>

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
        <li><a href='Ad_RegDereg.aspx' class='select'>Đăng ký / Hủy / Reset</a></li>
        <li><a href='Ad_ResendMT.aspx'>Bù nội dung</a></li>
            </ul>
    </div>
    <div class='fillterarea'>
        <table cellspacing="5" cellpadding="5">
            <tbody>
                <tr>
                    <td width="100">Số thuê bao:</td>
                    <td colspan="5">
                        <input style='width: 147px' type='text' class='textbox' runat="server" id="tbx_MSISDN" /></td>                  

                    <td width="200" align="right" rowspan="2">
                        <asp:Button CssClass="btn_search" runat="server" ID="btn_Search" Text="Tra cứu" OnClick="btn_Search_Click"/></td>
                </tr>
            </tbody>
        </table>
    </div>
    <table class="tbl_style center">
        <tbody>
            <tr>
                <th>Dịch vụ</th>
                <th>Gói cước</th>
                <th colspan='2'>Thao tác</th>
            </tr>
            <asp:Repeater runat="server" ID="rpt_Data_Sub" OnItemDataBound="rpt_Data_Sub_ItemDataBound">
                <ItemTemplate>
                     <tr>
                        <td>MTraffic</td>
                        <td><%#Eval("ServiceName") %></td>
                        <td width='100px'>
                            <asp:LinkButton runat="server" CssClass="btnintbl" CommandArgument='<%#Eval("ServiceID")%>'  ID="lbtn_Dereg"  Text="Hủy" OnClick="tbx_Dereg_Click" OnClientClick='<%# "return ConfirmDereg(\""+Eval("MSISDN")+"\",\""+Eval("ServiceName")+"\");"%>'><span class='iconhuy'>Hủy</span></asp:LinkButton>
                        </td>
                        <td width='100px'>
                            <asp:LinkButton runat="server" CssClass="btnintbl" CommandArgument='<%#Eval("ServiceID")%>' ID="lbtn_Reset" Text="Reset"><span class='iconreset'>Reset</span></asp:LinkButton>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Repeater runat="server" ID="rpt_Data_UnSub" OnItemDataBound="rpt_Data_UnSub_ItemDataBound">
                <ItemTemplate>
                    <tr>
                         <td>MTraffic</td>
                        <td><%#Eval("ServiceName") %></td>
                        <td colspan='2'>
                            <asp:LinkButton runat="server"  CssClass="btnintbl" CommandArgument='<%#Eval("ServiceID")%>' ID="lbtn_Reg" Text="Đăng ký" OnClick="tbx_Reg_Click" OnClientClick='<%# "return ConfirmReg(\""+Eval("MSISDN")+"\",\""+Eval("ServiceName")+"\");"%>'><span class='icondk'>Đăng ký</span></asp:LinkButton>

                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>

    <uc1:Admin_Paging ID="Admin_Paging1" runat="server" ShowPageSize="false" />
    <script language="javascript" type="text/javascript">
        function ConfirmDereg(MSISDN, ServiceName) {
            debugger;
            return confirm("Bạn có chắc muốn Hủy đăng ký dịch vụ ("+ServiceName+") cho số điện thoại ("+MSISDN+")?");
        }
        function ConfirmReg(MSISDN, ServiceName) {
            debugger;
            return confirm("Bạn có chắc muốn Đăng ký dịch vụ (" + ServiceName + ") cho số điện thoại (" + MSISDN + ")?");
        }
    </script>
</asp:Content>
