using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FineUIMvc.PumpMVC.Controllers;
using FineUIMvc.PumpMVC.DAL;
using FineUIMvc.PumpMVC.Models;
using System.Collections;
using Newtonsoft.Json.Linq;

namespace FineUIMvc.PumpMVC.Areas.Admin.Controllers
{
    [Authorize]
    public class RolesController : BaseController
    {
        private DBController db = new DBController();
        //
        // GET: /Admin/Powers/
        [MyAuth(MenuPower = "CoreRoleView")]
        public ActionResult Index()
        {
            ViewBag.CoreRoleNew = CheckPower("CoreRoleNew");
            ViewBag.CoreRoleEdit = CheckPower("CoreRoleEdit");
            ViewBag.CoreRoleDelete = CheckPower("CoreRoleDelete");
            string sql = string.Empty;
            if (GetUserType().Equals("3"))  //如果登录用户是客户
            {
                sql = sql + " and RType=1 and FCustomerID=" + GetUserCustomer();
                ViewBag.Hidden = true;
                ViewBag.ReadOnly = true;
                ViewBag.SelectValue = "1";
            }
            else
            {
                sql = sql + " and RType=0";
                ViewBag.Hidden = false;
                ViewBag.ReadOnly = false;
                ViewBag.SelectValue = "0";
            }
            Hashtable table = sys_rolesDal.Search(0, 20, "a.Name", "ASC", sql);
            ViewBag.Grid1DataSource = table["data"];
            ViewBag.Grid1RecordCount = Int32.Parse(table["total"].ToString());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grid1_PageIndexChanged(JArray Grid1_fields, int Grid1_pageIndex, int gridPageSize, string searchMessage, string searchCustomer)
        {
            var Grid1 = UIHelper.Grid("Grid1");

            string sql = string.Empty;

            if (GetUserType().Equals("3"))  //如果登录用户是客户
            {
                sql = sql + " and RType=1 and FCustomerID=" + GetUserCustomer();
            }
            else
            {
                sql = sql + " and RType=0";
            }
            if (!searchMessage.Equals(""))
            {
                sql = sql + " and a.Name like '%" + searchMessage + "%'";
            }
            if(!searchCustomer.Equals(""))
            {
                sql = sql + " and b.Name = '" + searchCustomer + "'";
            }
            Hashtable table = sys_rolesDal.Search(Grid1_pageIndex, gridPageSize, "a.Name", "DESC", sql);
            Grid1.DataSource(table["data"], Grid1_fields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grid1_Delete(JArray selectedRows, JArray Grid1_fields, int gridIndex, int gridPageSize)
        {
            foreach (string rowId in selectedRows)
            {
                Role item = db.roles.Find(Convert.ToInt32(rowId));
                db.roles.Remove(item);
            }
            db.SaveChanges();

            UpdateGrid(Grid1_fields, gridIndex, gridPageSize);

            return UIHelper.Result();
        }

        private void UpdateGrid(JArray Grid1_fields, int gridIndex, int gridPageSize)
        {
            var Grid1 = UIHelper.Grid("Grid1");
            string sql = string.Empty;

            //sql = sql + " and FDictID = " + Grid1_selectedRows;
            if (GetUserType().Equals("3"))  //如果登录用户是客户
            {
                sql = sql + " and RType=1 and FCustomerID=" + GetUserCustomer();
            }
            Hashtable table = sys_rolesDal.Search(gridIndex, gridPageSize, "a.Name", "DESC", sql);
            Grid1.DataSource(table["data"], Grid1_fields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyCustomPostBack(string type, JArray gridFields, JObject typeParams, int gridIndex, int gridPageSize)
        {
            var Grid1 = UIHelper.Grid("Grid1");
            string sql = string.Empty;
            var ttbSearch = UIHelper.TwinTriggerBox("ttbSearchMessage");
            var ttbSearchCus = UIHelper.TwinTriggerBox("ttbSearchCustomer");
            if (type == "trigger1")
            {
                ttbSearch.Text(String.Empty);
                ttbSearch.ShowTrigger1(false);
                ttbSearchCus.Text(String.Empty);
                ttbSearchCus.ShowTrigger1(false);
            }
            else if (type == "trigger2")
            {
                ttbSearch.ShowTrigger1(true);
                ttbSearchCus.Text(String.Empty);
                ttbSearchCus.ShowTrigger1(false);
                var triggerValue = typeParams.Value<string>("triggerValue");
                sql = " and a.Name like '%" + triggerValue + "%'";
            }
            else if (type == "trigger3")
            {
                ttbSearch.Text(String.Empty);
                ttbSearch.ShowTrigger1(false);
                ttbSearchCus.ShowTrigger1(true);
                var triggerCusValue = typeParams.Value<string>("triggerValue");
                sql = " and b.Name = '" + triggerCusValue + "'";
            }
            else if (type == "ddlType")
            {
                ttbSearch.ShowTrigger1(false);
                ttbSearch.Text(String.Empty);
                ttbSearchCus.Text(String.Empty);
                ttbSearchCus.ShowTrigger1(false);
                var ddlType = typeParams.Value<string>("ddlType");
                sql = sql + " and RType='" + ddlType + "'";
            }
            if (GetUserType().Equals("3"))  //如果登录用户是客户
            {
                sql = sql + " and RType=1 and FCustomerID=" + GetUserCustomer();
            }
            Hashtable table = sys_rolesDal.Search(gridIndex, gridPageSize, "a.Name", "DESC", sql);
            Grid1.DataSource(table["data"], gridFields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));
            Grid1.PageSize(gridPageSize);
            return UIHelper.Result();
        }
	}
}