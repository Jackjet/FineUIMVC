using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FineUIMvc.PumpMVC.Controllers;
using FineUIMvc.PumpMVC.DAL;
using FineUIMvc.PumpMVC.Models;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Text;
using System.Net;

namespace FineUIMvc.PumpMVC.Areas.BasicInfo.Controllers
{
    [Authorize]
    public class BASE_TIAOFENGController : BaseController
    {
        private DBController db = new DBController();

        #region 调峰列表页面
        [MyAuth(MenuPower = "CoreTiaoFengView")]
        public ActionResult Index()
        {
            ViewBag.CoreTiaoFengNew = CheckPower("CoreTiaoFengNew");
            ViewBag.CoreTiaoFengDelete = CheckPower("CoreTiaoFengDelete");
            ViewBag.CoreTiaoFengEdit = CheckPower("CoreTiaoFengEdit");
            Hashtable table = BASE_TIAOFENGDal.SearchTF(0, 20, "a.FCreateDate", "DESC", getPowerConst("tiaofeng"));
            ViewBag.Grid1DataSource = table["data"];
            ViewBag.Grid1RecordCount = Int32.Parse(table["total"].ToString());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grid1_PageIndexChanged(JArray Grid1_fields, int Grid1_pageIndex, int gridPageSize, string searchMessage)
        {
            var Grid1 = UIHelper.Grid("Grid1");

            string sql = string.Empty;
            if (!searchMessage.Equals(""))
            {
                sql = sql + " and FName like '%" + searchMessage + "%'";
            }
            sql = sql + getPowerConst("tiaofeng");
            Hashtable table = BASE_TIAOFENGDal.SearchTF(Grid1_pageIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
            Grid1.DataSource(table["data"], Grid1_fields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreTiaoFengDelete")]
        public ActionResult Grid1_Delete(JArray selectedRows, JArray Grid1_fields, int gridIndex, int gridPageSize)
        {
            string values = "";
            for (int i = 0; i < selectedRows.Count; i++)
            {
                if (i != selectedRows.Count - 1)
                {
                    values += "'" + selectedRows[i] + "',";
                }
                else
                {
                    values += "'" + selectedRows[i] + "'";
                }
            }

            Hashtable hasData = new Hashtable();
            hasData["id"] = values;
            hasData["FIsDelete"] = 1;
            hasData["FDelDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ssss"));
            hasData["FDelUser"] = GetIdentityName();
            BASE_TIAOFENGDal.DeleteList(hasData);
            Bll.Map_MarkerBll.DeleteMarker(8, values);
            UpdateGrid(Grid1_fields, gridIndex, gridPageSize);
            return UIHelper.Result();
        }

        private void UpdateGrid(JArray Grid1_fields, int gridIndex, int gridPageSize)
        {
            var Grid1 = UIHelper.Grid("Grid1");

            Hashtable table = BASE_TIAOFENGDal.SearchTF(gridIndex, gridPageSize, "a.FCreateDate", "DESC", getPowerConst("tiaofeng"));
            Grid1.DataSource(table["data"], Grid1_fields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyCustomPostBack(string type, JArray gridFields, JObject typeParams, int gridIndex, int gridPageSize)
        {
            var Grid1 = UIHelper.Grid("Grid1");
            string sql = string.Empty;
            var ttbSearch = UIHelper.TwinTriggerBox("ttbSearchMessage");
            if (type == "trigger1")
            {
                ttbSearch.Text(String.Empty);
                ttbSearch.ShowTrigger1(false);
            }
            else if (type == "trigger2")
            {
                ttbSearch.ShowTrigger1(true);
                var triggerValue = typeParams.Value<string>("triggerValue");
                sql = " and FName like '%" + triggerValue + "%'";
            }
            sql = sql + getPowerConst("tiaofeng");
            Hashtable table = BASE_TIAOFENGDal.SearchTF(gridIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
            Grid1.DataSource(table["data"], gridFields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));

            return UIHelper.Result();
        }
        #endregion

        #region 调峰新增修改框架图
        [MyAuth(MenuPower = "CoreTiaoFengNew")]
        public ActionResult TF_Diagram(string type)
        {
            string url = string.Empty;
            string id = Request["tiaofId"];
            if (type == "add")
            {
                url = Url.Content("~/BasicInfo/BASE_TIAOFENG/TFBasic_new");
                ViewBag.pageType = "add";
                ViewBag.TreeNodeShow = false;
            }
            else
            {
                url = Url.Content("~/BasicInfo/BASE_TIAOFENG/TFBasic_edit?tiaofId=" + id);
                ViewBag.pageType = "edit";
                ViewBag.tiaofId = id;
                ViewBag.TreeNodeShow = true;
            }
            PageContext.RegisterStartupScript("F.ui.panelCenterRegion.setIFrameUrl('" + url + "');");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Tree1_NodeClick(string nodeId, string tiaofId)
        {
            string url = string.Empty;
            switch (nodeId)
            {
                case "basic": url = Url.Content("~/BasicInfo/BASE_TIAOFENG/TFBasic_edit?tiaofId=" + tiaofId); break;
                case "tiaofArchives": url = Url.Content("~/OpenWindow/BaseDA/PumpArchives?baseId=" + tiaofId + "&pageType=tiaoF"); break;
            }
            PageContext.RegisterStartupScript("F.ui.panelCenterRegion.setIFrameUrl('" + url + "');");
            return UIHelper.Result();
        }
        #endregion

        #region 新增调峰基本信息页面
        [MyAuth(MenuPower = "CoreTiaoFengNew")]
        public ActionResult TFBasic_new()
        {
            ViewBag.ddlFMaterialDataSource = sys_dictDal.SearchDDL(" and FDictID=141");
            ViewBag.ddlFCaliberDataSource = sys_dictDal.SearchDDL(" and FDictID=142");
            ViewBag.ddlFEQuiTypeDataSource = sys_dictDal.SearchDDL(" and FDictID=143");
            ViewBag.ddlFInstallModeDataSource = sys_dictDal.SearchDDL(" and FDictID=144");
            ViewBag.ddlFCommunicationModeDataSource = sys_dictDal.SearchDDL(" and FDictID=145");
            ViewBag.ddlFReadMeterModeDataSource = sys_dictDal.SearchDDL(" and FDictID=146");
            ViewBag.ddlFBuriedModeDataSource = sys_dictDal.SearchDDL(" and FDictID=147");
            ViewBag.ddlFEQuiStateDataSource = sys_dictDal.SearchDDL(" and FDictID=148");
            if (GetUserType().Equals("3"))  //如果登录用户是客户
            {
                ViewBag.CustomerName = GetUserCustomerName();
                ViewBag.CustomerID = GetUserCustomer();
                ViewBag.MapTempID = GetCustomerMapTempID();
                ViewBag.ReadOnly = true;
            }
            else
            {
                ViewBag.CustomerName = "";
                ViewBag.CustomerID = "0";
                ViewBag.MapTempID = "";
                ViewBag.ReadOnly = false;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreTiaoFengNew")]
        public ActionResult btnBasicCreate_Click()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (BASE_TIAOFENGDal.Exist(" and FDTUCode='" + Request["tbSelectedDTU"] + "'").Rows.Count == 0)
                    {
                        string guid = Guid.NewGuid().ToString();
                        string tiaofId = Guid.NewGuid().ToString();
                        int FCustomerID = Convert.ToInt32(Request["tbxCustomerID"]);
                        Hashtable hasData = new Hashtable();
                        hasData["id"] = tiaofId;
                        hasData["FDTUCode"] = Request["tbSelectedDTU"];
                        hasData["FName"] = Request["tbxName"];
                        hasData["FCustomerID"] = FCustomerID;
                        hasData["FLngLat"] = Request["tbxLngLat"];
                        hasData["FMapAddress"] = Request["tbSelectedAddress"];
                        hasData["FWell"] = Request["hidFWell"];
                        hasData["FPipeline"] = Request["hidFPipeline"];
                        hasData["FEnterValve"] = Request["hidFEnterValve"];
                        hasData["FExitValve"] = Request["hidFExitValve"];
                        hasData["FBrand"] = Request["tbxFBrand"];
                        hasData["FMaterial"] = Request["ddlFMaterial"];
                        hasData["FCaliber"] = Request["ddlFCaliber"];
                        hasData["FEQuiType"] = Request["ddlFEQuiType"];
                        hasData["FInstallMode"] = Request["ddlFInstallMode"];
                        hasData["FCommunicationMode"] = Request["ddlFCommunicationMode"];
                        hasData["FReadMeterMode"] = Request["ddlFReadMeterMode"];
                        hasData["FBuriedMode"] = Request["ddlFBuriedMode"];
                        hasData["FEQuiState"] = Request["ddlFEQuiState"];
                        hasData["FGroundHeigh"] = Request["tbxFGroundHeigh"];
                        hasData["FBYCycle"] = Request["tbxFBYCycle"];
                        hasData["FGHCycle"] = Request["tbxFGHCycle"];
                        hasData["FNote"] = Request["tbxFNote"];
                        hasData["FSchemeID"] = Request["tbxFSchemeID"];
                        hasData["FCreateUser"] = GetIdentityName();
                        hasData["FIsDelete"] = 0;
                        hasData["FMarkerID"] = guid;
                        BASE_TIAOFENGDal.Insert(hasData);

                        Hashtable hasData1 = new Hashtable();
                        hasData1["id"] = Guid.NewGuid();
                        hasData1["DtuNum"] = Request["tbSelectedDTU"];
                        hasData1["BASEID"] = tiaofId;
                        DATA_TIAOFENGDal.Insert(hasData1);

                        if (FCustomerID > 0)
                        {
                            string[] lnglat = Request["tbxLngLat"].Split(',');
                            Hashtable has1 = new Hashtable();
                            Hashtable has2 = new Hashtable();
                            has1["ID"] = guid;
                            has1["FName"] = Request["tbxName"];
                            has1["FMapTempID"] = Request["tbxFMapTempID"];
                            has2["FMarkerID"] = guid;
                            int maxid = Convert.ToInt32(Dal.DBUtil.SelectDataTable("select isnull(max(ID),0) as maxId from Map_MarkerProperty").Rows[0]["maxId"].ToString()) + 1;
                            has2["FAliasName"] = "Marker" + maxid.ToString();
                            has2["FType"] = "8";
                            has2["FMarker"] = "[{\"lng\":" + lnglat[0] + ",\"lat\":" + lnglat[1] + "}]";
                            has2["FMID"] = tiaofId;
                            Bll.Map_MarkerBll.InsertMarker(has1);
                            Bll.Map_MarkerBll.InsertMarkerProperty(has2);
                        }

                        ShowNotify("成功！");
                        string url = Url.Action("TF_Diagram", "BASE_TIAOFENG", new { type = "edit", tiaofId = tiaofId });
                        // 关闭本窗体（触发窗体的关闭事件）'
                        PageContext.RegisterStartupScript("parent.updateLabelResult('" + url + "');");
                    }
                    else
                    {
                        ShowNotify("调峰编号重复，请更换！");
                    }
                }
            }
            catch
            {
                ShowNotify("失败！");
            }
            return UIHelper.Result();
        }
        #endregion

        #region 修改调峰基本信息页面
        [MyAuth(MenuPower = "CoreTiaoFengEdit")]
        public ActionResult TFBasic_edit(Guid? tiaofId)
        {
            BASE_TIAOFENG tf = db.BASE_TIAOFENG.Find(tiaofId);
            if (tf == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.ddlFMaterialDataSource = sys_dictDal.SearchDDL(" and FDictID=141");
                ViewBag.ddlFCaliberDataSource = sys_dictDal.SearchDDL(" and FDictID=142");
                ViewBag.ddlFEQuiTypeDataSource = sys_dictDal.SearchDDL(" and FDictID=143");
                ViewBag.ddlFInstallModeDataSource = sys_dictDal.SearchDDL(" and FDictID=144");
                ViewBag.ddlFCommunicationModeDataSource = sys_dictDal.SearchDDL(" and FDictID=145");
                ViewBag.ddlFReadMeterModeDataSource = sys_dictDal.SearchDDL(" and FDictID=146");
                ViewBag.ddlFBuriedModeDataSource = sys_dictDal.SearchDDL(" and FDictID=147");
                ViewBag.ddlFEQuiStateDataSource = sys_dictDal.SearchDDL(" and FDictID=148");
                ViewBag.ddlFMaterialSelect = tf.FMaterial;
                ViewBag.ddlFCaliberSelect = tf.FCaliber;
                ViewBag.ddlFEQuiTypeSelect = tf.FEQuiType;
                ViewBag.ddlFInstallModeSelect = tf.FInstallMode;
                ViewBag.ddlFCommunicationModeSelect = tf.FCommunicationMode;
                ViewBag.ddlFReadMeterModeSelect = tf.FReadMeterMode;
                ViewBag.ddlFBuriedModeSelect = tf.FBuriedMode;
                ViewBag.ddlFEQuiStateSelect = tf.FEQuiState;
                if (GetUserType().Equals("3"))  //如果登录用户是客户
                {
                    ViewBag.ReadOnly = true;
                }
                else
                {
                    ViewBag.ReadOnly = false;
                }
            }
            return View(tf);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreTiaoFengEdit")]
        public ActionResult btnBasicEdit_Click()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (BASE_TIAOFENGDal.Exist(" and FDTUCode='" + Request["tbSelectedDTU"] + "' and FDTUCode<>'" + Request["tbOldDTU"] + "'").Rows.Count == 0)
                    {
                        int FCustomerID = Convert.ToInt32(Request["tbxCustomerID"]);
                        Hashtable hasData = new Hashtable();
                        hasData["id"] = Request["tbxID"];
                        hasData["FDTUCode"] = Request["tbSelectedDTU"];
                        hasData["FName"] = Request["tbxName"];
                        hasData["FCustomerID"] = FCustomerID;
                        hasData["FLngLat"] = Request["tbxLngLat"];
                        hasData["FMapAddress"] = Request["tbSelectedAddress"];
                        hasData["FWell"] = Request["hidFWell"];
                        hasData["FPipeline"] = Request["hidFPipeline"];
                        hasData["FEnterValve"] = Request["hidFEnterValve"];
                        hasData["FExitValve"] = Request["hidFExitValve"];
                        hasData["FBrand"] = Request["tbxFBrand"];
                        hasData["FMaterial"] = Request["ddlFMaterial"];
                        hasData["FCaliber"] = Request["ddlFCaliber"];
                        hasData["FEQuiType"] = Request["ddlFEQuiType"];
                        hasData["FInstallMode"] = Request["ddlFInstallMode"];
                        hasData["FCommunicationMode"] = Request["ddlFCommunicationMode"];
                        hasData["FReadMeterMode"] = Request["ddlFReadMeterMode"];
                        hasData["FBuriedMode"] = Request["ddlFBuriedMode"];
                        hasData["FEQuiState"] = Request["ddlFEQuiState"];
                        hasData["FGroundHeigh"] = Request["tbxFGroundHeigh"];
                        hasData["FNote"] = Request["tbxFNote"];
                        hasData["FBYCycle"] = Request["tbxFBYCycle"];
                        hasData["FGHCycle"] = Request["tbxFGHCycle"];
                        hasData["FSchemeID"] = Request["tbxFSchemeID"];
                        hasData["FUpdUser"] = GetIdentityName();
                        hasData["FUpdDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        BASE_TIAOFENGDal.Update(hasData);


                        Hashtable hasData1 = new Hashtable();
                        hasData1["DtuNum"] = Request["tbSelectedDTU"];
                        hasData1["BASEID"] = Request["tbxID"];
                        DATA_TIAOFENGDal.Update(hasData1);

                        if (FCustomerID > 0)
                        {
                            if (Request["tbxFMarkerID"].ToString() != "")
                            {
                                string[] lnglat = Request["tbxLngLat"].Split(',');
                                Hashtable has1 = new Hashtable();
                                has1["ID"] = Request["tbxFMarkerID"];
                                has1["FName"] = Request["tbxName"];
                                has1["FMapTempID"] = Request["tbxFMapTempID"];
                                Bll.Map_MarkerBll.UpdateMarker(has1);
                                Hashtable has2 = new Hashtable();
                                has2["FMarkerID"] = Request["tbxFMarkerID"];
                                has2["FMarker"] = "[{\"lng\":" + lnglat[0] + ",\"lat\":" + lnglat[1] + "}]";
                                Bll.Map_MarkerBll.UpdateMarkerProperty(has2);
                            }
                        }

                        ShowNotify("成功！");
                        // 关闭本窗体（触发窗体的关闭事件）
                        PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                        return UIHelper.Result();
                    }
                    else
                    {
                        ShowNotify("调峰编号重复，请更换！");
                    }
                }
            }
            catch
            {
                ShowNotify("失败！");
            }
            return UIHelper.Result();
        }
        #endregion
	}
}