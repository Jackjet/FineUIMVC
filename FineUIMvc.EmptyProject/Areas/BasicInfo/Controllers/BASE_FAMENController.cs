using FineUIMvc.PumpMVC.Controllers;
using FineUIMvc.PumpMVC.DAL;
using FineUIMvc.PumpMVC.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FineUIMvc.PumpMVC.Areas.BasicInfo.Controllers
{
    [Authorize]
    public class BASE_FAMENController : BaseController
    {
        private DBController db = new DBController();

        #region 阀门列表页面
         [MyAuth(MenuPower = "CoreFaMenView")]
        // GET: /BasicInfo/Customer/
        public ActionResult Index()
        {
            ViewBag.CoreFaMenNew = CheckPower("CoreFaMenNew");
            ViewBag.CoreFaMenDelete = CheckPower("CoreFaMenDelete");
            ViewBag.CoreFaMenEdit = CheckPower("CoreFaMenEdit");
            Hashtable table = Base_FamenDal.Search(0, 20, "a.FCreateDate", "DESC", getPowerConst("famen"));
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
                sql = sql + " and a.FName like '%" + searchMessage + "%'";
            }
            sql = sql + getPowerConst("famen");
            Hashtable table = Base_FamenDal.Search(Grid1_pageIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
            Grid1.DataSource(table["data"], Grid1_fields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreFaMenDelete")]
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
            hasData["FUpdateDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ssss"));
            hasData["FUpdateUser"] = GetIdentityName();
            Base_FamenDal.DeleteList(hasData);
            Bll.Map_MarkerBll.DeleteMarker(2, values);
            UpdateGrid(Grid1_fields, gridIndex, gridPageSize);
            return UIHelper.Result();
        }

        private void UpdateGrid(JArray Grid1_fields, int gridIndex, int gridPageSize)
        {
            var Grid1 = UIHelper.Grid("Grid1");
            Hashtable table = Base_FamenDal.Search(gridIndex, gridPageSize, "a.FCreateDate", "DESC", getPowerConst("famen"));
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
                sql = " and a.FName like '%" + triggerValue + "%'";
            }
            sql = sql + getPowerConst("famen");
            Hashtable table = Base_FamenDal.Search(gridIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
            Grid1.PageSize(gridPageSize);
            Grid1.DataSource(table["data"], gridFields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));

            return UIHelper.Result();
        }
        #endregion

        #region 阀门新增修改框架图
        public ActionResult Famen_Diagram(string type)
        {
            string url = string.Empty;
            string id = Request["famenid"];
            if (type == "add")
            {
                url = Url.Content("~/BasicInfo/BASE_FAMEN/FamenBasic_new");
                ViewBag.pageType = "add";
                ViewBag.TreeNodeShow = false;
            }
            else
            {
                url = Url.Content("~/BasicInfo/BASE_FAMEN/FamenBasic_edit?id=" + id);
                ViewBag.pageType = "edit";
                ViewBag.famenId = id;
                ViewBag.TreeNodeShow = true;
            }
            PageContext.RegisterStartupScript("F.ui.panelCenterRegion.setIFrameUrl('" + url + "');");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Tree1_NodeClick(string nodeId, string famenId)
        {
            string url = string.Empty;
            switch (nodeId)
            {
                case "basic": url = Url.Content("~/BasicInfo/BASE_FAMEN/FamenBasic_edit?id=" + famenId); break;
                case "famenFJ": url = Url.Content("~/OpenWindow/BaseDA/PumpArchives?baseId=" + famenId + "&pageType=famenFJ"); break;
            }
            PageContext.RegisterStartupScript("F.ui.panelCenterRegion.setIFrameUrl('" + url + "');");
            return UIHelper.Result();
        }


        #endregion

        #region 新增阀门基本信息页面
        [MyAuth(MenuPower = "CoreFaMenNew")]
        public ActionResult FamenBasic_new()
        {
            ViewBag.ddlCZDataSource = sys_dictDal.SearchDDL(" and FDictID=149");
            ViewBag.ddlKJDataSource = sys_dictDal.SearchDDL(" and FDictID=150");
            ViewBag.ddlSBLXDataSource = sys_dictDal.SearchDDL(" and FDictID=156");
            ViewBag.ddlCZFSDataSource = sys_dictDal.SearchDDL(" and FDictID=152");
            ViewBag.ddlTXFSDataSource = sys_dictDal.SearchDDL(" and FDictID=153");
            ViewBag.ddlFSGNDataSource = sys_dictDal.SearchDDL(" and FDictID=154");
            ViewBag.ddlPSFSDataSource = sys_dictDal.SearchDDL(" and FDictID=155");
            ViewBag.ddlFMZXDataSource = sys_dictDal.SearchDDL(" and FDictID=159");
            ViewBag.ddlSMXSDataSource = sys_dictDal.SearchDDL(" and FDictID=157");
            ViewBag.ddlSBZTDataSource = sys_dictDal.SearchDDL(" and FDictID=158");
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
        [MyAuth(MenuPower = "CoreFaMenNew")]
        public ActionResult btnBasicCreate_Click()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Base_FamenDal.Exist(" and FDTUCode='" + Request["tbSelectedDTU"] + "'").Rows.Count == 0)
                    {
                        string guid = Guid.NewGuid().ToString();
                        string famenid = Guid.NewGuid().ToString();
                        int FCustomerID = Convert.ToInt32(Request["tbxCustomerID"]);
                        Hashtable hasData = new Hashtable();
                        hasData["id"] = famenid;
                        hasData["FCustomerID"] = FCustomerID;
                        hasData["FDTUCode"] = Request["tbxFDTUCode"];
                        hasData["FLngLat"] = Request["tbxLngLat"];
                        hasData["FMapAddress"] = Request["tbSelectedAddress"];
                        hasData["FName"] = Request["tbxFName"];
                        hasData["FBrand"] = Request["tbxFBrand"];
                        hasData["FMaterial"] = Request["ddlFMaterial"];

                        hasData["FDeviceType"] = Request["ddlFDeviceType"];
                        hasData["FOperationMode"] = Request["ddlFOperationMode"];
                        hasData["FCommunicationMode"] = Request["ddlFCommunicationMode"];

                        hasData["FCaliber"] = Request["ddlFCaliber"];
                        hasData["FValveFunction"] = Request["ddlFValveFunction"];
                        hasData["FDrainageMethod"] = Request["ddlFDrainageMethod"];
                        hasData["FValveSteering"] = Request["ddlFValveSteering"];
                        hasData["FBuriedForm"] = Request["ddlFBuriedForm"];
                        hasData["FGroundElevation"] = Request["tbxFGroundElevation"];
                        hasData["FWellRoom"] = Request["tbxFWellRoom"];
                        hasData["FLocationPipeline"] = Request["tbxFLocationPipeline"];
                        hasData["FEquipmentState"] = Request["ddlFEquipmentState"];
                        hasData["FBYCycle"] = Convert.ToInt32(Request["tbxFBYCycle"]);
                        hasData["FGHCycle"] = Convert.ToInt32(Request["tbxFGHCycle"]);
                        //hasData["FNote"] = Convert.ToDateTime(Request["dpInstallDate"].ToString());
                        hasData["FCreateDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        hasData["FCreateUser"] = GetIdentityName();
                        hasData["FMarkerID"] = guid;
                        hasData["FIsDelete"] = 0;
                        hasData["FSchemeID"] = Request["tbxFSchemeID"];
                        Base_FamenDal.Insert(hasData);

                        Hashtable hasData1 = new Hashtable();
                        hasData1["id"] = Guid.NewGuid();
                        hasData1["FDTUCode"] = Request["tbxFDTUCode"];
                        hasData1["BASEID"] = famenid;
                        DATA_FAMENDal.Insert(hasData1);
                        if (FCustomerID > 0)
                        {
                            string[] lnglat = Request["tbxLngLat"].Split(',');
                            Hashtable has1 = new Hashtable();
                            Hashtable has2 = new Hashtable();
                            has1["ID"] = guid;
                            has1["FName"] = Request["tbxFName"];
                            has1["FMapTempID"] = Request["tbxFMapTempID"];
                            has2["FMarkerID"] = guid;
                            int maxid = Convert.ToInt32(Dal.DBUtil.SelectDataTable("select isnull(max(ID),0) as maxId from Map_MarkerProperty").Rows[0]["maxId"].ToString()) + 1;
                            has2["FAliasName"] = "Marker" + maxid.ToString();
                            has2["FType"] = "2";
                            has2["FMarker"] = "[{\"lng\":" + lnglat[0] + ",\"lat\":" + lnglat[1] + "}]";
                            has2["FMID"] = famenid;
                            Bll.Map_MarkerBll.InsertMarker(has1);
                            Bll.Map_MarkerBll.InsertMarkerProperty(has2);
                        }

                        ShowNotify("成功！");
                        string url = Url.Action("Famen_Diagram", "BASE_FAMEN", new { type = "edit", famenid = famenid });
                        //return RedirectToAction("Pump_Diagram", "V_BFGL", new { type = "edit", pumpId = 1002 });
                        // 关闭本窗体（触发窗体的关闭事件）'
                        PageContext.RegisterStartupScript("parent.updateLabelResult('" + url + "');");
                    }
                    else
                    {
                        ShowNotify("阀门编号重复，请更换！"); 
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

        #region 修改阀门基本信息页面
        [MyAuth(MenuPower = "CoreFaMenEdit")]
        public ActionResult FamenBasic_edit(Guid id)
        {
            BASE_FAMEN  FAMEN= db.BASE_FAMEN.Find(id);
            if (FAMEN == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (GetUserType().Equals("3"))  //如果登录用户是客户
                {
                    ViewBag.ReadOnly = true;
                }
                else
                {
                    ViewBag.ReadOnly = false;
                }

                ViewBag.ddlCZDataSource = sys_dictDal.SearchDDL(" and FDictID=149");
                ViewBag.ddlKJDataSource = sys_dictDal.SearchDDL(" and FDictID=150");
                ViewBag.ddlSBLXDataSource = sys_dictDal.SearchDDL(" and FDictID=156");
                ViewBag.ddlCZFSDataSource = sys_dictDal.SearchDDL(" and FDictID=152");
                ViewBag.ddlTXFSDataSource = sys_dictDal.SearchDDL(" and FDictID=153");
                ViewBag.ddlFSGNDataSource = sys_dictDal.SearchDDL(" and FDictID=154");
                ViewBag.ddlPSFSDataSource = sys_dictDal.SearchDDL(" and FDictID=155");
                ViewBag.ddlFMZXDataSource = sys_dictDal.SearchDDL(" and FDictID=159");
                ViewBag.ddlSMXSDataSource = sys_dictDal.SearchDDL(" and FDictID=157");
                ViewBag.ddlSBZTDataSource = sys_dictDal.SearchDDL(" and FDictID=158");

                ViewBag.ddlCZDataSelect = FAMEN.FMaterial;
                ViewBag.ddlKJDataSelect = FAMEN.FCaliber;
                ViewBag.ddlSBLXDataSelect = FAMEN.FDeviceType;
                ViewBag.ddlCZFSDataSelect = FAMEN.FOperationMode;
                ViewBag.ddlTXFSDataSelect = FAMEN.FCommunicationMode;
                ViewBag.ddlFSGNDataSelect = FAMEN.FValveFunction;
                ViewBag.ddlPSFSDataSelect = FAMEN.FDrainageMethod;
                ViewBag.ddlFMZXDataSelect = FAMEN.FValveSteering;
                ViewBag.ddlSMXSDataSelect = FAMEN.FBuriedForm;
                ViewBag.ddlSBZTDataSelect = FAMEN.FEquipmentState;
            }
            return View(FAMEN);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreFaMenEdit")]
        public ActionResult btnBasicEdit_Click()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int FCustomerID = Convert.ToInt32(Request["tbxCustomerID"]);
                    Hashtable hasData = new Hashtable();
                    hasData["id"] = Request["tbxID"];
                    hasData["FCustomerID"] = FCustomerID;
                    hasData["FDTUCode"] = Request["tbxFDTUCode"];
                    hasData["FLngLat"] = Request["tbxLngLat"];
                    hasData["FMapAddress"] = Request["tbSelectedAddress"];
                    hasData["FName"] = Request["tbxFName"];
                    hasData["FBrand"] = Request["tbxFBrand"];
                    hasData["FMaterial"] = Request["ddlFMaterial"];

                    hasData["FDeviceType"] = Request["ddlFDeviceType"];
                    hasData["FOperationMode"] = Request["ddlFOperationMode"];
                    hasData["FCommunicationMode"] = Request["ddlFCommunicationMode"];

                    hasData["FCaliber"] = Request["ddlFCaliber"];
                    hasData["FValveFunction"] = Request["ddlFValveFunction"];
                    hasData["FDrainageMethod"] = Request["ddlFDrainageMethod"];
                    hasData["FValveSteering"] = Request["ddlFValveSteering"];
                    hasData["FBuriedForm"] = Request["ddlFBuriedForm"];
                    hasData["FGroundElevation"] = Request["tbxFGroundElevation"];
                    hasData["FWellRoom"] = Request["tbxFWellRoom"];
                    hasData["FLocationPipeline"] = Request["tbxFLocationPipeline"];
                    hasData["FEquipmentState"] = Request["ddlFEquipmentState"];
                    hasData["FBYCycle"] = Convert.ToInt32(Request["tbxFBYCycle"]);
                    hasData["FGHCycle"] = Convert.ToInt32(Request["tbxFGHCycle"]);
                    hasData["FSchemeID"] = Request["tbxFSchemeID"];
                    //hasData["FNote"] = Convert.ToDateTime(Request["dpInstallDate"].ToString());
                    hasData["FUpdateUser"] = GetIdentityName();
                    hasData["FUpdateDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Base_FamenDal.Update(hasData);

                    Hashtable hasData1 = new Hashtable();
                    hasData1["FDTUCode"] = Request["tbxFDTUCode"];
                    hasData1["BASEID"] = Request["tbxID"];
                    DATA_FAMENDal.Update(hasData1);

                    if (FCustomerID > 0)
                    {
                        if (Request["tbxFMarkerID"].ToString() != "")
                        {
                            string[] lnglat = Request["tbxLngLat"].Split(',');
                            Hashtable has1 = new Hashtable();
                            has1["ID"] = Request["tbxFMarkerID"];
                            has1["FName"] = Request["tbxFName"];
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