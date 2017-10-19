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
    /// <summary>
    /// 压力
    /// </summary>
    [Authorize]
    public class PressureController : BaseController
    {
        private DBController db = new DBController();

        #region 压力列表页面
        [MyAuth(MenuPower = "CorePaView")]
        public ActionResult Index()
        {
            ViewBag.CorePaNew = CheckPower("CorePaNew");
            ViewBag.CorePaDelete = CheckPower("CorePaDelete");
            ViewBag.CorePaEdit = CheckPower("CorePaEdit");
            Hashtable table = BASE_YALIDal.SearchLL(0, 20, "a.FCreateDate", "DESC", getPowerConst("pressure"));
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
            sql = sql + getPowerConst("pressure");
            Hashtable table = BASE_YALIDal.SearchLL(Grid1_pageIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
            Grid1.DataSource(table["data"], Grid1_fields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CorePaDelete")]
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
            BASE_YALIDal.DeleteList(hasData);
            Bll.Map_MarkerBll.DeleteMarker(7, values);
            UpdateGrid(Grid1_fields, gridIndex, gridPageSize);
            return UIHelper.Result();
        }

        private void UpdateGrid(JArray Grid1_fields, int gridIndex, int gridPageSize)
        {
            var Grid1 = UIHelper.Grid("Grid1");
            Hashtable table = BASE_YALIDal.SearchLL(gridIndex, gridPageSize, "a.FCreateDate", "DESC", getPowerConst("pressure"));
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
            sql = sql + getPowerConst("pressure");
            Hashtable table = BASE_YALIDal.SearchLL(gridIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
            Grid1.DataSource(table["data"], gridFields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));

            return UIHelper.Result();
        }
        #endregion

        #region 压力新增修改框架图
        [MyAuth(MenuPower = "CorePaNew")]
        public ActionResult Pa_Diagram(string type)
        {
            string url = string.Empty;
            string id = Request["paid"];
            if (type == "add")
            {
                url = Url.Content("~/BasicInfo/Pressure/PaBasic_new");
                ViewBag.pageType = "add";
                ViewBag.TreeNodeShow = false;
            }
            else
            {
                url = Url.Content("~/BasicInfo/Pressure/PaBasic_edit?paid=" + id);
                ViewBag.pageType = "edit";
                ViewBag.paid = id;
                ViewBag.TreeNodeShow = true;
            }
            PageContext.RegisterStartupScript("F.ui.panelCenterRegion.setIFrameUrl('" + url + "');");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Tree1_NodeClick(string nodeId, string paid)
        {
            string url = string.Empty;
            switch (nodeId)
            {
                case "basic": url = Url.Content("~/BasicInfo/Pressure/PaBasic_edit?paid=" + paid); break;
                case "paArchives": url = Url.Content("~/OpenWindow/BaseDA/PumpArchives?baseId=" + paid + "&pageType=paFJ"); break;
            }
            PageContext.RegisterStartupScript("F.ui.panelCenterRegion.setIFrameUrl('" + url + "');");
            return UIHelper.Result();
        }
        #endregion

        #region 新增压力基本信息页面
        [MyAuth(MenuPower = "CorePaNew")]
        public ActionResult PaBasic_new()
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
        [MyAuth(MenuPower = "CorePaNew")]
        public ActionResult btnBasicCreate_Click()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (BASE_YALIDal.Exist(" and FDTUCode='" + Request["tbSelectedDTU"] + "'").Rows.Count == 0)
                    {
                        string guid = Guid.NewGuid().ToString();
                        string paid = Guid.NewGuid().ToString();
                        int FCustomerID = Convert.ToInt32(Request["tbxCustomerID"]);
                        Hashtable hasData = new Hashtable();
                        hasData["id"] = paid;
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
                        hasData["FMpaUp"] = Request["tbxPaS"];
                        hasData["FMpaDown"] = Request["tbxPaX"];
                        hasData["FNote"] = Request["tbxFNote"];
                        hasData["FSchemeID"] = Request["tbxFSchemeID"];
                        hasData["FCreateUser"] = GetIdentityName();
                        hasData["FIsDelete"] = 0;
                        hasData["FMarkerID"] = guid;
                        BASE_YALIDal.Insert(hasData);

                        Hashtable hasData1 = new Hashtable();
                        hasData1["id"] = Guid.NewGuid();
                        hasData1["FDTUCode"] = Request["tbSelectedDTU"];
                        hasData1["BASEID"] = paid;
                        DATA_YALIDal.Insert(hasData1);

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
                            has2["FType"] = "7";
                            has2["FMarker"] = "[{\"lng\":" + lnglat[0] + ",\"lat\":" + lnglat[1] + "}]";
                            has2["FMID"] = paid;
                            Bll.Map_MarkerBll.InsertMarker(has1);
                            Bll.Map_MarkerBll.InsertMarkerProperty(has2);
                        }

                        ShowNotify("成功！");
                        string url = Url.Action("Pa_Diagram", "Pressure", new { type = "edit", paid = paid });
                        // 关闭本窗体（触发窗体的关闭事件）'
                        PageContext.RegisterStartupScript("parent.updateLabelResult('" + url + "');");
                    }
                    else
                    {
                        ShowNotify("压力编号重复，请更换！");
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

        #region 修改压力基本信息页面
        [MyAuth(MenuPower = "CorePaEdit")]
        public ActionResult PaBasic_edit(Guid? paid)
        {
            BASE_YALI flow = db.BASE_YALI.Find(paid);
            if (flow == null)
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
                ViewBag.ddlFMaterialSelect = flow.FMaterial;
                ViewBag.ddlFCaliberSelect = flow.FCaliber;
                ViewBag.ddlFEQuiTypeSelect = flow.FEQuiType;
                ViewBag.ddlFInstallModeSelect = flow.FInstallMode;
                ViewBag.ddlFCommunicationModeSelect = flow.FCommunicationMode;
                ViewBag.ddlFReadMeterModeSelect = flow.FReadMeterMode;
                ViewBag.ddlFBuriedModeSelect = flow.FBuriedMode;
                ViewBag.ddlFEQuiStateSelect = flow.FEQuiState;
                if (GetUserType().Equals("3"))  //如果登录用户是客户
                {
                    ViewBag.ReadOnly = true;
                }
                else
                {
                    ViewBag.ReadOnly = false;
                }
            }
            return View(flow);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CorePaEdit")]
        public ActionResult btnBasicEdit_Click()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (BASE_YALIDal.Exist(" and FDTUCode='" + Request["tbSelectedDTU"] + "' and FDTUCode<>'" + Request["tbOldDTU"] + "'").Rows.Count == 0)
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
                        hasData["FMpaUp"] = Request["tbxPaS"];
                        hasData["FMpaDown"] = Request["tbxPaX"];
                        hasData["FGHCycle"] = Request["tbxFGHCycle"];
                        hasData["FSchemeID"] = Request["tbxFSchemeID"];
                        hasData["FUpdUser"] = GetIdentityName();
                        hasData["FUpdDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        BASE_YALIDal.Update(hasData);


                        Hashtable hasData1 = new Hashtable();
                        hasData1["FDTUCode"] = Request["tbSelectedDTU"];
                        hasData1["BASEID"] = Request["tbxID"];
                        DATA_YALIDal.Update(hasData1);

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
                        ShowNotify("压力编号重复，请更换！");
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