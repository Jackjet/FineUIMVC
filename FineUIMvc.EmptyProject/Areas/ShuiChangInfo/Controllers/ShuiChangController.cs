using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FineUIMvc.PumpMVC.Controllers;
using FineUIMvc.PumpMVC.DAL;
using FineUIMvc.PumpMVC.Models;
using FineUIMvc.PumpMVC.Common;
using System.Collections;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Text;
using System.Net;
using System.Web.Script.Serialization;

namespace FineUIMvc.PumpMVC.Areas.ShuiChangInfo.Controllers
{
    [Authorize]
    public class ShuiChangController : BaseController
    {
        DBController DB = new DBController();
        private JavaScriptSerializer js = new JavaScriptSerializer();
        //
        // GET: /ShuiChangInfo/ShuiChang/
        public ActionResult Index()
        {
            int customerID = Convert.ToInt32(GetUserCustomer());
            var BASE_SHUICHANG = DB.BASE_SHUICHANG.FirstOrDefault(x => x.Panda_Customer.ID == customerID && x.FIsDelete == 0);
            if (BASE_SHUICHANG == null)
            {   
                return RedirectToAction("NotFound");
            }

            string S_URL_STR = BASE_SHUICHANG.FJKLink.Split('_')[0];
            string E_URL_STR = BASE_SHUICHANG.FJKLink.Split('_')[1];
            string URL_INDEX = S_URL_STR + "_Index";
            return RedirectToAction(URL_INDEX, new { customerID = customerID, fjLink = BASE_SHUICHANG.FJKLink, ShuiChangID = BASE_SHUICHANG.id });
        }
        public ActionResult NotFound()
        {

            return View();
        }
        public ActionResult HS_Index(int customerID, string fjLink, Guid ShuiChangID)
        {
            ViewBag.FjLink = fjLink;
            ViewBag.ShuiChangID = ShuiChangID;
            var BASE_SHUICHANG = DB.BASE_SHUICHANG.Where(x => x.Panda_Customer.ID == customerID && x.FIsDelete == 0).ToList();
            return View(BASE_SHUICHANG);
        }
     
        public ActionResult HS_S1(Guid ID)
        {
            ViewBag.ShuiChangID = ID;
            return View();
        }
        public ActionResult HS_S2(Guid ID)
        {
            ViewBag.ShuiChangID = ID;
            return View();
        }
        public void HS_S1_DataMain()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                Guid _waterID = Request["waterID"] == null ? Guid.Empty : Guid.Parse(Request["waterID"]);
                Guid _waterJZID = Request["waterJZID"] == null ? Guid.Empty : Guid.Parse(Request["waterJZID"]);
                using (var DB = new DBController())
                {
                    var q = (from a in DB.BASE_SHUICHANG
                             select new
                             {
                                 waterID = a.id,
                                 a.FCode,
                                 a.FName,
                                 a.FLngLat,
                                 a.FMapAddress,
                                 a.FType,
                                 a.FWater,
                                 a.FWaterPa,
                                 a.FEnterWNum,
                                 a.FExitWNum,
                                 a.FWaterM3,
                                 a.FRotaryPa,
                                 a.FInDiameter,
                                 a.FOutDiameter,
                                 a.FNote,
                                 a.FIsDelete,
                                 a.FMarkerID,
                                 a.FJKLink,
                                 FCustomerID = a.Panda_Customer.ID,
                                 waterJZ = DB.BASE_SHUICHANG_JZ.Select(b => new
                                 {
                                     waterJZID = b.ID,
                                     b.ShuiChangId,
                                     b.FName,
                                     b.FDTUCode,
                                     b.MachineType,
                                     b.CollectPeriod,
                                     b.CollectLength,
                                     b.ReadMode,
                                     b.AddressScheme.ID,
                                     b.FIsDelete,
                                     IsAlarm = DB.Alarm_Timely.Where(y => y.BaseID.Equals(b.ID) && y.FMarkerType == 1 && y.FStatus == 1).Count() > 0 ? 1 : 0,
                                     D_Data = DB.HS_Data_ShuiChang_MAIN.Where(c => c.BaseID.Equals(b.ID))
                                 }).Where(b => b.ShuiChangId.Equals(a.id) && b.FIsDelete == 0 && (!_waterJZID.Equals(Guid.Empty) ? b.waterJZID.Equals(_waterJZID) : true))
                             }).Where(u => u.waterID.Equals(_waterID) && u.FIsDelete == 0);

                    string userType = GetUserType();
                    switch (userType)
                    {
                        case "1": str = successMsg("无权限", "false"); ; break;
                        case "2": str = successMsg("无权限", "false"); ; break;
                        case "3":
                            int customerid = Convert.ToInt32(GetUserCustomer());
                            q = q.Where(x => x.FCustomerID == customerid);
                            str = successMsg("查询成功", "true", js.Serialize(q)); break;
                        case "4": str = successMsg("无权限", "false"); ; break;
                    }
                }
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "查询失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }


        #region 富源
        public ActionResult FY_Index(int customerID, string fjLink, Guid ShuiChangID)
        {
            ViewBag.FjLink = fjLink;
            ViewBag.ShuiChangID = ShuiChangID;
            var BASE_SHUICHANG = DB.BASE_SHUICHANG.Where(x => x.Panda_Customer.ID == customerID && x.FIsDelete == 0).ToList();
            return View(BASE_SHUICHANG);
        }
        public ActionResult FY_S1(Guid ID)
        {
            ViewBag.ShuiChangID = ID;
            return View();
        }
        public void FY_Data_ShuiChang_MAIN()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                Guid _waterID = Request["waterID"] == null ? Guid.Empty : Guid.Parse(Request["waterID"]);
                Guid _waterJZID = Request["waterJZID"] == null ? Guid.Empty : Guid.Parse(Request["waterJZID"]);
                using (var DB = new DBController())
                {
                    var q = (from a in DB.BASE_SHUICHANG
                             select new
                             {
                                 waterID = a.id,
                                 a.FCode,
                                 a.FName,
                                 a.FLngLat,
                                 a.FMapAddress,
                                 a.FType,
                                 a.FWater,
                                 a.FWaterPa,
                                 a.FEnterWNum,
                                 a.FExitWNum,
                                 a.FWaterM3,
                                 a.FRotaryPa,
                                 a.FInDiameter,
                                 a.FOutDiameter,
                                 a.FNote,
                                 a.FIsDelete,
                                 a.FMarkerID,
                                 a.FJKLink,
                                 FCustomerID = a.Panda_Customer.ID,
                                 waterJZ = DB.BASE_SHUICHANG_JZ.Select(b => new
                                 {
                                     waterJZID = b.ID,
                                     b.ShuiChangId,
                                     b.FName,
                                     b.FDTUCode,
                                     b.MachineType,
                                     b.CollectPeriod,
                                     b.CollectLength,
                                     b.ReadMode,
                                     b.AddressScheme.ID,
                                     b.FIsDelete,
                                     IsAlarm = DB.Alarm_Timely.Where(y => y.BaseID.Equals(b.ID) && y.FMarkerType == 1 && y.FStatus == 1).Count() > 0 ? 1 : 0,
                                     D_Data = DB.FY_Data_ShuiChang_MAIN.Where(c => c.BaseID.Equals(b.ID))
                                 }).Where(b => b.ShuiChangId.Equals(a.id) && b.FIsDelete == 0 && (!_waterJZID.Equals(Guid.Empty) ? b.waterJZID.Equals(_waterJZID) : true))
                             }).Where(u => u.waterID.Equals(_waterID) && u.FIsDelete == 0);

                    string userType = GetUserType();
                    switch (userType)
                    {
                        case "1": str = successMsg("无权限", "false"); ; break;
                        case "2": str = successMsg("无权限", "false"); ; break;
                        case "3":
                            int customerid = Convert.ToInt32(GetUserCustomer());
                            q = q.Where(x => x.FCustomerID == customerid);
                            str = successMsg("查询成功", "true", js.Serialize(q)); break;
                        case "4": str = successMsg("无权限", "false"); ; break;
                    }
                }
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "查询失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }
        public JsonResult FY_Data_ShuiChangByPage(Guid waterJZID, int pageIndex, int PageSize, DateTime? StartTime, DateTime? EndTime)
        {
            StringBuilder str = new StringBuilder();
            string msg = "";
            string success = "";
            List<FY_Data_ShuiChang> FY_Data_ShuiChang = new List<FY_Data_ShuiChang>();
            int total = 0;
            try
            {
                using (var DB = new DBController())
                {

                    string userType = GetUserType();
                    var query = DB.FY_Data_ShuiChang.Where(x => x.BaseID == waterJZID && x.TempTime <= EndTime.Value && x.TempTime >= StartTime.Value).ToList();

                    FY_Data_ShuiChang = query.OrderByDescending(x => x.TempTime).Skip((pageIndex - 1) * PageSize).Take(PageSize).ToList();
                    total = query.Count();

                    switch (userType)
                    {
                        case "1": msg = "无权限"; success = "false"; break;
                        case "2": msg = "无权限"; success = "false"; break;
                        case "3": msg = "查询成功"; success = "true"; break;
                        case "4": msg = "无权限"; success = "false"; break;
                    }
                }
                return Json(new { success = success, msg = msg, FY_Data_ShuiChang = FY_Data_ShuiChang, total = total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json(new { success = "false", msg = "查询失败" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
     


        #region 禹升园
        public ActionResult YSY_Index(int customerID, string fjLink, Guid ShuiChangID)
        {
            ViewBag.FjLink = fjLink;
            ViewBag.ShuiChangID = ShuiChangID;
            var BASE_SHUICHANG = DB.BASE_SHUICHANG.Where(x => x.Panda_Customer.ID == customerID && x.FIsDelete == 0).ToList();
            return View(BASE_SHUICHANG);
        }

        public ActionResult YSY_S1(Guid ID)
        {
            ViewBag.ShuiChangID = ID;
            return View();
        }

        public void YSY_Data_ShuiChang_MAIN()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                Guid _waterID = Request["waterID"] == null ? Guid.Empty : Guid.Parse(Request["waterID"]);
                Guid _waterJZID = Request["waterJZID"] == null ? Guid.Empty : Guid.Parse(Request["waterJZID"]);
                using (var DB = new DBController())
                {
                    var q = (from a in DB.BASE_SHUICHANG
                             select new
                             {
                                 waterID = a.id,
                                 a.FCode,
                                 a.FName,
                                 a.FLngLat,
                                 a.FMapAddress,
                                 a.FType,
                                 a.FWater,
                                 a.FWaterPa,
                                 a.FEnterWNum,
                                 a.FExitWNum,
                                 a.FWaterM3,
                                 a.FRotaryPa,
                                 a.FInDiameter,
                                 a.FOutDiameter,
                                 a.FNote,
                                 a.FIsDelete,
                                 a.FMarkerID,
                                 a.FJKLink,
                                 FCustomerID = a.Panda_Customer.ID,
                                 waterJZ = DB.BASE_SHUICHANG_JZ.Select(b => new
                                 {
                                     waterJZID = b.ID,
                                     b.ShuiChangId,
                                     b.FName,
                                     b.FDTUCode,
                                     b.MachineType,
                                     b.CollectPeriod,
                                     b.CollectLength,
                                     b.ReadMode,
                                     b.AddressScheme.ID,
                                     b.FIsDelete,
                                     IsAlarm = DB.Alarm_Timely.Where(y => y.BaseID.Equals(b.ID) && y.FMarkerType == 1 && y.FStatus == 1).Count() > 0 ? 1 : 0,
                                     D_Data = DB.FY_Data_ShuiChang_MAIN.Where(c => c.BaseID.Equals(b.ID))
                                 }).Where(b => b.ShuiChangId.Equals(a.id) && b.FIsDelete == 0 && (!_waterJZID.Equals(Guid.Empty) ? b.waterJZID.Equals(_waterJZID) : true))
                             }).Where(u => u.waterID.Equals(_waterID) && u.FIsDelete == 0);

                    string userType = GetUserType();
                    switch (userType)
                    {
                        case "1": str = successMsg("无权限", "false"); ; break;
                        case "2": str = successMsg("无权限", "false"); ; break;
                        case "3":
                            int customerid = Convert.ToInt32(GetUserCustomer());
                            q = q.Where(x => x.FCustomerID == customerid);
                            str = successMsg("查询成功", "true", js.Serialize(q)); break;
                        case "4": str = successMsg("无权限", "false"); ; break;
                    }
                }
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "查询失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }

        public JsonResult YSY_Data_ShuiChangByPage(Guid waterJZID, int pageIndex, int PageSize, DateTime? StartTime, DateTime? EndTime)
        {
            StringBuilder str = new StringBuilder();
            string msg = "";
            string success = "";
            List<FY_Data_ShuiChang> FY_Data_ShuiChang = new List<FY_Data_ShuiChang>();
            int total = 0;
            try
            {
                using (var DB = new DBController())
                {

                    string userType = GetUserType();
                    var query = DB.FY_Data_ShuiChang.Where(x => x.BaseID == waterJZID && x.TempTime <= EndTime.Value && x.TempTime >= StartTime.Value).ToList();

                    FY_Data_ShuiChang = query.OrderByDescending(x => x.TempTime).Skip((pageIndex - 1) * PageSize).Take(PageSize).ToList();
                    total = query.Count();

                    switch (userType)
                    {
                        case "1": msg = "无权限"; success = "false"; break;
                        case "2": msg = "无权限"; success = "false"; break;
                        case "3": msg = "查询成功"; success = "true"; break;
                        case "4": msg = "无权限"; success = "false"; break;
                    }
                }
                return Json(new { success = success, msg = msg, FY_Data_ShuiChang = FY_Data_ShuiChang, total = total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json(new { success = "false", msg = "查询失败" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

    }
}