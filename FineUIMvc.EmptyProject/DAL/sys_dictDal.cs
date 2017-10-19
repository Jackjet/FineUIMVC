using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;


namespace FineUIMvc.PumpMVC.DAL
{
    public class sys_dictDal
    {
        #region const
        private const string strCRM_dictList = @"SELECT FDictID,FName,FDescription FROM sys_dict ";

        private const string strCRM_dictItemList = @"SELECT FID,FDictID,FValue,FName,FParentValue,FDescription,FCustomizedID FROM sys_dictItems where 1=1 ";

        private const string strCRM_dictAdd = @"INSERT INTO sys_dictItems ( {0} ) VALUES( {1} )";

        private const string strCRM_dictDel = @"DELETE FROM sys_dictItems WHERE FID = @FID";

        private const string strCRM_dictDelList = @"DELETE FROM sys_dictItems ";
        #endregion

        public static DataTable SearchDict(string strWhere)
        {
            string where = strWhere;

            string sql = strCRM_dictList;

            if (!where.Equals(""))
            {
                sql = sql + " where " + where;
            }
            sql = sql + " ORDER BY FDescription ";
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }

        public static Hashtable SearchDictItem(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, strCRM_dictItemList);
        }

        public static DataTable SearchDDL(string strWhere)
        {
            string where = strWhere;

            string sql = strCRM_dictItemList;

            if (!where.Equals(""))
            {
                sql = sql + where;
            }
            sql = sql + " ORDER BY FValue";
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }



        //public static void Insert(Hashtable has)
        //{
        //    string columns = "";
        //    string values = "";
        //    foreach (DictionaryEntry de in has)
        //    {
        //        columns += "" + de.Key + ",";
        //        values += "@" + de.Key + ",";
        //    }
        //    string sql = string.Format(strCRM_dictAdd, columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1));

        //    DBUtil.Execute(sql, has);
        //}

        //public static void Delete(int id)
        //{
        //    Hashtable has = new Hashtable();
        //    has["FID"] = id;
        //    DBUtil.Execute(strCRM_dictDel, has);
        //}
        //public static void DeleteList(List<int> id)
        //{
        //    string sql = string.Empty;
        //    string values = "";
        //    for(int i=0;i<id.Count;i++)
        //    {
        //        if(i!=id.Count-1)
        //        {
        //            values += "" + id[i] + ",";
        //        }
        //        else
        //        {
        //            values += "" + id[i] + "";
        //        }
        //    }

        //    sql = strCRM_dictDelList + "WHERE FID IN(" + values + " )";

        //    DBUtil.Execute(sql);
        //}
    }
}