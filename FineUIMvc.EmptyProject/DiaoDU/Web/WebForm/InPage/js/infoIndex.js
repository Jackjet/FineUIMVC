

$(function () {
    resetLayout();
    $(window).resize(function () {
        resetLayout();
    });
    showHideFn();
    navAnimate();
    changePwdHover();
    exitBtnFn();
    leftClickChange();
    tempSelectList();
    tempSelectFn();
    copyTemp();
    newTemp();
    deleteTemp();
    editTemp();
    exitBtn();


    topNavClick();
    topNavChildClick();

    preViewTemp();


    childListMouseerter();
});

function resetLayout() {
    var $H = $(window).height();
    var $W = $(window).width();
    var hHeader = $('.pageHeader').height();
    $('.pageMain').height($H - hHeader);
   
    var popParentH = $('.popBox').height();
    $('.popMainWrap').height(popParentH - $('.popHeader').height());
   
}

function showHideFn() {
    $('.showHide').click(function () {
        var leftW = $('.pageMainLeft').width();
        var leftF = $('.leftNav').width();
        if ($(this).hasClass('show')) {
            $('.pageMainLeft').animate({ 'marginLeft': 0 });
            $(this).removeClass('show').html('<');
            $('.mapIframe').addClass('pulse animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
                $(this).removeClass('pulse animated');
            });

        } else {
            $('.pageMainLeft').animate({ 'marginLeft': -leftW });
            $(this).addClass('show').html('>');
            $('.mapIframe').addClass('pulse animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
                $(this).removeClass('pulse animated');
            });

        }
    });
}



function navAnimate() {
    $('.headerNav li').mouseenter(function () {

        $(this).find('div').addClass('rotateIn animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
            $(this).removeClass('rotateIn animated');
        });
        $(this).find('span').addClass('bounce animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
            $(this).removeClass('bounce animated');
        });
    })
}


function changePwdHover() {
    $('.changePwd').mouseover(function () {
        $(this).addClass('wobble animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
            $(this).removeClass('wobble animated');
        });
    });
}

function exitBtnFn(e) {
    $('.exitBtn').mouseover(function () {
        $(this).addClass('rubberBand animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
            $(this).removeClass('rubberBand animated');
        });
    });
}

//退出页面
function exitBtn() {
    $('.exitBtn').click(function () {
        window.location.href = "loginDo.html";
        window.history.forward(1);
    });
}

//左侧 切换
function leftClickChange() {
    $('.leftNav li').click(function () {
        var ind = $(this).index();
        if ($(this).hasClass('active')) {
            return;
        } else {
            $(this).addClass('active').siblings().removeClass('active');
            $('.navContentBox>div').eq(ind).removeClass('hide').siblings().addClass('hide');
        }
    });
}


//左侧 chart 

var option0 = {};



//载入模板
function tempSelectList() {
    $.ajax({
        url: '../../Service/Map_Template.ashx?method=SearchTemp',
        async:false,
        success: function (dat) {
            //alert(dat);
            //console.log(dat);
            var data = JSON.parse(dat);
            //alert(data[0].FMapTempName);
            $('.popMain').empty().removeClass('mCustomScrollbar _mCS_1 mCS_no_scrollbar');
            $.each(data, function (i, v) {
                var itemNew = '<li class="popItem clearfix active" data-tepeId="' + v.TempID + '">' +
                                  '<div class="checkMark noChecked"></div>' +
                                    '<div class="itemContent clearfix">' +
                                        '<div class="itemName">' + v.FMapTempName + '</div>' +
                                    '<div class="itemState noChecked">未使用</div>' +
                                  '</div>' +
                              '</li>';

                $('.popMain').append(itemNew);
            });

            popMainScroll();

            
        }
    });
}
function tempSelectFn() {
    $('body').delegate('.popMain li', 'click', function () {
        var ind = $(this).index();
       
       
       // alert(0);
       // alert($(this).attr('data-tepeId'));
        $(this).addClass('active').siblings().removeClass('active');
        $('.checkMark').addClass('noChecked');
        //$('.itemState').addClass('noChecked').html('未使用');
        $('.checkMark').eq(ind).removeClass('noChecked');
        //$('.itemState').eq(ind).removeClass('noChecked').html('使用中');
    });

}


//编辑 模板
function editTemp() {

    $('.changeFile').click(function () {
        var tepId;
        var $len=$('.popMain li').length;
        for (var i = 0; i < $len; i++) {
            if (!$('.popMain li').eq(i).find('.checkMark').hasClass('noChecked')) {
                tepId = $('.popMain li').eq(i).attr('data-tepeId');
            }
        }
        if (!tepId) {
            alert('请选择模板');
            return;
        }
        layer.open({
            type: 2,
            title: false,
            maxmin: false,
            shadeClose: true, //点击遮罩关闭层
            area: ['98%', '96%'],
            content: '/Web/WebForm/MapTools/index.html?mapTempID=' + tepId
        });
    });
}


//copy 模板


function copyTemp() {
    $('.copyFile').click(function () {
        var tepId;
        var $len = $('.popMain li').length;
        for (var i = 0; i < $len; i++) {
            if (!$('.popMain li').eq(i).find('.checkMark').hasClass('noChecked')) {
                tepId = $('.popMain li').eq(i).attr('data-tepeId');
            }
        }
        if (!tepId) {
            alert('请选择模板');
            return;
        }
        $.ajax({
            url: '../../Service/Map_Template.ashx?method=copyTemp',
            data: {
                'FMapTempID': tepId
            },
            success: function (dat) {

                var data = JSON.parse(dat);
               
                if (data.result) {

                    layer.msg('复制成功', { icon: 1 });
                   
                    tempSelectList();
                }
                
            }
        });
       
    });
}


//delete  模板

function deleteTemp() {
    $('.deleteFile').click(function () {

        //if (!confirm("你确定要删除该模板？")) {
        //    return;
        //}

        layer.confirm('你确定要删除该模板？', {
            title: "删除操作",
           
            area: ['300px','200px'],
            btn: ['是的', '取消'] //按钮
        }, function () {
            
        var tepId;
        var $len = $('.popMain li').length;
        for (var i = 0; i < $len; i++) {
           
            if (!$('.popMain li').eq(i).find('.checkMark').hasClass('noChecked')) {
                if (i <= 4) {
                    layer.msg('没有删除此模板权限', { icon: 1 });
                  
                    return;
                }
                tepId = $('.popMain li').eq(i).attr('data-tepeId');
            }
        }
        if (!tepId) {
            layer.msg('请选择模板', { icon: 1 });
            return;
        }
        $.ajax({
            url: '../../Service/Map_Template.ashx?method=deltemp',
            data: {
                'FMapTempID': tepId
            },
            success: function (dat) {
                var data = JSON.parse(dat);

                if (data.result) {

                    layer.msg('删除成功', { icon: 1 });

                    tempSelectList();
                }
               
            }
        });

        }, function () {
            return;
        });

    });
}

//预览模板

function preViewTemp() {
    $('.preView').click(function () {
        var tepId;
        var $len = $('.popMain li').length;
        for (var i = 0; i < $len; i++) {
            if (!$('.popMain li').eq(i).find('.checkMark').hasClass('noChecked')) {
                tepId = $('.popMain li').eq(i).attr('data-tepeId');
            }
        }
        if (!tepId) {
            alert('请选择模板');
            return;
        }

        $('.mapIframe').attr('src', 'qingpuMapShow.html?id=' + tepId);
    });
}

//new   模板

function newTemp() {
    $('.newFile').click(function () {

        var name;

        layer.prompt({ title: '请输入模板名称', area: ['300px', '200px'], formType: 0 }, function (pass, index) {
            layer.close(index);
            name = pass;
      


     
        if (!name) {
            
            return;
        }
        //var tepId;
        //var $len = $('.popMain li').length;
        //for (var i = 0; i < $len; i++) {

        //    if (!$('.popMain li').eq(i).find('.checkMark').hasClass('noChecked')) {
                
        //        tepId = $('.popMain li').eq(i).attr('data-tepeId');
        //    }
        //}
        //if (!tepId) {
        //    alert('请选择模板');
        //    return;
        //}
        $.ajax({
            url: '../../Service/Map_Template.ashx?method=inserttemplate',
            data: {
                'FMapTempName': name
            },
            success: function (dat) {
                var data = JSON.parse(dat);

                if (data.result) {
                    layer.msg('添加成功', { icon: 1 });
                  
                   tempSelectList();
                }

            }
        });

        });

    });
}

//popMain  的  滚动条

function popMainScroll() {    
    $('.popMainWrap').mCustomScrollbar({
        scrollbarPosition: "inside",
        theme: "minimal-dark"
    });
    
}



//上侧 navBAR 子菜单的 点击导航
function topNavChildClick() {
    $('.headerNav li.li_fenqu .childList li').click(function () {

        var index = $(this).index();
        switch (index){
            case 0: {
                layer.open({
                    type: 2,
                    anim: 3,
                    shade: .6,
                    title: false,
                    shadeClose: true, //点击遮罩关闭层
                    area: ['98%', '98%'],
                    content: '../MapTools/listIndex.html'
                });
                break;
            }
            case 1: {
                layer.open({
                    type: 2,
                    anim: 3,
                    shade: .6,
                    title: false,
                    shadeClose: true, //点击遮罩关闭层
                    area: ['98%', '98%'],
                    content: 'timeLine.html'
                });
                break;
            }
            case 2: {
                layer.open({
                    type: 2,
                    anim: 3,
                    shade: .6,
                    title: false,
                    shadeClose: true, //点击遮罩关闭层
                    area: ['706px', '336px'],
                    content: 'fenquIframe.html'
                });
                break;
            }
        }
             
    });

    $('.headerNav li.li_xitong .childList li').click(function () {

        var index = $(this).index();
        switch (index) {
            case 0: {
                var id = '';
                layer.open({
                    type: 2,
                    anim: 3,
                    shade: .6,
                    title: ['压力管理', 'text-align: center;'],
                    shadeClose: true, //点击遮罩关闭层
                    area: ['70%', '80%'],
                    content: '../MapTools/layer_yaliAddMid.html?typeIndex=7' + '&id=' + id,
                    success: function () {
                        console.log('click set');
                    }
                });
                break;
            }
        }

    });
    $('.headerNav li.li_liebiao .childList li').click(function () {

        var index = $(this).index();
        switch (index) {
            case 0: {
                var id = '';
                layer.open({
                    type: 2,
                    anim: 3,
                    shade: .6,
                    title: ['压力列表', 'text-align: center;'],
                    shadeClose: true, //点击遮罩关闭层
                    area: ['70%', '80%'],
                    content: '../MapTools/yaliLiebiao.html?typeIndex=7' + '&id=' + id,
                    success: function () {
                        console.log('click set');
                    }
                });
                break;
            }
        }

    });
}

//上侧 navBAR 导航
var navArr = ['fenqu','tiaofeng','fakong','nenghao'];
function topNavClick() {
    $('.headerNav li').click(function () {

        var index = $(this).index();
        if (index < 5 || index > 7) {
            if (index == 1) {
                $('.mapIframe').attr('src', 'qingpuMapShow.html');
            }
            return;
         }
       
       layer.open({
            type: 2,
            anim: 3,
            shade: .6,
            title: false, 
            shadeClose: true, //点击遮罩关闭层
            area: ['706px', '336px'],
            content: navArr[index-4]+'Iframe.html'
        });
    });
}



//导航 子菜单  的 mouseenter 事件


function childListMouseerter() {
   // alert(12321);
    $('.headerNav li').mouseenter(function(){
        $(this).find('.childList').stop().slideDown().siblings().find('.childList').stop().slideUp();
    });
    $('.headerNav li').mouseleave(function () {
        $(this).find('.childList').stop().slideUp().siblings().find('.childList').stop().slideUp();
    });
}