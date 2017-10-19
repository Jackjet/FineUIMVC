using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;

namespace Dal
{
    public class Data_YaLiDal
    {
        public static Hashtable SearchYALI(string strWhere, int index, int size, string sortField, string sortOrder)
        {
            String sql = @"select BASEID,FCustomerID,a.FDTUCode,FName,FMpaUp,FMpaDown,FCreateDate
                                   ,FOnLine,FMpa,FBatt,FUpdateDate,TempTime,Repeat,
                                   isnull((select top 1 1 from Alarm_Timely _c where _c.BaseID=a.id and FMarkerType=7 and FStatus=1),0) as FIsAlarm
                             from BASE_YALI a,DATA_YALI_MAIN b where a.id=b.baseid ";
            sql = sql + strWhere;

            if (String.IsNullOrEmpty(sortField) == false)
            {
                if (sortOrder != "desc") sortOrder = "asc";
                sql += " order by " + sortField + " " + sortOrder;
            }

            DataTable dt = DBUtil.SelectDataTablePager(sql, index, size);

            int count = Convert.ToInt32(DBUtil.SelectDataTable("select count(*) as countS from BASE_YALI a,DATA_YALI_MAIN b where a.id=b.baseid ").Rows[0][0].ToString());

            Hashtable result = new Hashtable();
            result["data"] = dt;
            result["total"] = count;

            return result;
        }
        public static Hashtable SearchYALI_Year(string strWhere, int index, int size, string sortField, string sortOrder, int year)
        {
            String sql = @"select BASEID,FCustomerID,a.FDTUCode,FName,FMpaUp,FMpaDown,a.FCreateDate
                                   ,FMpa,FBatt,TempTime,Repeat
                             from BASE_YALI a,DATA_YALI_" + year + " b where a.id=b.baseid ";
            sql = sql + strWhere;

            if (String.IsNullOrEmpty(sortField) == false)
            {
                if (sortOrder != "desc") sortOrder = "asc";
                sql += " order by " + sortField + " " + sortOrder;
            }

            DataTable dt = DBUtil.SelectDataTablePager(sql, index, size);

            int count = DBUtil.ExecuteScalar(sql);

            Hashtable result = new Hashtable();
            result["data"] = dt;
            result["total"] = count;

            return result;
        }
        public static string InsertYALI(Hashtable has)
        {
            string id = (has["id"] == null || has["id"].ToString() == "") ? Guid.NewGuid().ToString() : has["id"].ToString();
            has["id"] = id;
            string columns = "";
            string values = "";
            foreach (DictionaryEntry de in has)
            {
                columns += "" + de.Key + ",";
                values += "@" + de.Key + ",";
            }
            string sql = string.Format("insert into BASE_YALI ( {0} ) values( {1} )", columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1));

            DBUtil.Execute(sql, has);
            return id;
        }
        public static string InsertYALI_MAIN(Hashtable has)
        {
            string id = (has["id"] == null || has["id"].ToString() == "") ? Guid.NewGuid().ToString() : has["id"].ToString();
            has["id"] = id;
            string columns = "";
            string values = "";
            foreach (DictionaryEntry de in has)
            {
                columns += "" + de.Key + ",";
                values += "@" + de.Key + ",";
            }
            string sql = string.Format("insert into DATA_YALI_MAIN ( {0} ) values( {1} )", columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1));

            DBUtil.Execute(sql, has);
            return id;
        }
        public static void UpdateYALI(Hashtable has)
        {
            string set = "";
            string where = "";
            foreach (DictionaryEntry de in has)
            {
                if (de.Key.ToString() != "id")
                {
                    set += "" + de.Key + "= @" + de.Key + ",";
                }
                else
                {
                    where += "" + de.Key + "= @" + de.Key + "";
                }
            }
            string sql = string.Format("update BASE_YALI  set {0}  where {1}", set.Substring(0, set.Length - 1), where);

            DBUtil.Execute(sql, has);
        }
        public static void UpdateYALI_MAIN(Hashtable has)
        {
            string set = "";
            string where = "";
            foreach (DictionaryEntry de in has)
            {
                if (de.Key.ToString() != "BASEID")
                {
                    set += "" + de.Key + "= @" + de.Key + ",";
                }
                else
                {
                    where += "" + de.Key + "= @" + de.Key + "";
                }
            }
            string sql = string.Format("update DATA_YALI_MAIN  set {0}  where {1}", set.Substring(0, set.Length - 1), where);

            DBUtil.Execute(sql, has);
        }
        public static DataTable SearchInsertAlarm(string strWhere, string BaseID)
        {
            string where = strWhere;

            string sql = @"insert into Alarm_Timely select id as ParamID,BaseID='" + BaseID + "',FMarkerType,FKey,FMsg,FMsg as FSetMsg,FLev,FStatus=0,FIsPhone=0,FAlarmTime=null  "
                                                 + @" from Alarm_Param where 1=1 " + strWhere;

            if (!where.Equals(""))
            {
                sql = sql + where;
            }
            DataTable dt = DBUtil.SelectDataTable(sql);

            return dt;
        }
    }
}