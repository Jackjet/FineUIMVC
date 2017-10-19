var TFname = "";
var baseId = "";
var startDate = [formatDate(0) + " 00:00:00", formatDate(-1) + " 00:00:00"];
var endDate = [formatDate(1) + " 00:00:00", formatDate(0) + " 00:00:00"];
var urlJson = parseUrl();
var flagMs = new Date(startDate[1]).getTime();
urlJson["TFname"] = decodeURIComponent(urlJson["TFname"]);
console.log(urlJson["TFname"]);
var legData = [startDate[0], startDate[1]];
/*ready*/
$(document).ready(function () {
    
    setTimeRange()
    urlJson["TFname"] !== "undefined" ? $(".TFname").val(urlJson["TFname"]) : $(".TFname").val("");
    (urlJson["baseId"] == "undefined" || urlJson["baseId"] == "undefined#") ? $(".TFname").attr("data-id", "") : $(".TFname").attr("data-id", urlJson["baseId"]);
    TFname = $(".TFname").val();
    baseId = $(".TFname").attr("data-id");
    initRightChart();
    if (baseId !== "") {
        console.log('压力综合报表');
       // loadComparePress(startDate[0], endDate[0], startDate[1], endDate[1], baseId)
        for (var i = 0; i < 2; i++) {
            loadPressChart(i,startDate[i], endDate[i], baseId);
        }
    }
    searchTF();
    getTFname();//弹出调峰列表

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
    return dateTemp;
}
function msToDate(value) {
    var time = formatDate(0)+" 00:00:00";
    var times = Date.parse(new Date(time.replace(/-/g, "/")));
    d = new Date(value + times);
    var dM = (d.getMonth() + 1).toString().replace(/^(\d)$/, '0$1');
    var dD = d.getDate().toString().replace(/^(\d)$/, '0$1');
    var dH = d.getHours().toString().replace(/^(\d)$/, '0$1');
    var dMin = d.getMinutes().toString().replace(/^(\d)$/, '0$1');
    var dMs = d.getMinutes().toString().replace(/^(\d)$/, '0$1');
    var dateTemp =dH + ":" + dMin + ":" + dMs;
    return dateTemp;
}
function layout() {
    var chart_mainWrapH = $(".chart_mainWrap").height();
    var header_wrapH = $(".header_wrap").height();
    $(".chart_wrap").css({ "height": chart_mainWrapH - header_wrapH - 16 });
}

var chart_waterInOutPress = null;
var option1 = {};
function initRightChart() {
    chart_waterInOutPress = echarts.init($('#chart_waterInOutPress')[0]);
  
   option1 = {
        tooltip: {
            trigger: 'axis',
            formatter: function (params) {
                var tempStr = '';
                for (var i in params) {
                    if (params[i].data) {
                        tempStr += params[i].seriesName + ': ' + msToDate(params[i].data[0]) + ' ' + params[i].data[1] + 'MPa' + '<br/>';
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
        xAxis: [
	        {
	            type: 'value',
	            axisPointer: {
	                type: 'shadow'
	            },
	         //  min: flagMs,
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
            name: '',
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
            name: '',
            type: 'line',
            symbolSize: 0,
            itemStyle: {
                normal: {
                    color: '#e477c7',
                    lineStyle: {
                        color: '#e477c7',
                    }
                },
            },
           
            data: []
        }]
    };
    chart_waterInOutPress.setOption(option1);
}
var onLoading = true;
function loadPressChart(index, startDate, endDate, baseId) {
    if (onLoading && index == 0) {
        chart_waterInOutPress.showLoading();
    }
    $.ajax({
        url: '/V_CDJK/SearchYL_HisReport',//SearchYL_HisReportCompare,SearchYL_HisReport

        data: {
            "pageIndex": 0,
            "pageSize": 100000,
            "ID": baseId,
            "StartDate": startDate,
            "EndDate": endDate
        },
        dataType: 'JSON',
        // beforeSend: loadingFunction, 
        success: function (data) {
            $(".selectedWrap input").val("");
            var result = data.data;
            console.log(result);
             console.log('区间压力'+index);
             if (result.length > 0) {
                 option1.series[index].name = result[0].TempTime.split("T")[0];
                 option1.series[index].data = dealYLdata(result);
                 option1.legend.data.push(result[0].TempTime.split("T")[0]);
                 chart_waterInOutPress.setOption(option1);
               
             } else {
                 option1.series[index].name ='';
                 option1.series[index].data =[];
                 option1.legend.data.push([]);
                 chart_waterInOutPress.setOption(option1);
             }
             if (index == 1) {
                 chart_waterInOutPress.hideLoading();

             }
            onLoading = false;
            
        },
        //   complete: loadingMiss,
        error: function (data) {
            console.log('错误：' + data.responseText);
        }
    });

}

function loadComparePress(startDate0, endDate0, startDate1, endDate1, baseId) {
    $.ajax({
        url: '/V_CDJK/SearchYL_HisReportCompare',//SearchYL_HisReportCompare,SearchYL_HisReport
        data: {
            "StartDate1": startDate0,
            "EndDate1": endDate0,
            "ID": baseId,
            "StartDate2": startDate1,
            "EndDate2": endDate1,
            "pageIndex": 0,
            "pageSize":100000
        },
        dataType: 'JSON',
        // beforeSend: loadingFunction, 
        success: function (data) {
            console.log(data);
            var result = data.data;
            var seriesArr = [];
            var legendArr = [];
                for (var i in result) {
                    seriesArr.push(dealYLdata(result[i].detailData.data));
                    var legItem = result[i].field.split(' ');
                    legendArr.push(legItem[0]);
                }
                var option = {
                    legend: legendArr,
                    series: [{
                        name: legendArr[0],
                        data:seriesArr[0]
                    }, {
                        name: legendArr[1],
                        data: seriesArr[1]
                    }]
                };
            chart_waterInOutPress.setOption(option);
           // chart_waterInOutPress.hideLoading();
        },
        //   complete: loadingMiss,
        error: function (data) {
            console.log('错误：' + data.responseText);
        }
    });

}

function dealYLdata(data) {
    var itemArr = [];
   
    for (var i in data) {
        var tempArr = [];
        var timeMs = new Date(data[i].TempTime.replace('T', " ")).getTime();
        var zeroTimeMs = new Date(data[i].TempTime.split("T")[0] + " 00:00:00").getTime();
       
        var positiveMs = timeMs - zeroTimeMs;
        tempArr.push(positiveMs);
        tempArr.push(data[i].FMpa);
        itemArr.unshift(tempArr);
    }
       
    return itemArr;
}


/*图表搜索按钮*/
function searchTF() {
    $(".chart_searchBtn").click(function () {
        var isTime1 = false;
        var isTime2 = false;
        isTime1=compareTimeRange($(".startTime").eq(0).val(), $(".endTime").eq(0).val());
        isTime2 = compareTimeRange($(".startTime").eq(1).val(), $(".endTime").eq(1).val());
        if (isTime1 && isTime2) {
            onLoading = true;
            startDate = [$(".startTime").eq(0).val(), $(".startTime").eq(1).val()];
            endDate = [$(".endTime").eq(0).val(), $(".endTime").eq(1).val()];
            TFname = $('.TFname').val();
            baseId = $(".TFname").attr("data-id");
            for (var i = 0; i < 2; i++) {
                loadPressChart(i, startDate[i], endDate[i], baseId);
            }
        } else {
            alert('请输入正确的时间');
        }
       // loadPressChart(startDate, endDate, baseId);
      
    });
}


/*判断时间段间隔是否超过一天*/
function compareTimeRange(startT,endT) {
    var endMs = Date.parse(new Date(endT.replace(/-/g, "/")));
    var startMs = Date.parse(new Date(startT.replace(/-/g, "/")));
    var nextDayMs = getHistoryWeek(startT).getTime();
    if (endMs <= nextDayMs && endMs > startMs) {
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
   
    var getNextDay= new Date(nowYear, nowMonth, nowDay+ 1);
  
    return getNextDay;
}
/*搜索泵房 搜索图标点击*/

function getTFname() {
    $(".selectBtn").click(function () {
        var layerIndex = layer.open({
            type: 2,
            anim: 3,
            shade: .6,
            title: [""],//['参数设置', 'text-align:center;']
            shadeClose: true,
            area: ['90%', '90%'],
            content: ['/YCJK/V_CDJK/YL_selectYlName', 'no'],
            success: function () {
                //  alert('OK');
            }
        });
    });

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


function setTFname(tfId, tfName) {
    $(".TFname").val(tfName);
    $(".TFname").attr("data-id", tfId);
    baseId = $(".TFname").attr("data-id");
    TFname = $(".TFname").val();

    dayDataJson = {
        "BASEID": baseId,
        "timeType": 4,
        "s_date": startDate,
        "e_date": endDate
    };
    $(".chart_searchBtn").trigger('click');
  //  loadPressChart(startDate, endDate, baseId);
}

/*下拉列表*/
function dropdownMenu() {
   // $(".selectedWrap input").val($(".selectMenu  li.active").children().html());
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
        //$(".selectedWrap input").val($(this).children().html());
        $(this).parent().addClass("hide");

    });
    $(".chart_mainWrap").click(function () {

        $(".selectMenu").addClass("hide");
    });
}