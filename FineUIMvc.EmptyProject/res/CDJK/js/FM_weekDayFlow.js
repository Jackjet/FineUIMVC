var TFname = "";
var baseId = "";
var startDate = formatDate(0) + " 00:00:00";
var endDate = formatDate(7) + " 00:00:00";
var end30Date = formatDate(0)+" 23:59:59";
var urlJson = parseUrl();
var dayDataJson = {
    "BASEID": baseId,
    "timeType": 4,
    "s_date": startDate,
    "e_date": endDate
};

var day30Json = {
    "BASEID": baseId,
    "timeType": 5,
    "s_date": startDate,
    "e_date": end30Date
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
    day30Json = {
        "BASEID": baseId,
        "timeType": 5,
        "s_date": startDate,
        "e_date": end30Date
    };
    if (baseId !== "") {
        console.log('阀门周每日流量报表');
   
        loadFlowChart(dayDataJson);
        load30DayFlowChart();
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


function formatDate(i, date) {
    if (date) {
        var curD = new Date(date);
    } else {
        var curD = new Date();
    }
   
    var d = new Date(curD.getTime() + i * 24 * 60 * 60 * 1000);
    var dM = (d.getMonth() + 1).toString().replace(/^(\d)$/, '0$1');
    var dD = d.getDate().toString().replace(/^(\d)$/, '0$1');
    var dateTemp = d.getFullYear() + "-" + (dM) + "-" + dD;
    return dateTemp;
}

function getHistoryWeek(i) {
    var d = new Date();
    var nowYear = d.getFullYear();
    var nowMonth = d.getMonth();
    var nowDay = d.getDate();
    var nowDayofWeek = d.getDay();
    var getUpweekStartDate = new Date(nowYear, nowMonth, nowDay - nowDayofWeek - i * 7+1);
    var formatDay = formatDate(0, getUpweekStartDate);
    return formatDay;
}
function getBackNumDate(date, dayNum) {
   
    var curDateMs = new Date().getTime();
    var d = new Date(Date.parse(date));
    d.setDate(d.getDate() + dayNum);
    var dataMs = d.getTime();
    var dM = (d.getMonth() + 1).toString().replace(/^(\d)$/, '0$1');
    var dD = d.getDate().toString().replace(/^(\d)$/, '0$1');
    var dateTemp = d.getFullYear() + "-" + (dM) + "-" + dD;
    if ((dayNum == 29) && (dataMs > curDateMs)) {
        dateTemp = formatDate(0);
    }
    return dateTemp;
}

function layout() {
    var chart_mainWrapH = $(".chart_mainWrap").height();
    var header_wrapH = $(".header_wrap").height();
    $(".chart_wrap").css({ "height": chart_mainWrapH - header_wrapH - 16 });
}

var chart_weekDayFlow = null;
var chart_30DayFlow = null;
function initRightChart() {
    chart_weekDayFlow = echarts.init($('#chart_weekDayFlow')[0]); 
    chart_30DayFlow = echarts.init($('#chart_30DayFlow')[0]);
    var option1 = {
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
                name: '1',
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
                 name: '2',
                 type: 'line',
                 itemStyle: {
                     normal: {
                         color: '#dc62da',
                         lineStyle: { color: '#dc62da' }
                     },
                 },
                 data: []
             },
              {
                  name: '3',
                  type: 'line',
                  itemStyle: {
                      normal: {
                          color: '#56ccc5',
                          lineStyle: { color: '#56ccc5' }
                      },
                  },
                  data: []
              },
         {
             name: '4',
             type: 'line',
             itemStyle: {
                 normal: {
                     color: '#e2da23',
                     lineStyle: { color: '#e2da23' }
                 },
             },
             data: []
         },
          {
              name: '5',
              type: 'line',
              itemStyle: {
                  normal: {
                      color: '#de7813',
                      lineStyle: { color: '#de7813' }
                  },
              },
              data: []
          },
           {
               name: '6',
               type: 'line',
               itemStyle: {
                   normal: {
                       color: '#f52658',
                       lineStyle: { color: '#f52658' }
                   },
               },
               data: []
           },
            {
                name: '7',
                type: 'line',
                itemStyle: {
                    normal: {
                        color: '#6974e0',
                        lineStyle: { color: '#6974e0' }
                    },
                },
                data: []
            }
        ]
    };

    var option2= {
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
    chart_weekDayFlow.setOption(option1);
    chart_30DayFlow.setOption(option2);
}

/*一周日每时*/
function loadFlowChart(dataJson) {
    chart_weekDayFlow.showLoading();
    $.ajax({
        url: '/V_CDJK/SearchFM_LL',
        data: dataJson,
        dataType: 'JSON',
        //     beforeSend: loadingFunction,
        success: function (data) {
            chart_weekDayFlow.hideLoading();
            console.log(data);
            var data = data.obj;
            if (data.length < 168) {
                return;
            }
            var legData = [];
            var xData = [];
           
            var yData = new Array();

        /*    for (var i = 0; i < data.length; i++) {
                yData.push(data[i].result);
                xData.push(data[i].T_Time);
            }*/
            for (var i = 0; i < 7; i++) {
                yData[i] = new Array();
                for (var j = 0; j < 24; j++) {
                    xData[j] = j;
                    yData[i][j] = data[i * 24 + j].result;
                    if (j == 0) {
                        legData.push(data[i * 24 + j].T_Time.split('-')[0]);
                    }
                }
            }
            console.log(legData);
            console.log('周每日');
            var option = {
                legend:{
                    data:legData},
                xAxis: {
                    data:xData
                },
                series: [{
                    name:legData[0],
                    data: yData[0],
                }, {
                    name: legData[1],
                    data: yData[1],
                }, {
                    name: legData[2],
                    data: yData[2],
                }, {
                    name: legData[3],
                    data: yData[3],
                }, {
                    name: legData[4],
                    data: yData[4],
                }, {
                    name: legData[5],
                    data: yData[5],
                }, {
                    name: legData[6],
                    data: yData[6],
                }]
            };
            chart_weekDayFlow.setOption(option);
        },
        //   complete: loadingMiss,
        error: function (data) {
            console.log('错误：' + data.responseText);
        }
    });

}
function load30DayFlowChart() {
    chart_30DayFlow.showLoading();
    $.ajax({
        url: '/V_CDJK/SearchFM_LL',
        data: day30Json,
        dataType: 'JSON',
        //     beforeSend: loadingFunction,
        success: function (data) {
            chart_30DayFlow.hideLoading();
            console.log(data);
            console.log('最近30天阀门流量');
            var data = data.obj;
      
            var xData = [];

            var yData = [];
            for (var i = 0; i < data.length; i++) {
                xData.push(data[i].T_Time);
                yData.push(data[i].result);
            }
   
            console.log('周每日');
            var option = {
                xAxis: {
                    data: xData
                },
                series: [{
                    data: yData,
                }]
            };
            chart_30DayFlow.setOption(option);
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
        endDate = getBackNumDate(startDate, 7);
        end30Date = getBackNumDate(startDate, 29);
        TFname = $('.TFname').val();
        baseId = $(".TFname").attr("data-id");
        
        dayDataJson = {
            "BASEID": baseId,
            "timeType": 4,
            "s_date": startDate,
            "e_date": endDate
        };
        day30Json = {
            "BASEID": baseId,
            "timeType": 5,
            "s_date": startDate,
            "e_date": end30Date
        };
       
        loadFlowChart(dayDataJson, 'dayHour');
        load30DayFlowChart();
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
    startDate = $("#startTime").val();
    endDate = getBackNumDate(startDate, 7);
    end30Date = getBackNumDate(startDate, 29);
    dayDataJson = {
        "BASEID": baseId,
        "timeType": 4,
        "s_date": startDate,
        "e_date": endDate
    };
    day30Json = {
        "BASEID": baseId,
        "timeType": 5,
        "s_date": startDate,
        "e_date": end30Date
    };
    loadFlowChart(dayDataJson);
    load30DayFlowChart();
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
        var index = $(this).index();
        startDate = getHistoryWeek(index) + ' 00:00:00';
        endDate = getHistoryWeek(index-1) + ' 00:00:00';
        
        $("#startTime").val(startDate);
        $("#endTime").val(endDate);
        $(".selectedWrap input").val($(this).children().html());
        $(this).parent().addClass("hide");

    });
    $(".chart_mainWrap").click(function () {

        $(".selectMenu").addClass("hide");
    });
}