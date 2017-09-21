
using System;
using System.Collections.Generic;

namespace Mock.Data.Models
{
    public partial class AppModule
    {
        #region 把权限菜单转换化树形结点
        private TreeNode TransformTreeNode()
        {
            //TreeNode为自定义的树形结构相关的属性；右边为SysMenu表中对应的属性
            TreeNode treeNode = new TreeNode()
            {
                id = (int)this.Id,
                text = this.Name,
                expanded = this.Expanded,
                iconcls = this.Icon,
                href = this.LinkUrl,
                target = this.Target,
                attributes = new { url = this.LinkUrl },
                children = new List<TreeNode>()
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

                    LoadTreeNode(listMenus, node.children, node.id);
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
                        id = (int)item.Id,
                        title = item.Name,
                        expanded = item.Expanded,
                        folder = listMenus.FindAll(u=>u.PId==item.Id).Count>0?true:false,
                        data = new
                        {
                            item.Id,
                            item.LinkUrl,
                            item.SortCode,
                            item.Icon,
                            item.Target,
                            item.TypeCode,
                            item.EnCode
                        },
                        children = new List<TreeNode>()
                    };
                    listTreeNodes.Add(node);

                    LoadFancyTreeNode(listMenus, node.children, node.id);
                }
            }
        }
        #endregion
    }
}
