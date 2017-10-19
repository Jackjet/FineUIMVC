using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using Water.Web.Service;

namespace Dal
{
    public class T_ReportDal
    {
    

        public static Hashtable SearchTopLableDate(string strWhere)
        {
            //String sql = "select sum(convert(float,case when FLev=1 then P01 else 0 end)) Lev1_P01 ,sum(convert(float,case when FLev=2 then P01 else 0 end)) Lev2_P01 ,sum(convert(float,case when FLev=3 then P01 else 0 end)) Lev3_P01 ,sum(convert(float,case when FLev=1 then P01 else 0 end))-sum(convert(float,case when FLev=2 then P01 else 0 end)) Lev1_Miss,sum(convert(float,case when FLev=2 then P01 else 0 end))-sum(convert(float,case when FLev=3 then P01 else 0 end)) Lev2_Miss,sum(convert(float,case when FLev=1 then P01 else 0 end))-sum(convert(float,case when FLev=3 then P01 else 0 end)) LevZ_Miss ,(sum(convert(float,case when FLev=1 then P01 else 0 end))-sum(convert(float,case when FLev=2 then P01 else 0 end)))/sum(convert(float,case when FLev=1 then P01 else 0 end)) *100.00 Lev1_Miss_P,(sum(convert(float,case when FLev=2 then P01 else 0 end))-sum(convert(float,case when FLev=3 then P01 else 0 end)))/sum(convert(float,case when FLev=2 then P01 else 0 end)) *100.00 Lev2_Miss_P,(sum(convert(float,case when FLev=1 then P01 else 0 end))-sum(convert(float,case when FLev=3 then P01 else 0 end)))/sum(convert(float,case when FLev=1 then P01 else 0 end)) *100.00 LevZ_Miss_P,sum(convert(float,case when FLev=1 then P01 else 0 end))/1000*1.2 Mount,(sum(convert(float,case when FLev=1 then P01 else 0 end))-sum(convert(float,case when FLev=3 then P01 else 0 end)))/1000*1.2 Mount1 from PumpManager a,T_Data2016 b where a.ID=b.FPumpID ";
            //sql = sql + strWhere;
            
            String sql = @"with flow1 as( 
                                   select 1 id, convert(float,(sum(CONVERT(int,Max_P01))-sum(CONVERT(int,Min_P01)))) as Lev1_P01
                                    from (select b.ID as PumpID,max(CONVERT(int,isnull(P01,0))) as Max_P01,min(CONVERT(int,isnull(P01,0))) as Min_P01 
                                            from PumpManager b 
                                       left join T_Data2016 c on  c.FPumpID=b.ID " + strWhere
                                       + @" where b.FDeleted=0  and FLev=1 group by b.ID) a)
                               ,flow2 as(
                                   select 1 id, convert(float,(sum(CONVERT(int,Max_P01))-sum(CONVERT(int,Min_P01)))) as Lev2_P01
                                     from (select b.ID as PumpID,max(CONVERT(int,isnull(P01,0))) as Max_P01,min(CONVERT(int,isnull(P01,0))) as Min_P01 
                                             from  PumpManager b 
                                        left join T_Data2016 c on  c.FPumpID=b.ID " + strWhere
                                       + @" where b.FDeleted=0  and FLev=2 group by b.ID) a)
                               ,flow3 as (
                                   select 1 id, convert(float,(sum(CONVERT(int,Max_P01))-sum(CONVERT(int,Min_P01)))) as Lev3_P01
                                     from (select b.ID as PumpID,max(CONVERT(int,isnull(P01,0))) as Max_P01,min(CONVERT(int,isnull(P01,0))) as Min_P01 
                                             from  PumpManager b 
                                        left join T_Data2016 c on  c.FPumpID=b.ID " + strWhere
                                       + @" where b.FDeleted=0  and FLev=3 group by b.ID) a)
                           select Lev1_P01,Lev2_P01,Lev3_P01,(Lev1_P01-Lev2_P01) as Lev1_Miss,(Lev2_P01-Lev3_P01) as Lev2_Miss
                                 ,(Lev1_P01-Lev3_P01) as LevZ_Miss,((Lev1_P01-Lev2_P01)/Lev1_P01)*100.00 as Lev1_Miss_P
                              ,((Lev2_P01-Lev3_P01)/Lev2_P01)*100.00 as Lev2_Miss_P,((Lev1_P01-Lev3_P01)/Lev1_P01)*100.00 as LevZ_Miss_P
                              ,(Lev1_P01/1000)*1.2 as Mount,((Lev1_P01-Lev3_P01)/1000)*1.2 as Mount1
                               from flow1,flow2,flow3 where flow1.id= flow2.id and flow1.id	=flow3.id and flow2.id	=flow3.id ";

            ArrayList data = DBUtil.Select(sql);
            return data.Count > 0 ? (Hashtable)data[0] : null;
        }

        public static Hashtable SearchCurveData(string pumpId)
        {
            Hashtable has = new Hashtable();
            String sql = "select *,CONVERT(varchar(10),TM,120) as T_Data,CONVERT(varchar(10),TM,108) as T_Time  from T_Data2016 where datediff(day,AddTime,getdate())=0 and FPumpID='" + pumpId + "' order by T_Time";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }

        public static Hashtable SearchHistogramData(string pumpId,string time)
        {
            Hashtable has = new Hashtable();
            String sql = @"select CONVERT(varchar(10),isnull(TM,getdate()),120) as T_Data,T_dayHour as h_TM,(max(isnull(CONVERT(int,P01),0))-min(isnull(CONVERT(int,P01),0))) as data  from T_DayHours a
                        left join T_Data2016 on a.T_dayHour=datepart(hour,TM) and left(CONVERT(varchar(100),AddTime,120),10)='"+time+"' and FPumpID='" + pumpId + "'"
                        +@" where T_Lev=1
                         group by CONVERT(varchar(10),isnull(TM,getdate()),120),T_dayHour
                         order by h_TM";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }
        #region  一级二级网，用户（三级网）
        public static Hashtable SearchWatLeakRecMarchData_day1(int lev)
        {
            Hashtable has = new Hashtable();
            String sql = "select * from dbo.fun_watleakrecmarchdata_day1(" + lev + ") ";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }
        public static Hashtable SearchWatLeakRecMarchData_day2(int lev)
        {
            Hashtable has = new Hashtable();
            String sql = "select * from dbo.fun_watleakrecmarchdata_day2(" + lev + ") ";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }
        public static Hashtable SearchWatLeakRecMarchData_day3(int lev)
        {
            Hashtable has = new Hashtable();
            String sql = "select * from dbo.fun_watleakrecmarchdata_day3(" + lev + ") ";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }
        public static Hashtable SearchWatLeakRecMarchData_month(int num,int lev)
        {
            Hashtable has = new Hashtable();
            String sql = "select * from dbo.fun_watleakrecmarchdata_month(" + num + "," + lev + ") ";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }
        public static Hashtable SearchWatLeakRecMarchData_year(int num, int lev)
        {
            Hashtable has = new Hashtable();
            String sql = "select * from dbo.fun_watleakrecmarchdata_year(" + num + "," + lev + ") ";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }
        #endregion

        #region  总漏损
        public static Hashtable SearchWatLeakRecMarchData_rep4_day1()
        {
            Hashtable has = new Hashtable();
            String sql = "select * from dbo.fun_watleakrecmarchdata_rep4_day1() ";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }
        public static Hashtable SearchWatLeakRecMarchData_rep4_day2()
        {
            Hashtable has = new Hashtable();
            String sql = "select * from dbo.fun_watleakrecmarchdata_rep4_day2() ";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }
        public static Hashtable SearchWatLeakRecMarchData_rep4_day3()
        {
            Hashtable has = new Hashtable();
            String sql = "select * from dbo.fun_watleakrecmarchdata_rep4_day3() ";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }

        public static Hashtable SearchWatLeakRecMarchData_rep4_month(int num)
        {
            Hashtable has = new Hashtable();
            String sql = "select * from dbo.fun_watleakrecmarchdata_rep4_month(" + num + ") ";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }
        public static Hashtable SearchWatLeakRecMarchData_rep4_year(int num)
        {
            Hashtable has = new Hashtable();
            String sql = "select * from dbo.fun_watleakrecmarchdata_rep4_year(" + num + ") ";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }
        #endregion

        #region  用水成本
        public static Hashtable SearchWatLeakRecMarchData_rep5_day1()
        {
            Hashtable has = new Hashtable();
            String sql = "select * from dbo.fun_watleakrecmarchdata_rep5_day1() ";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }
        public static Hashtable SearchWatLeakRecMarchData_rep5_day2()
        {
            Hashtable has = new Hashtable();
            String sql = "select * from dbo.fun_watleakrecmarchdata_rep5_day2() ";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }
        public static Hashtable SearchWatLeakRecMarchData_rep5_day3()
        {
            Hashtable has = new Hashtable();
            String sql = "select * from dbo.fun_watleakrecmarchdata_rep5_day3() ";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }
        public static Hashtable SearchWatLeakRecMarchData_rep5_month(int num)
        {
            Hashtable has = new Hashtable();
            String sql = "select * from dbo.fun_watleakrecmarchdata_rep5_month(" + num + ") ";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }
        public static Hashtable SearchWatLeakRecMarchData_rep5_year(int num)
        {
            Hashtable has = new Hashtable();
            String sql = "select * from dbo.fun_watleakrecmarchdata_rep5_year(" + num + ") ";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }
        #endregion

        #region  损失成本
        public static Hashtable SearchWatLeakRecMarchData_rep6_day1()
        {
            Hashtable has = new Hashtable();
            String sql = "select * from dbo.fun_watleakrecmarchdata_rep6_day1() ";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }
        public static Hashtable SearchWatLeakRecMarchData_rep6_day2()
        {
            Hashtable has = new Hashtable();
            String sql = "select * from dbo.fun_watleakrecmarchdata_rep6_day2() ";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }
        public static Hashtable SearchWatLeakRecMarchData_rep6_day3()
        {
            Hashtable has = new Hashtable();
            String sql = "select * from dbo.fun_watleakrecmarchdata_rep6_day3() ";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }
        public static Hashtable SearchWatLeakRecMarchData_rep6_month(int num)
        {
            Hashtable has = new Hashtable();
            String sql = "select * from dbo.fun_watleakrecmarchdata_rep6_month(" + num + ") ";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }
        public static Hashtable SearchWatLeakRecMarchData_rep6_year(int num)
        {
            Hashtable has = new Hashtable();
            String sql = "select * from dbo.fun_watleakrecmarchdata_rep6_year(" + num + ") ";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }
        #endregion

        #region 周用水量

        /// <summary>
        /// 周用水量
        /// </summary>
        /// <param name="num">本周or上周（0或1）</param>
        /// <param name="pumpid">泵房id</param>
        /// <returns></returns>
        public static Hashtable SearchWatPumpWeekData(int num,int pumpid)
        {
            Hashtable has = new Hashtable();
            String sql = "select * from dbo.fun_watpumpweekdata(" + num + "," + pumpid + ")  order by id ";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }

        #endregion

        #region 月用水量

        /// <summary>
        /// 月用水量
        /// </summary>
        /// <param name="date">日期（用于提取年月）</param>
        /// <param name="pumpid">泵房id</param>
        /// <returns></returns>
        public static Hashtable SearchWatPumpMonthData(string date, int pumpid)
        {
            Hashtable has = new Hashtable();
            String sql = "select * from dbo.fun_watpumpmonthdata('" + date + "'," + pumpid + ")  order by id ";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }

        #endregion

        #region 年用水量
        /// <summary>
        /// 年用水量
        /// </summary>
        /// <param name="num">今年or去年（0或1）</param>
        /// <param name="pumpid">泵房id</param>
        /// <returns></returns>
        public static Hashtable SearchWatPumpYearData(int num, int pumpid)
        {
            Hashtable has = new Hashtable();
            String sql = "select * from dbo.fun_watpumpyeardata(" + num + "," + pumpid + ") ";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }
        #endregion

        #region 一级二级网月成本月损失成本
        public static Hashtable GetMonthCostData(int lev)
        {
            Hashtable has = new Hashtable();
            String sql = @"select a.id, CONVERT(varchar(10), right(a.FYear,2))+'-'+CONVERT(varchar(10),a.FMonth) as yearMonth,a.FFlow*FPrice as FFlowPrice, a.FLeakage*FPrice as FLeakagePrice,a.FTotalLeakage*FPrice as FTotalLeakPrice from(
                            select top 12 * from T_FlowLeakCost where FLev=" +lev+" order by id desc) a order by id ";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }
        #endregion

        #region 一级二级网月成本季度成本

        /// <summary>
        /// 月成本
        /// </summary>
        /// <param name="lev"></param>
        /// <returns></returns>
        public static Hashtable GetMonthCostDataBT(int lev)
        {
            Hashtable has = new Hashtable();
            String sql = @"select id,FMonth,FFlow*FPrice as FFlowPrice from T_FlowLeakCost where FLev="+lev+" and FYear=year(GETDATE()) order by id ";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }

        /// <summary>
        /// 季度成本
        /// </summary>
        /// <param name="lev"></param>
        /// <returns></returns>
        public static Hashtable GetQuarterCostDataBT(int lev)
        {
            Hashtable has = new Hashtable();
            String sql = @"select '一季度' as 'quarterS', isnull(sum(FFlow*FPrice),0) as FFlowPrice from T_FlowLeakCost where FLev=" + lev + " and FYear=year(GETDATE()) and DATENAME(quarter,FMonth)=1"
                         +@"union all
                         select '二季度' as 'quarterS', isnull(sum(FFlow*FPrice),0) as FFlowPrice from T_FlowLeakCost where FLev=" + lev + " and FYear=year(GETDATE()) and DATENAME(quarter,FMonth)=2"
                         +@"union all
                         select '三季度' as 'quarterS', isnull(sum(FFlow*FPrice),0) as FFlowPrice from T_FlowLeakCost where FLev=" + lev + " and FYear=year(GETDATE()) and DATENAME(quarter,FMonth)=3"
                         + @"union all
                         select '四季度' as 'quarterS', isnull(sum(FFlow*FPrice),0) as FFlowPrice from T_FlowLeakCost where FLev=" + lev + " and FYear=year(GETDATE()) and DATENAME(quarter,FMonth)=4 ";
            DataTable data = DBUtil.SelectDataTable(sql);
            has["data"] = data;
            return data.Rows.Count > 0 ? has : null;
        }
        #endregion

    }
}