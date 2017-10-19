var baseId = '';
var FMname = '';
var dtuCode = '';
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
    var startDate = formatDate() + " 00:00:00";
    var endDate = formatDate() + " 23:59:59";
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
       dtuCode = $(this).children("td[data-id=FDTUCode]").html();
       FMname = $(this).children("td[data-id=FName]").html();
      yearDataJson = {
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
            "timeType": 4,
            "s_date": startDate,
            "e_date": endDate
        };
       
        console.log(dayDataJson);
        loadPressChart(startDate, endDate, baseId);
        loadFlowChart(yearDataJson, 'yearMonth');
        loadFlowChart(monthDataJson, 'monthDay');
        loadFlowChart(dayDataJson, 'dayHour');

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
    function formatDate() {
        var d = new Date();
        var dM = (d.getMonth() + 1).toString().replace(/^(\d)$/, '0$1');
        var dD = d.getDate().toString().replace(/^(\d)$/, '0$1');
        var dateTemp = d.getFullYear() + "-" + (dM) + "-" + dD;
        year = d.getFullYear();
        month = dM;
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
            url: '/V_CDJK/SearchFM_Report',
            data: {
                "pageIndex": pageIndex,
                "pageSize": pageSize,
                "Name": searchText,
                "ID":'',
                "State": state
            },
            dataType: 'JSON',
            beforeSend: loadingFunction,
            success: function (data) {
                console.log(data);
                console.log('阀门列表');
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
                        tbData[i][key] = tbData[i][key] || tbData[i][key]==0 ? tbData[i][key] : "";
                }
            }
            var tempStr = '<tr>\
                            <td data-id="BaseID" data-value="' + tbData[i].BaseID + '">' + tbData[i].BaseID + '</td>\
                            <td class="status" data-id="FIsAlarm"><i class="status_' + tempState + '"></i></td>\
                            <td data-id="TempTime">' + tbData[i].TempTime + '</td>\
                            <td data-id="FDTUCode">' + tbData[i].FDTUCode + '</td>\
                            <td data-id="FName">' + tbData[i].FName + '</td>\
                            <td data-id="FMapAddress">' + tbData[i].FMapAddress + '</td>\
                            <td data-id="F40001">' + tbData[i].F40001 + '</td>\
                            <td data-id="F40002">' + tbData[i].F40002 + '</td>\
                            <td data-id="F40003">' + tbData[i].F40003 + '</td>\
                            <td data-id="F40004">' + tbData[i].F40004 + '</td>\
                            <td data-id="F40005">' + tbData[i].F40005 + '</td>\
                            <td data-id="FTotalLL">' + tbData[i].FTotalLL + '</td>\
                        </tr>';
            tbStr += tempStr;
        }
        $(".table2 tbody").append(tbStr);
        if (!(baseId == '')) {
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
        "timeType": 4,
        "s_date": startDate,
        "e_date": endDate
    };
    console.log(dayDataJson);
    rightSectionScroll();
    if (!baseId == '') {
        loadPressChart(startDate, endDate, baseId);
        loadFlowChart(yearDataJson, 'yearMonth');
        loadFlowChart(monthDataJson, 'monthDay');
        loadFlowChart(dayDataJson, 'dayHour');
    }

    /*显示报表导航*/
    createReportList(reportFM);
    showReportFn();
    cd_famenReportList();

});

var chart_realPress = null;
var chart_yearMonth = null;
var chart_monthDay = null;
var chart_dayHour = null;
function initRightChart() {

    chart_realPress = echarts.init($('#chart_realPress')[0]);
    chart_yearMonth = echarts.init($('#chart_yearMonth')[0]);
    chart_monthDay = echarts.init($('#chart_monthDay')[0]);
    chart_dayHour = echarts.init($('#chart_dayHour')[0]);

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
            name: '出水压力',
            type: 'line',
            showAllSymbol: true,
            itemStyle: {
                normal: {
                    color: '#e293ea',
                    lineStyle: { color: '#e293ea' }
                },
            },
            data: []
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
            data: []
        }]
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
    chart_realPress.setOption(option1);
    chart_yearMonth.setOption(option2);
    chart_monthDay.setOption(option2);
    chart_dayHour.setOption(option2);
}

function loadPressChart(startDate, endDate, baseId) {
    $.ajax({
        url: '/V_CDJK/SearchFM_HisReport',
        data: {
            "pageIndex": 0,
            "pageSize": 50,
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
                for (var i = 0; i < result.length; i++) {
                    yData.unshift(result[i].F40001);
                    outPress.unshift(result[i].F40002);
                    setPress.unshift(result[i].F40003);
                    var tempTime = result[i].TempTime ? result[i].TempTime.replace("T", " ") : "";
                    xData.unshift(tempTime);
                }
                console.log(yData);
                var option = {
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
                chart_realPress.setOption(option);
            }

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
        url: '/V_CDJK/SearchFM_LL',
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
            eval('chart_' + chartStr).setOption(option);
        },
        //   complete: loadingMiss,
        error: function (data) {
            console.log('错误：' + data.responseText);
        }
    });


  

}