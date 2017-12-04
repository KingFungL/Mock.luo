using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.ExtensionModel
{
    [Serializable]
    public class ResetPwd
    {
        public string LoginName { get; set; }

        public string NickName { get; set; }
        public string ModifyPwdToken { get; set; }
        public DateTime PwdCodeTme { get; set; }
        public int UserID { get; set; }
        public string ModfiyPwdCode { get; set; }
    }
}
