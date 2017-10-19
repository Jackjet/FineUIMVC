using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.Collections;

namespace Dal
{
    public class Map_AreaDal
    {
        public static DataTable Search(string strWhere)
        {
            String sql = @"select a.ID as AreaID,FMapTempID,FName,FCreateDate as AreaCreateDate,b.id as AreaProID,FMapAreaID,FAliasName,FAreaType,FArea
                                 ,FStrokeColor,FStrokeOpacity,FStrokeWeight,FStrokeStyle,FAreaColor,FAreaOpacity
                             from Map_Area a,Map_AreaProperty b where a.ID=b.FMapAreaID";
            sql = sql + strWhere + " order by a.FCreateDate";
            DataTable data = DBUtil.SelectDataTable(sql);
            return data;
        }
        public static DataTable SearchOverlay(string strWhere)
        {
            String sql = @"select a.FName,b.* from Map_Area a,Map_Area_Overlay b where a.ID=b.FMapAreaID ";
            sql = sql + strWhere + " order by a.FCreateDate";
            DataTable data = DBUtil.SelectDataTable(sql);
            return data;
        }
        public static string InsertArea(Hashtable has)
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
            string sql = string.Format("insert into Map_Area ( {0} ) values( {1} )", columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1));

            DBUtil.Execute(sql, has);
            return id;
        }
        public static string InsertAreaProperty(Hashtable has)
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
            string sql = string.Format("insert into Map_AreaProperty ( {0} ) values( {1} )", columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1));

            DBUtil.Execute(sql, has);
            return id;
        }
        public static string InsertAreaOverlay(Hashtable has)
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
            string sql = string.Format("insert into Map_Area_Overlay ( {0} ) values( {1} )", columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1));

            DBUtil.Execute(sql, has);
            return id;
        }
        public static void UpdateArea(Hashtable has)
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
            string sql = string.Format("update Map_Area  set {0}  where {1}", set.Substring(0, set.Length - 1), where);

            DBUtil.Execute(sql, has);
        }
        public static void UpdateAreaProperty(Hashtable has)
        {
            string set = "";
            string where = "";
            foreach (DictionaryEntry de in has)
            {
                if (de.Key.ToString() != "FMapAreaID")
                {
                    set += "" + de.Key + "= @" + de.Key + ",";
                }
                else
                {
                    where += "" + de.Key + "= @" + de.Key + "";
                }
            }
            string sql = string.Format("update Map_AreaProperty  set {0}  where {1}", set.Substring(0, set.Length - 1), where);

            DBUtil.Execute(sql, has);
        }
        public static void DeleteAreaOverlay(string FMapOverlayID, string FMapTempID)
        {
            string sql = string.Format("delete Map_Area_Overlay where FMapOverlayID='{0}' AND FMapTempID='{1}'", FMapOverlayID, FMapTempID);

            DBUtil.Execute(sql);
        }
    }
}