<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true"
    CodeBehind="Ad_MenuAdmin.aspx.cs" Inherits="MyAdmin.Admin.Ad_MenuAdmin" %>

<%@ Register Src="../Admin_Control/Admin_Paging.ascx" TagName="Admin_Paging" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Tools" runat="server">
    <a href="javascript:void(0);" onclick="return EditData();" runat="server" id="link_Edit">
        <span class="Edit"></span>Sửa </a>
    <asp:LinkButton runat="server" ID="lbtn_Delete" OnClientClick="return BeforeDeteleData();"
        ToolTip="Xóa tất cả mục đã chọn" OnClick="lbtn_Delete_Click">
        <span class="Delete"></span>
            Xóa
    </asp:LinkButton>
    <a href="Ad_MenuAdmin_Edit.aspx" runat="server" id="link_Add"><span class="Add"></span>
        Thêm </a>
    <asp:LinkButton runat="server" ID="lbtn_Active" OnClientClick="return Active();"
        ToolTip="Xóa tất cả mục đã chọn" OnClick="lbtn_Active_Click">
        <span class="Active"></span>
       K.Hoat
    </asp:LinkButton>
    <asp:LinkButton runat="server" ID="lbtn_UnActive" OnClick="lbtn_UnActive_Click" OnClientClick="return UnActive();">
        <span class="UnActive"></span>
       Hủy KH
    </asp:LinkButton>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_ToolBox" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_Search" runat="server">
    <label>
        Từ khóa:</label>
    <input type="text" runat="server" id="tbx_Search" />
    <asp:DropDownList runat="server" ID="ddl_MenuID_1" AutoPostBack="true" OnSelectedIndexChanged="ddl_MenuID_1_SelectedIndexChanged">
    </asp:DropDownList>
    <asp:DropDownList runat="server" ID="ddl_MenuID_2" AutoPostBack="true" OnSelectedIndexChanged="ddl_MenuID_2_SelectedIndexChanged">
    </asp:DropDownList>
    <select runat="server" id="sel_MenuID_3">
    </select>
    <input type="radio" runat="server" id="rad_Active" name="Active" /><label>K.hoạt</label>
    <input type="radio" runat="server" id="rad_UnActive" name="Active" /><label>Chưa.KH</label>
    <input type="radio" runat="server" id="rad_Both" name="Active" checked="true" /><label>Cả
        hai</label>
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
                    <asp:LinkButton runat="server" CssClass="Sort" ID="lbtn_Sort_1" CommandArgument="MenuID ASC"
                        OnClick="lbtn_Sort_Click">Mã Menu</asp:LinkButton>
                </th>
                <th>
                    <asp:LinkButton runat="server" CssClass="Sort" ID="lbtn_Sort_2" CommandArgument="MenuID_1 ASC"
                        OnClick="lbtn_Sort_Click">Mã Menu cấp 1</asp:LinkButton>
                </th>
                <th>
                    <asp:LinkButton runat="server" CssClass="Sort" ID="lbtn_Sort_3" CommandArgument="MenuID_2 ASC"
                        OnClick="lbtn_Sort_Click">Mã Menu cấp 2</asp:LinkButton>
                </th>
                <th>
                    <asp:LinkButton runat="server" CssClass="Sort" ID="lbtn_Sort_4" CommandArgument="MenuID_3 ASC"
                        OnClick="lbtn_Sort_Click">Mã Menu cấp 3</asp:LinkButton>
                </th>
                <th>
                    <asp:LinkButton runat="server" CssClass="Sort" ID="lbtn_Sort_5" CommandArgument="MenuName ASC"
                        OnClick="lbtn_Sort_Click">Tên Menu</asp:LinkButton>
                </th>
                <th>
                    <asp:LinkButton runat="server" CssClass="Sort" ID="lbtn_Sort_6" CommandArgument="PageCode ASC"
                        OnClick="lbtn_Sort_Click">Mã Menu</asp:LinkButton>
                </th>
                <th>
                    Link
                </th>
                <th>
                    Ưu tiên
                </th>
                <th>
                    Có Menu con
                </th>
                <th width="10">
                    Active
                </th>
                <th width="10">
                    Ảnh
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
                            <%#Eval("MenuID") %>
                        </td>
                        <td>
                            <%#Eval("MenuID_1")%>
                        </td>
                        <td>
                            <%#Eval("MenuID_2")%>
                        </td>
                        <td>
                            <%#Eval("MenuID_3")%>
                        </td>
                        <td>
                            <a href="Ad_MenuAdmin_Edit.aspx?ID=<%#Eval("MenuID") %>">
                                <%#Eval("MenuName")%></a>
                        </td>
                        <td>
                            <%#Eval("PageCode")%>
                        </td>
                        <td>
                            <a href='<%#Eval("Link") %>'>
                                <%#Eval("Link") %></a>
                        </td>
                        <td>
                            <%#Eval("Priority") %>
                        </td>
                        <td>
                            <%#Eval("IsHasChild")%>
                        </td>
                        <td>
                            <img src="<%#(((bool)Eval("IsActive"))?"../Images/Buttons/Active_Grid.png":"../Images/Buttons/UnActive_Grid.png") %>" />
                        </td>
                        <td align="center" width="10">
                            <img src="../Images/Buttons/View_en.png" onclick="ShowLoading(this,false,false); Show_FaceBoxImage(<%# "'"+ Eval("ImagePath").ToString() +"'"%>);" />
                        </td>
                        <td align="center" width="10">
                            <%#"<input type='checkbox' id='CheckAll_" + Container.ItemIndex.ToString() + "' value='" + Eval("MenuID").ToString() + "' />"%>
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
                            <%#Eval("MenuID") %>
                        </td>
                        <td>
                            <%#Eval("MenuID_1")%>
                        </td>
                        <td>
                            <%#Eval("MenuID_2")%>
                        </td>
                        <td>
                            <%#Eval("MenuID_3")%>
                        </td>
                        <td>
                            <a href="Ad_MenuAdmin_Edit.aspx?ID=<%#Eval("MenuID") %>">
                                <%#Eval("MenuName")%></a>
                        </td>
                        <td>
                            <%#Eval("PageCode")%>
                        </td>
                        <td>
                            <a href='<%#Eval("Link") %>'>
                                <%#Eval("Link") %></a>
                        </td>
                        <td>
                            <%#Eval("Priority") %>
                        </td>
                        <td>
                            <%#Eval("IsHasChild")%>
                        </td>
                        <td>
                            <img src="<%#(((bool)Eval("IsActive"))?"../Images/Buttons/Active_Grid.png":"../Images/Buttons/UnActive_Grid.png") %>" />
                        </td>
                        <td align="center" width="10">
                            <img src="../Images/Buttons/View_en.png" onclick="ShowLoading(this,false,false); Show_FaceBoxImage(<%# "'"+ Eval("ImagePath").ToString() +"'"%>);" />
                        </td>
                        <td align="center" width="10">
                            <%#"<input type='checkbox' id='CheckAll_" + Container.ItemIndex.ToString() + "' value='" + Eval("MenuID").ToString() + "' />"%>
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
            <uc1:Admin_Paging ID="Admin_Paging1" runat="server" ShowPageSize="true" />
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
                document.location = '../Admin/Ad_MenuAdmin_Edit.aspx?ID=' + hid_ListCheckAll.value;

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
