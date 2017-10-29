using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Dto
{
   public class ArtDetailDto:ArticleDto
    {      
        //作者头像
        public string HeadHref { get; set; }
        //一个文章对应的标签内容
        public List<BaseDto> TagsList { get; set; }
        public string PersonSignature { get; set; }

        public BaseDto PreviousPage { get; set; }

        public BaseDto NextPage { get; set; }

    }
}
