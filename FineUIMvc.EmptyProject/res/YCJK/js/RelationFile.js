$(document).ready(function () {
    //click list 
   
    var urlJson = parseUrl();
    var pumpId = urlJson["pumpID"];
    var jzId = urlJson["jzID"];
    console.log(urlJson);
    urlJson["pumpName"] = decodeURIComponent(urlJson["pumpName"]);
    urlJson["jzName"] = decodeURIComponent(urlJson["jzName"]);

   

    function parseUrl() {
        var url = window.location.href;
        //  alert(url);
        var i = url.indexOf('?');
        if (i == -1) { return };
        var queryStr = url.substr(i + 1);
        var arr1 = queryStr.split('&');
        var arr2 = {};
        for (j in arr1) {
            var tar = arr1[j].split('=');
            arr2[tar[0]] = tar[1];
        };
        return arr2;
    }

    
    
 
   
    $(".table2_wrap").on('click', "tr", function () {
        $(this).addClass("active").siblings().removeClass("active");

    });

    $('.images_wrap').mCustomScrollbar({
        scrollButtons: {
            enable: true,
            scrollType: "continuous",
            scrollSpeed: 20,
            scrollAmount: 40
        },
        axis: "y",
        set_width: false,
        scrollbarPosition: "inside",
        theme: "minimal-dark",
    });
    $('.table2_wrap').mCustomScrollbar({
        scrollButtons: {
            enable: true,
            scrollType: "continuous",
            scrollSpeed: 20,
            scrollAmount: 40
        },
        axis: "y",
        set_width: false,
        scrollbarPosition: "inside",
        theme: "minimal-dark",
    });

   /* $('.table2_wrap').mCustomScrollbar({
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
        callbacks: {
            whileScrolling: function () {
                var $that = this.mcs.left;

                $('.table1').css('left', $that);
                $('.table1_wrap').css('width', '100%');

            },
            onUpdate: function () {
                var $that = $('#mCSB_1_container').css('left');
                $('.table1').css('left', $that);
                $('.table1_wrap').css('width', '100%');
                dealWidth();
            }
        }
    });*/

    $(".ul_imgs").on("click","li",function () {
        //alert('选取泵房');
        /*
        <div class="bigImg_wrap">
    <div class="bigImg_box"></div>
    <p class="pumpName_text">泵房名称</p>
</div>
        */
        $('.bigImg_box').empty();
        $('.bigImg_box').append($(this).children('.img_box').html());
        $(".bigImg_wrap .pumpName_text").html($(this).children(".pumpName_text").html());
        var srcStr = $(this).children('.img_box').children("img").attr("src");
        var index = parent.layer.open({
            type: 1,
            anim: 3,
            shade: .6,
            title: false,
            shadeClose: true,
            area: ['75%', '80%'],
            content: '<div style="margin: 30px auto;  width: 80%;"><img src ="' + srcStr + '" width="100%" /></div>',//$('.bigImg_wrap'),
            success: function () {
                //  alert('OK');
            }
        });
    });

    function dealWidth() {
        $(".table2  tr:first-child td").each(function (j, v) {
            var tempW = $(this).width();
            $(".table1 thead th").eq(j).width(tempW);

        });
    }
    loadTable();
    function loadTable() {
   
        $.ajax({
            url: '/V_YCJK/Search_PumpFJ',
            data: {
                "pumpID": pumpId
            },
            dataType: 'JSON',
            beforeSend: loadingFunction,
            success: function (data) {
                console.log(data);
                dealData(data.obj);
         
                dealWidth();
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
    function dealData(fileData) {
        $(".ul_imgs").empty();
        $(".table2 tbody").empty();
        for (var i = 0; i < fileData.length; i++) {
            if(fileData[i].uploadPageType=="pumppic")
            {
                var str = ' <li class="img_wrap">\
                   <div class="img_box">\
                        <img src="' + fileData[i].FilePath.substr(1, fileData[i].FilePath.length) + '" alt="' + fileData[i].FileName + '" title="' + fileData[i].FileName + '"  />\
                   </div>\
                   <p class="pumpName_text" data-id="' + fileData[i].ID + '">' + fileData[i].FileName + '</p>\
               </li>';
                $(".ul_imgs").append(str);
            } else if (fileData[i].uploadPageType == "pumpattach") {
                var fileStr = '<tr>\
                            <td class="fileName" data-id="' + fileData[i].ID + '"><i class="fileTypeIco"></i> ' + fileData[i].FileName + '</td>\
                            <td class="fileType" data-field="TypeName">' + fileData[i].TypeName + '</td>\
                            <td class="fileSize" data-field="FileSize">'+fileData[i].FileSize+'KB</td>\
                            <td class="upDateTime" data-field="UpDateTime">' + fileData[i].UpDateTime.replace('T', ' ') + '</td>\
                            <td class="downloadStatus"><i class="loadIco load_0" ></i>下载</td>\
                        </tr>';
                $(".table2 tbody").append(fileStr);
            }
        }
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
            var tempStr = '<td></td>';
            for (var key in theadJson) {
                if (key == "FIsAlarm") {
                    var isAlarm = tbData[i][key];
                }
                if (key == "FOnLine") {
                    var onOff = tbData[i][key];
                }
                if (key == "InOutWaPa") {
                    var inOutV = tbData[i][key].split("/");
                    tempStr += '<td class="inOutPress" data-id="' + key + '"><span class="pressIn alarm">' + inOutV[0] + '</span><span class="pressOut">' + inOutV[1] + '</span></td>';
                } else if (key == "PumpJZName") {

                    if (isAlarm) {
                        var tempState = 2;
                    } else if (onOff) {
                        var tempState = 1
                    } else {
                        var tempState = 0;
                    }
                    tempStr += '<td class="equipName" data-id="' + key + '"><i class="status_' + tempState + '"></i><a>' + tbData[i][key] + '</a></td>';


                } else if (key == "TempTime") {
                    var createDate = '';
                    if (tbData[i][key] !== null) {
                        createDate = tbData[i][key].replace("T", " ")
                    }
                    tempStr += '<td data-id="' + key + '">' + createDate + '</td>';
                } else {
                    tempStr += '<td data-id="' + key + '">' + tbData[i][key] + '</td>';
                }
            }
            tempStr = '<tr>' + tempStr + '</tr>';
            tbStr += tempStr;
        }
        $(".table2 tbody").append(tbStr);
    }
    

    function layout() {
        var winW = $(window).width();
       
        dealWidth();
    }
    layout();
    $(window).resize(function () {
        layout();
    });

    $(" .table2 ").on("click", "tr td.downloadStatus", function () {
        var createTime = $(this).parent().children("td.upDateTime").html();
        var imgId = $(this).parent().children("td.fileName").attr("data-id");
        downloadImgFn(imgId);
    });

    function downloadImgFn(imgId) {
        var isLoad = confirm('你确定要下载选中的文件吗？');
        if (isLoad) {
            window.location.href = '/V_YCJK/DownLoadFJ?id=' + imgId
        }

   /*     F.confirm({
            message: '你确定要下载选中的文件吗？',
            target: '_top',
            ok: function () {
                window.location.href = '/V_YCJK/DownLoadFJ?id=' + imgId
            },
            error:function(){
             
            }
        });*/
        //$.ajax({
        //    url: '/V_YCJK/DownLoadFJ',
        //    data: {
        //        "id": imgId
        //    },
        //    dataType: 'JSON',
        //    beforeSend: loadingFunction,
        //    success: function (data) {
        //        console.log(data);
        //        console.log('下载文件');
        //    },
        //    complete: loadingMiss,
        //    error: function (data) {
        //        console.log('错误：' + data);
        //    }
        //});
    }

});
