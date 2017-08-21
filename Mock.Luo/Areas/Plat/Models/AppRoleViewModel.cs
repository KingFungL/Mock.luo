using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mock.Luo.Areas.Plat.Models
{
    public class AppRoleViewModel
    {
        public int? Id { get; set; }
        public string RoleName { get; set; }
        public int? SortCode { get; set; }
        public string Remark { get; set; }
        public bool? IsEnableMark { get; set; }
    }
}