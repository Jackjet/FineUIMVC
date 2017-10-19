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
    public class Power_newController : BaseController
    {
         private DBController db = new DBController();
        //
        // GET: /Admin/Power_new/
        [MyAuth(MenuPower = "CorePowerNew")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CorePowerNew")]
        public ActionResult btnCreate_Click()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Power power = new Power();
                    power.GroupName = Request["tbxGroupName"];
                    power.Name = Request["tbxName"];
                    power.Title = Request["tbxTitle"];
                    power.Remark = Request["tbxRemark"];
                    power.IsCustomerLook = Request["cboIsCustomerLook"] == "true" ? 1 : 0;
                    db.powers.Add(power);
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