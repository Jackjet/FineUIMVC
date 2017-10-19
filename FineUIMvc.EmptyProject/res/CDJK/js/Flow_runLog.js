var TFname = "";
var baseId = "";
var startDate = formatDate(0) + " 00:00:00";
var endDate = formatDate(1) + " 00:00:00";
var urlJson = parseUrl();
var tablePageSize = 20;
var tableSearchText = $("#tableSearchText").val();
var currentPage = $("#currentPage").html();
var tablePageIndex = currentPage - 1;
var totalPage = $("#totalPage").html();
$("#pageSize").html(tablePageSize);

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

    loadTable();
    searchTF();
    getTFname();//弹出调峰列表
    exportRunLog();//导出列表
    layout();
    pagingFn();
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
    var page_wrapH = $(".page_wrap").height();
    $(".table2_wrap").css({ "height": chart_mainWrapH - header_wrapH - page_wrapH - 36 });


}
/*图表搜索按钮*/
function searchTF() {
    $(".chart_searchBtn").click(function () {
        startDate = $("#startTime").val();
        endDate = $('#endTime').val();
        TFname = $('.TFname').val();
        baseId = $(".TFname").attr("data-id");
        loadTable();
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
            content: ['/YCJK/V_CDJK/Flow_selectName', 'no'],
            success: function () {
                //  alert('OK');
            }
        });
    });

}

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

function loadTable() {
    $.ajax({
        url: '/V_CDJK/SearchLL_HisReport',
        data: {
            "pageIndex": tablePageIndex,
            "pageSize": tablePageSize,
            "ID": baseId,
            "StartDate": startDate,
            "EndDate": endDate
        },
        dataType: 'JSON',
        beforeSend: loadingFunction,
        success: function (data) {
            console.log(data);
            console.log('测点流量日志');
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
            switch (key) {
                case "TempTime":
                    tbData[i][key] = tbData[i][key] ? tbData[i][key].replace("T", " ") : "";
                    break;
                default:
                    tbData[i][key] = tbData[i][key] ? tbData[i][key] : "";
            }
        }

        var tempStr = '<tr>\
                             <td data-id="BaseID">' + tbData[i].BaseID + '</td>\
                            <td data-id="TempTime">' + tbData[i].TempTime + '</td>\
                            <td data-id="FDTUCode">' + tbData[i].FDTUCode + '</td>\
                            <td data-id="FName">' + tbData[i].FName + '</td>\
                            <td data-id="C">' + tbData[i].C + '</td>\
                            <td data-id="E">' + tbData[i].E + '</td>\
                            <td data-id="P01">' + tbData[i].P01 + '</td>\
                            <td data-id="P02">' + tbData[i].P02 + '</td>\
                            <td data-id="A01">' + tbData[i].A01 + '</td>\
                            <td data-id="A02">' + tbData[i].A02 + '</td>\
                            <td data-id="A03">' + tbData[i].A03 + '</td>\
                             <td data-id="F4002">' + tbData[i].F40002 + '</td>\
                            <td data-id="F4003">' + tbData[i].F40003 + '</td>\
                            <td data-id="F4004">' + tbData[i].F40004 + '</td>\
                        </tr>';
        tbStr += tempStr;
    }
    $(".table2 tbody").append(tbStr);
   
}



function dealPage(total) {
    totalPage = Math.ceil(total / tablePageSize);
    $("#totalNum").html(total);
    $("#totalPage").html(totalPage);

}

/*分页*/
function pagingFn() {
    $(".page_box").on('click', 'a#nextPage', function () {

        if (tablePageIndex == (totalPage - 1)) {
            alert('当前为最后一页');
        } else {
            tablePageIndex++;
            $("#currentPage").html(tablePageIndex + 1);
            loadTable();
        }
    });
    $(".page_box").on('click', 'a#prevPage', function () {
        if (!(tablePageIndex == 0)) {
            tablePageIndex--;
            $("#currentPage").html(tablePageIndex + 1);
            loadTable()
        } else {
            alert('当前为第一页');
        }
    });
    $(".page_box").on('click', 'a#firstPage', function () {
        if (!(tablePageIndex == 0)) {
            tablePageIndex = 0;
            $("#currentPage").html(tablePageIndex + 1);
            loadTable();
        } else {
            alert('当前为第一页');
        }
    });
    $(".page_box").on('click', 'a#lastPage', function () {
        if (tablePageIndex == totalPage - 1) {
            alert('当前为最后一页');

        } else {
            tablePageIndex = totalPage - 1;
            // alert(pageIndex);
            $("#currentPage").html(tablePageIndex + 1);
            loadTable();
        }
    });
}




function setTFname(tfId, tfName) {
    $(".TFname").val(tfName);
    $(".TFname").attr("data-id", tfId);
    baseId = $(".TFname").attr("data-id");
    TFname = $(".TFname").val();
    loadTable();
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
/*导出日志*/
function exportRunLog() {
    $(".exportBtn").click(function () {
        startDate = $("#startTime").val();
        endDate = $('#endTime').val();
        TFname = $('.TFname').val();
        baseId = $(".TFname").attr("data-id");
        downLoadxsl();
    });
}
function downLoadxsl() {
    $.ajax({
        type: 'POST',
        url: '/V_CDJK/ReportLL_His',
        data: {
            "pageIndex": tablePageIndex,
            "pageSize": tablePageSize,
            "ID": baseId,
            "StartDate": startDate,
            "EndDate": endDate
        },
        dataType: "json",
        beforeSend: loadingFunction,
        success: function (data) {
            console.log(data);
            if (data.msg == 'ok') {
                var layHtm = '<a href="' + data.Url + '"><div style="padding-top:12px;font-size:16px; text-align:center">点击下载</div></a>';
                var loadLayer = layer.open({
                    title: "文件下载",
                    type: 1,
                    area: ['300px', '200px'],
                    content: layHtm,
                });

            } else {
                alert(data.msg);
            }

        },
        complete: loadingMiss,
        error: function (data) {
            console.log('错误：' + data.responseText);
        }
    });
}