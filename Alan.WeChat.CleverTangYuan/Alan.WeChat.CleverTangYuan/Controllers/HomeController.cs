using System;
using Alan.WeChat.CleverTangYuan.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeChat.Core.Messages.Normal;
using WeChat.Core;

namespace Alan.WeChat.CleverTangYuan.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            AliSqlContext context = new AliSqlContext();
            var category = new WeChatCategory { Name = "Todo" };
            context.WeChatCategories.InsertOnSubmit(category);
            context.SubmitChanges();
            return Json(category, JsonRequestBehavior.AllowGet);
        }

    }
}