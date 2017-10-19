using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using FineUIMvc;

namespace FineUIMvc.PumpMVC.DAL
{
    public class FLogParamDal
    {
        private const string str_FLogParam_UserUpd = @"UPDATE FLogParam_User SET {0} WHERE {1}";

        public static bool Exist(string UserID)
        {
            bool flag = true;
            string sql = @"select top 1 * from FLogParam_User where UserID= '" + UserID+"'";
            DataTable dt = publicDal.TableSearch(sql);

            if(dt.Rows.Count>0)
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public static DataTable SearchInsert(string UserID)
        {
            string sql = @"insert into FLogParam_User select UserID='" + UserID + "', ID as FField,FDefault as IsSelect,FDefault as IsSelect2 from FLogParam ";

            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }

        public static DataTable Search(string strWhere)
        {
            string where = strWhere;

            string sql = @"select ID,FName,FField from FLogParam where FMapView<>1 ";

            if (!where.Equals(""))
            {
                sql = sql + where + " order by FSort";
            }
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }

        public static DataTable Search(string strWhere, string IsSelect)
        {
            string where = strWhere;

            string sql = @"select a.FField, FName, b.FField as FFieldName," + IsSelect + ",FSort,FCommon from FLogParam_User a,FLogParam b where a.FField=b.ID ";

            if (!where.Equals(""))
            {
                sql = sql + where + " order by FSort";
            }
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }
        public static DataTable SearchParm()
        {
            string sql = @"select * from FLogParam  order by FSort ";

            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }

        public static void Update(Hashtable has)
        {
            publicDal.Update(has, str_FLogParam_UserUpd, "UserID,FField");
        }
    }
}