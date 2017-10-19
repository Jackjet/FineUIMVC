
var urlJson = parseUrl();
function AccBtnClickFn() {
    var dataJson = {};
    $(".accBtn").click(function () {
        var plcAddress = $(this).parent().attr("data-id");
        var text = $(this).parent().children("input").val();
        var dtu = urlJson["dtuCode"];
        dataJson.FPLCAddress = plcAddress;
        dataJson.text = text;
        dataJson.dtu = dtu;
        console.log(dataJson);
       getCommand(dataJson);
    });
}
  
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

function getCommand(dataJson) {
    console.log(dataJson);
    $.ajax({
        url: '/V_CDJK/GetFAMENCommand',
        data: dataJson,
        dataType: 'JSON',
        success: function (data) {
            console.log(data);
        },
        error: function (data) {
            console.log('泵房数据获取出错');
        }
    });
}


$(document).ready(function () {
    AccBtnClickFn();
});