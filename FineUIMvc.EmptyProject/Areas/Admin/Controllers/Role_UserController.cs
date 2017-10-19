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
    public class Role_UserController : BaseController
    {
        //
        // GET: /Admin/Role_User/
        [MyAuth(MenuPower = "CoreRoleUserView")]
        public ActionResult Index()
        {
            ViewBag.CoreRoleUserNew = CheckPower("CoreRoleUserNew");
            ViewBag.CoreRoleUserDelete = CheckPower("CoreRoleUserDelete");
            string sql = string.Empty;
            if (GetUserType().Equals("3"))  //如果登录用户是客户
            {
                sql = sql + " and RType=1 and FCustomerID=" + GetUserCustomer();
                ViewBag.Hidden = true;
            }
            else
            {
                sql = sql + " and RType=0 ";
                ViewBag.Hidden = false;
            }
            DataTable dt1 = sys_rolesDal.SearchTable(sql);
            int Grid1SelectedRowID = 0;
            if(dt1.Rows.Count>0)
            {
                ViewBag.Grid1DataSource = dt1;
                ViewBag.Grid1SelectedRowID = dt1.Rows[0]["ID"].ToString();
                Grid1SelectedRowID = Convert.ToInt32(dt1.Rows[0]["ID"].ToString());
            }
            else
            {
                ViewBag.Grid1DataSource = null;
                ViewBag.Grid1SelectedRowID = 0;
                Grid1SelectedRowID = 0;
            }
            DataTable dt2 = new DataTable();
            int count = 0;
            BindGrid2(Grid1SelectedRowID, 0, 20, "", out dt2, out count);
            ViewBag.Grid2DataSource = dt2;
            ViewBag.Grid2RecordCount = count;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleUser_Grid2_DoPostBack(JArray Grid1_fields,JArray Grid2_fields, int Grid2_pageIndex,
            string ttbSearchMessage, string ttbSearchCustomer, int ddlGridPageSize, string actionType, int selectedRoleId, JArray deleteUserIds)
        {
            var ttbSearchMessageUI = UIHelper.TwinTriggerBox("ttbSearchMessage");
            var ttbSearchCustomerUI = UIHelper.TwinTriggerBox("ttbSearchCustomer");
            if (actionType == "trigger1")
            {
                ttbSearchMessageUI.Text(String.Empty);
                ttbSearchMessageUI.ShowTrigger1(false);
                ttbSearchMessage = String.Empty;

                ttbSearchCustomerUI.Text(String.Empty);
                ttbSearchCustomerUI.ShowTrigger1(false);
                ttbSearchCustomer = String.Empty;

                DataTable dt1 = sys_rolesDal.SearchTable(" and RType=0");
                if (dt1.Rows.Count > 0)
                {
                    selectedRoleId = Convert.ToInt32(dt1.Rows[0]["ID"].ToString());
                }
                else
                {
                    selectedRoleId = 0;
                }
                var grid1UI = UIHelper.Grid("Grid1");
                grid1UI.DataSource(dt1, Grid1_fields);
            }
            else if (actionType == "trigger2")
            {
                ttbSearchMessageUI.ShowTrigger1(true);
                ttbSearchCustomerUI.Text(String.Empty);
                ttbSearchCustomerUI.ShowTrigger1(false);
            }
            else if (actionType == "trigger3")
            {
                ttbSearchCustomerUI.ShowTrigger1(true);
                ttbSearchMessageUI.Text(String.Empty);
                ttbSearchMessageUI.ShowTrigger1(false);
                string sql1 = string.Empty;
                if(!ttbSearchCustomer.Equals(""))
                {
                    sql1 = " and RType=1 and b.Name='" + ttbSearchCustomer + "'";
                }
                else
                {
                    sql1 = " and RType=0";
                }
                DataTable dt1 = sys_rolesDal.SearchTable(sql1);
                if (dt1.Rows.Count>0)
                {
                    selectedRoleId = Convert.ToInt32(dt1.Rows[0]["ID"].ToString());
                }
                else
                {
                    selectedRoleId = 0;
                }
                var grid1UI = UIHelper.Grid("Grid1");
                grid1UI.DataSource(dt1, Grid1_fields);
                ttbSearchMessage = "";
            }
            else if (actionType == "delete")
            {
                // 在操作之前进行权限检查
                if (!CheckPower("CoreRoleUserDelete"))
                {
                    CheckPowerFailWithAlert();
                    return UIHelper.Result();
                }

                string values = "";
                foreach (int userID in deleteUserIds)
                {
                    values = values+userID.ToString()+",";
                }
                values = values.Substring(0, values.LastIndexOf(','));
                sys_rolesDal.DeleteRoleUsersList(values, selectedRoleId.ToString());
            }

            var grid2UI = UIHelper.Grid("Grid2");
            DataTable dt2 = new DataTable();
            int count = 0;
            BindGrid2(selectedRoleId, Grid2_pageIndex, ddlGridPageSize, ttbSearchMessage,out dt2, out count);
            grid2UI.DataSource(dt2, Grid2_fields);
            grid2UI.RecordCount(count);
            grid2UI.PageSize(ddlGridPageSize);
            return UIHelper.Result();
        }

        private void BindGrid2(int grid1SelectedRowID, int pageIndex, int pageSize, string selectTest, out DataTable table, out int count)
        {
            int roleID = grid1SelectedRowID;
            string sql = string.Empty;
            table = new DataTable();
            count = 0;

            if (roleID == -1)
            {
                table = null;
                count = 0;
            }
            else
            {
                sql = sql + " and RoleID=" + roleID.ToString();
                if (!selectTest.Equals(""))
                {
                    sql = sql + " and b.UserName like '%" + selectTest + "%'";
                }
                if (GetUserType().Equals("3"))  //如果登录用户是客户
                {
                    sql = sql + " and b.FCustomerID=" + GetUserCustomer();
                }

                Hashtable has = Panda_UserInfoDal.SearchRoleUsers(pageIndex, pageSize, "a.UserID", "DESC", sql);
                count = Int32.Parse(has["total"].ToString());
                table = (DataTable)has["data"];
            }

        }
    }
}