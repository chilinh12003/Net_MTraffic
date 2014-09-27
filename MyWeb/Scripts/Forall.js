/// <reference path="jquery-1.4.1-vsdoc.js" />
var Domain = "http://localhost:8080/MTraffic";

var fn_BeginLoad = function ()
{

}
var fn_DocumentClick = function (e)
{

}
//Định nghĩa cho các function sẽ được chay khi bắt đầu
$(document).ready(function ()
{
    fn_BeginLoad();

    $(document).click(function (e)
    {
        fn_DocumentClick(e);
    });

    InitSlider();
    InitNewsSlide();
});

function InitSlider()
{
    $(window).load(function ()
    {
        $('#slider').nivoSlider({
            effect: 'random', //Specify sets like: 'fold,fade,sliceDown'
            slices: 15,
            animSpeed: 500, //Slide transition speed
            pauseTime: 3000,
            startSlide: 0, //Set starting Slide (0 index)
            directionNav: true, //Next & Prev
            directionNavHide: true, //Only show on hover
            controlNav: true, //1,2,3...
            controlNavThumbs: false, //Use thumbnails for Control Nav
            controlNavThumbsFromRel: false, //Use image rel for thumbs
            controlNavThumbsSearch: '.jpg', //Replace this with...
            controlNavThumbsReplace: '_thumb.jpg', //...this in thumb Image src
            keyboardNav: true, //Use left & right arrows
            pauseOnHover: true, //Stop animation while hovering
            manualAdvance: false, //Force manual transitions
            captionOpacity: 0.5, //Universal caption opacity
            beforeChange: function () { },
            afterChange: function () { },
            slideshowEnd: function () { }, //Triggers after all slides have been shown
            lastSlide: function () { }, //Triggers when last slide is shown
            afterLoad: function () { } //Triggers when slider has loaded
        });
    });
}

function InitNewsSlide()
{
    $(function ()
    {
        $(".inner").jCarouselLite({
            btnNext: ".next",
            btnPrev: ".prev",
            visible: 4,
            auto:0,
            scroll: 1
        });
    });
}


