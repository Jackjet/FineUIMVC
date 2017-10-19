$(document).ready(function(){
	//click list 
	var searchText = $("#searchText").val();
	var currentPage = $("#currentPage").html();
	var pageIndex = currentPage-1;
	var pageSize = 20;
	var totalPage = $("#totalPage").html();
	var state = '';
	var pumpJzId = '';
	$("#pageSize").html(pageSize);
	$(".searchBtn").click(function(){
		searchText = $("#searchText").val();
		loadTable();
	});
	$(".table2_wrap").on('click', "tr", function () {
	    if (!$(".myTable tr").hasClass("active") && pumpJzId=='')
	    {
	        $(".sectionBox1").hide();
	        $(".sectionBox2").show();
	        radialBar();
	        showChart();
	    }
	    $(this).addClass("active").siblings().removeClass("active");
	    pumpJzId = $(this).children("td[data-id=pumpJZId]").html();
	    getWaterElec(pumpJzId);
	    get7daysWaterUse(pumpJzId);
	    get7daysElecUse(pumpJzId);
	    getInOutWaPress(pumpJzId, 10);
	});
	
	//分页
	$(".page_box").on('click', 'a#nextPage', function () {
              
        if (pageIndex == (totalPage - 1)) {
            alert('当前为最后一页');
        } else {
            pageIndex++;
            $("#currentPage").html(pageIndex+1);
            loadTable();
        }
    });
    $(".page_box").on('click', 'a#prevPage', function () {      
        if (!(pageIndex == 0)) {
            pageIndex--;
            $("#currentPage").html(pageIndex +1);
            loadTable() 
        } else {
            alert('当前为第一页');
        }
    });
    $(".page_box").on('click', 'a#firstPage', function () {     
        if (!(pageIndex == 0)) {
            pageIndex = 0;
            $("#currentPage").html(pageIndex + 1);
            loadTable();
        } else {
            alert('当前为第一页');
        }
    });
    $(".page_box").on('click', 'a#lastPage', function () {       
        if (pageIndex == totalPage - 1) {
            alert('当前为最后一页');

        } else {            
            pageIndex = totalPage - 1;
            // alert(pageIndex);
            $("#currentPage").html(pageIndex + 1);
           loadTable();
        }
    });

	function formatDate() {
        var d = new Date();
        var dM = (d.getMonth() + 1).toString().replace(/^(\d)$/, '0$1');
        var dD = d.getDate().toString().replace(/^(\d)$/, '0$1');         
        var dateTemp = d.getFullYear() + "-" + (dM) + "-" +dD;
        return dateTemp;
    }

	$(".search_wrap button").each(function(i,v){
	    $(this).click(function () {
	        searchText = $("#searchText").val();
	        pageIndex = 0;
			var index = i;			
			$(this).addClass("active").siblings().removeClass("active");
			switch (index) {
			    case 0: {
			        state = 1;
			        loadTable();
			        break;
			    }
			    case 1: {
			        state = 0;
			        loadTable();
			        break;
			    }
			    case 2: {
			        state = 2;
			        loadTable();
			        break;
			    }
			    case 3: {
			        state = '';
			        loadTable();
			        break;
			    }
			}
			loadTable();
		});
	});
	$("#showHideBtn").click(function(){
		var winW = $(window).width();
		console.log(winW);
		var leftW = $('.chart_box').width();				       	    
	    if(winW<=1366)
	    {
	    	if ($(this).hasClass('show')) {	
	    		$('.list_wrap').css({ 'width': winW -75 });
	        $('.chart_box').animate({ 'marginRight': -leftW });
	        $(this).removeClass('show');				            						            		       		
	    	} else {	    	
	       	$('.chart_box').animate({ 'marginRight': 0 });
	       	/*$('.list_wrap').animate({ 'width': winW-leftW -75 },function(){    	
	        	
	        });*/
	        $(this).addClass('show');		     
	    }
	    }else if(winW>1366){
	    	 if ($(this).hasClass('show')) {	    	
	        $('.chart_box').animate({ 'marginRight': -leftW },function(){	        	       	             		
       			$('.list_wrap').animate({ 'width': $(".chart_box").offset().left -75 });
       			//$('.list_wrap').animate({ 'width': winW-leftW -77 });
	        });
	        $(this).removeClass('show');				            						            		       		
	    } else {	    		       
	       $('.list_wrap').animate({ 'width': winW-leftW -75 },function(){
	        	
	           //$('.list_wrap').animate({ 'width': $(".chart_box").offset().left -68 });
	        	$('.chart_box').animate({ 'marginRight': 0 });
	        });
	        $(this).addClass('show');		     
	    }
	    }
	});
	addScroll();
	function addScroll() {
	    $('.table2_wrap').mCustomScrollbar({
	        scrollButtons: {
	            enable: true,
	            scrollType: "continuous",
	            scrollSpeed: 20,
	            scrollAmount: 40
	        },
	        axis: "yx",
	        set_width: false,
	        scrollbarPosition: "inside",
	        theme: "minimal-dark",
	        advanced: { autoExpandHorizontalScroll: true },
	        callbacks: {
	            whileScrolling: function () {
	                var $that = this.mcs.left;
	                $('.table1').css('left', $that);
	                $('.table1_wrap').css('width', '100%');

	            },
	            onUpdate: function () {
	              /*  var $that = $('#mCSB_1_container').css('left');
	                $('.table1').css('left', $that);
	                $('.table1_wrap').css('width', '100%');
	                tableClone();*/

	            }
	        }
	    });
	}
	
	
	function tableClone() {
	    if ($('.table1 thead').next()) {
	        $('.table1 thead').next().remove();
	    }
	    $('.table1 thead').after($('.table2 tbody').clone());
	};
	

	loadTable();
	setInterval(function () {
	    loadTable();
	}, 10000);
	function loadTable() {
        $.ajax({ 
        	url: '/V_YCJK/Search_ReportList',
            data:{
                "pageIndex": pageIndex,
                "pageSize": pageSize,
                "State": state,
                "JZName":searchText
            },
            dataType: 'JSON',
            beforeSend: loadingFunction,
            success: function (data) {
                console.log(data);
                console.log(data.data[0].jsonName);
                dealTheadFn(data.data[0].jsonName);
                console.log(data.data[1].jsonData.data);
                dealTbodyFn(data.data[1].jsonData.data);
                console.log('vvvvvvvv');
                dealPage(data.data[1].jsonData.total);
                tableClone();
                addScroll();
            },
            complete: loadingMiss,
            error: function (data) {
                console.log('错误：' + data.responseText);
            }
        });
	}
	function loadingFunction() {
	    var $div = $('<div class="loading" style="position:absolute;left: 50%;top:50%;margin-left: -150px;margin-top: -70px;width: 300px;color:black;text-align:center;line-height: 140px;height: 140px;background: rgba(255,255,0);color: white;border-radius: 8px;"><img style="position: relative;top: 56%;left: 15%;" src="/res/YCJK/img/load1.gif" alt="loading....">正在加载中...</div>');
	    $('body').append($div);
	};
	function loadingMiss() {
	    $('.loading').remove();
	};
	
    //处理表头
	var theadJson = {};
	function dealTheadFn(thData) {
	    $(".table1 thead tr").empty();
	    $(".table2 thead tr").empty();
	     theadJson = {};
	     var tempStr = '<th class="thHide"></th><th class="thHide"></th><th class="thHide" data-id="pName"></th><th class="thHide"  data-id="jzName"></th><th class="thHide"  data-id="jzArea"></th>';
	    for (var i = 0; i < thData.length; i++) {
	        var tdData = thData[i].split(':'); 
	        theadJson[tdData[1]] = tdData[0];
	    }
	    tempStr += dealTheadTdFn(theadJson);
	   // tempStr += '<th><i class="setBtn"></i></th>'
	    $(".table1 thead tr").append(tempStr);
	    $(".table2 thead tr").append(tempStr);
	}

	function dealTheadTdFn(dealData) {
	    var thStr = '';
	    for (var i in dealData) {
	        thStr += '<th class="' + i + '">' + dealData[i] + '</th>';
	    }
	    return thStr;
	}
	function dealTbodyFn(tbData) {
	    $(".table2 tbody").empty();
	    var tbStr = '';
	    for (var i = 0; i < tbData.length; i++) {
	        var tempStr = '<td class="thHide" data-id="pumpJZId" data-value="' + tbData[i].pumpJZId + '">' + tbData[i].pumpJZId + '</td>\
                                        <td class="thHide" data-id="pumpID" data-value="' + tbData[i].pumpID + '">' + tbData[i].pumpID + '</td>\
                                        <td class="thHide" data-id="pName">' + tbData[i].pName + '</td>\
                                        <td class="thHide" data-id="jzName">' + tbData[i].jzName + '</td>\
                                        <td class="thHide" data-id="jzArea">' + tbData[i].jzArea + '</td>';
	        for (var key in theadJson) {
	          
	            if (key == "FOnLine") {  
	                var onOff = tbData[i][key];
	            }
	            switch (key) {
	                case "FIsAlarm":
	                    var isAlarm = tbData[i][key];
	                    if (isAlarm) {
	                        var tempState = 2;
	                    } else if (onOff) {
	                        var tempState = 1
	                    } else {
	                        var tempState = 0;
	                    }
	                    tempStr += '<td class="status" data-id="FIsAlarm"><i class="status_' + tempState + '"></i></td>';
	                    break;
	                case "InOutWaPa":
	                    var inOutV = tbData[i][key].split("/");
	                    tempStr += '<td class="inOutPress" data-id="' + key + '"><span class="pressIn alarm">' + inOutV[0] + '</span><span class="pressOut">' + inOutV[1] + '</span></td>';
	                    break;
	                case "PumpJZName":
	                   
	                    tempStr += '<td class="equipName" data-id="' + key + '" title="' + tbData[i][key] + '"><a>' + tbData[i][key] + '</a></td>';
	                    break;
	                case "FUpdateDate":
	                    var createDate = '';
	                    if (tbData[i][key] !== null) {
	                        createDate = tbData[i][key].replace("T", " ")
	                    }
	                    tempStr += '<td data-id="' + key + '">' + createDate + '</td>';
	                    break;
	                case "PActiveState":
	                    var statusStr = dealPumpStaFn(tbData[i][key]);
	                    tempStr += '<td class="pumpStatus" data-id="' + key + '">' + statusStr + '</td>';
	                    break;
	                    /*排水泵运行状态*/
	                case "PDrainage":
	                    var pStatusStr = dealPumpStaFn(tbData[i][key]);
	                    tempStr += '<td class="pumpStatus" data-id="' + key + '">' + pStatusStr + '</td>';
	                    break;
	                case "F41003":
	                    var sysStatusArr = ['手动', '自动', '远程自动'];
	                    tempStr += '<td class="syatemStatus" data-id="' + key + '">' + eval('sysStatusArr[' + tbData[i][key] + ']') + '</td>';
	                    break;
	                    /*门禁状态*/
	                case "F41093":
	                    tempStr += '<td data-id="' + key + '">' + (tbData[i][key] ? (tbData[i][key]=='1' ?'门在打开':'门在关闭'): "") + '</td>';
	                    break;
	                    /*泵房漏水信号*/
	                case "F41094":
	                    tempStr += '<td data-id="' + key + '">' + (tbData[i][key] ? (tbData[i][key] == '1' ? '发生漏水' : '正常') : "") + '</td>';
	                    break;
                        /*破窗报警*/
	                case "F41095":
	                    tempStr += '<td data-id="' + key + '">' + (tbData[i][key] ? (tbData[i][key] == '1' ? '发生报警' : '正常') : "") + '</td>';
	                    break;
	                    /*烟雾报警*/
	                case "F41096":
	                    tempStr += '<td data-id="' + key + '">' + (tbData[i][key] ? (tbData[i][key] == '1' ? '发生火灾' : '正常') : "") + '</td>';
	                    break;
	                    /*泵房照明*/
	                case "F41097":
	                    tempStr += '<td data-id="' + key + '">' + (tbData[i][key] ? (tbData[i][key] == '1' ? '灯开' : '灯关') : "") + '</td>';
	                    break;
                        //交流失电报警
	                case "F41098":
	                    tempStr += '<td data-id="' + key + '">' + (tbData[i][key] ? (tbData[i][key] == '1' ? '设备断电' : '正常') : "") + '</td>';
	                    break;
                        //电源异常
	                case "F41099":
	                    tempStr += '<td data-id="' + key + '">' + (tbData[i][key] ? (tbData[i][key] == '1' ? '异常' : '正常') : "") + '</td>';
	                    break;
	                default:
	                    tempStr += '<td data-id="' + key + '">' + (tbData[i][key] || tbData[i][key] ==0? tbData[i][key] : "") + '</td>';
	                    break;
	            }
	           }
	        tempStr = '<tr>' + tempStr + '</tr>';
	        tbStr += tempStr;	       
	    }
	    $(".table2 tbody").append(tbStr);
	    if (!(pumpJzId == '' )) {
	        var tempStr = $('.myTable tr td[data-value=' + pumpJzId + ']').parent().html();
	        if (tempStr) {
	            $(".myTable tr td[data-value=" + pumpJzId + "]").parent().addClass("active");
	        }
	    }

	    $("th.FIsAlarm").html("状态");
	}

    //泵状态处理
	function dealPumpStaFn(statusData) {
	    if (statusData == null) {
	        return '';
	    }
	    var pumpNumArr = statusData.split('/');
	    var statusStr = '';
	    for (var i = 0; i < pumpNumArr.length; i++) {
	     //   var staKey = pumpNumArr[i].fixed(0);
	        switch (pumpNumArr[i]) {
                case "0":
	            case "1":
	                statusStr += '<span class="offSta"></span>';
	                break;
	            case "2":
	                statusStr += '<span class="onSta"></span>';
	                break;
	            case "3":
	                statusStr += '<span class="alarmSta"></span>';
	                break;
	            default:
	                break;
	        }
	    }
	    return statusStr;
	    //<span class="onSta"></span><span class="offSta"></span><span class="alarmSta"></span>
	}

	function dealPage(total) {
	    totalPage = Math.ceil(total / pageSize);
	    $("#totalNum").html(total);
	    $("#totalPage").html(totalPage);
	    $("#currentPage").html(pageIndex+1);
	}

	function layout() {	  
		var winW = $(window).width();
		var pageWrapTop = $(".page_wrap").offset().top;
		var table2Top = $(".table2_wrap").offset().top;		
		$(".table2_wrap").css({"height":pageWrapTop - table2Top-6});
		if(winW<=1366){
			$(".list_wrap").css({"width":winW -51});
			
		}else if(winW>1366){
			var offsetR = $(".chart_box").offset().left;       
        	$(".list_wrap").css({"width":offsetR -51});
		}
		/*	console.log('layout');
		   console.log($('#mCSB_1_container').position().left);*/
        
		$(".page_wrap").css({ "width": $(".list_wrap").width() - 2 });
	
   }
    layout();
    $(window).resize(function () {     	
    	layout();    	
    	tableClone();
    });
    function dealData(data){
    	console.log(data);
    }
    $("#pageSize").click(function () {
        $(this).css({ "display": "none" })
        $("#editPageIndex").css({ "display": "inline-block" }).focus().html($(this).html());
    });
    $("#editPageIndex").blur(function () {
        $(this).css({ "display": "none" });
        var tempV = $(this).val();
        if (tempV == '' || !(/^\d*$/.test(tempV))) {
            $("#pageSize").css({ "display": "inline-block" });
            $("#editPageIndex").val($("#pageSize").val());
        } else if (/^\d*$/.test(tempV)) {
            $("#pageSize").html(tempV).css({ "display": "inline-block" });
            pageSize = tempV;
            loadTable();
        } 
    });

    //设置参数
    $(".setBtn").click(function () {
       
        var index = layer.open({
            type: 2,
            anim: 3,
            shade: .6,
            title:false,// ['参数设置', 'text-align:center;'],
            shadeClose: true,
            area: ['96%', '96%'],
            content:[ '/YCJK/V_YCJK/ParmSet2','no'],// '/YCJK/Window/pumpWindow?pumpID=' + urlJson["pumpID"] + '&pumpName=' + urlJson["pumpName"],
            end: function () {
                $('.table2_wrap').mCustomScrollbar("destroy");
                layout();
                loadTable();
            //    $(".table2").css("width", $(".table1").width());
               // addScroll();
              //  $('.table2_wrap').mCustomScrollbar("update");
                $('.table1').css('left', 0);
            }
        });
    });
    function rightSectionScroll() {
        $('.sectionBox').mCustomScrollbar({
            scrollbarPosition: "inside",
            theme: "minimal-dark"
        });
    }
   
    initRightChart();
    getJzStatus();
    getPumpJzCount();

   // getWaterUseTop10();
    //  getElecUseTop10();

    rightSectionScroll();

    createReportList(reportEG)
    showReportFn();
    eg_ReportList();
}); 