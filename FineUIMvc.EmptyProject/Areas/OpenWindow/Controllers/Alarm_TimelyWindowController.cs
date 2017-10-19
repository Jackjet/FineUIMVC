using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FineUIMvc.PumpMVC.Controllers;
using FineUIMvc.PumpMVC.DAL;
using FineUIMvc.PumpMVC.Models;
using FineUIMvc.PumpMVC.Common;
using System.Data;
using System.Web.Script.Serialization;

namespace FineUIMvc.PumpMVC.Areas.OpenWindow.Controllers
{
    public class Alarm_TimelyWindowController : BaseController
    {
        private DBController db = new DBController();
        //
        // GET: /OpenWindow/Alarm_TimelyWindow/
        public ActionResult Index()
        {
            List<Alarm_Timely_A> list = new List<Alarm_Timely_A>();
            list.AddRange(from x in db.Alarm_Timely.Where(x => x.FMarkerType == 1 && x.FStatus == 1)
                          from y in db.Panda_PumpJZ.Where(y => y.ID.Equals(x.BaseID))
                          from z in db.Panda_Pump.Where(z => z.ID.Equals(y.PumpId))
                          select new Alarm_Timely_A
                          {
                              id = x.id,
                              ParamID = x.ParamID,
                              PumpID = z.ID,
                              BaseID = x.BaseID,
                              FName = z.PName,
                              FCustomerID = z.Panda_Customer.ID,
                              PCompanyNumber = z.PCompanyNumber,
                              FMarkerType = x.FMarkerType,
                              FKey = x.FKey,
                              FMsg = x.FMsg,
                              FSetMsg = x.FSetMsg,
                              FLev = x.FLev,
                              FStatus = x.FStatus,
                              FIsPhone = x.FIsPhone,
                              FAlarmTime = x.FAlarmTime
                          });

            string userType = GetUserType();
            switch (userType)
            {
                case "1": ; break;
                case "2":
                    string number = GetUserCompanyNumber();
                    list = list.Where(x => x.PCompanyNumber.Equals(number)).ToList(); break;
                case "3":
                    int customerid = Convert.ToInt32(GetUserCustomer());
                    list = list.Where(x => x.FCustomerID == customerid).ToList(); break;
                case "4":
                    string[] sgroup = GetUserPumpGroup().Split(',');
                    if (sgroup.Length > 0)
                    {
                        for (int i = 0; i < sgroup.Length; i++)
                        {
                            sgroup[i] = sgroup[i].Substring(1, sgroup[i].Length - 2);
                        }
                    }
                    Guid[] igroup = Array.ConvertAll<string, Guid>(sgroup, delegate(string s) { return Guid.Parse(s); });
                    list = list.Where(x => igroup.Contains(x.PumpID)).ToList()
                    ; break;
            }

            var q4=(from x in db.Alarm_Timely.Where(x => x.FMarkerType == 4 && x.FStatus == 1)
                          from y in db.BASE_SHUICHANG_JZ.Where(y => y.ID.Equals(x.BaseID))
                          from z in db.BASE_SHUICHANG.Where(z => z.id.Equals(y.ShuiChangId))
                          select new Alarm_Timely_A
                          {
                              id = x.id,
                              ParamID = x.ParamID,
                              PumpID = z.id,
                              BaseID = x.BaseID,
                              FName = z.FName,
                              FCustomerID = z.Panda_Customer.ID,
                              PCompanyNumber = "9999",
                              FMarkerType = x.FMarkerType,
                              FKey = x.FKey,
                              FMsg = x.FMsg,
                              FSetMsg = x.FSetMsg,
                              FLev = x.FLev,
                              FStatus = x.FStatus,
                              FIsPhone = x.FIsPhone,
                              FAlarmTime = x.FAlarmTime
                          });

            switch (userType)
            {
                case "1": ; break;
                case "2":
                    string number = GetUserCompanyNumber();
                    q4 = q4.Where(x => x.PCompanyNumber.Equals(number)); break;
                case "3":
                    int customerid = Convert.ToInt32(GetUserCustomer());
                    q4 = q4.Where(x => x.FCustomerID == customerid); break;
                case "4":
                    number = GetUserCompanyNumber();
                    q4 = q4.Where(x => x.PCompanyNumber.Equals(number)); break;
            }

            list.AddRange(q4.ToList());

            list = list.OrderByDescending(x => x.FAlarmTime).ToList();
            ViewBag.Grid1DataSource = list;
            return View();
        }
	}
}