
using System;
using System.Collections.Generic;

namespace Mock.Code
{
    public class TreeSelectModel
    {
        public string id { get; set; }
        public string text { get; set; }
        public string parentId { get; set; }
        public object data { get; set; }
        /// <summary>
        /// 下拉comboboxTree控件所需
        /// </summary>
        public bool hasChildren { get; set; }
        public List<TreeSelectModel> ChildNodes { get; set; }
    }
}
