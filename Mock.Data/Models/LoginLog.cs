using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Models
{
    [Table("LoginLog")]
    public class LoginLog
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string LoginName { get; set; }
        public DateTime LoginTime { get; set; }
        [StringLength(50)]
        public string LoginResult { get; set; }
        [StringLength(50)]
        public string IP { get; set; }
        [StringLength(50)]
        public string Agent { get; set; }

    }
}
