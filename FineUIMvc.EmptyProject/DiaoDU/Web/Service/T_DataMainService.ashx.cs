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
    /// T_DataMainService 的摘要说明
    /// </summary>
    public class T_DataMainService : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string method = context.Request["method"].ToString().ToLower();

            switch (method)
            {
                case "getcurrentdata":
                    GetCurrentData();
                    break;
                case "getcurrentall":
                    GetCurrentAll();
                    break;
                default:

                    break;

            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void GetCurrentData()
        {
            String _FPumpID = HttpContext.Current.Request["FPumpID"];

            String sql = "select top 1 * from  T_DataMain  where FPumpID='" + _FPumpID + "'";
            DataTable dt = Dal.DBUtil.SelectDataTable(sql);

            Hashtable result = new Hashtable();
            string str = "";

            if (dt.Rows.Count == 1)
            {
                ArrayList data = Dal.DBUtil.DataTable2ArrayList(dt);
                result["data"] = data;
                result["total"] = 1;
                //加入报警状态
              
                String sqlpump = "select * from  PumpManager where ID= '" + _FPumpID + "'";
                DataTable dtpump = Dal.DBUtil.SelectDataTable(sqlpump);
                if (dtpump.Rows.Count == 1)
                {
                    if (dtpump.Rows[0]["FIsOnline"].ToString() == "1")
                    {
                        String sqlalarm = "select * from  T_Alarm where FPumpID='" + _FPumpID + "' and FStatus='1' ";
                        DataTable dtalarm = Dal.DBUtil.SelectDataTable(sql);
                        if (dtalarm.Rows.Count > 0)
                            str =  "2";
                    }
                    else                   
                        str = "0";                    
                }
                else
                    str = "0";                

            }
            else
            {
                result["total"] = 0;
            }

            result["FIsOnline"] = str;
            result["serverTime"] = System.DateTime.Now.ToString();

            String json = PluSoft.Utils.JSON.Encode(result);
            HttpContext.Current.Response.Write(json);
        }


        public void GetCurrentAll()
        {
            //查询条件
            string sqland = "";
            //分页
            int pageIndex = Convert.ToInt32(HttpContext.Current.Request["pageIndex"]);
            int pageSize = Convert.ToInt32(HttpContext.Current.Request["pageSize"]);
            //字段排序
            String sortField = HttpContext.Current.Request["sortField"];
            String sortOrder = HttpContext.Current.Request["sortOrder"];
            //业务层：数据库操作
            Hashtable result = Bll.T_DataMainBll.Search(sqland, pageIndex, pageSize, sortField, sortOrder);


            //JSON
            String json = PluSoft.Utils.JSON.Encode(result);
            HttpContext.Current.Response.Write(json);          
        }
    }
}