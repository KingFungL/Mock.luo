using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Models
{
    public class AppMenu : IEntity<AppUser>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public int? Id { get; set; }
        [StringLength(50)]
        public string MenuName { get; set; }
        public int? SortCode { get; set; }

        public int?  CreatorUserId { get; set; }
        public DateTime?  CreatorTime { get; set; }
        public bool?  DeleteMark { get; set; }
        public int? DeleteUserId { get; set; }
        public DateTime? DeleteTime { get; set; }
        public int? LastModifyUserId { get; set; }
        public DateTime? LastModifyTime { get; set; }

    }
}
