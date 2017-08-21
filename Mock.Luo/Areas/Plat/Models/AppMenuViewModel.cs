using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mock.Luo.Areas.Plat.Models
{
    public class AppMenuViewModel
    {
        public int? Id { get; set; }
        public int? PId { get; set; }
        public string MenuName { get; set; }
        public int? SortCode { get; set; }
        public string State { get; set; }
        public string Icon { get; set; }
        public string LinkUrl { get; set; }
        public string Target { get; set; }
    }
}