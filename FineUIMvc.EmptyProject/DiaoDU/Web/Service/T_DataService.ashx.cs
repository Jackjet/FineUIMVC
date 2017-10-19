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
    /// T_DataService 的摘要说明
    /// </summary>
    public class T_DataService : IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
             String tbname = "T_Data" + DateTime.Now.ToString("yyyyMM");


             //查询条件  
             string sqland = "";
             string begDateTime = HttpContext.Current.Request["begDateTime"];
             if (begDateTime != null && begDateTime != "")
             {
                 tbname = "T_Data" + DateTime.Parse(begDateTime).ToString("yyyyMM");    
                 sqland += " and " + tbname + ".FCreateDate >= '" + begDateTime + "' ";
             }   

             string endDateTime = HttpContext.Current.Request["endDateTime"];
             if (endDateTime != null && endDateTime != "")
             {
                 sqland += " and " + tbname + ".FCreateDate <= '" + endDateTime + "' ";
             }     

            string fpumpid = HttpContext.Current.Request["FPumpID"];
            if (fpumpid != null && fpumpid != "")
            {
                sqland += " and " + tbname + ".FPumpID = '" + fpumpid + "'";
            }      

            string FName = HttpContext.Current.Request["FName"];
            if (FName != null && FName != "")
            {
                sqland += " and PumpManager.FName like  '%" + FName + "%'";
            }                

            //分页
            int pageIndex = Convert.ToInt32(HttpContext.Current.Request["pageIndex"]);
            int pageSize = Convert.ToInt32(HttpContext.Current.Request["pageSize"]);
            int StartRecord = pageIndex * pageSize;

            string sql = "SELECT " + tbname + ".* "+
                  ",PumpManager.FNumber           "+  
                  ",PumpManager.FIsOnline         "+  
                  ",PumpManager.FName             "+  
                  ",PumpManager.FSortIndex        "+  
                  ",PumpManager.FCustomerID       "+  
                  ",PumpManager.FPumpType         "+  
                  ",PumpManager.FLifeMajorPumpNum  "+  
                  ",PumpManager.FLifeAuxPumpNum     "+  
                  ",PumpManager.FFireMajorPumpNum    "+  
                  ",PumpManager.FFireAuxPumpNum      "+  
                  ",PumpManager.FSprayMajorPumpNum   "+  
                  ",PumpManager.FSprayAuxPumpNum     "+  
                  ",PumpManager.FLifeOutNum         "+  
                  ",PumpManager.FFireOutNum       "+  
                  ",PumpManager.FSprayOutNum      "+  
                  ",PumpManager.FDewaterPumpNum   "+  
                  ",PumpManager.FLatAndLong       "+  
                  ",PumpManager.FMapAddress       "+  
                  ",PumpManager.FAddress          "+  
                  ",PumpManager.FDeleted          "+  
                  ",PumpManager.FURL FROM " + tbname + " INNER JOIN  PumpManager ON " + tbname + ".FPumpID = PumpManager.ID where 1=1 " + sqland + "  order by " + tbname + ".FCreateDate desc  offset " + StartRecord + " rows fetch next " + pageSize + " rows only;";
            DataTable dt = Dal.DBUtil.SelectDataTable(sql);
            ArrayList data = Dal.DBUtil.DataTable2ArrayList(dt);

            string sqlcount = @"SELECT count (*)  FROM " + tbname + " INNER JOIN  PumpManager ON " + tbname + ".FPumpID = PumpManager.ID where 1=1 " + sqland + "";
            Int64 count = Dal.DBUtil.ExecuteScalar64(sqlcount);

            Hashtable result = new Hashtable();
            result["data"] = data;
            result["total"] = count;         


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
            Hashtable has = Bll.T_DataBll.Get(id);
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
                        Bll.T_DataBll.Insert(row);
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
                        Bll.T_DataBll.Update(row);
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
                    Bll.T_DataBll.Delete(id);
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