using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mock.Data.AppModel;
using Mock.Data.Infrastructure;

namespace Mock.Data.Models
{
    
    public partial class AppModule : Entity<AppUser>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public AppModule()
        {
            this.RoleModules = new HashSet<AppRoleModule>();
        }
        public int Id { get; set; }
        public int? PId { get; set; }
        [StringLength(50)]
        public string EnCode { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public int? SortCode { get; set; }
        public bool? Expanded { get; set; }//默认是展开，还是收缩
        //我觉得，可根据其是否有子节点，来判断是否是文件夹
        //public bool? Folder { get; set; }//是否是文件夹，true,展示文件夹形式图标，否则显示文件的图标
        [StringLength(50)]
        public string Icon { get; set; }
        [StringLength(200)]
        public string LinkUrl { get; set; }
        [StringLength(20)]
        public string Target { get; set; }
        /// <summary>
        /// 类型:权限认证,菜单，父菜单，按钮
        /// </summary>
        [StringLength(50)]
        public string TypeCode { get; set; }
        public int? CreatorUserId { get; set; }
        public DateTime? CreatorTime { get; set; }
        public bool? DeleteMark { get; set; }
        public int? DeleteUserId { get; set; }
        public DateTime? DeleteTime { get; set; }
        public int? LastModifyUserId { get; set; }
        public DateTime? LastModifyTime { get; set; }
        public virtual ICollection<AppRoleModule> RoleModules { get; set; }

        #region 把权限菜单转换化树形结点
        private TreeNode TransformTreeNode()
        {
            //TreeNode为自定义的树形结构相关的属性；右边为SysMenu表中对应的属性
            TreeNode treeNode = new TreeNode()
            {
                Id = (int)this.Id,
                Text = this.Name,
                Expanded = this.Expanded,
                Iconcls = this.Icon,
                Href = this.LinkUrl,
                Target = this.Target,
                Attributes = new { url = this.LinkUrl },
                Children = new List<TreeNode>()
            };
            return treeNode;
        }

        #endregion

        #region 把权限菜单数据转换成符合带有递归关系的集合
        /// <summary>
        /// 把权限菜单数据转换成符合带有递归关系的集合
        /// </summary>
        /// <param name="listMenus">菜单模块数据集合</param>
        /// <returns></returns>
        public static List<TreeNode> ConvertTreeNodes(List<AppModule> listMenus)
        {
            List<TreeNode> listTreeNodes = new List<TreeNode>();
            LoadTreeNode(listMenus, listTreeNodes, 0);  //初始化菜单的父节点为0
            return listTreeNodes;
        }

        private static void LoadTreeNode(List<AppModule> listMenus, List<TreeNode> listTreeNodes, int pid)
        {
            foreach (AppModule per in listMenus)
            {
                if (per.PId == pid)
                {
                    TreeNode node = per.TransformTreeNode();
                    listTreeNodes.Add(node);

                    LoadTreeNode(listMenus, node.Children, node.Id);
                }
            }
        }

        #endregion

        #region FancyTree插件TreeGrid后台数据结构
        /// <summary>
        /// FancyTree插件TreeGrid后台数据结构
        /// </summary>
        /// <param name="listMenus">AppModule的List集合</param>
        /// <returns></returns>
        public static List<TreeNode> ConvertFancyTreeNodes(List<AppModule> listMenus)
        {
            List<TreeNode> listTreeNodes = new List<TreeNode>();
            LoadFancyTreeNode(listMenus, listTreeNodes, 0);
            return listTreeNodes;
        }
        private static void LoadFancyTreeNode(List<AppModule> listMenus, List<TreeNode> listTreeNodes, int pid)
        {
            foreach (AppModule item in listMenus)
            {
                if (item.PId == pid)
                {
                    TreeNode node = new TreeNode
                    {
                        Id = (int)item.Id,
                        Text = item.Name,
                        Title=item.Name,
                        Expanded = item.Expanded,
                        Folder = listMenus.FindAll(u => u.PId == item.Id).Count > 0 ? true : false,
                        Data = new
                        {
                            item.Id,
                            item.LinkUrl,
                            item.SortCode,
                            item.Icon,
                            item.Target,
                            item.TypeCode,
                            item.EnCode
                        },
                        Children = new List<TreeNode>()
                    };
                    listTreeNodes.Add(node);

                    LoadFancyTreeNode(listMenus, node.Children, node.Id);
                }
            }
        }
        #endregion

    }
}
