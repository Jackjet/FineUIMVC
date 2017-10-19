
     //$('.showHide').click(function () {
     //    if ($(this).hasClass('show')) {
     //        $('.wrap').animate({ 'right': '-220px' });
     //        $(this).removeClass('show').html('<<');
     //    } else {
     //        $('.wrap').animate({ 'right': '0' });
     //        $(this).addClass('show').html('>>');
     //    }
     //});

//var clearAll;

//function liShow(obj) {
//    var objIndex = $(obj).index() / 3;
//    $('.attrBoxUl li').eq(objIndex).removeClass('hide').siblings().addClass('hide');
//    alert(objIndex);
//}


function clickFn(obj) {
    var objId = $(obj).attr('nameId');
    var inputBoxInfo = $(obj).prev().val();
    var nameBoxInfo = $(obj).prev().prev();
    //			    	alert(inputBoxInfo);
    //			    	alert(objId);
    //			    	alert(over.length)
    for (var i = 0; i < over.length; i++) {

        if (over[i].mark == objId) {
            console.log(1);
            console.log(over[i]);
            console.log(2);
            over[i].name = inputBoxInfo;
            nameBoxInfo.html(inputBoxInfo);
            //			    			alert(over[i].name);
            //			    			alert("Ok");
            return;
        }
    }
}
var over = [];
var map;
var geoc;
var iconArr = [];
var markers = [];
var markersArray = [];
var overSettings = [];
var mapType = [BMAP_NORMAL_MAP, BMAP_SATELLITE_MAP, BMAP_PERSPECTIVE_MAP];
var pointTypeArr = ['管线', '泵站', '阀门', '流量', '水厂', '水源', '大表', '压力','调峰','水质'];
var tempIDNow;
// 页面载入时 进行 图像绘制

function drawGeoAgain(dat) {

    //  获得 数据 后的操作
    for (; ;) {
        var overlay;
        map.addOverlay();
        over.push(overlay);
    }



}


//var myIcon_qu = new BMap.Icon("./img/qu.png", new BMap.Size(30, 30));

//var myIcon_she = new BMap.Icon("./img/she.png", new BMap.Size(30, 30));
//var myIcon_fa = new BMap.Icon("./img/fa.png", new BMap.Size(30, 30));
//var myIcon_shui = new BMap.Icon("./img/dacahgn.png", new BMap.Size(30, 30));
//var myIcon_hu = new BMap.Icon("./img/hu.png", new BMap.Size(30, 30));
//var myIcon_dahu = new BMap.Icon("./img/dahu.png", new BMap.Size(30, 30));
//var myIcon_shuiyuan = new BMap.Icon("./img/shuiyuan.png", new BMap.Size(30, 30));
//var myIcon_jiaya = new BMap.Icon("./img/jiaya.png", new BMap.Size(30, 30));

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
function parseUrl() {

    var url = window.location.href;
     //alert(url);
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
    // alert(0);
    // alert(arr2);
    return arr2;


}
var v = parseUrl();
tempIDNow = v['mapTempID'];
function drawAgain(da) {

    for (var i = 0; i < da.length; i++) {
        map.addOverlay(da[i]);
        over.push(da[i]);
        if (type != 'marker') {

            $('.overlayBox').append('<div class="waaa" inner="' + id + '"><div class="nameBox" id="' + id + '  " >' + name + id + '</div><input type="text"><button title="' + id + '" class="changeName">修改名字</button></div>');

            var a = ff.getPath();
            var sColor = ff.getStrokeColor();
            var fColor = ff.getFillColor();
            var sWeight = ff.getStrokeWeight();
            var sOpacity = ff.getStrokeOpacity();
            var fOpacity = ff.getFillOpacity();
            var sStyle = ff.getStrokeStyle();

            $('.attrBoxUl').append('<li class="hide" title="' + id + '"><input type="color" ><button class="changeColor">修改颜色</button><br><input type="range" min="3" max="20"><button class="changeWeight">修改线宽</button><button title="' + id + '" class="edit">可编辑</button><button title="' + id + '" class="disEdit">关闭编辑</button><button title="' + id + '" class="delete">删除</button></li>');


        } else {
            $('.overlayBox').append('<div class="waaa" inner="' + id + '"><div class="nameBox" id="' + id + '  " >' + name + id + '</div><input type="text"><button title="' + id + '" class="changeName">修改名字</button></div>');

            var Lng = ff.getPosition().lng;
            var Lat = ff.getPosition().lat;

            ff.setIcon(myIcon_flow);
            markers.push(ff);
            $('.attrBoxUl').append('<li class="hide" title="' + id + '"><button class="setIcon" title="' + id + '">设置为开关</button><input type="text" ><button type="button" title="' + id + '" class="setFlag">设置Flag</button><button title="' + id + '" class="edit">可编辑</button><button title="' + id + '" class="disEdit">关闭编辑</button><button title="' + id + '" class="delete">删除</button></li>');

        }
    }
}

function init(type, zoom, min, max, mapSty, center) {

    //地图初始化
    var type = type ? type : BMAP_NORMAL_MAP;
    var zoom = zoom ? zoom : 14;
    var min = min ? min : 8;
    var max = max ? max : 19;
    var mapSty = mapSty ? mapSty : 'normal';//midnight hardedge
    var center = center ? center : '121.191705, 31.166028';

    map = new BMap.Map("map", { mapType: type, minZoom: min, maxZoom: max });//, { mapType: BMAP_SATELLITE_MAP }
    var point = new BMap.Point(center.split(',')[0], center.split(',')[1]);    // 创建点坐标
    map.centerAndZoom(point, zoom);                     // 初始化地图,设置中心点坐标和地图级别。
    map.enableScrollWheelZoom();
    map.setMapStyle({ style: mapSty });


    geoc = new BMap.Geocoder();

    //绘图工具初始化
    var styleOptions = {
        strokeColor: "blue",    //边线颜色。
        fillColor: "green",      //填充颜色。当参数为空时，圆形将没有填充效果。
        strokeWeight: 4,       //边线的宽度，以像素为单位。
        strokeOpacity: 0.8,	   //边线透明度，取值范围0 - 1。
        fillOpacity: 0.6,      //填充的透明度，取值范围0 - 1。
        strokeStyle: 'solid' //边线的样式，solid或dashed。
    }


    var styleOption1={
        strokeColor: "blue",    //边线颜色。
        fillColor: "#fff",      //填充颜色。当参数为空时，圆形将没有填充效果。
        strokeWeight: 1,       //边线的宽度，以像素为单位。
        strokeOpacity: 0.5,	   //边线透明度，取值范围0 - 1。
        fillOpacity: 0.6,      //填充的透明度，取值范围0 - 1。
        strokeStyle: 'dashed' //边线的样式，solid或dashed。
      }
    //icon 初始化

    //var myIcon_flow = new BMap.Icon("img/flow2.png", new BMap.Size(30, 30));

    //var myIcon_onOff = new BMap.Icon("img/onOff.png", new BMap.Size(30, 30));

   
   // iconArr.push(myIcon_qu, myIcon_she, myIcon_fa, myIcon_hu, myIcon_shui, myIcon_dahu, myIcon_jiaya, myIcon_shuiyuan);
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


    var drawingManager = new BMapLib.DrawingManager(map, {
        isOpen: false, //是否开启绘制模式
        enableDrawingTool: true, //是否显示工具栏
        drawingToolOptions: {
            anchor: BMAP_ANCHOR_TOP_LEFT, //位置
            offset: new BMap.Size(-200, 0), //偏离值
        },
        circleOptions: styleOption1, //圆的样式
          //            polylineOptions: styleOptions, //线的样式
        //              circleOptions: styleOptions, //圆的样式
        polylineOptions: styleOptions, //线的样式
        polygonOptions: styleOption1, //多边形的样式
        rectangleOptions: styleOption1  //矩形的样式
    });
   // $("#nav_left").click(function () { drawingManager._bindObjClick(); });
   

    drawingManager.addEventListener('overlaycomplete', overlaycomplete);
    $(document).delegate('.changeColor', 'click', changeStrokeColor);
    $(document).delegate('.changeWeight', 'click', changeStrokeWeight);
    $(document).delegate('.edit', 'click', editAble);
    $(document).delegate('.disEdit', 'click', editDis);
    $(document).delegate('.delete', 'click', deleteAble);
    $(document).delegate('.setIcon', 'click', setIconFlow);
    //$(document).delegate('.setFlag', 'click', setFlag);
    $(document).delegate('.changeName', 'click', changeNameFn);
    $(document).delegate('.btn', 'click', changeAttrFn);
    $(document).delegate('.closeBtn', 'click', closeFn);
    $(document).delegate('.deleteB', 'click', deleteFn);
    $(document).delegate('.editA', 'click', editA);
    $(document).delegate('.disEditD', 'click', diseditA);
     
}
$(window).load(function () {
   // init(0, 0, 0, 0, 0);

    $.ajax({
        url: '../../Service/Map_Template.ashx?method=SearchTemp',
        data: { "TempID": tempIDNow },
        success: function (data) {
           // alert('ok');
            //console.log(data);
            data = JSON.parse(data);
            data=data[0];
           
           
            var type = changMapType(data.FMapType);
            var zoom = data.FZoom;
            var min = data.FMinZoom;
            var max = data.FMaxZoom;
            var mapSty = data.FStyle;//midnight hardedge
            var center = data.FCenter;

            init(type, zoom, min, max, mapSty, center);
            getLine();
            getMarker();
            getArea();
            $('.adressSearchBtn').click(function () {
                adreessSearch();
            });
        }
    });
});



function getLine() {
    $.ajax({
        url: '../../Service/Map_Line.ashx?method=SearchLine',
        data: { "TempID": tempIDNow },
        success: function (data) {
           //alert('ok');
            console.log(data);
            data = $(JSON.parse(data));
            console.log(data);
            data.each(function (i, v) {
                var pointArrAll = [];
                var pointArr = JSON.parse(v.FLine);
                $(pointArr).each(function (ind,va) {
                   
                    var point = new BMap.Point(va.lng, va.lat);
                    pointArrAll.push(point);
                });
                //pointArrAll=
                
                //console.log(pointArrAll);
                var polyLine1 = new BMap.Polyline(pointArrAll, {
                    strokeWeight: v.FStrokeWeight,
                    strokeColor: v.FStrokeColor,
                    strokeOpacity: Number(v.FStrokeOpacity)/100,
                    strokeStyle: v.FStrokeStyle
                });
                polyLine1.Type = 'polyline';
                polyLine1.mark = v.LineID;
                polyLine1.name = v.FName;
                over.push(polyLine1);
                console.log(over);
                polyLine1.addEventListener('click',polonClick);
                map.addOverlay(polyLine1);
                // addLineNode(v.FName, v.LineID);
               
            });
            //new BMap.Point({ "lng": 121.182866, "lat": 31.182465 });
          
            //var poiArr = [{ "lng": 121.182866, "lat": 31.182465 }, { "lng": 121.254299, "lat": 31.165533 }, { "lng": 121.19882, "lat": 31.141429 }, { "lng": 121.167199, "lat": 31.155892 }, { "lng": 121.174529, "lat": 31.169365 }];
           
           
        }
    });
   

}



function getMarker() {
    $.ajax({
        url: '../../Service/Map_Marker.ashx?method=searchmarker',
        data: { "TempID": tempIDNow },
        success: function (da) {
            data = $(JSON.parse(da));
            data.each(function (i, v) {
                var va = JSON.parse(v.FMarker);
                console.log(va[0]);
                console.log(5555555555555555555555555555);
                var point = new BMap.Point(va[0].lng, va[0].lat);
       
                 var marker1 = new BMap.Marker(point);
                 over.push(marker1);
                // alert(v.FType);
                 marker1.setIcon(iconArr[v.FType]);
                 marker1.Type = 'marker';
                 marker1.mark = v.MarkerID;
                 marker1.name = v.FName;
                 marker1.FMID = v.FMID;
                 marker1.addEventListener('click', pointClick);
                 map.addOverlay(marker1);
                 //addMarkerNode(11, 2222, v.FMarker, v.FType);
            });
        

            //var mak = new BMap.Marker(new BMap.Point(121.19396, 31.165352));

            //map.addOverlay(mak);


        }
    });
}



function getArea(){
    $.ajax({
        url: '../../Service/Map_Area.ashx?method=searcharea',
        data: { "TempID": tempIDNow },
        success: function (da) {
            var data = $(JSON.parse(da));
          
            data.each(function (i, v) {
                
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
                        fillOpacity:  Number(v.FAreaOpacity)/100,
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
                    polyGon1.addEventListener('click', areaClick);
                    map.addOverlay(polyGon1);
                 
                } else if (v.FAreaType == 'circle') {
                    var pointArrAll = [];
                    [{ "lng": 121.22311, "lat": 31.178263 }, 2268.216921818377]
                    var pointArr = JSON.parse(v.FArea);

                        var point = new BMap.Point(pointArr[0].lng, pointArr[0].lat);
                        pointArrAll.push(point);
                        var radius=pointArr[1];
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
                    circle1.addEventListener('click', areaClick);
                    map.addOverlay(circle1);
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
                        fillOpacity: Number(v.FAreaOpacity)/100,
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
                    rectangle1.addEventListener('click', areaClick);
                    map.addOverlay(rectangle1);
                } 
                          
                console.log(v.FAreaType);
                console.log(over);
                console.log(1111111111111123);
            });


            //var mak = new BMap.Marker(new BMap.Point(121.19396, 31.165352));

            //map.addOverlay(mak);


        }
    });

}

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
            type = mapType[2];
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




function overlaycomplete(e) {
    console.log(e);
    var ff = e.overlay;
    var id = new Date().getTime();
    var name = "新加";
    ff.mark = id;
    ff.name = name;

    var type = e.drawingMode;
    ff.Type = type;

    if (type == 'marker') {

       

        //alert($('.BMapLib_box'));
        ff.pointerAttrArray = {
            'name': '新建' + Math.random(),//名称
            'markNumber': 0,//编号
            'position': '撒花',//位置
            'setDate': 0,//安装日期
            'fatherDomId': 0,//父节点ID
            'depth': Math.random().toFixed(2),//埋深
            'inlineNum': '22',//所在管线号
            'brand': '嗯嗯',//品牌
            'model': 'DN125',//型号
            'Maintenance': 'DN125',//维保周期
            'headwaters': 'DN125',//水源厂
            'caliber': '.8',//口径
            'collectionType': '人工采集',//   采集方式
            'color': 'red',//表示颜色
            'weight': 10
        };
        var $len = $('.BMapLib_box').length;
        var $iconIndex;
        for (var i = 0; i < $len; i++) {
            if ($('.BMapLib_box').eq(i).hasClass('active')) {
                $iconIndex = ($('.BMapLib_box').eq(i).attr('charIndex'));
                //alert(pointTypeArr[$iconIndex]);
                break;
            }
        }

        var $liLen = $('#nav_left li').length;
        var $liIconIndex;
        for (var i = 0; i < $liLen; i++) {
            if ($('#nav_left li').eq(i).hasClass('current')) {
                $liIconIndex = ($('#nav_left li').eq(i).attr('data-charIndex'));
               // alert(pointTypeArr[$liIconIndex]);
                break;
            }
        }
        var addComp;
        var address;
       
             geoc.getLocation(ff.getPosition(), function (rs) {
                addComp = rs.addressComponents;
                //alert(addComp.province + ", " + addComp.city + ", " + addComp.district + ", " + addComp.street + ", " + addComp.streetNumber);
                address= addComp.province + addComp.city + addComp.district + addComp.street + addComp.streetNumber;
                ff.address = address;
             });
       
       
       
        //alert();
        //ff.setIcon(iconArr[$iconIndex]);
        ff.setIcon(iconArr[$liIconIndex]);
        var Lng = ff.getPosition().lng;
        var Lat = ff.getPosition().lat;

       
        
        console.log(ff);
        console.log(12121221212121);
        var pointAr = [{ "lng": Lng,"lat":Lat}];
        var pointOp = {

            'ID': id,
            'lat': Lat,
            'lng': Lng,
            'pointA': pointAr,
            'infoArr': ff.pointerAttrArray,
            'iconIndex': $liIconIndex
        }

       // alert($liIconIndex);
        addMarkerNode(name, id, JSON.stringify(pointAr), $liIconIndex);
        

        $('.panelBox1 .panel-body .list-group2').append(
        '<button type="button" class="list-group-item list-group-item-info">' + id + '</button>');

        console.log(pointOp);

        overSettings.push(overOPt);

        console.log(overSettings);

        //ff.changgeName = function () {
        //    alert(this.pointerAttrArray['depth']);
        //}
        ff.addEventListener('click', pointClick);


    } else {
        if (type == 'polyline') {
            $('.BMapLib_box[charindex="undefined"]').trigger('click');
            $('#nav_left li:first').trigger('click');

            ff.polonGonAttrArray = {
                'name': '新建' + Math.random(),//名称
                'markNumber': '',//编号
                'position': '',//位置
                'setDate': 0,//安装日期
                'fatherDomId': 0,//父节点ID
                'depth': Math.random(),//埋深
                'lineNum': '',//管线ID
                'pipDia': 'DN50',//管径
                'piPStock': '铸铁',//管材
                'wallThickness': '',//壁厚
                'headwaters': '',//水源厂
                'length': '1000m',//管线长度
                'layerType': '管线',//   层类型
                'color': 'red',//表示颜色
                'weight': 10
            };

            var a = ff.getPath();
            var overOPt = {
                'ID': id,
                'pointArr': a,
                'drawType': type,
                'infoArr': ff.polonGonAttrArray
            };

            addLineNode(name, id, JSON.stringify(a));

            $('.panelBox1 .panel-body .list-group1').append(
            '<li class="list-group-item row">' +
                '<input att="' + id + '" class="col-xs-1" type="checkbox" /><label class="col-xs-11">' + id + '</label>' +
            '</li>');



            console.log(overOPt);

            overSettings.push(overOPt);
            console.log(overSettings);

            ff.addEventListener('click', polonClick);
        } else if (type == 'rectangle') {
            //var a = [];
            //var rectanglePoint = ff.Nk;
            //var start = rectanglePoint[1];
            //var end = rectanglePoint[3];
           // a.push(start,end);
           var a = ff.getPath();
            var overOPt = {
                'ID': id,
                'pointArr': a,
                'drawType': type,
                'infoArr': ff.polonGonAttrArray
            };


            console.log(ff);
            console.log(555666);
            addAreaNode(name, id, JSON.stringify(a), type);
            ff.addEventListener('click', areaClick);
            
        } else if (type == 'polygon') {
            $('.BMapLib_box[charindex="undefined"]').trigger('click');
            $('#nav_left li:first').trigger('click');
            
            var a = ff.getPath();

            var overOPt = {
                'ID': id,
                'pointArr': a,
                'drawType': type,
                'infoArr': ff.polonGonAttrArray
            };

            addAreaNode(name, id, JSON.stringify(a), type);

            ff.addEventListener('click', areaClick);
        } else if (type == 'circle') {
            
            var r = ff.getRadius();
            var a = [];
           a.push(ff.getCenter(),r);
            //var a = ff.getPath();
            var overOPt = {
                'ID': id,
                'pointArr': a,
                'drawType': type,
                'infoArr': ff.polonGonAttrArray
            };
            console.log(JSON.stringify(a));
            addAreaNode(name, id, JSON.stringify(a), type);

            ff.addEventListener('click', areaClick);
            
        }
        

    }


    if (type != 'marker') {

        $('.overlayBox').append('<div class="waaa" inner="' + id + '"><div class="nameBox" id="' + id + '  " >' + name + id + '</div><input type="text"><button title="' + id + '" class="changeName">修改名字</button></div>');

        var a = ff.getPath();
        var sColor = ff.getStrokeColor();
        //var fColor = ff.getFillColor();
        var sWeight = ff.getStrokeWeight();
        //var sOpacity = ff.getStrokeOpacity();
        //var fOpacity = ff.getFillOpacity();
        // var sStyle = ff.getStrokeStyle();

        $('.attrBoxUl').append('<li class="hide" title="' + id + '"><input type="color" ><button class="changeColor">修改颜色</button><br><input type="range" min="3" max="20"><button class="changeWeight">修改线宽</button><button title="' + id + '" class="edit">可编辑</button><button title="' + id + '" class="disEdit">关闭编辑</button><button title="' + id + '" class="delete">删除</button></li>');
        //var overOPt = {
        //    'ID': id,
        //    'pointArr': a,
        //    'drawType': type,
        //    'fColor': fColor,
        //    'sColor': sColor,
        //    'sWeight': sWeight,
        //    'sOpacity': sOpacity,
        //    'fOpacity': fOpacity,
        //    'sStyle': sStyle
        //};


        //overOPt['fColor'] = fColor;

        //overSettings.push(overOPt);

    } else {
        $('.overlayBox').append('<div class="waaa" inner="' + id + '"><div class="nameBox" id="' + id + '  " >' + name + id + '</div><input type="text"><button title="' + id + '" class="changeName">修改名字</button></div>');

        var Lng = ff.getPosition().lng;
        var Lat = ff.getPosition().lat;
        //var pointOp = {

        //    'ID': id,
        //    'lat': Lat,
        //    'lng': Lng
        //}
        var $len = $('.BMapLib_box').length;
        var $iconIndex;
        //alert($len);

        for (var i = 0; i < $len; i++) {
            if ($('.BMapLib_box').eq(i).hasClass('active')) {
                $iconIndex = ($('.BMapLib_box').eq(i).attr('charIndex'));
                break;
            }
        }

        ff.setIcon(iconArr[$iconIndex]);
        //ff.setIcon(myIcon_flow);
        //markers.push(ff);
        $('.attrBoxUl').append('<li class="hide" title="' + id + '"><button class="setIcon" title="' + id + '">设置为开关</button><input type="text" ><button type="button" title="' + id + '" class="setFlag">设置Flag</button><button title="' + id + '" class="edit">可编辑</button><button title="' + id + '" class="disEdit">关闭编辑</button><button title="' + id + '" class="delete">删除</button></li>');
        //overSettings.push(pointOp);
    }

    console.log(overSettings);

    console.log(a);
    over.push(ff);

    chang();

}

//polonClick
function polonClick() {

    if ($('.panelBox')) {
        ($('.panelBox')).remove();
    }

    for (var i = 0; i < over.length; i++) {
        if (over[i].Type == 'marker') {
            over[i].setAnimation(null);
            over[i].disableDragging();
        } else {
            over[i].disableEditing();
        }
    }
    this.enableEditing();

   // alert(3333);
    lineDrag();
   // alert(3333);

    $('.three_level li[data-mapid="' + this.mark + '"]').find('span').trigger('click');
    //var showStr = '<div class="panelBox panel panel-primary">' +
    //                '<div class="panel-heading">' + this.polonGonAttrArray['name'] + '<span class="pull-right close closeBtn bg-danger" >&times;</span></div>' +
    //                '<div class="panel-body">' +
    //                '<table class="table">' +
    //                   '<tr border=1>' +
    //                        '<td>名称</td>' +
    //                        '<td><input style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.polonGonAttrArray['name'] + '"></td>' +
    //                        '<td><button att="name" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                   '<tr border=1>' +
    //                        '<td>编号</td>' +
    //                        '<td><input style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.polonGonAttrArray['markNumber'] + '"></td>' +
    //                        '<td><button att="markNumber" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                   '<tr border=1>' +
    //                        '<td>位置</td>' +
    //                        '<td><input style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.polonGonAttrArray['position'] + '"></td>' +
    //                        '<td><button att="position" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                   '<tr border=1>' +
    //                        '<td>安装日期</td>' +
    //                        '<td><input style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.polonGonAttrArray['setDate'] + '"></td>' +
    //                        '<td><button att="setDate" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                    '<tr border=1>' +
    //                        '<td>父节点ID</td>' +
    //                        '<td><input style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.polonGonAttrArray['fatherDomId'] + '"></td>' +
    //                        '<td><button att="fatherDomId" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                   '<tr border=1>' +
    //                        '<td>埋深</td>' +
    //                        '<td><input style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.polonGonAttrArray['depth'] + '"></td>' +
    //                        '<td><button att="depth" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                   '<tr border=1>' +
    //                        '<td>管线ID</td>' +
    //                        '<td><input style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.polonGonAttrArray['lineNum'] + '"></td>' +
    //                        '<td><button att="lineNum" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                   '<tr border=1>' +
    //                        '<td>管径</td>' +
    //                        '<td><input style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.polonGonAttrArray['pipDia'] + '"></td>' +
    //                        '<td><button att="pipDia" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                   '<tr border=1>' +
    //                        '<td>管材</td>' +
    //                        '<td><input style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.polonGonAttrArray['piPStock'] + '"></td>' +
    //                        '<td><button att="piPStock" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                   '<tr border=1>' +
    //                        '<td>壁厚</td>' +
    //                        '<td><input style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.polonGonAttrArray['wallThickness'] + '"></td>' +
    //                        '<td><button att="wallThickness" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                   '<tr border=1>' +
    //                        '<td>水源厂</td>' +
    //                        '<td><input style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.polonGonAttrArray['headwaters'] + '"></td>' +
    //                        '<td><button att="headwaters" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                   '<tr border=1>' +
    //                        '<td>管线长度</td>' +
    //                        '<td><input style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.polonGonAttrArray['length'] + '"></td>' +
    //                        '<td><button att="length" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                   '<tr border=1>' +
    //                        '<td>曾类型</td>' +
    //                        '<td><input style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.polonGonAttrArray['layerType'] + '"></td>' +
    //                        '<td><button att="layerType" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                   '<tr border=1>' +
    //                        '<td>表示颜色</td>' +
    //                        '<td><input type="color" style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.polonGonAttrArray['color'] + '"></td>' +
    //                        '<td><button att="color" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                   '<tr border=1>' +
    //                        '<td>线条宽度</td>' +
    //                        '<td><input type="range" style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.polonGonAttrArray['weight'] + '"></td>' +
    //                        '<td><button att="weight" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                '</table>' +
    //                '<div class="row">' +
    //                   ' <div class="col-xs-6"><button class="editA btn btn-primary btn-block" att="' + this.mark + '">可编辑</button></div>' +
    //                   ' <div class="col-xs-6"><button class="disEditD btn btn-primary btn-block" att="' + this.mark + '">不可编辑</button></div>' +
    //                '</div><br>' +
    //                '<button class="deleteB btn btn-danger btn-block" att="' + this.mark + '">删除</button>' +
    //                '</div>' +
    //                '</div>';

    //$('body').append(showStr);
}


//areaClick

function areaClick() {
   
    for (var i = 0; i < over.length; i++) {
        if (over[i].Type == 'marker') {
            over[i].setAnimation(null);
            over[i].disableDragging();
        } else {
            over[i].disableEditing();

        }
    }

    this.enableEditing();
  

    areaDrag();
    $('.three_level li[data-mapid="' + this.mark + '"]').find('span').trigger('click');
  
}

//pointClick

function pointClick() {
    var labelTip;
    var $size = new BMap.Size(24, 0);
    labelTip = new BMap.Label('', {
        offset: $size
    });

    labelTip.setStyle({
        border: null,
        background: 'transparent'
    });

    this.setLabel(labelTip);
    var This = this;
    
    if ($('.panelBox')) {
        ($('.panelBox')).remove();
    }

    for (var i = 0; i < over.length; i++) {
        if (over[i].Type == 'marker') {
            over[i].setAnimation(null);
            over[i].disableDragging();
        } else {
            over[i].disableEditing();

        }
    }

    this.setAnimation(BMAP_ANIMATION_BOUNCE);
    this.enableDragging();
    this.addEventListener('dragstart', function (e) {
       
        this.setAnimation(null);
    });



    this.addEventListener('dragging', function (e) {

        var E = e;
        var This = this;
    
        var Poin = e.point;
        //console.log(Poin.lng);
       
      
        for (var i = 0; i < over.length; i++) {
           
            if (over[i].Type == 'polyline') {
               
                var a = BMapLib.GeoUtils.isPointOnPolyline(Poin, over[i]);
              //  console.log(a);
                if (a) {
                    //.log(Poin.lng);
                    labelTip.setStyle({
                        color: "#fff",                   //颜色
                        fontSize: "12px",               //字号
                        border: "1px solid #ffa970",                    //边
                        borderRadius: "2px",
                        height: "14px",
                        padding: "0 2px",
                        lineHeight: "14px",
                        cursor: "pointer",
                        boxShadow: "0 0 0 2px #fc7f3b",//橙色
                        backgroundColor: "#fc7f3b"
                    });
                    labelTip.setContent('松开鼠标定位在线【'+This.name+'】的'+e.point.lat+','+e.point.lng+'处');
                    labelTip.Lng = e.point.lng;
                    labelTip.Lat = e.point.lat;
                    //alert(3);
                   // alert('zzz');
                   
                    This.disableDragging();
                } 
            }
        }

       
    });

    this.addEventListener('dragend', function (e) {
        var positionPointArr = [];
        var positionPoint;
        // var content = labelTip.getContent();
        if (labelTip.Lng && labelTip.Lat) {
            positionPoint = new BMap.Point(labelTip.Lng, labelTip.Lat);
            
        } else {
            positionPoint = new BMap.Point(e.point.lng, e.point.lat);
        }
        
        positionPointArr.push(positionPoint);
        //console.log(labelTip.lng + ',' + labelTip.lat);
        labelTip.setContent(null);
        labelTip.setStyle({
            border: null,
            background: 'transparent',
            boxShadow: null,//橙色
        });
        markerDrag(This, positionPointArr);
            this.setPosition(positionPoint);
           
           
        //if(){
        
        //}
    });
    



    
    //                      console.log(ff);
    //

    $('.three_level li[data-mapid="' + this.mark + '"]').find('span').trigger('click');
    //var showStr = '<div class="panelBox panel panel-primary">' +
    //                '<div class="panel-heading">' + this.pointerAttrArray['name'] + '<span class="pull-right close closeBtn bg-danger" >&times;</span></div>' +
    //                '<div class="panel-body">' +
    //                '<table class="table">' +
    //                   '<tr border=1>' +
    //                        '<td>名称</td>' +
    //                        '<td><input style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.pointerAttrArray['name'] + '"></td>' +
    //                        '<td><button att="name" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                   '<tr border=1>' +
    //                        '<td>编号</td>' +
    //                        '<td><input style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.pointerAttrArray['markNumber'] + '"></td>' +
    //                        '<td><button att="markNumber" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                   '<tr border=1>' +
    //                        '<td>位置</td>' +
    //                        '<td><input style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.pointerAttrArray['position'] + '"></td>' +
    //                        '<td><button att="position" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                   '<tr border=1>' +
    //                        '<td>安装日期</td>' +
    //                        '<td><input style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.pointerAttrArray['setDate'] + '"></td>' +
    //                        '<td><button att="setDate" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                    '<tr border=1>' +
    //                        '<td>父节点ID</td>' +
    //                        '<td><input style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.pointerAttrArray['fatherDomId'] + '"></td>' +
    //                        '<td><button att="fatherDomId" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                   '<tr border=1>' +
    //                        '<td>埋深</td>' +
    //                        '<td><input style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.pointerAttrArray['depth'] + '"></td>' +
    //                        '<td><button att="depth" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                   '<tr border=1>' +
    //                        '<td>所在管线号</td>' +
    //                        '<td><input style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.pointerAttrArray['inlineNum'] + '"></td>' +
    //                        '<td><button att="inlineNum" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                   '<tr border=1>' +
    //                        '<td>品牌</td>' +
    //                        '<td><input style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.pointerAttrArray['brand'] + '"></td>' +
    //                        '<td><button att="brand" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                   '<tr border=1>' +
    //                        '<td>型号</td>' +
    //                        '<td><input style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.pointerAttrArray['model'] + '"></td>' +
    //                        '<td><button att="model" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                   '<tr border=1>' +
    //                        '<td>维保周期</td>' +
    //                        '<td><input style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.pointerAttrArray['Maintenance'] + '"></td>' +
    //                        '<td><button att="Maintenance" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                   '<tr border=1>' +
    //                        '<td>水源厂</td>' +
    //                        '<td><input style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.pointerAttrArray['headwaters'] + '"></td>' +
    //                        '<td><button att="headwaters" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                   '<tr border=1>' +
    //                        '<td>口径</td>' +
    //                        '<td><input style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.pointerAttrArray['caliber'] + '"></td>' +
    //                        '<td><button att="caliber" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                   '<tr border=1>' +
    //                        '<td>采集方式</td>' +
    //                        '<td><input style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.pointerAttrArray['collectionType'] + '"></td>' +
    //                        '<td><button att="collectionType" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                   '<tr border=1>' +
    //                        '<td>表示颜色</td>' +
    //                        '<td><input type="color" style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.pointerAttrArray['color'] + '"></td>' +
    //                        '<td><button att="color" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                   '<tr border=1>' +
    //                        '<td>线条宽度</td>' +
    //                        '<td><input type="range" style="border: 1px solid #ccc;background: transparent;width: 100%;height: 100%;" value="' + this.pointerAttrArray['weight'] + '" ></td>' +
    //                        '<td><button att="weight" iconId="' + this.mark + '" class="btn">修改</button></td>' +
    //                   '</tr>' +
    //                '</table>' +
    //                '<div class="row">' +
    //                   ' <div class="col-xs-6"><button class="editA btn btn-primary btn-block" att="' + this.mark + '">可移动</button></div>' +
    //                   ' <div class="col-xs-6"><button class="disEditD btn btn-primary btn-block" att="' + this.mark + '">不可移动</button></div>' +
    //                '</div><br>' +
    //                '<button class="deleteB btn btn-danger btn-block" att="' + this.mark + '">删除</button>' +
    //                '</div>' +
    //                '</div>';

   // $('body').append(showStr);
}

//hebing
function maxIn() {

    var $checkLen = $('.panelBox1 input:checked').length;
    //alert($checkLen);
    var $checkIdArr = [];
    var $allPointArr = [];
    var $polonGonAttrArray = [];
    var mark;
    if ($checkLen > 1) {
        for (var j = 0; j < $checkLen; j++) {
            $checkIdArr.push($('.panelBox1 input:checked').eq(j).attr('att'));
            console.log($checkIdArr);

        }
    } else {
        return;
    }


    //新写法  尝试

    for (var p = 0; p < $checkIdArr.length; p++) {
        for (var q = 0; q < over.length; q++) {
            if ($checkIdArr[p] == over[q].mark) {
                for (var r = 0; r < over[q].getPath().length; r++) {
                    $allPointArr.push(over[q].getPath()[r]);

                }
                //							map.removeOverlay(over[q]);
                //              			over.splice(q, 1);
            }
        }
    }
    mark = $checkIdArr[0];
    //alert(mark);
    var $nowAttArray;
    for (var s = 0; s < over.length; s++) {
        if (over[s].mark == mark) {
            $nowAttArray = over[s];

            break;
        }
    }
    //alert($checkIdArr);
    //alert($nowAttArray);
    $polonGonAttrArray = $nowAttArray.polonGonAttrArray = {
        'name': '新建' + Math.random(),//名称
        'markNumber': '',//编号
        'position': '',//位置
        'setDate': 0,//安装日期
        'fatherDomId': 0,//父节点ID
        'depth': Math.random(),//埋深
        'lineNum': '',//管线ID
        'pipDia': 'DN50',//管径
        'piPStock': '铸铁',//管材
        'wallThickness': '',//壁厚
        'headwaters': '',//水源厂
        'length': '1000m',//管线长度
        'layerType': '管线',//   层类型
        'color': 'red',//表示颜色
        'weight': 10
    };
    for (var t = 0; t < $checkIdArr.length; t++) {
        for (var u = 0; u < over.length; u++) {
            if (over[u].mark == $checkIdArr[t]) {
                map.removeOverlay(over[u]);
                over.splice(u, 1);
            }
        }
    }

    //alert(over.length);
    console.log(over);
    //				for (var i = 0; i < over.length; i++) {
    //                  for(var k=0;k<$checkIdArr.length;k++){
    //                  	//alert(over[i].mark);
    //
    //                  	if(over[i].mark==Number($checkIdArr[k])){
    //                  		for(var l=0;l<over[i].getPath().length;l++){
    //                  			$allPointArr.push(over[i].getPath()[l]);
    //
    //                  			mark=over[i].mark;
    //
    //                  			$polonGonAttrArray=over[i].polonGonAttrArray = {
    //	                        		'name': '新建' + Math.random(),//名称
    //	                        		'markNumber': '',//编号
    //	                        		'position': '',//位置
    //	                       			'setDate': 0,//安装日期
    //	                        		'fatherDomId': 0,//父节点ID
    //	                        		'depth': Math.random(),//埋深
    //			                        'lineNum': '',//管线ID
    //			                        'pipDia': 'DN50',//管径
    //			                        'piPStock': '铸铁',//管材
    //			                        'wallThickness': '',//壁厚
    //			                        'headwaters': '',//水源厂
    //			                        'length': '1000m',//管线长度
    //			                        'layerType': '管线',//   层类型
    //			                        'color': 'red',//表示颜色
    //			                        'weight': 10
    //                  			};
    //
    //                  		}
    //                  		map.removeOverlay(over[i]);
    //                  			over.splice(i, 1);
    //
    //
    //                  	}
    //
    //                  }
    //              }

    var newPolonArr = [];
    for (var m = 0; m < $allPointArr.length; m++) {
        newPolonArr.push($allPointArr[m]);
    }

    //alert($checkLen+100);
    for (var n = 0; n <= $checkLen; n++) {
        //alert(1);
        $('.panelBox1 input:checked').parent().remove();

    }

    $('.panelBox1 .panel-body .list-group1').append(
        '<li class="list-group-item row">' +
            '<input att="' + mark + '" class="col-xs-1" type="checkbox" /><label class="col-xs-11">' + mark + '</label>' +
        '</li>');

    var $color = $polonGonAttrArray['color'];
    var $strokeWeight = $polonGonAttrArray['weight'];
    console.log(newPolonArr);
    var newPolon = new BMap.Polyline(
        newPolonArr, { strokeColor: $color, strokeWeight: $strokeWeight });
    //创建折线*/
    newPolon.mark = mark;
    newPolon.polonGonAttrArray = $polonGonAttrArray;
    map.addOverlay(newPolon);
    over.push(newPolon);

    newPolon.addEventListener('click', polonClick);
    console.log(newPolon);
    console.log($allPointArr);
    console.log(over);
}

$('body').delegate('.panelBox1 .btn-bind', 'click', function () {
    maxIn();
});
$('body').delegate('.mergeIco', 'click', function () {
    maxIn();
});


//区域  点 拖动后的保存
function areaDrag() {

    for (var i = 0; i < over.length; i++) {
        if (over[i].type != 'marker') {

            over[i].addEventListener('lineupdate', function (a, b) {
                //  alert(a);

                if (this.Type == 'circle') {
                    var r = this.getRadius();
                    var lineArr = [];
                    lineArr.push(this.getCenter(), r);
                } else {
                    var lineArr = this.getPath();
                }
               
                var mapId = $('li[data-mapId="' + this.mark + '"]').attr("data-id");
              
                modifyProperty('../../Service/Map_Area.ashx?method=updareapro', mapId, 'FArea', JSON.stringify(lineArr));

            });


        }
    }
}


//折线,点拖动后保存函数

function lineDrag() {
  
    for (var i = 0; i < over.length; i++) {
        if (over[i].type != 'marker') {
           
            over[i].addEventListener('lineupdate', function (a, b) {
              //  alert(a);
                var lineArr = this.getPath();
                var mapId = $('li[data-mapId="' + this.mark + '"]').attr("data-id");

                modifyProperty('../../Service/Map_Line.ashx?method=updlinepro', mapId, 'FLine', JSON.stringify(lineArr));

            });
           
           
        } 
    }
}

function markerDrag(obj,pointStr) {
   ;
    for (var i = 0; i < over.length; i++) {
        if (over[i].Type == 'marker') {

           
                //  alert(a);
            console.log(obj.mark);
            var mapId = $('li[data-mapid="' + obj.mark + '"]').attr("data-id");
               // alert(1);
            console.log(mapId);
            modifyProperty('../../Service/Map_Marker.ashx?method=updmarkerpro', mapId, 'FMarker', JSON.stringify(pointStr));
               // alert(2);
          


        }
    }
}


//更改属性函数  特用（她）

function changeAttrFunc(id,attr,value) {
    if(!id||!attr||!value){
        return;
    }
   
    for (var i = 0; i < over.length; i++) {
        if (over[i].mark == id && over[i].type!='marker') {
            switch (attr){
                case 'FStrokeWeight' :{
                    over[i].setStrokeWeight(Number(value));
                    break;
                }
                case 'FStrokeColor' :{
                    over[i].setStrokeColor(value);
                    break;
                }
                case 'FStrokeOpacity' :{
                    over[i].setStrokeOpacity(Number(value)/100);
                    break;
                }
                case 'FStrokeStyle' :{
                    over[i].setStrokeStyle(value);
                    break;
                }
                case 'FAreaColor': {
                    over[i].setFillColor(value);
                    break;
                }
                case 'FAreaOpacity': {                  
                    over[i].setFillOpacity(Number(value) / 100);
                    break;
                }
            }
            

        }
    }
   
}

//修改颜色
function changeStrokeColor(id) {
   
    var a = id ? id : $(this).parent().attr('title');
    
  
    for (var i = 0; i < over.length; i++) {
        if (over[i].mark == a) {
            over[i].setStrokeColor($(this).prev().val());
            return;
        }
    }
}

//修改名字
function changeNameFn() {
    var a = $(this).attr('title');
    // alert(a);
    for (var i = 0; i < over.length; i++) {
        if (over[i].mark == a) {
            over[i].name = $(this).prev().val();
            // alert(over[i].name);
            $(this).prev().prev().html(over[i].name);
            //waaa

            return;
        }
    }
}
//修改填充颜色
function changeFillColor() {
    var a = $(this).parent().attr('title');
    for (var i = 0; i < over.length; i++) {
        if (over[i].mark == a) {
            over[i].setFillColor($(this).prev().val());
            return;
        }
    }
}


//更改

function changeAttrFn() {
    var m = $(this).attr('iconId');
    var n = $(this).parent().prev().children('input').val();
    var name = $(this).attr('att');
    //alert(name);
    //alert(n);

    if (name == 'color') {
        for (var i = 0; i < over.length; i++) {
            if (over[i].mark == m) {
                if (over[i].Type == 'marker') {
                    over[i].pointerAttrArray[name] = n;
                    // alert(over[i].pointerAttrArray[name]);

                } else {
                    over[i].polonGonAttrArray[name] = n;
                    //  alert(over[i].polonGonAttrArray[name]);

                    // alert(11111);
                    over[i].setStrokeColor(n);

                }

                break;
            }
        }
    } else if (name == 'weight') {
        for (var i = 0; i < over.length; i++) {
            if (over[i].mark == m) {
                if (over[i].Type == 'marker') {
                    return;

                } else {
                    over[i].polonGonAttrArray[name] = n;
                    //  alert(over[i].polonGonAttrArray[name]);

                    // alert(11111);
                    over[i].setStrokeWeight(n);

                }

                break;
            }
        }
    } else {
        for (var i = 0; i < over.length; i++) {
            if (over[i].mark == m) {
                if (over[i].Type == 'marker') {
                    over[i].pointerAttrArray[name] = n;
                    // alert(over[i].pointerAttrArray[name]);

                } else {
                    over[i].polonGonAttrArray[name] = n;
                    //  alert(over[i].polonGonAttrArray[name]);
                    //if (name = 'color') {
                    //    alert(11111);
                    //    over[i].setStrokeColor(n);
                    //}
                }



                break;
            }
        }
    }

}


//关闭按钮函数
function closeFn() {
    ($('.panelBox')).remove();
}
//左侧删除内容

function deleteFn(id) {
    // var a = $(this).attr('att');
    var a = id;
    //alert(a);
    for (var i = 0; i < over.length; i++) {
        if (over[i].mark == a) {
            if (over[i].Type == 'marker') {
                $('.panelBox1 .list-group *[att="a"]').remove();
                //alert(111);
            } else {
                $('.panelBox1 .list-group *[att="a"]').parent().remove();
            }
            map.removeOverlay(over[i]);
            over.splice(i, 1);
            ($('.panelBox')).remove();


            chang();
            return;
        }
    }
}

//线宽可调整
function weightChange() {
    var a = $(this).attr('att');

    for (var i = 0; i < over.length; i++) {
        if (over[i].mark == a && over[i].Type != 'marker') {
            over[i].setStrokeWeight($(this).prev().val());
            //alert($(this).prev().val());

            return;
        }
    }
}

//编辑或移动
function editA() {
    var a = $(this).attr('att');
    for (var i = 0; i < over.length; i++) {
        if (over[i].mark == a && over[i].Type != 'marker') {

            over[i].enableEditing();

            return;
        } else if (over[i].mark == a && over[i].Type == 'marker') {
            over[i].enableDragging();

            return;
        }

    }
}
//取消编辑 或移动
function diseditA() {
    var a = $(this).attr('att');
    for (var i = 0; i < over.length; i++) {
        if (over[i].mark == a && over[i].Type != 'marker') {
            over[i].disableEditing();

            return;
        } else if (over[i].mark == a && over[i].Type == 'marker') {
            over[i].disableDragging();

            return;
        }

    }
}
//修改图片样式
function changePic() {
    var a = $(this).parent().attr('title');
    for (var i = 0; i < over.length; i++) {
        if (over[i].mark == a && over[i].Type == 'marker') {

            over[i].setIcon();
            return;
        }
    }
}


//修改线宽
function changeStrokeWeight() {

    var a = $(this).parent().attr('title');

    for (var i = 0; i < over.length; i++) {
        if (over[i].mark == a) {
            over[i].setStrokeWeight($(this).prev().val());
            //alert($(this).prev().val());

            return;
        }
    }
}
function editDis() {
    var a = $(this).attr('title');
    for (var i = 0; i < over.length; i++) {
        if (over[i].mark == a && over[i].type != 'marker') {

            over[i].disableEditing();

            return;
        }

    }
}
function editAble() {
    var a = $(this).attr('title');
    for (var i = 0; i < over.length; i++) {
        if (over[i].mark == a && over[i].type != 'marker') {

            over[i].enableEditing();

            return;
        }

    }
}
function deleteAble() {
    //alert(111);
    var a = $(this).attr('title');
    //alert(a);
    for (var i = 0; i < over.length; i++) {
        if (over[i].mark == a) {
            map.removeOverlay(over[i]);
            $('.waaa[inner=' + over[i].mark + ']').remove();
            $('.attrBoxUl li[title=' + over[i].mark + ']').remove();
            over.splice(i, 1);
            return;
        }
    }
}

function setIconFlow() {
    var a = $(this).attr('title');

    for (var i = 0; i < over.length; i++) {
        if (over[i].mark == a) {
            if (over[i].Type == 'marker') {
                // alert(over[i].getTitle());
                over[i].setIcon(myIcon_onOff);
                return;
            }


        }
    }
}

//
function chang() {
    for (var i = 0; i < over.length; i++) {
        console.log(over[i]);

        (function (i) {
            over[i].onclick = function () {
                //alert(1);
                $('.attrBoxUl li[title=' + over[i].mark + ']').removeClass('hide').siblings().addClass('hide');
                // var path = over[i].getPath();
                // console.log(path);
            }
        })(i);

        //$('.colorInp').each(function () {
        //    $(this).minicolors();
        //});
    }
}

//地址搜索
function adreessSearch() {
    var $search = $('.adressSearchTxt').val();
   
    var local = new BMap.LocalSearch(map, {
        renderOptions: { map: map, panel: "searchResult" }
    });
    local.search($search);
}

$('body').delegate('.BMapLib_Drawing .BMapLib_box', 'click', function () {

    if ($(this).hasClass('active')) {

        return;

    } else {

        //		alert($(this).index());
        $(this).addClass('active').siblings().removeClass('active');

    }
});

$('body').delegate('#nav_left li', 'click', function () {

    if ($(this).hasClass('current')) {

        return;

    } else {

        //		alert($(this).index());
        $(this).addClass('current').siblings().removeClass('current');

    }
});

