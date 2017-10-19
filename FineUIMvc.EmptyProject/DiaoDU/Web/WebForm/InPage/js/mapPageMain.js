var over = [];

var currentJZ;

// 覆盖物点之 流量表
var overLiuLiang = [],overTiaoFeng=[],overErGong=[],overFamen=[],overYaLi=[];


//分开的覆盖物
//var overByCategory = {
//    'line': {},
//    'bengZhan': {},
//    'faMen': {},
//    'liuLiang': {},
//    'shuiChang': {},
//    'shuiYuan': {},
//    'daBiao': {},
//    'yaLi': {},
//    'tiaoFeng': {},
//    'shuiZhi':{}
//};
var overByCategory = [
[],[],[],[],[],[],[],[],[],[]
];


var tanChangOnOff = true;
var txtTanChuang = ['实时数据', '机组信息', '数据报表', '预警报警'];
var currentPumpData = [];

var labelColor = {};
var labelUnit = {};
var iconArr = [];
var iconArrError = [];
var iconArrOffline = [];
var map;
var panorama;
var vUrl;
var uId;
var getMarkerReady = false;
var markerTimer1,markerTimer2,markerTimer3,markerTimer4,markerTimer5,markerTimer6,markerTimer7,markerTimer8,markerTimer9;
var dotOutTimer;
var mapType = [BMAP_NORMAL_MAP,BMAP_SATELLITE_MAP,BMAP_PERSPECTIVE_MAP];
var inPolyPoints = [];
var outPolyPoints = [];
var drawingManager;
var tiaofengData = { dataAll: [] };
var yaliData = { dataAll: [] };
var liuLiangData = { dataAll: [] };
var faMenData = {dataAll:[]};

var erGongData = {dataAll:[]};
//var tiaofengDuiZhaoBiao = {
//    'shuiWei': 'F40005',
//    'storeShui': 'F40006',
//    'userShui': 'F40004',
//    'canUseTime': 'F40007',
//    'inPress': 'F40001',
//    'outPress': 'F40002',
//    'famenKai': 'F40009'
//};
var pointTypeArr=['管线','设备','阀门','户表','水厂','大户表','加压站','水源'];
$(function(){

	//var myIcon_qu=new BMap.Icon("img/qu.png",new BMap.Size(30,30));
	//var myIcon_she = new BMap.Icon("img/she.png",new BMap.Size(30,30));
	//var myIcon_fa = new BMap.Icon("img/fa.png",new BMap.Size(30,30));
	//var myIcon_shui= new BMap.Icon("img/dacahgn.png",new BMap.Size(30,30));
	//var myIcon_hu=new BMap.Icon("img/hu.png",new BMap.Size(30,30));
    ////var myIcon_dahu=new BMap.Icon("img/dahu.png",new BMap.Size(30,30));
	//var myIcon_dahu = new BMap.Icon("img/famenL.png", new BMap.Size(30, 30));
	//var myIcon_shuiyuan=new BMap.Icon("img/shuiyuan.png",new BMap.Size(30,30));
    //var myIcon_jiaya = new BMap.Icon("img/jiaya.png", new BMap.Size(30, 30));

    var myIcon_qu = new BMap.Icon("./img/guanxianL.png", new BMap.Size(30, 30));
    var myIcon_BengZhan = new BMap.Icon("./img/bz.png", new BMap.Size(30, 30));
    var myIcon_FaMen = new BMap.Icon("./img/fm.png", new BMap.Size(30, 30));
    var myIcon_LiuLiang = new BMap.Icon("./img/ll.png", new BMap.Size(30, 30));
    var myIcon_ShuiChang = new BMap.Icon("./img/sc.png", new BMap.Size(30, 30));
    var myIcon_ShuiYuan = new BMap.Icon("./img/sy.png", new BMap.Size(30, 30));
    var myIcon_DaBiao = new BMap.Icon("./img/db.png", new BMap.Size(30, 30));
    var myIcon_YaLi = new BMap.Icon("./img/yl.png", new BMap.Size(30, 30));
    var myIcon_TiaoFeng = new BMap.Icon("./img/tf.png", new BMap.Size(30, 30));
    var myIcon_ShuiZhi = new BMap.Icon("./img/sz.png", new BMap.Size(30, 30));

    var myIcon_qu_error = new BMap.Icon("./img/guanxianL.png", new BMap.Size(30, 30));
    var myIcon_BengZhan_error = new BMap.Icon("./img/bz_error.png", new BMap.Size(30, 30));
    var myIcon_FaMen_error = new BMap.Icon("./img/fm_error.png", new BMap.Size(30, 30));
    var myIcon_LiuLiang_error = new BMap.Icon("./img/ll_error.png", new BMap.Size(30, 30));
    var myIcon_ShuiChang_error = new BMap.Icon("./img/sc_error.png", new BMap.Size(30, 30));
    var myIcon_ShuiYuan_error = new BMap.Icon("./img/sy_error.png", new BMap.Size(30, 30));
    var myIcon_DaBiao_error = new BMap.Icon("./img/db_error.png", new BMap.Size(30, 30));
    var myIcon_YaLi_error = new BMap.Icon("./img/yl_error.png", new BMap.Size(30, 30));
    var myIcon_TiaoFeng_error = new BMap.Icon("./img/tf_error.png", new BMap.Size(30, 30));
    var myIcon_ShuiZhi_error = new BMap.Icon("./img/sz_error.png", new BMap.Size(30, 30));


    var myIcon_qu_offline = new BMap.Icon("./img/guanxianL.png", new BMap.Size(30, 30));
    var myIcon_BengZhan_offline = new BMap.Icon("./img/bz_off.png", new BMap.Size(30, 30));
    var myIcon_FaMen_offline = new BMap.Icon("./img/fm_off.png", new BMap.Size(30, 30));
    var myIcon_LiuLiang_offline = new BMap.Icon("./img/ll_off.png", new BMap.Size(30, 30));
    var myIcon_ShuiChang_offline = new BMap.Icon("./img/sc_off.png", new BMap.Size(30, 30));
    var myIcon_ShuiYuan_offline = new BMap.Icon("./img/sy_off.png", new BMap.Size(30, 30));
    var myIcon_DaBiao_offline = new BMap.Icon("./img/db_off.png", new BMap.Size(30, 30));
    var myIcon_YaLi_offline = new BMap.Icon("./img/yl_off.png", new BMap.Size(30, 30));
    var myIcon_TiaoFeng_offline = new BMap.Icon("./img/tf_off.png", new BMap.Size(30, 30));
    var myIcon_ShuiZhi_offline = new BMap.Icon("./img/sz_off.png", new BMap.Size(30, 30));

	var a=3;

    //iconArr.push(myIcon_qu,myIcon_she,myIcon_fa,myIcon_hu,myIcon_shui,myIcon_dahu,myIcon_jiaya,myIcon_shuiyuan,a);

	iconArr.push(
        myIcon_qu,
        myIcon_BengZhan,
        myIcon_FaMen,
        myIcon_LiuLiang,
        myIcon_ShuiChang,
        myIcon_ShuiYuan,
        myIcon_DaBiao,
        myIcon_YaLi,
        myIcon_TiaoFeng,
        myIcon_ShuiZhi
   );

	iconArrError.push(
        myIcon_qu_error,
        myIcon_BengZhan_error,
        myIcon_FaMen_error,
        myIcon_LiuLiang_error,
        myIcon_ShuiChang_error,
        myIcon_ShuiYuan_error,
        myIcon_DaBiao_error,
        myIcon_YaLi_error,
        myIcon_TiaoFeng_error,
        myIcon_ShuiZhi_error
   );

	iconArrOffline.push(
        myIcon_qu_offline,
        myIcon_BengZhan_offline,
        myIcon_FaMen_offline,
        myIcon_LiuLiang_offline,
        myIcon_ShuiChang_offline,
        myIcon_ShuiYuan_offline,
        myIcon_DaBiao_offline,
        myIcon_YaLi_offline,
        myIcon_TiaoFeng_offline,
        myIcon_ShuiZhi_offline
   );


    //label  颜色

	labelColor = {
	    '1': {
	        color: "#fff",                   //颜色
	        fontSize: "12px",               //字号
	        border: "1px solid #70d7f5",                    //边
	        borderRadius: "2px",
	        //height: "14px",
	        minHeight: "14px",
	        padding: "0 2px",
	        lineHeight: "14px",
	        cursor: "pointer",
	        boxShadow: "0 0 0 2px #00b7ee",
	        backgroundColor: "#00b7ee"    //蓝色
	    },
	    '2': {
	        color: "#fff",                   //颜色
	        fontSize: "12px",               //字号
	        border: "1px solid #ffa970",                    //边
	        borderRadius: "2px",
	        // height: "14px",
	        minHeight: "14px",
	        padding: "0 2px",
	        lineHeight: "14px",
	        cursor: "pointer",
	        boxShadow: "0 0 0 2px #fc7f3b",//橙色
	        backgroundColor: "#fc7f3b"
	    },
	    '3': {
	        color: "#fff",                   //颜色
	        fontSize: "12px",               //字号
	        border: "1px solid #70d7f5",                    //边
	        borderRadius: "2px",
	        // height: "14px",
	        minHeight: "14px",
	        padding: "0 2px",
	        lineHeight: "14px",
	        cursor: "pointer",
	        boxShadow: "0 0 0 2px #00b7ee",
	        backgroundColor: "#00b7ee"    //蓝色
	    },
	    '4': {
	        color: "#fff",                   //颜色
	        fontSize: "12px",               //字号
	        border: "1px solid #85db81",                    //边
	        borderRadius: "2px",
	        // height: "14px",
	        minHeight: "14px",
	        padding: "0 2px",
	        lineHeight: "14px",
	        cursor: "pointer",
	        boxShadow: "0 0 0 2px #26bf1f",
	        backgroundColor: "#26bf1f"     //绿色
	    },
	    '5': {
	        color: "#fff",                   //颜色
	        fontSize: "12px",               //字号
	        border: "1px solid #70d7f5",                    //边
	        borderRadius: "2px",
	        // height: "14px",
	        minHeight: "14px",
	        padding: "0 2px",
	        lineHeight: "14px",
	        cursor: "pointer",
	        boxShadow: "0 0 0 2px #00b7ee",
	        backgroundColor: "#00b7ee"    //蓝色
	    },
	    '6': {
	        color: "#fff",                   //颜色
	        fontSize: "12px",               //字号
	        border: "1px solid #85db81",                    //边
	        borderRadius: "2px",
	        // height: "14px",
	        minHeight: "14px",
	        padding: "0 2px",
	        lineHeight: "14px",
	        cursor: "pointer",
	        boxShadow: "0 0 0 2px #26bf1f",
	        backgroundColor: "#26bf1f"     //绿色
	    },
	    '7': {
	        color: "#fff",                   //颜色
	        fontSize: "12px",               //字号
	        border: "1px solid #70d7f5",                    //边
	        borderRadius: "2px",
	        // height: "14px",
	        minHeight: "14px",
	        padding: "0 2px",
	        lineHeight: "14px",
	        cursor: "pointer",
	        boxShadow: "0 0 0 2px #00b7ee",
	        backgroundColor: "#00b7ee"    //蓝色
	    },
	    '8': {
	        color: "#fff",                   //颜色
	        fontSize: "12px",               //字号
	        border: "1px solid #e09ff2",                    //边
	        borderRadius: "2px",
	        // height: "14px",
	        minHeight: "14px",
	        padding: "0 2px",
	        lineHeight: "14px",
	        cursor: "pointer",
	        boxShadow: "0 0 0 2px #c854e7",  //紫色
	        backgroundColor: "#c854e7"
	    },
	    '9': {
	        color: "#fff",                   //颜色
	        fontSize: "12px",               //字号
	        border: "1px solid #70d7f5",                    //边
	        borderRadius: "2px",
	        // height: "14px",
	        minHeight: "14px",
	        padding: "0 2px",
	        lineHeight: "14px",
	        cursor: "pointer",
	        boxShadow: "0 0 0 2px #00b7ee",
	        backgroundColor: "#00b7ee"    //蓝色
	    },
	    'offline': {
	        color: "#fff",                   //颜色
	        fontSize: "12px",               //字号
	        border: "1px solid #bcbcbc",                    //边
	        borderRadius: "2px",
	        // height: "14px",
	        minHeight: "14px",
	        padding: "0 2px",
	        lineHeight: "14px",
	        cursor: "pointer",
	        boxShadow: "0 0 0 2px #878787",
	        backgroundColor: "#878787"    //灰色
	    },
	    'error': {
	        color: "#fff",                   //颜色
	        fontSize: "12px",               //字号
	        border: "1px solid #f68b8b",                    //边
	        borderRadius: "2px",
	        //  height: "14px",
            minHeight:"14px",
	        padding: "0 2px",
	        lineHeight: "14px",
	        cursor: "pointer",
	        boxShadow: "0 0 0 2px #ef3030",
	        backgroundColor: "#ef3030"    //红色
	    },
	    'default': {
	        color: "#fff",                   //颜色
	        fontSize: "12px",               //字号
	        border: "1px solid #ffa970",                    //边
	        borderRadius: "2px",
	        // height: "14px",
	        minHeight: "14px",
	        padding: "0 2px",
	        lineHeight: "14px",
	        cursor: "pointer",
	        boxShadow: "0 0 0 2px #fc7f3b",//橙色
	        backgroundColor: "#fc7f3b"
	    }
	};

	 labelUnit = {
	    'markerTiaoFeng': {
	        "shuiXiangYeWei":{
	            "name":"水箱液位:",
                "Unit":" m"
	        },
	        "chuShuiZongLiang":{
	            "name":"储水总量:",
	            "Unit":" m³"
	        },
	        "chuShuiLiuLiang":{
	            "name":"出水流量:",
	            "Unit": " m3/h"
	        },
	        "tiaoFengNengLi" :{
	            "name":"调峰能力:",
	            "Unit": " min"
	        },
	        "inPress":{
	            "name":"进水压力:",
	            "Unit":" Mpa"
	        },
	        "outPress":{
	            "name":"出水压力:",
	            "Unit":" Mpa"
	        },
	        "liuFaYuanKai": {
	            "name": "开度:",
	            "Unit": " %"
	        }
	    },
	    'markerYaLi': {
	        "shangXian": {
	            "name": "上限:",
	            "Unit": " Mpa"
	        },
	        "xiaXian": {
	            "name": "下限:",
	            "Unit": " Mpa"
	        },
	        "shiShi": {
	            "name": "实时:",
	            "Unit": " Mpa"
	        },
	        "dianLiang": {
	            "name": "电量:",
	            "Unit": " Kwh"
	        }
	    },
	    'markerLiuliang': {
	        "liuLiang": {
	            "name": "流量:",
	            "Unit": " m³"
	        },
	        "yaLi": {
	            "name": "压力:",
	            "Unit": " Mpa"
	        }
	    },
	    'markerFaMen': {
	        "leiJiLiuLiang": {
	            "name": "累积流量:",
	            "Unit": " m³"
	        },
	        "inPress": {
	            "name": "进水压力:",
	            "Unit": " Mpa"
	        },
	        "outPress": {
	            "name": "出水压力:",
	            "Unit": " Mpa"
	        },
             "faMenKaiDu": {
	             "name": "阀门开度:",
                 "Unit": " %"
	         }
	    }
	};

	vUrl = parseUrl();

	if (!vUrl) {
	    uId = "30b9a1a6-0d4c-43db-b602-61426534480a";
	} else {
	    uId = vUrl['id'];
	}
	//alert(uId);
	getMapTemp();
    //mapInit(0,0,0,0,0,0);


    //叉号
	timesClose();


	tanCengScroll(); tanCengClose(); tanCengRightNavClickFn();


	//alarmGetFn2();

    //图表上的  radius  函数
	radialBar();

	//quanjing
	panoramaShow();

	panoramaShowYali();
});


//获取 URL 参数

function parseUrl() {

    var url = window.location.href;
      //alert(url);
    var i = url.indexOf('?');
    //alert(1);
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

 

//获取模板初始化地图
function getMapTemp() {


	$.ajax({
	    url: '../../Service/Map_Template.ashx?method=SearchTemp',
	    cache: false,
		data:{ "TempID": uId},
		success: function(dat){
			var tempData=JSON.parse(dat);
			//console.log(tempData);
			tempData=tempData[0];
			changMapType(tempData.FMapType);
			
			var type = changMapType(tempData.FMapType);
            var zoom   = tempData.FZoom;
            var min    = tempData.FMinZoom;
            var max    = tempData.FMaxZoom;
            var mapSty = tempData.FStyle;
            var center = tempData.FCenter;
           
            mapInit(type, zoom, max, min, mapSty, center);
            getLine();
            getMarker();
            getArea();
          
            clickToCenter(center);

            //分区check 函数
            fenCheck();
          //红旗 点击
            fenFlogClick();

		    //红旗全选
            checkAll();

            $('body').dblclick(function () {
                //clearInterval(markerTimer);
                markerTimer8 = setInterval(function () {
                    //alert($('.markerTiaoFeng .select_text').attr('data-name'));
                    for (var i = 0; i < over.length; i++) {
                        if (over[i].Type == 'marker') {
                            //if (over[i].FType == '8') {
                            //    over[i].getLabel().setContent();
                            //} else {
                            //    over[i].getLabel().setContent(1232132 + Math.random());
                            //}
                           
                        }
                      
                    }
                     
                 }, 2000);

            });
            $('body').delegate('.BMapLib_Drawing .BMapLib_box', 'click', function () {

                if ($(this).hasClass('active')) {
                    return;
                } else {
                    //		alert($(this).index());
                    $(this).addClass('active').siblings().removeClass('active');
                }
            });

            //testMarker();
           // console.log(markers);

		    //right tool show
            $('.midToos>div').click(function () {

                var ind = $(this).index();
                righttoolsContentShow(ind);

            });

		    // map Type set
            $('.mapTypeSet li').click(function () {
                var ind = $(this).index();
                if ($(this).hasClass('active')) {
                    return;
                }
                $(this).addClass('active').siblings().removeClass('active');
                map.setMapType(mapType[ind]);
                map.setCurrentCity("上海");
            });

            selectFn($('.selectBoxR'));
            selectFnLeft($('.selectBox1'));
            lineAndMaker($('.lineAndMaker>li'));



		    //draw  tool 
            $('body').delegate('.drawTools li', 'click', function () {
                if ($(this).hasClass('active')) {
                    return;
                } else {

                    $(this).addClass('active').siblings().removeClass('active');

                }
            });

		    //反面  小图标的点击事件

		    /*    $('body').delegate('.show_list ul li.showList_li1', 'click', function () {
                    var id=$(this).parent().parent().attr('title');              
                        top.layer.open({
                            type: 2,
                            anim: 3,
                            shade: .6,
                            title: false,
                            shadeClose: true, //点击遮罩关闭层
                            area: ['70%', '60%'],
                            content: '../Baidu/chartsFrame.html?id=' + id + '&bcc=333'
                        });             
                });*/

            $('body').delegate('.show_list ul li', 'click', function (i, v) {
                //var id = $(this).parent().parent().attr('title');
                var index = $(this).index();
               
                top.layer.open({
                    type: 2,
                    anim: 3,
                    shade: .6,
                    title: false,
                    shadeClose: true, //点击遮罩关闭层
                    area: ['70%', '60%'],
                    content: 'chartsIframe.html?id=' + 2 + '&index=' + index
                });
            });

		    //  draw  tool  hover fn
            drawToolsHover();


		    // 水站pop的操作  函数

            shuiZhanPop();


		    //滚动条

            panoramBoxScroll();

		    //报警
            warnShow();
            warnClose();
            warnScroll();

		    //选择选框 的  关闭事件
            panoramBoxClose();


		    //全景 点击 函数
            panoramClick();


		    //定位 类型的 
            dingweiListClick();
		   


		    //页面  resize
            windowResize();

            $(window).resize(function () {
               
                windowResize();
            });
		}
	});
}



//页面 resizerese
function windowResize() {
    $H = $(window).height();
    $W = $(window).width();
   // $('.timeLineBg').height(($H - 44 - 34));
    $('.warnBox').height($H - 44);
    $('.warnContentBox').height(($H - 44 - 34));
    var boxH = $('.warnContentBox').height();
    var contentH = $('.warnContent').height();
    if (boxH > contentH) {
        $('.timeLineBg').height(boxH);
    } else {
        $('.timeLineBg').height(contentH);
    }

}

//获取线
function getLine() {
    $.ajax({
        url: '../../Service/Map_Line.ashx?method=SearchLine',
        cache: false,
        data: { "TempID": uId },
        success: function (dat) {
            var data = $(JSON.parse(dat));
            console.log(data);
            console.log('line');
            data.each(function (i, v) {
                var pointArrAll = [];
                var pointArr = JSON.parse(v.FLine);
                $(pointArr).each(function (ind,va) {
                   
                    var point = new BMap.Point(va.lng, va.lat);
                    pointArrAll.push(point);
                });
                var polyLineNew = new BMap.Polyline(pointArrAll, {
                    strokeWeight: v.FStrokeWeight,
                    strokeColor: v.FStrokeColor,
                    strokeOpacity: Number(v.FStrokeOpacity)/100,
                    strokeStyle: v.FStrokeStyle
                });
                polyLineNew.Type = 'polyline';
                polyLineNew.mark = v.LineID;
                polyLineNew.name = v.FName;
                polyLineNew.TB_AreaOverlay = v.TB_AreaOverlay;
                polyLineNew.FType = 0;
                over.push(polyLineNew);
                
                overByCategory[0].push(polyLineNew);

               // console.log(over);
               // polyLineNew.addEventListener('click',polonClick);
                map.addOverlay(polyLineNew);
            });
           

        }
    });
}

//获取点
function getMarker() {
   
    $.ajax({
        url: '../../Service/Map_Marker.ashx?method=searchmarker',
        cache: false,
        data: { "TempID": uId },
        success: function (dat) {
            var data = $(JSON.parse(dat));
            console.log(data);
            console.log('marker');
            data.each(function (i, v) {
                var va = [];
                    
                if (!v || !(v.FMarker) || v.FMarker.length < 20) {
                    va = [{ "lng": 118.74516, "lat": 31.584326 }];
                   // alert(1);
                } else{
                    //alert(v.FMarker);                                        
                        va = JSON.parse(v.FMarker);
                       
                }
                 
                if(!va||va.length<1){
                    va = [{ "lng": 118.74516, "lat": 31.584326 }];
                }
                var point = new BMap.Point(va[0].lng, va[0].lat);
       
                var markerNew = new BMap.Marker(point);
                over.push(markerNew);

               

                //  markerNew.setIcon(iconArr[v.FType]);
                if (v.FType == 1) {
                    markerNew.setIcon(iconArr[v.FType]);
                } else {
                    markerNew.setIcon(iconArrOffline[v.FType]);
                }
               
               // console.log(v.FType);
                var $size;
                //if (v.FType == '4') {
                //    $size = new BMap.Size(23, -10);
                //} else {
                //    $size = new BMap.Size(16,-10);
                //};
               
                var $size = new BMap.Size(24, 0);
                 markerNew.Type = 'marker';
                 markerNew.mark = v.MarkerID;
                 markerNew.name = v.FName;
                 markerNew.TB_AreaOverlay = v.TB_AreaOverlay;
                 markerNew.FType = v.FType;
                 markerNew.FMID = v.FMID;
                 markerNew.FPoint = point;
                 overByCategory[Number(v.FType)].push(markerNew);
                 if (Number(v.FType) == 3) {
                     overLiuLiang.push(markerNew);
                 } else if (Number(v.FType) == 8) {
                     overTiaoFeng.push(markerNew);
                 } else if (Number(v.FType) == 1) {
                    overErGong.push(markerNew);
                 } else if (Number(v.FType) == 2) {
                     overFamen.push(markerNew);
                 } else if (Number(v.FType) == 7) {
                     overYaLi.push(markerNew);
                 }
                 
                 //console.log(overByCategory);
                //console.log(4543543534);
                // console.log(overLiuLiang);
                //marker  点击 事件
                 markerNew.addEventListener('click', function () {
                   
                    // alert(this.FType);

                     switch (this.FType) {
                         case '1': {
                             // bengZhanInfoUse(this.FMID);
                             mapClickOutFn(this.FMID);
                             break;
                         }
                         case '2': {
                             dianDOngFaInfoUse(this.FMID);
                             //infonaBox(this);
                             break;
                         }
                         case '3': {
                            // showContent2(3, this.mark);

                             liuLiangInfoUse(this.FMID);
                             break;
                         }
                         case '4': {
                             //infonaBox(this);
                             break;
                         }
                         case '5': {
                             //infonaBox(this);
                             break;
                         }
                         case '6': {
                             showContent2(3, this.mark);
                             break;
                         }
                         case '7': {
                             
                             yaliInfoUse(this.FMID);
                            // showYaliVContent(3,this.mark);
                             break;
                         }
                         case '8': {
                            
                             tiaoFengInfoUse(this.FMID);
                             //showContent(3, this.mark);
                             break;
                         }
                         default :{
                            // showContent(3, this.marker);
                             break;
                         }
                     }
                     //if (this.FType == '2') {
                     //    infonaBox(this);
                     //} else {
                     //    showContent(3, this.marker);
                        
                     //}
                    
                 });

                 var label;
                 setLabel();
                 function setLabel() {

                     label = new BMap.Label( Math.random().toFixed(2), {
                         offset: $size,
                         position: point
                     }
                );

                     //switch (v.FType) {
                     //    case '1': {
                     //        label.setStyle(labelColor['1']);
                     //        break;
                     //    }
                     //    case '2': {
                     //        label.setStyle(labelColor['2']);
                     //        break;
                     //    }
                     //    case '3': {
                     //        label.setStyle(labelColor['3']);
                     //        break;
                     //    }
                     //    case '4': {
                     //        label.setStyle(labelColor['4']);
                     //        break;
                     //    }
                     //    case '5': {
                     //        label.setStyle(labelColor['5']);
                     //        break;
                     //    }
                     //    case '6': {
                     //        label.setStyle(labelColor['6']);
                     //        break;
                     //    }
                     //    case '7': {
                     //        label.setStyle(labelColor['offline']);
                     //        label.hide();
                     //        break;
                     //    }
                     //    case '8': {
                     //        label.setStyle(labelColor['8']);
                     //        break;
                     //    }
                     //    case '9': {
                     //        label.setStyle(labelColor['9']);
                     //        break;
                     //    }
                     //    case 'offline': {
                     //        label.setStyle(labelColor['offline']);
                     //        break;
                     //    }
                     //    case 'error': {
                     //        label.setStyle(labelColor['error']);
                     //        break;
                     //    }
                     //    default: {
                     //        label.setStyle(labelColor['default']);
                     //        break;
                     //    }
                     //}

                     if (v.FType == 1) {
                         label.setStyle(labelColor['1']);
                     } else {
                         //默认离线
                         label.setStyle(labelColor['offline']);
                     }

                     
                     //label.setStyle({
                     //    color: "#fff",                   //颜色
                     //    fontSize: "12px",               //字号
                     //    border: "1px solid #ff9b9b",                    //边
                     //    borderRadius: "2px",
                     //    height: "14px",
                     //    padding: "0 2px",
                     //    lineHeight: "14px",
                     //    cursor: "pointer",
                     //    boxShadow: "0 0 0 2px #ff4c4c",
                     //    backgroundColor: "#ff4c4c"     //红色
                     //});
           
                     markerNew.setLabel(label);

                 }
                 
                 
                // markerNew.addEventListener('click', pointClick);
                 map.addOverlay(markerNew);
                 //if (v.FType != '8') {
                 //    markerTimer=setInterval(function () {
                 //        label.setContent(Math.random().toFixed(2));
                 //    }, 2000);

                 //}
                
                // markerNew.show();
            });
        
            tiaofengDataGet();

            yaliDataGet();
           
            liuLiangDataGet();


            dianDongFaDataGet();

            erGongBengZhanDataGet();

             //alarmGetFn();
            alarmGetFn2();
            clearInterval(dotOutTimer);
        
            dotOutTimer = setInterval(function () {
               
                tiaofengDataGet();
               
                yaliDataGet();
                liuLiangDataGet();

                dianDongFaDataGet();
                erGongBengZhanDataGet();
            },4000);

         
        }
    });
}



//获取区域



function getArea() {
    $.ajax({
        url: '../../Service/Map_Area.ashx?method=searcharea',
        cache: false,
        data: { "TempID": uId },
        success: function (da) {
            var data = $(JSON.parse(da));
            var $quyuItem = '';
            $('.areaMain').empty();
            data.each(function (i, v) {

                $quyuItem += ' <li  data-AreaID="' + v.AreaID + '" class="areaItem clearfix">' +
                                '<div class="checkMark active"></div>'+
                                '<div class="itemContent clearfix">'+
                                  '  <div class="itemName">' + v.FName+'</div>' +
                                 '   <div  data-AreaID="' + v.AreaID + '"  class="checkFlog"></div>' +
                                '</div>'+
                            '</li>';              
               
                if (v.FAreaType == 'polygon') {

                    var pointArrAll = [];
                    var pointArr = JSON.parse(v.FArea);
                    $(pointArr).each(function (ind, va) {

                        var point = new BMap.Point(va.lng, va.lat);
                        pointArrAll.push(point);
                    });
                    //pointArrAll=

                    //console.log(pointArrAll);
                    var polyGon1 = new BMap.Polygon(pointArrAll, {
                        fillColor: v.FAreaColor,
                        fillOpacity: Number(v.FAreaOpacity) / 100,
                        strokeWeight: v.FStrokeWeight,
                        strokeColor: v.FStrokeColor,
                        strokeOpacity: Number(v.FStrokeOpacity) / 100,
                        strokeStyle: v.FStrokeStyle
                    });
                    polyGon1.Type = 'polygon';
                    polyGon1.mark = v.AreaID;
                    polyGon1.name = v.FName;
                    over.push(polyGon1);
                    console.log(over);
                   // polyGon1.addEventListener('click', areaClick);
                    map.addOverlay(polyGon1);
                    polyGon1.hide();

                } else if (v.FAreaType == 'circle') {
                    var pointArrAll = [];
                    [{ "lng": 121.22311, "lat": 31.178263 }, 2268.216921818377]
                    var pointArr = JSON.parse(v.FArea);

                    var point = new BMap.Point(pointArr[0].lng, pointArr[0].lat);
                    pointArrAll.push(point);
                    var radius = pointArr[1];
                    //pointArrAll=

                    //console.log(pointArrAll);
                    var circle1 = new BMap.Circle(point, Number(radius), {
                        fillColor: v.FAreaColor,
                        fillOpacity: Number(v.FAreaOpacity) / 100,
                        strokeWeight: v.FStrokeWeight,
                        strokeColor: v.FStrokeColor,
                        strokeOpacity: Number(v.FStrokeOpacity) / 100,
                        strokeStyle: v.FStrokeStyle
                    });
                    circle1.Type = 'circle';
                    circle1.mark = v.AreaID;
                    circle1.name = v.FName;
                    over.push(circle1);
                    console.log(over);
                    //circle1.addEventListener('click', areaClick);
                    map.addOverlay(circle1);
                    circle1.hide();
                } else if (v.FAreaType == 'rectangle') {
                    var pointArrAll = [];
                    var pointArr = JSON.parse(v.FArea);
                    $(pointArr).each(function (ind, va) {

                        var point = new BMap.Point(va.lng, va.lat);
                        pointArrAll.push(point);
                    });
                    //pointArrAll=

                    //console.log(pointArrAll);
                    var rectangle1 = new BMap.Polygon(pointArrAll, {
                        fillColor: v.FAreaColor,
                        fillOpacity: Number(v.FAreaOpacity) / 100,
                        strokeWeight: v.FStrokeWeight,
                        strokeColor: v.FStrokeColor,
                        strokeOpacity: Number(v.FStrokeOpacity) / 100,
                        strokeStyle: v.FStrokeStyle
                    });
                    rectangle1.Type = 'rectangle';
                    rectangle1.mark = v.AreaID;
                    rectangle1.name = v.FName;
                    over.push(rectangle1);
                    console.log(over);
                    //rectangle1.addEventListener('click', areaClick);
                    map.addOverlay(rectangle1);
                    rectangle1.hide();
                }



                console.log(v.FAreaType);
                console.log(over);
                console.log(1111111111111123);


            });

            $('.areaMain').append($quyuItem);

            getMarkerReady = true;
            //var mak = new BMap.Marker(new BMap.Point(121.19396, 31.165352));

            //map.addOverlay(mak);


        }
    });

}


//各种数据 获取函数

//调峰
function tiaofengDataGet() {
    $.ajax({
        url: '/V_CDJK/SearchTF_Report',
        cache: false,
        data: {
            'pageIndex': 0,
            'pageSize': 5000
        },
        success: function (data) {
           // alert(data);
            var dataJSON = JSON.parse(data);
            dataJSON = dataJSON.data;
            tiaofengData.dataAll.length = 0;
            $(dataJSON).each(function (index, value) {
                var dataItem = {                   
                    'id':value.id,
                    'type': 8,
                    'baseId': value.BaseID,
                    'FIsAlarm':value.FIsAlarm,
                    'online': value.FOnLine,
                    'customerId': value.FCustomerID,
                    'updateTime': value.TempTime,
                    'FName': value.FName,
                    'categary': {
                        'inPress': value.F40001,
                        'outPress': value.F40002,
                        'xuShuiLiuLiang': value.F40003,
                        'shuiXiangRongJi': value.F40004,
                        'chuShuiLiuLiang': value.F40005,
                        'chuManShiJian': value.F40006,
                        'chuShuiZongLiang': value.F40007,
                        'tiaoFengShuiLiang': value.F40008,
                        'tiaoJieBiLi': value.F40009,
                        'chuShuiBiLi': value.F40010,
                        'tiaoFengNengLi': value.F40011,
                        'zhiLiuShiJian': value.F40012,
                        'shuiXiangYeWei': value.F40013,
                        'riTiaoFengLiang': value.F40014,
                        'tiaoKongZhuangTai': value.F40015,
                        'liuFaYuanKong': value.F40016,
                        'liuFaYuanKai': value.F40017
                    }
                };
                //var dataAll = {};
                //dataAll.push(dataItem);
                tiaofengData.dataAll.push(dataItem);
              
            });
            tiaofengDataUse(tiaofengData);
            clearInterval(markerTimer8);
            markerTimer8 = setInterval(function () {
                //alert($('.markerTiaoFeng .select_text').attr('data-name'));
                //for (var i = 0; i < over.length; i++) {
                tiaofengDataUse(tiaofengData);
               
                // }

            }, 2000);

            //console.log(tiaofengData);
            //console.log(23423423);
        },
        error: function () {
            //alert(1);
        }
    });
}

//压力数据 的获取

function yaliDataGet() {
    $.ajax({
        url: '/V_CDJK/SearchYL_Report',
        cache: false,
        data: {
            'pageIndex': 0,
            'pageSize': 5000
        },
       
        success: function (data) {
            //alert(123);
            //alert(data);
            
            var dataJSON = JSON.parse(data);
           
            //if (dataJSON.length < 1) {
            //    return;
            //}
            yaliData.dataAll.length = 0;
            $.each(dataJSON['data'], function (index, value) {
                var dataItem = {
                    'id': value.id,
                    'type': value.type,
                    'baseId': value.BASEID,
                    'online': value.FOnLine,
                    'customerId': value.FCustomerID,
                    'updateTime': value.FUpdateTime,
                    'categary': {
                        'shangXian': value.FMpaUp,
                        'xiaXian': value.FMpaDown,
                        'shiShi': value.FMpa,
                        'dianLiang': value.FBatt
                    }
                };
                //var dataAll = {};
                //dataAll.push(dataItem);
                yaliData.dataAll.push(dataItem);
            });
           
            yaliDataUse(yaliData);
            clearInterval(markerTimer7);
            markerTimer7 = setInterval(function () {
                //alert($('.markerTiaoFeng .select_text').attr('data-name'));
                //for (var i = 0; i < over.length; i++) {
                yaliDataUse(yaliData);

                // }

            }, 2000);

            //console.log(yaliData);
            //console.log('love or hate');
        },
        error: function () {
           // alert(1);
        }
    });
}


//流量数据 的 获取

function liuLiangDataGet() {

    $.ajax({
        url: '/V_CDJK/SearchLL_Report',
        cache: false,
        data: {
            'pageIndex': 0,
            'pageSize':5000
        },
        success: function (data) {

            
            var dataJSON = JSON.parse(data);
            console.log(dataJSON);
            console.log('琴瑟和鸣444');
            dataJSON = dataJSON.data;
            //if (dataJSON.length < 1) {
            //    return;
            //}
            liuLiangData.dataAll.length = 0;
            $(dataJSON).each(function (index, value) {
                var dataItem = {
                    'id': value.id,
                    'type': 3,
                    'baseId': value.BaseID,
                    'alarm':value.FIsAlarm,
                    'online': value.FOnLine,
                    'FMapAddress': value.FMapAddress,
                    'FName': value.FName,
                    'P01': value.P01,
                    'P02': value.P02,
                    'A01': value.A01,
                    'A02': value.A02,
                    'A03': value.A03,
                    'V': value.V,
                    //'customerId': value.FCustomerID,
                    'updateTime': value.TempTime,
                    'categary': {
                        'liuLiang': value.P01,
                        'yaLi': value.A03,
                        'fuLeiLiu': value.P02,
                        'shunShiLiu': value.A01,
                        'dianChiDianYa':value.A02

                        
                    }
                };
                
                liuLiangData.dataAll.push(dataItem);
                console.log(liuLiangData.dataAll);
                console.log(11111);
            });
           
            console.log(liuLiangData);
            console.log('琴瑟和鸣555');
            liuLiangDateUse(liuLiangData);
            clearInterval(markerTimer3);
            markerTimer3 = setInterval(function () {
                //alert($('.markerTiaoFeng .select_text').attr('data-name'));
                //for (var i = 0; i < over.length; i++) {
                liuLiangDateUse(liuLiangData);
               
                // }

            }, 1000);
            //center = dataJSON[0].PLngLat;
            //map.setCenter(new BMap.Point(center.split(',')[0], center.split(',')[1]));




        },
        error: function (data) {
            console.log('泵房数据获取出错');
        }
    });
}

//阀门 数据的 获取

function dianDongFaDataGet(){
    $.ajax({
        url: '/V_CDJK/SearchFM_Report',
        cache: false,
        data: {
            'pageIndex': 0,
            'pageSize': 5000
        },
        success: function (data) {
            // alert(data);
            var dataJSON = JSON.parse(data);
            dataJSON = dataJSON.data;
            console.log(dataJSON);
            console.log('111222');
            faMenData.dataAll.length = 0;
            $(dataJSON).each(function (index, value) {
                var dataItem = {
                    'id': value.id,
                    'type': 2,
                    'baseId': value.BaseID,
                    'FIsAlarm': value.FIsAlarm,
                    'online': value.FOnLine,
                    //'customerId': value.FCustomerID,
                    'updateTime': value.TempTime,
                    'FName': value.FName,
                    'categary': {
                        'inPress': value.F40001,
                        'outPress': value.F40002,
                        'yaLiSheDing': value.F40003,
                        'faMenKaiDu': value.F40004,
                        'shunShiLiuLiang': value.F40005,
                        'chuManShiJian': value.F40006,
                        'leiJiLiuLiang': value.FTotalLL
                        //'chuShuiZongLiang': value.F40007,
                       // 'tiaoFengShuiLiang': value.F40008,
                       // 'tiaoJieBiLi': value.F40009,
                       // 'chuShuiBiLi': value.F40010,
                        //'tiaoFengNengLi': value.F40011,
                        //'zhiLiuShiJian': value.F40012,
                        //'shuiXiangYeWei': value.F40013//,
                        //'riTiaoFengLiang': value.F40014,
                        //'tiaoKongZhuangTai': value.F40015,
                        //'liuFaYuanKong': value.F40016,
                        //'liuFaYuanKai': value.F40017
                    }
                };
                //var dataAll = {};
                //dataAll.push(dataItem);
                faMenData.dataAll.push(dataItem);

            });
            
           
            dianDongFaDateUse(faMenData);
           
            clearInterval(markerTimer2);
            markerTimer2 = setInterval(function () {
                //alert($('.markerTiaoFeng .select_text').attr('data-name'));
                //for (var i = 0; i < over.length; i++) {
                dianDongFaDateUse(faMenData);

                // }

            }, 2000);

            //console.log(tiaofengData);
            //console.log(23423423);
        },
        error: function () {
            //alert(1);
        }
    });
}

//二供泵站 数据  的获取

function erGongBengZhanDataGet() {
  
    $(overErGong).each(function (ind, val) {
        $.ajax({
            url: '/V_YCJK/Search_Pump_JinChuWat',
            async: true,
            data: {
                'pumpID': val.FMID
            },
            success: function (data) {
                              
                var dataJSON = JSON.parse(data);
                //console.log(dataJSON);
                //console.log(55555555555);

                var pointArr = dataJSON.obj;
               
              

                var inPress = pointArr[0].pumpJZ[0].F41006;

                val.getLabel().setContent(inPress);

                
                             
            },
            error: function () {

            }
        });
    });

    
}

//调峰数据 使用

function tiaofengDataUse(arr) {
    
    for (var i = 0; i < overTiaoFeng.length; i++) {
       
        
        //alert(str2.length);
        //var currentName = $('.markerTiaoFeng .select_text').attr('data-name');
        
        
        var str2 = [];
        var str3 = '';
        

        //$(currentName).each(function (ind, val) {
        //    if (val.length > 0) {
        //        str2.push(val);

        //    }

        //});

        for (var val in labelUnit['markerTiaoFeng']) {
           // console.log(val);

            if ($('.markerTiaoFeng .select_text').hasClass(val)) {
                str2.push(val);

            }
           // console.log('linxin');
        }

        //$(labelUnit['markerTiaoFeng']).each(function (ind, val) {
        //    console.log(ind);
        //    console.log('linxin');
        //});

        if (str2.length == 0) {
            var currentName = $('.markerTiaoFeng .select_text').attr('data-name');
            if (arr.dataAll[i].categary[currentName]) {
                overTiaoFeng[i].getLabel().setContent(arr.dataAll[i].categary[currentName]);

            } else {
                overTiaoFeng[i].getLabel().setContent('无');
            }
        } else {
            $(str2).each(function (ind, val) {
               
                if (arr.dataAll[i].categary[val]) {                   
                    str3 += arr.dataAll[i].categary[val] + labelUnit['markerTiaoFeng'][val]['Unit'] + '<br/>';
                } 
            });
            overTiaoFeng[i].getLabel().setContent(str3);
        }

        //alert(currentName);
               
                //if (arr.dataAll[i].categary[currentName]) {
                //    overTiaoFeng[i].getLabel().setContent(arr.dataAll[i].categary[currentName]);

                //} else {
                //    overTiaoFeng[i].getLabel().setContent('无');
                //}



               
                 if (arr.dataAll[i].isAlarm) {
                     // alert(1);
                     overTiaoFeng[i].setIcon(iconArrError[overTiaoFeng[i].FType]);
                     overTiaoFeng[i].getLabel().setStyle(labelColor['error']);
                 } else {
                     // alert(1);
                     if (arr.dataAll[i].online) {
                         overTiaoFeng[i].setIcon(iconArr[overTiaoFeng[i].FType]);
                         overTiaoFeng[i].getLabel().setStyle(labelColor[overTiaoFeng[i].FType]);
                     } else {
                         //   alert(1);
                         overTiaoFeng[i].setIcon(iconArrOffline[overTiaoFeng[i].FType]);
                         overTiaoFeng[i].getLabel().setStyle(labelColor['offline']);
                     }
                 }
              
          

    }
}


//压力数据 使用

function yaliDataUse(arr) {

    for (var i = 0; i < overYaLi.length; i++) {
      
       var str2 = [];
       var str3 = '';

       // 、、 var currentName = $('.markerYaLi .select_text').attr('data-name');
  
        // 、、 over[i].getLabel().setContent(arr.dataAll[item].categary[currentName]);

        for (var val in labelUnit['markerYaLi']) {
            // console.log(val);

            if ($('.markerYaLi .select_text').hasClass(val)) {
                str2.push(val);

            }
            // console.log('linxin');
        }

        if (str2.length == 0) {
            var currentName = $('.markerYaLi .select_text').attr('data-name');
            if (arr.dataAll[i].categary[currentName]) {
                overYaLi[i].getLabel().setContent(arr.dataAll[i].categary[currentName]);

            } else {
                overYaLi[i].getLabel().setContent('无');
            }
        } else {
            $(str2).each(function (ind, val) {

                if (arr.dataAll[i].categary[val]) {
                    str3 += arr.dataAll[i].categary[val] + labelUnit['markerYaLi'][val]['Unit'] + '<br/>';
                }
            });
            overYaLi[i].getLabel().setContent(str3);
        }



        if (arr.dataAll[i].isAlarm) {
            // alert(1);
            overYaLi[i].setIcon(iconArrError[overYaLi[i].FType]);
            overYaLi[i].getLabel().setStyle(labelColor['error']);
        } else {
            // alert(1);
            if (arr.dataAll[i].online) {
                overYaLi[i].setIcon(iconArr[overYaLi[i].FType]);
                overYaLi[i].getLabel().setStyle(labelColor[overYaLi[i].FType]);
            } else {
                //   alert(1);
                overYaLi[i].setIcon(iconArrOffline[overYaLi[i].FType]);
                overYaLi[i].getLabel().setStyle(labelColor['offline']);
            }
        }


       

    }
}



//流量 数据的 使用

function liuLiangDateUse(arr) {
    
    console.log(arr);
    console.log('shishi qinqin');
    for (var i = 0; i < overLiuLiang.length; i++) {
       // var currentName = $('.markerLiuliang .select_text').attr('data-name');


        var str2 = [];
        var str3 = '';     

        for (var val in labelUnit['markerLiuliang']) {
           // console.log(val);

            if ($('.markerLiuliang .select_text').hasClass(val)) {
                str2.push(val);

            }
           
        }

    
        if (str2.length == 0) {
            var currentName = $('.markerLiuliang .select_text').attr('data-name');
            if (arr.dataAll[i].categary[currentName]) {
                overLiuLiang[i].getLabel().setContent(arr.dataAll[i].categary[currentName]);

            } else {
                overLiuLiang[i].getLabel().setContent('无');
            }
        } else {
            $(str2).each(function (ind, val) {
                if (!arr.dataAll[i]) {
                    return;
                }
                if (arr.dataAll[i].categary[val]) {
                    str3 += arr.dataAll[i].categary[val] + labelUnit['markerLiuliang'][val]['Unit'] + '<br/>';
                }
            });
            overLiuLiang[i].getLabel().setContent(str3);
        }

        //if (arr.dataAll[i].categary[currentName]) {
        //    overLiuLiang[i].getLabel().setContent(arr.dataAll[i].categary[currentName]);

        //} else {
        //    overLiuLiang[i].getLabel().setContent('无');
        //}
       
        if (arr.dataAll[i].isAlarm) {
        
            overLiuLiang[i].setIcon(iconArrError[overLiuLiang[i].FType]);
            overLiuLiang[i].getLabel().setStyle(labelColor['error']);
        } else {
      
            if (arr.dataAll[i].online) {
                overLiuLiang[i].setIcon(iconArr[overLiuLiang[i].FType]);
                overLiuLiang[i].getLabel().setStyle(labelColor[overLiuLiang[i].FType]);
            } else {
             
                overLiuLiang[i].setIcon(iconArrOffline[overLiuLiang[i].FType]);
                overLiuLiang[i].getLabel().setStyle(labelColor['offline']);
            }

        }

        
    }
}



//电动阀 数据的使用

function dianDongFaDateUse(arr) {
   
   
    for (var i = 0; i < overFamen.length; i++) {
      
        //var currentName = $('.markerFaMen .select_text').attr('data-name');
       
        var str2 = [];
        var str3 = '';

        for (var val in labelUnit['markerFaMen']) {
            // console.log(val);

            if ($('.markerFaMen .select_text').hasClass(val)) {
                str2.push(val);

            }

        }


        if (str2.length == 0) {
            var currentName = $('.markerFaMen .select_text').attr('data-name');
            if (arr.dataAll[i].categary[currentName]) {
                overFamen[i].getLabel().setContent(arr.dataAll[i].categary[currentName]);

            } else {
                overFamen[i].getLabel().setContent('无');
            }
        } else {
            $(str2).each(function (ind, val) {

                if (arr.dataAll[i].categary[val]) {
                    str3 += arr.dataAll[i].categary[val] + labelUnit['markerFaMen'][val]['Unit'] + '<br/>';
                }
            });
            overFamen[i].getLabel().setContent(str3);
        }
        //if (arr.dataAll[i].categary[currentName]) {
        //    overFamen[i].getLabel().setContent(arr.dataAll[i].categary[currentName]);
            

        //} else {
        //    overFamen[i].getLabel().setContent('无');
            
        //}

        if (arr.dataAll[i].isAlarm) {
           
            overFamen[i].setIcon(iconArrError[overFamen[i].FType]);
            overFamen[i].getLabel().setStyle(labelColor['error']);
           
        } else {
         
            if (arr.dataAll[i].online) {
                overFamen[i].setIcon(iconArr[overFamen[i].FType]);
                overFamen[i].getLabel().setStyle(labelColor[overFamen[i].FType]);
               
            } else {
              
                overFamen[i].setIcon(iconArrOffline[overFamen[i].FType]);
                overFamen[i].getLabel().setStyle(labelColor['offline']);
               
            }

        }


    }
}

//各种  弹出的信息 处理


//调峰 弹出
function tiaoFengInfoUse(BaseId) {
  
    $.ajax({
        url: '/V_CDJK/SearchTF_Report',
        cache: false,
        data: {
            'ID': BaseId,
            'pageIndex': 0,
            'pageSize': 1
        },
        success: function (data) {

            var dataJSON = JSON.parse(data);
            dataJSON = dataJSON.data;
            var dataItem = {
                'id': dataJSON[0].id,
                'type': 8,
                'baseId': dataJSON[0].BaseID,
                'FIsAlarm': dataJSON[0].FIsAlarm,
                'online': dataJSON[0].FOnLine,
                'customerId': dataJSON[0].FCustomerID,
                'updateTime': dataJSON[0].TempTime,
                'FName': dataJSON[0].FName,
                'FDTUCode': dataJSON[0].FDTUCode,
                'FSchemeID': dataJSON[0].FSchemeID,
              
                'categary': {
                    'inPress': dataJSON[0].F40001,
                    'outPress': dataJSON[0].F40002,
                    'xuShuiLiuLiang': dataJSON[0].F40003,
                    'shuiXiangRongJi': dataJSON[0].F40004,
                    'chuShuiLiuLiang': dataJSON[0].F40005,
                    'chuManShiJian': dataJSON[0].F40006,
                    'chuShuiZongLiang': dataJSON[0].F40007,
                    'tiaoFengShuiLiang': dataJSON[0].F40008,
                    'tiaoJieBiLi': dataJSON[0].F40009,
                    'chuShuiBiLi': dataJSON[0].F40010,
                    'tiaoFengNengLi': dataJSON[0].F40011,
                    'zhiLiuShiJian': dataJSON[0].F40012,
                    'shuiXiangYeWei': dataJSON[0].F40013,
                    'riTiaoFengLiang': dataJSON[0].F40014,
                    'tiaoKongZhuangTai': dataJSON[0].F40015,
                    'liuFaYuanKong': dataJSON[0].F40016,
                    'liuFaYuanKai': dataJSON[0].F40017
                }
            };
            
           
                showContentTiaoFeng(dataItem, dataJSON[0].id);
           // outTimer = setInterval(function () {
                //alert($('.markerTiaoFeng .select_text').attr('data-name'));
                //for (var i = 0; i < over.length; i++) {
                //tiaofengDataUse(tiaofengData);
                // }

            //}, 2000);

           
        },
        error: function () {
       
        }
    });
}





//时间获取得 转换函数

function getNowFormatDate() {
    var date = new Date();
    var seperator1 = "-";
    var seperator2 = ":";
    var month = date.getMonth() + 1;
    var strDate = date.getDate();
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate+" ";
    //+ " " + date.getHours() + seperator2 + date.getMinutes()
    //+ seperator2 + date.getSeconds();
    return currentdate;
}



//压力弹出

function yaliInfoUse(BaseId) {
   
    $.ajax({
        url: '/V_CDJK/SearchYL_Report',
        cache: false,
        data: {
            'ID': BaseId,
            'pageIndex': 0,
            'pageSize': 1
        },
        success: function (data) {          
        
            console.log(data[0]);
           
            var dataJSON = JSON.parse(data).data;
           
            var yaliDotArr = [];
            var dataTime=[];
            var maxDotArr = [];
            var minDotArr = [];
            var dataItem = {
                'FName': dataJSON[0].FName,
                'updateTime': dataJSON[0].TempTime,
                'createDate': dataJSON[0].TempTime ? dataJSON[0].TempTime.replace('T', ' ') : '时间暂无获取',
                'categary': {
                    'shangXian': dataJSON[0].FMpaUp,
                    'xiaXian': dataJSON[0].FMpaDown,
                    'shiShi': dataJSON[0].FMpa,
                    'dianLiang': dataJSON[0].FBatt
                }
            };

            $.each(dataJSON, function (index,value) {
                var yaliDot = Number(value.FMpa);
                var yaliMax = Number(value.FMpaUp);
                var yaliMin = Number(value.FMpaDown);
                var yaliTime = (value.TempTime);
                yaliDotArr.push(yaliDot);
                maxDotArr.push(yaliMax);
                minDotArr.push(yaliMin);
                dataTime.push(yaliTime);
            });
           
           
            yaLiClickOut(dataItem, dataJSON[0].id,dataJSON);
           // showYaliVContent(yaliDotArr,dataTime, maxDotArr, minDotArr, dataItem);

            // outTimer = setInterval(function () {
            //alert($('.markerTiaoFeng .select_text').attr('data-name'));
            //for (var i = 0; i < over.length; i++) {
            //tiaofengDataUse(tiaofengData);
            // }

            //}, 2000);


        },
        error: function () {
     
        }
    });
}




//流量 弹出

function liuLiangInfoUse(BaseId) {

    $.ajax({
        url: '/V_CDJK/SearchLL_Report',
        cache: false,
        data: {
            'ID': BaseId,
            'pageIndex': 0,
            'pageSize':1
        },
        success: function (data) {
          
            var dataJSON = JSON.parse(data);
            dataJSON = dataJSON.data;
            var dataItem = {
                'id': dataJSON[0].id,
                'type': 3,
                'baseId': dataJSON[0].BaseID,
                'alarm': dataJSON[0].FIsAlarm,
                'online': dataJSON[0].FOnLine,
                
                'FMapAddress': dataJSON[0].FMapAddress,
                'FName': dataJSON[0].FName,
                'P01': dataJSON[0].P01,
                'P02': dataJSON[0].P02,
                'A01': dataJSON[0].A01,
                'A02': dataJSON[0].A02,
                'A03': dataJSON[0].A03,
                'V': dataJSON[0].V,
                //'customerId': dataJSON[0].FCustomerID,
                'updateTime': dataJSON[0].TempTime,
                'categary': {
                    'liuLiang': dataJSON[0].P01,
                    'yaLi': dataJSON[0].A03,
                    'fuLeiLiu': dataJSON[0].P02,
                    'shunShiLiu': dataJSON[0].A01,
                    'dianChiDianYa': dataJSON[0].A02


                }
            };
           
           // showContent2(dataItem, dataJSON[0].id);
            liuLiangClickOut(dataItem, dataJSON[0].id,dataJSON);
            //}, 2000);


        },
        error: function () {
           
        }
    });
}


//电动阀 弹出
function dianDOngFaInfoUse(BaseId) {
   

    if (BaseId) {
        var len = 4;
        dianDongFaClickOut(len);

    }

    $.ajax({
        url: '/V_CDJK/SearchFM_Report',
        cache: false,
        data: {
            'ID': BaseId,
            'pageIndex': 0,
            'pageSize': 1
        },
        success: function (data) {
            // alert(data);
            var dataJSON = JSON.parse(data);
            dataJSON = dataJSON.data;
           
                var dataItem = {
                    'id': dataJSON[0].id,
                    'type': 2,
                    'baseId': dataJSON[0].BaseID,
                    'FIsAlarm': dataJSON[0].FIsAlarm,
                    'online': dataJSON[0].FOnLine,
                    //'customerId': dataJSON[0].FCustomerID,
                    'updateTime': dataJSON[0].TempTime,
                    'FName': dataJSON[0].FName,
                    'categary': {
                        'inPress': dataJSON[0].F40001,
                        'outPress': dataJSON[0].F40002,
                        'yaLiSheDing': dataJSON[0].F40003,
                        'faMenKaiDu': dataJSON[0].F40004,
                        'shunShiLiuLiang': dataJSON[0].F40005,
                        'chuManShiJian': dataJSON[0].F40006,
                        'leiJiLiuLiang': dataJSON[0].FTotalLL
                        //'chuShuiZongLiang': dataJSON[0].F40007,
                        // 'tiaoFengShuiLiang': dataJSON[0].F40008,
                        // 'tiaoJieBiLi': dataJSON[0].F40009,
                        // 'chuShuiBiLi': dataJSON[0].F40010,
                        //'tiaoFengNengLi': dataJSON[0].F40011,
                        //'zhiLiuShiJian': dataJSON[0].F40012,
                        //'shuiXiangYeWei': dataJSON[0].F40013//,
                        //'riTiaoFengLiang': dataJSON[0].F40014,
                        //'tiaoKongZhuangTai': dataJSON[0].F40015,
                        //'liuFaYuanKong': dataJSON[0].F40016,
                        //'liuFaYuanKai': dataJSON[0].F40017
                    }
                };
                

                dianDongFaDetailUse(dataItem,dataJSON);
            
        },
        error: function () {
         
        }
    });
}

//二供泵站弹出


function bengZhanInfoUse() {

}



//二供泵站弹出层的  事件

//二供弹出的 点击 函数  
function mapClickOutFn(pumpID) {

    if (!tanChangOnOff) {
        return;
    }
    tanChangOnOff = false;
    // TweenMax.from('.mapClickShow', 2.8, { 'width': '-=100px', 'height': '-=100px', ease: Bounce.easeInOut });
    $('.mapClickShow').show().css('marginLeft', '0');

    // TweenMax.set('.mapClickShow', { 'scale': 0.5 });
    TweenMax.set('.mapClickShow', { 'scale': 0.5 });

    TweenLite.set(".mapClickShow", { y: '-=100', x: '-=100' });
    TweenLite.set(".mapClickShowMid", { transformPerspective: 500, rotationY: 90 });
    TweenLite.set(".mapClickShowRight", { 'x': '-417', autoAlpha: 0, zIndex: -1 });
    var Tw1 = new TimelineLite();

    Tw1.to('.mapClickShow', 0.3, { x: '+=100', y: '+=100', ease: Back.easeInOut, yoyo: true })
    .to('.mapClickShow', 0.4, { 'scale': 1, delay: 0.2, ease: Back.easeOut })
    .to('.mapClickShowMid', 0.2, { rotationY: 0, ease: Power2.easeOut, yoyo: true })
    .to('.mapClickShowRight', 0.4, { 'x': '+=417px', autoAlpha: 1 })

    //tanChangOnOff = true;

    tanChuangDataUse(pumpID);


    //mapClickShow
    //mapClickShowLeft
    //mapClickShowMid
    //mapClickShowRight



}


//二供弹出窗  里边 数据的  处理函数

function tanChuangDataUse(bengfangID) {
    $.ajax({
        url: '/V_YCJK/Search_Pump_JZReportList',
        async: true,
        cache: false,
        data: {
            'pumpID': bengfangID
        },
        success: function (data) {
           
            var dataJSON = JSON.parse(data);
            dataJSON = dataJSON.obj;
            console.log(dataJSON);
            console.log(3232);
            currentPumpData.length = 0;
            currentPumpData = dataJSON[0];

            console.log(currentPumpData);
            console.log(55556666);

            var JZID = currentPumpData.pumpJZ[0].pumpJZID;
            
            currentJZ = JZID;
            selectListFn(currentPumpData, JZID);

            tanCengTimeLineData(bengfangID, JZID);



            //clearInter(tanchuangDateTimer);
            // tanChuangDataGetNew();
            //tanchuangDateTimer = setInterval(function () {

            //}, 1000 * 10);

        },
        error: function () { }

    });
}



//二供弹出 窗 左侧 数据的  处理
//select 
function selectListFn(currentPumpData, JZID) {

    var pumpName = currentPumpData.PCustomPName;
    var pumpID = currentPumpData.pumpID;
    var jzLen = currentPumpData.pumpJZ;
    var pumpAddress = currentPumpData.PAddress;
    $('.pumpJz .pumpJzTxt').html(pumpAddress);
    $('.selectBox1 .select_ul1').empty();

    var PLngLat = currentPumpData.PLngLat;

  
    $('.quanjingItem').attr('pano', PLngLat);

    var listStr = '';
    $(jzLen).each(function (ind, val) {

        if (val.pumpJZID == JZID) {//val.PumpJZName
            listStr += '<li data-pumpID="' + pumpID + '" data-jzID="' + val.pumpJZID + '" data-value="' + (ind + 1) + '" class="cur">' + pumpName + '(' + val.PumpJZArea + ')' + '</li>';
            $('.selectBox1 .select_text1').html(pumpName + '(' + val.PumpJZArea + ')');
            jzDatashow(currentPumpData, ind, pumpID, JZID);
        } else {
            listStr += '<li data-pumpID="' + pumpID + '" data-jzID="' + val.pumpJZID + '" data-value="' + (ind + 1) + '" class="">' + pumpName + '(' + val.PumpJZArea + ')' + '</li>';
        }

    });

    $('.selectBox1 .select_ul1').append(listStr);



}

// 显示 泵房状态  等信息的处理

function jzDatashow(currentPumpData, index, pumpID, jzID) {


    var currentJZDataArr = currentPumpData.pumpJZ[index].D_Data[0];
    var pumpAux = currentPumpData.pumpJZ[index].Auxiliarypumpcount;
    var pumpRun = currentPumpData.pumpJZ[index].RunPumpNum;
    var pumpJZNum = pumpAux + pumpRun;
    //var outNum = currentPumpData.pumpJZ[index].DrainPumpNum;
    var machineType = $.trim(currentPumpData.pumpJZ[index].MachineType);

    var isAlarm = currentPumpData.pumpJZ[index].IsAlarm;
    var status;

    var typeStr;


    //所有叶轮隐藏
    $('.yelunItem').removeClass('active');

    var jzStateCurrent = currentJZDataArr['FOnLine'];
    var pumpArr = [];
    var pumpRunTime = [];
    var pumpDianArr = [];
    pumpArr[0] = currentJZDataArr['F41008'];
    pumpArr[1] = currentJZDataArr['F41009'];
    pumpArr[2] = currentJZDataArr['F41010'];
    pumpArr[3] = currentJZDataArr['F41011'];
    pumpArr[4] = currentJZDataArr['F41012'];
    pumpArr[5] = currentJZDataArr['F41013'];

    pumpDianArr[0] = currentJZDataArr['F41045'];
    pumpDianArr[1] = currentJZDataArr['F41046'];
    pumpDianArr[2] = currentJZDataArr['F41047'];
    pumpDianArr[3] = currentJZDataArr['F41048'];
    pumpDianArr[4] = currentJZDataArr['F41049'];
    pumpDianArr[5] = currentJZDataArr['F41050'];

    pumpRunTime[0] = currentJZDataArr['F41051'];
    pumpRunTime[1] = currentJZDataArr['F41052'];
    pumpRunTime[2] = currentJZDataArr['F41053'];
    pumpRunTime[3] = currentJZDataArr['F41054'];
    pumpRunTime[4] = currentJZDataArr['F41055'];
    pumpRunTime[5] = currentJZDataArr['F41056'];

    var inPress = currentJZDataArr['F41006'];
    var outPress = currentJZDataArr['F41007'];
    var outShunLiu = currentJZDataArr['F41025'];
    var outLeiJiLiu = currentJZDataArr['FTotalOutLL'];
    var totalDl = currentJZDataArr['FTotalDL'];
    var bianPinPinlv = currentJZDataArr['F41014'];


    $('.showLeftMid .inNum').html(inPress);
    $('.showLeftMid .outNum').html(outPress);
    // $('.leiJiLiu .liuLiangNum').html(outLeiJiLiu);
    //$('.shunShiLiu .liuLiangNum').html(outShunLiu);

    $('.showLeftBot .liuLiang .liuDianPinNum').html(outLeiJiLiu);
    $('.showLeftBot .dianLiang .liuDianPinNum').html(totalDl);
    $('.showLeftBot .bianPinQi .liuDianPinNum').html(bianPinPinlv);

    var caiJITime = '';
    if (currentJZDataArr['TempTime']) {
        caiJITime = currentJZDataArr['TempTime'].replace(/[^0-9]/ig, "");
       // alert(1);
    } else {
        caiJITime = '';
    }

    

    $('.pumpTimeTxt').html(changeTime(caiJITime));

    //左侧  机组状态的处理


    switch (isAlarm) {
        case 0: {
            switch (jzStateCurrent) {
                case 0: {
                    status = 'offline';
                    // statusTxt = '离线';
                    break;
                }
                case 1: {
                    status = 'online';
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
            status = 'error';
            //statusTxt = '报警';
            break;
        }
    }


    $('.pumpNameBg').removeClass('offline online error').addClass(status);

    if (machineType == '0') {
        typeStr = 'guan';

    } else if (machineType == '1') {
        typeStr = 'xiang';

    } else if (machineType == '2') {
        typeStr = 'wu';

    } else if (machineType == '1') {
    }



    $('.showMidBgBox .showMidBg').removeClass('guan xiang wu').addClass(typeStr);


    //对叶轮的 处理 

    var yelunState;
    for (var yeN = 0; yeN < pumpJZNum; yeN++) {

        if (pumpArr[yeN] == '0' || pumpArr[yeN] == '1') {
            yelunState = ' ';
        } else if (pumpArr[yeN] == '2') {
            yelunState = 'online';
        } else if (pumpArr[yeN] == '3') {
            yelunState = 'error';
        } else {
            yelunState = ' ';
        }

        $('.yelunItem').eq(yeN).addClass('active').find('.yelunStatus').removeClass('error online').addClass(yelunState);

    }




    //data信息的处理
    rightShiShiData(currentJZDataArr);



    infoDetailFn(pumpID, jzID);



}



//弹出窗 右侧实时数据 
function rightShiShiData(jzDataArr) {

    $.ajax({
        url: '/V_YCJK/ParmSelect_MapView',
        success: function (data) {

            var dataJSON = JSON.parse(data);
            dataJSON = dataJSON.obj;


            var dataListStr = '';
            $(dataJSON).each(function (ind, val) {
                $('.forData').empty();
                var dataTxt;
                // dataTxt = jzDataArr[val.FField] ? jzDataArr[val.FField] : '无';
                dataTxt = jzDataArr[val.FField] || (jzDataArr[val.FField] == 0) ? jzDataArr[val.FField] : ' ';
                if (val.FField == 'FUpdateDate') {
                    if (jzDataArr[val.FField] || jzDataArr[val.FField]==0) {
                        dataTxt = changeTime(dataTxt.replace(/[^0-9]/ig, ""));
                    } else {
                        dataTxt = '';
                    }
                   
                    
                   
                }
                if (val.FField == "InOutWaPa") {
                    if (jzDataArr.F41006||(jzDataArr.F41006)==0) {
                        jzDataArr.F41006 = jzDataArr.F41006;
                    } else {
                        jzDataArr.F41006 = '';
                    }
                    if (jzDataArr.F41007 || (jzDataArr.F41007) == 0) {
                        jzDataArr.F41007 = jzDataArr.F41007;
                    } else {
                        jzDataArr.F41007 = '';
                    }
                    
                    dataTxt = jzDataArr.F41006 + '/' + jzDataArr.F41007;
                }
                if (val.FField == 'F41093') {   //门禁
                    if (Number(dataTxt) == 0) {
                        dataTxt = '关闭';
                    } else if (Number(dataTxt) == 1) {
                        dataTxt = '打开';
                    } else {
                        dataTxt = ' ';
                    }
                }
                if (val.FField == 'F41094') {
                    if (Number(dataTxt) == 0) {
                        dataTxt = '正常';
                    } else if (Number(dataTxt) == 1) {
                        dataTxt = '漏水';
                    } else {
                        dataTxt = ' ';
                    }
                }
                if (val.FField == 'F41095') {
                    if (Number(dataTxt) == 0) {
                        dataTxt = '正常';
                    } else if (Number(dataTxt) == 1) {
                        dataTxt = '有破窗';
                    } else {
                        dataTxt = ' ';
                    }
                }
                if (val.FField == 'F41096') {
                    if (Number(dataTxt) == 0) {
                        dataTxt = '正常';
                    } else if (Number(dataTxt) == 1) {
                        dataTxt = '发生火灾';
                    } else {
                        dataTxt = ' ';
                    }
                }
                if (val.FField == 'F41097') {
                    if (Number(dataTxt) == 0) {
                        dataTxt = '灯关';
                    } else if (Number(dataTxt) == 1) {
                        dataTxt = '灯开';
                    } else {
                        dataTxt = ' ';
                    }
                }
                if (val.FField == 'F41098') {
                    if (Number(dataTxt) == 0) {
                        dataTxt = '正常';
                    } else if (Number(dataTxt) == 1) {
                        dataTxt = '设备断电';
                    } else {
                        dataTxt = ' ';
                    }
                }
                if (val.FField == 'F41099') {
                    if (Number(dataTxt) == 0) {
                        dataTxt = '正常';
                    } else if (Number(dataTxt) == 1) {
                        dataTxt = '异常';
                    } else {
                        dataTxt = ' ';
                    }
                }
                dataListStr += '<li class="listItem clearfix">' +
                   '     <div class="listItemName">' + val.FName + '：</div>' +
                   '     <div class="listItemTxt" data-FField="' + val.FField + '">' + dataTxt + '</div>' +
                   ' </li>';
            });
            $('.forData').append(dataListStr);



        }
    });
}


//机组详细信息函数
function infoDetailFn(pumpID, pumpJZID) {

    $.ajax({
        url: '/V_YCJK/Search_PumpJZDetail',
        data: {
            'pumpID': pumpID,
            'pumpJZID': pumpJZID
        },
        success: function (data) {

            var dataJSON = JSON.parse(data);
            dataJSON = dataJSON.obj;
            dealData(dataJSON[0]);





        }
    });
}


//机组详细 信息 处理 函数

function dealData(pumpData) {

    for (var key in pumpData) {
        switch (key) {
            case "InstallDate":
            case "AcceptanceDate":
                if (pumpData[key]) {
                    var date = changeTime(pumpData[key].replace(/[^0-9]/ig, ""));


                    $("div[data-txtval=" + key + "]").html(date);
                }
                break;
            case "TankIsSharing":
                if (pumpData[key] == 1) {
                    $("div[data-txtval=" + key + "]").html('是');
                } else {
                    $("div[data-txtval=" + key + "]").html('否');
                }
                break;
            case "PumpSoaking":
            case "WaterTankSterilizer":
            case "Warning":
            case "WaterQualityDetection":
            case "ControlValve":
                if (pumpData[key] == 1) {
                    $("div[data-txtval=" + key + "]").html('有');
                } else {
                    $("div[data-txtval=" + key + "]").html(' ');
                }
                break;
            default:
                $("div[data-txtval=" + key + "]").html(pumpData[key]);
                $("div[data-txtval=" + key + "]").html(pumpData[key]);
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
                        $("div[data-txtval=" + i + "]").html('有');
                    } else {
                        $("div[data-txtval=" + i + "]").html(' ');
                    }
                    break;
                default:
                    $("div[data-txtval=" + i + "]").html(jzData[i]);
                    break;
            }
        }
    }
    if (pumpData["pumpVQ"]) {

        var str = '';
        console.log(pumpData["pumpVQ"]);
        for (var j = 0; j < pumpData["pumpVQ"].length; j++) {

            str += '<div class="shiPinItem clearfix">                                         ' +
                  '         <div class="shiPinItemName">' + pumpData["pumpVQ"][j].QuipmentType + '</div>                                  ' +
                  '         <div class="shiPinItemMain">                                             ' +
                  '             <div class=" shiPinItemBox shiPinTypeBox clearfix">                  ' +
                  '                 <div class="shiPinItemName shiPinTypeName">类型：</div>           ' +
                  '                 <div class="shiPinItemTxt shiPinTypeTxt">' + pumpData["pumpVQ"][j].Type + '</div>             ' +
                  '             </div>                                                               ' +
                  '             <div class=" shiPinItemBox shiPinPinPaiBox clearfix">                ' +
                  '                 <div class="shiPinItemName shiPinPinPaiName">品牌：</div>         ' +
                  '                 <div class="shiPinItemTxt shiPinPinPaiTxt">' + pumpData["pumpVQ"][j].Brand + '</div>           ' +
                  '             </div>                                                               ' +
                  '             <div class=" shiPinItemBox shiPinXingHaoBox clearfix">               ' +
                  '                 <div class="shiPinItemName shiPinXingHaoName">型号：</div>        ' +
                  '                 <div class="shiPinItemTxt shiPinXingHaoTxt">' + pumpData["pumpVQ"][j].Number + '</div>            ' +
                  '             </div>                                                               ' +
                  '             <div class="shiPinItemBox shiPinDuanKouBox clearfix">                ' +
                  '                 <div class="shiPinItemName shiPinDuanKouName">IP端口号：</div>     ' +
                  '                 <div class="shiPinItemTxt shiPinDuanKouTxt">' + pumpData["pumpVQ"][j].Port + '</div>       ' +
                  '             </div>                                                               ' +
                  '         </div>                                                                   ' +
                  '     </div>';

        }
        $(".shiPinHeader").siblings().remove();
        $(".shiPinHeader").after(str);
    }

}



//  弹窗中  设备 下拉列表

function selectFnLeft(obj) {
    var select = obj;
    select.click(function (event) {
        var ind = $(this).index();
        //		for(var i=0;i<obj.length;i++){
        //			
        //			if(obj.eq(i).index()!='ind'){
        //				
        //				obj.eq(i).find('.select_ul1').slideUp();
        //				
        //			}
        //		}

        $(this).find('.select_ul1').slideToggle().end().parents('li').siblings().find('.select_ul1').slideUp();

        event.stopPropagation();
    });
    $("body").click(function () {
        $('.select_ul1').slideUp();
    });
    $('.select_ul1').delegate("li", "click", function (e) {
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


        currentPump = li.attr('data-pumpID');
        currentJZ = li.attr('data-jzID');


        jzDatashow(currentPumpData, li.index(), currentPump, currentJZ);

        infoDetailFn(currentPump, currentJZ);


       
        getWaterElec(currentJZ);
        get7daysWaterUse(currentJZ);
        get7daysElecUse(currentJZ);
        getInOutWaPress(currentJZ, 10);
        myChart1.resize();
        myChart2.resize();
        myChart3.resize();
    });

}



//弹层的 右侧  nav的函数
function tanCengRightNavClickFn() {
    $('.mapClickShowMid>.showItem').click(function () {
        if ($(this).hasClass('active')) {
            return;
        }

  

        $('.mapClickShowRight .headerBox').html(txtTanChuang[$(this).index()]).removeClass(' dataItem infoItem tubiaoItem baojingItem');

        $(this).addClass('active').siblings().removeClass('active');
        if ($(this).hasClass('dataItem')) {
            $('.dataItem').addClass('active').siblings().removeClass('active');
            $('.mapClickShowRight .headerBox').addClass('dataItem');
        } else if ($(this).hasClass('infoItem')) {
            $('.infoItem').addClass('active').siblings().removeClass('active');
            $('.mapClickShowRight .headerBox').addClass('infoItem');
        } else if ($(this).hasClass('tubiaoItem')) {
            $('.tubiaoItem').addClass('active').siblings().removeClass('active');
            $('.mapClickShowRight .headerBox').addClass('tubiaoItem');

            
            showChart();

            getWaterElec(currentJZ);
            get7daysWaterUse(currentJZ);
            get7daysElecUse(currentJZ);
            getInOutWaPress(currentJZ, 10);
            myChart1.resize();
            myChart2.resize();
            myChart3.resize();
            //setInterval(function () {
            //    getWaterElec(JZID);
            //    getInOutWaPress(JZID, 1);
            //    get7daysWaterUse(JZID);
            //    get7daysElecUse(JZID);
            //}, 5000);

        } else if ($(this).hasClass('baojingItem')) {
            $('.baojingItem').addClass('active').siblings().removeClass('active');
            $('.mapClickShowRight .headerBox').addClass('baojingItem');

            timeLineHeightFn();
        }



    });


}


//弹出层中  的 时间线的 函数

function tanCengTimeLineData(pumpId, jzId) {
    $.ajax({
        url: '/V_YCJK/SearchAlarm',
        data: {
            "pumpID": pumpId,
            "pumpJZID": jzId
        },
        success: function (data) {



            var dataJSON = (JSON.parse(data)).obj;
            if (dataJSON.length > 0) {
                // dealRealAlarm(data.obj);
                tanCengTimeLineDataUse(dataJSON);
                $('.forBaoJing').css('background', 'none');
            } else {

            }

        },
        error: function (data) {
            console.log('错误：' + data.responseText);
        }
    });
}


//弹出层中  的 时间线的 函数   数据使用

function tanCengTimeLineDataUse(timeLineData) {
    $('.forBaoJing').empty();
    var timeLineStr = '';


    $(timeLineData).each(function (ind, val) {
        var timeLineTime;
        timeLineTime = val.timeLineTime ? val.timeLineTime.replace('T', ' ') : '无';
        timeLineStr += '<div class="timeItem clearfix">            ' +
                  '      <div class="leftTimeBox">                  ' +
                  '          <div class="timeLine">                 ' +
                  '              <div class="timeQiu"></div>        ' +
                  '          </div>                                 ' +
                  '      </div>                                     ' +
                  '      <div class="rightContentBox">              ' +
                  '          <div class="warnHeader">' + timeLineTime + '</div>    ' +
                  '          <div class="warnTxt">' + val.FSetMsg + '</div>            ' +
                  '      </div>                                     ' +
                  '  </div>';
    });

    $('.forBaoJing').html(timeLineStr);



}

//弹层 时间线 高度 处理 函数

function timeLineHeightFn() {
    $('.leftTimeBox').height($('.leftTimeBox').next().height() + 20);
    //alert($('.leftTimeBox').next().height());
    $('.timeLine').height($('.timeLine').parent().height());
}


//弹层的  滚动条

function tanCengScroll() {
    $('.rightScrollBox').mCustomScrollbar({
        scrollbarPosition: "inside",
        theme: "minimal-dark"
    });
}
//  弹层的 点击 事件

function tanCengClose() {
    $('body').delegate('.mapClickShowRight .mapCloseBox', 'click', function () {

        $('.mapClickShow').hide();
        $('.mapClickShowMid>.showItem.dataItem').trigger('click');
        tanChangOnOff = true;
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


//画的测试点
//var markerAr = [
//	'121.111705,31.166028',
//	'121.121705,31.166028',
//	'121.191005,31.166028',
//	'121.161705,31.166028',
//	'121.171705,31.166028',
//	'121.181705,31.166028',
//	'121.191705,31.166028'
//];

//var markers = [];
//function testMarker() {
//    for (var i = 0; i < over.length; i++) {
//        if (over[i].Type != 'marker') {
//            return;
//        }
//        var marker = new BMap.Marker(new BMap.Point(over[i].split(',')[0], over[i].split(',')[1]));
//        //markers.push(marker);
//        //map.addOverlay(marker);
//    }
//}



//地图类型转换函数

function changMapType(type) {
    switch (type) {
        case 'BMAP_NORMAL_MAP': {
            type = mapType[0];
            return type;
            break;
        }
        case 'BMAP_SATELLITE_MAP': {
            type = mapType[1];
            return type;
            break;
        }
        case 'BMAP_PERSPECTIVE_MAP': {
            tempData.FMapType = mapType[2];
            return type;
            break;
        }
        default: {
            type = mapType[0];
            return type;
            break;
        }
    }

    
}



//判断点  是否在 区域内
function judgePoint(type, polyg) {
    $('.panoramaShowBox .panoramaShowBox').hide();
    var bengzhanNum = 0, famenNum = 0, liuliangNum = 0, shuichangNum = 0, shuiyuanNum = 0, dabiaoNum = 0, yaliNum = 0,tiaofengNum=0,shuizhiNum=0;
    var bengzhanStr ='', famenStr = '', liuliangStr = '', shuichangStr = '', shuiyuanStr = '', dabiaoStr = '', yaliStr = '',tiaofengStr='',shuizhiStr='';
    if (type == "polygon") {
        for (var i = 0; i < over.length; i++) {
            if (over[i].Type != 'marker') {
                continue;
            }

           

            var trueOfFalse = BMapLib.GeoUtils.isPointInPolygon(new BMap.Point(over[i].point['lng'], over[i].point['lat']), polyg);
           
            var overNum = Number(over[i].FType);
            switch (overNum) {
                case 1:
                    
                    if (trueOfFalse) {
                        bengzhanNum++;
                        bengzhanStr+='<li class="itemContentItem">'+
                                      '<div class="contentLeft"></div>'+
                                        '<div class="contentRight">'+
                                           ' <div class="contentRightTop"><span class="picBox picBox1" data-over="'+i+'"></span><span class="txtBox">'+over[i].name+'</span></div>'+
                                            '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[i].name + '</span></div>' +
                                         '</div>'+
                                        '<div class="rightTopNum">' + bengzhanNum + '</div>' +
                                    '</li>'
                    }

                   
                    break;
                
                case 2:
                   
                    if (trueOfFalse) {
                        famenNum++;
                        famenStr += '<li class="itemContentItem">' +
                                      '<div class="contentLeft"></div>' +
                                        '<div class="contentRight">' +
                                           ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + i + '"></span><span class="txtBox">' + over[i].name + '</span></div>' +
                                            '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[i].name + '</span></div>' +
                                         '</div>' +
                                        '<div class="rightTopNum">'+famenNum+'</div>' +
                                    '</li>'
                    }
                   
                    break;
                
                case 3:
                    
                    if (trueOfFalse) {
                        liuliangNum++;
                        liuliangStr += '<li class="itemContentItem">' +
                                      '<div class="contentLeft"></div>' +
                                        '<div class="contentRight">' +
                                           ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + i + '"></span><span class="txtBox">' + over[i].name + '</span></div>' +
                                            '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[i].name + '</span></div>' +
                                         '</div>' +
                                        '<div class="rightTopNum">'+liuliangNum+'</div>' +
                                    '</li>'
                    }
                   
                    break;
                
                case 4:
                    
                    if (trueOfFalse) {
                        shuichangNum++;
                        shuichangStr += '<li class="itemContentItem">' +
                                      '<div class="contentLeft"></div>' +
                                        '<div class="contentRight">' +
                                           ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + i + '"></span><span class="txtBox">' + over[i].name + '</span></div>' +
                                            '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[i].name + '</span></div>' +
                                         '</div>' +
                                        '<div class="rightTopNum">'+shuichangNum+'</div>' +
                                    '</li>'
                    }
                   
                    break;
                
                case 5:
                   
                    if (trueOfFalse) {
                        shuiyuanNum++;
                        shuiyuanStr += '<li class="itemContentItem">' +
                                      '<div class="contentLeft"></div>' +
                                        '<div class="contentRight">' +
                                           ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + i + '"></span><span class="txtBox">' + over[i].name + '</span></div>' +
                                            '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[i].name + '</span></div>' +
                                         '</div>' +
                                        '<div class="rightTopNum">'+shuiyuanNum+'</div>' +
                                    '</li>'
                    }
                   
                    break;
                
                case 6:
                   

                    if (trueOfFalse) {
                        dabiaoNum++;
                        dabiaoStr += '<li class="itemContentItem">' +
                                      '<div class="contentLeft"></div>' +
                                        '<div class="contentRight">' +
                                           ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + i + '"></span><span class="txtBox">' + over[i].name + '</span></div>' +
                                            '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[i].name + '</span></div>' +
                                         '</div>' +
                                        '<div class="rightTopNum">'+dabiaoNum+'</div>' +
                                    '</li>'
                    }
                   
                    break;
                case 7:
                    
                    if (trueOfFalse) {
                        yaliNum++;
                        yaliStr += '<li class="itemContentItem">' +
                                      '<div class="contentLeft"></div>' +
                                        '<div class="contentRight">' +
                                           ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + i + '"></span><span class="txtBox">' + over[i].name + '</span></div>' +
                                            '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[i].name + '</span></div>' +
                                         '</div>' +
                                        '<div class="rightTopNum">'+yaliNum+'</div>' +
                                    '</li>'
                    }
                   
                    break;
                case 8:
                    
                    if (trueOfFalse) {
                        tiaofengNum++;
                         tiaofengStr+= '<li class="itemContentItem">' +
                                      '<div class="contentLeft"></div>' +
                                        '<div class="contentRight">' +
                                           ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + i + '"></span><span class="txtBox">' + over[i].name + '</span></div>' +
                                            '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[i].name + '</span></div>' +
                                         '</div>' +
                                        '<div class="rightTopNum">'+tiaofengNum+'</div>' +
                                    '</li>'
                    }

                    break;

                case 9:
                    
                    if (trueOfFalse) {
                        shuizhiNum++;
                        shuizhiStr += '<li class="itemContentItem">' +
                                      '<div class="contentLeft"></div>' +
                                        '<div class="contentRight">' +
                                           ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + i + '"></span><span class="txtBox">' + over[i].name + '</span></div>' +
                                            '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[i].name + '</span></div>' +
                                         '</div>' +
                                        '<div class="rightTopNum">'+shuizhiNum+'</div>' +
                                    '</li>'
                    }

                    break;
                

            }

            //if (BMapLib.GeoUtils.isPointInPolygon(new BMap.Point(over[i].point['lng'], over[i].point['lat']), polyg)) {
            //   // alert(over[i].name + "在多边形");
            //    inPolyPoints.push(over[i]);
               
            //    //console.log();
            //} else {
            //  //alert(over[i].name + "不在多边形");
            //    outPolyPoints.push(over[i]);
            //}

            //console.log(polyg);
        }
        //map.removeOverlay(polyg);


       


    } else if (type == "rectangle") {
        for (var j = 0; j < over.length; j++) {
            if (over[j].Type != 'marker') {
                continue;
            }
            //alert(BMapLib.GeoUtils.isPointInRect(new BMap.Point(markers[j].point['lng'],markers[j].point['lat']),polyg.getBounds()));
            var trueOfFalse =BMapLib.GeoUtils.isPointInRect(new BMap.Point(over[j].point['lng'], over[j].point['lat']), polyg.getBounds());
            var overNum = Number(over[j].FType);
            switch (overNum) {
                case 1:

                    if (trueOfFalse) {
                        bengzhanNum++;
                        bengzhanStr += '<li class="itemContentItem">' +
                                      '<div class="contentLeft"></div>' +
                                        '<div class="contentRight">' +
                                           ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + j + '"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                            '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                         '</div>' +
                                        '<div class="rightTopNum">' + bengzhanNum + '</div>' +
                                    '</li>'
                    }


                    break;

                case 2:

                    if (trueOfFalse) {
                        famenNum++;
                        famenStr += '<li class="itemContentItem">' +
                                      '<div class="contentLeft"></div>' +
                                        '<div class="contentRight">' +
                                           ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + j + '"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                            '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                         '</div>' +
                                        '<div class="rightTopNum">' + famenNum + '</div>' +
                                    '</li>'
                    }

                    break;

                case 3:

                    if (trueOfFalse) {
                        liuliangNum++;
                        liuliangStr += '<li class="itemContentItem">' +
                                      '<div class="contentLeft"></div>' +
                                        '<div class="contentRight">' +
                                           ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + j + '"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                            '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                         '</div>' +
                                        '<div class="rightTopNum">' + liuliangNum + '</div>' +
                                    '</li>'
                    }

                    break;

                case 4:

                    if (trueOfFalse) {
                        shuichangNum++;
                        shuichangStr += '<li class="itemContentItem">' +
                                      '<div class="contentLeft"></div>' +
                                        '<div class="contentRight">' +
                                           ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + j + '"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                            '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                         '</div>' +
                                        '<div class="rightTopNum">' + shuichangNum + '</div>' +
                                    '</li>'
                    }

                    break;

                case 5:

                    if (trueOfFalse) {
                        shuiyuanNum++;
                        shuiyuanStr += '<li class="itemContentItem">' +
                                      '<div class="contentLeft"></div>' +
                                        '<div class="contentRight">' +
                                           ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + j + '"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                            '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                         '</div>' +
                                        '<div class="rightTopNum">' + shuiyuanNum + '</div>' +
                                    '</li>'
                    }

                    break;

                case 6:


                    if (trueOfFalse) {
                        dabiaoNum++;
                        dabiaoStr += '<li class="itemContentItem">' +
                                      '<div class="contentLeft"></div>' +
                                        '<div class="contentRight">' +
                                           ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + j + '"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                            '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                         '</div>' +
                                        '<div class="rightTopNum">' + dabiaoNum + '</div>' +
                                    '</li>'
                    }

                    break;
                case 7:

                    if (trueOfFalse) {
                        yaliNum++;
                        yaliStr += '<li class="itemContentItem">' +
                                      '<div class="contentLeft"></div>' +
                                        '<div class="contentRight">' +
                                           ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + j + '"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                            '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                         '</div>' +
                                        '<div class="rightTopNum">' + yaliNum + '</div>' +
                                    '</li>'
                    }

                    break;
                case 8:

                    if (trueOfFalse) {
                        tiaofengNum++;
                        tiaofengStr += '<li class="itemContentItem">' +
                                     '<div class="contentLeft"></div>' +
                                       '<div class="contentRight">' +
                                          ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + j + '"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                           '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                        '</div>' +
                                       '<div class="rightTopNum">' + tiaofengNum + '</div>' +
                                   '</li>'
                    }

                    break;

                case 9:

                    if (trueOfFalse) {
                        shuizhiNum++;
                        shuizhiStr += '<li class="itemContentItem">' +
                                      '<div class="contentLeft"></div>' +
                                        '<div class="contentRight">' +
                                           ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + j + '"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                            '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                         '</div>' +
                                        '<div class="rightTopNum">' + shuizhiNum + '</div>' +
                                    '</li>'
                    }

                    break;


            }

            //if (BMapLib.GeoUtils.isPointInPolygon(new BMap.Point(over[i].point['lng'], over[i].point['lat']), polyg)) {
            //   // alert(over[i].name + "在多边形");
            //    inPolyPoints.push(over[i]);
               
            //    //console.log();
            //} else {
            //  //alert(over[i].name + "不在多边形");
            //    outPolyPoints.push(over[i]);
            //}

            //console.log(polyg);
       

        }
       
    } else if (type == "circle") {
        for (var j = 0; j < over.length; j++) {
            if (over[j].Type != 'marker') {
                continue;
            }
           
            //alert(BMapLib.GeoUtils.isPointInRect(new BMap.Point(markers[j].point['lng'],markers[j].point['lat']),polyg.getBounds()));
            var trueOfFalse= BMapLib.GeoUtils.isPointInCircle(new BMap.Point(over[j].point['lng'], over[j].point['lat']), polyg);
                var overNum = Number(over[j].FType);
                switch (overNum) {
                    case 1:
                    
                        if (trueOfFalse) {
                            bengzhanNum++;
                            bengzhanStr+='<li class="itemContentItem">'+
                                          '<div class="contentLeft"></div>'+
                                            '<div class="contentRight">'+
                                               ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + j + '"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                                '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                             '</div>'+
                                            '<div class="rightTopNum">' + bengzhanNum + '</div>' +
                                        '</li>'
                        }

                   
                        break;
                
                    case 2:
                   
                        if (trueOfFalse) {
                            famenNum++;
                            famenStr += '<li class="itemContentItem">' +
                                          '<div class="contentLeft"></div>' +
                                            '<div class="contentRight">' +
                                               ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + j + '"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                                '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                             '</div>' +
                                            '<div class="rightTopNum">'+famenNum+'</div>' +
                                        '</li>'
                        }
                   
                        break;
                
                    case 3:
                    
                        if (trueOfFalse) {
                            liuliangNum++;
                            liuliangStr += '<li class="itemContentItem">' +
                                          '<div class="contentLeft"></div>' +
                                            '<div class="contentRight">' +
                                               ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + j + '"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                                '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                             '</div>' +
                                            '<div class="rightTopNum">'+liuliangNum+'</div>' +
                                        '</li>'
                        }
                   
                        break;
                
                    case 4:
                    
                        if (trueOfFalse) {
                            shuichangNum++;
                            shuichangStr += '<li class="itemContentItem">' +
                                          '<div class="contentLeft"></div>' +
                                            '<div class="contentRight">' +
                                               ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + j + '"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                                '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                             '</div>' +
                                            '<div class="rightTopNum">'+shuichangNum+'</div>' +
                                        '</li>'
                        }
                   
                        break;
                
                    case 5:
                   
                        if (trueOfFalse) {
                            shuiyuanNum++;
                            shuiyuanStr += '<li class="itemContentItem">' +
                                          '<div class="contentLeft"></div>' +
                                            '<div class="contentRight">' +
                                               ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + j + '"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                                '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                             '</div>' +
                                            '<div class="rightTopNum">'+shuiyuanNum+'</div>' +
                                        '</li>'
                        }
                   
                        break;
                
                    case 6:
                   

                        if (trueOfFalse) {
                            dabiaoNum++;
                            dabiaoStr += '<li class="itemContentItem">' +
                                          '<div class="contentLeft"></div>' +
                                            '<div class="contentRight">' +
                                               ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + j + '"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                                '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                             '</div>' +
                                            '<div class="rightTopNum">'+dabiaoNum+'</div>' +
                                        '</li>'
                        }
                   
                        break;
                    case 7:
                    
                        if (trueOfFalse) {
                            yaliNum++;
                            yaliStr += '<li class="itemContentItem">' +
                                          '<div class="contentLeft"></div>' +
                                            '<div class="contentRight">' +
                                               ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + j + '"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                                '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                             '</div>' +
                                            '<div class="rightTopNum">'+yaliNum+'</div>' +
                                        '</li>'
                        }
                   
                        break;
                    case 8:
                    
                        if (trueOfFalse) {
                            tiaofengNum++;
                            tiaofengStr+= '<li class="itemContentItem">' +
                                         '<div class="contentLeft"></div>' +
                                           '<div class="contentRight">' +
                                              ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + j + '"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                               '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                            '</div>' +
                                           '<div class="rightTopNum">'+tiaofengNum+'</div>' +
                                       '</li>'
                        }

                        break;

                    case 9:
                    
                        if (trueOfFalse) {
                            shuizhiNum++;
                            shuizhiStr += '<li class="itemContentItem">' +
                                          '<div class="contentLeft"></div>' +
                                            '<div class="contentRight">' +
                                               ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + j + '"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                                '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                             '</div>' +
                                            '<div class="rightTopNum">'+shuizhiNum+'</div>' +
                                        '</li>'
                        }

                        break;
                

                }                    
        }

    }
   // console.log(inPolyPoints);
    //console.log(outPolyPoints);
    //poverlayType.setStrokeOpacity(0);
    //poverlayType.setFillOpacity(0);

   
    map.removeOverlay(polyg);

    var strAtt = [];
    strAtt.push(bengzhanStr, famenStr, liuliangStr, shuichangStr, shuiyuanStr, dabiaoStr, yaliStr, tiaofengStr, shuizhiStr);

    for (var st = 1; st < 10; st++) {
        if (strAtt[st - 1] == '') {
           // alert(1);
            $('.panoramaShowBox .panoramaMainBox .panoramaItemContent[data-fType="' + st + '"]').parent().hide();
            console.log($('.panoramaShowBox .panoramaMainBox .panoramaItemContent[data-fType="' + st + '"]').parent().attr('class'));
        } else {
            //alert(2);
            $('.panoramaShowBox .panoramaMainBox .panoramaItemContent[data-fType="' + st + '"]').parent().show();
            $('.panoramaShowBox .panoramaMainBox .panoramaItemContent[data-fType="' + st + '"]').html(strAtt[st - 1]);
        }
       
       
    }

    $('.panoramaShowBox').show();
    //layer.tab({
    //    type: 1,
    //    area: ['80%', '80%'],
    //    shadeClose: true,
    //    tab: [{
    //        title: '二供设备' + ergongNum + '个',
    //        content: '<ul>' + ergongStr + '</ul>'
    //    },
    //             {
    //                 title: '阀门' + famenNum + '个',
    //                 content: '<ul>' + famenStr + '</ul>'
    //             },
    //             {
    //                 title: '户表' + huNum + '个',
    //                 content: '<ul>' + huStr + '</ul>'
    //             },
    //             {
    //                 title: '水厂' + shuiNum + '个',
    //                 content: '<ul>' + shuiStr + '</ul>'
    //             },
    //             {
    //                 title: '大户表' + dahuNum + '个',
    //                 content: '<ul>' + dahuStr + '</ul>'
    //             },
    //             {
    //                 title: '加压站' + jiayaNum + '个',
    //                 content: '<ul>' + jiayaStr + '</ul>'
    //             },
    //             {
    //                 title: '水源' + shuiyuanNum + '个',
    //                 content: '<ul>' + shuiyuanStr + '</ul>'
    //             }


    //    ]
    //});


    
}

//overlaycomplete

function overlaycomplete(e) {
    var poverlayType = e.overlay;
    var type = e.drawingMode;
    
    judgePoint(type, poverlayType);

   
    $('.BMapLib_Drawing .BMapLib_box').eq(0).trigger('click');
   
    drawingManager.close();
   
   
}


// right tools 切换 
function righttoolsContentShow(ind) {

    if ($('.midToos>div').eq(ind).hasClass('active')) {
        return;
    }
   // alert(over.length);
    if (ind == 1) {
        //定位列表生成
        if (!getMarkerReady) {
           // alert(1);
            return;
        }
        //alert(2);
        dingweiListShow();
        //searchShowFn('熊猫');
        $('.searchBtn').click(function () {
            searchBtnFn();
        });
        
    }
    $('.midToos>div').eq(ind).addClass('active').siblings().removeClass('active');
    $('.toolsContent>div').eq(ind).addClass('show').siblings().removeClass('show');
}


//设置里的 关闭按钮
function timesClose() {
    $('.closeBtnBox').click(function () {
        $('.midToos>div').removeClass('active');
        $('.toolsContent>div').removeClass('show');
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
    $("body").click(function (e) {
        $('.select_ul').slideUp();
    });
    $('.selectBoxR .select_ul').delegate("li", "click", function (e) {
        var li = $(this);

        if (li.attr('data-value') == 0) {
            return;
        }

        if (li.hasClass('cur')) {
            li.removeClass('cur');
            li.parent().prev('.select_text').removeClass(li.attr('data-name'));
        } else {
            li.addClass("cur");
            li.parent().prev('.select_text').addClass(li.attr('data-name'));
        }
        e.stopPropagation();

        //li.addClass("cur").siblings("li").removeClass();
        
        //      .end().data("value").toString();
        //      if (val !== $this.val()) {
        //           select_text.text(li.text());
        //          $this.val(val);
        //           $this.attr("data-value",val);
        //        }
       // li.parent().prev('.select_text').html(li.html()).attr('data-name', li.attr('data-name'));
        

        var len =  li.parent().find('li.cur');
        var str = '';
       
        len.each(function (ind, val) {
            //len[ind].html();
            str += len[ind].innerHTML+' ';
            
        });
        li.parent().prev('.select_text').html(str);
        
        
       
    });

}



//lineMarker 选择 取消  fn  选择  地图覆盖物显示隐藏


function lineAndMaker(sel) {
    var currentSelect = sel;
    var attrNum;
    sel.click(function () {
        var ind = $(this).index();
        $(this).toggleClass('active');
        attrNum = $(this).attr('data-charindex');

        if ($(this).hasClass('active')) {

            for (var i = 0; i < over.length; i++) {
                if (over[i].FType == attrNum) {
                    over[i].show();
                }
            }
        } else {

            for (var j = 0; j < over.length; j++) {
                if (over[j].FType == attrNum) {
                    over[j].hide();
                }
            }
        }

    });
}

//  draw  tool hover 拖框

function drawToolsHover() {
    var timer;
    $('.searchTool').stop().click(function () {
        if ($(this).hasClass('show')) {
            $('.drawTools').slideUp();
            $(this).removeClass('show');
        } else {
            $('.drawTools').slideDown();
            $(this).addClass('show');
        }
        
    });
    //$('.searchTool').stop().mouseleave(function () {

    //    timer = setTimeout(function () {
    //        $('.drawTools').slideUp();
    //    }, 800);
    //});
    //$('.drawTools').stop().mouseenter(function (e) {
    //    clearTimeout(timer);
    //    e.stopPropagation();
    //});


}

//  区域 中选择 函数

function fenCheck() {
    $('body').delegate('.fenQuBox .areaMain li', 'click', function () {
       
        if ($(this).find('.checkMark').hasClass('active')) {
            $(this).find('.checkMark').removeClass('active');
            $(this).find('.checkFlog').removeClass('active');

            for (var i = 0; i < over.length; i++) {
                if (over[i].Type == 'marker' || over[i].Type == 'polyline') {
                   
                    for (var j = 0; j < (over[i].TB_AreaOverlay).length; j++) {
                        
                        if (over[i].TB_AreaOverlay[j].FMapAreaID == $(this).attr('data-AreaID')) {
                            over[i].hide();
                            for (var k = 0; k < over.length; k++) {
                                if (over[k].mark == over[i].TB_AreaOverlay[j].FMapAreaID) {
                                    over[k].hide();
                                }
                            }
                        }
                    }
                    
                }

            }
            
        } else {
            $(this).find('.checkMark').addClass('active');

            for (var i = 0; i < over.length; i++) {
                if (over[i].Type == 'marker' || over[i].Type == 'polyline') {
                    
                    for (var j = 0; j < (over[i].TB_AreaOverlay).length; i++) {

                        if (over[i].TB_AreaOverlay[j].FMapAreaID == $(this).attr('data-AreaID')) {
                            over[i].show();
                            
                        }
                    }

                }

            }
        }
    });
    
}



//区域  全选函数


function checkAll() {
    $('body').delegate('.fenQuBox .checkAllBox .checkBox', 'click', function () {
        if ($(this).index() == 0) {
            if ($(this).hasClass('active')) {
                $(this).removeClass('active');
                for (var i = 0; i < $('.fenQuBox .areaMain li').length; i++) {
                    if ($('.fenQuBox .areaMain li').eq(i).find('.checkMark').hasClass('active')) {
                      
                        $('.fenQuBox .areaMain li').eq(i).trigger('click');
                    }
                   
                }
            } else {
                $(this).addClass('active');

                for (var i = 0; i < $('.fenQuBox .areaMain li').length; i++) {
                    if (!$('.fenQuBox .areaMain li').eq(i).find('.checkMark').hasClass('active')) {
                      
                        $('.fenQuBox .areaMain li').eq(i).trigger('click');
                    }

                }
            }
          
        } else {
            if ($(this).hasClass('active')) {
                $(this).removeClass('active');
                for (var i = 0; i < $('.fenQuBox .areaMain li').length; i++) {
                    if ($('.fenQuBox .areaMain li').eq(i).find('.checkFlog').hasClass('active')) {

                        $('.fenQuBox .areaMain li').eq(i).find('.checkFlog').trigger('click');
                    }

                }
            } else {
                $(this).addClass('active');
                for (var i = 0; i < $('.fenQuBox .areaMain li').length; i++) {
                    if (!$('.fenQuBox .areaMain li').eq(i).find('.checkFlog').hasClass('active')) {

                        $('.fenQuBox .areaMain li').eq(i).find('.checkFlog').trigger('click');
                    }

                }
            }
           
        }
    });
}

//区域 小红旗 点击函数
function fenFlogClick() {
    $('body').delegate('.fenQuBox .areaMain .checkFlog', 'click', function (e) {
        var index = $(this).index();

        
        if ($(this).hasClass('active')) {
            $(this).removeClass('active');
            for (var i = 0; i < over.length; i++) {
                if (over[i].Type != 'marker' && over[i].Type != 'polyline') {
                    if (over[i].mark == $(this).attr('data-AreaID')) {
                        over[i].hide();
                    }
                }

            }
        } else {
            $(this).addClass('active');
            for (var i = 0; i < over.length; i++) {
                if (over[i].Type != 'marker' && over[i].Type != 'polyline') {
                    if (over[i].mark == $(this).attr('data-AreaID')) {
                        over[i].show();
                    }
                }

            }
        }
        e.stopPropagation();
    });

}




//报警各个函数

//报警 关闭按钮
function warnClose() {
    $('.warnBox .closeSet').click(function () {
        $('.warnBox').hide();
    });
}

//报警显示
function warnShow() {
    $('.warnTool').click(function () {
        $('.warnBox').slideToggle();
    });
}
//报警 滚动条 

function warnScroll() {

    $('.warnContentBox').mCustomScrollbar({
        scrollbarPosition: "inside",
        theme: "minimal-dark"
    });

}



//选择 选框的 关闭按钮

function panoramBoxClose() {
    $('.panCloseBtn').click(function () {
        $('.panoramaShowBox').hide();
    });
}
//选择 选框 的 滚动条
function panoramBoxScroll() {
    $('.panoramaMainBox').mCustomScrollbar({
        scrollbarPosition: "inside",
        theme: "minimal-dark"
    });

}






//  定位功能  中的函数u

//回到中心 点击函数
function clickToCenter(center) {
    var center = center ? center : '121.191705, 31.166028';
    $('.backCenter').click(function () {
        map.panTo(new BMap.Point(center.split(',')[0], center.split(',')[1]));
    });
}



//搜索 按钮 点击事件  函数

function searchBtnFn() {
    var $searchTxt = $('.searchTxt').val();
    searchShowFn($searchTxt);
}
//  搜索 显示  的 函数

function searchShowFn(str) {
    if ($.trim(str) == '') {
       
        dingweiListShow();
        
    }
    else {
        dingweiListShow();
        var $len = $('.dingWeiBox .itemContentItem').length;
        //alert($len);
        for (var i = 0; i < $len; i++) {
            if ($('.dingWeiBox .itemContentItem').eq(i).html().indexOf(str) == -1) {
                $('.dingWeiBox .itemContentItem').eq(i).hide();
                if ($('.dingWeiBox .itemContentItem').eq(i).parent().html().indexOf(str) == -1) {
                    $('.dingWeiBox .itemContentItem').eq(i).parent().parent().hide();
                }
            }
        }
    }
   
    //$('.dingWeiBox .itemContentItem').click(function () {
    //    if ($(this).html()) {
    //        alert($(this).html().indexOf(str));
    //        $(this).hide();
    //    };
    //});
}


//点击 快速定位到   所选 地点位置

function clickToDingwei() {
   
    $('body').delegate('.dingWeiBox .itemContentItem', 'click', function () {
        var zoom = map.getZoom();
       
        //setTimeout(function () {
        //     map.setZoom(18);
        //   setTimeout(function () {
        //         map.setZoom(7);
        //         setTimeout(function () {
        //             map.setZoom(zoom);
        //             map.panTo(over[$(this).attr('data-over')].FPoint);
        //         }, 1500);
        //     }, 1200);
        // }, 1400);
        //alert(over[$(this).attr('data-over')].FPoint);
        map.panTo(over[$(this).attr('data-over')].FPoint);
       
        over[$(this).attr('data-over')].setAnimation(BMAP_ANIMATION_BOUNCE);
        var overCur = over[$(this).attr('data-over')];
        setTimeout(function () {
            overCur.setAnimation(null);
        }, 1000);
       
       
        
    });
}

//数据列表点击显示

function dingweiListClick(){
    $('.dingWeiBox .panoramaItemTitle').click(function () {
     
        if ($(this).hasClass('cur')) {
            $(this).next().slideUp();
            $(this).parent().siblings().find('.panoramaItemTitle').next().slideUp();
            $('.panoramaItemTitle').removeClass('cur');
        } else {
            $(this).next().slideDown();
            $(this).parent().siblings().find('.panoramaItemTitle').next().slideUp();
            $(this).addClass('cur');
        }
       
    });
}

//数据列表加载函数
function dingweiListShow() {

    if (!getMarkerReady) {
        return;
        
    }
    var bengzhanNum = 0, famenNum = 0, liuliangNum = 0, shuichangNum = 0, shuiyuanNum = 0, dabiaoNum = 0, yaliNum = 0, tiaofengNum = 0, shuizhiNum = 0;
    var bengzhanStr = '', famenStr = '', liuliangStr = '', shuichangStr = '', shuiyuanStr = '', dabiaoStr = '', yaliStr = '', tiaofengStr = '', shuizhiStr = '';

    for (var j = 0; j < over.length; j++) {
       
        if (over[j].Type != 'marker') {
            continue;
        }

       
        var overNum = Number(over[j].FType);
        //alert(overNum);
        switch (overNum) {
            case 1:

                    bengzhanNum++;
                    bengzhanStr += '<li class="itemContentItem" data-over="' + j + '">' +
                                  '<div class="contentLeft"></div>' +
                                    '<div class="contentRight">' +
                                       ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + j + '"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                        '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                     '</div>' +
                                    '<div class="rightTopNum">' + bengzhanNum + '</div>' +
                                '</li>'
               


                break;

            case 2:

               
                    famenNum++;
                    famenStr += '<li class="itemContentItem" data-over="' + j + '">' +
                                  '<div class="contentLeft"></div>' +
                                    '<div class="contentRight">' +
                                       ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + j + '"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                        '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                     '</div>' +
                                    '<div class="rightTopNum">' + famenNum + '</div>' +
                                '</li>'
               

                break;

            case 3:

                
                    liuliangNum++;
                    liuliangStr += '<li class="itemContentItem" data-over="' + j + '">' +
                                  '<div class="contentLeft"></div>' +
                                    '<div class="contentRight">' +
                                       ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + j + '"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                        '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                     '</div>' +
                                    '<div class="rightTopNum">' + liuliangNum + '</div>' +
                                '</li>'
              

                break;

            case 4:

               
                    shuichangNum++;
                    shuichangStr += '<li class="itemContentItem" data-over="' + j + '">' +
                                  '<div class="contentLeft"></div>' +
                                    '<div class="contentRight">' +
                                       ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + j + '"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                        '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                     '</div>' +
                                    '<div class="rightTopNum">' + shuichangNum + '</div>' +
                                '</li>'
               

                break;

            case 5:

               
                    shuiyuanNum++;
                    shuiyuanStr += '<li class="itemContentItem" data-over="' + j + '">' +
                                  '<div class="contentLeft"></div>' +
                                    '<div class="contentRight">' +
                                       ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + j + '"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                        '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                     '</div>' +
                                    '<div class="rightTopNum">' + shuiyuanNum + '</div>' +
                                '</li>'
                
                break;

            case 6:


                
                    dabiaoNum++;
                    dabiaoStr += '<li class="itemContentItem" data-over="' + j + '">' +
                                  '<div class="contentLeft"></div>' +
                                    '<div class="contentRight">' +
                                       ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + j + '"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                        '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                     '</div>' +
                                    '<div class="rightTopNum">' + dabiaoNum + '</div>' +
                                '</li>'


                break;
            case 7:

              
                    yaliNum++;
                    yaliStr += '<li class="itemContentItem" data-over="' + j + '">' +
                                  '<div class="contentLeft"></div>' +
                                    '<div class="contentRight">' +
                                       ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + j + '"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                        '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                     '</div>' +
                                    '<div class="rightTopNum">' + yaliNum + '</div>' +
                                '</li>'
               

                break;
            case 8:

               
                    tiaofengNum++;
                    tiaofengStr += '<li class="itemContentItem" data-over="' + j + '">' +
                                 '<div class="contentLeft"></div>' +
                                   '<div class="contentRight">' +
                                      ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + j + '"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                       '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                    '</div>' +
                                   '<div class="rightTopNum">' + tiaofengNum + '</div>' +
                               '</li>'
               
                break;

            case 9:

            
                    shuizhiNum++;
                    shuizhiStr += '<li class="itemContentItem" data-over="' + j + '">' +
                                  '<div class="contentLeft"></div>' +
                                    '<div class="contentRight">' +
                                       ' <div class="contentRightTop"><span class="picBox picBox1" data-over="' + j + '"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                        '<div class="contentRightBottom"><span class="picBox"></span><span class="txtBox">' + over[j].name + '</span></div>' +
                                     '</div>' +
                                    '<div class="rightTopNum">' + shuizhiNum + '</div>' +
                                '</li>'
                
                break;


        }



    }

    var strAtt = [];
    strAtt.push(bengzhanStr, famenStr, liuliangStr, shuichangStr, shuiyuanStr, dabiaoStr, yaliStr, tiaofengStr, shuizhiStr);

   
    for (var st = 1; st < 10; st++) {
        if (strAtt[st - 1] == '') {
           
            $(' .dingWeiBox .panoramaMainBox .panoramaItemContent[data-fType="' + st + '"]').parent().hide();
        } else {
          
            $(' .dingWeiBox .panoramaMainBox .panoramaItemContent[data-fType="' + st + '"]').parent().show();
            $(' .dingWeiBox .panoramaMainBox .panoramaItemContent[data-fType="' + st + '"]').html(strAtt[st - 1]);
           
        }


    }

    //dingweiListClick();

    //点击定位到 位置
    clickToDingwei();
}



//全景的点击函数

function panoramClick() {
    $('body').delegate('.panoramaItemContent .contentRight .picBox1', 'click', function () {
        var index = $(this).attr('data-over');
        if (over[index].Type == 'marker') {
            var url = window.location.href;
           


            // panorama.setId('0100010000130501122416015Z1');
         
            var point = new BMap.Point((over[index].FPoint).lng, (over[index].FPoint).lat);
          
            var panoramaService = new BMap.PanoramaService();

            panoramaService.getPanoramaByLocation(point,6600, function (d) {
                if (d) {
                  
                    panorama.setPov({ heading: -40, pitch: 6 });
                    panorama.setId(d.id);
                   
                    $('.panoramaBackBtn').show().click(function () {
                        window.location.href = url;
                    });

                } else {
                    alert('没有全景');
                    return;
                }

            });

         
            
            //panorama.setPosition(over[index].FPoint);
           
        }
       
    });
   
}


//水站  弹出的窗口  操作函数
function shuiZhanPop() {
    //弹出  大框的  关闭等操作

    //down  animate
    $('body').delegate('.close', 'click', function () {
       
        $('.stage').remove();

        //$(this).removeClass('zoomOutDown animated');
        //$('.stage').remove();
        //});
    });
    $('body').delegate('.dabiaoAndLiuliang .pump_name', 'click', function () {
        $('.fore').removeClass('clickTwo').addClass('click').css('z-index', '0');
        $('.back').removeClass('clickTwo').addClass('click').css('z-index', '2');
    });
    $('body').delegate('.backBtn', 'mouseover', function () {
        $(this).addClass('tada animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
            $(this).removeClass('tada animated');
        });
    });
    $('body').delegate('.backBtn', 'click', function () {
        $('.back').removeClass('click').addClass('clickTwo').css('z-index', '0');
        $('.fore').removeClass('click').addClass('clickTwo').css('z-index', '2');
    });

    $('body').delegate('.back_bottom  .pump_info li', 'mouseover', function () {
        $(this).addClass('bounce animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
            $(this).removeClass('bounce animated');
        });
    });


}




//压力 内容生成函数(已废除)


function showYaliVContent(dataArr,dataTimeArr,dataMaxArr,dataMinArr,dotFirst) {
    if ($('.stage')) {
        $('.stage').remove();
    }
    var addTime = dotFirst.createDate;
    var infoBox =
    '<div class="stage yaLi">' +
'	<div class="contentBox">' +
'		<div class="fore">' +
'			' +
'			<div class="fore_header">' +
'				' +
'				<div class="img-box yaliChartBox clearfix">' +
                      '<div id="yaLiChart"></div>'+
'				</div>' +
'				' +
'				<div class="time-box">' +
'					<p class="pump_time">采集时间：<span>' + addTime + '</span></p>					' +
'				</div>				' +
'				<div class="number_box">' +
'					<p class="pump_nameYali">' + dotFirst.fName + '</p>' +
'					<p class="pump_number"></p>' +
'				</div>' +
'				' +
'			</div>	' +
'			' +
'			<div class="closeBox"><span class="close">&times;</span></div>' +
'			<div class="content">' +
'				<ul class="power_box">' +
'					<li class="clearfix"><div class="power_name"><span class="power_name_bg child1"></span>压力上限</div><div class="number">' + dotFirst.categary['shangXian'] + '</div></li>' +
'					<li class="clearfix"><div class="power_name"><span class="power_name_bg child2"></span>压力下限</div><div class="number">' + dotFirst.categary['xiaXian'] + '</div></li>' +
'					<li class="clearfix"><div class="power_name"><span class="power_name_bg child3"></span>实时压力</div><div class="number">' + dotFirst.categary['shiShi'] + '</div></li>' +
'					<li class="clearfix"><div class="power_name"><span class="power_name_bg child4"></span>电池电量</div><div class="number">' + dotFirst.categary['dianLiang'] + '</div></li>' +
'				</ul>' +
'			</div>						' +
'		</div>' +
'		<div class="back">' +
'			<div class="closeBox"><span class="close">&times;</span></div>' +
'			<div class="back_header">' +
'				<p class="backBox"><span class="backBtn"></span></p>			' +
'                <div id="echart_box"></div>' +
'<div title=" pId " class="show_list"><ul><li class="showList_li1"></li><li class="showList_li2"></li><li class="showList_li3"></li><li class="showList_li4"></li><li class="showList_li5"></li></ul></div>' +
'			</div>			' +
'			<div class="back_content">' +
'				<ul>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg1"></span>名称</div>' +
'						<div class="item_content">管网</div>' +
'					</li>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg2"></span>品牌</div>' +
'						<div class="item_content">Panda</div>' +
'					</li>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg3"></span>保养周期</div>' +
'						<div class="item_content">6个月</div>' +
'					</li>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg4"></span>更换周期</div>' +
'						<div class="item_content">6个月</div>' +
'					</li>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg5"></span>材质</div>' +
'						<div class="item_content">PVC</div>' +
'					</li>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg6"></span>埋深</div>' +
'						<div class="item_content">0.8米</div>' +
'					</li>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg7"></span>设备类型</div>' +
'						<div class="item_content">官网监测</div>' +
'					</li>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg8"></span>设备状态</div>' +
'						<div class="item_content">使用中</div>' +
'					</li>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg9"></span>管径</div>' +
'						<div class="item_content">DF65</div>' +
'					</li>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg10"></span>口径</div>' +
'						<div class="item_content">DF65</div>' +
'					</li>' +
'				</ul>' +
'				' +
'			</div>' +
'		</div>' +
'	</div>' +
'</div> ';


    $('body').append(infoBox);
    $('.stage').css({
        'top': '50%',
        'left': '51%',
        'margin-top': -270 + "px",
    });
    $('.stage').ready(function () {
    }).addClass('bounceInDown animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend',
     function () {
         var dataArrR = dataArr.reverse();
         yaliChart(dataArrR,dataTimeArr, dataMaxArr, dataMinArr);
         $(this).removeClass('bounceInDown animated');

         //var myChart = echarts.init($('#echart_box')[0]);
         //myChart.setOption(option0);
         //function chart1Request(ID) {
         //    $.ajax({
         //        url: '../../Service/T_ReportService.ashx?method=getcurvedata',
         //         cache:false,
         //        data: { 'PumpID': ID },

         //        success: function (data) {
         //            var data = JSON.parse(data);
         //            var xData = [];
         //            var seriesData = [];
         //            $.each(data, function (i, v) {
         //                xData.push(v.T_Time);
         //                seriesData.push(v.A03)
         //            });
         //            var options1 = {
         //                xAxis: {
         //                    data: xData
         //                },
         //                series: {
         //                    data: seriesData
         //                },
         //            };
         //            myChart.setOption(options1);
         //        },
         //        error: function (data) {
         //        }
         //    });
         //}




         //chart1Request(193);


     });

    $('body').delegate('.pump_name', 'mouseover', function () {
        $(this).addClass('swing animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
            $(this).removeClass('swing animated');
        });
    });
}


//压力的  echarts

function yaliChart(pressArr,timeArr,mixArr,minArr) {
    
   // alert(timeArr);
  
    var yaliCha = echarts.init(document.getElementById('yaLiChart'));

    var yaliOption = {
        title: {
            show:false,
            text: '压力实时曲线'
        },
        tooltip: {
            trigger: 'axis'
        },
        legend: {
            show: false,
            data: ['最大值线', '压力曲线', '最小值线']
        },
        grid: {
            borderColor: 'gray',
            top: 4,
            left: 4,
            right: 4,
            bottom: 4
        },
        axisLine: {
            show: false,
            lineStyle: {
                color: '#ccc'
            }
        },

        xAxis: {
            show: false,
            type: 'category',
            boundaryGap: false,
            data: timeArr.reverse(),
            splitLine: {
                show: false
            }
        },
        yAxis: {
            show: false,
            type: 'value',
            axisLabel: {
                formatter: '{value} Mpa'
            },
            splitLine: {
                show: false
            }
        },
        color: 'blue',
        series: [
            {
                name: '最大值线',
                type: 'line',
                color: '#d48265',
                data: mixArr,
                showSymbol: false,
                itemStyle: {
                    normal: {
                        lineStyle: {
                            color: 'rgba(255,255,255,.8)',
                            type: 'dotted'
                        }
                    },
                },

            },
            {
                name: '压力曲线',
                type: 'line',
                symbol: "circle",      // 默认是空心圆（中间是白色的），改成实心圆
                symbolSize: 4,
                data: pressArr,
                //markPoint: {
                //    data: [
                //        { name: '最低', value: -2, xAxis: 1, yAxis: 0 }
                //    ]
                //},
                itemStyle: {
                    normal: {
                        color: '#FFD700',
                        borderColor: '#fff',
                        borderWidth: 1,
                        lineStyle: {
                            color: '#FFD700',

                        }
                    },
                },


            },
            {
                name: '最小值线',
                showSymbol: false,
                type: 'line',
                color: '#d48265',
                data: minArr,

                itemStyle: {
                    normal: {
                        lineStyle: {
                            color: 'rgba(255,255,255,.8)',
                            type: 'dotted'
                        }
                    },
                },

            }
        ]
    };


    yaliCha.setOption(yaliOption);
}

//大表和 流量 内容生成函数
function showContent2(data, pId) {
    if ($('.stage')) {
        $('.stage').remove();
    }
    
    var addTime = data.updateTime?data.updateTime:'无';
    var infoBox =
    '<div class="stage dabiaoAndLiuliang">' +
'	<div class="contentBox">' +
'		<div class="fore">' +
'			' +
'			' +
'			<div class="fore_header">' +
'				' +
'				<div class="img-box clearfix">' +
//'					<li><img src="img1/img1.png" /></li>' +
//'					<li><img src="img1/img2.png" /></li>' +
//'					<li><img src="img1/img1.png" /></li>' +
//'					<li><img src="img1/img2.png" /></li>' +
'				</div>' +
'				' +
'				<div class="time-box">' +
'					<p class="pump_time">采集时间：<span>' + addTime + '</span></p>					' +
'				</div>				' +
'				<div class="number_box">' +
'					<p class="pump_name">' + data.FName + '</p>' +
'					<p class="pump_number">23</p>' +
'				</div>' +
'				' +
'			</div>	' +
'			' +
'			<div class="closeBox"><span class="close">&times;</span></div>' +
'            <div class="camera"></div>' +
'			<div class="content">' +
'				<ul class="power_box">' +
'					<li class="clearfix"><div class="power_name"><span class="power_name_bg child1"></span>正累积流量</div><div class="number">' + (data.P01 ? data.P01 : ' 无') + '</div></li>' +
'					<li class="clearfix"><div class="power_name"><span class="power_name_bg child2"></span>压力</div><div class="number">' + (data.A03 ? data.A03 : '无 ') + '</div></li>' +
'					<li class="clearfix"><div class="power_name"><span class="power_name_bg child3"></span>瞬时流量</div><div class="number">' + (data.A01 ? data.A01 : '无 ') + '</div></li>' +
'					<li class="clearfix"><div class="power_name"><span class="power_name_bg child4"></span>负累积流量</div><div class="number">' + (data.P02 ? data.P02 : '无 ') + '</div></li>' +
'					<li class="clearfix"><div class="power_name"><span class="power_name_bg child5"></span>GPRS电压</div><div class="battery_box"><span class="battery_progress"></span></div><div class="number">' + (data.V ? data.V : ' 无') + '</div></li>' +
'					<li class="clearfix"><div class="power_name"><span class="power_name_bg child6"></span>电池电压</div><div class="battery_box"><span class="battery_progress"></span></div><div class="number">' + (data.A02 ? data.A02 : ' 无') + '</div></li>' +
'				</ul>' +
'			</div>						' +
'			<div class="fore_footer">' +
'				<ul class="warning_info_box">' +
'					<li><a href="javascript:void(0);">东泵房报警泵房水压报警1</a></li>' +
'					<li><a href="javascript:void(0);">泵房阀门异常报警2</a></li>' +
'					<li><a href="javascript:void(0);">泵房流量报警3</a></li>' +
'				</ul>' +
'			</div>				' +
'		</div>' +
'		<div class="back">' +
'			<div class="closeBox"><span class="close">&times;</span></div>' +
'			<div class="back_header">' +
'				<p class="backBox"><span class="backBtn"></span></p>			' +
'                <div id="echart_box"></div>' +
'<div title="' + pId + '" class="show_list"><ul><li class="showList_li1"></li><li class="showList_li2"></li><li class="showList_li3"></li><li class="showList_li4"></li><li class="showList_li5"></li></ul></div>' +
'			</div>			' +
'			<div class="back_content">' +
'				<ul>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg1"></span>名称</div>' +
'						<div class="item_content">管网</div>' +
'					</li>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg2"></span>品牌</div>' +
'						<div class="item_content">Panda</div>' +
'					</li>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg3"></span>保养周期</div>' +
'						<div class="item_content">6个月</div>' +
'					</li>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg4"></span>更换周期</div>' +
'						<div class="item_content">6个月</div>' +
'					</li>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg5"></span>材质</div>' +
'						<div class="item_content">PVC</div>' +
'					</li>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg6"></span>埋深</div>' +
'						<div class="item_content">0.8米</div>' +
'					</li>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg7"></span>设备类型</div>' +
'						<div class="item_content">官网监测</div>' +
'					</li>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg8"></span>设备状态</div>' +
'						<div class="item_content">使用中</div>' +
'					</li>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg9"></span>管径</div>' +
'						<div class="item_content">DF65</div>' +
'					</li>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg10"></span>口径</div>' +
'						<div class="item_content">DF65</div>' +
'					</li>' +
'				</ul>' +
'				' +
'			</div>' +
'		</div>' +
'	</div>' +
'</div> ';


    $('body').append(infoBox);
    $('.stage').css({
        'top': '50%',
        'left': '51%',
        'margin-top': -270 + "px",
    });
    $('.stage').ready(function () {
    }).addClass('bounceInDown animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend',
     function () {
         $(this).removeClass('bounceInDown animated');

         //var myChart = echarts.init($('#echart_box')[0]);
         //myChart.setOption(option0);
         //function chart1Request(ID) {
         //    $.ajax({
         //        url: '../../Service/T_ReportService.ashx?method=getcurvedata',
         //        data: { 'PumpID': ID },

         //        success: function (data) {
         //            var data = JSON.parse(data);
         //            var xData = [];
         //            var seriesData = [];
         //            $.each(data, function (i, v) {
         //                xData.push(v.T_Time);
         //                seriesData.push(v.A03)
         //            });
         //            var options1 = {
         //                xAxis: {
         //                    data: xData
         //                },
         //                series: {
         //                    data: seriesData
         //                },
         //            };
         //            myChart.setOption(options1);
         //        },
         //        error: function (data) {
         //        }
         //    });
         //}




         //chart1Request(193);


     });

    $('body').delegate('.pump_name', 'mouseover', function () {
        $(this).addClass('swing animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
            $(this).removeClass('swing animated');
        });
    });


}





//tiaofengData.dataAll.push(dataItem);

//{
//    'id':value.id,
//    'type': value.type,
//    'baseId': value.baseId,
//    'online': value.FOnLine,
//    'customerId': value.FCustomerID,
//    'updateTime':value.FUpdateTime,
//    'categary': {
//        'shuiWei': value.F40005,
//        'storeShui': value.F40006,
//        'userShui': value.F40004,
//        'canUseTime': value.F40007,
//        'inPress': value.F40001,
//        'outPress': value.F40002,
//        'famenKai': value.F40009
//    }


// 调峰内容生成函数
function showContentTiaoFeng(data, pId) {
    if ($('.stage')) {
      
        $('.stage').remove();
    }
    console.log(data);
    console.log(23231);
   // clearInterval(outTimer);
    var addTime = data.updateTime ? data.updateTime.replace('T', ' ') : '无 ';
    //var addTime = data.updateTime;
    var infoBox =
    '<div class="stage tiaoFeng">' +
'	<div class="contentBox">' +
'		<div class="fore">' +
'			' +
'			' +
'			<div class="fore_header">' +
'				' +
'				<div class="img-box clearfix">' +
'					<div></div>' +
'				</div>' +
'				' +
'				<div class="time-box">' +
'					<p class="pump_time">采集时间：<span>' + addTime + '</span></p>					' +
'				</div>				' +
'				<div class="number_box">' +
'					<p class="pump_name">' + data.FName + '</p>' +
'					<p class="pump_number"></p>' +
'				</div>' +
'				' +
'			</div>	' +
'			' +
'			<div class="closeBox"><span class="close">&times;</span></div>' +
//'            <div class="camera"></div>' +
'			<div class="content">' +
'				<ul class="power_box">' +
'                   <li class="clearfix"><div class="picBox"></div><p class="itemTxt">进水压力</p><p class="itemNumber">' + data.categary.inPress + 'Mpa</p></li>' +
'                   <li class="clearfix"><div class="picBox"></div><p class="itemTxt">出水压力</p><p class="itemNumber">' + data.categary.outPress + 'Mpa</p></li>' +
'                   <li class="clearfix"><div class="picBox"></div><p class="itemTxt">蓄水流量</p><p class="itemNumber">' + data.categary.xuShuiLiuLiang + 'm³/h</p></li>' +
'                   <li class="clearfix"><div class="picBox"></div><p class="itemTxt">出水流量</p><p class="itemNumber">' + data.categary.chuShuiLiuLiang + 'm³/h</p></li>' +
'                   <li class="clearfix"><div class="picBox"></div><p class="itemTxt">水箱容积</p><p class="itemNumber">' + data.categary.shuiXiangRongJi + 'm³</p></li>' +
'                   <li class="clearfix"><div class="picBox"></div><p class="itemTxt">储水总量</p><p class="itemNumber">' + data.categary.chuShuiZongLiang + 'm³</p></li>' +
'                   <li class="clearfix"><div class="picBox"></div><p class="itemTxt">调峰水量</p><p class="itemNumber">' + data.categary.tiaoFengShuiLiang + 'm³</p></li>' +
'                   <li class="clearfix"><div class="picBox"></div><p class="itemTxt">日调峰量</p><p class="itemNumber">' + data.categary.riTiaoFengLiang + 'm³</p></li>' +
'                   <li class="clearfix"><div class="picBox"></div><p class="itemTxt">储水比例</p><p class="itemNumber">' + data.categary.chuShuiBiLi + '%</p></li>' +
'                   <li class="clearfix"><div class="picBox"></div><p class="itemTxt">调峰能力</p><p class="itemNumber">' + data.categary.tiaoFengNengLi + 'mim</p></li>' +
'                   <li class="clearfix"><div class="picBox"></div><p class="itemTxt">滞留时间</p><p class="itemNumber">' + data.categary.zhiLiuShiJian + 'h</p></li>' +
'                   <li class="clearfix"><div class="picBox"></div><p class="itemTxt">水箱液位</p><p class="itemNumber">' + data.categary.shuiXiangYeWei + 'm</p></li>' +
'                   <li class="clearfix"><div class="picBox"></div><p class="itemTxt">蓄满时间</p><p class="itemNumber">' + data.categary.chuManShiJian + 'h</p></li>' +
'                   <li class="clearfix"><div class="picBox"></div><p class="itemTxt">调节比例</p><p class="itemNumber">' + data.categary.tiaoJieBiLi + '%</p></li>' +
'                   <li class="clearfix"><div class="picBox"></div><p class="itemTxt">阀门开度</p><p class="itemNumber">' + data.categary.liuFaYuanKai + '%</p><span class="itemSetBtn"></span><div class="kaiduSetBox"><div class="closeB">&times;</div><div class="liuLiangYuanKong clearfix"><p>流量阀远程控制</p><span class=" liuLiangFaStart">启用</span><span class="liuLiangFaClose">停用</span></div><div class="kaiduRadia"></div><div class="radioSetBox"><input class="kaiduZhi"><button class="kaiduBtn">开度设置</button></div></div></li>' +

//'					<li class="clearfix"><div class="power_name"><span class="power_name_bg child1"></span>水箱水位</divliuFaYuanKai><div class="number">' + data.categary.shuiWei + 'm</div></li>' +
//'					<li class="clearfix"><div class="power_name"><span class="power_name_bg child2"></span>储水量</div><div class="number">' + data.categary.storeShui + '吨</div></li>' +
//'					<li class="clearfix"><div class="power_name"><span class="power_name_bg child3"></span>用户用水量</div><div class="number">' + data.categary.userShui + '吨</div></li>' +
//'					<li class="clearfix"><div class="power_name"><span class="power_name_bg child4"></span>可用时间</div><div class="number">' + data.categary.canUseTime + 'Min</div></li>' +
//'					<li class="clearfix"><div class="power_name"><span class="power_name_bg child5"></span>进水压力</div><div class="number">' + data.categary.inPress + 'Mpa</div></li>' +
//'					<li class="clearfix"><div class="power_name"><span class="power_name_bg child6"></span>出水压力</div><div class="number">' + data.categary.outPress + 'Mpa</div></li>' +
//'					<li class="clearfix"><div class="power_name"><span class="power_name_bg child7"></span>阀门开度</div><div class="number">' + data.categary.tiaojiebili + '</div></li>' +
//'					<li class="clearfix"><div class="power_name"><span class="power_name_bg child8"></span>开度设置</div><div class="number"></div></li>' +
'				</ul>' +
'			</div>						' +
'		</div>' +
'		<div class="back">' +
'			<div class="closeBox"><span class="close">&times;</span></div>' +
'			<div class="back_header">' +
'				<p class="backBox"><span class="backBtn"></span></p>			' +
'                <div id="echart_box"></div>' +
'<div title="' + pId + '" class="show_list"><ul><li class="showList_li1"></li><li class="showList_li2"></li><li class="showList_li3"></li><li class="showList_li4"></li><li class="showList_li5"></li></ul></div>' +
'			</div>			' +
'			<div class="back_content">' +
'				<ul>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg1"></span>名称</div>' +
'						<div class="item_content">熊猫管网</div>' +
'					</li>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg2"></span>品牌</div>' +
'						<div class="item_content">熊猫</div>' +
'					</li>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg3"></span>保养周期</div>' +
'						<div class="item_content">6个月</div>' +
'					</li>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg4"></span>更换周期</div>' +
'						<div class="item_content">6个月</div>' +
'					</li>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg5"></span>材质</div>' +
'						<div class="item_content">PVC</div>' +
'					</li>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg6"></span>埋深</div>' +
'						<div class="item_content">0.8米</div>' +
'					</li>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg7"></span>设备类型</div>' +
'						<div class="item_content">官网监测</div>' +
'					</li>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg8"></span>设备状态</div>' +
'						<div class="item_content">使用中</div>' +
'					</li>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg9"></span>管径</div>' +
'						<div class="item_content">DF65</div>' +
'					</li>' +
'					<li class="clearfix">' +
'						<div class="item"><span class="item_bg item_bg10"></span>口径</div>' +
'						<div class="item_content">DF65</div>' +
'					</li>' +
'				</ul>' +
'				' +
'			</div>' +
'		</div>' +
'	</div>' +
'</div> ';

    $('body').append(infoBox);
    $('.stage').css({
        'top': '50%',
        'left': '51%',
        'margin-top': -270 + "px",
    });
    $('.stage').ready(function () {
    }).addClass('bounceInDown animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend',
     function () {
         $(this).removeClass('bounceInDown animated');

         //var myChart = echarts.init($('#echart_box')[0]);
         //myChart.setOption(option0);
         //function chart1Request(ID) {
         //    $.ajax({
         //        url: '../../Service/T_ReportService.ashx?method=getcurvedata',
         //        data: { 'PumpID': ID },

         //        success: function (data) {
         //            var data = JSON.parse(data);
         //            var xData = [];
         //            var seriesData = [];
         //            $.each(data, function (i, v) {
         //                xData.push(v.T_Time);
         //                seriesData.push(v.A03)
         //            });
         //            var options1 = {
         //                xAxis: {
         //                    data: xData
         //                },
         //                series: {
         //                    data: seriesData
         //                },
         //            };
         //            myChart.setOption(options1);
         //        },
         //        error: function (data) {
         //        }
         //    });
         //}




         //chart1Request(193);


     });

 $('body').delegate('.pump_name', 'mouseover', function () {
        $(this).addClass('swing animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
            $(this).removeClass('swing animated');
        });
    });

 var timerOnoff = true;

if (data.categary.liuFaYuanKong == '1') {
    $('.liuLiangFaStart').addClass('active');
} else {
    $('.liuLiangFaClose').addClass('active');
}


    //下命令
$('body').delegate('.itemSetBtn', 'click', function () {
    
    $('.kaiduSetBox').show();
    $('.kaiduRadia').empty();
    var $TiaoFengKaiSet = $('.kaiduRadia').radialIndicator({
        radius: 30,
        barWidth: 6,
        //barBgColor: '#21a0ff',
        barColor: '#21a0ff',
        initValue: data.categary.liuFaYuanKai,
        roundCorner: true,
        percentage: true
    }).data('radialIndicator');

    $('.kaiduBtn').click(function () {
        var This = $(this);
        if (!timerOnoff) {
            return;
        }
        
        //var timer;
        clearInterval(This.timer);
        var time = 5;
        timerOnoff = false;
        var val = $.trim($('.kaiduZhi').val());
        
        if (val != '') {
            //alert(val);
            
          
            // commondFn('F40017', val,data.baseId,8,data.FDTUCode,data.FSchemeID);
            $.ajax({
                url: '/V_CDJK/GetCommand',
                cache: false,
                data: {
                    'name': 'F40017',
                    'text': 1,
                    'id': data.baseId,
                    'type': 8,
                    'dtu': data.FDTUCode,
                    'FSchemeID': data.FSchemeID
                },
                success: function (data) {

                    var dataJSON = JSON.parse(data);
                    if (dataJSON.success) {
                        $('.kaiduZhi').attr('disabled', true);
                        This.timer = setInterval(function () {
                            if (time == -1) {
                                clearInterval(This.timer);
                                //$TiaoFengKaiSet.animate(val);
                                This.html('开度设置');
                                timerOnoff = true;
                                $('.kaiduZhi').attr('disabled', false).val('');
                                return;
                            }
                            This.html('(' + time + ')');
                            time--;
                        }, 1000);

                    } else {
                        timerOnoff = true;
                        alert('请稍后重试');
                    }
                },
                error: function (data) {

                }
            });
        } else {
            alert('请输入开度！');
            timerOnoff = true;
            return;
        }
        

       

    });
 

});
$('body').delegate('.closeB', 'click', function () {
    $('.kaiduSetBox').css('display','none');

});

$('body').delegate('.liuLiangFaStart', 'click', function () {
   
    var This = $(this);
    //if ($(this).hasClass('active')) {
    //    return;
    //}

    if (!timerOnoff) {

        return;
    }
    
    //var timer;
    clearInterval(This.timer);
    var time = 5;
    timerOnoff = false;
    // commondFn('F40016', 1, data.baseId,8, data.FDTUCode, data.FSchemeID);

    $.ajax({
        url: '/V_CDJK/GetCommand',
        cache:false,
        data: {
            'name': 'F40016',
            'text': 1,
            'id': data.baseId,
            'type': 8,
            'dtu': data.FDTUCode,
            'FSchemeID': data.FSchemeID
        },
        success: function (data) {

            var dataJSON = JSON.parse(data);
            if (dataJSON.success) {
                This.timer = setInterval(function () {
                    if (time == -1) {
                        clearInterval(This.timer);
                        This.html('启用');
                        timerOnoff = true;
                        return;
                    }
                    This.html('(' + time + ')');
                    time--;
                }, 1000);

            } else {
                timerOnoff = true;
                alert('请稍后重试');
            }
        },
        error: function (data) {
            
        }
    });

    
    //$(this).addClass('active').next().removeClass('active');

});
$('body').delegate('.liuLiangFaClose', 'click', function () {
   
    var This = $(this);
    //if ($(this).hasClass('active')) {
    //    return;
    //}
    //$(this).addClass('active').prev().removeClass('active');

    if (!timerOnoff) {
        return;
    }
   
    //var timer;
    clearInterval(This.timer);
    var time = 5;
    timerOnoff = false;
    // commondFn('F40016', 0, data.baseId,8,data.FDTUCode,data.FSchemeID);

    $.ajax({
        url: '/V_CDJK/GetCommand',
        cache: false,
        data: {
            'name': 'F40016',
            'text': 0,
            'id': data.baseId,
            'type': 8,
            'dtu': data.FDTUCode,
            'FSchemeID': data.FSchemeID
        },
        success: function (data) {

            var dataJSON = JSON.parse(data);
            if (dataJSON.success) {
                This.timer = setInterval(function () {
                    if (time == -1) {
                        clearInterval(This.timer);
                        This.html('停用');
                        timerOnoff = true;
                        return;
                    }
                    This.html('(' + time + ')');
                    time--;
                }, 1000);

            } else {
                timerOnoff = true;
                alert('请稍后重试');
            }
        },
        error: function (data) {

        }
    });
    
});

}

//

$('body').delegate('.back_content ul li', 'mouseenter', function () {

    $(this).addClass('lightSpeedIn animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
        $(this).removeClass('lightSpeedIn animated');
    });
});






//下   单命令 的函数
//function commondFn(name, txt, baseID,type, DTUcode, FSchemeID) {
//    $.ajax({
//        url: '/V_CDJK/GetCommand',
//        cache:false,
//        data: {
//            'name':name,
//            'text': txt,
//            'id':baseID,
//           'type':type,
//           'dtu':DTUcode,
//           'FSchemeID': FSchemeID
//        },
//        success: function (data) {
            
//            var dataJSON = JSON.parse(data);
//            if (dataJSON.success) {
//                alert(1);
//            }
//        },
//        error: function (data) {
//            return;
//        }
//    });
//}



//环形进度条的

//var info = '<div class="pressWra" ><div></div class="kaiduTitle">1号阀门开度<br/><br /><div class="kaiSet" style="float: left; width: 100px;height: 100px;"></div><div class="inputB" style="float: right;text-align: center;"><input class=" kaiInp" type="text" placeholder="请输入开度" style="width: 90px;height: 24px;border: none;border-bottom: 1px solid gray; margin: 10px 0 18px 0;"><br/><button class="kaiBtn" style="cursor: pointer;width: 80px;height: 30px;background: #7eaf2a;border: none;color: #fff;">设定开度</button></div></div>';
//var tempInfoBox = "";
//function infonaBox(marker) {

//    var infoBo = new BMapLib.InfoBox(map, info, {
//        boxStyle: {
//            background: "#fff"
//            , borderRadius: "6px"
//            , padding: "4px"
//            , boxShadow: "0 0 2px gray"
//            , textAlign: "center"
//            , width: "200px"
//            , height: "160px"
//        }
//          , closeIconMargin: "6px 10px 0 0"
//         , closeIconUrl: "img/closeBtn.png"
//          , enableAutoPan: true
//          , align: INFOBOX_AT_TOP

//    });


//    if (tempInfoBox != "") {
//        tempInfoBox.close();
//    }
//    tempInfoBox = infoBo;
//    infoBo.open(marker);

//    var $kaiSet = $('.kaiSet').radialIndicator({
//        radius: 40,
//        barWidth: 10,
//        initValue: 40,
//        roundCorner: true,
//        percentage: true
//    }).data('radialIndicator');

//    $('.kaiBtn').click(function () {

//        var val = $.trim($('.kaiInp').val());

//        if (val != '') {
//            //alert(val);
//            $kaiSet.animate(val);
//        } else {
//            alert('请输入开度！');
//        }

//    });
//}





//polonClick  折线点击函数
//function polonClick(){
//	for (var i = 0; i < over.length; i++) {
//        if (over[i].Type == 'marker') {
//            over[i].setAnimation(null);
//            over[i].disableDragging();
//        } else {
//            over[i].disableEditing();
//        }
//    }
//    this.enableEditing();
//}

//pointClick marker 点击函数
//function pointClick(){	
//    for (var i = 0; i<over.length; i++) {
//        if (over[i].Type =='marker') {
//            over[i].setAnimation(null);
//            over[i].disableDragging();
//        } else {
//            over[i].disableEditing();
//        }
//    }
//    this.setAnimation(BMAP_ANIMATION_BOUNCE);
//    this.enableDragging();
//}





//右侧 报警 列表  

//报警获取  新 

function alarmGetFn2() {
    
    $.ajax({
        url: '/V_YCJK/ALLAlarm_List',
        cache: false,
        success: function (data) {
            

            var dataJSON = JSON.parse(data);
            console.log(dataJSON);
            dataJSON = dataJSON.obj;
            console.log(dataJSON);
            console.log(54545454);

            if (dataJSON.length < 1) {
                return;
            }
            var onOff = false;
            var warnTimer = setInterval(function () {
                if (onOff) {
                    $('.warnTool').addClass('warn');
                }else {
                    $('.warnTool').removeClass('warn');
                }
                onOff = !onOff;

            }, 400);
            var dataStr = '';
            $(dataJSON).each(function (index, value) {
                var time = (value.TempTime) ? (value.TempTime).replace('T', ' ') : '无';
                dataStr += '<li class="warnItem">'
                            +'<div class="timeMarker"></div>'
                           + ' <div class="timeBox"><span class="dateTxt">' + time + '</span><span class="timeTxt"></span> <span class="timeGap">' + value.TimeRange + '</span></div>'
                           +' <div class="itemContent">'
                           + '     <div class="warnAdreess"><span class="adressBg"></span>' + value.FName + '</div>'
                           + '     <div class="warnTxt">' + value.FSetMsg + '</div>'
                           +' </div>'
                        + ' </li>';
            });
            $('.warnBox .warnContent').html(dataStr);
        },
        error: function () { }
    });
}

//报警获取
//function alarmGetFn() {
//    $.ajax({
//        url: '../../Service/T_AlarmService.ashx?method=searchalarm',
//        cache:false,
//        data: {
//            'ID':7
//        },
//        success: function (data) {
          
//            var dataJson = JSON.parse(data);
//            if (!dataJson.length >= 1) {
//                alert('wu');
//                return;
//            }
//            var onOff = false;
//            var warnTimer = setInterval(function () {
//                if (onOff) {
//                    $('.warnTool').addClass('warn');
//                }else {
//                    $('.warnTool').removeClass('warn');
//                }
//                onOff = !onOff;
               
//            }, 400);
//            var dataStr = '';
//            $(dataJson).each(function (index,value) {
//                dataStr += '<li class="warnItem">'
//                            +'<div class="timeMarker"></div>'
//                           + ' <div class="timeBox"><span class="dateTxt">' + (value.TempTime).replace('T',' ') + '</span><span class="timeTxt"></span> <span class="timeGap">' + value.TimeRange + '</span></div>'
//                           +' <div class="itemContent">'
//                           + '     <div class="warnAdreess"><span class="adressBg"></span>' + value.FName + '</div>'
//                           + '     <div class="warnTxt">' + value.FSetMsg + '</div>'
//                           +' </div>'
//                        + ' </li>';
//            });
//            $('.warnBox .warnContent').html(dataStr);
//        },
//        error: function () {
//            alert('错了啊');
//        }
//    });
//}



//全景动画

function panoramaShow() {

    $('.quanjingItem').click(function () {
        var panoLngLat = $('.quanjingItem').attr('pano');

        layer.open({
            type: 2,
            anim: 3,
            shade: .6,
            title: false,
            shadeClose: true,
            area: ['90%', '90%'],
            content: '/YCJK/V_YCJK/Panorama?latLng=' + panoLngLat,

        });
    });;
}



//新加压力全景动画

function panoramaShowYali() {

    $('.pumpQuanJing').click(function () {
       // var panoLngLat = $('.quanjingItem').attr('pano');

        layer.open({
            type: 2,
            anim: 3,
            shade: .6,
            title: false,
            shadeClose: true,
            area: ['90%', '90%'],
            content: 'http://map.baidu.com/#panoid=01000300001311011550342725G&panotype=street&heading=172.69&pitch=0&l=13&tn=B_NORMAL_MAP&sc=0&newmap=1&shareurl=1&pid=01000300001311011550342725G',

        });
    });;
}