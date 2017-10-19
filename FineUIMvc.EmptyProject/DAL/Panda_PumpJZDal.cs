using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using FineUIMvc;

namespace FineUIMvc.PumpMVC.DAL
{
    public class Panda_PumpJZDal
    {
        private const string str_PumpJZList = @" select a.ID,DTUCode as b_number,PumpJZName,c.PName,d.Name as CustomerName,b.FName as MachineTypeName,RunPumpNum,Auxiliarypumpcount,
                                                        DrainPumpNum,PumpJZArea as PumpJZAreaName,PumpJZAddressList,c.FCustomerID,PumpJZContactGroup 
                                                   from Panda_PumpJZ a
                                              left join sys_dictItems b on a.MachineType=b.FValue and b.FDictID=132
                                              left join Panda_Pump c on a.PumpId=c.ID
                                              left join Panda_Customer d on c.FCustomerID=d.ID
                                              where a.FIsDelete=0 ";

        private const string str_PumpJZUpd = @"UPDATE Panda_PumpJZ SET {0} WHERE {1};UPDATE Dtu_Base SET B_IsUsed=1 WHERE B_Number='{2}'";
        private const string str_PumpJZUpd2 = @"UPDATE Panda_PumpJZ SET {0} WHERE {1}";
        private const string str_PumpJZDel = @"UPDATE Panda_PumpJZ SET {0} WHERE {1};UPDATE Dtu_Base SET B_IsUsed=0 WHERE B_Number in (select DTUCode from Panda_PumpJZ where ID in({2}))";

        private const string str_PumpJZAdd = @"INSERT INTO Panda_PumpJZ ( {0} ) VALUES( {1} );SELECT SCOPE_IDENTITY() AS id;UPDATE Dtu_Base SET B_IsUsed=1 WHERE B_Number='{2}'";


        private const string str_PumpJZ_OnLineList = @"select c.ID as pumpID,c.PName,a.PumpJZName,a.PumpJZArea,c.PLngLat,b.*,
                                                       isnull((select top 1 1 from Alarm_Timely _c 
                                                             where _c.BaseID=a.id and FMarkerType=1 and FStatus=1),0) as FIsAlarm
                                                       from Panda_PumpJZ a 
                                                       inner join E_DATA_MAIN b on a.ID=b.BaseID
                                                       inner join Panda_Pump c on a.PumpId=c.ID
                                                       where a.FIsDelete=0 and c.FIsDelete=0 ";

        private const string str_PumpJZ_OnLineList_count = @"select count(*) as countS
                                                               from Panda_PumpJZ a 
                                                         inner join E_DATA_MAIN b on a.ID=b.BaseID
                                                         inner join Panda_Pump c on a.PumpId=c.ID
                                                              where a.FIsDelete=0 and c.FIsDelete=0 ";

        private const string str_PumpJZ_ReportList = @"select a.DTUCode,a.PumpJZName,b.FUpdateDate,isnull(F41005,0) +'/'+isnull(F41006,0) AS InOutWaPa,
                                                              b.FOnLine,isnull((select top 1 1 from Alarm_Timely _c 
                                                             where _c.BaseID=a.id and FMarkerType=1 and FStatus=1),0) as FIsAlarm
                                                       from Panda_PumpJZ a 
                                                       inner join E_DATA_MAIN b on a.ID=b.BaseID
                                                       inner join Panda_Pump c on a.PumpId=c.ID
                                                       where a.FIsDelete=0 and c.FIsDelete=0 ";

        private const string str_PumpJZ_ReportList_count = @"select count(*) from Panda_PumpJZ a 
                                                              inner join E_DATA_MAIN b on a.ID=b.BaseID
                                                              inner join Panda_Pump c on a.PumpId=c.ID
                                                              where a.FIsDelete=0 and c.FIsDelete=0 ";

        public static DataTable Exist(string strWhere)
        {
            string where = strWhere;

            string sql = @"select top 1 a.* from Panda_PumpJZ a, Panda_Pump b where a.PumpId=b.ID and a.FIsDelete=0 and b.FIsDelete=0 ";

            if (!where.Equals(""))
            {
                sql = sql + where;
            }
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }
        public static Hashtable SearchAlarmHistory(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            String sql = @"select c.PName,b.PumpJZName,FKey,FSetMsg,FAlarmTime,FEndAlarmTime,DATEDIFF(MI,FAlarmTime,FEndAlarmTime) as CXTime,d.FName as TypeName
                             from Alarm_Timely_History a
                             left join Panda_PumpJZ b on a.BaseID=b.ID and b.FIsDelete=0
                             left join Panda_Pump c on b.PumpId=c.ID and c.FIsDelete=0
                             left join sys_dictItems d on a.FType=d.FValue and d.FDictID=164
                             where a.FMarkerType=1  ";
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, sql);
        }
        public static Hashtable SearchAlarm(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            String sql = @"select a.id as AlarmID,FKey,FSetMsg,a.BaseID,c.PName,b.PumpJZName ,TempTime,
                                    TimeRange=case when DATEDIFF(MINUTE, FAlarmTime,getdate())<5 then '刚刚'
                                         when DATEDIFF(MINUTE, FAlarmTime,getdate()) between 5 and 60 then convert(varchar(20), DATEDIFF(MINUTE, FAlarmTime,getdate()))+'分钟前'
                                         when DATEDIFF(HOUR, FAlarmTime,getdate()) between 1 and 24 then convert(varchar(20), DATEDIFF(HOUR, FAlarmTime,getdate()))+'个小时前'
                                         when DATEDIFF(DAY, FAlarmTime,getdate()) between 1 and 30 then convert(varchar(20), DATEDIFF(DAY, FAlarmTime,getdate()))+'天前'
                                         when DATEDIFF(MONTH, FAlarmTime,getdate()) between 1 and 12 then convert(varchar(20), DATEDIFF(MONTH, FAlarmTime,getdate()))+'个月前'
                                         else 'N年前' end
                              from Alarm_Timely a,Panda_PumpJZ b,Panda_Pump c,E_DATA_MAIN d
                            where a.BaseID=b.id and b.id=d.BaseID and a.BaseID=d.BaseID and b.PumpId=c.ID and a.FMarkerType=1 and FStatus=1 and b.FIsDelete=0 and c.FIsDelete=0 ";
            //sql = sql + strWhere + " order by FAlarmTime desc ";
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, sql);
        }
        public static DataTable SearchAlarm(string strWhere)
        {
            String sql = @"select a.id as AlarmID,FKey,FSetMsg,a.BaseID,c.PName,b.PumpJZName ,TempTime,
                                    TimeRange=case when DATEDIFF(MINUTE, FAlarmTime,getdate())<5 then '刚刚'
                                         when DATEDIFF(MINUTE, FAlarmTime,getdate()) between 5 and 60 then convert(varchar(20), DATEDIFF(MINUTE, FAlarmTime,getdate()))+'分钟前'
                                         when DATEDIFF(HOUR, FAlarmTime,getdate()) between 1 and 24 then convert(varchar(20), DATEDIFF(HOUR, FAlarmTime,getdate()))+'个小时前'
                                         when DATEDIFF(DAY, FAlarmTime,getdate()) between 1 and 30 then convert(varchar(20), DATEDIFF(DAY, FAlarmTime,getdate()))+'天前'
                                         when DATEDIFF(MONTH, FAlarmTime,getdate()) between 1 and 12 then convert(varchar(20), DATEDIFF(MONTH, FAlarmTime,getdate()))+'个月前'
                                         else 'N年前' end
                              from Alarm_Timely a,Panda_PumpJZ b,Panda_Pump c,E_DATA_MAIN d
                            where a.BaseID=b.id and b.id=d.BaseID and a.BaseID=d.BaseID and b.PumpId=c.ID and a.FMarkerType=1 and FStatus=1 and b.FIsDelete=0 and c.FIsDelete=0 ";
            sql = sql + strWhere + " order by FAlarmTime desc ";
            return publicDal.TableSearch(sql);
        }

        public static Hashtable Search(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_PumpJZList);
        }

        public static DataTable SearchZLB_Count(string strWhere)
        {
            string where = strWhere;

            string sql = @"with BJ as(
                                      select 1 as id, COUNT(*) as bj_cont
                                        from Panda_PumpJZ a 
                                  inner join E_DATA_MAIN b on a.ID=b.BaseID
                                  inner join Panda_Pump c on a.PumpId=c.ID
                                       where a.FIsDelete=0 and c.FIsDelete=0
                                         and isnull((select top 1 1 from Alarm_Timely _c 
                                                      where _c.BaseID=a.id and FMarkerType=1 and FStatus=1),0)=1 " + strWhere+ ")"
                            + @",ZX as(
                                      select 1 as id,COUNT(*) as zx_cont
                                        from Panda_PumpJZ a 
                                  inner join E_DATA_MAIN b on a.ID=b.BaseID
                                  inner join Panda_Pump c on a.PumpId=c.ID
                                       where a.FIsDelete=0 and c.FIsDelete=0
                                         and b.FOnLine=1" + strWhere+ ")"
                            + @",LX as(
                                      select 1 as id,COUNT(*) as lx_cont
                                        from Panda_PumpJZ a 
                                  inner join E_DATA_MAIN b on a.ID=b.BaseID
                                  inner join Panda_Pump c on a.PumpId=c.ID
                                       where a.FIsDelete=0 and c.FIsDelete=0
                                         and b.FOnLine=0" + strWhere+ ")"
                            + @",AL as(
                                      select 1 as id,COUNT(*) as all_cont
                                        from Panda_PumpJZ a 
                                  inner join E_DATA_MAIN b on a.ID=b.BaseID
                                  inner join Panda_Pump c on a.PumpId=c.ID
                                       where a.FIsDelete=0 and c.FIsDelete=0 " + strWhere + ")"
                           + @"select bj_cont,zx_cont,lx_cont,all_cont
                                 from BJ
                            left join ZX on BJ.id=ZX.id
                            left join LX on BJ.id=LX.id
                            left join AL on BJ.id=AL.id";

            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }

        public static Hashtable Search_OnLineList(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_PumpJZ_OnLineList, str_PumpJZ_OnLineList_count);
        }

        public static Hashtable Search_ReportList(int index, int size, string sortField, string sortOrder, string select, string strWhere)
        {
            string sql = select + @" from E_DATA_MAIN b 
                             inner join Panda_PumpJZ a on a.ID=b.BaseID
                             inner join Panda_Pump c on a.PumpId=c.ID
                                  where a.FIsDelete=0 and c.FIsDelete=0 ";
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, sql, str_PumpJZ_ReportList_count);
        }
        public static Hashtable Search_YearReportList(int index, int size, string sortField, string sortOrder,string select, string strWhere,int year)
        {
//            string sql = select+@" from Panda_PumpJZ a 
//                             inner join Panda_Pump c on a.PumpId=c.ID
//                             inner join E_DATA" + year + " b on a.ID=b.BaseID where a.FIsDelete=0 and c.FIsDelete=0 ";
            string sql = select + @" from E_DATA" + year + " b "
                           + @"  inner join Panda_PumpJZ a on a.ID=b.BaseID
                                 inner join Panda_Pump c on a.PumpId=c.ID 
                                      where a.FIsDelete=0 and c.FIsDelete=0 ";
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, sql);
        }
        public static DataTable Search_YearReportList(string sortField, string sortOrder, string select, string strWhere, int year)
        {
            //            string sql = select+@" from Panda_PumpJZ a 
            //                             inner join Panda_Pump c on a.PumpId=c.ID
            //                             inner join E_DATA" + year + " b on a.ID=b.BaseID where a.FIsDelete=0 and c.FIsDelete=0 ";
            string sql = select + @" from E_DATA" + year + " b "
                           + @"  inner join Panda_PumpJZ a on a.ID=b.BaseID
                                 inner join Panda_Pump c on a.PumpId=c.ID 
                                      where a.FIsDelete=0 and c.FIsDelete=0 ";
            return publicDal.TableSearch(strWhere, sql);
        }

        public static void Insert(Hashtable has, string id)
        {
           publicDal.InsertUpd(has, str_PumpJZAdd,id);
        }

        public static void Update(Hashtable has, string id)
        {
            publicDal.UpdateUpd(has, str_PumpJZUpd, "ID", id);
        }
        public static void Update(Hashtable has)
        {
            publicDal.Update(has, str_PumpJZUpd2, "ID");
        }
        public static void DeleteList(Hashtable has)
        {
            string sql = "UPDATE Panda_PumpJZ SET {0} WHERE {1};UPDATE Dtu_Base SET B_IsUsed=0 WHERE B_Number in (select DTUCode from Panda_PumpJZ where ID in(" + has["ID"] + "))";
            publicDal.DeleteList(has, sql, "ID");
        }
    }
}