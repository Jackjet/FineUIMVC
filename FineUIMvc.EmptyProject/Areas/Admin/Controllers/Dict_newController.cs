using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Newtonsoft.Json.Linq;
using System.Collections;
using FineUIMvc.PumpMVC.DAL;
using FineUIMvc.PumpMVC.Models;
using FineUIMvc.PumpMVC.Controllers;

namespace FineUIMvc.PumpMVC.Areas.Admin.Controllers
{
     [Authorize]
    public class Dict_newController : BaseController
    {
        private DBController db = new DBController();
        [MyAuth(MenuPower = "CoreDictItemNew")]
        public ActionResult Index(string dictId)
        {
            ViewBag.hidDictIDTest=dictId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreDictItemNew")]
        public ActionResult btnCreate_Click()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    sys_dictItems dict = new sys_dictItems();
                    dict.FDictID = Convert.ToInt32(Request["hidDictID"]);
                    dict.FName = Request["tbxFName"];
                    dict.FValue = Request["tbxFValue"];
                    dict.FParentValue = Request["tbxFParentValue"];
                    dict.FDescription = Request["tbxFDescription"];
                    db.sys_dictItems.Add(dict);
                    db.SaveChanges();

                    ShowNotify("添加成功！");
                    // 关闭本窗体（触发窗体的关闭事件）
                    PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                }
                catch
                {
                    ShowNotify("添加失败！");
                }

            }

            return UIHelper.Result();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
	}
}