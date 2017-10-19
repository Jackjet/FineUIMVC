using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;

namespace Dal
{
    public class BaseToMarkerDal
    {
        public static Hashtable Search(string sql, int index, int size, string sortField, string sortOrder)
        {
            if (String.IsNullOrEmpty(sortField) == false)
            {
                if (sortOrder != "desc") sortOrder = "asc";
                sql += " order by " + sortField + " " + sortOrder;
            }

            DataTable dt = DBUtil.SelectDataTablePager(sql, index, size);
            //ArrayList data = DBUtil.DataTable2ArrayList(dt);

            int count = DBUtil.ExecuteScalar(sql);

            Hashtable result = new Hashtable();
            result["data"] = dt;
            result["total"] = count;

            return result;
        }
    }
}