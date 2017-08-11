using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Models
{
    public class UserRole
    {
        public int UserRoleId { get; set; }

        public int AppRoleId { get; set; }
        [ForeignKey("AppRoleId")]
        public AppRole AppRole { get; set; }

        public int AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
    }
}
