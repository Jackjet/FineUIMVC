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
    public class MenusController : BaseController
    {
        private DBController db = new DBController();
        [MyAuth(MenuPower = "CoreMenuView")] 
        public ActionResult Index()
        {
            ViewBag.MajorList = MenuHelper.Menus;
            ViewBag.CoreMenuNew = CheckPower("CoreMenuNew");
            ViewBag.CoreMenuEdit = CheckPower("CoreMenuEdit");
            ViewBag.CoreMenuDelete = CheckPower("CoreMenuDelete");
            return View();
        }

        private void UpdateGrid(JArray Grid1_fields)
        {
            MenuHelper.Reload();
            UIHelper.Grid("Grid1").DataSource(MenuHelper.Menus, Grid1_fields);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Window1_Close(JArray Grid1_fields)
        {
            UpdateGrid(Grid1_fields);
            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grid1_Delete(JArray selectedRows, JArray Grid1_fields)
        {
            foreach (string rowId in selectedRows)
            {
                sys_Menus menu = db.sys_Menus.Find(Convert.ToInt32(rowId));
                db.sys_Menus.Remove(menu);
            }
            db.SaveChanges();

            UpdateGrid(Grid1_fields);

            return UIHelper.Result();
        }
	}
}