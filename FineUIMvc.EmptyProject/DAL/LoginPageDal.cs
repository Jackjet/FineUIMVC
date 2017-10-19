using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FineUIMvc.PumpMVC.DAL
{
    public class LoginPageDal
    {
        private const string str_FamenList = @"SELECT ID , Host, FileName,Title from LoginPage where FIsDelete=0 ";

        private const string str_FamenUpd = @"UPDATE LoginPage SET {0} WHERE {1}";

        private const string str_FamenAdd = @"INSERT INTO LoginPage ( {0} ) VALUES( {1} )";

        public static DataTable Exist(string strWhere)
        {
            string where = strWhere;

            string sql = @"select top 1 * from LoginPage where 1=1 ";

            if (!where.Equals(""))
            {
                sql = sql + where;
            }
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }
        public static DataTable SearchInsertAlarm(string strWhere)
        {
            string where = strWhere;

            string sql = @"SELECT top 1 FileName from LoginPage where FIsDelete=0 ";

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

        public static int Insert(Hashtable has)
        {
            return publicDal.InsertGetID(has, str_FamenAdd);
        }

        public static void Update(Hashtable has)
        {
            publicDal.Update(has, str_FamenUpd, "ID");
        }
        public static void DeleteList(Hashtable has)
        {
            publicDal.DeleteList(has, str_FamenUpd, "ID");
        }
    }
}