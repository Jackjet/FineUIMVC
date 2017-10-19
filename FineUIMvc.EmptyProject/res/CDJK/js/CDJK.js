var pageIndex = 0;//左侧  列表的 获取 页码索引
var pumpNumTimer;//左侧 列表 泵房数量 的定时器
var scrollOnoff = true;
var leftIndex = 3;
var keHuZhuangTaiFenzuData = [], keHuZhuangTaiBengData = [];


var wuFenZuPageIndex = 0;//左侧  无分组泵房列表的 获取 页码索引
var wuFenZuScrollOnoff = true;
var wuFenZuArr = [];
$(function () {
  //  leftHoverShow();
 //   listItemScroll();
 //   pumpListItemScrollOne();
  //  pumpListItemScrollTwo();
    pageResize();
    leftNavClickFn();
//    leftListClickFn();
 //   leftOnOffAllFn();
//    leftSearchClickFn();
    infoIconFn();
    //载入时 列表获取
   // leftListDataGet();
   // pumpNumGet();

    //pumpNumTimer = setInterval(function () {
    //    pumpNumGet();
    //}, 3000);


    $(window).resize(function () {
        pageResize();
    });
    $(window).scroll(function () {
        // alert(1);
    });



    leftbengfangList2Fn();
    leftbengfangChildListItemClick();

    leftbengfangChildJizuFenzuDataGet();


    leftWuFenZubengfangChildListItemClick();


    leftWufenZuSearchClickFn();
    leftFenZuSearchClickFn();

    pumpWuFenZuNumGet();


});


//左侧hover显示  事件
function leftHoverShow() {

    var leftNavHoverTimer, leftListHoverTimer;

    $('.shebei').mouseenter(function () {

        clearTimeout(leftNavHoverTimer);
        clearInterval(leftListHoverTimer);
        $('.listWrap').stop().animate({ 'marginLeft': 0 }, 400);//600
        $('.listWrap').css('zIndex', ++leftIndex);
        $('.leftNav').css('zIndex', ++leftIndex);
    });


    $('.shebei').mouseleave(function () {
        leftNavHoverTimer = setTimeout(function () {

            $('.listWrap').stop().animate({ 'marginLeft': '-300px' }, 200);//400

        }, 500);

        $('.listWrap').mouseenter(function () {
            clearTimeout(leftNavHoverTimer);
        });
        $('.listWrap').mouseleave(function () {
            leftListHoverTimer = setTimeout(function () {
                $('.listWrap').stop().animate({ 'marginLeft': '-300px' }, 200);//400

            }, 500);
        });
    });


    var leftNavBengFangTimer, leftBengFangListHoverTimer;

    $('.bengfang').mouseenter(function () {
        clearTimeout(leftNavBengFangTimer);
        clearInterval(leftBengFangListHoverTimer);
        $('.bengfangListWrap2').stop().animate({ 'marginLeft': 0 }, 400);//600
        $('.bengfangListWrap2').css('zIndex', ++leftIndex);
        $('.leftNav').css('zIndex', ++leftIndex);

    });


    $('.bengfang').mouseleave(function () {
        leftNavBengFangTimer = setTimeout(function () {

            $('.bengfangListWrap2').stop().animate({ 'marginLeft': '-300px' }, 200);//400

        }, 500);

        $('.bengfangListWrap2').mouseenter(function () {
            clearTimeout(leftNavBengFangTimer);
        });
        $('.bengfangListWrap2').mouseleave(function () {
            leftBengFangListHoverTimer = setTimeout(function () {
                $('.bengfangListWrap2').stop().animate({ 'marginLeft': '-300px' }, 200);//400

            }, 500);
        });
    });



    var leftNavWuFenZuBengFangTimer, leftWuFenZuBengFangListHoverTimer;

    $('.bengfang').mouseenter(function () {
        clearTimeout(leftNavWuFenZuBengFangTimer);
        clearInterval(leftWuFenZuBengFangListHoverTimer);
        $('.bengfangListWrap').stop().animate({ 'marginLeft': 0 }, 400);//600
        $('.bengfangListWrap').css('zIndex', ++leftIndex);
        $('.leftNav').css('zIndex', ++leftIndex);
    });


    $('.bengfang').mouseleave(function () {
        leftNavWuFenZuBengFangTimer = setTimeout(function () {

            $('.bengfangListWrap').stop().animate({ 'marginLeft': '-300px' }, 200);//400

        }, 500);

        $('.bengfangListWrap').mouseenter(function () {
            clearTimeout(leftNavWuFenZuBengFangTimer);
        });
        $('.bengfangListWrap').mouseleave(function () {
            leftWuFenZuBengFangListHoverTimer = setTimeout(function () {
                $('.bengfangListWrap').stop().animate({ 'marginLeft': '-300px' }, 200);//400

            }, 500);
        });
    });

}



//list的  滚动条 函数

function listItemScroll() {
    $('.listWrap .listBox').mCustomScrollbar({
        scrollbarPosition: "inside",
        theme: "minimal-dark",
        callbacks: {
            whileScrolling: function () {
                var $that = this.mcs.left;
                // console.log($('#mCSB_1_container').height());
                // console.log($('.listBox #mCSB_1_container').position().top);
                var jian = $('#mCSB_1_container').height() + $('.listBox #mCSB_1_container').position().top;
                // console.log($('#mCSB_1_container').height() + $('.listBox #mCSB_1_container').position().top);
                // console.log($('.listBox').height());
                if (jian - 10 <= $('.listBox').height()) {
                    //console.log('获取');
                    if (scrollOnoff) {
                        leftListDataScrollGet();
                    }
                }
                // console.log('-----------------------------');
            }
        }
    });
}


//泵房样式一的 滚动条
function pumpListItemScrollOne() {
    $('.bengfangListWrap .listBox').mCustomScrollbar({
        scrollbarPosition: "inside",
        theme: "minimal-dark",
        callbacks: {
            whileScrolling: function () {
                var $that = this.mcs.left;
                // console.log($('#mCSB_1_container').height());
                // console.log($('.listBox #mCSB_1_container').position().top);
                var jian = $('#mCSB_2_container').height() + $('.bengfangListWrap .listBox #mCSB_2_container').position().top;
                // console.log($('#mCSB_1_container').height() + $('.listBox #mCSB_1_container').position().top);
                // console.log($('.listBox').height());
                if (jian - 10 <= $('.bengfangListWrap .listBox').height()) {
                    //console.log('获取');
                    if (wuFenZuScrollOnoff) {

                        leftWuFenZuListDataScrollGet();

                    }
                }
                // console.log('-----------------------------');
            }
        }
    });
}

//泵房样式二的 滚动条
function pumpListItemScrollTwo() {
    $('.bengfangListWrap2 .listBox').mCustomScrollbar({
        scrollbarPosition: "inside",
        theme: "minimal-dark",
        callbacks: {
            //whileScrolling: function () {
            //    var $that = this.mcs.left;
            //    // console.log($('#mCSB_1_container').height());
            //    // console.log($('.listBox #mCSB_1_container').position().top);
            //    var jian = $('#mCSB_3_container').height() + $('.listBox #mCSB_3_container').position().top;
            //    // console.log($('#mCSB_1_container').height() + $('.listBox #mCSB_1_container').position().top);
            //    // console.log($('.listBox').height());
            //    if (jian - 10 <= $('.listBox').height()) {
            //        //console.log('获取');
            //        if (scrollOnoff) {
            //            leftListDataScrollGet();
            //        }
            //    }
            //    // console.log('-----------------------------');
            //}
        }
    });
}





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
            $(this).addClass('active').siblings().removeClass('active');
           /* if (dataCla == "shebei") {
                return;
            } else {
                $(this).addClass('active').siblings().removeClass('active');
            }*/

        }


    
     
        switch (dataCla) {
            case "liuliang": {
                $('.mainIframe').attr('src', F.baseUrl + 'YCJK/V_CDJK/Flow_List');
                break;
            }
            case "yali": {
                $('.mainIframe').attr('src', F.baseUrl + 'YCJK/V_CDJK/PA_List');
                break;
            }
            case "tiaofeng": {
                $('.mainIframe').attr('src', F.baseUrl + 'YCJK/V_CDJK/TFBengZhan');
                break;
            }
            case "lieBiao": {
               
                break;
            }
            case "famen": {
                $('.mainIframe').attr('src', F.baseUrl + 'YCJK/V_CDJK/FM_List');
                break;
            }
            case "baobiao": {
               // $('.mainIframe').attr('src', F.baseUrl + 'YCJK/V_CDJK/CDmainReport');
                break;
            }

            case "baojing": {
              //  $('.mainIframe').attr('src', F.baseUrl + 'YCJK/V_YCJK/WarningAlarm?pageType=1');
                break;
            }
            case "help": {
           //     alert("暂无");
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

//左侧列表  info图标的  函数 
function infoIconFn() {
    $('body').delegate('.listWrap .listItemStatus', 'mouseenter ', function (e) {
        //alert(1);
        //alert(e.pageX);
        //alert(e.pageY);
        var FDTUCode = $(this).attr('data-FDTUCode');
        var PName = $(this).attr('data-PName');
        var FUpdateDate = $(this).attr('data-FUpdateDate');
        var PumpJZArea = $(this).attr('data-PumpJZArea');
        $('.infoShowBox').fadeIn().html(
              '<ul class="leftInfoBox clearfix">' +
    '    <li class="leftInfoItem clearfix">' +
    '        <div class="infoLeft clearfix">' +
    '            <div class="infoPic"></div>' +
    '            <div class="infoName">编号：</div>' +
    '        </div>' +
    '        <div class="infoRight">' +
    FDTUCode +
    '        </div>' +
    '    </li>' +
    '    <li class="leftInfoItem clearfix">' +
    '        <div class="infoLeft clearfix">' +
    '            <div class="infoPic"></div>' +
    '            <div class="infoName">所属泵房：</div>' +
    '        </div>' +
    '        <div class="infoRight">' +
    PName + '[' + PumpJZArea + ']' +
    '        </div>' +
    '    </li>' +
    '    <li class="leftInfoItem clearfix">' +
    '        <div class="infoLeft clearfix">' +
    '            <div class="infoPic"></div>' +
    '            <div class="infoName">最近时间：</div>' +
    '        </div>' +
    '        <div class="infoRight">' +
    FUpdateDate +
    '        </div>' +
    '    </li>' +
    '</ul>'
            ).css({
                'top': e.pageY - 41,
                'left': e.pageX + 36
            });
    });

    $('body').delegate('.listWrap .listItemStatus', 'mouseleave', function () {
        $('.infoShowBox').stop().fadeOut(10);
    });
}


//泵房数量 和 状态 获取函数  

function pumpNumGet() {
    $.ajax({
        url: '/V_YCJK/SearchZLB_Count',

        success: function (data) {
            var dataJSON = JSON.parse(data);
            dataJSON = dataJSON[0];
            $('.listWrap .numWrap .online').html(dataJSON['zx_cont']);
            $('.listWrap .numWrap .offline').html(dataJSON['lx_cont']);
            $('.listWrap .numWrap .baojing').html(dataJSON['bj_cont']);
            $('.listWrap .numWrap .all').html(dataJSON['all_cont']);
        }
    });
}


//左侧列表 获取数据 函数

function leftListDataGet(jzName, jzState) {

    //alert(pageIndex);
    $.ajax({
        url: '/V_YCJK/Search_OnLineList',
        data: {
            'JZName': jzName,
            'State': jzState,
            'pageIndex': pageIndex,
            'pageSize': 50
        },
        success: function (data) {
            var dataJSON = JSON.parse(data);
            dataJSON = dataJSON.data;
            $('.listWrap .listItemWrap').empty();
            var str = '';
            $(dataJSON).each(function (ind, val) {
                //  console.log(val);
                var status = '';
                var statusTxt = '';
                switch (val.FIsAlarm) {
                    case 0: {
                        switch (val.FOnLine) {
                            case 0: {
                                status = 'listItemPicSta0';
                                // statusTxt = '离线';
                                break;
                            }
                            case 1: {
                                status = 'listItemPicSta1';
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
                        status = 'listItemPicSta2';
                        //statusTxt = '报警';
                        break;
                    }
                }


                //alert(val.PumpJZName);
                if (val.FUpdateDate) {
                    var FUpdateDate = (val.FUpdateDate).replace('T', ' ');
                }


                str += '<li class="listItem clearfix" data-pumpId="' + val.pumpID + '" data-mark="' + val.BaseID + '">' +
                    '<div class="listItemPic ' + status + '"></div>' +
                    '<div class="listAddStatu clearfix">' +
                   '     <div class="listItemAddress">' + val.PumpJZName + '</div>' +
                  '      <div class="listItemStatus listItemStatus1" data-FDTUCode="' + val.FDTUCode + '" data-PName="' + val.PName + '" data-FUpdateDate="' + FUpdateDate + '" data-PumpJZArea="' + val.PumpJZArea + '"></div>' +   //' + statusTxt + '
                 '   </div>' +
                '</li>';

            });
            //alert(str);
            $('.listWrap .listItemWrap').append(str);
        }
    });
}


//左侧 列表 滚动 获取 数据函数
function leftListDataScrollGet() {
    scrollOnoff = false;
    pageIndex++;

    // alert(pageIndex);
    var searchTxt = $.trim($('.listWrap .searchTxt').val());
    //alert(searchTxt);
    var state;
    var index = $('.listWrap .statusWrap li.active').index();

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


    $.ajax({
        url: '/V_YCJK/Search_OnLineList',
        data: {
            'JZName': searchTxt,
            'State': state,
            'pageIndex': pageIndex,
            'pageSize': 50
        },
        success: function (data) {
            var dataJSON = JSON.parse(data);
            dataJSON = dataJSON.data;
            var str = '';
            $(dataJSON).each(function (ind, val) {
                var status = '';
                var statusTxt = '';
                switch (val.FIsAlarm) {
                    case 0: {
                        switch (val.FOnLine) {
                            case 0: {
                                status = 'listItemPicSta0';
                                //statusTxt = '离线';
                                break;
                            }
                            case 1: {
                                status = 'listItemPicSta1';
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
                        status = 'listItemPicSta2';
                        //statusTxt = '报警';
                        break;
                    }
                }
                var FUpdateDate = (val.FUpdateDate).replace('T', ' ');
                str += '<li class="listItem clearfix"  data-pumpId="' + val.pumpID + '" data-mark="' + val.BaseID + '">' +
                    '<div class="listItemPic ' + status + '"></div>' +
                    '<div class="listAddStatu clearfix">' +
                   '     <div class="listItemAddress">' + val.PumpJZName + '</div>' +
                  '     <div class="listItemStatus listItemStatus1" data-FDTUCode="' + val.FDTUCode + '" data-PName="' + val.PName + '" data-FUpdateDate="' + FUpdateDate + '"  data-PumpJZArea="' + val.PumpJZArea + '"></div>' +
                 '   </div>' +
                '</li>';


            });
            scrollOnoff = true;
            //alert(str);
            $('.listWrap .listItemWrap').append(str);
        }
    });
}

//左侧  在线 离线 全部  按钮的  点击函数

function leftOnOffAllFn() {
    $('.listWrap .statusWrap li').click(function () {
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
            $('.listWrap .searchTxt').val('');
            leftListDataGet('', state);

        }
    });
}

//左侧 搜索 按钮的  点击 搜索事件

function leftSearchClickFn() {
    $('.listWrap .listSearchBtn').click(function () {
        var searchTxt = $.trim($('.listWrap .searchTxt').val());
        //alert(searchTxt);

        var index = $('.listWrap .statusWrap li.active').index();

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

        leftListDataGet(searchTxt, state);
    });
}



//左侧 泵房  列表2  的 点击 收缩展开事件

function leftbengfangList2Fn() {
    $('body').delegate('.bengfangListWrap2 .listItemWrapZu>li', 'click', function () {
        if ($(this).hasClass('active')) {
            $(this).removeClass('active');
            $(this).find('.childList').slideUp();
            $(this).find('.arrowUpDown').removeClass('active');
        } else {
            $(this).addClass('active');
            $(this).find('.childList').slideDown();
            $(this).find('.arrowUpDown').addClass('active');
        }
    });
}


//左侧 泵房  列表1  的  中泵房点击 函数

function leftWuFenZubengfangChildListItemClick() {
    $('body').delegate('.bengfangListWrap .listItemWrap>li', 'click', function (ev) {
        var index = $(this).index();
        var random = Math.random();
        var pumpId = $(this).attr('data-pumpId');
        //var mark = $(this).attr('data-mark');
        $('.mainIframe').attr('src', F.baseUrl + 'YCJK/V_YCJK/EGBFSB?pumpId=' + pumpId + '&jzId=' + '');
        $('.leftNav li[data-cla="bengfang"]').addClass('active').siblings().removeClass('active');

        ev.stopPropagation();

    });


}

//左侧 泵房  列表2  的  中泵房点击 函数u
function leftbengfangChildListItemClick() {
    $('body').delegate('.bengfangListWrap2 .listItemWrapZu .childList li', 'click', function (ev) {
        var index = $(this).index();
        var random = Math.random();
        var pumpId = $(this).attr('data-pumpId');
        //var mark = $(this).attr('data-mark');
        $('.mainIframe').attr('src', F.baseUrl + 'YCJK/V_YCJK/EGBFSB?pumpId=' + pumpId + '&jzId=' + '');
        $('.leftNav li[data-cla="bengfang"]').addClass('active').siblings().removeClass('active');

        ev.stopPropagation();

    });


}


//左侧 泵房  列表2 和 列表1  的  中泵房  机组分组数据获得函数

function leftbengfangChildJizuFenzuDataGet(pumpName) {

    pumpName = pumpName ? pumpName : '';
    //alert(wuFenZuPageIndex);
    //alert(pageIndex);
    $.ajax({
        url: '/V_YCJK/Search_Pump',
        data: {
            'pumpName': pumpName,
            'pageIndex': wuFenZuPageIndex,
            'pageSize': 50
        },
        success: function (data) {
            var dataJSON = JSON.parse(data);
            dataJSON = dataJSON.data;

            // console.log(dataJSON);
            //console.log(data);
            //console.log('-----------------------------');

            if (dataJSON[0].jsonGroupName) {
                $('.bengfangListWrap').css('display', 'none');
                $('.bengfangListWrap2').css('display', 'block');
                //alert(1);
                keHuZhuangTaiFenzuData = dataJSON[0].jsonGroupName;
                keHuZhuangTaiBengData = dataJSON[1].jsonData['data'];
                //console.log(keHuZhuangTaiFenzuData);
                //console.log(keHuZhuangTaiBengData);

                leftbengfangChildJizuFenzuDatause(keHuZhuangTaiFenzuData, keHuZhuangTaiBengData);
            } else {

                $('.bengfangListWrap').css('display', 'block');
                $('.bengfangListWrap2').css('display', 'none');

                wuFenZuArr = dataJSON;
                leftbengfangChildJizuWuFenzuDatause(wuFenZuArr);

            }


        }
    });
}




//左侧 无分组  列表一  搜索 按钮的  点击 搜索事件

function leftWufenZuSearchClickFn() {
    $('.bengfangListWrap .listSearchBtn').click(function () {
        var searchTxt = $.trim($('.bengfangListWrap .searchTxt').val());
        //alert(searchTxt);


        wuFenZuPageIndex = 0;
        leftbengfangChildJizuFenzuDataGet(searchTxt);
    });
}


//左侧 分组  列表二  搜索 按钮的  点击 搜索事件

function leftFenZuSearchClickFn() {
    $('.bengfangListWrap2 .listSearchBtn').click(function () {

        var searchTxt = $.trim($('.bengfangListWrap2 .searchTxt').val());
        //alert(searchTxt);



        leftbengfangChildJizuFenzuDataGet(searchTxt);
    });
}




//左侧 泵房 列表1  的 泵房无分组  数据 使用函数
function leftbengfangChildJizuWuFenzuDatause(bengDataArr) {

    $('.bengfangListWrap .listItemWrap').empty();
    var str = '';
    $.each(bengDataArr, function (ind, val) {

        var GName = val.G_Name;
        var childStr = '';

        str += '<li class="listItem clearfix" data-pumpId="' + val.ID + '">' +
                   ' <div class="listItemPic hide"><div class="listItemPicBg"></div></div>' +
                    '<div class="listAddStatu clearfix">' +
                     '   <div class="listItemAddress">' + val.PName + '</div>' +
                      '  <div class="listItemStatus listItemStatus1" data-FDTUCode="' + val.PCode + '" data-PName="' + val.PName + '" >' +

                       ' </div>' +
                    '</div>' +
                '</li>';
    });


    $('.bengfangListWrap .listItemWrap').append(str);

}

//左侧 泵房  列表2  的  中泵房  机组分组数据  使用 函数

function leftbengfangChildJizuFenzuDatause(fenZu, bengDataArr) {
    $('.bengfangListWrap2 .listItemWrapZu').empty();
    var str = '';
    $.each(fenZu, function (ind, val) {
        var GID = val.ID;
        var GName = val.G_Name;
        var childStr = '';

        for (var i in bengDataArr) {

            if (bengDataArr[i].G_ID == GID) {
                childStr += '<li class="childListItem" data-pumpId="' + bengDataArr[i].ID + '" >' +
                        '<div class="childListItemPic"></div>' +
                        '<div class="childListName">' + bengDataArr[i].PName + '</div>' +
                       ' <div class="childListWin" data-FDTUCode="' + bengDataArr[i].PCode + '" data-PName="' + bengDataArr[i].PName + '" ></div>' +
                    '</li>';

            }
        }

        if (childStr != '') {
            str += '<li class="listItem clearfix active">' +
                '<div class="listItemPic">' +
                 '   <div class="listItemPicBg"></div>' +
                '</div>' +
                '<div class="listAddStatu clearfix">' +
                 '   <div class="listItemAddress">' + val.G_Name + '</div>' +
                  '  <div class="listItemStatus">' +
                   '     <div class="arrowUpDown active"></div>' +
                    '</div>' +
                '</div>' +
                '<ul class="childList active">' + childStr + '</ul>' +
           ' </li>';
        }

    });
    $('.bengfangListWrap2 .listItemWrapZu').append(str);

}


//左侧 无分组列表1 滚动 获取 数据函数
function leftWuFenZuListDataScrollGet() {

    wuFenZuScrollOnoff = false;
    wuFenZuPageIndex++;


    var searchTxt = $.trim($('.bengfangListWrap .searchTxt').val());
    // console.log(searchTxt);


    $.ajax({
        url: '/V_YCJK/Search_Pump',
        async: false,
        data: {
            'pumpName': searchTxt,
            'pageIndex': wuFenZuPageIndex,
            'pageSize': 50
        },
        success: function (data) {
            var dataJSON = JSON.parse(data);
            dataJSON = dataJSON.data;
            var str = '';
            $(dataJSON).each(function (ind, val) {
                var status = '';
                var statusTxt = '';

                str += '<li class="listItem clearfix" data-pumpId="' + val.ID + '">' +
                   ' <div class="listItemPic hide"><div class="listItemPicBg"></div></div>' +
                    '<div class="listAddStatu clearfix">' +
                     '   <div class="listItemAddress">' + val.PName + '</div>' +
                      '  <div class="listItemStatus listItemStatus1" data-FDTUCode="' + val.PCode + '" data-PName="' + val.PName + '" >' +

                       ' </div>' +
                    '</div>' +
                '</li>';


            });
            wuFenZuScrollOnoff = true;
            //alert(str);
            $('.bengfangListWrap .listItemWrap').append(str);
        }
    });
}


//左侧  泵房列表一 的 数量

function pumpWuFenZuNumGet() {
    $.ajax({
        url: '/V_YCJK/Search_PumpCount',

        success: function (data) {
            // console.log(data);
            // console.log(22222);
            var dataJSON = JSON.parse(data);
            dataJSON = dataJSON[0];
            $('.bengfangListWrap .numWrap .all').html(dataJSON['CountS']);
            $('.bengfangListWrap2 .numWrap .all').html(dataJSON['CountS']);
        }
    });
}