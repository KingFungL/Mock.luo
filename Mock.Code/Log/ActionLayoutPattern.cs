using log4net.Layout;
using log4net.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.Code
{
    public class ActionLayoutPattern : PatternLayout
    {
        public ActionLayoutPattern()
        {
            AddConverter(new ConverterInfo
            {
                Name = "actioninfo",
                Type = typeof(ActionConverter)
            });
        }
    }
}