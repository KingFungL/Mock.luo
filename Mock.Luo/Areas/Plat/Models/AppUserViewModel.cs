using Mock.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mock.Luo.Areas.Plat.Models
{
    public class AppUserViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string LoginName { get; set; }
        public string NickName { get; set; }
        public string StatusCode { get; set; }
        public List<TreeSelectModel> roleIds { get; set; }
    }

}