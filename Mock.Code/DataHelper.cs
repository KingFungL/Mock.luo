using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace Mock.Code
{
    public class DataHelper
    {

        #region  把对象转换成JSON格式
        //js序列化器
        static JavaScriptSerializer _jss = new JavaScriptSerializer();
        //日期序列化模版
        static IsoDateTimeConverter _timeConverter = new IsoDateTimeConverter();
        /// <summary>
        /// 把对象转换成JSON格式
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>json格式数据</returns>
        public static string ObjToJson(object obj)
        {

            return _jss.Serialize(obj);
        }

        //序列化成固定日期格式的JSON数据
        public static string ObjToJsonFormatDate(object obj)
        {
            _timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd";
            return JsonConvert.SerializeObject(obj, Formatting.Indented, _timeConverter);
        }
        #endregion

        //反序列化成对象
        public static T JsonToObj<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        //#region 把一个字符串进行MD5加密
        //public static string Md5(string str)
        //{
        //    return FormsAuthentication.HashPasswordForStoringInConfigFile(str, FormsAuthPasswordFormat.MD5.ToString());
        //}
        //#endregion

        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Md5Hash(string input)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public static Dictionary<string, object> GetAddOrEditOrDel<T>(HttpRequestBase request)
        {
            List<T> insertedList, deletedList, updatedList;
            string inserted = request["Inserted"] ?? "[]";
            insertedList = _jss.Deserialize<List<T>>(inserted);

            string deleted = request["Deleted"] ?? "[]";
            deletedList = _jss.Deserialize<List<T>>(deleted);

            string updated = request["Updated"] ?? "[]";
            updatedList = _jss.Deserialize<List<T>>(updated);

            Dictionary<string, object> di = new Dictionary<string, object>();
            di.Add("Inserted", insertedList);
            di.Add("Deleted", deletedList);
            di.Add("Updated", updatedList);
            return di;
        }
     


    }
}
