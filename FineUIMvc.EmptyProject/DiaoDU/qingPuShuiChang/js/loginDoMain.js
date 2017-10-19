$(function () {
   
    loginFun();

    //登录按钮点击登录
    $('.loginBtn').click(loginFun);
});

function zizMove() {
    $('.zizOpen').animate({ "marginLeft": 0 }, 800, function () {
        topMove();
        bottomMove();
        window.setTimeout(function () {
            $('.zizOpen').hide();
           // loginBoxMove();
        }, 100);

    });
}

function topMove() {
    $('.maskTop').animate({ "marginTop": '-51%' }, 1200);
}
function bottomMove() {
    $('.maskBottom').animate({ "marginBottom": "-51%" }, 1200);
}






//loginFn
function loginFun() {
    $.ajax({
        url: '/DiaoDU/Web/Service/Sys_UserService.ashx?method=getmaptempid',
        success: function (data) {
           // alert(data);
            var data = JSON.parse(data);
            if (data.success) {
                zizMove();
                // alert(1);
                var tempID = data.obj[0].FMapTempID;
                ///alert(tempID);
                if (tempID) {
                   // alert(1);
                    window.setTimeout(function () {
                        $('.loadingBg').addClass('hide');
                        window.location.href = './main.html?FMapTempID=' + tempID
                    }, 1800);
                } else {
                   // alert(2);
                    return;
                }
               
               // window.location.href = './infoIndex.html'
            } else {

                $('.loadingBg').addClass('hide');
                $('.error').removeClass('hide');
            }
        },
       
        error: function (data) {
            if (window.console) {
                console.log(data);
            }
        }
    });
    
};




