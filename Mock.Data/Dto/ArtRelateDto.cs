using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Dto
{
    //文章详情侧栏与下栏数据源
   public class ArtRelateDto
    {
        //相关文章
        public List<BaseDto> RelateArt { get; set; }
        //推荐文章
        public List<BaseDto> RecommendArt { get; set; }
        //分类
        public List<BaseDto> Category { get; set; }
        //文章归档
        public List<BaseDto> ArchiveFile { get; set; }
        //随机文章
        public List<BaseDto> RandomArt { get; set; }

    }
}
