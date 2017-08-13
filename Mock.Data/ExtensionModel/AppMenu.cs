
using System;
using System.Collections.Generic;

namespace Mock.Data.Models
{
    public partial class AppMenu
    {
        #region 把权限菜单转换化easyui的treenode
        private TreeNode TransformTreeNode()
        {
            //TreeNode为自定义的树形结构相关的属性；右边为SysMenu表中对应的属性
            TreeNode treeNode = new TreeNode()
            {
                id = (int)this.Id,
                text = this.MenuName,
                state = this.State,
                iconCls = this.Icon,
                Checked = false,
                href = this.LinkUrl,
                sortcode = this.SortCode,
                target = this.Target,
                attributes = new { url = this.LinkUrl },
                children = new List<TreeNode>()
            };
            return treeNode;
        }

        #endregion

        #region 把权限菜单数据转换成符合easyui的带有递归关系的集合
        public static List<TreeNode> ConvertTreeNodes(List<AppMenu> listPers)
        {
            List<TreeNode> listTreeNodes = new List<TreeNode>();
            LoadTreeNode(listPers, listTreeNodes, 0);  //初始化菜单的父节点为0
            return listTreeNodes;
        }

        private static void LoadTreeNode(List<AppMenu> listPers, List<TreeNode> listTreeNodes, int pid)
        {
            foreach (AppMenu per in listPers)
            {
                if (per.PId == pid)
                {
                    TreeNode node = per.TransformTreeNode();
                    listTreeNodes.Add(node);

                    LoadTreeNode(listPers, node.children, node.id);
                }
            }
        }

        #endregion
    }
}
