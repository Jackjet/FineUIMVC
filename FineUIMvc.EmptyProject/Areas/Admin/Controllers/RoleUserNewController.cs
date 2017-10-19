using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FineUIMvc.PumpMVC.Controllers;
using FineUIMvc.PumpMVC.DAL;
using FineUIMvc.PumpMVC.Models;
using System.Collections;
using System.Data;
using Newtonsoft.Json.Linq;

namespace FineUIMvc.PumpMVC.Areas.Admin.Controllers
{
    [Authorize]
    public class RoleUserNewController : BaseController
    {
        private DBController db = new DBController();
        //
        // GET: /Admin/RoleUserNew/
        [MyAuth(MenuPower = "CoreRoleUserNew")]
        public ActionResult Index(int roleId)
        {
            DataTable dt = new DataTable();
            int count = 0;
            BindGrid1(0, 20, "", out dt, out count, roleId);
            ViewBag.Grid1DataSource = dt;
            ViewBag.Grid1RecordCount = count;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreRoleUserNew")]
        public ActionResult RoleUserNew_btnSaveClose_Click(int roleId, JArray selectedRowIds)
        {
            try
            {
                foreach (int userID in selectedRowIds)
                {
                    Hashtable hasData = new Hashtable();
                    hasData["RoleID"] = roleId;
                    hasData["UserGH"] = 0;
                    hasData["UserID"] = userID;
                    if (userID.Equals(""))
                    {
                        continue;
                    }
                    sys_rolesDal.InsertRoleUsers(hasData);
                }
                ShowNotify("添加成功");
            }
            catch
            {
                ShowNotify("添加失败");
            }

            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleUserNew_DoPostBack(JArray Grid1_fields, int Grid1_pageIndex,
            string ttbSearchMessage, int ddlGridPageSize, string actionType, int roleId)
        {
            var ttbSearchMessageUI = UIHelper.TwinTriggerBox("ttbSearchMessage");
            if (actionType == "trigger1")
            {
                ttbSearchMessageUI.Text(String.Empty);
                ttbSearchMessageUI.ShowTrigger1(false);

                // 清空传入的搜索值
                ttbSearchMessage = String.Empty;
            }
            else if (actionType == "trigger2")
            {
                ttbSearchMessageUI.ShowTrigger1(true);
            }


            var grid1UI = UIHelper.Grid("Grid1");
            DataTable dt = new DataTable();
            int count = 0;
            BindGrid1(Grid1_pageIndex, ddlGridPageSize, ttbSearchMessage, out dt, out count, roleId);

            grid1UI.DataSource(dt, Grid1_fields, clearSelection: false);
            grid1UI.RecordCount(count);

            return UIHelper.Result();
        }

        private void BindGrid1(int pageIndex, int pageSize, string selectTest, out DataTable table, out int count,int RoleID)
        {
            string sql = string.Empty;
            if (!selectTest.Equals(""))
            {
                sql = sql + " and UserName like '%" + selectTest + "%'";
            }

            if (GetUserType().Equals("3"))  //如果登录用户是客户
            {
                sql = sql + " and a.FCustomerID=" + GetUserCustomer();
            }
            else
            {
                string roleName = db.roles.Where(x => x.ID == RoleID).FirstOrDefault().Name;
                if(roleName.Contains("管理员"))
                {
                    sql = sql + " and UserType=1";
                }
                else if (roleName.Contains("分公司"))
                {
                    sql = sql + " and UserType=2";
                }
                else if (roleName.Contains("客户"))
                {
                    sql = sql + " and UserType=3";
                }
                else if (roleName.Contains("其他"))
                {
                    sql = sql + " and UserType=4";
                }
            }

            Hashtable has = Panda_UserInfoDal.Search(pageIndex, pageSize, "FCreateDate", "DESC", sql);
            count = Int32.Parse(has["total"].ToString());
            table = (DataTable)has["data"];
        }
    }
}