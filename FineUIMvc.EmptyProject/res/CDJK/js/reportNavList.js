//showReportBtn
function showReportFn() {
    $(".showReportBtn").click(function () {
        $(this).hide();
        $(".reportNav_wrap").show();
        $(".report_wrap").stop().animate({ "height": "100%" }, 600);
    });
    $(".hideReportBtn").click(function () {
        $(".report_wrap").stop().animate({ "height": 50 }, 400, function () {
            $(".reportNav_wrap").hide();
            $(".showReportBtn").show();
        });

    });
}

function createReportList(tempArr) {
    var tempStr = '';
    for (var i = 0; i < tempArr.length; i++) {
        if (tempArr[i].size) {
            tempStr += '<li  data-url="' + tempArr[i].url + '" data-key="' + tempArr[i].dataKey + '" data-size="' + tempArr[i].size + '"><i class="' + tempArr[i].icon + '" ></i><a>' + tempArr[i].name + '</a></li>';
        } else {
            tempStr += '<li  data-url="' + tempArr[i].url + '" data-key="' + tempArr[i].dataKey + '"><i class="' + tempArr[i].icon + '" ></i><a>' + tempArr[i].name + '</a></li>';
        }
       
    }
    $(".report_list").empty();
    $(".report_list").append(tempStr);
}

function tf_waterInOutPress() {
    $(".report_list").on("click", "li", function () {
        var dataKey = $(this).attr("data-key");
           
            var TFnameCode = encodeURIComponent(TFname);
            var urlStr = $(this).attr("data-url");
            var areaWidth = $(this).attr("data-size");
            var winW = "";
            var winH = "";
            if (areaWidth) {
                winW = areaWidth.split(",")[0];
                winH = areaWidth.split(",")[1];
            } else {
                winW = '1100px';
                winH = '90%';
            }

              layer.open({
                   type: 2,
                   anim: 3,
                   shade: .6,
                   title: false,
                   shadeClose: true,
                   area: [winW, winH],
                   content: urlStr + '?TFname=' + TFnameCode + '&baseId=' + baseId + '&chartName=' + dataKey,
                
               });           
           
        });
}
/*测点-压力报表*/

function cd_yaliReportList() {
    $(".report_list").on("click", "li", function () {
        var dataKey = $(this).attr("data-key");
        var urlStr = $(this).attr("data-url");
       
        var areaWidth = $(this).attr("data-size");
        var winW = "";
        var winH = "";
        if (areaWidth) {
            winW = areaWidth.split(",")[0];
            winH = areaWidth.split(",")[1];
        } else {
            winW = '1100px';
            winH = '90%';
        }
        if (!YLname) {
            $(".table2_wrap tbody tr:first").trigger("click");
        }
            if (urlStr == '/YCJK/V_CDJK/YL_totalYLcurve') {
                var TFname = '';
            } else {
                console.log(YLname + 'aaaaaaaaa');
                var TFname = encodeURIComponent(YLname);
            }
   
        layer.open({
            type: 2,
            anim: 3,
            shade: .6,
            title: false,
            shadeClose: true,
            area: [winW, winH],
            content: urlStr + '?TFname=' + TFname + '&baseId=' + baseId,
            success: function () {
                //  alert('OK');
            }
        });
       
    });
}

/*测点-流量报表*/

function cd_flowReportList() {
    $(".report_list").on("click","li",function () {
        var dataKey = $(this).attr("data-key");
       
        var TFname = encodeURIComponent(FlowName);
        var urlStr = $(this).attr("data-url");
        var areaWidth = $(this).attr("data-size");
        var winW="";
        var winH = "";
        if (areaWidth) {
            winW = areaWidth.split(",")[0];
            winH = areaWidth.split(",")[1];
        }else {
            winW = '1100px';
            winH = '90%';
        }
              layer.open({
                    type: 2,
                    anim: 3,
                    shade: .6,
                    title: false,
                    shadeClose: true,
                    area: [winW, winH],
                    content:  urlStr + '?TFname=' + TFname + '&baseId=' + baseId,
                    success: function () {
                        //  alert('OK');
                    }
                });
           
    });
}

/*测点-阀门报表*/

function cd_famenReportList() {
    $(".report_list").on("click", "li", function () {
       
        var TFname = encodeURIComponent(FMname);
        var urlStr = $(this).attr("data-url");
        var areaWidth = $(this).attr("data-size");
        var winW = "";
        var winH = "";
        if (areaWidth) {
            winW = areaWidth.split(",")[0];
            winH = areaWidth.split(",")[1];
        } else {
            winW = '1100px';
            winH = '90%';
        }
                layer.open({
                    type: 2,
                    anim: 3,
                    shade: .6,
                    title: false,
                    shadeClose: true,
                    area: [winW, winH],
                    content: urlStr+'?TFname=' + TFname + '&baseId=' + baseId+'&dtuCode=' + dtuCode,
                   
                });
          
    });
}


/*二供报表*/
function eg_ReportList() {
    $(".report_list").on("click", "li", function () {
        var dataKey = $(this).attr("data-key");
            var jzID = $("tr.active").children("td[data-id=pumpJZId]").html();
            var PumpJZName = $("tr.active").children("td[data-id=jzName]").html();
            var pumpID = $("tr.active").children("td[data-id=pumpID]").html();
            var jzName = encodeURIComponent(PumpJZName);
            var pumpName = encodeURIComponent($("tr.active").children("td[data-id=pName]").html());
            var dtuCode = $("tr.active").children("td[data-id=DTUCode]").html();
            var chartTitle = encodeURIComponent($(this).children("a").html());
            var urlStr = $(this).attr("data-url");
            var areaWidth = $(this).attr("data-size");
            var winW = "";
            var winH = "";
            if (areaWidth) {
                winW = areaWidth.split(",")[0];
                winH = areaWidth.split(",")[1];
            } else {
                winW = '1100px';
                winH = '90%';
            }
            if (!(jzID && pumpID)) {
                 jzID = '';
                 pumpID = '';
                 jzName = '';
                 pumpName = '';
            }
            if (dataKey == 'pumpAutoControl' && !dtuCode) {
                alert('请选择机组！');
                return;
            }
            layer.open({
                type: 2,
                anim: 3,
                shade: .6,
                title: false,
                shadeClose: true,
                area: [winW, winH],
                content: urlStr+'?jzName=' + jzName + '&jzID=' + jzID + '&pumpID=' + pumpID + '&pumpName=' + pumpName + '&chartTitle=' + chartTitle + '&dataKey=' + dataKey + '&dtuCode=' + dtuCode,
                success: function () {
                    //  alert('OK');
                }
            });
 /*       switch (dataKey) {
            case "egRunLog":
                layer.open({
                    type: 2,
                    anim: 3,
                    shade: .6,
                    title: false,
                    shadeClose: true,
                    area: ['1100px', '90%'],
                    content: '/YCJK/V_Report/RunDayLog?jzName=' + jzName + '&jzID=' + jzID + '&pumpID=' + pumpID + '&pumpName=' + pumpName,
                    success: function () {
                        //  alert('OK');
                    }
                });
                break;
            case "egWaterInOutPress":
                layer.open({
                    type: 2,
                    anim: 3,
                    shade: .6,
                    title: false,
                    shadeClose: true,
                    area: ['1100px', '90%'],
                    content: '/YCJK/V_Report/egWaterInOutPress?jzName=' + jzName + '&jzID=' + jzID + '&pumpID=' + pumpID + '&pumpName=' + pumpName,
                    success: function () {
                        //  alert('OK');
                    }
                });
                break;
            case "egCompreReport":
                layer.open({
                    type: 2,
                    anim: 3,
                    shade: .6,
                    title: false,
                    shadeClose: true,
                    area: ['1100px', '90%'],
                    content: '/YCJK/V_Report/egCompreReport?jzName=' + jzName + '&jzID=' + jzID + '&pumpID=' + pumpID + '&pumpName=' + pumpName,
                    success: function () {
                        //  alert('OK');
                    }
                });
                break;
            case "dayHour_press":
                layer.open({
                    type: 2,
                    anim: 3,
                    shade: .6,
                    title: false,
                    shadeClose: true,
                    area: ['1100px', '90%'],
                    content: '/YCJK/V_Report/yearMonthDayChart?jzName=' + jzName + '&jzID=' + jzID + '&pumpID=' + pumpID + '&pumpName=' + pumpName+'&dataKey=' + dataKey+'&chartTitle='+chartTitle,
                    success: function () {
                        //  alert('OK');
                    }
                });
                break;
            case "dayHour_elec":
                layer.open({
                    type: 2,
                    anim: 3,
                    shade: .6,
                    title: false,
                    shadeClose: true,
                    area: ['1100px', '90%'],
                    content: '/YCJK/V_Report/dayHourElec?jzName=' + jzName + '&jzID=' + jzID + '&pumpID=' + pumpID + '&pumpName=' + pumpName+'&dataKey=' + dataKey + '&chartTitle=' + chartTitle,
                    success: function () {
                        //  alert('OK');
                    }
                });
                break;
            case "dayHour_flow":
                layer.open({
                    type: 2,
                    anim: 3,
                    shade: .6,
                    title: false,
                    shadeClose: true,
                    area: ['1100px', '90%'],
                    content: '/YCJK/V_Report/dayHourFlow?jzName=' + jzName + '&jzID=' + jzID + '&pumpID=' + pumpID + '&pumpName=' +pumpName+'&dataKey=' + dataKey + '&chartTitle=' + chartTitle,
                    success: function () {
                        //  alert('OK');
                    }
                });
                break;
            case "monthDay_press":
                layer.open({
                    type: 2,
                    anim: 3,
                    shade: .6,
                    title: false,
                    shadeClose: true,
                    area: ['1100px', '90%'],
                    content: '/YCJK/V_Report/MonthDayPress?jzName=' + jzName + '&jzID=' + jzID + '&pumpID=' + pumpID + '&pumpName=' +pumpName+' &dataKey=' + dataKey + '&chartTitle=' + chartTitle,
                    success: function () {
                        //  alert('OK');
                    }
                });
                break;
            case "monthDay_elec":
                layer.open({
                    type: 2,
                    anim: 3,
                    shade: .6,
                    title: false,
                    shadeClose: true,
                    area: ['1100px', '90%'],
                    content: '/YCJK/V_Report/MonthDayElec?jzName=' + jzName + '&jzID=' + jzID + '&pumpID=' + pumpID + '&pumpName=' +pumpName+' &dataKey=' + dataKey + '&chartTitle=' + chartTitle,
                    success: function () {
                        //  alert('OK');
                    }
                });
                break;
            case "monthDay_flow":
                layer.open({
                    type: 2,
                    anim: 3,
                    shade: .6,
                    title: false,
                    shadeClose: true,
                    area: ['1100px', '90%'],
                    content: '/YCJK/V_Report/MonthDayFlow?jzName=' + jzName + '&jzID=' + jzID + '&pumpID=' + pumpID + '&pumpName=' + pumpName+'&dataKey=' + dataKey + '&chartTitle=' + chartTitle,
                    success: function () {
                        //  alert('OK');
                    }
                });
                break;
            case "yearMonth_press":
                layer.open({
                    type: 2,
                    anim: 3,
                    shade: .6,
                    title: false,
                    shadeClose: true,
                    area: ['1100px', '90%'],
                    content: '/YCJK/V_Report/YearMonthPress?jzName=' + jzName + '&jzID=' + jzID + '&pumpID=' + pumpID + '&pumpName=' +pumpName+' &dataKey=' + dataKey + '&chartTitle=' + chartTitle,
                    success: function () {
                        //  alert('OK');
                    }
                });
                break;
            case "yearMonth_elec":
                layer.open({
                    type: 2,
                    anim: 3,
                    shade: .6,
                    title: false,
                    shadeClose: true,
                    area: ['1100px', '90%'],
                    content: '/YCJK/V_Report/YearMonthElec?jzName=' + jzName + '&jzID=' + jzID + '&pumpID=' + pumpID + '&pumpName=' +pumpName+'&dataKey=' + dataKey + '&chartTitle=' + chartTitle,
                    success: function () {
                        //  alert('OK');
                    }
                });
                break;
            case "yearMonth_flow":
                layer.open({
                    type: 2,
                    anim: 3,
                    shade: .6,
                    title: false,
                    shadeClose: true,
                    area: ['1100px', '90%'],
                    content: '/YCJK/V_Report/YearMonthFlow?jzName=' + jzName + '&jzID=' + jzID + '&pumpID=' + pumpID + '&pumpName=' +pumpName+'&dataKey=' + dataKey + '&chartTitle=' + chartTitle,
                    success: function () {
                        //  alert('OK');
                    }
                });
                break;
            case "rangePowerUse":
                layer.open({
                    type: 2,
                    anim: 3,
                    shade: .6,
                    title: false,
                    shadeClose: true,
                    area: ['1100px', '90%'],
                    content: '/YCJK/V_Report/rangePowerUse?jzName=' + jzName + '&jzID=' + jzID + '&pumpID=' + pumpID + '&pumpName=' +pumpName+'&chartTitle=' + chartTitle,
                    success: function () {
                        //  alert('OK');
                    }
                });
                break;
            case "pumpAutoControl":
                if (dtuCode) {
                    layer.open({
                        type: 2,
                        anim: 3,
                        shade: .6,
                        title: false,
                        shadeClose: true,
                        area: ['1100px', '90%'],
                        content: '/YCJK/V_Report/pumpAutoControl?dtuCode=' + dtuCode,
                        success: function () {
                            //  alert('OK');
                        }
                    });
                } else {
                    alert("请选择机组");
                }
                break;
                //
            default:
                break;
        }*/
    });
}