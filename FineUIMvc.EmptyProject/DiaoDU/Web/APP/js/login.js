$(function(){
	$('#loginBtn').click(loginFun);
	 //登录按钮点击登录函数    
	function loginFun() {
	  
                $.ajax({
                    url: '/web/Service/Sys_UserService.ashx?method=CheckLogin',
                    data: {
                        FName: $('#userName').val(),
                        FPassword: $('#psd').val()
                    },
                    //beforeSend: loadingFunction,
                    success: function (data) {
                        // console.log(data);
                       // alert(data);
                        var data = JSON.parse(data);
                        if (data.result == 1) {
                            window.location.href = './pressPosition.html'
                        } else {
                            alert('用户名或者密码错误！');
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
})
