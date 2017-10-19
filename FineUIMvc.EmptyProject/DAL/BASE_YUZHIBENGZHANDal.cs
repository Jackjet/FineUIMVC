using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using FineUIMvc;

namespace FineUIMvc.PumpMVC.DAL
{
    public class BASE_YUZHIBENGZHANDal
    {
        #region const
        private const string str_LLList = @"select a.id,b.Name as CustomerName,a.FDTUCode,FMapAddress,a.FName,FCaliber,FEQuiType,FCommunicationMode,FEQuiState
                                                    ,RunPumpNum,Auxiliarypumpcount,DrainPumpNum,FTankVolume
                                                 from BASE_YUZHIBENGZHAN a
                                            left join Panda_Customer b on a.FCustomerID=b.ID
                                                where a.FIsDelete=0 ";

        private const string str_LLAdd = @"INSERT INTO BASE_YUZHIBENGZHAN ( {0} ) VALUES( {1} )";

        private const string str_LLUpd = @"UPDATE BASE_YUZHIBENGZHAN SET {0} WHERE {1}";

        private const string str_LLDel = @"DELETE FROM BASE_YUZHIBENGZHAN WHERE id = @id";
        #endregion

        public static DataTable Exist(string strWhere)
        {
            string where = strWhere;

            string sql = @"select top 1 * from BASE_YUZHIBENGZHAN where FIsDelete=0 ";

            if (!where.Equals(""))
            {
                sql = sql + where;
            }
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }


        public static DataTable SearchLL(string strWhere)
        {
            string where = strWhere;

            string sql = str_LLList;

            if (!where.Equals(""))
            {
                sql = sql + where;
            }
            sql = sql + " ORDER BY a.FCreateDate desc ";
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }

        public static Hashtable SearchLL(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_LLList);
        }
//        public static Hashtable SearchLL_HisReport(int index, int size, string sortField, string sortOrder, string strWhere, int year)
//        {
//            string str_YL_HisReport = @"  select a.id as BaseID,a.FDTUCode,FMapAddress,FName,b.*
//                                            from DATA_LIULIANG_" + year + " b "
//                                  + @"    inner join BASE_YUZHIBENGZHAN a on a.id=b.BASEID"
//                                      + "   where a.FIsDelete=0";
//            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_YL_HisReport);
//        }
//        public static DataTable SearchLL_HisReport(string strWhere, int year)
//        {
//            string str_YL_HisReport = @"  select a.id as BaseID,a.FDTUCode,FMapAddress,FName,b.*
//                                            from DATA_LIULIANG_" + year + " b "
//                                  + @"    inner join BASE_YUZHIBENGZHAN a on a.id=b.BASEID"
//                                      + "   where a.FIsDelete=0";
//            return publicDal.TableSearch(strWhere, str_YL_HisReport);
//        }
//        public static Hashtable SearchLL_Report(int index, int size, string sortField, string sortOrder, string strWhere)
//        {
//            string str_YL_HisReport = @"  select a.id as BaseID,a.FDTUCode,FMapAddress,FName,FOnLine,b.*,
//                                                 isnull((select top 1 1 from Alarm_Timely _c 
//                                                       where _c.BaseID=a.id and FMarkerType=3 and FStatus=1),0) as FIsAlarm
//                                            from BASE_YUZHIBENGZHAN a
//                                      inner join DATA_LIULIANG_MAIN b on a.id=b.BASEID
//                                          where a.FIsDelete=0";
//            string str_YL_HisReport_count = @"  select count(*) as countS
//                                            from BASE_YUZHIBENGZHAN a
//                                      inner join DATA_LIULIANG_MAIN b on a.id=b.BASEID
//                                          where a.FIsDelete=0";
//            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_YL_HisReport, str_YL_HisReport_count);
//        }

        public static void Insert(Hashtable has)
        {
            publicDal.Insert(has, str_LLAdd);
        }

        public static void Update(Hashtable has)
        {
            publicDal.Update(has, str_LLUpd, "id");
        }
        public static void DeleteList(Hashtable has)
        {
            publicDal.DeleteList(has, str_LLUpd, "id");
        }
    }
}