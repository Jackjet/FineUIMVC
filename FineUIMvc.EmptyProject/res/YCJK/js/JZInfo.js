$(document).ready(function () {
    //click list 

    var urlJson = parseUrl();
    var pumpId = urlJson["pumpID"];
    var jzId = urlJson["jzID"];
    console.log(urlJson);
    urlJson["pumpName"] = decodeURIComponent(urlJson["pumpName"]);
    urlJson["jzName"] = decodeURIComponent(urlJson["jzName"]);



    function parseUrl() {
        var url = window.location.href;
        //  alert(url);
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





    $(".table2_wrap").on('click', "tr", function () {
        $(this).addClass("active").siblings().removeClass("active");

    });

    $('.main_wrap').mCustomScrollbar({
        scrollButtons: {
            enable: true,
            scrollType: "continuous",
            scrollSpeed: 20,
            scrollAmount: 40
        },
        axis: "y",
        set_width: false,
        scrollbarPosition: "inside",
        theme: "minimal-dark",
    });
    
 

   

   
    loadTable();
    function loadTable() {
        $.ajax({
            url: '/V_YCJK/Search_PumpJZDetail',
            data: {
                "pumpID": pumpId,
                "pumpJZID": jzId
            },
            dataType: 'JSON',
            beforeSend: loadingFunction,
            success: function (data) {
                console.log(data);
                console.log(typeof data);
                console.log('jzjzjzjzzj');
                dealData(data.obj[0]);
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

    //处理表头
    var theadJson = {};
    function dealData(pumpData) {
        for (var key in pumpData) {
            switch (key) {
                case "InstallDate":
                case "AcceptanceDate":
                    if (pumpData[key]) {
                        var date = new Date(parseInt(pumpData[key].replace(/[^0-9]/ig, "")));
                        date = date.toLocaleDateString();
                        //  oTime = date.getFullYear() + '-' + parseInt(date.getMonth() + 1) + '-' + date.getDate() + ' ' + date.getHours() + ':' + date.getMinutes + ':' + date.getSeconds();
                        $(".pumpInfo span.fieldValue[data-field=" + key + "]").html(date);
                    }
                    break;
                case "TankIsSharing":
                    if (pumpData[key] == 1) {
                        $(".pumpInfo span.fieldValue[data-field=" + key + "]").html('是');
                    } else {
                        $(".pumpInfo span.fieldValue[data-field=" + key + "]").html('否');
                    }
                    break;
                case "PumpSoaking":
                case "WaterTankSterilizer":
                case "Warning":
                case "WaterQualityDetection":
                case "ControlValve":
                    if (pumpData[key] == 1) {
                        $(".pumpInfo span.fieldValue[data-field=" + key + "]").html('有');
                    } else {
                        $(".pumpInfo span.fieldValue[data-field=" + key + "]").html('无');
                    }
                    break;
                default:
                    $(".pumpInfo span.fieldValue[data-field=" + key + "]").html(pumpData[key]);
                    $(".equipInfo span.fieldValue[data-field=" + key + "]").html(pumpData[key]);
                    break;
            }
     
        }
        // console.log(pumpData["pumpJZ"][0]);
        if (pumpData["pumpJZ"]) {
            var jzData = pumpData["pumpJZ"][0];
            for (var i in jzData) {
                switch (i) {
                    case "PumpJZPeak":
                    case "PumpJZPressReliValve":
                        if (jzData[i] == 1) {
                            $(".equipInfo span.fieldValue[data-field=" + i + "]").html('有');
                        } else {
                            $(".equipInfo span.fieldValue[data-field=" + i + "]").html('无');
                        }
                        break;
                    default:
                        $(".equipInfo span.fieldValue[data-field=" + i + "]").html(jzData[i]);
                        break;
                }
            }
        }
        if (pumpData["pumpVQ"]) {
        
            var str = '';
            console.log(pumpData["pumpVQ"]);
            for (var j = 0; j < pumpData["pumpVQ"].length; j++) {
                var tempStr = '<ul class="ul_pumpInfo2">\
                <li class="col1">\
                    <span class="fieldName">' + pumpData["pumpVQ"][j].QuipmentType + '</span>\
                </li>\
                <li class="col2">\
                    <span class="pumpName fieldName">类型：</span><span class="fieldValue">' + pumpData["pumpVQ"][j].Type + '</span>\
                </li>\
                <li class="col3">\
                    <span class="pumpName fieldName">品牌：</span><span class="fieldValue">' + pumpData["pumpVQ"][j].Brand + '</span>\
                </li>\
                <li class="col4">\
                    <span class=" fieldName">型号：</span><span class="fieldValue">' + pumpData["pumpVQ"][j].Number + '</span>\
                </li>\
                <li class="col5">\
                    <span class=" fieldName">IP端口号：</span><span class="fieldValue">' + pumpData["pumpVQ"][j].Port + '</span>\
                </li>\
          </ul>';
                str += tempStr;
            }
            $(".pumpInfo .infoMode2").empty();
            $(".pumpInfo .infoMode2").append(str);
        }
        
    }


    function layout() {
        var winW = $(window).width();
       
    }
    layout();
    $(window).resize(function () {
        layout();
    });
});
