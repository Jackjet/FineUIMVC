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
    /// Bas_Province 的摘要说明
    /// </summary>
    public class PumpManagerService : IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
                case "searchall":
                    SearchAll();
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
            string fnumber = HttpContext.Current.Request["FNumber"];
            if (fnumber != null && fnumber != "")
            {
                sqland += " and FNumber like '%" + fnumber + "%'";
            }

            string fname = HttpContext.Current.Request["FName"];
            if (fname != null && fname != "")
            {
                sqland += " and FName like '%" + fname + "%'";
            }



            //数据权限
            string isadmin = Sys_UserService.GetSession("FUserType").ToString();
            if (isadmin != "1")
            {   
                string customerid = Sys_UserService.GetSession("FCustomerID").ToString();
                sqland += " and FCustomerID = '" + customerid + "'";
            }


            //分页
            int pageIndex = Convert.ToInt32(HttpContext.Current.Request["pageIndex"]);
            int pageSize = Convert.ToInt32(HttpContext.Current.Request["pageSize"]);
            //字段排序
            String sortField = HttpContext.Current.Request["sortField"];
            String sortOrder = HttpContext.Current.Request["sortOrder"];
            //业务层：数据库操作
            //Hashtable result = new TestDB().SearchEmployees(key, pageIndex, pageSize, sortField, sortOrder);
            Hashtable result = Bll.PumpManagerBll.Search(sqland, pageIndex, pageSize, sortField, sortOrder);

            result["CurrentUserName"] = Sys_UserService.GetSession("FName");

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
            Hashtable has = Bll.PumpManagerBll.Get(id);
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
                        string Identity = Bll.PumpManagerBll.Insert(row);
                        result = Identity;
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
                        Bll.PumpManagerBll.Update(row);
                        result = id;
                    }
                    catch (Exception e)
                    {
                        result = "0";
                    }
                }

                //消防
                SetOutNumber(result, "消防", row["FFireOutNum"].ToString());
                //喷淋
                SetOutNumber(result, "喷淋", row["FSprayOutNum"].ToString());
                //生活
                SetOutNumber(result, "生活", row["FLifeOutNum"].ToString());
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
                    Bll.PumpManagerBll.Delete(id);
                    result = "1";
                }
                catch (Exception e)
                {
                    result = "0";
                }                   
               
            }

            HttpContext.Current.Response.Write("{\"result\":\"" + result + "\"}");
        }


        /*----------------自定义------------------*/
        public void SearchAll()
        {
            string sqland = "";
            string fnumber = HttpContext.Current.Request["FNumber"];
            if (fnumber != null && fnumber != "")
            {
                sqland += " and FNumber like '%" + fnumber + "%'";
            }

            string fname = HttpContext.Current.Request["FName"];
            if (fname != null && fname != "")
            {
                sqland += " and FName like '%" + fname + "%'";
            }

             //数据权限
            string isadmin = Sys_UserService.GetSession("FUserType").ToString();
            if (isadmin != "1")
            {
                //数据权限
                string customerid = Sys_UserService.GetSession("FCustomerID").ToString();
                sqland += " and FCustomerID = '" + customerid + "'";
            }

            String sql = "select * from  PumpManager where  1=1 and PumpManager.FDeleted !='1'   " + sqland + " order by FSortIndex asc";
            DataTable dt = Dal.DBUtil.SelectDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["FIsOnline"].ToString() == "1")
                {
                    string id = dt.Rows[i]["ID"].ToString();

                    String sqlalarm = "select * from  T_Alarm where FPumpID='" + id + "' and FStatus='1' ";
                    DataTable dtalarm = Dal.DBUtil.SelectDataTable(sqlalarm);

                    if (dtalarm.Rows.Count > 0)
                        dt.Rows[i]["FIsOnline"] = "2";

                }
            }


            Hashtable result = new Hashtable();

            if (dt.Rows.Count > 0)
            {
                ArrayList data = Dal.DBUtil.DataTable2ArrayList(dt);
                result["data"] = data;
                result["total"] = dt.Rows.Count;
                result["oncount"] = dt.Select("FIsOnline='1' or FIsOnline='2'").Length.ToString();
                result["offcount"] = dt.Select("FIsOnline='0'").Length.ToString();

            }
            else
            {
                result["total"] = 0;
                result["oncount"] = 0;
                result["offcount"] = 0;
            }

            result["CurrentUserName"] = Sys_UserService.GetSession("FName");

            String json = PluSoft.Utils.JSON.Encode(result);
            HttpContext.Current.Response.Write(json);
        }

        public void SetOutNumber(string id,string outtype,string number)
        {
             YM.Data.SqlScope ss = Dal.Sqler.Instance();
             using (ss.EnterQuery())
             {
                 //消防
                 if (number != "")
                 {
                     int outnum = int.Parse(number);

                     //删除实时报警信息
                     string select = "select *  from T_Out_Set where FPumpID='" + id + "' and FType='" + outtype + "' and FIndex > '" + outnum + "'";
                     DataTable dt = ss.ExecuteDataTable(select);

                     for (int i = 0; i < dt.Rows.Count; i++)
                     {
                         string name = dt.Rows[i]["FName"].ToString();
                         string sqldelete = @"delete from T_Alarm where FPumpID = '" + id + "' and  (T_Alarm.FContent ='" + name + "出水压力超高报警' or T_Alarm.FContent ='" + name + "出水压力超低报警')";
                         ss.ExecuteNonQuery(sqldelete);
                     }

                     //删除管道设置
                     string del = "delete from T_Out_Set where FPumpID='" + id + "' and FType='" + outtype + "' and FIndex > '" + outnum + "'";
                     ss.ExecuteNonQuery(del);

                     //增加多出的管道
                     for (int i = 0; i < outnum; i++)
                     {
                         int num = i + 1;

                         string sqlselect = "select * from T_Out_Set where FPumpID='" + id + "' and FType='" + outtype + "' and FIndex ='" + num + "'";
                         DataTable dtselect = ss.ExecuteDataTable(sqlselect);
                         if (dtselect.Rows.Count == 0)
                         {
                             string sqlinsert = "INSERT INTO [dbo].T_Out_Set([FPumpID],[FType],[FIndex],[FName])VALUES('" + id + "','" + outtype + "','" + num + "','" + outtype + "管道" + num + "号')";
                             ss.ExecuteNonQuery(sqlinsert);
                         }
                     }
                 }
             }

        }

    
    }
}