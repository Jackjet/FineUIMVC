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

namespace FineUIMvc.PumpMVC.Areas.OpenWindow.Controllers
{
      [Authorize]
    public class UserWindowController : BaseController
    {
        //
        // GET: /OpenWindow/UserWindow/
          public ActionResult Index(string type)
        {
            string sql = string.Empty;

            DataTable dt = new DataTable();
            int count = 0;
            BindGrid1(0, 20, "", "",type, out dt, out count);
            ViewBag.Grid1DataSource = dt;
            ViewBag.Grid1RecordCount = count;
            ViewBag.type = type;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FG_Grid1_DoPostBack(JArray Grid1_fields, int Grid1_pageIndex,
            string ttbSearchMessage, string ttbSearchCustomer, string ttbSearchCompany, int ddlGridPageSize, string actionType, string type)
        {
            var ttbSearchMessageUI = UIHelper.TwinTriggerBox("ttbSearchMessage");
            var ttbSearchCustomerUI = UIHelper.TwinTriggerBox("ttbSearchCustomer");
            var ttbSearchCompanyUI = UIHelper.TwinTriggerBox("ttbSearchCompany");
            string search = string.Empty;
            if (actionType == "trigger1")
            {
                ttbSearchMessageUI.Text(String.Empty);
                ttbSearchMessageUI.ShowTrigger1(false);
                ttbSearchMessage = String.Empty;

                ttbSearchCustomerUI.Text(String.Empty);
                ttbSearchCustomerUI.ShowTrigger1(false);
                ttbSearchCustomer = String.Empty;

                ttbSearchCompanyUI.Text(String.Empty);
                ttbSearchCompanyUI.ShowTrigger1(false);
                ttbSearchCompany = String.Empty;
            }
            else if (actionType == "trigger2")
            {
                ttbSearchMessageUI.ShowTrigger1(true);
                ttbSearchCustomerUI.Text(String.Empty);
                ttbSearchCustomerUI.ShowTrigger1(false);
                ttbSearchCustomer = String.Empty;
                ttbSearchCompanyUI.Text(String.Empty);
                ttbSearchCompanyUI.ShowTrigger1(false);
                ttbSearchCompany = String.Empty;
                search = ttbSearchMessage;
            }
            else if (actionType == "trigger3")
            {
                ttbSearchCustomerUI.ShowTrigger1(true);
                ttbSearchMessageUI.Text(String.Empty);
                ttbSearchMessageUI.ShowTrigger1(false);
                ttbSearchMessage = String.Empty;
                ttbSearchCompanyUI.Text(String.Empty);
                ttbSearchCompanyUI.ShowTrigger1(false);
                ttbSearchCompany = String.Empty;
                search = ttbSearchCustomer;
            }
            else if (actionType == "trigger4")
            {
                ttbSearchCompanyUI.ShowTrigger1(true);
                ttbSearchCustomerUI.Text(String.Empty);
                ttbSearchCustomerUI.ShowTrigger1(false);
                ttbSearchCustomer = String.Empty;
                ttbSearchMessageUI.Text(String.Empty);
                ttbSearchMessageUI.ShowTrigger1(false);
                ttbSearchMessage = "";
                search = ttbSearchCompany;
            }

            var grid1UI = UIHelper.Grid("Grid1");
            DataTable dt2 = new DataTable();
            int count = 0;
            BindGrid1(Grid1_pageIndex, ddlGridPageSize, search, actionType,type, out dt2, out count);
            grid1UI.DataSource(dt2, Grid1_fields);
            grid1UI.RecordCount(count);
            grid1UI.PageSize(ddlGridPageSize);
            return UIHelper.Result();
        }

        private void BindGrid1(int pageIndex, int pageSize, string selectTest, string actionType,string type, out DataTable table, out int count)
        {
            string sql = string.Empty;
            if (!selectTest.Equals(""))
            {
                if (actionType == "trigger2")
                {
                    sql = sql + " and a.UserName like '%" + selectTest + "%'";
                }
                else if (actionType == "trigger3")
                {
                    sql = sql + " and e.Name like '%" + selectTest + "%'";
                }
                else if (actionType == "trigger4")
                {
                    sql = sql + " and b.Name like '%" + selectTest + "'%";
                }
            }

            switch (type)
            {
                case "0": sql = sql + " and a.UserType = '1'"; break;//管理员
                case "1": sql = sql + " and a.UserType = '2'"; break;//分公司
                case "2": sql = sql + " and a.UserType = '3'"; break;//客户
            }

            Hashtable has = Panda_UserInfoDal.Search(pageIndex, pageSize, "ID", "asc", sql);
            count = Int32.Parse(has["total"].ToString());
            table = (DataTable)has["data"];
        }
	}
}