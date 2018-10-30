using System.Collections.Generic;
using System.Data;
using Mock.Code.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mock.Code.Json
{
    public static class Json
    {
        public static object ToJson(this string json)
        {

            return json == null ? null : JsonConvert.DeserializeObject(json);
        }
        public static string ToJson(this object obj)
        {
            return JsonHelper.SerializeObject(obj);
        }
        public static string ToJson(this object obj, string datetimeformats)
        {
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jsSettings.DateFormatString = datetimeformats;
            return JsonConvert.SerializeObject(obj, jsSettings);
        }
        public static T ToObject<T>(this string json)
        {
            return json == null ? default(T) : JsonConvert.DeserializeObject<T>(json);
        }
        public static List<T> ToList<T>(this string json)
        {
            return json == null ? null : JsonConvert.DeserializeObject<List<T>>(json);
        }
        public static DataTable ToTable(this string json)
        {
            return json == null ? null : JsonConvert.DeserializeObject<DataTable>(json);
        }
        public static JObject ToJObject(this string json)
        {
            return json == null ? JObject.Parse("{}") : JObject.Parse(json.Replace("&nbsp;", ""));
        }
    }
}
