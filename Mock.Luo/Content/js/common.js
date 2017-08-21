


com = {};
/**
* 格式化字符串
* 用法:
.formatString("{0}-{1}","a","b");
*/
com.formatString = function () {
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

            //if (options.showMsg) {
            //    $.alertMsg(data.message, '', function () {
            //        if (options.success && $.isFunction(options.success)) {
            //            options.success(data);
            //        }
            //    }, data.state);
            //}
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


/**
* 模块名：共通脚本
* 程序名: 通用工具函数
**/
var com = $.extend({}, com);/* 定义全局对象，类似于命名空间或包的作用 */

//去除表单中所有按钮,并且将所有文本框置为禁用
com.ignoreEle = function (dom) {
    dom.find('input').addClass("layui-disabled").attr("disabled", "");
    dom.find('textarea').addClass('layui-disabled').attr("disabled", "");
    dom.find('select').addClass('layui-disabled').prop("disabled", true);
    dom.find('button').remove();
    dom.find('a').removeAttr('href').removeAttr('onclick');//去掉a标签中的href属性去掉a标签中的onclick事件
};

//获取当前登录的使用信息
com.getCurrentInfo = function () {
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
com.getparams = function () {
    var urlParameters = location.search;
    return com.geturlparams(urlParameters);
};
com.geturlparams = function (urlParameters) {
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
* demo: com.formatString("{0}-{1}","a","b");
*/
com.formatString = function () {
    for (var i = 1; i < arguments.length; i++) {
        var exp = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        arguments[0] = arguments[0].replace(exp, arguments[i]);
    }
    return arguments[0];
};

/***是否启用*/
com.formatIsEnable = function (value, row) {
    var enableMark = '', selectwith = '', leftpx = 5;
    if (value == true) {
        enableMark = "启用";
        selectwith = 'layui-form-onswitch';
        leftpx = 32;
    } else {
        enableMark = "禁用";
        selectwith = '';
        leftpx = 5;
    }
    var check = com.formatString(
        '<div class="layui-box layui-unselect layui-form-switch {0}" lay- skin="_switch" style="margin-top:0px;" >\
            <em> {1}</em >\
            <i style= "top:3px;left:{2}px;" ></i>\
         </div>', selectwith, enableMark, leftpx);
    return check;
};

/**
*把表单元素序列化成对象
*/
com.serializeObject = function (form) {
    var o = {};
    if (!form) return o;
    $.each(form.serializeArray(), function (intdex) {
        if (o[this['name']]) {
            o[this['name']] = o[this['name']] + "," + this['value'];
        } else {
            o[this['name']] = this['value'];
        }
    });
    var $radio = form.find('input[type=radio],input[type=checkbox]');
    $.each($radio, function () {
        if (!o.hasOwnProperty(this.name)) {
            o[this.name] = false;
        } else if (this['value'] === 'on') {
            o[this['name']] = true;
        }
    });
    return o;
};
com.dialog = function (options) {
    //var index = layer.load(0, { shade: 0.3 });
    options = $.extend({
        type: 1,
        title: '标题',
        resize: false,
        url: '',
        shadeClose: true,
        value: '',
        name: 'id'
    }, options);
    $.get(options.url, {}, function (data) {
        options.content = '<input type="hidden" name="' + options.name + '" value="' + options.value + '"/>' + data;
        layer.open(options);
    }, 'html');
};
com.renderhtml = function (options) {
    //key是往后台传的键,
    //name是页面上隐藏的主键名称
    options = $.extend({
        url: '',
        key: 'id',
        name: 'id',
        editlayer: '#v-app',
        callback: ''
    }, options);

    var keyValue = $('input[name=' + options.name + ']').val();
    var key = options.key;
    var data = {};
    data[key] = keyValue;
    if (!!keyValue) {
        $.get(options.url + '/' + keyValue, {}, function (data) {
            var app = new Vue({
                el: options.editlayer,
                data: data
            });
            if (typeof (options.callback) == 'function') {
                options.callback(data);
            }
        }, 'json');
    } else {
        if (typeof (options.callback) == 'function') {
            options.callback();
        }
    }
};


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
        d += com.formatString('<button onclick="{0}(\'{1}\',{2})" class="layui-btn layui-btn-small {3}"> {4}</button>', v.func, v.title, v.id, v.css, v.show);
    });
    d += '</div>';
    return d;
};


com.upload_one_image = function (dialog_title, input_selector) {
    layer.open({
        type: 2,
        area: ['650px', '420px'],
        skin: 'layui-layer-molv',
        title: dialog_title,
        maxmin: true,
        content: '/Plat/Upload/ImageView',
        btn: ['确定', '取消'],
        yes: function (index, layero) {
            var iframeWin = window[layero.find('iframe')[0]['name']]; //得到iframe页的窗口对象，执行iframe页的方法：iframeWin.method();
            files = iframeWin.get_selected_files();
            if (files) {
                if (files.length > 1) {
                    $.layerMsg('只能选中一张图片', 'info');
                    return;
                }
                $(input_selector).val(files[0].url);
                $(input_selector + '-preview').attr('src', files[0].url);
                layer.close(index);
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
            //area: [img.width / 2 + 'px', img.height / 2 + 6 + 'px'], //宽高
            //content: '<div><img src=' + url + ' style="margin-top:2px;width:' + (img.width / 2) + 'px;height:' + (img.height / 2) + 'px;"/></div>'
            area: ['227px', '190px'],
            content: '<div class="text-center"><img src=' + url + ' style="max-width:227px;max-height:190px; min-width:180px;min-height:150px;"/></div>'
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
com.delete = function (url, element) {
    var Id = 0;
    if (!element) {
        element = '#dginfo'
    }
    if ($(element).bootstrapTable('getSelections')[0] != undefined) {
        var row = $(element).bootstrapTable('getSelections')[0];
        Id = row.Id;
    };
    if (Id == 0) { layer.msg('请先选择需要删除的信息！'); return; }
    $.layerConfirm({
        content: '是否删除这条信息?',
        callback: function () {
            com.ajax({
                url: url + '/' + Id,
                type: 'post',
                success: function (dataJson) {
                    $.procAjaxMsg(dataJson, function () {
                        layer.msg(dataJson.message);
                        $(element).bootstrapTable("refresh");
                    }, function () {
                        layer.msg(dataJson.message);
                    });
                }
            });
        }
    });
}


com.getselectid = function (dgelement) {
    if (!dgelement) {
        dgelement = '#dginfo'
    }
    var Id = 0;
    if ($('#dginfo').bootstrapTable('getSelections')[0] != undefined) {
        var row = $(dgelement).bootstrapTable('getSelections')[0];
        Id = row.Id;
    };
    if (Id == 0) {
        layer.msg('请先选择需要编辑的信息！');
    }
    return Id;
}