var dtuCode = '';
var urlJson = parseUrl();
dtuCode = urlJson["dtuCode"];
var dataJson = {};
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



function getCommand() {
    console.log(dataJson);
    $.ajax({
        url: '/V_YCJK/GetCommand',
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

function btnClickFn() {
    dataJson.dtu = dtuCode;
    $(".autoControl").click(function () {
        dataJson.text = 1;
        dataJson.FPLCAddress = 'V40003';
        getCommand();
    });
    $(".menufControl").click(menufBtnClick);
    function menufBtnClick() {
        dataJson.text = 0;
        dataJson.FPLCAddress = 'V40003';
        getCommand();
    }

    $(".remoteAuto").click(function () {
        dataJson.text = 2;
        dataJson.FPLCAddress = 'V40003';
        getCommand();
    });
    $(".accBtn").click(function () {
        var upText = $(this).parent().children(".upLimit").val();
        var downText = $(this).parent().children(".downLimit").val();
        var inOutType = $(this).parent().attr("class");
        if (inOutType == "waterIn_wrap") {
            if (upText) {
                dataJson.text = upText;
                dataJson.FPLCAddress = 'V40074';
                getCommand();
            }
            if (downText) {
                dataJson.text = downText;
                dataJson.FPLCAddress = 'V40073';
                getCommand();
            }
        } else if (inOutType == "waterOut_wrap") {
            if (upText) {
                dataJson.text = upText;
                dataJson.FPLCAddress = 'V40076';
                getCommand();
            }
            if (downText) {
                dataJson.text = downText;
                dataJson.FPLCAddress = 'V40075';
                getCommand();
            } 
        }
    });
 /*   function AccBtnClickFn(that) {

    }*/

}
$(document).ready(function () {
    btnClickFn();
});