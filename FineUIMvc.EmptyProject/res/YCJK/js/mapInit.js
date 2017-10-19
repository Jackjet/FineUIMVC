function mapInit(type, zoom, maxZo, minZo, themeStyle, center) {
	var type=type?type:BMAP_NORMAL_MAP;
	var zoom=zoom?zoom:17;
	var max = maxZo ? maxZo : 19;
	var min = minZo ? minZo : 6;
	var themeStyle = themeStyle ? themeStyle : 'light';
	//var mapType1 = new BMap.MapTypeControl({ mapTypes: [BMAP_NORMAL_MAP, BMAP_SATELLITE_MAP] });//, BMAP_HYBRID_MAP
	var center=center?center:'121.191705, 31.166028';
	    map=new BMap.Map("map",{mapType: type,maxZoom: max,minZoom:min});
	    map.centerAndZoom(new BMap.Point(center.split(',')[0],center.split(',')[1]),zoom);
		map.enableScrollWheelZoom();
		map.setMapStyle({ style: themeStyle });
		//map.addControl(mapType1);

		panorama = new BMap.Panorama('map');
		panorama.setPov({ heading: -40, pitch: 6 });

		var styleOptions = {
		    strokeColor: "#000",    //边线颜色。
		    fillColor: "#fff",      //填充颜色。当参数为空时，圆形将没有填充效果。
		    strokeWeight: 3,       //边线的宽度，以像素为单位。
		    strokeOpacity: .6,	   //边线透明度，取值范围0 - 1。
		    fillOpacity: 0.6,      //填充的透明度，取值范围0 - 1。
		    strokeStyle: 'dashed' //边线的样式，solid或dashed。
		}


	drawingManager = new BMapLib.DrawingManager(map, {
		    isOpen: false, //是否开启绘制模式
		    enableDrawingTool: true, //是否显示工具栏
		    drawingToolOptions: {
		        anchor: BMAP_ANCHOR_TOP_LEFT, //位置
		        offset: new BMap.Size(-400, 0), //偏离值
		    },
		    circleOptions: styleOptions, //园的样式
		    polygonOptions: styleOptions, //多边形的样式
		    rectangleOptions: styleOptions //矩形的样式
		});


		drawingManager.addEventListener('overlaycomplete', overlaycomplete);

}


