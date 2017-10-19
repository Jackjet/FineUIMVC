$(function () {
    

    //页面数据获取

    

    var data_timer;
            var $pumpName = $('.top_ul li .pumpName i');
            var $pumpName_len = $('.top_ul li .pumpName i').length;
            var $flow_out = $('.flow_out .flow_value');
            var $flow_out_len = $('.flow_out .flow_value').length;
            var $aperture_value = $('.aperture_value');


            var $flow_in = $('.flow_in .flow_value');
            var $flow_in_len = $('.flow_out .flow_value').length;

            var $total = $('.total_number');

            var $current = $('.current_number');

            var $on_off_box = $('.on_off_box div');


            var $em = $('.top_ul li em');

             data_load();
            

             data_timer = setInterval(function () {
                 data_load();
                 //alert(0)
             }, 3000);
        

  
    function data_load() {
        $.ajax({
            url: '/web/Service/T_PumpDataService.ashx?method=getjygfkzdata',
            cache: false,
            success: function (data) {
                //alert(data);
                console.log(data);
                var data = JSON.parse(data);
                console.log(data);
                data = data.data;





                //总瞬时 流量
                $($total[0]).html(data[0].TotlPV1);
                $($current[0]).html(data[0].PV1);
                $($total[1]).html(data[3].TotlPV1);
                $($current[1]).html(data[3].PV1);
                $($total[2]).html(data[3].TotlPV2);
                $($current[2]).html(data[3].PV2);
                //开度
                $($aperture_value[0]).html(data[6].F40061);
                $($aperture_value[1]).html(data[6].F40075);
                for (var i = 0; i < $pumpName_len; i++) {
                    // alert($pumpName[i]);
                    $($pumpName[i]).removeClass().addClass("pumpState" + data[i].F40007);
                    $($flow_out[i]).html(data[i].F40005);
                    $($flow_in[i]).html(data[i].F40006);
                    //左侧开关状态
                    $($on_off_box[i]).removeClass().addClass("on_off" + data[i].F40013);

                    //右侧 显示 装态
                    $($em[i]).removeClass().addClass("on_off" + data[i].fisonline);

                }



            },
            error: function (data) {
                alert('fuck');
            }
        });

    }
        
  



    //开度设置
	$('.aperture_set1').click(function(){
		layer.open({
		    content: '设置开度：<input style="width: 70%;height: .62rem;font-size: .38rem;color: #000;border-radius: 5px;text-indent: 18px;outline: none; class="aperture_input aperture_set1_input" max="100" min="1" placeholder="请输入1-100的数值" type="number"><br />'
    		, btn: '确定'
            , success: function (elem) {
                //console.log(elem);
            }
    		, yes: function (index) {
    		    var $input_box1 = $($(".layui-m-layer[index=" + index + "]"));   		   
    		    var $input1 = $input_box1.find('input').eq(0);
    		   
    		    console.log($input1.val());
    		    console.log($(".layui-m-layer[index=" + index + "]"));
    		    //$($input1).click(function () {
    		    //    alert('afafafa');
    		    //});

    		    if (Number($input1.val()) < 1) {

    		        alert('请输入大于0的数字！');
    		        return;
    		    } else if (Number($input1.val()) > 100) {
    		        alert('请输入小于100的数字！');
    		        return;
    		    }
      			
      			layer.close(index);
    			}
		});
	})
$(document).delegate('.aperture_input','keyup blur',function(){
    this.value = this.value.replace(/[^0-9-]+/, '');
    
  
    
});

	$('.aperture_set2').click(function(){
		layer.open({
		    content: '设置开度：<input style="width: 70%;height: .62rem;font-size: .38rem;color: #000;border-radius: 5px;text-indent: 18px;outline: none; class="aperture_input aperture_set2_input" placeholder="请输入1-100的数值" max="100" min="1" type="number"><br />'
    		,btn: '确定'
    		,yes: function(index){
    		    var $input_box2 = $($(".layui-m-layer[index=" + index + "]"));
    		    var $input2 = $input_box2.find('input').eq(0);

    		    console.log($input2.val());
    		    //$($input1).click(function () {
    		    //    alert('afafafa');
    		    //});

    		    if (Number($input2.val()) < 1) {

    		        alert('请输入大于0的数字！');
    		        return;
    		    } else if (Number($input2.val()) > 100) {
    		        alert('请输入小于100的数字！');
    		        return;
    		    }
      			layer.close(index);
    		}
		});
	})
	
	
	$('.on_off').click(function(){
		var ind=$('.on_off').index(this);
		if($(this).hasClass('off')){
			layer.open({
				content:'您确定要打开此阀门吗？'
				,btn:['打开','返回']
				,yes: function(index){	
					layer.close(index);
					console.log($(this))
					alert(ind);
					$('.on_off').eq(ind).removeClass('off');
					$('.on_off').eq(ind).addClass('border2');
				}
			});
		}else{
			layer.open({
			content:'您确定要关闭此阀门吗？'
			,btn:['关闭','返回']
			,yes: function(index){
				layer.close(index);
				console.log($(this));
				$('.on_off').eq(ind).addClass('off');
				$('.on_off').eq(ind).addClass('border1');
			}
		});
		}
		
	});

	
	
})
