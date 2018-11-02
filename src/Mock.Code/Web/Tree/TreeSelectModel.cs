using System.Collections.Generic;

namespace Mock.Code.Web.Tree
{
    public class TreeSelectModel
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string ParentId { get; set; }
        public object Data { get; set; }
        /// <summary>
        /// 下拉comboboxTree控件所需
        /// </summary>
        public bool HasChildren { get; set; }
        public List<TreeSelectModel> ChildNodes { get; set; }
    }
}
