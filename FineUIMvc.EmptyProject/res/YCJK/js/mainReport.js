$(function () {
    addScrollBar();
    layout();
    console.log('mainreport');
    navItemFn();
    getRealAlarm();
    getEquipNum();
  


    /* 
    initMainReportChart();
    getWaterUseTop10();
    getElecUseTop10();*/



    selectTimeType();
    var obj1 = '.waterUse_list';
    var obj2 = '.elecUse_list';
    hoverFn(obj1);
    hoverFn(obj2);

    showChartFn();
    table2Scroll();
    devidePage();
    console.log(equipNumArr);
    $(window).resize(function () {
        layout();
    });

    createReportList(reportEG);
    eg_ReportList();

})
//var urlJson = parseUrl();
//var pumpId = urlJson["pumpID"];
//var jzId = urlJson["jzID"];
var timeType = 'year';
var raPageIndex = 0;
var pageSize = 10;
var currentPage = $("#currentPage").html();
var pageIndex = currentPage - 1;
var totalPage = $("#totalPage").html();

var equipNumArr = {
    "pumpNum": 0,
    "jzNum": 0,
    "xsNum": 0,
    "gsNum": 0,
    "otherNum": 0,
    "onlineNum": 0,
    "alarmNum": 0,
    "offlineNum":0
};

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
        $(".reportNav_wrap").stop().animate({"marginRight":0},400);
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
        $('.main_wrap').mCustomScrollbar("scrollTo", "top", {scrollEasing:"easeOut"});
    });
    $(".nav_top10").click(function () {
        var scrollDis = $(".showEquipNum").height()-80;
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

/*获取各类设备数量*/
function getEquipNum() {
        $.ajax({
            url: '/V_YCJK/SearchZLB_Count',
            dataType: 'JSON',
            success: function (data) {
                console.log(data);
                console.log(data[0].zx_cont);
               // equipNumArr.jzNum = data[0].all_cont;//机组数
                equipNumArr.onlineNum = data[0].zx_cont;
                equipNumArr.alarmNum = data[0].bj_cont;
                equipNumArr.offlineNum = data[0].lx_cont;
                console.log('机组数量');
                setEquipNum();
            },
            error: function (data) {
                console.log('机组状态数据获取出错');
            }
        });
        $.ajax({
            url: '/V_YCJK/Pump_PumpJZ_Count',
            dataType: 'JSON',
            success: function (data) {
                console.log(data);
                var result = data.obj;
                console.log('机组数量');
              
                for (var i = 0; i < result.length; i++) {
                    switch (result[i]._name) {
                        case "泵房":
                            equipNumArr.pumpNum = result[i]._count;
                            break;
                        case "机组":
                            equipNumArr.jzNum = result[i]._count;
                            break;
                        case "罐式":
                            equipNumArr.gsNum = result[i]._count;
                            break;
                        case "箱式":
                            equipNumArr.xsNum = result[i]._count;
                            break;
                        default: break;
                    }
                }
                equipNumArr.otherNum = equipNumArr.jzNum - equipNumArr.gsNum - equipNumArr.xsNum;
                setEquipNum();
            },
            error: function (data) {
                console.log('泵房机组个数数据获取出错');
            }
        });
}

function setEquipNum() {
    for (var key in equipNumArr) {
        $('.itemNum[data-name=' + key + ']').html(equipNumArr[key]);
    }
}

/*前十机组报表*/

/*var mainReportChart1 = echarts.init($('#waterUseChart')[0]);
var mainReportChart2 = echarts.init($('#elecUseChart')[0]);*/
function initMainReportChart() {

    var option1 = {
        tooltip: {
            trigger: 'axis',
            axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
            }
        },
        grid: {
            left: '3%',
            right: '30px',
            bottom: '16px',
            top: '5px',
            containLabel: true
        },
        xAxis: [
            {
                type: 'category',
                data: [],
                axisTick: {
                    alignWithLabel: true
                },
                axisLine: {
                    lineStyle: {
                        color: "#dddddd"
                    }
                },
                axisTick: {
                    show: false
                },
                axisLabel: {
                    textStyle: {
                        color: '#c6c6c6'
                    }
                }
            }
        ],
        yAxis: [
            {
                type: 'value',
                axisLine: {
                    lineStyle: {
                        color: "#dddddd"
                    }
                },
                axisTick: {
                    show: false
                },
                axisLabel: {
                    textStyle: {
                        color: '#c6c6c6'
                    }
                }
            }
        ],
        series: [
            {
                name: '用水量',
                type: 'bar',
                barWidth: '50%',
                itemStyle: {
                    normal: {
                        color: new echarts.graphic.LinearGradient(
                            0, 0, 0, 1,
                            [
                                { offset: 0, color: '#59cdfd' },
                                { offset: 0.5, color: '#27befd' },
                                { offset: 1, color: '#07b4fd' }
                            ]
                        )
                    }
                },
                data: []
            }
        ]
    };
    var option2 = {
        tooltip: {
            trigger: 'axis',
            axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
            }
        },
        grid: {
            left: '3%',
            right: '30px',
            bottom: '16px',
            top: '5px',
            containLabel: true
        },
        xAxis: [
            {
                type: 'category',
                data: [1, 2, 3, 4, 5, 6, 7],
                axisTick: {
                    alignWithLabel: true
                },
                axisLine: {
                    lineStyle: {
                        color: "#dddddd"
                    }
                },
                axisTick: {
                    show: false
                },
                axisLabel: {
                    textStyle: {
                        color: '#c6c6c6'
                    }
                }
            }
        ],
        yAxis: [
            {
                type: 'value',
                axisLine: {
                    lineStyle: {
                        color: "#dddddd"
                    }
                },
                axisTick: {
                    show: false
                },
                axisLabel: {
                    textStyle: {
                        color: '#c6c6c6'
                    }
                }
            }
        ],
        series: [
            {
                name: '用电量',
                type: 'bar',
                barWidth: '50%',
                itemStyle: {
                    normal: {
                        color: new echarts.graphic.LinearGradient(
                            0, 0, 0, 1,
                            [
                                { offset: 0, color: '#fdc959' },
                                { offset: 0.5, color: '#fd8723' },
                                { offset: 1, color: '#fd6407' }
                            ]
                        )
                    }
                },
                data: []
            }
        ]
    };
    mainReportChart1.setOption(option1);
    mainReportChart2.setOption(option2);
}

function getWaterUseTop10() {
    $.ajax({
        url: '/V_YCJK/SL_PumpJZTop10',
        data: {
            "timeType":timeType
        },
        dataType: 'JSON',
        success: function (data) {
            console.log(data);
            console.log('用水量top10');
            var data = data.obj;
            var oTarget = $(".waterUse_list");
            dealTop10List(oTarget,data);
            var yAxisData = [];
            var xAxisData = [];
            for (var i = 0; i < data.length; i++) {
                yAxisData.push(data[i].FTotalOutLL);
                xAxisData.push(data[i].PumpJZName);
            }
            console.log(yAxisData);
            var option = {
                xAxis: [
                    {
                        data: xAxisData,
                    }
                ],
                series: [
                    {
                        data: yAxisData
                    }
                ]
            };
            mainReportChart1.setOption(option);
            console.log(data);
        },
        error: function (data) {
            console.log('用水量top10数据获取出错');
        }
    });
}

function dealTop10List(obj,waterUseData) {
    obj.empty();

    var str = '';
    var tempValue = 0;
    for(var i=0;i<waterUseData.length;i++)
    {
        if (obj.attr("class") == "waterUse_list")
        {
            tempValue = waterUseData[i].FTotalOutLL;
        } else if (obj.attr("class") == "elecUse_list") {
            tempValue = waterUseData[i].FTotalDL;
            console.log(tempValue);
        }
      //  var pAddress = waterUseData[i].PAddress ? waterUseData[i].PAddress : "";
        var tempStr = '<li data-id="' + waterUseData[i].BaseID + '" data-pumpId="' + waterUseData[i].ID + '">\
                        <div class="front">\
                              <div class="line1_wrap .clearfix">\
                                  <div class="seriNum_ico"><p>' + (i + 1) + '</p></div><div class="jzName">' + waterUseData[i].PumpJZName + '</div><div class="useValue">' + tempValue + '</div>\
                              </div>\
                              <div class="equipAddr">' + (waterUseData[i].PAddress ? waterUseData[i].PAddress : "") + '</div>\
                        </div>\
                        <div class="back">\
                            <div class="leftArrow"></div>\
                            <div class="backText">原理图</div>\
                            <div class="rightArrow"></div>\
                        </div>\
                    </li>';
        str += tempStr;
    }
    obj.append(str);
    for (var j = 0; j < 3; j++) {
        obj.find(".seriNum_ico").eq(j).addClass("top3_ico");
    }
}

function getElecUseTop10() {
    $.ajax({
        url: '/V_YCJK/DL_PumpJZTop10',
        data: {
            "timeType": timeType
        },
        dataType: 'JSON',
        success: function (data) {
            console.log(data);
            console.log('用电量top10');

            var data = data.obj;
            var oTarget = $(".elecUse_list");
            dealTop10List(oTarget,data);
            var yAxisData = [];
            var xAxisData = [];
            for (var i = 0; i < data.length; i++) {
                yAxisData.push(data[i].FTotalDL);
                xAxisData.push(data[i].PumpJZName);
            }
            console.log(yAxisData);
            var option = {
                xAxis: [
                    {
                        data: xAxisData,
                    }
                ],
                series: [
                    {
                        data: yAxisData
                    }
                ]
            };
            mainReportChart2.setOption(option);
        },
        error: function (data) {
            console.log('用电量top10数据获取出错');
        }
    });
}
/*实时报警*/
function getRealAlarm() {
    $.ajax({
        url: '/V_YCJK/SearchAlarm',
        data: {
            "pumpID": null,
            "pumpJZID": null,
            "pageIndex": raPageIndex,
            "pageSize": 10,//搜索是pageSize值无效
        },
        dataType: 'JSON',
    //    beforeSend: loadingFunction,
        success: function (data) {
            console.log(typeof data);
            console.log(data);
            console.log("实时报警数据");
            dealAlarmTable(data.obj.data);
            tableClone();
            dealPage(data.obj.total);
        },
   //     complete: loadingMiss,
        error: function (data) {
            console.log('错误：' + data.responseText);
        }
    });
}


function dealAlarmTable(alarmData) {
    $(".table2 tbody").empty();
    var trStr = '';
    for (var i = 0; i < alarmData.length; i++) {
        var tempStr = '<tr><td data-field="BaseID" data-key="' + alarmData[i].Fkey + '">' + alarmData[i].BaseID + '</td>\
                                        <td data-filed="PName" class="PName">' + alarmData[i].PName + '</td>\
                                        <td data-field="PumpJZName" class="PumpJZName">' + alarmData[i].PumpJZName + '</td>\
                                        <td data-field="FSetMsg">' + alarmData[i].FSetMsg + '</td>\
                                        <td data-field="TempTime">' + alarmData[i].TempTime.replace('T', ' ') + '</td>\
                                        <td data-field="TimeRange">' + alarmData[i].TimeRange + '</td>';
        trStr += tempStr;
    }
    $(".table2 tbody").append(trStr);
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

/*选择top10  年月日*/
function selectTimeType() {
    $(".selectTime a").click(function () {
        timeType = $(this).attr("data-timeType");
        $(this).addClass("active").siblings().removeClass("active");
        var dataType = $(this).parent().attr("data-type");
        if (dataType == "water") {
            getWaterUseTop10();
        } else if (dataType == "elec") {
            getElecUseTop10();
        }
    });
}

function hoverFn(obj) {
    $('body').delegate(obj+' .front', 'mouseover', function () {
        $(".click").removeClass("click");
        $(this).removeClass('clickTwo').addClass('click').css('z-index', '0');
        $(this).next().removeClass('clickTwo').addClass('click').css('z-index', '2');
    });
    $('body').delegate(obj+' .back', 'mouseleave', function () {
        $(this).removeClass('click').addClass('clickTwo').css('z-index', '0');
        $(this).prev().removeClass('click').addClass('clickTwo').css('z-index', '2');
    });

    $('body').delegate(obj, 'mouseleave', function () {
        $(this).find('.back').removeClass('click').addClass('clickTwo').css('z-index', '0');
        $(this).find('.back').prev().removeClass('click').addClass('clickTwo').css('z-index', '2');
    });
  
    $('body').delegate(obj +'  .back', 'click', function () {
        var baseId = $(this).parent().attr("data-id").toLowerCase();
        var pumpId = $(this).parent().attr("data-pumpId");
        parent.$('.mainIframe').attr('src', F.baseUrl + 'YCJK/V_YCJK/EGBFSB?pumpId=' + pumpId + '&jzId=' + baseId);
        parent.$('.leftNav li[data-cla="shebei"]').addClass('active').siblings().removeClass('active');
    });

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

    /*mainReportChart1.resize();
    mainReportChart2.resize();*/
}