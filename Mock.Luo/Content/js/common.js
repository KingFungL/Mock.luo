


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
        close: true,
        showMsg: false,
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
                $.alertMsg(data.Msg, '', '', data.Statu);
            }
        },
        error: function (xhr, status, error) {
            layer.close(index);

            var msg = xhr.responseText;
            var errMsg = top.layer.open({
                title: '错误提示',
                area: ['500px', '400px'],
                content: msg
            });
        }
    });
};