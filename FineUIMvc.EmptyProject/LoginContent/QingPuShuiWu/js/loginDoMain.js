var msgTipTimer;
$(function () {

    zizMove();




    //回车登录
    $(window).keydown(
        function (event) {
            if (event.keyCode == 13) {
                if ($('.userName').val() != '' && $('.userPwd').val() != '') {
                    $('.loginBtn').click();

                }
            }
        }
   );

    //登录按钮点击登录
    $('.loginBtn').click(loginFun);
});

function zizMove() {
    $('.zizOpen').animate({ "marginLeft": 0 }, 800, function () {
        topMove();
        bottomMove();
        window.setTimeout(function () {
            $('.zizOpen').hide();
            loginBoxMove();
        }, 100);

    });
}

function topMove() {
    $('.maskTop').animate({ "marginTop": '-51%' }, 1200);
}
function bottomMove() {
    $('.maskBottom').animate({ "marginBottom": "-51%" }, 1200);
}


function loginBoxMove() {

    $('.loginInput').animate({ "marginTop": '-182px' }, 900, function () {
        $('.logoBig').animate({ "opacity": "1" });

    });
}


function loginBtnAni1() {
    $('.loginBtn').stop().animate({ "lineHeight": "80px" }, 400, function () {
    });
}
function loginBtnAni2() {
    $('.loginBtn').stop().animate({ "lineHeight": "30px" }, 400, function () {
    });
}






//loginFn
function loginFun() {

    clearTimeout(msgTipTimer);
    var $Fname = $('.userName').val();
    var $FPassword = $('.userPwd').val();

    if ($.trim($Fname) == '') {
        msgTip('请输入用户名');
        return;
    } else if ($.trim($FPassword) == '') {
        msgTip('请输入密码');
        return;
    }
    $.ajax({
        type: "POST",
        url: '/Login/btnLogin_Click',
        data: {
            FName: $Fname,
            FPassword: $FPassword
        },
        // beforeSend: loadingFunction,
        success: function (data) {

            if (data.result == 1) {
                window.location.href = '/Home/Index'
            } else {
                msgTip(data.msg);
            }
        },
        //complete: loadingMiss,
        error: function (data) {
            if (window.console) {
                console.log(data);
            }
        }
    });
    function loadingMiss() {
        $('.loading').remove();
    };
    function loadingFunction() {
        var $div = $('<div class="loading" style="position:absolute;left: 50%;top:50%;margin-left: -150px;margin-top: -70px;width: 300px;color:black;text-align:center;line-height: 140px;height: 140px;background: rgba(255,255,0);color: white;border-radius: 8px;"><img style="position: relative;top: 56%;left: 15%;" src="/img/load1.gif" alt="loading....">正在加载中...</div>');
        $('body').append($div);
    };
};



function msgTip(msg) {

    TweenMax.set('.msgTipBox', { 'scale': 0.2, 'autoAlpha': 0 });
    $('.msgTipBox .msgTxt').html(msg);
    TweenMax.to('.msgTipBox', 0.6, { 'scale': 1, 'autoAlpha': 1 });

    msgTipTimer = setTimeout(function () {
        TweenMax.to('.msgTipBox', 0.6, { 'scale': 0.2, 'autoAlpha': 0 });
    }, 2900);

}



//loginFn
//function loginFun() {


//    $.ajax({
//        type: "POST",
//        url: '/Login/btnLogin_Click',
//        data: {
//            FName: $('.userName').val(),
//            FPassword: $('.userPwd').val()
//        },
//        beforeSend: loadingFunction,
//        success: function (data) {
//            console.log(data);

//            console.log(123);
//            if (data.result == 1) {
//                window.location.href = '/Home/Index'
//            } else {

//                //alert('用户名或者密码错误！');
//            }
//        },
//        complete: loadingMiss,
//        error: function (data) {
//            if (window.console) {
//                console.log(data);
//            }
//        }
//    });
//    function loadingMiss() {
//        $('.loading').remove();
//    };
//    function loadingFunction() {
//        var $div = $('<div class="loading" style="position:absolute;left: 50%;top:50%;margin-left: -150px;margin-top: -70px;width: 300px;color:black;text-align:center;line-height: 140px;height: 140px;background: rgba(255,255,0);color: white;border-radius: 8px;"><img style="position: relative;top: 56%;left: 15%;" src="./img/load1.gif" alt="loading....">正在加载中...</div>');
//        $('body').append($div);
//    };
//};



var msgTipTimer;
$(function () {

    zizMove();




    //回车登录
    $(window).keydown(
        function (event) {
            if (event.keyCode == 13) {
                if ($('.userName').val() != '' && $('.userPwd').val() != '') {
                    $('.loginBtn').click();

                }
            }
        }
   );

    //登录按钮点击登录
    $('.loginBtn').click(loginFun);
});

function zizMove() {
    $('.zizOpen').animate({ "marginLeft": 0 }, 800, function () {
        topMove();
        bottomMove();
        window.setTimeout(function () {
            $('.zizOpen').hide();
            loginBoxMove();
        }, 100);

    });
}

function topMove() {
    $('.maskTop').animate({ "marginTop": '-51%' }, 1200);
}
function bottomMove() {
    $('.maskBottom').animate({ "marginBottom": "-51%" }, 1200);
}


function loginBoxMove() {

    $('.loginInput').animate({ "marginTop": '-182px' }, 900, function () {
        $('.logoBig').animate({ "opacity": "1" });

    });
}


function loginBtnAni1() {
    $('.loginBtn').stop().animate({ "lineHeight": "80px" }, 400, function () {
    });
}
function loginBtnAni2() {
    $('.loginBtn').stop().animate({ "lineHeight": "30px" }, 400, function () {
    });
}






//loginFn
function loginFun() {

    clearTimeout(msgTipTimer);
    var $Fname = $('.userName').val();
    var $FPassword = $('.userPwd').val();

    if ($.trim($Fname) == '') {
        msgTip('请输入用户名');
        return;
    } else if ($.trim($FPassword) == '') {
        msgTip('请输入密码');
        return;
    }
    $.ajax({
        type: "POST",
        url: '/Login/btnLogin_Click',
        data: {
            FName: $Fname,
            FPassword: $FPassword
        },
        // beforeSend: loadingFunction,
        success: function (data) {

            if (data.result == 1) {
                window.location.href = '/Home/Index'
            } else {
                msgTip(data.msg);
            }
        },
        //complete: loadingMiss,
        error: function (data) {
            if (window.console) {
                console.log(data);
            }
        }
    });
    function loadingMiss() {
        $('.loading').remove();
    };
    function loadingFunction() {
        var $div = $('<div class="loading" style="position:absolute;left: 50%;top:50%;margin-left: -150px;margin-top: -70px;width: 300px;color:black;text-align:center;line-height: 140px;height: 140px;background: rgba(255,255,0);color: white;border-radius: 8px;"><img style="position: relative;top: 56%;left: 15%;" src="/img/load1.gif" alt="loading....">正在加载中...</div>');
        $('body').append($div);
    };
};



function msgTip(msg) {

    TweenMax.set('.msgTipBox', { 'scale': 0.2, 'autoAlpha': 0 });
    $('.msgTipBox .msgTxt').html(msg);
    TweenMax.to('.msgTipBox', 0.6, { 'scale': 1, 'autoAlpha': 1 });

    msgTipTimer = setTimeout(function () {
        TweenMax.to('.msgTipBox', 0.6, { 'scale': 0.2, 'autoAlpha': 0 });
    }, 2900);

}



//loginFn
//function loginFun() {


//    $.ajax({
//        type: "POST",
//        url: '/Login/btnLogin_Click',
//        data: {
//            FName: $('.userName').val(),
//            FPassword: $('.userPwd').val()
//        },
//        beforeSend: loadingFunction,
//        success: function (data) {
//            console.log(data);

//            console.log(123);
//            if (data.result == 1) {
//                window.location.href = '/Home/Index'
//            } else {

//                //alert('用户名或者密码错误！');
//            }
//        },
//        complete: loadingMiss,
//        error: function (data) {
//            if (window.console) {
//                console.log(data);
//            }
//        }
//    });
//    function loadingMiss() {
//        $('.loading').remove();
//    };
//    function loadingFunction() {
//        var $div = $('<div class="loading" style="position:absolute;left: 50%;top:50%;margin-left: -150px;margin-top: -70px;width: 300px;color:black;text-align:center;line-height: 140px;height: 140px;background: rgba(255,255,0);color: white;border-radius: 8px;"><img style="position: relative;top: 56%;left: 15%;" src="./img/load1.gif" alt="loading....">正在加载中...</div>');
//        $('body').append($div);
//    };
//};



var msgTipTimer;
$(function () {

    zizMove();




    //回车登录
    $(window).keydown(
        function (event) {
            if (event.keyCode == 13) {
                if ($('.userName').val() != '' && $('.userPwd').val() != '') {
                    $('.loginBtn').click();

                }
            }
        }
   );

    //登录按钮点击登录
    $('.loginBtn').click(loginFun);
});

function zizMove() {
    $('.zizOpen').animate({ "marginLeft": 0 }, 800, function () {
        topMove();
        bottomMove();
        window.setTimeout(function () {
            $('.zizOpen').hide();
            loginBoxMove();
        }, 100);

    });
}

function topMove() {
    $('.maskTop').animate({ "marginTop": '-51%' }, 1200);
}
function bottomMove() {
    $('.maskBottom').animate({ "marginBottom": "-51%" }, 1200);
}


function loginBoxMove() {

    $('.loginInput').animate({ "marginTop": '-182px' }, 900, function () {
        $('.logoBig').animate({ "opacity": "1" });

    });
}


function loginBtnAni1() {
    $('.loginBtn').stop().animate({ "lineHeight": "80px" }, 400, function () {
    });
}
function loginBtnAni2() {
    $('.loginBtn').stop().animate({ "lineHeight": "30px" }, 400, function () {
    });
}






//loginFn
function loginFun() {

    clearTimeout(msgTipTimer);
    var $Fname = $('.userName').val();
    var $FPassword = $('.userPwd').val();

    if ($.trim($Fname) == '') {
        msgTip('请输入用户名');
        return;
    } else if ($.trim($FPassword) == '') {
        msgTip('请输入密码');
        return;
    }
    $.ajax({
        type: "POST",
        url: '/Login/btnLogin_Click',
        data: {
            FName: $Fname,
            FPassword: $FPassword
        },
        // beforeSend: loadingFunction,
        success: function (data) {

            if (data.result == 1) {
                window.location.href = '/Home/Index'
            } else {
                msgTip(data.msg);
            }
        },
        //complete: loadingMiss,
        error: function (data) {
            if (window.console) {
                console.log(data);
            }
        }
    });
    function loadingMiss() {
        $('.loading').remove();
    };
    function loadingFunction() {
        var $div = $('<div class="loading" style="position:absolute;left: 50%;top:50%;margin-left: -150px;margin-top: -70px;width: 300px;color:black;text-align:center;line-height: 140px;height: 140px;background: rgba(255,255,0);color: white;border-radius: 8px;"><img style="position: relative;top: 56%;left: 15%;" src="/img/load1.gif" alt="loading....">正在加载中...</div>');
        $('body').append($div);
    };
};



function msgTip(msg) {

    TweenMax.set('.msgTipBox', { 'scale': 0.2, 'autoAlpha': 0 });
    $('.msgTipBox .msgTxt').html(msg);
    TweenMax.to('.msgTipBox', 0.6, { 'scale': 1, 'autoAlpha': 1 });

    msgTipTimer = setTimeout(function () {
        TweenMax.to('.msgTipBox', 0.6, { 'scale': 0.2, 'autoAlpha': 0 });
    }, 2900);

}



//loginFn
//function loginFun() {


//    $.ajax({
//        type: "POST",
//        url: '/Login/btnLogin_Click',
//        data: {
//            FName: $('.userName').val(),
//            FPassword: $('.userPwd').val()
//        },
//        beforeSend: loadingFunction,
//        success: function (data) {
//            console.log(data);

//            console.log(123);
//            if (data.result == 1) {
//                window.location.href = '/Home/Index'
//            } else {

//                //alert('用户名或者密码错误！');
//            }
//        },
//        complete: loadingMiss,
//        error: function (data) {
//            if (window.console) {
//                console.log(data);
//            }
//        }
//    });
//    function loadingMiss() {
//        $('.loading').remove();
//    };
//    function loadingFunction() {
//        var $div = $('<div class="loading" style="position:absolute;left: 50%;top:50%;margin-left: -150px;margin-top: -70px;width: 300px;color:black;text-align:center;line-height: 140px;height: 140px;background: rgba(255,255,0);color: white;border-radius: 8px;"><img style="position: relative;top: 56%;left: 15%;" src="./img/load1.gif" alt="loading....">正在加载中...</div>');
//        $('body').append($div);
//    };
//};



