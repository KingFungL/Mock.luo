using System;
using System.Web.Mvc;
using Mock.Code.Mail;
using Mock.Luo.Controllers;

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
                bool flag = helper.Send(entity.SendTo, entity.MainTitle, entity.Content);

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
        public string SendTo { get; set; }
        public string MainTitle { get; set; }
        public string Content { get; set; }
        public int Status { get; set; }
    }
}
