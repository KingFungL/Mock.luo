
using System.Collections.Generic;
using System.Text;

namespace Mock.Code
{
    public static class TreeSelect
    {
        /// <summary>
        /// 将select2实体集合转成下拉json字符串
        /// </summary>
        /// <param name="data">下拉实体list</param>
        /// <returns>格式化后的json字符串</returns>
        public static string TreeSelectJson(this List<TreeSelectModel> data)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append(TreeSelectJson(data, "0", ""));
            sb.Append("]");
            return sb.ToString();
        }
        private static string TreeSelectJson(List<TreeSelectModel> data, string parentId, string blank)
        {
            StringBuilder sb = new StringBuilder();
            var ChildNodeList = data.FindAll(t => t.parentId == parentId);
            var tabline = "";
            if (parentId != "0")
            {
                tabline = "　　";
            }
            if (ChildNodeList.Count > 0)
            {
                tabline = tabline + blank;
            }
            foreach (TreeSelectModel entity in ChildNodeList)
            {
                entity.text = tabline + entity.text;
                string strJson = entity.ToJson();
                sb.Append(strJson);
                sb.Append(TreeSelectJson(data, entity.id, tabline));
            }
            return sb.ToString().Replace("}{", "},{");
        }

        public static string ComboboxTreeJson(this List<TreeSelectModel> data,int PId=0)
        {
            List<TreeSelectModel> listTreeNodes = new List<TreeSelectModel>();
            ComboboxTreeJson(data, listTreeNodes, PId.ToString());
            return listTreeNodes.ToJson();
        }

        private static void ComboboxTreeJson(List<TreeSelectModel> listModels, List<TreeSelectModel>listTreeNodes,string pid)
        {
            foreach (TreeSelectModel item in listModels)
            {
                if (item.parentId == pid)
                {
                    TreeSelectModel node = new TreeSelectModel
                    {
                        id = item.id,
                        text = item.text,
                        parentId = item.parentId,
                        hasChildren=listModels.TreeWhere(u=>u.parentId==item.id).Count>0?true:false,
                        ChildNodes = new List<TreeSelectModel>()
                    };
                    listTreeNodes.Add(node);

                    ComboboxTreeJson(listModels, node.ChildNodes, node.id);
                }
            }
        }
    }
}