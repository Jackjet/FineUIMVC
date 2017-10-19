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
    /// Map_Area 的摘要说明
    /// </summary>
    public class Map_Area : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string method = context.Request["method"].ToString().ToLower();

            switch (method)
            {
                case "searcharea":
                    SearchArea();
                    break;
                case "insertarea":
                    InsertArea();
                    break;
                case "updareapro":
                    UpdAreaPro();
                    break;
                case "delarea":
                    DelArea();
                    break;
                case "insertareaoverlay":
                    InsertAreaOverlay();
                    break;
                default:

                    break;

            }
        }

        public void SearchArea()
        {
            string sql = "";
            string AreaID = HttpContext.Current.Request["AreaID"];
            string TempID = HttpContext.Current.Request["TempID"];
            if (AreaID != "" && AreaID != null)
            {
                sql = " and a.ID='" + AreaID + "'";
            }
            else if (TempID != "" && TempID != null)
            {
                sql = " and a.FMapTempID='" + TempID + "'";
            }
            DataTable dt = Bll.Map_AreaBll.Search(sql);
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
        public void InsertArea()
        {
            String json = "";
            string FName = HttpContext.Current.Request["FName"];
            string FMapTempID = HttpContext.Current.Request["FMapTempID"];
            string FArea = HttpContext.Current.Request["FArea"];
            string FAreaType = HttpContext.Current.Request["FAreaType"];
            string guid = Guid.NewGuid().ToString();
            Hashtable has1 = new Hashtable();
            Hashtable has2 = new Hashtable();
            has1["ID"] = guid;
            has1["FName"] = FName;
            has1["FMapTempID"] = FMapTempID;
            has2["FMapAreaID"] = guid;
            has2["FArea"] = FArea;
            has2["FAreaType"] = FAreaType;
            int maxid = Convert.ToInt32(Dal.DBUtil.SelectDataTable("select isnull(max(ID),0) as maxId from Map_AreaProperty").Rows[0]["maxId"].ToString()) + 1;
            has2["FAliasName"] = "Area" + maxid.ToString();
            try
            {
                Bll.Map_AreaBll.InsertArea(has1);
                Bll.Map_AreaBll.InsertAreaProperty(has2);
                DataTable dt = Bll.Map_AreaBll.Search(" and a.ID='" + guid + "'");
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
        public void InsertAreaOverlay()
        {
            string result = "";
            string FMapTempID = HttpContext.Current.Request["FMapTempID"];
            string FMapAreaID = HttpContext.Current.Request["FMapAreaID"];
            string FMapOverlayID = HttpContext.Current.Request["FMapOverlayID"];
            string FMapOverlayType = HttpContext.Current.Request["FMapOverlayType"];

            try
            {
                Bll.Map_AreaBll.DeleteAreaOverlay(FMapOverlayID, FMapTempID);

                Hashtable has = new Hashtable();
                string[] AreaID = FMapAreaID.Split(',');
                for (int i = 0; i < AreaID.Length; i++)
                {
                    has["FMapTempID"] = FMapTempID;
                    has["FMapAreaID"] = AreaID[i];
                    has["FMapOverlayID"] = FMapOverlayID;
                    has["FMapOverlayType"] = FMapOverlayType;
                    has["FCreateDate"] = DateTime.Now;
                    Bll.Map_AreaBll.InsertAreaOverlay(has);
                }

                result = "1";
            }
            catch (Exception e)
            {
                result = "0";
            }

            HttpContext.Current.Response.Write("{\"result\":\"" + result + "\"}");
        }

        public void UpdAreaPro()
        {
            string result = "";
            string id = HttpContext.Current.Request["id"];                    //主键id
            string TableField = HttpContext.Current.Request["TableField"];  //表字段
            string text = HttpContext.Current.Request["text"];        //字段值
            Hashtable has = new Hashtable();
            has["FMapAreaID"] = id;
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
                    Bll.Map_AreaBll.UpdateArea(has1);
                }
                if (has.Count > 1)
                {
                    Bll.Map_AreaBll.UpdateAreaProperty(has);
                }
                result = "1";
            }
            catch (Exception e)
            {
                result = "0";
            }
            HttpContext.Current.Response.Write("{\"result\":\"" + result + "\"}");
        }

        public void DelArea()
        {
            string result = "";
            try
            {
                string FMapAreaID = HttpContext.Current.Request["FMapAreaID"];
                SqlParameter[] ParamList ={
                  Dal.DBUtil.MakeInParam("@FMapAreaID",SqlDbType.NVarChar,200,FMapAreaID),
                  Dal.DBUtil.MakeOutParam("@ReMsg",SqlDbType.VarChar,530)
                };
                string ReMsg = "";
                string RePorcedure = "";
                RePorcedure = Dal.DBUtil.runProcedure("delArea", ParamList, out ReMsg);
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