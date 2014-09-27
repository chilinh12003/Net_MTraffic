<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Ad_KPI.aspx.cs" Inherits="MyAdmin.Admin_Report.Ad_KPI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Tools" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_ToolBox" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_Search" runat="server">
    <label>
        Tháng:</label>
    <select runat="server" id="sel_Month">
    </select>
    <label>
        Loại KPI
    </label>
    <select runat="server" id="sel_KPIType">
    </select>
    <asp:Button runat="server" ID="btn_Execute" OnClick="btn_Execute_Click" Text="Thống kê" />
    <div runat="server" id="div_MO" style="font-size: 12px;" visible="false">
        <fieldset style="width: 300px; margin-right: 30px;float: left; margin-top: 30px;">
            <legend>Tỷ lệ xử lý MO</legend>
            <p>
                <label>
                    Tỷ lệ:</label><label class="DanhDau">100%</label>
            </p>
            <p>
                <label>
                    Tổng MO:</label><label class="DanhDau"><%=Total.ToString(MyUtility.MyConfig.IntFormat)%></label>
            </p>
            <p>
                <label>
                    Tổng MO đã xử lý:</label><label class="DanhDau"><%=Total.ToString(MyUtility.MyConfig.IntFormat)%></label>
            </p>
        </fieldset>
        <fieldset style="width: 300px; margin-right: 30px;float: left; margin-top: 30px;">
            <legend>Tỷ lệ MO thành công</legend>
            <p>
                <label>
                    Tỷ lệ:</label><label class="DanhDau"><%=percent.ToString(MyUtility.MyConfig.DoubleFormat)%>%</label>
            </p>
            <p>
                <label>
                    Tổng MO:</label><label class="DanhDau"><%=Total.ToString(MyUtility.MyConfig.IntFormat)%></label>
            </p>
            <p>
                <label>
                    Tổng MO thành công:</label><label class="DanhDau"><%=TotalSuccess.ToString(MyUtility.MyConfig.IntFormat)%></label>
            </p>
        </fieldset>
    </div>
    <div runat="server" id="div_MT" style="font-size: 12px;" visible="false">
        <fieldset style="width: 300px; margin-right: 30px;float: left; margin-top: 30px;">
            <legend>Tỷ lệ MT thành công</legend>
            <p>
                <label>
                    Tỷ lệ:</label><label class="DanhDau"><%=percent.ToString(MyUtility.MyConfig.DoubleFormat)%>%</label>
            </p>
            <p>
                <label>
                    Tổng MT:</label><label class="DanhDau"><%=Total.ToString(MyUtility.MyConfig.IntFormat)%></label>
            </p>
            <p>
                <label>
                    Tổng MT thành công:</label><label class="DanhDau"><%=TotalSuccess.ToString(MyUtility.MyConfig.IntFormat)%></label>
            </p>
        </fieldset>
    </div>
    <div runat="server" id="div_Charge" style="font-size: 12px;" visible="false">
        <fieldset style="width: 300px; margin-right: 30px;float: left; margin-top: 30px;">
            <legend>Tỷ lệ Charge Tổng</legend>
            <p>
                <label>
                    Tỷ lệ:</label><label class="DanhDau"><%=Percent_Charge.ToString(MyUtility.MyConfig.DoubleFormat)%>%</label>
            </p>
            <p>
                <label>
                    Tổng Charge:</label><label class="DanhDau"><%=TotalCharge.ToString(MyUtility.MyConfig.IntFormat)%></label>
            </p>
            <p>
                <label>
                    Tổng Charge thành công:</label><label class="DanhDau"><%=TotalCharge_Success.ToString(MyUtility.MyConfig.IntFormat)%></label>
            </p>
        </fieldset>
        <fieldset style="width: 300px; margin-right: 30px;float: left; margin-top: 30px;">
            <legend>Tỷ lệ Charge Đăng ký</legend>
            <p>
                <label>
                    Tỷ lệ:</label><label class="DanhDau"><%=Percent_Charge_Reg.ToString(MyUtility.MyConfig.DoubleFormat)%>%</label>
            </p>
            <p>
                <label>
                    Tổng Charge:</label><label class="DanhDau"><%=TotalCharge_Reg.ToString(MyUtility.MyConfig.IntFormat)%></label>
            </p>
            <p>
                <label>
                    Tổng Charge thành công:</label><label class="DanhDau"><%=TotalCharge_Reg_Success.ToString(MyUtility.MyConfig.IntFormat)%></label>
            </p>
        </fieldset>
        <fieldset style="width: 300px; margin-right: 30px;float: left; margin-top: 30px;">
            <legend>Tỷ lệ Charge Renew</legend>
            <p>
                <label>
                    Tỷ lệ:</label><label class="DanhDau"><%=Percent_Charge_Renew.ToString(MyUtility.MyConfig.DoubleFormat)%>%</label>
            </p>
            <p>
                <label>
                    Tổng Charge:</label><label class="DanhDau"><%=TotalCharge_Renew.ToString(MyUtility.MyConfig.IntFormat)%></label>
            </p>
            <p>
                <label>
                    Tổng Charge thành công:</label><label class="DanhDau"><%=TotalCharge_Renew_Success.ToString(MyUtility.MyConfig.IntFormat)%></label>
            </p>
        </fieldset>
         <fieldset style="width: 300px; margin-right: 30px;float: left; margin-top: 30px;">
            <legend>Tỷ lệ Charge Hủy (đồng bộ)</legend>
            <p>
                <label>
                    Tỷ lệ:</label><label class="DanhDau"><%=Percent_Charge_UnReg.ToString(MyUtility.MyConfig.DoubleFormat)%>%</label>
            </p>
            <p>
                <label>
                    Tổng Charge:</label><label class="DanhDau"><%=TotalCharge_UnReg.ToString(MyUtility.MyConfig.IntFormat)%></label>
            </p>
            <p>
                <label>
                    Tổng Charge thành công:</label><label class="DanhDau"><%=TotalCharge_UnReg_Success.ToString(MyUtility.MyConfig.IntFormat)%></label>
            </p>
        </fieldset>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph_Content" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cph_Javascript" runat="server">
</asp:Content>
