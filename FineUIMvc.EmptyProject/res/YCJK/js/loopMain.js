$(function () {

	loopMainScroll();
	justifyLoopItemSize();
	pinDing();
	$(window).resize(function(){
		justifyLoopItemSize();
	});
    
	
	

	


	lunXunDataGet2();
	//lunXundataGet();
	//lunXunDateDealFn();


	rightSearchClickFn();
	rightOnOffAllFn();
	setJzId();


	nextClickFn();
	clearInterval(lunBoTimer);
	lunBoTimer = setInterval(function () {
	    lunXundataLunBoFn();
	}, 1000*60 );

	clearInterval(lunXunGetNewTimer);
	lunXunGetNewTimer = setInterval(function () {
	    lunXunGetNewUseFn();
	}, 1000 * 6);
});


var $jzName = '';
var $jzId = '';
var maxItemNum = 8;
var pageIndex = 0;



var nextClickOnOff = true;
var count = 0;
var yeNum = 0;
var lunBoTimer;
var lunXunGetNewTimer;
var lunXunDataBigArr = [];
var lunXunDataBiaoZhuBigArr = [];
var lunXunData = [];

var lunXunBiaoZHuData = [];


//调整loopItem size
function justifyLoopItemSize(){
	var $lW=$('.loopMain').width();
	if($lW>=1440){
		var fourItemW=1400;
		var $margin = ($lW - fourItemW) / 8;
		$('.loopItem').css({ 'marginLeft': $margin, 'marginRight': $margin });
		maxItemNum = 8;
		pageIndex = 0;
	}else if($lW<1440){
		var threeItemW=1050;
		var $margin=($lW-threeItemW)/6;
		$('.loopItem').css({ 'marginLeft': $margin, 'marginRight': $margin });
		maxItemNum = 9;
		pageIndex = 0;
		
		
	}
}



//loopMain  的滚动条

function loopMainScroll(){
	$('.loopMain').mCustomScrollbar({
        scrollbarPosition: "inside",
        theme: "minimal-dark"
    });
}



//pin  钉子的点击函数

function pinDing(){
	$('body').delegate('.loopItemContainer .loopPin','click',function(){
		if($(this).hasClass('pin')){
			$(this).removeClass('pin');
			$(this).prev().css('display', 'none');
			$(this).parent().parent().attr('data-fixedId', '');

			
			editLunxun($(this).parent().parent().index() + 1,'');

		}else {
			$(this).addClass('pin');
			$(this).prev().css('display', 'block');
			$(this).parent().parent().attr('data-fixedId', $(this).parent().parent().attr('data-loopid'));

			var jzID = $(this).parent().parent().attr('data-loopid');
			editLunxun($(this).parent().parent().index() + 1, jzID);
		}
		
	});
}

//页面 载入时  根据  屏幕分辨率  判断显示 的  块 数量 显示  数据

function lunXunNumByPixs() {
    switch( maxItemNum){
        case 8: {
            lunXundataGet();
            break;
        }
        case 9: {
            break;
        }
        default: {

            break;
        }
    }
}



// 页面 载入时    数据获取

function lunXunDataGet2(jzName, jzState) {
   var jzName = jzName ? jzName : '';
   var jzState = jzState ? jzState : '';
  
    $.ajax({
        url: '/V_YCJK/NoLunXunSelect',
        
        data: {
            'JZName': jzName,
            'State': jzState,
            'pageIndex': pageIndex,
            'pageSize': maxItemNum
        },
        success: function (data) {
         //   alert(data);
            var dataJSON = JSON.parse(data);
            count = dataJSON.count;
            yeNum = Math.ceil(count / maxItemNum);
            if (pageIndex == (yeNum-1)) {
                pageIndex = -1;
               // alert(pageIndex);
            }
            dataJSON = dataJSON.obj;
            lunXunData = dataJSON;
            
            lunXunDataBigArr = lunXunDataBigArr.concat(lunXunData);
         //   console.log(lunXunDataBigArr);
          //  console.log('qin123');
            if (lunXunDataBigArr.length < maxItemNum) {
                lunXunDataBigArr.length = maxItemNum;
            }
            
          //  alert(lunXunDataBigArr.length);
            //alert(1);
            //console.log(lunXunDataBigArr);
            //console.log(2222);
           // lunXunDataBigArr = lunXunDataBigArr.slice(1);
         
            // lunxunDataUse(lunXunData);
           // alert(lunXunDataBigArr);

            lunXundataGet();  //更改
           
        },
        error: function (data) {
            console.log('');
        }
    });
}


//根据  大数组数据 对 页面 显示 进行  处理的  函数
function lunXunDateDealFn() {
    // alert(lunXunDataBigArr);
    //alert(lunXunDataBiaoZhuBigArr.length);
    //console.log(lunXunDataBiaoZhuBigArr);
    //console.log('------');
    //lunXunDataBiaoZhuBigArr.shift();
   

   // console.log(lunXunDataBiaoZhuBigArr.length);
  //  console.log('------');
    var biaoZhuIndex = [];

    for (var i = 0; i < lunXunDataBiaoZhuBigArr.length; i++) {
       // alert(lunXunDataBiaoZhuBigArr[i]);
        // alert(lunXunDataBiaoZhuBigArr[i] != 'undefined');

        if (lunXunDataBiaoZhuBigArr[i] != undefined) {
            
            if (lunXunDataBigArr[i] != undefined) {
            lunXunDataBigArr.splice(i, 0, lunXunDataBiaoZhuBigArr[i]);
                //lunXunDataBigArr.length += 1;
            } else {
                lunXunDataBigArr.splice(i, 1, lunXunDataBiaoZhuBigArr[i]);
            }
            
            biaoZhuIndex.push(i);
            //console.log(lunXunDataBigArr);
            //console.log(lunXunDataBiaoZhuBigArr[i]);
            //console.log(333);
        }
    }



  

    //alert(lunXunDataBigArr);
   
    //截取 大数组 的 前边 几条使用
  
    lunXunDataBigArrInUse = lunXunDataBigArr.slice(0, maxItemNum);
    lunXunDataBigArr=lunXunDataBigArr.slice(maxItemNum);

    console.log(lunXunDataBigArrInUse);
    console.log(lunXunDataBigArr);
    console.log(321);
   
    lunxunDataUse(lunXunDataBigArrInUse);
    lunXunBiaoZhudataUse(biaoZhuIndex);
   

  
}

//页面载入数据   时候的  显示  处理

function lunxunDataUse(currentPumpData) {
    var itemLen = 0;
    var dataJsLen = currentPumpData.length;
    //if (dataJsLen < maxItemNum) {
    //    itemLen = dataJsLen;
    //} else {
    //    itemLen = maxItemNum;
    //}
    itemLen = dataJsLen;

    //alert(currentPumpData);
   // alert(3);
    var str = '';
    for (var i = 0; i < itemLen; i++) {

            var typeStr;
            var pumpArr = [];
            var pumpRunTime = [];
            var pumpDianArr = [];

        if (currentPumpData[i] == undefined) {
           // str += '<li class="loopItem" data-pumpId="" data-loopid="" data-fixedId="" data-FField="' + (i + 1) + '"></li>';
            continue;
        }
        
        var pumpAux = currentPumpData[i].Auxiliarypumpcount;
        var pumpRun = currentPumpData[i].RunPumpNum
        var pumpJZNum = pumpAux + pumpRun;
        var outNum = currentPumpData[i].DrainPumpNum;
        var machineType = $.trim(currentPumpData[i].MachineType);

        var currentPumpStateArr = currentPumpData[i].D_Data[0];

        
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
        var totalDl= currentPumpStateArr['FTotalDL'];

        var bianPinPinlv= currentPumpStateArr['F41014'];

        if (currentPumpStateArr['TempTime']) {
            var caiJITime = currentPumpStateArr['TempTime'].replace(/[^0-9]/ig, "");
        }


        if (machineType == '0') {
            typeStr = 'guan';

    
        } else if (machineType == '1') {
            typeStr = 'xiang';
  
        } else if (machineType == '2') {
            typeStr = ' ';
           
        } else if (machineType == '1') {
        }


        var status = '';
       
        switch (currentPumpData[i].IsAlarm) {
            case 0: {
                switch (currentPumpData[i].FOnLine) {
                    case 0: {
                        status = 'offline';
                        break;
                    }
                    case 1: {
                        status = 'online';
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

      
        var stateDealArr = [];

        for (var s = 0; s < pumpArr.length; s++) {
            if (pumpArr[s] == '0' || pumpArr[s] == '1') {
                stateDealArr.push('offline');
            } else if (pumpArr[s] == '2') {
                stateDealArr.push('online');
            } else if (pumpArr[s] == '3') {
                stateDealArr.push('error');
            } else {
                stateDealArr.push('offline');
            }
        }
        var strLi ='';
        for (var j = 0; j < pumpJZNum; j++) {
            strLi += '   <li class="yelunItem"> ' +
                     '    <div class="yelunLine"></div>   ' +
                     '    <div class="yelunStatus ' + stateDealArr [j]+ '"></div> ' +
                       '</li>                               ';
        }
        
        str += '<li class="loopItem" data-pumpId="' + currentPumpData[i].PumpId + '" data-loopid="' + currentPumpData[i].pumpJZID + '" data-fixedId="" data-FField="' + (i+1) + '">  ' +
            '<div class="loopHeader clearfix">                          '+
            '    <div class="pumpStusPic '+status+'"></div>                        '+
            '    <div class="pumpNameTxt">' + currentPumpData[i].PumpJZName + '</div>           ' +
            '</div>                                                     '+
            '<div class="loopPinAndSet clearfix">                       '+
            '    <div class="loopSet"></div>                            '+
            '    <div class="loopPin"></div>                            '+
            '</div>                                                     '+
            '<div class="loopContentBox ' + typeStr + '">                               ' +
            '    <div class="inNum">' + inPress + '</div>                          ' +
            '    <div class="outNum">' + outPress + '</div>                         ' +
            '    <div class="timeBox">' + changeTime(caiJITime) + '</div>         ' +
            '    <ul class="yelunStatusBox clearfix">                   ' + strLi+
            '    </ul>                                ' +
            '</div>                                                     '+
            '<div class="loopLiuLiangAndDian clearfix">                 '+
            '    <div class="loopLIuliang clearfix">                    '+
            '        <div class="liuLiangPic"></div>                    '+
            '        <div class="liuLiangNumBox">总流量：<span class="liuLiangNum">' + outLeiJiLiu + '</span></div>      ' +
            '    </div>                                                                                    '    +
            '    <div class="loopDianLiang clearfix">                                                      '    +
            '        <div class="dianLiangPic"></div>                                                      '    +
            '        <div class="dianLiangNumBox">总电量：<span class="dianLiangNum">' + totalDl + '</span></div>   ' +
            '    </div>                                                                                    '    +
            '    <div class="loopBianPin clearfix">                                                        '    +
            '        <div class="bianPinPic"></div>                                                        '    +
            '        <div class="bianPinNumBox">变频器频率：<span class="bianPinNum">' + bianPinPinlv + '</span></div>         ' +
            '    </div>                                                                                    '    +
            '</div>'+                                                                          
        '</li>';


    }

    $('.loopItemContainer').html(str);

    var $lW = $('.loopMain').width();
    if ($lW >= 1440) {
        var fourItemW = 1400;
        var $margin = ($lW - fourItemW) / 8;
        $('.loopItem').css({ 'marginLeft': $margin, 'marginRight': $margin });
    } else if ($lW < 1440) {
        var threeItemW = 1050;
        var $margin = ($lW - threeItemW) / 6;
        $('.loopItem').css({ 'marginLeft': $margin, 'marginRight': $margin });
    }

    lunXunDataBigArr = lunXunDataBigArr.slice(8);

}

//  页面载入时  有标注轮询数据的 获取


function lunXundataGet() {
    $.ajax({
        url: '/V_YCJK/LunXunSelect',
        
        success: function (data) {
            // alert(data);
            var dataJSON = JSON.parse(data);
            dataJSON = dataJSON.obj;
            lunXunBiaoZHuData = dataJSON;
            lunXunDataBiaoZhuBigArr = [];
            $(lunXunBiaoZHuData).each(function (ind, val) {              
                lunXunDataBiaoZhuBigArr[val.FField]=(val.pumpJZ[0]);
            });
            //console.log(lunXunDataBiaoZhuBigArr);
           // alert(lunXunDataBiaoZhuBigArr);
            //console.log(2);
            //console.log(3344);
            //lunXundataUse(lunXunBiaoZHuData);


            lunXunDateDealFn();//更改
        },
        error: function (data) {
            console.log('');
        }
    });
}


//页面载入时  有标注轮询数据的 处理  
function lunXunBiaoZhudataUse(lunXunBiaoZhuArr) {

    for (var i = 0; i < lunXunBiaoZhuArr.length; i++) {
        if ($('.loopItem[data-FField=' + (lunXunBiaoZhuArr[i]+1) + ']')) {
            var $This = $('.loopItem[data-FField=' + (lunXunBiaoZhuArr[i] + 1) + ']');
            //alert(i);
            $This.find('.loopPin').addClass('pin');
            $This.find('.loopSet').css('display', 'block');
            $This.attr('data-fixedId', $This.attr('data-loopid'));

        }
    }
   

    //$(lunXunBiaoZhuArr).each(function (index, value) {

    //    var pumpAux = lunXunBiaoZhuArr[index].pumpJZ[0].Auxiliarypumpcount;
    //    var pumpRun = lunXunBiaoZhuArr[index].pumpJZ[0].RunPumpNum;
    //    var pumpJZNum = pumpAux + pumpRun;
    //    var outNum = lunXunBiaoZhuArr[index].pumpJZ[0].DrainPumpNum;
    //    var machineType = $.trim(lunXunBiaoZhuArr[index].pumpJZ[0].MachineType);

    //    var currentPumpStateArr = lunXunBiaoZhuArr[index].pumpJZ[0].D_Data[0];
       
    //    var typeStr;
    //    var pumpArr = [];
    //    var pumpRunTime = [];
    //    var pumpDianArr = [];
    //    pumpArr[0] = currentPumpStateArr['F41008'];
    //    pumpArr[1] = currentPumpStateArr['F41009'];
    //    pumpArr[2] = currentPumpStateArr['F41010'];
    //    pumpArr[3] = currentPumpStateArr['F41011'];
    //    pumpArr[4] = currentPumpStateArr['F41012'];
    //    pumpArr[5] = currentPumpStateArr['F41013'];

    //    pumpDianArr[0] = currentPumpStateArr['F41045'];
    //    pumpDianArr[1] = currentPumpStateArr['F41046'];
    //    pumpDianArr[2] = currentPumpStateArr['F41047'];
    //    pumpDianArr[3] = currentPumpStateArr['F41048'];
    //    pumpDianArr[4] = currentPumpStateArr['F41049'];
    //    pumpDianArr[5] = currentPumpStateArr['F41050'];

    //    pumpRunTime[0] = currentPumpStateArr['F41051'];
    //    pumpRunTime[1] = currentPumpStateArr['F41052'];
    //    pumpRunTime[2] = currentPumpStateArr['F41053'];
    //    pumpRunTime[3] = currentPumpStateArr['F41054'];
    //    pumpRunTime[4] = currentPumpStateArr['F41055'];
    //    pumpRunTime[5] = currentPumpStateArr['F41056'];

    //    var inPress = currentPumpStateArr['F41006'];
    //    var outPress = currentPumpStateArr['F41007'];
    //    var outShunLiu = currentPumpStateArr['F41025'];
    //    var outLeiJiLiu = currentPumpStateArr['FTotalOutLL'];
    //    var totalDl = currentPumpStateArr['FTotalDL'];

    //    var bianPinPinlv = currentPumpStateArr['F41014'];

    //    if (currentPumpStateArr['TempTime']) {
    //        var caiJITime = currentPumpStateArr['TempTime'].replace(/[^0-9]/ig, "");
    //    }


    //    if (machineType == '0') {
    //        typeStr = 'guan';


    //    } else if (machineType == '1') {
    //        typeStr = 'xiang';

    //    } else if (machineType == '2') {
    //        typeStr = ' ';

    //    } else if (machineType == '1') {
    //    }


    //    var status = '';
    //    switch (lunXunBiaoZhuArr[index].IsAlarm) {
    //        case 0: {
    //            switch (lunXunBiaoZhuArr[index].FOnLine) {
    //                case 0: {
    //                    status = 'offline';
    //                    break;
    //                }
    //                case 1: {
    //                    status = 'online';
    //                    break;
    //                }
    //                    //case 2: {
    //                    //    status = 'listItemPicSta2';
    //                    //    statusTxt = '报警';
    //                    //    break;
    //                    //}
    //            }
    //            break;
    //        }
    //        case 1: {
    //            status = 'error';
    //            //statusTxt = '报警';
    //            break;
    //        }
    //    }

    //    var stateDealArr = [];

    //    for (var s = 0; s < pumpArr.length; s++) {
    //        if (pumpArr[s] == '0' || pumpArr[s] == '1') {
    //            stateDealArr.push('offline');
    //        } else if (pumpArr[s] == '2') {
    //            stateDealArr.push('online');
    //        } else if (pumpArr[s] == '3') {
    //            stateDealArr.push('error');
    //        } else {
    //            stateDealArr.push('offline');
    //        }
    //    }




    //    if ($('.loopItem[data-FField=' + value.FField + ']')) {
    //        var $This = $('.loopItem[data-FField=' + value.FField + ']');


    //        var stateDealArr = [];

    //        for (var s = 0; s < pumpArr.length; s++) {
    //            if (pumpArr[s] == '0' || pumpArr[s] == '1') {
    //                stateDealArr.push('offline');
    //            } else if (pumpArr[s] == '2') {
    //                stateDealArr.push('online');
    //            } else if (pumpArr[s] == '3') {
    //                stateDealArr.push('error');
    //            } else {
    //                stateDealArr.push('offline');
    //            }
    //        }

    //        var strLi = '';
    //        for (var j = 0; j < pumpJZNum; j++) {
    //            strLi += '   <li class="yelunItem"> ' +
    //                     '    <div class="yelunLine"></div>   ' +
    //                     '    <div class="yelunStatus ' + stateDealArr[j] + '"></div> ' +
    //                       '</li>                               ';
    //        }

            
    //        $This.attr('data-loopid', lunXunBiaoZhuArr[index].pumpJZ[0].pumpJZID);
    //        $This.attr('data-pumpId', lunXunBiaoZhuArr[index].pumpJZ[0].PumpId);
    //        $This.find('.pumpNameTxt').html(lunXunBiaoZhuArr[index].pumpJZ[0].PumpJZName);
    //        $This.find('.loopPin').addClass('pin');
    //        $This.find('.loopSet').css('display', 'block');
    //        $This.attr('data-fixedId', $This.attr('data-loopid'));
    //        $This.find('.pumpStusPic').removeClass('offline online error').addClass(status);
    //        $This.find('.inNum').html(inPress);
    //        $This.find('.outNum').html(outPress);
    //        $This.find('.timeBox').html(changeTime(caiJITime));
    //        $This.find('.liuLiangNum').html(outLeiJiLiu);
    //        $This.find('.dianLiangNum').html(totalDl);
    //        $This.find('.bianPinNum').html(bianPinPinlv);
    //        $This.find('.yelunStatusBox').html(strLi);
    //        $This.find('.loopContentBox').removeClass('guan xiang').addClass(typeStr);
            


    //    }
    //});
}


//初步 数据 处理完成 之后  进行  的轮播   操作   
function lunXundataLunBoFn() {
  
    pageIndex++;
    var searchTxt = $.trim($('.searchBox .searchTxt').val());
    //alert(searchTxt);

    var index = $('.statusBox li.active').index();

    switch (index) {
        case 0: {
            state = 1;
            break;
        }
        case 1: {
            state = '0';
            break;
        }
        case 2: {
            state = 2;
            break;
        }
        case 3: {
            state = '';
            break;
        }
        default: {
            state = '';
            break;
        }

    }


    //var deferred = $.Deferred();


    //deferred.then(lunXunDataGet2(searchTxt, state), lunXundataGet());

   
    //alert('ok');
    lunXunDataGet2(searchTxt, state);
    //lunXundataGet();
    //lunXunDateDealFn(); 更改

    nextClickOnOff = true;

}

//获取数据 之后 的  刷新操作

function lunXunGetNewUseFn() {
    var loopItemLen = $('.loopItemContainer>li').length;
   

    for (var l = 0; l < loopItemLen; l++) {
        var currentBengFangId = $('.loopItemContainer>li').eq(l).attr('data-PumpId');
        var currentJzId = $('.loopItemContainer>li').eq(l).attr('data-loopid');
        //alert(currentJzId);
        var $This = $('.loopItemContainer>li').eq(l);
        lunXunGetNewFn(currentBengFangId, currentJzId, $This);
    }
   
}


//初步 数据 处理完成 之后  进行  的数据刷新的数据 获取   操作 


function lunXunGetNewFn(bengfangID, bengFangJzID, $This) {
    $.ajax({
        url: '/V_YCJK/Search_Pump_JZReportList',
        data: {
            'pumpID': bengfangID,
            'pumpJZID':bengFangJzID
        },
        success: function (data) {
            // alert(data);
            var dataJSON = JSON.parse(data);
            dataJSON = dataJSON.obj;
           
            //console.log(dataJSON);
            //console.log('亲亲呢');
            everyItemGetDataFn(dataJSON, $This);



        },
        error: function (data) {
            //console.log('');
        }
    });
}


//单独 获取 数据 处理函数

function everyItemGetDataFn(everyItemData, $This) {
   
    var pumpAux = everyItemData[0].pumpJZ[0].Auxiliarypumpcount;
    var pumpRun = everyItemData[0].pumpJZ[0].RunPumpNum;
        var pumpJZNum = pumpAux + pumpRun;
        var outNum = everyItemData[0].pumpJZ[0].DrainPumpNum;
        var machineType = $.trim(everyItemData[0].pumpJZ[0].MachineType);

        var currentPumpStateArr = everyItemData[0].pumpJZ[0].D_Data[0];

        var typeStr;
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
        var totalDl = currentPumpStateArr['FTotalDL'];

        var bianPinPinlv = currentPumpStateArr['F41014'];

        if (currentPumpStateArr['TempTime']) {
            var caiJITime = currentPumpStateArr['TempTime'].replace(/[^0-9]/ig, "");
        }


        if (machineType == '0') {
            typeStr = 'guan';


        } else if (machineType == '1') {
            typeStr = 'xiang';

        } else if (machineType == '2') {
            typeStr = ' ';

        } else if (machineType == '1') {
        }


        var status = '';
        
        switch (everyItemData[0].pumpJZ[0].IsAlarm) {
            case 0: {
                switch (currentPumpStateArr.FOnLine) {
                    case 0: {
                        status = 'offline';
                        break;
                    }
                    case 1: {
                        status = 'online';
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

        var stateDealArr = [];

        for (var s = 0; s < pumpArr.length; s++) {
            if (pumpArr[s] == '0' || pumpArr[s] == '1') {
                stateDealArr.push('offline');
            } else if (pumpArr[s] == '2') {
                stateDealArr.push('online');
            } else if (pumpArr[s] == '3') {
                stateDealArr.push('error');
            } else {
                stateDealArr.push('offline');
            }
        }

      
            var stateDealArr = [];

            for (var s = 0; s < pumpArr.length; s++) {
                if (pumpArr[s] == '0' || pumpArr[s] == '1') {
                    stateDealArr.push('offline');
                } else if (pumpArr[s] == '2') {
                    stateDealArr.push('online');
                } else if (pumpArr[s] == '3') {
                    stateDealArr.push('error');
                } else {
                    stateDealArr.push('offline');
                }
            }

            var strLi = '';
            for (var j = 0; j < pumpJZNum; j++) {
                strLi += '   <li class="yelunItem"> ' +
                         '    <div class="yelunLine"></div>   ' +
                         '    <div class="yelunStatus ' + stateDealArr[j] + '"></div> ' +
                           '</li>                               ';
            }


            $This.attr('data-loopid', everyItemData[0].pumpJZ[0].pumpJZID);
            $This.attr('data-pumpId', everyItemData[0].pumpJZ[0].PumpId);
            $This.find('.pumpStusPic').removeClass('offline online error').addClass(status);
            $This.find('.inNum').html(inPress);
            $This.find('.outNum').html(outPress);
            $This.find('.timeBox').html(changeTime(caiJITime));
            $This.find('.liuLiangNum').html(outLeiJiLiu);
            $This.find('.dianLiangNum').html(totalDl);
            $This.find('.bianPinNum').html(bianPinPinlv);
            $This.find('.yelunStatusBox').html(strLi);
            $This.find('.loopContentBox').removeClass('guan xiang').addClass(typeStr);

  

}


//右侧  在线 离线 全部  按钮的  点击函数

function rightOnOffAllFn() {
    $('.statusBox>li').click(function () {
        var index = $(this).index();
        var state;
        if ($(this).hasClass('active')) {
            return;
        } else {
            $(this).addClass('active').siblings().removeClass('active');

            switch (index) {
                case 0: {
                    state = 1;
                    break;
                }
                case 1: {
                    state = '0';
                    break;
                }
                case 2: {
                    state = 2;
                    break;
                }
                case 3: {
                    state = '';
                    break;
                }
                default: {
                    state = '';
                    break;
                }

            }
            pageIndex = 0;
            $('.searchBox .searchTxt').val('');
           
           // var deferred = $.Deferred();
            lunXunDataBigArr = [];
          //  alert('ok');
           // deferred.then(lunXunDataGet2('', state), lunXundataGet());
            lunXunDataGet2('', state);
            //lunXundataGet();
            //lunXunDateDealFn();   更改


            clearInterval(lunBoTimer);
            lunBoTimer = setInterval(function () {
                lunXundataLunBoFn();
            }, 1000 * 60);

            clearInterval(lunXunGetNewTimer);
            lunXunGetNewTimer = setInterval(function () {
                lunXunGetNewUseFn();
            }, 1000 * 6);
        }
    });
}

//右侧 搜索 按钮的  点击 搜索事件

function rightSearchClickFn() {
    $('.searchBtn').click(function () {
        var searchTxt = $.trim($('.searchBox .searchTxt').val());
        //alert(searchTxt);

        var index = $('.statusBox li.active').index();

        switch (index) {
            case 0: {
                state = 1;
                break;
            }
            case 1: {
                state = 0;
                break;
            }
            case 2: {
                state = 2;
                break;
            }
            case 3: {
                state = '';
                break;
            }
            default: {
                state = '';
                break;
            }

        }
        pageIndex = 0;

        //var deferred = $.Deferred();


        //deferred.then(lunXunDataGet2(searchTxt,state), lunXundataGet());

        lunXunDataBigArr = [];
      //  alert('ok');
        lunXunDataGet2(searchTxt, state);
        //lunXundataGet();
        //lunXunDateDealFn();  更改


        clearInterval(lunBoTimer);
        lunBoTimer = setInterval(function () {
            lunXundataLunBoFn();
        }, 1000 * 60);

        clearInterval(lunXunGetNewTimer);
        lunXunGetNewTimer = setInterval(function () {
            lunXunGetNewUseFn();
        }, 1000 * 6);
       
    });
}




//   页面   轮询  机组 设置  固定的 id


function setJzId() {
    $('body').delegate('.loopSet', 'click', function () {
        var $This = $(this);
        layer.open({
            type: 2,
            anim: 3,
            shade: .6,
            title: false,
            shadeClose: true,
            area: ['1010px', '560px'],
            content: '/YCJK/Window/pumpJZWindow?pumpID='+' ',
            success: function () {
               // alert('OK');
            }
            
            , end: function () {
               // alert($jzId);
                $This.parent().parent().attr('data-fixedId', $jzId);
                $This.parent().parent().attr('data-loopid', $jzId);
                editLunxun($This.parent().parent().index()+1, $jzId);
            }
        });
            
    });
    
}



//  点击 pin  固定 一个 id

function pinClickFn() {
    $('body').delegate('.loopPin', 'click', function () {
        if (!$(this).hasClass('pin')) {
           
          
        }
        
    });
}




//      返回   jz  ID   使用函数
function setJzName(jzName, jzId) {
     $jzName = jzName;
     $jzId = jzId;
  
}



//  编辑  轮询  状态

function editLunxun(selectInd,baseId) {
    
    $.ajax({
        url: '/V_YCJK/EditLunXun',
        data:{
            'FField': selectInd,
            'BaseID': baseId
        },
        success: function (data) {
          //  alert(data);
            var dataJSON = JSON.parse(data);
            dataJSON = dataJSON.obj;



        },
        error: function (data) {
            console.log('');
        }
    });
}




//点击 next  的  立即切换

function nextClickFn() {
    $('.nextBtn').click(function () {
        if (nextClickOnOff) {
            nextClickOnOff = false;
            lunXundataLunBoFn();
            clearInterval(lunBoTimer);

            lunBoTimer = setInterval(function () {
                lunXundataLunBoFn();
            }, 1000 * 60);
        } 
       
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
