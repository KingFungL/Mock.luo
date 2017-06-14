
using System;
using System.Collections.Generic;

namespace Mock.Data.Models
{
    public partial class SysMenu
    {
        #region 把权限菜单转换化easyui的treenode
        private TreeNode TransformTreeNode()
        {
            //TreeNode为自定义的树形结构相关的属性；右边为SysMenu表中对应的属性
            TreeNode treeNode = new TreeNode()
            {
                id = this.ID,
                text = this.Name,
                state = "open",
                iconCls = this.Icon,
                Checked = false,
                attributes = new { url = this.GenerateUrl() },
                children = new List<TreeNode>()
            };
            return treeNode;
        }

        #region 组建结点对应的路由
        public string GenerateUrl()
        {
            return FormatUrl(this.LinkUrl);
        }

        //用来处理从数据库中如果读出是空的时候处理
        public string FormatUrl(string name)
        {
            return string.IsNullOrEmpty(name) ? "" : name;
        }
        #endregion
        #endregion

        #region 把权限菜单数据转换成符合easyui的带有递归关系的集合
        public static List<TreeNode> ConvertTreeNodes(List<SysMenu> listPers)
        {
            List<TreeNode> listTreeNodes = new List<TreeNode>();
            LoadTreeNode(listPers, listTreeNodes, 0);  //初始化菜单的父节点为0
            return listTreeNodes;
        }

        private static void LoadTreeNode(List<SysMenu> listPers, List<TreeNode> listTreeNodes, int pid)
        {
            foreach (SysMenu per in listPers)
            {
                if (per.PID == pid)
                {
                    TreeNode node = per.TransformTreeNode();
                    listTreeNodes.Add(node);

                    LoadTreeNode(listPers, node.children, node.id);
                }
            }
        }
        
        #endregion
    }

    public partial class SysMenu 
    {
        public int ID { get; set; }
        public Nullable<int> PID { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string LinkUrl { get; set; }
    }
}
