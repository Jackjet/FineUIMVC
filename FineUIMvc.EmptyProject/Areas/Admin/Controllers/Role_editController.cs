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
    public class Role_editController : BaseController
    {
        private DBController db = new DBController();
        //
        // GET: /Admin/Power_edit/
        [MyAuth(MenuPower = "CoreRoleEdit")]
        public ActionResult Index(int roleId)
        {
            if (GetUserType().Equals("3"))  //如果登录用户是客户
            {
                ViewBag.ReadOnly = true;
            }
            else
            {
                ViewBag.ReadOnly = false;
            }

            Role role = db.roles.Find(roleId);
            if (role == null)
            {
                return HttpNotFound();
            }
            ViewBag.tbSelectedCustomer = role.Panda_Customer == null ? "" : role.Panda_Customer.Name;
            int customerid = role.Panda_Customer == null ? 0 : role.Panda_Customer.ID;
            ViewBag.hidSelectedCustomer = customerid.ToString();
            ViewBag.RType = role.RType.ToString();
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreRoleEdit")]
        public ActionResult btnEdit_Click(Role role)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                //if (sys_rolesDal.Exist(" and Name='" + Request["tbxName"] + "' and Name<>'" + Request["tbxOldName"] + "'").Rows.Count == 0)
                //{
                    Role model = db.roles.Find(role.ID);
                    Hashtable has = new Hashtable();
                    has["ID"] = model.ID;
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
                    sys_rolesDal.Update(has);

                    ShowNotify("修改成功！");
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