using System;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

/// <summary>
/// Config 的摘要说明
/// </summary>
namespace Mock.luo.Content.js.ueditor.net.App_Code
{
    public static class Config
    {
        private static bool _noCache = true;
        private static JObject BuildItems()
        {
            var json = File.ReadAllText(HttpContext.Current.Server.MapPath("config.json"));
            return JObject.Parse(json);
        }

        public static JObject Items
        {
            get
            {
                if (_noCache || _items == null)
                {
                    _items = BuildItems();
                }
                return _items;
            }
        }
        private static JObject _items;


        public static T GetValue<T>(string key)
        {
            return Items[key].Value<T>();
        }

        public static String[] GetStringList(string key)
        {
            return Items[key].Select(x => x.Value<String>()).ToArray();
        }

        public static String GetString(string key)
        {
            return GetValue<String>(key);
        }

        public static int GetInt(string key)
        {
            return GetValue<int>(key);
        }
    }

}
