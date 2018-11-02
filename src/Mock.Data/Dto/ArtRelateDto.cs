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
        /// <summary>
        /// 相关文章
        /// </summary>
        public List<BaseDto> RelateArt { get; set; }
        /// <summary>
        /// 推荐文章
        /// </summary>
        public List<BaseDto> RecommendArt { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public List<BaseDto> Category { get; set; }
        /// <summary>
        /// 文章归档
        /// </summary>
        public List<BaseDto> ArchiveFile { get; set; }
        /// <summary>
        /// 随机文章
        /// </summary>
        public List<BaseDto> RandomArt { get; set; }
        /// <summary>
        /// 文章标签
        /// </summary>
        public List<BaseDto> ArtTag { get; set; }

    }
}
