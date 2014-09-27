<%@ Page Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true"
    CodeBehind="Ad_Permission.aspx.cs" Inherits="Administrator.Admin.Ad_Permission" %>

<%@ Register Src="../Admin_Control/Admin_Paging.ascx" TagName="Admin_Paging" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Tools" runat="server">
    <asp:LinkButton runat="server" ID="lbtn_Delete" OnClientClick="return BeforeDeteleData();"
        ToolTip="Xóa tất cả mục đã chọn" OnClick="lbtn_Delete_Click">
        <span class="Delete"></span>
            Xóa
    </asp:LinkButton>
    <a href="Ad_Permission_Edit.aspx" runat="server" id="link_Add"><span class="Add"></span>
        Thêm </a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_ToolBox" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_Search" runat="server">
    <label>
        Từ khóa:</label>
    <input type="text" runat="server" id="tbx_Search" />
    <select runat="server" id="sel_MemberGroup">
    </select>
    <asp:Button runat="server" ID="btn_Search" Text="Tìm kiếm" OnClick="btn_Search_Click" />
    <asp:Button runat="server" ID="btn_CancelSearch" Text="Hủy tìm" OnClick="btn_CancelSearch_Click"
        Visible="false" />
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
                <th>
                    Mã
                </th>
                <th>
                    Tên nhóm
                </th>
                <th>
                    <asp:LinkButton runat="server" CssClass="Sort" ID="lbtn_Sort_1" CommandArgument="MenuName ASC"
                        OnClick="lbtn_Sort_Click">Tên trang</asp:LinkButton>
                </th>
                <th>
                    <asp:LinkButton runat="server" CssClass="Sort" ID="lbtn_Sort_2" CommandArgument="ViewRole ASC"
                        OnClick="lbtn_Sort_Click">Xem</asp:LinkButton>
                </th>
                <th>
                    <asp:LinkButton runat="server" CssClass="Sort" ID="lbtn_Sort_3" CommandArgument="AddRole ASC"
                        OnClick="lbtn_Sort_Click">Thêm</asp:LinkButton>
                </th>
                <th>
                    <asp:LinkButton runat="server" CssClass="Sort" ID="lbtn_Sort_4" CommandArgument="DeleteRole ASC"
                        OnClick="lbtn_Sort_Click">Xóa</asp:LinkButton>
                </th>
                <th>
                    <asp:LinkButton runat="server" CssClass="Sort" ID="lbtn_Sort_5" CommandArgument="EditRole ASC"
                        OnClick="lbtn_Sort_Click">Sửa</asp:LinkButton>
                </th>
                <th>
                    <asp:LinkButton runat="server" CssClass="Sort" ID="lbtn_Sort_6" CommandArgument="ActiveRole ASC"
                        OnClick="lbtn_Sort_Click">K.Hoạt</asp:LinkButton>
                </th>
                <th>
                     <asp:LinkButton runat="server" CssClass="Sort" ID="lbtn_Sort_7" CommandArgument="InActiveRole ASC"
                        OnClick="lbtn_Sort_Click">Hủy.KH</asp:LinkButton>
                </th>
                <th>
                     <asp:LinkButton runat="server" CssClass="Sort" ID="lbtn_Sort_8" CommandArgument="PublishRole ASC"
                        OnClick="lbtn_Sort_Click">X.Bản</asp:LinkButton>
                </th>
                <th>
                     <asp:LinkButton runat="server" CssClass="Sort" ID="lbtn_Sort_9" CommandArgument="UnPublishRole ASC"
                        OnClick="lbtn_Sort_Click">Hủy.XB</asp:LinkButton>
                </th>
                <th>
                    Export
                </th>
                <th>
                    Import
                </th>
                <th align="center" width="10">
                    <input type="checkbox" onclick="SelectCheckBox_All(this);" />
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
                        <td>
                            <%#Eval("MemberGroupID") %>
                        </td>
                        <td>
                            <%#Eval("MemberGroupName") %>
                        </td>
                        <td>
                            <%#Eval("MenuName") %>
                        </td>
                        <td>
                            <input type="checkbox" onclick="return false;" <%# (bool)Eval("ViewRole")?" checked='checked'":"" %> />
                        </td>
                        <td>
                            <input type="checkbox" onclick="return false;" <%# (bool)Eval("AddRole")?" checked='checked'":"" %> />
                        </td>
                        <td>
                            <input type="checkbox" onclick="return false;" <%# (bool)Eval("DeleteRole")?" checked='checked'":"" %> />
                        </td>
                        <td>
                            <input type="checkbox" onclick="return false;" <%# (bool)Eval("EditRole")?" checked='checked'":"" %> />
                        </td>
                        <td>
                            <input type="checkbox" onclick="return false;" <%# (bool)Eval("ActiveRole")?" checked='checked'":"" %> />
                        </td>
                        <td>
                            <input type="checkbox" onclick="return false;" <%# (bool)Eval("InActiveRole")?" checked='checked'":"" %> />
                        </td>
                        <td>
                            <input type="checkbox" onclick="return false;" <%# (bool)Eval("PublishRole")?" checked='checked'":"" %> />
                        </td>
                        <td>
                            <input type="checkbox" onclick="return false;" <%# (bool)Eval("UnPublishRole")?" checked='checked'":"" %> />
                        </td>
                        <td>
                            <input type="checkbox" onclick="return false;" <%# (bool)Eval("ExportRole")?" checked='checked'":"" %> />
                        </td>
                        <td>
                            <input type="checkbox" onclick="return false;" <%# (bool)Eval("ImportRole")?" checked='checked'":"" %> />
                        </td>
                        <td align="center" width="10">
                            <%#"<input type='checkbox' id='CheckAll_" + Container.ItemIndex.ToString() + "' value='" + Eval("RoleID").ToString()+ "' />"%>
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
                            <%#Eval("MenuName")%>
                        </td>
                        <td>
                            <input type="checkbox" onclick="return false;" <%# (bool)Eval("ViewRole")?" checked='checked'":"" %> />
                        </td>
                        <td>
                            <input type="checkbox" onclick="return false;" <%# (bool)Eval("AddRole")?" checked='checked'":"" %> />
                        </td>
                        <td>
                            <input type="checkbox" onclick="return false;" <%# (bool)Eval("DeleteRole")?" checked='checked'":"" %> />
                        </td>
                        <td>
                            <input type="checkbox" onclick="return false;" <%# (bool)Eval("EditRole")?" checked='checked'":"" %> />
                        </td>
                        <td>
                            <input type="checkbox" onclick="return false;" <%# (bool)Eval("ActiveRole")?" checked='checked'":"" %> />
                        </td>
                        <td>
                            <input type="checkbox" onclick="return false;" <%# (bool)Eval("InActiveRole")?" checked='checked'":"" %> />
                        </td>
                        <td>
                            <input type="checkbox" onclick="return false;" <%# (bool)Eval("PublishRole")?" checked='checked'":"" %> />
                        </td>
                        <td>
                            <input type="checkbox" onclick="return false;" <%# (bool)Eval("UnPublishRole")?" checked='checked'":"" %> />
                        </td>
                        <td>
                            <input type="checkbox" onclick="return false;" <%# (bool)Eval("ExportRole")?" checked='checked'":"" %> />
                        </td>
                        <td>
                            <input type="checkbox" onclick="return false;" <%# (bool)Eval("ImportRole")?" checked='checked'":"" %> />
                        </td>
                        <td align="center" width="10">
                            <%#"<input type='checkbox' id='CheckAll_" + Container.ItemIndex.ToString() + "' value='" + Eval("RoleID").ToString()+ "' />"%>
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

    <script language="javascript" type="text/javascript">
        hid_ListCheckAll = document.getElementById("<%=hid_ListCheckAll.ClientID %>");

        ReCheck_CheckboxOnGrid();

        function EditData()
        {
            if (BeforeEditData())
            {
                document.location = '../Admin/Ad_Permission_Edit.aspx?ID=' + hid_ListCheckAll.value;

                return true;
            }
            return false;
        }

        function Active()
        {
            if (GetAllCheck('Xin hãy chọn ít nhất một mục để kích hoạt'))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        function UnActive()
        {
            if (GetAllCheck('Xin hãy chọn ít nhất một mục để hủy kích hoạt'))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    </script>

</asp:Content>
