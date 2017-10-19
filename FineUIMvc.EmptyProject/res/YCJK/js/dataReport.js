
var dataReport_chart1 = echarts.init($('#dataReport_chart1')[0]);
var dataReport_chart3 = echarts.init($('#dataReport_chart3')[0]);
var dataReport_chart4 = echarts.init($('#dataReport_chart4')[0]);
var dataReport_chart5 = echarts.init($('#dataReport_chart5')[0]);
var timeType = 3;
var year="";
var month="";
var day = formatDate(0);
var startDate = formatDate(0) + ' 00:00:00';
var endDate = formatDate(1) + ' 00:00:00';

var urlJson = parseUrl();
var jzId = urlJson["jzID"];

var powerDataJson = {};
//console.log('jzID' + jzId);
$(function () {
    var pageSize = 10000;//进出水压力
    layout();
    setScroll();
    initCharts();
    seletTime();
    getFlowInOutPress(jzId, pageSize);
    getWaterUse();
    getElecUse();
    getPowerUse();
    getflowInfo();
    $(window).resize(function () {
        layout();
    });
});



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

function parseUrl() {
    var url = window.location.href;
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
function initCharts() {

       
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
                name: '进水压力',
                type: 'line',
                itemStyle: {
                    normal: {
                        color: '#1cd6d0',
                        lineStyle: { color: '#1cd6d0' }
                    },
                },
                data: []
            },
            {
                name: '出水压力',
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
                        color: '#ed7450',
                        lineStyle: { color: '#ed7450' }
                    },
                },
                data: []
            }]
        };




        var option2 = {
            legend: {
                top: '30',
                left:'30',
                data: ['前天', '昨天', '今天']
            },
            tooltip: {
                trigger: 'axis',
                axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                    type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                }
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
                    name: '前天',
                    type: 'line',
                    showAllSymbol: true,
                    itemStyle: {
                        normal: {
                            color: '#f0a5a5'
                        }
                    },
                    data: [40, 30, 35, 20, 40, 30, 35, 67, 79, ]
                },
                 {
                     name: '昨天',
                     type: 'line',
                     showAllSymbol: true,
                     itemStyle: {
                         normal: {
                             color: '#dc93f4'
                         }
                     },
                     data: [40, 30, 35, 67, 79, 40, 30, 35, 20 ]
                 },
                {
                    name: '今天',
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
                    data: [40, 30, 35, 67, 79,30, 35, 20, 40, ]
                }
            ]
        };
        var option3 = {
            color: ['#3398DB'],
            tooltip: {
                trigger: 'axis',
                axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                    type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                }
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
                   name: '',
                   type: 'line',
                   showAllSymbol: true,
                   itemStyle: {
                       normal: {
                           color: '#0eb6fd'
                       }
                   },
                   data: [40, 30, 35, 20, 40, 30, 35, 67, 79, ]
               },
                 {
                     name: '',
                     type: 'line',
                     showAllSymbol: true,
                     symbolSize:0,
                     itemStyle: {
                         normal: {
                             color: '#21c0f0'
                         }
                     },
                     lineStyle: {
                         normal: {
                             type: 'dashed'
                         }
                     },
                     data: [40, 40, 40, 40, 40, 40, 40, 40, 40]
                 }
            ]
        };
        dataReport_chart1.setOption(option1);
        dataReport_chart3.setOption(option2);
        dataReport_chart4.setOption(option2);
        dataReport_chart5.setOption(option3);
        dataReport_chart1.resize();
      
        dataReport_chart3.resize();
        dataReport_chart4.resize();
        dataReport_chart5.resize();
 
}


function setScroll() {
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
        theme: "minimal-dark",
    });
}

function layout() {
    $(".tWaterPowerUse_box").css({ "width": $(".tWaterPowerUse_wrap").width() - $(".powerUseList_box").width() - 30 });
    dataReport_chart1.resize();
   
    dataReport_chart3.resize();
    dataReport_chart4.resize();
    dataReport_chart5.resize();
}

/*进出水压力*/
function getFlowInOutPress(jzId, pageSize) {
    //console.log('pageSize:   ' + pageSize);
    /*InOutWat_Flow_YW_YL     InOutWatPa
    dataJson = {
        "pumpJZID": jzID,
        "timeType": 4,
        "pageIndex": 0,
        "pageSize": 10000,
        "StartDate": startDate,
        "EndDate": endDate
    };*/
    $.ajax({
        url: '/V_YCJK/InOutWat_Flow_YW_YL',
        data: {
            "pumpJZID": jzId,
            "timeType": 4,
            "pageIndex": 0,
            "pageSize": pageSize,
            "StartDate": startDate,
            "EndDate": endDate
        },
        success: function (data) {
            var dataJSON = JSON.parse(data);
            //console.log(dataJSON);
            dataJSON = dataJSON.obj;
            //console.log('实时进出水压力');
            dealFlowInOutPress(dataJSON.data);


        },
        error: function (data) {
            console.log('泵房数据获取出错');
        }
    });
}

function dealFlowInOutPress(pressData) {
    var curDataJson = {
        "InPressArr": [],
        "OutPressArr": [],
        "setOutPressArr": [],
        "xAxisData": []
    };
    for (var i = 0; i < pressData.length; i++) {
        curDataJson.InPressArr.unshift(pressData[i].F41006);
        curDataJson.OutPressArr.unshift(pressData[i].F41007);
        curDataJson.setOutPressArr.unshift(pressData[i].F41702);
        curDataJson.xAxisData.unshift(pressData[i].day_time);
    }
   
   
    var option = {
        xAxis: [
	        {
	            data: curDataJson.xAxisData,
	        }
        ],
        series: [
             {
                 name: '进水压力',
                 data: curDataJson.InPressArr
             },
            {
                name: '出水压力',
                data: curDataJson.OutPressArr
            },
            {
                name: '出水设定压力',
                data: curDataJson.setOutPressArr
            }
        ]
    };
    //console.log('aaaaa');
   dataReport_chart1.setOption(option);
  
}

//用水量
function getWaterUse() {
    $.ajax({
        url: '/V_YCJK/SL_PumpJZ_YMD',
        data: {
            "timeType": timeType,
            "pumpJZID": jzId
        },
        dataType: 'JSON',
        beforeSend: loadingFunction,
        success: function (data) {
            //console.log(typeof data);
            //console.log(data);
            //console.log('用水量图表数据');
            //console.log(data.obj.data);
            dealWaterUseData(data.obj.data);
        },
        complete: loadingMiss,
        error: function (data) {
            console.log('错误：' + data.responseText);
        }
    });

}

function dealWaterUseData(waterUseData) {
    var xData = [];
    var legData = [];
    var serisData = new Array();
   
    for (var i = 0; i < waterUseData.length; i++) {
        serisData[i] = new Array(0);
        legData.push(waterUseData[i].field);
        var tempArr = waterUseData[i].detailData;
        //console.log(tempArr);
        for (var j = 0; j < tempArr.length; j++) {
            xData.push(tempArr[j].T_Time);
            
            serisData[i].push(tempArr[j].result);
        }
    }
    //console.log(legData);
    //console.log('yyyyyy');
    //console.log(serisData);
    var option = {
        legend:{
            data: legData
        },
        xAxis: [
                {
                    data:xData
                }],
        series: [{
            name: legData[2],
            data:serisData[2]
        },
        {
            name: legData[1],
            data: serisData[1]
        },
        {
            name: legData[0],
            data: serisData[0]
        }
        ]
    };
    dataReport_chart3.setOption(option);
}

function getElecUse() {
       $.ajax({
           url: '/V_YCJK/DL_PumpJZ_YMD',
           data: {
               "timeType": timeType,
               "pumpJZID": jzId
           },
           dataType: 'JSON',
           beforeSend: loadingFunction,
           success: function (data) {
               //console.log(typeof data);
               //console.log(data);
               //console.log('用电量图表数据');
               //console.log(data.obj.data);
               dealElecUseData(data.obj.data);
           },
           complete: loadingMiss,
           error: function (data) {
               console.log('错误：' + data.responseText);
           }
       });
  
}

function dealElecUseData(elecUseData) {
    var xData = [];
    var legData = [];
    var serisData = new Array();

    for (var i = 0; i < elecUseData.length; i++) {
        serisData[i] = new Array(0);
        legData.push(elecUseData[i].field);
        var tempArr = elecUseData[i].detailData;
        //console.log(tempArr);
        for (var j = 0; j < tempArr.length; j++) {
            xData.push(tempArr[j].T_Time);
            
            serisData[i].push(tempArr[j].result);
        }
    }
    //console.log(legData);
    //console.log('yyyyyy');
    //console.log(serisData);
    var option = {
        legend: {
            data: legData
        },
        xAxis: [
                {
                    data: xData
                }],
        series: [{
            name: legData[2],
            data: serisData[2]
        },
        {
            name: legData[1],
            data: serisData[1]
        },
        {
            name: legData[0],
            itemStyle: {
                normal: {
                    color: new echarts.graphic.LinearGradient(
                        0, 0, 0, 1,
                        [
                            { offset: 0, color: '#f9e1ba' },
                            { offset: 0.5, color: '#fd8622' },
                            { offset: 1, color: '#fd6406' }
                        ]
                    )
                }
            },
            data: serisData[0]
        }
        ]
    };
    dataReport_chart4.setOption(option);
}


/*吨水能耗*/

function getPowerUse() {
    switch (timeType){
        case 1:
            powerDataJson = {
                "timeType": timeType,
                "pumpJZID": jzId,
                "year":year
            };
            break;
        case 2:
            powerDataJson = {
                "timeType": timeType,
                "pumpJZID": jzId,
                "year": year,
                "month": month
            };
            break;
        case 3:
            powerDataJson = {
                "timeType": timeType,
                "pumpJZID": jzId,
                "day":day
            };
            break;
        default: break;
    }

    $.ajax({
        url: '/V_YCJK/SearchDSNH',
        data:powerDataJson,
        dataType: 'JSON',
        beforeSend: loadingFunction,
        success: function (data) {
            //console.log(typeof data);
            //console.log(data);
            //console.log('吨水能耗');
            //console.log(data.obj);
            dealPowerUseData(data.obj);
        },
        complete: loadingMiss,
        error: function (data) {
            console.log('错误：' + data.responseText);
        }
    });
}

function dealPowerUseData(powerUseData) {
    var xData = [];
    var yData = [];
    var dashData = [];
    for (var i = 0; i < powerUseData.length; i++) {
        switch (timeType) {
            case 1:
                xData.push(powerUseData[i].T_yearMonth);
                break;
            case 2:
                xData.push(powerUseData[i].t_monthDay);
                break;
            case 3:
                xData.push(powerUseData[i].T_dayHour);
                break;
            default: break;
        }
        dashData.push(0.8);
        yData.push(powerUseData[i].dsnh);
    }
    var option = {
        xAxis: [{
            data:xData
        }],
        series: [{
            data:yData
        },
        {
            data:dashData
        }]
    };
    dataReport_chart5.setOption(option);
}

function getflowInfo() {
    $.ajax({
        url: '/V_YCJK/SearchDSNHResult',
        data: powerDataJson,
        dataType: 'JSON',
        beforeSend: loadingFunction,
        success: function (data) {
            //console.log(typeof data);
            //console.log(data);
            //console.log('右下角表格信息');
            dealflowInfo(data);
        },
        complete: loadingMiss,
        error: function (data) {
            console.log('错误：' + data.responseText);
        }
    });
}

function dealflowInfo(infoData) {
    var sAccuFlow = 0;
    var eAccuFlow =0;
    var pAccuFlow = 0;
    var sElecUse = 0;
    var eElecUse = 0;
    var pElecUse = 0;
    var dsnh = 0;
    if (infoData.length==2) {
         sAccuFlow = infoData[0].FTotalOutLL;
         eAccuFlow = infoData[1].FTotalOutLL;
         pAccuFlow = eAccuFlow - sAccuFlow;
         sElecUse = infoData[0].FTotalDL;
         eElecUse = infoData[1].FTotalDL;
         pElecUse = eElecUse - sElecUse;
         if (pAccuFlow) {
             dsnh = (pElecUse / pAccuFlow).toFixed(2);
         }
        
    }
   
    $(".sAccuFlow").html(sAccuFlow);
    $(".eAccuFlow").html(eAccuFlow);
    $(".pAccuFlow").html(pAccuFlow);
    $(".sElecUse").html(sElecUse);
    $(".eElecUse").html(eElecUse );
    $(".pElecUse").html(pElecUse);
    $(".dsnh").html(dsnh);
    $(".setDsnh").html(0.8);
    $(".averDsnh").html(0);
}

function loadingFunction() {
    var $div = $('<div class="loading" style="position:absolute;left: 50%;top:50%;margin-left: -150px;margin-top: -70px;width: 300px;color:black;text-align:center;line-height: 140px;height: 140px;background: rgba(255,255,0);color: white;border-radius: 8px;"><img style="position: relative;top: 56%;left: 15%;" src="/res/YCJK/img/load1.gif" alt="loading....">正在加载中...</div>');
    $('body').append($div);
};
function loadingMiss() {
    $('.loading').remove();
};

function seletTime() {
    $(".selectData a").click(function () {
        // var dataTime = $(this).attr("class");
        $(this).addClass("active").siblings().removeClass("active");
        var dataType = $(this).parent().attr("data-type");
       
        timeType = $(this).index() + 1;
        
        switch (dataType){
            case "waterUseData":
                  getWaterUse();
                break;
            case "elecUseData":
                getElecUse();
                break;
            case "powerUseData":
                getPowerUse();
                getflowInfo();
                break;
            default:
                break;
        }
    });
}