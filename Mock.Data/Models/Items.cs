using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Models
{
    [Table("Items")]
    public class Items : IEntity<Items>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public int? Id { get; set; }
        public int PId { get; set; }
        [StringLength(50)]
        public string EnCode { get; set; }
        [StringLength(50)]
        public string FullName { get; set; }
        public bool? IsTree { get; set; }
        public int? Layers { get; set; }
        public int? SortCode { get; set; }
        public bool? EnabledMark { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        public DateTime? CreatorTime { get; set; }
        public int? CreatorUserId { get; set; }
        public DateTime? LastModifyTime { get; set; }
        public int? LastModifyUserId { get; set; }
        public DateTime? DeleteTime { get; set; }
        public int? DeleteUserId { get; set; }
        public bool? DeleteMark { get; set; }
        public virtual ICollection<ItemsDetail> ItemsDetail { get; set; }
    }
}
