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
    [Table("ItemsDetail")]
    public class ItemsDetail : Entity<ItemsDetail>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public ItemsDetail()
        {
            this.TagArts = new HashSet<TagArt>();
        }

        public int Id { get; set; }

        public int FId { get; set; }
        [ForeignKey("FId")]
        public Items Items { get; set; }
        [StringLength(50)]
        public string ItemCode { get; set; }
        [StringLength(50)]
        public string ItemName { get; set; }
        public int? SortCode { get; set; }
        public bool? IsEnableMark { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }
        public DateTime? CreatorTime { get; set; }
        public int? CreatorUserId { get; set; }
        public DateTime? LastModifyTime { get; set; }
        public int? LastModifyUserId { get; set; }
        public DateTime? DeleteTime { get; set; }
        public int? DeleteUserId { get; set; }
        public bool? DeleteMark { get; set; }

        public virtual ICollection<TagArt> TagArts { get; set; }
    }
}
