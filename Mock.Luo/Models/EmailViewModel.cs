using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mock.Luo.Models
{
    public class EmailViewModel
    {
        public string ToUserName { get; set; }

        public string FromUserName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Date { get; set; }

        //点击链接
        public string Link { get; set; }

        public string Code { get; set; }

    }
}