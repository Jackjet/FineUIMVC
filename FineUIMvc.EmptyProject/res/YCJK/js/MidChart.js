
var myChart1 = echarts.init($('#echart1')[0]);
var myChart2 = echarts.init($('#echart2')[0]);
var myChart3 = echarts.init($('#echart3')[0]);

var startDate = formatDate(0) + ' 00:00:00';
var endDate = formatDate(1) + ' 00:00:00';
function formatDate(i) {
    var curD = new Date();
    var d = new Date(curD.getTime() + i * 24 * 60 * 60 * 1000);
    var dM = (d.getMonth() + 1).toString().replace(/^(\d)$/, '0$1');
    var dD = d.getDate().toString().replace(/^(\d)$/, '0$1');
    var dateTemp = d.getFullYear() + "-" + (dM) + "-" + dD;
    return dateTemp;
}
function showChart() {
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
	            name: '进出水压力',
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
	            showAllSymbol: true,
	            itemStyle: {
	                normal: {
	                    color: '#2895f0',
	                    lineStyle: { color: '#2895f0' }
	                },
	            },
	            data: []
	        },
            {
                name: '出水压力',
                type: 'line',
                showAllSymbol: true,
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
        }
        ]
    };

    var option2 = {
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
                name: '用电量',
                type: 'bar',
                barWidth: '50%',
                itemStyle: {
                    normal: {
                        color: new echarts.graphic.LinearGradient(
                            0, 0, 0, 1,
                            [
                                { offset: 0, color: '#fdc959' },
                                { offset: 0.5, color: '#fd8723' },
                                { offset: 1, color: '#fd6407' }
                            ]
                        )
                    }
                },
                data: []
            }
        ]
    };
    myChart1.setOption(option1);
    myChart2.setOption(option2);
    myChart3.setOption(option3);
    myChart1.resize();
    myChart2.resize();
    myChart3.resize();
}

function radialBar() {
    $("#proBar1").radialIndicator({
        barColor: '#479ae6',
        barBgColor: "#a9cdee",
        barWidth: 4,
        initValue: 40,
        roundCorner: true,
        percentage: true
    });

    $("#proBar2").radialIndicator({
        barColor: '#fd6406',
        barBgColor: "#f8b68e",
        barWidth: 4,
        initValue: 40,
        roundCorner: true,
        percentage: true
    });
    //  var radialObj1 = $('#proBar1').data('radialIndicator');
    // var radialObj2 = $('#proBar2').data('radialIndicator');

}

function getWaterElec(jzId) {
    $.ajax({
        url: '/V_YCJK/dl_llTotal',
        data: {
            'pumpJZID': jzId
        },
        success: function (data) {
            var dataJSON = JSON.parse(data);
            dataJSON = dataJSON.obj;
            //console.log(dataJSON);
            if (dataJSON[0]) {
                dealWaterElecData(dataJSON[0]);
            }
            

        },
        error: function (data) {
            console.log('电量流量数据获取出错');
        }
    });
}


function dealWaterElecData(WEdata) {
   
    //.dataCircle_wrap
    for (var key in WEdata) {
        $('span[data-field=' + key + ']').html(WEdata[key]);
    }
    var radialObj1 = $('#proBar1').data('radialIndicator');
    var radialObj2 = $('#proBar2').data('radialIndicator');
    if (WEdata.ll_bf) {
        radialObj1.value(WEdata.ll_bf * 100);
    } else {
        radialObj1.value(0);
    }
    if (WEdata.dl_bf) {
        radialObj2.value(WEdata.dl_bf * 100);
    } else {
        radialObj2.value(0);
    }
   
    if (WEdata.ll_bf > 0) {
        $(".water_down").hide();
        $(".water_up").show();
    } else if (WEdata.ll_bf < 0) {
        $(".water_up").hide();
        $(".water_down").show();
    }
    if (WEdata.dl_bf > 0) {
        $(".elec_down").hide();
        $(".elec_up").show();
    } else if (WEdata.ll_bf < 0) {
        $(".elec_down").show();
        $(".elec_up").hide();
    }
}

//进出水压力
function getInOutWaPress(jzId, pageSize) {
    console.log('pageSize:   ' + pageSize); 
  
    //InOutWat_Flow_YW_YL
    $.ajax({
        url: '/V_YCJK/InOutWat_Flow_YW_YL?v=' + (new Date()).getTime(),
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
            dataJSON = dataJSON.obj;
            dealInOutWaPress(dataJSON.data, pageSize);
            myChart1.resize();
           

        },
        error: function (data) {
            console.log('进出水压力数据获取出错');
        }
    });
}

var oriData = {

};

function dealInOutWaPress(pressData, pageSize) {
    //alert(pageSize);
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
    if (pageSize == 10) {
        oriData = JSON.parse(JSON.stringify(curDataJson));
    } else if (pageSize == 1) {
        //console.log(oriData);
        //console.log(oriData["xAxisData"]);
        if (!oriData["xAxisData"]) {
            return false;
        }
       var oriLen=oriData["xAxisData"].length;
       var oriTime = oriData["xAxisData"][oriLen - 1];
      var curTime = curDataJson["xAxisData"][0];
      if (oriTime == curTime) {
            //console.log(curDataJson["xAxisData"][0]);
            //console.log(oriData["xAxisData"][length - 1]);
            //console.log('无新数据');
        } else {
            if (oriData.xAxisData.length == 10) {
                for (var key in oriData) {
                    oriData[key].shift();
                    oriData[key].push(curDataJson[key][0]);
                }
            } else if (oriData.xAxisData.length < 10) {
                for (var key in oriData) {
                    oriData[key].push(curDataJson[key][0]);
                }

            }

        }
    }

    var option = {
        xAxis: [
	        {
	            data: oriData.xAxisData,
	        }
        ],
        series: [
	        {
	            name: '进水压力',
	            data: oriData.InPressArr
	        },
            {
                name: '出水压力',
                data: oriData.OutPressArr
            },
            {
                name: '出水设定压力',
                data: oriData.setOutPressArr
            }
        ]
    };
    //console.log(1111111);
    //console.log(oriData);
    myChart1.setOption(option);
}


//近七日用水量对比
function get7daysWaterUse(jzId) {
    $.ajax({
        url: '/V_YCJK/UseWater7?v=' + (new Date()).getTime(),
        data: {
            'pumpJZID': jzId,
        },
        success: function (data) {
            //console.log(data);
            var dataJSON = JSON.parse(data);
            //console.log('近7日用水量');
            //   console.log(dataJSON);
            dataJSON = dataJSON.obj;
            dealWaterUseData(dataJSON);
            //      console.log(dataJSON);
            myChart2.resize();
            
        },
        error: function (data) {
            console.log('7日用水量数据获取出错');
        }
    });
}

function get7daysElecUse(jzId) {
    $.ajax({
        url: '/V_YCJK/UseElectric7?v=' + (new Date()).getTime(),
        data: {
            'pumpJZID': jzId,
        },
        success: function (data) {
            //console.log(data);
            var dataJSON = JSON.parse(data);
            //console.log('近7日用电量');
            //   console.log(dataJSON);
            dataJSON = dataJSON.obj;
            dealElecUseData(dataJSON);
            //      console.log(dataJSON);
            myChart3.resize();
        },
        error: function (data) {
            console.log('7日用电量数据获取出错');
        }
    });
}

function dealWaterUseData(waterUseData) {
    var xAxisData = [];
    var yAxisData = [];
    for (var i = 0; i < waterUseData.length; i++) {
        xAxisData.unshift(waterUseData[i].T_Time);
        yAxisData.unshift(waterUseData[i].result);
    }

    var option = {
        xAxis: [
	        {
	            data: xAxisData,
	        }
        ],
        series: [
	        {
	            data: yAxisData
	        }
        ]
    };
    myChart2.setOption(option);
}
function dealElecUseData(elecUseData) {
    var xAxisData = [];
    var yAxisData = [];
    for (var i = 0; i < elecUseData.length; i++) {
        xAxisData.unshift(elecUseData[i].T_Time);
        yAxisData.unshift(elecUseData[i].result);
    }
    var option = {
        xAxis: [
	        {
	            data: xAxisData,
	        }
        ],
        series: [
	        {
	            data: yAxisData
	        }
        ]
    };
    myChart3.setOption(option);
   
}