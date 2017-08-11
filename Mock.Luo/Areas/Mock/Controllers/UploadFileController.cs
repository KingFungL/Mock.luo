using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mock.Code;
using Mock.Data;
using System.Data.Entity;
using Mock.Luo.Controllers;
using Mock.Data.Models;
using MoBlog.Domain;
using Autofac.Core;

namespace Mock.Luo.Areas.Mock.Controllers
{
    public class UploadFileController : BaseController
    {
        //
        // GET: /Mock/UploadFile/

        private readonly IUploadRepository _service;
        public UploadFileController() { }
        public UploadFileController(IUploadRepository service)
        {
            _service = service;
        }
        
        public ActionResult UploadView()
        {
            return View();
        }


        public ActionResult Upload()
        {
            //得到上传的图片
            var file = Request.Files[0];


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

            AjaxMsg result = new AjaxMsg
            {
                code = "200",
                msg = "上传成功",
                obj = relativePath
            };
            return Content(DataHelper.ObjToJson(result));
        }

        public ActionResult SubmitForm(string UserName, string[] Url, string[] FileName, string[] FileSize, string Id = "")
        {
            //UploadifyEntity entity=DataHelper.JsonToObj<UploadifyEntity>(data);


            using (var db = new MockDbContext())
            {

                //新增
                if (string.IsNullOrEmpty(Id))
                {
                    Upload entity = new Upload
                    {
                        AddTime = DateTime.Now,
                        UserName = UserName,
                    };
                    if (Url.Length > 0)
                    {
                        int i = 0;
                        foreach (var url in Url)
                        {
                            UploadEntry entry = new UploadEntry
                            {
                                AddTime = DateTime.Now,
                                Url = url,
                                FileName = FileName[i],
                                FileSize = FileSize[i]
                            };
                            i++;
                            entity.UploadEntry.Add(entry);
                        }
                    }
                    db.Set<Upload>().Add(entity);
                    db.SaveChanges();
                }
                else
                {
                    int id = int.Parse(Id);
                    DateTime now = DateTime.Now;
                    Upload entity = db.Set<Upload>().AsNoTracking().Where(u => u.Id == id).First();
                    entity.UserName = UserName;
                    entity.AddTime = now;
                    db.Set<Upload>().Attach(entity);
                    db.Entry(entity).Property("UserName").IsModified = true;
                    db.Entry(entity).Property("AddTime").IsModified = true;

                    var entitys = db.Set<UploadEntry>().Where(u => u.FId == id).ToList();
                    entitys.ForEach(m => db.Entry<UploadEntry>(m).State = EntityState.Deleted);

                    if (Url != null && Url.Length > 0)
                    {
                        int i = 0;
                        foreach (var url in Url)
                        {
                            UploadEntry entry = new UploadEntry
                            {
                                FId = id,
                                AddTime = now,
                                Url = url,
                                FileName = FileName[i],
                                FileSize = FileSize[i]
                            };
                            i++;
                            db.Set<UploadEntry>().Add(entry);
                        }
                    }
                    db.SaveChanges();
                }
            }


            AjaxMsg result = AjaxMsg.Success(string.IsNullOrEmpty(Id) ? "新增成功1" : "编辑成功！");
            return Content(DataHelper.ObjToJson(result));

        }

        [HttpGet]
        public ActionResult GetForm(int Id)
        {
            var result = _service.GetFormById(Id);

            return Content(DataHelper.ObjToJson(result));
        }
    }


    public class UploadifyEntity
    {
        public string UserName { get; set; }
        public List<string> Url { get; set; }
        public List<string> FileName { get; set; }
    }
}
