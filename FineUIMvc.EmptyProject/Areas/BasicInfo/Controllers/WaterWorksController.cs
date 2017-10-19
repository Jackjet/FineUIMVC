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
    public class WaterWorksController : BaseController
    {
        private DBController db = new DBController();

        #region 水厂列表页面
        [MyAuth(MenuPower = "CoreWaterWorkView")]
        public ActionResult Index()
        {
            ViewBag.CoreWaterWorkNew = CheckPower("CoreWaterWorkNew");
            ViewBag.CoreWaterWorkDelete = CheckPower("CoreWaterWorkDelete");
            ViewBag.CoreWaterWorkEdit = CheckPower("CoreWaterWorkEdit");
            Hashtable table = BASE_SHUICHANGDal.Search(0, 20, "a.FCreateDate", "DESC", getPowerConst("water"));
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
            sql = sql + getPowerConst("water");
            Hashtable table = BASE_SHUICHANGDal.Search(Grid1_pageIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
            Grid1.DataSource(table["data"], Grid1_fields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreWaterWorkDelete")]
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
            BASE_SHUICHANGDal.DeleteList(hasData);
            Bll.Map_MarkerBll.DeleteMarker(4, values);
            UpdateGrid(Grid1_fields, gridIndex, gridPageSize);
            return UIHelper.Result();
        }

        private void UpdateGrid(JArray Grid1_fields, int gridIndex, int gridPageSize)
        {
            var Grid1 = UIHelper.Grid("Grid1");

            Hashtable table = BASE_SHUICHANGDal.Search(gridIndex, gridPageSize, "a.FCreateDate", "DESC", getPowerConst("water"));
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
            sql = sql + getPowerConst("water");
            Hashtable table = BASE_SHUICHANGDal.Search(gridIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
            Grid1.DataSource(table["data"], gridFields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));

            return UIHelper.Result();
        }
        #endregion

        #region 水厂新增修改框架图
        [MyAuth(MenuPower = "CoreWaterWorkNew")]
        public ActionResult Water_Diagram(string type)
        {
            string url = string.Empty;
            string id = Request["waterId"];
            if (type == "add")
            {
                url = Url.Content("~/BasicInfo/WaterWorks/WaterBasic_new");
                ViewBag.pageType = "add";
                ViewBag.TreeNodeShow = false;
            }
            else
            {
                url = Url.Content("~/BasicInfo/WaterWorks/WaterBasic_edit?id=" + id);
                ViewBag.pageType = "edit";
                ViewBag.waterId = id;
                ViewBag.TreeNodeShow = true;
            }
            PageContext.RegisterStartupScript("F.ui.panelCenterRegion.setIFrameUrl('" + url + "');");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Tree1_NodeClick(string nodeId, string waterId)
        {
            string url = string.Empty;
            switch (nodeId)
            {
                case "basic": url = Url.Content("~/BasicInfo/WaterWorks/WaterBasic_edit?id=" + waterId); break;
                case "WaterArchives": url = Url.Content("~/OpenWindow/BaseDA/PumpArchives?baseId=" + waterId + "&pageType=waterFJ"); break;
                case "WaterJZ": url = Url.Content("~/BasicInfo/WaterWorks/WaterJZ?id=" + waterId); break;
            }
            PageContext.RegisterStartupScript("F.ui.panelCenterRegion.setIFrameUrl('" + url + "');");
            return UIHelper.Result();
        }
        #endregion

        #region 新增水厂基本信息页面
        [MyAuth(MenuPower = "CoreWaterWorkNew")]
        public ActionResult WaterBasic_new()
        {
            ViewBag.ddlFTypeDataSource = sys_dictDal.SearchDDL(" and FDictID=160");
            ViewBag.ddlFRotaryPaDataSource = sys_dictDal.SearchDDL(" and FDictID=161");
            ViewBag.ddlFInDiameterDataSource = sys_dictDal.SearchDDL(" and FDictID=162");
            ViewBag.ddlFOutDiameterDataSource = sys_dictDal.SearchDDL(" and FDictID=163");
            ViewBag.txtCode = GetBillNo("BASE_SHUICHANG");
            if (GetUserType().Equals("1"))  //如果登录用户是管理员
            {
                ViewBag.Hidden = false;
            }
            else
            {
                ViewBag.Hidden = true;
            }
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
        [MyAuth(MenuPower = "CoreWaterWorkNew")]
        public ActionResult btnBasicCreate_Click()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string guid = Guid.NewGuid().ToString();
                    string waterid = Guid.NewGuid().ToString();
                    int FCustomerID = Convert.ToInt32(Request["tbxCustomerID"]);
                    Hashtable hasData = new Hashtable();
                    hasData["id"] = waterid;
                    hasData["FCode"] = Request["tbxCode"];
                    hasData["FName"] = Request["tbxName"];
                    hasData["FCustomerID"] = FCustomerID;
                    hasData["FLngLat"] = Request["tbxLngLat"];
                    hasData["FMapAddress"] = Request["tbSelectedAddress"];

                    hasData["FType"] = Request["ddlFType"];
                    hasData["FRotaryPa"] = Request["ddlFRotaryPa"];
                    hasData["FInDiameter"] = Request["ddlFInDiameter"];
                    hasData["FOutDiameter"] = Request["ddlFOutDiameter"];

                    hasData["FEnterWNum"] = Request["tbxFEnterWNum"];
                    hasData["FExitWNum"] = Request["tbxFExitWNum"];
                    hasData["FWater"] = Request["tbxFWater"];
                    hasData["FWaterPa"] = Request["tbxFWaterPa"];
                    hasData["FWaterM3"] = Request["tbxFWaterM3"];
                    hasData["FNote"] = Request["tbxFNote"];
                    hasData["FJKLink"] = Request["tbxFJKLink"];

                    hasData["FCreateUser"] = GetIdentityName();
                    hasData["FIsDelete"] = 0;
                    hasData["FMarkerID"] = guid;
                    BASE_SHUICHANGDal.Insert(hasData);

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
                        has2["FType"] = "4";
                        has2["FMarker"] = "[{\"lng\":" + lnglat[0] + ",\"lat\":" + lnglat[1] + "}]";
                        has2["FMID"] = waterid;
                        Bll.Map_MarkerBll.InsertMarker(has1);
                        Bll.Map_MarkerBll.InsertMarkerProperty(has2);
                    }

                    ShowNotify("成功！");
                    string url = Url.Action("Water_Diagram", "WaterWorks", new { type = "edit", waterId = waterid });
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

        #region 修改水厂基本信息页面
        [MyAuth(MenuPower = "CoreWaterWorkEdit")]
        public ActionResult WaterBasic_edit(Guid? id)
        {
            BASE_SHUICHANG water = db.BASE_SHUICHANG.Find(id);
            if (water == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.ddlFTypeDataSource = sys_dictDal.SearchDDL(" and FDictID=160");
                ViewBag.ddlFRotaryPaDataSource = sys_dictDal.SearchDDL(" and FDictID=161");
                ViewBag.ddlFInDiameterDataSource = sys_dictDal.SearchDDL(" and FDictID=162");
                ViewBag.ddlFOutDiameterDataSource = sys_dictDal.SearchDDL(" and FDictID=163");
                if (GetUserType().Equals("1"))  //如果登录用户是管理员
                {
                    ViewBag.Hidden = false;
                }
                else
                {
                    ViewBag.Hidden = true;
                }
                if (GetUserType().Equals("3"))  //如果登录用户是客户
                {
                    ViewBag.ReadOnly = true;
                }
                else
                {
                    ViewBag.ReadOnly = false;
                }
            }
            return View(water);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreWaterWorkEdit")]
        public ActionResult btnBasicEdit_Click()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int FCustomerID = Convert.ToInt32(Request["tbxCustomerID"]);
                    Hashtable hasData = new Hashtable();
                    hasData["id"] = Request["tbxID"];
                    hasData["FName"] = Request["tbxName"];
                    hasData["FCustomerID"] = FCustomerID;
                    hasData["FLngLat"] = Request["tbxLngLat"];
                    hasData["FMapAddress"] = Request["tbSelectedAddress"];

                    hasData["FType"] = Request["ddlFType"];
                    hasData["FRotaryPa"] = Request["ddlFRotaryPa"];
                    hasData["FInDiameter"] = Request["ddlFInDiameter"];
                    hasData["FOutDiameter"] = Request["ddlFOutDiameter"];

                    hasData["FEnterWNum"] = Request["tbxFEnterWNum"];
                    hasData["FExitWNum"] = Request["tbxFExitWNum"];
                    hasData["FWater"] = Request["tbxFWater"];
                    hasData["FWaterPa"] = Request["tbxFWaterPa"];
                    hasData["FWaterM3"] = Request["tbxFWaterM3"];
                    hasData["FNote"] = Request["tbxFNote"];
                    hasData["FJKLink"] = Request["tbxFJKLink"];
                    hasData["FUpdUser"] = GetIdentityName();
                    hasData["FUpdDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    BASE_SHUICHANGDal.Update(hasData);

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
            }
            catch
            {
                ShowNotify("失败！");
            }
            return UIHelper.Result();
        }
        #endregion

        //
        // GET: /BasicInfo/Customer/
        [MyAuth(MenuPower = "CoreWaterJZView")]
        public ActionResult WaterJZ(Guid id)
        {
            ViewBag.CorePumpJZNew = CheckPower("CoreWaterJZNew");
            ViewBag.CorePumpJZDelete = CheckPower("CoreWaterJZDelete");
            ViewBag.CorePumpJZEdit = CheckPower("CoreWaterJZEdit");
            ViewBag.CorePumpJZAlermSet = CheckPower("CoreWaterJZAlermSet");
            ViewBag.ShuiChangId = id.ToString();
            Hashtable table = BASE_SHUICHANG_JZDal.Search(0, 20, "a.FCreateDate", "DESC", " and a.ShuiChangId='" + id + "'");

            ViewBag.GridJZDataSource = table["data"];
            ViewBag.GridJZRecordCount = Int32.Parse(table["total"].ToString());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GridJZ_PageIndexChanged(JArray GridJZ_fields, int GridJZ_pageIndex, int gridPageSize, Guid ShuiChangId, string searchMessage)
        {
            var GridJZ = UIHelper.Grid("GridJZ");

            string sql = string.Empty;
            if (!searchMessage.Equals(""))
            {
                sql = sql + " and FName like '%" + searchMessage + "%'";
            }
            sql = sql + " and ShuiChangId = '" + ShuiChangId + "'";
            Hashtable table = BASE_SHUICHANG_JZDal.Search(GridJZ_pageIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
            GridJZ.DataSource(table["data"], GridJZ_fields);
            GridJZ.RecordCount(Int32.Parse(table["total"].ToString()));

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreWaterJZDelete")]
        public ActionResult GridJZ_Delete(JArray selectedRows, JArray GridJZ_fields, int gridIndex, int gridPageSize, Guid ShuiChangId)
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
            hasData["FUpdDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ssss"));
            hasData["FUpdUser"] = GetIdentityName();
            BASE_SHUICHANG_JZDal.DeleteList(hasData);

            UpdateGridJZ(GridJZ_fields, gridIndex, gridPageSize, ShuiChangId);
            return UIHelper.Result();
        }

        private void UpdateGridJZ(JArray GridJZ_fields, int gridIndex, int gridPageSize, Guid ShuiChangId)
        {
            var GridJZ = UIHelper.Grid("GridJZ");
            string sql = string.Empty;
            sql = sql + " and ShuiChangId = '" + ShuiChangId + "'";
            Hashtable table = BASE_SHUICHANG_JZDal.Search(gridIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
            GridJZ.DataSource(table["data"], GridJZ_fields);
            GridJZ.RecordCount(Int32.Parse(table["total"].ToString()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyCustomJZPostBack(string type, JArray gridFields, JObject typeParams, int gridIndex, int gridPageSize, Guid ShuiChangId)
        {
            var GridJZ = UIHelper.Grid("GridJZ");
            string sql = string.Empty;
            sql = sql + " and ShuiChangId = '" + ShuiChangId + "'";
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

            Hashtable table = BASE_SHUICHANG_JZDal.Search(gridIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
            GridJZ.DataSource(table["data"], gridFields);
            GridJZ.RecordCount(Int32.Parse(table["total"].ToString()));
            GridJZ.PageSize(gridPageSize);

            return UIHelper.Result();
        }

        #region 泵房机组新增页面
        [MyAuth(MenuPower = "CoreWaterJZNew")]
        public ActionResult WaterJZ_NEW(Guid ShuiChangId)
        {
            ViewBag.ddlTypeDataSource = sys_dictDal.SearchDDL(" and FDictID=132");     //类型

            ViewBag.ddlDqmsDataSource = sys_dictDal.SearchDDL(" and FDictID=138");   //读取模式
            ViewBag.ShuiChangId = ShuiChangId.ToString();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreWaterJZNew")]
        public ActionResult btnJZCreate_Click()
        {
            try
            {
                if (ModelState.IsValid)
                {

                    string jz_id = Guid.NewGuid().ToString();
                    Hashtable hasData = new Hashtable();
                    hasData["ID"] = jz_id;
                    hasData["FDTUCode"] = Request["tbSelectedDTU"];
                    hasData["ShuiChangId"] = Request["ShuiChangId"];
                    hasData["FName"] = Request["tbxJZName"];
                    hasData["MachineType"] = Request["ddlType"];
                    hasData["CollectPeriod"] = Request["txt_cjzq"]; ;//采集周期
                    hasData["CollectLength"] = Request["txt_cjcd"];//采集长度
                    hasData["ReadMode"] = Request["ddlDqms"];//读取模式
                    hasData["AddressList"] = Request["hidDZB"] == "" ? "0" : Request["hidDZB"];

                    hasData["FCreateUser"] = GetIdentityName();
                    hasData["FCreateDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    hasData["FIsDelete"] = 0;
                    BASE_SHUICHANG_JZDal.Insert(hasData);
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

        #region 泵房机组修改页面

        [MyAuth(MenuPower = "CoreWaterJZEdit")]
        public ActionResult WaterJZ_Edit(Guid ShuiChangId)
        {
            BASE_SHUICHANG_JZ BASE_SHUICHANG_JZ = db.BASE_SHUICHANG_JZ.Find(ShuiChangId);
            if (BASE_SHUICHANG_JZ == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.ddlTypeDataSource = sys_dictDal.SearchDDL(" and FDictID=132");     //类型
                ViewBag.ddlDqmsDataSource = sys_dictDal.SearchDDL(" and FDictID=138");   //读取模式
                ViewBag.ddlDqmsSelect = BASE_SHUICHANG_JZ.ReadMode.Trim().ToString();
                ViewBag.ddlTypeSelect = BASE_SHUICHANG_JZ.MachineType.Trim().ToString();
                ViewBag.ShuiChangId = ShuiChangId.ToString();
            }
            return View(BASE_SHUICHANG_JZ);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreWaterJZEdit")]
        public ActionResult btnJZEdit_Click()
        {
            try
            {
                if (ModelState.IsValid)
                {

                    Hashtable hasData = new Hashtable();
                    hasData["ID"] = Request["tbxID"];
                    hasData["FDTUCode"] = Request["tbSelectedDTU"];
                    hasData["FName"] = Request["tbxJZName"];
                    hasData["MachineType"] = Request["ddlType"];
                    hasData["CollectPeriod"] = Request["txt_cjzq"]; ;//采集周期
                    hasData["CollectLength"] = Request["txt_cjcd"];//采集长度
                    hasData["ReadMode"] = Request["ddlDqms"];//读取模式
                    hasData["AddressList"] = Request["hidDZB"] == "" ? "0" : Request["hidDZB"];

                    hasData["FCreateUser"] = GetIdentityName();
                    hasData["FCreateDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    hasData["FIsDelete"] = 0;
                    hasData["FUpdUser"] = GetIdentityName();
                    hasData["FUpdDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    BASE_SHUICHANG_JZDal.Update(hasData);

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
    }
}