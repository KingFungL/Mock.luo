using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Data.Dto
{
    public class BaseDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

       /// <summary>
       /// 显示名称
       /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
    }
}
