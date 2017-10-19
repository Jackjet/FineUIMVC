function mapInit(type,zoom,maxZo,minZo,themeStyle,center){
	var type=type?type:BMAP_NORMAL_MAP;
	var zoom=zoom?zoom:17;
	var max = maxZo ? maxZo : 19;
	var min = minZo ? minZo : 7;
	var themeStyle = themeStyle ? themeStyle : 'normal';
	//var mapType1 = new BMap.MapTypeControl({ mapTypes: [BMAP_NORMAL_MAP, BMAP_SATELLITE_MAP] });//, BMAP_HYBRID_MAP
	var center=center?center:'121.191705, 31.166028';
	map=new BMap.Map("map",{mapType: type,maxZoom: max,minZoom:min,enableMapClick:false});
	    map.centerAndZoom(new BMap.Point(center.split(',')[0],center.split(',')[1]),zoom);
		map.enableScrollWheelZoom();
		map.setMapStyle({ style: themeStyle });
		//map.addControl(mapType1);

		 panorama = new BMap.Panorama('map');
		panorama.setPov({ heading: -40, pitch: 6 });

		var styleOptions = {
		    strokeColor: "#000",    //������ɫ��
		    fillColor: "#fff",      //�����ɫ��������Ϊ��ʱ��Բ�ν�û�����Ч����
		    strokeWeight: 3,       //���ߵĿ�ȣ�������Ϊ��λ��
		    strokeOpacity: .6,	   //����͸���ȣ�ȡֵ��Χ0 - 1��
		    fillOpacity: 0.6,      //����͸���ȣ�ȡֵ��Χ0 - 1��
		    strokeStyle: 'dashed' //���ߵ���ʽ��solid��dashed��
		}


	drawingManager = new BMapLib.DrawingManager(map, {
		    isOpen: false, //�Ƿ�������ģʽ
		    enableDrawingTool: true, //�Ƿ���ʾ������
		    drawingToolOptions: {
		        anchor: BMAP_ANCHOR_TOP_LEFT, //λ��
		        offset: new BMap.Size(-400, 0), //ƫ��ֵ
		    },
		    circleOptions: styleOptions, //԰����ʽ
		    polygonOptions: styleOptions, //����ε���ʽ
		    rectangleOptions: styleOptions //���ε���ʽ
		});


		drawingManager.addEventListener('overlaycomplete', overlaycomplete);

}


