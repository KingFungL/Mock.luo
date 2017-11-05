
using System;

namespace Mock.Data
{
    public class OperatorModel
    {
        public int? UserId { get; set; }
        public string UserCode { get; set; }
        public string LoginName { get; set; }
        /// <summary>
        /// 登录唯一标识符
        /// </summary>
        public string LoginToken { get; set; }
        public DateTime? LoginTime { get; set; }

        public string NickName { get; set; }

        public string Avatar { get; set; }

        public bool? IsSystem { get; set; }
    }
}
