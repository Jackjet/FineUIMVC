var JZname = "";
var jzID = "";
var pumpName = "";
var pumpID = "";
var year = '';
var month = '';
var startDate = formatDate(0);
var endDate = formatDate(1);
var urlJson = parseUrl();
var pageSize = 1000;
var chartTitle = decodeURIComponent(urlJson["chartTitle"]);


urlJson["jzName"] = decodeURIComponent(urlJson["jzName"]);
urlJson["pumpName"] = decodeURIComponent(urlJson["pumpName"]);
console.log(urlJson["jzName"]);

var chartTypeJson = {
    chartType: "",
    chartTime: ""
};
var dataJson = {
    "BASEID": jzID,
    "year": year,
    "month": month,
    "timeType": 2
};
var seriesName = {
    "elec": "电量",
    "flow": "流量"
};
var chartRequestUrl = '/V_YCJK/SearchBF_LL';
function dealDataKey() {
    if (urlJson["dataKey"]) {
        var chartTypeArr = urlJson["dataKey"].split("_");
        chartTypeJson.chartType = chartTypeArr[1];
        chartTypeJson.chartTime = chartTypeArr[0];
    }
}


/*ready*/
$(document).ready(function () {
    $("#startTime").val(startDate);
    $("#endTime").val(endDate);
    $(".chartItem_title").html(chartTitle);
    setPumpName(urlJson["pumpName"], urlJson["pumpID"]);
    setJzName(urlJson["jzName"], urlJson["jzID"]);
    JZname = $(".JZname").val();
    jzID = $(".JZname").attr("data-id");
  
    pumpID = $(".pumpName").attr("data-id");

    dataJson = {
        "BASEID": jzID,
        "year": year,
        "month": month,
        "timeType": 2
    };
    dealDataKey();
    initRightChart();
    selectPumpAndJz();
    if (jzID !== "") {
        getInOutWaPress();
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
    elem: "#startTime",
    format: "YYYY-MM-DD",
    choose: function (datas) {
        var timeArr = datas.split("-");
        $("#startTime").val(timeArr[0] + '-' + timeArr[1]);
    }
});
laydate({
    elem: "#endTime",
    format: "YYYY-MM-DD",
    istime: false,
    istoday: false,
    issure: true,
});



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
    var dateTemp = d.getFullYear() + "-" + (dM);
    year = d.getFullYear();
    month = dM;
    return dateTemp;
}

function layout() {
    var chart_mainWrapH = $(".chart_mainWrap").height();
    var header_wrapH = $(".header_wrap").height();
    $(".chart_wrap").css({ "height": chart_mainWrapH - header_wrapH - 16 });
}

var chart_waterInOutPress = null;
function initRightChart() {

    chart_waterInOutPress = echarts.init($('#chart_waterInOutPress')[0]);

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
            data: ['流量']
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
	            name: '流量',
	            min: 0,
	            //  interval: 50,
	            axisLabel: {
	                formatter: '{value} m³'
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
            name: '流量',
            type: 'line',
            itemStyle: {
                normal: {
                    color: '#ed7450',
                    lineStyle: { color: '#ed7450' }
                },
            },
            data: []
        }]
    };
    chart_waterInOutPress.setOption(option1);

}

/*图表搜索按钮*/
function searchTF() {
    $(".chart_searchBtn").click(function () {
        startDate = $("#startTime").val();
        var startDateArr = startDate.split("-");
        year = startDateArr[0];
        month = startDateArr[1];
        JZname = $('.JZname').val();
        jzID = $(".JZname").attr("data-id");

        dataJson = {
            "BASEID": jzID,
            "year": year,
            "month": month,
            "timeType": 2
        };


        getInOutWaPress();
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



/*下拉列表*/
function dropdownMenu() {
    $(".selectedWrap input").val($(".selectMenu  li.active").children().html());
    $(".triangleWrap").click(function (event) {
        event.stopPropagation();
        $(".selectMenu").removeClass("hide");
    });

    $(".selectMenu li").click(function (event) {
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
        $("#startTime").val(startDate);
        $("#endTime").val(endDate);
        $(".selectedWrap input").val($(this).children().html());
        $(this).parent().addClass("hide");

    });
    $(".chart_mainWrap").click(function () {

        $(".selectMenu").addClass("hide");
    });
}


function getInOutWaPress() {

    $.ajax({
        url: chartRequestUrl,
        data: dataJson,
        dataType: 'JSON',
        success: function (data) {
            console.log(data);

            if (chartTypeJson.chartType == "press") {
                dealInPressDataFn(data.obj);
            } else {
                console.log(data);
                dealDataFn(data.obj);
            }
            chart_waterInOutPress.resize();

        },
        error: function (data) {
            console.log('泵房数据获取出错');
        }
    });
}

function dealDataFn(dealData) {
    var curDataJson = {
        "yData": [],
        "xAxisData": []
    };
    for (var i = 0; i < dealData.length; i++) {
        curDataJson.yData.push(dealData[i].result);
        curDataJson.xAxisData.push(dealData[i].day_time);
    }
    var option = {

        xAxis: [
	        {
	            data: curDataJson.xAxisData,
	        }
        ],
        series: [
	        {
	            name: '流量',
	            data: curDataJson.yData
	        }]
    };
    chart_waterInOutPress.setOption(option);

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

