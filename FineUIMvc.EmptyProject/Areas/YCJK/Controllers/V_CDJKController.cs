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
using FineUIMvc.PumpMVC.SetCommandService;
using FineUIMvc.PumpMVC.ReportModel;
using FineUIMvc.PumpMVC.Common.ExcelExport;

namespace FineUIMvc.PumpMVC.Areas.YCJK.Controllers
{
    [Authorize]
    public class V_CDJKController : BaseController
    {
        private DBController db = new DBController();
        private IExcelExport _excelExport = new NPOIExcelExport();
        private JavaScriptSerializer js = new JavaScriptSerializer();

        //
        // GET: /YCJK/V_CDJK/
        public ActionResult CDJK()
        {
            return View();
        }
        public ActionResult PA_List()
        {
            ViewBag.UserName = GetUserName();
            return View();
        }
        public ActionResult YL_runLog()
        {
            return View();
        }
        public ActionResult YL_rangeCompare()
        {
            return View();
        }
        public ActionResult YL_compreReport()
        {
            return View();
        }
        public ActionResult YL_totalYLcurve()
        {
            return View();
        }
        public ActionResult YL_selectYlName()
        {
            return View();
        }

        public ActionResult Flow_List()
        {
            return View();
        }
        public ActionResult Flow_runLog()
        {
            return View();
        }
        public ActionResult Flow_compreReport()
        {
            return View();
        }
        public ActionResult Flow_selectName()
        {
            return View();
        }
        public ActionResult CDmainReport()
        {
            return View();
        }
        public ActionResult ReportCharts()
        {
            return View();
        }

        public ActionResult TFBengZhan()
        {
            return View();
        }

        public ActionResult TF_reportWaterInOutPress()
        {
            return View();
        }

        public ActionResult SelectTFname()
        {
            return View();
        }

        public ActionResult TF_runLog()
        {
            return View();
        }
        public ActionResult TF_waterBoxYW()
        {
            return View();
        }
        public ActionResult TF_weekDayFlow()
        {
            return View();
        }
        public ActionResult FM_List()
        {
            return View();
        }
        public ActionResult FM_setPressAperture()
        {
            return View();
        }
        public ActionResult FM_compreReport()
        {
            return View();
        }
        public ActionResult FM_selectFmName()
        {
            return View();
        }
        public ActionResult FM_runLog()
        {
            return View();
        }
        public ActionResult FM_weekDayReport()
        {
            return View();
        }
        /// <summary>
        /// 压力
        /// </summary>
        public void SearchYL_Report()
        {
            string strwhere = "";
            string _Name = Request["Name"];
            string _State = Request["State"];
            string _ID = Request["ID"];
            if (_Name != "" && _Name != null)
            {
                strwhere = strwhere + " and (a.FName like '%" + _Name + "%' or a.FDTUCode like '%" + _Name + "%')";
            }

            if (_ID != "" && _ID != null)
            {
                strwhere = strwhere + " and a.id='" + _ID + "'";
            }

            switch (_State)
            {
                case "0": strwhere = strwhere + " and b.FOnLine='" + _State + "'"; break;     //离线
                case "1": strwhere = strwhere + " and b.FOnLine='" + _State + "'"; break;    //在线
                case "2": strwhere = strwhere + @" and isnull((select top 1 1 from Alarm_Timely _c 
                                              where _c.BaseID=a.id and FMarkerType=7 and FStatus=1),0)=1"; break;  //报警
            }

            strwhere = strwhere + getPowerConst("pressure");

            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);
            Hashtable result = BASE_YALIDal.SearchYL_Report(pageIndex, pageSize, "FCreateDate", "desc", strwhere);
            string json = PluSoft.Utils.JSON.Encode(result);
            Response.Write(json);
        }
        /// <summary>
        /// 流量
        /// </summary>
        public void SearchLL_Report()
        {
            string strwhere = "";
            string _Name = Request["Name"];
            string _State = Request["State"];
            string _ID = Request["ID"];
            if (_Name != "" && _Name != null)
            {
                strwhere = strwhere + " and (a.FName like '%" + _Name + "%' or a.FDTUCode like '%" + _Name + "%')";
            }

            if (_ID != "" && _ID != null)
            {
                strwhere = strwhere + " and a.id='" + _ID + "'";
            }

            switch (_State)
            {
                case "0": strwhere = strwhere + " and b.FOnLine='" + _State + "'"; break;     //离线
                case "1": strwhere = strwhere + " and b.FOnLine='" + _State + "'"; break;    //在线
                case "2": strwhere = strwhere + @" and isnull((select top 1 1 from Alarm_Timely _c 
                                              where _c.BaseID=a.id and FMarkerType=3 and FStatus=1),0)=1"; break;  //报警
            }
            strwhere = strwhere + getPowerConst("flow");

            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);
            Hashtable result = BASE_LIULIANGDal.SearchLL_Report(pageIndex, pageSize, "FCreateDate", "desc", strwhere);
            string json = PluSoft.Utils.JSON.Encode(result);
            Response.Write(json);
        }
        /// <summary>
        /// 调峰
        /// </summary>
        public void SearchTF_Report()
        {
            string strwhere = "";
            string _Name = Request["Name"];
            string _State = Request["State"];
            string _ID = Request["ID"];
            if (_Name != "" && _Name != null)
            {
                strwhere = strwhere + " and (a.FName like '%" + _Name + "%' or a.FDTUCode like '%" + _Name + "%')";
            }

            if (_ID != "" && _ID != null)
            {
                strwhere = strwhere + " and a.id='" + _ID + "'";
            }

            switch (_State)
            {
                case "0": strwhere = strwhere + " and b.FOnLine='" + _State + "'"; break;     //离线
                case "1": strwhere = strwhere + " and b.FOnLine='" + _State + "'"; break;    //在线
                case "2": strwhere = strwhere + @" and isnull((select top 1 1 from Alarm_Timely _c 
                                              where _c.BaseID=a.id and FMarkerType=8 and FStatus=1),0)=1"; break;  //报警
            }
            strwhere = strwhere + getPowerConst("tiaofeng");

            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);
            Hashtable result = BASE_TIAOFENGDal.SearchTF_Report(pageIndex, pageSize, "FCreateDate", "desc", strwhere);
            string json = PluSoft.Utils.JSON.Encode(result);
            Response.Write(json);
        }
        /// <summary>
        /// 阀门
        /// </summary>
        public void SearchFM_Report()
        {
            string strwhere = "";
            string _Name = Request["Name"];
            string _State = Request["State"];
            string _ID = Request["ID"];
            if (_Name != "" && _Name != null)
            {
                strwhere = strwhere + " and (a.FName like '%" + _Name + "%' or a.FDTUCode like '%" + _Name + "%')";
            }

            if (_ID != "" && _ID != null)
            {
                strwhere = strwhere + " and a.id='" + _ID + "'";
            }

            switch (_State)
            {
                case "0": strwhere = strwhere + " and b.FOnLine='" + _State + "'"; break;     //离线
                case "1": strwhere = strwhere + " and b.FOnLine='" + _State + "'"; break;    //在线
                case "2": strwhere = strwhere + @" and isnull((select top 1 1 from Alarm_Timely _c 
                                              where _c.BaseID=a.id and FMarkerType=3 and FStatus=1),0)=1"; break;  //报警
            }
            strwhere = strwhere + getPowerConst("famen");

            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);
            Hashtable result = Base_FamenDal.SearchFM_Report(pageIndex, pageSize, "FCreateDate", "desc", strwhere);
            string json = PluSoft.Utils.JSON.Encode(result);
            Response.Write(json);
        }
        /// <summary>
        /// 压力历史
        /// </summary>
        public void SearchYL_HisReport()
        {
            string strwhere = "";
            string _Name = Request["Name"];
            string Start = Request["StartDate"];   //开始日期
            string End = Request["EndDate"];   //结束日期
            string _ID = Request["ID"];
            int year = Convert.ToDateTime(Start).Year;
            if (_ID != "" && _ID != null)
            {
                strwhere = strwhere + " and a.id='" + _ID + "'";
            }
            if (_Name != "" && _Name != null)
            {
                strwhere = strwhere + " and (a.FName like '%" + _Name + "%' or a.FDTUCode like '%" + _Name + "%')";
            }
            strwhere = strwhere + " and b.TempTime between '" + Start + "' and '" + End + "'";
            strwhere = strwhere + getPowerConst("pressure");

            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);
            Hashtable result = BASE_YALIDal.SearchYL_HisReport(pageIndex, pageSize, "b.BASEID,TempTime", "desc", strwhere, year);
            string json = PluSoft.Utils.JSON.Encode(result);
            Response.Write(json);
        }
        //导出压力历史
        public void ReportYL_His()
        {
            string strwhere = "";
            string _Name = Request["Name"];
            string Start = Request["StartDate"];   //开始日期
            string End = Request["EndDate"];   //结束日期
            string _ID = Request["ID"];
            int year = Convert.ToDateTime(Start).Year;
            if (_ID != "" && _ID != null)
            {
                strwhere = strwhere + " and a.id='" + _ID + "'";
            }
            if (_Name != "" && _Name != null)
            {
                strwhere = strwhere + " and (a.FName like '%" + _Name + "%' or a.FDTUCode like '%" + _Name + "%')";
            }
            strwhere = strwhere + " and b.TempTime between '" + Start + "' and '" + End + "'";
            strwhere = strwhere + getPowerConst("pressure");
            DataTable dtresult = BASE_YALIDal.SearchYL_HisReport(strwhere, year);
            try
            {
                List<YL_runLog> YaLiLog = ModelConvertHelper<YL_runLog>.ConvertToModel(dtresult).ToList();
                var excelExport = _excelExport.CreateWorkbook();
                excelExport.AddSheet(YaLiLog, "");
                string path = System.Web.HttpContext.Current.Server.MapPath("~/DownLoad");
                if (Directory.Exists(path)) Directory.Delete(path, true);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                string pdfname = "/压力日志" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
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
        public void SearchYL_HisReportCompare()
        {
            string strwhere1 = "";
            string strwhere2 = "";
            string _Name = Request["Name"];
            string Start1 = Request["StartDate1"];   //开始日期1
            string End1 = Request["EndDate1"];   //结束日期1
            string Start2 = Request["StartDate2"];   //开始日期2
            string End2 = Request["EndDate2"];   //结束日期2
            string _ID = Request["ID"];
            int year1 = Convert.ToDateTime(Start1).Year;
            if (_ID != "" && _ID != null)
            {
                strwhere1 = strwhere1 + " and a.id='" + _ID + "'";
            }
            if (_Name != "" && _Name != null)
            {
                strwhere1 = strwhere1 + " and (a.FName like '%" + _Name + "%' or a.FDTUCode like '%" + _Name + "%')";
            }
            strwhere1 = strwhere1 + " and b.TempTime between '" + Start1 + "' and '" + End1 + "'";
            strwhere1 = strwhere1 + getPowerConst("pressure");

            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);
            Hashtable result = BASE_YALIDal.SearchYL_HisReport(pageIndex, pageSize, "b.BASEID,TempTime", "desc", strwhere1, year1);
            string json1 = PluSoft.Utils.JSON.Encode(result);

            int year2 = Convert.ToDateTime(Start2).Year;
            if (_ID != "" && _ID != null)
            {
                strwhere2 = strwhere2 + " and a.id='" + _ID + "'";
            }
            if (_Name != "" && _Name != null)
            {
                strwhere2 = strwhere2 + " and (a.FName like '%" + _Name + "%' or a.FDTUCode like '%" + _Name + "%')";
            }
            strwhere2 = strwhere2 + " and b.TempTime between '" + Start2 + "' and '" + End2 + "'";
            strwhere2 = strwhere2 + getPowerConst("pressure");

            result = BASE_YALIDal.SearchYL_HisReport(pageIndex, pageSize, "b.BASEID,TempTime", "desc", strwhere2, year2);
            string json2 = PluSoft.Utils.JSON.Encode(result);

            string json = "{\"data\":["
                               + "{\"field\":\"" + Start1 + "\",\"detailData\":" + json1 + " }, "
                               + "{\"field\":\"" + Start2+ "\",\"detailData\":" + json2 + " } "
                      + "]}";
            Response.Write(json);
        }

        /// <summary>
        /// 流量历史
        /// </summary>
        public void SearchLL_HisReport()
        {
            string strwhere = "";
            string _Name = Request["Name"];
            string Start = Request["StartDate"];   //开始日期
            string End = Request["EndDate"];   //结束日期
            string _ID = Request["ID"];
            int year = Convert.ToDateTime(Start).Year;
            if (_ID != "" && _ID != null)
            {
                strwhere = strwhere + " and a.id='" + _ID + "'";
            }
            if (_Name != "" && _Name != null)
            {
                strwhere = strwhere + " and (a.FName like '%" + _Name + "%' or a.FDTUCode like '%" + _Name + "%')";
            }
            strwhere = strwhere + " and b.TempTime between '" + Start + "' and '" + End + "'";
            strwhere = strwhere + getPowerConst("flow");

            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);
            Hashtable result = BASE_LIULIANGDal.SearchLL_HisReport(pageIndex, pageSize, "b.BASEID,TempTime", "desc", strwhere, year);
            string json = PluSoft.Utils.JSON.Encode(result);
            Response.Write(json);
        }
        //导出流量历史
        public void ReportLL_His()
        {
            string strwhere = "";
            string _Name = Request["Name"];
            string Start = Request["StartDate"];   //开始日期
            string End = Request["EndDate"];   //结束日期
            string _ID = Request["ID"];
            int year = Convert.ToDateTime(Start).Year;
            if (_ID != "" && _ID != null)
            {
                strwhere = strwhere + " and a.id='" + _ID + "'";
            }
            if (_Name != "" && _Name != null)
            {
                strwhere = strwhere + " and (a.FName like '%" + _Name + "%' or a.FDTUCode like '%" + _Name + "%')";
            }
            strwhere = strwhere + " and b.TempTime between '" + Start + "' and '" + End + "'";
            strwhere = strwhere + getPowerConst("flow");
            DataTable dtresult = BASE_LIULIANGDal.SearchLL_HisReport(strwhere, year);
            try
            {
                List<LL_runLog> YaLiLog = ModelConvertHelper<LL_runLog>.ConvertToModel(dtresult).ToList();
                var excelExport = _excelExport.CreateWorkbook();
                excelExport.AddSheet(YaLiLog, "");
                string path = System.Web.HttpContext.Current.Server.MapPath("~/DownLoad");
                if (Directory.Exists(path)) Directory.Delete(path, true);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                string pdfname = "/流量日志" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
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
        /// <summary>
        /// 调峰历史
        /// </summary>
        public void SearchTF_HisReport()
        {
            string strwhere = "";
            string _Name = Request["Name"];
            string Start = Request["StartDate"];   //开始日期
            string End = Request["EndDate"];   //结束日期
            string _ID = Request["ID"];
            int year = Convert.ToDateTime(Start).Year;
            if (_ID != "" && _ID != null)
            {
                strwhere = strwhere + " and a.id='" + _ID + "'";
            }
            if (_Name != "" && _Name != null)
            {
                strwhere = strwhere + " and (a.FName like '%" + _Name + "%' or a.FDTUCode like '%" + _Name + "%')";
            }
            strwhere = strwhere + " and b.TempTime between '" + Start + "' and '" + End + "'";
            strwhere = strwhere + getPowerConst("tiaofeng");

            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);
            Hashtable result = BASE_TIAOFENGDal.SearchTF_HisReport(pageIndex, pageSize, "b.BASEID,TempTime", "desc", strwhere, year);
            string json = PluSoft.Utils.JSON.Encode(result);
            Response.Write(json);
        }
        //导出调峰历史
        public void ReportTF_His()
        {
            string strwhere = "";
            string _Name = Request["Name"];
            string Start = Request["StartDate"];   //开始日期
            string End = Request["EndDate"];   //结束日期
            string _ID = Request["ID"];
            int year = Convert.ToDateTime(Start).Year;
            if (_ID != "" && _ID != null)
            {
                strwhere = strwhere + " and a.id='" + _ID + "'";
            }
            if (_Name != "" && _Name != null)
            {
                strwhere = strwhere + " and (a.FName like '%" + _Name + "%' or a.FDTUCode like '%" + _Name + "%')";
            }
            strwhere = strwhere + " and b.TempTime between '" + Start + "' and '" + End + "'";
            strwhere = strwhere + getPowerConst("tiaofeng");
            DataTable dtresult = BASE_TIAOFENGDal.SearchTF_HisReport(strwhere, year);
            try
            {
                List<TF_runLog> YaLiLog = ModelConvertHelper<TF_runLog>.ConvertToModel(dtresult).ToList();
                var excelExport = _excelExport.CreateWorkbook();
                excelExport.AddSheet(YaLiLog, "");
                string path = System.Web.HttpContext.Current.Server.MapPath("~/DownLoad");
                if (Directory.Exists(path)) Directory.Delete(path, true);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                string pdfname = "/调峰日志" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
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
        /// <summary>
        /// 阀门历史
        /// </summary>
        public void SearchFM_HisReport()
        {
            string strwhere = "";
            string _Name = Request["Name"];
            string Start = Request["StartDate"];   //开始日期
            string End = Request["EndDate"];   //结束日期
            string _ID = Request["ID"];
            int year = Convert.ToDateTime(Start).Year;
            if (_ID != "" && _ID != null)
            {
                strwhere = strwhere + " and a.id='" + _ID + "'";
            }
            if (_Name != "" && _Name != null)
            {
                strwhere = strwhere + " and (a.FName like '%" + _Name + "%' or a.FDTUCode like '%" + _Name + "%')";
            }
            strwhere = strwhere + " and b.TempTime between '" + Start + "' and '" + End + "'";
            strwhere = strwhere + getPowerConst("famen");

            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);
            Hashtable result = Base_FamenDal.SearchFM_HisReport(pageIndex, pageSize, "b.BASEID,TempTime", "desc", strwhere, year);
            string json = PluSoft.Utils.JSON.Encode(result);
            Response.Write(json);
        }

        //导出阀门历史
        public void ReportFM_His()
        {
            string strwhere = "";
            string _Name = Request["Name"];
            string Start = Request["StartDate"];   //开始日期
            string End = Request["EndDate"];   //结束日期
            string _ID = Request["ID"];
            int year = Convert.ToDateTime(Start).Year;
            if (_ID != "" && _ID != null)
            {
                strwhere = strwhere + " and a.id='" + _ID + "'";
            }
            if (_Name != "" && _Name != null)
            {
                strwhere = strwhere + " and (a.FName like '%" + _Name + "%' or a.FDTUCode like '%" + _Name + "%')";
            }
            strwhere = strwhere + " and b.TempTime between '" + Start + "' and '" + End + "'";
            strwhere = strwhere + getPowerConst("famen");

            DataTable dtresult = Base_FamenDal.SearchFM_HisReport(strwhere, year);
            try
            {
                List<FM_runLog> YaLiLog = ModelConvertHelper<FM_runLog>.ConvertToModel(dtresult).ToList();
                var excelExport = _excelExport.CreateWorkbook();
                excelExport.AddSheet(YaLiLog, "");
                string path = System.Web.HttpContext.Current.Server.MapPath("~/DownLoad");
                if (Directory.Exists(path)) Directory.Delete(path, true);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                string pdfname = "/阀门日志" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
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
        /// <summary>
        /// 流量计压力实时曲线
        /// </summary>
        public void SearchLL_SSPa()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                string BASEID = Request["BASEID"];
                int pageIndex = Convert.ToInt32(Request["pageIndex"]);
                int pageSize = Convert.ToInt32(Request["pageSize"]);
                string Start = Request["StartDate"];   //开始日期
                string End = Request["EndDate"];   //结束日期
                int year = Convert.ToDateTime(Start).Year;
                string sql = @"select TempTime,isnull(A03,0) as A03,isnull(F40002,0) as F40002,isnull(F40003,0) as F40003 from DATA_LIULIANG_" + year + " where BASEID='" + BASEID + "'"
                           + " and TempTime between '" + Start + "' and '" + End + "'";
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

        #region 年/月/日/区间 最大值-最小值时间段累计计算
        publicDal pubDal = new publicDal();
        public void SearchLL_Flow()
        {
            pubDal.Search_DateType("DATA_LIULIANG_", "P01");
        }
        public void SearchTF_InLL()
        {
            pubDal.Search_DateType("DATA_TIAOFENG_", "FTotalInLL");
        }
        public void SearchTF_OutLL()
        {
            pubDal.Search_DateType("DATA_TIAOFENG_", "FTotalOutLL");
        }
        public void SearchFM_LL()
        {
            pubDal.Search_DateType("DATA_FAMEN_", "FTotalLL");
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
                string name = Request["name"];
                string text = Request["text"];
                string id = Request["id"]; 
                string type = Request["type"];
                string dtu = Request["dtu"];
                string FSchemeID = Request["FSchemeID"];

                bool power = CheckPumpControlPower(name);
                if (power == false)
                {
                    str = BaseController.successMsg("无权限", "false");
                }
                else 
                {
                    if (name != "" && name != null && text != "" && text != null
                         && type != "" && type != null && dtu != "" && dtu != null)
                    {
                        DataTable dt = new DataTable();
                        if (FSchemeID != "" && FSchemeID != null)
                        {
                            dt = publicDal.TableSearch("select FNumber,FPLCAddress,FRate from AddressScheme a,AddressSchemeEntry b where a.ID=b.FSchemeID and FSchemeID='" + FSchemeID + "' and FDBAddress='" + name + "'");
                        }
                        else
                        {
                            dt = publicDal.TableSearch("select FNumber,FPLCAddress,FRate from AddressScheme a,AddressSchemeEntry b where a.ID=b.FSchemeID and FType='" + type + "' and FIsBaseScheme=1 and FDBAddress='" + name + "'");
                        }
                        string FPLCAddress = "";
                        string FRate = "";
                        string slaveId = "";
                        if (dt.Rows.Count > 0)
                        {
                            FPLCAddress = dt.Rows[0]["FPLCAddress"].ToString();
                            FRate = dt.Rows[0]["FRate"].ToString();
                            slaveId = dt.Rows[0]["FNumber"].ToString();
                        }

                        bool flag = SetCommand(name, text, id, type, dtu, FSchemeID, FPLCAddress, FRate, slaveId);
                        if (flag == true)
                        {
                            str = BaseController.successMsg("成功", "true", "[{\"name\":\"" + name + "\",\"text\":\"" + text + "\",\"id\":\"" + id + "\",\"type\":\"" + type + "\",\"dtu\":\"" + dtu + "\",\"FSchemeID\":\"" + FSchemeID + "\",\"FPLCAddress\":\"" + FPLCAddress + "\",\"FRate\":\"" + FRate + "\",\"slaveId\":\"" + slaveId + "\"}]");

                        }
                        else
                        {
                            str = BaseController.successMsg("失败", "false");
                        }
                       // str = BaseController.successMsg("成功", "true", "[{\"name\":\"" + name + "\",\"text\":\"" + text + "\",\"id\":\"" + id + "\",\"type\":\"" + type + "\",\"dtu\":\"" + dtu + "\",\"FSchemeID\":\"" + FSchemeID + "\",\"FPLCAddress\":\"" + FPLCAddress + "\",\"FRate\":\"" + FRate + "\",\"slaveId\":\"" + slaveId + "\"}]");
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
        public bool SetCommand(string name, string text, string id, string type, string dtu, string FSchemeID, string FPLCAddress, string FRate, string slaveId)
        {
            try
            {
                Service1Client service = new Service1Client();
                return service.SetCommand(name, text, id, type, dtu, FSchemeID, FPLCAddress, FRate, slaveId);

            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void GetFAMENCommand()
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
                        if (FPLCAddress.Equals("40003"))
                        {
                            FRate = "0.01";
                        }
                        //str = BaseController.successMsg("成功", "true", "[{\"text\":\"" + text + "\",\"dtu\":\"" + dtu + "\",\"FPLCAddress\":\"" + FPLCAddress + "\",\"FRate\":\"" + FRate + "\",\"slaveId\":\"01\"}]");
                        bool flag = SetFAMENCommand(text, dtu, FPLCAddress, FRate, "01");
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
        public bool SetFAMENCommand(string text, string dtu, string FPLCAddress, string FRate, string slaveId)
        {
            try
            {
                FaMenService.Service1Client service = new FaMenService.Service1Client();
                return service.SetCommand("", text, "", "", dtu, "", FPLCAddress, FRate, slaveId);
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion

        #region 页面显示参数设置
        public void FieldShow()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                string _tableName = Request["tableName"];
                string sql = "select FName,FField from T_Field where FView=1 and FType='" + _tableName + "' order by FSort ";
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
        #endregion


        public void getYaPumpData()
        {
            string FDTUCode = Request["FDTUCode"];
            string Start = Request["StartDate"];   //开始日期
            string End = Request["EndDate"];   //结束日期

            DataTable dt = new DataTable();

            if (FDTUCode.Equals("62E9A332-939A-4E28-B30B-F270D8CB314F"))
            {
                dt = publicDal.TableSearch(@"select PumpJZName FName,TempTime,F41007 FMpa from E_DATA2017 b
                                       inner join Panda_PumpJZ a on b.BASEID=a.ID
                                       where b.BASEID='62E9A332-939A-4E28-B30B-F270D8CB314F' and TempTime between '" + Start + "' and '" + End + "' order by b.BASEID,TempTime");
            }
            else
            {
                string baseId = "";
                switch (FDTUCode)
                {
                    case "03160926018": baseId = "C6564E94-3A1C-4373-84E0-0A7A52FB0A01"; break;
                    case "03160926076": baseId = "76ADB5EA-FA22-4509-866E-66F3D45255BA"; break;
                    case "03160926027": baseId = "7804C171-4EBB-4712-8B84-243A5D3B2A66"; break;
                    case "03160926028": baseId = "2A97083A-3ECD-4427-86FF-397D3F3C46E7"; break;
                    case "03160926015": baseId = "DCA007CE-F676-479B-ADB7-700F26FBD68B"; break;
                    case "03160926026": baseId = "3C54C54F-6AE2-48A9-B542-3EE17869720A"; break;
                }
                dt = publicDal.TableSearch(@"select FName,TempTime,FMpa from DATA_YALI_2017 b
                                         inner join BASE_YALI a on a.id=b.BASEID
                                       where b.BASEID='" + baseId + "' and TempTime between '" + Start + "' and '" + End + "' order by b.BASEID,TempTime");
            }

            string json = PluSoft.Utils.JSON.Encode(dt);
            Response.Write(json);
        }
    }
}