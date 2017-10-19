using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.Collections;

namespace Dal
{
    public class Map_MarkerDal
    {
        public static DataTable Search(string strWhere)
        {
            String sql = @"select a.ID as MarkerID,FMapTempID,FName,FCreateDate as MarkerCreateDate,b.ID as MarkerProID
                                 ,FMarkerID,FAliasName,FType,FLineID,FParentID,FMarker,FMID
                             from Map_Marker a,Map_MarkerProperty b where a.ID=b.FMarkerID and a.FIsDelete=0 ";
            sql = sql + strWhere + " order by a.FCreateDate";
            DataTable data = DBUtil.SelectDataTable(sql);
            return data;
        }

        public static string InsertMarker(Hashtable has)
        {
            string id = (has["ID"] == null || has["ID"].ToString() == "") ? Guid.NewGuid().ToString() : has["ID"].ToString();
            has["ID"] = id;
            string columns = "";
            string values = "";
            foreach (DictionaryEntry de in has)
            {
                columns += "" + de.Key + ",";
                values += "@" + de.Key + ",";
            }
            string sql = string.Format("insert into Map_Marker ( {0} ) values( {1} )", columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1));

            DBUtil.Execute(sql, has);
            return id;
        }
        public static string InsertMarkerProperty(Hashtable has)
        {
            string id = (has["id"] == null || has["id"].ToString() == "") ? Guid.NewGuid().ToString() : has["id"].ToString();
            //has["id"] = id;
            has.Remove("id");
            string columns = "";
            string values = "";
            foreach (DictionaryEntry de in has)
            {
                columns += "" + de.Key + ",";
                values += "@" + de.Key + ",";
            }
            string sql = string.Format("insert into Map_MarkerProperty ( {0} ) values( {1} )", columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1));

            DBUtil.Execute(sql, has);
            return id;
        }
        public static void UpdateMarker(Hashtable has)
        {
            string set = "";
            string where = "";
            foreach (DictionaryEntry de in has)
            {
                if (de.Key.ToString() != "ID")
                {
                    set += "" + de.Key + "= @" + de.Key + ",";
                }
                else
                {
                    where += "" + de.Key + "= @" + de.Key + "";
                }
            }
            string sql = string.Format("update Map_Marker  set {0}  where {1}", set.Substring(0, set.Length - 1), where);

            DBUtil.Execute(sql, has);
        }
        public static void UpdateMarkerProperty(Hashtable has)
        {
            string set = "";
            string where = "";
            foreach (DictionaryEntry de in has)
            {
                if (de.Key.ToString() != "FMarkerID")
                {
                    set += "" + de.Key + "= @" + de.Key + ",";
                }
                else
                {
                    where += "" + de.Key + "= @" + de.Key + "";
                }
            }
            string sql = string.Format("update Map_MarkerProperty  set {0}  where {1}", set.Substring(0, set.Length - 1), where);

            DBUtil.Execute(sql, has);
        }
        public static void DeleteMarker(int FType,string idList)
        {
            string delSql = @"update a set a.FIsDelete=1 from Map_Marker a,Map_MarkerProperty b where a.ID=b.FMarkerID and a.FIsDelete=0 and FType="+FType+" and b.FMID in (" + idList + ")";
            DBUtil.Execute(delSql);
        }
    }
}