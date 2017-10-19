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
    public class DictsController : BaseController
    {
        private DBController db = new DBController();
        //
        // GET: /Admin/Dicts/
        [MyAuth(MenuPower = "CoreDictView")] 
        public ActionResult Index()
        {
            ViewBag.CoreDictItemNew = CheckPower("CoreDictItemNew");
            ViewBag.CoreDictItemDelete = CheckPower("CoreDictItemDelete");
            ViewBag.Grid1DataSource = sys_dictDal.SearchDict("");
            ViewBag.Grid2DataSource = null;
            ViewBag.Grid2RecordCount = 0;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grid1_RowSelect(string selectedRowId, int gridIndex, int gridPageSize, JArray Grid2_fields)
        {
            string sql = string.Empty;

            sql = sql + " and FDictID = " + selectedRowId;
            Hashtable table = sys_dictDal.SearchDictItem(gridIndex, gridPageSize, "FValue", "DESC", sql);
            UIHelper.Grid("Grid2").DataSource(table["data"], Grid2_fields);
            UIHelper.Grid("Grid2").RecordCount(Int32.Parse(table["total"].ToString()));
            return UIHelper.Result();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grid2_PageIndexChanged(string selectedRowId, JArray Grid2_fields, int Grid2_pageIndex, int gridPageSize)
        {
            var grid2 = UIHelper.Grid("Grid2");

            if (selectedRowId.Equals("-1"))
            {
                ViewBag.Grid2DataSource = null;
                ViewBag.Grid2RecordCount = 0;
                grid2.DataSource(null, Grid2_fields);
                grid2.RecordCount(0);
            }
            else
            {
                string sql = string.Empty;

                sql = sql + " and FDictID = " + selectedRowId;
                Hashtable table = sys_dictDal.SearchDictItem(Grid2_pageIndex, gridPageSize, "FValue", "DESC", sql);
                grid2.DataSource(table["data"], Grid2_fields);
                grid2.RecordCount(Int32.Parse(table["total"].ToString()));
            }

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreDictItemDelete")]
        public ActionResult Grid2_Delete(JArray selectedRows, JArray Grid2_fields, int gridIndex, int gridPageSize, string Grid1_selectedRows)
        {
            foreach (string rowId in selectedRows)
            {
                sys_dictItems item = db.sys_dictItems.Find(Convert.ToInt32(rowId));
                db.sys_dictItems.Remove(item);
            }
            db.SaveChanges();

            UpdateGrid(Grid2_fields, gridIndex, gridPageSize, Grid1_selectedRows);

            return UIHelper.Result();
        }

        private void UpdateGrid(JArray Grid2_fields, int gridIndex, int gridPageSize, string Grid1_selectedRows)
        {
            var grid2 = UIHelper.Grid("Grid2");
            string sql = string.Empty;

            sql = sql + " and FDictID = " + Grid1_selectedRows;
            Hashtable table = sys_dictDal.SearchDictItem(gridIndex, gridPageSize, "FValue", "DESC", sql);
            grid2.DataSource(table["data"], Grid2_fields);
            grid2.RecordCount(Int32.Parse(table["total"].ToString()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ddlGridPageSize_SelectedIndexChanged(string ddlGridPageSize, string ddlGridPageSize_text, int gridIndex, string selectedRowId, JArray Grid2_fields)
        {
            var grid2 = UIHelper.Grid("Grid2");
            string sql = string.Empty;

            sql = sql + " and FDictID = " + selectedRowId;
            Hashtable table = sys_dictDal.SearchDictItem(gridIndex, int.Parse(ddlGridPageSize), "FValue", "DESC", sql);
            grid2.DataSource(table["data"], Grid2_fields);
            grid2.RecordCount(Int32.Parse(table["total"].ToString()));
            grid2.PageSize(int.Parse(ddlGridPageSize));
            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Window1_Close(JArray Grid2_fields, int gridIndex, int gridPageSize, string Grid1_selectedRows)
        {
            UpdateGrid(Grid2_fields, gridIndex, gridPageSize, Grid1_selectedRows);
            return UIHelper.Result();
        }
	}
}