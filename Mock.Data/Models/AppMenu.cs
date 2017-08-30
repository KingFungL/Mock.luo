using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Models
{
    public partial class AppMenu : IEntity<AppUser>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public int? Id { get; set; }
        public int? PId { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public int? SortCode { get; set; }
        public bool? Expanded { get; set; }//默认是展开，还是收缩
        public bool? Folder { get; set; }//是否是文件夹，true,展示文件夹形式图标，否则显示文件的图标
        [StringLength(50)]
        public string Icon { get; set; }
        [StringLength(200)]
        public string LinkUrl { get; set; }
        [StringLength(20)]
        public string Target { get; set; }
        public int? CreatorUserId { get; set; }
        public DateTime? CreatorTime { get; set; }
        public bool? DeleteMark { get; set; }
        public int? DeleteUserId { get; set; }
        public DateTime? DeleteTime { get; set; }
        public int? LastModifyUserId { get; set; }
        public DateTime? LastModifyTime { get; set; }

    }
}
