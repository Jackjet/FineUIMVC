using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace Dal
{
    public class Sys_UserDal
    {
        static String tbname = "Sys_User";

        public static Hashtable Search(string key, int index, int size, string sortField, string sortOrder)
        {
            string sql = @"SELECT SYS_USER.*, T_Customer.FName as CustomerName FROM SYS_USER LEFT JOIN  T_Customer ON SYS_USER.FCustomerID = T_Customer.ID   where  1=1 and SYS_USER.FDeleted !='1'  " + key + "";

            if (String.IsNullOrEmpty(sortField) == false)
            {
                if (sortOrder != "desc") sortOrder = "asc";
                sql += " order by " + sortField + " " + sortOrder;
            }

            DataTable dt = DBUtil.SelectDataTablePager(sql, index, size);
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
            //has["id"] = id;
            has.Remove("id");

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