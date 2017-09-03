using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mock.Luo.Areas.Plat.Models
{
    public class AppModuleViewModel
    {
        public int? Id { get; set; }
        public int? PId { get; set; }
        public string Name { get; set; }
        public int? SortCode { get; set; }
        public bool? Expanded { get; set; }
        public bool? Folder { get; set; }
        public string Icon { get; set; }
        public string LinkUrl { get; set; }
        public string Target { get; set; }
        public bool? Status { get; set; }
        public string TypeCode { get; set; }
    }
}