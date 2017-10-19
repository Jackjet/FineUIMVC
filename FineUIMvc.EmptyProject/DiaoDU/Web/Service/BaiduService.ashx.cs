using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Text;

namespace Water.Web.Service
{
    /// <summary>
    /// BaiduService 的摘要说明
    /// </summary>
    public class BaiduService : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string method = context.Request["method"].ToString().ToLower();

            switch (method)
            {
                case "getgroupbaidu":
                    GetGroupBaidu();
                    break;
                case "getmessage":
                    GetMessage();
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
        

        /*----------------自定义------------------*/

        /// <summary>
        /// baidu地图获取mark点
        /// </summary>
        public void GetGroupBaidu()
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

            String sql = "select * from PumpManager where 1=1" + sqland;
            DataTable dt = Dal.DBUtil.SelectDataTable(sql);

            StringBuilder str = new StringBuilder();

            int count = dt.Rows.Count;
            string id = "";
            string name = "";
            string position = "";

            str.Append("[");
            for (int i = 0; i < count; i++)
            {
                id = dt.Rows[i]["id"].ToString();
                name = dt.Rows[i]["fname"].ToString();
                position = dt.Rows[i]["FLatAndLong"].ToString();

                if (position == "")
                {
                    position = "116.404, 39.915";
                }

                if (i == count - 1)
                {
                    str.Append("{id: \"" + id + "\", name: \"" + name + "\", position: \"" + position + "\"}");
                }
                else
                {
                    str.Append("{id: \"" + id + "\", name: \"" + name + "\", position: \"" + position + "\"},");
                }
            }


            str.Append("]");

            HttpContext.Current.Response.Write(str.ToString());

        }

        /// <summary>
        /// baidu地图刷新图标及label
        /// </summary>
        public void GetMessage()
        {
            String id = HttpContext.Current.Request["id"];


            string number = "";
            string pumpname = "";
            string alarm = "0";
            string message = "";



            String sql = @"SELECT * from PumpManager where id='" + id + "'";
            DataTable dt = Dal.DBUtil.SelectDataTable(sql);

            if (dt.Rows.Count == 1)
            {
                number = dt.Rows[0]["fnumber"].ToString();
                pumpname = dt.Rows[0]["fname"].ToString();

                String sqlalarm = @"SELECT * from T_Alarm where FPumpID='" + id + "' and FStatus='1'";
                DataTable dtalarm = Dal.DBUtil.SelectDataTable(sqlalarm);

                if (dtalarm.Rows.Count > 0)
                    alarm = "1";

                for (int i = 0; i < dtalarm.Rows.Count; i++)
                    message += "" + dtalarm.Rows[i]["FContent"].ToString() + "<br>";

            }

            StringBuilder str = new StringBuilder();
            str.Append("{\"number\":\"" + number + "\",\"pumpname\":\"" + pumpname + "\",\"alarm\":\"" + alarm + "\",\"message\":\"" + message.ToString() + "\"}");
            HttpContext.Current.Response.Write(str.ToString());
        }



    
    }
}