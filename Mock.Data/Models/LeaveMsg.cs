using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Models
{
    [Table("LeaveMsg")]
    public class LeaveMsg : IEntity<LeaveMsg>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public LeaveMsg()
        {
            this.ReLeaves = new HashSet<ReLeave>();
        }

        public int? Id { get; set; }
        [StringLength(50)]
        public string LTitle { get; set; }
        public string LContent { get; set; }
        public bool IsAduit { get; set; }
        public int? CreatorUserId { get; set; }
        public DateTime? CreatorTime { get; set; }
        public bool? DeleteMark { get; set; }
        public int? DeleteUserId { get; set; }
        public DateTime? DeleteTime { get; set; }
        public int? LastModifyUserId { get; set; }
        public DateTime? LastModifyTime { get; set; }
        [ForeignKey("CreatorUserId")]
        public virtual AppUser AppUser { get; set; }
        public virtual ICollection<ReLeave> ReLeaves { get; set; }
    }
}
