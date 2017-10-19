var baseId = '';
var YLname = '';
$(document).ready(function () {
    //click list 
    var searchText = $("#searchText").val();
    var currentPage = $("#currentPage").html();
    var pageIndex = currentPage - 1;
    var pageSize = 20;
    var totalPage = $("#totalPage").html();
    var state = '';
    
    $("#pageSize").html(pageSize);
    $(".searchBtn").click(function () {
        searchText = $("#searchText").val();
        loadTable();
    });
    $(".table2_wrap").on('click', "tr", function () {
        if (!$(".myTable tr").hasClass("active") && baseId == '') {
            $(".sectionBox1").hide();
            $(".sectionBox2").show();
          //  radialBar();
          //  showChart();
        }
        $(this).addClass("active").siblings().removeClass("active");
        baseId = $(this).children("td[data-id=BaseID]").html();
        YLname = $(this).children("td[data-id=FName]").html();
      
        //getWaterElec(pumpJzId);
        //get7daysWaterUse(pumpJzId);
        //get7daysElecUse(pumpJzId);
        //getInOutWaPress(pumpJzId, 10);
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
            url: '/V_CDJK/SearchYL_Report',
            data: {
                "pageIndex": pageIndex,
                "pageSize": pageSize,
                "Name": searchText,
                "State": state
            },
            dataType: 'JSON',
            beforeSend: loadingFunction,
            success: function (data) {
                console.log(data);
                console.log('压力列表');
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
                switch (key){
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
                        tbData[i][key] = tbData[i][key] ? tbData[i][key]: "";
                }
           
            }
            var tempStr = '<tr>\
                            <td data-id="BaseID" data-value="' + tbData[i].BaseID + '">' + tbData[i].BaseID + '</td>\
                            <td class="status" data-id="FIsAlarm"><i class="status_' + tempState + '"></i></td>\
                             <td data-id="TempTime">' + tbData[i].TempTime + '</td>\
                            <td data-id="FDTUCode">' + tbData[i].FDTUCode + '</td>\
                            <td data-id="FName">' + tbData[i].FName + '</td>\
                            <td data-id="FMpa">' + tbData[i].FMpa + '</td>\
                            <td data-id="FLL">' + tbData[i].FLL + '</td>\
                            <td data-id="FMpaUp">' + tbData[i].FMpaUp + '</td>\
                            <td data-id="FMpaDown">' + tbData[i].FMpaDown + '</td>\
                            <td data-id="FMapAddress">' + tbData[i].FMapAddress + '</td>\
                            <td data-id="FBatt">' + tbData[i].FBatt + '</td>\
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
        /*	console.log('layout');
		   console.log($('#mCSB_1_container').position().left);*/

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
        $('.sectionBox').mCustomScrollbar({
            scrollbarPosition: "inside",
            theme: "minimal-dark"
        });
    }
    var totalCurve = {
        "icon": "lineBar_ico",
        "name": "总压力曲线",
        "url": "/YCJK/V_CDJK/YL_totalYLcurve"
    };
    var curUserName = $("#curUserName").val();
    if (curUserName == 'panda') {
        reportYL.push(totalCurve);
    }
    createReportList(reportYL);
    showReportFn();
    cd_yaliReportList();
    //initRightChart();
    //getJzStatus();
    //getPumpJzCount();
    //getWaterUseTop10();
    //getElecUseTop10();
    //rightSectionScroll();
});