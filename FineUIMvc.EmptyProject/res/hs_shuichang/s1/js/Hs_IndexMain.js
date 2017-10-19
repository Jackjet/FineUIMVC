$(function () {
    leftNavClickFn();
    leftListClickFn();
    pageResize();
    leftNavAni();
   
});









//页面重置

function pageResize() {

    //	var listWrapHeight= $('.listWrap').height();
    $('.listBox').height($('.listWrap').height() - 160);
    //	console.log($('#mCSB_1_container').height());
    //	console.log($('.listBox #mCSB_1_container').position().top);
    //	
    //	console.log($('#mCSB_1_container').height()+$('.listBox #mCSB_1_container').position().top);
    //	console.log($('.listBox').height());
    //	console.log('-----------------------------');
}



//左侧 NAV点击 切换  函数
function leftNavClickFn() {
    $('.leftNav li').click(function () {
        var index = $(this).index();
        var dataCla = $(this).attr('data-cla');
        if ($(this).hasClass('active')) {
            return;
        } else {
            if (dataCla == "shebei") {
                return;
            } else {
                $(this).addClass('active').siblings().removeClass('active');
            }

        }

        switch (dataCla) {
            case "shebei": {
                break;
            }
            case "bengfang": {
                break;
            }
            case "ditu": {
                $('.mainIframe').attr('src', F.baseUrl + 'YCJK/V_YCJK/Map?id=' + Math.random);
                break;
            }
            case "lieBiao": {
                $('.mainIframe').attr('src', F.baseUrl + 'YCJK/V_YCJK/listControl');
                break;
            }
            case "lunXun": {
                $('.mainIframe').attr('src', F.baseUrl + 'YCJK/V_YCJK/loopCheck');
                break;
            }
            case "baobiao": {
                $('.mainIframe').attr('src', F.baseUrl + 'YCJK/V_Report/mainReport');
                break;
            }

            case "baojing": {
                $('.mainIframe').attr('src', F.baseUrl + 'YCJK/V_YCJK/WarningAlarm?pageType=1');
                break;
            }
            case "help": {
                alert("暂无");
                break;
            }
        }
    });
}


// 左侧  列表 的  点击 函数

function leftListClickFn() {
    $('body').delegate('.listWrap .listItemWrap li', 'click', function () {
        var index = $(this).index();
        var random = Math.random();
        var pumpId = $(this).attr('data-pumpId');
        var mark = $(this).attr('data-mark');
        $('.mainIframe').attr('src', F.baseUrl + 'YCJK/V_YCJK/EGBFSB?pumpId=' + pumpId + '&jzId=' + mark);
        $('.leftNav li[data-cla="shebei"]').addClass('active').siblings().removeClass('active');
    });


}


//左侧 nav的动画

function leftNavAni() {
    var leftNavTopBoxTimer,leftNavTimer,sheBeiListTimer ,onOff;
   
    $('.leftNavTopBox').stop().mouseenter(function () {
       
        clearTimeout(leftNavTopBoxTimer);
        
        $('.leftNav').show();
        $('.sheBeiList').show();
        $('.navClickSanjiao').show();
        TweenMax.set('.leftNav', { rotationX: 0, y: '-=86px', autoAlpha: 0 });
        TweenMax.set(".sheBeiList", { rotationY: 0, 'x': '-106', scaleX: 0.6, autoAlpha: 0});
        var timeLine = new TimelineLite();
        timeLine.to('.leftNav', 0.1, { y: '+=43', autoAlpha: 0.3 })
            .to('.leftNav', 0.2, { y: '0', autoAlpha: 1, ease: Power2.easeOut })
            .to('.sheBeiList', 0.1, {'x': '+=56',scaleX:1, autoAlpha: .1})
            .to('.sheBeiList', 0.2, {'x': '+=50',scaleX:1, autoAlpha: 1});
    });
    $('.leftNavTopBox').stop().mouseleave(function () {
        
        leftNavTopBoxTimer = setTimeout(function () {
            var timeLine2 = new TimelineLite();
            TweenLite.set(".sheBeiList", { transformPerspective: 500 });
            TweenLite.set(".leftNav", { transformPerspective: 500 });
            timeLine2.to('.sheBeiList', 0.3, { rotationY: 90, transformOrigin: "left top" })
                     .to('.leftNav', 0.3, { rotationX: 90, transformOrigin: "left top" })
            ;
            $('.navClickSanjiao').hide();
           

        }, 600);
    });

    $('.leftNav').stop().mouseenter(function () {
        
        clearTimeout(leftNavTopBoxTimer);
 
    });

   
    $('.leftNav').stop().mouseleave(function () {
       
        leftNavTopBoxTimer = setTimeout(function () {
            var timeLine2 = new TimelineLite();
            TweenLite.set(".sheBeiList", { transformPerspective: 500 });
            TweenLite.set(".leftNav", { transformPerspective: 500 });
            timeLine2.to('.sheBeiList', 0.3, { rotationY: 90, transformOrigin: "left top" })
                     .to('.leftNav', 0.3, { rotationX: 90, transformOrigin: "left top" })
            ;
            $('.navClickSanjiao').hide();
           
        }, 600);
    });
   
    
    $('.sheBeiList').stop().mouseenter(function () {

        clearTimeout(leftNavTopBoxTimer);

    });
    $('.sheBeiList').stop().mouseleave(function () {
        
        leftNavTopBoxTimer = setTimeout(function () {
            var timeLine2 = new TimelineLite();
            TweenLite.set(".sheBeiList", { transformPerspective: 500 });
            TweenLite.set(".leftNav", { transformPerspective: 500 });
            timeLine2.to('.sheBeiList', 0.3, { rotationY: 90, transformOrigin: "left top" })
                     .to('.leftNav', 0.3, { rotationX: 90, transformOrigin: "left top" })
            ;
            $('.navClickSanjiao').hide();
            
        }, 600);
    });

}


