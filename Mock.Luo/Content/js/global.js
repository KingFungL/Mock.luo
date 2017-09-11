/**
 * 创建模态窗。
 * @param {Object} options
 */
$.layerOpen = function (options) {
    var defaults = {
        id: "default" + Math.random(),
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
    if (!com.ispc()) {
        options.width = '100%';
        options.height = '100%';
    }

    top.layer.open({
        id: options.id,
        type: options.type,
        scrollbar: false,
        skin: options.skin,
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
        skin: 'layui-layer-molv',
        content: "",
        icon: 3,
        resize: false,
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
        resize: options.resize,
        skin: options.skin,
        anim: options.anim
    }, function (index) {
        if (options.callback && $.isFunction(options.callback)) {
            options.callback();
        }
        layer.close(index);
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
    /**
   * 绑定Select选项。
   * @param {Object} options
   */
    $.fn.bindSelect = function (options) {
        var defaults = {
            id: "id",
            text: "text",
            search: true,
            multiple: false,
            title: "==请选择==",
            url: "",
            param: [],
            change: null
        };
        var options = $.extend(defaults, options);
        options.placeholder = options.title;
        options.minimumResultsForSearch = options.search == true ? 0 : -1;

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
                    $element.select2(options);
                    $element.on("change", function (e) {
                        if (options.change != null) {
                            options.change(data[$(this).find("option:selected").index()]);
                        }
                        $("#select2-" + $element.attr('id') + "-container").html($(this).find("option:selected").text());
                        return false;
                    });
                }
            });
        } else {
            $element.select2(options);
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


/*封装bootstrapTable的表格默认功能*/
$.fn.dataGrid = function (options) {
    var $element = $(this);
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
        onToggle: function (cardView) {
            var element = $element.parent('.fixed-table-body');
            if (cardView == true) {
                element.removeClass('table-responsive')
            } else {
                element.addClass('table-responsive');
            }
        },
        onLoadSuccess: function (data) {
            $element.parent('.fixed-table-body').addClass('table-responsive');
            if (typeof (options.callback) == 'function') {
                options.callback(data);
            }
        }
    };
    options = $.extend(defaults, options);
    return $element.bootstrapTable(options);
};
/*封装jqgrid的表格默认功能*/
//$.fn.jqGrid = function (options) {
//    var defaults = {
//        datatype: "json",
//        autowidth: true,
//        rownumbers: true,
//        shrinkToFit: false,
//        gridview: true
//    };
//    var options = $.extend(defaults, options);
//    var $element = $(this);
//    return $element.jqGrid(options);
//};


/**/
$.fn.jqGridRowValue = function () {
    var $grid = $(this);
    var selectedRowIds = $grid.jqGrid("getGridParam", "selarrrow");
    if (selectedRowIds != "") {
        var json = [];
        var len = selectedRowIds.length;
        for (var i = 0; i < len; i++) {
            var rowData = $grid.jqGrid('getRowData', selectedRowIds[i]);
            json.push(rowData);
        }
        return json;
    } else {
        return $grid.jqGrid('getRowData', $grid.jqGrid('getGridParam', 'selrow'));
    }
}

$.fn.jqGridRowValue = function () {
    var $grid = $(this);
    var selectedRowIds = $grid.jqGrid("getGridParam", "selarrrow");
    if (selectedRowIds != "") {
        var json = [];
        var len = selectedRowIds.length;
        for (var i = 0; i < len; i++) {
            var rowData = $grid.jqGrid('getRowData', selectedRowIds[i]);
            json.push(rowData);
        }
        return json;
    } else {
        return $grid.jqGrid('getRowData', $grid.jqGrid('getGridParam', 'selrow'));
    }
}

$.fn.comboBoxTree = function (options) {
    var u = $(this),
        e = u.attr("id");
    if (!e) return !1;
    var i = $.extend({
        description: "==请选择==",
        id: "id",
        text: "text",
        title: "title",
        height: null,
        width: null,
        allowSearch: !1,
        url: !1,
        param: null,
        method: "GET",
        appendTo: null,
        click: null,
        icon: !1,
        data: null,
        dataItemName: !1
    }, options),
        f = {
            rendering: function () {
                var t, r;
                return u.find(".ui-select-text").length == 0 && u.html("<div class=\"ui-select-text\" style='color:#999;'>" + i.description + "<\/div>"), t = '<div class="ui-select-option">', t += '<div class="ui-select-option-content" style="max-height: ' + i.maxHeight + '"><\/div>', i.allowSearch && (t += '<div class="ui-select-option-search"><input type="text" class="form-control" style="padding:0px;" placeholder="搜索关键字" /><span class="input-query" title="回车搜索"><i class="fa fa-search"><\/i><\/span><\/div>'), t += "<\/div>", r = $(t), r.attr("id", e + "-option"), i.appendTo ? $(i.appendTo).prepend(r) : $("body").prepend(r), $("#" + e + "-option")
            }, loadtreeview: function (n, t) {
                o.treeview({
                    onnodeclick: function (t) {
                        if (n.click) {
                            var i = "ok";
                            if (i = n.click(t), i == "false") return !1
                        }
                        u.attr("data-value", t.id).attr("data-text", t.text);
                        u.find(".ui-select-text").html(t.text).css("color", "#000");
                        u.trigger("change")
                    }, height: n.height,
                    data: t,
                    description: n.description
                })
            }, loadData: function (i) {
                var r = [];
                r = i.data ? i.data : com.asyncGet({
                    url: i.url,
                    data: i.param,
                    type: i.method
                });
                i.dataItemName ? (i.data = [], $.each(r, function (n, t) {
                    var r = top.learun.data.get(["dataItem", i.dataItemName, t[i.text]]);
                    r != "" && (t[i.text] = r);
                    i.data.push(t)
                })) : i.data = r
            }, searchData: function (t, i) {
                var u = !1,
                    r = [];
                return $.each(t, function (n, t) {
                    var e = {},
                        o, s;
                    for (o in t) o != "ChildNodes" && (e[o] = t[o]);
                    s = !1;
                    e.text.indexOf(i) != -1 && (s = !0);
                    e.hasChildren && (e.ChildNodes = f.searchData(t.ChildNodes, i), e.ChildNodes.length > 0 ? s = !0 : e.hasChildren = !1);
                    s && (u = !0, r.push(e))
                }), r
            }
        },
        r = f.rendering(),
        o = $("#" + e + "-option").find(".ui-select-option-content");
    return f.loadData(i), f.loadtreeview(i, i.data), i.allowSearch && (r.find(".ui-select-option-search").find("input").bind("keypress", function () {
        if (event.keyCode == "13") {
            var t = $(this),
                i = $(this).val(),
                r = f.searchData(t[0].opt.data, i);
            f.loadtreeview(t[0].opt, r)
        }
    }).focus(function () {
        $(this).select()
    })[0].opt = i), i.icon && (r.find("i").remove(), r.find("img").remove()), u.find(".ui-select-text").unbind("click"), u.find(".ui-select-text").bind("click", function (t) {
        var s;
        if (u.attr("readonly") == "readonly" || u.attr("disabled") == "disabled") return !1;
        if ($(this).parent().addClass("ui-select-focus"), r.is(":hidden")) {
            u.find(".ui-select-option").hide();
            $(".ui-select-option").hide();
            var o = u.offset().left,
                f = u.offset().top + 37,
                e = u.width();
            i.width && (e = i.width);
            r.height() + f < $(window).height() ? r.slideDown(150).css({
                top: f,
                left: o,
                width: e
            }) : (s = f - r.height() - 32, r.show().css({
                top: s,
                left: o,
                width: e
            }), r.attr("data-show", !0));
            r.css("border-top", "1px solid #ccc");
            i.appendTo && r.css("position", "inherit");
            r.find(".ui-select-option-search").find("input").select()
        } else r.attr("data-show") ? r.hide() : r.slideUp(150);
        t.stopPropagation()
    }), u.find("li div").click(function (t) {
        var t = t ? t : window.event,
            i = t.srcElement || t.target;
        $(i).hasClass("bbit-tree-ec-icon") || (r.slideUp(150), t.stopPropagation())
    }), $(document).click(function (t) {
        var t = t ? t : window.event,
            i = t.srcElement || t.target;
        $(i).hasClass("bbit-tree-ec-icon") || $(i).hasClass("form-control") || (r.attr("data-show") ? r.hide() : r.slideUp(150), u.removeClass("ui-select-focus"), t.stopPropagation())
    }), u
}
$.fn.comboBoxTreeSetValue = function (i) {
    if (!!i) {
        var r = $(this),
            u = $("#" + r.attr("id") + "-option").find(".ui-select-option-content");
        return u.find("ul").find("[data-value=" + i + "]").trigger("click"), r
    }
}

