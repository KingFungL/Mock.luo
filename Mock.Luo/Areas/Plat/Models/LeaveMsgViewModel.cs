using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Luo.Areas.Plat.Models
{
    public class LeaveMsgViewModel
    {
        public int? Id { get; set; }
        public string LTitle { get; set; }
        public string LContent { get; set; }
        public bool IsAduit { get; set; }
    }
}
