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
    public class Menu_editController : BaseController
    {
        private DBController db = new DBController();

        // GET: Admin/Menu_edit/?menuId=5
        [MyAuth(MenuPower = "CoreMenuEdit")]
        public ActionResult Index(int menuId)
        {
            ViewBag.ddlParentDataSource = ResolveDDL<Menus>(MenuHelper.Menus);
            ViewBag.ddlPowerDataSource = db.powers;
            sys_Menus menu = db.sys_Menus.Find(menuId);
            if (menu == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.ddlParentSelect = menu.ParentID.ToString() == "0" ? "-1" : menu.ParentID.ToString();
                ViewBag.ddlPowerSelect = menu.ViewPowerID.ToString();
            }

            return View(menu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreMenuEdit")]
        public ActionResult btnEdit_Click([Bind(Include = "ID,Name,ParentID,SortIndex,ViewPowerID,NavigateUrl,ImageUrl,Remark")] sys_Menus menu)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    sys_Menus model = db.sys_Menus.Find(menu.ID);

                    string fname = Request["tbxName"];
                    int ParentID = Convert.ToInt32(Request["ddlParent"]);
                    int SortIndex = Convert.ToInt32(Request["tbxSortIndex"]);
                    int ViewPowerID = Convert.ToInt32(Request["ddlPower"]);
                    //Hashtable hasDataID = new Hashtable();
                    //hasDataID = Sys_PowersDal.Get(Request["tbxViewPower"].ToString(), "A");
                    //int ViewPowerID = 0;
                    //if (hasDataID != null)
                    //{
                    //    ViewPowerID = Convert.ToInt32(hasDataID["FID"].ToString());
                    //}
                    string Url = Request["tbxUrl"];
                    string Icon = Request["tbxIcon"];
                    string Remark = Request["tbxRemark"];

                    model.Name = fname;
                    model.ParentID = ParentID == -1 ? 0 : ParentID;
                    model.SortIndex = SortIndex;
                    model.ViewPowerID = ViewPowerID;
                    model.ImageUrl = Icon;
                    model.NavigateUrl = Url;
                    model.Remark = Remark;
                    db.SaveChanges();
                    //db.Entry(menu).State = EntityState.Modified;
                    //db.SaveChanges();

                    // 关闭本窗体（触发窗体的关闭事件）
                    PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                }

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