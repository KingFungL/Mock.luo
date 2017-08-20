/**
 * 将form里面的内容序列化成json
 * 相同的checkbox用分号拼接起来
 * @param {dom} 指定的选择器
 * @param {obj} 需要拼接在后面的json对象
 * @method serializeJson
 * */
$.fn.serializeJson = function (otherString) {
    var serializeObj = {},
        array = this.serializeArray();
    $(array).each(function () {
        if (serializeObj[this.name]) {
            serializeObj[this.name] += ';' + this.value;
        } else {
            serializeObj[this.name] = this.value;
        }
    });

    if (otherString != undefined) {
        var otherArray = otherString.split(';');
        $(otherArray).each(function () {
            var otherSplitArray = this.split(':');
            serializeObj[otherSplitArray[0]] = otherSplitArray[1];
        });
    }
    return serializeObj;
};

/**
 * 将josn对象赋值给form
 * @param {dom} 指定的选择器
 * @param {obj} 需要给form赋值的json对象
 * @method serializeJson
 * */
$.fn.setForm = function (jsonValue) {
    if (!jsonValue) return;
    var obj = this;
    $.each(jsonValue, function (name, ival) {
        var $oinput = obj.find("input[name=" + name + "]");
        if ($oinput.attr("type") == "checkbox") {
            if (ival !== null) {
                var checkboxObj = $("[name=" + name + "]");
                var checkArray = ival.split(";");
                for (var i = 0; i < checkboxObj.length; i++) {
                    for (var j = 0; j < checkArray.length; j++) {
                        if (checkboxObj[i].value == checkArray[j]) {
                            checkboxObj[i].click();
                        }
                    }
                }
            }
        }
        else if ($oinput.attr("type") == "radio") {
            $oinput.each(function () {
                var radioObj = $("[name=" + name + "]");
                for (var i = 0; i < radioObj.length; i++) {
                    if (radioObj[i].value == ival) {
                        radioObj[i].click();
                    }
                }
            });
        }
        else if ($oinput.attr("type") == "textarea") {
            obj.find("[name=" + name + "]").html(ival);
        }
        else {
            obj.find("[name=" + name + "]").val(ival);
        }
    })
}



/**
 * 创建模态窗。
 * @param {Object} options
 */
$.layerOpen = function (options) {
    var defaults = {
        id: "default",
        title: '系统窗口',
        type: 2,
        skin: 'layui-layer-molv',
        width: "auto",
        height: "auto",
        content: '',
        closeBtn: 0,
        shade: 0.3,
        maxmin: true,
        btn: ['确认', '取消'],
        btnclass: ['btn btn-primary', 'btn btn-danger'],
        yes: null
    };
    var options = $.extend(defaults, options);
    layer.open({
        id: options.id,
        type: options.type,
        scrollbar: false,
        //skin: options.skin,
        shade: options.shade,
        shadeClose: true,
        maxmin: options.maxmin,
        title: options.title,
        fix: false,
        area: [options.width, options.height],
        content: options.content,
        btn: options.btn,
        btnclass: options.btnclass,
        yes: function (index, layero) {
            if (options.yes && $.isFunction(options.yes)) {
                var iframebody = layer.getChildFrame('body', index);
                var iframeWin = layero.find('iframe')[0].contentWindow;
                options.yes(iframebody, iframeWin, index);
            }
        },
        cancel: function () {
            return true;
        }
    });

}

/**
 * 关闭模态窗。
 */
$.layerClose = function () {
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.close(index);
}

/**
 * 创建询问框。
 * @param {Object} options
 */
$.layerConfirm = function (options) {
    var defaults = {
        title: '提示',
        //skin: 'layui-layer-molv',
        content: "",
        icon: 3,
        shade: 0.3,
        anim: 4,
        btn: ['确认', '取消'],
        btnclass: ['btn btn-primary', 'btn btn-danger'],
        callback: null
    };
    var options = $.extend(defaults, options);
    layer.confirm(options.content, {
        title: options.title,
        icon: options.icon,
        btn: options.btn,
        btnclass: options.btnclass,
        //skin: options.skin,
        anim: options.anim
    }, function () {
        if (options.callback && $.isFunction(options.callback)) {
            options.callback();
        }
    }, function () {
        return true;
    });
}
/**
 * 创建一个信息弹窗。
 * @param {String} content 
 * @param {String} type 
 */
$.layerMsg = function (content, type, callback) {
    if (type != undefined) {
        var icon = "";

        switch (type) {
            case 'warning':
            case 0:
                icon = 0; break;
            case 'ok':
            case 1:
            case 'success':
                icon = 1; break;
            case 'err':
            case 'error':
            case 2:
                icon = 2; break;
            case 'question':
                icon = 3; break;
            case 'lock':
            case 4:
            case 'nopermission':
                icon = 4; break;
            case 'longface':
                icon = 5; break;
            case 'info':
            case 6:
                icon = 6; break;
            default:
                icon = 0; break;
        }

        top.layer.msg(content, { icon: icon, time: 2000 }, function () {
            if (callback && $.isFunction(callback)) {
                callback();
            }
        });
    } else {
        top.layer.msg(content, function () {
            if (callback && $.isFunction(callback)) {
                callback();
            }
        });
    }
}

$.procAjaxMsg = function (json, funcSuc, funcErr) {
    if (!json.state) {
        return;
    }
    var state = json.state;
    switch (state) {

        case "success":
            if (funcSuc) {
                funcSuc(json);
            }
            break;
        case "error":
            if (funcErr) {
                funcErr(json);
            }
            break;
        case "nologin":
            //是否登录
            $.alertMsg(json.message, '系统提示', function () {
                if (window != top) {
                    top.location.href = json.data;
                }
                else {
                    window.location.href = json.data;
                }
            }, state);
            break;
        case "nopermission":
            //是否有权
            $.alertMsg(json.message, '系统提示', null, state);
            break;
    }
}

$.validateUrl = function (url, funcSuc, funcErr, type) {
    $.ajax({
        type: type,
        url: url,
        success: function (data) {
            if (data.Msg) {
                funcErr(data);
            } else {
                funcSuc();
            }
        }
    });
}

$.alertMsg = function (msg, title, funcSuc, icon) {
    if (title == '' || title == undefined) title = '提示';
    if (layer) {
        var type = 1;
        if (icon) {
            switch (icon) {
                case 'ok': case 'success': type = 1; break;
                case 'err': case 'error': type = 2; break;
                case 'question': type = 3; break;
                case 'lock': case 'nopermission': type = 4; break;
                case 'longface': type = 5; break;
                case 'smile': type = 6; break;
                default: type = 1;
            }
        }
        layer.open({
            title: title
            , content: msg
            , icon: type
            , yes: function (index) {
                layer.close(index);
                if (typeof (funcSuc) == 'function') {
                    funcSuc();
                }
            }
        });
    }
    else {
        alert(title + "\r\n " + msg);
        if (funcSuc) {
            funcSuc();
        }
    }
},

    /**
     * 绑定Select选项。
     * @param {Object} options
     */
    $.fn.bindSelect = function (options) {
        var defaults = {
            id: "id",
            text: "text",
            search: true,
            //multiple: false,
            title: "请选择",
            url: "",
            param: [],
            change: null
        };
        var options = $.extend(defaults, options);
        var $element = $(this);
        if (options.url != "") {
            $.ajax({
                url: options.url,
                data: options.param,
                type: "post",
                dataType: "json",
                async: false,
                success: function (data) {
                    $.each(data, function (i) {
                        $element.append($("<option></option>").val(data[i][options.id]).html(data[i][options.text]));
                    });
                    $element.select2({
                        placeholder: options.title,
                        //multiple: options.multiple,
                        minimumResultsForSearch: options.search == true ? 0 : -1
                    });
                    $element.on("change", function (e) {
                        if (options.change != null) {
                            options.change(data[$(this).find("option:selected").index()]);
                        }
                        $("#select2-" + $element.attr('id') + "-container").html($(this).find("option:selected").text().replace(/　　/g, ''));
                    });
                }
            });
        } else {
            $element.select2({
                minimumResultsForSearch: -1
            });
        }
    }

/**
 * 绑定Enter提交事件。
 * @param {Object} $event 需要触发的元素或事件。
 */
$.fn.bindEnterEvent = function ($event) {
    var $selector = $(this);
    $.each($selector, function () {
        $(this).unbind("keydown").bind("keydown", function (event) {
            if (event.keyCode == 13) {
                if ($.isFunction($event)) {
                    $event();
                } else {
                    $event.click();
                }
            }
        });
    });
}

/**
 * 绑定Change提交事件。
 * @param {Object} $event 需要触发的元素或事件。
 * 
 */
$.fn.bindChangeEvent = function ($event) {
    var $selector = $(this);
    $.each($selector, function () {
        $(this).unbind("change").bind("change", function (event) {
            if ($.isFunction($event)) {
                $event();
            } else {
                $event.click();
            }
        })
    });
};
/**
 * 控制授权按钮
 */
$.fn.authorizeButton = function (callback) {
    var url = location.pathname + location.search;
    var that = this;
    com.ajax({
        url: '/Admin/SysRoleMenu/GetAuthorizeButton',
        data: { url: url },
        success: function (childModules) {
            if (childModules.length > 0) {
                var $toolbar = $(that);
                var _buttons = '';
                $.each(childModules, function (index, item) {
                    _buttons += "<button id='" + item.EnCode + "' type=\"button\" class=\"btn btn-default\">";
                    _buttons += "   <span class='" + item.IconCls + "' aria-hidden='true'></span> " + item.Name + "";
                    _buttons += "</button>";
                });
                $toolbar.html(_buttons);
            }
            if (typeof (callback) == 'function') {
                callback()
            }
        }
    })

}

/**
 * 获取数据表格选中行主键值。
 */
$.fn.gridSelectedRowValue = function () {
    var $selectedRows = $(this).children('tbody').find("input[type=checkbox]:checked");
    var result = [];
    if ($selectedRows.length > 0) {
        for (var i = 0; i < $selectedRows.length; i++) {
            result.push($selectedRows[i].value);
        }
    }
    return result;
}

/**
 * 获取URL指定参数值。
 * @param {String} name
 */
$.getQueryString = function (name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

/**
 * 序列化和反序列化表单字段。
 * @param {Object} formdate
 * @param {Function} callback 
 */
$.fn.formSerialize = function (formdate, callback) {
    var $form = $(this);
    if (!!formdate) {
        for (var key in formdate) {
            var $field = $form.find("[name=" + key + "]");
            if ($field.length == 0) {
                continue;
            }
            var value = $.trim(formdate[key]);
            var type = $field.attr('type');
            if ($field.hasClass('select2')) {
                type = "select2";
            }
            switch (type) {
                case "checkbox":
                    value == "true" ? $field.attr("checked", 'checked') : $field.removeAttr("checked");
                    break;
                case "select2":
                    if (!$field[0].multiple) {
                        $field.select2().val(value).trigger("change");
                    } else {
                        var values = value.split(',');
                        $field.select2().val(values).trigger("change");
                    }
                    break;
                case "radio":
                    $field.each(function (index, $item) {
                        if ($item.value == value) {
                            $item.checked = true;
                        }
                    });
                    break;
                default:
                    $field.val(value);
                    break;
            }

        };
        // 特殊的表单字段可以在回调函数中手动赋值。
        if (callback && $.isFunction(callback)) {
            callback(formdate);
        }
        return false;
    }
    var postdata = {};
    $form.find('input,select,textarea').each(function (r) {
        var $this = $(this);
        var id = $this.attr('id');
        var type = $this.attr('type');
        switch (type) {
            case "checkbox":
                postdata[id] = $this.is(":checked");
                break;
            default:
                var value = $this.val() == "" ? "&nbsp;" : $this.val();
                if (!$.getQueryString("id")) {
                    value = value.replace(/&nbsp;/g, '');
                }
                postdata[id] = value;
                break;
        }
    });
    //if ($('[name=__RequestVerificationToken]').length > 0) {
    //    postdata["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
    //}
    return postdata;
}

/**
 * 提交表单。
 * @param {Object} options
 */
$.formSubmit = function (options) {
    var defaults = {
        url: "",
        data: {},
        type: "post",
        async: true,
        success: null,
        close: true,
        showMsg: true,
        showLoading: true
    };

    var options = $.extend(defaults, options);

    var index = layer.load(0, { shade: false });

    $.ajax({
        url: options.url,
        data: options.data,
        type: options.type,
        async: options.async,
        dataType: "json",
        success: function (data) {
            layer.close(index);
            if (options.success && $.isFunction(options.success)) {
                options.success(data);
            }
            //if (options.close) {
            //    $.layerClose();
            //}
            if (options.showMsg) {
                $.alertMsg(data.Msg, '', '', data.state);
            }
        },
        error: function (xhr, status, error) {
            layer.close(index);

            var msg = xhr.responseText;
            var errMsg = top.layer.open({
                title: '错误提示',
                area: ['500px', '400px'],
                content: msg
            })
            console.log(status)

            //$.layerMsg(error, "err");
        },
    });
}

/*
 1.function：在一个数组的第index位置插入一个对象或值item
 2.author:luozQ
 3. demo:  var d=[{'id':1},{'id':2}];
           d.insert(0,{'id':0});
           d 的值就变成了 [{'id':0},{'id':1},{'id':2}]
*/
Array.prototype.insert = function (index, item) {
    this.splice(index, 0, item);
};



$.fn.dataGrid = function (options) {
    var defaults = {
        method: 'post', //请求方式（*）  method: 'post', //请求方式（*）
        toolbar: '#toolbar',
        striped: true,
        cache: false,
        pagination: true,
        sortable: true,
        singleSelect: true,
        sortName: 'Id',
        sortStable: true,
        sortOrder: "desc",
        sidePagination: "server",
        pageNumber: 1,
        pageSize: 10,
        pageList: [10, 20, 30, 40, 50],
        strictSearch: true,
        showColumns: true,                  //是否显示所有的列
        showRefresh: true,                  //是否显示刷新按钮
        minimumCountColumns: 2,             //最少允许的列数
        clickToSelect: true,                //是否启用点击选中行
        uniqueId: "Id",                     //每一行的唯一标识，一般为主键列
        showToggle: true,                    //是否显示详细视图和列表视图的切换按钮
        cardView: false,                    //是否显示详细视图
    };
    var options = $.extend(defaults, options);

    var $element = $(this);
    return $element.bootstrapTable(options);
};