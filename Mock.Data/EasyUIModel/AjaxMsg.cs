using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data
{
    public class AjaxMsg
    {
        public string code { get; set; }
        public string msg { get; set; }
        public object obj { get; set; }

        public static AjaxMsg Success(string code, string msg, object obj)
        {
            return new AjaxMsg { code = code, msg = msg, obj = obj };
        }
        public static AjaxMsg Success(string msg, object obj = null)
        {
            return Success("200", msg, obj);
        }

        public static AjaxMsg Error(string code, string msg, object obj)
        {
            return new AjaxMsg { code = code, msg = msg, obj = obj };
        }

        public static AjaxMsg Error(string msg, object obj = null)
        {
            return Success("403", msg, obj);
        }
    }
}
