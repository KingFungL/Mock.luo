namespace Mock.Code.Security
{
    /// <summary>
    /// MD5加密
    /// </summary>
    public class Md5Helper
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">加密字符</param>
        /// <param name="code">加密位数16/32</param>
        /// <returns></returns>
        public static string Md5(string str, int code)
        {
            string strEncrypt = string.Empty;
            if (code == 16)
            {
                strEncrypt = DataHelper.Md5Hash(str).Substring(8, 16);
            }

            if (code == 32)
            {
                strEncrypt = DataHelper.Md5Hash(str);
            }

            return strEncrypt;
        }
    }
}
