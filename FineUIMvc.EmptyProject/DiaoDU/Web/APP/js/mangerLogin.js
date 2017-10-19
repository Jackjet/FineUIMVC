$(function () {


    function parseUrl() {

        var url = window.location.href;
        // alert(url);
        var i = url.indexOf('?');
        //alert(1);
        if (i == -1) { return };
        var queryStr = url.substr(i + 1);
        var arr1 = queryStr.split('&');
        var arr2 = {};
        for (j in arr1) {
            var tar = arr1[j].split('=');

            arr2[tar[0]] = tar[1];
        };
        // alert(0);
        // alert(arr2);
        return arr2;


    }
    var v = parseUrl();
    //alert(v);
    //alert(v.key);
    //登录按钮点击登录函数    
    function loginFun() {

        $.ajax({
            url: '/web/Service/Sys_UserService.ashx?method=mangeruser',
            data: {
                FAppToken: 'muser',
            },
            //beforeSend: loadingFunction,
            success: function (data) {
                console.log(data);
                // alert(data);
                var data = JSON.parse(data);
                if (data.result == 1) {
                    window.location.href = '/web/WebForm/Baidu/mobileSelectList.html'
                } else {
                    alert('登录失败！');
                }
            },
            //complete: loadingMiss,
            error: function (data) {
                //alert(0);
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

    loginFun();
})
