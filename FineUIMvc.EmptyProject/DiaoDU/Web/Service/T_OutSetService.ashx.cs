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
    /// T_OutSetService 的摘要说明
    /// </summary>
    public class T_OutSetService : IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
            string sqland = "";
            string FPumpID = HttpContext.Current.Request["FPumpID"];
            if (FPumpID != null && FPumpID != "")
            {
                sqland += " and FPumpID = '" + FPumpID + "'";
            }

            string FType = HttpContext.Current.Request["FType"];
            if (FType != null && FType != "")
            {
                sqland += " and FType = '" + FType + "'";
            }

            string FIndex = HttpContext.Current.Request["FIndex"];
            if (FIndex != null && FIndex != "")
            {
                sqland += " and FIndex = '" + FIndex + "'";
            }


            //分页
            int pageIndex = Convert.ToInt32(HttpContext.Current.Request["pageIndex"]);
            int pageSize = Convert.ToInt32(HttpContext.Current.Request["pageSize"]);
            //字段排序
            String sortField = HttpContext.Current.Request["sortField"];
            String sortOrder = HttpContext.Current.Request["sortOrder"];
            //业务层：数据库操作
            Hashtable result = Bll.T_OutSetBll.Search(sqland, pageIndex, pageSize, sortField, sortOrder);  

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
            Hashtable has = Bll.T_OutSetBll.Get(id);
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
                    row["createtime"] = DateTime.Now;

                    try
                    {
                        Bll.T_OutSetBll.Insert(row);
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
                        Bll.T_OutSetBll.Update(row);
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
                    Bll.T_OutSetBll.Delete(id);
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