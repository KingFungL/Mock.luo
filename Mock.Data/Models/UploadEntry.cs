using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Models
{
    [Table("UploadEntry")]
    public class UploadEntry
    {
        public int Id { get; set; }
        public int FId { get; set; }
        [ForeignKey("FId")]
        public Upload Upload { get; set; }
        public DateTime? AddTime { get; set; }
        [StringLength(200)]
        public string Url { get; set; }
        [StringLength(50)]
        public string FileName { get; set; }
        [StringLength(50)]
        public string FileSize { get; set; }

    }
}
