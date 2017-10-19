using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Web.SessionState;
using System.Text;
using FineUIMvc.PumpMVC.Controllers;
using FineUIMvc.PumpMVC.DAL;

namespace Water.Web.Service
{
    /// <summary>
    /// Sys_UserService 的摘要说明
    /// </summary>
    public class Sys_UserService : IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
                case "checklogin":
                    CheckLogin();
                    break;
                case "mangeruser":
                    MangeruUser();
                    break;
                case "getcurrentuser":
                    GetCurrentUser();
                    break;
                case "checkwxlogin":
                    CheckWXLogin();
                    break;
                case "checkmvclogin":
                    CheckMVCLogin(context);
                    break;
                case "getmaptempid":
                    GetMapTempID();
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
            string FName = HttpContext.Current.Request["FName"];
            if (FName != null && FName != "")
            {
                sqland += " and SYS_USER.FName like '%" + FName + "%'";
            }

            //数据权限
            string isadmin = Sys_UserService.GetSession("FUserType").ToString();
            if (isadmin != "1")
            {
                string customerid = Sys_UserService.GetSession("FCustomerID").ToString();
                sqland += " and SYS_USER.FCustomerID = '" + customerid + "' ";
            }


            //分页
            int pageIndex = Convert.ToInt32(HttpContext.Current.Request["pageIndex"]);
            int pageSize = Convert.ToInt32(HttpContext.Current.Request["pageSize"]);
            //字段排序
            String sortField = HttpContext.Current.Request["sortField"];
            String sortOrder = HttpContext.Current.Request["sortOrder"];
            //业务层：数据库操作
            //Hashtable result = new TestDB().SearchEmployees(key, pageIndex, pageSize, sortField, sortOrder);
            Hashtable result = Bll.Sys_UserBll.Search(sqland, pageIndex, pageSize, sortField, sortOrder);

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
            Hashtable has = Bll.Sys_UserBll.Get(id);
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
                        Bll.Sys_UserBll.Insert(row);
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
                        Bll.Sys_UserBll.Update(row);
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
                    Bll.Sys_UserBll.Delete(id);
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
        public void CheckLogin()
        {
            string result = "";

            String _FName = HttpContext.Current.Request["FName"];
            String _FPassword = HttpContext.Current.Request["FPassword"];

            //String sql = "select * from Sys_User where FName='" + _FName + "' and FPassword='" + _FPassword + "'";
            String sql = "select * from Panda_UserInfo where UserName='" + _FName + "' and UserPwd=substring(sys.fn_sqlvarbasetostr(HashBytes('MD5','" + _FPassword + "')),3,32)";
            DataTable dt = Dal.DBUtil.SelectDataTable(sql);

            if (dt.Rows.Count == 1)
            {
                DataRow dr = dt.Rows[0];
                LogonSuccess(dr);

                string a = GetSession("FCustomerID").ToString();

                result = "1";
            }
            else
                result = "0";

            HttpContext.Current.Response.Write("{\"result\":\"" + result + "\"}");
        }
        public void MangeruUser()
        {
            string result = "";


            String sql = "select * from Sys_User where FName='admin'";
            DataTable dt = Dal.DBUtil.SelectDataTable(sql);

            if (dt.Rows.Count == 1)
            {
                DataRow dr = dt.Rows[0];
                LogonSuccess(dr);

                string a = GetSession("FCustomerID").ToString();

                result = "1";
            }
            else
                result = "0";

            HttpContext.Current.Response.Write("{\"result\":\"" + result + "\"}");
        }

        public void CheckWXLogin()
        {
            string result = "";

            String _pctoken = HttpContext.Current.Request["pctoken"];

            String sql = @"select b.* from SYS_USER_WEIXIN a
                           inner join SYS_USER b on a.FUserKey=b.FName
                            where FPCToken='" + _pctoken + "' and FDelete=0 and FDeleted=0";
            DataTable dt = Dal.DBUtil.SelectDataTable(sql);

            if (dt.Rows.Count == 1)
            {
                DataRow dr = dt.Rows[0];
                LogonSuccess(dr);

                string a = GetSession("FCustomerID").ToString();

                result = "1";
            }
            else
                result = "0";

            HttpContext.Current.Response.Write("{\"result\":\"" + result + "\"}");
        }

        public void CheckMVCLogin(HttpContext context)
        {
            string result = "";
            if (context.Session["UserID"] != null && Convert.ToInt32(context.Session["UserID"]) > 0)
            {
                result = "1";
            }
            else
            {
                result = "0";
            }
            HttpContext.Current.Response.Write("{\"result\":\"" + result + "\"}");
        }

        public void GetMapTempID()
        {
            StringBuilder str = new StringBuilder();
            try
            {
                if (HttpContext.Current.Session["UserID"] != null && Convert.ToInt32(HttpContext.Current.Session["UserID"]) > 0)
                {
                    int UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
                    string sql = "select FMapTempID from Panda_UserInfo a,Panda_Customer b where a.FCustomerID=b.ID and a.ID=" + UserID;
                    DataTable dt= publicDal.TableSearch(sql);
                    if(dt.Rows.Count>0)
                    {
                        string json = "[{\"FMapTempID\":\"" + dt.Rows[0]["FMapTempID"].ToString() + "\"}]";
                        str = BaseController.successMsg("成功", "true",json);
                    }
                    else
                    {
                        str = BaseController.successMsg("该用户没有地图模板", "false");
                    }
                }
                else
                {
                    str = BaseController.successMsg("用户登录失效，请重新登陆", "false");
                }
            }
            catch (Exception e)
            {
                str.Clear();
                string msg = "失败," + e.Message;
                str = BaseController.successMsg(msg, "false");
            }
            HttpContext.Current.Response.Write(str);
        }


        public void GetCurrentUser()
        {
            HttpContext.Current.Response.Write("{\"ID\":\"" + GetSession("ID") + "\",\"FName\":\"" + GetSession("FName") + "\",\"FCustomerID\":\"" + GetSession("FCustomerID") + "\"}");
        }



        /// <summary>
        /// 用户登录成功的后续操作，设置Session和Cookies
        /// </summary>
        public static void LogonSuccess(DataRow user)
        {
            //设置SESSION
            HttpContext.Current.Session["UserRow"] = user;

            HttpContext.Current.Session["ID"] = user["ID"].ToString();
            //HttpContext.Current.Session["FName"] = user["FName"].ToString();
            HttpContext.Current.Session["FName"] = user["UserName"].ToString();
            //HttpContext.Current.Session["FUserType"] = user["FUserType"].ToString();               
            //HttpContext.Current.Session["FCustomerID"] = user["FCustomerID"].ToString();
            HttpContext.Current.Session["FUserType"] = 2;
            HttpContext.Current.Session["FCustomerID"] = 1;

        }

        public static string GetSession(string strName)
        {
            if (HttpContext.Current.Session != null && HttpContext.Current.Session[strName] != null)
                return HttpContext.Current.Session[strName].ToString();
            return "";
        }
    
    
    }
}