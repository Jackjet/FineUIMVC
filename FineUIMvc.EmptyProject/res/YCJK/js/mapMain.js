var map;
var panorama;
var drawingManager;
var mapType = [BMAP_NORMAL_MAP,BMAP_SATELLITE_MAP,BMAP_PERSPECTIVE_MAP];

var tanChangOnOff = true;

var currentPump= '', currentJZ = '';
var currentPumpData = [];


var txtTanChuang = ['实时数据','机组信息','数据报表','预警报警'];

//弹窗 内 数据的 刷新

//var tanchuangDateTimer;

//管理员
var adminDataArr = [];
var adminDataArr2 = [];
var adminDataGetOnOff = true;
var adminPageIndex = 0;
var adminPageSize = 60;

var isAdmin=false;
var adminDataArrGetTimer;


//客户
var myIcon_BengZhan = new BMap.Icon("/res/YCJK/img/bz.png", new BMap.Size(30, 30));
labelColor = {
    '1': {
        color: "#fff",                   //颜色
        fontSize: "12px",               //字号
        border: "1px solid #70d7f5",                    //边
        borderRadius: "2px",
        height: "14px",
        padding: "0 2px",
        lineHeight: "14px",
        cursor: "pointer",
        boxShadow: "0 0 0 2px #00b7ee",
        backgroundColor: "#00b7ee"    //蓝色
    }
}
var customDataArr = [];
var customDataGetOnOff = true;
var customPageIndex = 0;
var customPageSize = 60;

var isCustom = false;
var customDataArrGetTimer;



//分公司
var sideComDataArr = [];
var sideComDataGetOnOff = true;
var sideComPageIndex = 0;
var sideComPageSize = 60;

var isSideCompony = false;
var sideComDataArrGetTimer;


//其他

var otherDataArr = [];
var otherDataGetOnOff = true;
var otherPageIndex = 0;
var otherPageSize = 60;

var isOther = false;
var otherDataArrGetTimer;




$(function () {

    loginTypeChoice();

	
	pageResize();
	$(window).resize(function(){
		pageResize();
	});
	rightSectionClick();

	initRightChart();
	getJzStatus();
	getPumpJzCount();
	//getWaterUseTop10();
	//getElecUseTop10();


	//mapClickOutFn();

	selectFn($('.selectBox'));

	tanCengClose();

	tanCengScroll();

	tanCengRightNavClickFn();

    //  管理员数据分组 获取 

	clearInterval(adminDataArrGetTimer);

	adminDataArrGetTimer = setInterval(function () {
	    adminPageIndex++;
	   
	    if ((adminPageIndex != 0) && (isAdmin) && adminDataGetOnOff) {
	       
	        getDataFnAdmin();
	    } else {
	        adminPageIndex--;
	    }

	}, 1000);

    //  客户数据分组 获取 

	clearInterval(customDataArrGetTimer);

	customDataArrGetTimer = setInterval(function () {
	    customPageIndex++;

	    if ((customPageIndex != 0) && (isCustom) && customDataGetOnOff) {

	        getDataFnCustom();
	    } else {
	        customPageIndex--;
	    }

	}, 1000);

    // 分公司数据分组 获取 

	clearInterval(sideComDataArrGetTimer);

	sideComDataArrGetTimer = setInterval(function () {
	    sideComPageIndex++;

	    if ((sideComPageIndex != 0) && (isSideCompony) && sideComDataGetOnOff) {
	        //alert('aa');
	        getDataFnSideCompany();
	    } else {
	       // alert('dd');
	        sideComPageIndex--;
	    }

	}, 1000);


    // 其他数据分组 获取 

	clearInterval(otherDataArrGetTimer);

	otherDataArrGetTimer = setInterval(function () {
	    otherPageIndex++;

	    if ((otherPageIndex != 0) && (isOther) && otherDataGetOnOff) {

	        getDataFnOther();
	    } else {
	        otherPageIndex--;
	    }

	}, 1000);



	

    //图表上的  radius  函数
	radialBar();



    //全景

	panoramaShow();

  
});

function getMap(type, zoom, maxZo, minZo, themeStyle, center) {
    mapInit(type, zoom, maxZo, minZo, themeStyle, center);
	rightSectionScroll();
}


function overlaycomplete(e){
	
}



//页面 载入时   登录者  类型的  选择 函数

function loginTypeChoice() {
    $.ajax("/V_YCJK/SearchTemp")
　　.done(function (data) {
        // alert(data);
        // console.log(data);
        // console.log(5555555555555);
         var dataJSON = JSON.parse(data);
         dataJSON = dataJSON.obj;
         //console.log(dataJSON);
        // console.log(111122);
         var userType = Number(dataJSON.data[0].json_user[0].UserType);
         var userJson =dataJSON.data[1].json_map[0];
        // alert(userType);

         switch (userType) {
             case 1: {
                 getMap(0, 6, 19, 4, 0, '106.982553,35.393253');
                 isAdmin = true;
                 getDataFnAdmin();

                 break;
             }
             case 2: {
                 getMap(0, 6, 19, 4, 0, '110.919575,32.073821');
                 isSideCompony = true;
                 getDataFnSideCompany();

                 break;
             }
             case 3: {
                 var FMapType = userJson.userJson;
                 var FZoom = userJson.FZoom;
                 var FMaxZoom = userJson.FMaxZoom;
                 var FMinZoom = userJson.FMinZoom;
                 var FStyle = userJson.FStyle;
                 var FCenter = userJson.FCenter;
                 getMap(FMapType, FZoom, FMaxZoom, FMinZoom, FStyle, FCenter);

                 isCustom = true;
                 getDataFnCustom();
                 break;
             }
             case 4: {
                 getMap(0, 6, 19, 4, 0, '110.919575,32.073821');
                 isOther = true;
                 getDataFnOther();
                 break;
             }
         }
    })
　　.fail(function () { alert("出错啦！"); })
    
}


//页面  载入 之后  点的  获取 函数


//管理员
function getDataFnAdmin() {

    if (!adminDataGetOnOff) {
        return;
    }
    adminDataGetOnOff = false;
    $.ajax({
        url: '/V_YCJK/SearchMarker',
        data: {
            'pageIndex': adminPageIndex,  
            'pageSize': adminPageSize     
        },
        success: function (data) {
            //console.log(data);
            var dataJSON = JSON.parse(data);
           // alert(adminPageIndex);
           // console.log(dataJSON);
           // console.log(55555555555);

            var pointArr = dataJSON.obj;

            var count = dataJSON.count;

            var yeNum = Math.ceil(count / customPageSize);
             // alert(yeNum);


            if (pointArr[0].pumpJZ[0]) {
                var firstJzId = pointArr[0].pumpJZ[0].pumpJZID;
            } else {
                var firstJzId = 0;
            }
          

            if (adminPageIndex > (yeNum - 1)) {
                 // alert('fan');
                adminType(adminDataArr2);
                //console.log(adminDataArr2);

                //console.log(12312);
                clearInterval(adminDataArrGetTimer);
                return;
            } else if (adminPageIndex == 0) {
                adminDataArr = [];
                // for (var i = 0; i < 500; i++) {
                $(pointArr).each(function (ind, val) {
                    if (val.PLngLat) {
                        var firstJzId;
                        if (val.pumpJZ.length) {
                            firstJzId = val.pumpJZ[0].pumpJZID;
                        } else {
                            firstJzId = '';
                        }

                        var point = new BMap.Point(val.PLngLat.split(',')[0], val.PLngLat.split(',')[1]);
                        point.userData = {};
                        point.userData.pumpID = val.pumpID;
                        point.userData.firstJzId = firstJzId;
                        adminDataArr.push(point);
                    }

                });
                // }


                adminType(adminDataArr);
                adminDataGetOnOff = true;
            } else {
               
                // for (var i = 0; i < 500; i++) {
                $(pointArr).each(function (ind, val) {
                    if (val.PLngLat) {
                        var firstJzId;
                        if (val.pumpJZ.length) {
                            firstJzId = val.pumpJZ[0].pumpJZID;
                        } else {
                            firstJzId = '';
                        }

                        var point = new BMap.Point(val.PLngLat.split(',')[0], val.PLngLat.split(',')[1]);
                        point.userData = {};
                        point.userData.pumpID = val.pumpID;
                        point.userData.firstJzId = firstJzId;
                        adminDataArr2.push(point);
                       
                    }

                });
                // }


               
                adminDataGetOnOff = true;
            }
            // alert(adminPageIndex);
            //if (adminPageIndex >0) {
            //    return;
            //    clearInterval(adminDataArrGetTimer);
            //}
            //if (pointArr.length == 0) {
               
            //    return;
            //   // alert(adminPageIndex);
            //}
           

        },
        error: function () {

        }
    });
}


//非管理员

//客户
function getDataFnCustom() {

    if (!customDataGetOnOff) {
        return;
    }
    customDataGetOnOff = false;
    $.ajax({
        url: '/V_YCJK/SearchMarker',
        data: {
            'pageIndex':customPageIndex,
            'pageSize': customPageSize
        },
        success: function (data) {
            //alert(data);
           // console.log(data);
            var dataJSON = JSON.parse(data);
            //console.log(dataJSON);
            //console.log(55555555555);

            var pointArr = dataJSON.obj;

            var count = dataJSON.count;

            var yeNum = Math.ceil(count / customPageSize);
          //  alert(yeNum);

            if (pointArr.length < 1) {
                return;
            }

            var firstJzId = pointArr[0].pumpJZ[0].pumpJZID;

            if (customPageIndex > (yeNum - 1)) {
                //  alert('fan');
                clearInterval(customDataArrGetTimer);
                //yaLiPressGetNew(customDataArr);

                //window.setInterval(function () {
                //    yaLiPressGetNew(customDataArr);
                //}, 6000);
                return;
            }

            $(pointArr).each(function (ind, val) {
                var inPress = val.pumpJZ[0].D_Data[0].F41006;
                var outPress = val.pumpJZ[0].D_Data[0].F41007;
                if (val.PLngLat) {
                   

                    var firstJzId;
                    if (val.pumpJZ.length) {
                        firstJzId = val.pumpJZ[0].pumpJZID;
                    } else {
                        firstJzId = '';
                    }
                    var $size = new BMap.Size(24, 0);
                    var point = new BMap.Point(val.PLngLat.split(',')[0], val.PLngLat.split(',')[1]);
                    var newMarker = new BMap.Marker(point);
                    var label = new BMap.Label(inPress, {
                        offset: $size,
                        position: point
                    });
                    label.setStyle(labelColor['1']);

                    newMarker.pumpID = val.pumpID;
                    newMarker.setTitle ( val.PName);
                    newMarker.jZID = firstJzId;
                    newMarker.setLabel(label);
                    newMarker.setIcon(myIcon_BengZhan);
                    newMarker.addEventListener('click', kehuType);
                    map.addOverlay(newMarker);
                    customDataArr.push(newMarker);

                    //设置定时 获取
                    window.setInterval(function () {
                        yaLiPressGetNew2(newMarker);
                    }, 6000*3);

                    //var markerClusterer = new BMapLib.MarkerClusterer(map, { markers: customDataArr });
                }

            });

           
            customDataGetOnOff = true;
        },
        error: function () { }
    });
}


//非管理员 显示 压力的  获取 函数


//new  用法

function yaLiPressGetNew2(point) {
    if(point){
        $.ajax({
            url: '/V_YCJK/Search_Pump_JinChuWat',
            async: true,
            timeout: 3000,
            data: {
                'pumpID': point.pumpID
            },
            success: function (data) {

                // console.log(data);
                var dataJSON = JSON.parse(data);
                //console.log(dataJSON);
                //console.log(55555555555);

                var pointArr = dataJSON.obj;

                var inPress = pointArr[0].pumpJZ[0].F41006;

                point.getLabel().setContent(inPress);
            },
            error: function () {

            }
        });
    };
}

//old 用法
function yaLiPressGetNew(pointArr) {
   // alert(0);
    $(pointArr).each(function (ind, val) {
        $.ajax({
            url: '/V_YCJK/Search_Pump_JinChuWat',
            async:true,
            data: {
                'pumpID': val.pumpID
            },
            success: function (data) {
               
               // console.log(data);
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




//分公司

function getDataFnSideCompany() {
    if (!sideComDataGetOnOff) {
        return;
    }
    sideComDataGetOnOff = false;
    $.ajax({
        url: '/V_YCJK/SearchMarker',
        async: true,
        data: {
            'pageIndex': sideComPageIndex,
            'pageSize': sideComPageSize
        },
        success: function (data) {
           // alert(data);
            //console.log(data);
            var dataJSON = JSON.parse(data);
            //console.log(dataJSON);
            //console.log(55555555555);

            var pointArr = dataJSON.obj;

            //console.log(pointArr);
            var count = dataJSON.count;

            var yeNum = Math.ceil(count / sideComPageSize);
           // alert(yeNum);

//
           // var firstJzId = pointArr[0].pumpJZ[0].pumpJZID;


            if (sideComPageIndex > (yeNum - 1)) {
                // alert('fan');
                clearInterval(sideComDataArrGetTimer);
      
                //yaLiPressGetNew(sideComDataArr);

                //window.setInterval(function () {
                //    yaLiPressGetNew(sideComDataArr);
                //}, 6000);
                return;
            }
            $(pointArr).each(function (ind, val) {
                var inPress = val.pumpJZ[0].D_Data[0].F41006;
                var outPress = val.pumpJZ[0].D_Data[0].F41007;
                if (val.PLngLat) {
                   

                    var firstJzId;
                    if (val.pumpJZ.length) {
                        firstJzId = val.pumpJZ[0].pumpJZID;
                    } else {
                        firstJzId = '';
                    }
                    var $size = new BMap.Size(24, 0);
                    var point = new BMap.Point(val.PLngLat.split(',')[0], val.PLngLat.split(',')[1]);
                    var newMarker = new BMap.Marker(point);
                    var label = new BMap.Label(inPress, {
                        offset: $size,
                        position: point
                    });
                    label.setStyle(labelColor['1']);

                    newMarker.pumpID = val.pumpID;
                    newMarker.setTitle ( val.PName);
                    newMarker.jZID = firstJzId;
                    newMarker.setLabel(label);
                    newMarker.setIcon(myIcon_BengZhan);

                    newMarker.addEventListener('click', sideCompanyType);
                    map.addOverlay(newMarker);
                    sideComDataArr.push(newMarker);

                    //设置定时 获取
                    window.setInterval(function () {
                        yaLiPressGetNew2(newMarker);
                    }, 6000*3);
                }

            });


            sideComDataGetOnOff = true;
        },
        error: function () { }
    });
}


//其他客户

function getDataFnOther() {
    if (!otherDataGetOnOff) {
        return;
    }
    otherDataGetOnOff = false;
    $.ajax({
        url: '/V_YCJK/SearchMarker',
        async: true,
        data: {
            'pageIndex': otherPageIndex,
            'pageSize': otherPageSize
        },
        success: function (data) {
            // alert(data);
            //console.log(data);
            var dataJSON = JSON.parse(data);
            //console.log(dataJSON);
            //console.log(55555555555);

            var pointArr = dataJSON.obj;

            var count = dataJSON.count;

            var yeNum = Math.ceil(count / otherPageSize);
            // alert(yeNum);

            var firstJzId = pointArr[0].pumpJZ[0].pumpJZID;

            if (otherPageIndex > (yeNum - 1)) {
                // alert('fan');
                clearInterval(otherDataArrGetTimer);

                //yaLiPressGetNew(otherDataArr);

                //window.setInterval(function () {
                //    yaLiPressGetNew(otherDataArr);
                //}, 6000);
                return;
            }
            $(pointArr).each(function (ind, val) {
                var inPress = val.pumpJZ[0].D_Data[0].F41006;
                var outPress = val.pumpJZ[0].D_Data[0].F41007;
                if (val.PLngLat) {


                    var firstJzId;
                    if (val.pumpJZ.length) {
                        firstJzId = val.pumpJZ[0].pumpJZID;
                    } else {
                        firstJzId = '';
                    }
                    var $size = new BMap.Size(24, 0);
                    var point = new BMap.Point(val.PLngLat.split(',')[0], val.PLngLat.split(',')[1]);
                    var newMarker = new BMap.Marker(point);
                    var label = new BMap.Label(inPress, {
                        offset: $size,
                        position: point
                    });
                    label.setStyle(labelColor['1']);

                    newMarker.pumpID = val.pumpID;
                    newMarker.setTitle(val.PName);
                    newMarker.jZID = firstJzId;
                    newMarker.setLabel(label);
                    newMarker.setIcon(myIcon_BengZhan);

                    newMarker.addEventListener('click', otherType);
                    map.addOverlay(newMarker);
                    otherDataArr.push(newMarker);

                    //设置定时 获取
                    window.setInterval(function () {
                        yaLiPressGetNew2(newMarker);
                    }, 6000*3);
                }

            });


            otherDataGetOnOff = true;
        },
        error: function () { }
    });
}

//  页面  地图 初始  载入之后  的 不同类型 登录者的  处理函数

function adminType(pointArr) {
    if (document.createElement('canvas').getContext) {  // 判断当前浏览器是否支持绘制海量点
        
        var options = {
            size: BMAP_POINT_SIZE_BIG,
            shape: BMAP_POINT_SHAPE_WATERDROP,
            color: 'rgba(0,0,255,0.4)'
        }
        var pointCollection = new BMap.PointCollection(pointArr, options);  // 初始化PointCollection
        pointCollection.addEventListener('click', function (e) {
           // alert('单击点的坐标为：' + e.point.lng + ',' + e.point.lat + e.point.log1 + JSON.stringify(e.point));
            // 监听点击事件

            var pumpID = e.point.userData.pumpID;
            var JZID = e.point.userData.firstJzId;
            //alert(pumpID);
            mapClickOutFn(pumpID,JZID);
        });
        map.addOverlay(pointCollection);  // 添加Overlay
    } else {
        alert('请在chrome、safari、IE8+以上浏览器查看');
    }
}
function sideCompanyType() {
    //var markerClusterer = new BMapLib.MarkerClusterer(map, { markers: markers });

    var pumpID = this.pumpID;
    var JZID = this.jZID;
    mapClickOutFn(pumpID, JZID);

}
function kehuType() {
    var pumpID = this.pumpID;
    var JZID = this.jZID;
    mapClickOutFn(pumpID, JZID);
   

}
function otherType() {
    var pumpID = this.pumpID;
    var JZID = this.jZID;
    mapClickOutFn(pumpID, JZID);
}

//右侧 区域的  滚动条函数

function rightSectionScroll(){
	$('.sectionBox').mCustomScrollbar({
        scrollbarPosition: "inside",
        theme: "minimal-dark"
    });
}


//页面重置函数
function pageResize(){
	var $H=$(window).height();
	var $W=$(window).width();
	if($W<=1366){
		$('.mapBox').animate({'right':'12'},300);
		
	}else if($W>1366) {		
		if(!$('.showHideBtn').hasClass('show')){
			$('.mapBox').animate({'right':'362'},300);
		}
		
	}
}

//右侧区域的 点击 显示隐藏事件

function rightSectionClick(){
	$('.showHideBtn').click(function(){
		var $W=$(window).width();
		
		if($(this).hasClass('show')){
			$(this).removeClass('show');
			$('.rightSection').animate({'right':'-3'},300);
			if($W<=1366){
				$('.mapBox').animate({'right':'12'},300);
			}else {
				$('.mapBox').animate({'right':'362'},300);
			}
		}else {
			$(this).addClass('show');
			$('.rightSection').animate({'right':'-348'},300);
			if($W<=1366){
				$('.mapBox').animate({'right':'12'},300);
			}else {
				$('.mapBox').animate({'right':'12'},300);
			}
		}
		
		
		
	});
}






//弹出层的  事件

//弹出的 点击 函数  
function mapClickOutFn(pumpID,JZID) {
   
    if (!tanChangOnOff) {
        return;
    }
    tanChangOnOff = false;
       // TweenMax.from('.mapClickShow', 2.8, { 'width': '-=100px', 'height': '-=100px', ease: Bounce.easeInOut });
    $('.mapClickShow').show().css('marginLeft','0');

       // TweenMax.set('.mapClickShow', { 'scale': 0.5 });
        TweenMax.set('.mapClickShow', { 'scale': 0.5 });

        TweenLite.set(".mapClickShow",{y:'-=100',x:'-=100'});
        TweenLite.set(".mapClickShowMid", { transformPerspective: 500, rotationY: 90 });
        TweenLite.set(".mapClickShowRight",{'x':'-417',autoAlpha:0,zIndex:-1});
        var Tw1 = new  TimelineLite();

        Tw1.to('.mapClickShow', 0.3, { x: '+=100', y: '+=100', ease: Back.easeInOut, yoyo: true })
        .to('.mapClickShow', 0.4, { 'scale': 1, delay: 0.2, ease: Back.easeOut })
        .to('.mapClickShowMid', 0.2, { rotationY: 0, ease: Power2.easeOut,yoyo: true })
        .to('.mapClickShowRight', 0.4, { 'x': '+=417px', autoAlpha: 1 })
    
        //tanChangOnOff = true;
        
        tanChuangDataUse(pumpID, JZID);
      

    //mapClickShow
    //mapClickShowLeft
    //mapClickShowMid
    //mapClickShowRight
        
    

}


//弹出窗  里边 数据的  处理函数

function tanChuangDataUse(bengfangID,JZID) {
    $.ajax({
        url: '/V_YCJK/Search_Pump_JZReportList',
        async: true,
        data: {
            'pumpID': bengfangID
        },
        success: function (data) {
            var dataJSON = JSON.parse(data);
            dataJSON = dataJSON.obj;


           
            currentPumpData.length = 0;
            currentPumpData = dataJSON[0];

            //console.log(currentPumpData);
            //console.log(55556666);

            if (JZID) {
                selectListFn(currentPumpData, JZID);

                tanCengTimeLineData(bengfangID, JZID);
            }
           
          

           
            //clearInter(tanchuangDateTimer);
            // tanChuangDataGetNew();
            //tanchuangDateTimer = setInterval(function () {

            //}, 1000 * 10);
           
        },
        error: function () { }

    });
}


//弹窗 数据的  刷新  函数

//function tanChuangDataGetNew() {
//    $.ajax({
//        url: '/V_YCJK/Search_Pump_JZReportList',
//        data: {
//            'pumpID': bengfangID
//        },
//        success: function (data) {
//            var dataJSON = JSON.parse(data);
//            dataJSON = dataJSON.obj;


//            currentPumpData.length = 0;
//            currentPumpData = dataJSON[0];

          
//            var index = $('.selectBox .select_ul li.cur').index();
//            jzDatashow(currentPumpData, index);

//        },
//        error: function () { }

//    });
//}


//弹出 窗 左侧 数据的  处理
//select 
function selectListFn(currentPumpData, JZID) {
   
    //console.log(currentPumpData);
    //console.log('琴琴勤');
    var pumpName=currentPumpData.PCustomPName;
    var pumpID=currentPumpData.pumpID;
    var jzLen = currentPumpData.pumpJZ;
    var pumpAddress = currentPumpData.PAddress;
    var PLngLat = currentPumpData.PLngLat;
    currentJZ = currentPumpData.pumpJZ[0].pumpJZID;
    $('.pumpJz .pumpJzTxt').html(pumpAddress);
    $('.selectBox .select_ul').empty();
    $('.quanjingItem').attr('pano', PLngLat);
    var listStr = '';
    $(jzLen).each(function (ind, val) {
       
        if (val.pumpJZID == JZID) {//val.PumpJZName
            listStr += '<li data-pumpID="' + pumpID + '" data-jzID="' + val.pumpJZID + '" data-value="' + (ind + 1) + '" class="cur">' + pumpName + '(' + val.PumpJZArea + ')' + '</li>';
            $('.selectBox .select_text').html(pumpName + '(' + val.PumpJZArea + ')');
            jzDatashow(currentPumpData, ind,pumpID,JZID);
        }else {
            listStr += '<li data-pumpID="' + pumpID + '" data-jzID="' + val.pumpJZID + '" data-value="' + (ind + 1) + '" class="">' + pumpName + '(' + val.PumpJZArea + ')' + '</li>';
        }
       
    });

    $('.selectBox .select_ul').append( listStr);

    
    
}

// 显示 泵房状态  等信息的处理

function jzDatashow(currentPumpData, index,pumpID,jzID) {
   
   

    var currentJZDataArr = currentPumpData.pumpJZ[index].D_Data[0];
    var pumpAux = currentPumpData.pumpJZ[index].Auxiliarypumpcount;
    var pumpRun = currentPumpData.pumpJZ[index].RunPumpNum;
    var pumpJZNum = pumpAux + pumpRun;
    //var outNum = currentPumpData.pumpJZ[index].DrainPumpNum;
    var machineType = $.trim(currentPumpData.pumpJZ[index].MachineType);

    var PLngLat = currentPumpData.PLngLat;
    var isAlarm = currentPumpData.pumpJZ[index].IsAlarm;
    var status;

    var typeStr;
    

    //所有叶轮隐藏
    $('.yelunItem').removeClass('active');


    $('.quanjingItem').attr('pano', PLngLat);
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


    if (currentJZDataArr['TempTime']) {
        var caiJITime = currentJZDataArr['TempTime'].replace(/[^0-9]/ig, "");
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
                yelunState=' ';
            } else if (pumpArr[yeN] == '2') {
                yelunState='online';
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

            //console.log(dataJSON);
            //console.log('qinqinqin');
            var dataListStr = '';
            $(dataJSON).each(function (ind, val) {
                $('.forData').empty();
               
                var dataTxt;
                // dataTxt = jzDataArr[val.FField] ? jzDataArr[val.FField] : '无';
                dataTxt = jzDataArr[val.FField] || (jzDataArr[val.FField] == 0) ? jzDataArr[val.FField] : ' ';
                if (val.FField == 'FUpdateDate') {
                    dataTxt = changeTime(dataTxt.replace(/[^0-9]/ig, ""));
                }
                if (val.FField == "InOutWaPa") {
                   
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
                dataListStr += '<li class="listItem clearfix">'+
                   '     <div class="listItemName">' + val.FName + '：</div>' +
                   '     <div class="listItemTxt" data-FField="' + val.FField + '">' + dataTxt + '</div>' +
                   ' </li>';
            });
            $('.forData').append(dataListStr);



        }
    });
}


//机组详细信息函数
function infoDetailFn(pumpID,pumpJZID) {

    $.ajax({
        url: '/V_YCJK/Search_PumpJZDetail',
        data:{
            'pumpID':pumpID,
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
                    //console.log(pumpData[key]);
                    //console.log('qinqin');
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
        //console.log(pumpData["pumpVQ"]);
        for (var j = 0; j < pumpData["pumpVQ"].length; j++) {

             str += '<div class="shiPinItem clearfix">                                         ' +
                   '         <div class="shiPinItemName">' + pumpData["pumpVQ"][j].QuipmentType + '</div>                                  ' +
                   '         <div class="shiPinItemMain">                                             '+
                   '             <div class=" shiPinItemBox shiPinTypeBox clearfix">                  '+
                   '                 <div class="shiPinItemName shiPinTypeName">类型：</div>           '+
                   '                 <div class="shiPinItemTxt shiPinTypeTxt">' + pumpData["pumpVQ"][j].Type + '</div>             ' +
                   '             </div>                                                               '+
                   '             <div class=" shiPinItemBox shiPinPinPaiBox clearfix">                '+
                   '                 <div class="shiPinItemName shiPinPinPaiName">品牌：</div>         '+
                   '                 <div class="shiPinItemTxt shiPinPinPaiTxt">' + pumpData["pumpVQ"][j].Brand + '</div>           ' +
                   '             </div>                                                               '+
                   '             <div class=" shiPinItemBox shiPinXingHaoBox clearfix">               '+
                   '                 <div class="shiPinItemName shiPinXingHaoName">型号：</div>        '+
                   '                 <div class="shiPinItemTxt shiPinXingHaoTxt">' + pumpData["pumpVQ"][j].Number + '</div>            ' +
                   '             </div>                                                               '+
                   '             <div class="shiPinItemBox shiPinDuanKouBox clearfix">                '+
                   '                 <div class="shiPinItemName shiPinDuanKouName">IP端口号：</div>     '+
                   '                 <div class="shiPinItemTxt shiPinDuanKouTxt">' + pumpData["pumpVQ"][j].Port + '</div>       ' +
                   '             </div>                                                               '+
                   '         </div>                                                                   '+
                   '     </div>';                                                                     
                   
        }
        $(".shiPinHeader").siblings().remove();
        $(".shiPinHeader").after(str);
    }

}



//  弹窗中  设备 下拉列表

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
    $("body").click(function () {
        $('.select_ul').slideUp();
    });
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

            //弹窗的 图表

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
               $('.forBaoJing').css('background','none');
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
        timeLineStr += '<div class="timeItem clearfix">            '+
                  '      <div class="leftTimeBox">                  '+          
                  '          <div class="timeLine">                 '+
                  '              <div class="timeQiu"></div>        '+
                  '          </div>                                 '+
                  '      </div>                                     '+
                  '      <div class="rightContentBox">              '+
                  '          <div class="warnHeader">' + timeLineTime+ '</div>    ' +
                  '          <div class="warnTxt">' +val.FSetMsg + '</div>            ' +
                  '      </div>                                     '+
                  '  </div>';                                     
    });

    $('.forBaoJing').html(timeLineStr);

    

}

//弹层 时间线 高度 处理 函数

function timeLineHeightFn() {
    $('.leftTimeBox').height($('.leftTimeBox').next().height()+20);
    //alert($('.leftTimeBox').next().height());
    $('.timeLine').height($('.timeLine').parent().height());
}


//弹层的  滚动条

function tanCengScroll(){
    $('.rightScrollBox').mCustomScrollbar({
        scrollbarPosition: "inside",
        theme: "minimal-dark"
    });
}
//  弹层的 点击 事件

function tanCengClose() {
    $('body').delegate('.mapClickShowRight .closeBox', 'click', function () {
        $('.mapClickShow').hide();
        tanChangOnOff = true;
    });
}


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

//时间转换

function changeTime(time) {
    if (!time) {
        return;
    }
    var Time = new Date(Number(time));
    return Time.getFullYear() + "-" + (Time.getMonth() + 1) + "-" + Time.getDate() + " " + Time.getHours() + ":" + Time.getMinutes() + ":" + Time.getSeconds();
}
