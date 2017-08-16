using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Models
{
    [Table("AppUser")]
    public class AppUser : IEntity<AppUser>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public AppUser()
        {
            this.UserRoles = new HashSet<UserRole>();
        }
        [Key]
        public int? Id { get; set; }
        [StringLength(50)]
        public string LoginName { get; set; }
        [StringLength(50)]
        public string LoginPassword { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(50)]
        public string Birthday { get; set; }
        [StringLength(50)]
        public string PersonalWebsite { get; set; }
        public int? Sex { get; set; }
        [StringLength(50)]
        public string NickName { get; set; }
        [StringLength(50)]
        public string PersonSignature { get; set; }
        [StringLength(50)]
        public string UserSecretkey { get; set; }
        [StringLength(100)]
        public string HeadHref { get; set; }
        public int LoginCount { get; set; }
        public DateTime? LastLoginTime { get; set; }
        [StringLength(50)]
        public string LastLogIp { get; set; }
        [StringLength(50)]
        public string StatusCode { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public int? CreatorUserId { get; set; }
        public DateTime? CreatorTime { get; set; }
        public bool? DeleteMark { get; set; }
        public int? DeleteUserId { get; set; }
        public DateTime? DeleteTime { get; set; }
        public int? LastModifyUserId { get; set; }
        public DateTime? LastModifyTime { get; set; }
    }
}
