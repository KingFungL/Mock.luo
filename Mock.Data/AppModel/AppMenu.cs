
using System;
using System.Collections.Generic;

namespace Mock.Data.Models
{
    public partial class AppMenu
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
                folder=this.Folder,
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
        public static List<TreeNode> ConvertTreeNodes(List<AppMenu> listMenus)
        {
            List<TreeNode> listTreeNodes = new List<TreeNode>();
            LoadTreeNode(listMenus, listTreeNodes, 0);  //初始化菜单的父节点为0
            return listTreeNodes;
        }

        private static void LoadTreeNode(List<AppMenu> listMenus, List<TreeNode> listTreeNodes, int pid)
        {
            foreach (AppMenu per in listMenus)
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
        public static List<TreeNode> ConvertFancyTreeNodes(List<AppMenu> listMenus)
        {
            List<TreeNode> listTreeNodes = new List<TreeNode>();
            LoadFancyTreeNode(listMenus, listTreeNodes, 0);
            return listTreeNodes;
        }
        private static void LoadFancyTreeNode(List<AppMenu> listMenus, List<TreeNode> listTreeNodes, int pid)
        {
            foreach (AppMenu item in listMenus)
            {
                if (item.PId == pid)
                {
                    TreeNode node = new TreeNode
                    {
                        id = (int)item.Id,
                        title = item.Name,
                        expanded = item.Expanded,
                        folder = item.Folder,
                        data = new
                        {
                            Id = item.Id,
                            LinkUrl = item.LinkUrl,
                            SortCode = item.SortCode,
                            Icon = item.Icon,
                            Target = item.Target,
                            Expanded = item.Expanded,
                            Folder = item.Folder
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
