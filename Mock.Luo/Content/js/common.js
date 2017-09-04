
/**
* 模块名：共通脚本
* 程序名: 通用工具函数
**/
var com = {};
/**
* 格式化字符串
* 用法:
.format_str("{0}-{1}","a","b");
*/
com.format_str = function () {
    for (var i = 1; i < arguments.length; i++) {
        var exp = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        arguments[0] = arguments[0].replace(exp, arguments[i]);
    }
    return arguments[0];
};


//发送ajax请求
com.ajax = function (options) {
    var defaults = {
        url: "",
        data: {},
        type: "post",
        async: true,
        success: null,
        close: options.showMsg || false,
        showMsg: false,
        showLoading: true
    };
    var options = $.extend(defaults, options);


    var index = 0;
    if (options.showLoading == true) {
        index = layer.load(0, { shade: false });
    }

    $.ajax({
        url: options.url,
        data: options.data,
        type: options.type,
        async: options.async,
        dataType: "json",
        success: function (data) {
            if (options.showLoading == true) {
                layer.close(index);
            }

            if (options.showMsg) {
                $.procAjaxMsg(data, function () {
                    //默认操作成功后，关闭iframe框
                    if (options.close) {
                        $.layerClose();
                    }
                    //然后弹出成功操作的提示
                    $.layerMsg(data.message, data.state, function () {
                        //回调成功后的刷新表格操作
                        if (options.success && $.isFunction(options.success)) {
                            options.success(data);
                        }
                    });
                }, function () {
                    //操作失败时
                    $.layerMsg(data.message, data.state);
                })
            } else {
                if (options.success && $.isFunction(options.success)) {
                    options.success(data);
                }
            }
        },
        error: function (xhr, status, error) {
            layer.close(index);
            debugger;
            var msg = xhr.responseText;
            var errMsg = top.layer.open({
                title: '错误提示',
                area: ['500px', '400px'],
                content: msg
            });
        }
    });
};




//去除表单中所有按钮,并且将所有文本框置为禁用
com.ignoreEle = function (dom) {
    dom.find('input').addClass("layui-disabled").attr("disabled", "");
    dom.find('textarea').addClass('layui-disabled').attr("disabled", "");
    dom.find('select').addClass('layui-disabled').prop("disabled", true);
    dom.find('button').remove();
    dom.find('a').removeAttr('href').removeAttr('onclick');//去掉a标签中的href属性去掉a标签中的onclick事件
};

//获取当前登录的个人信息
com.get_currentinfo = function () {
    var d = '';
    com.ajax({
        url: '/Login/GetCurrentUserInfo',
        async: false,
        type: 'get',
        success: function (data) {
            d = data;
        }
    });
    return d;
};

//返回当前 URL 的查询部分（问号 ? 之后的部分）。
com.get_params = function () {
    var urlParameters = location.search;
    return com.get_urlparams(urlParameters);
};
com.get_urlparams = function (urlParameters) {
    var requestParameters = new Object();
    if (urlParameters.indexOf('?') != -1) {
        var parameters = decodeURI(urlParameters.substr(1));
        parameterArray = parameters.split('&');
        for (var i = 0; i < parameterArray.length; i++) {
            requestParameters[parameterArray[i].split('=')[0]] = (parameterArray[i].split('=')[1]);
        }
    }
    else {
        console.info('There is no request parameters');
    }
    return requestParameters;
};

/*
* function:格式化字符串
* demo: com.format_str("{0}-{1}","a","b");
*/
com.format_str = function () {
    for (var i = 1; i < arguments.length; i++) {
        var exp = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        arguments[0] = arguments[0].replace(exp, arguments[i]);
    }
    return arguments[0];
};

/***是否启用*/
com.format_enable = function (value, row) {
    var text = '数据为空', label_class = 'warning';
    if (value === true) {
        text = '启用';
        label_class = 'success';
    } else if (value === false) {
        text = "禁用";
        label_class = 'danger';
    }
    var check = com.format_str('<span class="label label-{0}">{1}</span>', label_class, text);
    return check;
};

com.format_yes = function (value, row) {
    var text = '数据为空', label_class = 'warning';
    if (value == true) {
        text = '是';
        label_class = 'success';
    } else {
        text = "否";
        label_class = 'danger';
    }
    return com.format_str('<span class="label label-{0}">{1}</span>', label_class, text);
}

com.button = function (obj) {
    var func, title, id, css, show;
    /*
 
    var obj=[{
      'func':'new ButtonInit().showdetail',
      'title':'查看详情',
      'id':1,
      'css':'warm',
      'show':'查看详情'
    }]
    */


    if (!(obj && obj instanceof Array && obj.length > 0)) {
        console.error('obj参数必须是数组！');
        return '';
    }


    var d = '<div class="btn-group" style= "width:100%;text-align:center;" >';
    $.each(obj, function (i, v) {
        if (!v.id) {
            console.error("id是必须的！");
            return '';
        }
        d += com.format_str('<button onclick="{0}(\'{1}\',{2})" class="layui-btn layui-btn-small {3}"> {4}</button>', v.func, v.title, v.id, v.css, v.show);
    });
    d += '</div>';
    return d;
};


com.upload_one_image = function (dialog_title, input_selector) {
    $.layerOpen({
        area: ['650px', '420px'],
        title: dialog_title,
        maxmin: true,
        content: '/Mock/Upload/ImageView',
        yes: function (iframebody, iframeWin, index) {
            var files = iframeWin.get_selected_files();
            if (files) {
                if (files.length > 1) {
                    $.layerMsg('只能选中一张图片', 'info');
                    return;
                }
                $('#' + input_selector).val(files[0]);
                $('#' + input_selector + '-preview').attr('src', files[0]);
                vm[input_selector] = files[0];
                top.layer.close(index);
            } else {
                $.layerMsg('请选择你上传的图片！', 'info')
            }
        }
    });
};
com.image_preview_dialog = function (url) {
    var img = new Image();
    img.src = url;
    img.onload = function () {
        top.layer.open({
            type: 1,
            title: false,
            closeBtn: 0,
            shadeClose: true,
            area: [img.width / 2 + 'px', img.height / 2 + 6 + 'px'], //宽高
            content: '<div><img src=' + url + ' style="margin-top:2px;width:' + (img.width / 2) + 'px;height:' + (img.height / 2) + 'px;"/></div>'
            //area: ['227px', '190px'],
            //content: '<div class="text-center"><img src=' + url + ' style="max-width:227px;max-height:190px; min-width:180px;min-height:150px;"/></div>'
        });
    };
    img.onerror = function () {
        layer.msg('图片已不存在！');
    }
}

/*
function:统一的删除方法,必要的条件是，列表数据的主键为Id,后端使用统一的删除方法
date:2017-8-10
*/
com.delete = function (url, element, type) {
    var Id = 0;
    if (!element) {
        element = '#dginfo'
    }
    if (!type) {
        type = 'boot';
    }
    var $grid = $(element);
    Id = com.get_selectid(element, type);

    if (!Id) { layer.msg('请先选择需要删除的信息！'); return; }
    $.layerConfirm({
        content: '是否删除这条信息?',
        callback: function () {
            com.ajax({
                url: url + '/' + Id,
                type: 'post',
                success: function (dataJson) {
                    $.procAjaxMsg(dataJson, function () {
                        layer.msg(dataJson.message);
                        if (type == 'boot') {
                            $(element).bootstrapTable("refresh");
                        } else {
                            $(element).trigger('reloadGrid').jqGrid('resetSelection');
                        }
                    }, function () {
                        layer.msg(dataJson.message);
                    });
                }
            });
        }
    });
}

/*得到选中的节点id（单选）*/
com.get_selectid = function (dgelement, type) {
    if (!dgelement) {
        dgelement = '#dginfo'
    }
    var Id = 0;
    var $grid = $(dgelement);
    //如果是jqGrid的列表，使用jqGrid的方法获取主键值
    if (type === 'jq') {
        var row = $grid.jqGridRowValue();
        //当得到的行记录不是数组时，则为对象
        if (!(row instanceof Array)) {
            Id = row.Id;
        };
    } else {
        if ($grid.bootstrapTable('getSelections')[0] != undefined) {
            var row = $(dgelement).bootstrapTable('getSelections')[0];
            Id = row.Id;
        };
    }
    return Id;
}
//编辑前统一提示信息,当Id为0时，说明未选中任何记录，其他时，将Id,作为回调函数的参数
com.edit = function (dgelement, type, callback) {
    var Id = com.get_selectid(dgelement, type);
    if (!Id) {
        layer.msg('请先选择需要编辑的信息！');
    } else {
        callback(Id);
    }
}



com.ispc = function () {
    var userAgentInfo = navigator.userAgent;
    var Agents = ["Android", "iPhone",
        "SymbianOS", "Windows Phone",
        "iPad", "iPod"];
    var flag = true;
    for (var v = 0; v < Agents.length; v++) {
        if (userAgentInfo.indexOf(Agents[v]) > 0) {
            flag = false;
            break;
        }
    }
    return flag;
}
com.asyncGet = function (t) {
    var i = null,
        t = $.extend({
            type: "GET",
            dataType: "json",
            async: !1,
            cache: !1,
            success: function (n) {
                i = n
            }
        }, t);
    return $.ajax(t), i
}

com.tabiframe_Name = function () {
    return top.$(".J_iframe:visible").attr("name")
};
/*得到当前可见的iframe对象*/
com.currentIframe = function () {
    return top.frames[com.tabiframe_Name()].contentWindow != undefined ? top.frames[com.tabiframe_Name()].contentWindow : top.frames[com.tabiframe_Name()]
}
com.checkedRow = function (n) {
    var i = !0;
    return n == undefined || n == "" || n == "null" || n == "undefined" ? (i = !1, $.layerMsg(
        "请先选中一条记录后再操作！")): n.split(",").length > 1 && $.layerMsg(
            "很抱歉,一次只能选择一条记录！", 0)
}