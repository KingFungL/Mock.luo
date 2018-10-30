using System.Collections.Generic;

namespace Mock.Data.AppModel
{
    ///<summary>
    ///树型结点
    ///id：节点ID，对加载远程数据很重要。
    //text：显示节点文本。
    //state：节点状态，
    //checked：表示该节点是否被选中。
    //attributes: 被添加到节点的自定义属性。
    //children: 一个节点数组声明了若干节点。
    //iconcls:设置节点的图标
    ///</summary>
    public class TreeNode
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string State { get; set; }
        public string Iconcls { get; set; }
        public bool Checked { get; set; }
        public object Attributes { get; set; }
        public string Href { get; set; }
        public string Target { get; set; }
        public bool? Folder { get; set; }
        public bool? Expanded { get; set; }
        public string Title { get; set; }
        public object Data { get; set; }
        public List<TreeNode> Children { get; set; }

    }

}
