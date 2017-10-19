$(document).ready(function () {
    //click list 
    var currentPage = $("#currentPage").html();
    var pageIndex = currentPage - 1;
    var pageSize = 20;
    var totalPage = $("#totalPage").html();
  
    var urlJson = parseUrl();
    urlJson["pumpName"] = decodeURIComponent(urlJson["pumpName"]);
    console.log(urlJson);
    var searchText = '';
    $("#pageSize").html(pageSize);
    function parseUrl() {
        var url = window.location.href;
        console.log(url);
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

 
    $(".searchBtn").click(function () {
        searchText = $(".searchText").val();
        loadTable();
    });
    $(".table2_wrap").on('click', "tr", function () {
        var pumpId = $(this).children(".ID").html();
        var pumpName = $(this).children(".PName").html();
        $(this).addClass("active").siblings().removeClass("active");
     // alert(pumpId);
        var index = parent.layer.getFrameIndex(window.name);
        parent.setPumpName(pumpName,pumpId);
        parent.layer.close(index);
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
            $("#currentPage").html(pageIndex + 1);
            loadTable();
        }
    });

    function dealPage(total) {
        totalPage = Math.ceil(total / pageSize);
    }
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
                dealWidth();
            }
        }
    });

    function dealWidth() {
        $(".table2  tr:first-child td").each(function (j, v) {
            var tempW = $(this).width();
            $(".table1 thead th").eq(j).width(tempW);

        });
    }
    loadTable();
    function loadTable() {
        // alert(state);
        $.ajax({
            url: '/Window/getPumpList',
            data: {
                "pageIndex": pageIndex,
                "pageSize": pageSize,
                "PName": searchText
            },
            dataType: 'JSON',
            beforeSend: loadingFunction,
            success: function (data) {
                console.log(typeof data);
                console.log(data);
                console.log(data.data);
                dealTbodyFn(data.data);
                dealPage(data.total);
                dealWidth();
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
            tbStr += '<tr><td class="ID">' + tbData[i].ID + '</td><td class="PName">' + tbData[i].PName + '</td><td class="CustomerName">' + tbData[i].CustomerName + '</td><td class="ProvinceCity">' + tbData[i].ProvinceCity + '</td></tr>';      
        }
        $(".table2 tbody").append(tbStr);
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
        dealWidth();
        $(".page_wrap").css({ "width": $(".list_wrap").width() - 2 });
    }
    layout();
    $(window).resize(function () {
        layout();

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