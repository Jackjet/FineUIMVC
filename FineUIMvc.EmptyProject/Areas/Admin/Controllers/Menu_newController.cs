using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Data.Entity;
using Newtonsoft.Json.Linq;
using System.Collections;
using FineUIMvc.PumpMVC.DAL;
using FineUIMvc.PumpMVC.Models;
using FineUIMvc.PumpMVC.Controllers;

namespace FineUIMvc.PumpMVC.Areas.Admin.Controllers
{
     [Authorize]
    public class Menu_newController : BaseController
    {
        private DBController db = new DBController();
         [MyAuth(MenuPower = "CoreMenuNew")]
        public ActionResult Index()
        {
            ViewBag.ddlParentDataSource = ResolveDDL<Menus>(MenuHelper.Menus);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreMenuNew")]
        public ActionResult btnCreate_Click()
        {
            try
            {
                string fname = Request["tbxName"];
                int ParentID = Convert.ToInt32(Request["ddlParent"]);
                int SortIndex = Convert.ToInt32(Request["tbxSortIndex"]);
                Hashtable hasDataID = new Hashtable();
                hasDataID = Sys_PowersDal.Get(Request["tbxViewPower"].ToString(), "A");
                int ViewPowerID = 0;
                if (hasDataID != null)
                {
                    ViewPowerID = Convert.ToInt32(hasDataID["FID"].ToString());
                }
                string Url = Request["tbxUrl"];
                string Icon = Request["tbxIcon"];
                string Remark = Request["tbxRemark"];

                sys_Menus model = new sys_Menus();
                model.Name = fname;
                model.ParentID = ParentID == -1 ? 0 : ParentID;
                model.SortIndex = SortIndex;
                model.ViewPowerID = ViewPowerID;
                model.ImageUrl = Icon;
                model.NavigateUrl = Url;
                model.Remark = Remark;
                db.sys_Menus.Add(model);
                db.SaveChanges();

                ShowNotify("添加成功！");
                // 关闭本窗体（触发窗体的关闭事件）
                PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
            }
            catch
            {
                ShowNotify("添加失败！");
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