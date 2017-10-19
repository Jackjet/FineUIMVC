/// <reference path="../../../Areas/YCJK/Views/V_YCJK/WarningAlarm.cshtml" />
/// <reference path="../../../Areas/YCJK/Views/V_YCJK/WarningAlarm.cshtml" />
var map;
var indexForRightItem = 0;
var mapType = [BMAP_NORMAL_MAP, BMAP_SATELLITE_MAP, BMAP_PERSPECTIVE_MAP];
var iframeUrl = ['YCJK/V_YCJK/JZInfo', 'YCJK/V_Report/RunDayLog', 'http://weibo.com', 'YCJK/V_YCJK/dataReport', 'YCJK/V_YCJK/WarningAlarm', 'YCJK/V_YCJK/RelationFile'];


var threePicBgArr = {
   
};


var sheBeiStatePic = {
    'stop': '/res/YCJK/img/pumpstatus_stop.gif',
    'warn': '/res/YCJK/img/pumpstatus_fault.gif',
    'run': '/res/YCJK/img/pumpstatus_run.gif'
};


var shuiChangID;
var marker;

var center;
var currentShuiChangData = [];
var pumpDataGetTimer;
var currentInd;
$(function () {

    shuiChangID=$('.pageLeft').attr('data-sid');
 
    flowS1Can();
    judgeScreen();
    pageLeftBotShowFn();
    pageLeftBotArrowFn();
    $(window).resize(function () {
        judgeScreen();
        pageLeftBotShowFn();
       
    });


    getMap();
    rightShowBtnFn();
    showHideBtnFn();

    leftBotNavFn();
    rightBotSHowHideFn();
    midScroll();

    rightTopScroll();
    
    threeBoxScroll();
   

   // pumpDataGet(v.pumpId)

    shuiChangDataGet(shuiChangID);
    clearInterval(pumpDataGetTimer);
    window.setInterval(function () {
        // pumpDataGetNew(.pumpId);


    }, 5000);
    pumpDataGetTimer = setInterval(function () {
       
       // rightDituPressGet(currentPumpData);
      
        //  pumpDataGetNew(v.pumpId);

        shuiChangDataGetNew(shuiChangID);

        shuiChangDataUse(currentShuiChangData);
    }, 4000);


    rightDataSetTab();

   

    selectFn($('.selectBox'));

    
});



//地图初始化
function mapInit(type, zoom, maxZo, minZo, themeStyle, center) {
    var type = type ? type : BMAP_NORMAL_MAP;
    var zoom = zoom ? zoom : 17;
    var max = maxZo ? maxZo : 19;
    var min = minZo ? minZo : 7;
    //alert(center);
    var themeStyle = themeStyle ? themeStyle : 'light';
    var center = center ? center : '121.191705, 31.166028';
    map = new BMap.Map("mapBox", { mapType: type, maxZoom: max, minZoom: min });
    map.centerAndZoom(new BMap.Point(center.split(',')[0], center.split(',')[1]), zoom);
    map.enableScrollWheelZoom();
    map.setMapStyle({ style: themeStyle });



}

//获取地图
function getMap() {

    mapInit(0, 0, 0, 0, 0, 0);
}

function overlaycomplete(e) {

}


//根据屏幕 大小判断 如何显示 中部和右部
function judgeScreen() {
    var $rightTopH = $('.pageRightTop').height();
    $('.rightTopMainBox').height($rightTopH - 38 + 'px');
    var $W = $(window).width();
    if ($W <= 1366) {
        $('.pageRight').css('right', '-360px');
        $('.pageMid').css('right', '-360px');
        $('.pageLeft').css('right', '12px');
        $('.midShow').show();
        $('.rightShow').show();
        $('.threeSection .threePicBox').css('padding-top', '-20px');
    } else if ($W > 1366 && $W <= 1600) {
        $('.pageRight').css('right', '-360px');
        $('.pageMid').css('right', '0');
        $('.pageLeft').css('right', '362px');
        $('.midShow').hide();
        $('.rightShow').show();
        $('.threeSection .threePicBox').css('padding-top', '10px');
    } else if ($W > 1600) {
        $('.pageRight').css('right', '0');
        $('.pageMid').css('right', '348px');//362
        $('.pageLeft').css('right', '724px');
        $('.midShow').hide();
        $('.rightShow').hide();
        $('.threeSection .threePicBox').css('padding-top', '100px');
    }
}



//rightShowBtn  的点击 事件

function rightShowBtnFn() {
    $('.midShow').click(function () {
        var $W = $(window).width();
        if ($W <= 1366) {
            $(this).hide().siblings().show();
            $('.pageRight').css('right', '-360px');
            $('.pageMid').css('right', '0');
            $('.pageLeft').css('right', '12px');

       

        } else if ($W > 1366 && $W <= 1600) {

            $(this).hide().siblings().show();
            $('.pageRight').css('right', '-360px');
            $('.pageMid').css('right', '0');
            $('.pageLeft').css('right', '362px');


        } else if ($W > 1600) {

            $(this).hide();
            if ($(".rightShow").is(":hidden")) {
                $('.pageRight').css('right', '0');
                $('.pageMid').css('right', '348px');//362
                $('.pageLeft').css('right', '724px');
            } else {
                $('.pageRight').css('right', '-360px');
                $('.pageMid').css('right', '0');
                $('.pageLeft').css('right', '362px');
            }
          

        }
    });
    $('.rightShow').click(function () {
        var $W = $(window).width();
        if ($W <= 1366) {
            $(this).hide().siblings().show();
            $('.pageRight').css('right', '0');
            $('.pageMid').css('right', '-360px');
            $('.pageLeft').css('right', '12px');

        } else if ($W > 1366 && $W <= 1600) {
            $(this).hide().siblings().show();
            $('.pageRight').css('right', '0');
            $('.pageMid').css('right', '-360px');
            $('.pageLeft').css('right', '362px');
        } else if ($W > 1600) {
            $(this).hide();
            if ($(".midShow").is(":hidden")) {
                $('.pageRight').css('right', '0');
                $('.pageMid').css('right', '348px');//362
                $('.pageLeft').css('right', '724px');
            } else {
                $('.pageRight').css('right', '0');
                $('.pageMid').css('right', '-360px');
                $('.pageLeft').css('right', '362px');
            }
        }
    });
}


//中部 的滚动条

function midScroll() {
    $('.pageMidBox').mCustomScrollbar({
        scrollbarPosition: "inside",
        theme: "minimal-dark"
    });
}


//右侧上部的 滚动条
function rightTopScroll() {

    $('.rightTopMainBox').mCustomScrollbar({
        scrollbarPosition: "inside",
        theme: "minimal-dark"
    });
}


//中部和 右边 的  隐藏按钮
function showHideBtnFn() {
    $('.showHideBtn1').click(function () {
        var $W = $(window).width();
        if ($W <= 1366) {
            $('.pageMid').animate({ 'right': '-360px' }, 300);
            $('.pageLeft').css('right', '12px');
            $('.midShow').show();
        } else if ($W > 1366 && $W <= 1600) {
            $('.midShow').show();
            $('.pageMid').animate({ 'right': '-360px' }, 300);
            if ($(".rightShow").is(":hidden")) {
                $('.pageLeft').css('right', '362px');
            } else {
                $('.pageLeft').animate({ 'right': '12px' }, 300);
            }

        } else if ($W > 1600) {
            $('.midShow').show();
            $('.pageMid').animate({ 'right': '-360px' }, 300);
            if ($(".rightShow").is(":hidden")) {
                $('.pageLeft').animate({ 'right': '362px' }, 300);
            } else {
                $('.pageLeft').animate({ 'right': '12px' }, 300);
            }

        }


    });


    $('.showHideBtn2').click(function () {
        var $W = $(window).width();
        if ($W <= 1366) {
            $('.pageRight').animate({ 'right': '-360px' }, 300);
            $('.pageLeft').css('right', '12px');
            $('.rightShow').show();
        } else if ($W > 1366 && $W <= 1600) {
            $('.rightShow').show();
            $('.pageRight').animate({ 'right': '-360px' }, 300);
            if ($(".midShow").is(":hidden")) {

                $('.pageLeft').css('right', '362px');
            } else {
                $('.pageLeft').animate({ 'right': '12px' }, 300);
            }

        } else if ($W > 1600) {
            $('.rightShow').show();
            $('.pageRight').animate({ 'right': '-360px' }, 300);
            if ($(".midShow").is(":hidden")) {
                $('.pageMid').animate({ 'right': '0' }, 300);
                $('.pageLeft').animate({ 'right': '362px' }, 300);
            } else {
                $('.pageLeft').animate({ 'right': '12px' }, 300);
            }

        }


    });
}

//右侧  底部的 地图视频 框的显示 隐藏
function rightBotSHowHideFn() {
    $('.mapBoxShowHide').click(function () {
        var $pageRightBoxH = $('.pageRightBox').height();
        var $pageRightBotH = $('.pageRightBot').height();
        if ($(this).hasClass('show')) {

            $(this).removeClass('show');
            $('.pageRightBot').animate({ 'marginBottom': '-356px' }, 300);
            $('.pageRightTop').animate({ 'bottom': '12px' }, 100, function () {
                var $rightTopH = $('.pageRightTop').height();
                //alert($rightTopH);
                $('.rightTopMainBox').height($rightTopH - 38 + 'px');
            });

        } else {
            $(this).addClass('show');
            $('.pageRightBot').animate({ 'marginBottom': '0' }, 300);
            $('.pageRightTop').animate({ 'bottom': $pageRightBotH + 10 + 'px' }, 100, function () {

                var $rightTopH = $('.pageRightTop').height();
                $('.rightTopMainBox').height($rightTopH - 38 + 'px');
            });

        }
    });
}



//pageLeftBot 的 显示事件

function pageLeftBotShowFn() {
    var $H = $(window).height();
    if ($('.botArrow').hasClass('active')) {
        $('.pageLeftBot').css('marginTop', 0);//0
    } else {
        $('.pageLeftBot').css('marginTop', $H - 90);

    }

}

//左侧  底部  的   nav点击 函数
function leftBotNavFn() {
    $('.leftBotNav li').click(function () {

        var index = $(this).index();

        if ($(this).hasClass('active')) {
            if ($('.botArrow').hasClass('active')) {
                $('.leftBotFrame').attr('src', F.baseUrl + iframeUrl[index] + '?pumpID=' + v.pumpId + '&pumpName=' + encodeURIComponent(currentPumpData.PName) + '&jzID=' + currentJZ + '&jzName=' + encodeURIComponent(currentJZName) + '&pageType=0');
                // alert($('.leftBotFrame').attr('src'));
                return;
            } else {
                $('.botArrow').addClass('active');
                $('.pageLeftBot').animate({ 'marginTop': 0 }, 400);//0
            }
        } else {
            $(this).addClass('active').siblings().removeClass('active');


            if ($('.botArrow').hasClass('active')) {
                $('.leftBotFrame').attr('src', F.baseUrl + iframeUrl[index] + '?pumpID=' + v.pumpId + '&pumpName=' + encodeURIComponent(currentPumpData.PName) + '&jzID=' + currentJZ + '&jzName=' + encodeURIComponent(currentJZName) + '&pageType=0');
                // alert($('.leftBotFrame').attr('src'));
                return;
            } else {
                $('.leftBotFrame').attr('src', '');
                $('.botArrow').addClass('active');
                $('.pageLeftBot').animate({ 'marginTop': 0 }, 400);//0
                setTimeout(function () {
                    $('.leftBotFrame').attr('src', F.baseUrl + iframeUrl[index] + '?pumpID=' + v.pumpId + '&pumpName=' + encodeURIComponent(currentPumpData.PName) + '&jzID=' + currentJZ + '&jzName=' + encodeURIComponent(currentJZName) + '&pageType=0');
                    // alert($('.leftBotFrame').attr('src'));
                }, 450);
            }
        }
    });
}


//pageLeftBot  右侧 箭头 点击  事件函数

function pageLeftBotArrowFn() {
    $('.botArrow').click(function () {
        var $H = $(window).height();
        if ($(this).hasClass('active')) {
            $(this).removeClass('active');
            $('.pageLeftBot').animate({ 'marginTop': $H - 90 }, 400);
        } else {
            $(this).addClass('active');
            $('.pageLeftBot').animate({ 'marginTop': 0 }, 400);//0
        }

    });
}



// 3D 框的  滚动条事件

function threeBoxScroll() {
    $('.threeSection').mCustomScrollbar({
        scrollbarPosition: "inside",
        theme: "minimal-dark"
    });
}


//泵房数据的  刷新 函数  (暂时 没有用到 这个 函数)
function pumpDataGetNew(bengfangID) {
    if (bengfangID) {

        $.ajax({
            url: '/V_YCJK/Search_Pump_JZReportList',
            data: {
                'pumpID': bengfangID
            },
            success: function (data) {
                var dataJSON = JSON.parse(data);
                dataJSON = dataJSON.obj;


                currentPumpData.length = 0;
                currentPumpData = dataJSON[0];
                //center = dataJSON[0].PLngLat;
                //map.setCenter(new BMap.Point(center.split(',')[0], center.split(',')[1]));




            },
            error: function (data) {
                console.log('泵房数据获取出错');
            }
        });
    }
}



//泵房数据的  获取 函数
function pumpDataGet(bengfangID) {
    if (bengfangID) {

        $.ajax({
            url: '/V_YCJK/Search_Pump_JZReportList',
            data: {
                'pumpID': bengfangID
            },
            success: function (data) {
                var dataJSON = JSON.parse(data);
                dataJSON = dataJSON.obj;


                currentPumpData.length = 0;
                currentPumpData = dataJSON[0];
                var jzLen = (currentPumpData.pumpJZ).length;
                // alert(jzLen);
                var labelTop = -(jzLen * 34 + 12);

                center = dataJSON[0].PLngLat;
                map.setCenter(new BMap.Point(center.split(',')[0], center.split(',')[1]));
                var myIcon = new BMap.Icon("/res/images/map/addMarker.png", new BMap.Size(50, 50), { anchor: new BMap.Size(10, 10) });
                marker = new BMap.Marker(new BMap.Point(center.split(',')[0], center.split(',')[1]), { icon: myIcon });


                var labelStr = '';
                var JZArr = currentPumpData.pumpJZ;



                $(JZArr).each(function (ind, val) {

                    var isAlarm = JZArr[ind].IsAlarm;
                    var status;

                    var jzStateCurrent = JZArr[ind].D_Data[0]['FOnLine'];
                    var inPress = JZArr[ind].D_Data[0]['F41006'];
                    var outPress = JZArr[ind].D_Data[0]['F41007'];


                    switch (isAlarm) {
                        case 0: {
                            switch (jzStateCurrent) {
                                case 0: {
                                    status = 'gray';
                                    // statusTxt = '离线';
                                    break;
                                }
                                case 1: {
                                    status = 'blue';
                                    // statusTxt = '在线';
                                    break;
                                }
                                    //case 2: {
                                    //    status = 'listItemPicSta2';
                                    //    statusTxt = '报警';
                                    //    break;
                                    //}
                            }
                            break;
                        }
                        case 1: {
                            status = 'red';
                            //statusTxt = '报警';
                            break;
                        }
                    }

                    labelStr += '<li class="mapLabelItem clearfix">' +
                                           '<div class="areaName ' + status + '">[' + val.PumpJZArea + ']</div>' +
                                           '<div class="inPress">' +
                                                '<div class="bgBox"></div><div class="txtBox">进：' + inPress + '</div>' +
                                           '</div>' +
                                           '<div class="outPress">' +
                                                '<div class="bgBox"></div><div class="txtBox">出：' + outPress + '</div>' +
                                           '</div>' +

                                       ' </li>';
                });



                var label = new BMap.Label('<div>' +
                        '<ul class="mapLabelItemWrap">' +
                            labelStr +
                        '</ul>' +
                        '<div class="sanJiao"></div>' +
                    '</div>', {
                        offset: new BMap.Size(-118, labelTop),                  //label的偏移量，为了让label的中心显示在点上
                        position: new BMap.Point(center.split(',')[0], center.split(',')[1])
                    });

                label.setStyle({
                    position: "absolute",
                    color: "red",
                    fontSize: "14px",
                    width: "286px",
                    textAlign: "center",
                    background: "#fff",
                    border: "1px solid #999999",
                    boxShadow: "6px 3px 4px 1px  #bbb",
                    cursor: "pointer"
                });
                marker.setLabel(label);
                map.addOverlay(marker);



            },
            error: function (data) {
                console.log('泵房数据获取出错');
            }
        });
    }
}



//水厂 数据 刷新 数据的获取

function shuiChangDataGetNew(shuiChangID) {
    $.ajax({
        url: '/ShuiChang/HS_S1_DataMain',
        data: {
            'waterID': shuiChangID
        },
        success: function (data) {
            // alert(data);
            var dataJSON = JSON.parse(data);
            console.log(dataJSON);
            currentShuiChangData = dataJSON.obj[0];
            console.log(dataJSON);
            
        },
        error: function (data) {
            alert(data);
        }
    });
}

//水厂 数据的 获取函数 

function shuiChangDataGet(shuiChangID) {
    $.ajax({
        url: '/ShuiChang/HS_S1_DataMain',
        data: {
            'waterID': shuiChangID
        },
        success: function (data) {
           // alert(data);
            var dataJSON = JSON.parse(data);
            currentShuiChangData = dataJSON.obj[0];
            shuiChangDataUse(currentShuiChangData);
        },
        error: function (data) {
            alert(data);
        }
    });
}



//水厂  数据的使用
function shuiChangDataUse(shuiChangDataArr) {
    console.log('琴瑟和鸣');
    console.log(shuiChangDataArr);
    var shuiChangName = shuiChangDataArr.FName;
    var isAlarm = shuiChangDataArr.IsAlarm;


    center = shuiChangDataArr.FLngLat;
    map.setCenter(new BMap.Point(center.split(',')[0], center.split(',')[1]));
    var myIcon = new BMap.Icon("/res/images/map/addMarker.png", new BMap.Size(50, 50), { anchor: new BMap.Size(10, 10) });
    marker = new BMap.Marker(new BMap.Point(center.split(',')[0], center.split(',')[1]), { icon: myIcon });


    map.addOverlay(marker);
    var shuiChangDetailData = shuiChangDataArr.waterJZ[0].D_Data[0];

    var shuiChangStatus;
    var scStateCurrent = shuiChangDetailData['FOnLine'];

    switch (isAlarm) {
        case 0: {
            switch (jzStateCurrent) {
                case 0: {
                    shuiChangStatus = 'sCState0';
                    // statusTxt = '离线';
                    break;
                }
                case 1: {
                    shuiChangStatus = 'sCState1';
                    // statusTxt = '在线';
                    break;
                }
                default: {
                    shuiChangStatus = 'sCState0';
                    break;
                }

                    //case 2: {
                    //    status = 'listItemPicSta2';
                    //    statusTxt = '报警';
                    //    break;
                    //}
            }
            break;
        }
        case 1: {
            shuiChangStatus = 'sCState2';
            //statusTxt = '报警';
            break;
        }
        default: {
            shuiChangStatus = 'sCState0';
        }
    }



    $('.sCBox .sCState').removeClass('sCState1 sCState0 sCState2').addClass(shuiChangStatus);

    

    var caiJiTime = changeTime((shuiChangDetailData.TempTime).replace(/[^0-9]/ig, ""));

    $('.rightTopMainBox .dataBox .getDataTime .getDataTxt span').html(caiJiTime);

   

   
    var shuiChangDetail = {};
    shuiChangDetail['FDTUCode'] = shuiChangDetailData.FDTUCode;

    shuiChangDetail['inPressCurrent'] = shuiChangDetailData.F40013;
    shuiChangDetail['inPressTotal'] = shuiChangDetailData.F40014;
    shuiChangDetail['leftShuiWei'] = shuiChangDetailData.F40012;
    shuiChangDetail['leftLv'] = shuiChangDetailData.F40016;
    shuiChangDetail['outPressCurrent1'] = shuiChangDetailData.F40006;
    shuiChangDetail['outPressTotal1'] = shuiChangDetailData.F40007;
    shuiChangDetail['outPressCurrent2'] = shuiChangDetailData.F40009;
    shuiChangDetail['outPressTotal2'] = shuiChangDetailData.F40010;
    shuiChangDetail['outRight'] = shuiChangDetailData.F40002;

    shuiChangDetail['PHZhi'] = shuiChangDetailData.F40003;
    shuiChangDetail['outZhuo'] = shuiChangDetailData.F40004;
    shuiChangDetail['outLv'] = shuiChangDetailData.F40005;
    shuiChangDetail['pinLv'] = shuiChangDetailData.F40017;

    shuiChangDetail['dianLiangBox1'] = shuiChangDetailData.F40018;
    shuiChangDetail['dianLiangBox2'] = shuiChangDetailData.F40019;
    shuiChangDetail['dianLiangBox3'] = shuiChangDetailData.F40020;
    shuiChangDetail['dianLiangBox4'] = shuiChangDetailData.F40021;
    shuiChangDetail['dianLiangBox5'] = shuiChangDetailData.F40022;
    shuiChangDetail['dianLiangBox6'] = shuiChangDetailData.F40023;

    shuiChangDetail['yeLun1'] = shuiChangDetailData.F40025;
    shuiChangDetail['yeLun2'] = shuiChangDetailData.F40026;
    shuiChangDetail['yeLun3'] = shuiChangDetailData.F40027;
    shuiChangDetail['yeLun4'] = shuiChangDetailData.F40028;
    shuiChangDetail['yeLun5'] = shuiChangDetailData.F40029;
    shuiChangDetail['yeLun6'] = shuiChangDetailData.F40030;

    $('.sCBox .sCName').html(shuiChangName + '[' + shuiChangDetail['FDTUCode'] + ']');

    var left3D = [
        'inPressCurrent',
        'inPressTotal', 
        'leftShuiWei',
        'leftLv',
        'outPressCurrent1',
        'outPressTotal1',
        'outPressCurrent2',
        'outPressTotal2',
        'outRight'
    ];
    var leftPumpDianLiang = [
        'dianLiangBox1',
        'dianLiangBox2',
        'dianLiangBox3',
        'dianLiangBox4',
        'dianLiangBox5',
        'dianLiangBox6'
    ];
    var leftPumpState = [
        'yeLun1',
        'yeLun2',
        'yeLun3',
        'yeLun4',
        'yeLun5',
        'yeLun6'
    ];
    var rightInfo = [
        'outRight',
        'PHZhi',
        'outZhuo',
        'outLv',
        'outPressCurrent1',
        'outPressTotal1',
        'outPressCurrent2',
        'outPressTotal2',
        'leftShuiWei',
        'inPressCurrent',
        'inPressTotal',
        'leftLv',
        'pinLv'
    ];
   
    $(left3D).each(function (ind, val) {
       
        $('.layoutwarp').find('.' + val + ' .shuiChuagNum').html(shuiChangDetail[val]);
       
    });
    $(leftPumpDianLiang).each(function (ind, val) {

        $('.layoutwarp').find('.' + val ).html(shuiChangDetail[val]+'A');

    });
    $(leftPumpState).each(function (ind, val) {
        var state;
        switch (Number(val)) {
            
            case 0: {
                state = 'stop';
                break;
            }
            case 1: {
                state = 'run';
                break;
            }
            case 2: {
                state = 'error';
                break;
            }
            default: {
                state = 'stop';
                break;
            }
        }

        $('.layoutwarp').find('.' + val).removeClass('run stop error').addClass(state);

    });
   $(rightInfo).each(function (ind, val) {
       $('.rightTopMainBox .inFoBox').find('.' + val).html(shuiChangDetail[val]);
   });


}


//右侧  小  百度地图 框 中 数据  刷新 函数 

function rightDituPressGet(currentPumpData) {
    var labelStr = '';
    var JZArr = currentPumpData.pumpJZ;



    $(JZArr).each(function (ind, val) {

        var isAlarm = JZArr[ind].IsAlarm;
        var status;

        var jzStateCurrent = JZArr[ind].D_Data[0]['FOnLine'];
        var inPress = JZArr[ind].D_Data[0]['F41006'];
        var outPress = JZArr[ind].D_Data[0]['F41007'];


        switch (isAlarm) {
            case 0: {
                switch (jzStateCurrent) {
                    case 0: {
                        status = 'gray';
                        // statusTxt = '离线';
                        break;
                    }
                    case 1: {
                        status = 'blue';
                        // statusTxt = '在线';
                        break;
                    }
                        //case 2: {
                        //    status = 'listItemPicSta2';
                        //    statusTxt = '报警';
                        //    break;
                        //}
                }
                break;
            }
            case 1: {
                status = 'red';
                //statusTxt = '报警';
                break;
            }
        }

        labelStr += '<li class="mapLabelItem clearfix">' +
                               '<div class="areaName ' + status + '">[' + val.PumpJZArea + ']</div>' +
                               '<div class="inPress">' +
                                                '<div class="bgBox"></div><div class="txtBox">进：' + inPress + '</div>' +
                               '</div>' +
                               '<div class="outPress">' +
                                     '<div class="bgBox"></div><div class="txtBox">出：' + outPress + '</div>' +
                                           '</div>' +

                           ' </li>';
    });



    var label = marker.getLabel();

    label.setContent('<div>' +
                        '<ul class="mapLabelItemWrap">' +
                            labelStr +
                        '</ul>' +
                        '<div class="sanJiao"></div>' +
                    '</div>');


}


//  right  数据和 设置 的  切换


function rightDataSetTab() {

    $('.rightTopTab li').click(function () {
        var index = $(this).index();
        if ($(this).hasClass('active')) {
            return;

        } else {
            $(this).addClass('active').siblings().removeClass('active');
            if (index == 0) {
                $('.rightTopMainBox .dataBox').addClass('active');
                $('.rightTopMainBox .setBox').removeClass('active');
            } else {
                $('.rightTopMainBox .dataBox').removeClass('active');
                $('.rightTopMainBox .setBox').addClass('active');
            }

        }
    });
}


//  显示类型选择函数

function selectFn(obj) {
    var select = obj;
    select.click(function (event) {
        var ind = $(this).index();
        //		for(var i=0;i<obj.length;i++){
        //			
        //			if(obj.eq(i).index()!='ind'){
        //				
        //				obj.eq(i).find('.select_ul').slideUp();
        //				
        //			}
        //		}

        $(this).find('.select_ul').slideToggle().end().parents('li').siblings().find('.select_ul').slideUp();

        event.stopPropagation();
    });
    //$("body").click(function () {
    //    $('.select_ul').slideUp();
    //});
    $('.select_ul').delegate("li", "click", function (e) {
        var li = $(this);

        if (li.attr('data-value') == 0) {
            return;
        }
        li.addClass("cur").siblings("li").removeClass();

        //      .end().data("value").toString();
        //      if (val !== $this.val()) {
        //           select_text.text(li.text());
        //          $this.val(val);
        //           $this.attr("data-value",val);
        //        }
        li.parent().prev('.select_text').html(li.html()).attr('data-name', li.attr('data-name'));


    });

}

//时间转换

function changeTime(time) {
    var Time = new Date(Number(time));
    
    return Time.getFullYear() + "-" + (Time.getMonth() + 1) + "-" + Time.getDate() + " " + Time.getHours() + ":" + Time.getMinutes() + ":" + Time.getSeconds();
}
