var baseId = '';
var TFname = "";
$(document).ready(function () {
    //click list 
    var searchText = $("#searchText").val();
    var currentPage = $("#currentPage").html();
    var pageIndex = currentPage - 1;
    var pageSize = 20;
    var totalPage = $("#totalPage").html();
    var state = '';
    var year = '';
    var month = '';
    var day = formatDate();
    var startDate = formatDate(0) + " 00:00:00";
    var endDate = formatDate(1) + " 00:00:00";
    
    $("#pageSize").html(pageSize);
    $(".searchBtn").click(function () {
        searchText = $("#searchText").val();
        loadTable();
    });
    $(".table2 tbody").on('click', "tr", function () {
        /*  if (!$(".myTable tr").hasClass("active") && baseId == '') {
              $(".sectionBox1").hide();
              $(".sectionBox2").show();
          }*/
      
        $(this).addClass("active").siblings().removeClass("active");
        baseId = $(this).children("td[data-id=BaseID]").html();
        TFname = $(this).children("td[data-id=FName]").html();
       
        getChartsData(startDate, endDate, TFname,baseId);
     
    });

   
    //分页
    $(".page_box").on('click', 'a#nextPage', function () {

        if (pageIndex == (totalPage - 1)) {
            alert('当前为最后一页');
        } else {
            pageIndex++;
            $("#currentPage").html(pageIndex + 1);
            loadTable();
        }
    });
    $(".page_box").on('click', 'a#prevPage', function () {
        if (!(pageIndex == 0)) {
            pageIndex--;
            $("#currentPage").html(pageIndex + 1);
            loadTable()
        } else {
            alert('当前为第一页');
        }
    });
    $(".page_box").on('click', 'a#firstPage', function () {
        if (!(pageIndex == 0)) {
            pageIndex = 0;
            $("#currentPage").html(pageIndex + 1);
            loadTable();
        } else {
            alert('当前为第一页');
        }
    });
    $(".page_box").on('click', 'a#lastPage', function () {
        if (pageIndex == totalPage - 1) {
            alert('当前为最后一页');

        } else {
            pageIndex = totalPage - 1;
            // alert(pageIndex);
            $("#currentPage").html(pageIndex + 1);
            loadTable();
        }
    });

    function dealPage(total) {
        totalPage = Math.ceil(total / pageSize);
    }

   /* function formatDate() {
        var d = new Date();
        var dM = (d.getMonth() + 1).toString().replace(/^(\d)$/, '0$1');
        var dD = d.getDate().toString().replace(/^(\d)$/, '0$1');
        var dateTemp = d.getFullYear() + "-" + (dM) + "-" + dD;
        year = d.getFullYear();
        month = dM;
        return dateTemp;
    }*/

    function formatDate(i) {
        var curD = new Date();
        var d = new Date(curD.getTime() + i * 24 * 60 * 60 * 1000);
        var dM = (d.getMonth() + 1).toString().replace(/^(\d)$/, '0$1');
        var dD = d.getDate().toString().replace(/^(\d)$/, '0$1');
        var dateTemp = d.getFullYear() + "-" + (dM) + "-" + dD;
        return dateTemp;
    }

    $(".search_wrap button").each(function (i, v) {
        $(this).click(function () {
            searchText = $("#searchText").val();
            var index = i;
            $(this).addClass("active").siblings().removeClass("active");
            switch (index) {
                case 0: {
                    state = 1;
                    loadTable();
                    break;
                }
                case 1: {
                    state = 0;
                    loadTable();
                    break;
                }
                case 2: {
                    state = 2;
                    loadTable();
                    break;
                }
                case 3: {
                    state = '';
                    loadTable();
                    break;
                }
            }
            loadTable();
        });
    });
    $("#showHideBtn").click(function () {
        var winW = $(window).width();
        console.log(winW);
        var leftW = $('.chart_box').width();
        if (winW <= 1366) {
            if ($(this).hasClass('show')) {
                $('.list_wrap').css({ 'width': winW - 75 });
                $('.chart_box').animate({ 'marginRight': -leftW });
                $(this).removeClass('show');
            } else {
                $('.chart_box').animate({ 'marginRight': 0 });
                /*$('.list_wrap').animate({ 'width': winW-leftW -75 },function(){    	
                    
                });*/
                $(this).addClass('show');
            }
        } else if (winW > 1366) {
            if ($(this).hasClass('show')) {
                $('.chart_box').animate({ 'marginRight': -leftW }, function () {
                    $('.list_wrap').animate({ 'width': $(".chart_box").offset().left - 75 });
                    //$('.list_wrap').animate({ 'width': winW-leftW -77 });
                });
                $(this).removeClass('show');
            } else {
                $('.list_wrap').animate({ 'width': winW - leftW - 75 }, function () {

                    //$('.list_wrap').animate({ 'width': $(".chart_box").offset().left -68 });
                    $('.chart_box').animate({ 'marginRight': 0 });
                });
                $(this).addClass('show');
            }
        }
    });
    addScroll();
    function addScroll() {
        $('.table2_wrap').mCustomScrollbar({
            scrollButtons: {
                enable: true,
                scrollType: "continuous",
                scrollSpeed: 20,
                scrollAmount: 40
            },
            axis: "yx",
            set_width: false,
            scrollbarPosition: "inside",
            theme: "minimal-dark",
            advanced: { autoExpandHorizontalScroll: true },
            callbacks: {
                whileScrolling: function () {
                    var $that = this.mcs.left;
                    $('.table1').css('left', $that);
                    $('.table1_wrap').css('width', '100%');

                },
                onUpdate: function () {
                    var $that = $('#mCSB_1_container').css('left');
                    $('.table1').css('left', $that);
                    $('.table1_wrap').css('width', '100%');
                    tableClone();
                }
            }
        });
    }


    function tableClone() {
        if ($('.table1 thead').next()) {
            $('.table1 thead').next().remove();
        }
        $('.table1 thead').after($('.table2 tbody').clone());
    };


    loadTable();
    setInterval(function () {
        loadTable();
    }, 10000);
    function loadTable() {

        $.ajax({
            url: '/V_CDJK/SearchTF_Report',
            data: {
                "pageIndex": pageIndex,
                "pageSize": pageSize,
                "Name": searchText,
                "State": state,
                "ID":''//baseId
            },
            dataType: 'JSON',
            beforeSend: loadingFunction,
            success: function (data) {
                console.log(data);
                console.log('调峰泵站');
                dealTbodyFn(data.data);
                dealPage(data.total);
                tableClone();
                addScroll();
            },
            complete: loadingMiss,
            error: function (data) {
                console.log('错误：' + data.responseText);
            }
        });
    }
    function loadingFunction() {
        var $div = $('<div class="loading" style="position:absolute;left: 50%;top:50%;margin-left: -150px;margin-top: -70px;width: 300px;color:black;text-align:center;line-height: 140px;height: 140px;background: rgba(255,255,0);color: white;border-radius: 8px;"><img style="position: relative;top: 56%;left: 15%;" src="/res/YCJK/img/load1.gif" alt="loading....">正在加载中...</div>');
        $('body').append($div);
    };
    function loadingMiss() {
        $('.loading').remove();
    };

    function dealTbodyFn(tbData) {
        $(".table2 tbody").empty();
        var tbStr = '';
        for (var i = 0; i < tbData.length; i++) {
            for (var key in tbData[i]) {
                if (key == "FOnLine") {
                    var onOff = tbData[i][key];
                }
                switch (key) {
                    case "TempTime":
                        tbData[i][key] = tbData[i][key] ? tbData[i][key].replace("T", " ") : "";
                        break;
                    case "FIsAlarm":
                        var isAlarm = tbData[i][key];
                        if (isAlarm) {
                            var tempState = 2;
                        } else if (onOff) {
                            var tempState = 1
                        } else {
                            var tempState = 0;
                        }
                        break;
                    default:
                        tbData[i][key] = tbData[i][key] ? tbData[i][key] : "";
                }
            }
           
            var tempStr = '<tr>\
                            <td data-id="BaseID" data-value="' + tbData[i].BaseID + '">' + tbData[i].BaseID + '</td>\
                            <td class="status" data-id="FIsAlarm"><i class="status_' + tempState + '"></i></td>\
                            <td data-id="DtuNum">' + tbData[i].DtuNum + '</td>\
                            <td data-id="TempTime">' + tbData[i].TempTime + '</td>\
                             <td data-id="FName">' + tbData[i].FName + '</td>\
                             <td data-id="F40001"><span>' + tbData[i].F40001 + '</span><span class="FOut">' + tbData[i].F40002 + '</span></td>\
                             <td data-id="FTotalInLL"><span>' + tbData[i].FTotalInLL + '</span><span class="FOut">' + tbData[i].FTotalOutLL + '</span></td>\
                            <td data-id="F40003">' + tbData[i].F40003 + '</td>\
                            <td data-id="F40004">' + tbData[i].F40004 + '</td>\
                            <td data-id="F40005">' + tbData[i].F40005 + '</td>\
                            <td data-id="F40006">' + tbData[i].F40006+ '</td>\
                            <td data-id="F40007">' + tbData[i].F40007 + '</td>\
                            <td data-id="F40008">' + tbData[i].F40008 + '</td>\
                            <td data-id="F40009">' + tbData[i].F40009 + '</td>\
                            <td data-id="F40010">' + tbData[i].F40010 + '</td>\
                            <td data-id="F40011">' + tbData[i].F40011 + '</td>\
                            <td data-id="F40012">' + tbData[i].F40012 + '</td>\
                            <td data-id="F40013">' + tbData[i].F40013+ '</td>\
                            <td data-id="F40014">' + tbData[i].F40014 + '</td>\
                            <td data-id="F40015">' + (tbData[i].F40015?(tbData[i].F40015=="1"?"打开" :"关闭"):" ")+ '</td>\
                            <td data-id="F40016">' +( tbData[i].F40016 ? (tbData[i].F40016 == "1" ? "启用" : "停用") : " ") + '</td>\
                        </tr>';
            tbStr += tempStr; 
        }
        $(".table2 tbody").append(tbStr); 
        if (!(baseId=='')) {
            var tempStr = $('.myTable tr td[data-value=' + baseId + ']').parent().html();
            if (tempStr) {
                $(".myTable tr td[data-value=" + baseId + "]").parent().addClass("active");
            }
        }
    }



    function dealPage(total) {
        totalPage = Math.ceil(total / pageSize);
        $("#totalNum").html(total);
        $("#totalPage").html(totalPage);
    }

    function layout() {
        var winW = $(window).width();
        var pageWrapTop = $(".page_wrap").offset().top;
        var table2Top = $(".table2_wrap").offset().top;
        $(".table2_wrap").css({ "height": pageWrapTop - table2Top - 6 });
        if (winW <= 1366) {
            $(".list_wrap").css({ "width": winW - 51 });

        } else if (winW > 1366) {
            var offsetR = $(".chart_box").offset().left;
            $(".list_wrap").css({ "width": offsetR - 51 });
        }

        $(".page_wrap").css({ "width": $(".list_wrap").width() - 2 });

    }
    layout();
    $(window).resize(function () {
        layout();
        tableClone();
    });
    function dealData(data) {
        console.log(data);
    }
    $("#pageSize").click(function () {
        $(this).css({ "display": "none" })
        $("#editPageIndex").css({ "display": "inline-block" }).focus().html($(this).html());
    });
    $("#editPageIndex").blur(function () {
        $(this).css({ "display": "none" });
        var tempV = $(this).val();
        if (tempV == '' || !(/^\d*$/.test(tempV))) {
            $("#pageSize").css({ "display": "inline-block" });
            $("#editPageIndex").val($("#pageSize").val());
        } else if (/^\d*$/.test(tempV)) {
            $("#pageSize").html(tempV).css({ "display": "inline-block" });
            pageSize = tempV;
            loadTable();
        }
    });

    //设置参数
    $(".setBtn").click(function () {

        var index = layer.open({
            type: 2,
            anim: 3,
            shade: .6,
            title: false,// ['参数设置', 'text-align:center;'],
            shadeClose: true,
            area: ['96%', '96%'],
            content: ['/YCJK/V_YCJK/ParmSet2', 'no'],// '/YCJK/Window/pumpWindow?pumpID=' + urlJson["pumpID"] + '&pumpName=' + urlJson["pumpName"],
            end: function () {
                $('.table2_wrap').mCustomScrollbar("destroy");
                layout();
                loadTable();
                //    $(".table2").css("width", $(".table1").width());
                // addScroll();
                //  $('.table2_wrap').mCustomScrollbar("update");
                $('.table1').css('left', 0);
            }
        });
    });
    function rightSectionScroll() {
        $('.chart_mainBox').mCustomScrollbar({
            scrollbarPosition: "inside",
            theme: "minimal-dark"
        });
    }
    initRightChart();
    var yearDataJson = {
        "BASEID": baseId,
        "year": year,

        "timeType": 1
    };
    var monthDataJson = {
        "BASEID": baseId,
        "year": year,
        "month": month,
        "timeType": 2
    };
    var dayDataJson = {
        "BASEID": baseId,
        "day": day,
        "timeType": 3
    };

    rightSectionScroll();
  //  waterInOutPress(startDate, endDate, baseId);
   // loadFlowChart(yearDataJson, 'yearMonth');
 //   loadFlowChart(monthDataJson, 'monthDay');
    //   loadFlowChart(dayDataJson, 'dayHour');

    createReportList(reportTF);
    showReportFn();
    tf_waterInOutPress();
  //  tf_waterInOutPress(TFname, baseId);
});

var chart_waterInOutPress = null;
var chart_tfWaterFlow = null;
var chart_waterLevel = null;
var chart_tfAbility = null;
function initRightChart() {

    chart_waterInOutPress = echarts.init($('#chart_waterInOutPress')[0]);
    chart_tfWaterFlow = echarts.init($('#chart_tfWaterFlow')[0]);
    chart_waterLevel = echarts.init($('#chart_waterLevel')[0]);
    chart_tfAbility = echarts.init($('#chart_tfAbility')[0]);

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
            data: ['进水压力','出水压力']
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
            data: ['水箱容积', '储水总量','调峰水量','日调峰量']
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
                name: '水箱液位',
                type: 'line',
                itemStyle: {
                    normal: {
                        color: '#c2a1dc',
                        lineStyle: { color: '#c2a1dc' }
                    },
                },
                data: []
            }
        ]
    };
    chart_waterInOutPress.setOption(option1);
    chart_tfWaterFlow.setOption(option2);
     chart_waterLevel.setOption(option3);
     chart_tfAbility.setOption(option1);
}

function getChartsData(startDate, endDate, TFname, baseId) {
    $.ajax({
        url: '/V_CDJK/SearchTF_HisReport',
        data: {
            "pageIndex": 0,
            "pageSize": 50,
            "Name": TFname, //
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
            dealChartsData(result);
        },
        //   complete: loadingMiss,
        error: function (data) {
            console.log('错误：' + data.responseText);
        }
    });

}
function loadFlowChart(dataJson, chartStr) {

    /*年每月*/
    $.ajax({
        url: '/V_CDJK/SearchLL_Flow',
        data: dataJson,
        dataType: 'JSON',
        //     beforeSend: loadingFunction,
        success: function (data) {
            console.log(data);
            var data = data.obj;
            console.log(chartStr);
            var xData = [];
            var yData = [];
            for (var i = 0; i < data.length; i++) {
                yData.unshift(data[i].result);
                xData.unshift(data[i].T_Time);
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
            eval('chart_' + chartStr).setOption(option);
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
    console.log(result);
    for (var i = 0; i < result.length; i++) {
        series1Data.unshift(result[i].F40001);
        series2Data.unshift(result[i].F40002);
        tfWaterSeries1.unshift(result[i].F40004);
        tfWaterSeries2.unshift(result[i].F40007);
        tfWaterSeries3.unshift(result[i].F40008);
        tfWaterSeries4.unshift(result[i].F40014);
        waterLevelSeries.unshift(result[i].F40013);

        tfAbilitySeries1.unshift((result[i].F40011 / 60).toFixed(2));
        tfAbilitySeries2.unshift(result[i].F40012);
        var tempTime = result[i].TempTime ? result[i].TempTime.replace("T", " ") : "";
        xData.unshift(tempTime);
    }

    var option = {
        xAxis: {
            data: xData
        },
        series: [{
            data: series1Data
        },
        {
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
    var optionWL = {
        xAxis: {
            data: xData
        },
        series: [{
            data: waterLevelSeries
        }]
    };
    /*调峰能力*/
    var optionTFAbility = {
      
        legend: {
            data: ['调峰能力', '滞留时间']
        },
        xAxis: {
            data: xData
        },
        yAxis: {
            name:"时间(h)"
        },
        series: [{
            name: "调峰能力",
            data: tfAbilitySeries1
        },
        {
            name: "滞留时间",
            data: tfAbilitySeries2
        }]
    };
    chart_waterInOutPress.setOption(option);
    chart_tfWaterFlow.setOption(optionTF);
    chart_waterLevel.setOption(optionWL);
    chart_tfAbility.setOption(optionTFAbility);
}


