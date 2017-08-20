using Mock.Luo.Controllers;
using System;
using Mock.Code;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mock.Data;

namespace Mock.Luo.Areas.Mock.Controllers
{
    public class EditorController : BaseController
    {
        //
        // GET: /Mock/Editor/

        public ActionResult MarkDownView()
        {
            return View();
        }

        public ActionResult CodeView()
        {
            return View();
        }
        public ActionResult SimditorView()
        {
            return View();
        }

        public ActionResult EmailIndex()
        {
            return View();
        }

        public ActionResult SendView()
        {
            return View();
        }

        public ActionResult SendOkView()
        {
            return View();
        }
        public ActionResult InboxView()
        {
            return View();
        }

        public ActionResult DraftboxView()
        {
            return View();
        }

        public ActionResult OutboxView()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult SendEmail(EmailEntity entity)
        {
            ActionResult result;
            MailHelper helper = new MailHelper();
            try
            {
                bool flag = helper.Send(entity.sendTo, entity.mainTitle, entity.content);

                if (flag == true)
                {
                    result = Success("发送成功！");

                }
                else
                {
                    result = Error("发送失败！");
                }
            }
            catch (Exception ex)
            {
                result = Error("发送失败！" + ex);
            }

            return result;
        }
    }
    public class EmailEntity
    {
        public string sendTo { get; set; }
        public string mainTitle { get; set; }
        public string content { get; set; }
        public int status { get; set; }
    }
}
