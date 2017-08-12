using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Models
{
    [Table("ThirdUser")]
    public class ThirdUser
    {
        public int? Id { get; set; }
        [StringLength(50)]
        public string BindSource { get; set; }
        [StringLength(50)]
        public string BindUserCode { get; set; }
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }
        public DateTime? AddTime { get; set; }
    }
}
