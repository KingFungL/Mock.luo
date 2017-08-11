using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Models
{
    public class AppMenu
    {
        public int AppMenuId { get; set; }
        [StringLength(50)]
        public string MenuName { get; set; }
        public int? SortCode { get; set; }

    }
}
