using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using FineUIMvc;

namespace FineUIMvc.PumpMVC.DAL
{
    public class E_DATADal
    {
        private const string str_E_DATAMAINUpd = @"UPDATE E_DATA_MAIN SET {0} WHERE {1}";
        private const string str_E_DATAMAINAdd = @"INSERT INTO E_DATA_MAIN ( {0} ) VALUES( {1} )";
        public static void Insert(Hashtable has)
        {
            publicDal.Insert(has, str_E_DATAMAINAdd);
        }

        public static void Update(Hashtable has)
        {
            publicDal.Update(has, str_E_DATAMAINUpd, "BaseID");
        }

        public static DataTable Search_M(int i, string date, string BaseID, string field)
        {
            string sql = @"select top (dbo.udf_DaysInMonth('" + date + "')) T_MonthDay T_Time,0 result,isnull(data_max,0) as data_max,isnull(data_min,0) as data_min from T_MonthDays a "
                   +@" left join(
                           select  day(CONVERT(varchar(10),isnull(TempTime,getdate()),120)) as rowNum,CONVERT(varchar(10),isnull(TempTime,getdate()),120) as T_Data,
                                 max(" + field + ") as data_max,min(" + field + ") as data_min from E_DATA" + Convert.ToDateTime(date).Year.ToString() + " "
                       + @"    where (datediff(month,TempTime,getdate())=" + i + ") and BaseID='" + BaseID + "' and " + field + ">0 and " + field + " is not null "
                   +@"    group by CONVERT(varchar(10),isnull(TempTime,getdate()),120))b on a.id=b.rowNum	
                              order by T_Time";

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
            return dt;
        }

        public static DataTable Search_Y(int i, string date, string BaseID, string field)
        {
            string sql = @"select T_yearMonth as T_Time,0 result,isnull(data_max,0) as data_max,isnull(data_min,0) as data_min from T_YearMonths a
                          left join(
                          select month(isnull(TempTime,getdate())) as T_Data,
                          max(" + field + ") as data_max,min(" + field + ") as data_min from E_DATA" + Convert.ToDateTime(date).Year.ToString() + ""
                        + @"  where datediff(year,TempTime,getdate())=" + i + " and BaseID='" + BaseID + "' and " + field + ">0 and " + field + " is not null "
                        +@"    group by month(isnull(TempTime,getdate())))b on a.T_yearMonth=b.T_Data
                            where a.T_lev=1";

            DataTable dt = new DataTable();
            try
            {
                  dt = publicDal.TableSearch(sql);
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
            }
            catch
            {
                dt = null;
            }

            return dt;
        }

        public static DataTable Search_D(int i, string date, string BaseID, string field)
        {
            string sql = @"select T_dayHour T_Time,0 result,isnull(data_max,0) as data_max,isnull(data_min,0) as data_min from T_DayHours a
                           left join(
                           select datepart(hour,isnull(TempTime,getdate())) as T_Data,
                           max(" + field + ") as data_max,min(" + field + ") as data_min from E_DATA" + Convert.ToDateTime(date).Year.ToString() + ""
                      + @" where datediff(day,TempTime,getdate())=" + i + " and BaseID='" + BaseID + "' and " + field + ">0 and " + field + " is not null "
                    + @"   group by datepart(hour,isnull(TempTime,getdate())))b on a.T_dayHour=b.T_Data
                             where a.T_lev=1";

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
            return dt;
        }

        public static DataTable SearchDSNH_M(string BaseID,string year, string month)
        {
            string sql = @"select top (dbo.udf_DaysInMonth('" + year + "-" + month + "-01" + "')) id,t_monthDay,isnull(data_dl,0) as FTotalDL,isnull(data_sl,0) as FTotalOutLL,"
                           + @"   dsnh=case when data_dl=0 then 0 else isnull((round(data_dl/data_sl,2)),0) end from T_MonthDays a
                              left join(
                              select  day(CONVERT(varchar(10),isnull(TempTime,getdate()),120)) as rowNum,CONVERT(varchar(10),isnull(TempTime,getdate()),120) as T_Data,
                                     (max(isnull(CONVERT(float,FTotalDL),0))-min(isnull(CONVERT(float,FTotalDL),0))) as data_dl,
                              (max(isnull(CONVERT(float,FTotalOutLL),0))-min(isnull(CONVERT(float,FTotalOutLL),0))) as data_sl from E_DATA" + year + ""
                          + @"    where year(TempTime)=" + year + " and month(TempTime)=" + month + " and BaseID='" + BaseID + "' and (FTotalDL>0 and FTotalDL is not null)  and (FTotalOutLL>0 and FTotalOutLL is not null) "
                            + @"       group by CONVERT(varchar(10),isnull(TempTime,getdate()),120))b on a.id=b.rowNum	
                                 order by id ";

            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }

        public static DataTable SearchDSNH_Y(string BaseID, string year)
        {
            string sql = @"select T_yearMonth,isnull(data_dl,0) as FTotalDL,isnull(data_sl,0) as FTotalOutLL,
                           dsnh=case when data_dl=0 then 0 else isnull((round(data_dl/data_sl,2)),0) end from T_YearMonths a
                           left join(
                           select month(isnull(TempTime,getdate())) as T_Data,
                           (max(isnull(CONVERT(float,FTotalDL),0))-min(isnull(CONVERT(float,FTotalDL),0))) as data_dl,
                           (max(isnull(CONVERT(float,FTotalOutLL),0))-min(isnull(CONVERT(float,FTotalOutLL),0))) as data_sl from E_DATA" + year + ""
                        + @"   where BaseID='" + BaseID + "' and (FTotalDL>0 and FTotalDL is not null)  and (FTotalOutLL>0 and FTotalOutLL is not null) "
                        +@"     group by month(isnull(TempTime,getdate())))b on a.T_yearMonth=b.T_Data
                             where a.T_lev=1";

            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }

        public static DataTable SearchDSNH_D(string BaseID, string date)
        {
            string sql = @"select T_dayHour,isnull(data_dl,0) as FTotalDL,isnull(data_sl,0) as FTotalOutLL,
                            dsnh=case when data_dl=0 then 0 else isnull((round(data_dl/data_sl,2)),0) end from T_DayHours a
                            left join(
                            select datepart(hour,isnull(TempTime,getdate())) as T_Data,
                            (max(isnull(CONVERT(float,FTotalDL),0))-min(isnull(CONVERT(float,FTotalDL),0))) as data_dl,
                            (max(isnull(CONVERT(float,FTotalOutLL),0))-min(isnull(CONVERT(float,FTotalOutLL),0))) as data_sl from E_DATA" + Convert.ToDateTime(date).Year.ToString() + ""
                          + @"  where CONVERT(varchar(10),TempTime,120)='" + date + "' and BaseID='" + BaseID + "' and (FTotalDL>0 and FTotalDL is not null)  and (FTotalOutLL>0 and FTotalOutLL is not null) "
                          +@"    group by datepart(hour,isnull(TempTime,getdate())))b on a.T_dayHour=b.T_Data
                              where a.T_lev=1";

            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }
        public static DataTable SearchDSNH_Q(string BaseID,string s_date, string e_date)
        {
            string sql = @"select FStr as T_Time,isnull(data_dl,0) as FTotalDL,isnull(data_sl,0) as FTotalOutLL,
                            dsnh=case when data_dl=0 then 0 else isnull((round(data_dl/data_sl,2)),0) end from dbo.FunGetDayHour('" + s_date + "','" + e_date + "') a "
                          + @"  left join(
                            select year(TempTime) as FYear,month(TempTime) as FMonth,day(TempTime) as FDay,datepart(hour,TempTime) as FHour,
                            (max(isnull(CONVERT(float,FTotalDL),0))-min(isnull(CONVERT(float,FTotalDL),0))) as data_dl,
                            (max(isnull(CONVERT(float,FTotalOutLL),0))-min(isnull(CONVERT(float,FTotalOutLL),0))) as data_sl from E_DATA" + Convert.ToDateTime(s_date).Year.ToString() + ""
                          + @"  where (TempTime between '" + s_date + "' and '" + e_date + "') and BaseID='" + BaseID + "' and (FTotalDL>0 and FTotalDL is not null)  and (FTotalOutLL>0 and FTotalOutLL is not null) "
                          + @"   group by year(TempTime),month(TempTime),day(TempTime),datepart(hour,TempTime))b on a.FYear=b.FYear and a.FMonth=b.FMonth and a.FDay=b.FDay and a.FHour=b.FHour
                              order by a.FYear,a.FMonth,a.FDay,a.FHour ";

            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }
    }
}