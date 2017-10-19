$(function () {
    var scId = $("#scId").attr("data-sid");
    getWaterInfo(scId, realPageSize);
    var timer = setInterval(function () {
        getWaterInfo(scId, 1);
    }, 10000);
    var startTime = formatDate1(0) + " 00:00:00";
    var endTime = formatDate1(1) + " 00:00:00";
    $("#startTime1").val(startTime);
    $("#startTime2").val(startTime);
    $("#endTime1").val(endTime);
    $("#endTime2").val(endTime);
    changeChartBlock();
    initChartFn();
    searchClickFn();
    //layout();
    addScroll();
    turnPage();
    $(window).resize(function () {
        //layout();
        realChart1.resize();
        realChart2.resize();
        historyChart1.resize();
        historyChart2.resize();
    });
    console.log(window.location);

    laydate({
        elem: "#startTime1",
        format: "YYYY-MM-DD  hh:mm:ss",
        istime: true,
        istoday: false,
        issure: true,
        fixed: true
    });
    laydate({
        elem: "#endTime1",
        format: "YYYY-MM-DD  hh:mm:ss",
        istime: true,
        istoday: false,
        issure: true,
    });
    laydate({
        elem: "#startTime2",
        format: "YYYY-MM-DD  hh:mm:ss",
        istime: true,
        istoday: false,
        issure: true,
    });
    laydate({
        elem: "#endTime2",
        format: "YYYY-MM-DD  hh:mm:ss",
        istime: true,
        istoday: false,
        issure: true,
    });
});
var tabIndex0 = 2;
var tabIndex1 = 2;
var blockNum = 0;
var realPageSize = 20;
var historyPageSize = 100000;
var currentPage = [$(".currentPage").eq(0).html(), $(".currentPage").eq(1).html()];
var pageIndex = [currentPage[0], currentPage[1]];
var totalPage = [$(".totalPage").eq(0).html(), $(".totalPage").eq(1).html()];
var titleArr = ['水箱信息表格', '液位历史曲线图', '液位实时曲线图'];
function changeChartBlock() {
    $(".title-box .chartIcos").click(function () {
        var index = $(this).index();

        var tabNum = $(this).parents(".block-box").attr("data-boxnum");
        var waterJzId = $(this).parents(".block-box").attr("data-jzId");
        //chart-header

        var startTime = $(".block-box[data-boxnum=" + tabNum + "]").find(".startTime").val();
        var endTime = $(".block-box[data-boxnum=" + tabNum + "]").find(".endTime").val();
        if (tabNum == 0) {
            tabIndex0 = index;
        } else if (tabNum == 1) {
            tabIndex1 = index;
        }
        $(this).addClass("active").siblings().removeClass("active");
        $(this).parent(".title-box").next().children().addClass("hide").eq(index).removeClass("hide");
        $(this).parents(".block-box").find(".titleText").html(titleArr[index]);
        if (index == 0 || index == 1) {
            $(".searchBox").eq(tabNum).removeClass("hide");
            $(".searchBtn").eq(tabNum).trigger("click");

        } else if (index == 2) {
            console.log('show realChart');
            $(".searchBox").eq(tabNum).addClass("hide");

            //  getMainByPage(waterJzId, startTime, endTime, tabNum, 'realChart',1);
        }
        //layout();
        realChart1.resize();
        realChart2.resize();
        historyChart1.resize();
        historyChart2.resize();

    });
}

/*搜索*/
function searchClickFn() {
    $(".searchBtn").click(function () {
        blockNum = $(this).parents(".block-box").attr("data-boxnum");
        var waterJzId = $(this).parents(".block-box").attr("data-jzId");
        var startTime = $(".block-box[data-boxnum=" + blockNum + "]").find(".startTime").val();
        var endTime = $(".block-box[data-boxnum=" + blockNum + "]").find(".endTime").val();
        console.log(eval('tabIndex' + blockNum));
        var tabNum = eval('tabIndex' + blockNum);
        if (tabNum == 1) {
            getMainByPage(waterJzId, startTime, endTime, blockNum, 'historyChart', historyPageSize, 1);
        } else if (tabNum == 0) {
            getMainByPage(waterJzId, startTime, endTime, blockNum, 'table', 10, pageIndex[blockNum]);
        } else {
            return false;
        }
    });
}
/**/
var realChart1 = null;
var realChart2 = null;
var historyChart1 = null;
var historyChart2 = null;
function initChartFn() {
    realChart1 = echarts.init($('.realChartBox')[0]);
    realChart2 = echarts.init($('.realChartBox')[1]);
    historyChart1 = echarts.init($('.historyChartBox')[0]);
    historyChart2 = echarts.init($('.historyChartBox')[1]);
    var option1 = {
        tooltip: {
            trigger: 'axis',
        },
        grid: {
            left: '3%',
            right: '30px',
            bottom: '3%',
            containLabel: true
        },
        legend: {
            data: ['水箱液位']
        },
        xAxis: [
	        {
	            type: 'category',
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
	                textStyle: {
	                    color: '#c6c6c6'
	                }
	            }
	        }
        ],
        yAxis: [
	        {
	            type: 'value',
	            name: '液位(m)',
	            min: 0,
                max:5,
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
            name: '水箱液位',
            type: 'line',
            label: {
                normal: {
                    show: true,
                    position: 'top',
                }
            },
            //  symbol:'circle',
            symbolSize: 6,
            showAllSymbol: true,
            itemStyle: {
                normal: {
                    color: '#55ccfd',
                    lineStyle: { color: '#55ccfd' }
                },
            },
            data: []
        }]

    };
    realChart1.setOption(option1);
    realChart2.setOption(option1);
    historyChart1.setOption(option1);
    historyChart2.setOption(option1);
    realChart2.setOption({
        series: [
       {
           name: '水箱液位',
           type: 'line',
           symbol: 'circle',
           symbolSize: 10,
           showAllSymbol: true,
           itemStyle: {
               normal: {
                   color: '#40c139',
                   lineStyle: { color: '#40c139' }
               },
           },
           data: []
       }]
    });
    historyChart2.setOption({
        series: [
       {
           name: '水箱液位',
           type: 'line',
           symbol: 'circle',
           symbolSize: 10,
           showAllSymbol: true,
           itemStyle: {
               normal: {
                   color: '#40c139',
                   lineStyle: { color: '#40c139' }
               },
           },
           data: [1, 2, 4, 5, 6, 3, 5, 6, 4, 3, 2]
       }]
    })
}
function layout() {

    var blockW = $(".block-box").width();
    //chartLog-box
    var sW = $(".schematic-box").width();
    $(".chartLog-box").width(blockW - sW - 20);
    realChart1.resize();
    realChart2.resize();
    historyChart1.resize();
    historyChart2.resize();

}


/*获取水箱液位*/
function getWaterInfo(waterId, pageSize) {
    $.ajax({
        url: '/ShuiChang/FY_Data_ShuiChang_MAIN',
        data: {
            waterID: waterId
        },
        dataType: 'JSON',
        success: function (data) {
            console.log(data);
            console.log('水箱液位信息');
            console.log(data.obj[0].waterJZ);
            dealWaterInfo(data.obj[0].waterJZ);
            var startTime = formatDate1(0) + " 00:00:00";
            var endTime = formatDate1(1) + " 00:00:00";
            var waterJzId0 = $(".block-box").eq(0).attr("data-jzId");
            var waterJzId1 = $(".block-box").eq(1).attr("data-jzId");

            getMainByPage(waterJzId0, startTime, endTime, 0, 'realChart', pageSize, 1);
        //    getMainByPage(waterJzId1, startTime, endTime, 1, 'realChart', pageSize, 1);
        },
        error: function (data) {
            console.log('错误：' + data.responseText);
        }
    });
}

function dealWaterInfo(data) {
    for (var i in data) {
        //header-title
        console.log(i);
        var curObj = $(".block-box[data-boxnum=" + (i * 1) + "]");
        curObj.find(".header-title").html(data[i].FName);
        curObj.attr("data-jzId", data[i].waterJZID);
        if (data[i].D_Data.length > 0) {
            curObj.find(".header-title").append('(' + data[i].D_Data[0].FDTUCode + ')');
            curObj.find(".acquisitionTime").children().html(DateTostring(data[i].D_Data[0].TempTime));
            if (data[i].D_Data[0].F40004) {
                curObj.find(".waterHeight").html(data[i].D_Data[0].F40004 + 'm');
            } else {
                curObj.find(".waterHeight").html('');
            }

            if (data[i].D_Data[0].F40001 == 0) {
                curObj.find(".lowAlarm").show();
            } else {
                curObj.find(".lowAlarm").hide();
            }
            if (data[i].D_Data[0].F40002 == 0) {
                curObj.find(".highAlarm").show();
            } else {
                curObj.find(".highAlarm").hide();
            }
        }
    }
}

/*function getChartsData(waterJzId, startTime, endTime, blockNum, chartType) {
    $.ajax({
        url: '/ShuiChang/FY_Data_ShuiChangByPage',
        data: {
            "waterJZID": waterJzId,
            "pageIndex": 1, //pageIndex[blockNum],
            "PageSize": 10,
            "StartTime": startTime,
            "EndTime": endTime
        },
        dataType: 'JSON',
        success: function (data) {
            console.log(data);
            console.log('chartsData');
            console.log(blockNum);
            dealChartsData(data.FY_Data_ShuiChang, blockNum, chartType);
        },
        error: function (data) {
            console.log('错误：' + data.responseText);
        }
    });
}*/
var oriData = [{}, {}];
function dealChartsData(data, blockNum, chartType, pageSize) {
    var curDataJson = {
        "waterH": [],
        "xData": []
    };
    for (var i in data) {
        curDataJson.waterH.unshift(data[i].F40004);
        curDataJson.xData.unshift(DateTostring(data[i].TempTime));
    }
    if (pageSize == realPageSize && chartType == 'realChart') {
        console.log('pageSize==realPageSize');
        oriData[blockNum].waterH = curDataJson.waterH;
        oriData[blockNum].xData = curDataJson.xData;
    } else if (pageSize == 1 && chartType == 'realChart') {
        console.log('pageSize==1');
        if (!oriData[blockNum]["xData"].length > 0) {
            return false;
        }

        var oriLen = oriData[blockNum]["xData"].length;
        var oriTime = oriData[blockNum]["xData"][oriLen - 1];
        var curTime = curDataJson.xData[0];
        if (oriTime == curTime) {
            return false;
        } else {
            if (oriLen == realPageSize) {
                for (var key in oriData[blockNum]) {
                    oriData[blockNum][key].shift();
                    oriData[blockNum][key].push(curDataJson[key][0]);
                }
            } else if (oriLen < realPageSize) {
                for (var key in oriData[blockNum]) {
                    oriData[blockNum][key].push(curDataJson[key][0]);
                }
            }
            curDataJson = oriData[blockNum];
        }
    }
    if (chartType == 'historyChart') {
        var option = {
            xAxis: {
                data: curDataJson.xData
            },
            series: [
           {
               symbolSize: 0,
               data: curDataJson.waterH
           }]
        }
    } else if (chartType == 'realChart') {
        var option = {
            xAxis: {
                data: curDataJson.xData
            },
            series: [
           {
               data: curDataJson.waterH
           }]
        }
    }

    var chartStr = chartType + (blockNum * 1 + 1);
    eval(chartStr).setOption(option);

}


function dealRealChartData() {
    var waterH = [];
    var xData = [];
    var curDataJson = {};
    for (var i in data) {
        curDataJson.waterH.unshift(data.F40004);
        curDataJson.xData.unshift(data.TempTime);
    }
    if (pageSize == realPageSize && chartType == 2) {
        console.log('pageSize==realPageSize');
        oriDataJson[blockNum].waterH = curDataJson.waterH;
        oriDataJson[blockNum].xData = curDataJson.xData;
    } else if (pageSize == 1 && chartType == 2) {
        console.log('pageSize==1');
        if (!oriData[blockNum]["xData"]) {
            return false;
        }
        var oriLen = oriData[blockNum]["xData"].length;
        var oriTime = oriData["xData"][oriLen - 1];
        var curTime = xData[0];
        if (oriTime == curTime) {
            return false;
        } else {
            if (oriLen == realPageSize) {
                for (var key in oriData[blockNum]) {
                    oriData[blockNum][key].shift();
                    oriData[blockNum][key].push(curDataJson[key][0]);
                }
            } else if (oriLen < realPageSize) {
                for (var key in oriData[blockNum]) {
                    oriData[blockNum][key].push(curDataJson[key][0]);
                }
            }
            curDataJson = oriData[blockNum];
        }
    }

    var option = {
        xAxis: {
            data: curDataJson.xData
        },
        series: [
       {
           data: curDataJson.waterH
       }]
    }
    var chartStr = chartType + (blockNum * 1 + 1);
    eval(chartStr).setOption(option);
}

function getMainByPage(waterJzId, startTime, endTime, blockNum, chartType, pageSize, pageIndex) {
    $.ajax({
        url: '/ShuiChang/FY_Data_ShuiChangByPage',
        data: {
            "waterJZID": waterJzId,
            "pageIndex": pageIndex,
            "PageSize": pageSize,
            "StartTime": startTime,
            "EndTime": endTime
        },
        dataType: 'JSON',
        success: function (data) {
            console.log(data);
            console.log(chartType);
            console.log(blockNum);
            if (chartType == 'table') {
                console.log('table');
                dealPage(blockNum, data.total)
                dealTableData(data.FY_Data_ShuiChang);
                addScroll();
            } else {
                dealChartsData(data.FY_Data_ShuiChang, blockNum, chartType, pageSize);
            }


        },
        error: function (data) {
            console.log('错误：' + data.responseText);
        }
    });
}

function dealPage(blockNum, total) {
    totalPage[blockNum] = Math.ceil(total / 10);
    $(".totalNum").eq(blockNum).html(total);
    $(".totalPage").eq(blockNum).html(totalPage[blockNum]);
}

function dealTableData(data) {
    var tempStr = '';
    var alarmStr = ['报警', '正常'];
    for (var i in data) {
        tempStr += '<tr>\
            <td class="hide">' + data[i].BaseID + '</td>\
            <td>' + alarmStr[data[i].F40001] + '</td>\
            <td>' + alarmStr[data[i].F40002] + '</td>\
            <td>' + (data[i].F40004 == null ? '' : data[i].F40004) + '</td>\
            <td>' + DateTostring(data[i].TempTime) + '</td>\
            </tr>';
    }
    $(".block-box[data-boxnum=" + blockNum * 1 + "]").find(".table2 tbody").empty();
    $(".block-box[data-boxnum=" + blockNum * 1 + "]").find(".table2 tbody").append(tempStr);
    $(".block-box[data-boxnum=" + blockNum * 1 + "]").find(".table1 tbody").empty();
    $(".block-box[data-boxnum=" + blockNum * 1 + "]").find(".table1 tbody").append(tempStr);
}
function tableClone() {
    var obj = $(".block-box[data-boxnum=" + blockNum * 1 + "]");

    obj.find('.table1 tbody').empty();
    obj.find('.table1 tbody').append(obj.find('.table2 tbody').clone());

};

function addScroll() {
    $('.table2_wrap').mCustomScrollbar({
        scrollButtons: {
            enable: true,
            scrollType: "continuous",
            scrollSpeed: 20,
            scrollAmount: 40
        },
        axis: "y",
        set_width: false,
        scrollbarPosition: "inside",
        theme: "minimal-dark",
        advanced: { autoExpandHorizontalScroll: true },
    });
    $('.mainWrap').mCustomScrollbar({
        scrollButtons: {
            enable: true,
            scrollType: "continuous",
            scrollSpeed: 20,
            scrollAmount: 40
        },
        axis: "y",
        set_width: false,
        scrollbarPosition: "inside",
        theme: "minimal-dark",
        advanced: { autoExpandHorizontalScroll: true },
    });
}

//分页
function turnPage() {
    //分页
    $(".page_box").on('click', 'a.nextPage', function () {

        blockNum = $(this).parents(".block-box").attr("data-boxnum");
        var startTime = $(".block-box[data-boxnum=" + blockNum + "]").find(".startTime").val();
        var endTime = $(".block-box[data-boxnum=" + blockNum + "]").find(".endTime").val();
        var waterJzId = $(".block-box[data-boxnum=" + blockNum + "]").attr("data-jzId");
        if (pageIndex[blockNum] == (totalPage[blockNum])) {
            alert('当前为最后一页');
        } else {
            pageIndex[blockNum]++;
            $(".block-box[data-boxnum=" + blockNum * 1 + "]").find(".currentPage").html(pageIndex[blockNum]);
            getMainByPage(waterJzId, startTime, endTime, blockNum, 'table', 10, pageIndex[blockNum]);
        }
    });
    $(".page_box").on('click', 'a.prevPage', function () {
        blockNum = $(this).parents(".block-box").attr("data-boxnum");
        var startTime = $(".block-box[data-boxnum=" + blockNum + "]").find(".startTime").val();
        var endTime = $(".block-box[data-boxnum=" + blockNum + "]").find(".endTime").val();
        var waterJzId = $(".block-box[data-boxnum=" + blockNum + "]").attr("data-jzId");
        if (!(pageIndex[blockNum] == 1)) {
            pageIndex[blockNum]--;
            $(".block-box[data-boxnum=" + blockNum * 1 + "]").find(".currentPage").html(pageIndex[blockNum]);
            getMainByPage(waterJzId, startTime, endTime, blockNum, 'table', 10, pageIndex[blockNum]);
        } else {
            alert('当前为第一页');
        }
    });
    $(".page_box").on('click', 'a.firstPage', function () {
        blockNum = $(this).parents(".block-box").attr("data-boxnum");
        var startTime = $(".block-box[data-boxnum=" + blockNum + "]").find(".startTime").val();
        var endTime = $(".block-box[data-boxnum=" + blockNum + "]").find(".endTime").val();
        var waterJzId = $(".block-box[data-boxnum=" + blockNum + "]").attr("data-jzId");
        if (!(pageIndex[blockNum] == 1)) {
            pageIndex[blockNum] = 1;
            $(".block-box[data-boxnum=" + blockNum * 1 + "]").find(".currentPage").html(pageIndex[blockNum]);
            getMainByPage(waterJzId, startTime, endTime, blockNum, 'table', 10, pageIndex[blockNum]);
        } else {
            alert('当前为第一页');
        }
    });
    $(".page_box").on('click', 'a.lastPage', function () {
        blockNum = $(this).parents(".block-box").attr("data-boxnum");
        console.log(blockNum);
        var startTime = $(".block-box[data-boxnum=" + blockNum + "]").find(".startTime").val();
        var endTime = $(".block-box[data-boxnum=" + blockNum + "]").find(".endTime").val();
        var waterJzId = $(".block-box[data-boxnum=" + blockNum + "]").attr("data-jzId");
        if (pageIndex[blockNum] == totalPage[blockNum]) {
            alert('当前为最后一页');
        } else {
            pageIndex[blockNum] = totalPage[blockNum];
            $(".block-box[data-boxnum=" + blockNum * 1 + "]").find(".currentPage").html(pageIndex[blockNum]);
            getMainByPage(waterJzId, startTime, endTime, blockNum, 'table', 10, pageIndex[blockNum]);
        }
    });
}
function alarmAniFn() {
    var Tw1 = new TimelineLite();
    Tw1.fromTo(".highAlarm", 1.5, { scaleX: 0.2, scaleY: 0.2 }, { scaleX: 1.5, scaleY: 1.5 }).to(".highAlarm", 0.5, { scaleX: 1, scaleY: 1 });
}

/*日期格式化 ms to string*/
function DateTostring(dateStr) {
    var d = dateStr.replace(/[^0-9]/gi, "");
    d = new Date(d * 1);
    var dM = (d.getMonth() + 1).toString().replace(/^(\d)$/, '0$1');
    var dD = d.getDate().toString().replace(/^(\d)$/, '0$1');
    var dH = d.getHours().toString().replace(/^(\d)$/, '0$1');
    var dMin = d.getMinutes().toString().replace(/^(\d)$/, '0$1');
    var dMs = d.getSeconds().toString().replace(/^(\d)$/, '0$1');
    var dateTemp = d.getFullYear() + "-" + (dM) + "-" + dD + " " + dH + ":" + dMin + ":" + dMs;

    return dateTemp;
}
/*获取距离当前日期天数时间*/
function formatDate1(i) {
    var curD = new Date();
    var d = new Date(curD.getTime() + i * 24 * 60 * 60 * 1000);
    var dM = (d.getMonth() + 1).toString().replace(/^(\d)$/, '0$1');
    var dD = d.getDate().toString().replace(/^(\d)$/, '0$1');
    var dateTemp = d.getFullYear() + "-" + (dM) + "-" + dD;
    return dateTemp;
}