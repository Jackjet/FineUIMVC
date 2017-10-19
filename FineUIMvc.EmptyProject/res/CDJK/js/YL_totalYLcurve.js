var TFname = "";
var baseId = "";
var dtuCodeArr = ['62E9A332-939A-4E28-B30B-F270D8CB314F', '03160926018', '03160926076', '03160926027', '03160926028', '03160926015', '03160926026'];
var cgJzId = '62e9a332-939a-4e28-b30b-f270d8cb314f';
var startDate = formatDate(0) + " 00:00:00";
var endDate = formatDate(1) + " 00:00:00";
var flagMs = new Date(startDate).getTime();
var urlJson = parseUrl();
var dataJson = {
    "pumpJZID": cgJzId,
    "timeType": 4,
    "pageIndex": 0,
    "pageSize": 10000,
    "StartDate": startDate,
    "EndDate": endDate
};
/*重固水厂dataId  '1d01a99f-e178-4e8c-8f45-f09c3c386392'*/

/*ready*/
$(document).ready(function () {
    $("#startTime").val(startDate);
    $("#endTime").val(endDate);
   
    initRightChart();
    
    for (var i = 0; i < dtuCodeArr.length; i++) {
        loadPressChart(i,startDate, endDate);
    }
    getFlowChartsData();
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


function  msToDate(value) {
    d = new Date(value);
    var dM = (d.getMonth() + 1).toString().replace(/^(\d)$/, '0$1');
    var dD = d.getDate().toString().replace(/^(\d)$/, '0$1');
    var dH = d.getHours().toString().replace(/^(\d)$/, '0$1');
    var dMin = d.getMinutes().toString().replace(/^(\d)$/, '0$1');
    var dMs = d.getMinutes().toString().replace(/^(\d)$/, '0$1');
    var dateTemp = d.getFullYear() + "-" + (dM) + "-" + dD + " " + dH + ":" + dMin + ":" + dMs;
    return dateTemp;
}
function initRightChart() {
    chart_waterInOutPress = echarts.init($('#chart_waterInOutPress')[0]);
    chart_fmRealFlow = echarts.init($('#chart_fmRealFlow')[0]);

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
            data: ["青浦重固水厂", "青浦重固压力点6", "青浦重固压力点5", "青浦重固压力点4", "青浦重固压力点2", "青浦重固压力点1", "青浦重固压力点3"]
        },
        xAxis: [
	        {
	            type: 'value',
	            axisPointer: {
	                type: 'shadow'
	            },
	            min:flagMs,
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
            name: '青浦重固水厂',
            type: 'line',
            symbolSize:0,
            itemStyle: {
                normal: {
                    color: '#ed7450',
                    lineStyle: { color: '#ed7450' }
                },
            },
            data: [1, 2, 2, 3, 4, 5, 5, 6, 7, 7]
        }, {
            name: '青浦重固压力点6',
            type: 'line',
            symbolSize: 0,
            itemStyle: {
                normal: {
                    color: '#dc62da',
                    lineStyle: { color: '#dc62da' }
                },
            },
            data: []
        },
              {
                  name: '青浦重固压力点5',
                  type: 'line',
                  symbolSize: 0,
                  itemStyle: {
                      normal: {
                          color: '#56ccc5',
                          lineStyle: { color: '#56ccc5' }
                      },
                  },
                  data: []
              },
         {
             name: '青浦重固压力点4',
             type: 'line',
             symbolSize: 0,
             itemStyle: {
                 normal: {
                     color: '#e2da23',
                     lineStyle: { color: '#e2da23' }
                 },
             },
             data: []
         },
          {
              name: '青浦重固压力点2',
              type: 'line',
              symbolSize: 0,
              itemStyle: {
                  normal: {
                      color: '#de7813',
                      lineStyle: { color: '#de7813' }
                  },
              },
              data: []
          },
           {
               name: '青浦重固压力点1',
               type: 'line',
               symbolSize: 0,
               itemStyle: {
                   normal: {
                       color: '#f52658',
                       lineStyle: { color: '#f52658' }
                   },
               },
               data: []
           },
            {
                name: '青浦重固压力点3',
                type: 'line',
                symbolSize: 0,
                itemStyle: {
                    normal: {
                        color: '#6974e0',
                        lineStyle: { color: '#6974e0' }
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
            show: false,
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

    chart_waterInOutPress.setOption(option1);
    chart_fmRealFlow.setOption(option2);
}

/*03170519236*/
var onLoading = true;
function loadPressChart(index,startDate, endDate) {
    if (onLoading&&index==0) {
        chart_waterInOutPress.showLoading();
        console.log(flagMs+'shijianshijian ');
        chart_waterInOutPress.setOption({ xAxis: [{ min: flagMs }] });
    }
   // chart_waterInOutPress.showLoading();
    $.ajax({
        url: '/V_CDJK/getYaPumpData',
        data: {
            "FDTUCode": dtuCodeArr[index],
            "StartDate": startDate,
            "EndDate": endDate
        },
        dataType: 'JSON',
        success: function (data) {
            console.log(index+'index');
            console.log(data);
            console.log('所有压力测点');
            var option = chart_waterInOutPress.getOption();
            option.series[index].data = dealYLdata(data);
            chart_waterInOutPress.setOption(option);
            if (index == dtuCodeArr.length-1) {
                chart_waterInOutPress.hideLoading();
                chart_waterInOutPress.setOption({
                    tooltip: {
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
                    }
                })
            }
            onLoading = false;
        },
        error: function (data) {
            console.log('错误：' + data.responseText);
        }
    });

}


function dealYLdata(data) {
    var itemArr = [];
    for(var i in data)
    {
        var tempArr = [];
        var timeMs =new Date(data[i].TempTime.replace('T', " ")).getTime();
        tempArr.push(timeMs);
        tempArr.push(data[i].FMpa);
        itemArr.push(tempArr);
    }
//    console.log(itemArr);
    return itemArr;
}

/*重固水厂进出水流量*/
function getFlowChartsData() {
    chart_fmRealFlow.showLoading();
    $.ajax({
        url: '/V_YCJK/InOutWat_Flow_YW_YL',
        data: dataJson,
        dataType: 'JSON',
        success: function (data) {
          //  console.log(data);
            console.log('eg出水流量');
            var result = data.obj;
         //   console.log(result);

            dealInPressDataFn(result.data);
            chart_fmRealFlow.resize();
            chart_fmRealFlow.hideLoading();
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
        inFlowData.unshift(data[i].F41024);
        outFlowData.unshift(data[i].F41025);
    }
    console.log(inPressData);
    
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
    chart_fmRealFlow.setOption(option2);
}



/*图表搜索按钮*/
function searchTF() {
    $(".chart_searchBtn").click(function () {
       
        $(".selectedWrap input").val('');
        onLoading = true;
        startDate = $("#startTime").val();
        endDate = $('#endTime').val();

        flagMs = new Date(startDate).getTime();
        dataJson.StartDate = startDate;
        dataJson.EndDate = endDate;
        for (var i = 0; i < dtuCodeArr.length; i++) {
            loadPressChart(i, startDate, endDate);
        }
        getFlowChartsData();
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