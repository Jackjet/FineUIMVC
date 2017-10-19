using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Data;
using System.Web.SessionState;
using System.Collections;
using System.Data.SqlClient;
using System.Text;
using FineUIMvc.PumpMVC.Controllers;

namespace Water.Web.Service
{
    /// <summary>
    /// Data_TiaoFeng 的摘要说明
    /// </summary>
    public class Data_TiaoFeng : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string method = context.Request["method"].ToString().ToLower();

            switch (method)
            {
                case "searchtiaofeng":
                    SearchTiaoFeng();
                    break;
                default:

                    break;

            }
        }

        public void SearchTiaoFeng()
        {
            string sql = "";
            string BaseId = HttpContext.Current.Request["BaseId"];
            if (BaseId != "" && BaseId != null)
            {
                sql = " and a.id='" + BaseId + "'";
            }

            DataTable dt = Bll.Data_TiaoFengBll.Search(sql);
            String json = "";
            if (dt.Rows.Count == 0)
            {
                json = PluSoft.Utils.JSON.Encode("[]");
            }
            else
            {
                json = PluSoft.Utils.JSON.Encode(dt);
            }

            HttpContext.Current.Response.Write(json);
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