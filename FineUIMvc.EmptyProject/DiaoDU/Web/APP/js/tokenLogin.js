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

      ;
    };

    loginFun();
})
