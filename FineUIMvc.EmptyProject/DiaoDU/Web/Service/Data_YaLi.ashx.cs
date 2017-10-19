using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Data;
using System.Web.SessionState;
using System.Collections;
using System.Data.SqlClient;

namespace Water.Web.Service
{
    /// <summary>
    /// Data_YaLi 的摘要说明
    /// </summary>
    public class Data_YaLi : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string method = context.Request["method"].ToString().ToLower();
            switch (method)
            {
                case "insertyali":
                    InsertYALI();
                    break;
                case "updyali":
                    UpdYALI();
                    break;
                case "searchyali":
                    SearchYALI();
                    break;
                case "searchyali_year":
                    SearchYALI_Year();
                    break;
            }
        }
        public void SearchYALI()
        {
            string ID = HttpContext.Current.Request["ID"];   //搜索id
            string SearchText = HttpContext.Current.Request["SearchText"];   //查询内容
            string strwhere = string.Empty;
            if (ID != null && ID != "")
            {
                strwhere = strwhere + " and a.id='" + ID + "'";
            }
            if (SearchText != null && SearchText != "")
            {
                strwhere = strwhere + " and FName='" + SearchText + "'";
            }
            int pageIndex = Convert.ToInt32(HttpContext.Current.Request["pageIndex"]);
            int pageSize = Convert.ToInt32(HttpContext.Current.Request["pageSize"]);
            Hashtable result = Bll.Data_YaLiBll.SearchYALI(strwhere, pageIndex, pageSize, "a.FCreateDate", "desc");
            string json = PluSoft.Utils.JSON.Encode(result);
            HttpContext.Current.Response.Write(json);
        }
        public void SearchYALI_Year()
        {
            string ID = HttpContext.Current.Request["ID"];   //搜索id
            string Start = HttpContext.Current.Request["StartDate"];   //开始日期
            string End = HttpContext.Current.Request["EndDate"];   //结束日期
            string strwhere = string.Empty;
            int year = Convert.ToDateTime(Start).Year;
            if (ID != null && ID != "")
            {
                strwhere = strwhere + " and a.id='" + ID + "'";
            }
            strwhere = strwhere + " and TempTime between '" + Start + "' and '" + End + "'";
            int pageIndex = Convert.ToInt32(HttpContext.Current.Request["pageIndex"]);
            int pageSize = Convert.ToInt32(HttpContext.Current.Request["pageSize"]);
            Hashtable result = Bll.Data_YaLiBll.SearchYALI_Year(strwhere, pageIndex, pageSize, "TempTime", "desc", year);
            string json = PluSoft.Utils.JSON.Encode(result);
            HttpContext.Current.Response.Write(json);
        }
        public void InsertYALI()
        {
            string result = "";
            string FDTUCode = HttpContext.Current.Request["FDTUCode"];
            string msg = "";
            DataTable data = Dal.DBUtil.SelectDataTable("select * from BASE_YALI where FDTUCode='" + FDTUCode + "'");
            if (data.Rows.Count > 0)
            {
                result = "0";
                msg = "编码禁止重复";
            }
            else
            {
                string FName = HttpContext.Current.Request["FName"];
                string FMpaUp = HttpContext.Current.Request["FMpaUp"];
                string FMpaDown = HttpContext.Current.Request["FMpaDown"];
                string guid = Guid.NewGuid().ToString();
                Hashtable has1 = new Hashtable();
                Hashtable has2 = new Hashtable();
                has1["id"] = guid;
                has1["FCustomerID"] = 1;
                has1["FDTUCode"] = FDTUCode;
                has1["FName"] = FName;
                has1["FMpaUp"] = FMpaUp;
                has1["FMpaDown"] = FMpaDown;
                has1["FCreateDate"] = DateTime.Now;
                has2["id"] = Guid.NewGuid();
                has2["BASEID"] = guid;
                has2["FOnLine"] = 0;
                has2["FDTUCode"] = FDTUCode;
                try
                {
                    Bll.Data_YaLiBll.InsertYALI(has1);
                    Bll.Data_YaLiBll.InsertYALI_MAIN(has2);
                    Bll.Data_YaLiBll.SearchInsertAlarm(" and FMarkerType=7 ", guid);
                    result = "1";
                }
                catch (Exception e)
                {
                    result = "0";
                }
            }

            HttpContext.Current.Response.Write("{\"msg\":\"" + msg + "\",\"result\":\"" + result + "\"}");
        }
        public void UpdYALI()
        {
            string result = "";
            string id = HttpContext.Current.Request["id"];                    //主键id
            string FDTUCode = HttpContext.Current.Request["FDTUCode"];
            string oldFDTUCode = HttpContext.Current.Request["oldFDTUCode"];
            string msg = "";
            DataTable data = Dal.DBUtil.SelectDataTable("select * from BASE_YALI where FDTUCode='" + FDTUCode + "' and FDTUCode<>'" + oldFDTUCode + "'");
            if (data.Rows.Count > 0)
            {
                result = "0";
                msg = "编码禁止重复";
            }
            else
            {
                string FName = HttpContext.Current.Request["FName"];
                string FMpaUp = HttpContext.Current.Request["FMpaUp"];
                string FMpaDown = HttpContext.Current.Request["FMpaDown"];
                Hashtable has1 = new Hashtable();
                Hashtable has2 = new Hashtable();
                has1["id"] = id;
                has1["FDTUCode"] = FDTUCode;
                has1["FName"] = FName;
                has1["FMpaUp"] = FMpaUp;
                has1["FMpaDown"] = FMpaDown;
                has2["BASEID"] = id;
                has2["FDTUCode"] = FDTUCode;

                try
                {
                    Bll.Data_YaLiBll.UpdateYALI(has1);
                    Bll.Data_YaLiBll.UpdateYALI_MAIN(has2);
                    result = "1";
                }
                catch (Exception e)
                {
                    result = "0";
                }
            }
            HttpContext.Current.Response.Write("{\"msg\":\"" + msg + "\",\"result\":\"" + result + "\"}");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}