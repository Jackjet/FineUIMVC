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

namespace FineUIMvc.PumpMVC.Areas.OpenWindow.Controllers
{
    [Authorize]
    public class AddressSchemeWindowController : Controller
    {
        //
        // GET: /OpenWindow/AddressSchemeWindow/
        [MyAuth(MenuPower = "CoreAddressSchemeView")]
        public ActionResult Index(string id)
        {
            ViewBag.FType = id;
            Hashtable table = AddressSchemeDal.Search(0, 20, "FName", "ASC", " and FType='"+id+"'");
            ViewBag.Grid1DataSource = table["data"];
            ViewBag.Grid1RecordCount = Int32.Parse(table["total"].ToString());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grid1_PageIndexChanged(JArray Grid1_fields, int Grid1_pageIndex, int gridPageSize, string searchMessage,string FType)
        {
            var Grid1 = UIHelper.Grid("Grid1");

            string sql = " and FType='" + FType + "'";

            if (!searchMessage.Equals(""))
            {
                sql = sql + " and FName like '%" + searchMessage + "%'";
            }
            Hashtable table = AddressSchemeDal.Search(Grid1_pageIndex, gridPageSize, "FName", "ASC", sql);
            Grid1.DataSource(table["data"], Grid1_fields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));

            return UIHelper.Result();
        }

        private void UpdateGrid(JArray Grid1_fields, int gridIndex, int gridPageSize, string FType)
        {
            var Grid1 = UIHelper.Grid("Grid1");
            string sql = " and FType='" + FType + "'";

            Hashtable table = AddressSchemeDal.Search(gridIndex, gridPageSize, "FName", "ASC", sql);
            Grid1.DataSource(table["data"], Grid1_fields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyCustomPostBack(string type, JArray gridFields, JObject typeParams, int gridIndex, int gridPageSize, string FType)
        {
            var Grid1 = UIHelper.Grid("Grid1");
            string sql = " and FType='" + FType + "'";
            var ttbSearch = UIHelper.TwinTriggerBox("ttbSearchMessage");
            if (type == "trigger1")
            {
                ttbSearch.Text(" ");
                ttbSearch.ShowTrigger1(false);
            }
            else if (type == "trigger2")
            {
                ttbSearch.ShowTrigger1(true);
                var triggerValue = typeParams.Value<string>("triggerValue");
                sql = " and FName like '%" + triggerValue + "%'";
            }

            Hashtable table = AddressSchemeDal.Search(gridIndex, gridPageSize, "FName", "ASC", sql);
            Grid1.DataSource(table["data"], gridFields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));
            Grid1.PageSize(gridPageSize);
            return UIHelper.Result();
        }
	}
}