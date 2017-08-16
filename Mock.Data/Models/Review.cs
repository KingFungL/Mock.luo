using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Models
{
    [Table("Review")]
    public class Review
    {
        public int Id { get; set; }
        public int? AId { get; set; }
        public int? PId { get; set; }
        [ForeignKey("AId")]
        public Article Article { get; set; }
        [StringLength(500)]
        public string Text { get; set; }
        [StringLength(50)]
        public string Ip { get; set; }
        [StringLength(50)]
        public string Agent { get; set; }
        public DateTime? AddTime { get; set; }
        public int? AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public AppUser AppUser { get; set; }
        [StringLength(50)]
        public string Author { get; set; }
        [StringLength(50)]
        public string AuEmail { get; set; }
        [StringLength(50)]
        public string Status { get; set; }

    }
}
