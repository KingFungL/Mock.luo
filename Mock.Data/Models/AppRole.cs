using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Models
{
    [Table("AppRole")]
    public class AppRole
    {
        public AppRole()
        {
            this.UserRoles = new HashSet<UserRole>();
        }
        public int AppRoleId { get; set; }
        [StringLength(50)]
        public string Guid { get; set; }
        [StringLength(50)]
        public string RoleName { get; set; }
        public  virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
