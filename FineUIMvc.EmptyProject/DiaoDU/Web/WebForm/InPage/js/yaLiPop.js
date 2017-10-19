$(function () {
    // yaLiClickOut();
    YcloseBtnClick();
    YaTuBiaoHover();
    YaToBFn();
    YbToAFn();
    YbNavClickFn();
});


//页面点击   出现  流量pop框  

function yaLiClickOut(data, pId,pJSON) {

    if ($('.yaLiBox').hasClass('fadeIn')) {
        $('.yaLiBox').removeClass('fadeIn animated');
        $('.yaLiBox').addClass('fadeOut animated');
        $('.yaLiBox').css('marginTop', '-1000888px');
    } else {

        $('.yaLiBox').css('marginTop', '-279px');
        $('.yaLiBox').removeClass('fadeOut animated');
        $('.yaLiBox').addClass('fadeIn animated');

        yaLiDataUse(data, pId);
        yaLiInfoGet(pJSON);


        yaLiReportList(pJSON);
        yaLiBaoBiaoList(reportYL);
    }




}



//流量框 中的 数据 处理函数
function yaLiDataUse(data, pId) {

    console.log(data);
    console.log('qinAINI');
    var addTime = data.updateTime ? (data.updateTime).replace('T', ' ') : '无';
    $('.yaLiA .timeBox .caiJiTime span').html(addTime);
  
    $('.yaLiA .numberBox .pumpName').html(data.FName);
    $('.yaLiA .numberBox .pumpNumber').html('');

    $('.yaLiA .aContentList .yaLiDianLiang .number').html((data.categary.dianLiang || (data.categary.dianLiang == 0) ? data.categary.dianLiang : ' '));
    $('.yaLiA .aContentList .yaLiShangXian .number').html((data.categary.shangXian || (data.categary.shangXian == 0) ? data.categary.shangXian : ' '));
    $('.yaLiA .aContentList .shiShiYaLi .number').html((data.categary.shiShi || (data.categary.shiShi == 0) ? data.categary.shiShi : ' '));
    $('.yaLiA .aContentList .yaLiXiaXian .number').html((data.categary.xiaXian || (data.categary.xiaXian == 0) ? data.categary.xiaXian : ' '));
   
    //$('.yaLiA .aContentList .yaLi .number').html(' 无');
    //$('.yaLiB .echart_box').attr('title', pId);
}
//关闭按钮 点击 函数

function YcloseBtnClick() {

    $('.yaLiBox .closeBox').click(function () {

        $('.yaLiA').removeClass('bounceOutLeft bounceInLeft animated');
        $('.yaLiB').removeClass('bounceOutLeft bounceInLeft animated');
        $('.yaLiB').addClass('bounceOutLeft animated');
        $('.yaLiA').addClass('bounceInLeft animated');
        $('.yaLiBox').removeClass('fadeIn animated');
        $('.yaLiBox').addClass('fadeOut animated');
        $('.yaLiBox').css('marginTop', '-1000888px');


        $('.yaLiBox .bContentBox .cotentLeftNav .bLeftNavItem:first').trigger('click');

    });
}
//A 面  图表  hover 动画
function YaTuBiaoHover() {
    $('.yaLiBox .aContentList .tubiaoList .tuBiaoItem').hover(function () {

        $(this).addClass('tada animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
            $(this).removeClass('tada animated');
        });
    });

}

//A面  点击 转到 B面

function YaToBFn() {

    $('.yaLiA .numberBox .pumpName').click(function () {
        $('.yaLiA').removeClass('bounceOutLeft bounceInLeft animated');
        $('.yaLiB').removeClass('bounceOutLeft bounceInLeft animated');
        $('.yaLiA').addClass('bounceOutLeft animated');

        $('.yaLiB').addClass('bounceInLeft animated');
       
    });
}

//B面  点击 转到A面

function YbToAFn() {

    $('.yaLiB .backBox').click(function () {

        $('.yaLiA').removeClass('bounceOutLeft bounceInLeft animated');
        $('.yaLiB').removeClass('bounceOutLeft bounceInLeft animated');
        $('.yaLiB').addClass('bounceOutLeft animated');
        $('.yaLiA').addClass('bounceInLeft animated');
    });
}


//b面  nav 的点击 切换函数

function YbNavClickFn() {
    $('.yaLiBox .bContentBox .cotentLeftNav .bLeftNavItem').click(function () {
        var index = $(this).index();
        if ($(this).hasClass('active')) {
            $(this).siblings().removeClass('active');

            return;
        }
        $(this).addClass('active').siblings().removeClass('active');

       
        $('.yaLiBox .bRightUl').css({ 'opacity': 0, 'z-index': 1 });
        $('.yaLiBox .bRightUl').eq(index).css({ 'opacity': 1, 'z-index': 2 });
    });
}




var tableArr=[
    'BASE_FAMEN',
    'BASE_YALI',
    'BASE_LIULIANG',
    'BASE_JIAYAZHAN',
    'BASE_JIAYAZHAN_JZ',
    'BASE_SHUICHANG',
    'BASE_SHUICHANG_JZ',
    'BASE_TIAOFENG'
];

//B面  压力 信息的获得函数

function yaLiInfoGet(pJSON) {
    $.ajax({      
      url: '/V_CDJK/FieldShow',
      cache: false,
      data: {
          tableName: 'BASE_YALI',
      },
      success: function (data) {
          var dataJSON = JSON.parse(data);
          var dataJSONList = dataJSON.obj;

          yaLiInfoUse(dataJSONList,pJSON);
          console.log(dataJSON);
          console.log('haoqinqin');
      },
      error: function (data) { }
    });
}


//B面  压力信息的使用

function yaLiInfoUse(arr,pJSON) {
    if (arr.length < 1) {
        return;
    }

    var str = '';
    $(arr).each(function (ind, val) {
        str += '<li class="bRightItem clearfix" data-name="' + val.FField + '" title="' + pJSON[0][val.FField] + '">' +
                      '    <div class="bRightItemName" >'+val.FName+'</div>'+
                      '    <div class="bRightItemNumber" >' + pJSON[0][val.FField] + ' </div>' +
                      '</li>';
    });

    $('.yaLiB .bContentBox .bRightContentWrap  .bRightUl.info').empty().append(str);
}









//报表  list 生成函数

function yaLiBaoBiaoList(rArr) {
  
    $(".yaLiBox .baoBiaoBox .baoBiaoUl").html('');
    var str = '';

    for (var i = 0; i < rArr.length; i++) {
        str += '<li class="baoBiaoListItem clearfix" data-url="' + rArr[i].url + '"><span class="leftBG"></span class="rightName">' + rArr[i].name + '<span></span></li>';

    }
    
    $(".yaLiBox .baoBiaoBox .baoBiaoUl").append(str);
}

//报表  list 的 图表点击 函数

function yaLiReportList(currentPointJSON) {

    $(".yaLiBox .baoBiaoBox .baoBiaoUl").on("click", "li", function () {

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
            id:12,
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