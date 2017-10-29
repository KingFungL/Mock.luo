using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Dto
{
    public class SiteStatistics
    {
        //文章数量
        public int? ArticleCount { get; set; }
        //点赞
        public int? PointViewCount { get; set; }
        //文章类型
        public  int? ArticleTypeCount { get; set; }
        //标签
        public int? TagCount { get; set; }
        //评论
        public int? ReplyCount { get; set; }
        //查看
        public int? ViewHitCount { get; set; }
    }
}
