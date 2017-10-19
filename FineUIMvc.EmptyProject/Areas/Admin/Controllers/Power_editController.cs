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
    public class Power_editController : BaseController
    {
        private DBController db = new DBController();
        //
        // GET: /Admin/Power_edit/
        [MyAuth(MenuPower = "CorePowerEdit")]
        public ActionResult Index(int powerId)
        {
            Power power = db.powers.Find(powerId);
            if (power == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.cboIsCustomerLook = power.IsCustomerLook==0?false:true;
            }

            return View(power);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CorePowerEdit")]
        public ActionResult btnEdit_Click(Power power)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                    Power model = db.powers.Find(power.ID);
                    model.GroupName = Request["tbxGroupName"];
                    model.Name = Request["tbxName"];
                    model.Title = Request["tbxTitle"];
                    model.Remark = Request["tbxRemark"];
                    power.IsCustomerLook = Request["cboIsCustomerLook"] == "true" ? 1 : 0;
                    db.SaveChanges();

                    // 关闭本窗体（触发窗体的关闭事件）
                    PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                //}

                ShowNotify("修改成功！");
                // 关闭本窗体（触发窗体的关闭事件）
                PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
            }
            catch
            {
                ShowNotify("修改失败！");
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