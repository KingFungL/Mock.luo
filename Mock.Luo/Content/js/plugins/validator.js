﻿function ValidationMessage(n, t) {
    try {
        removeMessage(n);
        n.focus();
        if(n.next().hasClass("layui-form-select")){
            layer.tips(t, n.next(), {
                tips: 3
            });
        }else{
            layer.tips(t, n, {
                tips: 3
            });
        }
        return (n.hasClass("layui-input") || n.hasClass("ui-select") || n.next().hasClass("layui-form-select")) && n.addClass("has-error"), n.hasClass("ui-select") , n.change(function () {
            n.val() && removeMessage(n)
        }), n.bind("input propertychange", function () {
            n.val() && removeMessage(n)
        }), n.hasClass("input-datepicker") && $(document).click(function (t) {
            !n.val() || removeMessage(n);
            t.stopPropagation()
        }), n.hasClass("ui-select") && $(document).click(function (t) {
            !n.attr("data-value") || removeMessage(n);
            t.stopPropagation()
        }), !1
    } catch (r) {
        alert(r)
    }
}

function removeMessage(n) {
    n.removeClass("has-error");
}
$.fn.Validform = function () {
    function r(n) {
        return n = $.trim(n), n == null || n == undefined || n.length == 0 ? !0 : !1
    }

    function u(n) {
        return reg = /^[-+]?\d+$/, reg.test(n) ? !0 : !1
    }

    function f(n) {
        var t = $.trim(n);
        return t == null || t == undefined || t.length == 0 ? !0 : (reg = /^[-+]?\d+$/, reg.test(n) ? !0 : !1)
    }

    function e(n) {
        return reg = /^\w{3,}@\w+(\.\w+)+$/, reg.test(n) ? !0 : !1
    }

    function o(n) {
        var t = $.trim(n);
        return t == null || t == undefined || t.length == 0 ? !0 : (reg = /^\w{3,}@\w+(\.\w+)+$/, reg.test(n) ? !0 : !1)
    }

    function s(n) {
        return reg = /^[a-z,A-Z]+$/, reg.test(n) ? !0 : !1
    }

    function h(n) {
        var t = $.trim(n);
        return t == null || t == undefined || t.length == 0 ? !0 : (reg = /^[a-z,A-Z]+$/, reg.test(n) ? !0 : !1)
    }

    function c(n, t) {
        return (reg = /^[0-9]+$/, n = $.trim(n), n.length > t) ? !1 : reg.test(n) ? !0 : !1
    }

    function l(n, t) {
        var i = $.trim(n);
        return i.length == 0 || i == null || i == undefined ? !0 : (reg = /^[0-9]+$/, n = $.trim(n), n.length > t) ? !1 : reg.test(n) ? !0 : !1
    }

    function a(n, t) {
        return n = $.trim(n), n.length == 0 || n.length > t ? !1 : !0
    }

    function v(n, t) {
        var i = $.trim(n);
        return i == null || i == undefined || i.length == 0 ? !0 : (n = $.trim(n), n.length > t ? !1 : !0)
    }

    function y(n) {
        return reg = /^(\d{3,4}\-)?[1-9]\d{6,7}$/, reg.test(n) ? !0 : !1
    }

    function i(n) {
        var t = $.trim(n);
        return t == null || t == undefined || t.length == 0 ? !0 : (reg = /^(\d{3,4}\-)?[1-9]\d{6,7}$/, reg.test(n) ? !0 : !1)
    }

    function p(n) {
        return reg = /^(\+\d{2,3}\-)?\d{11}$/, reg.test(n) ? !0 : !1
    }

    function w(n) {
        var t = $.trim(n);
        return t == null || t == undefined || t.length == 0 ? !0 : (reg = /^(\+\d{2,3}\-)?\d{11}$/, reg.test(n) ? !0 : !1)
    }

    function b(n) {
        return reg_mobile = /^(\+\d{2,3}\-)?\d{11}$/, reg_phone = /^(\d{3,4}\-)?[1-9]\d{6,7}$/, reg_mobile.test(n) || reg_phone.test(n) ? !0 : !1
    }

    function k(n) {
        var t = $.trim(n);
        return t == null || t == undefined || t.length == 0 ? !0 : (reg = /^(\+\d{2,3}\-)?\d{11}$/, reg2 = /^(\d{3,4}\-)?[1-9]\d{6,7}$/, reg.test(n) || reg2.test(n) ? !0 : !1)
    }

    function d(n) {
        return reg = /^http:\/\/[a-zA-Z0-9]+\.[a-zA-Z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\"\"])*$/, reg.test(n) ? !0 : !1
    }

    function g(n) {
        var t = $.trim(n);
        return t.length == 0 || t == null || t == undefined ? !0 : (reg = /^http:\/\/[a-zA-Z0-9]+\.[a-zA-Z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\"\"])*$/, reg.test(n) ? !0 : !1)
    }

    function nt(n, t) {
        return n.length != 0 && t.length != 0 ? n == t ? !0 : !1 : !1
    }

    function tt(n) {
        if (n.length != 0) return reg = /^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/, reg.test(n) ? !0 : !1
    }

    function it(n) {
        var t = $.trim(n);
        return t == null || t == undefined || t.length == 0 ? !0 : n.length != 0 ? (reg = /^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/, reg.test(n) ? !0 : !1) : void 0
    }

    function rt(n) {
        if (n.length != 0) return reg = /^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$/, reg.test(n) ? !0 : !1
    }

    function ut(n) {
        var t = $.trim(n);
        return t == null || t == undefined || t.length == 0 ? !0 : n.length != 0 ? (reg = /^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$/, reg.test(n) ? !0 : !1) : void 0
    }

    function ft(n) {
        if (n.length != 0) return reg = /^((20|21|22|23|[0-1]\d)\:[0-5][0-9])(\:[0-5][0-9])?$/, reg.test(n) ? !0 : !1
    }

    function et(n) {
        var t = $.trim(n);
        return t == null || t == undefined || t.length == 0 ? !0 : n.length != 0 ? (reg = /^((20|21|22|23|[0-1]\d)\:[0-5][0-9])(\:[0-5][0-9])?$/, reg.test(n) ? !0 : !1) : void 0
    }

    function ot(n) {
        if (n.length != 0) return reg = /^[\u0391-\uFFE5]+$/, reg.test(str) ? !0 : !1
    }

    function st(n) {
        var t = $.trim(n);
        return t == null || t == undefined || t.length == 0 ? !0 : n.length != 0 ? (reg = /^[\u0391-\uFFE5]+$/, reg.test(str) ? !0 : !1) : void 0
    }

    function ht(n) {
        if (n.length != 0) return reg = /^\d{6}$/, reg.test(str) ? !0 : !1
    }

    function ct(n) {
        var t = $.trim(n);
        return t == null || t == undefined || t.length == 0 ? !0 : n.length != 0 ? (reg = /^\d{6}$/, reg.test(str) ? !0 : !1) : void 0
    }

    function lt(n) {
        if (n.length != 0) return reg = /^[-\+]?\d+(\.\d+)?$/, reg.test(n) ? !0 : !1
    }

    function at(n) {
        var t = $.trim(n);
        return t == null || t == undefined || t.length == 0 ? !0 : n.length != 0 ? (reg = /^[-\+]?\d+(\.\d+)?$/, reg.test(n) ? !0 : !1) : void 0
    }

    function vt(n) {
        if (n.length != 0) return reg = /^\d{15}(\d{2}[A-Za-z0-9;])?$/, reg.test(n) ? !0 : !1
    }

    function yt(n) {
        var t = $.trim(n);
        return t == null || t == undefined || t.length == 0 ? !0 : n.length != 0 ? (reg = /^\d{15}(\d{2}[A-Za-z0-9;])?$/, reg.test(n) ? !0 : !1) : void 0
    }

    function pt(n) {
        return /^(\d+)\.(\d+)\.(\d+)\.(\d+)$/g.test(n) && RegExp.$1 < 256 && RegExp.$2 < 256 && RegExp.$3 < 256 && RegExp.$4 < 256 ? !0 : !1
    }
    var n = "",
        t = !0;
    return ($(this).find("[isvalid=yes]").each(function () {
        var kt = $(this).attr("checkexpession"),
            wt = $(this).attr("errormsg"),
            bt;
        if (kt != undefined) {
            wt == undefined && (wt = "");
            bt = $(this).hasClass("ui-select") ? $(this).attr("data-value") : $(this).val();
            switch (kt) {
                case "NotNull":
                    if (r(bt)) return n = wt + "不能为空！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "Num":
                    if (!u(bt)) return n = wt + "必须为数字！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "NumOrNull":
                    if (!f(bt)) return n = wt + "数字或空！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "Email":
                    if (!e(bt)) return n = wt + "必须为E-mail格式！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "EmailOrNull":
                    if (!o(bt)) return n = wt + "为E-mail格式或空！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "EmailAndNotNull":
                    if (!o(bt) || r(bt)) return n = wt + "为E-mail格式且不能为空！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "EnglishStr":
                    if (!s(bt)) return n = wt + "必须为字符串！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "EnglishStrOrNull":
                    if (!h(bt)) return n = wt + "字符串或空！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "LenNum":
                    if (!c(bt, $(this).attr("length"))) return n = wt + "必须为" + $(this).attr("length") + "位数字！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "LenNumOrNull":
                    if (!l(bt, $(this).attr("length"))) return n = wt + "必须为" + $(this).attr("length") + "位数字！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "LenStr":
                    if (!a(bt, $(this).attr("length"))) return n = wt + "必须小于" + $(this).attr("length") + "位字符！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "LenStrOrNull":
                    if (!v(bt, $(this).attr("length"))) return n = wt + "必须小于" + $(this).attr("length") + "位字符！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "Phone":
                    if (!y(bt)) return n = wt + "必须电话格式！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "PhoneOrNull":
                    if (!i(bt)) return n = wt + "电话格式或者空！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "Fax":
                    if (!i(bt)) return n = wt + "必须为传真格式！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "Mobile":
                    if (!p(bt)) return n = wt + "必须为手机格式！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "MobileOrNull":
                    if (!w(bt)) return n = wt + "手机格式或者空！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "MobileOrPhone":
                    if (!b(bt)) return n = wt + "电话格式或手机格式！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "MobileOrPhoneOrNull":
                    if (!k(bt)) return n = wt + "电话格式或手机格式或空！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "Uri":
                    if (!d(bt)) return n = wt + "必须为网址格式！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "UriOrNull":
                    if (!g(bt)) return n = wt + "网址格式或空！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "Equal":
                    if (!nt(bt, $(this).attr("eqvalue"))) return n = wt + "不相等！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "Date":
                    if (!tt(bt, $(this).attr("eqvalue"))) return n = wt + "必须为日期格式！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "DateOrNull":
                    if (!it(bt, $(this).attr("eqvalue"))) return n = wt + "必须为日期格式！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "DateTime":
                    if (!rt(bt, $(this).attr("eqvalue"))) return n = wt + "必须为日期时间格式！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "DateTimeOrNull":
                    if (!ut(bt, $(this).attr("eqvalue"))) return n = wt + "必须为日期时间格式！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "Time":
                    if (!ft(bt, $(this).attr("eqvalue"))) return n = wt + "必须为时间格式！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "TimeOrNull":
                    if (!et(bt, $(this).attr("eqvalue"))) return n = wt + "必须为时间格式！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "ChineseStr":
                    if (!ot(bt, $(this).attr("eqvalue"))) return n = wt + "必须为中文！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "ChineseStrOrNull":
                    if (!st(bt, $(this).attr("eqvalue"))) return n = wt + "必须为中文！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "Zip":
                    if (!ht(bt, $(this).attr("eqvalue"))) return n = wt + "必须为邮编格式！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "ZipOrNull":
                    if (!ct(bt, $(this).attr("eqvalue"))) return n = wt + "必须为邮编格式！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "Double":
                    if (!lt(bt, $(this).attr("eqvalue"))) return n = wt + "必须为小数！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "DoubleOrNull":
                    if (!at(bt, $(this).attr("eqvalue"))) return n = wt + "必须为小数！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "IDCard":
                    if (!vt(bt, $(this).attr("eqvalue"))) return n = wt + "必须为身份证格式！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "IDCardOrNull":
                    if (!yt(bt, $(this).attr("eqvalue"))) return n = wt + "必须为身份证格式！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "IsIP":
                    if (!pt(bt, $(this).attr("eqvalue"))) return n = wt + "必须为IP格式！\n", t = !1, ValidationMessage($(this), n), !1;
                    break;
                case "IPOrNull":
                    if (!isIPOrNullOrNull(bt, $(this).attr("eqvalue"))) return n = wt + "必须为IP格式！\n", t = !1, ValidationMessage($(this), n), !1
            }
        }
    }), $(this).find("[fieldexist=yes]").length > 0) ? !1 : t
}