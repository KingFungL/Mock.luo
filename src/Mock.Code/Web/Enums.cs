using System.ComponentModel;

namespace Mock.Code.Web
{
    public enum DbLogType
    {
        [Description("其他")]
        Other = 0,
        [Description("登录")]
        Login = 1,
        [Description("退出")]
        Exit = 2,
        [Description("访问")]
        Visit = 3,
        [Description("新增")]
        Create = 4,
        [Description("删除")]
        Delete = 5,
        [Description("修改")]
        Update = 6,
        [Description("提交")]
        Submit = 7,
        [Description("异常")]
        Exception = 8,
    }

    public enum StatusCode
    {
        /// <summary>
        /// 暂存
        /// </summary>
        [Description("暂存")]
        Tempsave = 0,
        /// <summary>
        /// 提交
        /// </summary>
        [Description("提交")]
        Submit = 1,
        /// <summary>
        /// 发布
        /// </summary>
        [Description("发布")]
        Release = 2,
        /// <summary>
        /// 已审核
        /// </summary>
        [Description("暂存")]
        Audited = 3,
        /// <summary>
        /// 未审核
        /// </summary>
        [Description("未审核")]
        Notaudited = 4,
        /// <summary>
        /// 拉黑
        /// </summary>
        [Description("拉黑")]
        Defriend = 5,
        /// <summary>
        /// 不可见，删除
        /// </summary>
        [Description("删除")]
        Deleted = 6,

        /// <summary>
        /// 启用
        /// </summary>
        Enable,
        /// <summary>
        /// 禁用
        /// </summary>
        Forbidden
    }


    public enum EnCode
    {
        /// <summary>
        /// 通用字典
        /// </summary>
        Common,
        /// <summary>
        /// 机构分类
        /// </summary>
        Dep,
        /// <summary>
        /// 目标分类
        /// </summary>
        Target,
        /// <summary>
        /// 用户配置
        /// </summary>
        Config,
        /// <summary>
        /// 文章类别
        /// </summary>
        FTypeCode,
        /// <summary>
        /// 标签
        /// </summary>
        Tag
    }

    public enum CategoryCode
    {
        //分类为随机文章
        Random,
        /// <summary>
        /// 学习
        /// </summary>
        Study,
        /// <summary>
        /// 轻松时刻
        /// </summary>
        Justfun,
        /// <summary>
        /// 人生感悟
        /// </summary>
        Feelinglife

    }
    /// <summary>
    /// 第三方或本地登录类型
    /// </summary>
    public enum IdentityType
    {
        /// <summary>
        /// QQ互联，
        /// </summary>
        Qq,
        Email,
        LoginName,
        Phone,
        WeChat,
        Sina

    }

    public enum ModuleCode
    {
        /// <summary>
        /// 菜单
        /// </summary>
        Menu,
        /// <summary>
        /// 父菜单
        /// </summary>
        PMenu,
        /// <summary>
        /// 按钮
        /// </summary>
        Button,
        /// <summary>
        /// 权限认证
        /// </summary>
        Permission,
    }


}
