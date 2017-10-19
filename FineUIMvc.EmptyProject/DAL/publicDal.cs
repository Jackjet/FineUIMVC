using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using FineUIMvc.PumpMVC.Controllers;
using System.Text;

namespace FineUIMvc.PumpMVC.DAL
{
    public class publicDal
    {
        #region 基本增删改查通用
        public static Hashtable HashSearch(int index, int size, string sortField, string sortOrder, string strWhere, string sql)
        {
            string where = strWhere;

            if (!where.Equals(""))
            {
                sql = sql + where;
            }

            if (String.IsNullOrEmpty(sortField) == false)
            {
                sortOrder = sortOrder.ToUpper();
                if (sortOrder != "DESC") sortOrder = "ASC";
                sql += " order by " + sortField + " " + sortOrder + " ";
                int StartRecord = 0;
                StartRecord = index * size;
                sql += " offset " + StartRecord + " rows fetch next " + size + " rows only";
            }
            DataTable dt;
            Int64 count = 0;
            using ( var DB = new DBController())
            {
                dt = DB.SqlQueryForDataTatable(sql);

                count = DB.ExecuteScalar(sql);
            }
            Hashtable result = new Hashtable();
            result["data"] = dt;
            result["total"] = count;


            return result;
        }

        public static Hashtable HashSearch(int index, int size, string sortField, string sortOrder, string strWhere, string sql, string sqlcount)
        {
            string where = strWhere;

            if (!where.Equals(""))
            {
                sql = sql + where;
                sqlcount = sqlcount + where;
            }

            if (String.IsNullOrEmpty(sortField) == false)
            {
                sortOrder = sortOrder.ToUpper();
                if (sortOrder != "DESC") sortOrder = "ASC";
                sql += " order by " + sortField + " " + sortOrder + " ";
                int StartRecord = 0;
                StartRecord = index * size;
                sql += " offset " + StartRecord + " rows fetch next " + size + " rows only";
            }

            DataTable dt;
            Int64 count = 0;
            using (var DB = new DBController())
            {
                dt = DB.SqlQueryForDataTatable(sql);

                count = Convert.ToInt64(DB.SqlQueryForDataTatable(sqlcount).Rows[0][0].ToString());
            }

            Hashtable result = new Hashtable();
            result["data"] = dt;
            result["total"] = count;

            return result;
        }

        public static DataTable TableSearch(string strWhere, string sql)
        {
            string where = strWhere;

            if (!where.Equals(""))
            {
                sql = sql + where;
            }

            DataTable dt;

            using (var DB = new DBController())
            {
                dt = DB.SqlQueryForDataTatable(sql);
            }

            return dt;
        }

        public static DataTable TableSearch(string sql)
        {
            DataTable dt;

            using (var DB = new DBController())
            {
                dt = DB.SqlQueryForDataTatable(sql);
            }

            return dt;
        }

        public static Hashtable has_Select(string sql)
        {
            ArrayList data;
            using (var DB = new DBController())
            {
                data = DB.Select(sql);
            }
            return data.Count > 0 ? (Hashtable)data[0] : null;
        }

        public static ArrayList arr_Select(string sql)
        {
            ArrayList data;
            using (var DB = new DBController())
            {
                data = DB.Select(sql);
            }
            return data.Count > 0 ? data : null;
        }

        public static void Insert(Hashtable has, string InsSql)
        {
            string columns = "";
            string values = "";
            foreach (DictionaryEntry de in has)
            {
                columns += " " + de.Key + ",";
                values += "@" + de.Key + ",";
            }
            string sql = string.Format(InsSql, columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1));

            using (var DB = new DBController())
            {
                DB.Execute(sql, has);
            }
        }

        public static void InsertUpd(Hashtable has, string InsSql, string id)
        {
            string columns = "";
            string values = "";
            foreach (DictionaryEntry de in has)
            {
                columns += " " + de.Key + ",";
                values += "@" + de.Key + ",";
            }
            string sql = string.Format(InsSql, columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1), id);

            using (var DB = new DBController())
            {
                DB.Execute(sql, has);
            }
        }
        public static int InsertGetID(Hashtable has, string InsSql)
        {
            string columns = "";
            string values = "";
            int id = 0;
            foreach (DictionaryEntry de in has)
            {
                columns += " " + de.Key + ",";
                values += "@" + de.Key + ",";
            }
            string sql = string.Format(InsSql, columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1));

            using (var DB = new DBController())
            {
                id=DB.ExecuteID(sql, has);
            }
            return id;
        }

        public static void Update(Hashtable has, string UpdSql, string keyName)
        {
            string set = "";
            string where = "";
            foreach (DictionaryEntry de in has)
            {
                if (!(',' + keyName + ',').Contains(',' + de.Key.ToString() + ','))
                {
                    set += " " + de.Key + " = @" + de.Key + ",";
                }
                else
                {
                    where += " " + de.Key + " = @" + de.Key + " and ";
                }
            }
            string sql = string.Format(UpdSql, set.Substring(0, set.Length - 1), where);
            sql = sql.Substring(0, sql.Length - 4);
            using (var DB = new DBController())
            {
                DB.Execute(sql, has);
            }
        }

        public static void Update(string UpdSql)
        {
            using (var DB = new DBController())
            {
                DB.Execute(UpdSql);
            }
        }
        public static void UpdateUpd(Hashtable has, string UpdSql, string keyName, string id)
        {
            string set = "";
            string where = "";
            foreach (DictionaryEntry de in has)
            {
                if (de.Key.ToString() != keyName)
                {
                    set += " " + de.Key + " = @" + de.Key + ",";
                }
                else
                {
                    where += " " + de.Key + " = @" + de.Key + "";
                }
            }
            string sql = string.Format(UpdSql, set.Substring(0, set.Length - 1), where, id);
            using (var DB = new DBController())
            {
                DB.Execute(sql, has);
            }
        }

        public static void Delete(string DelSql)
        {
            using (var DB = new DBController())
            {
                DB.Execute(DelSql);
            }
        }

        public static void DeleteList(Hashtable has, string DelSql)
        {
            string sql = string.Format(DelSql, has["ID"]);

            using (var DB = new DBController())
            {
                DB.Execute(sql, has);
            }
        }

        public static void DeleteList(Hashtable has, string DelSql, string keyName)
        {
            string set = "";
            string where = "";
            foreach (DictionaryEntry de in has)
            {
                if (de.Key.ToString() != keyName)
                {
                    set += " " + de.Key + " = @" + de.Key + ",";
                }
                else
                {
                    where += " " + de.Key + " in ( " + de.Value + ")";
                }
            }
            string sql = string.Format(DelSql, set.Substring(0, set.Length - 1), where);
            //string sql = string.Format(DelSql, has["ID"]);

            using (var DB = new DBController())
            {
                DB.Execute(sql, has);
            }
        }

        public static void UpdList(Hashtable has, string UpdSql, string keyName)
        {
            string set = "";
            string where = "";
            foreach (DictionaryEntry de in has)
            {
                if (de.Key.ToString() != keyName)
                {
                    set += " " + de.Key + " = @" + de.Key + ",";
                }
                else
                {
                    where += " " + de.Key + " in ( " + de.Value + ")";
                }
            }
            string sql = string.Format(UpdSql, set.Substring(0, set.Length - 1), where);

            using (var DB = new DBController())
            {
                DB.Execute(sql, has);
            }
        }
        #endregion

        #region DATA_-按日期统计
        /// <summary>
        /// 年
        /// </summary>
        /// <param name="year"></param>
        /// <param name="BaseID"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static DataTable Search_Y(string table_, string year, string BaseID, string field)
        {
            string sql = @"select T_yearMonth as T_Time,0 result,isnull(data_max,0) as data_max,isnull(data_min,0) as data_min from T_YearMonths a
                          left join(
                          select month(isnull(TempTime,getdate())) as T_Data,
                          max(" + field + ") as data_max,min(" + field + ") as data_min from " + table_ + year + ""
                        + @"  where BASEID='" + BaseID + "' and " + field + ">0 and " + field + " is not null "
                        + @"    group by month(isnull(TempTime,getdate())))b on a.T_yearMonth=b.T_Data
                            where a.T_lev=1";
            DataTable dt = new DataTable();
            try
            {
                dt = TableSearch(sql);
            }
            catch
            {
                dt = null;
            }
            return dt;
        }
        /// <summary>
        /// 月
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="BaseID"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static DataTable Search_M(string table_, string year, string month, string BaseID, string field)
        {
            string sql = @"select top (dbo.udf_DaysInMonth('" + year + "-" + month + "-01" + "'))  T_MonthDay T_Time,0 result,isnull(data_max,0) as data_max,isnull(data_min,0) as data_min from T_MonthDays a "
                   + @" left join(
                           select  day(CONVERT(varchar(10),isnull(TempTime,getdate()),120)) as rowNum,CONVERT(varchar(10),isnull(TempTime,getdate()),120) as T_Data,
                                  max(" + field + ") as data_max,min(" + field + ") as data_min from " + table_ + year + " "
                       + @"    where year(TempTime)=" + year + " and month(TempTime)=" + month + " and BASEID='" + BaseID + "' and " + field + ">0 and " + field + " is not null "
                   + @"    group by CONVERT(varchar(10),isnull(TempTime,getdate()),120))b on a.id=b.rowNum	
                              order by T_Time";

            DataTable dt = new DataTable();
            try
            {
                dt = TableSearch(sql);
            }
            catch
            {
                dt = null;
            }
            return dt;
        }
        /// <summary>
        /// 日
        /// </summary>
        /// <param name="date"></param>
        /// <param name="BaseID"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static DataTable Search_D(string table_, string date, string BaseID, string field)
        {
            string sql = @"select T_dayHour as T_Time,0 result,isnull(data_max,0) as data_max,isnull(data_min,0) as data_min from T_DayHours a
                           left join(
                           select datepart(hour,isnull(TempTime,getdate())) as T_Data,
                           max(" + field + ") as data_max,min(" + field + ") as data_min from " + table_ + Convert.ToDateTime(date).Year.ToString() + ""
                      + @" where CONVERT(varchar(10),TempTime,120)='" + date + "' and BASEID='" + BaseID + "' and " + field + ">0 and " + field + " is not null "
                    + @"   group by datepart(hour,isnull(TempTime,getdate())))b on a.T_dayHour=b.T_Data
                             where a.T_lev=1";

            DataTable dt = new DataTable();
            try
            {
                dt = TableSearch(sql);
            }
            catch
            {
                dt = null;
            }
            return dt;
        }
        /// <summary>
        /// 日期-小时区间
        /// </summary>
        /// <param name="s_date"></param>
        /// <param name="e_date"></param>
        /// <param name="BaseID"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static DataTable Search_Q(string table_, string s_date, string e_date, string BaseID, string field)
        {
            string sql = @"select FStr as T_Time,0 result,isnull(data_max,0) as data_max,isnull(data_min,0) as data_min from dbo.FunGetDayHour('" + s_date + "','" + e_date + "') a "
                     + @"   left join(
                        select year(TempTime) as FYear,month(TempTime) as FMonth,day(TempTime) as FDay,datepart(hour,TempTime) as FHour,
                        max(" + field + ") as data_max,min(" + field + ") as data_min from " + table_ + Convert.ToDateTime(s_date).Year.ToString() + ""
                     + @"   where (TempTime between '" + Convert.ToDateTime(s_date).ToShortDateString() + "' and '" + Convert.ToDateTime(e_date).ToShortDateString() + "') and BASEID='" + BaseID + "' and " + field + ">0 and " + field + " is not null"
                    + @"    group by year(TempTime),month(TempTime),day(TempTime),datepart(hour,TempTime))b on a.FYear=b.FYear and a.FMonth=b.FMonth and a.FDay=b.FDay and a.FHour=b.FHour
                          order by a.FYear,a.FMonth,a.FDay,a.FHour ";

            DataTable dt = new DataTable();
            try
            {
                dt = TableSearch(sql);
            }
            catch
            {
                dt = null;
            }
            return dt;
        }

        /// <summary>
        /// 月-日期区间
        /// </summary>
        /// <param name="s_date"></param>
        /// <param name="e_date"></param>
        /// <param name="BaseID"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static DataTable Search_MD(string table_, string s_date, string e_date, string BaseID, string field)
        {
            string sql = @"select FStr as T_Time,0 result,isnull(data_max,0) as data_max,isnull(data_min,0) as data_min from dbo.FunGetMonthDay('" + s_date + "','" + e_date + "') a "
                     + @"   left join(
                        select year(TempTime) as FYear,month(TempTime) as FMonth,day(TempTime) as FDay,
                        max(" + field + ") as data_max,min(" + field + ") as data_min from " + table_ + Convert.ToDateTime(s_date).Year.ToString() + ""
                     + @"   where (TempTime between '" + Convert.ToDateTime(s_date).ToString("yyyy-MM-dd") + " 00:00:00" + "' and '" + Convert.ToDateTime(e_date).ToString("yyyy-MM-dd") + " 23:59:59" + "') and BASEID='" + BaseID + "' and " + field + ">0 and " + field + " is not null"
                    + @"    group by year(TempTime),month(TempTime),day(TempTime))b on a.FYear=b.FYear and a.FMonth=b.FMonth and a.FDay=b.FDay
                          order by a.FYear,a.FMonth,a.FDay ";

            DataTable dt = new DataTable();
            try
            {
                dt = TableSearch(sql);
            }
            catch
            {
                dt = null;
            }
            return dt;
        }

        public void Search_DateType(string table_, string field)
        {
            StringBuilder str = new StringBuilder();
            try
            {
                string timeType = HttpContext.Current.Request["timeType"];   //搜索年/月/日
                string BASEID = HttpContext.Current.Request["BASEID"];
                string year = HttpContext.Current.Request["year"];
                string month = HttpContext.Current.Request["month"];
                string day = HttpContext.Current.Request["day"];
                string s_date = HttpContext.Current.Request["s_date"];
                string e_date = HttpContext.Current.Request["e_date"];

                DataTable dt = new DataTable();
                switch (timeType)
                {
                    case "1": dt = publicDal.Search_Y(table_, year, BASEID, field); break;      //年
                    case "2": dt = publicDal.Search_M(table_, year, month, BASEID, field); break;      //月
                    case "3": dt = publicDal.Search_D(table_, day, BASEID, field); break;      //日
                    case "4": dt = publicDal.Search_Q(table_, s_date, e_date, BASEID, field); break;      //日期-小时区间
                    case "5": dt = publicDal.Search_MD(table_, s_date, e_date, BASEID, field); break;      //月-日期区间
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i < dt.Rows.Count - 1)
                    {
                        if (Convert.ToInt32(dt.Rows[i]["data_min"]) > Convert.ToInt32(dt.Rows[i]["data_max"]))
                        {
                            dt.Rows[i]["data_max"] = dt.Rows[i]["data_min"];
                        }
                        dt.Rows[i + 1]["data_min"] = dt.Rows[i]["data_max"];
                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i]["data_min"]) > Convert.ToInt32(dt.Rows[i]["data_max"]))
                        {
                            dt.Rows[i]["data_max"] = dt.Rows[i]["data_min"];
                        }
                    }
                    dt.Rows[i]["result"] = Convert.ToInt32(dt.Rows[i]["data_max"]) - Convert.ToInt32(dt.Rows[i]["data_min"]);
                }

                str = BaseController.successMsg("查询成功", "true", PluSoft.Utils.JSON.Encode(dt));
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "查询失败," + e.Message;
                str = BaseController.successMsg(msg, "false");
            }
            HttpContext.Current.Response.Write(str);
        }
        #endregion
    }
}