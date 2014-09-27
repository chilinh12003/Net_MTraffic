/// <reference path="jquery-1.4.2.js" />
/// <reference path="ForAll.js" />

var IsReloadPage = false; //Để cho phép khi nhấn nút close trên Facebox có được reload lại trang hay không

//Control div
var div_FaceBox;
var div_FaceBoxContent;
var chk_IsShowFaceBox;
var chk_IsDeleteContent;

//Cho phép FaceBox có được  fixed hay không
var IsFaceBoxFixed = true;

var SaveScrollTopInCookie = false;

//ScrollTop của page được lưu trong cookie khi trang refresh
var Scroll_Top = 0;

//Khái báo hàm call back, hàm này sẽ được gọi khi hiển thị xong facebox
var CallBack_FaceBox = function() { };

//Khải báo hàm call back khi tắt facebox
var CallBack_CloseFaceBox = function() { };

//Kích thước của Facebox được cộng thêm vào khi load IFrame
var ExcessIframe_w = 0;
var ExcessIframe_h = 10;

function FaceBox_SetScrollTop()
{
    //Lấy thông tin ScrollTop trong cookie
    Scroll_Top = ReadCookie("FaceBox_ScrollTop");
    if (Scroll_Top)
    {
        SetScrollTop(Number(Scroll_Top));

        //Xóa bỏ Scroll top trong cookie đi
        EraseCookie("FaceBox_ScrollTop");
    }
}

//Khởi tạo đầu tiên cho facebox
function InitFaceBox()
{    
    div_FaceBox = document.getElementById("FaceBox");

    div_FaceBox.style.position = "absolute";
    div_FaceBox.style.display = "block";
    div_FaceBox.style.visibility = "hidden";
    div_FaceBox.style.top = "-1000px";
    div_FaceBox.style.left = "-1000px";
    
    div_FaceBox.innerHTML = '<div class="FaceBox_T">\
                                <div class="FaceBox_TL">\
                                    <div class="FaceBox_TR">\
                                        <div class="FaceBox_TC">\
                                        </div>\
                                    </div>\
                                </div>\
                            </div>\
                            <div class="FaceBox_M">\
                                <div class="FaceBox_ML">\
                                    <div class="FaceBox_MR">\
                                        <div class="FaceBox_MC">\
                                            <div class="FaceBoxContent" id="FaceBoxContent" ></div>\
                                        </div>\
                                        <div class="FaceBox_Footer">\
                                            <img src="../Images/Images/FaceBox_Close.png" alt="Đóng" title="Đóng" onclick="CloseFaceBox();" />\
                                        </div>\
                                    </div>\
                                </div>\
                            </div>\
                            <div class="FaceBox_B">\
                                <div class="FaceBox_BL">\
                                    <div class="FaceBox_BR">\
                                        <div class="FaceBox_BC">\
                                        </div>\
                                    </div>\
                                </div>\
                            </div>';

    div_FaceBoxContent = document.getElementById("FaceBoxContent");
}

//Hiển thị facebox
function Show_FaceBox()
{
    //Nếu cho phép lưu Scroll top và cookie
    if (SaveScrollTopInCookie)
    {
        Scroll_Top = GetScrollTop();
        CreateCookie("FaceBox_ScrollTop", Scroll_Top, 1);
    }

    div_FaceBox.style.position = 'absolute';
    div_FaceBox.style.zIndex = 5;

    var content_W = div_FaceBoxContent.offsetWidth;
    var content_H = div_FaceBoxContent.offsetHeight;

    if (!content_H || !content_W)
    {
        content_H = content_W = 0;
    }
    var faceBox_W = content_W;
    var faceBox_H = content_H + 52;


    div_FaceBox.style.textAlign = "left";

    //Nếu mà chiều dài hoặc rộng của faceBox lớn hơn của màn hình thì
    //Kho cho phép Fixed FaceBox
    if (faceBox_H >= GetHeight())
    {
        IsFaceBoxFixed = false;
        div_FaceBoxContent.style.overflow = "auto";
        faceBox_H = GetHeight() - 10;
        div_FaceBoxContent.style.height = (faceBox_H - 52) + "px";
        div_FaceBoxContent.style.width = (div_FaceBoxContent.offsetWidth + 17) + "px";
        faceBox_W = faceBox_W + 17;
    }
    else if (faceBox_W <= 70) //nếu như nhỏ hơn width của Image close
    {
        faceBox_W = 70;
        div_FaceBoxContent.style.textAlign = "center";
    }
    else
    {
        div_FaceBoxContent.style.overflow = "hidden";
    }

    div_FaceBox.style.width = faceBox_W + "px";
    div_FaceBox.style.height = faceBox_H + "px";

    var x = 0;
    var y = 0;

    if (IsFaceBoxFixed)
    {
        x = GetWidth() / 2 - faceBox_W / 2 + GetScrollLeft();
        y = GetHeight() / 2 - faceBox_H / 2;
    }
    else
    {
        x = GetWidth() / 2 - faceBox_W / 2 + GetScrollLeft();
        y = GetHeight() / 2 - faceBox_H / 2 + GetScrollTop();
    }

    x = x < 0 ? 0 : x;
    y = y < 0 ? 0 : y;

    div_FaceBox.style.left = x + 'px';
    div_FaceBox.style.top = y + 'px';

    div_FaceBox.style.visibility = "visible";
    chk_IsShowFaceBox.checked = true;
    div_FaceBox.focus();

    if (IsFaceBoxFixed)
    {
        div_FaceBox.style.position = "fixed";
    }
    else
    {
        div_FaceBox.style.position = "absolute";
    }

    IsFaceBoxFixed = true;
}

function IframeLoadComplete(frameId)
{
    //Làm cho Iframe có thể auto height, width frame.document.body.scrollHeight
    var frame = document.getElementById(frameId);
    var innerDoc = (frame.contentDocument) ? frame.contentDocument : frame.contentWindow.document;
    var objToResize = (frame.style) ? frame.style : frame;

    objToResize.height = (parseInt(innerDoc.body.style.height) + ExcessIframe_h) + 'px';
    objToResize.width = (parseInt(innerDoc.body.style.width) + ExcessIframe_w) + 'px';

    Show_FaceBox();

    CallBack_FaceBox();

    //ẩn div loading
    HideLoading();
}

//Khi load facebox xong
function FaceBoxLoadComplete()
{
    Show_FaceBox();
    CallBack_FaceBox();

    //ẩn div loading
    HideLoading();
}

//Đóng facebox
function CloseFaceBox()
{
    HideLoading();
    div_FaceBox.style.position = "absolute";
    div_FaceBox.style.display = "block";
    div_FaceBox.style.visibility = "hidden";
    div_FaceBox.style.top = "-1000px";
    div_FaceBox.style.left = "-1000px";
    div_FaceBox.style.width = "auto";
    div_FaceBox.style.height = "auto";


    div_FaceBox.style.zIndex = 0;

    chk_IsShowFaceBox.checked = false;

    //Nếu mà cho phép reload lại trang
    if (IsReloadPage)
    {
        IsReloadPage = false;
        location.reload(true);
    }

    //Nếu cho phép xóa nội dụng trong facebox
    if (chk_IsDeleteContent.checked)
    {
        div_FaceBoxContent.innerHTML = "";
        div_FaceBoxContent.style.width = "auto";
        div_FaceBoxContent.style.height = "auto";
    }

    //Chạy hàm callback close facebox
    CallBack_CloseFaceBox();
}

function Set_IsDeleteContent(b_Value)
{
    chk_IsDeleteContent.checked = b_Value;
}

function BeginShowFaceBox()
{
    div_FaceBox.style.visibility = "hidden";
    div_FaceBoxContent.style.width = "auto";
    div_FaceBoxContent.style.height = "auto";
    div_FaceBox.style.width = "auto";
    div_FaceBox.style.height = "auto";
}

//Hiển thị facebox khi có liên quan tới ajax
function Show_FaceBoxAjax(str_URL)
{   
    BeginShowFaceBox();
    AjaxFunction(str_URL);
    return false;
}
function Show_FaceBoxHTML(str_HTML)
{
    BeginShowFaceBox();
    div_FaceBoxContent.innerHTML = str_HTML;
    FaceBoxLoadComplete();
    return false;
}
function Show_FaceBoxImage(img_URL)
{
    BeginShowFaceBox();
    if (img_URL.length < 1)
    {
        Show_FaceBox_Alert(1, "KHÔNG CÓ DỮ LIỆU!");
        return false;
    }

    var img_MaxWidth = GetWidth() - 100;
    var img_MaxHeight = GetHeight() - 70;
    
    div_FaceBoxContent.innerHTML = '<img alt="" title="" style="cursor:pointer; max-height:' + img_MaxHeight + 'px; max-width:' + img_MaxWidth + 'px;" src="' + img_URL + '" onload=" FaceBoxLoadComplete();" onclick="OpenImageOnNewWindows(this.src);"/>';
    return false;
}
function Show_FaceBoxVideo(video_URL)
{
    //video_URL = 'YouTube - Modern Talking - Let_s Talk About Love.flv';
    BeginShowFaceBox();
    var img_MaxWidth = GetWidth() - 100;
    var img_MaxHeight = GetHeight() - 70;
    div_FaceBoxContent.innerHTML = '<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" width="470" height="320" id="single1" name="single1">\
                                        <param name="movie" value="../Uploads/Video/player.swf">\
                                        <param name="allowfullscreen" value="true">\
                                        <param name="allowscriptaccess" value="always">\
                                        <param name="wmode" value="transparent">\
                                        <param name="flashvars" value="file=' + video_URL + '&autostart=true&stretching=exactfit">\
                                        <embed\
                                            type="application/x-shockwave-flash"\
                                            id="single2"\
                                            name="single2"\
                                            src="../Uploads/Video/player.swf"\
                                            width="470"\
                                            height="320"\
                                            bgcolor="undefined"\
                                            allowscriptaccess="always"\
                                            allowfullscreen="true"\
                                            wmode="transparent"\
                                            flashvars="file=' + video_URL + '&autostart=true&stretching=exactfit"\
                                        />\
                                    </object>';
    FaceBoxLoadComplete();
    return false;
}
function Show_FaceBox_Alert(AlertType, str_Message)
{
    BeginShowFaceBox();
    var str_HTML = '';
    switch (AlertType)
    {

        case 0: //Error
            str_HTML = ' <div class="FaceBoxAlert_UnSuccess">' + str_Message + '</div>';
            break;
        case 1: //Warning
            str_HTML = ' <div class="FaceBoxAlert_Warning">' + str_Message + '</div>';
            break;
        case 2: //Success
            str_HTML = ' <div class="FaceBoxAlert_Success">' + str_Message + '</div>';
            break;
        case 3:
            str_HTML = str_Message;
            break;
    }


    div_FaceBoxContent.innerHTML = str_HTML;
    FaceBoxLoadComplete();

    return false;
}
function OpenImageOnNewWindows(img_URL)
{
    window.open(img_URL);
}

function Show_FaceBoxIframe(URL, IsShowScroll)
{
    IsFaceBoxFixed = false;
    BeginShowFaceBox();

    if (IsShowScroll)
    {
        div_FaceBoxContent.innerHTML = ' <iframe src="' + URL + '" id="ifr_FaceBox" frameborder="0" scrolling="auto" onload="IframeLoadComplete(\'ifr_FaceBox\');"></iframe>';
    }
    else
    {
        div_FaceBoxContent.innerHTML = ' <iframe src="' + URL + '" id="ifr_FaceBox" frameborder="0" scrolling="no" onload="IframeLoadComplete(\'ifr_FaceBox\');"></iframe>';
    }
    return false;
}

//Load Iframe với kích thước của facebox được cộng thêm, để đảm bảo facebox không bị thiếu.
function Show_FaceBoxIframe_Excess(URL, IsShowScroll, Excess_w, Excess_h)
{
    ExcessIframe_h = Excess_h;
    ExcessIframe_w = Excess_w
    
    Show_FaceBoxIframe(URL, IsShowScroll);
}

//Hàm ajax
function AjaxFunction(URL_Get)
{
    URL_Get = CreateParameterTime(URL_Get);

    var xmlHttp = CreateAjax();
    
    xmlHttp.onreadystatechange = function()
    {
        if (xmlHttp.readyState == 4)
        {
            if (xmlHttp.responseText == "0" || xmlHttp.responseText == "")
            {
                Show_FaceBox_Alert(1, "KHÔNG CÓ DỮ LIỆU!");
            }
            else
            {
                div_FaceBoxContent.innerHTML = xmlHttp.responseText;
                FaceBoxLoadComplete();
            }
        }
    }
    
    xmlHttp.open("GET", URL_Get, true);
    xmlHttp.send(null);
}