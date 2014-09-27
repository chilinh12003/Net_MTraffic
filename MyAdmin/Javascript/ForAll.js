var Domain = "http://localhost:8080/VOVTrafficCMS";

//Tạo Cookie
function CreateCookie(name, value, days)
{
    if (days)
    {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else var expires = "";
    document.cookie = name + "=" + value + expires + "; path=/";
}

//Đọc Cookie
function ReadCookie(name)
{
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++)
    {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

///Xóa Cookie
function EraseCookie(name)
{
    CreateCookie(name, "", -1);
}



function Random_Number(len)
{
    chars = new Array('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');

    charCount = chars.length();

    stringLength = len;
    var outputString;
    i = 0;
    do
    {
        random = Math.floor(Math.random());
        random *= charCount;
        random = chars[random];
        outputString += parseString(random);
        i++;
    }
    while (i < stringLength);

    return outputString;
}

function AskBeforeDelete(message)
{
    return confirm(message);
}

function isNumberKey_Date(evt)
{
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode == 47)//dấu "/"
        return true;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

function isNumberKey_int(evt)
{
    var charCode = (evt.which) ? evt.which : event.keyCode

    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

function isNumberKey_double(evt)
{
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode == 46)//dấu "."
        return true;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}


//Kiểm tra Email
function CheckEmail(mail)
{
    /* text=document.getElementById(txt).value;*/
    mail = trim(mail);
    var re = /^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,4}$/
    if (mail.match(re) == null)
    {
        return false;
    }
    return true;
}

function trim(sString)
{
    while (sString.substring(0, 1) == ' ')
    {
        sString = sString.substring(1, sString.length);

    }
    while (sString.substring(sString.length - 1, sString.length) == ' ')
    {
        sString = sString.substring(0, sString.length - 1);
    }
    return sString;
}

/// <summary>
/// Lấy chiều rộng cho phép của màn hình
/// </summary>
function GetWidth()
{
    //#region
    var w = 0;
    if (self.innerHeight)
    {
        w = self.innerWidth;
    }
    else if (document.documentElement && document.documentElement.clientHeight)
    {
        w = document.documentElement.clientWidth;
    }
    else if (document.body)
    {
        w = document.body.clientWidth;
    }
    return w;
    //#endregion
}

/// <summary>
/// Lấy chiều cao cho phép của màn hình
/// </summary>
function GetHeight()
{
    //#region
    var h = 0;
    if (self.innerHeight)
    {
        h = self.innerHeight;
    }
    else if (document.documentElement && document.documentElement.clientHeight)
    {
        h = document.documentElement.clientHeight;
    }
    else if (document.body)
    {
        h = document.body.clientHeight;
    }
    return h
    //#endregion
}

/// <summary>
/// Lấy tọa độ x của chuột
/// </summary>
function GetMouseX(e)
{
    //#region
    var tempX = 0;

    if (typeBrowser == 0)
    {
        tempX = e.clientX - divBanDo.offsetLeft + document.body.scrollLeft - document.body.clientLeft;
    } else
    {
        tempX = e.pageX - divBanDo.offsetLeft + document.body.scrollLeft - document.body.clientLeft;
    }
    if (tempX < 0) { tempX = 0; }
    return tempX;
    //#endregion
}

/// <summary>
/// Lấy tọa độ y của chuột
/// </summary>
function GetMouseY(e)
{
    //#region
    var tempY = 0;
    if (typeBrowser == 0)
    {
        tempY = e.clientY - divBanDo.offsetTop + document.body.scrollTop - document.body.clientTop;
    } else
    {
        tempY = e.pageY - divBanDo.offsetTop + document.body.scrollTop - document.body.clientTop;
    }

    if (tempY < 0) { tempY = 0; }
    return tempY;
    //#endregion
}

function GetScrollLeft()
{
    var Temp_Left = 0;
    if (self.pageYOffset)
    {
        Temp_Left = self.pageXOffset;
    }
    else if (document.documentElement && document.documentElement.scrollTop)
    {

        Temp_Left = document.documentElement.scrollLeft;

    }
    else if (document.body)
    {
        Temp_Left = document.body.scrollLeft;
    }
    if (!Temp_Left || Temp_Left < 0)
        Temp_Left = 0;

    return Number(Temp_Left);
}
function GetScrollTop()
{
    var Temp_Top;
    if (self.pageYOffset)
    {
        Temp_Top = self.pageYOffset;
    }
    else if (document.documentElement && document.documentElement.scrollTop)
    {
        Temp_Top = document.documentElement.scrollTop;
    }
    else if (document.body)
    {
        Temp_Top = document.body.scrollTop;
    }

    if (!Temp_Top || Temp_Top < 0)
        Temp_Top = 0;

    return Number(Temp_Top);
}


//Hàm dùng để Khởi tạo đối tượng Ajax (XMLHTML)
function CreateAjax()
{
    //#region
    var XmlHttp;

    //Creating object of XMLHTTP in IE
    try
    {
        XmlHttp = new ActiveXObject("Msxml2.XMLHTTP");
    }
    catch (e)
    {
        try
        {
            XmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
        }
        catch (oc)
        {
            XmlHttp = null;
        }
    }
    //Creating object of XMLHTTP in Mozilla and Safari 
    if (!XmlHttp && typeof XMLHttpRequest != "undefined")
    {
        XmlHttp = new XMLHttpRequest();
    }
    return XmlHttp;

    //#endregion
}

function CreateParameterTime(str_URL)
{
    if (str_URL.indexOf("?", 0) > 0)
    {
        return str_URL + "&time=" + (new Date()).getTime();
    }
    else
    {
        return str_URL + "?time=" + (new Date()).getTime();
    }
}

function OpenNewWindow(str_URL)
{
    if (str_URL && str_URL.length > 0)
        window.open(str_URL);
    return false;
}


function ShowLoading(ctr_This, IsFixed, IsAddClick)
{
    if (!ctr_This)
        return false;

    //Chuyển đối tượng DOM sang đối tượng của Jquery
    if (ctr_This.id)
    {
        ctr_This = $("#" + ctr_This.id);
    }
    else
    {
        ctr_This = $(ctr_This);
    }

    var w = $(ctr_This).outerWidth();
    var h = $(ctr_This).outerHeight();

    var ctr_Loading = $("#divLoading");
    $(ctr_Loading).css("position", "absolute");

    var top = $(ctr_This).offset().top - (IsFixed ? GetScrollTop() : 0);
    var left = $(ctr_This).offset().left;

    $(ctr_Loading).css("width", w + "px");
    $(ctr_Loading).css("height", h + "px");
    $(ctr_Loading).css("top", top + "px");
    $(ctr_Loading).css("left", left + "px");

    if (IsFixed)
    {
        $(ctr_Loading).css("position", "fixed");
    }

    if (IsAddClick)
    {
        //Thêm sự kiện click cho document
        document.onmousedown = DoMouseDownWhenLoading;
    }
}

function DoMouseDownWhenLoading(evt)
{
    if (evt == null) { evt = window.event; }

    if (confirm("Hệ thống đang xử lý, bạn có chắc muốn thực hiện hành động này?"))
    {
        HideLoading();
    }
    else
    {
        return false;
    }
}

//Cho pháp ẩn loading hay không
var AllowHideLoading = true;
function HideLoading()
{
    if (!AllowHideLoading)
    {
        AllowHideLoading = true;
        return false;
    }
    document.onmousedown = null;

    var ctr_Loading = $("#divLoading");
    $(ctr_Loading).css("position", "absolute");
    $(ctr_Loading).css("top", "-1000px");
    $(ctr_Loading).css("left", "-1000px");
}

function ChangeURL(str_URL)
{
    window.location.href = str_URL;
}


function ShowNoteInfo(ctr_This, ID_DivNode, str_NoteContent)
{

    if (!ctr_This)
    {
        return;
    }
    //Chuyển đối tượng DOM sang đối tượng của Jquery
    ctr_This = $(ctr_This);

    var divNoteInfo = $("#" + ID_DivNode);
    if (divNoteInfo.get(0))
    {
        divNoteInfo.html(' <div class="NoteInfo_In">\
                                <div class="NoteInfo_L">\
                                </div>\
                                <div class="NoteInfo_R">' +
                                    str_NoteContent +
                                '</div>\
                            </div>');
    }
    else
    {
        divNoteInfo = $("<div></div>");
        divNoteInfo.attr('id', ID_DivNode);
        divNoteInfo.attr('class', "NoteInfo");

        divNoteInfo.html(' <div class="NoteInfo_In">\
                                <div class="NoteInfo_L">\
                                </div>\
                                <div class="NoteInfo_R">' +
                                    str_NoteContent +
                                '</div>\
                            </div>');
        ctr_This.parent().append(divNoteInfo);
    }
    var top = $(ctr_This).offset().top;
    var left = $(ctr_This).offset().left + $(ctr_This).outerWidth();

    divNoteInfo.css("visibility", 'visible');
    divNoteInfo.css("top", top + 'px');
    divNoteInfo.css("left", left + 'px');
    divNoteInfo.css("z-index", "9");

}

function HideNoteInfo(ID_DivNode)
{
    var divNoteInfo = $("#" + ID_DivNode);
    if (!divNoteInfo.get(0))
        return;
    divNoteInfo.remove();
}

function ShowErrorInfo(ctr_This, ID_DivError, str_NoteContent)
{
    if (!ctr_This)
    {
        return;
    }
    //Chuyển đối tượng DOM sang đối tượng của Jquery
    ctr_This = $(ctr_This);

    var divErrorInfo = $("#" + ID_DivError);

    if (divErrorInfo.get(0))
    {
        $(divErrorInfo).html('  <div class="ErrorInfo_In">\
                                        <div class="ErrorInfo_L">\
                                        </div>\
                                        <div class="ErrorInfo_R">' +
                                            str_NoteContent +
                                        '</div>\
                                    </div>');
    }
    else
    {
        divErrorInfo = $("<div></div>");
        divErrorInfo.attr('id', ID_DivError);
        divErrorInfo.attr('class', "ErrorInfo");

        divErrorInfo.html('  <div class="ErrorInfo_In">\
                                        <div class="ErrorInfo_L">\
                                        </div>\
                                        <div class="ErrorInfo_R">' +
                                            str_NoteContent +
                                        '</div>\
                                    </div>');
        ctr_This.parent().append(divErrorInfo);
    }
    var top = $(ctr_This).offset().top;
    var left = $(ctr_This).offset().left + $(ctr_This).outerWidth();

    divErrorInfo.css("visibility", 'visible');
    divErrorInfo.css("top", top + 'px');
    divErrorInfo.css("left", left + 'px');
    divErrorInfo.css("z-index", "9");
}


function HideErrorInfo(ID_DivError)
{
    var divErrorInfo = $("#" + ID_DivError);
    if (!divErrorInfo.get(0))
        return;
    divErrorInfo.remove();
}
