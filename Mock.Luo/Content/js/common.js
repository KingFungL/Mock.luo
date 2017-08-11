


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