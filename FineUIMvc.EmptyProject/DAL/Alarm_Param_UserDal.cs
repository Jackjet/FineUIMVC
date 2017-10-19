using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using FineUIMvc;

namespace FineUIMvc.PumpMVC.DAL
{
    public class Alarm_Param_UserDal
    {
        public static bool Exist(string UserID)
        {
            bool flag = true;
            string sql = @"select top 1 * from Alarm_Param_User where UserID= '" + UserID + "'";
            DataTable dt = publicDal.TableSearch(sql);

            if (dt.Rows.Count > 0)
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
        }
        public static DataTable Search(string strWhere)
        {
            string where = strWhere;

            string sql = @"select * from Alarm_Param_User where 1=1 ";

            if (!where.Equals(""))
            {
                sql = sql + where ;
            }
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }

        public static void Insert(Hashtable has)
        {
            publicDal.Insert(has, "INSERT INTO Alarm_Param_User ( {0} ) VALUES( {1} )");
        }
        public static void Update(Hashtable has)
        {
            publicDal.Update(has, "UPDATE Alarm_Param_User SET {0} WHERE {1}", "UserID");
        }
    }
}