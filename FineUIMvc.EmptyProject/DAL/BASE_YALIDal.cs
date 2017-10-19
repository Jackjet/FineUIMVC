using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using FineUIMvc;

namespace FineUIMvc.PumpMVC.DAL
{
    public class BASE_YALIDal
    {
        #region const
        private const string str_YLList = @"select a.id,b.Name as CustomerName,a.FDTUCode,FMapAddress,a.FName,FCaliber,FEQuiType,FCommunicationMode,FEQuiState,FMpaUp,FMpaDown
                                                 from BASE_YALI a
                                            left join Panda_Customer b on a.FCustomerID=b.ID
                                                where a.FIsDelete=0 ";

        private const string str_YLAdd = @"INSERT INTO BASE_YALI ( {0} ) VALUES( {1} )";

        private const string str_YLUpd = @"UPDATE BASE_YALI SET {0} WHERE {1}";

        private const string str_YLDel = @"DELETE FROM BASE_YALI WHERE id = @id";
        #endregion

        public static DataTable Exist(string strWhere)
        {
            string where = strWhere;

            string sql = @"select top 1 * from BASE_YALI where FIsDelete=0 ";

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

            string sql = str_YLList;

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
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_YLList);
        }
        public static Hashtable SearchYL_HisReport(int index, int size, string sortField, string sortOrder, string strWhere, int year)
        {
            string str_YL_HisReport = @"select a.id as BaseID,a.FDTUCode,FMapAddress,FName,FMpaUp,FMpaDown,b.id,FMpa,FLL,FBatt,TempTime,Repeat
                                                    from DATA_YALI_" + year + " b "
                                          +@" inner join BASE_YALI a on a.id=b.BASEID"
                                                 + "  where a.FIsDelete=0 ";
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_YL_HisReport);
        }
        public static DataTable SearchYL_HisReport(string strWhere, int year)
        {
            string str_YL_HisReport = @"select a.id as BaseID,a.FDTUCode,FMapAddress,FName,FMpaUp,FMpaDown,FMpa,FLL,FBatt,TempTime,Repeat
                                                    from DATA_YALI_" + year + " b "
                                          + @" inner join BASE_YALI a on a.id=b.BASEID"
                                                 + "  where a.FIsDelete=0 ";
            return publicDal.TableSearch(strWhere, str_YL_HisReport);
        }
        public static Hashtable SearchYL_Report(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            string str_YL_Report = @"select a.id as BaseID,a.FDTUCode,FMapAddress,FName,FMpaUp,FMpaDown,FOnLine,b.id,FMpa,FLL,FBatt,TempTime,Repeat,
                                               isnull((select top 1 1 from Alarm_Timely _c 
                                                        where _c.BaseID=a.id and FMarkerType=7 and FStatus=1),0) as FIsAlarm
                                          from BASE_YALI a
                                    inner join DATA_YALI_MAIN b on a.id=b.BASEID
                                         where a.FIsDelete=0 ";
            string str_YL_Report_count = @"select count(*)  as countS
                                          from BASE_YALI a
                                    inner join DATA_YALI_MAIN b on a.id=b.BASEID
                                         where a.FIsDelete=0 ";
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_YL_Report, str_YL_Report_count);
        }

        public static void Insert(Hashtable has)
        {
            publicDal.Insert(has, str_YLAdd);
        }

        public static void Update(Hashtable has)
        {
            publicDal.Update(has, str_YLUpd, "id");
        }
        public static void DeleteList(Hashtable has)
        {
            publicDal.DeleteList(has, str_YLUpd, "id");
        }
    }
}