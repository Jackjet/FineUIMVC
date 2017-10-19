using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Web.SessionState;

namespace Water.Web.Service
{
    /// <summary>
    /// T_AlarmService 的摘要说明
    /// </summary>
    public class T_AlarmService : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string method = context.Request["method"].ToString().ToLower();

            switch (method)
            {
                case "searchalarm":
                    SearchAlarm();
                    break;
                //case "get":
                //    Get();
                //    break;
                //case "save":
                //    Save();
                //    break;
                //case "remove":
                //    Remove();
                //    break;
                //case "searchall":
                //    SearchAll();
                //    break;
                    
                default:

                    break;

            }
        }

        public void SearchAlarm()
        {
            int id = Convert.ToInt32(HttpContext.Current.Request["ID"]);   //搜索id
            string strwhere = string.Empty;
            DataTable dt = Bll.T_AlarmBll.SearchAlarm(strwhere,id);
            string json = PluSoft.Utils.JSON.Encode(dt);
            HttpContext.Current.Response.Write(json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>

        /// <summary>
        /// 
        /// </summary>
//        public void Search()
//        {
//            //查询条件
//            String _FPumpID = HttpContext.Current.Request["FPumpID"];

//            string sqland = "";
//            string FPumpID = HttpContext.Current.Request["FPumpID"];
//            if (FPumpID != null && FPumpID != "")
//            {
//                sqland += " and FPumpID = '" + FPumpID + "'";
//            }

//            //数据权限
//            string isadmin = Sys_UserService.GetSession("FUserType").ToString();
//            if (isadmin != "1")
//            {
//                string customerid = Sys_UserService.GetSession("FCustomerID").ToString();
//                sqland += " and PumpManager.FCustomerID = '" + customerid + "' ";
//            }

//            //分页
//            int pageIndex = Convert.ToInt32(HttpContext.Current.Request["pageIndex"]);
//            int pageSize = Convert.ToInt32(HttpContext.Current.Request["pageSize"]);
//            //字段排序
//            String sortField = HttpContext.Current.Request["sortField"];
//            String sortOrder = HttpContext.Current.Request["sortOrder"];
//            //业务层：数据库操作
//            //Hashtable result = new TestDB().SearchEmployees(key, pageIndex, pageSize, sortField, sortOrder);
//            Hashtable result = Bll.T_AlarmBll.Search(sqland, pageIndex, pageSize, sortField, sortOrder);


//            //JSON
//            String json = PluSoft.Utils.JSON.Encode(result);
//            HttpContext.Current.Response.Write(json);
//        }
//        /// <summary>
//        /// 
//        /// </summary>
//        public void Get()
//        {
//            String id = HttpContext.Current.Request["id"];
//            Hashtable has = Bll.T_AlarmBll.Get(id);
//            String json = PluSoft.Utils.JSON.Encode(has);
//            HttpContext.Current.Response.Write(json);
//        }
//        /// <summary>
//        /// 
//        /// </summary>
//        public void Save()
//        {
//            string result = "";
//            String json = HttpContext.Current.Request["data"];
//            ArrayList rows = (ArrayList)PluSoft.Utils.JSON.Decode(json);
//            foreach (Hashtable row in rows)
//            {
//                String id = row["id"] != null ? row["id"].ToString() : "";

//                if (id == "")       //新增：id为空，或_state为added
//                {
//                    row["FCreateDate"] = DateTime.Now;

//                    try
//                    {
//                        Bll.T_AlarmBll.Insert(row);
//                        result = "1";
//                    }
//                    catch (Exception e)
//                    {
//                        result = "0";
//                    }

//                }

//                else if (id != "") //更新：_state为空或modified
//                {
//                    try
//                    {
//                        Bll.T_AlarmBll.Update(row);
//                        result = "1";
//                    }
//                    catch (Exception e)
//                    {
//                        result = "0";
//                    }


//                }
//            }
//            HttpContext.Current.Response.Write("{\"result\":\"" + result + "\"}");
//        }
//        /// <summary>
//        /// 
//        /// </summary>
//        public void Remove()
//        {
//            string result = "";
//            String idStr = HttpContext.Current.Request["id"];
//            if (String.IsNullOrEmpty(idStr)) return;
//            String[] ids = idStr.Split(',');
//            for (int i = 0, l = ids.Length; i < l; i++)
//            {
//                string id = ids[i];
//                try
//                {
//                    Bll.T_AlarmBll.Delete(id);
//                    result = "1";
//                }
//                catch (Exception e)
//                {
//                    result = "0";
//                }

//            }

//            HttpContext.Current.Response.Write("{\"result\":\"" + result + "\"}");
//        }

//        /*----------------自定义------------------*/

//        public void SearchAll()
//        {
//            string sqland = "";
//            string FPumpID = HttpContext.Current.Request["FPumpID"];
//            if (FPumpID != null && FPumpID != "")
//            {
//                sqland += " and FPumpID = '" + FPumpID + "'";
//            }  

//            //数据权限
//            string isadmin = Sys_UserService.GetSession("FUserType").ToString();
//            if (isadmin != "1")
//            {
//                string customerid = Sys_UserService.GetSession("FCustomerID").ToString();
//                sqland += " and PumpManager.FCustomerID = '" + customerid + "' ";
//            }

//            String _FPumpID = HttpContext.Current.Request["FPumpID"];
//            String sql = @"SELECT   dbo.PumpManager.FName, dbo.T_Alarm.* FROM      dbo.T_Alarm INNER JOIN dbo.PumpManager ON dbo.T_Alarm.FPumpID = dbo.PumpManager.ID
//                           where  FStatus='1'  " + sqland + "  order by FAlarmDate desc";
         


//            DataTable dt = Dal.DBUtil.SelectDataTable(sql);

//            Hashtable result = new Hashtable();

//            if (dt.Rows.Count > 0)
//            {
//                ArrayList data = Dal.DBUtil.DataTable2ArrayList(dt);
//                result["data"] = data;
//                result["total"] = dt.Rows.Count;
//            }
//            else
//            {
//                result["total"] = 0;
//            }

//            String json = PluSoft.Utils.JSON.Encode(result);
//            HttpContext.Current.Response.Write(json);
//        }
        
    }
}