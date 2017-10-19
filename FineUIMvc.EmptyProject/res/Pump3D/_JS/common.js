
var com = com || {};
//格式化时间成毫秒
com.formatMS = function (datetime) {
    var datetime = datetime;
    datetime = datetime.replace(new RegExp("-", "gm"), "/");
    return (new Date(datetime)).getTime() + 8 * 60 * 60 * 1000;
}
//得到请求页面的值
com.getRequest = function (name, url) {
    var url = url || window.location.href;
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.split("?")[1];
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = decodeURI(strs[i].split("=")[1]);           
        }
    }
    return theRequest[name];
};
com.getRequests = function () {
    var url = url || window.location.href;
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.split("?")[1];
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = decodeURI(strs[i].split("=")[1]);
        }
    }
    return theRequest;
};

