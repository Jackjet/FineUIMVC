using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using Water.Web.Service;

namespace Dal
{
    public class T_DataMainDal
    {
        static String tbname = "T_Data";

        public static Hashtable Search(string key, int index, int size, string sortField, string sortOrder)
        {
            string sqland = "";
            //数据权限
            string isadmin = Sys_UserService.GetSession("FUserType").ToString();
            if (isadmin != "1")
            {
                //数据权限
                string customerid = Sys_UserService.GetSession("FCustomerID").ToString();
                sqland += " and FCustomerID = '" + customerid + "'";
            }



            string sql = @"select * from PumpManager where FDeleted !='1' " + sqland;

            if (String.IsNullOrEmpty(sortField) == false)
            {
                if (sortOrder != "desc") sortOrder = "asc";
                sql += " order by " + sortField + " " + sortOrder;
            }

            DataTable dt = DBUtil.SelectDataTablePager(sql, index, size);

            dt.Columns.Add("DataMain");


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["FIsOnline"].ToString() == "1")
                {
                    string id = dt.Rows[i]["ID"].ToString();
                    String sqlalarm = "select * from  T_Alarm where FPumpID='" + id + "' and FStatus='1' ";
                    DataTable dtalarm = Dal.DBUtil.SelectDataTable(sqlalarm);

                    if (dtalarm.Rows.Count > 0)
                        dt.Rows[i]["FIsOnline"] = "2";  
                }

                String _FPumpID = dt.Rows[i]["ID"].ToString();
                String sql_data = "select top 1 * from  T_DataMain  where FPumpID='" + _FPumpID + "'";
                DataTable dt_data = Dal.DBUtil.SelectDataTable(sql_data);

                if (dt_data.Rows.Count == 1)
                    dt.Rows[i]["DataMain"] = new Commond.Json().Dtb2Json(dt_data).ToString();
            }

            ArrayList data = DBUtil.DataTable2ArrayList(dt);

            int count = DBUtil.ExecuteScalar(sql);

            Hashtable result = new Hashtable();
            result["data"] = data;
            result["total"] = count;

            return result;
        }
        public static Hashtable Get(string id)
        {
            string sql = "select * from " + tbname + " where id = '" + id + "'";
            ArrayList data = DBUtil.Select(sql);
            return data.Count > 0 ? (Hashtable)data[0] : null;
        }
        public static string Insert(Hashtable has)
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
            string sql = string.Format("insert into " + tbname + " ( {0} ) values( {1} )", columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1));

            DBUtil.Execute(sql, has);
            return id;
        }
        public static void Delete(string id)
        {
            Hashtable has = new Hashtable();
            has["id"] = id;
            DBUtil.Execute("delete from " + tbname + " where id = @id", has);
        }
        public static void Update(Hashtable has)
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
            string sql = string.Format("update " + tbname + "  set {0}  where {1}", set.Substring(0, set.Length - 1), where);

            DBUtil.Execute(sql, has);
        }
    }
}