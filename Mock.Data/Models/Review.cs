using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Models
{
    [Table("Review")]
    public class Review: IEntity<Review>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public int? Id { get; set; }
        public int? AId { get; set; }
        public int? PId { get; set; }
        [ForeignKey("AId")]
        public Article Article { get; set; }
        [StringLength(500)]
        public string Text { get; set; }
        [StringLength(50)]
        public string Ip { get; set; }
        [StringLength(50)]
        public string Agent { get; set; }
        [StringLength(50)]
        public string AuName { get; set; }
        [StringLength(50)]
        public string AuEmail { get; set; }
        /// <summary>
        /// 是否已审核
        /// </summary>
        public bool? IsAduit { get; set; }
        public int? CreatorUserId { get; set; }
        [ForeignKey("CreatorUserId")]
        public AppUser AppUser { get; set; }
        public DateTime? CreatorTime { get; set; }
        public bool? DeleteMark { get; set; }
        public int? DeleteUserId { get; set; }
        public DateTime? DeleteTime { get; set; }
        public int? LastModifyUserId { get; set; }
        public DateTime? LastModifyTime { get; set; }

    }
}
