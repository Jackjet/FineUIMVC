using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using FineUIMvc;

namespace FineUIMvc.PumpMVC.DAL
{
    public class BASE_TIAOFENGDal
    {
        #region const
        private const string str_TFList = @"select a.id,b.Name as CustomerName,a.FDTUCode,FMapAddress,a.FName,FCaliber,FEQuiType,FCommunicationMode,FEQuiState
                                                 from BASE_TIAOFENG a
                                            left join Panda_Customer b on a.FCustomerID=b.ID
                                                where a.FIsDelete=0 ";

        private const string str_TFAdd = @"INSERT INTO BASE_TIAOFENG ( {0} ) VALUES( {1} )";

        private const string str_TFUpd = @"UPDATE BASE_TIAOFENG SET {0} WHERE {1}";

        private const string str_TFDel = @"DELETE FROM BASE_TIAOFENG WHERE id = @id";
        #endregion
        public static DataTable Exist(string strWhere)
        {
            string where = strWhere;

            string sql = @"select top 1 * from BASE_TIAOFENG where FIsDelete=0 ";

            if (!where.Equals(""))
            {
                sql = sql + where;
            }
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }


        public static DataTable SearchTF(string strWhere)
        {
            string where = strWhere;

            string sql = str_TFList;

            if (!where.Equals(""))
            {
                sql = sql + where;
            }
            sql = sql + " ORDER BY a.FCreateDate desc ";
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }

        public static Hashtable SearchTF(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_TFList);
        }

        public static Hashtable SearchTF_Report(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            string str_TF_Report = @" select a.id as BaseID,FCustomerID,FDTUCode,FName,FSchemeID,FOnLine,b.*,
                                                isnull((select top 1 1 from Alarm_Timely _c 
                                                      where _c.BaseID=a.id and FMarkerType=8 and FStatus=1),0) as FIsAlarm
                                           from BASE_TIAOFENG a
                                     inner join DATA_TIAOFENG_MAIN b on a.id=b.BASEID
                                          where a.FIsDelete=0 ";
            string str_TF_Report_count = @"  select count(*) as countS
                                            from BASE_TIAOFENG a
                                      inner join DATA_TIAOFENG_MAIN b on a.id=b.BASEID
                                          where a.FIsDelete=0";
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_TF_Report, str_TF_Report_count);
        }

        public static Hashtable SearchTF_HisReport(int index, int size, string sortField, string sortOrder, string strWhere, int year)
        {
            string str_YL_HisReport = @"  select a.id as BaseID,FCustomerID,FDTUCode,FName,FSchemeID,b.*
                                            from DATA_TIAOFENG_" + year + " b "
                             +@"         inner join BASE_TIAOFENG a on a.id=b.BASEID"
                                      + "   where a.FIsDelete=0";
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_YL_HisReport);
        }
        public static DataTable SearchTF_HisReport(string strWhere, int year)
        {
            string str_YL_HisReport = @"  select a.id as BaseID,FCustomerID,FDTUCode,FName,FSchemeID,b.*
                                            from DATA_TIAOFENG_" + year + " b "
                             + @"         inner join BASE_TIAOFENG a on a.id=b.BASEID"
                                      + "   where a.FIsDelete=0";
            return publicDal.TableSearch(strWhere, str_YL_HisReport);
        }
        public static void Insert(Hashtable has)
        {
            publicDal.Insert(has, str_TFAdd);
        }

        public static void Update(Hashtable has)
        {
            publicDal.Update(has, str_TFUpd, "id");
        }
        public static void DeleteList(Hashtable has)
        {
            publicDal.DeleteList(has, str_TFUpd, "id");
        }
    }
}