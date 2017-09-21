
window.$ui = {};
$(function () {
    $ui.initIndex();
    //com.ajax({
    //    url: '/Plat/AppModule/GetUserModule',
    //    async:false,
    //    success: function (data) {
    //        $ui.initMenu(data);
    //    }
    //});
});
(function ($, $ui) {
    $.extend($ui, {
        initMenu: function (menulist) {
            //var menulist=$ui.get_static_menu();
            var $sidemenu = $('#side-menu');
            var html = '';
            for (var i = 0; i < menulist.length; i++) {
                var template = '<li>\
                <a href="#">\
                    <i class="{0}"></i>\
                    <span class="nav-label">{1}</span>\
                    <span class="fa arrow"></span>\
                </a>';

                var jsonobj = menulist[i];
                html += com.format_str(template, jsonobj.iconcls, jsonobj.text);
                html += '<ul class="nav nav-second-level collapse">';

                var temple = '<li>\
                          <a class="J_menuItem" href="{0}" target="{1}"><i class="{2}"></i>{3}\
                      ';
                if (jsonobj.children.length > 0) {

                    for (var j = 0; j < jsonobj.children.length; j++) {
                        var m = jsonobj.children[j];
                        html += com.format_str(temple, m.href, m.target, m.iconcls, m.text);
                        var json3 = m;
                        if (m && m.children && m.children.length > 0) {
                            html += '<span class="fa arrow"></span></a>';
                            html += '<ul class="nav nav-third-level">';
                            var template3 = '<li>\
                                           <a class="J_menuItem" href="{0}"  target="{1}"><i class="{2}"></i>{3}</a>\
                                     </li>';
                            for (var k = 0; k < m.children.length; k++) {
                                var t = m.children[k];
                                html += com.format_str(template3, t.href, t.target, t.iconcls, t.text);
                            }
                            html += '</ul>';
                        } else {
                            html += '</a>';
                        }
                    }
                }
                html += ' </li>';
                html += '</ul></li>';
            }
            $sidemenu.append(html);
        },
        data: {},
        initIndex: function () {
            com.ajax({
                async: false,
                url: '/Home/GetClientsJson',
                success: function (data) {
                    $ui.data = data;
                    $ui.initMenu(data.authorizeMenu);
                }
            });
        }
    });
})(jQuery, window.$ui);



//三级菜单

/*
 <li>
    <a href="#"><i class="fa fa-edit"></i> <span class="nav-label">表单</span><span class="fa arrow"></span></a>
                <ul class="nav nav-second-level">
                    <li>
                        <a href="#">文件上传 <span class="fa arrow"></span></a>
                        <ul class="nav nav-third-level">
                            <li><a class="J_menuItem" href="form_webuploader.html">百度WebUploader</a>
                            </li>
                            <li><a class="J_menuItem" href="form_file_upload.html">DropzoneJS</a>
                            </li>
                            <li><a class="J_menuItem" href="form_avatar.html">头像裁剪上传</a>
                            </li>
                        </ul>
                    </li>
                    <li><a class="J_menuItem" href="suggest.html">搜索自动补全</a>
                    </li>
                    <li><a class="J_menuItem" href="layerdate.html">日期选择器layerDate</a>
                    </li>
                </ul>
            </li>
*/


$ui.get_static_menu = function () {
    var menulist = [{
        'text': '主页',
        'id': 1,
        'PId': 0,
        'iconCls': 'fa fa-home',
        'href': '',
        'sortcode': 1,
        'target': '_self',
        'children': [{
            'text': '上传文件',
            'id': 2,
            'PId': 1,
            'iconCls': 'fa fa-upload',
            'href': '/Mock/UploadFile/UploadView',
            'sortcode': 2,
            'target': '_self'
        }, {
            'text': 'FontAwesome',
            'id': 5,
            'PId': 1,
            'iconCls': 'fa fa-gift',
            'href': 'http://dnt.dkill.net/dnt/font/',
            'sortcode': 3,
            'target': '_self'
        }]
    }, {
        'text': 'EasyUI示例',
        'id': 4,
        'PId': 0,
        'iconCls': 'fa fa-amazon',
        'sortcode': 4,
        'target': '_self',
        'children': [{
            'text': 'DatalistView',
            'id': 3,
            'PId': 1,
            'iconCls': 'fa fa-tablet',
            'href': '/Home/DatalistView',
            'sortcode': 3,
            'target': '_self'
        }]
    }, {
        'text': '表单',
        'id': 6,
        'PId': 0,
        'iconCls': 'fa fa-list',
        'sortcode': 7,
        'target': '_self',
        'children': [{
            'text': '编辑器',
            'id': 3,
            'PId': 1,
            'iconCls': 'fa fa-edit',
            'sortcode': 8,
            'children': [{
                'text': 'MarkDown编辑器',
                'id': 7,
                'PId': 6,
                'iconCls': 'fa fa-camera-retro',
                'href': '/Mock/Editor/MarkDownView',
                'sortcode': 9
            }, {
                'text': 'Simditor编辑器',
                'id': 8,
                'PId': 6,
                'iconCls': 'fa fa-camera-retro',
                'href': '/Mock/Editor/SimditorView',
                'sortcode': 8
            }, {
                'text': '信箱',
                'id': 8,
                'PId': 6,
                'iconCls': 'fa fa-map',
                'href': '/Mock/Editor/EmailIndex?data=0',
                'sortcode': 9
            }]
        }]
    }, {
        'text': '工具',
        'id': 11,
        'PId': 0,
        'iconCls': 'fa fa-th-list',
        'sortcode': 7,
        'children': [{
            'text': 'css格式化',
            'id': 12,
            'PId': 11,
            'iconCls': 'fa fa-thumbs-o-up',
            'sortcode': 7,
            'href': '/Mock/Code/CssFormatIndex',
            'target': '_self'
        }]
    }
    ];
    return menulist;
};