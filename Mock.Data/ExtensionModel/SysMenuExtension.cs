using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.ExtensionModel
{
    public class SysMenuExtension
    {
        public int? _parentId { get; set; }
        public string MenuState { get; set; }

        public bool Checked { get; set; }

        public string iconCls { get; set; }

        public int? MenuId { get; set; }

        public int? MenuPId { get; set; }

        public string MenuName { get; set; }

        public string PMenuName { get; set; }

        public string LinkUrl { get; set; }

        public bool? IsEnableMark { get; set; }

        public bool? IsVisible { get; set; }

        public int? SortCode { get; set; }

        public  string Icon { get; set; }
        public string state { get; set; }
    }
}
