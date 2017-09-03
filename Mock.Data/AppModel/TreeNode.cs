using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data
{
    /// <summary>
    /// 树型结点
    /// id：节点ID，对加载远程数据很重要。
    //text：显示节点文本。
    //state：节点状态，
    //checked：表示该节点是否被选中。
    //attributes: 被添加到节点的自定义属性。
    //children: 一个节点数组声明了若干节点。
    //iconcls:设置节点的图标
    /// </summary>
    public class TreeNode
    {
        public int id { get; set; }
        public string text { get; set; }
        public string state { get; set; }
        public string iconcls { get; set; }
        public bool @checked { get; set; }
        public object attributes { get; set; }
        public string href { get; set; }
        public string target { get; set; }
        public bool? folder { get; set; }
        public bool? expanded { get; set; }
        public string title { get; set; }
        public object data { get; set; }
        public List<TreeNode> children { get; set; }

    }

}
