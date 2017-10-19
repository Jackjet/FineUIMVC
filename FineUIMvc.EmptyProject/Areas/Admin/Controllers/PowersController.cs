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
    public class PowersController : BaseController
    {
        private DBController db = new DBController();
        //
        // GET: /Admin/Powers/
        [MyAuth(MenuPower = "CorePowerView")]
        public ActionResult Index()
        {
            ViewBag.CorePowerNew = CheckPower("CorePowerNew");
            ViewBag.CorePowerEdit = CheckPower("CorePowerEdit");
            ViewBag.CorePowerDelete = CheckPower("CorePowerDelete");
            Hashtable table = Sys_PowersDal.Search(0, 20, "Name", "DESC", "");
            ViewBag.Grid1DataSource = table["data"];
            ViewBag.Grid1RecordCount = Int32.Parse(table["total"].ToString());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grid1_PageIndexChanged(JArray Grid1_fields, int Grid1_pageIndex, int gridPageSize)
        {
            var Grid1 = UIHelper.Grid("Grid1");

            string sql = string.Empty;

            //sql = sql + " and FDictID = " + selectedRowId;
            Hashtable table = Sys_PowersDal.Search(Grid1_pageIndex, gridPageSize, "Name", "DESC", sql);
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
                Power item = db.powers.Find(Convert.ToInt32(rowId));
                db.powers.Remove(item);
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
            Hashtable table = Sys_PowersDal.Search(gridIndex, gridPageSize, "Name", "DESC", sql);
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
            if (type == "trigger1")
            {
                ttbSearch.Text(String.Empty);
                ttbSearch.ShowTrigger1(false);
            }
            else if (type == "trigger2")
            {
                ttbSearch.ShowTrigger1(true);
                var triggerValue = typeParams.Value<string>("triggerValue");
                sql = " and (Name like '%" + triggerValue + "%' or Title like '%" + triggerValue + "%')";
            }

            Hashtable table = Sys_PowersDal.Search(gridIndex, gridPageSize, "Name", "DESC", sql);
            Grid1.DataSource(table["data"], gridFields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));
            Grid1.PageSize(gridPageSize);
            return UIHelper.Result();
        }
    }
}