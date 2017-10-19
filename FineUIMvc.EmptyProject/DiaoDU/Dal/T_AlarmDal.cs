using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace Dal
{
    public class T_AlarmDal
    {
//        static String tbname = "T_Alarm";

//        public static Hashtable Search(string key, int index, int size, string sortField, string sortOrder)
//        {
//            string sql = @"SELECT     dbo.PumpManager.FName, dbo.T_Alarm.*
//                            FROM      dbo.T_Alarm INNER JOIN
//                            dbo.PumpManager ON dbo.T_Alarm.FPumpID = dbo.PumpManager.ID
//                            where  T_Alarm.FStatus='1' " + key + " order by FCreateDate desc";


//            if (String.IsNullOrEmpty(sortField) == false)
//            {
//                if (sortOrder != "desc") sortOrder = "asc";
//                sql += " order by " + sortField + " " + sortOrder;
//            }

//            DataTable dt = DBUtil.SelectDataTablePager(sql, index, size);
//            ArrayList data = DBUtil.DataTable2ArrayList(dt);

//            int count = DBUtil.ExecuteScalar(sql);

//            Hashtable result = new Hashtable();
//            result["data"] = data;
//            result["total"] = count;

//            return result;
//        }
//        public static Hashtable Get(string id)
//        {
//            string sql = "select * from " + tbname + " where id = '" + id + "'";
//            ArrayList data = DBUtil.Select(sql);
//            return data.Count > 0 ? (Hashtable)data[0] : null;
//        }
//        public static string Insert(Hashtable has)
//        {
//            string id = (has["id"] == null || has["id"].ToString() == "") ? Guid.NewGuid().ToString() : has["id"].ToString();
//            has["id"] = id;

//            string columns = "";
//            string values = "";
//            foreach (DictionaryEntry de in has)
//            {
//                columns += "" + de.Key + ",";
//                values += "@" + de.Key + ",";
//            }
//            string sql = string.Format("insert into " + tbname + " ( {0} ) values( {1} )", columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1));

//            DBUtil.Execute(sql, has);
//            return id;
//        }
//        public static void Delete(string id)
//        {
//            Hashtable has = new Hashtable();
//            has["id"] = id;
//            DBUtil.Execute("delete from " + tbname + " where id = @id", has);
//        }
//        public static void Update(Hashtable has)
//        {
//            string set = "";
//            string where = "";
//            foreach (DictionaryEntry de in has)
//            {
//                if (de.Key.ToString() != "id")
//                {
//                    set += "" + de.Key + "= @" + de.Key + ",";
//                }
//                else
//                {
//                    where += "" + de.Key + "= @" + de.Key + "";
//                }
//            }
//            string sql = string.Format("update " + tbname + "  set {0}  where {1}", set.Substring(0, set.Length - 1), where);

//            DBUtil.Execute(sql, has);
//        }

        public static DataTable SearchAlarm(string strWhere,int id)
        {
            String sql = @"select a.id as AlarmID,FSetMsg,a.BaseID,b.FName ,TempTime,
                                    TimeRange=case when DATEDIFF(MINUTE, FAlarmTime,getdate())<5 then '刚刚'
                                         when DATEDIFF(MINUTE, FAlarmTime,getdate()) between 5 and 60 then convert(varchar(20), DATEDIFF(MINUTE, FAlarmTime,getdate()))+'分钟前'
                                         when DATEDIFF(HOUR, FAlarmTime,getdate()) between 1 and 24 then convert(varchar(20), DATEDIFF(HOUR, FAlarmTime,getdate()))+'个小时前'
                                         when DATEDIFF(DAY, FAlarmTime,getdate()) between 1 and 30 then convert(varchar(20), DATEDIFF(DAY, FAlarmTime,getdate()))+'天前'
                                         when DATEDIFF(MONTH, FAlarmTime,getdate()) between 1 and 12 then convert(varchar(20), DATEDIFF(MONTH, FAlarmTime,getdate()))+'个月前'
                                         else 'N年前' end
                             from Alarm_Timely a,BASE_YALI b,DATA_YALI_MAIN c
                            where a.BaseID=b.id and b.id=c.BASEID and a.BaseID=c.BASEID and a.FMarkerType=" + id + " and FStatus=1 ";
            sql = sql + strWhere + " order by FAlarmTime desc ";
            DataTable data = DBUtil.SelectDataTable(sql);
            return data;
        }
    }
}