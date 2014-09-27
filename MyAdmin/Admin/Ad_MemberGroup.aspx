<%@ Page Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true"
    CodeBehind="Ad_MemberGroup.aspx.cs" Inherits="MyAdmin.Admin.Ad_MemberGroup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Tools" runat="server">
    <a href="javascript:void(0);" onclick="return EditData();" runat="server" id="link_Edit">
        <span class="Edit"></span>
        Sửa </a>
    <asp:LinkButton runat="server" ID="lbtn_Delete" OnClientClick="return BeforeDeteleData();"
        ToolTip="Xóa tất cả mục đã chọn" OnClick="lbtn_Delete_Click">
        <span class="Delete"></span>
        <label>
            Xóa</label>
    </asp:LinkButton>
    <a href="Ad_MemberGroup_Edit.aspx" runat="server" id="link_Add">
        <span class="Add"></span>
        Thêm </a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_ToolBox" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_Search" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph_Content" runat="server">
    <table class="Data" border="0" cellpadding="0" cellspacing="0">
        <tbody>
            <tr class="Table_Header">
                <th class="Table_TL"></th>
                <th width="10">STT </th>
                <th>Mã</th>
                <th>Tên nhóm</th>
                <th>Mã đối tác</th>
                <th>Tên đối tác</th>
                <th align="center" width="10">
                    <input type="checkbox" onclick="SelectCheckBox_All(this);" /></th>
                <th class="Table_TR"></th>
            </tr>
            <asp:Repeater runat="server" ID="rpt_Data">
                <ItemTemplate>
                    <tr class="Table_Row_1">
                        <td class="Table_ML_1">
                        </td>
                        <td>
                            <%#(Container.ItemIndex + PageIndex).ToString()%>
                        </td>
                        <td>
                            <%#Eval("MemberGroupID") %>
                        </td>
                        <td>
                            <%#Eval("MemberGroupName") %>
                        </td>
                        <td>
                            <%#Eval("PartnerID") %>
                        </td>
                         <td>
                            <%#Eval("PartnerName") %>
                        </td>
                        <td align="center" width="10">
                            <%#"<input type='checkbox' id='CheckAll_" + Container.ItemIndex.ToString() + "' value='" + Eval("MemberGroupID").ToString() + "' />"%>
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
                            <%#Eval("MemberGroupID") %>
                        </td>
                        <td>
                            <%#Eval("MemberGroupName") %>
                        </td>
                        <td>
                            <%#Eval("PartnerID") %>
                        </td>
                          <td>
                            <%#Eval("PartnerName") %>
                        </td>
                        <td align="center" width="10">
                            <%#"<input type='checkbox' id='CheckAll_" + Container.ItemIndex.ToString() + "' value='" + Eval("MemberGroupID").ToString() + "' />"%>
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
    <div class="Div_Hidden">
        <input type="hidden" runat="server" id="hid_ListCheckAll" />
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cph_Javascript" runat="server">

    <script language="javascript" type="text/javascript">
        hid_ListCheckAll = document.getElementById("<%=hid_ListCheckAll.ClientID %>");

        ReCheck_CheckboxOnGrid();

        function EditData() {
            if (BeforeEditData()) {
                document.location = '../Admin/Ad_MemberGroup_Edit.aspx?ID=' + hid_ListCheckAll.value;
                return true;
            }
            return false;
        }
    </script>

</asp:Content>
