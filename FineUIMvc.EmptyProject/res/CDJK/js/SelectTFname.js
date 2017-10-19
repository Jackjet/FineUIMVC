var tablePageIndex = 0;
var tablePageSize = 20;
var tableSearchText = $("#tableSearchText").val();
var currentPage = $("#currentPage").html();
var tablePageIndex = currentPage - 1;
var totalPage = $("#totalPage").html();
$("#pageSize").html(tablePageSize);

$(document).ready(function () {
  
    loadTable();
    pagingFn(); //分页
    layout();
    $(window).resize(function () {
        layout();
    });
    addScroll();
    searchFn();
});



function loadTable() {
    $.ajax({
        url: '/V_CDJK/SearchTF_Report',
        data: {
            "pageIndex": tablePageIndex,
            "pageSize": tablePageSize,
            "Name": tableSearchText,
            "State": '',
            "ID": ''
        },
        dataType: 'JSON',
        beforeSend: loadingFunction,
        success: function (data) {
            console.log(data);
            console.log('调峰泵站');
            dealTbodyFn(data.data);
            dealPage(data.total);
            tableClone();
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
                            <td data-id="DtuNum">' + tbData[i].DtuNum + '</td>\
                             <td data-id="FName">' + tbData[i].FName + '</td>\
                             <td data-id="TempTime">' + tbData[i].TempTime + '</td>\
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
function tableClone() {
    if ($('.table1 thead').next()) {
        $('.table1 thead').next().remove();
    }
    $('.table1 thead').after($('.table2 tbody').clone());
}

$(".table2_wrap").on('click', "tr", function () {
    var tfId = $(this).children("td[data-id=BaseID]").html();
    var tfName = $(this).children("td[data-id=FName]").html();
    $(this).addClass("active").siblings().removeClass("active");
    var index = parent.layer.getFrameIndex(window.name);
    parent.setTFname(tfId, tfName);
    parent.layer.close(index);
});
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

function layout(){
    var tableWrapH= $("#tableWrap").height();
    var headerH = $(".list_header").height();
    var pageWrapH = $(".page_wrap").height();
    console.log(tableWrapH + 'aaaaa' + headerH + 'bbbb' + pageWrapH+'cccccc');
    $(".list_table").css({ "height": tableWrapH - headerH - pageWrapH -52 });
}

function addScroll() {
    $('.table2_wrap').mCustomScrollbar({
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

function searchFn() {
    $(".searchBtn").click(function () {
        tableSearchText = $("#tableSearchText").val();
        tablePageIndex = 0;
        loadTable();
    });
}