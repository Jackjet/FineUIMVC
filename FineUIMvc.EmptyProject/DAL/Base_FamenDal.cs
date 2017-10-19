using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FineUIMvc.PumpMVC.DAL
{
    public class Base_FamenDal
    {
        private const string str_FamenList = @"SELECT a.id ,
                                                      b.Name as CustomerName,
                                                     a.FDTUCode ,
                                                     a.FName ,                                                   
                                                     a.FBrand FROM [dbo].[BASE_FAMEN] a
                                                     left join Panda_Customer b on a.FCustomerID=b.ID
                                                      where a.FIsDelete=0 ";

        private const string str_FamenUpd = @"UPDATE BASE_FAMEN SET {0} WHERE {1}";

        private const string str_FamenAdd = @"INSERT INTO BASE_FAMEN ( {0} ) VALUES( {1} )";

        public static DataTable Exist(string strWhere)
        {
            string where = strWhere;

            string sql = @"select top 1 * from BASE_FAMEN where FIsDelete=0 ";

            if (!where.Equals(""))
            {
                sql = sql + where;
            }
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }

        public static Hashtable Search(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_FamenList);
        }

        public static Hashtable SearchFM_HisReport(int index, int size, string sortField, string sortOrder, string strWhere, int year)
        {
            string str_FM_HisReport = @"  select a.ID as BaseID,a.FDTUCode,FMapAddress,FName,FDeviceType,b.*
                                            from DATA_FAMEN_" + year + " b "
                                   +@"   inner join BASE_FAMEN a on a.ID=b.BASEID"
                                      + "   where a.FIsDelete=0";
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_FM_HisReport);
        }
        public static DataTable SearchFM_HisReport(string strWhere, int year)
        {
            string str_FM_HisReport = @"  select a.ID as BaseID,a.FDTUCode,FMapAddress,FName,FDeviceType,b.*
                                            from DATA_FAMEN_" + year + " b "
                                   + @"   inner join BASE_FAMEN a on a.ID=b.BASEID"
                                      + "   where a.FIsDelete=0";
            return publicDal.TableSearch(strWhere, str_FM_HisReport);
        }
        public static Hashtable SearchFM_Report(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            string str_FM_HisReport = @"  select a.ID as BaseID,a.FDTUCode,FMapAddress,FName,FDeviceType,FOnLine,b.*,
                                                 isnull((select top 1 1 from Alarm_Timely _c 
                                                       where _c.BaseID=a.ID and FMarkerType=3 and FStatus=1),0) as FIsAlarm
                                            from BASE_FAMEN a
                                      inner join DATA_FAMEN_MAIN b on a.ID=b.BASEID
                                          where a.FIsDelete=0";
            string str_FM_HisReport_count = @"  select count(*) as countS
                                            from BASE_FAMEN a
                                      inner join DATA_FAMEN_MAIN b on a.ID=b.BASEID
                                          where a.FIsDelete=0";
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_FM_HisReport, str_FM_HisReport_count);
        }

        public static int Insert(Hashtable has)
        {
            return publicDal.InsertGetID(has, str_FamenAdd);
        }

        public static void Update(Hashtable has)
        {
            publicDal.Update(has, str_FamenUpd, "id");
        }
        public static void DeleteList(Hashtable has)
        {
            publicDal.DeleteList(has, str_FamenUpd, "id");
        }
    }
}