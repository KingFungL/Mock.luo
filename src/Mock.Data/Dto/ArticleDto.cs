using Mock.Code.Helper;
using Mock.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Dto
{
    public class ArticleDto
    {
        public int Id { get; set; }

        //类别Id
        public int? FId { get; set; }
        //类别编码  
        public string TypeCode { get; set; }
        //类别名称 
        public string TypeName { get; set; }
        //发布人
        public string NickName { get; set; }
        //几小时/秒前
        public string TimeSpan { get; set; }
        public string Title { get; set; }
        public string Keywords { get; set; }
        public string Source { get; set; }
        public string Excerpt { get; set; }
        public string Content { get; set; }
        public int ViewHits { get; set; }
        //评论
        public int CommentQuantity { get; set; }
        public int PointQuantity { get; set; }
        public string Thumbnail { get; set; }
        public bool IsAudit { get; set; }
        public bool Recommend { get; set; }
        public bool IsStickie { get; set; }
        public int? CreatorUserId { get; set; }
        public DateTime? CreatorTime { get; set; }
    }
}
