# Mock.luo
* 这是我的示例代码库。
* 开发工具:VS2017
* 前端框架：H+、vue、layUI2.1.1
* 后端：EF6、MVC5
* 数据库:SQL sever 2008 
* 后端使用Autofac做依赖注入,Mapper做实体ViewModel映射,
## Mock.Luo项目进展
* 用户管理：列表页：bootstrapTable，表单页：bootstrap页面布局
* 角色管理：列表页:bootstrapTable，表单页：layui 的表单
* 系统功能：列表页:FancyTree写的treeGrid项，表单：layui的表单，一个顺序关联的填写表单使用的wizcard，从力软哪里搞来的，jqGrid写的treegrid,可以配置一个菜单下的按钮及权限，权限可作为按钮的子节点，设置为隐藏形式。
* 字典管理：左树使用:z-tree,列表使用bootstrapTable，表单使用：layui的表单，分类管理列表使用jqGrid的treeGrid，这个树表格看起来更简约，表单依旧使用layui的表单
* 文章管理：列表依旧使用：bootstrapTable，表单使用:bootstrap页面布局，这个以前写的MoBlog，稍微调了一下，手机兼容性一般，毕竟有一个编辑器。
* 测试管理：目前仅有下拉树
* 评论管理:列表页bootstrapTable，功能点：删除，设置状态（已审核与拉黑）
* 用户登录，注册待完成，密码找回待完成，登录后，个人权限的获取未完成。
#### 功能点介绍
* 在弹出弹出框的时候，检测用户使用的非pc端，宽高就100%
* 目前只做了用户配角色。角色选择用户。角色配置菜单及按钮权限功能。菜单下配置按钮及权限。

#### 以下是控件的使用介绍
* vue+layui+bootstrap+ （H+的整体布局）
* vue:这个渐进式框架已经不能用棒来形容了。
* layui。使用的最新版2.1.1。layer这个大名鼎鼎的弹出层，相信大家一定会喜欢的。
* bootstrap。初次使用，基本靠抄与看文档。
* H+ :这个，一开始从某些群里下载的，然后把左边树和上面的tabs的功能拿过来了，而且也是自适应的，以前就觉得的它的样式很好看，就慢慢地抄过来了。不知道会不会侵权啊，我也就做一个自己的博客系统。嘿嘿。
* 下拉树是从力软官网demo的部门管理抄过来的，使用的控件是wdTree，封装一个方法，将树形控件变成了comboboxTree.
* 下拉列表:vue-multiselect，这个在github上一搜就能搜到，算了还是给个链接吧:[vue-multiselect](https://github.com/monterail/vue-multiselect)，这个控件支持单选，多选（标签形式）很棒，文档也很齐全,相信大家不会被全英文吓到。
*  FancyTree [github链接](https://github.com/mar10/fancytree) 这个项目中使用了bootstrap的TreeGrid，文档完全看不懂，我看了两天，终究还是败给自己的英语阅读能力，看这个文档也不错，主要是官网文档乱，实在看不懂就 [点这个](http://www.lxway.com/95495251.htm)
*  zTree，这个控件也非常有名，只不过没下拉树实现，自己写的也是非常丑，之前拿select2模拟comboboxTree,现在不用了。zTree就拿来作树形结构分类而已。
*  文章管理中的编辑器就是百度的ueditor，很重量级，我也只会用他的几个函数，其他的就用不到了
*  jgGrid这个在 字典分类中的列表中使用了treeGrid功能。看起来非常地简约。[demo链接](http://www.guriddo.net/demo/guriddojs/)，反正我不是从网站下载的，我是从NFine这个.NET开源框架拿来的，然后，代码也是抄的NFINE,
*  bootstrapTable  大多数表格使用的是这个。[文档链接](http://bootstrap-table.wenzhixin.net.cn/)
*  登录界面的记住密码使用的iCheck控件，选中，记住密码。更漂亮.有多种皮肤 [github icheck链接](https://github.com/fronteed/icheck)
* 关于表单验证，也是从力软上的拿来的validator,没有仔细看他们的代码，不过知道怎么用，并改进其中的方法，使用layer的弹出tip层来显示出错信息，我觉得这个验证应该是他们自己扩展的。
* 关于上传控件，我们用的是uploadify自己改的，之前在学校项目的时候，我的学长借鉴163邮箱封装uploadify，然后，我觉得不太好用，自己也看了源码，照着163邮箱的上传附件功能，自己再次重写了一下uploadify控件，当然是指扩展。