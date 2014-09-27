<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Ad_Register.aspx.cs" Inherits="MyAdmin.Admin_CCare.Ad_Register" %>

<%@ Register Src="../Admin_Control/Admin_Paging.ascx" TagName="Admin_Paging" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Header" runat="server">
    <link href="../CSS/CheckInfo.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Tools" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_ToolBox" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_Search" runat="server">
    <label>
        Số điện thoại</label>
    <input type="text" runat="server" id="tbx_MSISDN" />
    <asp:Button runat="server" ID="btn_Execute" OnClick="btn_Execute_Click" Text="Thực hiện" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph_Content" runat="server">
    <div class="register">
        <div class="left">
            <fieldset>
                <legend>Dịch vụ đã đăng ký</legend>
                <div class="NoiDung">
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
                                    Cấu trúc hủy
                                </th>
                                <th>
                                    Ngày đăng ký
                                </th>
                                <th>
                                    Ngày hết hạn
                                </th>
                                <th>
                                    Hủy
                                </th>
                                <th class="Table_TR">
                                </th>
                            </tr>
                            <asp:Repeater runat="server" ID="rpt_Data_Reg">
                                <ItemTemplate>
                                    <tr class="Table_Row_1">
                                        <td class="Table_ML_1">
                                        </td>
                                        <td>
                                            <%#(Container.ItemIndex + PageIndex).ToString()%>
                                        </td>
                                        <td>
                                            <%#Eval("ServiceName")%>
                                        </td>
                                        <td>
                                            <%#Eval("DeregKeyword")%>
                                        </td>
                                        <td>
                                            <%#Eval("EffectiveDate") == DBNull.Value ? string.Empty : ((DateTime)Eval("EffectiveDate")).ToString(MyUtility.MyConfig.LongDateFormat)%>
                                        </td>
                                        <td>
                                            <%#Eval("ExpiryDate") == DBNull.Value ? string.Empty : ((DateTime)Eval("ExpiryDate")).ToString(MyUtility.MyConfig.LongDateFormat)%>
                                        </td>
                                        <td>
                                        
                                            <asp:Button runat="server" ID="tbx_Dereg" Text="Hủy" CommandArgument='<%#Eval("ServiceID")%>' OnClick="tbx_Dereg_Click" OnClientClick='<%# "return ConfirmDereg(\""+Eval("MSISDN")+"\",\""+Eval("ServiceName")+"\");"%>'/>
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
                                        <td>
                                            <%#Eval("ServiceName")%>
                                        </td>
                                        <td>
                                            <%#Eval("DeregKeyword")%>
                                        </td>
                                        <td>
                                            <%#Eval("EffectiveDate") == DBNull.Value ? string.Empty : ((DateTime)Eval("EffectiveDate")).ToString(MyUtility.MyConfig.LongDateFormat)%>
                                        </td>
                                        <td>
                                            <%#Eval("ExpiryDate") == DBNull.Value ? string.Empty : ((DateTime)Eval("ExpiryDate")).ToString(MyUtility.MyConfig.LongDateFormat)%>
                                        </td>
                                        <td>
                                            <asp:Button runat="server" ID="tbx_Dereg" Text="Hủy" CommandArgument='<%#Eval("ServiceID")%>' OnClick="tbx_Dereg_Click" OnClientClick='<%# "return ConfirmDereg(\""+Eval("MSISDN")+"\",\""+Eval("ServiceName")+"\");"%>'/>
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
                        </div>
                        <div class="Table_BR">
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>
        <div class="right">
            <fieldset>
                <legend>Dịch vụ chưa đăng ký</legend>
                <div class="NoiDung">
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
                                    Cấu trúc Đăng ký
                                </th>
                                <th>
                                    Đăng ký
                                </th>
                                <th class="Table_TR">
                                </th>
                            </tr>
                            <asp:Repeater runat="server" ID="rpt_Data_NotReg">
                                <ItemTemplate>
                                    <tr class="Table_Row_1">
                                        <td class="Table_ML_1">
                                        </td>
                                        <td>
                                            <%#(Container.ItemIndex + PageIndex).ToString()%>
                                        </td>
                                        <td>
                                            <%#Eval("ServiceName")%>
                                        </td>
                                        <td>
                                            <%#Eval("RegKeyword")%>
                                        </td>
                                        <td>
                                            <asp:Button runat="server" ID="tbx_Reg" Text="Đăng ký" CommandArgument='<%#Eval("ServiceID")%>' OnClick="tbx_Reg_Click" OnClientClick='<%# "return ConfirmReg(\""+Eval("MSISDN")+"\",\""+Eval("ServiceName")+"\");"%>'/>
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
                                        <td>
                                            <%#Eval("ServiceName")%>
                                        </td>
                                        <td>
                                            <%#Eval("RegKeyword")%>
                                        </td>
                                        <td>
                                            <asp:Button runat="server" ID="tbx_Reg" Text="Đăng ký" CommandArgument='<%#Eval("ServiceID")%>' OnClick="tbx_Reg_Click" OnClientClick='<%# "return ConfirmReg(\""+Eval("MSISDN")+"\",\""+Eval("ServiceName")+"\");"%>'/>
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
                        </div>
                        <div class="Table_BR">
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cph_Javascript" runat="server">
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
