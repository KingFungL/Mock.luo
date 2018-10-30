using log4net.Layout;
using log4net.Util;

namespace Mock.Code.Log
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