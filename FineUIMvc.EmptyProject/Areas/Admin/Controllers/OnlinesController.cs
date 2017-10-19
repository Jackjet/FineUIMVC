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
using System.Data.Entity;
using Newtonsoft.Json.Linq;

namespace FineUIMvc.PumpMVC.Areas.Admin.Controllers
{
    public class OnlinesController : BaseController
    {
        //
        // GET: /Admin/Onlines/
         [MyAuth(MenuPower = "CoreOnlineView")]
        public ActionResult Index()
        {
            DataTable dt = new DataTable();
            int count = 0;
            BindGrid(0, 20, "",  out dt, out count);
            ViewBag.Grid1DataSource = dt;
            ViewBag.Grid1RecordCount = count;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Online_DoPostBack(JArray Grid1_fields, int Grid1_pageIndex, 
            string ttbSearchMessage, int ddlGridPageSize, string actionType)
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
            DataTable dt2 = new DataTable();
            int count = 0;
            BindGrid(Grid1_pageIndex, ddlGridPageSize, ttbSearchMessage,  out dt2, out count);
            grid1UI.DataSource(dt2, Grid1_fields);
            grid1UI.RecordCount(count);

            return UIHelper.Result();
        }

        private void BindGrid(int pageIndex, int pageSize, string selectTest,  out DataTable table, out int count)
        {
            string sql = string.Empty;
            if (!selectTest.Equals(""))
            {
                sql = sql + " and UserName like '%" + selectTest + "%'";
            }

            if (GetUserType().Equals("2"))
            {
                sql = sql + " and FCompanyNumber='" + GetUserCompanyNumber() + "'";
            }
            else if (GetUserType().Equals("3"))
            {
                sql = sql + " and FCustomerID=" + GetUserCustomer();
            }

            Hashtable has = Sys_OnlineDal.Search(pageIndex, pageSize, "UpdateTime", "desc", sql);
            count = Int32.Parse(has["total"].ToString());
            table = (DataTable)has["data"];
        }
	}
}