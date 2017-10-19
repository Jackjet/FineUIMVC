using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Water.Web.Service
{
    /// <summary>
    /// Bas_Province 的摘要说明
    /// </summary>
    public class Bas_ProvinceService : IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
                case "get":
                    Get();
                    break;
                case "save":
                    Save();
                    break;
                case "remove":
                    Remove();
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

        /// <summary>
        /// 
        /// </summary>

        /// <summary>
        /// 
        /// </summary>
        public void Search()
        {
            //查询条件
            string key = HttpContext.Current.Request["key"];
            //分页
            int pageIndex = Convert.ToInt32(HttpContext.Current.Request["pageIndex"]);
            int pageSize = Convert.ToInt32(HttpContext.Current.Request["pageSize"]);
            //字段排序
            String sortField = HttpContext.Current.Request["sortField"];
            String sortOrder = HttpContext.Current.Request["sortOrder"];
            //业务层：数据库操作
            //Hashtable result = new TestDB().SearchEmployees(key, pageIndex, pageSize, sortField, sortOrder);
            Hashtable result = Bll.Bas_ProvinceBll.Search(key, pageIndex, pageSize, sortField, sortOrder);


            //JSON
            String json = PluSoft.Utils.JSON.Encode(result);
            HttpContext.Current.Response.Write(json);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Get()
        {
            String id = HttpContext.Current.Request["id"];
            Hashtable has = Bll.Bas_ProvinceBll.Get(id);
            String json = PluSoft.Utils.JSON.Encode(has);
            HttpContext.Current.Response.Write(json);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
            string result = "";
            String json = HttpContext.Current.Request["data"];
            ArrayList rows = (ArrayList)PluSoft.Utils.JSON.Decode(json);
            foreach (Hashtable row in rows)
            {
                String id = row["id"] != null ? row["id"].ToString() : "";

                if (id == "")       //新增：id为空，或_state为added
                {
                    row["FCreateDate"] = DateTime.Now;

                    try
                    {
                        Bll.Bas_ProvinceBll.Insert(row);
                        result = "1";
                    }
                    catch (Exception e)
                    {
                        result = "0";
                    }        
                 
                }

                else if (id != "") //更新：_state为空或modified
                {
                    try
                    {
                        Bll.Bas_ProvinceBll.Update(row);
                        result = "1";
                    }
                    catch (Exception e)
                    {
                        result = "0";
                    }       

                   
                }
            }
            HttpContext.Current.Response.Write("{\"result\":\"" + result + "\"}");
        }
        /// <summary>
        /// 
        /// </summary>
        public void Remove()
        {
            string result = "";
            String idStr = HttpContext.Current.Request["id"];
            if (String.IsNullOrEmpty(idStr)) return;
            String[] ids = idStr.Split(',');
            for (int i = 0, l = ids.Length; i < l; i++)
            {
                string id = ids[i];
                try
                {
                    Bll.Bas_ProvinceBll.Delete(id);
                    result = "1";
                }
                catch (Exception e)
                {
                    result = "0";
                }                   
               
            }

            HttpContext.Current.Response.Write("{\"result\":\"" + result + "\"}");
        }
    }
}