using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Models
{
    [Table("Article")]
    public class Article : IEntity<Article>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public int? Id { get; set; }
        public int? FId { get; set; }
        [ForeignKey("FId")]
        public ItemsDetail ItemsDetail { get; set; }
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(200)]
        public string Keywords { get; set; }
        [StringLength(200)]
        public string Source { get; set; }
        [StringLength(200)]
        public string Excerpt { get; set; }
        public string Content { get; set; }
        public int? ViewHits { get; set; }
        public int? CommentQuantity { get; set; }
        public int? PointQuantity { get; set; }
        [StringLength(200)]
        public string thumbnail { get; set; }
        public bool? IsAudit { get; set; }
        public bool? Recommend { get; set; }
        public bool? IsStickie { get; set; }
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
    }
}
