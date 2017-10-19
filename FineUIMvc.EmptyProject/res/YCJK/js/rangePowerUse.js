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
var powerDataJson = {};

urlJson["jzName"] = decodeURIComponent(urlJson["jzName"]);
urlJson["pumpName"] = decodeURIComponent(urlJson["pumpName"]);
console.log(urlJson["jzName"]);



/*ready*/
$(document).ready(function () {
    $("#startTime").val(startDate);
    $("#endTime").val(endDate);
    $(".chartItem_title").html(chartTitle);
    setPumpName(urlJson["pumpName"], urlJson["pumpID"]);
    setJzName(urlJson["jzName"], urlJson["jzID"]);
    JZname = $(".JZname").val();
    jzID = $(".JZname").attr("data-id");

    powerDataJson = {
        "pumpJZID": jzID,
        "timeType": 4,
        "s_date": startDate,
        "e_date": endDate
    };
    pumpID = $(".pumpName").attr("data-id");

    initRightChart();
    selectPumpAndJz();
    if (jzID !== "") {
        getPowerUse();
        getflowInfo();
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
	            name: '能耗',
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
                }
            },
            data: []
        }]
    };

    chart_waterInOutPress.setOption(option1);

}

/*图表搜索按钮*/
function searchTF() {
    $(".chart_searchBtn").click(function () {
        $(".selectedWrap input").val('');
        startDate = $("#startTime").val();
        endDate = $('#endTime').val();
        JZname = $('.JZname').val();
        jzID = $(".JZname").attr("data-id");

        powerDataJson = {
            "pumpJZID": jzID,
            "timeType": 4,
            "s_date": startDate,
            "e_date": endDate
        };
        getPowerUse();
        getflowInfo();
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


function getPowerUse() {
   
    $.ajax({
        url: '/V_YCJK/SearchDSNH',
        data: powerDataJson,
        dataType: 'JSON',
        success: function (data) {
            console.log(typeof data);
            console.log(data);
            console.log('吨水能耗');
            console.log(data.obj);
            dealPowerUseData(data.obj);
        },
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
        xData.push(powerUseData[i].T_Time);
       
        dashData.push(0.8);
        yData.push(powerUseData[i].dsnh);
    }
    var option = {
        xAxis: [{
            data: xData
        }],
        series: [{
            data: yData
        },
        {
            data: dashData
        }]
    };
    chart_waterInOutPress.setOption(option);
}

function getflowInfo() {
    $.ajax({
        url: '/V_YCJK/SearchDSNHResult',
        data: powerDataJson,
        dataType: 'JSON',
      //  beforeSend: loadingFunction,
        success: function (data) {
            console.log(typeof data);
            console.log(data);
            console.log('右下角表格信息');
            dealflowInfo(data);
        },
    //    complete: loadingMiss,
        error: function (data) {
            console.log('错误：' + data.responseText);
        }
    });
}

function dealflowInfo(infoData) {
    var sAccuFlow = 0;
    var eAccuFlow = 0;
    var pAccuFlow = 0;
    var sElecUse = 0;
    var eElecUse = 0;
    var pElecUse = 0;
    var dsnh = 0;
    if (infoData.length == 2) {
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
    $(".eElecUse").html(eElecUse);
    $(".pElecUse").html(pElecUse);
    $(".dsnh").html(dsnh);
    $(".setDsnh").html(0.8);
    $(".averDsnh").html(0);
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

