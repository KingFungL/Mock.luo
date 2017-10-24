using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Models
{
    [Table("LogInfo")]
    public class LogInfo
    {
        [Key]
        public long LogID { get; set; }
        public DateTime LogDate { get; set; }
        public string Thread { get; set; }
        [StringLength(50)]
        public string LogLevel { get; set; }
        [StringLength(500)]
        public string Logger { get; set; }
        public string Message { get; set; }

        public string Exception { get; set; }
    }
}
