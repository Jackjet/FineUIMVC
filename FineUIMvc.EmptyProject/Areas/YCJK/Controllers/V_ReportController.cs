using FineUIMvc.PumpMVC.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FineUIMvc.PumpMVC.Areas.YCJK.Controllers
{
    [Authorize]
    public class V_ReportController : BaseController
    {
        //
        // GET: /YCJK/V_Report/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RunDayLog()
        {
            return View();
        }
        public ActionResult mainReport()
        {
            return View();
        }
        public ActionResult egWaterInOutPress()
        {
            return View();
        }
        public ActionResult eg_rangeCompare()
        {
            return View();
        }
        public ActionResult egCompreReport()
        {
            return View();
        }
        public ActionResult yearMonthDayChart()
        {
            return View();
        }
        public ActionResult dayHourElec()
        {
            return View();
        }
        public ActionResult dayHourFlow()
        {
            return View();
        }
        public ActionResult MonthDayPress()
        {
            return View();
        }
        public ActionResult MonthDayElec()
        {
            return View();
        }
        public ActionResult MonthDayFlow()
        {
            return View();
        }
        public ActionResult YearMonthPress()
        {
            return View();
        }
        public ActionResult YearMonthElec()
        {
            return View();
        }
        public ActionResult YearMonthFlow()
        {
            return View();
        }
        public ActionResult rangePowerUse()
        {
            return View();
        }

        public ActionResult pumpAutoControl()
        {
            return View();
        }
	}
}