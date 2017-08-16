using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Models
{
    [Table("Upload")]
    public class Upload : IEntity<Upload>, ICreationAudited, IDeleteAudited, IModificationAudited
    {

        [Required, Key]
        public int? Id { get; set; }
        public int? FId { get; set; }
        [StringLength(100)]
        public string Url { get; set; }
        [StringLength(50)]
        public string FileName { get; set; }
        [StringLength(50)]
        public string FileSize { get; set; }
        [StringLength(20)]
        public string Type { get; set; }
        [StringLength(20)]
        public string Mime { get; set; }
        public int? CreatorUserId { get; set; }
        [ForeignKey("CreatorUserId")]
        public AppUser AppUser { get; set; }
        public DateTime? CreatorTime { get; set; }
        public bool? DeleteMark { get; set; }
        public int? DeleteUserId { get; set; }
        public DateTime? DeleteTime { get; set; }
        public int? LastModifyUserId { get; set; }
        public DateTime? LastModifyTime { get; set; }
    }
}
