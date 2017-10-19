using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using FineUIMvc;

namespace FineUIMvc.PumpMVC.DAL
{
    public class Panda_CustomerDal
    {
        private const string str_CusList = @"select a.*,b.FName as CustomerLevelName,c.FName as CustomerTypeName,FMapTempID
                                               from Panda_Customer a
                                          left join sys_dictItems b on a.CustomerLevel=b.FValue and b.FDictID=51
                                          left join sys_dictItems c on a.CustomerType=c.FValue and c.FDictID=50
                                              where FIsDelete=0 ";
        private const string str_CusUrlList = @"select  pct.ID as ID ,pct.NavigateUrl as NavigateUrl,tm.Name as TopMenuName from                                           Panda_CustomerTopMenu pct left join sys_TopMenus tm
                                               on pct.TopMenuID=tm.ID where 1=1";

        private const string str_CusUpd = @"UPDATE Panda_Customer SET {0} WHERE {1}";

        private const string str_CusAdd = @"INSERT INTO Panda_Customer ( {0} ) VALUES( {1} )";

        private const string str_UrlUpd = @"UPDATE Panda_CustomerTopMenu SET {0} WHERE {1}";

        private const string str_UrlAdd = @"INSERT INTO Panda_CustomerTopMenu ( {0} ) VALUES( {1} )";

        private const string str_TopMenusList = @"SELECT ID as FValue,Name as FName FROM sys_TopMenus where 1=1 ";

        public static Hashtable Search(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_CusList);
        }

        public static DataTable Exist(string strWhere)
        {
            string where = strWhere;

            string sql = @"select top 1 * from Panda_Customer where FIsDelete=0 ";

            if (!where.Equals(""))
            {
                sql = sql + where;
            }
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }

        public static void Insert(Hashtable has)
        {
            publicDal.Insert(has, str_CusAdd);
        }
        public static void InsertUrl(Hashtable has)
        {
            publicDal.Insert(has, str_UrlAdd);
        }

        public static void Update(Hashtable has)
        {
            publicDal.Update(has, str_CusUpd, "ID");
        }
        public static void UpdateUrl(Hashtable has)
        {
            publicDal.Update(has, str_UrlUpd, "ID");
        }
        public static void DeleteList(Hashtable has)
        {
            publicDal.DeleteList(has, str_CusUpd, "ID");
        }

        public static DataTable SearchMenusDDL(string strWhere)
        {
            string where = strWhere;

            string sql = str_TopMenusList;

            if (!where.Equals(""))
            {
                sql = sql + where;
            }
            sql = sql + " ORDER BY FValue";
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }
        internal static Hashtable SearchUrl(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_CusUrlList);
        }


        internal static DataTable ExistUrl(string strWhere)
        {
            string where = strWhere;

            string sql = @"select top 1 * from Panda_CustomerTopMenu where 1=1 ";

            if (!where.Equals(""))
            {
                sql = sql + where;
            }
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }
    }
}