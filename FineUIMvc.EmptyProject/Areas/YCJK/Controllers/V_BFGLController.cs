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

namespace FineUIMvc.PumpMVC.Areas.YCJK.Controllers
{
    [Authorize]
    public class V_BFGLController : BaseController
    {
        private DBController db = new DBController();

        #region Pump 泵房列表页面
        //
        // GET: /BasicInfo/Customer/
        [MyAuth(MenuPower = "CorePumpView")]
        public ActionResult Index()
        {
            ViewBag.CorePumpNew = CheckPower("CorePumpNew");
            ViewBag.CorePumpDelete = CheckPower("CorePumpDelete");
            ViewBag.CorePumpEdit = CheckPower("CorePumpEdit");
            Hashtable table = Panda_PumpDal.Search(0, 50, "a.FCreateDate", "DESC", getPowerConst("pump"));
            ViewBag.Grid1DataSource = table["data"];
            ViewBag.Grid1RecordCount = Int32.Parse(table["total"].ToString());
            switch (GetUserType())
            {
                case "1": ViewBag.FGSHidden = false; ViewBag.KHHidden = false; break;
                case "2": ViewBag.FGSHidden = false; ViewBag.KHHidden = true; break;
                case "3": ViewBag.FGSHidden = true; ViewBag.KHHidden = false; break;
                case "4": ViewBag.FGSHidden = true; ViewBag.KHHidden = true; break;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grid1_PageIndexChanged(JArray Grid1_fields, int Grid1_pageIndex, int gridPageSize, string searchMessage, string searchDTU, string searchCustomer, string searchFGS)
        {
            var Grid1 = UIHelper.Grid("Grid1");

            string sql = string.Empty;
            if (!searchMessage.Equals(""))
            {
                sql = sql + " and (PName like '%" + searchMessage + "%' or PCustomPName like '%" + searchMessage + "%' )";
            }
            if (!searchDTU.Equals(""))
            {
                sql = sql + " and a.ID in(select PumpId from Panda_PumpJZ where FIsDelete=0 and DTUCode like '%" + searchDTU + "%')";
            }
            if (!searchCustomer.Equals(""))
            {
                sql = sql + " and b.Name like '%" + searchCustomer + "%'";
            }
            if (!searchFGS.Equals(""))
            {
                sql = sql + " and c.Fengongsi ='" + searchFGS + "'";
            }
            sql = sql + getPowerConst("pump");
            Hashtable table = Panda_PumpDal.Search(Grid1_pageIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
            Grid1.DataSource(table["data"], Grid1_fields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CorePumpDelete")]
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
            hasData["ID"] = values;
            hasData["FIsDelete"] = 1;
            hasData["FDelDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ssss"));
            hasData["FDelUser"] = GetIdentityName();
            Panda_PumpDal.DeleteList(hasData);
            Bll.Map_MarkerBll.DeleteMarker(1, values);

            UpdateGrid(Grid1_fields, gridIndex, gridPageSize);
            return UIHelper.Result();
        }

        private void UpdateGrid(JArray Grid1_fields, int gridIndex, int gridPageSize)
        {
            var Grid1 = UIHelper.Grid("Grid1");
            string sql = string.Empty;
            sql = sql + getPowerConst("pump");
            Hashtable table = Panda_PumpDal.Search(gridIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
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
            var ttbSearchDTU = UIHelper.TwinTriggerBox("ttbSearchDTU");
            var ttbSearchCustomer = UIHelper.TwinTriggerBox("ttbSearchCustomer");
            var ttbSearchFGS = UIHelper.TwinTriggerBox("ttbSearchFGS");
            if (type == "trigger1")
            {
                ttbSearch.Text(String.Empty);
                ttbSearch.ShowTrigger1(false);
                ttbSearchDTU.Text(String.Empty);
                ttbSearchDTU.ShowTrigger1(false);
                ttbSearchCustomer.Text(String.Empty);
                ttbSearchCustomer.ShowTrigger1(false);
                ttbSearchFGS.Text(String.Empty);
                ttbSearchFGS.ShowTrigger1(false);
            }
            else if (type == "trigger2")
            {
                ttbSearch.ShowTrigger1(true);
                var triggerValue = typeParams.Value<string>("triggerValue");
                sql = " and (PName like '%" + triggerValue + "%' or PCustomPName like '%" + triggerValue + "%' )";
            }
            else if (type == "trigger3")
            {
                ttbSearchDTU.ShowTrigger1(true);
                var triggerValue = typeParams.Value<string>("triggerValue");
                sql = sql + " and a.ID in(select PumpId from Panda_PumpJZ where FIsDelete=0 and DTUCode like '%" + triggerValue + "%')";
            }
            else if (type == "trigger4")
            {
                ttbSearchCustomer.ShowTrigger1(true);
                var triggerValue = typeParams.Value<string>("triggerValue");
                sql = sql + " and b.Name like '%" + triggerValue + "%'";
            }
            else if (type == "trigger5")
            {
                ttbSearchFGS.ShowTrigger1(true);
                var triggerValue = typeParams.Value<string>("triggerValue");
                sql = sql + " and c.Fengongsi ='" + triggerValue + "'";
            }
            sql = sql + getPowerConst("pump");
            Hashtable table = Panda_PumpDal.Search(gridIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
            Grid1.DataSource(table["data"], gridFields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));
            Grid1.PageSize(gridPageSize);

            return UIHelper.Result();
        }
        #endregion

        #region 泵房新增修改框架图
      
        public ActionResult Pump_Diagram(string type)
        {
            string url = string.Empty;
            string pumpId = Request["pumpId"];
            if (type == "add")
            {
                url = Url.Content("~/YCJK/V_BFGL/PumpBasic_new");
                ViewBag.pageType = "add";
                ViewBag.TreeNodeShow = false;
            }
            else
            {
                url = Url.Content("~/YCJK/V_BFGL/PumpBasic_edit?id=" + pumpId);
                ViewBag.pageType = "edit";
                ViewBag.pumpId = pumpId;
                ViewBag.TreeNodeShow = true;
            }
            PageContext.RegisterStartupScript("F.ui.panelCenterRegion.setIFrameUrl('" + url + "');");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Tree1_NodeClick(string nodeId, string pumpId)
        {
            string url = string.Empty;
            switch (nodeId)
            {
                case "basic": url = Url.Content("~/YCJK/V_BFGL/PumpBasic_edit?id=" + pumpId); break;
                case "pumpJZ": url = Url.Content("~/YCJK/V_BFGL/PumpJZ?id=" + pumpId); ; break;
                case "pumpVideoQuipment": url = Url.Content("~/YCJK/V_BFGL/PumpVideoQuipment?id=" + pumpId); ; break;
                case "pumpArchives": url = Url.Content("~/OpenWindow/BaseDA/PumpArchives?baseId=" + pumpId + "&pageType=pumpJZ"); ; break;
                //case "pumpSB": url = Url.Content("~/YCJK/V_BFGL/PumpSB?id=" + pumpId); ; break;
            }
            PageContext.RegisterStartupScript("F.ui.panelCenterRegion.setIFrameUrl('" + url + "');");
            return UIHelper.Result();
        }


        #endregion

        #region 新增泵房基本信息页面
        [MyAuth(MenuPower = "CorePumpNew")]
        public ActionResult PumpBasic_new()
        {
            ViewBag.ddlCompanyDataSource = sys_DeptDal.GetCompanyList("");
            ViewBag.ddlFocusMonitorDataSource = sys_dictDal.SearchDDL(" and FDictID=52");
            ViewBag.ddlProvinceDataSource = sys_dictDal.SearchDDL(" and FDictID=108");
            ViewBag.ddlPumpLocation = sys_dictDal.SearchDDL(" and FDictID=137");
            ViewBag.ddlPTypeDataSource = sys_dictDal.SearchDDL(" and FDictID=165");
            ViewBag.txtPCode = GetBillNo("Panda_Pump");
            if (GetUserType().Equals("3"))  //如果登录用户是客户
            {
                ViewBag.CustomerName = GetUserCustomerName();
                ViewBag.CustomerID = GetUserCustomer();
                ViewBag.MapTempID = GetCustomerMapTempID();
                ViewBag.KHReadOnly = true;
                ViewBag.FGSReadOnly = true;
                ViewBag.KHHidden = false;
                ViewBag.FGSHidden = true;
                ViewBag.SelectValue = "0";
                ViewBag.KHRequired = true;
                ViewBag.FGSRequired = false;
            }
            else if (GetUserType().Equals("2"))
            {
                ViewBag.CustomerName = "";
                ViewBag.CustomerID = "0";
                ViewBag.KHReadOnly = true;
                ViewBag.FGSReadOnly = true;
                ViewBag.KHHidden = true;
                ViewBag.FGSHidden = false;
                ViewBag.SelectValue = GetUserCompanyNumber();
                ViewBag.KHRequired = false;
                ViewBag.FGSRequired = true;
            }
            else
            {
                ViewBag.CustomerName = "";
                ViewBag.CustomerID = "0";
                ViewBag.KHReadOnly = false;
                ViewBag.FGSReadOnly = false;
                ViewBag.KHHidden = false;
                ViewBag.FGSHidden = false;
                ViewBag.SelectValue = "0";
                ViewBag.KHRequired = false;
                ViewBag.FGSRequired = false;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CorePumpNew")]
        public ActionResult btnBasicCreate_Click()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (GetUserType().Equals("1"))  //如果登录用户是管理员
                    {
                        if (Request["tbSelectedCustomer"].Equals("") && Request["ddlCompany"].Equals("0"))
                        {
                            ShowNotify("客户和分公司必须存在一项或两项！");
                            return UIHelper.Result();
                        }
                    }
                    string guid = Guid.NewGuid().ToString();
                    string pumpId = Guid.NewGuid().ToString();
                    int FCustomerID = Convert.ToInt32(Request["tbxCustomerID"]);
                    Hashtable hasData = new Hashtable();
                    hasData["ID"] = pumpId;
                    hasData["PCode"] = Request["tbxPCode"];
                    hasData["PName"] = Request["tbxPName"];
                    hasData["PCustomPName"] = Request["tbxPName"];
                    hasData["IsFocusMonitoring"] = Request["ddlFocusMonitoring"];
                    hasData["FCustomerID"] = FCustomerID;
                    hasData["PCompanyNumber"] = Request["ddlCompany"];
                    hasData["PProvince"] = Request["ddlProvince"];
                    hasData["PCity"] = Request["ddlCity"];
                    hasData["PLngLat"] = Request["tbxLngLat"];
                    hasData["PAddress"] = Request["tbSelectedAddress"];
                    hasData["PXXAddress"] = Request["tbxAddress"];
                    hasData["FCreateUser"] = GetIdentityName();
                    hasData["FCreateDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    hasData["FIsDelete"] = 0;
                    hasData["TankIsSharing"] = Convert.ToInt32(Request["rbtnTankIsSharing"]);
                    hasData["MasterControlPLCIP"] = Request["tbxMasterControlPLCIP"];
                    hasData["PumpSoaking"] = Convert.ToInt32(Request["ddlPumpSoaking"]);
                    hasData["Warning"] = Convert.ToInt32(Request["ddlWarning"]);
                    hasData["WaterQualityDetection"] = Convert.ToInt32(Request["ddlWaterQualityDetection"]);
                    hasData["ControlValve"] = Convert.ToInt32(Request["ddlControlValve"]);
                    hasData["WaterTankSterilizer"] = Convert.ToInt32(Request["ddlWaterTankSterilizer"]);
                    hasData["PumpLocation"] = Convert.ToInt32(Request["ddlPumpLocation"]);
                    hasData["InstallDate"] = Convert.ToDateTime(Request["dpInstallDate"].ToString());
                    hasData["AcceptanceDate"] = Convert.ToDateTime(Request["dpAcceptanceDate"].ToString());
                    hasData["WaterFloor"] = Convert.ToInt32(Request["tbxWaterFloor"] == "" ? "1" : Request["tbxWaterFloor"]);
                    hasData["DressingCycle"] = Convert.ToInt32(Request["tbxDressingCycle"] == "" ? "1" : Request["tbxDressingCycle"]);
                    hasData["FMarkerID"] = guid;
                    hasData["PType"] = Request["ddlPType"];
                    Panda_PumpDal.Insert(hasData);

                    if (FCustomerID > 0)
                    {
                        //string FMapTempID = db.Panda_Customer.Where(x => x.ID == FCustomerID).FirstOrDefault().FMapTempID.ToString();
                        string[] lnglat = Request["tbxLngLat"].Split(',');
                        Hashtable has1 = new Hashtable();
                        Hashtable has2 = new Hashtable();
                        has1["ID"] = guid;
                        has1["FName"] = Request["tbxPName"];
                        has1["FMapTempID"] = Request["tbxFMapTempID"];
                        has2["FMarkerID"] = guid;
                        int maxid = Convert.ToInt32(Dal.DBUtil.SelectDataTable("select isnull(max(ID),0) as maxId from Map_MarkerProperty").Rows[0]["maxId"].ToString()) + 1;
                        has2["FAliasName"] = "Marker" + maxid.ToString();
                        has2["FType"] = "1";
                        has2["FMarker"] = "[{\"lng\":" + lnglat[0] + ",\"lat\":" + lnglat[1] + "}]";
                        has2["FMID"] = pumpId;
                        Bll.Map_MarkerBll.InsertMarker(has1);
                        Bll.Map_MarkerBll.InsertMarkerProperty(has2);
                    }

                    ShowNotify("成功！");
                    string url = Url.Action("Pump_Diagram", "V_BFGL", new { type = "edit", pumpId = pumpId });
                    //return RedirectToAction("Pump_Diagram", "V_BFGL", new { type = "edit", pumpId = 1002 });
                    // 关闭本窗体（触发窗体的关闭事件）'
                    PageContext.RegisterStartupScript("parent.updateLabelResult('" + url + "');");
                }
            }
            catch
            {
                ShowNotify("失败！");
            }
            return UIHelper.Result();
        }
        #endregion

        #region 修改泵房基本信息页面
        [MyAuth(MenuPower = "CorePumpEdit")]
        public ActionResult PumpBasic_edit(Guid id)
        {
            Panda_Pump pump = db.Panda_Pump.Find(id);
            if (pump == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (GetUserType().Equals("3"))  //如果登录用户是客户
                {
                    ViewBag.KHReadOnly = true;
                    ViewBag.FGSReadOnly = true;
                    ViewBag.KHHidden = false;
                    ViewBag.FGSHidden = true;
                    ViewBag.KHRequired = true;
                    ViewBag.FGSRequired = false;
                }
                else if (GetUserType().Equals("2"))
                {
                    ViewBag.KHReadOnly = false;
                    ViewBag.FGSReadOnly = false;
                    ViewBag.KHHidden = true;
                    ViewBag.FGSHidden = false;
                    ViewBag.KHRequired = false;
                    ViewBag.FGSRequired = true;
                }
                else
                {
                    ViewBag.KHReadOnly = false;
                    ViewBag.FGSReadOnly = false;
                    ViewBag.KHHidden = false;
                    ViewBag.FGSHidden = false;
                    ViewBag.KHRequired = false;
                    ViewBag.FGSRequired = false;
                }
                ViewBag.ddlCompanyDataSource = sys_DeptDal.GetCompanyList("");
                ViewBag.ddlFocusMonitorDataSource = sys_dictDal.SearchDDL(" and FDictID=52");
                ViewBag.ddlProvinceDataSource = sys_dictDal.SearchDDL(" and FDictID=108");
                ViewBag.ddlPumpLocation = sys_dictDal.SearchDDL(" and FDictID=137");
                ViewBag.ddlCityDataSource = sys_dictDal.SearchDDL(" and FDictID=120 and FParentValue='" + pump.PProvince + "'");
                ViewBag.ddlPTypeDataSource = sys_dictDal.SearchDDL(" and FDictID=165");
                ViewBag.ddlCompanySelect = pump.PCompanyNumber.ToString().Trim();
                ViewBag.ddlFocusMonitorSelect = pump.IsFocusMonitoring.ToString().Trim();
                ViewBag.ddlProvinceSelect = pump.PProvince.ToString().Trim();
                ViewBag.ddlCitySelect = pump.PCity.ToString().Trim();
                ViewBag.tbSelectedCustomer = pump.Panda_Customer == null ? "" : pump.Panda_Customer.Name;
                ViewBag.ddlTankIsSharing = pump.TankIsSharing.ToString().Trim();
                ViewBag.ddlPType = pump.PType.ToString().Trim();
                ViewBag.PumpSoaking = pump.PumpSoaking.ToString().Trim();
                ViewBag.Warning = pump.Warning.ToString().Trim();
                ViewBag.WaterQualityDetection = pump.WaterQualityDetection;
                ViewBag.ControlValve = pump.ControlValve;
                ViewBag.WaterTankSterilizer = pump.WaterTankSterilizer;
                ViewBag.PumpLocation = pump.PumpLocation;
                ViewBag.MasterControlPLCIP = pump.MasterControlPLCIP;
                ViewBag.Panda_Customer = pump.Panda_Customer;
                int customerid = pump.Panda_Customer == null ? 0 : pump.Panda_Customer.ID;
                ViewBag.hidSelectedCustomer = customerid.ToString();
                string FMapTempID = pump.Panda_Customer == null ? "" : pump.Panda_Customer.FMapTempID.ToString();
                ViewBag.FMapTempID = FMapTempID;
            }
            return View(pump);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CorePumpEdit")]
        public ActionResult btnBasicEdit_Click()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (GetUserType().Equals("1"))  //如果登录用户是管理员
                    {
                        if (Request["tbSelectedCustomer"].Equals("") && Request["ddlCompany"].Equals("0"))
                        {
                            ShowNotify("客户和分公司必须存在一项或两项！");
                            return UIHelper.Result();
                        }
                    }
                    int FCustomerID = Convert.ToInt32(Request["tbxCustomerID"]);
                    Hashtable hasData = new Hashtable();
                    hasData["ID"] = Request["tbxID"];
                    hasData["PCode"] = Request["tbxPCode"];
                    hasData["PName"] = Request["tbxPName"];
                    hasData["PCustomPName"] = Request["tbxPName"];
                    hasData["IsFocusMonitoring"] = Request["ddlFocusMonitoring"];
                    hasData["FCustomerID"] = FCustomerID;
                    hasData["PCompanyNumber"] = Request["ddlCompany"];
                    hasData["PProvince"] = Request["ddlProvince"];
                    hasData["PCity"] = Request["ddlCity"];
                    hasData["PLngLat"] = Request["tbxLngLat"];
                    hasData["PAddress"] = Request["tbSelectedAddress"];
                    hasData["PXXAddress"] = Request["tbxAddress"];
                    hasData["FUpdUser"] = GetIdentityName();
                    hasData["FUpdDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    hasData["TankIsSharing"] = Convert.ToInt32(Request["rbtnTankIsSharing"]);
                    hasData["MasterControlPLCIP"] = Request["tbxMasterControlPLCIP"];
                    hasData["PumpSoaking"] = Convert.ToInt32(Request["ddlPumpSoaking"]);
                    hasData["Warning"] = Convert.ToInt32(Request["ddlWarning"]);
                    hasData["WaterQualityDetection"] = Convert.ToInt32(Request["ddlWaterQualityDetection"]);
                    hasData["ControlValve"] = Convert.ToInt32(Request["ddlControlValve"]);
                    hasData["WaterTankSterilizer"] = Convert.ToInt32(Request["ddlWaterTankSterilizer"]);
                    hasData["PumpLocation"] = Convert.ToInt32(Request["ddlPumpLocation"]);
                    hasData["InstallDate"] = Convert.ToDateTime(Request["dpInstallDate"]);
                    hasData["AcceptanceDate"] = Convert.ToDateTime(Request["dpAcceptanceDate"]);
                    hasData["WaterFloor"] = Convert.ToInt32(Request["tbxWaterFloor"] == "" ? "1" : Request["tbxWaterFloor"]);
                    hasData["DressingCycle"] = Convert.ToInt32(Request["tbxDressingCycle"] == "" ? "1" : Request["tbxDressingCycle"]);
                    hasData["PType"] = Request["ddlPType"];
                    Panda_PumpDal.Update(hasData);

                    if (FCustomerID > 0)
                    {
                        if (Request["tbxFMarkerID"].ToString() != "")
                        {
                            //string FMapTempID = db.Panda_Customer.Where(x => x.ID == FCustomerID).FirstOrDefault().FMapTempID.ToString();
                            string[] lnglat = Request["tbxLngLat"].Split(',');

                            Hashtable has1 = new Hashtable();
                            has1["ID"] = Request["tbxFMarkerID"];
                            has1["FName"] = Request["tbxPName"];
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

        #region 泵房机组模块

        #region 泵房机组列表页面
        //
        // GET: /BasicInfo/Customer/
        [MyAuth(MenuPower = "CorePumpJZView")]
        public ActionResult PumpJZ(Guid id)
        {
            ViewBag.CorePumpJZNew = CheckPower("CorePumpJZNew");
            ViewBag.CorePumpJZDelete = CheckPower("CorePumpJZDelete");
            ViewBag.CorePumpJZEdit = CheckPower("CorePumpJZEdit");
            ViewBag.CorePumpJZAlermSet = CheckPower("CorePumpJZAlermSet");
            ViewBag.pumpId = id.ToString();
            Hashtable table = Panda_PumpJZDal.Search(0, 20, "a.FCreateDate", "DESC", " and PumpId='" + id + "'");
            DataTable dt = (DataTable)table["data"];
            if (dt.Rows.Count > 0)
            {
                int customerid = Convert.ToInt32(dt.Rows[0]["FCustomerID"]);
                if (customerid > 0)
                {
                    ViewBag.Hidden = false;
                }
                else
                {
                    ViewBag.Hidden = true;
                }
            }
            else
            {
                Hashtable table1 = Panda_PumpDal.Search(0, 20, "a.FCreateDate", "DESC", " and a.ID='" + id + "'");
                DataTable dt1 = (DataTable)table1["data"];
                int customerid = Convert.ToInt32(dt1.Rows[0]["FCustomerID"]);
                if (customerid > 0)
                {
                    ViewBag.Hidden = false;
                }
                else
                {
                    ViewBag.Hidden = true;
                }
            }
            ViewBag.GridJZDataSource = table["data"];
            ViewBag.GridJZRecordCount = Int32.Parse(table["total"].ToString());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GridJZ_PageIndexChanged(JArray GridJZ_fields, int GridJZ_pageIndex, int gridPageSize, Guid pumpid, string searchMessage)
        {
            var GridJZ = UIHelper.Grid("GridJZ");

            string sql = string.Empty;
            if (!searchMessage.Equals(""))
            {
                sql = sql + " and PumpJZName like '%" + searchMessage + "%'";
            }
            sql = sql + " and pumpid = '" + pumpid + "'";
            Hashtable table = Panda_PumpJZDal.Search(GridJZ_pageIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
            GridJZ.DataSource(table["data"], GridJZ_fields);
            GridJZ.RecordCount(Int32.Parse(table["total"].ToString()));

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CorePumpJZDelete")]
        public ActionResult GridJZ_Delete(JArray selectedRows, JArray GridJZ_fields, int gridIndex, int gridPageSize, Guid pumpid)
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
            hasData["ID"] = values;
            hasData["FIsDelete"] = 1;
            hasData["FDelDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ssss"));
            hasData["FDelUser"] = GetIdentityName();
            Panda_PumpJZDal.DeleteList(hasData);

            UpdateGridJZ(GridJZ_fields, gridIndex, gridPageSize, pumpid);
            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CorePumpJZEdit")]
        public ActionResult GridJZ_Move(JArray selectedRows, JArray GridJZ_fields, int gridIndex, int gridPageSize, Guid pumpid, Guid selectPumpId)
        {
            if (Panda_PumpJZDal.Exist(" and PumpId='" + selectPumpId + "' and PumpJZArea=(select PumpJZArea from Panda_PumpJZ where ID='" + selectedRows[0].ToString() + "')").Rows.Count > 0)
            {
                ShowNotify("此泵房存在和机组相同的设备分区，请更换！");
            }
            else
            {
                Hashtable hasData = new Hashtable();
                hasData["ID"] = selectedRows[0].ToString();
                hasData["PumpId"] = selectPumpId;
                hasData["FUpdUser"] = GetIdentityName();
                hasData["FUpdDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                Panda_PumpJZDal.Update(hasData);
            }
            UpdateGridJZ(GridJZ_fields, gridIndex, gridPageSize, pumpid);
            return UIHelper.Result();
        }

        private void UpdateGridJZ(JArray GridJZ_fields, int gridIndex, int gridPageSize, Guid pumpid)
        {
            var GridJZ = UIHelper.Grid("GridJZ");
            string sql = string.Empty;
            sql = sql + " and pumpid = '" + pumpid + "'";
            Hashtable table = Panda_PumpJZDal.Search(gridIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
            GridJZ.DataSource(table["data"], GridJZ_fields);
            GridJZ.RecordCount(Int32.Parse(table["total"].ToString()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CorePumpJZEdit")]
        public ActionResult GridJZ_ContactGroup(JArray selectedRows, string ContactGroup, JArray GridJZ_fields, int gridIndex, int gridPageSize, Guid pumpid)
        {
            Hashtable hasData = new Hashtable();
            hasData["ID"] = selectedRows[0].ToString();
            hasData["PumpJZContactGroup"] = ContactGroup;
            Panda_PumpJZDal.Update(hasData);
            UpdateGridJZ(GridJZ_fields, gridIndex, gridPageSize, pumpid);
            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyCustomJZPostBack(string type, JArray gridFields, JObject typeParams, int gridIndex, int gridPageSize, Guid pumpid)
        {
            var GridJZ = UIHelper.Grid("GridJZ");
            string sql = string.Empty;
            sql = sql + " and pumpid = '" + pumpid + "'";
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
                sql = " and PumpJZName like '%" + triggerValue + "%'";
            }

            Hashtable table = Panda_PumpJZDal.Search(gridIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
            GridJZ.DataSource(table["data"], gridFields);
            GridJZ.RecordCount(Int32.Parse(table["total"].ToString()));
            GridJZ.PageSize(gridPageSize);

            return UIHelper.Result();
        }
        #endregion

        #region 泵房机组新增页面
        [MyAuth(MenuPower = "CorePumpJZNew")]
        public ActionResult PumpJZ_new(Guid pumpid)
        {
            ViewBag.ddlTypeDataSource = sys_dictDal.SearchDDL(" and FDictID=132");     //类型
            ViewBag.ddlPumpJZAreaDataSource = sys_dictDal.SearchDDL(" and FDictID=133");   //所在区域
            ViewBag.ddlDqmsDataSource = sys_dictDal.SearchDDL(" and FDictID=138");   //读取模式
            ViewBag.ddlZHFTypeDataSource = sys_dictDal.SearchDDL(" and FDictID=139");   //止回阀类型
            ViewBag.pumpId = pumpid.ToString();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CorePumpJZNew")]
        public ActionResult btnJZCreate_Click(string ddlPumpJZArea, string ddlPumpJZArea_text, bool ddlPumpJZArea_isUserInput)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string area = ddlPumpJZArea;
                    if (!ddlPumpJZArea_isUserInput)
                    {
                        area = ddlPumpJZArea;
                    }
                    else
                    {
                        area = ddlPumpJZArea_text;
                    }

                    if (Panda_PumpJZDal.Exist(" and PumpId='" + Request["pumpId"] + "' and PumpJZArea='" + area.Trim() + "'").Rows.Count == 0)
                    {
                        if (Panda_PumpJZDal.Exist(" and DTUCode='" + Request["tbSelectedDTU"] + "'").Rows.Count == 0)
                        {
                            string jz_id = Guid.NewGuid().ToString();
                            Hashtable hasData = new Hashtable();
                            hasData["ID"] = jz_id;
                            hasData["DTUCode"] = Request["tbSelectedDTU"];
                            hasData["PumpId"] = Request["pumpId"];
                            hasData["PumpJZName"] = Request["tbxJZName"];
                            hasData["MachineType"] = Request["ddlType"];
                            hasData["RunPumpNum"] = Request["ddlRunPumpNum"];
                            hasData["Auxiliarypumpcount"] = Request["ddlAuxiliarypumpcount"];
                            hasData["DrainPumpNum"] = Request["ddlDrainPumpNum"];
                            hasData["IsAlarm"] = Request["cboOpenMessage"] == "true" ? 1 : 0;
                            hasData["PumpJZArea"] = area;
                            hasData["PumpJZAddressList"] = Request["hidDZB"] == "" ? "0" : Request["hidDZB"];
                            hasData["PumpJZCollectPeriod"] = Request["txt_cjzq"];//采集周期
                            hasData["PumpJZCollectLength"] = Request["txt_cjcd"];//采集长度
                            hasData["PumpJZReadMode"] = Request["ddlDqms"];//读取模式
                            hasData["PumpJZInletDiameter"] = Request["txt_jkgj"];//进口管径
                            hasData["PumpJZOutletDiameter"] = Request["txt_ckgj"];//出口管径
                            hasData["PumpJZBrandSet"] = Request["txt_ctpp"];//成套品牌
                            hasData["PumpJZPumpBrand"] = Request["txt_sbpp"];//水泵品牌
                            hasData["PumpJZMainPumpFlow"] = Request["txt_zbll"];//主泵流量
                            hasData["PumpJZMainPumpLift"] = Request["txt_zbyc"];//主泵扬程
                            hasData["PumpJZAuxiliPumpFlow"] = Request["txt_fbll"];//辅泵流量
                            hasData["PumpJZAuxiliPumpLift"] = Request["txt_fbyc"];//辅泵扬程
                            hasData["PumpJZMainPumpPower"] = Request["txt_zbgl"];//主泵功率
                            hasData["PumpJZAuxiliPumpPower"] = Request["txt_fbgl"];//辅泵功率
                            hasData["PumpJZTankVolume"] = Request["txt_sxrj"];//水箱容积
                            hasData["PumpJZTankLength"] = Request["txt_sxcc"];//水箱尺寸
                            hasData["PumpJZPressReliValve"] = Request["cboYNJYF"] == "true" ? 1 : 0;//有无减压阀
                            hasData["PumpJZPeak"] = Request["cboYNTFGN"] == "true" ? 1 : 0;//调峰功能
                            hasData["PumpJZCheckValve"] = Request["ddlZHFType"];//止回阀类型
                            hasData["PumpJZInPUpper"] = Request["txt_jsS"];//进水压力上限
                            hasData["PumpJZInPLower"] = Request["txt_jsX"];//进水压力下限
                            hasData["PumpJZOutPUpper"] = Request["txt_csS"];//出水压力上限
                            hasData["PumpJZOutPLower"] = Request["txt_csX"];//出水压力下限
                            hasData["PumpJZReChlorUpper"] = Request["txt_ylS"];//余氯值上限
                            hasData["PumpJZReChlorLower"] = Request["txt_ylX"];//余氯值下限
                            hasData["PumpJZTurbidUpper"] = Request["txt_zdS"];//浊度值上限
                            hasData["PumpJZTurbidLower"] = Request["txt_zdX"];//浊度值下限
                            hasData["PumpJZPHUpper"] = Request["txt_phS"];//PH值上限
                            hasData["PumpJZPHLower"] = Request["txt_phX"];//PH值下限
                            hasData["PumpJZTankUpper"] = Request["txt_sxS"];//水箱上
                            hasData["PumpJZTankLower"] = Request["txt_sxX"];//水箱下
                            hasData["FCreateUser"] = GetIdentityName();
                            hasData["FCreateDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            hasData["FIsDelete"] = 0;
                            Panda_PumpJZDal.Insert(hasData, Request["tbSelectedDTU"]);
                            Alarm_ParamDal.SearchInsertAlarm(" and FMarkerType=1 ", jz_id.ToString());

                            Hashtable hasData1 = new Hashtable();
                            hasData1["ID"] = Guid.NewGuid();
                            hasData1["FDTUCode"] = Request["tbSelectedDTU"];
                            hasData1["BaseID"] = jz_id;
                            E_DATADal.Insert(hasData1);
                            ShowNotify("成功！");
                            // 关闭本窗体（触发窗体的关闭事件）
                            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                        }
                        else
                        {
                            ShowNotify("已存在此机组编号，请更换！");
                        }
                    }
                    else
                    {
                        ShowNotify("已存在此设备分区，请更换！");
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

        #region 泵房机组修改页面
        [MyAuth(MenuPower = "CorePumpJZEdit")]
        public ActionResult PumpJZ_edit(Guid pumpJZId)
        {
            Panda_PumpJZ pumpJZ = db.Panda_PumpJZ.Find(pumpJZId);
            if (pumpJZ == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.ddlTypeDataSource = sys_dictDal.SearchDDL(" and FDictID=132");     //类型
                ViewBag.ddlPumpJZAreaDataSource = sys_dictDal.SearchDDL(" and FDictID=133");   //所在区域
                ViewBag.ddlDqmsDataSource = sys_dictDal.SearchDDL(" and FDictID=138");   //读取模式
                ViewBag.ddlZHFTypeDataSource = sys_dictDal.SearchDDL(" and FDictID=139");   //止回阀类型
                ViewBag.ddlDqmsSelect = pumpJZ.PumpJZReadMode.Trim().ToString();
                ViewBag.ddlZHFTypeSelect = pumpJZ.PumpJZCheckValve.Trim().ToString();
                ViewBag.ddlTypeSelect = pumpJZ.MachineType.Trim().ToString();
                ViewBag.ddlPumpJZAreaSelect = pumpJZ.PumpJZArea.Trim().ToString();
                ViewBag.ddlRunPumpNumSelect = pumpJZ.RunPumpNum.ToString();
                ViewBag.ddlAuxiliarypumpcountSelect = pumpJZ.Auxiliarypumpcount.ToString();
                ViewBag.ddlDrainPumpNumSelect = pumpJZ.DrainPumpNum.ToString();
                ViewBag.cboOpenMessage = pumpJZ.IsAlarm == 0 ? false : true;
                ViewBag.cboYNJYF = pumpJZ.PumpJZPressReliValve == 0 ? false : true;
                ViewBag.cboYNTFGN = pumpJZ.PumpJZPeak == 0 ? false : true;
            }
            return View(pumpJZ);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CorePumpJZEdit")]
        public ActionResult btnJZEdit_Click(string ddlPumpJZArea, string ddlPumpJZArea_text, bool ddlPumpJZArea_isUserInput)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string area = ddlPumpJZArea;
                    if (!ddlPumpJZArea_isUserInput)
                    {
                        area = ddlPumpJZArea;
                    }
                    else
                    {
                        area = ddlPumpJZArea_text;
                    }

                    if (Panda_PumpJZDal.Exist(" and PumpId='" + Request["pumpId"] + "' and PumpJZArea='" + area.Trim() + "' and PumpJZArea<>'" + area.Trim() + "'").Rows.Count == 0)
                    {
                        if (Panda_PumpJZDal.Exist(" and DTUCode='" + Request["tbSelectedDTU"] + "' and DTUCode<>'" + Request["tbSelectedDTU"] + "'").Rows.Count == 0)
                        {
                            Hashtable hasData = new Hashtable();
                            hasData["ID"] = Request["tbxID"];
                            hasData["DTUCode"] = Request["tbSelectedDTU"];
                            hasData["PumpJZName"] = Request["tbxJZName"];
                            hasData["MachineType"] = Request["ddlType"];
                            hasData["RunPumpNum"] = Request["ddlRunPumpNum"];
                            hasData["Auxiliarypumpcount"] = Request["ddlAuxiliarypumpcount"];
                            hasData["DrainPumpNum"] = Request["ddlDrainPumpNum"];
                            hasData["PumpJZArea"] = area;
                            hasData["PumpJZAddressList"] = Request["hidDZB"] == "" ? "0" : Request["hidDZB"];
                            hasData["IsAlarm"] = Request["cboOpenMessage"] == "true" ? 1 : 0;
                            hasData["PumpJZCollectPeriod"] = Request["txt_cjzq"];//采集周期
                            hasData["PumpJZCollectLength"] = Request["txt_cjcd"];//采集长度
                            hasData["PumpJZReadMode"] = Request["ddlDqms"];//读取模式
                            hasData["PumpJZInletDiameter"] = Request["txt_jkgj"];//进口管径
                            hasData["PumpJZOutletDiameter"] = Request["txt_ckgj"];//出口管径
                            hasData["PumpJZBrandSet"] = Request["txt_ctpp"];//成套品牌
                            hasData["PumpJZPumpBrand"] = Request["txt_sbpp"];//水泵品牌
                            hasData["PumpJZMainPumpFlow"] = Request["txt_zbll"];//主泵流量
                            hasData["PumpJZMainPumpLift"] = Request["txt_zbyc"];//主泵扬程
                            hasData["PumpJZAuxiliPumpFlow"] = Request["txt_fbll"];//辅泵流量
                            hasData["PumpJZAuxiliPumpLift"] = Request["txt_fbyc"];//辅泵扬程
                            hasData["PumpJZMainPumpPower"] = Request["txt_zbgl"];//主泵功率
                            hasData["PumpJZAuxiliPumpPower"] = Request["txt_fbgl"];//辅泵功率
                            hasData["PumpJZTankVolume"] = Request["txt_sxrj"];//水箱容积
                            hasData["PumpJZTankLength"] = Request["txt_sxcc"];//水箱尺寸
                            hasData["PumpJZPressReliValve"] = Request["cboYNJYF"] == "true" ? 1 : 0;//有无减压阀
                            hasData["PumpJZPeak"] = Request["cboYNTFGN"] == "true" ? 1 : 0;//调峰功能
                            hasData["PumpJZCheckValve"] = Request["ddlZHFType"];//止回阀类型
                            hasData["PumpJZInPUpper"] = Request["txt_jsS"];//进水压力上限
                            hasData["PumpJZInPLower"] = Request["txt_jsX"];//进水压力下限
                            hasData["PumpJZOutPUpper"] = Request["txt_csS"];//出水压力上限
                            hasData["PumpJZOutPLower"] = Request["txt_csX"];//出水压力下限
                            hasData["PumpJZReChlorUpper"] = Request["txt_ylS"];//余氯值上限
                            hasData["PumpJZReChlorLower"] = Request["txt_ylX"];//余氯值下限
                            hasData["PumpJZTurbidUpper"] = Request["txt_zdS"];//浊度值上限
                            hasData["PumpJZTurbidLower"] = Request["txt_zdX"];//浊度值下限
                            hasData["PumpJZPHUpper"] = Request["txt_phS"];//PH值上限
                            hasData["PumpJZPHLower"] = Request["txt_phX"];//PH值下限
                            hasData["PumpJZTankUpper"] = Request["txt_sxS"];//水箱上
                            hasData["PumpJZTankLower"] = Request["txt_sxX"];//水箱下
                            hasData["FUpdUser"] = GetIdentityName();
                            hasData["FUpdDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            Panda_PumpJZDal.Update(hasData, Request["tbSelectedDTU"]);

                            Hashtable hasData1 = new Hashtable();
                            hasData1["FDTUCode"] = Request["tbSelectedDTU"];
                            hasData1["BaseID"] = Request["tbxID"];
                            E_DATADal.Update(hasData1);
                            ShowNotify("成功！");
                            // 关闭本窗体（触发窗体的关闭事件）
                            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                        }
                        else
                        {
                            ShowNotify("已存在此机组编号，请更换！");
                        }
                    }
                    else
                    {
                        ShowNotify("已存在此设备分区，请更换！");
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

        #endregion

        #region 泵房视频设备模块

        #region 泵房视频设备列表页面
        //
        // GET: /BasicInfo/Customer/
        [MyAuth(MenuPower = "CorePumpVideoQView")]
        public ActionResult PumpVideoQuipment(Guid id)
        {
            ViewBag.CorePumpVideoQNew = CheckPower("CorePumpVideoQNew");
            ViewBag.CorePumpVideoQDelete = CheckPower("CorePumpVideoQDelete");
            ViewBag.CorePumpVideoQEdit = CheckPower("CorePumpVideoQEdit");
            ViewBag.pumpId = id.ToString();
            Hashtable table = Panda_PumpVideoQuipmentDal.Search(0, 20, "a.CreateOn", "DESC", " and PumpId='" + id + "'");
            ViewBag.GridVQDataSource = table["data"];
            ViewBag.GridVQRecordCount = Int32.Parse(table["total"].ToString());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GridVQ_PageIndexChanged(JArray GridVQ_fields, int GridVQ_pageIndex, int gridPageSize, string pumpid)
        {
            var GridVQ = UIHelper.Grid("GridVQ");
            string sql = string.Empty;

            sql = sql + " and pumpid = '" + pumpid + "'";
            Hashtable table = Panda_PumpVideoQuipmentDal.Search(GridVQ_pageIndex, gridPageSize, "a.CreateOn", "DESC", sql);
            GridVQ.DataSource(table["data"], GridVQ_fields);
            GridVQ.RecordCount(Int32.Parse(table["total"].ToString()));

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CorePumpVideoQDelete")]
        public ActionResult GridVQ_Delete(JArray selectedRows, JArray GridVQ_fields, int gridIndex, int gridPageSize, string pumpid)
        {
            string values = "";
            for (int i = 0; i < selectedRows.Count; i++)
            {
                if (i != selectedRows.Count - 1)
                {
                    values += "" + selectedRows[i] + ",";
                }
                else
                {
                    values += "" + selectedRows[i] + "";
                }
            }

            Hashtable hasData = new Hashtable();
            hasData["ID"] = values;
            hasData["IsActive"] = 1;
            hasData["UpdateOn"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ssss"));
            hasData["UpdateBy"] = GetIdentityName();
            Panda_PumpVideoQuipmentDal.DeleteList(hasData);

            UpdateGridVQ(GridVQ_fields, gridIndex, gridPageSize, pumpid);
            return UIHelper.Result();
        }

        private void UpdateGridVQ(JArray GridVQ_fields, int gridIndex, int gridPageSize, string pumpid)
        {
            var GridVQ = UIHelper.Grid("GridVQ");
            string sql = string.Empty;
            sql = sql + " and pumpid = '" + pumpid + "'";
            Hashtable table = Panda_PumpVideoQuipmentDal.Search(gridIndex, gridPageSize, "a.CreateOn", "DESC", sql);
            GridVQ.DataSource(table["data"], GridVQ_fields);
            GridVQ.RecordCount(Int32.Parse(table["total"].ToString()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyCustomVQPostBack(string type, JArray gridFields, JObject typeParams, int gridIndex, int gridPageSize, string pumpid)
        {
            var GridVQ = UIHelper.Grid("GridVQ");
            string sql = string.Empty;
            sql = sql + " and pumpid = '" + pumpid + "'";
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
                sql = " and PumpJZName like '%" + triggerValue + "%'";
            }

            Hashtable table = Panda_PumpVideoQuipmentDal.Search(gridIndex, gridPageSize, "a.CreateOn", "DESC", sql);
            GridVQ.PageSize(gridPageSize);
            GridVQ.DataSource(table["data"], gridFields);
            GridVQ.RecordCount(Int32.Parse(table["total"].ToString()));

            return UIHelper.Result();
        }
        #endregion

        #region 泵房视频设备新增页面
        [MyAuth(MenuPower = "CorePumpVideoQNew")]
        public ActionResult PumpVQ_new(string pumpid)
        {
            ViewBag.ddlTypeDataSource = sys_dictDal.SearchDDL(" and FDictID=134");     //类型
            ViewBag.pumpId = pumpid.ToString();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CorePumpVideoQNew")]
        public ActionResult btnVQCreate_Click()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Hashtable hasData = new Hashtable();
                    hasData["QuipmentType"] = Request["ddlType"];
                    hasData["PumpId"] = Request["pumpId"];
                    hasData["Type"] = Request["tbxType"];
                    hasData["Brand"] = Request["tbxBrand"];
                    hasData["Number"] = Request["tbxNumber"];
                    hasData["IP"] = Request["tbxIP"];
                    hasData["Port"] = Request["tbxPort"];
                    hasData["UserName"] = Request["tbxUserName"];
                    hasData["PassWord"] = Request["tbxPassWord"];
                    hasData["FOrderBy"] = Request["tbxFOrderBy"];
                    hasData["Mark"] = Request["tbxMark"];
                    hasData["Rtmp"] = Request["tbxRtmp"];
                    hasData["Hls"] = Request["tbxHls"];
                    hasData["CreateBy"] = GetIdentityName();
                    hasData["CreateOn"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    hasData["IsActive"] = 0;
                    Panda_PumpVideoQuipmentDal.Insert(hasData);
                    ShowNotify("成功！");
                    // 关闭本窗体（触发窗体的关闭事件）
                    PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                }
            }
            catch
            {
                ShowNotify("失败！");
            }
            return UIHelper.Result();
        }
        #endregion

        #region 泵房视频设备修改页面
        [MyAuth(MenuPower = "CorePumpVideoQEdit")]
        public ActionResult PumpVQ_edit(int pumpVQId)
        {
            Panda_PumpVQ pumpVQ = db.Panda_PumpVQ.Find(pumpVQId);
            if (pumpVQ == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.ddlTypeDataSource = sys_dictDal.SearchDDL(" and FDictID=134");     //类型
                ViewBag.ddlTypeSelect = pumpVQ.QuipmentType.Trim().ToString();
            }
            return View(pumpVQ);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CorePumpVideoQEdit")]
        public ActionResult btnVQEdit_Click()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Hashtable hasData = new Hashtable();
                    hasData["ID"] = Request["tbxID"];
                    hasData["QuipmentType"] = Request["ddlType"];
                    hasData["Type"] = Request["tbxType"];
                    hasData["Brand"] = Request["tbxBrand"];
                    hasData["Number"] = Request["tbxNumber"];
                    hasData["IP"] = Request["tbxIP"];
                    hasData["Port"] = Request["tbxPort"];
                    hasData["UserName"] = Request["tbxUserName"];
                    hasData["PassWord"] = Request["tbxPassWord"];
                    hasData["FOrderBy"] = Request["tbxFOrderBy"];
                    hasData["Mark"] = Request["tbxMark"];
                    hasData["Rtmp"] = Request["tbxRtmp"];
                    hasData["Hls"] = Request["tbxHls"];
                    hasData["UpdateBy"] = GetIdentityName();
                    hasData["UpdateOn"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Panda_PumpVideoQuipmentDal.Update(hasData);
                    ShowNotify("成功！");
                    // 关闭本窗体（触发窗体的关闭事件）
                    PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                }
            }
            catch
            {
                ShowNotify("失败！");
            }
            return UIHelper.Result();
        }
        #endregion
        #endregion


        #region 泵房设备
        #region 泵房设备列表页面
        //
        // GET: /BasicInfo/Customer/
        public ActionResult PumpSB(int id)
        {
            ViewBag.pumpId = id.ToString();
            //Hashtable table = Panda_PumpSBDal.Search(0, 20, "a.FCreateDate", "DESC", " and PumpId=" + id);
            //ViewBag.GridSBDataSource = table["data"];
            //ViewBag.GridSBRecordCount = Int32.Parse(table["total"].ToString());
            ViewBag.GridSBDataSource = null;
            ViewBag.GridSBRecordCount = 0;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GridSB_PageIndexChanged(JArray GridSB_fields, int GridSB_pageIndex, int gridPageSize, int pumpid)
        {
            var GridSB = UIHelper.Grid("GridSB");

            string sql = string.Empty;

            sql = sql + " and pumpid = " + pumpid;
            Hashtable table = Panda_PumpSBDal.Search(GridSB_pageIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
            GridSB.DataSource(table["data"], GridSB_fields);
            GridSB.RecordCount(Int32.Parse(table["total"].ToString()));

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GridSB_Delete(JArray selectedRows, JArray GridSB_fields, int gridIndex, int gridPageSize, int pumpid)
        {
            string values = "";
            for (int i = 0; i < selectedRows.Count; i++)
            {
                if (i != selectedRows.Count - 1)
                {
                    values += "" + selectedRows[i] + ",";
                }
                else
                {
                    values += "" + selectedRows[i] + "";
                }
            }

            Hashtable hasData = new Hashtable();
            hasData["ID"] = values;
            hasData["FIsDelete"] = 1;
            hasData["FDelDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ssss"));
            hasData["FDelUser"] = GetIdentityName();
            Panda_PumpSBDal.DeleteList(hasData);

            UpdateGridSB(GridSB_fields, gridIndex, gridPageSize, pumpid);
            return UIHelper.Result();
        }

        private void UpdateGridSB(JArray GridSB_fields, int gridIndex, int gridPageSize, int pumpid)
        {
            var GridSB = UIHelper.Grid("GridSB");
            string sql = string.Empty;
            sql = sql + " and pumpid = " + pumpid;
            Hashtable table = Panda_PumpSBDal.Search(gridIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
            GridSB.DataSource(table["data"], GridSB_fields);
            GridSB.RecordCount(Int32.Parse(table["total"].ToString()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyCustomSBPostBack(string type, JArray gridFields, JObject typeParams, int gridIndex, int gridPageSize, int pumpid)
        {
            var GridSB = UIHelper.Grid("GridSB");
            string sql = string.Empty;
            sql = sql + " and pumpid = " + pumpid;
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
                sql = " and PumpSBName like '%" + triggerValue + "%'";
            }

            Hashtable table = Panda_PumpSBDal.Search(gridIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
            GridSB.DataSource(table["data"], gridFields);
            GridSB.RecordCount(Int32.Parse(table["total"].ToString()));

            return UIHelper.Result();
        }
        #endregion

        #region 泵房设备新增页面
        public ActionResult PumpSB_new(int pumpid)
        {
            //ViewBag.ddlTypeDataSource = sys_dictDal.SearchDDL(" and FDictID=132");     //类型
            //ViewBag.ddlPumpSBAreaDataSource = sys_dictDal.SearchDDL(" and FDictID=133");   //所在区域
            ViewBag.pumpId = pumpid.ToString();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult btnSBCreate_Click()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Hashtable hasData = new Hashtable();
                    //hasData["DTUCode"] = Request["hidDTU"];
                    //hasData["PumpId"] = Request["pumpId"];
                    //hasData["PumpJZName"] = Request["tbxJZName"];
                    //hasData["MachineType"] = Request["ddlType"];
                    //hasData["RunPumpNum"] = Request["ddlRunPumpNum"];
                    //hasData["Auxiliarypumpcount"] = Request["ddlAuxiliarypumpcount"];
                    //hasData["DrainPumpNum"] = Request["ddlDrainPumpNum"];
                    //hasData["IsAlarm"] = Request["cboOpenMessage"] == "true" ? 1 : 0;
                    //hasData["PumpJZArea"] = Request["ddlPumpJZArea"];
                    //hasData["PumpJZAddressList"] = Request["hidDZB"] == "" ? "0" : Request["hidDZB"];
                    //hasData["FCreateUser"] = GetIdentityName();
                    //hasData["FCreateDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    //hasData["FIsDelete"] = 0;
                    //Panda_PumpJZDal.Insert(hasData, Convert.ToInt32(Request["hidDTU"]));
                    ShowNotify("成功！");
                    // 关闭本窗体（触发窗体的关闭事件）
                    PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                }
            }
            catch
            {
                ShowNotify("失败！");
            }
            return UIHelper.Result();
        }
        #endregion

        #region 泵房设备修改页面
        public ActionResult PumpSB_edit(int pumpSBId)
        {
            //Panda_PumpSB pumpSB = db.Panda_PumpSB.Find(pumpSBId);
            //if (pumpSB == null)
            //{
            //    return HttpNotFound();
            //}
            //else
            //{
            //    ViewBag.ddlTypeDataSource = sys_dictDal.SearchDDL(" and FDictID=132");     //类型
            //    ViewBag.ddlPumpSBAreaDataSource = sys_dictDal.SearchDDL(" and FDictID=133");   //所在区域
            //    ViewBag.ddlTypeSelect = pumpJZ.MachineType.Trim().ToString();
            //    ViewBag.ddlPumpJZAreaSelect = pumpJZ.PumpJZArea.Trim().ToString();
            //    ViewBag.ddlRunPumpNumSelect = pumpJZ.RunPumpNum.ToString();
            //    ViewBag.ddlAuxiliarypumpcountSelect = pumpJZ.Auxiliarypumpcount.ToString();
            //    ViewBag.ddlDrainPumpNumSelect = pumpJZ.DrainPumpNum.ToString();
            //    ViewBag.cboOpenMessage = pumpJZ.IsAlarm == 0 ? false : true;
            //}
            //return View(pumpJZ);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult btnSBEdit_Click()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Hashtable hasData = new Hashtable();
                    //hasData["ID"] = Request["tbxID"];
                    //hasData["DTUCode"] = Request["hidDTU"];
                    //hasData["PumpJZName"] = Request["tbxJZName"];
                    //hasData["MachineType"] = Request["ddlType"];
                    //hasData["RunPumpNum"] = Request["ddlRunPumpNum"];
                    //hasData["Auxiliarypumpcount"] = Request["ddlAuxiliarypumpcount"];
                    //hasData["DrainPumpNum"] = Request["ddlDrainPumpNum"];
                    //hasData["PumpJZArea"] = Request["ddlPumpJZArea"];
                    //hasData["PumpJZAddressList"] = Request["hidDZB"] == "" ? "0" : Request["hidDZB"];
                    //hasData["IsAlarm"] = Request["cboOpenMessage"] == "true" ? 1 : 0;
                    //hasData["FUpdUser"] = GetIdentityName();
                    //hasData["FUpdDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    //Panda_PumpJZDal.Update(hasData, Convert.ToInt32(Request["hidDTU"]));
                    ShowNotify("成功！");
                    // 关闭本窗体（触发窗体的关闭事件）
                    PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                }
            }
            catch
            {
                ShowNotify("失败！");
            }
            return UIHelper.Result();
        }
        #endregion
        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ddlSheng_SelectedIndexChanged(FormCollection values)
        {
            List<ListItem> items = new List<ListItem>();
            items.Add(new ListItem("选择地区市", "-1", true));
            if (values["ddlProvince"] != "-1")
            {
                DataTable dt = sys_dictDal.SearchDDL(" and FDictID=120 and FParentValue='" + values["ddlProvince"] + "'");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListItem item = new ListItem();
                    item.Text = dt.Rows[i]["FName"].ToString();
                    item.Value = dt.Rows[i]["FValue"].ToString();
                    items.Add(item);
                }
            }
            // 更新前台数据
            UIHelper.DropDownList("ddlCity").LoadData(items.ToArray());
            //UIHelper.DropDownList("ddlShi").DataSource(sys_dictDal.SearchDDL(" and FDictID=120 and FParentValue='" + values["ddlProvince"] + "'"));

            return UIHelper.Result();
        }
    }
}