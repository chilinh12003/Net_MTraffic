/// <reference path="jquery-1.4.2.js" />
/// <reference path="ForAll.js" />

//CheckBox cho biết ToolBox có hiện hoặc ẩn khi postback
var chk_ShowToolBox;

//Thẻ div chứa nội dung của Searchbox
var divToolBox_Content;

//Span chứa dòng chữ "mở ra", "đóng lại" ở phía cuối của Toolbox
var spanToolBox_Text;

//Span chưa Image Up, Down bên dưới của Toolbox
var spanToolBox_Image;

var DelayHideSupMenu;

//ID của SupMenu đang hiện
var SubMenuCurrentShow_ID;

//Hidden list các ID cần thực hiện một hành động như Xóa, sửa, publish...
var hid_ListCheckAll;

//Tạo hàm sẽ load cuối cùng
var LastLoadComplete = function LastOnLoad(){};

function ShowSubMenu(ctr_This)
{
    //nếu tồn tại 1 SubMenu đang hiện thì ẩn menu đó đi
    if (SubMenuCurrentShow_ID)
    {
        document.getElementById(SubMenuCurrentShow_ID).style.display = "none";
        SubMenuCurrentShow_ID = null;
    }

    if (DelayHideSupMenu)
    {
        clearTimeout(DelayHideSupMenu);
    }

    if (!ctr_This)
        return;

    var left = $(ctr_This).offset().left;
    var top = $(ctr_This).offset().top + 24;

    var ctr_SumMenu_Item = document.getElementById(ctr_This.title);

    if (!ctr_SumMenu_Item)
        return;

    ctr_SumMenu_Item.style.top = top + "px";
    ctr_SumMenu_Item.style.left = left + "px";

    ctr_SumMenu_Item.style.display = "inline";
    SetWidthChildMenu(ctr_SumMenu_Item);
}
function SetWidthChildMenu(ctr_Parent)
{
   
    if (!ctr_Parent)
        return;
    var arr_Child = ctr_Parent.childNodes;
    if (!arr_Child || arr_Child.length < 1)
        return;
        
    var w = ctr_Parent.offsetWidth;
    
    if (!w || w < 1)
        w = 150;
        
    for (var i = 0; i < arr_Child.length; i++)
    {
        if (!arr_Child[i] || arr_Child[i].tagName != "A")
            continue;
        if (!arr_Child[i].style.width || parseInt(arr_Child[i].style.width) < 50)
            arr_Child[i].style.width = w + "px";
    }
}
//Khi con trỏ chuột thoát khỏi menu
function HideSubMenu(SubMenu_ID)
{
    if (!SubMenu_ID)
        return;

    if (DelayHideSupMenu)
    {
        clearTimeout(DelayHideSupMenu);
        SubMenuCurrentShow_ID = null;
    }

    SubMenuCurrentShow_ID = SubMenu_ID;

    DelayHideSupMenu = setTimeout("HideDelaySubMenu('" + SubMenu_ID + "')", 1000);
}

function HideDelaySubMenu(SubMenu_ID)
{
    var ctr_SubMenu = document.getElementById(SubMenu_ID);
    if (ctr_SubMenu)
    {
        ctr_SubMenu.style.display = "none";
    }
    SubMenuCurrentShow_ID = null;
}

///Khi con trỏ chuột thoát ra khỏi SupMenu
function HideSubMenu_OnSupMenu(ctr_This)
{
    if (!ctr_This)
        return;

    ctr_This.style.display = "none";
}

///Khi con trỏ chuột trỏ tới SupMenu
function UndoHideSubMenu()
{
    if (DelayHideSupMenu)
        clearTimeout(DelayHideSupMenu);
}

//Show/Hide ToolBox
function ToggleToolBox()
{
    if (!divToolBox_Content)
    {
        return;
    }
    chk_ShowToolBox.checked = !chk_ShowToolBox.checked;
    if (chk_ShowToolBox.checked)
    {
        divToolBox_Content.style.display = "inline";
        spanToolBox_Text.innerHTML = "Đóng";
        spanToolBox_Image.style.backgroundImage = "url(../Images/Icons/Up.gif)";
    }
    else
    {
        divToolBox_Content.style.display = "none";
        spanToolBox_Text.innerHTML = "Mở";
        spanToolBox_Image.style.backgroundImage = "url(../Images/Icons/Down.gif)";
    }
}

function ShowHideToolBox(IsShow)
{
    if (IsShow)
    {
        chk_ShowToolBox.checked = true;
        divToolBox_Content.style.display = "inline";
        spanToolBox_Text.innerHTML = "Đóng";
        spanToolBox_Image.style.backgroundImage = "url(../Images/Icons/Up.gif)";
    }
    else
    {
        chk_ShowToolBox.checked = false;
        divToolBox_Content.style.display = "none";
        dspanToolBox_Text.innerHTML = "Mở";
        spanToolBox_Image.style.backgroundImage = "url(../Images/Icons/Down.gif)";
    }
}

//Thiết lập lại Show/Hide cho ToolBox khi Postback
function ToggleToolBox_Onload()
{
    if (chk_ShowToolBox.checked)
    {
        divToolBox_Content.style.display = "inline";
        spanToolBox_Text.innerHTML = "Đóng";
        spanToolBox_Image.style.backgroundImage = "url(../Images/Icons/Up.gif)";
    }
    else
    {
        divToolBox_Content.style.display = "none";
        spanToolBox_Text.innerHTML = "Mở";
        spanToolBox_Image.style.backgroundImage = "url(../Images/Icons/Down.gif)";
    }
}





var AllowShowMessage = true;
///Tạo Cookie để cho phép hiển thị thông báo
function SetCookieShowMessage(IsShow)
{
    if (IsShow == true)
    {
        CreateCookie("AllowShowMessage", "1", 3);
        AllowShowMessage = true;
    }
    else
    {
        CreateCookie("AllowShowMessage", "0", 3);
        AllowShowMessage = false;
    }
}


//Khởi tạo Cookie về Cho phép hiển thị thông báo
function CreateCookieShowMessage()
{
    var value = ReadCookie("AllowShowMessage");

    //Nếu Cookie không tồn tại thì tạo cookie
    if (value == null)
    {
        CreateCookie("AllowShowMessage", "1", 3);
    }
}

///Lấy giá trị AllowShowMessage trong cookie và gán cho checkbox control
function SetCheckBoxShowMessageFromCookie(chk_ShowMessage_ID)
{
    var chk_ShowMessage;
    if (chk_ShowMessage_ID)
    {
        chk_ShowMessage = document.getElementById(chk_ShowMessage_ID);
    }
    var value = ReadCookie("AllowShowMessage");

    AllowShowMessage = value == 1 || !value ? true : false;

    //Gắn checked cho checkbox ShowMessage nếu Checkbox tồn tại
    if (chk_ShowMessage)
        chk_ShowMessage.checked = AllowShowMessage;
}

///Lẫy Cookie về cho phép hiển thị thông báo
function GetCookieShowMessage()
{
    var value = ReadCookie("AllowShowMessage");

    AllowShowMessage = value == 1 || !value ? true : false;
}

///Hiển thị 1 câu thông báo nào đó
function ShowMessage(mess)
{
    //Kiểm tra xem người dùng có cho phép hiển thị thông báo hay không
    if (AllowShowMessage)
        alert(mess);
    return false;
}


//Chọn (bỏ chọn) tất cả các checkbox trên GridView
function SelectCheckBox_All(ctr_This)
{
    if (ctr_This)
    {
        for (var i = 0; i < 1000; i++)
        {
            var ctr_Check = document.getElementById("CheckAll_" + i);
            if (ctr_Check)
            {
                ctr_Check.checked = ctr_This.checked;
            }
            else
            {
                break;
            }
        }
    }

}

function BeforeDeteleData()
{
    if (confirm("Bạn có chắc muốn xóa các mục đã chọn?"))
    {
        if (!hid_ListCheckAll)
            return false;
        
        hid_ListCheckAll.value = "";
        for (var i = 0; i < 1000; i++)
        {
            var chk_CheckAll = document.getElementById("CheckAll_" + i);
            if (chk_CheckAll)
            {
                if (chk_CheckAll.checked)
                {
                    if (hid_ListCheckAll.value.length > 0)
                    {
                        hid_ListCheckAll.value = hid_ListCheckAll.value + "|" + chk_CheckAll.value;
                    }
                    else
                    {
                        hid_ListCheckAll.value = chk_CheckAll.value;
                    }
                }
            }
            else
            {
                break;
            }
        }

        if (hid_ListCheckAll.value.length < 1)
        {
            alert("Xin hãy chọn ít nhất một mục để xóa!");
            return false;
        }
        else
        {
            return true;
        }
    }
    else
    {
        return false;
    }
}

function BeforeEditData()
{
    if (!hid_ListCheckAll)
        return false;
        
    hid_ListCheckAll.value = "";
    for (var i = 0; i < 1000; i++)
    {
        var chk_CheckAll = document.getElementById("CheckAll_" + i);
        if (chk_CheckAll)
        {
            if (chk_CheckAll.checked)
            {
                if (hid_ListCheckAll.value.length > 0)
                {
                    hid_ListCheckAll.value = hid_ListCheckAll.value + "|" + chk_CheckAll.value;
                }
                else
                {
                    hid_ListCheckAll.value = chk_CheckAll.value;
                }
                break;
            }
        }
        else
        {
            break;
        }
    }
    if (hid_ListCheckAll.value.length < 1)
    {
        alert("Xin hãy chọn một mục để bắt đầu chỉnh sửa!");
        return false;
    }
    else
    {
        return true;
    }
}

function GetAllCheck(str_Alert)
{
    if (!hid_ListCheckAll)
        return false;

    hid_ListCheckAll.value = "";
    for (var i = 0; i < 1000; i++)
    {
        var chk_CheckAll = document.getElementById("CheckAll_" + i);
        if (chk_CheckAll)
        {
            if (chk_CheckAll.checked)
            {
                if (hid_ListCheckAll.value.length > 0)
                {
                    hid_ListCheckAll.value = hid_ListCheckAll.value + "|" + chk_CheckAll.value;
                }
                else
                {
                    hid_ListCheckAll.value = chk_CheckAll.value;
                }
            }
        }
        else
        {
            break;
        }
    }

    if (hid_ListCheckAll.value.length < 1)
    {
        alert(str_Alert);
        return false;
    }
    else
    {
        return true;
    }
}



///Check lại tất cả các checkbox trên grid đã được chọn trước đó
function ReCheck_CheckboxOnGrid()
{
    if (!hid_ListCheckAll || hid_ListCheckAll.value.length < 1)
        return false;

    var arr_ID = hid_ListCheckAll.value.split("|");

    if (arr_ID.length < 1)
        return;

    for (var i = 0; i < 1000; i++)
    {
        var chk_CheckAll = document.getElementById("CheckAll_" + i);
        
        if (chk_CheckAll)
        {
            for (var j = 0; j < arr_ID.length; j++)
            {
                if (chk_CheckAll.value == arr_ID[j])
                {
                    chk_CheckAll.checked = true;
                }
            }
        }
        else
        {
            break;
        }
    }
    
}















