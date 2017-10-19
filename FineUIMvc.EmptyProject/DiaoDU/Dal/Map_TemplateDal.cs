using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.Collections;

namespace Dal
{
    public class Map_TemplateDal
    {
        public static DataTable Search(string strWhere)
        {
            String sql = @"select a.id as TempID,FMapTempName,FCreateDate as TempCreateDate,b.ID as TempProID
                                 ,FMapTempID,FAliasName,FMinZoom,FMaxZoom,FCenter,FZoom,FFeatures,FMapType,FStyle
                             from Map_Template a,Map_TempProperty b where a.id=b.FMapTempID";
            sql = sql + strWhere + " order by a.FCreateDate";
            DataTable data = DBUtil.SelectDataTable(sql);
            return data;
        }
        public static string InsertTemplate(Hashtable has)
        {
            string id = (has["id"] == null || has["id"].ToString() == "") ? Guid.NewGuid().ToString() : has["id"].ToString();
            has["id"] = id;
            string columns = "";
            string values = "";
            foreach (DictionaryEntry de in has)
            {
                columns += "" + de.Key + ",";
                values += "@" + de.Key + ",";
            }
            string sql = string.Format("insert into Map_Template ( {0} ) values( {1} )", columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1));

            DBUtil.Execute(sql, has);
            return id;
        }
        public static string InsertTempProperty(Hashtable has)
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
            string sql = string.Format("insert into Map_TempProperty ( {0} ) values( {1} )", columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1));

            DBUtil.Execute(sql, has);
            return id;
        }
        public static void UpdateTemp(Hashtable has)
        {
            string set = "";
            string where = "";
            foreach (DictionaryEntry de in has)
            {
                if (de.Key.ToString() != "id")
                {
                    set += "" + de.Key + "= @" + de.Key + ",";
                }
                else
                {
                    where += "" + de.Key + "= @" + de.Key + "";
                }
            }
            string sql = string.Format("update Map_Template  set {0}  where {1}", set.Substring(0, set.Length - 1), where);

            DBUtil.Execute(sql, has);
        }
        public static void UpdateTempProperty(Hashtable has)
        {
            string set = "";
            string where = "";
            foreach (DictionaryEntry de in has)
            {
                if (de.Key.ToString() != "FMapTempID")
                {
                    set += "" + de.Key + "= @" + de.Key + ",";
                }
                else
                {
                    where += "" + de.Key + "= @" + de.Key + "";
                }
            }
            string sql = string.Format("update Map_TempProperty  set {0}  where {1}", set.Substring(0, set.Length - 1), where);

            DBUtil.Execute(sql, has);
        }
    }
}