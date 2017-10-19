$(function () {
    addScrollBar();
    layout();
    console.log('mainreport');
    navItemFn(); 
    table2Scroll();
    moreCharts();

    $(window).resize(function () {
        layout();
    });
})

var timeType = 'year';
var raPageIndex = 0;
var pageSize = 10;
var currentPage = $("#currentPage").html();
var pageIndex = currentPage - 1;
var totalPage = $("#totalPage").html();



function parseUrl() {
    var url = window.location.href;
    alert(url);
    var i = url.indexOf('?');
    if (i == -1) { return };
    var queryStr = url.substr(i + 1);
    var arr1 = queryStr.split('&');
    var arr2 = {};
    for (j in arr1) {
        var tar = arr1[j].split('=');
        arr2[tar[0]] = tar[1];
    };
    return arr2;
}

/*更多图表*/
function moreCharts() {
    $(".report_list li").click(function () {
        var index = $(this).index();
        switch (index) {
            case 0:
                alert(index);
                var layerIdx = layer.open({
                    type: 2,
                    anim: 3,
                    shade: .6,
                    title: false,// ['参数设置', 'text-align:center;'],
                    shadeClose: true,
                    area: ['1000px', '500px'],
                    content: ['/YCJK/V_CDJK/ReportCharts', 'no'],
                    end: function () {                       
                    }
                });
                break;
            default: break;
        }
    });
}


function addScrollBar() {
    $('.main_wrap').mCustomScrollbar({
        scrollButtons: {
            enable: true,
            scrollType: "continuous",
            scrollSpeed: 20,
            scrollAmount: 40
        },
        axis: "y",
        set_width: false,
        scrollbarPosition: "inside",
        theme: "minimal-dark"
    });
}


/*右侧固定导航*/
function navItemFn() {

    var reportNavHoverTimer, reportListHoverTimer;

    $(".nav_more").mouseenter(function () {
        clearTimeout(reportNavHoverTimer);
        clearInterval(reportListHoverTimer);
        $(".reportNav_wrap").stop().animate({ "marginRight": 0 }, 400);
    });
    $(".nav_more").mouseleave(function () {
        reportNavHoverTimer = setTimeout(function () {
            $(".reportNav_wrap").animate({ "marginRight": -318 }, 200);
        }, 500);
        $(".reportNav_wrap").mouseenter(function () {
            clearTimeout(reportNavHoverTimer);
        });
        $(".reportNav_wrap").mouseleave(function () {
            reportListHoverTimer = setTimeout(function () {
                $(".reportNav_wrap").animate({ "marginRight": -318 }, 200);
            }, 500);
        });
    });

    $(".nav_toTop").click(function () {
        $('.main_wrap').mCustomScrollbar("scrollTo", "top", { scrollEasing: "easeOut" });
    });
    $(".nav_top10").click(function () {
        var scrollDis = $(".showEquipNum").height() - 80;
        //   alert(scrollDis);
        $('.main_wrap').mCustomScrollbar("scrollTo", scrollDis, { moveDragger: true });
    });

    $(".nav_alarm").click(function () {
        var mainWrapH = $(".main_wrap").height();
        var alarmH = $(".alarm_wrap ").height();

        if (alarmH < mainWrapH) {
            $('.main_wrap').mCustomScrollbar("scrollTo", "bottom", { scrollEasing: "easeOut" });
        }

    });
}


function tableClone() {
    if ($('.table1 thead').next()) {
        $('.table1 thead').next().remove();
    }
    $('.table1 thead').after($('.table2 tbody').clone());
};

function table2Scroll() {
    $(".table2_wrap").mCustomScrollbar({
        scrollButtons: {
            enable: true,
            scrollType: "continuous",
            scrollSpeed: 20,
            scrollAmount: 40
        },
        axis: "y",
        set_width: false,
        scrollbarPosition: "inside",
        theme: "minimal-dark"
    });
}

function devidePage() {
    //分页
    $(".page_box").on('click', 'a#nextPage', function () {

        if (pageIndex == (totalPage - 1)) {
            alert('当前为最后一页');
        } else {
            pageIndex++;
            $("#currentPage").html(pageIndex + 1);
            getRealAlarm();
        }
    });
    $(".page_box").on('click', 'a#prevPage', function () {
        if (!(pageIndex == 0)) {
            pageIndex--;
            $("#currentPage").html(pageIndex + 1);
            getRealAlarm();
        } else {
            alert('当前为第一页');
        }
    });
    $(".page_box").on('click', 'a#firstPage', function () {
        if (!(pageIndex == 0)) {
            pageIndex = 0;
            $("#currentPage").html(pageIndex + 1);
            getRealAlarm();
        } else {
            alert('当前为第一页');
        }
    });
    $(".page_box").on('click', 'a#lastPage', function () {
        if (pageIndex == totalPage - 1) {
            alert('当前为最后一页');

        } else {
            pageIndex = totalPage - 1;
            // alert(pageIndex);
            $("#currentPage").html(pageIndex + 1);
            getRealAlarm();
        }
    });


}

function dealPage(total) {
    totalPage = Math.ceil(total / pageSize);
    $("#totalNum").html(total);
    $("#totalPage").html(totalPage);
    if (total == 0) {
        $("#currentPage").html(0);
    } else {
        $("#currentPage").html(pageIndex + 1);
    }
}


function showChartFn() {
    $(".report_list li").click(function () {
        $(this).addClass("active").siblings().removeClass("active");
    });
}


function layout() {
    var main_wrapW = $(".main_wrap").width();
    var useWrapW = $(".use_wrap").width();
    $(".chart_wrap").css({ "width": main_wrapW - useWrapW * 2 });
    
}