
/**
* 模块名：共通脚本
* 程序名: 通用工具函数
**/
window.com = {};
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
        close: true,
        showMsg: false,
        showLoading: true
    };
    options = $.extend(defaults, options);


    var index = 0;
    if (options.showLoading === true) {
        index = layer.load(0, { shade: false });
    }

    $.ajax({
        url: options.url,
        data: options.data,
        type: options.type,
        async: options.async,
        dataType: "json",
        success: function (data) {
            if (options.showLoading === true) {
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
                    console.log(data);
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
            console.log(status);
            console.log(error);
            var msg = xhr.responseText;
            console.log(xhr)
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
    if (urlParameters.indexOf('?') !== -1) {
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
    if (value === true) {
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
                        if (type === 'boot') {
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
        dgelement = '#dginfo';
    }
    var Id = 0;
    var $grid = $(dgelement);
    var row;
    //如果是jqGrid的列表，使用jqGrid的方法获取主键值
    if (type === 'jq') {
        row = $grid.jqGridRowValue();
        //当得到的行记录不是数组时，则为对象
        if (!(row instanceof Array)) {
            Id = row.Id;
        }
    } else {
        if ($grid.bootstrapTable('getSelections')[0] !== undefined) {
            row = $(dgelement).bootstrapTable('getSelections')[0];
            Id = row.Id;
        }
    }
    return Id;
};
//编辑前统一提示信息,当Id为0时，说明未选中任何记录，其他时，将Id,作为回调函数的参数
com.edit = function (dgelement, type, callback) {
    var Id = com.get_selectid(dgelement, type);
    if (!Id) {
        layer.msg('在操作之前，请先选中一条记录！');
    } else {
        callback(Id);
    }
};



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
};
com.asyncGet = function (t) {
    var i = null;
    t = $.extend({
        type: "GET",
        dataType: "json",
        async: !1,
        cache: !1,
        success: function (n) {
            i = n;
        }
    }, t);
    return $.ajax(t), i;
};

com.tabiframe_Name = function () {
    return top.$(".J_iframe:visible").attr("name");
};
/*得到当前可见的iframe对象*/
com.currentIframe = function () {
    return top.frames[com.tabiframe_Name()].contentWindow !== undefined ? top.frames[com.tabiframe_Name()].contentWindow : top.frames[com.tabiframe_Name()];
};
com.checkedRow = function (n) {
    var i = !0;
    if (n === undefined || n === "" || n === "null" || n === "undefined" || n instanceof Array) {
        $.layerMsg("请先选中一条记录后再操作！"); return false;
    } else {
        return true;
    }
}

    ; (function ($, com) {
        $.extend(com, {
            get_layui_iframe_name: function (element) {
                if (element === null || element === undefined || element === "") {
                    return top.$('#Form').children('iframe')[0].name;
                } else {
                    return top.$(element).children('iframe')[0].name;
                }
            }, select_icon: function (controlId, formId) {
                $.layerOpen({
                    id: "select_icon",
                    title: '选取图标',
                    content: '/Plat/AppModule/Icon?controlId=' + controlId + '&formId=' + formId,
                    width: "1000px",
                    height: "600px",
                    btn: []
                });
            },
            get_randNum: function (minNum, maxNum) {
                switch (arguments.length) {
                    case 1:
                        return parseInt(Math.random() * minNum + 1, 10);
                        break;
                    case 2:
                        return parseInt(Math.random() * (maxNum - minNum + 1) + minNum, 10);
                        break;
                    default:
                        return 0;
                        break;
                }
            },
            parseJson: function (myJSONString) {
                if (myJSONString && myJSONString.length > 0) {
                    myJSONString = myJSONString.replace(/\n/g, "</br>").replace(/\r/g, "\\\\r");
                    return JSON.parse(myJSONString);
                } else {
                    return "";
                }
            },
            replaceSpce: function (myJSONString) {
                if (myJSONString && myJSONString.length > 0) {
                    myJSONString = myJSONString.replace(/\n/g, "").replace(/\r/g, "");
                    return JSON.parse(myJSONString);
                } else {
                    return "";
                }
            },
            isEmail: function (value) {
                if (value == "" || value == undefined) return false;
                //对电子邮箱的验证
                var myreg = /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;
                if (!myreg.test(value)) {
                    return false;
                } 
                return true;
            }
        });

    })(window.jQuery, window.com);

com.faces = function () {
    return { "[微笑]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/5c/huanglianwx_thumb.gif", "[嘻嘻]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/0b/tootha_thumb.gif", "[哈哈]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/6a/laugh.gif", "[可爱]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/14/tza_thumb.gif", "[可怜]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/af/kl_thumb.gif", "[挖鼻]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/0b/wabi_thumb.gif", "[吃惊]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/f4/cj_thumb.gif", "[害羞]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/6e/shamea_thumb.gif", "[挤眼]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/c3/zy_thumb.gif", "[闭嘴]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/29/bz_thumb.gif", "[鄙视]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/71/bs2_thumb.gif", "[爱你]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/6d/lovea_thumb.gif", "[泪]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/9d/sada_thumb.gif", "[偷笑]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/19/heia_thumb.gif", "[亲亲]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/8f/qq_thumb.gif", "[生病]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/b6/sb_thumb.gif", "[太开心]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/58/mb_thumb.gif", "[白眼]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/d9/landeln_thumb.gif", "[右哼哼]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/98/yhh_thumb.gif", "[左哼哼]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/6d/zhh_thumb.gif", "[嘘]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/a6/x_thumb.gif", "[衰]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/af/cry.gif", "[委屈]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/73/wq_thumb.gif", "[吐]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/9e/t_thumb.gif", "[哈欠]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/cc/haqianv2_thumb.gif", "[抱抱]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/27/bba_thumb.gif", "[怒]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/7c/angrya_thumb.gif", "[疑问]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/5c/yw_thumb.gif", "[馋嘴]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/a5/cza_thumb.gif", "[拜拜]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/70/88_thumb.gif", "[思考]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/e9/sk_thumb.gif", "[汗]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/24/sweata_thumb.gif", "[困]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/40/kunv2_thumb.gif", "[睡]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/96/huangliansj_thumb.gif", "[钱]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/90/money_thumb.gif", "[失望]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/0c/sw_thumb.gif", "[酷]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/40/cool_thumb.gif", "[色]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/20/huanglianse_thumb.gif", "[哼]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/49/hatea_thumb.gif", "[鼓掌]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/36/gza_thumb.gif", "[晕]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/d9/dizzya_thumb.gif", "[悲伤]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/1a/bs_thumb.gif", "[抓狂]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/62/crazya_thumb.gif", "[黑线]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/91/h_thumb.gif", "[阴险]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/6d/yx_thumb.gif", "[怒骂]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/60/numav2_thumb.gif", "[互粉]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/89/hufen_thumb.gif", "[心]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/40/hearta_thumb.gif", "[伤心]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/ea/unheart.gif", "[猪头]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/58/pig.gif", "[熊猫]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/6e/panda_thumb.gif", "[兔子]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/81/rabbit_thumb.gif", "[ok]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/d6/ok_thumb.gif", "[耶]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/d9/ye_thumb.gif", "[good]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/d8/good_thumb.gif", "[NO]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/ae/buyao_org.gif", "[赞]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/d0/z2_thumb.gif", "[来]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/40/come_thumb.gif", "[弱]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/d8/sad_thumb.gif", "[草泥马]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/7a/shenshou_thumb.gif", "[神马]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/60/horse2_thumb.gif", "[囧]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/15/j_thumb.gif", "[浮云]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/bc/fuyun_thumb.gif", "[给力]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/1e/geiliv2_thumb.gif", "[围观]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/f2/wg_thumb.gif", "[威武]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/70/vw_thumb.gif", "[奥特曼]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/bc/otm_thumb.gif", "[礼物]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/c4/liwu_thumb.gif", "[钟]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/d3/clock_thumb.gif", "[话筒]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/9f/huatongv2_thumb.gif", "[蜡烛]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/d9/lazhuv2_thumb.gif", "[蛋糕]": "http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/3a/cakev2_thumb.gif" };
    }

//简易编辑器
com.layEditor = function (options) {
    var html = ['<div class="layui-unselect fly-edit">'
        , '<span type="face" title="插入表情"><i class="iconfont icon-yxj-expression" style="top: 1px;"></i></span>'
        //, '<span type="picture" title="插入图片：img[src]"><i class="iconfont icon-tupian"></i></span>'
        , '<span type="href" title="超链接格式：a(href)[text]"><i class="iconfont icon-lianjie"></i></span>'
        , '<span type="code" title="插入代码或引用"><i class="iconfont icon-emwdaima" style="top: 1px;"></i></span>'
        , '<span type="hr" title="插入水平线">hr</span>'
        , '<span type="yulan" title="预览"><i class="iconfont icon-yulan1"></i></span>'
        , '</div>'].join('');

    var log = {}, mod = {
        face: function (editor, self) { //插入表情
            var str = '', ul, face = com.faces();
            for (var key in face) {
                str += '<li title="' + key + '"><img src="' + face[key] + '"></li>';
            }
            str = '<ul id="LAY-editface" class="layui-clear">' + str + '</ul>';
            layer.tips(str, self, {
                tips: 3
                , time: 0
                , skin: 'layui-edit-face'
            });
            $(document).on('click', function () {
                layer.closeAll('tips');
            });
            $('#LAY-editface li').on('click', function () {
                var title = $(this).attr('title') + ' ';
                com.focusInsert(editor[0], 'face' + title);
            });
        }
        , picture: function (editor) { //插入图片
            layer.open({
                type: 1
                , id: 'fly-jie-upload'
                , title: '插入图片'
                , area: 'auto'
                , shade: false
                , area: '465px'
                , fixed: false
                , offset: [
                    editor.offset().top - $(window).scrollTop() + 'px'
                    , editor.offset().left + 'px'
                ]
                , skin: 'layui-layer-border'
                , content: ['<ul class="layui-form layui-form-pane" style="margin: 20px;">'
                    , '<li class="layui-form-item">'
                    , '<label class="layui-form-label">URL</label>'
                    , '<div class="layui-input-inline">'
                    , '<input required name="image" placeholder="支持直接粘贴远程图片地址" value="" class="layui-input">'
                    , '</div>'
                    , '<button type="button" class="layui-btn layui-btn-primary" id="uploadImg"><i class="layui-icon">&#xe67c;</i>上传图片</button>'
                    , '</li>'
                    , '<li class="layui-form-item" style="text-align: center;">'
                    , '<button type="button" lay-submit lay-filter="uploadImages" class="layui-btn">确认</button>'
                    , '</li>'
                    , '</ul>'].join('')
                , success: function (layero, index) {
                    var image = layero.find('input[name="image"]');

                    //执行上传实例
                    layui.upload.render({
                        elem: '#uploadImg'
                        , url: '/Mock/Upload/LayuiImage'
                        , size: 200
                        , done: function (res) {
                            if (res.code == 0) {
                                image.val(res.data.src);
                            } else {
                                layer.msg(res.msg, { icon: 5 });
                            }
                        }
                    });

                    layui.form.on('submit(uploadImages)', function (data) {
                        var field = data.field;
                        if (!field.image) return image.focus();
                        com.focusInsert(editor[0], 'img[' + field.image + '] ');
                        layer.close(index);
                    });
                }
            });
        }
        , href: function (editor) { //超链接
            layer.prompt({
                title: '请输入合法链接'
                , shade: false
                , fixed: false
                , id: 'LAY_flyedit_href'
                , offset: [
                    editor.offset().top - $(window).scrollTop() + 'px'
                    , editor.offset().left + 'px'
                ]
            }, function (val, index, elem) {
                if (!/^http(s*):\/\/[\S]/.test(val)) {
                    layer.tips('这根本不是个链接，不要骗我。', elem, { tips: 1 })
                    return;
                }
                com.focusInsert(editor[0], ' a(' + val + ')[' + val + '] ');
                layer.close(index);
            });
        }
        , code: function (editor) { //插入代码
            layer.prompt({
                title: '请贴入代码或任意文本'
                , formType: 2
                , maxlength: 10000
                , shade: false
                , id: 'LAY_flyedit_code'
                , area: ['800px', '360px']
            }, function (val, index, elem) {
                com.focusInsert(editor[0], '[pre]\n' + val + '\n[/pre]');
                layer.close(index);
            });
        }
        , hr: function (editor) { //插入水平分割线
            com.focusInsert(editor[0], '[hr]');
        }
        , yulan: function (editor) { //预览
            var content = editor.val();

            content = /^\{html\}/.test(content)
                ? content.replace(/^\{html\}/, '')
                : com.content(content);

            layer.open({
                type: 1
                , title: '预览'
                , shade: false
                , area: ['100%', '100%']
                , scrollbar: false
                , content: '<div class="detail-body" style="margin:20px;">' + content + '</div>'
            });
        }
    };

    options = options || {};

    $(options.elem).each(function (index) {
        var that = this, othis = $(that), parent = othis.parent();
        parent.prepend(html);
        parent.find('.fly-edit span').on('click', function (event) {
            var type = $(this).attr('type');
            mod[type].call(that, othis, this);
            if (type === 'face') {
                event.stopPropagation()
            }
        });
    });
}

//内容转义
com.content = function (content) {
    //支持的html标签
    var html = function (end) {
        return new RegExp('\\n*\\[' + (end || '') + '(pre|hr|div|span|p|table|thead|th|tbody|tr|td|ul|li|ol|li|dl|dt|dd|h2|h3|h4|h5)([\\s\\S]*?)\\]\\n*', 'g');
    };
    content = com.escape(content || '') //XSS
        .replace(/img\[([^\s]+?)\]/g, function (img) {  //转义图片
            return '<img src="' + img.replace(/(^img\[)|(\]$)/g, '') + '">';
        }).replace(/@(\S+)(\s+?|$)/g, '@<a href="javascript:;" class="fly-aite">$1</a>$2') //转义@
        .replace(/face\[([^\s\[\]]+?)\]/g, function (face) {  //转义表情
            var alt = face.replace(/^face/g, '');
            return '<img alt="' + alt + '" title="' + alt + '" src="' + com.faces()[alt] + '">';
        }).replace(/a\([\s\S]+?\)\[[\s\S]*?\]/g, function (str) { //转义链接
            var href = (str.match(/a\(([\s\S]+?)\)\[/) || [])[1];
            var text = (str.match(/\)\[([\s\S]*?)\]/) || [])[1];
            if (!href) return str;
            var rel = /^(http(s)*:\/\/)\b(?!(\w+\.)*(sentsin.com|layui.com))\b/.test(href.replace(/\s/g, ''));
            return '<a href="' + href + '" target="_blank"' + (rel ? ' rel="nofollow"' : '') + '>' + (text || href) + '</a>';
        }).replace(html(), '\<$1 $2\>').replace(html('/'), '\</$1\>') //转移HTML代码
        .replace(/\n/g, '<br>') //转义换行   
    return content;
}
com.escape = function (html) {
    return String(html || '').replace(/&(?!#?[a-zA-Z0-9]+;)/g, '&amp;')
        .replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/'/g, '&#39;').replace(/"/g, '&quot;');
}



com.focusInsert = function (obj, str) {
    var result, val = obj.value;
    obj.focus();
    if (document.selection) { //ie
        result = document.selection.createRange();
        document.selection.empty();
        result.text = str;
    } else {
        result = [val.substring(0, obj.selectionStart), str, val.substr(obj.selectionEnd)];
        obj.focus();
        obj.value = result.join('');
    }
};

com.decodeText=function (d) {
    var content = /^\{html\}/.test(d)
        ? d.replace(/^\{html\}/, '')
        : com.content(d);
    return content;
}

