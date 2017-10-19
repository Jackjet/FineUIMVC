$(function () {
    v = parseUrl();
   
    itemStateClickFn('.surroundingsValMain>li', '.surroundingsValBoxHeader .checkAll');
    itemStateClickFn('.surroundingsStateMain>li', '.surroundingsStateBoxHeader .checkAll');
    itemStateClickFn('.panalItemBox>li', '.panalHeader .checkAll');

    itemStateClickFn('.dianDongFaMain>div', '');
    itemStateClickFn('.shuiXiangMain>div', '');
    itemStateClickFn('.pressOutWaterMain>div', '');

    itemStateClickFn('.bianPinTop>div', '');
    itemStateClickFn('.bianPinBot>div', '');

    itemStateClickFn('.otherBox>li', '');

    itemStateClickFn('.sheBeiQiTing>li', '');
    
    checkAllFn();
    pumpFn();
    resetState();
});

var v;
var dataItemState = [];
var bengFangHuanJing = [
    'F_hj_yl',
    'F_hj_zd',
    'F_hj_ph',
    'F_hj_sw',
    'F_hj_rjy',
    'F_hj_ddl',
    'F_hj_sd',
    'F_hj_wd',
    'F_hj_wsyw'
];
var bengFangZhuangTai = [
    'F_zt_m',
    'F_zt_d',
    'F_zt_dy',
    'F_zt_pc',
    'F_zt_ups',
    'F_zt_ls',
    'F_zt_hj'
];
var panalShowHide = [
    'F_szmb_m',
    'F_szmb_d',
    'F_szmb_fswd',
    'F_szmb_psb',
    'F_szmb_xdy',
    'F_szmb_dsqh',
    'F_szmb_csyl',
    'F_szmb_ddf'
];

var dianDongFa = [
    'F_ddf_df1',
    'F_ddf_df2',
    'F_ddf_df3',
    'F_ddf_df4'
];
var shuiXiang = [
    'F_sx_sx1',
    'F_sx_sx2',
    'F_sx_sx3',
    'F_sx_sx4'
];
var pressPaiShui = [
    'F_yl_jsyl',
    'F_yl_csyl',
    'F_psb_psb'
];

var bianPinIndex = [
    'F_bpq_1',
    'F_bpq_2',
    'F_bpq_3',
    'F_bpq_4',
    'F_bpq_5',
    'F_bpq_6'
];
var bianPinVal = [
    'F_bpq_yxsj',
    'F_bpq_yxpl',
    'F_bpq_zldy',
    'F_bpq_scdy',
    'F_bpq_wd',
    'F_bpq_gl'
];

var other = [
    'F_qt_jsll1',
    'F_qt_csll1',
    'F_qt_jlll1',
    'F_qt_clll1',
    'F_qt_ljdl',
    'F_qt_jsll2',
    'F_qt_csll2',
    'F_qt_jlll2',
    'F_qt_clll2',
    'F_qt_uady',
    'F_qt_ubdy',
    'F_qt_ucdy',
    'F_qt_iadl',
    'F_qt_ibdl',
    'F_qt_icdl'
];

var qiTing = [
    'F_szmb_sbjjqt',
    'F_szmb_kzbqt',
    'F_szmb_xdsz'
];
//获取 传值
function parseUrl() {

    var url = window.location.href;
    // alert(url);
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
    // alert(arr2);
    return arr2;


}


//  状态点击  函数
function itemStateClickFn(clickNode,checkAll) {
    $(clickNode).click(function () {
        var index = $(this).index();
        //alert(index);
        var dataItemStr = '';
        var real;
        switch (clickNode){
            case '.surroundingsValMain>li': {
                dataItemStr = bengFangHuanJing[index];
                break;
            }
            case '.surroundingsStateMain>li': {
                dataItemStr = bengFangZhuangTai[index];
                break;
            }
            case '.panalItemBox>li': {
                dataItemStr = panalShowHide[index];
                break;
            }
            case '.dianDongFaMain>div': {
                dataItemStr = dianDongFa[index];
                break;
            }
            case '.shuiXiangMain>div': {
                dataItemStr = shuiXiang[index];
                break;
            }
            case '.pressOutWaterMain>div': {
                dataItemStr = pressPaiShui[index];
                break;
            }
            case '.bianPinTop>div': {
                dataItemStr = bianPinIndex[index];
                break;
            }
            case '.bianPinBot>div': {
                dataItemStr = bianPinVal[index];
                break;
            }
            case '.otherBox>li': {
                dataItemStr = other[index];
                break;
            }
            case '.sheBeiQiTing>li': {
                dataItemStr = qiTing[index];
                break;
            }
        }
        if ($(this).hasClass('active')) {
            real = 0;
            var $This = $(this);
            $.ajax({
                url: '/V_YCJK/EditControl',
                data: {
                    'pumpID': v.pumpId,
                    'TableField': dataItemStr,
                    'text': real
                },
                success: function (data) {
                    var dataJSON = JSON.parse(data);
                    if (dataJSON.success) {
                       // alert(111);
                        $This.removeClass('active');
                        //storeDate(v.pumpId, 'F_bpq_yxsj', 0);
                        var showLen = $(clickNode + '.active').length;
                        var allLen = $(clickNode).length;
                        if (showLen == allLen) {
                            $(checkAll).addClass('active');
                        } else {
                            $(checkAll).removeClass('active');
                        }

                    }
                    // alert(dataJSON[0].PName);

                },
                error: function () {

                }
            });

            
           
        } else {

            real = 1;
            var $This = $(this);
            $.ajax({
                url: '/V_YCJK/EditControl',
                data: {
                    'pumpID': v.pumpId,
                    'TableField': dataItemStr,
                    'text': real
                },
                success: function (data) {
                    var dataJSON = JSON.parse(data);
                    if (dataJSON.success) {
                       // alert(111);
                        $This.addClass('active');
                        var showLen = $(clickNode + '.active').length;
                        var allLen = $(clickNode).length;

                        if (showLen == allLen) {
                            $(checkAll).addClass('active');
                        } else {
                            $(checkAll).removeClass('active');
                        }

                    } else {
                        alert(dataJSON.msg);
                    }

                },
                error: function () {

                }
            });

          

        }
    });
}


//checkall的  函数

function checkAllFn() {
    $('.checkAll').click(function () {
       // alert('in');
        var $Mark = '';
        var real;
        if ($(this).hasClass('bfhj')) {
            $Mark = 'bfhj';
        } else if ($(this).hasClass('bfzt')) {
            $Mark = 'bfzt';
        } else if ($(this).hasClass('szmb')) {
            $Mark = 'szmb';
        }
        var $This = $(this);
        if ($(this).hasClass('active')) {
            //alert(v.pumpId);
           // alert($Mark);
            real = 0;

            $.ajax({
                url: '/V_YCJK/EditQXControl',
                data: {
                    'pumpID': v.pumpId,
                    'group': $Mark,
                    'text': real
                },
                success: function (data) {
                    var dataJSON = JSON.parse(data);
                    if (dataJSON.success) {
                      //  alert(111);
                         //    alert(data);
                        $This.removeClass('active');
                        $This.parent().next().find('li').removeClass('active');
                    } else {
                        alert(dataJSON.msg);
                    }

                },
                error: function (data) {
                   // alert('error');
                   // alert(data)
                }
            });

            
        } else {

            real = 1;
            //alert(33);
            $.ajax({
                url: '/V_YCJK/EditQXControl',
                data: {
                    'pumpID': v.pumpId,
                    'group': $Mark,
                    'text': real
                },
                success: function (data) {
                    var dataJSON = JSON.parse(data);
                    if (dataJSON.success) {
                      //  alert(111);
                       // alert(data);
                        $This.addClass('active');
                        $This.parent().next().find('li').addClass('active');
                    } else {
                        alert(dataJSON.msg);
                    }

                },
                error: function (data) {
                   // alert('error');
                   // alert(data)
                }
            });

        }
    });
}


//水泵的函数

function pumpFn() {
    $('.pumpMain').click(function () {

       
        var real;
        var $This = $(this);
        if ($(this).hasClass('active')) {
            real = 0;
            $.ajax({
                url: '/V_YCJK/EditControl',
                data: {
                    'pumpID': v.pumpId,
                    'TableField': 'F_sb_sb',
                    'text': real
                },
                success: function (data) {
                  //  alert(data);
                    var dataJSON = JSON.parse(data);
                    if (dataJSON.success) {
                        //alert(1);
                        $This.removeClass('active');
                        $This.find('.pumpCheck').removeClass('active');
                        $This.find('.pumpMainPic').removeClass('active');
                        $This.find('.pumpDian').fadeOut();
                        $This.find('.pumpYun').fadeOut();

                    }
                    // alert(dataJSON[0].PName);

                },
                error: function () {

                }
            });

        } else {
            real = 1;
            $.ajax({
                url: '/V_YCJK/EditControl',
                data: {
                    'pumpID': v.pumpId,
                    'TableField': 'F_sb_sb',
                    'text': real
                },
                success: function (data) {
                    var dataJSON = JSON.parse(data);
                    if (dataJSON.success) {
                        $This.addClass('active');
                        $This.find('.pumpCheck').addClass('active');
                        $This.find('.pumpMainPic').addClass('active');
                        $This.find('.pumpDian').fadeIn();
                        $This.find('.pumpYun').fadeIn();

                    }
                    // alert(dataJSON[0].PName);

                },
                error: function () {

                }
            });

           
        }
    });


    $('.pumpDian').click(function (e) {
        var real;
        var $This = $(this);
        if ($(this).hasClass('active')) {
            real = 0;
            $.ajax({
                url: '/V_YCJK/EditControl',
                data: {
                    'pumpID': v.pumpId,
                    'TableField': 'F_sb_dl',
                    'text': real
                },
                success: function (data) {
                    var dataJSON = JSON.parse(data);
                    if (dataJSON.success) {

                        $This.removeClass('active');
                    }
                    // alert(dataJSON[0].PName);

                },
                error: function () {

                }
            });

        } else {
            real = 1;
            $.ajax({
                url: '/V_YCJK/EditControl',
                data: {
                    'pumpID': v.pumpId,
                    'TableField': 'F_sb_dl',
                    'text': real
                },
                success: function (data) {
                    var dataJSON = JSON.parse(data);
                    if (dataJSON.success) {

                        $This.addClass('active');
                    }
                    // alert(dataJSON[0].PName);

                },
                error: function () {

                }
            });
        }
        e.stopPropagation();
    });
    $('.pumpYun').click(function (e) {
        var real;
        var $This = $(this);

        if ($(this).hasClass('active')) {
            real = 0;
            $.ajax({
                url: '/V_YCJK/EditControl',
                data: {
                    'pumpID': v.pumpId,
                    'TableField': 'F_sb_yxsj',
                    'text': real
                },
                success: function (data) {
                    var dataJSON = JSON.parse(data);
                    if (dataJSON.success) {

                        $This.removeClass('active');
                    }
                    // alert(dataJSON[0].PName);

                },
                error: function () {

                }
            });
        } else {
            real = 1;
            $.ajax({
                url: '/V_YCJK/EditControl',
                data: {
                    'pumpID': v.pumpId,
                    'TableField': 'F_sb_yxsj',
                    'text': real
                },
                success: function (data) {
                    var dataJSON = JSON.parse(data);
                    if (dataJSON.success) {

                        $This.addClass('active');
                    }
                    // alert(dataJSON[0].PName);

                },
                error: function () {

                }
            });
        }
        e.stopPropagation();
    });
}



//电动阀 函数

function diianDongFaFn() {
    
}

//后台  存值 函数

function storeDate(pumpId,dataItemStr,real) {
    $.ajax({        
         url: '/V_YCJK/EditControl',
            data: {
                'pumpID': pumpId,
                'TableField': dataItemStr,
                'text': real
            },
            success: function (data) {
                var dataJSON = JSON.parse(data);
               // alert(dataJSON.success);
                if (dataJSON.success) {
                
                }
                // alert(dataJSON[0].PName);

            },
            error: function () {

            }
    });
}




//  页面载入时 初始状态 数据获取

function resetState() {
    $.ajax({
        url: '/V_YCJK/Search_Control',
        data: {
            'pumpID': v.pumpId
        },
        success: function (data) {
            var dataJSON = JSON.parse(data);
            if (dataJSON.success) {
                dataItemState = dataJSON.obj[0];
                //console.log(dataItemState);
                resetStateUse(dataItemState);
            } else {
               // alert('noIn');
            }
            // alert(dataJSON[0].PName);

        },
        error: function () { 
          //  alert('error');
        }
    });
}

///页面载入时 初始状态 数据使用

function resetStateUse(dataJson) {
        
    var bengFangHuanJingNum = 0, bengFangZhuangTaiNum = 0,
      panalShowHideNum = 0, dianDongFaNum = 0, shuiXiangNum = 0,
      pressPaiShuiNum = 0, bianPinIndexNum = 0, bianPinValNum = 0,
      otherNum = 0, qiTingNum = 0
    ;

    var surroundingsValLen = $('.surroundingsValMain>li').length;
    var surroundingsStateLen= $('.surroundingsStateMain>li').length;
    var panalItemLen = $('.panalItemBox>li').length;
    $(bengFangHuanJing).each(function (ind,val) {
        var trueOrFalse = dataJson[val];
        
        
        if(trueOrFalse){
            $('.surroundingsValMain>li').eq(ind).addClass('active');
            bengFangHuanJingNum++;
        }else {
            $('.surroundingsValMain>li').eq(ind).removeClass('active');
            
        }
        
    });
    if (bengFangHuanJingNum < surroundingsValLen) {
        $('.surroundingsValMain').prev().find('.checkAll').removeClass('active');
        
    } else if (bengFangHuanJingNum == surroundingsValLen) {
        $('.surroundingsValMain').prev().find('.checkAll').addClass('active');
        
    }

    $(bengFangZhuangTai).each(function (ind, val) {
        var trueOrFalse = dataJson[val];
         

        if (trueOrFalse) {
            $('.surroundingsStateMain>li').eq(ind).addClass('active');
            bengFangZhuangTaiNum++;
        } else {
            $('.surroundingsStateMain>li').eq(ind).removeClass('active');
            
        }
    });
    
     if (bengFangZhuangTaiNum < surroundingsStateLen) {
        $('.surroundingsStateMain').prev().find('.checkAll').removeClass('active');
        
     } else if (bengFangZhuangTaiNum == surroundingsStateLen) {
        $('.surroundingsStateMain').prev().find('.checkAll').addClass('active');
        
    }
    $(panalShowHide).each(function (ind, val) {
        var trueOrFalse = dataJson[val];
         

        if (trueOrFalse) {
            $('.panalItemBox>li').eq(ind).addClass('active');
            panalShowHideNum++;
        } else {
            $('.panalItemBox>li').eq(ind).removeClass('active');
            
        }
    });

    if (panalShowHideNum < panalItemLen) {
        $('.panalItemBox').prev().find('.checkAll').removeClass('active');
        
    } else if (panalShowHideNum == panalItemLen) {
        $('.panalItemBox').prev().find('.checkAll').addClass('active');
        
    }

    $(dianDongFa).each(function (ind, val) {
        var trueOrFalse = dataJson[val];
         

        if (trueOrFalse) {
            $('.dianDongFaMain>div').eq(ind).addClass('active');
        } else {
            $('.dianDongFaMain>div').eq(ind).removeClass('active');
            dianDongFaNum++;
        }
    });

    $(shuiXiang).each(function (ind, val) {
        var trueOrFalse = dataJson[val];
         

        if (trueOrFalse) {
            $('.shuiXiangMain>div').eq(ind).addClass('active');
        } else {
            $('.shuiXiangMain>div').eq(ind).removeClass('active');
            shuiXiangNum++;
        }
    });

    $(pressPaiShui).each(function (ind, val) {
        var trueOrFalse = dataJson[val];
         

        if (trueOrFalse) {
            $('.pressOutWaterMain>div').eq(ind).addClass('active');
        } else {
            $('.pressOutWaterMain>div').eq(ind).removeClass('active');
            pressPaiShuiNum++;
        }
    });
    $(bianPinIndex).each(function (ind, val) {
        var trueOrFalse = dataJson[val];
         

        if (trueOrFalse) {
            $('.bianPinTop>div').eq(ind).addClass('active');
        } else {
            $('.bianPinTop>div').eq(ind).removeClass('active');
            bianPinIndexNum++;
        }
    });
    $(bianPinVal).each(function (ind, val) {
        var trueOrFalse = dataJson[val];
         

        if (trueOrFalse) {
            $('.bianPinBot>div').eq(ind).addClass('active');
        } else {
            $('.bianPinBot>div').eq(ind).removeClass('active');
            bianPinValNum++;
        }
    });
    $(other).each(function (ind, val) {
        var trueOrFalse = dataJson[val];

        if (trueOrFalse) {
            $('.otherBox>li').eq(ind).addClass('active');
        } else {
            $('.otherBox>li').eq(ind).removeClass('active');
            otherNum++;
        }
    });

    $(qiTing).each(function (ind, val) {
        var trueOrFalse = dataJson[val];

        if (trueOrFalse) {
            $('.sheBeiQiTing>li').eq(ind).addClass('active');
        } else {
            $('.sheBeiQiTing>li').eq(ind).removeClass('active');
            qiTingNum++;
        }
    });



    //水泵的  

    var sbTrueOrFalse = dataJson['F_sb_sb'];
    var dlTrueOrFalse = dataJson['F_sb_dl'];
    var yxsjTrueOrFalse = dataJson['F_sb_yxsj'];

   
    if (sbTrueOrFalse) {
        $('.pumpMain').addClass('active');
        $('.pumpMain').find('.pumpCheck').addClass('active');
        $('.pumpMain').find('.pumpMainPic').addClass('active');
        $('.pumpMain').find('.pumpDian').fadeIn();
        $('.pumpMain').find('.pumpYun').fadeIn();
    } else {
        $('.pumpMain').removeClass('active');
        $('.pumpMain').find('.pumpCheck').removeClass('active');
        $('.pumpMain').find('.pumpMainPic').removeClass('active');
        $('.pumpMain').find('.pumpDian').fadeOut();
        $('.pumpMain').find('.pumpYun').fadeOut();
      
    }

    if (dlTrueOrFalse) {
        $('.pumpDian').addClass('active');
       
    } else {
        $('.pumpDian').removeClass('active');
       

    }

    if (yxsjTrueOrFalse) {
        $('.pumpYun').addClass('active');

    } else {
        $('.pumpYun').removeClass('active');


    }


   
}

