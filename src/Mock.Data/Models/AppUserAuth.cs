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
    [Table("AppUserAuth")]
    public class AppUserAuth : Entity<AppUserAuth>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        [Key]
        public int Id { get; set; }
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual  AppUser AppUser { get; set; }
        [StringLength(50)]
        public string IdentityType { get; set; }
        [StringLength(50)]
        public string OpenId { get; set; }
        [StringLength(50)]
        public string AccessToken { get; set; }
        public int? ExpiresAt { get; set; }
        public int? CreatorUserId { get; set; }
        public DateTime? CreatorTime { get; set; }
        public bool? DeleteMark { get; set; }
        public int? DeleteUserId { get; set; }
        public DateTime? DeleteTime { get; set; }
        public int? LastModifyUserId { get; set; }
        public DateTime? LastModifyTime { get; set; }

    }
}

