using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Dto
{
    //最新吐槽回复dto
    [Serializable]
    public class ReplyDto
    {
        public int? Id { get; set; }
        public string Text { get; set; }

        public int? AId { get; set; }

        public string ArticleTitle { get; set; }

        public string NickName { get; set; }

        public string HeadHref { get; set; }
    }
}
