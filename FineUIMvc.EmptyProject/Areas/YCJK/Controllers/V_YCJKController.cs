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
using FineUIMvc.PumpMVC.SetCommand2Service;
using FineUIMvc.PumpMVC.FaMenService;
using FineUIMvc.PumpMVC.ReportModel;
using FineUIMvc.PumpMVC.Common.ExcelExport;

namespace FineUIMvc.PumpMVC.Areas.YCJK.Controllers
{
    [Authorize]
    public class V_YCJKController : BaseController
    {
        private DBController db = new DBController();
        private IExcelExport _excelExport = new NPOIExcelExport();
        private JavaScriptSerializer js = new JavaScriptSerializer();
        //
        // GET: /YCJK/V_YCJK/
        //public ActionResult Index()
        //{
        //    return View();
        //}
        #region
        [MyAuth(MenuPower = "CoreEGBFView")]
        public ActionResult EGBF()
        {
            return View();
        }
        public ActionResult Pump3D()
        {
            return View();
        }

        public ActionResult Map()
        {
            return View();
        }

        public ActionResult EGBFSB(string id)
        {
            return View();
        }

        public ActionResult listControl()
        {
            return View();
        }

        public ActionResult loopCheck()
        {
            return View();
        }
        public ActionResult RunDayLog()
        {
            return View();
        }
        public ActionResult RelationFile()
        {
            return View();
        }
        public ActionResult JZInfo()
        {
            return View();
        }

        public ActionResult WarningAlarm()
        {
            return View();
        }

        public ActionResult ParmSet1()
        {
            return View();
        }
        public ActionResult ParmSet2()
        {
            return View();
        }
        public ActionResult dataReport()
        {
            return View();
        }
        public ActionResult Panorama()
        {
            return View();
        }
        public ActionResult video()
        {
            return View();
        }
        #endregion

        #region Report

        #region 泵房、机组基本信息
        /// <summary>
        /// 在线列表
        /// </summary>
        public void Search_OnLineList()
        {
            string strwhere = "";
            string _JZName = Request["JZName"];
            string _State = Request["State"];
            if (_JZName != "" && _JZName != null)
            {
                strwhere = strwhere + " and (a.PumpJZName like '%" + _JZName + "%' or a.DTUCode like '%" + _JZName + "%')";
            }
            switch (_State)
            {
                case "0": strwhere = strwhere + " and b.FOnLine='" + _State + "'"; break;     //离线
                case "1": strwhere = strwhere + " and b.FOnLine='" + _State + "'"; break;    //在线
                case "2": strwhere = strwhere + @" and isnull((select top 1 1 from Alarm_Timely _c 
                                              where _c.BaseID=a.id and FMarkerType=1 and FStatus=1),0)=1"; break;  //报警
            }

            strwhere = strwhere + getPowerConst("pumpJZ");

            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);
            Hashtable result = Panda_PumpJZDal.Search_OnLineList(pageIndex, pageSize, "c.PName asc,DTUCode", "desc", strwhere);
            string json = PluSoft.Utils.JSON.Encode(result);
            Response.Write(json);
        }
        /// <summary>
        /// 在线离线报警总数
        /// </summary>
        public void SearchZLB_Count()
        {
            DataTable dt = Panda_PumpJZDal.SearchZLB_Count(getPowerConst("pumpJZ"));
            string json = PluSoft.Utils.JSON.Encode(dt);
            Response.Write(json);
        }
        /// <summary>
        /// 机组列表
        /// </summary>
        public void Search_ReportList()
        {
            string strwhere = "";
            string _JZName = Request["JZName"];
            string _State = Request["State"];
            if (_JZName != "" && _JZName != null)
            {
                strwhere = strwhere + " and (a.PumpJZName like '%" + _JZName + "%' or a.DTUCode like '%" + _JZName + "%')";
            }
            switch (_State)
            {
                case "0": strwhere = strwhere + " and b.FOnLine='" + _State + "'"; break;     //离线
                case "1": strwhere = strwhere + " and b.FOnLine='" + _State + "'"; break;    //在线
                case "2": strwhere = strwhere + @" and isnull((select top 1 1 from Alarm_Timely _c 
                                              where _c.BaseID=a.id and FMarkerType=1 and FStatus=1),0)=1"; break;  //报警
            }

            strwhere = strwhere + getPowerConst("pumpJZ");

            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);

            string select = "select a.ID as pumpJZId,c.ID as pumpID,a.PumpJZName as jzName,c.PName as pName,a.PumpJZArea as jzArea, ";

            if (!FLogParamDal.Exist(GetIdentityName()))
            {
                FLogParamDal.SearchInsert(GetIdentityName());
            }


            DataTable dt = FLogParamDal.Search(" and UserID='" + GetIdentityName() + "' and IsSelect2=1", "IsSelect2");

            string jsonName = "";

            jsonName = jsonName + "[\"是否在线:FOnLine\",\"是否报警:FIsAlarm\"";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonName = jsonName + ",\"" + dt.Rows[i]["FName"].ToString() + ":" + dt.Rows[i]["FFieldName"].ToString() + "\"";
                switch (dt.Rows[i]["FFieldName"].ToString())
                {
                    case "DTUCode":
                        select = select + "a.DTUCode,"; break;
                    case "PumpJZName":
                        select = select + "a.PumpJZName,"; break;
                    case "PumpJZArea":
                        select = select + "a.PumpJZArea,"; break;
                    case "FUpdateDate":
                        select = select + "b.FUpdateDate,"; break;
                    case "PumpName":
                        select = select + "c.PName as PumpName,"; break;
                    case "InOutWaPa":
                        select = select + "convert(varchar(50),isnull(b.F41006,0)) +'/'+convert(varchar(50),isnull(b.F41007,0)) AS InOutWaPa,"; break;
                    case "PActiveState":
                        select = select + @"PActiveState=case when RunPumpNum+Auxiliarypumpcount=6 
                                                              then convert(varchar(50),isnull(b.F41008,0))+'/'+convert(varchar(50),isnull(b.F41009,0))+'/'+convert(varchar(50),isnull(b.F41010,0))+'/'+convert(varchar(50),isnull(b.F41011,0))+'/'+convert(varchar(50),isnull(b.F41012,0))+'/'+convert(varchar(50),isnull(b.F41013,0))
				                                              when RunPumpNum+Auxiliarypumpcount=5 
                                                              then convert(varchar(50),isnull(b.F41008,0))+'/'+convert(varchar(50),isnull(b.F41009,0))+'/'+convert(varchar(50),isnull(b.F41010,0))+'/'+convert(varchar(50),isnull(b.F41011,0))+'/'+convert(varchar(50),isnull(b.F41012,0))
				                                              when RunPumpNum+Auxiliarypumpcount=4 
                                                              then convert(varchar(50),isnull(b.F41008,0))+'/'+convert(varchar(50),isnull(b.F41009,0))+'/'+convert(varchar(50),isnull(b.F41010,0))+'/'+convert(varchar(50),isnull(b.F41011,0))
				                                              when RunPumpNum+Auxiliarypumpcount=3 
                                                              then convert(varchar(50),isnull(b.F41008,0))+'/'+convert(varchar(50),isnull(b.F41009,0))+'/'+convert(varchar(50),isnull(b.F41010,0))
				                                              when RunPumpNum+Auxiliarypumpcount=2 
                                                              then convert(varchar(50),isnull(b.F41008,0))+'/'+convert(varchar(50),isnull(b.F41009,0))
				                                              when RunPumpNum+Auxiliarypumpcount=1 
                                                              then convert(varchar(50),isnull(b.F41008,0))
				                                              end,"; break;
                    case "PDrainage":
                        select = select + @"PDrainage= case when DrainPumpNum=4 then convert(varchar(50),isnull(b.F41102,0))+'/'+convert(varchar(50),isnull(b.F41103,0))+'/'+convert(varchar(50),isnull(b.F41104,0))+'/'+convert(varchar(50),isnull(b.F41105,0))
                                                            when DrainPumpNum=3 then convert(varchar(50),isnull(b.F41102,0))+'/'+convert(varchar(50),isnull(b.F41103,0))+'/'+convert(varchar(50),isnull(b.F41104,0))
                                                            when DrainPumpNum=2 then convert(varchar(50),isnull(b.F41102,0))+'/'+convert(varchar(50),isnull(b.F41103,0))
                                                            when DrainPumpNum=1 then convert(varchar(50),isnull(b.F41102,0))
                                                            end,"; break;
                    default: select = select + "b." + dt.Rows[i]["FFieldName"].ToString() + ","; break;
                }

            }
            select = select + "b.FOnLine,isnull((select top 1 1 from Alarm_Timely _c  where _c.BaseID=a.id and FMarkerType=1 and FStatus=1),0) as FIsAlarm";
            jsonName = jsonName + "]";

            Hashtable result = Panda_PumpJZDal.Search_ReportList(pageIndex, pageSize, "DTUCode", "desc", select, strwhere);

            string jsonData = PluSoft.Utils.JSON.Encode(result);
            //string jsonName = "[\"是否在线:FOnLine\",\"是否报警:FIsAlarm\",\"设备编号:DTUCode\",\"设备名称:PumpJZName\",\"最后采集时间:FUpdateDate\",\"进/出水压力(Mpa):InOutWaPa\"]";
            string json = "{\"data\":["
                                    + "{\"jsonName\":" + jsonName + " }, "
                                    + "{\"jsonData\":" + jsonData + " } "
                           + "]}";
            Response.Write(json);
        }

        /// <summary>
        /// 泵房机组列表
        /// </summary>
        public void Search_Pump_JZReportList()
        {
            Guid _pumpID = Guid.Parse(Request["pumpID"]);
            Guid _pumpJZID = Request["pumpJZID"] == null ? Guid.Empty : Guid.Parse(Request["pumpJZID"]);
            using (var DB = new DBController())
            {
                var q = (from a in DB.Panda_Pump
                         select new
                         {
                             pumpID = a.ID,
                             a.PName,
                             a.PCustomPName,
                             a.PLngLat,
                             a.FIsDelete,
                             a.PCompanyNumber,
                             FCustomerID = a.Panda_Customer.ID,
                             a.PAddress,
                             pumpVQ = DB.Panda_PumpVQ.Select(c => new
                                 {
                                     c.PumpId,
                                     QuipmentType = DB.sys_dictItems.Where(i => i.FValue == c.QuipmentType && i.FDictID == 134).FirstOrDefault().FName,
                                     c.Type,
                                     c.Brand,
                                     c.Number,
                                     c.UserName,
                                     c.PassWord,
                                     c.Mark,
                                     c.IP,
                                     c.Port,
                                     c.IsActive,
                                     c.FOrderBy,
                                     c.Hls,
                                     c.Rtmp
                                 }).Where(c => c.PumpId.Equals(a.ID) && c.IsActive == false).OrderBy(c => c.FOrderBy),
                             pumpJZ = DB.Panda_PumpJZ.Select(b => new
                             {
                                 pumpJZID = b.ID,
                                 b.PumpId,
                                 b.DTUCode,
                                 b.PumpJZName,
                                 b.MachineType,
                                 b.RunPumpNum,
                                 b.Auxiliarypumpcount,
                                 b.DrainPumpNum,
                                 IsAlarm = DB.Alarm_Timely.Where(y => y.BaseID.Equals(b.ID) && y.FMarkerType == 1 && y.FStatus == 1).Count() > 0 ? 1 : 0,
                                 b.PumpJZArea,
                                 b.AddressScheme.FName,
                                 FSchemeID = b.AddressScheme.ID,
                                 b.PumpJZCollectPeriod,
                                 b.PumpJZCollectLength,
                                 b.PumpJZReadMode,
                                 b.PumpJZTankVolume,
                                 b.PumpJZTankLength,
                                 b.PumpJZInletDiameter,
                                 b.PumpJZOutletDiameter,
                                 b.PumpJZBrandSet,
                                 b.PumpJZPumpBrand,
                                 b.PumpJZMainPumpFlow,
                                 b.PumpJZMainPumpLift,
                                 b.PumpJZAuxiliPumpFlow,
                                 b.PumpJZAuxiliPumpLift,
                                 b.PumpJZMainPumpPower,
                                 b.PumpJZAuxiliPumpPower,
                                 b.PumpJZPressReliValve,
                                 b.PumpJZPeak,
                                 b.PumpJZCheckValve,
                                 b.PumpJZInPUpper,
                                 b.PumpJZInPLower,
                                 b.PumpJZOutPUpper,
                                 b.PumpJZOutPLower,
                                 b.PumpJZReChlorUpper,
                                 b.PumpJZReChlorLower,
                                 b.PumpJZTurbidUpper,
                                 b.PumpJZTurbidLower,
                                 b.PumpJZPHUpper,
                                 b.PumpJZPHLower,
                                 b.PumpJZTankUpper,
                                 b.PumpJZTankLower,
                                 b.FIsDelete,
                                 D_Data = DB.E_DATA_MAIN.Where(c => c.BaseID.Equals(b.ID))
                             }).Where(b => b.PumpId.Equals(a.ID) && b.FIsDelete == 0 && (!_pumpJZID.Equals(Guid.Empty) ? b.pumpJZID.Equals(_pumpJZID) : true)),
                         }).Where(u => u.pumpID.Equals(_pumpID) && u.FIsDelete == 0);

                if (!_pumpJZID.Equals(Guid.Empty))
                {
                    //  q = q.Where(x => x.pumpJZ == x.pumpJZ.Where(y => y.pumpJZID.Equals(_pumpJZID)));
                }

                string userType = GetUserType();
                switch (userType)
                {
                    case "1": ; break;
                    case "2":
                        string number = GetUserCompanyNumber();
                        q = q.Where(x => x.PCompanyNumber.Equals(number)); break;
                    case "3":
                        int customerid = Convert.ToInt32(GetUserCustomer());
                        q = q.Where(x => x.FCustomerID == customerid); break;
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
                        q = q.Where(x => igroup.Contains(x.pumpID))
                        ; break;
                }

                StringBuilder str = new StringBuilder();
                str.AppendFormat("{0}\"success\":true,\"obj\":", "{");
                str.AppendFormat("{0}", js.Serialize(q));
                str.AppendFormat("{0}", "}");
                Response.Write(str);
            }
        }

        /// <summary>
        /// 年份历史报表
        /// </summary>
        public void Search_YearReportList()
        {
            string strwhere = "";
            string pumpID = Request["pumpID"];   //搜索泵房id
            string pumpJZID = Request["pumpJZID"];   //搜索机组id
            string Start = Request["StartDate"];   //开始日期
            string End = Request["EndDate"];   //结束日期
            int year = Convert.ToDateTime(Start).Year;
            if (pumpID != null && pumpID != "")
            {
                strwhere = strwhere + " and c.id='" + pumpID + "'";
            }
            if (pumpJZID != null && pumpJZID != "")
            {
                strwhere = strwhere + " and b.BASEID='" + pumpJZID + "'";
            }
            strwhere = strwhere + " and b.TempTime between '" + Start + "' and '" + End + "'";
            strwhere = strwhere + getPowerConst("pumpJZ");

            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);

            string select = "select a.ID as pumpJZId,c.ID as pumpID, ";

            if (!FLogParamDal.Exist(GetIdentityName()))
            {
                FLogParamDal.SearchInsert(GetIdentityName());
            }

            DataTable dt = FLogParamDal.Search(" and UserID='" + GetIdentityName() + "' and IsSelect=1", "IsSelect");

            string jsonName = "";

            jsonName = jsonName + "[\"泵房ID:pumpID\",\"机组ID:pumpJZId\"";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonName = jsonName + ",\"" + dt.Rows[i]["FName"].ToString() + ":" + dt.Rows[i]["FFieldName"].ToString() + "\"";
                switch (dt.Rows[i]["FFieldName"].ToString())
                {
                    case "DTUCode":
                        select = select + "a.DTUCode,"; break;
                    case "PumpJZName":
                        select = select + "a.PumpJZName,"; break;
                    case "PumpJZArea":
                        select = select + "a.PumpJZArea,"; break;
                    case "FUpdateDate":
                        select = select + "b.FCreateDate as FUpdateDate,"; break;
                    case "PumpName":
                        select = select + "c.PName as PumpName,"; break;
                    case "InOutWaPa":
                        select = select + "convert(varchar(50),isnull(b.F41006,0)) +'/'+convert(varchar(50),isnull(b.F41007,0)) AS InOutWaPa,"; break;
                    case "PActiveState":
                        select = select + @"PActiveState=case when RunPumpNum+Auxiliarypumpcount=6 
                                                              then convert(varchar(50),isnull(b.F41008,0))+'/'+convert(varchar(50),isnull(b.F41009,0))+'/'+convert(varchar(50),isnull(b.F41010,0))+'/'+convert(varchar(50),isnull(b.F41011,0))+'/'+convert(varchar(50),isnull(b.F41012,0))+'/'+convert(varchar(50),isnull(b.F41013,0))
				                                              when RunPumpNum+Auxiliarypumpcount=5 
                                                              then convert(varchar(50),isnull(b.F41008,0))+'/'+convert(varchar(50),isnull(b.F41009,0))+'/'+convert(varchar(50),isnull(b.F41010,0))+'/'+convert(varchar(50),isnull(b.F41011,0))+'/'+convert(varchar(50),isnull(b.F41012,0))
				                                              when RunPumpNum+Auxiliarypumpcount=4 
                                                              then convert(varchar(50),isnull(b.F41008,0))+'/'+convert(varchar(50),isnull(b.F41009,0))+'/'+convert(varchar(50),isnull(b.F41010,0))+'/'+convert(varchar(50),isnull(b.F41011,0))
				                                              when RunPumpNum+Auxiliarypumpcount=3 
                                                              then convert(varchar(50),isnull(b.F41008,0))+'/'+convert(varchar(50),isnull(b.F41009,0))+'/'+convert(varchar(50),isnull(b.F41010,0))
				                                              when RunPumpNum+Auxiliarypumpcount=2 
                                                              then convert(varchar(50),isnull(b.F41008,0))+'/'+convert(varchar(50),isnull(b.F41009,0))
				                                              when RunPumpNum+Auxiliarypumpcount=1 
                                                              then convert(varchar(50),isnull(b.F41008,0))
				                                              end,"; break;
                    case "PDrainage":
                        select = select + @"PDrainage= case when DrainPumpNum=4 then convert(varchar(50),isnull(b.F41102,0))+'/'+convert(varchar(50),isnull(b.F41103,0))+'/'+convert(varchar(50),isnull(b.F41104,0))+'/'+convert(varchar(50),isnull(b.F41105,0))
                                                            when DrainPumpNum=3 then convert(varchar(50),isnull(b.F41102,0))+'/'+convert(varchar(50),isnull(b.F41103,0))+'/'+convert(varchar(50),isnull(b.F41104,0))
                                                            when DrainPumpNum=2 then convert(varchar(50),isnull(b.F41102,0))+'/'+convert(varchar(50),isnull(b.F41103,0))
                                                            when DrainPumpNum=1 then convert(varchar(50),isnull(b.F41102,0))
                                                            end,"; break;
                    default: select = select + "b." + dt.Rows[i]["FFieldName"].ToString() + ","; break;
                }

            }
            select = select.Substring(0, select.LastIndexOf(','));
            jsonName = jsonName + "]";

            Hashtable result = Panda_PumpJZDal.Search_YearReportList(pageIndex, pageSize, "TempTime", "desc", select, strwhere, year);

            string jsonData = PluSoft.Utils.JSON.Encode(result);
            //jsonName = "[\"泵房ID:pumpID\",\"机组ID:pumpJZId\",\"设备编号:DTUCode\",\"设备名称:PumpJZName\",\"最后采集时间:TempTime\",\"进/出水压力(Mpa):InOutWaPa\"]";
            string json = "{\"data\":["
                                    + "{\"jsonName\":" + jsonName + " }, "
                                    + "{\"jsonData\":" + jsonData + " } "
                           + "]}";
            Response.Write(json);
        }
        public void ReportRunDayLog()
        {
            string strwhere = "";
            string pumpID = Request["pumpID"];   //搜索泵房id
            string pumpJZID = Request["pumpJZID"];   //搜索机组id
            string Start = Request["StartDate"];   //开始日期
            string End = Request["EndDate"];   //结束日期
            int year = Convert.ToDateTime(Start).Year;
            if (pumpID != null && pumpID != "")
            {
                strwhere = strwhere + " and c.id='" + pumpID + "'";
            }
            if (pumpJZID != null && pumpJZID != "")
            {
                strwhere = strwhere + " and b.BASEID='" + pumpJZID + "'";
            }
            strwhere = strwhere + " and b.TempTime between '" + Start + "' and '" + End + "'";
            strwhere = strwhere + getPowerConst("pumpJZ");

            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);

            string select = "select a.ID as pumpJZId,c.ID as pumpID, ";

            if (!FLogParamDal.Exist(GetIdentityName()))
            {
                FLogParamDal.SearchInsert(GetIdentityName());
            }

            DataTable dt = FLogParamDal.Search(" and UserID='" + GetIdentityName() + "' and IsSelect=1", "IsSelect");

            string jsonName = "";

            jsonName = jsonName + "[\"泵房ID:pumpID\",\"机组ID:pumpJZId\"";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonName = jsonName + ",\"" + dt.Rows[i]["FName"].ToString() + ":" + dt.Rows[i]["FFieldName"].ToString() + "\"";
                switch (dt.Rows[i]["FFieldName"].ToString())
                {
                    case "DTUCode":
                        select = select + "a.DTUCode,"; break;
                    case "PumpJZName":
                        select = select + "a.PumpJZName,"; break;
                    case "PumpJZArea":
                        select = select + "a.PumpJZArea,"; break;
                    case "FUpdateDate":
                        select = select + "b.FCreateDate as FUpdateDate,"; break;
                    case "PumpName":
                        select = select + "c.PName as PumpName,"; break;
                    case "InOutWaPa":
                        select = select + "convert(varchar(50),isnull(b.F41006,0)) +'/'+convert(varchar(50),isnull(b.F41007,0)) AS InOutWaPa,"; break;
                    case "PActiveState":
                        select = select + @"PActiveState=case when RunPumpNum+Auxiliarypumpcount=6 
                                                              then convert(varchar(50),isnull(b.F41008,0))+'/'+convert(varchar(50),isnull(b.F41009,0))+'/'+convert(varchar(50),isnull(b.F41010,0))+'/'+convert(varchar(50),isnull(b.F41011,0))+'/'+convert(varchar(50),isnull(b.F41012,0))+'/'+convert(varchar(50),isnull(b.F41013,0))
				                                              when RunPumpNum+Auxiliarypumpcount=5 
                                                              then convert(varchar(50),isnull(b.F41008,0))+'/'+convert(varchar(50),isnull(b.F41009,0))+'/'+convert(varchar(50),isnull(b.F41010,0))+'/'+convert(varchar(50),isnull(b.F41011,0))+'/'+convert(varchar(50),isnull(b.F41012,0))
				                                              when RunPumpNum+Auxiliarypumpcount=4 
                                                              then convert(varchar(50),isnull(b.F41008,0))+'/'+convert(varchar(50),isnull(b.F41009,0))+'/'+convert(varchar(50),isnull(b.F41010,0))+'/'+convert(varchar(50),isnull(b.F41011,0))
				                                              when RunPumpNum+Auxiliarypumpcount=3 
                                                              then convert(varchar(50),isnull(b.F41008,0))+'/'+convert(varchar(50),isnull(b.F41009,0))+'/'+convert(varchar(50),isnull(b.F41010,0))
				                                              when RunPumpNum+Auxiliarypumpcount=2 
                                                              then convert(varchar(50),isnull(b.F41008,0))+'/'+convert(varchar(50),isnull(b.F41009,0))
				                                              when RunPumpNum+Auxiliarypumpcount=1 
                                                              then convert(varchar(50),isnull(b.F41008,0))
				                                              end,"; break;
                    case "PDrainage":
                        select = select + @"PDrainage= case when DrainPumpNum=4 then convert(varchar(50),isnull(b.F41102,0))+'/'+convert(varchar(50),isnull(b.F41103,0))+'/'+convert(varchar(50),isnull(b.F41104,0))+'/'+convert(varchar(50),isnull(b.F41105,0))
                                                            when DrainPumpNum=3 then convert(varchar(50),isnull(b.F41102,0))+'/'+convert(varchar(50),isnull(b.F41103,0))+'/'+convert(varchar(50),isnull(b.F41104,0))
                                                            when DrainPumpNum=2 then convert(varchar(50),isnull(b.F41102,0))+'/'+convert(varchar(50),isnull(b.F41103,0))
                                                            when DrainPumpNum=1 then convert(varchar(50),isnull(b.F41102,0))
                                                            end,"; break;
                    default: select = select + "b." + dt.Rows[i]["FFieldName"].ToString() + ","; break;
                }

            }
            select = select.Substring(0, select.LastIndexOf(','));
            jsonName = jsonName + "]";

            DataTable dtresult = Panda_PumpJZDal.Search_YearReportList("TempTime", "desc", select, strwhere, year);
            try
            {
                List<RunDayLog> YaLiLog = ModelConvertHelper<RunDayLog>.ConvertToModel(dtresult).ToList();
                var excelExport = _excelExport.CreateWorkbook();
                excelExport.AddSheet(YaLiLog, "");
                string path = System.Web.HttpContext.Current.Server.MapPath("~/DownLoad");
                if (Directory.Exists(path)) Directory.Delete(path, true);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                string pdfname = "/运行日志" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
                string pathFile = excelExport.SaveFile(path + pdfname);
                Hashtable result = new Hashtable();
                result["msg"] = "ok";
                result["Url"] = "/DownLoad" + pdfname;
                string json = PluSoft.Utils.JSON.Encode(result);
                Response.Write(json);
            }
            catch (Exception)
            {
                Hashtable result = new Hashtable();
                result["msg"] = "error";
                string json = PluSoft.Utils.JSON.Encode(result);
                Response.Write(json);
            }

        }
        public void Search_Pump()
        {
            string sql = string.Empty;
            string json = string.Empty;
            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);
            string pumpName = Request["pumpName"];   //搜索泵房名称

            sql = @"select isnull(g.ID,0) as G_ID,a.ID,a.PCode,a.PName,a.PCustomPName,b.Name as CustomerName,c.Fengongsi,d.FName+e.FName as ProvinceCity,
                           PLngLat,PAddress,PXXAddress,TankIsSharing,PumpSoaking,Warning,WaterQualityDetection,ControlValve,WaterTankSterilizer,
                           PumpLocation,MasterControlPLCIP,InstallDate,AcceptanceDate,WaterFloor,DressingCycle,FMarkerID 
                      from Panda_Pump a
                 left join Panda_Customer b on a.FCustomerID=b.ID
                 left join A_U_DEP c on a.PCompanyNumber=c.U8number
                 left join sys_dictItems d on a.PProvince=d.FValue and d.FDictID=108
                 left join sys_dictItems e on a.PCity=e.FValue and e.FDictID=120 
                 left join Panda_PumpFG_P f on f.PumpID=a.ID
                 left join Panda_PumpFG g on f.GroupID=g.ID
                     where a.FIsDelete=0 ";

            if (pumpName != null && pumpName != "")
            {
                sql = sql + " and a.PName like '%" + pumpName + "%'";
            }

            string userType = GetUserType();  //登录用户类型
            string sqlGroupName = string.Empty;

            switch (userType)
            {
                case "1": json = PluSoft.Utils.JSON.Encode(publicDal.HashSearch(pageIndex, pageSize, "a.FCreateDate", "desc", "", sql)); break;
                //case "2": sql = sql + " and a.PCompanyNumber='" + GetUserCompanyNumber() + "'";
                //    json = PluSoft.Utils.JSON.Encode(publicDal.HashSearch(pageIndex, pageSize, "a.FCreateDate", "desc", "", sql)); break;
                case "2": sql = sql + " and a.PCompanyNumber='" + GetUserCompanyNumber() + "'";
                    sqlGroupName = "select 0 as ID,'未分组' as G_Name union all select ID,G_Name from Panda_PumpFG where FCompanyNumber=" + GetUserCompanyNumber();
                    json = "{\"data\":["
                                  + "{\"jsonGroupName\":" + PluSoft.Utils.JSON.Encode(publicDal.TableSearch(sqlGroupName)) + " }, "
                                  + "{\"jsonData\":" + PluSoft.Utils.JSON.Encode(publicDal.HashSearch(pageIndex, pageSize, "isnull(g.ID,0),a.FCreateDate", "desc", "", sql)) + " } "
                         + "]}"; break;
                case "3": sql = sql + " and a.FCustomerID=" + GetUserCustomer();
                    sqlGroupName = "select 0 as ID,'未分组' as G_Name union all select ID,G_Name from Panda_PumpFG where FCustomerID=" + GetUserCustomer();
                    json = "{\"data\":["
                                  + "{\"jsonGroupName\":" + PluSoft.Utils.JSON.Encode(publicDal.TableSearch(sqlGroupName)) + " }, "
                                  + "{\"jsonData\":" + PluSoft.Utils.JSON.Encode(publicDal.HashSearch(pageIndex, pageSize, "isnull(g.ID,0),a.FCreateDate", "desc", "", sql)) + " } "
                         + "]}"; break;
                case "4": sql = sql + " and a.ID in(" + GetUserPumpGroup() + ")";
                    json = PluSoft.Utils.JSON.Encode(publicDal.HashSearch(pageIndex, pageSize, "a.FCreateDate", "desc", "", sql)); break;
            }

            Response.Write(json);
        }

        public void Search_PumpCount()
        {
            string sql = string.Empty;
            string json = string.Empty;

            sql = @"select count(a.ID) as CountS
                      from Panda_Pump a
                 left join Panda_Customer b on a.FCustomerID=b.ID
                 left join A_U_DEP c on a.PCompanyNumber=c.U8number
                 left join sys_dictItems d on a.PProvince=d.FValue and d.FDictID=108
                 left join sys_dictItems e on a.PCity=e.FValue and e.FDictID=120 
                 left join Panda_PumpFG_P f on f.PumpID=a.ID
                 left join Panda_PumpFG g on f.GroupID=g.ID
                     where a.FIsDelete=0 ";

            string userType = GetUserType();  //登录用户类型

            switch (userType)
            {
                case "1": ; break;
                case "2": sql = sql + " and a.PCompanyNumber='" + GetUserCompanyNumber() + "'"; break;
                case "3": sql = sql + " and a.FCustomerID=" + GetUserCustomer(); break;
                case "4": sql = sql + " and a.ID in(" + GetUserPumpGroup() + ")"; break;
            }
            json = PluSoft.Utils.JSON.Encode(publicDal.TableSearch(sql));
            Response.Write(json);
        }

        /// <summary>
        /// 机组详情
        /// </summary>
        public void Search_PumpJZDetail()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                Guid _pumpID = Request["pumpID"] == null ? Guid.Empty : Guid.Parse(Request["pumpID"]);
                Guid _pumpJZID = Request["pumpJZID"] == null ? Guid.Empty : Guid.Parse(Request["pumpJZID"]);
                using (var DB = new DBController())
                {
                    var q = (from a in DB.Panda_Pump
                             select new
                             {
                                 pumpID = a.ID,
                                 a.PName,
                                 a.PCustomPName,
                                 a.PLngLat,
                                 a.FIsDelete,
                                 a.PCompanyNumber,
                                 FCustomerID = a.Panda_Customer.ID,
                                 a.TankIsSharing,
                                 a.PumpSoaking,
                                 a.Warning,
                                 a.WaterQualityDetection,
                                 a.ControlValve,
                                 a.WaterTankSterilizer,
                                 a.PumpLocation,
                                 a.MasterControlPLCIP,
                                 a.InstallDate,
                                 a.AcceptanceDate,
                                 a.WaterFloor,
                                 a.DressingCycle,
                                 pumpJZ = DB.Panda_PumpJZ.Select(b => new
                                 {
                                     pumpJZID = b.ID,
                                     b.PumpId,
                                     b.DTUCode,
                                     b.PumpJZName,
                                     b.MachineType,
                                     b.RunPumpNum,
                                     b.Auxiliarypumpcount,
                                     b.DrainPumpNum,
                                     IsAlarm = DB.Alarm_Timely.Where(y => y.BaseID.Equals(b.ID) && y.FMarkerType == 1 && y.FStatus == 1).Count() > 0 ? 1 : 0,
                                     b.PumpJZArea,
                                     b.AddressScheme.FName,
                                     FSchemeID = b.AddressScheme.ID,
                                     b.PumpJZCollectPeriod,
                                     b.PumpJZCollectLength,
                                     b.PumpJZReadMode,
                                     b.PumpJZTankVolume,
                                     b.PumpJZTankLength,
                                     b.PumpJZInletDiameter,
                                     b.PumpJZOutletDiameter,
                                     b.PumpJZBrandSet,
                                     b.PumpJZPumpBrand,
                                     b.PumpJZMainPumpFlow,
                                     b.PumpJZMainPumpLift,
                                     b.PumpJZAuxiliPumpFlow,
                                     b.PumpJZAuxiliPumpLift,
                                     b.PumpJZMainPumpPower,
                                     b.PumpJZAuxiliPumpPower,
                                     b.PumpJZPressReliValve,
                                     b.PumpJZPeak,
                                     b.PumpJZCheckValve,
                                     b.PumpJZInPUpper,
                                     b.PumpJZInPLower,
                                     b.PumpJZOutPUpper,
                                     b.PumpJZOutPLower,
                                     b.PumpJZReChlorUpper,
                                     b.PumpJZReChlorLower,
                                     b.PumpJZTurbidUpper,
                                     b.PumpJZTurbidLower,
                                     b.PumpJZPHUpper,
                                     b.PumpJZPHLower,
                                     b.PumpJZTankUpper,
                                     b.PumpJZTankLower,
                                     b.FIsDelete
                                     //pumpFile = DB.Panda_PumpDA.Select(d => new
                                     //{
                                     //    d.BaseId,
                                     //    d.FileName,
                                     //    d.FilePath,
                                     //    d.FileType,
                                     //    d.FileType2,
                                     //    d.uploadPageType,
                                     //    d.FileSize,
                                     //    d.UpDateTime,
                                     //    d.FPageSource
                                     //}).Where(d => d.BaseId.Equals(b.ID) && d.FPageSource.Equals("pumpJZ"))
                                 }).Where(b => b.PumpId.Equals(a.ID) && b.FIsDelete == 0 && b.pumpJZID.Equals(_pumpJZID)),
                                 pumpVQ = DB.Panda_PumpVQ.Select(c => new
                                 {
                                     c.PumpId,
                                     QuipmentType = DB.sys_dictItems.Where(i => i.FValue == c.QuipmentType && i.FDictID == 134).FirstOrDefault().FName,
                                     c.Type,
                                     c.Brand,
                                     c.Number,
                                     c.UserName,
                                     c.PassWord,
                                     c.Mark,
                                     c.IP,
                                     c.Port,
                                     c.IsActive,
                                     c.FOrderBy
                                 }).Where(c => c.PumpId.Equals(a.ID) && c.IsActive == false).OrderBy(c => c.FOrderBy)
                             }).Where(u => u.pumpID.Equals(_pumpID) && u.FIsDelete == 0);

                    string userType = GetUserType();
                    switch (userType)
                    {
                        case "1": ; break;
                        case "2":
                            string number = GetUserCompanyNumber();
                            q = q.Where(x => x.PCompanyNumber.Equals(number)); break;
                        case "3":
                            int customerid = Convert.ToInt32(GetUserCustomer());
                            q = q.Where(x => x.FCustomerID == customerid); break;
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
                            q = q.Where(x => igroup.Contains(x.pumpID))
                            ; break;
                    }
                    str = successMsg("查询成功", "true", js.Serialize(q));
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
        public void Search_Pump_JinChuWat()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                Guid _pumpID = Request["pumpID"] == null ? Guid.Empty : Guid.Parse(Request["pumpID"]);
                using (var DB = new DBController())
                {
                    var q = (from a in DB.Panda_Pump
                             select new
                             {
                                 pumpID = a.ID,
                                 a.FIsDelete,
                                 a.PCompanyNumber,
                                 FCustomerID = a.Panda_Customer.ID,
                                 pumpJZ = DB.Panda_PumpJZ.Select(b => new
                                 {
                                     pumpJZID = b.ID,
                                     b.PumpId,
                                     F41006 = DB.E_DATA_MAIN.Where(x => x.BaseID.Equals(b.ID)).FirstOrDefault().F41006,
                                     F41007 = DB.E_DATA_MAIN.Where(x => x.BaseID.Equals(b.ID)).FirstOrDefault().F41007,
                                     b.FIsDelete
                                 }).Where(b => b.PumpId.Equals(a.ID) && b.FIsDelete == 0)
                             }).Where(u => u.pumpID.Equals(_pumpID) && u.FIsDelete == 0);

                    string userType = GetUserType();
                    switch (userType)
                    {
                        case "1": ; break;
                        case "2":
                            string number = GetUserCompanyNumber();
                            q = q.Where(x => x.PCompanyNumber.Equals(number)); break;
                        case "3":
                            int customerid = Convert.ToInt32(GetUserCustomer());
                            q = q.Where(x => x.FCustomerID == customerid); break;
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
                            q = q.Where(x => igroup.Contains(x.pumpID))
                            ; break;
                    }
                    str = successMsg("查询成功", "true", js.Serialize(q));
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

        /// <summary>
        /// 附件
        /// </summary>
        public void Search_PumpFJ()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                string _pumpID = Request["pumpID"];
                DataTable table = Panda_PumpDADal.Search(" and baseId = '" + _pumpID + "' and FPageSource='pumpJZ'");
                str = successMsg("查询成功", "true", PluSoft.Utils.JSON.Encode(table));
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "查询失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }

        public ActionResult DownLoadFJ()
        {
            int id = Convert.ToInt32(Request["id"]);
            var q = db.Panda_PumpDA.Where(x => x.ID == id);
            string fileName = q.FirstOrDefault().FileName;
            //FileDownHelp.DownLoadold(fileName);
            fileName = HttpUtility.UrlEncode(fileName, Encoding.GetEncoding("UTF-8"));
            return File(Server.MapPath(q.FirstOrDefault().FilePath), Utilities.MimeType(fileName), fileName);
        }

        #endregion

        #region 参数设置
        public void EditControl()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                string _pumpID = Request["pumpID"];
                string TableField = Request["TableField"];  //表字段
                string text = Request["text"];        //字段值
                //DataTable dt = ControlSetDal.Search(" and PumpId=" + _pumpID);
                Hashtable has = new Hashtable();
                has["PumpId"] = _pumpID;
                has[TableField] = text;
                //if (dt.Rows.Count > 0)
                //{
                ControlSetDal.Update(has);
                str = successMsg("成功", "true");
                //}
                //else
                //{
                //    ControlSetDal.Insert(has);
                //    str = successMsg("成功", "true");
                //}
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }
        public void EditQXControl()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                string _pumpID = Request["pumpID"];
                string group = Request["group"];  //全选组
                string text = Request["text"];        //字段值
                //DataTable dt = ControlSetDal.Search(" and PumpId=" + _pumpID);
                Hashtable has = new Hashtable();
                has["PumpId"] = _pumpID;
                switch (group)
                {
                    case "bfhj": has["F_hj_yl"] = text;
                        has["F_hj_zd"] = text;
                        has["F_hj_ph"] = text;
                        has["F_hj_sw"] = text;
                        has["F_hj_rjy"] = text;
                        has["F_hj_ddl"] = text;
                        has["F_hj_sd"] = text;
                        has["F_hj_wd"] = text;
                        has["F_hj_wsyw"] = text; break; //泵房环境组
                    case "bfzt": has["F_zt_m"] = text;
                        has["F_zt_d"] = text;
                        has["F_zt_dy"] = text;
                        has["F_zt_pc"] = text;
                        has["F_zt_ups"] = text;
                        has["F_zt_ls"] = text;
                        has["F_zt_hj"] = text; break;    //泵房状态组
                    case "szmb": has["F_szmb_m"] = text;
                        has["F_szmb_d"] = text;
                        has["F_szmb_fswd"] = text;
                        has["F_szmb_psb"] = text;
                        has["F_szmb_xdy"] = text;
                        has["F_szmb_dsqh"] = text;
                        has["F_szmb_csyl"] = text;
                        has["F_szmb_ddf"] = text; break;    //设置面板组
                }
                //if (dt.Rows.Count > 0)
                //{
                ControlSetDal.Update(has);
                str = successMsg("成功", "true");
                //}
                //else
                //{
                //    ControlSetDal.Insert(has);
                //    str = successMsg("成功", "true");
                //}
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }

        public void Search_Control()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                string _pumpID = Request["pumpID"];
                DataTable dt1 = ControlSetDal.Search(" and PumpId='" + _pumpID + "'");
                if (dt1.Rows.Count == 0)
                {
                    Hashtable has = new Hashtable();
                    has["PumpId"] = _pumpID;
                    has["F_hj_yl"] = 1;
                    has["F_hj_zd"] = 1;
                    has["F_hj_ph"] = 1;
                    has["F_hj_sw"] = 1;
                    has["F_hj_rjy"] = 1;
                    has["F_hj_wd"] = 1;
                    has["F_yl_jsyl"] = 1;
                    has["F_yl_csyl"] = 1;
                    has["F_bpq_1"] = 1;
                    has["F_zt_m"] = 1;
                    has["F_zt_d"] = 1;
                    has["F_zt_dy"] = 1;
                    has["F_zt_pc"] = 1;
                    has["F_zt_ups"] = 1;
                    has["F_zt_ls"] = 1;
                    has["F_bpq_yxsj"] = 1;
                    has["F_bpq_yxpl"] = 1;
                    has["F_bpq_zldy"] = 1;
                    has["F_bpq_scdy"] = 1;
                    has["F_bpq_wd"] = 1;
                    has["F_bpq_gl"] = 1;
                    has["F_qt_jsll1"] = 1;
                    has["F_qt_csll1"] = 1;
                    has["F_qt_jlll1"] = 1;
                    has["F_qt_clll1"] = 1;
                    has["F_qt_ljdl"] = 1;
                    has["F_qt_jlll2"] = 1;
                    has["F_qt_clll2"] = 1;
                    has["F_szmb_m"] = 1;
                    has["F_szmb_d"] = 1;
                    has["F_szmb_fswd"] = 1;
                    has["F_szmb_psb"] = 1;
                    has["F_szmb_xdy"] = 1;
                    has["F_szmb_dsqh"] = 1;
                    has["F_szmb_csyl"] = 1;
                    has["F_szmb_ddf"] = 1;
                    has["F_szmb_sbjjqt"] = 1;
                    ControlSetDal.Insert(has);
                }
                DataTable dt2 = ControlSetDal.Search(" and PumpId='" + _pumpID + "'");
                str = successMsg("查询成功", "true", PluSoft.Utils.JSON.Encode(dt2));
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "查询失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }
        #endregion

        #region 报警
        public void SearchAlarm()
        {
            StringBuilder str = new StringBuilder();
            string pumpID = Request["pumpID"];   //搜索泵房id
            string pumpJZID = Request["pumpJZID"];   //搜索机组id
            //string Start = Request["StartDate"];   //开始日期
            //string End = Request["EndDate"];   //结束日期
            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);
            string FKey = Request["FKey"];
            try
            {
                string strwhere = string.Empty;
                if (pumpID != null && pumpID != "")
                {
                    strwhere = strwhere + " and b.PumpId='" + pumpID + "'";
                }
                if (pumpJZID != null && pumpJZID != "")
                {
                    strwhere = strwhere + " and b.ID='" + pumpJZID + "'";
                }
                if (FKey != null && FKey != "")
                {
                    strwhere = strwhere + " and FKey='" + FKey + "'";
                }
                //strwhere = strwhere + " and TempTime between '" + Start + "' and '" + End + "'";
                strwhere = strwhere + getPowerConst("pumpJZ");
                if (pageIndex >= 0 && pageSize > 0)
                {
                    Hashtable table = Panda_PumpJZDal.SearchAlarm(pageIndex, pageSize, "FAlarmTime", "desc", strwhere);
                    str = successMsg("查询成功", "true", PluSoft.Utils.JSON.Encode(table));
                }
                else
                {
                    DataTable table = Panda_PumpJZDal.SearchAlarm(strwhere);
                    str = successMsg("查询成功", "true", PluSoft.Utils.JSON.Encode(table));
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

        public void SearchAlarmHistory()
        {
            StringBuilder str = new StringBuilder();
            string pumpID = Request["pumpID"];   //搜索泵房id
            string pumpJZID = Request["pumpJZID"];   //搜索机组id
            string Start = Request["StartDate"];   //开始日期
            string End = Request["EndDate"];   //结束日期
            string FKey = Request["FKey"];
            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);
            try
            {
                string strwhere = string.Empty;
                if (pumpID != null && pumpID != "")
                {
                    strwhere = strwhere + " and b.PumpId='" + pumpID + "'";
                }
                if (pumpJZID != null && pumpJZID != "")
                {
                    strwhere = strwhere + " and b.ID='" + pumpJZID + "'";
                }
                if (FKey != null && FKey != "")
                {
                    strwhere = strwhere + " and FKey='" + FKey + "'";
                }
                strwhere = strwhere + " and FAlarmTime between '" + Start + "' and '" + End + "'";
                strwhere = strwhere + getPowerConst("pumpJZ");
                Hashtable table = Panda_PumpJZDal.SearchAlarmHistory(pageIndex, pageSize, "FAlarmTime", "desc", strwhere);
                str = successMsg("查询成功", "true", PluSoft.Utils.JSON.Encode(table));
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "查询失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }

        #region 报警显示参数
        public void Alarm_Param_UserSearch()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                DataTable dt1 = Alarm_Param_UserDal.Search(" and UserID='" + GetIdentityName() + "'");
                if (dt1.Rows.Count == 0)
                {
                    Hashtable has = new Hashtable();
                    has["UserID"] = GetIdentityName();
                    has["FVoice"] = 1;
                    has["FDisplay"] = 1;
                    has["FPosition"] = 1;
                    Alarm_Param_UserDal.Insert(has);
                }
                DataTable dt2 = Alarm_Param_UserDal.Search(" and UserID='" + GetIdentityName() + "'");
                str = successMsg("查询成功", "true", PluSoft.Utils.JSON.Encode(dt2));
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "查询失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }

        public void Alarm_Param_UserEdit()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                string TableField = Request["TableField"];  //表字段
                string text = Request["text"];        //字段值
                Hashtable has = new Hashtable();
                has["UserID"] = GetIdentityName();
                has[TableField] = text;

                Alarm_Param_UserDal.Update(has);
                str = successMsg("成功", "true");

            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }

        public void ALLAlarm_List()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                List<Alarm_Timely_A> list = new List<Alarm_Timely_A>();
                list.AddRange(from x in db.Alarm_Timely.Where(x => x.FMarkerType == 1 && x.FStatus == 1 && x.FAlarmTime != null)
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

                var q4 = (from x in db.Alarm_Timely.Where(x => x.FMarkerType == 4 && x.FStatus == 1)
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

                str = successMsg("查询成功", "true", js.Serialize(list));
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }

        #endregion
        #endregion

        #region 列表显示参数设置
        public void ParmSelect_MapView()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                DataTable dt = FLogParamDal.Search("");
                str = successMsg("查询成功", "true", PluSoft.Utils.JSON.Encode(dt));
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "查询失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }
        public void ParmSelect1()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                DataTable dt = FLogParamDal.Search(" and UserID='" + GetIdentityName() + "'", "IsSelect");
                str = successMsg("查询成功", "true", PluSoft.Utils.JSON.Encode(dt));
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "查询失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }
        public void ParmSelect2()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                DataTable dt = FLogParamDal.Search(" and UserID='" + GetIdentityName() + "'", "IsSelect2");
                str = successMsg("查询成功", "true", PluSoft.Utils.JSON.Encode(dt));
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "查询失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }

        public void EditParmSelect1()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                string FField = Request["FField"];        //字段值
                string value = Request["selectValue"];        //字段值
                Hashtable has = new Hashtable();
                has["UserID"] = GetIdentityName();
                has["FField"] = FField;
                has["IsSelect"] = value;
                FLogParamDal.Update(has);
                str = successMsg("成功", "true");
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }
        public void EditParmSelect2()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                string FField = Request["FField"];        //字段值
                string value = Request["selectValue"];        //字段值
                Hashtable has = new Hashtable();
                has["UserID"] = GetIdentityName();
                has["FField"] = FField;
                has["IsSelect2"] = value;
                FLogParamDal.Update(has);
                str = successMsg("成功", "true");
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }

        public void ParmSelect()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                DataTable dt = FLogParamDal.SearchParm();
                str = successMsg("查询成功", "true", PluSoft.Utils.JSON.Encode(dt));
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "查询失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }
        #endregion

        #region EChart

        /// <summary>
        /// 用水量和用电量
        /// </summary>
        public void dl_llTotal()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                string pumpJZID = Request["pumpJZID"];   //搜索机组id

                string sql = @"select last_FTotalDL,last_FTotalOutLL,now_FTotalDL,now_FTotalOutLL,
                               case when last_FTotalDL=0 then 0 else cast(((now_FTotalDL-last_FTotalDL)/last_FTotalDL)*100.00 as decimal(18,2)) end as dl_bf
                              ,case when last_FTotalOutLL=0 then 0 else cast(((now_FTotalOutLL-last_FTotalOutLL)/last_FTotalOutLL)*100.00 as decimal(18,2)) end as ll_bf
                                from (
                               select id=1, MAX(CONVERT(float,isnull(FTotalDL,0)))-MIN(CONVERT(float,isnull(FTotalDL,0))) AS last_FTotalDL,
                                      MAX(CONVERT(float,isnull(FTotalOutLL,0)))-MIN(CONVERT(float,isnull(FTotalOutLL,0))) AS last_FTotalOutLL
                                 from E_DATA" + DateTime.Now.Year + " where BaseID='" + pumpJZID + "' and datediff(MONTH,TempTime,getdate())=1 ) a,"
                               + @"(select id=1,MAX(CONVERT(float,isnull(FTotalDL,0)))-MIN(CONVERT(float,isnull(FTotalDL,0))) AS now_FTotalDL,
                                      MAX(CONVERT(float,isnull(FTotalOutLL,0)))-MIN(CONVERT(float,isnull(FTotalOutLL,0))) AS now_FTotalOutLL
                                 from E_DATA" + DateTime.Now.Year + " where BaseID='" + pumpJZID + "' and datediff(MONTH,TempTime,getdate())=0 ) b where a.id=b.id";
                DataTable dt = publicDal.TableSearch(sql);
                str = successMsg("查询成功", "true", PluSoft.Utils.JSON.Encode(dt));
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "查询失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }
        /// <summary>
        /// 进出水压力实时曲线图
        /// </summary>
        public void InOutWatPa()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                string pumpJZID = Request["pumpJZID"];   //搜索机组id
                int pageIndex = Convert.ToInt32(Request["pageIndex"]);
                int pageSize = Convert.ToInt32(Request["pageSize"]);
                string Start = Request["StartDate"];   //开始日期
                string End = Request["EndDate"];   //结束日期
                int year = DateTime.Now.Year;
                string sql = string.Empty;
                if (Start != null && Start != "" && End != null && End != "")
                {
                    year = Convert.ToDateTime(Start).Year;
                    sql = @"select CONVERT(varchar(10),FCreateDate,24) as day_time,isnull(F41006,0) as F41006,isnull(F41007,0) as F41007,isnull(F41702,0) as F41702 from E_DATA" + year + " where BaseID='" + pumpJZID + "'"
                        + " and TempTime between '" + Start + "' and '" + End + "'";
                }
                else
                {
                    sql = @"select CONVERT(varchar(10),FCreateDate,24) as day_time,isnull(F41006,0) as F41006,isnull(F41007,0) as F41007,isnull(F41702,0) as F41702 from E_DATA" + year + " where BaseID='" + pumpJZID + "' and datediff(day,TempTime,getdate())=0";
                }

                Hashtable result = publicDal.HashSearch(pageIndex, pageSize, "TempTime", "desc", "", sql);
                str = successMsg("查询成功", "true", PluSoft.Utils.JSON.Encode(result));
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "查询失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }

        /// <summary>
        /// 近七日用水量对比图
        /// </summary>
        public void UseWater7()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                string pumpJZID = Request["pumpJZID"];   //搜索机组id

                string sql = @" select isnull(day_time,convert(char,dateadd(DD,-6,getdate()),23)) as T_Time,0 result,isnull(data_max,0) as data_max,isnull(data_min,0) as data_min from (select 1 as 'a') a left join (
                                       select CONVERT(varchar(10),TempTime,23) as day_time,
                                      MAX(FTotalOutLL) as data_max,MIN(FTotalOutLL) as data_min
                                 from E_DATA" + DateTime.Now.AddDays(-6).Year + " "
                                     + "where BaseID='" + pumpJZID + "' and FTotalOutLL>0 and FTotalOutLL is not null and datediff(day,TempTime,getdate())=6 group by CONVERT(varchar(10),TempTime,23)) b on 1=1"
                               + @"union all
                               select isnull(day_time,convert(char,dateadd(DD,-5,getdate()),23)) as T_Time,0 result,isnull(data_max,0) as data_max,isnull(data_min,0) as data_min from (select 1 as 'a') a left join (
                               select CONVERT(varchar(10),TempTime,23) as day_time,
                                      MAX(FTotalOutLL) as data_max,MIN(FTotalOutLL) as data_min
                                 from E_DATA" + DateTime.Now.AddDays(-5).Year + " "
                                      + "where BaseID='" + pumpJZID + "' and FTotalOutLL>0 and FTotalOutLL is not null and datediff(day,TempTime,getdate())=5  group by CONVERT(varchar(10),TempTime,23)) b on 1=1"
                               + @"union all
select isnull(day_time,convert(char,dateadd(DD,-4,getdate()),23)) as T_Time,0 result,isnull(data_max,0) as data_max,isnull(data_min,0) as data_min from (select 1 as 'a') a left join (
                               select CONVERT(varchar(10),TempTime,23) as day_time,
                                     MAX(FTotalOutLL) as data_max,MIN(FTotalOutLL) as data_min
                                 from E_DATA" + DateTime.Now.AddDays(-4).Year + " "
                                     + " where BaseID='" + pumpJZID + "' and FTotalOutLL>0 and FTotalOutLL is not null and datediff(day,TempTime,getdate())=4  group by CONVERT(varchar(10),TempTime,23)) b on 1=1"
                               + @"union all
select isnull(day_time,convert(char,dateadd(DD,-3,getdate()),23)) as T_Time,0 result,isnull(data_max,0) as data_max,isnull(data_min,0) as data_min from (select 1 as 'a') a left join (
                               select CONVERT(varchar(10),TempTime,23) as day_time,
                                     MAX(FTotalOutLL) as data_max,MIN(FTotalOutLL) as data_min
                                 from E_DATA" + DateTime.Now.AddDays(-3).Year + " "
                                     + " where BaseID='" + pumpJZID + "' and FTotalOutLL>0 and FTotalOutLL is not null and datediff(day,TempTime,getdate())=3  group by CONVERT(varchar(10),TempTime,23)) b on 1=1"
                               + @"union all
select isnull(day_time,convert(char,dateadd(DD,-2,getdate()),23)) as T_Time,0 result,isnull(data_max,0) as data_max,isnull(data_min,0) as data_min from (select 1 as 'a') a left join (
                               select CONVERT(varchar(10),TempTime,23) as day_time,
                                     MAX(FTotalOutLL) as data_max,MIN(FTotalOutLL) as data_min
                                 from E_DATA" + DateTime.Now.AddDays(-2).Year + " "
                                     + " where BaseID='" + pumpJZID + "' and FTotalOutLL>0 and FTotalOutLL is not null and datediff(day,TempTime,getdate())=2  group by CONVERT(varchar(10),TempTime,23)) b on 1=1"
                               + @"union all
select isnull(day_time,convert(char,dateadd(DD,-1,getdate()),23)) as T_Time,0 result,isnull(data_max,0) as data_max,isnull(data_min,0) as data_min from (select 1 as 'a') a left join (
                               select CONVERT(varchar(10),TempTime,23) as day_time,
                                    MAX(FTotalOutLL) as data_max,MIN(FTotalOutLL) as data_min
                                 from E_DATA" + DateTime.Now.AddDays(-1).Year + " "
                                     + " where BaseID='" + pumpJZID + "' and FTotalOutLL>0 and FTotalOutLL is not null and datediff(day,TempTime,getdate())=1  group by CONVERT(varchar(10),TempTime,23)) b on 1=1"
                               + @"union all
select isnull(day_time,convert(char,dateadd(DD,-0,getdate()),23)) as T_Time,0 result,isnull(data_max,0) as data_max,isnull(data_min,0) as data_min from (select 1 as 'a') a left join (
                               select CONVERT(varchar(10),TempTime,23) as day_time,
                                    MAX(FTotalOutLL) as data_max,MIN(FTotalOutLL) as data_min
                                 from E_DATA" + DateTime.Now.Year + " "
                                     + " where BaseID='" + pumpJZID + "' and FTotalOutLL>0 and FTotalOutLL is not null and datediff(day,TempTime,getdate())=0  group by CONVERT(varchar(10),TempTime,23)) b on 1=1";
                DataTable dt = publicDal.TableSearch(sql);
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (j < dt.Rows.Count - 1)
                    {
                        if (Convert.ToInt32(dt.Rows[j]["data_min"]) > Convert.ToInt32(dt.Rows[j]["data_max"]))
                        {
                            dt.Rows[j]["data_max"] = dt.Rows[j]["data_min"];
                        }
                        dt.Rows[j + 1]["data_min"] = dt.Rows[j]["data_max"];
                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[j]["data_min"]) > Convert.ToInt32(dt.Rows[j]["data_max"]))
                        {
                            dt.Rows[j]["data_max"] = dt.Rows[j]["data_min"];
                        }
                    }
                    dt.Rows[j]["result"] = Convert.ToInt32(dt.Rows[j]["data_max"]) - Convert.ToInt32(dt.Rows[j]["data_min"]);
                }
                str = successMsg("查询成功", "true", PluSoft.Utils.JSON.Encode(dt));
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "查询失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }

        /// <summary>
        /// 近七日用电量对比图
        /// </summary>
        public void UseElectric7()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                string pumpJZID = Request["pumpJZID"];   //搜索机组id

                string sql = @"select isnull(day_time,convert(char,dateadd(DD,-6,getdate()),23)) as T_Time,0 result,isnull(data_max,0) as data_max,isnull(data_min,0) as data_min from (select 1 as 'a') a left join (
select CONVERT(varchar(10),TempTime,23) as day_time,
                                      MAX(FTotalDL) as data_max,MIN(FTotalDL) as data_min 
                                 from E_DATA" + DateTime.Now.AddDays(-6).Year + " "
                                     + "where BaseID='" + pumpJZID + "' and FTotalDL>0 and FTotalDL is not null and datediff(day,TempTime,getdate())=6   group by CONVERT(varchar(10),TempTime,23)) b on 1=1"
                               + @"union all
select isnull(day_time,convert(char,dateadd(DD,-5,getdate()),23)) as T_Time,0 result,isnull(data_max,0) as data_max,isnull(data_min,0) as data_min from (select 1 as 'a') a left join (
                               select CONVERT(varchar(10),TempTime,23) as day_time,
                                      MAX(FTotalDL) as data_max,MIN(FTotalDL) as data_min
                                 from E_DATA" + DateTime.Now.AddDays(-5).Year + " "
                                      + "where BaseID='" + pumpJZID + "' and FTotalDL>0 and FTotalDL is not null and datediff(day,TempTime,getdate())=5   group by CONVERT(varchar(10),TempTime,23)) b on 1=1"
                               + @"union all
select isnull(day_time,convert(char,dateadd(DD,-4,getdate()),23)) as T_Time,0 result,isnull(data_max,0) as data_max,isnull(data_min,0) as data_min from (select 1 as 'a') a left join (
                               select CONVERT(varchar(10),TempTime,23) as day_time,
                                      MAX(FTotalDL) as data_max,MIN(FTotalDL) as data_min
                                 from E_DATA" + DateTime.Now.AddDays(-4).Year + " "
                                     + " where BaseID='" + pumpJZID + "' and FTotalDL>0 and FTotalDL is not null and datediff(day,TempTime,getdate())=4   group by CONVERT(varchar(10),TempTime,23)) b on 1=1"
                               + @"union all
select isnull(day_time,convert(char,dateadd(DD,-3,getdate()),23)) as T_Time,0 result,isnull(data_max,0) as data_max,isnull(data_min,0) as data_min from (select 1 as 'a') a left join (
                               select CONVERT(varchar(10),TempTime,23) as day_time,
                                      MAX(FTotalDL) as data_max,MIN(FTotalDL) as data_min
                                 from E_DATA" + DateTime.Now.AddDays(-3).Year + " "
                                     + " where BaseID='" + pumpJZID + "' and FTotalDL>0 and FTotalDL is not null and datediff(day,TempTime,getdate())=3   group by CONVERT(varchar(10),TempTime,23)) b on 1=1"
                               + @"union all
select isnull(day_time,convert(char,dateadd(DD,-2,getdate()),23)) as T_Time,0 result,isnull(data_max,0) as data_max,isnull(data_min,0) as data_min from (select 1 as 'a') a left join (
                               select CONVERT(varchar(10),TempTime,23) as day_time,
                                      MAX(FTotalDL) as data_max,MIN(FTotalDL) as data_min
                                 from E_DATA" + DateTime.Now.AddDays(-2).Year + " "
                                     + " where BaseID='" + pumpJZID + "' and FTotalDL>0 and FTotalDL is not null and datediff(day,TempTime,getdate())=2   group by CONVERT(varchar(10),TempTime,23)) b on 1=1"
                               + @"union all
select isnull(day_time,convert(char,dateadd(DD,-1,getdate()),23)) as T_Time,0 result,isnull(data_max,0) as data_max,isnull(data_min,0) as data_min from (select 1 as 'a') a left join (
                               select CONVERT(varchar(10),TempTime,23) as day_time,
                                      MAX(FTotalDL) as data_max,MIN(FTotalDL) as data_min
                                 from E_DATA" + DateTime.Now.AddDays(-1).Year + " "
                                     + " where BaseID='" + pumpJZID + "' and FTotalDL>0 and FTotalDL is not null and datediff(day,TempTime,getdate())=1   group by CONVERT(varchar(10),TempTime,23)) b on 1=1"
                               + @"union all
select isnull(day_time,convert(char,dateadd(DD,-0,getdate()),23)) as T_Time,0 result,isnull(data_max,0) as data_max,isnull(data_min,0) as data_min from (select 1 as 'a') a left join (
                               select CONVERT(varchar(10),TempTime,23) as day_time,
                                      MAX(FTotalDL) as data_max,MIN(FTotalDL) as data_min
                                 from E_DATA" + DateTime.Now.Year + " "
                                     + " where BaseID='" + pumpJZID + "' and FTotalDL>0 and FTotalDL is not null and datediff(day,TempTime,getdate())=0   group by CONVERT(varchar(10),TempTime,23)) b on 1=1";

                DataTable dt = publicDal.TableSearch(sql);
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (j < dt.Rows.Count - 1)
                    {
                        if (Convert.ToInt32(dt.Rows[j]["data_min"]) > Convert.ToInt32(dt.Rows[j]["data_max"]))
                        {
                            dt.Rows[j]["data_max"] = dt.Rows[j]["data_min"];
                        }
                        dt.Rows[j + 1]["data_min"] = dt.Rows[j]["data_max"];
                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[j]["data_min"]) > Convert.ToInt32(dt.Rows[j]["data_max"]))
                        {
                            dt.Rows[j]["data_max"] = dt.Rows[j]["data_min"];
                        }
                    }
                    dt.Rows[j]["result"] = Convert.ToInt32(dt.Rows[j]["data_max"]) - Convert.ToInt32(dt.Rows[j]["data_min"]);
                }
                str = successMsg("查询成功", "true", PluSoft.Utils.JSON.Encode(dt));
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "查询失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }

        /// <summary>
        /// 泵房机组数量信息
        /// </summary>
        public void Pump_PumpJZ_Count()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                string sql = @"select '泵房' _name, count(*) as _count from Panda_Pump a where FIsDelete=0 " + getPowerConst("pump")
                               + @" union all
                               select '机组' _name,count(a.ID) as _count from Panda_PumpJZ a,Panda_Pump c where a.PumpId=c.ID and a.FIsDelete=0  and c.FIsDelete=0 " + getPowerConst("pumpJZ")
                               + @" union all
                               select left(b.FName,2) as _name,count(*) as _count from Panda_PumpJZ a,sys_dictItems b,Panda_Pump c where a.PumpId=c.ID and a.MachineType=b.FValue and b.FDictID=132
                                and a.FIsDelete=0 and c.FIsDelete=0 " + getPowerConst("pumpJZ") + " group by left( b.FName,2) ";
                DataTable dt = publicDal.TableSearch(sql);
                str = successMsg("查询成功", "true", PluSoft.Utils.JSON.Encode(dt));
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "查询失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }

        /// <summary>
        /// 年用电量前十机组
        /// </summary>
        public void DL_PumpJZTop10()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                string timeType = Request["timeType"];   //搜索年/月/日
                string timeStr = string.Empty;

                switch (timeType)
                {
                    case "year": timeStr = " "; break;
                    case "month": timeStr = " and TempTime>='" + new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd") + " 00:00:00' and TempTime<'" + new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).ToString("yyyy-MM-dd") + " 00:00:00'"; break;
                    case "day": ; timeStr = " and TempTime>='" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00' and TempTime<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + " 00:00:00'"; break;
                }

                //                string sql = @"select top 10 c.ID, a.BaseID, b.PumpJZName,c.PAddress, MAX(FTotalDL)-MIN(FTotalDL) AS FTotalDL
                //                                from E_DATA" + DateTime.Now.Year + " a, Panda_PumpJZ b,Panda_Pump c "
                //                          + @"  where b.PumpId=c.ID and b.FIsDelete=0 and c.FIsDelete=0  and FTotalDL>0 and FTotalDL is not null 
                //                                 and a.BaseID=b.ID and " + timeStr + " " + getPowerConst("pumpJZ")
                //                        + @" group by a.BaseID,PumpJZName,c.PAddress,c.ID
                //                            order by FTotalDL desc ";
                string sql = @"with top10 as (
                             select top 10 BaseID,MAX(FTotalDL)-MIN(FTotalDL) AS FTotalDL from E_DATA" + DateTime.Now.Year + " a,Panda_PumpJZ b,Panda_Pump c "
                         + @"   where 1=1 " + timeStr + " " + getPowerConst("pumpJZ") + " and a.BASEID=b.ID and b.PumpId=c.ID and b.FIsDelete=0 and c.FIsDelete=0 and FTotalDL>0 and FTotalDL is not null group by BaseID order by FTotalDL desc)"
                          + @"  select c.ID,a.BaseID,b.PumpJZName,c.PAddress,FTotalDL from Panda_PumpJZ b,Panda_Pump c ,top10 a 
                              where b.PumpId=c.ID and b.FIsDelete=0 and c.FIsDelete=0 and a.BaseID=b.ID
                              order by FTotalDL desc ";
                DataTable dt = publicDal.TableSearch(sql);
                str = successMsg("查询成功", "true", PluSoft.Utils.JSON.Encode(dt));
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "查询失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }
        /// <summary>
        /// 年用水量前十机组
        /// </summary>
        public void SL_PumpJZTop10()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                string timeType = Request["timeType"];   //搜索年/月/日
                string timeStr = string.Empty;

                switch (timeType)
                {
                    case "year": timeStr = " "; break;
                    case "month": timeStr = "and TempTime>='" + new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd") + " 00:00:00' and TempTime<'" + new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).ToString("yyyy-MM-dd") + " 00:00:00'"; break;
                    case "day": ; timeStr = "and TempTime>='" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00' and TempTime<'" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + " 00:00:00'"; break;
                }
                //                string sql = @"select top 10 c.ID,a.BaseID,b.PumpJZName,c.PAddress, MAX(FTotalOutLL)-MIN(FTotalOutLL) AS FTotalOutLL
                //                                from E_DATA" + DateTime.Now.Year + " a, Panda_PumpJZ b,Panda_Pump c "
                //                          + @"  where b.PumpId=c.ID and b.FIsDelete=0 and c.FIsDelete=0  and FTotalOutLL>0 and FTotalOutLL is not null 
                //                                 and a.BaseID=b.ID and " + timeStr + " " + getPowerConst("pumpJZ")
                //                        + @" group by a.BaseID,PumpJZName ,c.PAddress,c.ID
                //                            order by FTotalOutLL desc ";
                string sql = @"with top10 as (
                             select top 10 BaseID,MAX(FTotalOutLL)-MIN(FTotalOutLL) AS FTotalOutLL from E_DATA" + DateTime.Now.Year + " a,Panda_PumpJZ b,Panda_Pump c "
                           + @"   where 1=1 " + timeStr + " " + getPowerConst("pumpJZ") + " and a.BASEID=b.ID and b.PumpId=c.ID and b.FIsDelete=0 and c.FIsDelete=0 and FTotalOutLL>0 and FTotalOutLL is not null group by BaseID order by FTotalOutLL desc)"
                            + @"  select c.ID,a.BaseID,b.PumpJZName,c.PAddress,FTotalOutLL from Panda_PumpJZ b,Panda_Pump c ,top10 a 
                              where b.PumpId=c.ID and b.FIsDelete=0 and c.FIsDelete=0 and a.BaseID=b.ID
                              order by FTotalOutLL desc ";
                DataTable dt = publicDal.TableSearch(sql);
                str = successMsg("查询成功", "true", PluSoft.Utils.JSON.Encode(dt));
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "查询失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }

        /// <summary>
        /// （年/月/日）用电量分析对比
        /// </summary>
        public void DL_PumpJZ_YMD()
        {
            StringBuilder str = new StringBuilder();
            String json = "";
            try
            {
                string pumpJZID = Request["pumpJZID"];   //搜索机组id
                string timeType = Request["timeType"];   //搜索年/月/日
                String json1 = "";
                String json2 = "";
                String json3 = "";
                switch (timeType)
                {
                    case "1":  //年
                        json1 = PluSoft.Utils.JSON.Encode(E_DATADal.Search_Y(0, DateTime.Now.ToString("yyyy-MM-dd"), pumpJZID, "FTotalDL"));
                        DataTable dt1 = E_DATADal.Search_Y(1, DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd"), pumpJZID, "FTotalDL");
                        if (dt1 == null)
                        {
                            json2 = "[]";
                        }
                        else
                        {
                            json2 = PluSoft.Utils.JSON.Encode(dt1);
                        }
                        DataTable dt2 = E_DATADal.Search_Y(2, DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd"), pumpJZID, "FTotalDL");
                        if (dt2 == null)
                        {
                            json3 = "[]";
                        }
                        else
                        {
                            json3 = PluSoft.Utils.JSON.Encode(dt2);
                        }
                        json = "{\"data\":["
                               + "{\"field\":\"" + DateTime.Now.Year.ToString() + "年" + "\",\"detailData\":" + json1 + " }, "
                               + "{\"field\":\"" + DateTime.Now.AddYears(-1).Year.ToString() + "年" + "\",\"detailData\":" + json2 + " }, "
                               + "{\"field\":\"" + DateTime.Now.AddYears(-2).Year.ToString() + "年" + "\",\"detailData\":" + json3 + " } "
                      + "]}"; break;
                    case "2": //月
                        json1 = PluSoft.Utils.JSON.Encode(E_DATADal.Search_M(0, DateTime.Now.ToString("yyyy-MM-dd"), pumpJZID, "FTotalDL"));
                        json2 = PluSoft.Utils.JSON.Encode(E_DATADal.Search_M(1, DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"), pumpJZID, "FTotalDL"));
                        json3 = PluSoft.Utils.JSON.Encode(E_DATADal.Search_M(2, DateTime.Now.AddMonths(-2).ToString("yyyy-MM-dd"), pumpJZID, "FTotalDL"));
                        json = "{\"data\":["
                               + "{\"field\":\"" + DateTime.Now.Month.ToString() + "月" + "\",\"detailData\":" + json1 + " }, "
                               + "{\"field\":\"" + DateTime.Now.AddMonths(-1).Month.ToString() + "月" + "\",\"detailData\":" + json2 + " }, "
                               + "{\"field\":\"" + DateTime.Now.AddMonths(-2).Month.ToString() + "月" + "\",\"detailData\":" + json3 + " } "
                      + "]}"; break;
                    case "3": //日
                        json1 = PluSoft.Utils.JSON.Encode(E_DATADal.Search_D(0, DateTime.Now.ToString("yyyy-MM-dd"), pumpJZID, "FTotalDL"));
                        json2 = PluSoft.Utils.JSON.Encode(E_DATADal.Search_D(1, DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"), pumpJZID, "FTotalDL"));
                        json3 = PluSoft.Utils.JSON.Encode(E_DATADal.Search_D(2, DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd"), pumpJZID, "FTotalDL"));
                        json = "{\"data\":["
                               + "{\"field\":\"" + DateTime.Now.ToString("yyyy-MM-dd") + "\",\"detailData\":" + json1 + " }, "
                               + "{\"field\":\"" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "\",\"detailData\":" + json2 + " }, "
                               + "{\"field\":\"" + DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd") + "\",\"detailData\":" + json3 + " } "
                      + "]}"; break;
                }
                str = successMsg("查询成功", "true", json);
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "查询失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }

        /// <summary>
        /// （年/月/日）用水量分析对比
        /// </summary>
        public void SL_PumpJZ_YMD()
        {
            StringBuilder str = new StringBuilder();
            String json = "";
            try
            {
                string pumpJZID = Request["pumpJZID"];   //搜索机组id
                string timeType = Request["timeType"];   //搜索年/月/日
                String json1 = "";
                String json2 = "";
                String json3 = "";
                switch (timeType)
                {
                    case "1":  //年
                        json1 = PluSoft.Utils.JSON.Encode(E_DATADal.Search_Y(0, DateTime.Now.ToString("yyyy-MM-dd"), pumpJZID, "FTotalOutLL"));
                        DataTable dt1 = E_DATADal.Search_Y(1, DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd"), pumpJZID, "FTotalOutLL");
                        if (dt1 == null)
                        {
                            json2 = "[]";
                        }
                        else
                        {
                            json2 = PluSoft.Utils.JSON.Encode(dt1);
                        }
                        DataTable dt2 = E_DATADal.Search_Y(2, DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd"), pumpJZID, "FTotalOutLL");
                        if (dt2 == null)
                        {
                            json3 = "[]";
                        }
                        else
                        {
                            json3 = PluSoft.Utils.JSON.Encode(dt2);
                        }
                        json = "{\"data\":["
                               + "{\"field\":\"" + DateTime.Now.Year.ToString() + "年" + "\",\"detailData\":" + json1 + " }, "
                               + "{\"field\":\"" + DateTime.Now.AddYears(-1).Year.ToString() + "年" + "\",\"detailData\":" + json2 + " }, "
                               + "{\"field\":\"" + DateTime.Now.AddYears(-2).Year.ToString() + "年" + "\",\"detailData\":" + json3 + " } "
                      + "]}"; break;
                    case "2": //月
                        json1 = PluSoft.Utils.JSON.Encode(E_DATADal.Search_M(0, DateTime.Now.ToString("yyyy-MM-dd"), pumpJZID, "FTotalDL"));
                        json2 = PluSoft.Utils.JSON.Encode(E_DATADal.Search_M(1, DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"), pumpJZID, "FTotalOutLL"));
                        json3 = PluSoft.Utils.JSON.Encode(E_DATADal.Search_M(2, DateTime.Now.AddMonths(-2).ToString("yyyy-MM-dd"), pumpJZID, "FTotalOutLL"));
                        json = "{\"data\":["
                               + "{\"field\":\"" + DateTime.Now.Month.ToString() + "月" + "\",\"detailData\":" + json1 + " }, "
                               + "{\"field\":\"" + DateTime.Now.AddMonths(-1).Month.ToString() + "月" + "\",\"detailData\":" + json2 + " }, "
                               + "{\"field\":\"" + DateTime.Now.AddMonths(-2).Month.ToString() + "月" + "\",\"detailData\":" + json3 + " } "
                      + "]}"; break;
                    case "3": //日
                        json1 = PluSoft.Utils.JSON.Encode(E_DATADal.Search_D(0, DateTime.Now.ToString("yyyy-MM-dd"), pumpJZID, "FTotalOutLL"));
                        json2 = PluSoft.Utils.JSON.Encode(E_DATADal.Search_D(1, DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"), pumpJZID, "FTotalOutLL"));
                        json3 = PluSoft.Utils.JSON.Encode(E_DATADal.Search_D(2, DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd"), pumpJZID, "FTotalOutLL"));
                        json = "{\"data\":["
                               + "{\"field\":\"" + DateTime.Now.ToString("yyyy-MM-dd") + "\",\"detailData\":" + json1 + " }, "
                               + "{\"field\":\"" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "\",\"detailData\":" + json2 + " }, "
                               + "{\"field\":\"" + DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd") + "\",\"detailData\":" + json3 + " } "
                      + "]}"; break;
                }
                str = successMsg("查询成功", "true", json);
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "查询失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }

        /// <summary>
        /// （当年/月/日）吨水能耗
        /// </summary>
        public void SearchDSNH()
        {
            StringBuilder str = new StringBuilder();
            String json = "";
            try
            {
                string pumpJZID = Request["pumpJZID"];   //搜索机组id
                string timeType = Request["timeType"];   //搜索年/月/日
                string year = Request["year"];
                string month = Request["month"];
                string day = Request["day"];
                string s_date = Request["s_date"];
                string e_date = Request["e_date"];
                switch (timeType)
                {
                    case "1":  //年
                        json = PluSoft.Utils.JSON.Encode(E_DATADal.SearchDSNH_Y(pumpJZID, year)); break;
                    case "2": //月
                        json = PluSoft.Utils.JSON.Encode(E_DATADal.SearchDSNH_M(pumpJZID, year, month)); break;
                    case "3": //日
                        json = PluSoft.Utils.JSON.Encode(E_DATADal.SearchDSNH_D(pumpJZID, day)); break;
                    case "4": //区间
                        json = PluSoft.Utils.JSON.Encode(E_DATADal.SearchDSNH_Q(pumpJZID, s_date, e_date)); break;
                }
                str = successMsg("查询成功", "true", json);
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "查询失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }

        /// <summary>
        /// （当年/月/日）吨水能耗
        /// </summary>
        public void SearchDSNHResult()
        {
            StringBuilder str = new StringBuilder();
            String json = string.Empty;
            try
            {
                string pumpJZID = Request["pumpJZID"];   //搜索机组id
                string timeType = Request["timeType"];   //搜索年/月/日
                string year = Request["year"];
                string month = Request["month"];
                string day = Request["day"];
                string s_date = Request["s_date"];
                string e_date = Request["e_date"];
                string dateStr = string.Empty;
                DataTable dt = new DataTable();
                dt.Columns.Add("FTotalDL", typeof(string));
                dt.Columns.Add("FTotalOutLL", typeof(string));
                switch (timeType)
                {
                    case "1": dateStr = "year(TempTime)='" + year + "'";  //年
                        break;
                    case "2": dateStr = "year(TempTime)='" + year + "' and month(TempTime)='" + month + "'";  //月
                        break;
                    case "3": dateStr = "CONVERT(varchar(10),TempTime,120)='" + day + "'"; year = Convert.ToDateTime(day).Year.ToString();  //日
                        break;
                    case "4": dateStr = "(TempTime between '" + s_date + "' and '" + e_date + "')"; year = Convert.ToDateTime(s_date).Year.ToString();  //区间
                        break;
                }
                string sql1 = @"select top 1 FTotalDL,FTotalOutLL from E_DATA" + year + " where " + dateStr + " and BaseID='" + pumpJZID + "' order by TempTime asc";
                string sql2 = @"select top 1 FTotalDL,FTotalOutLL from E_DATA" + year + " where " + dateStr + " and BaseID='" + pumpJZID + "' order by TempTime desc";

                DataTable dt1 = publicDal.TableSearch(sql1);
                DataTable dt2 = publicDal.TableSearch(sql2);
                if (dt1.Rows.Count > 0 && dt2.Rows.Count > 0)
                {
                    DataRow row1 = dt.NewRow();
                    row1["FTotalDL"] = dt1.Rows[0]["FTotalDL"];
                    row1["FTotalOutLL"] = dt1.Rows[0]["FTotalOutLL"];
                    dt.Rows.Add(row1);
                    DataRow row2 = dt.NewRow();
                    row2["FTotalDL"] = dt2.Rows[dt2.Rows.Count - 1]["FTotalDL"];
                    row2["FTotalOutLL"] = dt2.Rows[dt2.Rows.Count - 1]["FTotalOutLL"];
                    dt.Rows.Add(row2);
                }
                json = PluSoft.Utils.JSON.Encode(dt);
                if (string.IsNullOrEmpty(json))
                {
                    json = "[]";
                }
                str = successMsg("查询成功", "true", json);
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "查询失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(json);
        }

        /// <summary>
        /// 进出水压力,进出水瞬时流量，水箱液位，出水压力设定实时曲线图
        /// </summary>
        public void InOutWat_Flow_YW_YL()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                string pumpJZID = Request["pumpJZID"];   //搜索机组id
                int pageIndex = Convert.ToInt32(Request["pageIndex"]);
                int pageSize = Convert.ToInt32(Request["pageSize"]);
                string Start = Request["StartDate"];   //开始日期
                string End = Request["EndDate"];   //结束日期
                int year = DateTime.Now.Year;
                string sql = string.Empty;
                if (Start != null && Start != "" && End != null && End != "")
                {
                    year = Convert.ToDateTime(Start).Year;
                    sql = @"select CONVERT(varchar(10),TempTime,24) as day_time,isnull(F41006,0) as F41006,isnull(F41007,0) as F41007,isnull(F41024,0) as F41024,isnull(F41025,0) as F41025,isnull(F41020,0) as F41020,isnull(F41702,0) as F41702 from E_DATA" + year + " with(nolock) where BaseID='" + pumpJZID + "'"
                        + " and TempTime between '" + Start + "' and '" + End + "'";
                }
                else
                {
                    sql = @"select CONVERT(varchar(10),TempTime,24) as day_time,isnull(F41006,0) as F41006,isnull(F41007,0) as F41007,isnull(F41024,0) as F41024,isnull(F41025,0) as F41025,isnull(F41020,0) as F41020,isnull(F41702,0) as F41702 from E_DATA" + year + " with(nolock) where BaseID='" + pumpJZID + "' and datediff(day,TempTime,getdate())=0";
                }

                Hashtable result = publicDal.HashSearch(pageIndex, pageSize, "BaseID,TempTime", "desc", "", sql);
                str = successMsg("查询成功", "true", PluSoft.Utils.JSON.Encode(result));
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "查询失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }
        #endregion

        #region 轮询（轮流查询）
        public void LunXunSelect()
        {
            StringBuilder str = new StringBuilder();
            int UserID = Convert.ToInt32(GetIdentityName());
            try
            {
                DataTable dt = publicDal.TableSearch("select top 1 * from LunXunParam where UserID='" + UserID + "'");
                if (dt.Rows.Count == 0)
                {
                    string insert = @"insert into LunXunParam 
                                      select 1 as FField,NULL as BaseID," + UserID + " as UserID union all "
                                   + "select 2 as FField,NULL as BaseID," + UserID + " as UserID union all "
                                   + "select 3 as FField,NULL as BaseID," + UserID + " as UserID union all "
                                   + "select 4 as FField,NULL as BaseID," + UserID + " as UserID union all "
                                   + "select 5 as FField,NULL as BaseID," + UserID + " as UserID union all "
                                   + "select 6 as FField,NULL as BaseID," + UserID + " as UserID union all "
                                   + "select 7 as FField,NULL as BaseID," + UserID + " as UserID union all "
                                   + "select 8 as FField,NULL as BaseID," + UserID + " as UserID union all "
                                   + "select 9 as FField,NULL as BaseID," + UserID + " as UserID";
                    publicDal.TableSearch(insert);

                    str = successMsg("无数据", "true", "[]");
                }
                else
                {
                    using (var DB = new DBController())
                    {
                        Guid _BaseID = Guid.Empty;

                        var q = (from _lx in DB.LunXunParam
                                 select new
                                 {
                                     _lx.ID,
                                     _lx.FField,
                                     _lx.BaseID,
                                     _lx.UserID,
                                     pumpJZ = DB.Panda_PumpJZ.Select(b => new
                                      {
                                          pump = DB.Panda_Pump.Select(c => new
                                          {
                                              pumpID = c.ID,
                                              c.PName,
                                              c.PCustomPName,
                                              c.PLngLat,
                                              c.FIsDelete,
                                              c.PCompanyNumber,
                                              FCustomerID = c.Panda_Customer.ID,
                                              c.TankIsSharing,
                                              c.PumpSoaking,
                                              c.Warning,
                                              c.WaterQualityDetection,
                                              c.ControlValve,
                                              c.WaterTankSterilizer,
                                              c.PumpLocation,
                                              c.MasterControlPLCIP,
                                              c.InstallDate,
                                              c.AcceptanceDate,
                                              c.WaterFloor,
                                              c.DressingCycle
                                          }).Where(c => c.pumpID.Equals(b.PumpId) && c.FIsDelete == 0),
                                          pumpJZID = b.ID,
                                          b.PumpId,
                                          b.DTUCode,
                                          b.PumpJZName,
                                          b.MachineType,
                                          b.RunPumpNum,
                                          b.Auxiliarypumpcount,
                                          b.DrainPumpNum,
                                          FOnLine = DB.E_DATA_MAIN.Where(y => y.BaseID.Equals(b.ID)).FirstOrDefault().FOnLine,
                                          IsAlarm = DB.Alarm_Timely.Where(y => y.BaseID.Equals(b.ID) && y.FMarkerType == 1 && y.FStatus == 1).Count() > 0 ? 1 : 0,
                                          b.PumpJZArea,
                                          b.AddressScheme.FName,
                                          b.PumpJZCollectPeriod,
                                          b.PumpJZCollectLength,
                                          b.PumpJZReadMode,
                                          b.PumpJZTankVolume,
                                          b.PumpJZTankLength,
                                          b.PumpJZInletDiameter,
                                          b.PumpJZOutletDiameter,
                                          b.PumpJZBrandSet,
                                          b.PumpJZPumpBrand,
                                          b.PumpJZMainPumpFlow,
                                          b.PumpJZMainPumpLift,
                                          b.PumpJZAuxiliPumpFlow,
                                          b.PumpJZAuxiliPumpLift,
                                          b.PumpJZMainPumpPower,
                                          b.PumpJZAuxiliPumpPower,
                                          b.PumpJZPressReliValve,
                                          b.PumpJZPeak,
                                          b.PumpJZCheckValve,
                                          b.PumpJZInPUpper,
                                          b.PumpJZInPLower,
                                          b.PumpJZOutPUpper,
                                          b.PumpJZOutPLower,
                                          b.PumpJZReChlorUpper,
                                          b.PumpJZReChlorLower,
                                          b.PumpJZTurbidUpper,
                                          b.PumpJZTurbidLower,
                                          b.PumpJZPHUpper,
                                          b.PumpJZPHLower,
                                          b.PumpJZTankUpper,
                                          b.PumpJZTankLower,
                                          b.FIsDelete,
                                          D_Data = DB.E_DATA_MAIN.Where(c => c.BaseID.Equals(b.ID))
                                      }).Where(b => b.pumpJZID.Equals(_lx.BaseID) && b.FIsDelete == 0)
                                 }).Where(_lx => _lx.UserID == UserID && !_lx.BaseID.Equals(_BaseID));

                        //if (_JZName != "" && _JZName != null)
                        //{
                        //    q = q.Where(x => x.pumpJZ == x.pumpJZ.Where(y => y.DTUCode.Contains(_JZName) || y.PumpJZName.Contains(_JZName)));
                        //}
                        //switch (_State)
                        //{
                        //    case 0: q = q.Where(x => x.pumpJZ == x.pumpJZ.Where(y => y.OnLine == _State)); break;     //离线
                        //    case 1: q = q.Where(x => x.pumpJZ == x.pumpJZ.Where(y => y.OnLine == _State)); break;    //在线
                        //    case 2: q = q.Where(x => x.pumpJZ == x.pumpJZ.Where(y => y.IsAlarm == 1)); break;  //报警
                        //}
                        str = successMsg("查询成功", "true", js.Serialize(q));
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

        public void NoLunXunSelect()
        {
            StringBuilder str = new StringBuilder();
            int UserID = Convert.ToInt32(GetIdentityName());
            try
            {
                using (var DB = new DBController())
                {
                    Guid _BaseID = Guid.Empty;
                    string _JZName = Request["JZName"];
                    string _State = Request["State"];
                    int pageIndex = Convert.ToInt32(Request["pageIndex"]) + 1;
                    int pageSize = Convert.ToInt32(Request["pageSize"]);

                    var q = (from b in DB.Panda_PumpJZ
                             from p in DB.Panda_Pump.Where(i => i.ID.Equals(b.PumpId))
                             select new
                             {
                                 pump = DB.Panda_Pump.Select(c => new
                                 {
                                     pumpID = c.ID,
                                     c.PName,
                                     c.PCustomPName,
                                     c.PLngLat,
                                     c.FIsDelete,
                                     c.PCompanyNumber,
                                     FCustomerID = c.Panda_Customer.ID,
                                     c.TankIsSharing,
                                     c.PumpSoaking,
                                     c.Warning,
                                     c.WaterQualityDetection,
                                     c.ControlValve,
                                     c.WaterTankSterilizer,
                                     c.PumpLocation,
                                     c.MasterControlPLCIP,
                                     c.InstallDate,
                                     c.AcceptanceDate,
                                     c.WaterFloor,
                                     c.DressingCycle
                                 }).Where(c => c.pumpID.Equals(b.PumpId) && c.FIsDelete == 0),
                                 pump_Delete = p.FIsDelete,
                                 pumpJZID = b.ID,
                                 C_ID = p.Panda_Customer.ID,
                                 CompanyNumber = p.PCompanyNumber,
                                 b.PumpId,
                                 b.DTUCode,
                                 b.PumpJZName,
                                 b.MachineType,
                                 b.RunPumpNum,
                                 b.Auxiliarypumpcount,
                                 b.DrainPumpNum,
                                 FOnLine = DB.E_DATA_MAIN.Where(y => y.BaseID.Equals(b.ID)).FirstOrDefault().FOnLine,
                                 IsAlarm = DB.Alarm_Timely.Where(y => y.BaseID.Equals(b.ID) && y.FMarkerType == 1 && y.FStatus == 1).Count() > 0 ? 1 : 0,
                                 b.PumpJZArea,
                                 b.AddressScheme.FName,
                                 b.PumpJZCollectPeriod,
                                 b.PumpJZCollectLength,
                                 b.PumpJZReadMode,
                                 b.PumpJZTankVolume,
                                 b.PumpJZTankLength,
                                 b.PumpJZInletDiameter,
                                 b.PumpJZOutletDiameter,
                                 b.PumpJZBrandSet,
                                 b.PumpJZPumpBrand,
                                 b.PumpJZMainPumpFlow,
                                 b.PumpJZMainPumpLift,
                                 b.PumpJZAuxiliPumpFlow,
                                 b.PumpJZAuxiliPumpLift,
                                 b.PumpJZMainPumpPower,
                                 b.PumpJZAuxiliPumpPower,
                                 b.PumpJZPressReliValve,
                                 b.PumpJZPeak,
                                 b.PumpJZCheckValve,
                                 b.PumpJZInPUpper,
                                 b.PumpJZInPLower,
                                 b.PumpJZOutPUpper,
                                 b.PumpJZOutPLower,
                                 b.PumpJZReChlorUpper,
                                 b.PumpJZReChlorLower,
                                 b.PumpJZTurbidUpper,
                                 b.PumpJZTurbidLower,
                                 b.PumpJZPHUpper,
                                 b.PumpJZPHLower,
                                 b.PumpJZTankUpper,
                                 b.PumpJZTankLower,
                                 b.FIsDelete,
                                 D_Data = DB.E_DATA_MAIN.Where(c => c.BaseID.Equals(b.ID))
                             }).Where(b => b.FIsDelete == 0 && b.pump_Delete == 0 && !DB.LunXunParam.Select(_lx => new { _lx.BaseID, _lx.UserID }).Where(_lx => _lx.UserID == UserID && !_lx.BaseID.Equals(_BaseID)).Select(_lx => _lx.BaseID).Contains(b.pumpJZID));

                    if (_JZName != "" && _JZName != null)
                    {
                        q = q.Where(y => y.DTUCode.Contains(_JZName) || y.PumpJZName.Contains(_JZName));
                    }
                    if (!_State.Equals(""))
                    {
                        int state = Convert.ToInt32(_State);
                        switch (state)
                        {
                            case 0: q = q.Where(y => y.FOnLine == state); break;     //离线
                            case 1: q = q.Where(y => y.FOnLine == state); break;    //在线
                            case 2: q = q.Where(y => y.IsAlarm == 1); break;  //报警
                        }
                    }

                    string userType = GetUserType();
                    switch (userType)
                    {
                        case "1": ; break;
                        case "2":
                            string number = GetUserCompanyNumber();
                            q = q.Where(x => x.CompanyNumber.Equals(number)); break;
                        case "3":
                            int customerid = Convert.ToInt32(GetUserCustomer());
                            q = q.Where(x => x.C_ID == customerid); break;
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
                            q = q.Where(x => igroup.Contains(x.PumpId))
                            ; break;
                    }

                    dynamic t = q;
                    int allcount = q.Count();

                    int pagecount = allcount % pageSize == 0 ? allcount / pageSize : allcount / pageSize + 1;
                    t = PagingSorting.SortAndPage<dynamic>(t, pageIndex, pagecount, pageSize, "desc", "PumpId");
                    str = successMsg("查询成功", "true", allcount, js.Serialize(t));
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

        public void EditLunXun()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                string FField = Request["FField"];        //字段值
                string BaseID = Request["BaseID"];        //字段值
                Hashtable has = new Hashtable();
                has["UserID"] = GetIdentityName();
                has["FField"] = FField;
                if (!BaseID.Equals(""))
                {
                    has["BaseID"] = BaseID;
                }
                else
                {
                    has["BaseID"] = DBNull.Value;
                }
                publicDal.Update(has, "UPDATE LunXunParam SET {0} WHERE {1}", "UserID,FField");
                str = successMsg("成功", "true");
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "失败," + e.Message;
                str = successMsg(msg, "false");
            }
            Response.Write(str);
        }
        #endregion

        #region 地图
        public void SearchTemp()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                string json_user = string.Empty;
                string json_map = string.Empty;
                string userSql = @"select a.FCustomerID,a.FCompanyNumber,a.UserName,b.Name as CustomerName,c.Fengongsi,a.UserType
                                 from Panda_UserInfo a
                            left join Panda_Customer b on a.FCustomerID=b.ID
                            left join A_U_DEP c on a.FCompanyNumber=c.U8number
                                where a.ID='" + GetIdentityName() + "'";

                DataTable dt_user = publicDal.TableSearch(userSql);
                if (dt_user.Rows.Count > 0)
                {
                    json_user = PluSoft.Utils.JSON.Encode(dt_user);
                    if (dt_user.Rows[0]["CustomerName"].ToString().Equals(""))
                    {
                        json_map = PluSoft.Utils.JSON.Encode("[]");
                    }
                    else
                    {
                        string mapSql = @"select a.id as TempID,FMapTempName,a.FCreateDate as TempCreateDate,b.ID as TempProID
                                        ,b.FMapTempID,FAliasName,FMinZoom,FMaxZoom,FCenter,FZoom,FFeatures,FMapType,FStyle
                                    from Map_Template a,Map_TempProperty b,Panda_Customer c where a.id=b.FMapTempID and a.id=c.FMapTempID
                                     and c.ID='" + dt_user.Rows[0]["FCustomerID"].ToString() + "'";
                        json_map = PluSoft.Utils.JSON.Encode(publicDal.TableSearch(mapSql));
                    }
                    string json = "{\"data\":["
                                 + "{\"json_user\":" + json_user + " }, "
                                 + "{\"json_map\":" + json_map + " } "
                        + "]}";
                    str = successMsg("查询成功", "true", json);
                }
                else
                {
                    str = successMsg("用户不存在", "false");
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

        public void SearchMarker()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                using (var DB = new DBController())
                {
                    int pageIndex = Convert.ToInt32(Request["pageIndex"]) + 1;
                    int pageSize = Convert.ToInt32(Request["pageSize"]);
                    var q = (from a in DB.Panda_Pump
                             select new
                             {
                                 pumpID = a.ID,
                                 a.PName,
                                 a.PCustomPName,
                                 a.PLngLat,
                                 a.FIsDelete,
                                 a.PCompanyNumber,
                                 CompanyName = DB.A_U_DEP.Where(x => x.U8number.Equals(a.PCompanyNumber)).FirstOrDefault().Fengongsi,
                                 FCustomerID = a.Panda_Customer.ID,
                                 FCustomerName = a.Panda_Customer.Name,
                                 a.PAddress,
                                 pumpJZ = DB.Panda_PumpJZ.Select(b => new
                                 {
                                     pumpJZID = b.ID,
                                     b.PumpId,
                                     b.DTUCode,
                                     b.PumpJZName,
                                     b.MachineType,
                                     IsAlarm = DB.Alarm_Timely.Where(y => y.BaseID.Equals(b.ID) && y.FMarkerType == 1 && y.FStatus == 1).Count() > 0 ? 1 : 0,
                                     b.PumpJZArea,
                                     b.FIsDelete,
                                     D_Data = DB.E_DATA_MAIN.Select(c => new { c.BaseID, c.FOnLine, c.FTotalDL, c.FTotalOutLL, c.F41006, c.F41007 }).Where(c => c.BaseID.Equals(b.ID))
                                 }).Where(b => b.PumpId.Equals(a.ID) && b.FIsDelete == 0),
                             }).Where(u => u.FIsDelete == 0);

                    string userType = GetUserType();
                    switch (userType)
                    {
                        case "1": ; break;
                        case "2":
                            string number = GetUserCompanyNumber();
                            q = q.Where(x => x.PCompanyNumber.Equals(number)); break;
                        case "3":
                            int customerid = Convert.ToInt32(GetUserCustomer());
                            q = q.Where(x => x.FCustomerID == customerid); break;
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
                            q = q.Where(x => igroup.Contains(x.pumpID))
                            ; break;
                    }
                    dynamic t = q;
                    int allcount = q.Count();

                    int pagecount = allcount % pageSize == 0 ? allcount / pageSize : allcount / pageSize + 1;
                    t = PagingSorting.SortAndPage<dynamic>(t, pageIndex, pagecount, pageSize, "desc", "pumpID");
                    str = successMsg("查询成功", "true", allcount, js.Serialize(t));
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
        #endregion

        #region 年/月/日/区间 最大值-最小值时间段累计计算
        publicDal pubDal = new publicDal();
        /// <summary>
        /// 流量
        /// </summary>
        public void SearchBF_LL()
        {
            pubDal.Search_DateType("E_DATA", "FTotalOutLL");
        }
        /// <summary>
        /// 进水压力
        /// </summary>
        public void SearchBF_InYL()
        {
            pubDal.Search_DateType("E_DATA", "F41006");
        }
        /// <summary>
        /// 出水压力
        /// </summary>
        public void SearchBF_OutYL()
        {
            pubDal.Search_DateType("E_DATA", "F41007");
        }
        /// <summary>
        /// 电量
        /// </summary>
        public void SearchBF_DL()
        {
            pubDal.Search_DateType("E_DATA", "FTotalDL");
        }
        #endregion

        #region 控制
        /// <summary>
        /// 接收下发命令
        /// </summary>
        public void GetCommand()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                string FPLCAddress = Request["FPLCAddress"];
                string text = Request["text"];
                string dtu = Request["dtu"];
                string FRate = "0.01";
                string user = GetUserName();
                if (!user.Equals("panda"))
                {
                    str = BaseController.successMsg("无权限", "false");
                }
                else
                {
                    if (text != "" && text != null && FPLCAddress != "" && FPLCAddress != null && dtu != "" && dtu != null)
                    {
                        if (FPLCAddress.Equals("V40003"))
                        {
                            FRate = "1";
                        }
                        //str = BaseController.successMsg("成功", "true", "[{\"text\":\"" + text + "\",\"dtu\":\"" + dtu + "\",\"FPLCAddress\":\"" + FPLCAddress + "\",\"FRate\":\"" + FRate + "\",\"slaveId\":\"01\"}]");
                        bool flag = SetCommand(text, dtu, FPLCAddress, FRate, "01");
                        if (flag == true)
                        {
                            //str = BaseController.successMsg("成功", "true", "[{\"name\":\"" + name + "\",\"text\":\"" + text + "\",\"id\":\"" + id + "\",\"type\":\"" + type + "\",\"dtu\":\"" + dtu + "\",\"FSchemeID\":\"" + FSchemeID + "\",\"FPLCAddress\":\"" + FPLCAddress + "\",\"FRate\":\"" + FRate + "\",\"slaveId\":\"" + slaveId + "\"}]");
                            str = BaseController.successMsg("成功", "true", "[{\"text\":\"" + text + "\",\"dtu\":\"" + dtu + "\",\"FPLCAddress\":\"" + FPLCAddress + "\",\"FRate\":\"" + FRate + "\",\"slaveId\":\"01\"}]");

                        }
                        else
                        {
                            str = BaseController.successMsg("失败", "false");
                        }
                    }
                    else
                    {
                        str = BaseController.successMsg("失败", "false");
                    }
                }
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "失败," + e.Message;
                str = BaseController.successMsg(msg, "false");
            }
            Response.Write(str);
        }
        public bool SetCommand(string text, string dtu, string FPLCAddress, string FRate, string slaveId)
        {
            try
            {
                SetCommand2Service.Service1Client service = new SetCommand2Service.Service1Client();
                return service.SetCommand("", text, "", "", dtu, "", FPLCAddress, FRate, slaveId);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 接收下发命令
        /// </summary>
        public void GetCommandText()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                string FPLCAddress = Request["FPLCAddress"];
                string text = Request["text"];
                string dtu = Request["dtu"];
                string FRate = "0.01";
                string user = Request["user"];
                if (!user.Equals("panda"))
                {
                    str = BaseController.successMsg("无权限", "false");
                }
                else
                {
                    if (text != "" && text != null && FPLCAddress != "" && FPLCAddress != null && dtu != "" && dtu != null)
                    {
                        if (FPLCAddress.Equals("V40003"))
                        {
                            FRate = "1";
                        }
                        //str = BaseController.successMsg("成功", "true", "[{\"text\":\"" + text + "\",\"dtu\":\"" + dtu + "\",\"FPLCAddress\":\"" + FPLCAddress + "\",\"FRate\":\"" + FRate + "\",\"slaveId\":\"01\"}]");
                        bool flag = SetCommand(text, dtu, FPLCAddress, FRate, "01");
                        if (flag == true)
                        {
                            //str = BaseController.successMsg("成功", "true", "[{\"name\":\"" + name + "\",\"text\":\"" + text + "\",\"id\":\"" + id + "\",\"type\":\"" + type + "\",\"dtu\":\"" + dtu + "\",\"FSchemeID\":\"" + FSchemeID + "\",\"FPLCAddress\":\"" + FPLCAddress + "\",\"FRate\":\"" + FRate + "\",\"slaveId\":\"" + slaveId + "\"}]");
                            str = BaseController.successMsg("成功", "true", "[{\"text\":\"" + text + "\",\"dtu\":\"" + dtu + "\",\"FPLCAddress\":\"" + FPLCAddress + "\",\"FRate\":\"" + FRate + "\",\"slaveId\":\"01\"}]");

                        }
                        else
                        {
                            str = BaseController.successMsg("失败", "false");
                        }
                    }
                    else
                    {
                        str = BaseController.successMsg("失败", "false");
                    }
                }
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "失败," + e.Message;
                str = BaseController.successMsg(msg, "false");
            }
            Response.Write(str);
        }
        #endregion
        #endregion
    }
}