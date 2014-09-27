<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Ad_ResendMT.aspx.cs" Inherits="MyAdmin.Admin_CCare.Ad_ResendMT" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Tools" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_ToolBox" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_Search" runat="server">
    <div class="NewLine_Pad">
        <label style="width:100px;">
            Số điện thoại:</label>
        <input type="text" runat="server" id="tbx_MSISDN" />
    </div>
   <div class="NewLine_Pad">
        <label style="width:100px;">
            Dịch vụ:
        </label>
        <select runat="server" id="sel_Service">
        </select>
    </div>
    <div class="NewLine_Pad">
        <label style="width:100px;">
            Nội dung MT:</label>
        <textarea runat="server" id="tbx_MT" style="height: 50px; width: 90%;"></textarea>
    </div>
    <div class="NewLine_Pad">
    <label style="width:100px;"></label>
    <asp:Button runat="server" ID="btn_SendMT" Text="Gửi MT" OnClick="btn_SendMT_Click" />
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph_Content" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cph_Javascript" runat="server">
</asp:Content>
