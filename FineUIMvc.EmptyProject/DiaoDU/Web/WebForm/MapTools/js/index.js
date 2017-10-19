
//var typeStr = ["管线", "二供", "阀门", "户表", "水厂", "大户表", "加压站", "水源"];

$(document).ready(function () {
  
    
    // var tempMapId = '29b9a1a6-0d4c-43db-b602-61426534480a';
          
    layout();
    $(window).resize(function () {
        layout();               
    });
    function layout() {
        var hHeight = $("#header").height();
        var wHeight = $(window).height();
        var navWidth = $("#nav_left").width();
        var tOffsetLeft = $("#tree_right").offset().left;
        console.log(tOffsetLeft);
        $(".main").css("height", wHeight - hHeight);
        $("#tree_right").css("height", wHeight - hHeight);
        $("#map_container").css("width", tOffsetLeft - navWidth - 18);
        var mcWidth = $("#map_container").width();
        var mcHeight = $("#map_container").height();
        $("#map").css({ "width": mcWidth - 74, "height": mcHeight - 70 });
        var mapH = $("#map").height();
        $(".property_main").css({ height: mapH - $(".tree_main").height() - 34 });
               
    }
    //   var typeJson = {"ergong": "二供", "famen": "阀门","hubiao": "户表", "shuichang": "水厂","dahubiao": "大户表","jiayazhan": "加压站", "shuiyuan": "水源"};
          
    initLevelTwo(leftIconArr);
    initGetMarker();
    //初始化加载 Marker二级菜单
    function initLevelTwo(leftIconArr) {
        for (var i = 1; i < leftIconArr.length; i++) {
            $(".two_level").append('<li class="node_' + i + '"><span><img width="24" height="24" src="'+leftIconArr[i].src+'" />' + leftIconArr[i].name + '</span><ul id="ul_' + i + '" class="three_level" data-type="Dot"> </ul></li>');
        }                
    }
           
    //左侧滚动条
    $('.nav_left_wrap').mCustomScrollbar({
        scrollbarPosition: "inside",
        theme: "minimal-dark"
    });
        
    $(".property_main").on('click', '.tri_ico', function () {
        $(this).parent().siblings("ul").slideToggle();
    });
    // addNode
    //function addNode(nodeName, nodeType, mapId) {
    //    $('#ul_' + nodeType).append('<li  data-mapId="' + mapId + '"><input type="checkbox" /><span class="nodeName">' + nodeName + '</span></li>');
    //}
    //addNode('管线1', 'guanxian', '2222');
    //addNode('设备1', 'shebei', '2222');

      
    /*  var guanxian_1 = new Guanxian('管线3', 'guanxian', '2222');
      guanxian_1.addNode();
      guanxian_1.showProperty();*/

    //预览按钮
    $(".scan").click(function () {
        console.log('预览');
        var index = layer.open({
            type: 2,
            anim: 3,
            shade: .6,
            title: false,
            shadeClose: true, //点击遮罩关闭层
            area: ['96%', '86%'],
            content: '../InPage/qingpuMapShow.html?id=' + tempMapId
        });
    });

    $(".deleteIco").click(function () {
        var len = $(".three_level span.current").length;
        if (len)
        {
            if (confirm("确定删除此节点?")) {
                var nodeId = $(".three_level span.current").parent("li").attr("data-id");
                var dataType = $(".three_level span.current").parents(".three_level").attr("data-type");
                var mapId = $(".three_level span.current").parent("li").attr("data-mapid");
                var tempUrl = 'mapTemp/html_map.html'
                initMapNode(tempUrl);
                if (dataType == "guanxian") {
                    deleteLineNode(nodeId);
                } else if (dataType == "Dot") {
                    deleteMarkerNode(nodeId);
                } else if (dataType == 'area') {
                    deleteAreaNode(nodeId);
                } else{
                    return;
                }
            }
        }
        deleteFn(mapId);
    });
           

    //mergeIco click
    $(".mergeIco").click(function () {
        var $inputs = $("#ul_guanxian input[type='checkbox']");
        console.log($inputs.length);
        var arrChecked = [];
        for (var i = 0, n = 0; i < $inputs.length; i++) {
            if ($inputs[i].checked) {
                n++;
                console.log($inputs[i]);
                arrChecked.push($inputs.eq(i).parent("li").attr("data-mapId"));
            }
        }
        console.log(n);
        console.log(arrChecked);
    });

    //deleteNode  根据mapId找到对应的dataId 传到后台数据库删除成功后从节点处删除
    function deleteAreaNode(tempMapId) {
        $.ajax({
            type: 'POST',
            url: '../../Service/Map_Area.ashx?method=delarea',
            data: {
                'FMapAreaID': tempMapId,
            },
            dataType: "json",
            success: function (data) {
                if (data.result) {
                    $(".three_level span.current").parent("li").remove();
                    console.log('delArea');
                }
                        
                console.log(data);
            },
            error: function (data) {
                console.log('错误：' + data.responseText);
            }
        });
    }

    function deleteLineNode(tempMapId) { 
               
        $.ajax({
            type: 'POST',
            url: '../../Service/Map_Line.ashx?method=delline',
            data: {
                'FMapLineID': tempMapId,
            },
            dataType: "json",
            success: function (data) {                      
                if (data.result)
                {
                    $(".three_level span.current").parent("li").remove();
                }
                console.log('kkkkk' );
                console.log(data);
            },
            error: function (data) {
                console.log('错误：' + data.responseText);
            }
        });
    }

    function deleteMarkerNode(tempMapId) {
          
        $.ajax({
            type: 'POST',
            url: '../../Service/Map_Marker.ashx?method=delmarker',
            data: {
                'FMarkerID': tempMapId,
            },
            dataType: "json",
            success: function (data) {
                if (data.result) {
                    $(".three_level span.current").parent("li").remove();
                }
                console.log('kkkkk');
                console.log(data);
            },
            error: function (data) {
                console.log('错误：' + data.responseText);
            }
        });
    }
    $(".tree_main").on("click", "#node_top", function () {
        initMapNode('mapTemp/html_map.html');
    });

    $(".tree_main").on("click", '.two_level>li>span', function () {
        $(this).siblings("ul").slideToggle();
    });

    //三级节点击事件
    $(".tree_main").on("click", '.three_level span', function () {
        $(".three_level span").removeClass("current");
        $(this).addClass("current");
        var searchNodeId = $(this).parent("li").attr('data-id');     
        var MapId = $(this).parent("li").attr('data-mapid');
        var tempStr = $(this).parents(".three_level").attr("data-type");
        var tempUrl = 'mapTemp/html_' + tempStr + '.html';              
        if (tempStr == 'guanxian')
        {
            //  alert(searchNodeId);
            getLineProperty(searchNodeId, tempUrl);
        }else if(tempStr == 'Dot')
        {
            getMarkerProperty(searchNodeId, tempUrl);
        } else if (tempStr == 'area') {
            getAreaProperty(searchNodeId, tempUrl);
        } else {
            return;
        }
    });
            
    $(".three_level").on('change', 'input[type="checkbox"]', function () {
        if ($(this).is(':checked')) {
            alert($(this).parent("li").attr("data-mapId"));
        }
    });

    $(".property_main").on('focus', 'input[type="text"]', function () {
        var oldValue = $(this).val();
          
        $(this).parent("li").siblings("li").children(".mySlider").removeClass("show");
        $(this).parent("li").children(".mySlider").addClass("show");
        if($(this).attr("data-field")=="FFeatures")
        {
            $(this).siblings(".map_features").slideDown();
        } else {
            $(".map_features").slideUp();
        }
    });
        
            //修改所属区域
    $(".property_main").on('mouseenter', '.li_area', function (e) {
                $(".line_areaModify").show();
                e.stopPropagation();
            });
           
     $(".property_main").on('mouseleave', '.li_area', function () {
       
         $(".line_areaModify").hide();
         console.log('hide');
     });
     $(".property_main").on('click', '.line_areaModify', function (e) {
         e.stopPropagation();         
         var index = layer.open({
             type: 2,
             anim: 3,
             shade: .6,
             title: false,
             shadeClose: true, //点击遮罩关闭层
             area: ['620px', '400px'],
             content: 'layer_areaModify.html',
             success: function () {
                 console.log(222);
                 //belongAreajson
                 var allAreajson = {};
                 $('#ul_area li').each(function () {                      
                     allAreajson[$(this).attr("data-id")] = $(this).children("span").html();
                 });
                 var rightValue = layer.getChildFrame(".rightSelect", index);
                 var leftValue = layer.getChildFrame(".leftSelect", index);
                 for (var i in belongAreajson) {
                     rightValue.append('<li data-areaid="' + i + '">' + belongAreajson[i] + '</li>')
                     delete allAreajson[i];
                 }
                 leftValue.empty();
                 for (var j in allAreajson) {
                    // leftValue.append('<li data-areaid="' + j + '">nnnn</li>')
                    leftValue.append('<li data-areaid="' + j + '">' + allAreajson[j] + '</li>')
                 }
             }
         });
     });
     //地图样式
     $(".property_main").on('click', '#id_layerBtn', function () {
         console.log(1);
         var styleText = $('input[data-field="FStyle"]').val();
         var styleV = $('input[data-field="FStyle"]').attr('data-style');
         console.log(styleV)
        var index =  layer.open({
             type: 2,
             anim: 3,
             shade: .6,
             title: false,
             shadeClose: true, //点击遮罩关闭层
             area: ['760px', '400px'],
             content: 'layer_mapStyle.html',
             success: function () {
                 console.log(222);
                 var currentStyle = layer.getChildFrame(".style_imgBox", index);
                 currentStyle.children('img[alt="' + styleV + '"]').parents(".style_styleBox").addClass('current');
             }
         });
        
     });
           
        
            //关联设备ID弹层
            $(".property_main").on('click', ".fMidBtn", function () {
                var typeIndex = getMarkerType();
                var id = $('input[data-field="FMID"]').attr("data-mid");
                getMarkerType();
                var index = layer.open({
                    type: 2,
                    anim: 3,
                    shade: .6,
                    title: ['列表数据名称','text-align: center;'],
                    shadeClose: true, 
                    area: ['70%', '80%'],
                    content: 'layer_fMid.html?typeIndex='+typeIndex+'&id='+id,
                    success: function () {
                        console.log(222);
                      //  var currentStyle = layer.getChildFrame(".style_imgBox", index);
                      //  currentStyle.children('img[alt="' + styleV + '"]').parents(".style_styleBox").addClass('current');
                    }
                });
            });

            //展开地图元素列表      downBtn
            $(".property_main").on('click', '.downBtn', function (e) {
                var oldValue = $(this).siblings("input").val();
                $(this).siblings("ul.map_features").slideToggle();
                e.stopPropagation();
            });


            $(".property_main").on('click', 'input[type="text"]', function (e) {
                e.stopPropagation();
            });
            $("#tree_right").click(function (e) {
                $(".mySlider").removeClass("show");
                $("ul.map_features").slideUp();
            });
            
            //修改属性
            $(".property_main").on('blur', 'input[type="text"]', function () {
                var nodeType = $(".property_main").children(".property_box").attr("data-type");            
                var url = '../../Service/Map_' + nodeType + '.ashx?method=upd' + nodeType.toLowerCase() + 'pro';
                var tableField = $(this).attr("data-field");
                var proId = $(".property_main").find('input[type="hidden"]').val();              
                var newValue = $(this).val();
                if (tableField == 'FName')
                {
                    $('.three_level li[data-id="' + proId + '"]').children(".nodeName").html(newValue);               
                }
                if (nodeType == "Template")
                {
                    url = '../../Service/Map_Template.ashx?method=updtemppro';
                    if (tableField == "FMapTempName")
                    {
                        $('#node_top').html(newValue);
                    }                    
                }
                if (tableField == "FFeatures" || tableField == "FPavDate" || tableField == "FStyle" || tableField == "FStrokeOpacity" || tableField == "FAreaOpacity") {
                   // $(this).siblings(".map_features").slideUp();
                    return;                
                }
                console.log(url + ' ' + proId + '  ' + tableField + '  ' + newValue);
               
               modifyProperty(url, proId, tableField, newValue);
             
            });
            

            //select  change event
            $(".property_main").on('change', '.cla_selector', function () {
                console.log('select change');
                var nodeType = $(".property_main").children(".property_box").attr("data-type");
                if (nodeType == "Template") {
                 var   url = '../../Service/Map_Template.ashx?method=updtemppro';
                } else {
                    var url = '../../Service/Map_' + nodeType + '.ashx?method=upd' + nodeType.toLowerCase() + 'pro';
                }
                // $('select[data-field="FMapType"]').selectedIndex;
             
                var tableField = $(this).attr("data-field");
                var proId = $(".property_main").find('input[type="hidden"]').val();
                var newValue = $(this).val();
                var selectedIndex = $('select[data-field="FMapType"] option[value="' + newValue + '"]').index();
                console.log(selectedIndex);
                if (tableField == "FFeatures")
                {
                    newValue = newValue.join(',');
                }
                console.log(url + ' ' + proId + '  ' + tableField + '  ' + newValue);
                modifyProperty(url, proId, tableField, newValue, selectedIndex);

            });
           

              
            $(".property_main").on('click', '.map_features a', function (e) {
                e.stopPropagation();
                $(this).toggleClass("current");
                //  var map_features = {};
                var curText = $(this).html();
                var curValue = $(this).parent("li").attr("data-value");
                if ($(this).hasClass("current"))
                {                  
                    map_features[curValue] = curText;
                } else {
                   delete(map_features[curValue]);
                }
                console.log('kkkkkkk');
                console.log(map_features);
              
                var newFvalue = dealMapFeajson(map_features);
                var proId = $(".property_main").find('input[type="hidden"]').val();
                modifyProperty('../../Service/Map_Template.ashx?method=updtemppro', proId, 'FFeatures', newFvalue);
            });

        });


function getMarkerType() {
    var leftIcoNameArr = [];
    for (var i = 0; i < leftIconArr.length; i++) {
        leftIcoNameArr.push(leftIconArr[i].name);
        var typeName = $("input[data-field='FType']").val();
    }
    console.log(typeName);
    return (leftIcoNameArr.indexOf(typeName));
}
    function setMapStyle(styleV, styleText) {
            $('input[data-field="FStyle"]').val(styleText);
            $('input[data-field="FStyle"]').attr("data-style", styleV);
            var proId = $(".property_main").find('input[type="hidden"]').val();
            modifyProperty('../../Service/Map_Template.ashx?method=updtemppro', proId, 'FStyle', styleV);
        }
        function dealMapFeajson(mapFeatures) {
            var inputFvalue = '';
            var arrFeatures = [];//元素值                               
            for (var key in map_features) {
                inputFvalue += map_features[key] + ',';
                arrFeatures.push(key);
            }
            var newFvalue = arrFeatures.join(',');
            console.log(newFvalue);
            inputFvalue = inputFvalue.substring(0, inputFvalue.length - 1);
            console.log(inputFvalue);
            $("input[data-field='FFeatures']").val(inputFvalue);
            return newFvalue;
        }
   
     
//修改属性值
        function modifyProperty(url, id, tableField, text,selectedIndex,midName) {
          console.log(typeof text);
          console.log(url);
            
            $.ajax({
                type: 'POST',
                url: url,
                data: {
                    'id': id,
                    'TableField': tableField,
                    'text':text
                },
                dataType: "json",
                success: function (data) {
                    console.log(data);
                    var result = data.result; 
                  //  console.log(result);
                    if (data.result == '1')
                    {
                      var mapId =  $('li[data-id="' + id + '"]').attr("data-mapid");
                        console.log('success');
                        switch (tableField) {
                            case "FMapType":
                                map.setMapType(mapType[selectedIndex]);
                                break;
                            case "FZoom":
                                map.setZoom(Number(text));
                                break;
                            case "FStyle":
                                map.setMapStyle({style:text});
                                break; 
                            case "FStrokeColor":
                                changeAttrFunc(mapId, 'FStrokeColor', text);
                                break;
                            case "FStrokeOpacity":
                                changeAttrFunc(mapId, 'FStrokeOpacity', text);
                             
                                $("#id_opacity").val(text + '%');
                                break; 
                            case "FStrokeWeight":
                                changeAttrFunc(mapId, 'FStrokeWeight', text); 
                                break;
                            case "FStrokeStyle":
                                changeAttrFunc(mapId, 'FStrokeStyle', text);
                                break;
                            case "FLine":
                                $("input[data-field='FLine']").val(text); 
                                break;
                            case "FAreaOpacity":                            
                                changeAttrFunc(mapId, 'FAreaOpacity', text);
                                $("#area_opacity").val(text + '%');
                                break;
                            case "FAreaColor":
                                changeAttrFunc(mapId, 'FAreaColor', text);
                                                         
                                break;
                            case "FMID":
                                $('input[data-field="FMID"]').val(midName);
                                $('input[data-field="FMID"]').attr("data-mid", text);
                                                         
                                break;
                            default:
                                break;
                        }
                    } else if (data.result == '0') {
                        console.log(parseInt($("#id_opacity").val().slice(0, -1)));                      
                    }
                },
                error: function (data) {
                    console.log('错误：' + data.responseText);
                }
            });
        }


        function getAreaProperty(areaId, tempUrl) {

            $.ajax({
                type: 'POST',
                url: '../../Service/Map_Area.ashx?method=searcharea',
                data: {
                    'TempID': tempMapId,
                    'AreaID': areaId
                },
                dataType: "json",
                success: function (data) {
                    console.log('wwwwwwww');
                    console.log(data);
                    dealData(data[0]);
                    ajaxload(tempUrl);
                },
                error: function (data) {
                    console.log('错误：' + data.responseText);
                }
            });
        }
        function getLineProperty(lineId,tempUrl) {
          
            $.ajax({
                type: 'POST',
                url: '../../Service/Map_Line.ashx?method=searchline',
                data: {
                    'TempID': tempMapId,
                    'LineID':lineId
                },
                dataType: "json",
                success: function (data) {
                    console.log('wwwwwwww');
                    console.log(data);
                    dealData(data[0]);
                    ajaxload(tempUrl);
                },
                error: function (data) {
                    console.log('错误：' + data.responseText);
                }
            });
        }

        function getMarkerProperty(markerId, tempUrl) {          
            $.ajax({
                type: 'POST',
                url: '../../Service/Map_Marker.ashx?method=searchmarker',
                data: {
                    'TempID': tempMapId,
                    'MarkerID': markerId
                },
                dataType: "json",
                success: function (data) {
                    console.log('mmmmmmm');
                    console.log(data);
                    dealData(data[0]);
                    ajaxload(tempUrl);
                },
                error: function (data) {
                    console.log('错误：' + data.responseText);
                }
            });
        }

        function ajaxload(local) {
            var htmlobj = $.ajax({
                url: local,
                asyn: false,
                type: "GET",
                success: function (data) {
                    $('.property_main').html(data);
                    setData();
                    var indexStar = local.indexOf('_');
                    var indexEnd = local.indexOf('.');
                    var typeStr = local.slice(indexStar + 1, indexEnd);
                    if (typeStr == "area") {
                        lineColor('area_color');
                        lineColor('line_color');
                    } else if (typeStr == "guanxian") {
                        lineColor('line_color');
                    }                 
                    console.log(typeStr);
               
                    var staMinZoom = $('input[data-field="FMinZoom"]').val();
                    var staMaxZoom = $('input[data-field="FMaxZoom"]').val();
                    var staCurZoom = $('input[data-field="FZoom"]').val();
                    var staWidth = $('input[data-field="FStrokeWeight"]').val();
                    var staOpacity = $('input[data-field="FStrokeOpacity"]').val();
                    var staAreaOpacity = $('input[data-field="FAreaOpacity"]').val();
                    console.log(staOpacity);
                 
                    var proId = $(".property_main").find('input[type="hidden"]').val();
                   
                    console.log(staOpacity);
                    console.log(staAreaOpacity);
                    sliderFn(staCurZoom, staMinZoom, staMaxZoom, staWidth, staOpacity, proId, staAreaOpacity);
                    console.log(local);
                    if (local == 'mapTemp/html_guanxian.html')
                    {
                        var startD = $("#line_layDate").val().replace('T', ' ');
                        $("#line_layDate").val(startD);
                        console.log(startD);
                        laydate({
                            elem: "#line_layDate",
                            format: "YYYY-MM-DD  hh:mm:ss",
                            start: startD,
                            istime: true,
                            istoday: false,
                            issure: true,
                            choose: function (datas) {
                                console.log('得到' + datas);
                                modifyProperty('../../Service/Map_Line.ashx?method=updlinepro', proId, 'FPavDate', datas);
                                $("#line_layDate").val(datas);
                            }
                        });

                    }
                }
            });
        }


        //map FFeatures multiple select
        function multiSelector() {
            $(".map_features a").click(function () {
                $(this).addClass("current");
                var curText = $(this).html();
                var curValue = $(this).attr("data-value");
                console.log(curText + ' ' + curValue);
            });
        }

        //silder
        var slider_opacity = {};
        function sliderFn(staCurZoom, staMinZoom, staMaxZoom, staWidth, staOpacity, proId, staAreaOpacity) {
            $("#slider-range-min").slider({
                range: "min",
                value: staMinZoom,
                min: 1,
                max: 19,
                stop: function (event, ui) {
                    $("#id_minZoom").val(ui.value);
                    modifyProperty('../../Service/Map_Template.ashx?method=updtemppro', proId, 'FMinZoom', ui.value);
                }
            });
            $("#id_minZoom").val($("#slider-range-min").slider("value"));
            $("#slider-range-max").slider({
                range: "min",
                value: staMaxZoom,
                min: 1,
                max: 19,
                stop: function (event, ui) {
                    $("#id_maxZoom").val(ui.value);
                    modifyProperty('../../Service/Map_Template.ashx?method=updtemppro', proId, 'FMaxZoom', ui.value);
                }
            });
            $("#id_maxZoom").val($("#slider-range-max").slider("value"));
            $("#slider-range-cur").slider({
                range: "min",
                value: staCurZoom,
                min: 1,
                max: 19,
                stop: function (event, ui) {
                    $("#id_curZoom").val(ui.value);
                    modifyProperty('../../Service/Map_Template.ashx?method=updtemppro', proId, 'FZoom', ui.value);
                }
            });
            $("#id_curZoom").val($("#slider-range-cur").slider("value"));
            $("#slider-range-width").slider({
                range: "min",
                value: staWidth,
                min: 1,
                max: 15,
                stop: function (event, ui) {
                    $("#id_lineWidth").val(ui.value);
                    modifyProperty('../../Service/Map_Line.ashx?method=updlinepro', proId, 'FStrokeWeight', ui.value);
                }
            });
            $("#id_lineWidth").val($("#slider-range-width").slider("value"));
         
            slider_opacity = $("#slider-range-opacity").slider({
                range: "min",
                value: staOpacity,
                min: 0,
                max: 100,
                stop: function (event, ui) {
                    //  $("#id_opacity").val(ui.value + '%');
                    console.log('opacity');
                    modifyProperty('../../Service/Map_Line.ashx?method=updlinepro', proId, 'FStrokeOpacity', ui.value);
                }
            });
            $("#id_opacity").val($("#slider-range-opacity").slider("value") + '%');

            $("#slider-area-opacity").slider({
                range: "min",
                value: staAreaOpacity,
                min: 0,
                max: 100,
                stop: function (event, ui) {
                    console.log('opacity');
                    modifyProperty('../../Service/Map_Area.ashx?method=updareapro', proId, 'FAreaOpacity', ui.value);
                }
            });
            $("#area_opacity").val($("#slider-area-opacity").slider("value") + '%');
        }


        function Treenode(nodeName, nodeType, mapId) {
            this.nodeType = nodeType;
            this.mapId = mapId;
            this.nodeName = nodeName;

        }
        Treenode.prototype.addNode = function () {
            $('#ul_' + this.nodeType).append('<li  data-mapId="' + this.mapId + '"><span  class="nodeName">' + this.nodeName + '</span><input type="checkbox" /></li>');       
        };

        Treenode.prototype.showProperty = function () {
            var tempStr = $("li[data-mapId='" + this.mapId + "']").parents(".three_level").attr("data-type");
            var tempUrl = 'mapTemp/html_' + tempStr + '.html';
            ajaxload(tempUrl, this.mapId);
        };


        function Guanxian(nodeName, nodeType, mapId) {
            Treenode.call(this, nodeName, nodeType, mapId);
        }
        for (var i in Treenode.prototype) {
            Guanxian.prototype[i] = Treenode.prototype[i];
        }

        function AreaNode(nodeName, nodeType, mapId, areaType) {
            Treenode.call(this, nodeName, nodeType, mapId);
            this.areaType = areaType;
        }
        for (var j in Treenode.prototype) {
            AreaNode.prototype[i] = Treenode.prototype[i];
        }
        AreaNode.prototype.addNode = function () {
            $('#ul_' + this.nodeType).append('<li  data-mapId="' + this.mapId + '" data-areaType="' + this.areaType + '"><span  class="nodeName">' + this.nodeName + '</span><input type="checkbox" /></li>');
        };
        //获取地图模板ID

        var tempMapId = tempIDNow;

        //初始化
        function initMapNode(tempUrl) {
            $.ajax({
                type: 'POST',
                url: '../../Service/Map_Template.ashx?method=searchtemp',
                data: {
                    'TempID': tempMapId,
                },
                dataType: "json",
                success: function (data) {
                    console.log(data[0]);
                    $('#node_top').html(data[0].FMapTempName);
                    dealData(data[0]);
                    if (tempUrl) {
                        ajaxload(tempUrl);
                    }
                },
                error: function (data) {
                    console.log('错误：' + data.responseText);
                }
            });
        }

        initGetArea();
        //加载区域数据
        function initGetArea() {
            $.ajax({
                type: 'POST',
                url: '../../Service/Map_Area.ashx?method=searcharea',
                data: {
                    'TempID': tempMapId,
                },
                dataType: "json",
                success: function (data) {
                    // console.log(data);
                    for (var i = 0; i < data.length; i++) {
                        $('#ul_area').append('<li data-mapId=' + data[i].FMapAreaID + ' data-id=' + data[i].FMapAreaID + '><span class="nodeName">' + data[i].FName + '</span><input type="checkbox" /></li>');
                    }
                },
                error: function (data) {
                    console.log('错误：' + data.responseText);
                }
            });
        }
        //加载管线数据
        function initGetLine() {
            $.ajax({
                type: 'POST',
                url: '../../Service/Map_Line.ashx?method=searchline',
                data: {
                    'TempID': tempMapId,
                },
                dataType: "json",
                success: function (data) {
                    // console.log(data);
                    for (var i = 0; i < data.length; i++) {
                        $('#ul_0').append('<li data-mapId=' + data[i].FMapLineID + ' data-id=' + data[i].FMapLineID + '><span class="nodeName">' + data[i].FName + '</span><input type="checkbox" /></li>');
                    }
                },
                error: function (data) {
                    console.log('错误：' + data.responseText);
                }
            });
        }
        //加载Marker数据
        function initGetMarker() {
            $.ajax({
                url: '../../Service/Map_Marker.ashx?method=searchmarker',
                data: { "TempID": tempMapId },
                success: function (result) {
                    data = $(JSON.parse(result));
                    console.log(data);
                    //   var typeStr = ['guanxian', 'ergong', 'famen', 'hubiao', 'shuichang', 'dahubiao', 'jiayazhan', 'shuiyuan'];
                    data.each(function (i, v) {
                        $('#ul_' + v.FType).append('<li  data-mapId="' + v.FMarkerID + '" data-id="' + v.FMarkerID + '"><span  class="nodeName">' + v.FName + '</span><input type="checkbox" /></li>');
                    });
                }
            });
        }
        initMapNode();
        initGetLine();

        //新增区域
        function addAreaNode(nodeName, mapId, pointArray, areaType) {
         
            console.log(pointArray);
            var regpointArray = JSON.parse(pointArray);
            var pointFlag = false;
            for (var i = 0; i < regpointArray.length-1; i++)
            {
                if (regpointArray[i].lat == regpointArray[i + 1].lat && regpointArray[i].lng == regpointArray[i + 1].lng) {
                    pointFlag = true;
                } else {
                    pointFlag = false;
                }
            }
          
            console.log(regpointArray);
            console.log('aaaa');
     
            if (regpointArray[1] == 0||pointFlag) {
                console.log('[] point');
                return;
            }
            $.ajax({
                type: 'POST',
                url: '../../Service/Map_Area.ashx?method=insertarea',
                data: {
                    'FName': nodeName,
                    'FMapTempID': tempMapId,
                    'FArea': pointArray,
                    'FAreaType': areaType
                },
                dataType: "json",
                success: function (data) {
                    console.log('阿啊');
                    console.log(data.result[0]);
                      
                    var areaNode = new AreaNode(nodeName, 'area', mapId,areaType);
                    areaNode.addNode();
                    dealData(data.result[0]);
                    areaNode.showProperty();
                    $('.three_level li[data-mapid="' + mapId + '"]').attr("data-id", data.result[0].FMapAreaID);
                    $('.three_level li[data-mapid="' + mapId + '"]').find('span').trigger('click');
                    //   $('.three_level li[data-mapid="' + mapId + '"]')[0].scrollIntoView();
                    scrollToTarget(mapId);
                },
                error: function (data) {
                    console.log('错误：' + data.responseText);
                }
            });
        }
    
    //新增管线
        function addLineNode(nodeName, mapId,pointArray) {      
            $.ajax({
                type: 'POST',
                url: '../../Service/Map_Line.ashx?method=insertline',
                data: {
                    'FName': nodeName,
                    'FMapTempID': tempMapId,
                    'FLine': pointArray
                },
                dataType:"json",
                success: function (data) {
                    console.log('阿啊');
                    console.log(data.result[0]);
                    console.log(data.result[0].FMapLineID);
                   // var nodeName = data.result[0].FName;                  
                    var guanxian_1 = new Guanxian(nodeName, 0, mapId);
                    guanxian_1.addNode();
                    dealData(data.result[0]);
                    guanxian_1.showProperty();
                    $('.three_level li[data-mapid="' + mapId + '"]').attr("data-id", data.result[0].FMapLineID);
                    $('.three_level li[data-mapid="' + mapId + '"]').find('span').trigger('click');
                    scrollToTarget(mapId);
                },
                error: function (data) {
                    console.log('错误：' + data.responseText);
                }
            });
        }
    
        //新增Marker
        function addMarkerNode(nodeName, mapId, pointArray,typeIndex) {
        //    var typeStr = ['guanxian','ergong', 'famen', 'hubiao', 'shuichang', 'dahubiao', 'jiayazhan', 'shuiyuan'];           
            console.log(typeIndex);
            if (typeIndex != 0)
            {
                var nodeType = typeIndex;
            } else {
                return
            }
            $.ajax({
                type: 'POST',
                url: '../../Service/Map_Marker.ashx?method=insertmarker',
                dataType:'json',
                data: {
                    'FName': nodeName,
                    'FMapTempID': tempMapId,
                    'FType': typeIndex,
                    'FMarker': pointArray
                },
                success: function (data) {
                    console.log(data);
                    var result = data.result[0];
                    var shebei = new Treenode(nodeName, nodeType, mapId);
                    shebei.addNode();
                    dealData(result);
                    shebei.showProperty();
                    $('.three_level li[data-mapid="' + mapId + '"]').attr("data-id", data.result[0].FMarkerID);
                    $('.three_level li[data-mapid="' + mapId + '"]').find('span').trigger('click');
                    scrollToTarget(mapId);
                },
                error: function (data) {
                    console.log('错误：' + data.responseText);
                }
            });
        }
      //  addMarkerNode('设备','1111111','',1);

        function scrollToTarget(mapId) {
            var mainContainer = $("#tree_list");
            var scrollToTag = $('.three_level li[data-mapid="' + mapId + '"]');
            console.log('aaaaaaaa');
            console.log(scrollToTag.offset().top);
            console.log(mainContainer.offset().top);
            console.log(mainContainer.scrollTop());
            mainContainer.animate({
                scrollTop: scrollToTag.offset().top - mainContainer.offset().top+mainContainer.scrollTop()
            },1000);
        }
//根据后台数据设置属性值
    var myDatajson = {};
    function dealData(data){
        for(var key in data)
        {
             myDatajson[key]=data[key];  //设置key值与数据库字段对应
        }
        console.log(myDatajson);
    }

    
    function setData() {     
            $(".data-key").each(function () {             
                var data_field = $(this).attr("data-field");
                var temp = eval('myDatajson.' + data_field);
                $(this).val(temp);                          
            });
            console.log(myDatajson.FFeatures);
            if (myDatajson.FFeatures)
            {
                dealFeaturesV(myDatajson.FFeatures);
            }
            if (myDatajson.FAreaType)
            {
                console.log(myDatajson.FAreaType);
                var areaTypeArr = { "rectangle": "矩形", "circle": "圆形", "polygon": "折线" };
                $('input[data-field="FAreaType"]').val(areaTypeArr[myDatajson.FAreaType]);
            }
            if (myDatajson.FType) {

                console.log(myDatajson.FType);
                //   var typeArr = ['管线', '二供', '阀门', '户表', '水厂', '大户表', '加压站', '水源'];
                $('input[data-field="FType"]').val(leftIconArr[myDatajson.FType].name);
            }
            if (myDatajson.FStyle) {
                dealMapStyleV(myDatajson.FStyle);
            }
            if (myDatajson.TB_AreaOverlay) {
                dealBelongArea(myDatajson.TB_AreaOverlay);
            }
            if (myDatajson.FMID) {
                dealMID(myDatajson.FMID);
                
            }
        }

    var map_features = {};

//初始地图元素值处理
    function dealFeaturesV(tempv) {
        var arrOldFeaV = tempv.split(',')
        var strOldFeaT = [];
        map_features = {};
        for (var i = 0; i < arrOldFeaV.length; i++) {
            //console.log(arrOldFeaV[i]);
            $('.map_features li[data-value="' + arrOldFeaV[i] + '"]').children("a").addClass("current");
            strOldFeaT.push($('.map_features li[data-value="' + arrOldFeaV[i] + '"]').children("a").html());
        }
        $(".map_features a.current").each(function (i, v) {
            var curText = $(this).html();
            var curValue = $(this).parent("li").attr("data-value");
            map_features[curValue] = curText;
        });      
        console.log(map_features);
        $("input[data-field='FFeatures']").val(strOldFeaT.join(','));
    }
//初始管线所属区域
    var belongAreajson = {};
    function dealBelongArea(tempAreaV) {
        console.log( tempAreaV instanceof  Array);
      
       
        belongAreajson = {};
        $(".belongArea").empty();       
        for (var i = 0; i < tempAreaV.length;i++) {
            belongAreajson[tempAreaV[i].FMapAreaID] = tempAreaV[i].FName;
            $(".belongArea").append('<li data-areaid="' + tempAreaV[i].FMapAreaID + '">' + tempAreaV[i].FName + '</li>');
        }
        return belongAreajson;
    }
    var map_style = {
        "normal": "默认地图样式(normal)",
        "light": "清新蓝风格(light)",
        "dark": "黑夜风格(dark)",
        "redalert": "红色警戒风格(redalert)",
        "googlelite": "精简风格(googlelite)",
        "grassgreen": "自然绿风格(grassgreen)",
        "midnight": "午夜蓝风格(midnight)",
        "pink": "浪漫粉风格(pink)",
        "darkgreen": "青春绿风格(darkgreen)",
        "bluish": "清清新蓝绿风格(bluish)",
        "grayscale": "高端灰风格(grayscale)",
        "hardedge": "强边界风格(hardedge)"       
    };
    function dealMapStyleV(styleV) {
        $('input[data-field="FStyle"]').val(map_style[styleV]);
        $('input[data-field="FStyle"]').attr("data-style", styleV); 
    }
    function dealMID(midV) {
        //  $('input[data-field="FMID"]').val();
        var typeIndex = getMarkerType();
       $('input[data-field="FMID"]').attr("data-mid", midV);
        var proId = $(".property_main").find('input[type="hidden"]').val();
        $.ajax({
            type: 'POST',
            url: '../../Service/BaseToMarker.ashx?method=search',
            data: {
                'Type': typeIndex,
                'ID': midV,
            },
            dataType: "json",
            success: function (data) {
                console.log('MID');
                console.log(data);
                var result = data.data;
                console.log(result);
               $('input[data-field="FMID"]').val(result[1].jsonData.data[0].FName);
            },
            error: function (data) {
                console.log('错误：' + data.responseText);
            }
        });
    }
   
    function lineColor(ele_colorId) {
        $('#' + ele_colorId + '.minicolors').minicolors({

            control: $(this).attr('data-control') || 'hue',
            defaultValue: $(this).attr('data-defaultValue') || '',

            inline: $(this).attr('data-inline') === 'true',
            letterCase: $(this).attr('data-letterCase') || 'lowercase',
            opacity: $(this).attr('data-opacity'),

            position: $(this).attr('data-position') || 'bottom left',
       
            //change: function (hex, opacity) {

            //    //console.log(hex + ' - ' + opacity);

            //},
            theme: 'bootstrap'

        });
    }

    function saveBelongArea(mapAreaID, index, newBelongArea) {
        var mapOverlayID = $(".property_box  input[type='hidden']").val();
        var tempMapId = $("#tree_list").attr("data-tempid");
        var overlayType = $(".property_box").attr("data-type");
               $.ajax({
                    type: 'POST',
                    url: '../../Service/Map_Area.ashx?method=insertareaoverlay',
                    data: {
                        'FMapTempID': tempMapId,
                        'FMapAreaID': mapAreaID,
                        'FMapOverlayID': mapOverlayID,
                        'FMapOverlayType': overlayType
                    },
                    dataType: "json",
                    success: function (data) {
                        console.log('AREAaREA');
                        console.log(data);
                        if(data.result=='1')
                        {
                            layer.close(index);
                            $(".belongArea").empty();
                            belongAreajson = {};
                           
                            for (var i in newBelongArea) {
                                $(".belongArea").append('<li data-areaid="' + i + '">' + newBelongArea[i] + '</li>');
                                belongAreajson[i] = newBelongArea[i];
                            }                         
                        }
                    },
                    error: function (data) {
                        console.log('错误：' + data.responseText);
                    }
                });
    }

