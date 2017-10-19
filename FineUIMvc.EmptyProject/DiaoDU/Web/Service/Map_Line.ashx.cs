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
    /// Map_Line 的摘要说明
    /// </summary>
    public class Map_Line : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string method = context.Request["method"].ToString().ToLower();

            switch (method)
            {
                case "searchline":
                    SearchLine();
                    break;
                case "insertline":
                    InsertLine();
                    break;
                case "updlinepro":
                    UpdLinePro();
                    break;
                case "delline":
                    DelLine();
                    break;
                default:

                    break;

            }
        }

        public void SearchLine()
        {
            string sql = "";
            string LineID = HttpContext.Current.Request["LineID"];
            string TempID = HttpContext.Current.Request["TempID"];
            if (LineID != "" && LineID != null)
            {
                sql = " and a.ID='" + LineID + "'";
            }
            else  if (TempID != "" && TempID != null)
            {
                sql = " and a.FMapTempID='" + TempID + "'";
            }
            DataTable dt = Bll.Map_LineBll.Search(sql);
            String json = "";
            if (dt.Rows.Count==0)
            {
                json = PluSoft.Utils.JSON.Encode("[]");
            }
            else
            {
                dt.Columns.Add("TB_AreaOverlay", typeof(object));
                for (int i = 0; i < dt.Rows.Count;i++ )
                {
                    DataTable dt_area = Bll.Map_AreaBll.SearchOverlay(" and FMapOverlayID='" + dt.Rows[i]["LineID"].ToString() + "' and FMapOverlayType='line'");
                    dt.Rows[i]["TB_AreaOverlay"] = dt_area;
                }
                json = PluSoft.Utils.JSON.Encode(dt);
            }

            HttpContext.Current.Response.Write(json);
        }
        public void InsertLine()
        {
            String json = "";
            string FName = HttpContext.Current.Request["FName"];
            string FMapTempID = HttpContext.Current.Request["FMapTempID"];
            string FLine = HttpContext.Current.Request["FLine"];
            string guid = Guid.NewGuid().ToString();
            Hashtable has1 = new Hashtable();
            Hashtable has2 = new Hashtable();
            has1["ID"] = guid;
            has1["FName"] = FName;
            has1["FMapTempID"] = FMapTempID; 
            has2["FMapLineID"] = guid;
            has2["FLine"] = FLine;
            int maxid = Convert.ToInt32(Dal.DBUtil.SelectDataTable("select isnull(max(ID),0) as maxId from Map_LineProperty").Rows[0]["maxId"].ToString())+1;
            has2["FAliasName"] = "Line"+maxid.ToString();
            try
            {
                Bll.Map_LineBll.InsertLine(has1);
                Bll.Map_LineBll.InsertLineProperty(has2);
                DataTable dt = Bll.Map_LineBll.Search(" and a.ID='" + guid + "'");
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
        public void UpdLinePro()
        {
            string result = "";
            string id = HttpContext.Current.Request["id"];                    //主键id
            string TableField = HttpContext.Current.Request["TableField"];  //表字段
            string text = HttpContext.Current.Request["text"];        //字段值
            Hashtable has = new Hashtable();
            has["FMapLineID"] = id;
            if (!TableField.Equals("FName"))
            {
                has[TableField] = text;
            }
            try
            {
                if (TableField.Equals("FName"))
                {
                    Hashtable has1 = new Hashtable();
                    has1["ID"] = id;
                    has1["FName"] = text;
                    Bll.Map_LineBll.UpdateLine(has1);
                }
                if (has.Count > 1)
                {
                    Bll.Map_LineBll.UpdateLineProperty(has);
                }
                result = "1";
            }
            catch (Exception e)
            {
                result = "0";
            }
            HttpContext.Current.Response.Write("{\"result\":\"" + result + "\"}");
        }

        public void DelLine()
        {
            string result = "";
            try
            {
                string FMapLineID = HttpContext.Current.Request["FMapLineID"];
                SqlParameter[] ParamList ={
                  Dal.DBUtil.MakeInParam("@FMapLineID",SqlDbType.NVarChar,200,FMapLineID),
                  Dal.DBUtil.MakeOutParam("@ReMsg",SqlDbType.VarChar,530)
                };
                string ReMsg = "";
                string RePorcedure = "";
                RePorcedure = Dal.DBUtil.runProcedure("delLine", ParamList, out ReMsg);
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