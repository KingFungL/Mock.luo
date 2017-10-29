using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data
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
        tempsave = 0,
        /// <summary>
        /// 提交
        /// </summary>
        [Description("提交")]
        submit = 1,
        /// <summary>
        /// 发布
        /// </summary>
        [Description("发布")]
        release = 2,
        /// <summary>
        /// 已审核
        /// </summary>
        [Description("暂存")]
        audited = 3,
        /// <summary>
        /// 未审核
        /// </summary>
        [Description("未审核")]
        notaudited = 4,
        /// <summary>
        /// 拉黑
        /// </summary>
        [Description("拉黑")]
        defriend = 5,
        /// <summary>
        /// 不可见，删除
        /// </summary>
        [Description("删除")]
        deleted = 6,

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
        Permission
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
}
