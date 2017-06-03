using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AjaxMsg
    {
        public string code { get; set; }
        public string msg { get; set; }
        public object obj { get; set; }

        public static AjaxMsg Success(string code, string msg, string obj)
        {
            return new AjaxMsg { code = code, msg = msg, obj = obj };
        }

        public static AjaxMsg Success(string code, string msg)
        {
            return Success(code, msg, null);
        }

        public static AjaxMsg Success(string msg="成功！")
        {
            return Success("200", msg);
        }

        public static AjaxMsg Error(string code, string msg, string obj)
        {
            return new AjaxMsg { code = code, msg = msg, obj = obj };
        }

        public static AjaxMsg Error(string code, string msg)
        {
            return Error(code, msg, null);
        }

        public static AjaxMsg Error(string msg="失败")
        {
            return Error("403", msg);
        }

    }
}