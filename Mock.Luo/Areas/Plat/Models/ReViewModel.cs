using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mock.Luo.Areas.Plat.Models
{
    public class ReViewModel
    {
        public int Id { get; set; }
        public int? AId { get; set; }
        public int? PId { get; set; }
        public string Text { get; set; }
        public string Ip { get; set; }
        public string Agent { get; set; }
        public string AuName { get; set; }
        public string AuEmail { get; set; }
        public bool? IsAduit { get; set; }
        public int? CreatorUserId { get; set; }
        public DateTime? CreatorTime { get; set; }
    }
}