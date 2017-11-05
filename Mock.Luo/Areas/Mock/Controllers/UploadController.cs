using Mock.Code;
using Mock.Luo.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mock.Luo.Areas.Mock.Controllers
{
    public class UploadController : BaseController
    {
        // GET: Mock/Upload
        #region 上传图片视图 ImageView
        public ActionResult ImageView()
        {
            return View();
        }
        #endregion

        private AjaxResult UploadImg()
        {
            var file = Request.Files[0];

            string extionName = Path.GetExtension(file.FileName);//  得到 :   .jpg  
            string postfix = extionName.Substring(1, extionName.Length - 1);

            string[] img_type = { "jpg", "jpeg", "gif", "png", "bmp", "webp" };
            if (!img_type.Contains(postfix))
            {
                return AjaxResult.Error("请使用常用的图片类型！你的类型为:" + extionName);
            }
            else
            {
                return UploadFile(file);
            }
        }

        #region 上传单个图片  Image
        [HttpPost]
        public ActionResult Image()
        {
            return Content(UploadImg().ToJson());
        }
        #endregion

        #region 上传文件 UploadFile
        private AjaxResult UploadFile(HttpPostedFileBase file)
        {
            string newFileName = file.FileName.Replace(" ", "");
            newFileName = newFileName.Replace("%7F", "");
            newFileName = newFileName.Substring(newFileName.LastIndexOf('\\') + 1);

            DateTime now = DateTime.Now;
            string datepath = now.Year + "_" + now.Month + "_" + now.Day;
            //产生一个随机名称
            string aFirstName = Guid.NewGuid().ToString();
            //真实文件名
            string filename = file.FileName;
            //扩展名
            string aLastName = Path.GetExtension(newFileName);
            //要保存的文件路径
            var path = Path.Combine(Server.MapPath(Request.ApplicationPath), "UploadFile", datepath);


            string savefilename = aFirstName + aLastName;
            //要保存的文件名称
            if (System.IO.Directory.Exists(path) == false)//如果不存在就创建file文件夹 
            {
                System.IO.Directory.CreateDirectory(path);
            }
            //保存文件到指定的目录
            file.SaveAs(Path.Combine(path, savefilename));//路径+随机名称+.+扩展名
            //上传之后图片的相对路径
            string relativePath = "/UploadFile/" + datepath + "/" + savefilename;

            AjaxResult result = AjaxResult.Success("上传成功！", relativePath);

            return result;
        }
        #endregion

        /// <summary>
        /// layui文本编辑图片上传接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LayuiImage()
        {
            AjaxResult amm = UploadImg();
            string title = Request.Files[0].FileName;
            if (amm.state== ResultType.error.ToString())
            {
                var codeMsg = new
                {
                    code = 1,
                    msg =amm.message,
                    data = new
                    {
                        src = "",
                        titile = title
                    }
                };
                return Result(codeMsg);
            }
            else
            {
                var codeMsg = new
                {
                    code = 0,
                    msg = "上传成功",
                    data = new
                    {
                        src = amm.data,
                        title = title
                    }
                };
                return Result(codeMsg);
            }
        }

    }
}