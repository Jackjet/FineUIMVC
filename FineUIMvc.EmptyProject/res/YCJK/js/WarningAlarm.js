
var searchText = $("#searchText").val();
var currentPage = $("#currentPage").html();
var pageIndex = currentPage - 1;
var raPageIndex = 0;
var pageSize = 16;
var totalPage = $("#totalPage").html();
var state = '';
var fKey = '';
var scrollOnoff = true;
$("#pageSize").html(pageSize);
$("#startTime").val(formatDate() + " 00: 00:00");
$("#endTime").val(formatDate() + " 23: 59:59");
var startDate = $("#startTime").val();
var endDate = $("#endTime").val();
var urlJson = parseUrl();
var pumpId = urlJson["pumpID"];
var jzId = urlJson["jzID"];
var pageType = urlJson["pageType"];
urlJson["pumpName"] = decodeURIComponent(urlJson["pumpName"]);
urlJson["jzName"] = decodeURIComponent(urlJson["jzName"]);



$(document).ready(function () {

    console.log(urlJson);
    $(window).resize(function () {
        layout();
    });
    
    $(".table2_wrap").on('click', "tr", function () {
        $(this).addClass("active").siblings().removeClass("active");
    });


    searchHistoryAlarm();
    loadRealAlarm();
    loadHistory();
    getHistoryAlarm();//获取泵房历史报警
    tableClone();
    checkPageType();
    devidePage();
    layout();
    table2Scrollbar();
    selectPumpAndJz();
    realAlarmListScroll();
   /*   $("#pageSize").click(function () {
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
            loadHistory();
        }
    });*/

});



function setPumpName(name, id) {
    $(".pumpName input").val(name);
    $(".pumpName input").attr("data-id", id);
}
function setJzName(name, id) {
    $(".machName input").val(name);
    $(".machName input").attr("data-id", id);
}




function parseUrl() {
    var url = window.location.href;
   // alert(url);
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

//时间格式化
function formatDate() {
    var d = new Date();
    var dM = (d.getMonth() + 1).toString().replace(/^(\d)$/, '0$1');
    var dD = d.getDate().toString().replace(/^(\d)$/, '0$1');
    var dateTemp = d.getFullYear() + "-" + (dM) + "-" + dD;
    return dateTemp;
}

function dealPage(total) {
    totalPage = Math.ceil(total / pageSize);
}


function dealRealAlarm(realAlarmData) {
    $(".ul_alarmList").empty();
    var str = '';
    for (var i = 0; i < realAlarmData.length; i++) {
        var tempStr = '<li data-id="' + realAlarmData[i].BaseID + '" data-fKey="' + realAlarmData[i].FKey + '">\
                            <div class="line1_box .clearfix">\
                                <p class="real_pumpName" data-id="PName">' + realAlarmData[i].PName + '</p>\
                                <p class="real_keepTime" data-id="TimeRange">' + realAlarmData[i].TimeRange + '</p>\
                            </div>\
                            <div class="line_box real_jzName" data-id="PumpJZName">' + realAlarmData[i].PumpJZName + '</div>\
                            <div class="line2_box">\
                                <p class="real_alarmTime" data-id="TempTime">【' + realAlarmData[i].TempTime.replace('T', ' ') + '】</p>\
                                <p class="real_alarmInfo" data-id="FSetMsg">' + realAlarmData[i].FSetMsg + '</p>\
                            </div>\
                      </li>';
        str += tempStr;
    }
    $(".ul_alarmList").append(str);
}

function loadRealAlarm() {
    $.ajax({
        url: '/V_YCJK/SearchAlarm',
        data: {
            "pumpID": pumpId,
            "pumpJZID": jzId,
            "pageIndex": raPageIndex,
            "pageSize": 5, //搜索是pageSize值无效
            "StartDate": '2016-01-01',//startDate
            "EndDate": '2017-05-09'//endDate
        },
        dataType: 'JSON',
        beforeSend: loadingFunction,
        success: function (data) {
            console.log(typeof data);
            console.log(data);
            if (data.obj.data.length > 0) {
                dealRealAlarm(data.obj.data);
            }
            scrollOnoff = true;
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

//及时报警滚动条函数listItemScroll
function realAlarmListScroll() {
    $('.alarmListScroll_wrap ').mCustomScrollbar({
        scrollbarPosition: "inside",
        theme: "minimal-dark",
        callbacks: {
            whileScrolling: function () {
                var $that = this.mcs.left;
             //    console.log($('#mCSB_2_container').height());
              //   console.log($('.alarmListScroll_wrap #mCSB_2_container').position().top);
                var jian = $('#mCSB_2_container').height() + $('.alarmListScroll_wrap #mCSB_2_container').position().top;
                // console.log($('#mCSB_1_container').height() + $('.listBox #mCSB_1_container').position().top);
                // console.log($('.listBox').height());
                if (jian - 10 <= $('.alarmListScroll_wrap').height()) {
                    console.log('获取');
                    if (scrollOnoff) {
                       // realAlarmScrollGet();
                    }
                }
                // console.log('-----------------------------');
            }
        }
    });
}


//leftListDataScrollGet
function realAlarmScrollGet() {
    scrollOnoff = false;
    raPageIndex++;
    alert(raPageIndex);
  
    loadRealAlarm();
   
  //  scrollOnoff = true;
}


//历史报警

function getHistoryAlarm() {
    $(".ul_alarmList").on("click", "li", function () {
        $(this).addClass("active").siblings().removeClass("active");
        jzId = $(this).attr("data-id");
        fKey = $(this).attr("data-fKey");
        startDate = '';
        endDate = '';
        loadHistory();
    });
}

function loadHistory() {
    //SearchAlarmHistory
    $.ajax({
        url: '/V_YCJK/SearchAlarmHistory',
        data: {
            "pumpID": pumpId,
            "pumpJZID": jzId,
            "pageIndex": pageIndex,
            "pageSize": pageSize,
            "StartDate": startDate,
            "EndDate": endDate,
            "FKey": fKey
        },
        dataType: 'JSON',
        beforeSend: loadingFunction,
        success: function (data) {
            console.log(typeof data);
            console.log(data);
            console.log('历史报警');
            console.log(data.obj.data);
            historyTableBody(data.obj.data);
            dealPage(data.obj.total)
            tableClone();
            checkPageType();
            layout();
            // dealWidth();
        },
        complete: loadingMiss,
        error: function (data) {
            console.log('错误：' + data.responseText);
        }
    });

}


function historyTableBody(historyData) {
    $(".table2 tbody").empty();
    var trStr = '';
    for (var i = 0; i < historyData.length; i++) {
        var tempStr = '<tr><td></td>\
                                        <td data-filed="PName" class="PName">' + historyData[i].PName + '</td>\
                                        <td data-field="PumpJZName" class="PumpJZName">' + historyData[i].PumpJZName + '</td>\
                                        <td data-field="FSetMsg">' + historyData[i].FSetMsg + '</td>\
                                        <td data-field="FAlarmTime">' + historyData[i].FAlarmTime.replace('T', ' ') + '</td>\
                                        <td data-field="FEndAlarmTime">' + historyData[i].FEndAlarmTime.replace('T', ' ') + '</td>\
                                        <td data-field="CXTime">' + historyData[i].CXTime + '</td>\
                                        <td data-field="TypeName">' + historyData[i].TypeName + '</td></tr>';
        trStr += tempStr;
    }
    $(".table2 tbody").append(trStr);
}
function dealPage(total) {
    totalPage = Math.ceil(total / pageSize);
    $("#totalNum").html(total);
    $("#totalPage").html(totalPage);
    if (total == 0) {
        $("#currentPage").html(0);
    } else {
        $("#currentPage").html(pageIndex + 1);
    }
}

//选取泵房和机组
function selectPumpAndJz() {
    $(".pumpName .selectBtn").click(function () {
        //alert('选取泵房');
        var index = layer.open({
            type: 2,
            anim: 3,
            shade: .6,
            title: ['泵房列表', 'text-align: center;color: #909090'],
            shadeClose: true,
            area: ['800px', '620px'],
            content: '/YCJK/Window/pumpWindow?pumpID=' + urlJson["pumpID"] + '&pumpName=' + urlJson["pumpName"],
            success: function () {
                //  alert('OK');
            }
        });
    });
    $(".machName .selectBtn").click(function () {
        //    alert('选取机组');
        var pumpID = $(".pumpName input").attr("data-id");
        var index = layer.open({
            type: 2,
            anim: 3,
            shade: .6,
            title: ['机组列表', 'text-align: center;color: #909090'],
            shadeClose: true,
            area: ['800px', '620px'],
            content: '/YCJK/Window/pumpJZWindow?jzID=' + urlJson["jzID"] + '&jzName=' + urlJson["jzName"] + '&pumpID=' + pumpID,
            success: function () {
                //  alert('OK');
            }
        });
    });
}


function tableClone() {
    if ($('.table1 thead').next()) {
        $('.table1 thead').next().remove();
    }
    $('.table1 thead').after($('.table2 tbody').clone());
};


function checkPageType() {
    if (pageType == 0 || pageType == '0#') {
        $(".searchCriteria").hide();
        $("th.PName").hide();
        $("th.PumpJZName").hide();
        $(".myTable td.PName").addClass("hide");
        $(".myTable td.PumpJZName").addClass("hide");
        setPumpName(urlJson["pumpName"], urlJson["pumpID"]);
        setJzName(urlJson["jzName"], urlJson["jzID"]);
    } else if (pageType == 1 || pageType == '1#') {
        $(".main_wrap ").css("paddingLeft", 35);
    }
}


function layout() {
    var winW = $(window).width();
    var pageWrapTop = $(".page_wrap").offset().top;
    // var table2Top = $(".table2_wrap").offset().top;
    //    $(".table2_wrap").css({ "height": pageWrapTop - table2Top - 14 });
    //     alert($(".table2_wrap").height());
    $(".alarmList_wrap").css({ "height": $(".list_wrap ").height() - 76 });
    $(".table2_wrap").css({ "height": $(".alarmList_wrap").height() - 58 });
    $(".alarmListScroll_wrap ").css({ "height": $(".alarmList_wrap ").height() - 40 });
    $(".detailAlarm_wrap").css({ "width": $(".list_table ").width() - $(".alarmList_wrap ").width() - 16 });
    $(".page_wrap").css({ "width": $(".detailAlarm_wrap").width() });
    //  dealWidth();
}

function devidePage() {
    //分页
    $(".page_box").on('click', 'a#nextPage', function () {

        if (pageIndex == (totalPage - 1)) {
            alert('当前为最后一页');
        } else {
            pageIndex++;
            $("#currentPage").html(pageIndex + 1);
            loadHistory();
        }
    });
    $(".page_box").on('click', 'a#prevPage', function () {
        if (!(pageIndex == 0)) {
            pageIndex--;
            $("#currentPage").html(pageIndex + 1);
            loadHistory()
        } else {
            alert('当前为第一页');
        }
    });
    $(".page_box").on('click', 'a#firstPage', function () {
        if (!(pageIndex == 0)) {
            pageIndex = 0;
            $("#currentPage").html(pageIndex + 1);
            loadHistory();
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
            loadHistory();
        }
    });


}


function table2Scrollbar() {
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
                //    dealWidth();
            }
        }
    });
}


function searchHistoryAlarm() {
    $(".searchBtn").click(function () {
        startDate = $("#startTime").val();
        endDate = $("#endTime").val();
        pumpId = $(".pumpName input").attr("data-id");
        jzId = $(".machName input").attr("data-id");
        console.log(pumpId);
        console.log(jzId);
        loadHistory();
    });
}