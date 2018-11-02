using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Models
{
    [Table("AppRoleModule")]
    public class AppRoleModule
    {
        [Key]
        public int Id { get; set; }
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public AppRole AppRole { get; set; }
        public int ModuleId { get; set; }
        [ForeignKey("ModuleId")]
        public AppModule AppModule { get; set; }
    }
}
