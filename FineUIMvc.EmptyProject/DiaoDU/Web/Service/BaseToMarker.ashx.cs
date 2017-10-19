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
    /// BaseToMarker 的摘要说明
    /// </summary>
    public class BaseToMarker : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string method = context.Request["method"].ToString().ToLower();
            switch (method)
            {
                case "search":
                    Search();
                    break;
              
            }
        }

        public void Search()
        {
            string Type = HttpContext.Current.Request["Type"];
            string ID = HttpContext.Current.Request["ID"];   //搜索id
            string Content = HttpContext.Current.Request["Content"]; //搜索内容（代码，名称等）
            string strwhere = string.Empty;
            string sql = string.Empty;
            string json = string.Empty;
            string jsonName = string.Empty;

            if (ID != null && ID != "")
            {
                strwhere = strwhere + " and id='" + ID + "'";
            }
            if (Content != null && Content != "")
            {
                switch (Type)
                {
                    case "0": ; break; //管线
                    case "1": strwhere = strwhere + " and (DTUCode='" + Content + "' or PumpJZName like '%" + Content + "%')"; break; //泵站
                    case "2": ; break; //阀门
                    case "3": ; break; //流量
                    case "4": ; break; //水厂
                    case "5": ; break; //水源
                    case "6": ; break; //大表
                    case "7": strwhere = strwhere + " and (FDTUCode='" + Content + "' or FName like '%" + Content + "%')"; break; //压力
                    case "8": strwhere = strwhere + " and (FDTUCode='" + Content + "' or FName like '%" + Content + "%')"; break; //调峰
                    case "9": ; break; //水质
                    case "10": ; break; //加压站
                }
            }
            switch (Type)
            {
                case "0": ; break; //管线
                case "1": sql = "select id,DTUCode as FDTUCode,PumpJZName as FName from Panda_PumpJZ where FIsDelete=0  " + strwhere;
                    jsonName = "[\"ID\",\"编码\",\"名称\"]"; break; //泵站
                case "2": ; break; //阀门
                case "3": ; break; //流量
                case "4": ; break; //水厂
                case "5": ; break; //水源
                case "6": ; break; //大表
                case "7": sql = "select id,FDTUCode,FName as FName,FMpaUp,FMpaDown from BASE_YALI where 1=1 " + strwhere;
                    jsonName = "[\"ID\",\"编码\",\"名称\",\"压力上限\",\"压力下限\"]"; break; //压力
                case "8": sql = "select id,FDTUCode,FName as FName from BASE_TIAOFENG where 1=1 " + strwhere;
                    jsonName = "[\"ID\",\"编码\",\"名称\"]"; break; //调峰
                case "9": ; break; //水质
                case "10": ; break; //加压站
            }

            if (!sql.Equals(""))
            {
                //DataTable data = Dal.DBUtil.SelectDataTable(sql);
                //分页
                int pageIndex = Convert.ToInt32(HttpContext.Current.Request["pageIndex"]);
                int pageSize = Convert.ToInt32(HttpContext.Current.Request["pageSize"]);
                ////字段排序
                //String sortField = HttpContext.Current.Request["sortField"];
                //String sortOrder = HttpContext.Current.Request["sortOrder"];
                //业务层：数据库操作
                Hashtable result = Bll.BaseToMarkerbll.Search(sql, pageIndex, pageSize, "FCreateDate", "desc");
                string jsonData = PluSoft.Utils.JSON.Encode(result);

                json = "{\"data\":["
                                    + "{\"jsonName\":" + jsonName + " }, "
                                    + "{\"jsonData\":" + jsonData + " } "
                           + "]}";
            }
            else
            {
                json = "[]";
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