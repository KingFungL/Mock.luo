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
    public class AppUser
    {
        public AppUser()
        {
            this.UserRoles = new HashSet<UserRole>();
        }
        [Key]
        public int AppUserId { get; set; }
        [StringLength(50)]
        public string Guid { get; set; }
        [StringLength(50)]
        public string UserName { get; set; }
        [StringLength(50)]
        public string LoginName { get; set; }
        [StringLength(50)]
        public string LoginPassword { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        public int? Sex { get; set; }

        public int? BranchOfficeId { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

    }
}
