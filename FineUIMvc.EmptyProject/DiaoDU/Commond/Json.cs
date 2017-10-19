using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Script.Serialization;
using System.Collections;

namespace Commond
{
    public class Json
    {

        /// <summary>
        /// DataTable转Json
        /// </summary>
        /// <param name="dtb"></param>
        /// <returns></returns>
        public string Dtb2Json(DataTable dtb)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            ArrayList dic = new ArrayList();
            foreach (DataRow row in dtb.Rows)
            {
                Dictionary<string, object> drow = new Dictionary<string, object>();
                foreach (DataColumn col in dtb.Columns)
                {
                    drow.Add(col.ColumnName, row[col.ColumnName]);
                }
                dic.Add(drow);
            }
            return jss.Serialize(dic);
        }


        /// <summary>
        /// Json转DataTable
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public DataTable Json2Dtb(string json)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            ArrayList dic = jss.Deserialize<ArrayList>(json);
            DataTable dtb = new DataTable();

            if (dic.Count > 0)
            {
                foreach (Dictionary<string, object> drow in dic)
                {
                    if (dtb.Columns.Count == 0)
                    {
                        foreach (string key in drow.Keys)
                        {
                            dtb.Columns.Add(key, drow[key].GetType());
                        }
                    }

                    DataRow row = dtb.NewRow();
                    foreach (string key in drow.Keys)
                    {

                        row[key] = drow[key];
                    }
                    dtb.Rows.Add(row);
                }
            }
            return dtb;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ArrayList DataTable2ArrayList(DataTable data)
        {
            ArrayList array = new ArrayList();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                DataRow row = data.Rows[i];

                Hashtable record = new Hashtable();
                for (int j = 0; j < data.Columns.Count; j++)
                {
                    object cellValue = row[j];
                    if (cellValue.GetType() == typeof(DBNull))
                    {
                        cellValue = null;
                    }
                    record[data.Columns[j].ColumnName] = cellValue;
                }
                array.Add(record);
            }
            return array;
        }
    }
}