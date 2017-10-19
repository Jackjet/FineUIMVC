/// <reference path="../../../Areas/YCJK/Views/V_YCJK/video.cshtml" />
/// <reference path="../../../Areas/YCJK/Views/V_YCJK/video.cshtml" />
/// <reference path="../../../Areas/YCJK/Views/V_YCJK/video.cshtml" />
/// <reference path="../../../Areas/YCJK/Views/V_YCJK/WarningAlarm.cshtml" />
/// <reference path="../../../Areas/YCJK/Views/V_YCJK/WarningAlarm.cshtml" />
var map;
var indexForRightItem = 0;
var mapType = [BMAP_NORMAL_MAP, BMAP_SATELLITE_MAP, BMAP_PERSPECTIVE_MAP];
var iframeUrl = ['YCJK/V_YCJK/JZInfo', 'YCJK/V_Report/RunDayLog', 'YCJK/Window/WeiKaiTong', 'YCJK/V_YCJK/dataReport', 'YCJK/V_YCJK/WarningAlarm', 'YCJK/V_YCJK/RelationFile'];


var threePicBgArr = {
    'wu': ['url(/res/YCJK/img/b_1.png)', 'url(/res/YCJK/img/b_2.png)', 'url(/res/YCJK/img/b_3.png)', 'url(/res/YCJK/img/b_4.png)', 'url(/res/YCJK/img/b_5.png)', 'url(/res/YCJK/img/b_6.png)'],
    'xiang': ['url(/res/YCJK/img/x_1.png)', 'url(/res/YCJK/img/x_2.png)', 'url(/res/YCJK/img/x_3.png)', 'url(/res/YCJK/img/x_4.png)', 'url(/res/YCJK/img/x_5.png)', 'url(/res/YCJK/img/x_6.png)'],
    'guan': ['url(/res/YCJK/img/g_1.png)', 'url(/res/YCJK/img/g_2.png)', 'url(/res/YCJK/img/g_3.png)', 'url(/res/YCJK/img/g_4.png)', 'url(/res/YCJK/img/g_5.png)', 'url(/res/YCJK/img/g_6.png)']
};


var sheBeiStatePic = {
    'stop': '/res/YCJK/img/pumpstatus_stop.gif',
    'warn': '/res/YCJK/img/pumpstatus_fault.gif',
    'run': '/res/YCJK/img/pumpstatus_run.gif'
};
var sheBeiStatePosition = {
    'wu': [
			[
				{ 'left': 417, 'top': 236 },
				{ 'left': 546, 'top': 285 }
			],
			[
				{ 'left': 417, 'top': 236 },
				{ 'left': 546, 'top': 285 }
			],
			[
				{ 'left': 374, 'top': 221 },
				{ 'left': 480, 'top': 260 },
				{ 'left': 593, 'top': 302 }

			],
			[
				{ 'left': 357, 'top': 215 },
				{ 'left': 437, 'top': 245 },
				{ 'left': 521, 'top': 275 },
				{ 'left': 608, 'top': 307 }

			],
			[
			   { 'left': 336, 'top': 206 },
 				{ 'left': 406, 'top': 232 },
 				{ 'left': 478, 'top': 260 },
 				{ 'left': 554, 'top': 287 },
 				{ 'left': 632, 'top': 317 }
			]

    ],

    'guan': [
				[
					{ 'left': 336, 'top': 298 },
					{ 'left': 444, 'top': 341 }
				],
				[
					{ 'left': 336, 'top': 298 },
					{ 'left': 444, 'top': 341 }
				],
				[
					{ 'left': 290, 'top': 280 },
					{ 'left': 388, 'top': 317 },
					{ 'left': 491, 'top': 360 }

				],
				[
					{ 'left': 278, 'top': 275 },
					{ 'left': 352, 'top': 305 },
					{ 'left': 430, 'top': 335 },
					{ 'left': 511, 'top': 368 }

				],

				[
				   { 'left': 244, 'top': 262 },
	 				{ 'left': 315, 'top': 290 },
	 				{ 'left': 388, 'top': 317 },
	 				{ 'left': 465, 'top': 348 },
	 				{ 'left': 544, 'top': 380 }
				],

				[
				   { 'left': 244, 'top': 262 },
	 				{ 'left': 315, 'top': 290 },
	 				{ 'left': 388, 'top': 317 },
	 				{ 'left': 465, 'top': 348 },
	 				{ 'left': 544, 'top': 380 },
	 				{ 'left': 623, 'top': 411 }
				]
    ],
    'xiang': [

				[
					{ 'left': 336, 'top': 298 },
					{ 'left': 444, 'top': 341 }
				],
				[
					{ 'left': 336, 'top': 298 },
					{ 'left': 444, 'top': 341 }
				],

				[
					{ 'left': 290, 'top': 280 },
					{ 'left': 388, 'top': 317 },
					{ 'left': 491, 'top': 360 }

				],
				[
					{ 'left': 278, 'top': 275 },
					{ 'left': 352, 'top': 305 },
					{ 'left': 430, 'top': 335 },
					{ 'left': 511, 'top': 368 }

				],
				[
				   { 'left': 244, 'top': 262 },
	 				{ 'left': 315, 'top': 292 },
	 				{ 'left': 388, 'top': 317 },
	 				{ 'left': 465, 'top': 348 },
	 				{ 'left': 544, 'top': 380 }
				]
    ]
};

var sheBeiStateIndex = {
    'wu': [
			[
				 [7], [5, 6], [8, 9]
			],
			[
				 [7], [5, 6], [8, 9]
			],
			[
				[10], [7, 8], [9], [11, 12]

			],
			[
				[16], [12, 13], [14, 15], [17, 18], [19, 20]

			],
			[
			    [19], [14, 15], [16, 17], [18], [20, 21], [22, 23]
			]

    ],


    'guan': [
				[
					  [5], [3, 4], [6, 7]
				],
				[
					  [5], [3, 4], [6, 7]
				],
				[
					 [7], [4, 5], [6], [8, 9]

				],
				[
					 [9], [5, 6], [7, 8], [10, 11], [12, 13]

				],

				[
				   [12], [7, 8], [9, 10], [11], [13, 14], [15, 16]
				],

				[
				    [12], [7, 8], [9, 10], [11, 12], [13, 14], [15, 16]
				]
    ],

    'xiang': [

				[
					[11], [9, 10], [12, 13]
				],
				[
					[11], [9, 10], [12, 13]
				],

				[
					 [14], [11, 12], [13], [15, 16]

				],
				[
					 [14], [10, 11], [12, 13], [15, 16], [17, 18]

				],
				[
				    [18], [13, 14], [15, 16], [17], [19, 20], [21, 22]
				]
    ]
};



var itemStateArr = {

    'bengFangHuanJing': {
        'first': 0,
        'F_hj_yl': 0,
        'F_hj_zd': 0,
        'F_hj_ph': 0,
        'F_hj_sw': 0,
        'F_hj_rjy': 0,
        'F_hj_ddl': 0,
        'F_hj_sd': 0,
        'F_hj_wd': 0,
        'F_hj_wsyw': 0
    },
    'bengFangZhuangTai': {
        'first': 0,
        'F_zt_m': 0,
        'F_zt_d': 0,
        'F_zt_dy': 0,
        'F_zt_pc': 0,
        'F_zt_ups': 0,
        'F_zt_ls': 0,
        'F_zt_hj': 0
    },
    'panalShowHide': {
        'first': 0,
        'F_szmb_m': 0,
        'F_szmb_d': 0,
        'F_szmb_fswd': 0,
        'F_szmb_psb': 0,
        'F_szmb_xdy': 0,
        'F_szmb_dsqh': 0,
        'F_szmb_csyl': 0,
        'F_szmb_ddf': 0
    },
    'other': {
        'first': 0,
        'F_qt_jsll1': 0,
        'F_qt_csll1': 0,
        'F_qt_jlll1': 0,
        'F_qt_clll1': 0,
        'F_qt_ljdl': 0,
        'F_qt_jsll2': 0,
        'F_qt_csll2': 0,
        'F_qt_jlll2': 0,
        'F_qt_clll2': 0,
        'F_qt_uady': 0,
        'F_qt_ubdy': 0,
        'F_qt_ucdy': 0,
        'F_qt_iadl': 0,
        'F_qt_ibdl': 0,
        'F_qt_icdl': 0
    },
    'qiTing': {
        'first': 0,
        'second':0,
        'F_szmb_sbjjqt': 0,
        'F_szmb_kzbqt': 0,
        'F_szmb_xdsz': 0
    },
    'dianDongFa': {
        'first': 0,
        'F_ddf_df1': 0,
        'F_ddf_df2': 0,
        'F_ddf_df3': 0,
        'F_ddf_df4': 0
    },
    'shuiXiang': {
        'first': 0,
        'F_sx_sx1': 0,
        'F_sx_sx2': 0,
        'F_sx_sx3': 0,
        'F_sx_sx4': 0
    },
    'pressPaiShui': {
        'first': 0,
        'F_yl_jsyl': 0,
        'F_yl_csyl': 0,
        'F_psb_psb': 0
    },
    'bianPinIndex': {
        'first': 0,
        'F_bpq_1': 0,
        'F_bpq_2': 0,
        'F_bpq_3': 0,
        'F_bpq_4': 0,
        'F_bpq_5': 0,
        'F_bpq_6': 0
    },
    'bianPinVal': {
        'first': 0,
        'F_bpq_yxsj': 0,
        'F_bpq_yxpl': 0,
        'F_bpq_zldy': 0,
        'F_bpq_scdy': 0,
        'F_bpq_wd': 0,
        'F_bpq_gl': 0
    }
    ,
    'shuiBeng': {
        'first': 0,
        'F_sb_sb': 0
    },
    'dianLiang': {
        'first': 0,
        'F_sb_dl': 0
    },
    'yxsjShuiBeng': {
        'first': 0,
        'F_sb_yxsj': 0
    }




};

var bengFangHuanJing = [
    'F_hj_yl',
    'F_hj_zd',
    'F_hj_ph',
    'F_hj_sw',
    'F_hj_rjy',
    'F_hj_ddl',
    'F_hj_sd',
    'F_hj_wd',
    'F_hj_wsyw'
];
var bengFangZhuangTai = [
    'F_zt_m',
    'F_zt_d',
    'F_zt_dy',
    'F_zt_pc',
    'F_zt_ups',
    'F_zt_ls',
    'F_zt_hj'
];
var panalShowHide = [
    'F_szmb_m',
    'F_szmb_d',
    'F_szmb_fswd',
    'F_szmb_psb',
    'F_szmb_xdy',
    'F_szmb_dsqh',
    'F_szmb_csyl',
    'F_szmb_ddf'
];

var dianDongFa = [
    'F_ddf_df1',
    'F_ddf_df2',
    'F_ddf_df3',
    'F_ddf_df4'
];
var shuiXiang = [
    'F_sx_sx1',
    'F_sx_sx2',
    'F_sx_sx3',
    'F_sx_sx4'
];
var pressPaiShui = [
    'F_yl_jsyl',
    'F_yl_csyl',
    'F_psb_psb'
];

var bianPinIndex = [
    'F_bpq_1',
    'F_bpq_2',
    'F_bpq_3',
    'F_bpq_4',
    'F_bpq_5',
    'F_bpq_6'
];
var bianPinVal = [
    'F_bpq_yxsj',
    'F_bpq_yxpl',
    'F_bpq_zldy',
    'F_bpq_scdy',
    'F_bpq_wd',
    'F_bpq_gl'
];


var bianPinValTxt = {
    'F_bpq_yxsj': '运行时间',
    'F_bpq_yxpl': '运行频率',
    'F_bpq_zldy': '直流电压',
    'F_bpq_scdy': '输出电压',
    'F_bpq_wd': '温度',
    'F_bpq_gl': '功率'
};
var other = [
    'F_qt_jsll1',
    'F_qt_csll1',
    'F_qt_jlll1',
    'F_qt_clll1',
    'F_qt_ljdl',
    'F_qt_jsll2',
    'F_qt_csll2',
    'F_qt_jlll2',
    'F_qt_clll2',
    'F_qt_uady',
    'F_qt_ubdy',
    'F_qt_ucdy',
    'F_qt_iadl',
    'F_qt_ibdl',
    'F_qt_icdl'
];

var otherTxt = {
    'F_qt_jsll1': '1#进瞬流量',
    'F_qt_csll1': '1#出瞬流量',
    'F_qt_jlll1': '1#进累流量',
    'F_qt_clll1': '1#出累流量',
    'F_qt_ljdl': '累计电量',
    'F_qt_jsll2': '2#进瞬流量',
    'F_qt_csll2': '2#出瞬流量',
    'F_qt_jlll2': '2#进累流量',
    'F_qt_clll2': '2#出累流量',
    'F_qt_uady': 'Ua电压',
    'F_qt_ubdy': 'Ub电压',
    'F_qt_ucdy': 'Uc电压',
    'F_qt_iadl': 'Ia电流',
    'F_qt_ibdl': 'Ib电流',
    'F_qt_icdl': 'Ic电流'
};

var qiTing = [
    'F_szmb_sbjjqt',
    'F_szmb_kzbqt',
    'F_szmb_xdsz'
];

var pumpArr = [
    'pump1',
    'pump2',
    'pump3',
    'pump4',
    'pump5',
    'pump6'
];

var marker;
var v;
var currentJZ;
var currentJZName;
var center;
var currentPumpData = {};
var pumpDataGetTimer;
var currentInd;
$(function () {
    v = parseUrl();

    judgeScreen();
    pageLeftBotShowFn();
    pageLeftBotArrowFn();
    $(window).resize(function () {
        judgeScreen();
        pageLeftBotShowFn();
        //alert(encodeURIComponent(currentPumpData.PName));
        //alert(decodeURIComponent(encodeURIComponent(currentPumpData.PName)));
    });

    getMap();
    rightShowBtnFn();
    showHideBtnFn();
    threeToTwoBtn();
    leftBotNavFn();
    rightBotSHowHideFn();
    midScroll();
    rightTopScroll();
    highAndLowChange();
    threeBoxScroll();
    twoBoxScroll();

    currentJZ = v.jzId;
    var deferred = $.Deferred();


    deferred.then(pumpDataGet(v.pumpId), resetState());

    clearInterval(pumpDataGetTimer);
    window.setInterval(function () {
        pumpDataGetNew(v.pumpId);
    }, 5000);
    pumpDataGetTimer = setInterval(function () {
        // alert(1);
        //pumpDataGet(v.pumpId);
      
        if (currentInd) {
            var ind = currentInd;
        } else {
            var ind = 0;
        }
        jzDataUse(ind);

        rightDataItemDataFn(currentPumpData);
        rightDituPressGet(currentPumpData);
        //rightDataItemFn(currentPumpData);
         pumpDataGetNew(v.pumpId);
    }, 4000);



    //set框 下发命令
    setBoxControlCommond();


    rightDataSetTab();
    rightVideoMapTab();

    xianShiFn();

    selectFn($('.selectBox'));

    showChart();

    radialBar();
    getWaterElec(currentJZ);
    get7daysWaterUse(currentJZ);
    get7daysElecUse(currentJZ);
    getInOutWaPress(currentJZ, 10);
    setInterval(function () {
        getWaterElec(currentJZ);
        getInOutWaPress(currentJZ, 1);
        get7daysWaterUse(currentJZ);
        get7daysElecUse(currentJZ);
    }, 5000);



});

//获取  URL  信息

function parseUrl() {

    var url = window.location.href;
    // alert(url);
    var i = url.indexOf('?');
    // alert(1);
    if (i == -1) { return };
    var queryStr = url.substr(i + 1);
    var arr1 = queryStr.split('&');
    var arr2 = {};
    for (j in arr1) {
        var tar = arr1[j].split('=');

        arr2[tar[0]] = tar[1];
    };
    // alert(arr2);
    return arr2;


}


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

            //			if($(this).hasClass('show')){
            //				$(this).removeClass('show').hide().siblings().removeClass('show').hide();
            //				$('.pageRight').css('right','-360px');
            //				$('.pageMid').css('right','-360px');
            //				$('.pageLeft').css('right','12px');
            //				
            //			}else {
            //				$('.pageMid').css('right','0');
            //				$('.pageLeft').css('right','12px');
            //			}

        } else if ($W > 1366 && $W <= 1600) {

            $(this).hide().siblings().show();
            $('.pageRight').css('right', '-360px');
            $('.pageMid').css('right', '0');
            $('.pageLeft').css('right', '362px');


            //			if($(this).hasClass('show')){
            //				$(this).removeClass('active');
            //				if($(this).siblings().hasClass('active')){
            //					$('.pageRight').css('right','0');
            //					$('.pageLeft').css('right','362px');
            //				}else {
            //					$('.pageRight').css('right','-360px');
            //					$('.pageLeft').css('right','12px');
            //				}
            //				$('.pageMid').css('right','-360px');
            //			}else {
            //				$('.pageMid').css('right','0');
            //				$('.pageLeft').css('right','362px');
            //			}
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
            //			if($(this).hasClass('show')){
            //				$(this).removeClass('active').siblings().removeClass('active');
            //				$('.pageRight').css('right','0');
            //				$('.pageMid').css('right','362px');
            //				$('.pageLeft').css('right','724px');
            //			}else {
            //				$('.pageMid').css('right','0');
            //				$('.pageLeft').css('right','362px');
            //			}



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
                $('.leftBotFrame').attr('src', F.baseUrl + iframeUrl[index] + '?pumpID=' + v.pumpId + '&pumpName=' + encodeURIComponent(currentPumpData.PName) + '&jzID=' + currentJZ + '&jzName=' + encodeURIComponent(currentJZName)+'&pageType=0');
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

//左侧  高 中 低区  的切换
function highAndLowChange() {
    $('body').delegate('.pageLeftTop li', 'click', function () {
        var index = $(this).index();
        indexForRightItem = $(this).index();
        // alert(mfs.length);


        if ($(this).hasClass('active')) {
            currentInd = index;
            return;
        } else {

            $(this).addClass('active').siblings().removeClass('active');
            currentJZ = $(this).attr('data-BaseId');
            currentInd = index;
            jzDataUse(index);


            //点击  jz  右侧 数据框 的刷新

            resetState();


        }


    });
    //$('.pageLeftTop li').click(function () {
    //	var index=$(this).index();
    //	if($(this).hasClass('active')){
    //		return;
    //	}else {
    //		$(this).addClass('active').siblings().removeClass('active');
    //		$('.pageLeftMid>div').css('display','none');
    //		$('.pageLeftMid>div').eq(index).css('display','block');
    //	}
    //});



}


  //机组的 3D 2D 状态 数据的 处理  函数  


function jzDataUse(index) {
    // mfs.length = 0;
    $('#layoutwarp canvas').remove();
    // 或者 $('#layoutwarp').empty();
    //隐藏所有2D泵
    $('.twoSection .pumpWrap li').removeClass('active');
    $('.twoSection .pumpWrap li .pump').removeClass('aux');
  
    $('.shuiXiangClass').removeClass('active');
    $('.shuiGuanClass').removeClass('active');
    //框的 水流
    $('.flowT').removeClass('hide');
    $('.flowR1').removeClass('hide');
    $('.flowR2').removeClass('hide');
    $('.flowB').removeClass('hide');
    $('.flowL1').removeClass('hide');
    $('.flowL2').removeClass('hide');
    $('.flowH4').removeClass('hide');
    $('.flowH5').removeClass('hide');
    $('.flowVL1').removeClass('hide');
    $('.flowHB1').removeClass('hide');
    $('.flowHB2').removeClass('hide');
    //$('.pageLeftMid>div').css('display', 'none');
    //$('.pageLeftMid>div').eq(index).css('display', 'block');


    //alert(currentJZ);


   

    //alert(currentPumpData.PName);

    // currentPumpData.pumpJZ[index].PumpJZName;

    //console.log(index);
    //console.log(currentPumpData);
    //console.log(currentPumpData.pumpJZ);
    //console.log(currentPumpData.pumpJZ[index]);
    //console.log('where is qinqin2');
    $('.jzName').html(currentPumpData.pumpJZ[index].PumpJZName + ' [' + currentPumpData.pumpJZ[index].DTUCode + ']');
    currentJZName = currentPumpData.pumpJZ[index].PumpJZName;
    var pumpAux = currentPumpData.pumpJZ[index].Auxiliarypumpcount;
    var pumpRun = currentPumpData.pumpJZ[index].RunPumpNum
    var pumpJZNum = pumpAux + pumpRun;
    var outNum = currentPumpData.pumpJZ[index].DrainPumpNum;
    var machineType = $.trim(currentPumpData.pumpJZ[index].MachineType);

    var isAlarm = currentPumpData.pumpJZ[index].IsAlarm;
    var status;

    var typeStr;
    // alert(pumpJZNum);
    //alert(pumpRun);
    //alert(pumpAux);

    var notFlowArr = [];
    var currentPumpStateArr = currentPumpData.pumpJZ[index].D_Data[0];


    //右侧(ing)
    // rightDataItemFn(currentPumpStateArr);



    var jzStateCurrent = currentPumpStateArr['FOnLine'];
    var pumpArr = [];
    var pumpRunTime = [];
    var pumpDianArr = [];
    pumpArr[0] = currentPumpStateArr['F41008'];
    pumpArr[1] = currentPumpStateArr['F41009'];
    pumpArr[2] = currentPumpStateArr['F41010'];
    pumpArr[3] = currentPumpStateArr['F41011'];
    pumpArr[4] = currentPumpStateArr['F41012'];
    pumpArr[5] = currentPumpStateArr['F41013'];

    pumpDianArr[0] = currentPumpStateArr['F41045'];
    pumpDianArr[1] = currentPumpStateArr['F41046'];
    pumpDianArr[2] = currentPumpStateArr['F41047'];
    pumpDianArr[3] = currentPumpStateArr['F41048'];
    pumpDianArr[4] = currentPumpStateArr['F41049'];
    pumpDianArr[5] = currentPumpStateArr['F41050'];

    pumpRunTime[0] = currentPumpStateArr['F41051'];
    pumpRunTime[1] = currentPumpStateArr['F41052'];
    pumpRunTime[2] = currentPumpStateArr['F41053'];
    pumpRunTime[3] = currentPumpStateArr['F41054'];
    pumpRunTime[4] = currentPumpStateArr['F41055'];
    pumpRunTime[5] = currentPumpStateArr['F41056'];

    var inPress = currentPumpStateArr['F41006'];
    var outPress = currentPumpStateArr['F41007'];
    var outShunLiu = currentPumpStateArr['F41025'];
    var outLeiJiLiu = currentPumpStateArr['FTotalOutLL'];

    $('.inPress .pressN').html(inPress);
    $('.outPress .pressN').html(outPress);
    $('.leiJiLiu .liuLiangNum').html(outLeiJiLiu);
    $('.shunShiLiu .liuLiangNum').html(outShunLiu);

    //左侧  机组状态的处理


    switch (isAlarm) {
        case 0: {
            switch (jzStateCurrent) {
                case 0: {
                    status = 'jzState0';
                    // statusTxt = '离线';
                    break;
                }
                case 1: {
                    status = 'jzState1';
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
            status = 'jzState2';
            //statusTxt = '报警';
            break;
        }
    }


    $('.jzBox .jzState').removeClass().addClass('jzState ' + status);

    if (machineType == '0') {
        typeStr = 'guan';

        $('.shuiGuanClass').addClass('active');
    } else if (machineType == '1') {
        typeStr = 'xiang';
        $('.shuiXiangClass').addClass('active');
    } else if (machineType == '2') {
        typeStr = 'wu';
        $('.shuiXiangClass').removeClass('active');
        $('.shuiGuanClass').removeClass('active');
    } else if (machineType == '1') {
    }

    if (pumpRun) {
        for (var zhu = 0; zhu < pumpRun; zhu++) {
            $('.twoSection .pumpWrap li').eq(zhu).addClass('active');
            $('.twoSection .pumpWrap li').eq(zhu).find('.pump>div').removeClass().addClass('pumpState' + pumpArr[zhu]);

            if (pumpArr[zhu] != '2') {
                $('.twoSection .pumpWrap li').eq(zhu).find('>div').removeClass('active');
                //alert(11);
                //alert(pumpArr[zhu]);
                var $father = $('.twoSection .pumpWrap li').eq(zhu).find('.pump');

                $father.find('.pumpTime').html(pumpRunTime[zhu] || (pumpRunTime[zhu] == 0) ? pumpRunTime[zhu] + 'h' : ' ' + 'h');
                $father.find('.pumpDian').html(pumpDianArr[zhu] || (pumpDianArr[zhu] == 0) ? pumpDianArr[zhu] + 'A' : ' ' + 'A');

                notFlowArr.push(pumpArr[zhu]);
            } else {
                // alert(1);
                //alert(pumpArr[zhu]);
                $('.twoSection .pumpWrap li').eq(zhu).find('>div').addClass('active');
                var $father = $('.twoSection .pumpWrap li').eq(zhu).find('.pump');
                $father.find('.pumpTime').html(pumpRunTime[zhu] || (pumpRunTime[zhu] == 0) ? pumpRunTime[zhu] + 'h' : ' ' + 'h');
                $father.find('.pumpDian').html(pumpDianArr[zhu] || (pumpDianArr[zhu] == 0) ? pumpDianArr[zhu] + 'A' : ' ' + 'A');

            };
        }
    }



    if (pumpAux) {
        for (var aux = pumpRun; aux < pumpJZNum; aux++) {

            $('.twoSection .pumpWrap li').eq(aux).addClass('active');
            $('.twoSection .pumpWrap li').eq(aux).find('.pump').addClass('aux');
            $('.twoSection .pumpWrap li').eq(aux).find('.pump>div').removeClass().addClass('pumpState' + pumpArr[aux]);

            if (pumpArr[aux] != '2') {
                // alert(21);
                //alert(pumpArr[aux]);
                $('.twoSection .pumpWrap li').eq(aux).find('>div').removeClass('active');
                var $father = $('.twoSection .pumpWrap li').eq(aux).find('.pump');
                $father.find('.pumpTime').html(pumpRunTime[aux] + 'h');
                $father.find('.pumpDian').html(pumpDianArr[aux] + 'A');
                notFlowArr.push(pumpArr[aux]);
            } else {
                // alert(2);
                // alert(pumpArr[aux]);
                $('.twoSection .pumpWrap li').eq(aux).find('>div').addClass('active');
                var $father = $('.twoSection .pumpWrap li').eq(aux).find('.pump');

                $father.find('.pumpTime').html(pumpRunTime[aux] + 'h');
                $father.find('.pumpDian').html(pumpDianArr[aux] + 'A');
            };
        }
    }


    // alert(notFlowArr.length == pumpJZNum);
    if (notFlowArr.length == pumpJZNum) {
        $('.flowT').addClass('hide');
        $('.flowR1').addClass('hide');
        $('.flowR2').addClass('hide');
        $('.flowB').addClass('hide');
        $('.flowL1').addClass('hide');
        $('.flowL2').addClass('hide');
        $('.flowH4').addClass('hide');
        $('.flowH5').addClass('hide');
        $('.flowVL1').addClass('hide');
        $('.flowHB1').addClass('hide');
        $('.flowHB2').addClass('hide');
    };


    // alert(typeStr);
    switch (pumpJZNum) {
        case 1: {

            if (!$('.threeToTwoWrap').hasClass('two')) {
                $('.threeToTwoWrap').stop().animate({ 'margin-left': '-50%' }, 300);
                $('.threeToTwoWrap').parent().next().css('display', 'none');
                $('.threeToTwoWrap').parent().next().next().css('display', 'block');
                $('.threeToTwoWrap').find('.threeToTwoPic').addClass('two');
                $('.threeToTwoWrap').addClass('no');
            } else {
                $('.threeToTwoWrap').stop().animate({ 'margin-left': '-50%' }, 300);
                $('.threeToTwoWrap').parent().next().css('display', 'none');
                $('.threeToTwoWrap').parent().next().next().css('display', 'block');
                if (!$('.threeToTwoWrap').find('.threeToTwoPic').hasClass('two')) {
                    $('.threeToTwoWrap').find('.threeToTwoPic').addClass('two');
                }
                if (!$('.threeToTwoWrap').hasClass('no')) {
                    $('.threeToTwoWrap').addClass('no')
                }
            }

            break;
        }
        case 2: {
            $('#layoutwarp').css('backgroundImage', threePicBgArr[typeStr][1]);
            flowTypeAndNumSelect(typeStr, 2);
            pumpStateDealFn(typeStr, 1, pumpArr);
            if (!$('.threeToTwoWrap').hasClass('two') && $('.threeToTwoWrap').find('.threeToTwoPic').hasClass('two')) {
                $('.threeToTwoWrap').addClass('two');
            }
            $('.threeToTwoWrap').removeClass('no');
            break;
        }
        case 3: {

            $('#layoutwarp').css('backgroundImage', threePicBgArr[typeStr][2]);
            flowTypeAndNumSelect(typeStr, 3);
            pumpStateDealFn(typeStr, 2, pumpArr);
            if (!$('.threeToTwoWrap').hasClass('two') && $('.threeToTwoWrap').find('.threeToTwoPic').hasClass('two')) {
                $('.threeToTwoWrap').addClass('two');
            }
            $('.threeToTwoWrap').removeClass('no');

            break;
        }
        case 4: {
            $('#layoutwarp').css('backgroundImage', threePicBgArr[typeStr][3]);
            flowTypeAndNumSelect(typeStr, 4);
            pumpStateDealFn(typeStr, 3, pumpArr);
            if (!$('.threeToTwoWrap').hasClass('two') && $('.threeToTwoWrap').find('.threeToTwoPic').hasClass('two')) {
                $('.threeToTwoWrap').addClass('two');
            }
            $('.threeToTwoWrap').removeClass('no');

            break;
        }
        case 5: {
            $('#layoutwarp').css('backgroundImage', threePicBgArr[typeStr][4]);
            flowTypeAndNumSelect(typeStr, 5);
            pumpStateDealFn(typeStr, 4, pumpArr);
            if (!$('.threeToTwoWrap').hasClass('two') && $('.threeToTwoWrap').find('.threeToTwoPic').hasClass('two')) {
                $('.threeToTwoWrap').addClass('two');
            }
            $('.threeToTwoWrap').removeClass('no');

            break;
        }
        case 6: {
            if (typeStr != 'guan') {

                if (!$('.threeToTwoWrap').hasClass('two')) {
                    $('.threeToTwoWrap').stop().animate({ 'margin-left': '-50%' }, 300);
                    $('.threeToTwoWrap').parent().next().css('display', 'none');
                    $('.threeToTwoWrap').parent().next().next().css('display', 'block');
                    $('.threeToTwoWrap').find('.threeToTwoPic').addClass('two');
                    $('.threeToTwoWrap').addClass('no');
                }
            } else {
                if (!$('.threeToTwoWrap').hasClass('two') && $('.threeToTwoWrap').find('.threeToTwoPic').hasClass('two')) {
                    $('.threeToTwoWrap').addClass('two');
                }
                $('.threeToTwoWrap').removeClass('no');
                $('#layoutwarp').css('backgroundImage', threePicBgArr[typeStr][5]);
                flowTypeAndNumSelect(typeStr, 6);
                pumpStateDealFn(typeStr, 5, pumpArr);
            }

            break;
        }
        default: {
            $('.threeToTwoWrap').removeClass('no');
            $('#layoutwarp').css('backgroundImage', threePicBgArr[typeStr][4]);
            flowTypeAndNumSelect(typeStr, 5);
            pumpStateDealFn(typeStr, 4, pumpArr);
            break;
        }
    }
    // $('#layoutwarp').css('backgroundImage', threePicBgArr['guan'][index % 5]);

}

//3D 和2D 的小按钮

function threeToTwoBtn() {
    $('.threeToTwoWrap').click(function () {
        if ($(this).hasClass('no')) {
            return;
        }
        if ($(this).hasClass('two')) {
            $(this).removeClass('two');
            $(this).stop().animate({ 'margin-left': '0' }, 300);
            $(this).parent().next().css('display', 'block');
            $(this).parent().next().next().css('display', 'none');
            $(this).find('.threeToTwoPic').removeClass('two');
        } else {
            $(this).addClass('two');
            $(this).stop().animate({ 'margin-left': '-50%' }, 300);
            $(this).parent().next().css('display', 'none');
            $(this).parent().next().next().css('display', 'block');
            $(this).find('.threeToTwoPic').addClass('two');
        }
    });
    //$('.threeBtn').click(function(){
    //	$(this).parent().animate({'margin-left':'-50%'},300);
    //	$(this).parent().parent().next().next().css('display','block');
    //	$(this).parent().parent().next().css('display','none');
    //	$(this).next().addClass('two');
    //});
    //$('.twoBtn').click(function(){
    //	$(this).parent().animate({'margin-left':'0'},300);
    //	$(this).parent().parent().next().next().css('display','none');
    //	$(this).parent().parent().next().css('display','block');
    //	$(this).prev().removeClass('two');
    //});
}

// 3D 框的  滚动条事件

function threeBoxScroll() {
    $('.threeSection').mCustomScrollbar({
        scrollbarPosition: "inside",
        theme: "minimal-dark"
    });
}

// 2D 框的  滚动条事件
function twoBoxScroll() {
    $('.twoSection').mCustomScrollbar({
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
            async:false,
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
                var labelTop = -(jzLen*34+12);

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



                var label = new BMap.Label('<div>'+
                        '<ul class="mapLabelItemWrap">'+
                            labelStr+
                        '</ul>' +
                        '<div class="sanJiao"></div>'+
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

               

                pumpDataUse(currentPumpData);
                

            },
            error: function (data) {
                console.log('泵房数据获取出错');
            }
        });
    }
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


//泵房数据  获取后  机组 nav的 处理 函数
function pumpDataUse(pumpDat) {
    var JZArr = pumpDat.pumpJZ;

    //console.log(JZArr);
    //console.log(343434);
    $('.pumpNameTxtBox').html(pumpDat.PName);

    var str = '';
    $(JZArr).each(function (ind, val) {
       
        //console.log(val.pumpJZID);
        //console.log(222);
        str += ' <li class="high clearfix" data-BaseId="' + val.pumpJZID + '">' +
                   '<div class="leftNavPicBox"></div>' +
                  ' <div class="leftNavTxtBox">' + val.PumpJZArea + '</div>' +
               '</li>';

       
    });

    $('.pageLeftTop').html(str);

   
   
    if (currentJZ) {
        $('.pageLeftTop li[data-BaseId="' + currentJZ + '"]').trigger('click');
       // alert(currentJZ);

    } else {
      //  alert(1);
        $('.pageLeftTop li:first-child').trigger('click');
    }



}

//泵房状态的 处理  函数  (设备上的  小图标  和canvas)

function pumpStateDealFn(type, num, stateArr) {

    $('#layoutwarp div.stateBox').remove();
    var str = '';
    var stateDealArr = [];
    var flowStopArr = [];

    for (var s = 0; s < stateArr.length; s++) {
        if (stateArr[s] == '0' || stateArr[s] == '1') {
            stateDealArr.push('stop');
        } else if (stateArr[s] == '2') {
            stateDealArr.push('run');
        } else if (stateArr[s] == '3') {
            stateDealArr.push('warn');
        } else {
            stateDealArr.push('stop');
        }
    }
    for (var item in sheBeiStatePosition[type][num]) {

        str += '<div class="stateBox" style="background: url(' + sheBeiStatePic['stop'] + ') no-repeat ;left:' + sheBeiStatePosition[type][num][item]['left'] + 'px ;top:' + sheBeiStatePosition[type][num][item]['top'] + 'px ;"></div>';
    }


    $('#layoutwarp').append(str);

    var stateArrLen = $('.stateBox').length;
    for (var i = 0; i < stateArrLen; i++) {
        $('.stateBox').eq(i).css('backgroundImage', 'url(' + sheBeiStatePic[stateDealArr[i]] + ')');
        if (stateDealArr[i] != 'run') {
            flowStopArr.push(i);

            if (type == 'wu') {

                var $le = sheBeiStateIndex[type][num][i + 1].length;

                for (var $l = 0; $l < $le; $l++) {

                    mfs[sheBeiStateIndex[type][num][i + 1][$l]].flowHide();
                }

            } else if (type == 'guan') {
                var $le = sheBeiStateIndex[type][num][i + 1].length;

                for (var $l = 0; $l < $le; $l++) {

                    mfs[sheBeiStateIndex[type][num][i + 1][$l]].flowHide();
                }
            } else if (type == 'xiang') {
                var $le = sheBeiStateIndex[type][num][i + 1].length;

                for (var $l = 0; $l < $le; $l++) {

                    mfs[sheBeiStateIndex[type][num][i + 1][$l]].flowHide();
                }
            }
        }


    }


    if (flowStopArr.length == stateArrLen) {
        mfs[sheBeiStateIndex[type][num][0][0]].flowHide();
    }

    //if (type == 'wu') {
    //    for (var t = 1; t <= (num + 1) ; t++) {
    //        var $le = sheBeiStateIndex[type][num][t].length;
    //        for (var $l = 0; $l < $le; $l++) {
    //            mfs[sheBeiStateIndex[type][num][t][$l]].flowHide();
    //        }
    //    }
    //}

    //alert(1);
}
//left  header  的 高中低 区 tab  

// left 设备 框 的显示   内容 处理 事件

function leftSheBeiShowFn(bengfangID) {

    if (bengfangID) {
        $.ajax({
            url: '/V_YCJK/Search_Pump_JZReportList',
            data: {
                'pumpID': bengfangID
            },
            success: function (data) {
                var dataJSON = JSON.parse(data);
                dataJSON = dataJSON.obj;
                // alert(data);
                // alert(dataJSON[0].PName);

            },
            error: function () {

            }
        });
    } else {


        //crflow(738, 161, 658, 217); //来源

        //crflow(640, 390, 585, 432); //流入泵
        //crflow(528, 385, 507, 401);
        //crflow(450, 359, 432, 372);
        //crflow(375, 333, 361, 342);
        //crflow(302, 308, 292, 313);
        //crflow(354, 275, 346, 280);

        //crflow(230, 352, 208, 366); //泵流出 7   左边1
        //crflow(208, 374, 344, 432);//8     左边1右侧

        //crflow(301, 382, 272, 400);     //左边2 
        //crflow(270, 400, 341, 431);   //左边2 右侧

        //crflow(376, 411, 344, 432);   //左边3
        //crflow(344, 432, 278, 478);    //总  出

        //crflow(446, 446, 424, 461);     //左边4
        //crflow(426, 465, 351, 433);   //左边4  左

        //crflow(527, 477, 505, 493);    //左边5
        //crflow(498, 495, 344, 432);   //左边 5  左


        //$('#layoutwarp').css('backgroundImage', threePicBgArr['guan'][3]);

    }
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

//  right 视频 和 地图 的  切换


function rightVideoMapTab() {

    $('.rightBotTab li').click(function () {
        var index = $(this).index();
        if ($(this).hasClass('active')) {
            return;

        } else {
            $(this).addClass('active').siblings().removeClass('active');
            if (index == 0) {
                $('.rightBotMainBox .mapBox').addClass('active');
                $('.rightBotMainBox .videoBox').removeClass('active');
            } else {
                $('.rightBotMainBox .mapBox').removeClass('active');
                $('.rightBotMainBox .videoBox').addClass('active');

                if ($('.videoIframe').attr('src') != '') {
                    return;
                } else {
                    $('.videoIframe').attr('src', 'video?videoPanals=' );
                    ///res/YCJK/demo.html
              
                }
              
            }

        }
    });
}





//右侧视频通道的生成函数
function videoDeal() {
    var videoPanal = [];
    videoPanal=currentPumpData.pumpVQ;
    return videoPanal;
}

//  右侧 显示  按钮 的 点击事件  

function xianShiFn() {
    $('.xianShi').click(function () {
        layer.open({
            type: 2,
            anim: 3,
            shade: .6,
            title: false,
            shadeClose: true,
            area: ['1010px', '560px'],
            content: '/YCJK/Window/dataItemShowHide?pumpId=' + v.pumpId,
            success: function () {
                //  alert('OK');
            },

            end: function () {
                resetState();
            }
        });
    });
}



//  页面载入时  右侧显示初始状态 数据获取

function resetState() {
    $.ajax({
        url: '/V_YCJK/Search_Control',
        data: {
            'pumpID': v.pumpId
        },
        success: function (data) {
            var dataJSON = JSON.parse(data);
            if (dataJSON.success) {
                dataItemState = dataJSON.obj[0];
                //console.log(dataItemState);
                
           var deferItem = $.Deferred();

               deferItem.then(resetStateUse(dataItemState), (rightDataItemFn(currentPumpData), rightDataItemDataFn(currentPumpData)));
               


            } else {
                // alert('noIn');
            }
            // alert(dataJSON[0].PName);

        },
        error: function () {
            //  alert('error');
        }
    });
}

///页面载入时 初始状态 数据使用


//itemStateArr = {
//    'itemZuState': {
//        'currentState':[0,0,0,0,0,0,0,0,0,0]
//    },
//    'bengFangHuanJing': {

///'pressPaiShui': {
//    'first': 0,


function resetStateUse(dataJson) {

    var bengFangHuanJingNum = 0, bengFangZhuangTaiNum = 0,
      panalShowHideNum = 0, dianDongFaNum = 0, shuiXiangNum = 0,
      pressPaiShuiNum = 0, bianPinIndexNum = 0, bianPinValNum = 0,
      otherNum = 0, qiTingNum = 0
    ;

    var surroundingsValLen = $('.surroundingsValMain>li').length;
    var surroundingsStateLen = $('.surroundingsStateMain>li').length;
    var panalItemLen = $('.panalItemBox>li').length;
    $(bengFangHuanJing).each(function (ind, val) {
        var trueOrFalse = dataJson[val];


        if (trueOrFalse) {
            itemStateArr['bengFangHuanJing'][val] = 1;
            bengFangHuanJingNum++;
        } else {
            itemStateArr['bengFangHuanJing'][val] = 0;

        }

    });
    if (bengFangHuanJingNum == 0) {
        itemStateArr['bengFangHuanJing']['first'] = 0;

    } else if (bengFangHuanJingNum > 0) {
        itemStateArr['bengFangHuanJing']['first'] = 1;

    }

    $(bengFangZhuangTai).each(function (ind, val) {
        var trueOrFalse = dataJson[val];


        if (trueOrFalse) {
            itemStateArr['bengFangZhuangTai'][val] = 1;
            bengFangZhuangTaiNum++;
        } else {
            itemStateArr['bengFangZhuangTai'][val] = 0;

        }
    });

    if (bengFangZhuangTaiNum == 0) {
        itemStateArr['bengFangZhuangTai']['first'] = 0;

    } else if (bengFangZhuangTaiNum > 0) {
        itemStateArr['bengFangZhuangTai']['first'] = 1;

    }
    $(panalShowHide).each(function (ind, val) {
        var trueOrFalse = dataJson[val];


        if (trueOrFalse) {
            itemStateArr['panalShowHide'][val] = 1;
            panalShowHideNum++;
        } else {
            itemStateArr['panalShowHide'][val] = 0;

        }
    });

    if (panalShowHideNum == 0) {
        itemStateArr['panalShowHide']['first'] = 0;

    } else if (panalShowHideNum > 0) {
        itemStateArr['panalShowHide']['first'] = 1;

    }



    $(dianDongFa).each(function (ind, val) {
        var trueOrFalse = dataJson[val];


        if (trueOrFalse) {
            itemStateArr['dianDongFa'][val] = 1;

            dianDongFaNum++;
        } else {
            itemStateArr['dianDongFa'][val] = 0;

        }
    });

    $(shuiXiang).each(function (ind, val) {
        var trueOrFalse = dataJson[val];


        if (trueOrFalse) {
            itemStateArr['shuiXiang'][val] = 1;
        } else {
            itemStateArr['shuiXiang'][val] = 0;
            shuiXiangNum++;
        }
    });

    $(pressPaiShui).each(function (ind, val) {
        var trueOrFalse = dataJson[val];


        if (trueOrFalse) {
            itemStateArr['pressPaiShui'][val] = 1;
        } else {
            itemStateArr['pressPaiShui'][val] = 0;
            pressPaiShuiNum++;
        }
    });
    $(bianPinIndex).each(function (ind, val) {
        var trueOrFalse = dataJson[val];


        if (trueOrFalse) {
            itemStateArr['bianPinIndex'][val] = 1;
        } else {
            itemStateArr['bianPinIndex'][val] = 0;
            bianPinIndexNum++;
        }
    });
    $(bianPinVal).each(function (ind, val) {
        var trueOrFalse = dataJson[val];


        if (trueOrFalse) {
            itemStateArr['bianPinVal'][val] = 1;
            bianPinValNum++;
        } else {
            itemStateArr['bianPinVal'][val] = 0;

        }
    });

    if (bianPinValNum == 0) {
        itemStateArr['bianPinVal']['first'] = 0;

    } else if (bianPinValNum > 0) {
        itemStateArr['bianPinVal']['first'] = 1;

    }

    $(other).each(function (ind, val) {
        var trueOrFalse = dataJson[val];

        if (trueOrFalse) {
            itemStateArr['other'][val] = 1;
            otherNum++;
        } else {
            itemStateArr['other'][val] = 0;

        }
    });

    if (otherNum == 0) {
        itemStateArr['other']['first'] = 0;

    } else if (otherNum > 0) {
        itemStateArr['other']['first'] = 1;

    }

    $(qiTing).each(function (ind, val) {
        var trueOrFalse = dataJson[val];

        if (trueOrFalse) {
            itemStateArr['qiTing'][val] = 1;
        } else {
            itemStateArr['qiTing'][val] = 0;
            qiTingNum++;
        }
    });



    //水泵的  

    var sbTrueOrFalse = dataJson['F_sb_sb'];
    var dlTrueOrFalse = dataJson['F_sb_dl'];
    var yxsjTrueOrFalse = dataJson['F_sb_yxsj'];


    if (sbTrueOrFalse) {
        itemStateArr['shuiBeng']['F_sb_sb'] = 1;
        itemStateArr['shuiBeng']['first'] = 1;
    } else {
        itemStateArr['shuiBeng']['F_sb_sb'] = 0;
        itemStateArr['shuiBeng']['first'] = 0;

    }

    if (dlTrueOrFalse) {
        itemStateArr['dianLiang']['F_sb_dl'] = 1;
        itemStateArr['dianLiang']['first'] = 1;

    } else {
        itemStateArr['dianLiang']['F_sb_dl'] = 0;
        itemStateArr['dianLiang']['first'] = 0;

    }

    if (yxsjTrueOrFalse) {
        itemStateArr['yxsjShuiBeng']['F_sb_yxsj'] = 1;
        itemStateArr['yxsjShuiBeng']['first'] = 1;

    } else {
        itemStateArr['yxsjShuiBeng']['F_sb_yxsj'] = 0;
        itemStateArr['yxsjShuiBeng']['first'] = 0;


    }


    //console.log(itemStateArr);

}

//右侧  列表 载入时的  显示  处理函数
function rightDataItemFn(currentPumpData) {

    $('.firstLine').empty();
    $('.lastLine').empty();

    //console.log(currentPumpData);
    //console.log('琴瑟和鸣');
    var index = $('.pageLeftTop li.active').index();
    if (index < 0) {
        index = 0;
    }
   
    var pumpAux = currentPumpData.pumpJZ[index].Auxiliarypumpcount;
    var pumpRun = currentPumpData.pumpJZ[index].RunPumpNum;
    var pumpJZNum = pumpAux + pumpRun;
    //var outNum = currentPumpData.pumpJZ[index].DrainPumpNum;


    //var currentPumpStateArr = currentPumpData.pumpJZ[index].D_Data[0];
    //var currentJZDataArr = currentPumpStateArr;
    //var caijiTime = currentJZDataArr['FUpdateDate'];
    //var yuAndPHArr = {};
    //yuAndPHArr['F_hj_yl'] = currentJZDataArr['F41087'];
    //yuAndPHArr['F_hj_zd'] = currentJZDataArr['F41088'];
    //yuAndPHArr['F_hj_ph'] = currentJZDataArr['F41089'];
    //yuAndPHArr['F_hj_sw'] = currentJZDataArr['F41092'];
    //yuAndPHArr['F_hj_rjy'] = currentJZDataArr['F41091'];
    //yuAndPHArr['F_hj_ddl'] = currentJZDataArr['F41090'];     
    //yuAndPHArr['F_hj_sd'] = currentJZDataArr['F41100'];
    //yuAndPHArr['F_hj_wd'] = currentJZDataArr['F41101'];
    //yuAndPHArr['F_hj_wsyw'] = currentJZDataArr['F41101'];


    //var menDengArr = {};

    //menDengArr['F_zt_m'] = currentJZDataArr['F41093'];
    //menDengArr['F_zt_d'] = currentJZDataArr['F41097'];
    //menDengArr['F_zt_dy'] = currentJZDataArr['F41099'];
    //menDengArr['F_zt_pc'] = currentJZDataArr['F41095'];
    //menDengArr['F_zt_ups'] = currentJZDataArr['F41098'];
    //menDengArr['F_zt_ls'] = currentJZDataArr['F41094'];
    //menDengArr['F_zt_hj'] = currentJZDataArr['F41096'];



    //var inPressCut = currentJZDataArr['F41006'];
    //var outPressCut = currentJZDataArr['F41007'];



    //var caiJITime = currentPumpStateArr['TempTime'].replace(/[^0-9]/ig, "");
    //caiJITime = changeTime(caiJITime);


    //$('.getDataTime .getDataTxt span').html(caiJITime);



    //$.each(yuAndPHArr, function (index, value) {

    //    $('.PHAndOthersItemBox>li').find('.PHAndOthersTxt').html(value);

    //    if (itemStateArr['bengFangHuanJing'][index] == 0) {
    //        $('.PHAndOthersItemBox>li.' + index).hide();
    //    }
    //});

    //if (itemStateArr['bengFangHuanJing']['first'] == 0) {
    //    $('.PHAndOthers').hide();
    //}

    //$.each(menDengArr, function (index, value) {
    //    var menDengState='';
    //    if (value == 1) {
    //        menDengState = '';
    //    } else {
    //        menDengState = 'warn';
    //    }
    //    $('.everythingIOtemBox>li').find('.everythingItemTxt').addClass(menDengState);

    //    if (itemStateArr['bengFangZhuangTai'][index] == 0) {
    //        $('.everythingIOtemBox>li.' + index).hide();
    //    }
    //});

    //if (itemStateArr['bengFangZhuangTai']['first'] == 0) {
    //    $('.everythingStateBox').hide();
    //}


    //进压力
    if (itemStateArr['pressPaiShui']['F_yl_jsyl'] == 1) {
        var $hLeft = $('.firstLine').height();
        var $hRight = $('.lastLine').height();
        var str = '<div class="waterIn clearfix">' +
                            '<div class="waterInPic"></div>' +
                            '<div class="waterInRight">' +
                           '     <div class="waterTxt">进水压力</div>' +
                          '      <div class="waterNum">4545.45</div>' +
                         '   </div>' +
                        '</div>';

        if ($hLeft >= $hRight) {
            $('.firstLine').append(str);
        } else {

            $('.lastLine').append(str);
        }
    }

    //出压力
    if (itemStateArr['pressPaiShui']['F_yl_csyl'] == 1) {
        var $hLeft = $('.firstLine').height();
        var $hRight = $('.lastLine').height();
        var str = '<div class="waterOut clearfix">' +
                                '    <div class="waterOutSet"></div>' +
                                '    <div class="rightNum">234</div>' +
                                '    <div class="waterOutPic"></div>' +
                                '    <div class="waterOutRight">' +
                                '        <div class="waterTxt">出水压力</div>' +
                                '        <div class="waterNum">4545.45</div>' +
                                '    </div>' +
                                '</div>';

        if ($hLeft >= $hRight) {

            $('.lastLine').append(str);

        } else {
            $('.firstLine').append(str);
        }
    }


    //水箱的显示

    for (var j = 0; j < 4; j++) {
        if (itemStateArr['shuiXiang']['F_sx_sx' + (j + 1)] == 1) {
            var $hLeft = $('.firstLine').height();
            var $hRight = $('.lastLine').height();
            var str = '<div class="shuiXiang F_sx_sx' + (j + 1) + ' clearfix">' +
                               '    <div class="shuiXiangLeft warn"></div>' +
                               '    <div class="shuiXiangRight">' +
                               '        <div class="shuiXiangNum"><span class="upDown down"></span>435.34</div>' +
                               '        <div class="shuiXiangTxt">' + (j + 1) + '#水箱正常</div>' +
                               '        <div class=""></div>' +
                               '    </div>' +
                               '</div>';

            if ($hLeft >= $hRight) {
                $('.lastLine').append(str);
            } else {
                $('.firstLine').append(str);
            }
        }
    }


    //other显示

    if (itemStateArr['other']['first'] == 1) {
        //console.log(11);
        var $hLeft = $('.firstLine').height();
        var $hRight = $('.lastLine').height();
        var strChild = '';
        $.each(itemStateArr['other'], function (index, value) {
            var bgColor = '';
            if (itemStateArr['other'][index] == 1) {
                if (index != 'first') {
                    switch (index) {
                        case 'F_qt_jsll1': {
                            bgColor = 'main';
                            break;
                        }
                        case 'F_qt_csll1': {
                            bgColor = 'main';
                            break;
                        }
                        case 'F_qt_jlll1': {
                            bgColor = 'main';
                            break;
                        }
                        case 'F_qt_clll1': {
                            bgColor = 'main';
                            break;
                        }
                        case 'F_qt_ljdl': {
                            bgColor = 'main';
                            break;
                        }
                        default: {
                            bgColor = 'third';
                            break;
                        }
                    }
                    strChild += ' <li class="leijiItem ' + index + ' clearfix">' +
                                         '            <div class="leijiItemPic ' + bgColor + ' "></div>' +
                                         '            <div class="leijiItemName">' + otherTxt[index] + '：</div>' +
                                         '            <div class="leijiItemNum"></div>' +
                                         '        </li>';
                }

            }

        });
        var strWrap = '<div class="leijiLiangBox">' +
                           '    <ul class="leijiItemBox">' + strChild +
                           '   </ul>' +
                           '</div>';

        if ($hLeft >= $hRight) {
            $('.lastLine').append(strWrap);
        } else {
            $('.firstLine').append(strWrap);
        }


    } else {
        // console.log(222);
    }



    //变频器 

    if (itemStateArr['bianPinVal']['first'] == 1) {
        for (var bianPinVaNum = 0; bianPinVaNum < 6; bianPinVaNum++) {
            if (itemStateArr['bianPinIndex']['F_bpq_' + (bianPinVaNum + 1)] == 1) {
                var $hLeft = $('.firstLine').height();
                var $hRight = $('.lastLine').height();
                var strChild = '';
                $.each(itemStateArr['bianPinVal'], function (index, value) {

                    if (itemStateArr['bianPinVal'][index] == 1) {
                        if (index != 'first') {
                            strChild += ' <li class="bianPinItem ' + index + ' clearfix">' +
                                      '      <div class="bianpinItemPic ' + index + '"></div>' +
                                      '      <div class="bianPinItemName ">' + bianPinValTxt[index] + ':</div>' +
                                      '      <div class="bianPinItemNum"></div>' +
                                      '  </li>';
                        }

                    }

                });

                var strW = '<div class="bianPin ' + 'F_bpq_' + (bianPinVaNum + 1) + '">' +
                               '    <div class="bianPinHeader clearfix">' +
                               '        <div class="bianPinPic">' + (bianPinVaNum + 1) + '</div>' +
                               '        <div class="bianPinTxt">变频器</div>' +
                               '    </div>' +
                               '    <ul class="bianPinItemBox ">' + strChild +
                               '    </ul>' +
                               '</div>';

                if ($hLeft >= $hRight) {
                    $('.lastLine').append(strW);
                } else {
                    $('.firstLine').append(strW);
                }
            }
        }
    }

    //泵


    if (itemStateArr['shuiBeng']['first'] == 1) {
        var strDl = '', strYxsj = '';
        if (itemStateArr['dianLiang']['first'] == 1) {
            strDl += '<li class="pumpItem F_sb_dl clearfix">' +
                                      '    <div class="pumpItemPic dianLiu"></div>' +
                                      '    <div class="pumpItemName space">电流:</div>' +
                                      '    <div class="pumpItemNum">5464.1A</div>' +
                                      '</li>';
        } else {
            strDl += '';
        }

        if (itemStateArr['yxsjShuiBeng']['first'] == 1) {
            strYxsj += ' <li class="pumpItem F_sb_yxsj clearfix">' +
               '     <div class="pumpItemPic yunTime"></div>' +
               '     <div class="pumpItemName">运行时间:</div>' +
               '     <div class="pumpItemNum">5464.1A</div>' +
               '</li>';
        } else {
            strYxsj += '';
        }



        for (var shuiBengZhuNum = 0; shuiBengZhuNum < pumpRun; shuiBengZhuNum++) {
            var $hLeft = $('.firstLine').height();
            var $hRight = $('.lastLine').height();
            var strShuiBZhu = '<div class="pumpStopStart pump' + (shuiBengZhuNum + 1) + '">' +
                               '    <div class="pumpHeader clearfix">' +
                               '        <div class="pumpHeaderPic zhu warn"></div>' +
                               '        <div class="pumpHeaderTxt">' +
                               '            泵<span>' + (shuiBengZhuNum + 1) + '</span>(<i>主</i>)' +
                               '        </div>' +
                               '    </div>' +
                               '    <ul class="pumpItemBox">' + strDl + strYxsj +
                               '    </ul>' +
                               '    <div class="pumpFoot">' +
                               '        <button class="pumpStart">远程启动</button>' +
                               '        <button class="pumpStop">远程停止</button>' +
                               '    </div>' +
                               '</div>';

            if ($hLeft >= $hRight) {
                $('.lastLine').append(strShuiBZhu);
            } else {
                $('.firstLine').append(strShuiBZhu);
            }
        }
        for (var shuiBengFuNum = pumpRun; shuiBengFuNum < (pumpAux + pumpRun) ; shuiBengFuNum++) {
            var $hLeft = $('.firstLine').height();
            var $hRight = $('.lastLine').height();
            var strShuiBFu = '<div class="pumpStopStart pump' + (shuiBengFuNum + 1) + '">' +
                               '    <div class="pumpHeader clearfix">' +
                               '        <div class="pumpHeaderPic  warn"></div>' +
                               '        <div class="pumpHeaderTxt">' +
                               '            泵<span>' + (shuiBengFuNum + 1) + '</span>(<i>辅</i>)' +
                               '        </div>' +
                               '    </div>' +
                               '    <ul class="pumpItemBox">' + strDl + strYxsj +
                               '    </ul>' +
                               '    <div class="pumpFoot">' +
                               '        <button class="pumpStart">远程启动</button>' +
                               '        <button class="pumpStop">远程停止</button>' +
                               '    </div>' +
                               '</div>';

            if ($hLeft >= $hRight) {
                $('.lastLine').append(strShuiBFu);
            } else {
                $('.firstLine').append(strShuiBFu);
            }
        }


    } else {
    }

    //if (itemStateArr['shuiBeng']['first'] == 1) {
    //    for (var i = 0; i < pumpJZNum; i++) {
    //        var $hLeft = $('.firstLine').height();
    //        var $hRight = $('.lastLine').height();
    //        var str = '';
    //        if ($hLeft >= $hRight) {
    //            str += '';
    //        } else {

    //        }
    //    }

    //}


    //电动阀
    for (var dianDong = 0; dianDong < 4; dianDong++) {
        if (itemStateArr['dianDongFa']['F_ddf_df' + (dianDong + 1)] == 1) {
            var $hLeft = $('.firstLine').height();
            var $hRight = $('.lastLine').height();
            var strDianDong = '<div class="dianDongFa F_ddf_df' + (dianDong + 1) + '" >' +
                                '    <div class="dianDongFaHeader">' +
                                '        <div class="dianDongFaNum"></div>' +
                                '        <div class="dianDongFaTxt">' + (dianDong + 1) + '#电动阀开度</div>' +
                                '    </div>' +
                                '    <div class="dianDongFaMid ">' +
                                '        <button class="allOpen">全开</button>' +
                                '        <button class="allClose">全关</button>' +
                                '    </div>' +
                                '    <div class="dianDongFaFoot">' +
                                '        <button class="dianDongFaSet">设置</button>' +
                                '        <button class="dianDongFaReset">复位</button>' +
                                '    </div>' +
                                '</div>';

            if ($hLeft >= $hRight) {
                $('.lastLine').append(strDianDong);
            } else {
                $('.firstLine').append(strDianDong);
            }
        }
    }


    //排水泵

    if (itemStateArr['pressPaiShui']['F_psb_psb'] == 1) {
        var $hLeft = $('.firstLine').height();
        var $hRight = $('.lastLine').height();
        var str = ' <div class="paiShui clearfix">' +
                                '    <div class="paiShuiLeft">' +
                                '        <div class="paiShuiLeftHeader">排水泵</div>' +
                                '        <ul class="paiShuiList">' +
                                '        </ul>' +
                                '    </div>' +
                                '    <div class="paiShuiRight">' +
                                '        <button class="paiShuiOpen active">开</button><button class="paiShuiClose">关</button>' +
                                '    </div>' +
                                '</div>';

        if ($hLeft >= $hRight) {
            $('.lastLine').append(str);
        } else {
            $('.firstLine').append(str);
        }
    }


    //设置 框的   内容显示处理

    //设备启停
    if (itemStateArr['qiTing']['F_szmb_sbjjqt'] == 1) {
        $('.kongzhiStop>li').eq(0).addClass('active');
        
    }
    if (itemStateArr['qiTing']['F_szmb_kzbqt'] == 1) {
        for (var kzbqtNum = 1; kzbqtNum <= pumpJZNum; kzbqtNum++) {
            $('.kongzhiStop>li').eq(kzbqtNum).addClass('active');
        }
       
    }
    if (itemStateArr['qiTing']['F_szmb_xdsz'] == 1) {
        $('.xianDingSet').addClass('active');
    }

}

//右侧  列表 载入时的  数据 处理  
function rightDataItemDataFn(currentPumpData) {
    var index = $('.pageLeftTop li.active').index();
     //alert(index);

    var pumpAux = currentPumpData.pumpJZ[index].Auxiliarypumpcount;
    var pumpRun = currentPumpData.pumpJZ[index].RunPumpNum
    var pumpJZNum = pumpAux + pumpRun;
    var outNum = currentPumpData.pumpJZ[index].DrainPumpNum;
    var adress = currentPumpData.PAddress;
    var caiJiPeriad = currentPumpData.pumpJZ[index].PumpJZCollectPeriod;

    var currentPumpStateArr = currentPumpData.pumpJZ[index].D_Data[0];
    var currentJZDataArr = currentPumpStateArr;
    var caijiTime = currentJZDataArr['FUpdateDate'];
    var yuAndPHArr = {};
    yuAndPHArr['F_hj_yl'] = currentJZDataArr['F41087'];
    yuAndPHArr['F_hj_zd'] = currentJZDataArr['F41088'];
    yuAndPHArr['F_hj_ph'] = currentJZDataArr['F41089'];
    yuAndPHArr['F_hj_sw'] = currentJZDataArr['F41092'];
    yuAndPHArr['F_hj_rjy'] = currentJZDataArr['F41091'];
    yuAndPHArr['F_hj_ddl'] = currentJZDataArr['F41090'];
    yuAndPHArr['F_hj_sd'] = currentJZDataArr['F41100'];
    yuAndPHArr['F_hj_wd'] = currentJZDataArr['F41101'];
    yuAndPHArr['F_hj_wsyw'] = currentJZDataArr['F41101'];


    var menDengArr = {};

    menDengArr['F_zt_m'] = currentJZDataArr['F41093'];
    menDengArr['F_zt_d'] = currentJZDataArr['F41097'];
    menDengArr['F_zt_dy'] = currentJZDataArr['F41099'];
    menDengArr['F_zt_pc'] = currentJZDataArr['F41095'];
    menDengArr['F_zt_ups'] = currentJZDataArr['F41098'];
    menDengArr['F_zt_ls'] = currentJZDataArr['F41094'];
    menDengArr['F_zt_hj'] = currentJZDataArr['F41096'];



    var inPressCut = currentJZDataArr['F41006'];
    var outPressCut = currentJZDataArr['F41007'];
    var outPressSet = currentJZDataArr['F41141'];

    var shuXiangDaArr = {};

    if (currentPumpStateArr['TempTime']) {
        var caiJITime = currentPumpStateArr['TempTime'].replace(/[^0-9]/ig, "");
    }
   
    caiJITime = changeTime(caiJITime);


    $('.getDataTime .getDataTxt span').html(caiJITime);

    $('.setBox .timeBoxTxtContent').html(caiJITime);
    $('.adressBox .adressBoxTxtContent').html(adress);
    

    



    $.each(yuAndPHArr, function (index, value) {
        //  alert(index);
        var valTxt = (value || value == 0) ? value : '';
       // alert(valTxt);
        $('.PHAndOthersItemBox>li.'+index).find('.PHAndOthersTxt').html(valTxt);

        if (itemStateArr['bengFangHuanJing'][index] == 0) {
            $('.PHAndOthersItemBox>li.' + index).hide();
        } else {
            $('.PHAndOthersItemBox>li.' + index).fadeIn();
        }
    });

    if (itemStateArr['bengFangHuanJing']['first'] == 0) {
        $('.PHAndOthers').hide();
    } else {
        $('.PHAndOthers').fadeIn();
    }

    $.each(menDengArr, function (index, value) {

        var menDengState = '';
        if (value == 1) {
            menDengState = 'warn';
        } else {
            menDengState = '';
        }
       
        $('.everythingIOtemBox>li.'+index).find('.everythingItemTxt').addClass(menDengState);

        if (itemStateArr['bengFangZhuangTai'][index] == 0) {
            $('.everythingIOtemBox>li.' + index).hide();
        } else {
            $('.everythingIOtemBox>li.' + index).fadeIn();
        }
    });

    if (itemStateArr['bengFangZhuangTai']['first'] == 0) {
        $('.everythingStateBox').hide();
    } else {
        $('.everythingStateBox').fadeIn();
    }





    //set 
    var menDengSetArr = {};

    menDengSetArr['F_szmb_m'] = currentJZDataArr['F41093'];
    menDengSetArr['F_szmb_d'] = currentJZDataArr['F41097'];
    menDengSetArr['F_szmb_fswd'] = currentJZDataArr['F41099'];
    menDengSetArr['F_szmb_psb'] = currentJZDataArr['F41719'];
    menDengSetArr['F_szmb_xdy'] = currentJZDataArr['F41720'];
    menDengSetArr['F_szmb_dsqh'] = currentJZDataArr['F41713'];
    menDengSetArr['F_szmb_csyl'] = currentJZDataArr['F41141']; //F41702
    menDengSetArr['F_szmb_ddf'] = currentJZDataArr['F41709'];


    //console.log(currentJZDataArr);
    //console.log(menDengSetArr);
    //console.log('qinqinqin222');

   

    // set  里边的  
    $.each(menDengSetArr, function (index, value) {
       
        //console.log(menDengSetArr);
        //console.log('13qinqin');
        //alert(index);
       // alert(value);
        var menDengSetState = '';
        if (value == 1) {
            menDengSetState = 'kai';
            $('.menDengBox>li.' + index).find('.menDengTopPic').removeClass('kai').addClass(menDengSetState);
            $('.menDengBox>li.' + index).find('.menDengButtonBox .menDengOpen').addClass('active');
            $('.menDengBox>li.' + index).find('.menDengButtonBox .menDengClose').removeClass('active');
        } else {
            menDengSetState = '';
            $('.menDengBox>li.' + index).find('.menDengTopPic').removeClass('kai').addClass(menDengSetState);
            $('.menDengBox>li.' + index).find('.menDengButtonBox .menDengClose').addClass('active');
            $('.menDengBox>li.' + index).find('.menDengButtonBox .menDengOpen').removeClass('active');
        }

        if (index == 'F_szmb_ddf') {
           
            if (menDengSetArr['F_szmb_ddf'] != null && menDengSetArr['F_szmb_ddf'] != 'undefined' && menDengSetArr['F_szmb_ddf'] != ' ') {
               // alert(menDengSetArr['F_szmb_ddf']);
                $('.menDengBox>li.' + index).find('.diandongfaBg').html(menDengSetArr['F_szmb_ddf'] + '%');
            }
        }
       
        if (index == 'F_szmb_csyl') {
            if (menDengSetArr['chushuiyaBg'] != 'null' && menDengSetArr['chushuiyaBg'] != 'undefined') {
           
                $('.menDengBox>li.' + index).find('.chushuiyaBg').html(menDengSetArr['F_szmb_csyl']);
            }
        }
       
        if (index == 'F_szmb_dsqh') {
            if (menDengSetArr['dingshiBg'] != 'null' && menDengSetArr['dingshiBg'] != 'undefined' ) {
                $('.menDengBox>li.' + index).find('.dingshiBg').html(menDengSetArr['F_szmb_dsqh']);
            }
        }

        if (itemStateArr['panalShowHide'][index] == 0) {
            $('.menDengBox>li.' + index).hide();
        } else {
            $('.menDengBox>li.' + index).show();
        }
    });

    if (itemStateArr['panalShowHide']['first'] == 0) {
        $('.menDengBox').hide();
    } else {
        $('.menDengBox').show();
    }



    //控制 设备的 启停  

    //switch (pmpStateArr[value]) {
    //    case '0': {
    //        sta = 'offline';
    //        break;
    //    }
    //    case '1': {
    //        sta = 'offline';
    //        break;
    //    }
    //    case '2': {
    //        sta = 'run';
    //        break;
    //    }
    //    case '3': {
    //        sta = 'warn';
    //        break;
    //    }
    //    default: {
    //        sta = '1';
    //    }
    //}
    var qitingState = {};
    qitingState['F_szmb_sbjjqt'] = currentJZDataArr['F41701'];
    qitingState['F_szmb_kzbqt1'] = currentJZDataArr['F41008'];
    qitingState['F_szmb_kzbqt2'] = currentJZDataArr['F41009'];
    qitingState['F_szmb_kzbqt3'] = currentJZDataArr['F41010'];
    qitingState['F_szmb_kzbqt4'] = currentJZDataArr['F41011'];
    qitingState['F_szmb_kzbqt5'] = currentJZDataArr['F41012'];
    qitingState['F_szmb_kzbqt6'] = currentJZDataArr['F41013'];
    

    $.each(qitingState, function (index, value) {
      // alert(index);
       // alert(value);
        switch (value) {
            case '0': {
                $('.kongzhiItem.' + index).find('.start').removeClass('active');
                $('.kongzhiItem.' + index).find('.stop').addClass('active');
                break;
            }
            case '1': {
              //  alert( $('.kongzhiItem.' + index).find('.start').html());
                $('.kongzhiItem.' + index).find('.start').removeClass('active');
                $('.kongzhiItem.' + index).find('.stop').addClass('active');
                break;
            }
            case '2': {
                $('.kongzhiItem.' + index).find('.start').addClass('active');
                $('.kongzhiItem.' + index).find('.stop').removeClass('active');
                break;
            }
            default: {
                $('.kongzhiItem.' + index).find('.start').removeClass('active');
                $('.kongzhiItem.' + index).find('.stop').addClass('active');
                break;
            }
        }
        
    });


    //进出水  压力 数据

    $('.waterIn  .waterNum').html(inPressCut);
    $('.waterOut  .waterNum').html(outPressCut);
    $('.waterOut  .rightNum').html(outPressSet);


    //水箱数据的  处理


    var shuXiangYeWeiArr = {};

    shuXiangYeWeiArr['F_sx_sx1'] = currentJZDataArr['F41020'];
    shuXiangYeWeiArr['F_sx_sx2'] = currentJZDataArr['F41021'];
    shuXiangYeWeiArr['F_sx_sx3'] = currentJZDataArr['F41022'];
    shuXiangYeWeiArr['F_sx_sx4'] = currentJZDataArr['F41023'];


    var shuiXiang1BaoJing = {};
    shuiXiang1BaoJing['gao'] = currentJZDataArr['F41035'];
    shuiXiang1BaoJing['di'] = currentJZDataArr['F41036'];

    var shuiXiang2BaoJing = {};
    shuiXiang1BaoJing['gao'] = currentJZDataArr['F41037'];
    shuiXiang1BaoJing['di'] = currentJZDataArr['F41038'];


    for (var shuiXiangYeweiNum = 0; shuiXiangYeweiNum < 4; shuiXiangYeweiNum++) {

        var downOrUp = '';
        var shuixiangSta = '';
        var warnTxt = '';
        switch (shuiXiangYeweiNum) {
            case 0: {

                if ((shuiXiang1BaoJing)['gao'] == 1) {

                    downOrUp = 'up';
                    shuixiangSta = 'warn';
                    warnTxt = '1#水箱高位报警';
                } else if ((shuiXiang1BaoJing)['di'] == 1) {
                    downOrUp = 'down';
                    shuixiangSta = 'warn';
                    warnTxt = '1#水箱低位报警';
                } else {
                    downOrUp = '';
                    shuixiangSta = '';
                    warnTxt = '1#水箱正常';
                }
                break;
            }
            case 1: {

                if ((shuiXiang2BaoJing)['gao'] == 1) {
                    downOrUp = 'up';
                    shuixiangSta = 'warn';
                    warnTxt = '2#水箱高位报警';

                } else if ((shuiXiang2BaoJing)['di'] == 1) {
                    downOrUp = 'down';
                    shuixiangSta = 'warn';
                    warnTxt = '2#水箱低位报警';
                } else {
                    downOrUp = '';
                    shuixiangSta = '';
                    warnTxt = '2#水箱正常';
                }
                break;
            }
                //case 2: {
                //    if ((shuiXiang3BaoJing)['gao'] == 1) {
                //        downOrUp = 'up';
                //    } else if ((shuiXiang3BaoJing)['di'] == 1) {
                //        downOrUp = 'down';
                //    } else {
                //        downOrUp = '';
                //    }
                //    break;
                //}
                //case 3: {
                //    if ((shuiXiang4BaoJing)['gao'] == 1) {
                //        downOrUp = 'up';
                //    } else if ((shuiXiang4BaoJing)['di'] == 1) {
                //        downOrUp = 'down';
                //    } else {
                //        downOrUp = '';
                //    }
                //    break;
                //}
        }

        // alert(shuXiangYeWeiArr['F_sx_sx'+(shuiXiangYeweiNum + 1)]);
        $('.F_sx_sx' + (shuiXiangYeweiNum + 1)).find('.shuiXiangLeft').removeClass('warn').addClass(shuixiangSta);
        $('.F_sx_sx' + (shuiXiangYeweiNum + 1)).find('.shuiXiangNum').html('<span class="upDown ' + downOrUp + '"></span>' + shuXiangYeWeiArr['F_sx_sx' + (shuiXiangYeweiNum + 1)]);
    }



    //other  的数据处理

    //1#
    var otherDataArr1 = {};

    otherDataArr1['F_qt_jsll1'] = currentJZDataArr['F41024'];
    otherDataArr1['F_qt_csll1'] = currentJZDataArr['F41025'];
    otherDataArr1['F_qt_jlll1'] = currentJZDataArr['FTotalInLL'];
    otherDataArr1['F_qt_clll1'] = currentJZDataArr['FTotalOutLL'];
    otherDataArr1['F_qt_ljdl'] = currentJZDataArr['FTotalDL'];

    $(other).each(function (index, value) {

      
        $('.leijiItemBox').find('.' + value).find('.leijiItemNum').html(otherDataArr1[value]);


    });



    //变频

    var bianPin1Arr = {};
    bianPin1Arr['F_bpq_yxsj'] = currentJZDataArr['F41057'];
    bianPin1Arr['F_bpq_yxpl'] = currentJZDataArr['F41014'];
    bianPin1Arr['F_bpq_zldy'] = currentJZDataArr['F41069'];
    bianPin1Arr['F_bpq_scdy'] = currentJZDataArr['F41075'];
    bianPin1Arr['F_bpq_wd'] = currentJZDataArr['F41081'];
    bianPin1Arr['F_bpq_gl'] = currentJZDataArr['F41063'];

    var bianPin2Arr = {};
    bianPin2Arr['F_bpq_yxsj'] = currentJZDataArr['F41058'];
    bianPin2Arr['F_bpq_yxpl'] = currentJZDataArr['F41015'];
    bianPin2Arr['F_bpq_zldy'] = currentJZDataArr['F41070'];
    bianPin2Arr['F_bpq_scdy'] = currentJZDataArr['F41076'];
    bianPin2Arr['F_bpq_wd'] = currentJZDataArr['F41082'];
    bianPin2Arr['F_bpq_gl'] = currentJZDataArr['F41064'];

    var bianPin3Arr = {};
    bianPin3Arr['F_bpq_yxsj'] = currentJZDataArr['F41059'];
    bianPin3Arr['F_bpq_yxpl'] = currentJZDataArr['F41016'];
    bianPin3Arr['F_bpq_zldy'] = currentJZDataArr['F41071'];
    bianPin3Arr['F_bpq_scdy'] = currentJZDataArr['F41077'];
    bianPin3Arr['F_bpq_wd'] = currentJZDataArr['F41083'];
    bianPin3Arr['F_bpq_gl'] = currentJZDataArr['F41065'];

    var bianPin4Arr = {};
    bianPin4Arr['F_bpq_yxsj'] = currentJZDataArr['F41060'];
    bianPin4Arr['F_bpq_yxpl'] = currentJZDataArr['F41017'];
    bianPin4Arr['F_bpq_zldy'] = currentJZDataArr['F41072'];
    bianPin4Arr['F_bpq_scdy'] = currentJZDataArr['F41078'];
    bianPin4Arr['F_bpq_wd'] = currentJZDataArr['F41084'];
    bianPin4Arr['F_bpq_gl'] = currentJZDataArr['F41066'];

    var bianPin5Arr = {};
    bianPin5Arr['F_bpq_yxsj'] = currentJZDataArr['F41061'];
    bianPin5Arr['F_bpq_yxpl'] = currentJZDataArr['F41018'];
    bianPin5Arr['F_bpq_zldy'] = currentJZDataArr['F41073'];
    bianPin5Arr['F_bpq_scdy'] = currentJZDataArr['F41079'];
    bianPin5Arr['F_bpq_wd'] = currentJZDataArr['F41085'];
    bianPin5Arr['F_bpq_gl'] = currentJZDataArr['F41067'];

    var bianPin6Arr = {};
    bianPin6Arr['F_bpq_yxsj'] = currentJZDataArr['F41062'];
    bianPin6Arr['F_bpq_yxpl'] = currentJZDataArr['F41019'];
    bianPin6Arr['F_bpq_zldy'] = currentJZDataArr['F41074'];
    bianPin6Arr['F_bpq_scdy'] = currentJZDataArr['F41080'];
    bianPin6Arr['F_bpq_wd'] = currentJZDataArr['F41086'];
    bianPin6Arr['F_bpq_gl'] = currentJZDataArr['F41068'];


    $(bianPinIndex).each(function (index, value) {

        switch (index) {
            case 0: {
                $('.' + value).find('li.F_bpq_yxsj').find('.bianPinItemNum').html(bianPin1Arr['F_bpq_yxsj']);
                $('.' + value).find('li.F_bpq_yxpl').find('.bianPinItemNum').html(bianPin1Arr['F_bpq_yxpl']);
                $('.' + value).find('li.F_bpq_zldy').find('.bianPinItemNum').html(bianPin1Arr['F_bpq_zldy']);
                $('.' + value).find('li.F_bpq_scdy').find('.bianPinItemNum').html(bianPin1Arr['F_bpq_scdy']);
                $('.' + value).find('li.F_bpq_wd').find('.bianPinItemNum').html(bianPin1Arr['F_bpq_wd']);
                $('.' + value).find('li.F_bpq_gl').find('.bianPinItemNum').html(bianPin1Arr['F_bpq_gl']);
                break;
            }
            case 1: {
                $('.' + value).find('li.F_bpq_yxsj').find('.bianPinItemNum').html(bianPin2Arr['F_bpq_yxsj']);
                $('.' + value).find('li.F_bpq_yxpl').find('.bianPinItemNum').html(bianPin2Arr['F_bpq_yxpl']);
                $('.' + value).find('li.F_bpq_zldy').find('.bianPinItemNum').html(bianPin2Arr['F_bpq_zldy']);
                $('.' + value).find('li.F_bpq_scdy').find('.bianPinItemNum').html(bianPin2Arr['F_bpq_scdy']);
                $('.' + value).find('li.F_bpq_wd').find('.bianPinItemNum').html(bianPin2Arr['F_bpq_wd']);
                $('.' + value).find('li.F_bpq_gl').find('.bianPinItemNum').html(bianPin2Arr['F_bpq_gl']);
                break;
            }
            case 2: {
                $('.' + value).find('li.F_bpq_yxsj').find('.bianPinItemNum').html(bianPin3Arr['F_bpq_yxsj']);
                $('.' + value).find('li.F_bpq_yxpl').find('.bianPinItemNum').html(bianPin3Arr['F_bpq_yxpl']);
                $('.' + value).find('li.F_bpq_zldy').find('.bianPinItemNum').html(bianPin3Arr['F_bpq_zldy']);
                $('.' + value).find('li.F_bpq_scdy').find('.bianPinItemNum').html(bianPin3Arr['F_bpq_scdy']);
                $('.' + value).find('li.F_bpq_wd').find('.bianPinItemNum').html(bianPin3Arr['F_bpq_wd']);
                $('.' + value).find('li.F_bpq_gl').find('.bianPinItemNum').html(bianPin3Arr['F_bpq_gl']);
                break;
            }
            case 3: {
                $('.' + value).find('li.F_bpq_yxsj').find('.bianPinItemNum').html(bianPin4Arr['F_bpq_yxsj']);
                $('.' + value).find('li.F_bpq_yxpl').find('.bianPinItemNum').html(bianPin4Arr['F_bpq_yxpl']);
                $('.' + value).find('li.F_bpq_zldy').find('.bianPinItemNum').html(bianPin4Arr['F_bpq_zldy']);
                $('.' + value).find('li.F_bpq_scdy').find('.bianPinItemNum').html(bianPin4Arr['F_bpq_scdy']);
                $('.' + value).find('li.F_bpq_wd').find('.bianPinItemNum').html(bianPin4Arr['F_bpq_wd']);
                $('.' + value).find('li.F_bpq_gl').find('.bianPinItemNum').html(bianPin4Arr['F_bpq_gl']);
                break;
            }
            case 4: {
                $('.' + value).find('li.F_bpq_yxsj').find('.bianPinItemNum').html(bianPin5Arr['F_bpq_yxsj']);
                $('.' + value).find('li.F_bpq_yxpl').find('.bianPinItemNum').html(bianPin5Arr['F_bpq_yxpl']);
                $('.' + value).find('li.F_bpq_zldy').find('.bianPinItemNum').html(bianPin5Arr['F_bpq_zldy']);
                $('.' + value).find('li.F_bpq_scdy').find('.bianPinItemNum').html(bianPin5Arr['F_bpq_scdy']);
                $('.' + value).find('li.F_bpq_wd').find('.bianPinItemNum').html(bianPin5Arr['F_bpq_wd']);
                $('.' + value).find('li.F_bpq_gl').find('.bianPinItemNum').html(bianPin5Arr['F_bpq_gl']);
                break;
            }
            case 5: {
                $('.' + value).find('li.F_bpq_yxsj').find('.bianPinItemNum').html(bianPin6Arr['F_bpq_yxsj']);
                $('.' + value).find('li.F_bpq_yxpl').find('.bianPinItemNum').html(bianPin6Arr['F_bpq_yxpl']);
                $('.' + value).find('li.F_bpq_zldy').find('.bianPinItemNum').html(bianPin6Arr['F_bpq_zldy']);
                $('.' + value).find('li.F_bpq_scdy').find('.bianPinItemNum').html(bianPin6Arr['F_bpq_scdy']);
                $('.' + value).find('li.F_bpq_wd').find('.bianPinItemNum').html(bianPin6Arr['F_bpq_wd']);
                $('.' + value).find('li.F_bpq_gl').find('.bianPinItemNum').html(bianPin6Arr['F_bpq_gl']);
                break;
            }
        }

    });



    //泵的 数据 


    var pmpStateArr = {};
    var pumpDlStateArr = {};
    var pumpRunTimeStateArr = {};
    pmpStateArr['pump1'] = currentJZDataArr['F41008'];
    pmpStateArr['pump2'] = currentJZDataArr['F41009'];
    pmpStateArr['pump3'] = currentJZDataArr['F41010'];
    pmpStateArr['pump4'] = currentJZDataArr['F41011'];
    pmpStateArr['pump5'] = currentJZDataArr['F41012'];
    pmpStateArr['pump6'] = currentJZDataArr['F41013'];



    pumpDlStateArr['pump1'] = currentPumpStateArr['F41045'];
    pumpDlStateArr['pump2'] = currentPumpStateArr['F41046'];
    pumpDlStateArr['pump3'] = currentPumpStateArr['F41047'];
    pumpDlStateArr['pump4'] = currentPumpStateArr['F41048'];
    pumpDlStateArr['pump5'] = currentPumpStateArr['F41049'];
    pumpDlStateArr['pump6'] = currentPumpStateArr['F41050'];

    pumpRunTimeStateArr['pump1'] = currentPumpStateArr['F41051'];
    pumpRunTimeStateArr['pump2'] = currentPumpStateArr['F41052'];
    pumpRunTimeStateArr['pump3'] = currentPumpStateArr['F41053'];
    pumpRunTimeStateArr['pump4'] = currentPumpStateArr['F41054'];
    pumpRunTimeStateArr['pump5'] = currentPumpStateArr['F41055'];
    pumpRunTimeStateArr['pump6'] = currentPumpStateArr['F41056'];



    $(pumpArr).each(function (index, value) {
        var sta = '';
        switch (pmpStateArr[value]) {
            case '0': {
                sta = 'offline';
                break;
            }
            case '1': {
                sta = 'offline';
                break;
            }
            case '2': {
                sta = 'run';
                break;
            }
            case '3': {
                sta = 'warn';
                break;
            }
            default: {
                sta = '1';
            }
        }
        
        
        $('.' + value).find('.pumpHeaderPic').removeClass('warn run offline').addClass(sta);
        $('.' + value).find('.F_sb_dl .pumpItemNum').html(pumpDlStateArr[value] || (pumpDlStateArr[value] == 0) ? pumpDlStateArr[value] + 'A' : ' ' + 'A');
        $('.' + value).find('.F_sb_yxsj .pumpItemNum').html(pumpRunTimeStateArr[value] || (pumpRunTimeStateArr[value] == 0) ? pumpRunTimeStateArr[value] + 'h' : ' ' + 'h');
    });



    //电动阀的  数据

    var diandongfaArr = {};

    diandongfaArr['F_ddf_df1'] = currentJZDataArr['F41041'];
    diandongfaArr['F_ddf_df2'] = currentJZDataArr['F41044'];
    diandongfaArr['F_ddf_df3'] = '无';
    diandongfaArr['F_ddf_df4'] = '无';


    var dianDong1OpenClose = {};
    dianDong1OpenClose['open'] = currentJZDataArr['F41039'];
    dianDong1OpenClose['close'] = currentJZDataArr['F41040'];

    var dianDong2OpenClose = {};
    dianDong1OpenClose['open'] = currentJZDataArr['F41042'];
    dianDong1OpenClose['close'] = currentJZDataArr['F41043'];


    for (var dianDongfaNum = 0; dianDongfaNum < 4; dianDongfaNum++) {
        // alert((dianDong1OpenClose)['open']);
        //alert((dianDong1OpenClose)['close']);
        var openORClose = '';
        switch (dianDongfaNum) {
            case 0: {

                if ((dianDong1OpenClose)['open'] == 1) {

                    openORClose = 'open';
                    $('.F_ddf_df' + (dianDongfaNum + 1)).find('.allOpen').addClass('active');
                    $('.F_ddf_df' + (dianDongfaNum + 1)).find('.allClose').removeClass('active');
                } else if ((dianDong1OpenClose)['close'] == 1) {
                    openORClose = 'close';
                    $('.F_ddf_df' + (dianDongfaNum + 1)).find('.allClose').addClass('active');
                    $('.F_ddf_df' + (dianDongfaNum + 1)).find('.allOpen').removeClass('active');
                } else {
                    downOrUp = '';
                    shuixiangSta = '';
                    warnTxt = '1#水箱正常';
                }
                break;
            }

                //case 2: {
                //    if ((shuiXiang3BaoJing)['gao'] == 1) {
                //        downOrUp = 'up';
                //    } else if ((shuiXiang3BaoJing)['di'] == 1) {
                //        downOrUp = 'down';
                //    } else {
                //        downOrUp = '';
                //    }
                //    break;
                //}
                //case 3: {
                //    if ((shuiXiang4BaoJing)['gao'] == 1) {
                //        downOrUp = 'up';
                //    } else if ((shuiXiang4BaoJing)['di'] == 1) {
                //        downOrUp = 'down';
                //    } else {
                //        downOrUp = '';
                //    }
                //    break;
                //}
        }

        $('.F_ddf_df' + (dianDongfaNum + 1)).find('.dianDongFaNum').html(diandongfaArr['F_ddf_df' + (dianDongfaNum + 1)]);

    }




    //排水泵  数据

    var paiShuiStateArr = [];
    paiShuiStateArr[0] = currentJZDataArr['F41102'];
    paiShuiStateArr[1] = currentJZDataArr['F41103'];
    paiShuiStateArr[2] = currentJZDataArr['F41104'];
    paiShuiStateArr[3] = currentJZDataArr['F41105'];


    var paiShuiChildStr = '';
    //alert(outNum);
    for (var paiSHuiNum = 0; paiSHuiNum < outNum; paiSHuiNum++) {
        var paiShuiChildSta = '';
       // alert(paiShuiStateArr[paiSHuiNum]);
        switch (paiShuiStateArr[paiSHuiNum]) {
            case '0': {
                paiShuiChildSta = 'offline';
                break;
            }
            case '1': {
                paiShuiChildSta = 'offline';
                break;
            }
            case '2': {
                paiShuiChildSta = 'run';
                break;
            }
            case '3': {
                paiShuiChildSta = 'warn';
                break;
            }
            default: {
                paiShuiChildSta = 'offline';
                break;
            }
        }
        paiShuiChildStr += '<li class="' + paiShuiChildSta + '"></li>';

    }

    $('.paiShuiList').html(paiShuiChildStr);







}




//set  框内  下达命令

function setBoxControlCommond() {
    var setItemArr = ['F_szmb_m',
                      'F_szmb_d', 
                      'F_szmb_fswd',
                      'F_szmb_psb',
                      'F_szmb_xdy'];

    var setItem2 = {
        'dataValBox': 'dianDongFaSet',
        'chuShuiValBox': 'chuShuiYaSet',
        'dingShiBox': 'dingShiSet'
    };

    var aPumpCommondArr = [
        'F_szmb_sbjjqt',
        'F_szmb_kzbqt1',
        'F_szmb_kzbqt2',
        'F_szmb_kzbqt3',
        'F_szmb_kzbqt4',
        'F_szmb_kzbqt5',
        'F_szmb_kzbqt6'
    ];

    var timerOnoff = true;
    $.each(setItemArr, function (ind,val) {
        $('body').delegate('.menDengBox li.' + val + ' .menDengOpen', 'click', function () {
            var This = $(this);
            if (!timerOnoff) {
                return;
            }

           
            clearInterval(This.timer);
            var time = 5;
            timerOnoff = false;
            


          

                // commondFn('F40017', val,data.baseId,8,data.FDTUCode,data.FSchemeID);
                $.ajax({
                    url: '/V_CDJK/GetCommand',
                    cache: false,
                    data: {
                        'name': This.parent().parent().attr('data-name'),
                        'text': 1,
                        'id': currentJZ,
                        'type': 1,
                        'dtu': currentPumpData.pumpJZ[0].DTUCode,
                        'FSchemeID': currentPumpData.pumpJZ[0].FSchemeID
                    },
                    success: function (data) {

                        var dataJSON = JSON.parse(data);
                        if (dataJSON.success) {
                            tipMsg(dataJSON.msg);
                            $('.kaiduZhi').attr('disabled', true);
                            This.timer = setInterval(function () {
                                if (time == -1) {
                                    clearInterval(This.timer);
                                    //$TiaoFengKaiSet.animate(val);
                                    This.html('开');
                                    timerOnoff = true;
                                    $('.kaiduZhi').attr('disabled', false).val('');
                                    return;
                                }
                                This.html('(' + time + ')');
                                time--;
                            }, 1000);

                        } else {
                            timerOnoff = true;
                            tipMsg(dataJSON.msg);
                        }
                    },
                    error: function (data) {

                    }
                });
            

        });
        $('body').delegate('.menDengBox li.' + val + ' .menDengClose', 'click', function () {
            var This = $(this);
            if (!timerOnoff) {
                return;
            }


            clearInterval(This.timer);
            var time = 5;
            timerOnoff = false;


            var txt = '';
            var dataName = '';
            if (val == 'F_szmb_fswd') {
              
                txt = 0;
                dataName = This.parent().parent().attr('data-name2');
            } else {
               
                txt = 2;
                dataName = This.parent().parent().attr('data-name');
            }
            
           
            // commondFn('F40017', val,data.baseId,8,data.FDTUCode,data.FSchemeID);
            $.ajax({
                url: '/V_CDJK/GetCommand',
                cache: false,
                data: {
                    'name': dataName,
                    'text': txt,
                    'id': currentJZ,
                    'type': 1,
                    'dtu': currentPumpData.pumpJZ[0].DTUCode,
                    'FSchemeID': currentPumpData.pumpJZ[0].FSchemeID
                },
                success: function (data) {
                  
                 var dataJSON = JSON.parse(data);
                 if (dataJSON.success) {
                     tipMsg(dataJSON.msg);
                $('.kaiduZhi').attr('disabled', true);
                This.timer = setInterval(function () {
                    if (time == -1) {
                        clearInterval(This.timer);
                        //$TiaoFengKaiSet.animate(val);
                        This.html('关');
                        timerOnoff = true;
                        $('.kaiduZhi').attr('disabled', false).val('');
                        return;
                    }
                    This.html('(' + time + ')');
                    time--;
                }, 1000);

            } else {
                timerOnoff = true;
                tipMsg(dataJSON.msg);
            }
                },
                error: function (data) {

                }
            });

        });
    });


   
    for (var val in setItem2) {
        // alert(setItem2[val]);
        var vvv = val;
        $('body').delegate('.menDengBox li .' + (setItem2[val]), 'click', function () {
            var This = $(this);
            if (!timerOnoff) {
                return;
            }


            clearInterval(This.timer);
            var time = 5;
            timerOnoff = false;
            var inputVal = $.trim(This.prev().val());
           
           

            if (inputVal != '') {
                //alert(val);

               // alert(inputVal);
                $.ajax({
                    url: '/V_CDJK/GetCommand',
                    cache: false,
                    data: {
                        'name': This.parent().parent().attr('data-name'),
                        'text': inputVal,
                        'id': currentJZ,
                        'type': 1,
                        'dtu': currentPumpData.pumpJZ[0].DTUCode,
                        'FSchemeID': currentPumpData.pumpJZ[0].FSchemeID
                    },
                    success: function (data) {

                        var dataJSON = JSON.parse(data);
                        if (dataJSON.success) {
                            tipMsg(dataJSON.msg);
                            This.timer = setInterval(function () {
                                if (time == -1) {
                                    clearInterval(This.timer);
                                    //$TiaoFengKaiSet.animate(val);
                                    This.html('设置');
                                    timerOnoff = true;
                                   
                                    return;
                                }
                                This.html('(' + time + ')');
                                time--;
                            }, 1000);

                        } else {
                            timerOnoff = true;
                            tipMsg(dataJSON.msg);
                        }
                    },
                    error: function (data) {

                    }
                });
            } else {
                tipMsg(dataJSON.msg);
                timerOnoff = true;
                return;
            }

            
        });
    }
    


    $.each(aPumpCommondArr, function (ind, val) {
        $('body').delegate('.kongzhiStop li.' + val + ' .kongzhiRight .start', 'click', function () {
            var This = $(this);
            if (!timerOnoff) {
                return;
            }


            clearInterval(This.timer);
            var time = 5;
            timerOnoff = false;





            // commondFn('F40017', val,data.baseId,8,data.FDTUCode,data.FSchemeID);
            $.ajax({
                url: '/V_CDJK/GetCommand',
                cache: false,
                data: {
                    'name': This.parent().parent().attr('data-name'),
                    'text': 1,
                    'id': currentJZ,
                    'type': 1,
                    'dtu': currentPumpData.pumpJZ[0].DTUCode,
                    'FSchemeID': currentPumpData.pumpJZ[0].FSchemeID
                },
                success: function (data) {

                    var dataJSON = JSON.parse(data);
                    if (dataJSON.success) {
                        tipMsg(dataJSON.msg);
                        $('.kaiduZhi').attr('disabled', true);
                        This.timer = setInterval(function () {
                            if (time == -1) {
                                clearInterval(This.timer);
                                //$TiaoFengKaiSet.animate(val);
                                This.html('启动');
                                timerOnoff = true;
                                $('.kaiduZhi').attr('disabled', false).val('');
                                return;
                            }
                            This.html('(' + time + ')');
                            time--;
                        }, 1000);

                    } else {
                        timerOnoff = true;
                        tipMsg(dataJSON.msg);
                       
                    }
                },
                error: function (data) {

                }
            });


        });
        $('body').delegate('.kongzhiStop li.' + val + ' .kongzhiRight .stop', 'click', function () {
            var This = $(this);
            if (!timerOnoff) {
                return;
            }


            clearInterval(This.timer);
            var time = 5;
            timerOnoff = false;


            var txt = '';
            var dataName = '';
            if (val == 'F_szmb_fswd') {

                txt = 0;
                dataName = This.parent().parent().attr('data-name2');
            } else {

                txt = 2;
                dataName = This.parent().parent().attr('data-name');
            }


            // commondFn('F40017', val,data.baseId,8,data.FDTUCode,data.FSchemeID);
            $.ajax({
                url: '/V_CDJK/GetCommand',
                cache: false,
                data: {
                    'name': dataName,
                    'text': txt,
                    'id': currentJZ,
                    'type': 1,
                    'dtu': currentPumpData.pumpJZ[0].DTUCode,
                    'FSchemeID': currentPumpData.pumpJZ[0].FSchemeID
                },
                success: function (data) {

                    var dataJSON = JSON.parse(data);
                    if (dataJSON.success) {
                        $('.kaiduZhi').attr('disabled', true);
                        tipMsg(dataJSON.msg);
                        This.timer = setInterval(function () {
                            if (time == -1) {
                                clearInterval(This.timer);
                                //$TiaoFengKaiSet.animate(val);
                                This.html('停止');
                                timerOnoff = true;
                                $('.kaiduZhi').attr('disabled', false).val('');
                                return;
                            }
                            This.html('(' + time + ')');
                            time--;
                        }, 1000);

                    } else {
                        timerOnoff = true;
                        tipMsg(dataJSON.msg);
                    }
                },
                error: function (data) {

                }
            });

        });
    });



    
    
}

//set框 内  数据的处理

function setWinDataUse() {
    
}



//根据高度 添加项目

function insertItemJudgeHeight() {
    var $hRight, $hLeft;
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
    if (!time) {
        return;
    }
    var Time = new Date(Number(time));
    return Time.getFullYear() + "-" + (Time.getMonth() + 1) + "-" + Time.getDate() + " " + Time.getHours() + ":" + Time.getMinutes() + ":" + Time.getSeconds();
}



//操作提示 函数
function tipMsg(msg) {
    $.gritter.add({

        title: '操作提示',

        text: msg,
        class_name: '  gritter-info gritter-center',
        // image: 'images/screen.png',

        sticky: false,

        time: '1000'
    });
}