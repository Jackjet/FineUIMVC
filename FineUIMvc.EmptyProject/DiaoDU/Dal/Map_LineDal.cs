using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.Collections;

namespace Dal
{

    public class Map_LineDal
    {
        public static DataTable Search(string strWhere)
        {
            String sql = @"select a.ID as LineID,FMapTempID,FName,FCreateDate as LineCreateDate,b.id as LineProID,FMapLineID,FAliasName,FLineType,FLine
                                 ,FPipeSize,FPipeMaterials,FPavDate,FDepth,FLength,FThickness,FStrokeColor
                                 ,FStrokeOpacity,FStrokeWeight,FStrokeStyle,FStrokeParentID
                             from Map_Line a,Map_LineProperty b where a.ID=b.FMapLineID";
            sql = sql + strWhere + " order by a.FCreateDate";
            DataTable data = DBUtil.SelectDataTable(sql);
            return data;
        }
        public static string InsertLine(Hashtable has)
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
            string sql = string.Format("insert into Map_Line ( {0} ) values( {1} )", columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1));

            DBUtil.Execute(sql, has);
            return id;
        }
        public static string InsertLineProperty(Hashtable has)
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
            string sql = string.Format("insert into Map_LineProperty ( {0} ) values( {1} )", columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1));

            DBUtil.Execute(sql, has);
            return id;
        }
        public static void UpdateLine(Hashtable has)
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
            string sql = string.Format("update Map_Line  set {0}  where {1}", set.Substring(0, set.Length - 1), where);

            DBUtil.Execute(sql, has);
        }
        public static void UpdateLineProperty(Hashtable has)
        {
            string set = "";
            string where = "";
            foreach (DictionaryEntry de in has)
            {
                if (de.Key.ToString() != "FMapLineID")
                {
                    set += "" + de.Key + "= @" + de.Key + ",";
                }
                else
                {
                    where += "" + de.Key + "= @" + de.Key + "";
                }
            }
            string sql = string.Format("update Map_LineProperty  set {0}  where {1}", set.Substring(0, set.Length - 1), where);

            DBUtil.Execute(sql, has);
        }
    }
}