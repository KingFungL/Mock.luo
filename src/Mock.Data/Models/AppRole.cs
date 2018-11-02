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
    [Table("AppRole")]
    public class AppRole : Entity<AppRole>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public AppRole()
        {
            this.UserRoles = new HashSet<AppUserRole>();
        }
        public int Id { get; set; }
        [StringLength(50)]
        public string RoleName { get; set; }
        [StringLength(50)]
        public string Remark { get; set; }
        public int? SortCode { get; set; }

        public bool? IsEnableMark { get; set; }
        public virtual ICollection<AppUserRole> UserRoles { get; set; }
        public int? CreatorUserId { get; set; }
        public DateTime? CreatorTime { get; set; }
        public bool? DeleteMark { get; set; }
        public int? DeleteUserId { get; set; }
        public DateTime? DeleteTime { get; set; }
        public int? LastModifyUserId { get; set; }
        public DateTime? LastModifyTime { get; set; }
    }
}
