
$(function () {
    var $sidemenu = $('#side-menu');
    var menulist = [{
        'MenuName': '主页',
        'Id': 1,
        'PId': 0,
        'icon': 'fa fa-home',
        'href': '',
        'SortCode': 1,
        'target': '_self',
        'children': [{
            'MenuName': '上传文件',
            'Id': 2,
            'PId': 1,
            'icon': 'fa fa-upload',
            'href': '/Mock/UploadFile/UploadView',
            'SortCode': 2,
            'target': '_self'
        }, {
            'MenuName': 'FontAwesome',
            'Id': 5,
            'PId': 1,
            'icon': 'fa fa-gift',
            'href': 'http://dnt.dkill.net/dnt/font/',
            'SortCode': 3,
            'target': '_self'
        }]
    }, {
        'MenuName': 'EasyUI示例',
        'Id': 4,
        'PId': 0,
        'icon': 'fa fa-amazon',
        'SortCode': 4,
        'target': '_self',
        'children': [{
            'MenuName': 'DatalistView',
            'Id': 3,
            'PId': 1,
            'icon': 'fa fa-tablet',
            'href': '/Home/DatalistView',
            'SortCode': 3,
            'target': '_self',
        }]
    }, {
        'MenuName': '表单',
        'Id': 6,
        'PId': 0,
        'icon': 'fa fa-list',
        'SortCode': 7,
        'target': '_self',
        'children': [{
            'MenuName': '编辑器',
            'Id': 3,
            'PId': 1,
            'icon': 'fa fa-edit',
            'SortCode': 8,
            'children': [{
                'MenuName': 'MarkDown编辑器',
                'Id': 7,
                'PId': 6,
                'icon': 'fa fa-camera-retro',
                'href': '/Mock/Editor/MarkDownView',
                'SortCode': 9
            }, {
                'MenuName': 'Simditor编辑器',
                'Id': 8,
                'PId': 6,
                'icon': 'fa fa-camera-retro',
                'href': '/Mock/Editor/SimditorView',
                'SortCode': 8
            }, {
                'MenuName': '信箱',
                'Id': 8,
                'PId': 6,
                'icon': 'fa fa-map',
                'href': '/Mock/Editor/EmailIndex?data=0',
                'SortCode': 9
            }]
        }]
    }, {
        'MenuName': '工具',
        'Id': 11,
        'PId': 0,
        'icon': 'fa fa-th-list',
        'SortCode': 7,
        'children': [{
            'MenuName': 'css格式化',
            'Id': 12,
            'PId': 11,
            'icon': 'fa fa-thumbs-o-up',
            'SortCode': 7,
            'href': '/Mock/Code/CssFormatIndex',
            'target': '_self',
        }]
    }
    ];
    var html = '';
    for (var i = 0; i < menulist.length; i++) {
        var template = '<li>\
                <a href="#">\
                    <i class="{0}"></i>\
                    <span class="nav-label">{1}</span>\
                    <span class="fa arrow"></span>\
                </a>'

        var jsonobj = menulist[i];
        html += com.formatString(template, jsonobj.icon, jsonobj.MenuName);
        html += '<ul class="nav nav-second-level collapse">';

        var temple = '<li>\
                          <a class="J_menuItem" href="{0}" data-index="{1}" target="{2}"><i class="{3}"></i>{4}\
                      ';
        if (jsonobj.children.length > 0) {

            for (var j = 0; j < jsonobj.children.length; j++) {
                var m = jsonobj.children[j];
                if (m.target != '_blank') {
                    m.target = '_self';
                }
                html += com.formatString(temple, m.href, m.SortCode, m.target, m.icon, m.MenuName);
                var json3 = m;
                if (m && m.children && m.children.length > 0) {
                    html += '<span class="fa arrow"></span></a>';
                    html += '<ul class="nav nav-third-level">'
                    var template3 = '<li>\
                                           <a class="J_menuItem" href="{0}" data-index="{1}" target="{2}"><i class="{3}"></i>{4}</a>\
                                     </li>'
                    for (var k = 0; k < m.children.length; k++) {
                        var t = m.children[k]
                        html += com.formatString(template3, t.href, t.SortCode, t.target, t.icon, t.MenuName);
                    }
                    html += '</ul>';
                } else {
                    html += '</a>'
                }
            }
        }
        html += ' </li>';
        html += '</ul></li>';
    }
    $sidemenu.append(html);
});


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