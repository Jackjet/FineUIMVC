using FineUIMvc.PumpMVC.Controllers;
using FineUIMvc.PumpMVC.DAL;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FineUIMvc.PumpMVC.Areas.BasicInfo.Controllers
{
    public class WebPortalController : BaseController
    {
        //
        // GET: /BasicInfo/WebPortal/
        public ActionResult Index()
        {
            ViewBag.CoreRoleUserNew = CheckPower("CoreRoleUserNew");
            ViewBag.CoreRoleUserDelete = CheckPower("CoreRoleUserDelete");
            string sql = string.Empty;

            DataTable dt1 = WebPortalDal.SearchTable(sql);
            int Grid1SelectedRowID = 0;
            if (dt1.Rows.Count > 0)
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
        private void BindGrid2(int grid1SelectedRowID, int pageIndex, int pageSize, string selectTest, out DataTable table, out int count)
        {
            int WId = grid1SelectedRowID;
            string sql = string.Empty;
            table = new DataTable();
            count = 0;

            if (WId == -1)
            {
                table = null;
                count = 0;
            }
            else
            {
                sql = sql + " and a.WId=" + WId.ToString();
                Hashtable has = WebPortalDal.SearchWebP_User(pageIndex, pageSize, "b.Name", "ASC", sql);
                count = Int32.Parse(has["total"].ToString());
                table = (DataTable)has["data"];
            }

        }
        public ActionResult RoleUser_Grid2_DoPostBack(JArray Grid1_fields, JArray Grid2_fields, int Grid2_pageIndex,
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
                if (!ttbSearchCustomer.Equals(""))
                {
                    sql1 = " and RType=1 and b.Name='" + ttbSearchCustomer + "'";
                }
                else
                {
                    sql1 = " and RType=0";
                }
                DataTable dt1 = WebPortalDal.SearchTable(sql1);
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
                ttbSearchMessage = "";
            }
            else if (actionType == "delete")
            {
                // 在操作之前进行权限检查
              
                string values = "";
                foreach (int userID in deleteUserIds)
                {
                    values = values + userID.ToString() + ",";
                }
                values = values.Substring(0, values.LastIndexOf(','));
                WebPortalDal.DeletWebP_UserList(Convert.ToInt32(values));
            }

            var grid2UI = UIHelper.Grid("Grid2");
            DataTable dt2 = new DataTable();
            int count = 0;
            BindGrid2(selectedRoleId, Grid2_pageIndex, ddlGridPageSize, ttbSearchMessage, out dt2, out count);
            grid2UI.DataSource(dt2, Grid2_fields);
            grid2UI.RecordCount(count);
            grid2UI.PageSize(ddlGridPageSize);
            return UIHelper.Result();
        }
        public ActionResult WebP_User_new(int WId)
        {
            ViewBag.WId = WId.ToString();
            ViewBag.ddlCompanyDataSource = sys_DeptDal.GetCompanyList("");

            ViewBag.ddlTypeDataSource = sys_dictDal.SearchDDL(" and FDictID=121"); // 用户类型

            ViewBag.SelectValue = "3";
            ViewBag.Hidden = false;


            return View();
        }
        public ActionResult btnCreate_Click()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string name = "";

                    if (Request["ddlType"] == "4")
                    {
                        name = Request["tbxUserID"];
                    }
                    if (Request["ddlType"] == "3")
                    {
                        name = Request["tbxCustomerID"];
                    }
                    if (Request["ddlType"] == "2")
                    {
                        name = Request["ddlCompany"];
                    }
                    if (WebPortalDal.Exist(name, Convert.ToInt32(Request["ddlType"])).Rows.Count == 0)
                    {
                        Hashtable hasData = new Hashtable();
                        hasData["WId"] = Request["tbxWID"];
                        hasData["UserID"] = Request["tbxUserID"];
                        hasData["Type"] = Request["ddlType"];
                        hasData["CustomerID"] = Request["tbxCustomerID"];
                        hasData["DepID"] = Request["ddlCompany"];

                        WebPortalDal.Insert(hasData);

                        ShowNotify("添加成功！");
                        // 关闭本窗体（触发窗体的关闭事件）
                        PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                    }
                    else
                    {
                        ShowNotify("用户名重复，请更换！");
                    }
                }
                catch
                {
                    ShowNotify("添加失败！");
                }

            }

            return UIHelper.Result();
        }
    }
}