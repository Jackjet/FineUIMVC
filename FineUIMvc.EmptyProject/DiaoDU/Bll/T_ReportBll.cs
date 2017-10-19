using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace Bll
{
    public class T_ReportBll
    {
        public static Hashtable SearchTopLableDate(string strWhere)
        {
            return Dal.T_ReportDal.SearchTopLableDate(strWhere);
        }

        public static Hashtable SearchCurveData(string pumpId)
        {
            return Dal.T_ReportDal.SearchCurveData(pumpId);
        }

        public static Hashtable SearchHistogramData(string pumpId,string time)
        {
            return Dal.T_ReportDal.SearchHistogramData(pumpId, time);
        }
        #region  一级二级网，用户（三级网）
        public static Hashtable SearchWatLeakRecMarchData_day1(int lev)
        {
            return Dal.T_ReportDal.SearchWatLeakRecMarchData_day1(lev);
        }

        public static Hashtable SearchWatLeakRecMarchData_day2(int lev)
        {
            return Dal.T_ReportDal.SearchWatLeakRecMarchData_day2(lev);
        }

        public static Hashtable SearchWatLeakRecMarchData_day3(int lev)
        {
            return Dal.T_ReportDal.SearchWatLeakRecMarchData_day3(lev);
        }

        public static Hashtable SearchWatLeakRecMarchData_month(int num, int lev)
        {
            return Dal.T_ReportDal.SearchWatLeakRecMarchData_month(num,lev);
        }
        public static Hashtable SearchWatLeakRecMarchData_year(int num, int lev)
        {
            return Dal.T_ReportDal.SearchWatLeakRecMarchData_year(num,lev);
        }

        #endregion

        #region  总漏损
        public static Hashtable SearchWatLeakRecMarchData_rep4_day1()
        {
            return Dal.T_ReportDal.SearchWatLeakRecMarchData_rep4_day1();
        }

        public static Hashtable SearchWatLeakRecMarchData_rep4_day2()
        {
            return Dal.T_ReportDal.SearchWatLeakRecMarchData_rep4_day2();
        }

        public static Hashtable SearchWatLeakRecMarchData_rep4_day3()
        {
            return Dal.T_ReportDal.SearchWatLeakRecMarchData_rep4_day3();
        }

        public static Hashtable SearchWatLeakRecMarchData_rep4_month(int num)
        {
            return Dal.T_ReportDal.SearchWatLeakRecMarchData_rep4_month(num);
        }
        public static Hashtable SearchWatLeakRecMarchData_rep4_year(int num)
        {
            return Dal.T_ReportDal.SearchWatLeakRecMarchData_rep4_year(num);
        }
        #endregion

        #region  用水成本
        public static Hashtable SearchWatLeakRecMarchData_rep5_day1()
        {
            return Dal.T_ReportDal.SearchWatLeakRecMarchData_rep5_day1();
        }

        public static Hashtable SearchWatLeakRecMarchData_rep5_day2()
        {
            return Dal.T_ReportDal.SearchWatLeakRecMarchData_rep5_day2();
        }

        public static Hashtable SearchWatLeakRecMarchData_rep5_day3()
        {
            return Dal.T_ReportDal.SearchWatLeakRecMarchData_rep5_day3();
        }
        public static Hashtable SearchWatLeakRecMarchData_rep5_month(int num)
        {
            return Dal.T_ReportDal.SearchWatLeakRecMarchData_rep5_month(num);
        }
        public static Hashtable SearchWatLeakRecMarchData_rep5_year(int num)
        {
            return Dal.T_ReportDal.SearchWatLeakRecMarchData_rep5_year(num);
        }
        #endregion

        #region  损失成本
        public static Hashtable SearchWatLeakRecMarchData_rep6_day1()
        {
            return Dal.T_ReportDal.SearchWatLeakRecMarchData_rep6_day1();
        }

        public static Hashtable SearchWatLeakRecMarchData_rep6_day2()
        {
            return Dal.T_ReportDal.SearchWatLeakRecMarchData_rep6_day2();
        }

        public static Hashtable SearchWatLeakRecMarchData_rep6_day3()
        {
            return Dal.T_ReportDal.SearchWatLeakRecMarchData_rep6_day3();
        }
        public static Hashtable SearchWatLeakRecMarchData_rep6_month(int num)
        {
            return Dal.T_ReportDal.SearchWatLeakRecMarchData_rep6_month(num);
        }
        public static Hashtable SearchWatLeakRecMarchData_rep6_year(int num)
        {
            return Dal.T_ReportDal.SearchWatLeakRecMarchData_rep6_year(num);
        }
        #endregion

        #region 周用水量

        /// <summary>
        /// 周用水量
        /// </summary>
        /// <param name="num">本周or上周（0或1）</param>
        /// <param name="pumpid">泵房id</param>
        /// <returns></returns>
        public static Hashtable SearchWatPumpWeekData(int num, int pumpId)
        {
            return Dal.T_ReportDal.SearchWatPumpWeekData(num, pumpId);
        }

        #endregion

        #region 月用水量

        /// <summary>
        /// 月用水量
        /// </summary>
        /// <param name="date">日期（用于提取年月）</param>
        /// <param name="pumpid">泵房id</param>
        /// <returns></returns>
        public static Hashtable SearchWatPumpMonthData(string date, int pumpId)
        {
            return Dal.T_ReportDal.SearchWatPumpMonthData(date, pumpId);
        }

        #region 周用水量

        /// <summary>
        /// 年用水量
        /// </summary>
        /// <param name="num">今年or去年（0或1）</param>
        /// <param name="pumpid">泵房id</param>
        /// <returns></returns>
        public static Hashtable SearchWatPumpYearData(int num, int pumpId)
        {
            return Dal.T_ReportDal.SearchWatPumpYearData(num, pumpId);
        }

        #endregion

        #endregion

        #region 一级二级网月成本月损失成本
        public static Hashtable GetMonthCostData(int lev)
        {
            return Dal.T_ReportDal.GetMonthCostData(lev);
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
            return Dal.T_ReportDal.GetMonthCostDataBT(lev);
        }

        /// <summary>
        /// 季度成本
        /// </summary>
        /// <param name="lev"></param>
        /// <returns></returns>
        public static Hashtable GetQuarterCostDataBT(int lev)
        {
            return Dal.T_ReportDal.GetQuarterCostDataBT(lev);
        }
        #endregion

    }
}