$(function () {
   // liuLiangClickOut();
    closeBtnClick();
    aTuBiaoHover();
    aToBFn();
    bToAFn();
    bNavClickFn();
    baoBiaoScroll();
   
   
});


//页面点击   出现  流量pop框  

function liuLiangClickOut(data,pId,pJSON) {

    if ($('.liuLiangBox').hasClass('fadeIn')) {
        $('.liuLiangBox').removeClass('fadeIn animated');
        $('.liuLiangBox').addClass('fadeOut animated');
        $('.liuLiangBox').css('marginTop', '-1000888px');
    } else {

        $('.liuLiangBox').css('marginTop', '-279px');
        $('.liuLiangBox').removeClass('fadeOut animated');
        $('.liuLiangBox').addClass('fadeIn animated');

        liuLiangDataUse(data, pId);
        liuLiangBInfoGet(pJSON);
       
        liuLiangReportList(pJSON);
        liuLiangBaoBiaoList(reportLL);
    }




}



//流量框 中的 数据 处理函数
function liuLiangDataUse(data, pId) {

    var addTime = data.updateTime ? (data.updateTime).replace('T',' ') : '无';
    $('.liuLiangA .timeBox .caiJiTime').html(addTime);
    $('.liuLiangA .numberBox .pumpName').html(data.FName);
    $('.liuLiangA .numberBox .pumpNumber').html('');

    
    $('.liuLiangA .aContentList .zhengLeiJi .number').html((data.P01 || (data.P01==0) ? data.P01 : ' '));
    $('.liuLiangA .aContentList .fuLeiji .number').html((data.P02 || (data.P02 == 0) ? data.P02 : ' '));
    $('.liuLiangA .aContentList .yaLi .number').html((data.A03 || (data.A03 == 0) ? data.A03 : ' '));
    $('.liuLiangA .aContentList .shunShiLiuLiang .number').html((data.A01 || (data.A01 == 0) ? data.A01 : ' '));
    $('.liuLiangA .aContentList .dianChiDianYa .number').html((data.A02 || (data.A02 == 0) ? data.A02 : ' '));
    $('.liuLiangA .aContentList .dianYa .number').html((data.V || (data.V == 0) ? data.V : ' '));
    //$('.liuLiangA .aContentList .liuLiang .number').html(' 无');



    $('.liuLiangB .echart_box').attr('title', pId);
}
//关闭按钮 点击 函数

function closeBtnClick() {

    $('.liuLiangBox .closeBox').click(function () {
        
        $('.liuLiangA').removeClass('bounceOutLeft bounceInLeft animated');
        $('.liuLiangB').removeClass('bounceOutLeft bounceInLeft animated');
        $('.liuLiangB').addClass('bounceOutLeft animated');
        $('.liuLiangA').addClass('bounceInLeft animated');
        $('.liuLiangBox').removeClass('fadeIn animated');
        $('.liuLiangBox').addClass('fadeOut animated');
        $('.liuLiangBox').css('marginTop', '-1000888px');

        $('.liuLiangBox .bContentBox .cotentLeftNav .bLeftNavItem:first').trigger('click');


    });
}
//A 面  图表  hover 动画
function aTuBiaoHover() {
    $('.liuLiangBox .aContentList .tubiaoList .tuBiaoItem').hover(function () {

        $(this).addClass('tada animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
            $(this).removeClass('tada animated');
        });
    });


}

//A面  点击 转到 B面

function aToBFn() {

    $('.liuLiangA .numberBox .pumpName').click(function () {
        $('.liuLiangA').removeClass('bounceOutLeft bounceInLeft animated');
        $('.liuLiangB').removeClass('bounceOutLeft bounceInLeft animated');
        $('.liuLiangA').addClass('bounceOutLeft animated');

        $('.liuLiangB').addClass('bounceInLeft animated');
       
    });
   
}

//B面  点击 转到A面

function bToAFn() {

    $('.liuLiangB .backBox').click(function () {

        $('.liuLiangA').removeClass('bounceOutLeft bounceInLeft animated');
        $('.liuLiangB').removeClass('bounceOutLeft bounceInLeft animated');
        $('.liuLiangB').addClass('bounceOutLeft animated');
        $('.liuLiangA').addClass('bounceInLeft animated');
    });
}


//b面  nav 的点击 切换函数

function bNavClickFn() {
    $('.liuLiangBox .bContentBox .cotentLeftNav .bLeftNavItem').click(function () {
        var index = $(this).index();
        if ($(this).hasClass('active')) {
            $(this).siblings().removeClass('active');

            return;
        }
        $(this).addClass('active').siblings().removeClass('active');

        $('.liuLiangBox .bRightUl').css({'opacity': 0,'z-index':1});
        $('.liuLiangBox .bRightUl').eq(index).css({'opacity':1,'z-index': 2});

    });
}


//B面  信息的获得函数

function liuLiangBInfoGet(pJSON) {

    console.log(pJSON);
    console.log('qinqiqndsafdsfsdfsf');
    $.ajax({
        url: '/V_CDJK/FieldShow',
        cache: false,
        data: {
            tableName: 'BASE_LIULIANG',
        },
        success: function (data) {
            var dataJSON = JSON.parse(data);
            var dataJSONList = dataJSON.obj;

            liuLiangBInfoUse(dataJSONList,pJSON);
           
        },
        error: function (data) { }
    });
}


//B面  流量信息的使用

function liuLiangBInfoUse(arr,pJSON) {
    
    if (arr.length < 1) {
        return;
    }

   
    var str = '';
    $(arr).each(function (ind, val) {
        str += '<li class="bRightItem clearfix" data-name="' + val.FField + '" title="' + pJSON[0][val.FField] + '">' +
                      '    <div class="bRightItemName" >' + val.FName + '</div>' +
                      '    <div class="bRightItemNumber" >' + pJSON[0][val.FField] + ' </div>' +
                      '</li>';
    });

    $('.liuLiangB .bContentBox .bRightContentWrap  .bRightUl.info').empty().append(str);
}




function baoBiaoScroll() {
    $('.baoBiaoBox').mCustomScrollbar({
        scrollbarPosition: "inside",
        theme: "minimal-dark"
    });
}



//报表  list 生成函数

function liuLiangBaoBiaoList(rArr) {
   
    $(".liuLiangBox .baoBiaoBox .baoBiaoUl").html('');
    var str = '';
    for (var i = 0; i < rArr.length; i++) {
        str += '<li class="baoBiaoListItem clearfix" data-url="'+rArr[i].url+'"><span class="leftBG"></span class="rightName">'+rArr[i].name+'<span></span></li>';
        
    }
    $(".liuLiangBox .baoBiaoBox .baoBiaoUl").append(str);
}

//报表  list 的 图表点击 函数

function liuLiangReportList(currentPointJSON) {
 
    $(".liuLiangBox .baoBiaoBox .baoBiaoUl").on("click", "li", function () {
       
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
            shade: .6,
            id:11,
            title: false,
            shadeClose: true,
            area: [winW, winH],
            content:urlStr+'?TFname='+TFname+'&baseId='+baseId,
            success: function () {
                //  alert('OK');
            }
        });

    });
}