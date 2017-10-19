var JZname = "";
var jzID = "";
var pumpName = "";
var pumpID = "";
var year = '';
var month = '';
var startDate = formatDate(0) + " 00:00:00";
var endDate = formatDate(1) + " 00:00:00";
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
    $("#startTime").val(startDate);
    $("#endTime").val(endDate);
    //    $(".chartItem_title").html(chartTitle);
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
        getChartsData();
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
    format: "YYYY-MM-DD  hh:mm:ss",
    istime: true,
    istoday: false,
    issure: true,
});
laydate({
    elem: "#endTime",
    format: "YYYY-MM-DD  hh:mm:ss",
    istime: true,
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
    var dateTemp = d.getFullYear() + "-" + (dM) + "-" + dD;
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
var chart_waterInOutFlow = null;
var chart_waterBoxLevel = null;
function initRightChart() {

    chart_waterInOutPress = echarts.init($('#chart_waterInOutPress')[0]);
    chart_waterInOutFlow = echarts.init($('#chart_waterInOutFlow')[0]);
    chart_waterBoxLevel = echarts.init($('#chart_waterBoxLevel')[0]);
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
            data: ['进水压力', '出水压力']
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
            itemStyle: {
                normal: {
                    color: '#3398DB',
                    lineStyle: { color: '#3398DB' }
                },
            },
            data: []
        },
        {
            name: '出水压力设定',
            type: 'line',
            showSymbol: false,
            lineStyle: {
                normal: {
                    type: "dashed"
                }
            },
            itemStyle: {
                normal: {
                    color: '#3398DB',
                    lineStyle: { color: '#3398DB' }
                },
            },
            data: []
        }]
    };

    var option2 = {
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
            itemStyle: {
                normal: {
                    color: '#3398DB',
                    lineStyle: { color: '#3398DB' }
                },
            },
            data: []
        }]
    };
    var option3 = {
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
            data: ['液位']
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
	            name: '液位',
	            min: 0,
	            //  interval: 50,
	          
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
            name: '液位',
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
    chart_waterInOutFlow.setOption(option2);
    chart_waterBoxLevel.setOption(option3);

}

/*图表搜索按钮*/
function searchTF() {
    $(".chart_searchBtn").click(function () {
        startDate = $("#startTime").val();
        endDate = $('#endTime').val();
        JZname = $('.JZname').val();
        jzID = $(".JZname").attr("data-id");
        dataJson = {
            "pumpJZID": jzID,
            "timeType": 4,
            "pageIndex": 0,
            "pageSize":10000,
            "StartDate": startDate,
            "EndDate": endDate
        };
        getChartsData(); 
        
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
    console.log(dataJson);
    console.log(chartRequestUrl);
    $.ajax({
        url: chartRequestUrl,
        data: dataJson,
        dataType: 'JSON',
        success: function (data) {

            chart_waterInOutPress.resize();

        },
        error: function (data) {
            console.log('泵房数据获取出错');
        }
    });
}

function getWaterOutPress() {
    $.ajax({
        url: chartRequestOutUrl,
        data: dataJson,
        dataType: 'JSON',
        success: function (data) {
            console.log(data);
            $(".selectedWrap input").val("");
            console.log(data);
            console.log('eg出水流量');
            var result = data.obj;
            console.log(result);
            dealWaterOutdata(result);


            chart_waterInOutPress.resize();

        },
        error: function (data) {
            console.log('出水流量数据获取出错');
        }
    });
}

//InOutWat_Flow_YW_YL

function getChartsData() {
    chart_waterInOutFlow.showLoading();
    $.ajax({
        url: '/V_YCJK/InOutWat_Flow_YW_YL',
        data: dataJson,
        dataType: 'JSON',
        success: function (data) {
            console.log(data);
            $(".selectedWrap input").val("");
            console.log(data);
            console.log('eg出水流量');
            var result = data.obj;
            console.log(result);
            
            dealInPressDataFn(result.data);
            chart_waterInOutPress.resize();
            chart_waterInOutFlow.hideLoading();
        },
        error: function (data) {
            console.log('二供压力流量水箱液位');
        }
    });
}



function dealInPressDataFn(data) {
    var inPressData = [];
    var outPressData = [];
    var outSetPressData = [];
    var inFlowData = [];
    var outFlowData = [];
    var waterBoxLevelData = [];
    var xData = [];
    for (var i = 0; i < data.length; i++) {
        xData.unshift(data[i].day_time);
        inPressData.unshift(data[i].F41006);
        outPressData.unshift(data[i].F41007);
        outSetPressData.unshift(data[i].F41702);
        inFlowData.unshift(data[i].F41024);
        outFlowData.unshift(data[i].F41025);
        waterBoxLevelData.unshift(data[i].F41020);
    }
    console.log(inPressData);
    var option1 = {
        xAxis: {
            data: xData
        },
        series: [{
            data: inPressData
        },
        {
            data: outPressData
        },
        {
            data: outSetPressData
        }]
        
    };
    var option2 = {
        xAxis: {
            data: xData
        },
        series: [{
            data: inFlowData
        },
        {
            data: outFlowData
        }]
    };
    var option3 = {
        xAxis: {
            data: xData
        },
        series: [{
            data: waterBoxLevelData
        }]
    };
    chart_waterInOutPress.setOption(option1);
    chart_waterInOutFlow.setOption(option2);
    chart_waterBoxLevel.setOption(option3);
}
function dealWaterOutdata(data) {
    var xData = [];
    var series1Data = [];
    for (var i = 0; i < data.length; i++) {
        xData.push(data[i].T_Time);
        series1Data.push(data[i].result);
    }
    var option = chart_waterInOutPress.getOption();
    option.series[1].data = series1Data;
    option.xAxis.data = xData;

    console.log(option.xAxis.data);
    chart_waterInOutPress.setOption(option);
    chart_waterInOutPress.setOption({
        xAxis: {
            data: xData,
            axisLabel: {
                formatter: function (value) {

                    if (value) {
                        if (value.indexOf("-")) {
                            return value.split('-')[1] + '点';
                        } else {
                            return value;
                        }
                    } else {
                        return value;
                    }

                }
            },
        }
    });
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

