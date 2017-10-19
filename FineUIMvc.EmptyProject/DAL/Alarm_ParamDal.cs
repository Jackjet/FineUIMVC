using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using FineUIMvc;

namespace FineUIMvc.PumpMVC.DAL
{
    public class Alarm_ParamDal
    {
        public static Hashtable Search(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, "select * from Alarm_Param where 1=1 ");
        }

        public static void Insert(Hashtable has)
        {
            publicDal.Insert(has, "INSERT INTO Alarm_Param ( {0} ) VALUES( {1} )");
        }
        public static void Update(Hashtable has)
        {
            publicDal.Update(has, "UPDATE Alarm_Param SET {0} WHERE {1}", "id");
        }
        public static void DeleteList(Hashtable has)
        {
            publicDal.DeleteList(has, "UPDATE Alarm_Param SET {0} WHERE {1}", "id");
        }

        public static void InsertTimely(Hashtable has)
        {
            publicDal.Insert(has, "INSERT INTO Alarm_Timely ( {0} ) VALUES( {1} )");
        }
        public static void UpdateTimely(Hashtable has)
        {
            publicDal.Update(has, "UPDATE Alarm_Timely SET {0} WHERE {1}", "id");
        }

        public static DataTable SearchInsertAlarm(string strWhere, string BaseID)
        {
            string where = strWhere;

            string sql = @"insert into Alarm_Timely select id as ParamID,BaseID='" + BaseID + "',FMarkerType,FKey,FMsg,FMsg as FSetMsg,FLev,FStatus=0,FIsPhone=0,FAlarmTime=null  "
                                                 + @" from Alarm_Param where FIsDef=1 " + strWhere;

            if (!where.Equals(""))
            {
                sql = sql + where;
            }
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }

        public static DataTable SearchAlarmParm(string strWhere, int FMarkerType, string BaseID)
        {
            string where = strWhere;

            string sql = @"select a.id as ParamID,b.id as TimelyID,a.FMarkerType,a.FKey,a.FMsg as A_FMsg,b.FMsg,b.FSetMsg,
                                  FLev=case when b.FLev is null then a.FLev else b.FLev end,
                                  selectItem=case when b.id is null then 'false' else 'true' end 
                             from Alarm_Param a
                        left join Alarm_Timely b on a.id=b.ParamID and b.FMarkerType=" + FMarkerType + " and b.BaseID='" + BaseID + "' where a.FMarkerType=" + FMarkerType + " ";

            if (!where.Equals(""))
            {
                sql = sql + where;
            }
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }

        /// <summary>
        /// 根据选中项找出Alarm_Timely没有的ParamID为插入操作做准备
        /// </summary>
        /// <param name="selectid"></param>
        /// <param name="FMarkerType"></param>
        /// <param name="BaseID"></param>
        /// <returns></returns>
        public static DataTable SearchNotParmInsert(string selectid, int FMarkerType, string BaseID)
        {
            string sql = @"select id,FMarkerType,FKey,FMsg,FLev from Alarm_Param where id in (" + selectid + ")"
                           +" and id not in(select ParamID from  Alarm_Timely where  FMarkerType=" + FMarkerType + " and BaseID='" + BaseID + "' )";

            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }

        /// <summary>
        /// 根据选中项找出Alarm_Timely有的ParamID为修改操作做准备
        /// </summary>
        /// <param name="selectid"></param>
        /// <param name="FMarkerType"></param>
        /// <param name="BaseID"></param>
        /// <returns></returns>
        public static DataTable SearchHaveParmUpdate(string selectid, int FMarkerType, string BaseID)
        {
            string sql = @"select a.*,b.id as TimelyID from Alarm_Param a,Alarm_Timely b where a.id=b.ParamID and b.FMarkerType=" + FMarkerType + " and b.BaseID='" + BaseID + "'	and a.id in (" + selectid + ")";

            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }

        public static DataTable SearchPump_JZID(string BaseID)
        {
            string sql = @"select ID from Panda_PumpJZ where PumpId=(select PumpId from Panda_PumpJZ where ID='" + BaseID + "') and ID<>'" + BaseID + "'";
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }

        public static DataTable TongBu(string BaseID1, string BaseID2, int FMarkerType)
        {
            string sql = @"delete from Alarm_Timely where BaseID='"+BaseID2+"' and FMarkerType="+FMarkerType+";"
                         +@"insert into Alarm_Timely 
                             select ParamID,BaseID='"+BaseID2+"',FMarkerType,FKey,FMsg,FSetMsg,FLev,FStatus,FIsPhone,FAlarmTime "
                              + @" from Alarm_Timely where BaseID='" + BaseID1 + "' and FMarkerType=" + FMarkerType + "";
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }

        /// <summary>
        /// 根据选中项找出Alarm_Timely已有的作为多余项执行删除操作
        /// </summary>
        /// <param name="has"></param>
        /// <param name="FMarkerType"></param>
        /// <param name="BaseID"></param>
        public static void DeleteTimelyList(Hashtable has, int FMarkerType, string BaseID)
        {
            publicDal.DeleteList(has, "delete from  Alarm_Timely where  FMarkerType=" + FMarkerType + " and BaseID='" + BaseID + "' and ParamID not in ({0})");
        }
    }
}