

//tubiao
var mapChart1 = echarts.init($('#map_echart1')[0]);
//var mapChart2 = echarts.init($('#map_echart2')[0]);
//var mapChart3 = echarts.init($('#map_echart3')[0]);
function initRightChart() {

    var option1 = {
        title: {
            text: '',
            subtext: '',
            x: 'center'
        },
        tooltip: {
            trigger: 'item',
            formatter: "{b} : {c} ({d}%)"
        },
        label: { normal: {show:true}},
        legend: {
            orient: 'vertical',
            right: 50,
            bottom: 20,
            data: [
                {name:'箱式',icon:'circle' },
                { name: '柜式', icon: 'circle' },
                { name: '卧式', icon: 'circle' }
                ]
            },
        series: [
            {
                name: '访问来源',
                type: 'pie',
                radius: '55%',
                center: ['40%', '45%'],
                selectedOffset: 4,
                label: {
                    normal: {
                        show: true,
                        position:'inside',
                        formatter: "{c}"
                    }
                },
                data: [
                    { itemStyle: { normal: { color: '#ffab5a' } } },
                    { selected: true, itemStyle: { normal: { color: '#a0e792' } } },
                    {   itemStyle: { normal: { color: '#5ebfe7' } } },
                  
                ],
               
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
            bottom: '20px',
            top: '20px',
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
            bottom: '20px',
            top:'20px',
            containLabel: true
        },
        xAxis: [
            {
                type: 'category',
                data: [1,2,3,4,5,6,7],
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
    mapChart1.setOption(option1);
    //mapChart2.setOption(option2);
    //mapChart3.setOption(option3);
}

function getJzStatus() {
    $.ajax({
        url: '/V_YCJK/SearchZLB_Count',
        dataType:'JSON',
        success: function (data) {
            //console.log(data);
            //console.log(data[0].zx_cont);
            $(".li_online .disNum").html(data[0].zx_cont);
            $(".li_alarm .disNum").html(data[0].bj_cont);
            $(".li_offline .disNum").html(data[0].lx_cont);
            //console.log('机组数量');
        },
        error: function (data) {
            console.log('机组状态数据获取出错');
        }
    });
}

function getPumpJzCount() {
    $.ajax({
        url: '/V_YCJK/Pump_PumpJZ_Count',
        dataType: 'JSON',
        success: function (data) {
            //console.log(data);
            var result = data.obj;
            $(".pumpJzNum .pumpNum").html(result[0]._count);
            $(".pumpJzNum .jzNum").html(result[1]._count);
            //console.log('机组数量');
            var pieData = [];
            var legData = [{ icon: 'circle' }, { icon: 'circle' },{ icon: 'circle' }];
            var colorArr = [{ normal: { color: '#ffab5a' } }, { normal: { color: '#a0e792' } }, { normal: { color: '#5ebfe7' } }];
            for (var i = 2; i < result.length; i++) {
                var tempJson = {};
                tempJson["value"] = result[i]._count;
                tempJson["name"] = result[i]._name;
                tempJson["itemStyle"] = colorArr[i - 2];
                legData[i - 2]["name"] = result[i]._name;
                pieData.push(tempJson);
            }
            //console.log(pieData);
            //console.log(legData);
            var option = {
                legend: {
                    data: legData
                },
                series: [{ data: pieData }]
            };
            mapChart1.setOption(option);
        },
        error: function (data) {
            console.log('泵房机组个数数据获取出错');
        }
    });
}
// 
function dealPumpJzData(pjData) {
    
}

//水能耗前10
function getWaterUseTop10() {
    $.ajax({
        url: '/V_YCJK/SL_PumpJZTop10',
        data: {
            "timeType": 'day'
        },
        dataType:'JSON',
        success: function (data) {
            //console.log(data);
            //console.log('用水量top10');
            var data = data.obj;
            var yAxisData = [];
            var xAxisData = [];
            for (var i = 0; i < data.length; i++) {
                yAxisData.push(data[i].FTotalOutLL);
                xAxisData.push(data[i].PumpJZName);
            }
            //console.log(yAxisData);
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
            mapChart2.setOption(option);
           //console.log(data);
         },
        error: function (data) {
            console.log('用水量top10数据获取出错');
        }
    });
}



function dealWaterConsup10() {

 
   
   // myChart1.setOption(option);
}


//年用电量前十机组
function getElecUseTop10() {
    $.ajax({
        url: '/V_YCJK/DL_PumpJZTop10',
        data: {
            "timeType": 'day'
        },
       dataType:'JSON',
        success: function (data) {
            //console.log(data);
            //console.log('用电量top10');
         
            var data = data.obj;
            var yAxisData = [];
            var xAxisData = [];
            for (var i = 0; i < data.length; i++) {
                yAxisData.push(data[i].FTotalDL);
                xAxisData.push(data[i].PumpJZName);
            }
            //console.log(yAxisData);
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
            mapChart3.setOption(option);
        },
        error: function (data) {
            console.log('用电量top10数据获取出错');
        }
    });
}


function dealWaterUse10(waterUseData) {
    
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
  //  myChart2.setOption(option);
}
function dealElecUse10(elecUseData) {
  
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
   // myChart3.setOption(option);
}