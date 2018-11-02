using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Mock.Data.Models
{
    [Table("TagArt")]
    public class TagArt
    {
        [Key]
        public int Id { get; set; }
        public int TagId { get; set; }
        [ForeignKey("TagId")]
        public ItemsDetail ItemsDetail { get; set; }
        public int AId { get; set; }

        [ForeignKey("AId")]

        public Article Article { get; set; }
    }
}
