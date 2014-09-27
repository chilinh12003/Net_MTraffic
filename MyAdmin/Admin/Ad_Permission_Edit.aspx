<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Ad_Permission_Edit.aspx.cs" Inherits="MyAdmin.Admin.Ad_Permission_Edit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cph_Header" runat="server">
<link href="../CSS/TreeView.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Tools" runat="server">
    <a href="Ad_Permission.aspx" runat="server" id="link_Cancel"><span class="Cancel"></span>
        Hủy </a>
    <asp:LinkButton runat="server" ID="lbtn_Save" OnClick="lbtn_Save_Click" OnClientClick="return BeforeSaveData();">
     <span class="Save"></span>
            Lưu
    </asp:LinkButton>
    <asp:LinkButton runat="server" ID="lbtn_Accept" OnClick="lbtn_Apply_Click" OnClientClick="return BeforeSaveData();">
     <span class="Accept"></span>
            Apply
    </asp:LinkButton>
    <a href="Ad_Permission_Edit.aspx" runat="server" id="link_Add"><span class="Add"></span>
        Thêm </a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_ToolBox" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_Search" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph_Content" runat="server">
    <div class="Edit_Left">
        <div class="Edit_Title">
            Nhóm thành viên:</div>
        <div class="Edit_Control">
             <asp:DropDownList runat="server" ID="ddl_MemberGroup" AutoPostBack="true" OnSelectedIndexChanged="ddl_MemberGroup_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
         <div class="Edit_Title">
            Trang chưa phân quyền:</div>
        <div class="Edit_Control" style="height: auto;">
            <div class="TreeView">
                <asp:Repeater runat="server" ID="rpt_MenuAdmin">
                    <ItemTemplate>
                        <div <%# (bool)Eval("IsHasChild")?" class='ParentNode'":"class='ChildNode'" %>>
                            <input type="checkbox" id='chk_<%#Eval("ParentOrder")+"_"+Eval("Order") %>' value='<%#Eval("PageCode") %>' onchange='AddToListPageCode(this,<%#Eval("IsHasChild").ToString().ToLower() %>); CheckChild(this,<%#Eval("Order") %>,<%#Eval("ChildCount") %>,<%#Eval("IsHasChild").ToString().ToLower() %>);' />
                            <label for='chk_<%#Eval("ParentOrder")+"_"+Eval("Order") %>'>
                                <%#Eval("MenuName") %></label>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <input type="hidden" runat="server" id="hid_ListPageCode" />
            </div>
        </div>
    </div>
    <div class="Edit_Right">
        <div class="Properties_Header">
            <div class="Properties_Header_In">
                Thông tin chi tiết khác
            </div>
        </div>
        <div class="Properties">
            <div class="Properties_Title" style="color:Red;" >
               Chọn tất cả:</div>
            <div class="Properties_Control">
                <input type="checkbox" runat="server" id="chk_All" onchange="CheckAll(this);" />
            </div>
           <div class="Properties_Title">
               Quyền Thêm:</div>
            <div class="Properties_Control">
                <input type="checkbox" runat="server" id="chk_AddRole" />
            </div>
           <div class="Properties_Title">
               Quyền Xóa:</div>
            <div class="Properties_Control">
                <input type="checkbox" runat="server" id="chk_DeleteRole" />
            </div>
           <div class="Properties_Title">
               Quyền Sửa:</div>
            <div class="Properties_Control">
                <input type="checkbox" runat="server" id="chk_EditRole" />
            </div>
           <div class="Properties_Title">
               Quyền Xem:</div>
            <div class="Properties_Control">
                <input type="checkbox" runat="server" id="chk_ViewRole" />
            </div>
           <div class="Properties_Title">
               Quyền Kích hoát:</div>
            <div class="Properties_Control">
                <input type="checkbox" runat="server" id="chk_ActiveRole" />
            </div>
           <div class="Properties_Title">
               Quyền Hủy Kích hoạt:</div>
            <div class="Properties_Control">
                <input type="checkbox" runat="server" id="chk_InActiveRole" />
            </div>
           <div class="Properties_Title">
               Quyền Xuất bản:</div>
            <div class="Properties_Control">
                <input type="checkbox" runat="server" id="chk_PublishRole" />
            </div>
             <div class="Properties_Title">
               Quyền Hủy Xuất bản:</div>
            <div class="Properties_Control">
                <input type="checkbox" runat="server" id="chk_UnPublishRole" />
            </div>
             <div class="Properties_Title">
               Quyền Hủy Xuất bản:</div>
            <div class="Properties_Control">
                <input type="checkbox" runat="server" id="chk_ImportRole" />
            </div>
             <div class="Properties_Title">
               Quyền Export:</div>
            <div class="Properties_Control">
                <input type="checkbox" runat="server" id="chk_ExportRole" />
            </div>
             
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cph_Javascript" runat="server">
<script language="javascript" type="text/javascript">

    var hid_ListPageCode = document.getElementById("<%=hid_ListPageCode.ClientID %>");
    var chk_AddRole = document.getElementById('<%=chk_AddRole.ClientID %>');
    var chk_DeleteRole = document.getElementById('<%=chk_DeleteRole.ClientID %>');
    var chk_EditRole = document.getElementById('<%=chk_EditRole.ClientID %>');
    var chk_ViewRole = document.getElementById('<%=chk_ViewRole.ClientID %>');
    var chk_ActiveRole = document.getElementById('<%=chk_ActiveRole.ClientID %>');
    var chk_InActiveRole = document.getElementById('<%=chk_InActiveRole.ClientID %>');
    var chk_PublishRole = document.getElementById('<%=chk_PublishRole.ClientID %>');
    var chk_UnPublishRole = document.getElementById('<%=chk_UnPublishRole.ClientID %>');
    var chk_ImportRole = document.getElementById('<%=chk_ImportRole.ClientID %>');
    var chk_ExportRole = document.getElementById('<%=chk_ExportRole.ClientID %>');


    function ReCheckList()
    {
        var arr = hid_ListPageCode.value.split("|");
        if (arr.length < 1)
        {
            return;
        }
        var list_temp = "";
        for (var i = 1; i < 1000; i++)
        {
            var chk_Child = document.getElementById("chk_" + i + "_1");
            if (!chk_Child)
                break;
            for (var j = 1; j < 1000; j++)
            {
                var chk_Child_2 = document.getElementById("chk_" + i + "_" + j);
                if (!chk_Child)
                    break;
                for (var k = 0; k < arr.length; k++)
                {
                    if ("`" + chk_Child_2.value + "`" == arr[k])
                    {
                        chk_Child_2.checked = true;
                        if (list_temp.length > 0)
                        {
                            hid_ListPageCode.value += "|`" + ctr_this.value + "`";
                        }
                        else
                        {
                            hid_ListPageCode.value = "`" + ctr_this.value + "`";
                        }
                    }
                }
                
            }
        }
        //Sẽ có trường hợp trang này không còn trong treeview nhưng Pagecode vẫn còn trong hid_ListPageCode,
        //Vì vẫy để đảm bảo dữ liệu thì chỉ lấy những pageCode còn tồn tại trong treeview.
        hid_ListPageCode.value = list_temp;
    }
    function AddToListPageCode(ctr_this,IsHasChild)
    {
        if (IsHasChild)
            return;
            
       
        if (ctr_this.checked)
        {
            hid_ListPageCode.value = hid_ListPageCode.value.replace("|`" + ctr_this.value+"`", "");
            hid_ListPageCode.value = hid_ListPageCode.value.replace("`" + ctr_this.value + "`", "");
            if (hid_ListPageCode.value.length > 0)
            {
                hid_ListPageCode.value += "|`" + ctr_this.value + "`";
            }
            else
            {
                hid_ListPageCode.value = "`" + ctr_this.value + "`";
            }
        } else
        {
            hid_ListPageCode.value = hid_ListPageCode.value.replace("|`" + ctr_this.value + "`", "");
            hid_ListPageCode.value = hid_ListPageCode.value.replace("`" + ctr_this.value + "`", "");
        }
    }
    function CheckChild(ctr_this,Order, ChildCount, IsHasChild)
    {
        if (!IsHasChild)
            return;
            
        for (var i = 1; i <= ChildCount; i++)
        {
            var chk_Child = document.getElementById("chk_" + Order + "_" + i);
            if (chk_Child)
            {
                chk_Child.checked = ctr_this.checked;
                AddToListPageCode(chk_Child, false);
            }
        }
    }
    function CheckAll(ctr_this)
    {
        chk_AddRole.checked = ctr_this.checked;
        chk_DeleteRole.checked = ctr_this.checked;
        chk_EditRole.checked = ctr_this.checked;
        chk_ViewRole.checked = ctr_this.checked;
        chk_ActiveRole.checked = ctr_this.checked;
        chk_InActiveRole.checked = ctr_this.checked;
        chk_PublishRole.checked = ctr_this.checked;
        chk_UnPublishRole.checked = ctr_this.checked;
        chk_ImportRole.checked = ctr_this.checked;
        chk_ExportRole.checked = ctr_this.checked;
    }
    function BeforeSaveData()
    {
        if (hid_ListPageCode.value.length < 1)
        {
            alert("Xin hãy chọn một trang cần phân quyền trước.");
            return false;
        }
        if (!chk_AddRole.checked &&
            !chk_DeleteRole.checked &&
            !chk_EditRole.checked &&
            !chk_ViewRole.checked &&
            !chk_ActiveRole.checked &&
            !chk_InActiveRole.checked &&
            !chk_PublishRole.checked &&
            !chk_UnPublishRole.checked &&
            !chk_ImportRole.checked &&
            !chk_ExportRole.checked)
        {
            alert("Xin hãy chọn ít nhất một quyền cần cấp.");
            return false;
        }
    }
    ReCheckList();
</script>
</asp:Content>
