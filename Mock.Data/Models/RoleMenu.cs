using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Models
{
    [Table("RoleMenu")]
    public class RoleMenu
    {
        [Key]
        public int Id { get; set; }
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public AppRole AppRole { get; set; }
        public int MenuId { get; set; }
        [ForeignKey("MenuId")]
        public AppMenu AppMenu { get; set; }
    }
}
