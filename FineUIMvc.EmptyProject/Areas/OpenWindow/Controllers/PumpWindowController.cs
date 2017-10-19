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
    public class PumpWindowController : BaseController
    {
        //
        // GET: /OpenWindow/PumpWindow/
         [MyAuth(MenuPower = "CorePumpView")]
        public ActionResult Index()
        {
            if (Request["type"].ToString().Equals("mult"))
            {
                ViewBag.Grid1Mult = true;
            }
            else
            {
                ViewBag.Grid1Mult = false;
            }
            string sql = string.Empty;
            string page = Request["page"];
            ViewBag.pageSource = page;
             switch(page)
             {
                 case "pumpG": sql = sql + " and a.ID not in (select pumpID from Panda_PumpFG a,Panda_PumpFG_P b where a.ID=b.GroupID and a.FIsDelete=0) "; break;
                 case "GroupP": ; break;
             }
             sql = sql + getPowerConst("pump");
             Hashtable table = Panda_PumpDal.Search(0, 20, "a.FCreateDate", "DESC", sql);
            ViewBag.Grid1DataSource = table["data"];
            ViewBag.Grid1RecordCount = Int32.Parse(table["total"].ToString());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
         public ActionResult Grid1_PageIndexChanged(JArray Grid1_fields, int Grid1_pageIndex, int gridPageSize, string searchMessage,string pageSource)
        {
            var Grid1 = UIHelper.Grid("Grid1");

            string sql = string.Empty;
            if (!searchMessage.Equals(""))
            {
                sql = sql + " and (PName like '%" + searchMessage + "%' or PCustomPName like '%" + searchMessage + "%' or a.ID in (select pumpId from Panda_PumpJZ where FIsDelete=0 and DTUCode='" + searchMessage + "'))";
            }
            switch (pageSource)
            {
                case "pumpG": sql = sql + " and a.ID not in (select pumpID from Panda_PumpFG a,Panda_PumpFG_P b where a.ID=b.GroupID and a.FIsDelete=0) "; break;
                case "GroupP": ; break;
            }
            sql = sql + getPowerConst("pump");
            Hashtable table = Panda_PumpDal.Search(Grid1_pageIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
            Grid1.DataSource(table["data"], Grid1_fields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));

            return UIHelper.Result();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyCustomPostBack(string type, JArray gridFields, JObject typeParams, int gridIndex, int gridPageSize, string pageSource)
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
                sql = " and (PName like '%" + triggerValue + "%' or PCustomPName like '%" + triggerValue + "%'  or a.ID in (select pumpId from Panda_PumpJZ where FIsDelete=0 and DTUCode='" + triggerValue + "'))";
            }
            switch (pageSource)
            {
                case "pumpG": sql = sql + " and a.ID not in (select pumpID from Panda_PumpFG a,Panda_PumpFG_P b where a.ID=b.GroupID and a.FIsDelete=0) "; break;
                case "GroupP": ; break;
            }
            sql = sql + getPowerConst("pump");
            Hashtable table = Panda_PumpDal.Search(gridIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
            Grid1.DataSource(table["data"], gridFields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));
            Grid1.PageSize(gridPageSize);
            return UIHelper.Result();
        }
	}
}