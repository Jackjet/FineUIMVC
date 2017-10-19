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
    /// Map_Marker 的摘要说明
    /// </summary>
    public class Map_Marker : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string method = context.Request["method"].ToString().ToLower();

            switch (method)
            {
                case "searchmarker":
                    SearchMarker();
                    break;
                case "insertmarker":
                    InsertMarker();
                    break;
                case "updmarkerpro":
                    UpdMarkerPro();
                    break;
                case "delmarker":
                    DelMarker();
                    break;
                default:

                    break;

            }
        }
        public void SearchMarker()
        {
            string sql = "";
            string MarkerID = HttpContext.Current.Request["MarkerID"];
            string TempID = HttpContext.Current.Request["TempID"];
            if (MarkerID != "" && MarkerID!=null)
            {
                sql = " and a.ID='" + MarkerID + "'";
            }
            else if (TempID != "" && TempID != null)
            {
                sql = " and a.FMapTempID='" + TempID + "'";
            }
            DataTable dt = Bll.Map_MarkerBll.Search(sql);
            String json = "";
            if (dt.Rows.Count == 0)
            {
                json = PluSoft.Utils.JSON.Encode("[]");
            }
            else
            {
                dt.Columns.Add("TB_AreaOverlay", typeof(object));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataTable dt_area = Bll.Map_AreaBll.SearchOverlay(" and FMapOverlayID='" + dt.Rows[i]["MarkerID"].ToString() + "' and FMapOverlayType='marker'");
                    dt.Rows[i]["TB_AreaOverlay"] = dt_area;
                }
                json = PluSoft.Utils.JSON.Encode(dt);
            }

            HttpContext.Current.Response.Write(json);
        }

        public void InsertMarker()
        {
            String json = "";
            string FName = HttpContext.Current.Request["FName"];
             string FMapTempID = HttpContext.Current.Request["FMapTempID"];
             string FMarker = HttpContext.Current.Request["FMarker"];
             string FType = HttpContext.Current.Request["FType"];
            string guid = Guid.NewGuid().ToString();
            Hashtable has1 = new Hashtable();
            Hashtable has2 = new Hashtable();
            has1["ID"] = guid;
            has1["FName"] = FName;
            has1["FMapTempID"] = FMapTempID; 
            has2["FMarkerID"] = guid;
            has2["FMarker"] = FMarker;
            has2["FType"] = FType;
            int maxid = Convert.ToInt32(Dal.DBUtil.SelectDataTable("select isnull(max(ID),0) as maxId from Map_MarkerProperty").Rows[0]["maxId"].ToString()) + 1;
            has2["FAliasName"] = "Marker" + maxid.ToString();
            try
            {
                Bll.Map_MarkerBll.InsertMarker(has1);
                Bll.Map_MarkerBll.InsertMarkerProperty(has2);
                DataTable dt = Bll.Map_MarkerBll.Search(" and a.ID='" + guid + "'");
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
        public void UpdMarkerPro()
        {
            string result = "";
            string id = HttpContext.Current.Request["id"];                    //主键id
            string TableField = HttpContext.Current.Request["TableField"];  //表字段
            string text = HttpContext.Current.Request["text"];        //字段值
            Hashtable has = new Hashtable();
            has["FMarkerID"] = id;
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
                    Bll.Map_MarkerBll.UpdateMarker(has1);
                }
                if (has.Count > 1)
                {
                    Bll.Map_MarkerBll.UpdateMarkerProperty(has);
                }
                result = "1";
            }
            catch (Exception e)
            {
                result = "0";
            }
            HttpContext.Current.Response.Write("{\"result\":\"" + result + "\"}");
        }

        public void DelMarker()
        {
            string result = "";
            try
            {
                string FMarkerID = HttpContext.Current.Request["FMarkerID"];
                SqlParameter[] ParamList ={
                  Dal.DBUtil.MakeInParam("@FMarkerID",SqlDbType.NVarChar,200,FMarkerID),
                  Dal.DBUtil.MakeOutParam("@ReMsg",SqlDbType.VarChar,530)
                };
                string ReMsg = "";
                string RePorcedure = "";
                RePorcedure = Dal.DBUtil.runProcedure("delMarker", ParamList, out ReMsg);
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