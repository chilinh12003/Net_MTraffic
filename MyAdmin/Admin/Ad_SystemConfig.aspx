<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true"
    CodeBehind="Ad_SystemConfig.aspx.cs" Inherits="MyAdmin.Admin.Ad_SystemConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Tools" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_ToolBox" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_Search" runat="server">
    <input type="checkbox" runat="server" id="chk_ShowMessage" onchange="SetCookieShowMessage(this.checked);" />
    <label for="<%= chk_ShowMessage.ClientID %>">
        Cho phép hiển thị thông báo</label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph_Content" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cph_Javascript" runat="server">

    <script language="javascript" type="text/javascript">

        SetCheckBoxShowMessageFromCookie("<%= chk_ShowMessage.ClientID %>");
        
    </script>

</asp:Content>
