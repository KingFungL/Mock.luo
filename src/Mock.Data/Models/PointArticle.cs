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
        public virtual AppUser AppUser { get; set; }
        public DateTime? AddTime { get; set; }
       /// <summary>
       /// 点赞人IP
       /// </summary>
       [StringLength(500)]
        public string IP { get; set; }
        [StringLength(500)]
        public string Browser { get; set; }
        [StringLength(500)]
        public string System { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(50)]
        public string LoginName { get; set; }
        [StringLength(500)]
        public string Agent { get; set; }
    }
}
