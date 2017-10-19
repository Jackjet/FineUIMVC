using FineUIMvc.PumpMVC.Controllers;
using FineUIMvc.PumpMVC.DAL;
using FineUIMvc.PumpMVC.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FineUIMvc.PumpMVC.Areas.Admin.Controllers
{
    [Authorize]
    public class LoginPageController : BaseController
    {
        private DBController db = new DBController();

        #region 登录页列表
        //
        // GET: /BasicInfo/PumpG/
        [MyAuth(MenuPower = "LoginPageView")]
        public ActionResult Index()
        {
            ViewBag.LoginPageNew = CheckPower("LoginPageNew");
            ViewBag.LoginPageDelete = CheckPower("LoginPageDelete");
            ViewBag.LoginPageEdit = CheckPower("LoginPageEdit");

            if (GetUserType().Equals("1"))  //如果登录用户是管理员
            {
                ViewBag.Hidden = false;
            }
            else if (GetUserType().Equals("2"))  //如果登录用户是分公司
            {
                ViewBag.Hidden = true;
            }
            else if (GetUserType().Equals("3"))  //如果登录用户是客户
            {
                ViewBag.Hidden = true;
            }
            else                                //如果登录用户是其他用户（进不去这个页面）
            {
                ViewBag.Hidden = true;
            }

            DataTable dt = new DataTable();
            int count = 0;
            BindGrid1(0, 20, "", "", out dt, out count);
            ViewBag.Grid1DataSource = dt;
            ViewBag.Grid1RecordCount = count;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FZ_Grid1_DoPostBack(JArray Grid1_fields, int Grid1_pageIndex,
            string ttbSearchMessage, string ttbSearchCustomer, string ttbSearchCompany,
            int ddlGridPageSize, string actionType, JArray deleteIds)
        {
            var ttbSearchMessageUI = UIHelper.TwinTriggerBox("ttbSearchMessage");
            string search = string.Empty;
            if (actionType == "trigger1")
            {
                ttbSearchMessageUI.Text(String.Empty);
                ttbSearchMessageUI.ShowTrigger1(false);
                ttbSearchMessage = String.Empty;
                search = ttbSearchMessage;
            }

            else if (actionType == "trigger2")
            {
                ttbSearchMessageUI.ShowTrigger1(true);
                search = ttbSearchMessage;
            }
            else if (actionType == "delete")
            {
                string values = "";
                foreach (int ID in deleteIds)
                {
                    values = values + ID.ToString() + ",";
                }
                values = values.Substring(0, values.LastIndexOf(','));
                Hashtable hasData = new Hashtable();
                hasData["ID"] = values;
                hasData["FIsDelete"] = 1;
                hasData["FUpdateDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ssss"));
                hasData["FUpdateUser"] = GetIdentityName();
                LoginPageDal.DeleteList(hasData);
            }

            var grid1UI = UIHelper.Grid("Grid1");
            DataTable dt2 = new DataTable();
            int count = 0;
            BindGrid1(Grid1_pageIndex, ddlGridPageSize, search, actionType, out dt2, out count);
            grid1UI.DataSource(dt2, Grid1_fields);
            grid1UI.RecordCount(count);
            grid1UI.PageSize(ddlGridPageSize);
            return UIHelper.Result();
        }

        private void BindGrid1(int pageIndex, int pageSize, string selectTest, string actionType, out DataTable table, out int count)
        {
            string sql = string.Empty;
            if (!selectTest.Equals(""))
            {
                if (actionType == "trigger2")
                {
                    sql = sql + " and Host like '%" + selectTest + "%'";
                }

            }
            Hashtable has = LoginPageDal.Search(pageIndex, pageSize, "FCreateDate", "asc", sql);
            count = Int32.Parse(has["total"].ToString());
            table = (DataTable)has["data"];
        }
        #endregion

        #region 新增登录页
        [MyAuth(MenuPower = "LoginPageNew")]
        public ActionResult LoginPage_New()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult btnLoginPageCreate_Click()
        {
            if (ModelState.IsValid)
            {
                if (LoginPageDal.Exist(" and Host='" + Request["tbxHost"] + "'").Rows.Count == 0)
                {
                    try
                    {
                        Hashtable hasData = new Hashtable();
                        hasData["FCreateUser"] = Convert.ToInt32(GetIdentityName());
                        hasData["FCreateDate"] = DateTime.Now;
                        hasData["FIsDelete"] = 0;
                        hasData["Host"] = Request["tbxHost"];
                        hasData["FileName"] = Request["tbxFileName"];
                        hasData["Title"] = Request["tbxTitle"];
                        LoginPageDal.Insert(hasData);
                        ShowNotify("添加成功！");
                        // 关闭本窗体（触发窗体的关闭事件）
                        PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                    }
                    catch
                    {
                        ShowNotify("添加失败！");
                    }
                }
                else
                {
                    ShowNotify("域名重复，请更换！");
                }

            }

            return UIHelper.Result();
        }
        #endregion

        #region 修改登录页
        [MyAuth(MenuPower = "LoginPageEdit")]
        public ActionResult LoginPage_Edit(int ID)
        {
            LoginPage fg = db.LoginPage.Find(ID);
            if (fg == null)
            {
                return HttpNotFound();
            }
            else
            {
            }

            return View(fg);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult btnLoginPageEdit_Click()
        {
            try
            {
                if (ModelState.IsValid)
                {

                    Hashtable hasData = new Hashtable();
                    hasData["ID"] = Request["tbxID"];
                    hasData["Host"] = Request["tbxHost"];
                    hasData["FileName"] = Request["tbxFileName"];
                    hasData["Title"] = Request["tbxTitle"];
                    hasData["FUpdateUser"] = GetIdentityName();
                    hasData["FUpdateDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    LoginPageDal.Update(hasData);

                    ShowNotify("成功！");
                    // 关闭本窗体（触发窗体的关闭事件）
                    PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                    return UIHelper.Result();

                }
            }
            catch
            {
                ShowNotify("失败！");
            }
            return UIHelper.Result();
        }
        #endregion
    }
}