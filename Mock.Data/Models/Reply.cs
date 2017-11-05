using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Models
{
    [Table("Reply")]
    public class Reply : IEntity<Reply>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        [Key]
        public int? Id { get; set; }
        public int? PId { get; set; }
        public string Text { get; set; }
        [StringLength(50)]
        public string Ip { get; set; }
        [StringLength(50)]
        public string Agent { get; set; }
        //系统
        [StringLength(50)]
        public string System { get; set; }
        /// <summary>
        /// 地理位置
        /// </summary>
        [StringLength(50)]
        public string GeoPosition { get; set; }
        /// <summary>
        /// 使用的网络供应商
        /// </summary> 
        [StringLength(50)]
        public string QQ { get; set; }
        [StringLength(50, ErrorMessage = "昵称不能超过50字符")]
        public string AuName { get; set; }
        [StringLength(400, ErrorMessage = "个人网址不能超过400字符")]
        public string PersonalWebsite { get; set; }
        [StringLength(50, ErrorMessage = "邮箱不能超过50字符")]
        public string AuEmail { get; set; }
        [StringLength(50)]
        public string UserHost { get; set; }
        /// <summary>
        /// 是否已审核
        /// </summary>
        public bool? IsAduit { get; set; }
        /// <summary>
        /// 评论人的头像，如果未登录状态下，则随机生成一个头像地址。已登录状态下，取用户表的头像地址
        /// </summary>
        [StringLength(500)]
        public string Avatar { get; set; }
        public int? CreatorUserId { get; set; }
        public DateTime? CreatorTime { get; set; }
        public bool? DeleteMark { get; set; }
        public int? DeleteUserId { get; set; }
        public DateTime? DeleteTime { get; set; }
        public int? LastModifyUserId { get; set; }
        public DateTime? LastModifyTime { get; set; }
        [ForeignKey("CreatorUserId")]
        public virtual AppUser AppUser { get; set; }

    }

    [Table("Review")]
    public class Review : Reply
    {
        public int? AId { get; set; }
        [ForeignKey("AId")]
        public virtual Article Article { get; set; }
    }

    [Table("GuestBook")]
    public class GuestBook : Reply
    {
        
    }

}
