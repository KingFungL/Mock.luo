using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Models
{
    [Table("PointArticle")]
    public class PointArticle
    {
        [Key]
        public int Id { get; set; }
        public int AId { get; set; }
        [ForeignKey("AId")]
        public Article Article { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual AppUser AppUser { get; set; }
        public DateTime? AddTime { get; set; }
    }

}
