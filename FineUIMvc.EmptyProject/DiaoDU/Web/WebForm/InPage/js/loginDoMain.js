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
    $.ajax({
        url: '../../Service/Sys_UserService.ashx?method=CheckLogin',
        data: {
            FName: $('.userName').val(),
            FPassword: $('.userPwd').val()
        },
        beforeSend: loadingFunction,
        success: function (data) {
            
             var data = JSON.parse(data);
           
          
            if (data.result == 1) {
              
                window.location.href = './infoIndex.html'
            } else {
                alert('用户名或者密码错误！');
            }
        },
        complete: loadingMiss,
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
        var $div = $('<div class="loading" style="position:absolute;left: 50%;top:50%;margin-left: -150px;margin-top: -70px;width: 300px;color:black;text-align:center;line-height: 140px;height: 140px;background: rgba(255,255,0);color: white;border-radius: 8px;"><img style="position: relative;top: 56%;left: 15%;" src="./img/load1.gif" alt="loading....">正在加载中...</div>');
        $('body').append($div);
    };
};



