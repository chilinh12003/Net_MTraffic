
///Hàm tạo Khung (Frame
function Create_Frame(Type, ID_Frame, str_Title)
{
    var div_Frame = document.getElementById(ID_Frame);
    var html_Before = div_Frame.innerHTML;
    var html_Begin = '';
    var html_End = '';
    var w = 0;
    switch (Type)
    {        
        case 1: //Frame_1
            w = 10;
            html_Begin = '  <div class="Frame_1">\
                                <div class="Frame_1_T">\
                                    <div class="Frame_1_TL">\
                                        <div class="Frame_1_TR">\
                                            <div class="Frame_1_TC">' + str_Title + '\
                                            </div>\
                                        </div>\
                                    </div>\
                                </div>\
                                <div class="Frame_1_M">\
                                    <div class="Frame_1_ML">\
                                        <div class="Frame_1_MR">\
                                            <div class="Frame_1_MC">';

            html_End = '                    </div>\
                                        </div>\
                                    </div>\
                                </div>\
                                <div class="Frame_1_B">\
                                    <div class="Frame_1_BL">\
                                        <div class="Frame_1_BR">\
                                            <div class="Frame_1_BC">\
                                            </div>\
                                        </div>\
                                    </div>\
                                </div>\
                            </div>';
            break;
        default:

            break;
    }

    if (div_Frame.offsetWidth && div_Frame.offsetWidth > w)
        div_Frame.style.width = (div_Frame.offsetWidth - w) + 'px';
    div_Frame.innerHTML = html_Begin + html_Before + html_End;

}
