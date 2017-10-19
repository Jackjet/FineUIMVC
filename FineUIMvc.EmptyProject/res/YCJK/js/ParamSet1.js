$(function () {

    loadTable();
    function loadTable() {
        // alert(state);
        $.ajax({
            url: '/V_YCJK/ParmSelect1',           
            dataType: 'JSON',
            beforeSend: loadingFunction,
            success: function (data) {
                console.log(typeof data);
                console.log(data);
                dealField(data.obj)
            },
            complete: loadingMiss,
            error: function (data) {
                console.log('错误：' + data.responseText);
            }
        });
    }
    function loadingFunction() {
        var $div = $('<div class="loading" style="position:absolute;left: 50%;top:50%;margin-left: -150px;margin-top: -70px;width: 300px;color:black;text-align:center;line-height: 140px;height: 140px;background: rgba(255,255,0);color: white;border-radius: 8px;"><img style="position: relative;top: 56%;left: 15%;" src="/res/YCJK/img/load.gif" alt="loading....">正在加载中...</div>');
        $('body').append($div);
    };
    function loadingMiss() {
        $('.loading').remove();
    };

    function dealField(result) {
        $(".ul_default").empty();
        $(".ul_others").empty();
        var defaultStr = '';
        var othersStr = '';
        for (var i = 0; i < result.length; i++) {
            var str = '';
            if (result[i].FCommon == 1 || result[i].FCommon == 4) {
                var iClass =( result[i].IsSelect==1?'checked':'');
                str = '<li data-field="' + result[i].FFieldName + '" data-id="' + result[i].FField + '" data-type="' + result[i].FCommon + '"><i class=' + iClass + ' ></i><label>' + result[i].FName + '</label></li>';
                othersStr += str;
            } else if (result[i].FCommon == 2 || result[i].FCommon == 3) {
                var iClass = (result[i].IsSelect == 1 ? 'checked' : '');
                str = '<li data-field="' + result[i].FFieldName + '" data-id="' + result[i].FField + '" data-type="' + result[i].FCommon + '"><i class=' + iClass + ' ></i><label>' + result[i].FName + '</label></li>';
                defaultStr += str;
            }
            
        }
        $(".ul_default").append(defaultStr);
        $(".ul_others").append(othersStr);
    }
    //ParmSelect1
    var oDefault = $(".ul_default");
    var oOthers = $(".ul_others");
    var orDefaultOp = getOptions(oDefault);
    var orOthersOp = getOptions(oOthers);
    console.log(orDefaultOp);
    console.log(orOthersOp);
    function getOptions(obj) {
        var opsArr = [];
        obj.find("i").each(function (i, v) {
            if ($(this).hasClass("checked")) {
                opsArr.push(i);
            }
        });
        return opsArr;
    }
   
    function clearOps(obj,orOpsArr) {
        obj.find("i]").each(function (i, v) {
                $(this).removeClass("checked");
        });
        for (var j = 0; j < orOpsArr.length; j++) {
            console.log(orOpsArr[j]);
            obj.find("i").eq(orOpsArr[j]).addClass(checked);
        }
    }
 /*   $(".saveBtn").click(function () {
        var defaultOpArr = getOptions(oDefault);
        var othersOpArr = getOptions(oOthers);
        console.log(defaultOpArr);
    });*/

    $(".cancelBtn").click(function () {
        clearOps(oDefault, orDefaultOp);
        clearOps(oOthers, orOthersOp);
    });
    //EditParmSelect1
    var isSelected = 0;
    $(".main_wrap").on("click", "li", function () {
        var _that = $(this);
        var originSelect = 0;
        var opFieldId = $(this).attr("data-id");
        if (_that.attr("data-type") == 2 || $(this).parent("li").attr("data-type") == 4) {
            console.log("不可更改状态");
            return;
        } else {
            if (_that.children("i").hasClass("checked")) {
                originSelect = 1;
                isSelected = 0;
            } else {
                isSelected = 1;
                originSelect = 0;
            }
            editParmSelect(originSelect, opFieldId);
        }
    });

    function editParmSelect(oriSelect, opFieldId) {
        alert(isSelected);
        $.ajax({
            url: '/V_YCJK/EditParmSelect1',
            data: {
                "FField": opFieldId,
                "selectValue": isSelected
            },
            dataType: 'JSON',
            beforeSend: loadingFunction,
            success: function (data) {
                console.log(typeof data);
                console.log(data);
                if (data.msg == "成功") {
                    if (isSelected == 1) {
                        $("li[data-id=" + opFieldId + "]").children("i").addClass("checked");
                    } else {
                        $("li[data-id=" + opFieldId + "]").children("i").removeClass("checked");
                    }
                   
                } else {
                    console.log('oriSelect:' + oriSelect);
                    return;
                    /*if (oriSelect == 1) {
                        $("i[data-id=" + opFieldId + "]").addClass("checked");
                    } else {
                        $("i[data-id=" + opFieldId + "]").removeClass("checked");
                    }*/
                }
            },
            complete: loadingMiss,
            error: function (data) {
                console.log('错误：' + data.responseText);
            }
        });
    }
});