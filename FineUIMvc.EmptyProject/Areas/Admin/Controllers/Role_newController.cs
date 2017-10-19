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
    public class Role_newController : BaseController
    {
        private DBController db = new DBController();
        [MyAuth(MenuPower = "CoreRoleNew")]
        public ActionResult Index()
        {
            if (GetUserType().Equals("3"))  //如果登录用户是客户
            {
                ViewBag.CustomerName = GetUserCustomerName();
                ViewBag.CustomerID = GetUserCustomer();
                ViewBag.ReadOnly = true;
                ViewBag.SelectValue = "1";
                ViewBag.Hidden = false;
            }
            else
            {
                ViewBag.CustomerName = "";
                ViewBag.CustomerID = "0";
                ViewBag.ReadOnly = false;
                ViewBag.SelectValue = "0";
                ViewBag.Hidden = true;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreRoleNew")]
        public ActionResult btnCreate_Click()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //if (sys_rolesDal.Exist(" and Name='" + Request["tbxName"] + "'").Rows.Count == 0)
                    //{
                        Hashtable has = new Hashtable();
                        has["Name"] = Request["tbxName"];
                        has["Remark"] = Request["tbxRemark"];
                        has["RType"] = Convert.ToInt32(Request["ddlType"]);
                        if (Request["ddlType"].Equals("1"))
                        {
                            has["FCustomerID"] = Convert.ToInt32(Request["tbxCustomerID"]);
                        }
                        else
                        {
                            has["FCustomerID"] = 0;
                        }
                        sys_rolesDal.Insert(has);
                        ShowNotify("添加成功！");
                        // 关闭本窗体（触发窗体的关闭事件）
                        PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                    //}
                    //else
                    //{
                    //    ShowNotify("角色名重复，请更换！");
                    //}
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