(function ($) {
    "use strict";

    $.fn.treegridData = function (options, param) {
        //如果是调用方法
        if (typeof options == 'string') {
            return $.fn.treegridData.methods[options](this, param);
        }

        //如果是初始化组件
        options = $.extend({}, $.fn.treegridData.defaults, options || {});
        var target = $(this);
        //得到根节点
        target.getRootNodes = function (data) {
            var result = [];
            $.each(data, function (index, item) {
                if (!item[options.parentColumn]) {
                    result.push(item);
                }
            });
            return result;
        };
        var j = 0;
        //递归获取子节点并且设置子节点
        target.getChildNodes = function (data, parentNode, parentIndex, tbody) {
            $.each(data, function (i, item) {
                if (item[options.parentColumn] == parentNode[options.id]) {
                    var tr = $('<tr></tr>');
                    var nowParentIndex = (parentIndex + (j++) + 1);
                    tr.addClass('treegrid-' + nowParentIndex);
                    tr.addClass('treegrid-parent-' + parentIndex);
                    $.each(options.columns, function (index, column) {
                        var td = $('<td></td>');
                        if (options.columns[index].align) {
                            td.css('text-align', options.columns[index].align);
                        }
                        if (typeof (options.columns[index].formatter) == 'function') {
                            var text = options.columns[index].formatter(item[column.field], item, i);
                            td.html(text);
                        } else {
                            td.text(item[column.field]);
                        }
                        if (!options.columns[index].hidden) {
                            tr.append(td);
                        }
                    });

                    tbody.append(tr);
                    target.getChildNodes(data, item, nowParentIndex, tbody)

                }
            });
        };
        target.addClass('table');
        //表格样式
        if (options.striped) {
            target.addClass('table-striped');
        }
        if (options.bordered) {
            target.addClass('table-bordered');
        }
        if (options.hover) {
            target.addClass('table-hover');
        }
        if (options.url) {
            $.ajax({
                type: options.type,
                url: options.url,
                data: options.queryParams,
                dataType: "JSON",
                success: function (data, textStatus, jqXHR) {
                    var rows = data.rows;
                    //构造表头
                    var thr = $('<tr></tr>');
                    $.each(options.columns, function (i, item) {
                        var th = $('<th style="padding:10px;"></th>');
                        th.text(item.title);
                        if (options.columns[i].align) {
                            th.css('text-align', options.columns[i].align);
                        }
                        if (typeof(options.columns[i].css)=='object') {
                            th.css(options.columns[i].css)
                        }
                        if (!options.columns[i].hidden) {
                            thr.append(th);
                        }
                    });
                    var thead = $('<thead></thead>');
                    thead.append(thr);
                    target.append(thead);

                    //构造表体
                    var tbody = $('<tbody></tbody>');
                    var rootNode = target.getRootNodes(rows);
                    $.each(rootNode, function (i, item) {
                        var tr = $('<tr></tr>');
                        tr.addClass('treegrid-' + (j + i));
                        $.each(options.columns, function (index, column) {
                            var td = $('<td></td>');
                            if (options.columns[index].align) {
                                td.css('text-align', options.columns[index].align);
                            }
                            if (typeof (options.columns[index].formatter) == 'function') {
                                var text = options.columns[index].formatter(item[column.field], item, i);
                                td.html(text);
                            } else {
                                td.text(item[column.field]);
                            }
                            if (!options.columns[index].hidden) {
                                tr.append(td);
                            }
                        });
                        tbody.append(tr);
                        target.getChildNodes(rows, item, (j + i), tbody);
                    });
                    target.append(tbody);
                    target.treegrid({
                        expanderExpandedClass: options.expanderExpandedClass,
                        expanderCollapsedClass: options.expanderCollapsedClass
                    });
                    target.find('tbody tr').on('click', function () {
                        $(this).addClass("tree-item-selected-light").siblings().removeClass("tree-item-selected-light");
                    })
                    if (!options.expandAll) {
                        target.treegrid('collapseAll');
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    layer.msg(textStatus)
                    console.error(XMLHttpRequest.status);
                    console.error(XMLHttpRequest.readyState);
                    console.error(textStatus);
                },
            });
        }
        else {
            //也可以通过defaults里面的data属性通过传递一个数据集合进来对组件进行初始化....有兴趣可以自己实现，思路和上述类似
        }
        return target;
    };

    $.fn.treegridData.methods = {
        getAllNodes: function (target, data) {
            return target.treegrid('getAllNodes');
        },
        //组件的其他方法也可以进行类似封装........
        getSelected: function (target) {

        }
    };

    $.fn.treegridData.defaults = {
        id: 'Id',
        parentColumn: 'PId',
        data: [],    //构造table的数据集合
        type: "GET", //请求数据的ajax类型
        url: null,   //请求数据的ajax的url
        queryParams: {}, //请求数据的ajax的data属性
        expandColumn: null,//在哪一列上面显示展开按钮
        expandAll: true,  //是否全部展开
        striped: false,   //是否各行渐变色
        bordered: false,  //是否显示边框
        hover: false,
        columns: [],
        expanderExpandedClass: 'glyphicon glyphicon-chevron-down',//展开的按钮的图标
        expanderCollapsedClass: 'glyphicon glyphicon-chevron-right',//缩起的按钮的图标

    };
})(jQuery);