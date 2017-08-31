using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mock.Luo.Areas.Plat.Models
{
    public class ArticleViewModel
    {
        public int? Id { get; set; }
        public int? FId { get; set; }
        public string Title { get; set; }
        public string Keywords { get; set; }
        public string Source { get; set; }
        public string Excerpt { get; set; }
        public string Content { get; set; }
        public int? ViewHits { get; set; }
        public int? CommentQuantity { get; set; }
        public int? PointQuantity { get; set; }
        public string thumbnail { get; set; }
        public bool? IsAudit { get; set; }
        public bool? Recommend { get; set; }
        public bool? IsStickie { get; set; }
    }
}