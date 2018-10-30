using System.Collections.Generic;
using Mock.Code.Web.Tree;

namespace Mock.luo.Areas.Plat.Models
{
    public class AppUserViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string LoginName { get; set; }
        public string NickName { get; set; }
        public string StatusCode { get; set; }
        public List<TreeSelectModel> RoleIds { get; set; }
    }

}