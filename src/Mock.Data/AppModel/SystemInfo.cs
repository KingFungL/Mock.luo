using Mock.Code.Web;

namespace Mock.Data.AppModel
{
    public class SystemInfo
    {
        /// <summary>
        /// 当前Tab页面模块Id
        /// </summary>
        public static string CurrentModuleId => WebHelper.GetCookie("currentmoduleId");

        /// <summary>
        /// 当前登录用户Id
        /// </summary>
        public static int CurrentUserId
        {
            get
            {
                return (int) OperatorProvider.Provider.CurrentUser.UserId;
               
            }
        }
    }
}
