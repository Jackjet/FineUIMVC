
var curPumpId = '';
var curJzId = '';
$(document).ready(function () {
    //click list 
    var searchText = $("#searchText").val();
    var currentPage = $("#currentPage").html();
    var pageIndex = currentPage - 1;
    var pageSize = 20;
    var totalPage = $("#totalPage").html();
    var state = '';
    $("#pageSize").html(pageSize);
    $("#startTime").val(formatDate() + " 00: 00:00");
    $("#endTime").val(formatDate() + " 23: 59:59");
    var startDate = $("#startTime").val();
    var endDate = $("#endTime").val();
    var urlJson = parseUrl();
    var pumpId = urlJson["pumpID"];
    var jzId = urlJson["jzID"];
    //console.log(urlJson);
    urlJson["pumpName"] = decodeURIComponent(urlJson["pumpName"]);
    urlJson["jzName"] = decodeURIComponent(urlJson["jzName"]);
   // alert(urlJson["jzName"]);
    setPumpName(urlJson["pumpName"], urlJson["pumpID"]);
    setJzName(urlJson["jzName"], urlJson["jzID"]);

    /*导出运行日志*/
    exportRunLog(pageSize);

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

    function formatDate() {
        var d = new Date();
        var dM = (d.getMonth() + 1).toString().replace(/^(\d)$/, '0$1');
        var dD = d.getDate().toString().replace(/^(\d)$/, '0$1');
        var dateTemp = d.getFullYear() + "-" + (dM) + "-" + dD;
        return dateTemp;
    }

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
    $(".searchBtn").click(function () {
        startDate = $("#startTime").val();
        endDate = $("#endTime").val();
        pumpId = $(".pumpName input").attr("data-id");
        jzId = $(".machName input").attr("data-id");
        //console.log(pumpId);
        //console.log(jzId);
        loadTable();
    });
    $(".table2_wrap").on('click', "tr", function () {
        $(this).addClass("active").siblings().removeClass("active");

    });


    //设置
    $(".setBtn").click(function () {
        var index =layer.open({
            type: 2,
            anim: 3,
            shade: .6,
            title:['参数设置','text-align:center;'],
            shadeClose: true,
            area: ['98%', '98%'],
            content: '/YCJK/V_YCJK/ParmSet1',// '/YCJK/Window/pumpWindow?pumpID=' + urlJson["pumpID"] + '&pumpName=' + urlJson["pumpName"],
            end: function () {
               $('.table2_wrap').mCustomScrollbar("destroy");
               loadTable();
               $('.table2_wrap').mCustomScrollbar("update");
                $('.table1').css('left', 0);
            }
        });
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
        return dateTemp;
    }
    addScroll();
 /*   function addScroll() {
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
          //  advanced: { autoExpandHorizontalScroll: true },
            callbacks: {
                whileScrolling: function () {
                    var $that = this.mcs.left;
                    $('.table1').css('left', $that);
                    $('.table1_wrap').css('width', '100%');

                },
                onUpdate: function () {
                    //setTimeout(function () {
                    //    var $that = $('#mCSB_1_container').css('left');
                    //    $('.table1').css('left', $that);
                    //    $('.table1_wrap').css('width', '100%');
                       
                    //    $(".table1").css("width", $(".table2").width());
                    //    $('#mCSB_3_container').css("width", $(".table1").width());
                    
                    //      alert('fuck');
                    //}, 2000);
                }
            }
        });
        
    }*/
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
                   // tableClone();

                }
            }
        });
    }


    loadTable();
    function loadTable() {
        // alert(state);
        $.ajax({
            url: '/V_YCJK/Search_YearReportList',
            data: {
                "pumpID": pumpId,
                "pumpJZID": jzId,
                "pageIndex": pageIndex,
                "pageSize": pageSize,
                "StartDate": startDate,
                "EndDate": endDate
            },
            dataType: 'JSON',
            beforeSend: loadingFunction,
            success: function (data) {
                //console.log(data);
              
                //console.log(data.data[0].jsonName);
                dealTheadFn(data.data[0].jsonName);
                //console.log(data.data[1].jsonData.data);
                dealTbodyFn(data.data[1].jsonData.data);
                dealPage(data.data[1].jsonData.total);
                tableClone();
                addScroll();
            },
            complete: loadingMiss,
            error: function (data) {
                console.log('错误：' + data.responseText);
            }
        });
    }
  
    function tableClone() {
        if ($('.table1 thead').next()) {
            $('.table1 thead').next().remove();
        }
        $('.table1 thead').after($('.table2 tbody').clone());
    };
    //处理表头
    var theadJson = {};
    function dealTheadFn(thData) {
        $(".table1 thead tr").empty();
        $(".table2 thead tr").empty();
        theadJson = {};
        var tempStr = '<th></th>';
        for (var i = 0; i < thData.length; i++) {
            var tdData = thData[i].split(':');
            theadJson[tdData[1]] = tdData[0];
        }
        tempStr += dealTheadTdFn(theadJson);
        //console.log(tempStr);
        $(".table1 thead tr").append(tempStr);
        $(".table2 thead tr").append(tempStr);
    }

    function dealTheadTdFn(dealData) {
        var thStr = '';
        for (var i in dealData) {
            thStr += '<th class="' + i + '">' + dealData[i] + '</th>';
        }
        return thStr;
    }
    function dealTbodyFn(tbData) {
        $(".table2 tbody").empty();
        var tbStr = '';
        for (var i = 0; i < tbData.length; i++) {
            var tempStr = '<td data-id="pumpJZId" data-value="' + tbData[i].pumpJZId + '">' + tbData[i].pumpJZId + '</td>';
            
            for (var key in theadJson) {
                switch (key) {
                    case "InOutWaPa":
                        var inOutV = tbData[i][key].split("/");
                        tempStr += '<td class="inOutPress" data-id="' + key + '"><span class="pressIn alarm">' + inOutV[0] + '</span><span class="pressOut">' + inOutV[1] + '</span></td>';
                        break;
                    case "PumpJZName":
                        tempStr += '<td class="equipName" data-id="' + key + '">' + tbData[i][key] + '</td>';
                        break;
                    case "FUpdateDate":
                        var createDate = '';
                        if (tbData[i][key] !== null) {
                            createDate = tbData[i][key].replace("T", " ")
                        }
                        tempStr += '<td data-id="' + key + '">' + createDate + '</td>';
                        break;
                    case "PActiveState":
                        var statusStr = dealPumpStaFn(tbData[i][key]);
                        tempStr += '<td class="pumpStatus" data-id="' + key + '">' + statusStr + '</td>';
                        break;
                        //F41003
                    case "F41003":
                        var sysStatusArr = ['手动','自动','远程自动'];
                        tempStr += '<td class="syatemStatus" data-id="' + key + '">' + eval('sysStatusArr[' + tbData[i][key] + ']') + '</td>';
                        break;
                    default:
                        tempStr += '<td data-id="' + key + '">' + (tbData[i][key] || tbData[i][key] ==0? tbData[i][key] : "") + '</td>';
                        break;
                }
                /*   if (key == "InOutWaPa") {
                    var inOutV = tbData[i][key].split("/");
                    tempStr += '<td class="inOutPress" data-id="' + key + '"><span class="pressIn alarm">' + inOutV[0] + '</span><span class="pressOut">' + inOutV[1] + '</span></td>';
                } else if (key == "PumpJZName") {
                    tempStr += '<td class="equipName" data-id="' + key + '">' + tbData[i][key] + '</td>';
                } else if (key == "FUpdateDate") {
                    var createDate = '';
                    if (tbData[i][key] !== null) {
                        createDate = tbData[i][key].replace("T", " ")
                    }
                    tempStr += '<td data-id="' + key + '">' + createDate + '</td>';
                } else {
                    tempStr += '<td data-id="' + key + '">' + tbData[i][key] + '</td>';
                }*/
              
                
                
            }
            tempStr = '<tr>' + tempStr + '</tr>';
            tbStr += tempStr;
        }
        $(".table2 tbody").append(tbStr);
    }

    //泵状态处理
    function dealPumpStaFn(statusData) {
        if (statusData == null) {
            return '';
        }
        var pumpNumArr = statusData.split('/');
        var statusStr = '';
        for (var i = 0; i < pumpNumArr.length; i++) {
            switch (pumpNumArr[i]) {
                case "0":
                case "1":
                    statusStr += '<span class="offSta"></span>';
                    break;
                case "2":
                    statusStr += '<span class="onSta"></span>';
                    break;
                case "3":
                    statusStr += '<span class="alarmSta"></span>';
                    break;
                default:
                    break;
            }
        }
        return statusStr;
        //<span class="onSta"></span><span class="offSta"></span><span class="alarmSta"></span>
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

    function layout() {
        var winW = $(window).width();
        var pageWrapTop = $(".page_wrap").offset().top;
        var table2Top = $(".table2_wrap").offset().top;
        $(".table2_wrap").css({ "height": pageWrapTop - table2Top - 6 });
        $(".page_wrap").css({ "width": $(".list_wrap").width() - 2 });
    }
    layout();
    $(window).resize(function () {
        layout();
        tableClone();
    });
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
});

function setPumpName(name, id) {
    $(".pumpName input").val(name);
    $(".pumpName input").attr("data-id", id);
    curPumpId = id;
}
function setJzName(name, id) {
    $(".machName input").val(name);
    $(".machName input").attr("data-id", id);
    curJzId = id;
}

/*导出日志*/
function exportRunLog(pageSize) {
    $(".exportBtn").click(function () {
        var curJzId = $(this).parents("tr").children("td[data-id=pumpJZId]").html();
        var startDate = $("#startTime").val();
        var endDate = $("#endTime").val();
        var pageIndex = $("#currentPage").html() - 1;
        downLoadxsl(startDate, endDate, pageIndex, pageSize);
    });
}

/*下载*/
function downLoadxsl(startDate, endDate, pageIndex, pageSize) {
    $.ajax({
        type: 'POST',
        url: '/V_YCJK/ReportRunDayLog',
        data: {
            "pumpID": curPumpId,
            "pumpJZID": curJzId,
            "pageIndex": pageIndex,
            "pageSize": pageSize,
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
function loadingFunction() {
    var $div = $('<div class="loading" style="position:absolute;left: 50%;top:50%;margin-left: -150px;margin-top: -70px;width: 300px;color:black;text-align:center;line-height: 140px;height: 140px;background: rgba(255,255,0);color: white;border-radius: 8px;"><img style="position: relative;top: 56%;left: 15%;" src="/res/YCJK/img/load1.gif" alt="loading....">正在加载中...</div>');
    $('body').append($div);
};
function loadingMiss() {
    $('.loading').remove();
};
