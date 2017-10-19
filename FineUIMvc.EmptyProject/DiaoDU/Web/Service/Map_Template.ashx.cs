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
    /// Map_Template 的摘要说明
    /// </summary>
    public class Map_Template : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string method = context.Request["method"].ToString().ToLower();

            switch (method)
            {
                case "searchtemp":
                    SearchTemp();
                    break;
                case "inserttemplate":
                    InsertTemplate();
                    break;
                case "updtemppro":
                    UpdTempPro();
                    break;
                case "deltemp":
                    DelTemp();
                    break;
                case "copytemp":
                    copyTemp();
                    break;
                default:

                    break;

            }
        }
        public void SearchTemp()
        {
            string TempID = HttpContext.Current.Request["TempID"];
            string sql = string.Empty;
            if(TempID!="" && TempID!=null)
            {
                sql = " and a.ID='" + TempID + "'";
            }
            DataTable dt = Bll.Map_TemplateBll.Search(sql);
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
        public void InsertTemplate()
        {
            String json = "";
            string FMapTempName = HttpContext.Current.Request["FMapTempName"];
            string guid = Guid.NewGuid().ToString();
            Hashtable has1 = new Hashtable();
            Hashtable has2 = new Hashtable();
            has1["id"] = guid;
            has1["FMapTempName"] = FMapTempName;
            has2["FMapTempID"] = guid;
            int maxid = Convert.ToInt32(Dal.DBUtil.SelectDataTable("select isnull(max(ID),0) as maxId from Map_TempProperty").Rows[0]["maxId"].ToString()) + 1;
            has2["FAliasName"] = "Template" + maxid.ToString();
            try
            {
                Bll.Map_TemplateBll.InsertTemplate(has1);
                Bll.Map_TemplateBll.InsertTempProperty(has2);
                DataTable dt = Bll.Map_TemplateBll.Search(" and a.ID='" + guid + "'");
                if (dt.Rows.Count == 0)
                {
                    json = PluSoft.Utils.JSON.Encode("[]");
                }
                else
                {
                    json = PluSoft.Utils.JSON.Encode(dt);
                }
            }
            catch (Exception e)
            {
                json = "[]";
            }
            HttpContext.Current.Response.Write("{\"result\":" + json + "}");
        }
        public void UpdTempPro()
        {
            string result = "";
            string id = HttpContext.Current.Request["id"];                    //主键id
            string TableField = HttpContext.Current.Request["TableField"];  //表字段
            string text = HttpContext.Current.Request["text"];        //字段值
            Hashtable has = new Hashtable();
            has["FMapTempID"] = id;
            if (!TableField.Equals("FMapTempName"))
            {
                has[TableField] = text;
            }
            try
            {
                if (TableField.Equals("FMapTempName"))
                {
                    Hashtable has1 = new Hashtable();
                    has1["id"] = id;
                    has1["FMapTempName"] = text;
                    Bll.Map_TemplateBll.UpdateTemp(has1);
                }
                if(has.Count>1)
                {
                    Bll.Map_TemplateBll.UpdateTempProperty(has);
                }
                result = "1";
            }
            catch (Exception e)
            {
                result = "0";
            }
            HttpContext.Current.Response.Write("{\"result\":\"" + result + "\"}");
        }

        public void DelTemp()
        {
            string result = "";
            try
            {
                string FMapTempID = HttpContext.Current.Request["FMapTempID"]; 
                SqlParameter[] ParamList ={
                  Dal.DBUtil.MakeInParam("@FMapTempID",SqlDbType.NVarChar,200,FMapTempID),
                  Dal.DBUtil.MakeOutParam("@ReMsg",SqlDbType.VarChar,530)
                };
                string ReMsg = "";
                string RePorcedure = "";
                RePorcedure = Dal.DBUtil.runProcedure("delTemp", ParamList, out ReMsg);
                if (ReMsg == "1")
                {
                    result = "1";
                }
                else
                {
                    result = "0";
                }
            }
            catch
            {
                result = "0";
            }
            HttpContext.Current.Response.Write("{\"result\":\"" + result + "\"}");
        }

        public void copyTemp()
        {
            string result = "";
            try
            {
                string FMapTempID = HttpContext.Current.Request["FMapTempID"];
                SqlParameter[] ParamList ={
                  Dal.DBUtil.MakeInParam("@FMapTempID",SqlDbType.NVarChar,50,FMapTempID),
                  Dal.DBUtil.MakeOutParam("@ReMsg",SqlDbType.VarChar,530)
                };
                string ReMsg = "";
                string RePorcedure = "";
                RePorcedure = Dal.DBUtil.runProcedure("copyTemp", ParamList, out ReMsg);
                if (ReMsg == "1")
                {
                    result = "1";
                }
                else
                {
                    result = "0";
                }
            }
            catch
            {
                result = "0";
            }
            HttpContext.Current.Response.Write("{\"result\":\"" + result + "\"}");
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