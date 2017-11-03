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
            this.Articles = new HashSet<Article>();
            this.GuestBooks = new HashSet<GuestBook>();
            this.Uploads = new HashSet<Upload>();
            this.AppUserAuths = new HashSet<AppUserAuth>();
            this.Reviews = new HashSet<Review>();
            this.GuestBooks = new HashSet<GuestBook>();
        }
        [Key]
        public int? Id { get; set; }
        [StringLength(50)]
        public string LoginName { get; set; }
        [StringLength(50,ErrorMessage ="最长为11")]
        public string QQ { get; set; }
        [StringLength(50)]
        public string LoginPassword { get; set; }
        [StringLength(20,ErrorMessage ="手机长度不能超过20位")]
        public string Phone { get; set; }
        [StringLength(50, ErrorMessage = "邮件长度不能超过50位")]
        public string Email { get; set; }
        public bool EmailIsValid { get; set; }

        [StringLength(50)]
        public string Birthday { get; set; }
        [StringLength(300)]
        public string PersonalWebsite { get; set; }
        [StringLength(10)]
        public string Gender { get; set; }
        [StringLength(50)]
        public string NickName { get; set; }
        [StringLength(100,ErrorMessage ="个性签名不能超过100字")]
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
        public int? CreatorUserId { get; set; }
        public DateTime? CreatorTime { get; set; }
        public bool? DeleteMark { get; set; }
        public int? DeleteUserId { get; set; }
        public DateTime? DeleteTime { get; set; }
        public int? LastModifyUserId { get; set; }
        public DateTime? LastModifyTime { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<Upload> Uploads { get; set; }
        public virtual ICollection<AppUserAuth> AppUserAuths { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<GuestBook> GuestBooks { get; set; }

    }
}
