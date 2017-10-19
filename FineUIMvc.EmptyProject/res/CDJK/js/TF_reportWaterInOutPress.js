var TFname = "";
var baseId = "";
var startDate = formatDate(0) + " 00:00:00";
var endDate = formatDate(1) + " 00:00:00";
var urlJson = parseUrl();
/*var tablePageIndex = 0;
var tablePageSize = 20;
var tableSearchText = $("#tableSearchText").val();
var currentPage = $("#currentPage").html();
var tablePageIndex = currentPage - 1;
var totalPage = $("#totalPage").html();
$("#pageSize").html(tablePageSize);*/

urlJson["TFname"] = decodeURIComponent(urlJson["TFname"]);
console.log(urlJson["TFname"]);

/*ready*/
$(document).ready(function () {
    $("#startTime").val(startDate);
    $("#endTime").val(endDate);
    urlJson["TFname"] !== "undefined" ? $(".TFname").val(urlJson["TFname"]) : $(".TFname").val("");
    (urlJson["baseId"]== "undefined" || urlJson["baseId"] == "undefined#" )?$(".TFname").attr("data-id", ""): $(".TFname").attr("data-id", urlJson["baseId"]);
    TFname = $(".TFname").val();
    baseId = $(".TFname").attr("data-id");
    initRightChart();

    if (baseId !== "") {
        getChartsData(startDate, endDate);
        getChartsDayHour(startDate, endDate);
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
      console.log('url     '+url);
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
    var  chart_mainWrapH = $(".chart_mainWrap").height();
    var  header_wrapH = $(".header_wrap").height();
    $(".chart_wrap").css({ "height": chart_mainWrapH - header_wrapH -16});
}

var chart_waterInOutPress = null;
var chart_tfWaterFlow = null;

var chart_tfDayHour = null;

var chart_elecUse = null;
var chart_pumpRunTime = null;
//var chart_unusual = null;
function initRightChart() {

    chart_waterInOutPress = echarts.init($('#chart_waterInOutPress')[0]);
    chart_tfWaterFlow = echarts.init($('#chart_tfWaterFlow')[0]);
 
    chart_tfDayHour = echarts.init($('#chart_tfDayHour')[0]);

    chart_elecUse = echarts.init($('#chart_elecUse')[0]);
    chart_pumpRunTime = echarts.init($('#chart_pumpRunTime')[0]);
  //  chart_unusual = echarts.init($('#chart_unusual')[0]);
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
            name: '',
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
            name: '',
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
            data: ['水箱容积', '储水总量', '调峰水量', '日调峰量']
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
	            name: '水量(m³)',
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
            name: '水箱容积',
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
            name: '储水总量',
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
            name: '调峰水量',
            type: 'line',
            itemStyle: {
                normal: {
                    color: '#c2a1dc',
                    lineStyle: { color: '#c2a1dc' }
                },
            },
            data: []
        },
        {
            name: '日调峰量',
            type: 'line',
            itemStyle: {
                normal: {
                    color: '#62e0df',
                    lineStyle: { color: '#62e0df' }
                },
            },
            data: []
        }]
    };

    var option3 = {
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
                name: '异常',
                type: 'line',
                itemStyle: {
                    normal: {
                        color: '#c2a1dc',
                        lineStyle: { color: '#c2a1dc' }
                    },
                },
                data: [5,5,7,8,9,9,2,5,6,8,9,0]
            }
        ]
    };

    var option4 = {
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
            data: ['进水流量', '供水流量','二级流量','收费流量','漏损量']
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
            name: '进水流量',
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
            name: '供水流量',
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
            name: '二级流量',
            type: 'line',
            itemStyle: {
                normal: {
                    color: '#c2a1dc ',
                    lineStyle: { color: '#c2a1dc' }
                },
            },
            data: [5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5]
        },
        {
            name: '收费流量',
            type: 'line',
            itemStyle: {
                normal: {
                    color: '#62e0df',
                    lineStyle: { color: '#62e0df' }
                },
            },
            data: [3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3]
        },
        {
            name: '漏损量',
            type: 'line',
            itemStyle: {
                normal: {
                    color: '#ec9d90',
                    lineStyle: { color: '#ec9d90' }
                },
            },
            data: [1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1]
        }]
    };
    var optionRT = {
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
            data: ['泵1', '泵2', '泵3']
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
	            name: '时间',
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
            name: '泵1',
            type: 'line',
            itemStyle: {
                normal: {
                    color: '#c2a1dc ',
                    lineStyle: { color: '#c2a1dc' }
                },
            },
            data: [11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11]
        },
        {
            name: '泵2',
            type: 'line',
            itemStyle: {
                normal: {
                    color: '#62e0df',
                    lineStyle: { color: '#62e0df' }
                },
            },
            data: [10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10]
        },
        {
            name: '泵3',
            type: 'line',
            itemStyle: {
                normal: {
                    color: '#ec9d90',
                    lineStyle: { color: '#ec9d90' }
                },
            },
            data: [12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12]
        }]
    };

    var optionEU = {
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
                name: '耗电量',
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
                name: '耗电量',
                type: 'line',
                itemStyle: {
                    normal: {
                        color: '#c2a1dc',
                        lineStyle: { color: '#c2a1dc' }
                    },
                },
                data: [3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3]
            }
        ]
    };
   
    chart_waterInOutPress.setOption(option1);
    chart_tfWaterFlow.setOption(option2);
    chart_tfDayHour.setOption(option4);

   
    chart_elecUse.setOption(optionEU);
    chart_pumpRunTime.setOption(optionRT);
   // chart_unusual.setOption(option3);
   
}

function getChartsData(startDate, endDate) {
 
    $.ajax({
        url: '/V_CDJK/SearchTF_HisReport',
        data: {
            "pageIndex": 0,
            "pageSize": 100000000,
            "Name": TFname,
            "StartDate": startDate,
            "EndDate": endDate,
            "ID":baseId
        },
        dataType: 'JSON',
        // beforeSend: loadingFunction,
        success: function (data) {
           
            console.log(data);
            console.log('进出水压力');
            var result = data.data;
            console.log(result);
            dealChartsData(result);
        },
        //   complete: loadingMiss,
        error: function (data) {
            console.log('错误：' + data.responseText);
        }
    });

}


function dealChartsData(result) {
    var series1Data = [];
    var series2Data = [];

    var tfWaterSeries1 = [];
    var tfWaterSeries2 = [];
    var tfWaterSeries3 = [];
    var tfWaterSeries4 = [];
    var waterLevelSeries = [];
    var tfAbilitySeries1 = [];
    var tfAbilitySeries2 = [];
    var xData = [];
    for (var i = 0; i < result.length; i++) {
        series1Data.unshift(result[i].F40001);
        series2Data.unshift(result[i].F40002);

        tfWaterSeries1.unshift(result[i].F40004);
        tfWaterSeries2.unshift(result[i].F40007);
        tfWaterSeries3.unshift(result[i].F40008);
        tfWaterSeries4.unshift(result[i].F40014);
        waterLevelSeries.unshift(result[i].F40013);

        tfAbilitySeries1.unshift(result[i].F40011 );
        tfAbilitySeries2.unshift(result[i].F40012*60);
        var tempTime = result[i].TempTime ? result[i].TempTime.replace("T", " ") : "";
        xData.unshift(tempTime);
    }

    var option = {
        legend: {
            data: ['进水压力', '出水压力']
        },
        xAxis: {
            data: xData
        },
        series: [{
            name:"进水压力",
            data: series1Data
        },
        {
            name: "出水压力",
            data: series2Data
        }]
    };
  
    var optionTF = {
        xAxis: {
            data: xData
        },
        series: [{
            data: tfWaterSeries1
        },
        {
            data: tfWaterSeries2
        },
        {
            data: tfWaterSeries3
        },
        {
            data: tfWaterSeries4
        }]
    };
  
  

    chart_waterInOutPress.setOption(option);
    chart_tfWaterFlow.setOption(optionTF);

}

/*日每时*/
function getChartsDayHour(startDate, endDate) {
    chart_tfDayHour.showLoading();
    $.ajax({
        url: '/V_CDJK/SearchTF_InLL',
        data: {
             "BASEID": baseId,
            "timeType": 4,
            "s_date": startDate,
            "e_date":endDate
        },
        dataType: 'JSON',
        // beforeSend: loadingFunction,
        success: function (data) {
            console.log(data);
            console.log('tf进水流量');
            var result = data.obj;
            console.log(result);
            dealWaterIndata(result);
           
            
        },
        //   complete: loadingMiss,
        error: function (data) {
            console.log('错误：' + data.responseText);
        }
    });
  
   
}
function getWaterOutPress() {
    $.ajax({
        url: '/V_CDJK/SearchTF_OutLL',
        data: {
            "BASEID": baseId,
            "timeType": 4,
            "s_date": startDate,
            "e_date": endDate
        },
        dataType: 'JSON',
        // beforeSend: loadingFunction,
        success: function (data) {
            chart_tfDayHour.hideLoading();
            $(".selectedWrap input").val("");
            console.log(data);
            console.log('tf出水流量');
            var result = data.obj;
            console.log(result);
            dealWaterOutdata(result);
        },
        //   complete: loadingMiss,
        error: function (data) {
            console.log('错误：' + data.responseText);
        }
    });

}

function dealWaterIndata(data,name) {
    var series1Data = [];
    var xData = [];
    for (var i = 0; i < data.length; i++) {
        xData.push(data[i].T_Time);
        series1Data.push(data[i].result);
    }
    var option = chart_tfDayHour.getOption();
   option.series[0].data = series1Data;
   chart_tfDayHour.setOption(option);
   getWaterOutPress();
}
function dealWaterOutdata(data, name) {
    var xData = [];
    var series1Data = [];
    for (var i = 0; i < data.length; i++) {
        xData.push(data[i].T_Time);
        series1Data.push(data[i].result);
    }
    var option = chart_tfDayHour.getOption();
    option.series[1].data = series1Data;
    option.xAxis.data = xData;

    console.log(option.xAxis.data);
    chart_tfDayHour.setOption(option);
    chart_tfDayHour.setOption({
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
    chart_tfDayHour.resize();
}

function setElecUseChart() {
  
} 


/*图表搜索按钮*/
function searchTF() {
    $(".chart_searchBtn").click(function () {
      
        startDate = $("#startTime").val();
        endDate = $('#endTime').val();
        TFname = $('.TFname').val();
        baseId = $(".TFname").attr("data-id");
        getChartsData(startDate, endDate);
        getChartsDayHour(startDate, endDate);
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
            content: ['/YCJK/V_CDJK/SelectTFname','no'],
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
      getChartsData(startDate, endDate);
      getChartsDayHour(startDate, endDate);
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
                  startDate = formatDate(0)+' 00:00:00';
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