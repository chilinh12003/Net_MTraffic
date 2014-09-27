<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Ad_News_Edit.aspx.cs" Inherits="MyAdmin.Admin_News.Ad_News_Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_Header" runat="server">
    <link href="../CSS/autocomplete.css" rel="stylesheet" type="text/css" />
     <link href="../Calendar/dhtmlgoodies_calendar/dhtmlgoodies_calendar.css" rel="stylesheet"
        type="text/css" />

    <script src="../Calendar/dhtmlgoodies_calendar/dhtmlgoodies_calendar.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Tools" runat="server">
    <a href="Ad_News.aspx" runat="server" id="link_Cancel"><span class="Cancel"></span>Hủy </a>
    <asp:LinkButton runat="server" ID="lbtn_Save" OnClick="lbtn_Save_Click" OnClientClick="return CheckAll();">
     <span class="Save"></span>
            Lưu
    </asp:LinkButton>
    <asp:LinkButton runat="server" ID="lbtn_Accept" OnClick="lbtn_Apply_Click" OnClientClick="return CheckAll();">
     <span class="Accept"></span>
            Apply
    </asp:LinkButton>
    <a href="Ad_News_Edit.aspx" runat="server" id="link_Add"><span class="Add"></span>Thêm </a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_ToolBox" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_Search" runat="server">
    <div class="Edit_Left">
        <div class="Edit_Title">
            Nhóm dịch vụ</div>
        <div class="Edit_Control">
            <asp:DropDownList runat="server" ID="ddl_ServiceGroup" OnSelectedIndexChanged="ddl_ServiceGroup_IndexChanged" AutoPostBack="true">
            </asp:DropDownList>
        </div>
        <div class="Edit_Title">
            Dịch vụ</div>
        <div class="Edit_Control">
            <asp:DropDownList runat="server" ID="ddl_Service" OnSelectedIndexChanged="ddl_Service_IndexChanged" AutoPostBack="true">
            </asp:DropDownList>
        </div>
        <div class="Edit_Title" style="height: 40px;">
            Thời gian trả tin</div>
        <div class="Edit_Control" style="height: 50px;">
         <div class="NewLine" style="padding-bottom:3px; padding-top:0px;">
            <label>Chọn thời gian trả tin cho dịch vụ:</label><label runat="server" id="label_PushTime"></label>
            </div>
            <input type="text" runat="server" id="tbx_PushTime" style="width: 70px;" />
            <input type="button" value="..." onclick="displayCalendar(document.getElementById('<%=tbx_PushTime.ClientID %>'),'dd/mm/yyyy',this)" />
            <div>
                <label>
                    Giờ:</label></div>
            <select runat="server" id="sel_PushHour">
            </select>
            <div>
                <label>
                    Phút:</label></div>
            <select runat="server" id="sel_PushMinute">
            </select>
            <div style="display:none;">
                <label>
                    Giây:</label></div>
            <select  style="display:none;" runat="server" id="sel_PushSecond">
            </select>
           
        </div>
        <div class="NewLine " runat="server" id="div_Street">
            <div class="Edit_Title">
                Địa điểm
            </div>
            <div class="Edit_Control">
                <select runat="server" id="sel_Position">
                </select>
            </div>
            <div class="Edit_Title">
                Tên đường phố
            </div>
            <div class="Edit_Control">
                <input type="text" runat="server" id="tbx_StreetName" autocomplete="off" onkeyup="return auto(this,event);" onblur="BlurAuto(this);" onfocus="FocusAuto(this);" />
                <input type="hidden" runat="server" id="hid_StreetID" />
            </div>
        </div>
        <div class="Edit_Title">
            Tiêu đề
        </div>
        <div class="Edit_Control" style="position: relative;">
            <div class="auto" runat="server" id="div_StreetList">
                <ul id="ul_StreetList">
                </ul>
            </div>
            <input type="text" runat="server" id="tbx_NewsName" style="width: 99%;" />
        </div>
        <div class="NewLine">
            <div class="Edit_Title" style="height: 150px;">
                Nội dung:</div>
            <div class="Edit_Control_Editor">
                <textarea id="tbx_Contents" onkeyup="return CheckMaxLength(this,479,event);" runat="server" style="float: left; height: 150px; width: 99%;"></textarea>
                <div id="div_Length" style="float: left; margin-top: 6px; width: 100%; font-size: 12px; font-weight: bold;">
                    Bạn đã nhập vào 0/800 ký tự</div>
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
            <div class="Properties_Title" style="display: none;">
                Tình trạng:</div>
            <div class="Properties_Control" style="display: none;">
                <select runat="server" id="sel_Status">
                </select>
            </div>
            <div class="Properties_Title">
                Loại tin:</div>
            <div class="Properties_Control">
                <select runat="server" id="sel_NewsType">
                </select>
            </div>
            <div class="Properties_Title">
                Ưu tiên:</div>
            <div class="Properties_Control">
                <input type="text" runat="server" id="tbx_Priority" value="0" onkeypress="return isNumberKey_int(event);" />
            </div>
            <div class="Properties_Title">
                Kích hoạt:</div>
            <div class="Properties_Control">
                <input type="checkbox" runat="server" id="chk_Active" checked="checked" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph_Content" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cph_Javascript" runat="server">
    <script language="javascript" type="text/javascript">
        var sel_PushHour = document.getElementById("<%= sel_PushHour.ClientID %>");
        var sel_PushMinute = document.getElementById("<%= sel_PushMinute.ClientID %>");
        var sel_Service = document.getElementById("<%= ddl_Service.ClientID %>")
        var tbx_NewsName = document.getElementById("<%= tbx_NewsName.ClientID %>")
        function ChangePushTime(PushTime)
        {
            var arr_time = PushTime.split(":");
            if (arr_time.length != 2)
                return;
            var PushHour = parseInt( Number(arr_time[0]));
            var PushMinute = parseInt(Number(arr_time[1]));

            $(sel_PushHour).val(PushHour);
            $(sel_PushMinute).val(PushMinute);
            
        }
        function CheckMaxLength(othis, iMax, e)
        {
            if (iMax < $(othis).val().length)
            {
                $(othis).val(String($(othis).val()).substring(0, iMax + 1));
                $("#div_Length").html("Bạn đã nhập vào: " + ($(othis).val().length) + "/" + (iMax + 1) + " ký tự");
                return false;
            }
            else
            {
                $("#div_Length").html("Bạn đã nhập vào: " + ($(othis).val().length) + "/" + (iMax + 1) + " ký tự");
            }
        }

        var tbx_Contents = document.getElementById("<%=tbx_Contents.ClientID %>");
        $("#div_Length").html("Bạn đã nhập vào: " + ($(tbx_Contents).val().length) + "/" + (480) + " ký tự");

        var tbx_StreetName = document.getElementById("<%= tbx_StreetName.ClientID %>");
        var hid_StreetID = document.getElementById("<%= hid_StreetID.ClientID %>");
        var sel_Position = document.getElementById("<%= sel_Position.ClientID %>");

        function ChoiseValue(ctr_this)
        {
            if (Auto_TimeOut)
                clearTimeout(Auto_TimeOut);

            if (!ctr_this)
                return;

            var ctr_current_li = $("#ul_StreetList").children("li.active").get(0);

            if (ctr_current_li)
            {
                $(ctr_current_li).removeClass("active");
            }
            if (ctr_this)
            {
                $(ctr_this).addClass("active");
            }

            AddStreet($(ctr_this).attr("value"), $(ctr_this).html());
        }

        function AddStreet(StreetID, StreetName)
        {

            $(tbx_StreetName).val(StreetName);
            $(hid_StreetID).val(StreetID);
            $(tbx_StreetName).focus();

        }
        function auto(ctr_this, e)
        {

            try
            {
                if (e == null) { e = window.event; }

                if (e.keyCode != 38 && e.keyCode != 40 && e.keyCode != 13)
                {
                    SearchStreet(ctr_this);
                    return;
                }
                var ctr_current_li = $("#ul_StreetList").children("li.active").get(0);
                var ctr_next_li = $("#ul_StreetList").children("li").get(0);

                if (e.keyCode == 13) //la enter
                {
                    if (!ctr_current_li)
                        return;

                    AddStreet($(ctr_current_li).attr("value"), $(ctr_current_li).html());
                }
                if (e.keyCode == 38) //la Up
                {
                    if (ctr_current_li)
                    {
                        ctr_next_li = $(ctr_current_li).prev().get(0);
                    }
                }
                if (e.keyCode == 40) //La down)
                {
                    if (ctr_current_li)
                    {
                        ctr_next_li = $(ctr_current_li).next().get(0);
                    }
                }
                if (e.keyCode == 40 || e.keyCode == 38)
                {
                    if (ctr_current_li)
                    {
                        $(ctr_current_li).removeClass("active");
                    }
                    if (ctr_next_li)
                    {
                        $(ctr_next_li).addClass("active");
                    }
                    else
                    {
                        ctr_next_li = $("#ul_StreetList").children("li").get(0);
                        if (ctr_next_li)
                        {
                            $(ctr_next_li).addClass("active");
                        }
                    }
                }
            }
            catch (ex)
            {
                alert(ex);
            }
        }

        function GetStreetID(ctr_this)
        {
            try
            {

                if (!ctr_this || !sel_Position)
                    return false;

                var PositionID = $(sel_Position).val();
                var StreetName = $(ctr_this).val();

                var url = Domain + "/MyAjax.NewsAjax.MyAjaxNews.GetStreetID.ajax";

                $.ajax({
                    type: "POST", //Phương thức gửi request là POST hoặc GET
                    data: "PositionID=" + PositionID + "&StreetName=" + StreetName,
                    url: url, //Đường dẫn tới nơi xử lý request ajax
                    success: function (string)
                    {
                        var arr_JSON = $.parseJSON(string);

                        $.each(arr_JSON, function ()
                        {
                            //nếu như biến là kết quả trả về
                            if (this.Parameter == "Result")
                            {
                                //nếu kiểu trả về là success
                                if (this.CurrentTypeResult == "1")
                                {
                                    $(hid_StreetID).val(this.Value);
                                    HideErrorInfo("Auto_Error");
                                }
                                else
                                {
                                    ShowErrorInfo(tbx_StreetName, "Auto_Error", "Hiện không có đường phố nào phù hợp với nội dung đã nhập (" + $(tbx_StreetName).val() + ") xin hãy kiểm tra lại");
                                }
                            }
                            return false;
                        });
                    }
                });
            }
            catch (ex)
            {
                alert(ex);
            }
        }
        function SearchStreet(ctr_this)
        {

            if (!ctr_this || !sel_Position)
                return false;

            var PositionID = $(sel_Position).val();
            var StreetName = $(ctr_this).val();

            var url = Domain + "/MyAjax.NewsAjax.MyAjaxNews.SearchStreet.ajax";

            $.ajax({
                type: "POST", //Phương thức gửi request là POST hoặc GET
                data: "PositionID=" + PositionID + "&StreetName=" + StreetName,
                url: url, //Đường dẫn tới nơi xử lý request ajax
                success: function (string)
                {
                    var arr_JSON = $.parseJSON(string);

                    $.each(arr_JSON, function ()
                    {
                        //nếu như biến là kết quả trả về
                        if (this.Parameter == "Result")
                        {
                            //nếu kiểu trả về là success
                            if (this.CurrentTypeResult == "1")
                            {
                                $("#ul_StreetList").html(this.Value);
                            }
                            else
                            {
                                alert(this.Description);
                            }
                        }
                        return false;
                    });
                }
            });
        }

        function CheckAll()
        {

        }

        function HideAuto()
        {
            GetStreetID(tbx_StreetName);

            $("#ul_StreetList").html("");
        }

        var Auto_TimeOut = null;
        function BlurAuto(ctr_this)
        {
            Auto_TimeOut = setTimeout(' HideAuto();', 400);            
        }
        function FocusAuto(ctr_this)
        {
            SearchStreet(ctr_this);
        }
    </script>
</asp:Content>
