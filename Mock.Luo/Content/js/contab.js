
$ui = {

    //初始化左边树形结点单击事件效果
    init_menuitem: function () {
        var o = $(this).attr("href"),
            m = $(this).data("index"),
            l = $.trim($(this)[0].innerHTML),
            target = $(this).attr('target'),
            k = true;
        if (o == undefined || $.trim(o).length == 0 || o == '#' || o == 'undefined') {
            return false
        }

        if (target == '_top') {
            window.open(o);
            return;
        }

        $(".J_menuTab").each(function () {
            if ($(this).data("id") == o) {
                if (!$(this).hasClass("active")) {
                    $(this).addClass("active").siblings(".J_menuTab").removeClass("active");
                    $ui.animate(this);
                    $(".J_mainContent .J_iframe").each(function () {
                        if ($(this).data("id") == o) {
                            $(this).show().siblings(".J_iframe").hide();
                            return false
                        }
                    })
                }
                k = false;
                return false
            }
        });
        if (k) {
            var p = '<a href="javascript:;" class="active J_menuTab" data-index="' + m + '" data-id="' + o + '">' + l + ' <i class="fa fa-times-circle"></i></a>';
            $(".J_menuTab").removeClass("active");
            var n = '<iframe class="J_iframe" name="iframe' + m + '" width="100%" height="100%" src="' + o + '" frameborder="0" data-id="' + o + '" seamless></iframe>';
            $(".J_mainContent").find("iframe.J_iframe").hide().parents(".J_mainContent").append(n);
            $(".J_menuTabs .page-tabs-content").append(p);
            $ui.animate($(".J_menuTab.active"))
        }
        return false
    },
    //点击关闭页签的图标
    close_tabs: function () {
        var $this = $(this).parents(".J_menuTab");
        var m = $this.data("id");
        var l = $this.width();
        //关闭当前active的页签
        if ($this.hasClass("active")) {
            $ui.close($this);
        } else {
            $this.remove();
            $(".J_mainContent .J_iframe").each(function () {
                if ($(this).data("id") == m) {
                    $(this).remove();
                    return false
                }
            });
            $ui.animate($(".J_menuTab.active"))
        }
        return false
    },
    //关闭其他页签
    tabsCloseOther: function () {
        $(".page-tabs-content").children("[data-id]").not(":first").not(".active").each(function () {
            $('.J_iframe[data-id="' + $(this).data("id") + '"]').remove();
            $(this).remove()
        });
        $(".page-tabs-content").css("margin-left", "0")
    },
    f: function (l) {
        var k = 0;
        $(l).each(function () {
            k += $(this).outerWidth(true)
        });
        return k
    },
    //切换tabs的动画效果
    animate: function (n) {
        var o = $ui.f($(n).prevAll()),
            q = $ui.f($(n).nextAll());
        var l = $ui.f($(".content-tabs").children().not(".J_menuTabs"));
        var k = $(".content-tabs").outerWidth(true) - l;
        var p = 0;
        if ($(".page-tabs-content").outerWidth() < k) {
            p = 0
        } else {
            if (q <= (k - $(n).outerWidth(true) - $(n).next().outerWidth(true))) {
                if ((k - $(n).next().outerWidth(true)) > q) {
                    p = o;
                    var m = n;
                    while ((p - $(m).outerWidth()) > ($(".page-tabs-content").outerWidth() - k)) {
                        p -= $(m).prev().outerWidth();
                        m = $(m).prev()
                    }
                }
            } else {
                if (o > (k - $(n).outerWidth(true) - $(n).prev().outerWidth(true))) {
                    p = o - $(n).prev().outerWidth(true)
                }
            }
        }
        $(".page-tabs-content").animate({
            marginLeft: 0 - p + "px"
        }, "fast")
    },
    sibling_active: function () {
        if (!$(this).hasClass("active")) {
            var k = $(this).data("id");
            $(".J_mainContent .J_iframe").each(function () {
                if ($(this).data("id") == k) {
                    $(this).show().siblings(".J_iframe").hide();
                    return false
                }
            });
            $(this).addClass("active").siblings(".J_menuTab").removeClass("active");
            $ui.animate(this)
        }
    },
    //关闭所有页签
    tabCloseAll: function () {
        $(".page-tabs-content").children("[data-id]").not(":first").each(function () {
            $('.J_iframe[data-id="' + $(this).data("id") + '"]').remove();
            $(this).remove()
        });
        $(".page-tabs-content").children("[data-id]:first").each(function () {
            $('.J_iframe[data-id="' + $(this).data("id") + '"]').show();
            $(this).addClass("active")
        });
        $(".page-tabs-content").css("margin-left", "0")
    },
    tabLeft: function () {
        var o = Math.abs(parseInt($(".page-tabs-content").css("margin-left")));
        var l = $ui.f($(".content-tabs").children().not(".J_menuTabs"));
        var k = $(".content-tabs").outerWidth(true) - l;
        var p = 0;
        if ($(".page-tabs-content").width() < k) {
            return false
        } else {
            var m = $(".J_menuTab:first");
            var n = 0;
            while ((n + $(m).outerWidth(true)) <= o) {
                n += $(m).outerWidth(true);
                m = $(m).next()
            }
            n = 0;
            if ($ui.f($(m).prevAll()) > k) {
                while ((n + $(m).outerWidth(true)) < (k) && m.length > 0) {
                    n += $(m).outerWidth(true);
                    m = $(m).prev()
                }
                p = $ui.f($(m).prevAll())
            }
        }
        $(".page-tabs-content").animate({
            marginLeft: 0 - p + "px"
        }, "fast")
    },
    tabRight: function () {
        var o = Math.abs(parseInt($(".page-tabs-content").css("margin-left")));
        var l = $ui.f($(".content-tabs").children().not(".J_menuTabs"));
        var k = $(".content-tabs").outerWidth(true) - l;
        var p = 0;
        if ($(".page-tabs-content").width() < k) {
            return false
        } else {
            var m = $(".J_menuTab:first");
            var n = 0;
            while ((n + $(m).outerWidth(true)) <= o) {
                n += $(m).outerWidth(true);
                m = $(m).next()
            }
            n = 0;
            while ((n + $(m).outerWidth(true)) < (k) && m.length > 0) {
                n += $(m).outerWidth(true);
                m = $(m).next()
            }
            p = $ui.f($(m).prevAll());
            if (p > 0) {
                $(".page-tabs-content").animate({
                    marginLeft: 0 - p + "px"
                }, "fast")
            }
        }
    },
    //刷新当前选中的tabs页签
    tabRefresh: function () {
        var index = $(".page-tabs-content").children(".active").attr('data-index');
        document["iframe" + index].location.reload()
    },
    //关闭当前选中的页签
    tabCloseSelect: function () {
        var $this = $(".page-tabs-content").children(".active");
        $ui.close($this);
    },
    //关闭当前选的页签
    close: function ($this) {
        var m = $this.data("id");
        if ($this.next(".J_menuTab").size()) {
            var k = $this.next(".J_menuTab:eq(0)").data("id");
            $this.next(".J_menuTab:eq(0)").addClass("active");
            $(".J_mainContent .J_iframe").each(function () {
                if ($(this).data("id") == k) {
                    $(this).show().siblings(".J_iframe").hide();
                    return false
                }
            });
            var n = parseInt($(".page-tabs-content").css("margin-left"));
            if (n < 0) {
                $(".page-tabs-content").animate({
                    marginLeft: (n + l) + "px"
                }, "fast")
            }
            $this.remove();
            $(".J_mainContent .J_iframe").each(function () {
                if ($(this).data("id") == m) {
                    $(this).remove();
                    return false
                }
            })
        }
        if ($this.prev(".J_menuTab").size()) {
            var k = $this.prev(".J_menuTab:last").data("id");
            $this.prev(".J_menuTab:last").addClass("active");
            $(".J_mainContent .J_iframe").each(function () {
                if ($(this).data("id") == k) {
                    $(this).show().siblings(".J_iframe").hide();
                    return false
                }
            });
            $this.remove();
            $(".J_mainContent .J_iframe").each(function () {
                if ($(this).data("id") == m) {
                    $(this).remove();
                    return false
                }
            })
        }
    }
}

$(function () {

    $(".J_menuItem").each(function (k) {
        if (!$(this).attr("data-index")) {
            $(this).attr("data-index", k + 1)
        }
    });
    //初始化左边树菜单
    $(".J_menuItem").on("click", $ui.init_menuitem);
    //点击关闭的图标时，关闭当前选中面签
    $(".J_menuTabs").on("click", ".J_menuTab i.fa-times-circle", $ui.close_tabs);
    //关闭非首页与当前active的其他页签
    $(".J_tabCloseOther").on("click", $ui.tabsCloseOther);
    //关闭所有页签，除首页
    $(".J_tabCloseAll").on("click", $ui.tabCloseAll);
    //刷新当前tabs
    $('.J_tabRefresh').on('click', $ui.tabRefresh);
    
    $(".J_menuTabs").on("click", ".J_menuTab", $ui.sibling_active);
    //关闭当前active的页签
    $('.J_tabCloseSelect').on('click', $ui.tabCloseSelect);
    //应该是切换到左边的tabs
    $(".J_tabLeft").on("click", $ui.tabLeft);

    $(".J_tabRight").on("click", $ui.tabRight);
})
