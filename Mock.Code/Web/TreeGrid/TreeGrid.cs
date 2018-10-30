using System.Collections.Generic;
using System.Text;

namespace Mock.Code.Web.TreeGrid
{
    public static class TreeGrid
    {
        /// <summary>
        /// 将TreeGridModel集合转成TreeGridJosn字符串
        /// </summary>
        /// <param name="data">List<TreeGridModel>集合</param>
        /// <returns></returns>
        public static string TreeGridJson(this List<TreeGridModel> data,int id=0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{ \"rows\": [");
            sb.Append(TreeGridJson(data, -1, id.ToString()));
            sb.Append("]}");
            return sb.ToString();
        }
        private static string TreeGridJson(List<TreeGridModel> data, int index, string parentId)
        {
            StringBuilder sb = new StringBuilder();
            var childNodeList = data.FindAll(t => t.ParentId == parentId);
            if (childNodeList.Count > 0) { index++; }
            foreach (TreeGridModel entity in childNodeList)
            {
                string strJson = entity.EntityJson;
                strJson = strJson.Insert(1, "\"loaded\":" + (entity.Loaded == true ? false : true).ToString().ToLower() + ",");
                strJson = strJson.Insert(1, "\"expanded\":" + (entity.Expanded).ToString().ToLower() + ",");
                strJson = strJson.Insert(1, "\"isLeaf\":" + (entity.IsLeaf == true ? false : true).ToString().ToLower() + ",");
                strJson = strJson.Insert(1, "\"parent\":\"" + parentId + "\",");
                strJson = strJson.Insert(1, "\"level\":" + index + ",");
                sb.Append(strJson);
                sb.Append(TreeGridJson(data, index, entity.Id));
            }
            return sb.ToString().Replace("}{", "},{");
        }
    }
}
