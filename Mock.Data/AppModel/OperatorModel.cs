
using System;

namespace Mock.Data.AppModel
{
    public class OperatorModel
    {
        public int UserId { get; set; }
        public string UserCode { get; set; }
        public string LoginName { get; set; }

        public string Email { get; set; }

        public string PersonalWebsite { get; set; }
        /// <summary>
        /// 登录唯一标识符
        /// </summary>
        public string LoginToken { get; set; }
        public DateTime? LoginTime { get; set; }

        public string NickName { get; set; }

        public string Avatar { get; set; }

        public bool? IsSystem { get; set; }
        public bool IsAdmin { get; set; }
    }
}
