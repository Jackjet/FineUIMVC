using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using FineUIMvc.PumpMVC.Controllers;
using FineUIMvc.PumpMVC.DAL;
using FineUIMvc.PumpMVC.Models;
using System.Collections;
using Newtonsoft.Json.Linq;


namespace FineUIMvc.PumpMVC.Areas.OpenWindow.Controllers
{
    [Authorize]
    public class AlarmContactWindowController : BaseController
    {
        private DBController db = new DBController();
        //
        // GET: /OpenWindow/AlarmContactWindow/
        [MyAuth(MenuPower = "CoreAlarmContactView")]
        public ActionResult Index(string type, string FCustomerID, string PumpJZContactGroup)
        {
            Hashtable table = AlarmContactDal.SearchGroupCon(0, 20, "Contacts", "DESC", " and FType='" + type + "' and FCustomerID='" + FCustomerID + "'");
            ViewBag.Grid1DataSource = table["data"];
            ViewBag.Grid1RecordCount = Int32.Parse(table["total"].ToString());
            ViewBag.type = type;
            ViewBag.FCustomerID = FCustomerID;
            string[] ls = PumpJZContactGroup.Split(',');
            //int[] l = Array.ConvertAll<string, int>(ls, delegate(string s) { return int.Parse(s); });
            ViewBag.GridSelectedRow = ls;
            return View();
        }
    }
}