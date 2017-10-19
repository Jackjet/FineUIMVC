using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Reflection;
using System.Data;
using System.Web.SessionState;

namespace Water.Web.Service
{
    /// <summary>
    /// T_ReportService 的摘要说明
    /// </summary>
    public class T_ReportService : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string method = context.Request["method"].ToString().ToLower();
            switch (method)
            {
                //近三月漏水量
                case "getwatleakrecmarchdata":
                    GetWatLeakRecMarchData();

                    break;
                //月成本和月损失成本
                case "getmonthcostdata":
                    GetMonthCostData();
                    break;
                //月和季度成本（饼图）
                case "getmonthquarterdata":
                    GetMonthQuarterData();
                    break;
                case "gettoplabledata":
                    GetTopLableData();
                    break;
                //曲线图
                case "getcurvedata":
                    GetCurveData();
                    break;
                //某泵房每小时最大水量减最小水量
                case "gethistogramdata":
                    GetHistogramData();
                    break;
                //某泵房上周和本周每天最大水量减最小水量
                case "getwatpumpweekdata":
                    GetWatPumpWeekData();
                    break;
                //某泵房某月每天最大水量减最小水量
                case "getwatpumpmonthdata":
                    GetWatPumpMonthData();
                    break;
                //某泵房去年和今年每月最大水量减最小水量
                case "getwatpumpyeardata":
                    GetWatPumpYearData();
                    break;
                default:

                    break;

            }
        }
        public void GetTopLableData() {
            string sql = "";
            string timeStr = HttpContext.Current.Request["timeStr"];
            int year = Convert.ToDateTime(HttpContext.Current.Request["date"]).Year;
            switch(timeStr)
            {
                case "year": sql = " and year(TM)=" + year; break;
                case "month": sql = " and month(TM)=month(getdate()) and year(TM)=" + year; break;
                case "day": sql = " and day(TM)=day(getdate()) and year(TM)=" + year; break;
            }
            Hashtable has = Bll.T_ReportBll.SearchTopLableDate(sql);
            String json = PluSoft.Utils.JSON.Encode(has);
            HttpContext.Current.Response.Write(json);
        }

        /// <summary>
        /// 曲线图
        /// </summary>
        public void GetCurveData()
        {
            String _PumpID = HttpContext.Current.Request["PumpID"];
            Hashtable has = Bll.T_ReportBll.SearchCurveData(_PumpID);
            String json = "";
            if (has== null)
            {
                json = PluSoft.Utils.JSON.Encode("[]");
            }
            else
            {
                json = PluSoft.Utils.JSON.Encode(has["data"]);
            }
            
            HttpContext.Current.Response.Write(json);
        }

        /// <summary>
        /// 某泵房每小时最大水量减最小水量柱形图
        /// </summary>
        public void GetHistogramData()
        {
            String _PumpID = HttpContext.Current.Request["PumpID"];
            String _time = HttpContext.Current.Request["Date"];
            Hashtable has = Bll.T_ReportBll.SearchHistogramData(_PumpID, _time);
            String json = "";
            if (has == null)
            {
                json = PluSoft.Utils.JSON.Encode("");
            }
            else
            {
                json = PluSoft.Utils.JSON.Encode(has["data"]);
            }
            HttpContext.Current.Response.Write(json);
        }

        /// <summary>
        /// 某泵房上周和本周每天最大水量减最小水量曲线图
        /// </summary>
        public void GetWatPumpWeekData()
        {
            String _PumpID = HttpContext.Current.Request["PumpID"];
            //本周
            Hashtable has1 = Bll.T_ReportBll.SearchWatPumpWeekData(0,Convert.ToInt32(_PumpID));
            //上周
            Hashtable has2 = Bll.T_ReportBll.SearchWatPumpWeekData(1, Convert.ToInt32(_PumpID));

            String json = "";
            String json_week1 = PluSoft.Utils.JSON.Encode(has1["data"]);
            String json_week2 = PluSoft.Utils.JSON.Encode(has2["data"]);

            json = "{\"data\":["
                                + "{\"week\":\"本周\",\"detailData\":" + json_week1 + " }, "
                                + "{\"week\":\"上周\",\"detailData\":" + json_week2 + " } "
                       + "]}";

            HttpContext.Current.Response.Write(json);
        }

        /// <summary>
        /// 某泵房某月每天最大水量减最小水量
        /// </summary>
        public void GetWatPumpMonthData()
        {
            String _PumpID = HttpContext.Current.Request["PumpID"];
            String _time = HttpContext.Current.Request["Date"];
            Hashtable has = Bll.T_ReportBll.SearchWatPumpMonthData(_time+"-01",Convert.ToInt32(_PumpID));
            String json = "";
            if (has == null)
            {
                json = PluSoft.Utils.JSON.Encode("");
            }
            else
            {
                json = PluSoft.Utils.JSON.Encode(has["data"]);
            }
            HttpContext.Current.Response.Write(json);
        }

        /// <summary>
        /// 某泵房今年和去年每月最大水量减最小水量曲线图
        /// </summary>
        public void GetWatPumpYearData()
        {
            String _PumpID = HttpContext.Current.Request["PumpID"];
            //本周
            Hashtable has1 = Bll.T_ReportBll.SearchWatPumpYearData(0, Convert.ToInt32(_PumpID));
            //上周
            Hashtable has2 = Bll.T_ReportBll.SearchWatPumpYearData(1, Convert.ToInt32(_PumpID));

            String json = "";
            String json_year1 = PluSoft.Utils.JSON.Encode(has1["data"]);
            String json_year2 = PluSoft.Utils.JSON.Encode(has2["data"]);

            json = "{\"data\":["
                                + "{\"week\":\"今年\",\"detailData\":" + json_year1 + " }, "
                                + "{\"week\":\"去年\",\"detailData\":" + json_year2 + " } "
                       + "]}";

            HttpContext.Current.Response.Write(json);
        }

        public void GetWatLeakRecMarchData()
        {
            String _ReportID = HttpContext.Current.Request["ReportID"];
            string timeStr = HttpContext.Current.Request["timeStr"];
            switch (_ReportID)
            {
                case "4": ; break;
                case "5": ; break;
                case "6": ; break;
            }
            String json = "";
            String json_lev1 = "";
            String json_lev2 = "";
            String json_lev3 = "";
            if (_ReportID.Equals("1") || _ReportID.Equals("2") || _ReportID.Equals("3"))
            {
                if(timeStr.Equals("day"))
                {
                    Hashtable has1 = Bll.T_ReportBll.SearchWatLeakRecMarchData_day1(Convert.ToInt32(_ReportID));
                    json_lev1 = PluSoft.Utils.JSON.Encode(has1["data"]);
                    Hashtable has2 = Bll.T_ReportBll.SearchWatLeakRecMarchData_day2(Convert.ToInt32(_ReportID));
                    json_lev2 = PluSoft.Utils.JSON.Encode(has2["data"]);
                    Hashtable has3 = Bll.T_ReportBll.SearchWatLeakRecMarchData_day3(Convert.ToInt32(_ReportID));
                    json_lev3 = PluSoft.Utils.JSON.Encode(has3["data"]);
                    json = "{\"data\":["
                                     + "{\"timer\":\"" + ((DataTable)has1["data"]).Rows[0]["TM_day"].ToString() + "\",\"detailData\":" + json_lev1 + " }, "
                                     + "{\"timer\":\"" + ((DataTable)has2["data"]).Rows[0]["TM_day"].ToString() + "\",\"detailData\":" + json_lev2 + " }, "
                                     + "{\"timer\":\"" + ((DataTable)has3["data"]).Rows[0]["TM_day"].ToString() + "\",\"detailData\":" + json_lev3 + " } "
                            + "]}";
                }
                else if (timeStr.Equals("month"))
                {
                    Hashtable has1 = Bll.T_ReportBll.SearchWatLeakRecMarchData_month(0,Convert.ToInt32(_ReportID));
                    json_lev1 = PluSoft.Utils.JSON.Encode(has1["data"]);
                    Hashtable has2 = Bll.T_ReportBll.SearchWatLeakRecMarchData_month(1, Convert.ToInt32(_ReportID));
                    json_lev2 = PluSoft.Utils.JSON.Encode(has2["data"]);
                    Hashtable has3 = Bll.T_ReportBll.SearchWatLeakRecMarchData_month(2, Convert.ToInt32(_ReportID));
                    json_lev3 = PluSoft.Utils.JSON.Encode(has3["data"]);
                    json = "{\"data\":["
                                     + "{\"timer\":\"" + DateTime.Now.Month.ToString() + "月" + "\",\"detailData\":" + json_lev1 + " }, "
                                     + "{\"timer\":\"" + DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-1).Month.ToString() + "月" + "\",\"detailData\":" + json_lev2 + " }, "
                                     + "{\"timer\":\"" + DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-2).Month.ToString() + "月" + "\",\"detailData\":" + json_lev3 + " } "
                            + "]}";
                }
                else if (timeStr.Equals("year"))
                {
                    Hashtable has1 = Bll.T_ReportBll.SearchWatLeakRecMarchData_year(0, Convert.ToInt32(_ReportID));
                    json_lev1 = PluSoft.Utils.JSON.Encode(has1["data"]);
                    Hashtable has2 = Bll.T_ReportBll.SearchWatLeakRecMarchData_year(1, Convert.ToInt32(_ReportID));
                    json_lev2 = PluSoft.Utils.JSON.Encode(has2["data"]);
                    Hashtable has3 = Bll.T_ReportBll.SearchWatLeakRecMarchData_year(2, Convert.ToInt32(_ReportID));
                    json_lev3 = PluSoft.Utils.JSON.Encode(has3["data"]);
                    json = "{\"data\":["
                                     + "{\"timer\":\"" + DateTime.Now.Year.ToString() + "年" + "\",\"detailData\":" + json_lev1 + " }, "
                                     + "{\"timer\":\"" + (DateTime.Now.Year - 1).ToString() + "年" + "\",\"detailData\":" + json_lev2 + " }, "
                                     + "{\"timer\":\"" + (DateTime.Now.Year - 2).ToString() + "年" + "\",\"detailData\":" + json_lev3 + " } "
                            + "]}";
                }
            }
            else
            {
                if (_ReportID.Equals("4"))
                {
                    if (timeStr.Equals("day"))
                    {
                        Hashtable has1 = Bll.T_ReportBll.SearchWatLeakRecMarchData_rep4_day1();
                        json_lev1 = PluSoft.Utils.JSON.Encode(has1["data"]);
                        Hashtable has2 = Bll.T_ReportBll.SearchWatLeakRecMarchData_rep4_day2();
                        json_lev2 = PluSoft.Utils.JSON.Encode(has2["data"]);
                        Hashtable has3 = Bll.T_ReportBll.SearchWatLeakRecMarchData_rep4_day3();
                        json_lev3 = PluSoft.Utils.JSON.Encode(has3["data"]);
                        json = "{\"data\":["
                                         + "{\"timer\":\"" + ((DataTable)has1["data"]).Rows[0]["TM_day"].ToString() + "\",\"detailData\":" + json_lev1 + " }, "
                                         + "{\"timer\":\"" + ((DataTable)has2["data"]).Rows[0]["TM_day"].ToString() + "\",\"detailData\":" + json_lev2 + " }, "
                                         + "{\"timer\":\"" + ((DataTable)has3["data"]).Rows[0]["TM_day"].ToString() + "\",\"detailData\":" + json_lev3 + " } "
                                + "]}";
                    }
                    else if (timeStr.Equals("month"))
                    {
                        Hashtable has1 = Bll.T_ReportBll.SearchWatLeakRecMarchData_rep4_month(0);
                        json_lev1 = PluSoft.Utils.JSON.Encode(has1["data"]);
                        Hashtable has2 = Bll.T_ReportBll.SearchWatLeakRecMarchData_rep4_month(1);
                        json_lev2 = PluSoft.Utils.JSON.Encode(has2["data"]);
                        Hashtable has3 = Bll.T_ReportBll.SearchWatLeakRecMarchData_rep4_month(2);
                        json_lev3 = PluSoft.Utils.JSON.Encode(has3["data"]);
                        json = "{\"data\":["
                                         + "{\"timer\":\"" + DateTime.Now.Month.ToString() + "月" + "\",\"detailData\":" + json_lev1 + " }, "
                                         + "{\"timer\":\"" + DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-1).Month.ToString() + "月" + "\",\"detailData\":" + json_lev2 + " }, "
                                         + "{\"timer\":\"" + DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-2).Month.ToString() + "月" + "\",\"detailData\":" + json_lev3 + " } "
                                + "]}";
                    }
                    else if (timeStr.Equals("year"))
                    {
                        Hashtable has1 = Bll.T_ReportBll.SearchWatLeakRecMarchData_rep4_year(0);
                        json_lev1 = PluSoft.Utils.JSON.Encode(has1["data"]);
                        Hashtable has2 = Bll.T_ReportBll.SearchWatLeakRecMarchData_rep4_year(1);
                        json_lev2 = PluSoft.Utils.JSON.Encode(has2["data"]);
                        Hashtable has3 = Bll.T_ReportBll.SearchWatLeakRecMarchData_rep4_year(2);
                        json_lev3 = PluSoft.Utils.JSON.Encode(has3["data"]);
                        json = "{\"data\":["
                                         + "{\"timer\":\"" + DateTime.Now.Year.ToString() + "年" + "\",\"detailData\":" + json_lev1 + " }, "
                                         + "{\"timer\":\"" + (DateTime.Now.Year - 1).ToString() + "年" + "\",\"detailData\":" + json_lev2 + " }, "
                                         + "{\"timer\":\"" + (DateTime.Now.Year - 2).ToString() + "年" + "\",\"detailData\":" + json_lev3 + " } "
                                + "]}";
                    }
                }
                else if(_ReportID.Equals("5"))
                {
                    if (timeStr.Equals("day"))
                    {
                        Hashtable has1 = Bll.T_ReportBll.SearchWatLeakRecMarchData_rep5_day1();
                        json_lev1 = PluSoft.Utils.JSON.Encode(has1["data"]);
                        Hashtable has2 = Bll.T_ReportBll.SearchWatLeakRecMarchData_rep5_day2();
                        json_lev2 = PluSoft.Utils.JSON.Encode(has2["data"]);
                        Hashtable has3 = Bll.T_ReportBll.SearchWatLeakRecMarchData_rep5_day3();
                        json_lev3 = PluSoft.Utils.JSON.Encode(has3["data"]);
                        json = "{\"data\":["
                                         + "{\"timer\":\"" + ((DataTable)has1["data"]).Rows[0]["TM_day"].ToString() + "\",\"detailData\":" + json_lev1 + " }, "
                                         + "{\"timer\":\"" + ((DataTable)has2["data"]).Rows[0]["TM_day"].ToString() + "\",\"detailData\":" + json_lev2 + " }, "
                                         + "{\"timer\":\"" + ((DataTable)has3["data"]).Rows[0]["TM_day"].ToString() + "\",\"detailData\":" + json_lev3 + " } "
                                + "]}";
                    }
                    else if (timeStr.Equals("month"))
                    {
                        Hashtable has1 = Bll.T_ReportBll.SearchWatLeakRecMarchData_rep5_month(0);
                        json_lev1 = PluSoft.Utils.JSON.Encode(has1["data"]);
                        Hashtable has2 = Bll.T_ReportBll.SearchWatLeakRecMarchData_rep5_month(1);
                        json_lev2 = PluSoft.Utils.JSON.Encode(has2["data"]);
                        Hashtable has3 = Bll.T_ReportBll.SearchWatLeakRecMarchData_rep5_month(2);
                        json_lev3 = PluSoft.Utils.JSON.Encode(has3["data"]);
                        json = "{\"data\":["
                                         + "{\"timer\":\"" + DateTime.Now.Month.ToString() + "月" + "\",\"detailData\":" + json_lev1 + " }, "
                                         + "{\"timer\":\"" + DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-1).Month.ToString() + "月" + "\",\"detailData\":" + json_lev2 + " }, "
                                         + "{\"timer\":\"" + DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-2).Month.ToString() + "月" + "\",\"detailData\":" + json_lev3 + " } "
                                + "]}";
                    }
                    else if (timeStr.Equals("year"))
                    {
                        Hashtable has1 = Bll.T_ReportBll.SearchWatLeakRecMarchData_rep5_year(0);
                        json_lev1 = PluSoft.Utils.JSON.Encode(has1["data"]);
                        Hashtable has2 = Bll.T_ReportBll.SearchWatLeakRecMarchData_rep5_year(1);
                        json_lev2 = PluSoft.Utils.JSON.Encode(has2["data"]);
                        Hashtable has3 = Bll.T_ReportBll.SearchWatLeakRecMarchData_rep5_year(2);
                        json_lev3 = PluSoft.Utils.JSON.Encode(has3["data"]);
                        json = "{\"data\":["
                                         + "{\"timer\":\"" + DateTime.Now.Year.ToString() + "年" + "\",\"detailData\":" + json_lev1 + " }, "
                                         + "{\"timer\":\"" + (DateTime.Now.Year - 1).ToString() + "年" + "\",\"detailData\":" + json_lev2 + " }, "
                                         + "{\"timer\":\"" + (DateTime.Now.Year - 2).ToString() + "年" + "\",\"detailData\":" + json_lev3 + " } "
                                + "]}";
                    }
                }
                else if (_ReportID.Equals("6"))
                {
                    if (timeStr.Equals("day"))
                    {
                        Hashtable has1 = Bll.T_ReportBll.SearchWatLeakRecMarchData_rep6_day1();
                        json_lev1 = PluSoft.Utils.JSON.Encode(has1["data"]);
                        Hashtable has2 = Bll.T_ReportBll.SearchWatLeakRecMarchData_rep6_day2();
                        json_lev2 = PluSoft.Utils.JSON.Encode(has2["data"]);
                        Hashtable has3 = Bll.T_ReportBll.SearchWatLeakRecMarchData_rep6_day3();
                        json_lev3 = PluSoft.Utils.JSON.Encode(has3["data"]);
                        json = "{\"data\":["
                                         + "{\"timer\":\"" + ((DataTable)has1["data"]).Rows[0]["TM_day"].ToString() + "\",\"detailData\":" + json_lev1 + " }, "
                                         + "{\"timer\":\"" + ((DataTable)has2["data"]).Rows[0]["TM_day"].ToString() + "\",\"detailData\":" + json_lev2 + " }, "
                                         + "{\"timer\":\"" + ((DataTable)has3["data"]).Rows[0]["TM_day"].ToString() + "\",\"detailData\":" + json_lev3 + " } "
                                + "]}";
                    }
                    else if (timeStr.Equals("month"))
                    {
                        Hashtable has1 = Bll.T_ReportBll.SearchWatLeakRecMarchData_rep6_month(0);
                        json_lev1 = PluSoft.Utils.JSON.Encode(has1["data"]);
                        Hashtable has2 = Bll.T_ReportBll.SearchWatLeakRecMarchData_rep6_month(1);
                        json_lev2 = PluSoft.Utils.JSON.Encode(has2["data"]);
                        Hashtable has3 = Bll.T_ReportBll.SearchWatLeakRecMarchData_rep6_month(2);
                        json_lev3 = PluSoft.Utils.JSON.Encode(has3["data"]);
                        json = "{\"data\":["
                                         + "{\"timer\":\"" + DateTime.Now.Month.ToString() + "月" + "\",\"detailData\":" + json_lev1 + " }, "
                                         + "{\"timer\":\"" + DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-1).Month.ToString() + "月" + "\",\"detailData\":" + json_lev2 + " }, "
                                         + "{\"timer\":\"" + DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-2).Month.ToString() + "月" + "\",\"detailData\":" + json_lev3 + " } "
                                + "]}";
                    }
                    else if (timeStr.Equals("year"))
                    {
                        Hashtable has1 = Bll.T_ReportBll.SearchWatLeakRecMarchData_rep6_year(0);
                        json_lev1 = PluSoft.Utils.JSON.Encode(has1["data"]);
                        Hashtable has2 = Bll.T_ReportBll.SearchWatLeakRecMarchData_rep6_year(1);
                        json_lev2 = PluSoft.Utils.JSON.Encode(has2["data"]);
                        Hashtable has3 = Bll.T_ReportBll.SearchWatLeakRecMarchData_rep6_year(2);
                        json_lev3 = PluSoft.Utils.JSON.Encode(has3["data"]);
                        json = "{\"data\":["
                                         + "{\"timer\":\"" + DateTime.Now.Year.ToString() + "年" + "\",\"detailData\":" + json_lev1 + " }, "
                                         + "{\"timer\":\"" + (DateTime.Now.Year - 1).ToString() + "年" + "\",\"detailData\":" + json_lev2 + " }, "
                                         + "{\"timer\":\"" + (DateTime.Now.Year - 2).ToString() + "年" + "\",\"detailData\":" + json_lev3 + " } "
                                + "]}";
                    }
                }
            }

            HttpContext.Current.Response.Write(json);
        }

        public void GetMonthCostData()
        {
            String _ReportID = HttpContext.Current.Request["ReportID"];
            string json = "";

            //switch (_ReportID)
            //{
                //case "1": jsonStr = "{\"data\": [{"
                //                         + "\"monthData1\": [{\"monthType\":\"月成本\",\"Y\": [220, 182, 900, 222, 11, 565, 23, 123, 442, 321, 90, 149]}],"
                //                         + "\"monthData2\": [{\"monthType\":\"月损失成本\",\"Y\": [100,20,50,888,30,50,40,222,58,62,46,52]}]"
                //                   + "}]}"; break;
                //case "2": jsonStr = "{\"data\": [{"
                //                         + "\"monthData1\": [{\"monthType\":\"月成本\",\"Y\": [220, 182, 191, 233, 999, 11, 310, 123, 43, 321, 90, 149]}],"
                //                         + "\"monthData2\": [{\"monthType\":\"月损失成本\",\"Y\": [100,888,50,60,30,50,40,99,58,62,46,52]}]"
                //                   + "}]}"; break;
            //}

            if(_ReportID.Equals("1") || _ReportID.Equals("2"))
            {
                Hashtable has = Bll.T_ReportBll.GetMonthCostData(Convert.ToInt32(_ReportID));
                json = PluSoft.Utils.JSON.Encode(has["data"]);
            }

            HttpContext.Current.Response.Write(json);
        }

        public void GetMonthQuarterData()
        {
            String _ReportID = HttpContext.Current.Request["ReportID"];
            string jsonStr = "";

            //String json = "";
            //String json_week1 = PluSoft.Utils.JSON.Encode(has1["data"]);
            //String json_week2 = PluSoft.Utils.JSON.Encode(has2["data"]);
            if (_ReportID.Equals("1") || _ReportID.Equals("2"))
            {
                Hashtable has1 = Bll.T_ReportBll.GetMonthCostDataBT(Convert.ToInt32(_ReportID));
                String json1 = PluSoft.Utils.JSON.Encode(has1["data"]);

                Hashtable has2 = Bll.T_ReportBll.GetQuarterCostDataBT(Convert.ToInt32(_ReportID));
                String json2 = PluSoft.Utils.JSON.Encode(has2["data"]);

                jsonStr = "{"
                             + "\"monthData\":" + json1 + ","
                             + "\"quarterData\":" + json2 +"}";
            }
            //string jsonStr = "{"
            //                     + "\"monthData\":["
            //                                     + "{\"name\":\"1月\",\"value\":\"335\"},"
            //                                     + "{\"name\":\"2月\",\"value\":\"310\"},"
            //                                     + "{\"name\":\"3月\",\"value\":\"234\"},"
            //                                     + "{\"name\":\"4月\",\"value\":\"365\"},"
            //                                     + "{\"name\":\"5月\",\"value\":\"768\"},"
            //                                     + "{\"name\":\"6月\",\"value\":\"301\"},"
            //                                     + "{\"name\":\"7月\",\"value\":\"251\"},"
            //                                     + "{\"name\":\"8月\",\"value\":\"510\"},"
            //                                     + "{\"name\":\"9月\",\"value\":\"858\"},"
            //                                     + "{\"name\":\"10月\",\"value\":\"301\"},"
            //                                     + "{\"name\":\"11月\",\"value\":\"251\"},"
            //                                     + "{\"name\":\"12月\",\"value\":\"510\"}"
            //                     + "],"
            //                     + "\"quarterData\":["
            //                                       + "{\"name\":\"1季度\",\"value\":\"835\"},"
            //                                       + "{\"name\":\"2季度\",\"value\":\"679\"},"
            //                                       + "{\"name\":\"3季度\",\"value\":\"1548\"},"
            //                                       + "{\"name\":\"4季度\",\"value\":\"748\"}"
            //                     + "]"
            //               + "}";

            //switch (_ReportID)
            //{
            //    case "1": jsonStr = "{"
            //                     + "\"monthData\":["
            //                                     + "{\"name\":\"1月\",\"value\":\"45\"},"
            //                                     + "{\"name\":\"2月\",\"value\":\"55\"},"
            //                                     + "{\"name\":\"3月\",\"value\":\"234\"},"
            //                                     + "{\"name\":\"4月\",\"value\":\"365\"},"
            //                                     + "{\"name\":\"5月\",\"value\":\"45\"},"
            //                                     + "{\"name\":\"6月\",\"value\":\"301\"},"
            //                                     + "{\"name\":\"7月\",\"value\":\"251\"},"
            //                                     + "{\"name\":\"8月\",\"value\":\"44\"},"
            //                                     + "{\"name\":\"9月\",\"value\":\"858\"},"
            //                                     + "{\"name\":\"10月\",\"value\":\"301\"},"
            //                                     + "{\"name\":\"11月\",\"value\":\"251\"},"
            //                                     + "{\"name\":\"12月\",\"value\":\"510\"}"
            //                     + "],"
            //                     + "\"quarterData\":["
            //                                       + "{\"name\":\"1季度\",\"value\":\"835\"},"
            //                                       + "{\"name\":\"2季度\",\"value\":\"4\"},"
            //                                       + "{\"name\":\"3季度\",\"value\":\"45\"},"
            //                                       + "{\"name\":\"4季度\",\"value\":\"748\"}"
            //                     + "]"
            //               + "}"; break;
            //    case "2": jsonStr = "{"
            //                     + "\"monthData\":["
            //                                     + "{\"name\":\"1月\",\"value\":\"335\"},"
            //                                     + "{\"name\":\"2月\",\"value\":\"45\"},"
            //                                     + "{\"name\":\"3月\",\"value\":\"234\"},"
            //                                     + "{\"name\":\"4月\",\"value\":\"365\"},"
            //                                     + "{\"name\":\"5月\",\"value\":\"77\"},"
            //                                     + "{\"name\":\"6月\",\"value\":\"77\"},"
            //                                     + "{\"name\":\"7月\",\"value\":\"251\"},"
            //                                     + "{\"name\":\"8月\",\"value\":\"88\"},"
            //                                     + "{\"name\":\"9月\",\"value\":\"858\"},"
            //                                     + "{\"name\":\"10月\",\"value\":\"301\"},"
            //                                     + "{\"name\":\"11月\",\"value\":\"88\"},"
            //                                     + "{\"name\":\"12月\",\"value\":\"510\"}"
            //                     + "],"
            //                     + "\"quarterData\":["
            //                                       + "{\"name\":\"1季度\",\"value\":\"77\"},"
            //                                       + "{\"name\":\"2季度\",\"value\":\"679\"},"
            //                                       + "{\"name\":\"3季度\",\"value\":\"88\"},"
            //                                       + "{\"name\":\"4季度\",\"value\":\"748\"}"
            //                     + "]"
            //               + "}"; break;
            //}

            //String json = PluSoft.Utils.JSON.Encode(jsonStr);
            HttpContext.Current.Response.Write(jsonStr);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}