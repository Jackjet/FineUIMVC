$(function () {
    warnInfoTypeFn();
    warnSetFn();
    txtWanBoxFn();
    picWanBoxFn();
    warnPositionFn();
    warnSetDataGet();

    warnListGet();

   warnListGetNewTimer();

});


var warnMesArr1 = [];
var warnMesArr2 = [];
var warnLastTime = 0;

//第一次弹出的报警 的时间
var warnFirstTime = 0;


var warnMesArr1Timer;


//刷新 函数获取  数据 的定时器
var warnGetNewTimer;


var shanTimer;
var shanOnoff = true;


//报警显示 类型的 点击函数




function warnInfoTypeFn() {
    $('.warnSetBox .closeBox').click(function () {
        $('.warnSetContainer').hide();
    });
    $('.warnSetBox').click(function (e) {
        e.stopPropagation();
    });
    
    $('.warnSetContainer').click(function () {
        $(this).hide();
    });

    $('.showType').click(function () {
        $('.showType').removeClass('active');
        $(this).addClass('active');
        if ($(this).hasClass('txtType')) {
            $('.warnInfoPosition').show();
            $('.warnPositonBox').show();
            $('.warnContentBox').show();
            $('.warnShowHideBox').hide();
            var warnIn = true;
            txtWarnRemoveFn(warnIn);

            warnSetDataSet('FDisplay', 1);

        } else {
            $('.warnInfoPosition').hide();
            $('.warnPositonBox').hide();
            $('.warnContentBox').hide();
            $('.warnShowHideBox').show();
            var warnIn = false;
            txtWarnRemoveFn(warnIn);
            warnSetDataSet('FDisplay', 2);
        }
    });
}


//报警设置 的 点击函数

function warnSetFn() {
    
    $('.warnShowHideBox .rightSet').click(function () {
        $('.warnSetContainer').show();
        if ($('.txtType').hasClass('active')) {
            $('.warnInfoPosition').show();
            $('.warnPositonBox').show();
            
        } else {
            $('.warnInfoPosition').hide();
            $('.warnPositonBox').hide();
        }
    });
    $('.warnContentBox .setAndSound .setBtn').click(function () {
        $('.warnSetContainer').show();
        if ($('.txtType').hasClass('active')) {
            $('.warnInfoPosition').show();
            $('.warnPositonBox').show();
        } else {
            $('.warnInfoPosition').hide();
            $('.warnPositonBox').hide();
        }
    });

    $('.warnContentBox .setAndSound .soundBtn').click(function () {
        
        if ($(this).hasClass('noSound')) {
            $(this).removeClass('noSound');
            $('.warnSetContainer .warnSoundBox').removeClass('noSound');
            warnSetDataSet('FVoice', 1);
        } else {
            $(this).addClass('noSound');
            $('.warnSetContainer .warnSoundBox').addClass('noSound');
            warnSetDataSet('FVoice', 2);
        }
    });
}


//文字报警框的 点击事件

function txtWanBoxFn() {
    $('.warnContentBox .close').click(function () {
        
        var warnIn = false;
        txtWarnRemoveFn(warnIn);
        clearInterval(warnMesArr1Timer);

        clearInterval(shanTimer);
        shanOnoff = true;
       // $('.warnContentBox').hide();
    });
    $('.warnContentBox .more').click(function () {
        F.ui.Window1.show('/OpenWindow/Alarm_TimelyWindow', '及时报警列表',1000,500);
    });
}

//图片报警框的 点击事件

function picWanBoxFn() {
   
    $('.warnShowHideBox .rightInfo').click(function () {
        F.ui.Window1.show('/OpenWindow/Alarm_TimelyWindow', '及时报警列表', 1000, 500);
    });

    $('.warnShowHideBox').mouseover(function () {
        $('.warnShowHideBox ').stop().animate({'right': '2px'},400);
    });
    $('.warnShowHideBox').mouseleave(function () {
        $('.warnShowHideBox ').stop().animate({ 'right': '-100px' }, 400);
    });
   
}


//报警 的显示 位置声音控制
function warnPositionFn() {

    $('.warnSetContainer .warnSoundBox').click(function () {

        if ($(this).hasClass('noSound')) {
            $(this).removeClass('noSound');
            $('.warnContentBox .soundBtn').removeClass('noSound');
           
            warnSetDataSet('FVoice', 1);
        } else {
            $(this).addClass('noSound');
            $('.warnContentBox .soundBtn').addClass('noSound');
            warnSetDataSet('FVoice', 2);
        }
    });


    $('.warnPositonBox li').click(function () {
        if ($(this).attr('data-mark') == '0') {
            alert('no');
            return;
        }
        $(this).addClass('active').siblings().removeClass('active');
        switch ($(this).attr('data-mark')) {
            case '1': {
                $('.warnContentBox').removeClass('leftTop top rightTop right rightBottom bottom leftBottom left').addClass('rightBottom');
                warnSetDataSet('FPosition', 1);
                break;
            }
            case '2': {
                $('.warnContentBox').removeClass('leftTop top rightTop right rightBottom bottom leftBottom left').addClass('bottom');
                warnSetDataSet('FPosition', 2);
                break;
            }
            case '3': {
                $('.warnContentBox').removeClass('leftTop top rightTop right rightBottom bottom leftBottom left').addClass('leftBottom');
                warnSetDataSet('FPosition', 3);
                break;
            }
            case '4': {
                $('.warnContentBox').removeClass('leftTop top rightTop right rightBottom bottom leftBottom left').addClass('left');
                warnSetDataSet('FPosition', 4);
                break;
            }
            case '5': {
                $('.warnContentBox').removeClass('leftTop top rightTop right rightBottom bottom leftBottom left').addClass('leftTop');
                warnSetDataSet('FPosition', 5);
                break;
            }
            case '6': {
                $('.warnContentBox').removeClass('leftTop top rightTop right rightBottom bottom leftBottom left').addClass('top');
                warnSetDataSet('FPosition', 6);
                break;
            }
            case '7': {
                $('.warnContentBox').removeClass('leftTop top rightTop right rightBottom bottom leftBottom left').addClass('rightTop');
                warnSetDataSet('FPosition', 7);
                break;
            }
            case '8': {
                $('.warnContentBox').removeClass('leftTop top rightTop right rightBottom bottom leftBottom left').addClass('right');
                warnSetDataSet('FPosition', 8);
                break;
            }
        }
    });
}



//warn  报警  后台设置data获取函数

function warnSetDataGet() {
    $.ajax({
        url: '/V_YCJK/Alarm_Param_UserSearch',
        success: function (data) {
           
            var dataJSON = JSON.parse(data);
            dataJSON = dataJSON.obj[0];
            //console.log(dataJSON);
            //console.log(54545454);

            switch(Number(dataJSON.FVoice)){
                case 1: {
                    
                    $('.warnSetContainer .warnSoundBox').removeClass('noSound');
                    $('.warnContentBox .setAndSound .soundBtn').removeClass('noSound');
                    break;
                }
                case 2: {
                   
                    $('.warnSetContainer .warnSoundBox').addClass('noSound');
                    $('.warnContentBox .setAndSound .soundBtn').addClass('noSound');
                    break;
                }
            }

            switch (Number(dataJSON.FDisplay)) {
                case 1: {
                   
                    $('.txtType').addClass('active').next().removeClass('active');
                    //$('.warnContentBox').show();
                    $('.warnShowHideBox').hide();
                    $('.warnInfoPosition').show();
                    $('.warnPositonBox').show();
               
                  
                    break;
                }
                case 2: {
                    
                    $('.txtType').removeClass('active').next().addClass('active');
                    $('.warnContentBox').hide();
                    //$('.warnShowHideBox').show();
                    $('.warnInfoPosition').hide();
                    $('.warnPositonBox').hide();
                    break;
                }
            }

            switch (Number(dataJSON.FPosition)) {
                case 1: {
                    $('.warnContentBox').removeClass('leftTop top rightTop right rightBottom bottom leftBottom left').addClass('rightBottom');
                    break;
                }
                case 2: {
                    $('.warnContentBox').removeClass('leftTop top rightTop right rightBottom bottom leftBottom left').addClass('bottom');
                    break;
                }
                case 3: {
                    $('.warnContentBox').removeClass('leftTop top rightTop right rightBottom bottom leftBottom left').addClass('leftBottom');
                    break;
                }
                case 4: {
                    $('.warnContentBox').removeClass('leftTop top rightTop right rightBottom bottom leftBottom left').addClass('left');
                    break;
                }
                case 5: {
                    $('.warnContentBox').removeClass('leftTop top rightTop right rightBottom bottom leftBottom left').addClass('leftTop');
                    break;
                }
                case 6: {
                    
                    $('.warnContentBox').removeClass('leftTop top rightTop right rightBottom bottom leftBottom left').addClass('top');
                    break;
                }
                case 7: {
                    $('.warnContentBox').removeClass('leftTop top rightTop right rightBottom bottom leftBottom left').addClass('rightTop');
                    break;
                }
                case 8: {
                    $('.warnContentBox').removeClass('leftTop top rightTop right rightBottom bottom leftBottom left').addClass('right');
                    break;
                }
            }
            
        },
        error: function () { }
    });
}


//warn  报警  后台设置data传递函数

function warnSetDataSet(TableField, text) {
    $.ajax({
        url: '/V_YCJK/Alarm_Param_UserEdit',
        data: {
            'TableField': TableField,
            'text': text
        },
        success: function (data) {
           // alert(data);


        },
        error: function () { }
    });
}



//文字报警  的 框进入消失函数

function txtWarnRemoveFn(warnIn) {

    if (warnIn) {
       
        TweenMax.fromTo('.warnContentBox', .9, { autoAlpha: 0, scale: 0.6 }, { autoAlpha: 1, scale: 1, ease: Back.easeInOut });
        $('.warnContentBox').css('marginRight', 0);
    } else {
        $('.warnContentBox').css('marginRight', 0);
        TweenMax.fromTo('.warnContentBox', 0.9, { autoAlpha: 1, scale: 1 }, { autoAlpha: 0,scale:0.6, ease: Back.easeInOut });
    }
}

//报警信息 列表 刷新定时器

function warnListGetNewTimer() {
    clearInterval(warnGetNewTimer);
    window.setTimeout(function () {
       
        warnGetNewTimer = setInterval(function () {
            warnListGetNew();
        }, 1000 * 4);
    },1000*10);

}

//报警信息 列表  刷新获取  函数
function warnListGetNew() {
    $.ajax({
        url: '/V_YCJK/ALLAlarm_List',
        success: function (data) {
           // alert(data);

            var dataJSON = JSON.parse(data);
            //console.log(dataJSON);
            dataJSON = dataJSON.obj;
            //console.log(dataJSON);
            //console.log(54545454+'222');

            warnMesArr2 = dataJSON;

            warnInfoDeal(warnMesArr2, warnLastTime);

        },
        error: function () { }
    });
}

//报警 信息 列表的获取

function warnListGet() {
    $.ajax({
        url: '/V_YCJK/ALLAlarm_List',
        success: function (data) {
            //alert(data);

            var dataJSON = JSON.parse(data);
            //console.log(dataJSON);
            dataJSON = dataJSON.obj;
            //console.log(dataJSON);
            //console.log(54545454+"11111");

            warnMesArr1 = dataJSON;
            warnMesArr2 = dataJSON;
           

            if (warnMesArr2.length < 1) {
                warnLastTime = '';
                return;
            }
            if (warnMesArr2[0].FAlarmTime) {
                warnLastTime = Number((warnMesArr2[0].FAlarmTime).replace(/[^0-9]/ig, ""));
                warnFirstTime = warnLastTime;
               // alert(warnLastTime);
            } else {
                warnLastTime = '';
                warnFirstTime = '';
            }
           
            warnInfoYemianShow();

            warnInfoLunBo();
        },
        error: function () { }
    });
}



//报警新信息 是否更换的处理 函数  

function warnInfoDeal(dataArr, warnLastTime) {

    if (dataArr.length < 1) {
        ///alert(1);
        return;
    }
    var warnNewTime;
    if (dataArr[0].FAlarmTime) {
        warnNewTime =Number((dataArr[0].FAlarmTime).replace(/[^0-9]/ig, ""));
    } else {
        warnNewTime = '时间暂无换取';
    }
    
    //console.log(warnLastTime + '' + warnNewTime);
    //console.log(123);

    if ((warnNewTime > warnLastTime) || warnLastTime=='') {
        warnMesArr1 = dataArr;
       
        warnLastTime = warnNewTime;
        warnInfoYemianShow();
        warnInfoLunBo();
    } else {
       
        warnLastTime = warnNewTime;
        return;
    }
    

}





//报警信息 的 页面显示   函数 

function warnInfoYemianShow() {
    if (warnMesArr1.length < 1) {
        clearInterval(warnMesArr1Timer);
        clearInterval(shanTimer);
        shanOnoff = true;

        return;
    }
    var warnTime;
    if (warnMesArr1[0].FAlarmTime) {
        warnTime = changeTime(Number((warnMesArr1[0].FAlarmTime).replace(/[^0-9]/ig, "")));
    } else {
        warnTime = '时间暂无换取';
    }
  

    var warnMes = warnMesArr1[0].FMsg;

    var warnName = warnMesArr1[0].FName;

    $('.warnContentBox .warnContentHeader .contentHeaderTxt').html(warnName);
    $('.warnContentBox .warnContentMain .contentMainTime').html(warnTime);
    $('.warnContentBox .warnContentMain .contentMainTxt').html(warnMes);
    txtWarnRemoveFn(true);
}

//报警信息的  轮播 以及  处理函数 
function warnInfoLunBo() {
    clearInterval(warnMesArr1Timer);
    shanPic();
    warnMesArr1Timer = setInterval(function () {
        txtWarnRemoveFn(false);
        //alert(1);
        var shiftItem = warnMesArr1.shift();
       // warnMesArr1.push(shiftItem);
        window.setTimeout(function () {
            warnInfoYemianShow();
        }, 1000 * 2);
        //
    }, 1000 * 10);
   
}


//右侧 报警 图片的 一闪一闪的处理 函数

function  shanPic(){
   
    clearInterval(shanTimer);
   
    shanTimer = setInterval(function () {
        if (shanOnoff) {
            $('.leftCircle').addClass('hide');
        } else {
            $('.leftCircle').removeClass('hide');
        }
        shanOnoff = !shanOnoff;
    }, 600);
}


//时间转换

function changeTime(time) {
    if (!time) {
        return;
    }
    var Time = new Date(Number(time));
    return Time.getFullYear() + "-" + (Time.getMonth() + 1) + "-" + Time.getDate() + " " + Time.getHours() + ":" + Time.getMinutes() + ":" + Time.getSeconds();
}
