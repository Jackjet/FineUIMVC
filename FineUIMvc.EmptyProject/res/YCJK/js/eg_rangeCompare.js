var JZname = "";
var jzID = "";
var pumpName = "";
var pumpID = "";
var year = '';
var month = '';
var startDate = [formatDate(0) + " 00:00:00", formatDate(-1) + " 00:00:00"];
var endDate = [formatDate(1) + " 00:00:00", formatDate(0) + " 00:00:00"];
var urlJson = parseUrl();
var pageSize = 1000;
var chartTitle = decodeURIComponent(urlJson["chartTitle"]);

urlJson["jzName"] = decodeURIComponent(urlJson["jzName"]);
urlJson["pumpName"] = decodeURIComponent(urlJson["pumpName"]);
console.log(urlJson["jzName"]);

var dataJson = {
    "pumpJZID": jzID,
    "timeType": 4,
    "pageIndex": 0,
    "pageSize": 10000,
    "StartDate": startDate,
    "EndDate": endDate
};

var chartRequestUrl = '/V_YCJK/SearchBF_InYL';
var chartRequestOutUrl = '/V_YCJK/SearchBF_OutYL';


/*ready*/
$(document).ready(function () {
    setTimeRange();
    setPumpName(urlJson["pumpName"], urlJson["pumpID"]);
    setJzName(urlJson["jzName"], urlJson["jzID"]);
    JZname = $(".JZname").val();
    jzID = $(".JZname").attr("data-id");

    pumpID = $(".pumpName").attr("data-id");
    dataJson = {
        "pumpJZID": jzID,
        "timeType": 4,
        "pageIndex": 0,
        "pageSize": 10000,
        "StartDate": startDate,
        "EndDate": endDate
    };
    initRightChart();
    selectPumpAndJz();
    if (jzID !== "") {
        for (var i = 0; i < 2; i++) {
            getChartsData(i, startDate[i], endDate[i], jzID);
        }
        
    }

    searchTF()
    layout();
    addScroll();
    $(window).resize(function () {
        layout();
    });

    dropdownMenu();
});

laydate({
    elem: "#startTime0",
    format: "YYYY-MM-DD  hh:mm:ss",
    istime: true,
    istoday: false,
    issure: true,
});
laydate({
    elem: "#endTime0",
    format: "YYYY-MM-DD  hh:mm:ss",
    istime: true,
    istoday: false,
    issure: true,
});
laydate({
    elem: "#startTime1",
    format: "YYYY-MM-DD  hh:mm:ss",
    istime: true,
    istoday: false,
    issure: true,
});
laydate({
    elem: "#endTime1",
    format: "YYYY-MM-DD  hh:mm:ss",
    istime: true,
    istoday: false,
    issure: true,
});


function setTimeRange() {
    $(".searchTime").each(function (i) {
        var index = i;
        $(this).children(".startTime").val(startDate[i]);
        $(this).children(".endTime").val(endDate[i]);
    });
}

function parseUrl() {
    var url = window.location.href;
    console.log('url     ' + url);
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

function formatDate(i) {
    var curD = new Date();
    var d = new Date(curD.getTime() + i * 24 * 60 * 60 * 1000);
    var dM = (d.getMonth() + 1).toString().replace(/^(\d)$/, '0$1');
    var dD = d.getDate().toString().replace(/^(\d)$/, '0$1');
    var dateTemp = d.getFullYear() + "-" + (dM) + "-" + dD;
    year = d.getFullYear();
    month = dM;
    return dateTemp;
}

function msToDate(value) {
    var time = formatDate(0) + " 00:00:00";
    var times = Date.parse(new Date(time.replace(/-/g, "/")));
    d = new Date(value + times);
    var dM = (d.getMonth() + 1).toString().replace(/^(\d)$/, '0$1');
    var dD = d.getDate().toString().replace(/^(\d)$/, '0$1');
    var dH = d.getHours().toString().replace(/^(\d)$/, '0$1');
    var dMin = d.getMinutes().toString().replace(/^(\d)$/, '0$1');
    var dMs = d.getMinutes().toString().replace(/^(\d)$/, '0$1');
    var dateTemp = dH + ":" + dMin + ":" + dMs;
    return dateTemp;
}


function layout() {
    var chart_mainWrapH = $(".chart_mainWrap").height();
    var header_wrapH = $(".header_wrap").height();
    $(".chart_wrap").css({ "height": chart_mainWrapH - header_wrapH - 16 });
}

var chart_waterInOutPress = null;
var chart_waterInOutFlow = null;
var option1 = {};
var option2 = {};
function initRightChart() {

    chart_waterInOutPress = echarts.init($('#chart_waterInOutPress')[0]);
    chart_waterInOutFlow = echarts.init($('#chart_waterInOutFlow')[0]);
    option1 = {
        tooltip: {
            trigger: 'axis',
            formatter: function (params) {
                var tempStr = '';
                for (var i in params) {
                    if (params[i].data) {
                        tempStr += params[i].seriesName+' ' + ': ' + msToDate(params[i].data[0]) + ' ' + params[i].data[1] + 'MPa' + '<br/>';
                    } else {
                        tempStr += params[i].seriesName + ' : <br/>';
                    }
                }
                return tempStr;
            }
        },
        grid: {
            left: '3%',
            right: '30px',
            bottom: '3%',
            containLabel: true
        },
        legend: {
            data: []
        },
        toolbox: {
            show: true,
            feature: {
                dataView: { readOnly: false },
                magicType: { type: ['line', 'bar'] },
            }
        },
        xAxis: [
	        {
	            type: 'value',
	            data: [],
	            axisPointer: {
	                type: 'shadow'
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
	                formatter: function (value) {
	                    return msToDate(value);
	                },
	                textStyle: {
	                    color: '#c6c6c6'
	                }
	            }
	        }
        ],
        yAxis: [
	        {
	            type: 'value',
	            name: '压力',
	            min: 0,
	            //  interval: 50,
	            axisLabel: {
	                formatter: '{value} Mpa'
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
        series: [
        {
            name: '进水压力',
            type: 'line',
            symbolSize:0,
            itemStyle: {
                normal: {
                    color: '#ed7450',
                    lineStyle: { color: '#ed7450' }
                },
            },
            data: []
        },
        {
            name: '出水压力',
            type: 'line',
            symbolSize: 0,
            itemStyle: {
                normal: {
                    color: '#3398DB',
                    lineStyle: { color: '#3398DB' }
                },
            },
            data: []
        }, {
            name: '进水压力',
            type: 'line',
            symbolSize: 0,
            itemStyle: {
                normal: {
                    color: '#ed7ab0',
                    lineStyle: { color: '#ed7ab0' }
                },
            },
            data: []
        },
        {
            name: '出水压力',
            type: 'line',
            symbolSize: 0,
            itemStyle: {
                normal: {
                    color: '#339a6B',
                    lineStyle: { color: '#339a6B' }
                },
            },
            data: []
        }]
    };
    option2 = {
        tooltip: {
            trigger: 'axis',
            formatter: function (params) {
                var tempStr = '';
                for (var i in params) {
                    if (params[i].data) {
                        tempStr += params[i].seriesName + ' ' + ': ' + msToDate(params[i].data[0]) + ' ' + params[i].data[1] + '' + '<br/>';
                    } else {
                        tempStr += params[i].seriesName + ' : <br/>';
                    }
                }
                return tempStr;
            }
        },
        grid: {
            left: '3%',
            right: '30px',
            bottom: '3%',
            containLabel: true
        },
        legend: {
            data: ['进水瞬时流量', '出水瞬时流量']
        },
        toolbox: {
            show: true,
            feature: {
                dataView: { readOnly: false },
                magicType: { type: ['line', 'bar'] },
            }
        },
        xAxis: [
	        {
	            type: 'value',
	            data: [],
	            axisPointer: {
	                type: 'shadow'
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
	                formatter: function (value) {
	                    return msToDate(value);
	                },
	                textStyle: {
	                    color: '#c6c6c6'
	                }
	            }
	        }
        ],
        yAxis: [
	        {
	            type: 'value',
	            name: '流量',
	            min: 0,
	            //  interval: 50,
	            axisLabel: {
	                formatter: '{value} Mpa'
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
        series: [
        {
            name: '进水瞬时流量',
            type: 'line',
            symbolSize: 0,
            itemStyle: {
                normal: {
                    color: '#ed7450',
                    lineStyle: { color: '#ed7450' }
                },
            },
            data: []
        },
        {
            name: '出水瞬时流量',
            type: 'line',
            symbolSize: 0,
            itemStyle: {
                normal: {
                    color: '#3398DB',
                    lineStyle: { color: '#3398DB' }
                },
            },
            data: []
        }, {
            name: '进水瞬时流量',
            type: 'line',
            symbolSize: 0,
            itemStyle: {
                normal: {
                    color: '#ed7ab0',
                    lineStyle: { color: '#ed7ab0' }
                },
            },
            data: []
        },
        {
            name: '出水瞬时流量',
            type: 'line',
            symbolSize: 0,
            itemStyle: {
                normal: {
                    color: '#339a6B',
                    lineStyle: { color: '#339a6B' }
                },
            },
            data: []
        }]
    };
    chart_waterInOutPress.setOption(option1);
    chart_waterInOutFlow.setOption(option2);
}

/*图表搜索按钮*/
function searchTF() {
    $(".chart_searchBtn").click(function () {
        var isTime1 = false;
        var isTime2 = false;
        isTime1 = compareTimeRange($(".startTime").eq(0).val(), $(".endTime").eq(0).val());
        isTime2 = compareTimeRange($(".startTime").eq(1).val(), $(".endTime").eq(1).val());
        if (isTime1 && isTime2) {
            onLoading = true;
           var  startDate = [$(".startTime").eq(0).val(), $(".startTime").eq(1).val()];
           var  endDate = [$(".endTime").eq(0).val(), $(".endTime").eq(1).val()];
            TFname = $('.TFname').val();
          var   jzID = $(".JZname").attr("data-id");
            for (var i = 0; i < 2; i++) {
                getChartsData(i, startDate[i], endDate[i], jzID);
            }
        } else {
            alert('请输入正确的时间');
        }
    });
}
/*判断时间段间隔是否超过一天*/
function compareTimeRange(startT, endT) {
    var endMs = Date.parse(new Date(endT.replace(/-/g, "/")));
    var startMs = Date.parse(new Date(startT.replace(/-/g, "/")));
    var nextDayMs = getHistoryWeek(startT).getTime();

    if (endMs <=nextDayMs && endMs > startMs) {
        isTimeRight = true;
    } else {
        isTimeRight = false;
    }
    return isTimeRight;
}
/*获取某一天后一天*/
function getHistoryWeek(value) {
    var d = new Date(value);
    var nowYear = d.getFullYear();
    var nowMonth = d.getMonth();
    var nowDay = d.getDate();

    var getNextDay = new Date(nowYear, nowMonth, nowDay + 1);

    return getNextDay;
}
function addScroll() {
    $('.chart_wrap').mCustomScrollbar({
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



/*下拉列表*/
function dropdownMenu() {
    $(".selectedWrap input").val($(".selectMenu  li.active").children().html());
    $(".triangleWrap").click(function (event) {
        event.stopPropagation();
        $(this).parents(".selectDiagWrap").find(".selectMenu").removeClass("hide");
    });

    $(".selectMenu li").click(function (event) {
        var index = $(this).parent(".selectMenu").attr("data-index")
        var startDate = '';
        var endDate = '';
        event.stopPropagation();
        var dataKey = $(this).attr("data-key");
        switch (dataKey) {
            case "today":
                startDate = formatDate(0) + ' 00:00:00';
                endDate = formatDate(1) + ' 00:00:00';
                break;
            case "yestoday":
                startDate = formatDate(-1) + ' 00:00:00';
                endDate = formatDate(0) + ' 00:00:00';
                break;
            case "bYestoday":
                startDate = formatDate(-2) + ' 00:00:00';
                endDate = formatDate(-1) + ' 00:00:00';
                break;
            case "today0-6":
                startDate = formatDate(0) + ' 00:00:00';
                endDate = formatDate(0) + ' 06:00:00';
                break;
            case "today6-12":
                startDate = formatDate(0) + ' 06:00:00';
                endDate = formatDate(0) + ' 12:00:00';
                break;
            case "today12-y0":
                startDate = formatDate(0) + ' 12:00:00';
                endDate = formatDate(1) + ' 00:00:00';
                break;
            default: break;
        }
        $("#startTime" + index).val(startDate)
        $("#endTime" + index).val(endDate)
        $(".selectedWrap").eq(index).children("input").val($(this).children().html());
        $(this).parent().addClass("hide");

    });
    $(".chart_mainWrap").click(function () {

        $(".selectMenu").addClass("hide");
    });
}

//InOutWat_Flow_YW_YL
var onLoading = true;
function getChartsData(index, startDate, endDate, jzID) {
    if (onLoading && index == 0) {
        chart_waterInOutPress.showLoading();
    }
    $.ajax({
        url: '/V_YCJK/InOutWat_Flow_YW_YL',
        data: {
            "pumpJZID": jzID,
            "timeType": 4,
            "pageIndex": 0,
            "pageSize": 10000,
            "StartDate": startDate,
            "EndDate": endDate
        },
        dataType: 'JSON',
        success: function (data) {
            $(".selectedWrap input").val("");
            console.log('二供区间进出水压力');
            var result = data.obj;
            console.log(result);
            var seriesData = dealInPressDataFn(result.data);
            option1.series[2 * index].name = '进水压力 ' + ($("#startTime" + index).val().split(' '))[0];
            option1.series[2 * index + 1].name = '出水压力 ' + ($("#startTime" + index).val().split(' '))[0];
            option1.legend.data.push(option1.series[2 * index].name);
            option1.legend.data.push(option1.series[2 * index+1].name);
            option1.series[2 * index].data = seriesData.pressIn;
            option1.series[2 * index + 1].data = seriesData.pressOut;
            chart_waterInOutPress.setOption(option1);
            
            option2.series[2 * index].name = '进水瞬时流量 ' + ($("#startTime" + index).val().split(' '))[0];
            option2.series[2 * index + 1].name = '出水瞬时流量 ' + ($("#startTime" + index).val().split(' '))[0];
            option2.legend.data.push(option2.series[2 * index].name);
            option2.legend.data.push(option2.series[2 * index + 1].name);
            option2.series[2 * index].data = seriesData.flowIn;
            option2.series[2 * index + 1].data = seriesData.flowOut;
            chart_waterInOutFlow.setOption(option2);


            if (index == 1) {
                chart_waterInOutPress.hideLoading();
              
           }
            onLoading = false;
            

            chart_waterInOutPress.resize();
           
        },
        error: function (data) {
            console.log('二供压力流量水箱液位');
        }
    });
}

function dealInPressDataFn(data) {
    var itemInArr = [];
    var itemOutArr = [];
    var itemFlowIn = [];
    var itemFlowOut = [];
    var timeTempArr = [];
    for (var i = 0; i < data.length; i++) {
        var tempArr1 = [];
        var tempArr2 = [];
        var tempArr3 = [];
        var tempArr4 = [];
        var timeArr = data[i].day_time.split(":");
        var timeSe = Number(timeArr[0]) * 24 + Number(timeArr[1]) * 60 + Number(timeArr[2] * 60);
        var timeStr = '2017-01-01 ' + data[i].day_time;
        var timeMs = new Date(timeStr).getTime();
        var zeroTimeMs = new Date('2017-01-01' + " 00:00:00").getTime();
        var positiveMs = timeMs - zeroTimeMs;
        tempArr1.push(positiveMs);
        timeTempArr.push(timeSe);
        tempArr1.push(data[i].F41006);
        itemInArr.unshift(tempArr1);

        tempArr2.push(positiveMs);
        tempArr2.push(data[i].F41007);
        itemOutArr.unshift(tempArr2);

        tempArr3.push(positiveMs);
        tempArr3.push(data[i].F41024);
        itemFlowIn.unshift(tempArr3);

        tempArr4.push(positiveMs);
        tempArr4.push(data[i].F41025);
        itemFlowOut.unshift(tempArr4);
    }
   
    var seriesData = {
        "pressIn": itemInArr,
        "pressOut": itemOutArr,
        "flowIn": itemFlowIn,
        "flowOut":itemFlowOut
    };
    console.log(seriesData);
    return seriesData;
}

function setPumpName(name, id) {
    $(".pumpName").val(name);
    $(".pumpName").attr("data-id", id);
}
function setJzName(name, id) {
    $(".JZname").val(name);
    $(".JZname").attr("data-id", id);
}

function selectPumpAndJz() {
    $(".sPump_wrap .selectBtn").click(function () {
        //alert('选取泵房');
        pumpName = encodeURIComponent($(".pumpName").val());
        pumpID = $(".pumpName").attr("data-id");
        var index = layer.open({
            type: 2,
            anim: 3,
            shade: .6,
            title: ['泵房列表', 'text-align: center;color: #909090'],
            shadeClose: true,
            area: ['800px', '620px'],
            content: '/YCJK/Window/pumpWindow?pumpID=' + pumpID + '&pumpName=' + pumpName,
            success: function () {
                //  alert('OK');
            }
        });
    });
    $(".sJZ_wrap .selectBtn").click(function () {
        pumpID = $(".pumpName").attr("data-id");
        pumpName = $(".pumpName").val();
        jzName = encodeURIComponent($(".JZname").val());
        jzID = $(".JZname").attr("data-id");
        console.log('/YCJK/Window/pumpJZWindow?jzID=' + jzID + '&jzName=' + jzName + '&pumpID=' + pumpID);
        var index = layer.open({
            type: 2,
            anim: 3,
            shade: .6,
            title: ['机组列表', 'text-align: center;color: #909090'],
            shadeClose: true,
            area: ['800px', '620px'],
            content: '/YCJK/Window/pumpJZWindow?jzID=' + jzID + '&jzName=' + jzName + '&pumpID=' + pumpID,
            success: function () {
                //  alert('OK');
            }
        });
    });


}

