using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using FineUIMvc;

namespace FineUIMvc.PumpMVC.DAL
{
    public class ControlSetDal
    {
        private const string str_CusList = @"select * from ControlSet where 1=1 ";
        private const string str_ControlSetUpd = @"UPDATE ControlSet SET {0} WHERE {1}";
        private const string str_ControlSetAdd = @"INSERT INTO ControlSet ( {0} ) VALUES( {1} )";

        public static DataTable Search(string strWhere)
        {
            string where = strWhere;

            string sql = str_CusList;

            if (!where.Equals(""))
            {
                sql = sql + where;
            }
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }

        public static void Insert(Hashtable has)
        {
            publicDal.Insert(has, str_ControlSetAdd);
        }
        public static void Update(Hashtable has)
        {
            publicDal.Update(has, str_ControlSetUpd, "PumpId");
        }
    }
}