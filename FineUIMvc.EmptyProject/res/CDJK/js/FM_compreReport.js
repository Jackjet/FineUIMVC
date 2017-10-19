var TFname = "";
var baseId = "";
var startDate = formatDate(0) + " 00:00:00";
var endDate = formatDate(1) + " 00:00:00";
var urlJson = parseUrl();
var dayDataJson = {
    "BASEID": baseId,
    "timeType": 4,
    "s_date": startDate,
    "e_date": endDate
};


urlJson["TFname"] = decodeURIComponent(urlJson["TFname"]);
console.log(urlJson["TFname"]);

/*ready*/
$(document).ready(function () {
    $("#startTime").val(startDate);
    $("#endTime").val(endDate);
    urlJson["TFname"] !== "undefined" ? $(".TFname").val(urlJson["TFname"]) : $(".TFname").val("");
    (urlJson["baseId"] == "undefined" || urlJson["baseId"] == "undefined#") ? $(".TFname").attr("data-id", "") : $(".TFname").attr("data-id", urlJson["baseId"]);
    TFname = $(".TFname").val();
    baseId = $(".TFname").attr("data-id");
    initRightChart();
    dayDataJson = {
        "BASEID": baseId,
        "timeType": 4,
        "s_date": startDate,
        "e_date": endDate
    };
    if (baseId !== "") {
        console.log('阀门综合报表');
        loadPressChart(startDate, endDate, baseId);
        loadFlowChart(dayDataJson);
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
    return dateTemp;
}

function layout() {
    var chart_mainWrapH = $(".chart_mainWrap").height();
    var header_wrapH = $(".header_wrap").height();
    $(".chart_wrap").css({ "height": chart_mainWrapH - header_wrapH - 16 });
}

var chart_waterInOutPress = null;
var chart_fmRealFlow = null;

var chart_fmRangeFlow = null;

function initRightChart() {
    //chart_fmRealFlow chart_fmRangeFlow
    chart_waterInOutPress = echarts.init($('#chart_waterInOutPress')[0]);
    chart_fmRealFlow = echarts.init($('#chart_fmRealFlow')[0]);
    chart_fmRangeFlow = echarts.init($('#chart_fmRangeFlow')[0]);
 
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
            data: ['进水压力', '出水压力', '压力设定']
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
            data: [1, 2, 2, 3, 4, 5, 5, 6, 7, 7]
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
            data: [1, 2, 2, 3, 4, 5, 5, 6, 7, 7]
        },
        {
            name: '压力设定',
            type: 'line',
            symbolSize: 0,
            itemStyle: {
                normal: {
                    color: '#3398DB',
                    lineStyle: { color: '#3398DB' }
                },
            },
            lineStyle: {
                normal: {
                    type: 'dashed'
                },
            },
            data: [1, 2, 2, 3, 4, 5, 5, 6, 7, 7]
        }]
    };

    var option2= {
        color: ['#3398DB'],
        tooltip: {
            trigger: 'axis',
        },
        grid: {
            left: '3%',
            right: '30px',
            bottom: '3%',
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
                    onZero: true,
                    lineStyle: {
                        color: "#dddddd"
                    }
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
                        color: '#3398DB',
                        lineStyle: { color: '#3398DB' }
                    },
                },
                data: []
            }
        ]
    };

    chart_waterInOutPress.setOption(option1);
    chart_fmRealFlow.setOption(option2);
    chart_fmRangeFlow.setOption(option2);

}



function loadPressChart(startDate, endDate, baseId) {
    $.ajax({
        url: '/V_CDJK/SearchFM_HisReport',//chart_fmRealFlow chart_fmRangeFlow
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
            console.log(data);
            console.log('实时阀门压力');
            var result = data.data;
            console.log(result);
            if (result) {
                var yData = [];
                var xData = [];
                var outPress = [];
                var setPress = [];
                var realFlow = [];
                for (var i = 0; i < result.length; i++) {
                    yData.unshift(result[i].F40001);
                    outPress.unshift(result[i].F40002);
                    setPress.unshift(result[i].F40003);
                    realFlow.unshift(result[i].F40005);
                    var tempTime = result[i].TempTime ? result[i].TempTime.replace("T", " ") : "";
                    xData.unshift(tempTime);
                }
                console.log(yData);
                var optionPress = {
                    xAxis: {
                        data: xData
                    },
                    series: [{
                        data: yData
                    },
                    {
                        data: outPress
                    },
                    {
                        data: setPress
                    }]
                };
                var optionRealFlow = {
                    xAxis: {
                        data: xData
                    },
                    series: [{
                        data: realFlow
                    }]
                };
                chart_waterInOutPress.setOption(optionPress);
                chart_fmRealFlow.setOption(optionRealFlow);
            }

        },
        //   complete: loadingMiss,
        error: function (data) {
            console.log('错误：' + data.responseText);
        }
    });

}


/*日每时*/
function loadFlowChart(dataJson) {
    $.ajax({
        url: '/V_CDJK/SearchFM_LL',
        data: dataJson,
        dataType: 'JSON',
        //     beforeSend: loadingFunction,
        success: function (data) {
            console.log(data);
            var data = data.obj;
           
            var xData = [];
            var yData = [];
            for (var i = 0; i < data.length; i++) {
                yData.push(data[i].result);
                xData.push(data[i].T_Time);
            }
            console.log(xData);
            var option = {
                xAxis: {
                    data: xData
                },
                series: [{
                    data: yData
                }]
            };
            chart_fmRangeFlow.setOption(option);
        },
        //   complete: loadingMiss,
        error: function (data) {
            console.log('错误：' + data.responseText);
        }
    });

}



/*图表搜索按钮*/
function searchTF() {
    $(".chart_searchBtn").click(function () {
        startDate = $("#startTime").val();
        endDate = $('#endTime').val();
        TFname = $('.TFname').val();
        baseId = $(".TFname").attr("data-id");
        dayDataJson = {
            "BASEID": baseId,
            "timeType": 4,
            "s_date": startDate,
            "e_date": endDate
        };
        loadPressChart(startDate, endDate, baseId);
        loadFlowChart(dayDataJson, 'dayHour');
    });
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
            content: ['/YCJK/V_CDJK/FM_selectFmName', 'no'],
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
    loadPressChart(startDate, endDate, baseId);
    loadFlowChart(dayDataJson);
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