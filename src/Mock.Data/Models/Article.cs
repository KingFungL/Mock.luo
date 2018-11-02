using Mock.Code.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mock.Data.Infrastructure;

namespace Mock.Data.Models
{
    [Table("Article")]
    [Serializable]
    public class Article : Entity<Article>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public Article()
        {
            this.Reviews = new HashSet<Review>();
            this.TagArts = new HashSet<TagArt>();
        }
        public int Id { get; set; }
        public int? FId { get; set; }
        [ForeignKey("FId")]
        public ItemsDetail ItemsDetail { get; set; }
        [StringLength(200)]
        public string Title { get; set; }
        [StringLength(400)]
        public string Keywords { get; set; }
        [StringLength(400)]
        public string Source { get; set; }
        [StringLength(400)]
        public string Excerpt { get; set; }
        public string Content { get; set; }
        public int? ViewHits { get; set; }
        public int? CommentQuantity { get; set; }
        public int? PointQuantity { get; set; }
        [StringLength(400)]
        public string Thumbnail { get; set; }
        public bool? IsAudit { get; set; }
        public bool? Recommend { get; set; }
        public bool? IsStickie { get; set; }
        /// <summary>
        /// 随笔档案   如2019年1月
        /// </summary>
        [StringLength(50)]
        public string Archive { get; set; }
        public int? CreatorUserId { get; set; }
        [ForeignKey(" CreatorUserId")]
        public AppUser AppUser { get; set; }
        public DateTime? CreatorTime { get; set; }
        public bool? DeleteMark { get; set; }
        public int? DeleteUserId { get; set; }
        public DateTime? DeleteTime { get; set; }
        public int? LastModifyUserId { get; set; }
        public DateTime? LastModifyTime { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<TagArt> TagArts { get; set; }

     
    }
}
