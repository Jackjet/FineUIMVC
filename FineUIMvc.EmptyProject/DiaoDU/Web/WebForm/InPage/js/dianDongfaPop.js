var $TiaoFengKaiSet;
$(function () {
	//dianDongFaClickOut(3);
    dianDongFaCloseBtnClick();
	
    dianDOngFaNavClickFn();
    dianDOngFaKaiSetFn();
});





//页面  开度设置 按钮的 函数
function dianDOngFaKaiSetFn() {
    $('.dianDongKaiduBtn').click(function () {

        var val = $.trim($('.dianDongKaiduZhi').val());

        if (val != '') {
            //alert(val);
            if (Number(val) < 0) {
                alert("请输入大于0的数字！");
                $('.dianDongKaiduZhi').val('');
            } else if (Number(val) > 100) {
                alert("请输入小于100的数字！");
                $('.dianDongKaiduZhi').val('');
            } else if (Number(val) >= 0 && Number(val) <= 100) {
                $TiaoFengKaiSet.animate(val);
                $('.dianDongKaiduZhi').val('');
            } else {
                alert("请输入合法开度！");
                $('.dianDongKaiduZhi').val('');
            }
            
        } else {
            alert('请输入开度！');
        }

    });
}



//页面点击   出现  流量pop框  

function dianDongFaClickOut(len){
	
		if($('.dianDongFaBox').hasClass('fadeIn')){
			$('.dianDongFaBox').removeClass('fadeIn animated');
			$('.dianDongFaBox').addClass('fadeOut animated');	
			$('.dianDongFaBox').css('marginTop','-1000888px');	
		}else {
			
			$('.dianDongFaBox').css('marginTop','-279px');
			$('.dianDongFaBox').removeClass('fadeOut animated');	
			$('.dianDongFaBox').addClass('fadeIn animated');	
		}
		
   
		 $TiaoFengKaiSet = $TiaoFengKaiSet|| $('.dianDongKaiduRadia').radialIndicator({
	        radius: 46,
	        barWidth: 6,
	        barBgColor: '#0b5d8e',//'#21a0ff',
	        barColor: '#f5f208',//'#fff900',
	        initValue: 40,
	        roundCorner: true,
	        percentage: true
	    }).data('radialIndicator');
		

		

	    //  
		
	    
	    if(len>3){
	        TweenLite.set(".dianDongFaBox .cotentLeftNav .info", { autoAlpha: 0, scale: 0.4, });
	        TweenLite.set(".dianDongFaBox .cotentLeftNav .baobiao", { autoAlpha: 0, scale: 0.4, });
	        TweenLite.set(".dianDongFaBox .cotentLeftNav .baojing", { autoAlpha: 0, scale: 0.4, });
	        TweenLite.set(".dianDongFaBox .cotentLeftNav .detail", { autoAlpha: 0, scale: 0.4, });
	    	var tl = new TimelineLite();
	        tl.to(".dianDongFaBox .cotentLeftNav .detail", 0.4, { autoAlpha: 1, scale: 1, ease: Back.easeInOut })
              .to(".dianDongFaBox .cotentLeftNav .info", 0.4, { autoAlpha: 1, scale: 1, ease: Back.easeInOut })
              .to(".dianDongFaBox .cotentLeftNav .baobiao", 0.4, { autoAlpha: 1, scale: 1, ease: Back.easeInOut })
              .to(".dianDongFaBox .cotentLeftNav .baojing", 0.4, { autoAlpha: 1, scale: 1, ease: Back.easeInOut })
              
	        $(".dianDongFaBox .cotentLeftNav .detail").addClass('active');
	        $('.dianDongFaBox .bRightUl.detail').addClass('active bounceInUp animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
	            $('.dianDongFaBox .bRightUl.detail').removeClass('bounceInUp animated');
	        });

	    	
	    }else {
	        TweenLite.set(".dianDongFaBox .cotentLeftNav .info", { autoAlpha: 0, scale: 0.4, });
	        TweenLite.set(".dianDongFaBox .cotentLeftNav .baobiao", { autoAlpha: 0, scale: 0.4, });
	        TweenLite.set(".dianDongFaBox .cotentLeftNav .baojing", { autoAlpha: 0, scale: 0.4, });
	        TweenLite.set(".dianDongFaBox .cotentLeftNav .detail", { autoAlpha: 0, scale: 0.01,height: 0 });
	    	var tl = new TimelineLite();
	    	tl.to(".dianDongFaBox .cotentLeftNav .info", 0.4, { autoAlpha: 1, scale: 1, ease: Back.easeInOut })
              .to(".dianDongFaBox .cotentLeftNav .baobiao", 0.4, { autoAlpha: 1, scale: 1, ease: Back.easeInOut })
              .to(".dianDongFaBox .cotentLeftNav .baojing", 0.4, { autoAlpha: 1, scale: 1, ease: Back.easeInOut })

	    	$(".dianDongFaBox .cotentLeftNav .info").addClass('active');
	    	$('.dianDongFaBox .bRightUl.info').addClass('active bounceInUp animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
	    	    $('.dianDongFaBox .bRightUl.info').removeClass('bounceInUp animated');
	    	});
	    	
	    }
	    
	    
}



//电动阀框 中的 数据 处理函数
function dianDongFaDataUse(data,pId){
	
	 
}
//关闭按钮 点击 函数

function  dianDongFaCloseBtnClick(){
	
	$('.dianDongFaBox .closeBox').click(function(){		
		$('.dianDongFaBox').removeClass('fadeIn animated');
		$('.dianDongFaBox').addClass('fadeOut animated');	
		$('.dianDongFaBox').css('marginTop','-1000888px');
		
		$(".dianDongFaBox .cotentLeftNav .bLeftNavItem").removeClass('active');
		$('.dianDongFaBox .bRightUl').removeClass('active');
		//$('.dianDongFaBox .bRightUl').css('opacity', 0);
	});
}


//  nav 的点击 切换函数

function dianDOngFaNavClickFn() {
    $('.dianDongFaBox .bContentBox .cotentLeftNav .bLeftNavItem').click(function () {
		var index=$(this).index();
		if($(this).hasClass('active')){
			$(this).siblings().removeClass('active');			
			return;
		}
		
		$(this).addClass('active').siblings().removeClass('active');
		
		$('.dianDongFaBox .bRightUl').removeClass('active');
		
		$('.dianDongFaBox .bRightUl').eq(index).addClass('active');
		
	});
}




//电动阀 detail 的 数据使用

function dianDongFaDetailUse(data,pJSON) {
    var addTime = data.updateTime ? (data.updateTime).replace('T', ' ') : '无';
    $('.dianDongFaBox .bRightContentWrap .detail .caiJiTime').html(addTime);
    //$('.dianDongFaBox .bRightContentWrap .pumpName').html(data.FName);
   // $('.dianDongFaBox .bRightContentWrap .pumpNumber').html('');


    $('.dianDongFaBox .bRightContentWrap .detail .inPress').html((data.categary.inPress || (data.categary.inPress == 0) ? data.categary.inPress : ' 无'));
    $('.dianDongFaBox .bRightContentWrap .detail .outPress').html((data.categary.outPress || (data.categary.outPress == 0) ? data.categary.outPress : '无 '));
    $('.dianDongFaBox .bRightContentWrap .detail .yaLiSheDing').html((data.categary.yaLiSheDing || (data.categary.yaLiSheDing == 0) ? data.categary.yaLiSheDing : '无 '));
    $('.dianDongFaBox .bRightContentWrap .detail .faMenKaiDu').html((data.categary.faMenKaiDu || (data.categary.faMenKaiDu == 0) ? data.categary.faMenKaiDu : '无 '));
    $('.dianDongFaBox .bRightContentWrap .detail .leiJiLiuLiang').html((data.categary.leiJiLiuLiang || (data.categary.leiJiLiuLiang == 0) ? data.categary.leiJiLiuLiang : '无 '));
    

    var kaidu=data.categary.faMenKaiDu || (data.categary.faMenKaiDu == 0) ? data.categary.faMenKaiDu : '无 ';
    
    $TiaoFengKaiSet.animate(kaidu);


    dianDongFaInfoGet(pJSON);

    dianDongFaReportList(pJSON);
    diaoDongFabaoBiaoList(reportFM);

}



//电动阀 info 的获取

function dianDongFaInfoGet(pJSON) {
    $.ajax({
        url: '/V_CDJK/FieldShow',
        cache: false,
        data: {
            tableName: 'BASE_YALI',
        },
        success: function (data) {
            var dataJSON = JSON.parse(data);
            var dataJSONList = dataJSON.obj;

            dianDongFaInfoUse(dataJSONList, pJSON);
            console.log(dataJSON);
            console.log('haoqinqin');
        },
        error: function (data) { }
    });
}


//电动阀  info 信息的使用

function dianDongFaInfoUse(arr, pJSON) {
    if (arr.length < 1) {
        return;
    }

    var str = '';
    $(arr).each(function (ind, val) {
        var d = (pJSON[0][val.FField] || (pJSON[0][val.FField] == 0)) ? pJSON[0][val.FField] : '';
        
        str += '<li class="bRightItem clearfix" data-name="' + val.FField + '" title="' + d + '">' +
                      '    <div class="bRightItemName" >' + val.FName + '</div>' +
                      '    <div class="bRightItemNumber" >' + d + ' </div>' +
                      '</li>';
    });

    $('.dianDongFaBox .bContentBox .bRightContentWrap  .bRightUl.info').empty().append(str);
}




//报表  list 生成函数

function diaoDongFabaoBiaoList(rArr) {
    $(".dianDongFaBox .baoBiaoBox .baoBiaoUl").html('');
    var str = '';
    for (var i = 0; i < rArr.length; i++) {
        str += '<li class="baoBiaoListItem clearfix" data-url="' + rArr[i].url + '"><span class="leftBG"></span class="rightName">' + rArr[i].name + '<span></span></li>';

    }
    $(".dianDongFaBox .baoBiaoBox .baoBiaoUl").append(str);
}

//报表  list 的 图表点击 函数

function dianDongFaReportList(currentPointJSON) {

    $(".dianDongFaBox .baoBiaoBox .baoBiaoUl").on("click", "li", function () {

        var baseId = currentPointJSON[0].BaseID;
        var dtuCode = currentPointJSON[0].FDTUCode;
        var FlowName = currentPointJSON[0].FName;
        var TFname = encodeURIComponent(FlowName);
        var urlStr = $(this).attr("data-url");

        if (!urlStr) {
            alert('暂无');
            return;

        }

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

        var index = layer.open({
            type: 2,
            anim: 3,
            id:13,
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