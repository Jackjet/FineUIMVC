using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FineUIMvc.PumpMVC.Controllers;
using FineUIMvc.PumpMVC.DAL;
using FineUIMvc.PumpMVC.Models;
using FineUIMvc.PumpMVC.Common;
using System.Collections;
using System.Data;
using System.Data.Entity;
using Newtonsoft.Json.Linq;

namespace FineUIMvc.PumpMVC.Areas.BasicInfo.Controllers
{
    [Authorize]
    public class CompressorStationController : BaseController
    {
        private DBController db = new DBController();

        #region 加压站列表页面
        /// <summary>
        /// 加压站
        /// </summary>
        /// <returns></returns>
        [MyAuth(MenuPower = "CoreJYZView")]
        public ActionResult Index()
        {
            ViewBag.CoreJYZNew = CheckPower("CoreJYZNew");
            ViewBag.CoreJYZDelete = CheckPower("CoreJYZDelete");
            ViewBag.CoreJYZEdit = CheckPower("CoreJYZEdit");
            Hashtable table = BASE_JIAYAZHANDal.SearchJYZ(0, 50, "a.FCreateDate", "DESC", getPowerConst("jiayazhan"));
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
        public ActionResult Grid1_PageIndexChanged(JArray Grid1_fields, int Grid1_pageIndex, int gridPageSize, string searchMessage)
        {
            var Grid1 = UIHelper.Grid("Grid1");

            string sql = string.Empty;
            if (!searchMessage.Equals(""))
            {
                sql = sql + " and a.FName like '%" + searchMessage + "%'";
            }
            sql = sql + getPowerConst("jiayazhan");
            Hashtable table = BASE_JIAYAZHANDal.SearchJYZ(Grid1_pageIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
            Grid1.DataSource(table["data"], Grid1_fields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreJYZDelete")]
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
            BASE_JIAYAZHANDal.DeleteList(hasData);
            Bll.Map_MarkerBll.DeleteMarker(10, values);
            UpdateGrid(Grid1_fields, gridIndex, gridPageSize);
            return UIHelper.Result();
        }

        private void UpdateGrid(JArray Grid1_fields, int gridIndex, int gridPageSize)
        {
            var Grid1 = UIHelper.Grid("Grid1");
            string sql = string.Empty;
            sql = sql + getPowerConst("jiayazhan");
            Hashtable table = BASE_JIAYAZHANDal.SearchJYZ(gridIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
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
            sql = sql + getPowerConst("jiayazhan");
            Hashtable table = BASE_JIAYAZHANDal.SearchJYZ(gridIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
            Grid1.DataSource(table["data"], gridFields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));
            Grid1.PageSize(gridPageSize);

            return UIHelper.Result();
        }
        #endregion

        #region 加压站新增修改框架图
        [MyAuth(MenuPower = "CoreJYZNew")]
        public ActionResult JYZ_Diagram(string type)
        {
            string url = string.Empty;
            string jyzId = Request["jyzId"];
            if (type == "add")
            {
                url = Url.Content("~/BasicInfo/CompressorStation/JYZ_new");
                ViewBag.pageType = "add";
                ViewBag.TreeNodeShow = false;
            }
            else
            {
                url = Url.Content("~/BasicInfo/CompressorStation/JYZ_edit?id=" + jyzId);
                ViewBag.pageType = "edit";
                ViewBag.jyzId = jyzId;
                ViewBag.TreeNodeShow = true;
            }
            PageContext.RegisterStartupScript("F.ui.panelCenterRegion.setIFrameUrl('" + url + "');");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Tree1_NodeClick(string nodeId, string jyzId)
        {
            string url = string.Empty;
            switch (nodeId)
            {
                case "basic": url = Url.Content("~/BasicInfo/CompressorStation/JYZ_edit?id=" + jyzId); break;
                case "jyzJZ": url = Url.Content("~/BasicInfo/CompressorStation/JYZ_JZ?id=" + jyzId); ; break;
                case "jyzArchives": url = Url.Content("~/OpenWindow/BaseDA/PumpArchives?baseId=" + jyzId + "&pageType=jyzJZ"); ; break;
            }
            PageContext.RegisterStartupScript("F.ui.panelCenterRegion.setIFrameUrl('" + url + "');");
            return UIHelper.Result();
        }

        #endregion

        #region 加压站新增
        [MyAuth(MenuPower = "CoreJYZNew")]
        public ActionResult JYZ_new()
        {
            ViewBag.ddlCompanyDataSource = sys_DeptDal.GetCompanyList("");
            ViewBag.ddlProvinceDataSource = sys_dictDal.SearchDDL(" and FDictID=108");
            ViewBag.txtFCode = GetBillNo("BASE_JIAYAZHAN");
            if (GetUserType().Equals("3"))  //如果登录用户是客户
            {
                ViewBag.CustomerName = GetUserCustomerName();
                ViewBag.CustomerID = GetUserCustomer();
                ViewBag.MapTempID = GetCustomerMapTempID();
                ViewBag.KHReadOnly = true;
                ViewBag.JKLJ = true;
            }
            else
            {
                ViewBag.CustomerName = "";
                ViewBag.CustomerID = "0";
                ViewBag.KHReadOnly = false;
                ViewBag.JKLJ = false;
                ViewBag.MapTempID = "";
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
                    string guid = Guid.NewGuid().ToString();
                    string jyzId = Guid.NewGuid().ToString();
                    int FCustomerID = Convert.ToInt32(Request["tbxCustomerID"]);
                    Hashtable hasData = new Hashtable();
                    hasData["ID"] = jyzId;
                    hasData["FCode"] = Request["tbxFCode"];
                    hasData["FName"] = Request["tbxFName"];
                    hasData["FCustomerID"] = FCustomerID;
                    hasData["FCompanyNumber"] = Request["ddlCompany"];
                    hasData["FProvince"] = Request["ddlProvince"];
                    hasData["FCity"] = Request["ddlCity"];
                    hasData["FLngLat"] = Request["tbxLngLat"];
                    hasData["FAddress"] = Request["tbSelectedAddress"];
                    hasData["FXXAddress"] = Request["tbxAddress"];
                    hasData["FCreateUser"] = GetIdentityName();
                    hasData["FCreateDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    hasData["FIsDelete"] = 0;
                    hasData["FMarkerID"] = guid;
                    hasData["FJKLink"] = Request["tbxFJKLink"];
                    hasData["FNote"] = Request["tbxFNote"];
                    BASE_JIAYAZHANDal.Insert(hasData);

                    if (FCustomerID > 0)
                    {
                        //string FMapTempID = db.Panda_Customer.Where(x => x.ID == FCustomerID).FirstOrDefault().FMapTempID.ToString();
                        string[] lnglat = Request["tbxLngLat"].Split(',');
                        Hashtable has1 = new Hashtable();
                        Hashtable has2 = new Hashtable();
                        has1["ID"] = guid;
                        has1["FName"] = Request["tbxFName"];
                        has1["FMapTempID"] = Request["tbxFMapTempID"];
                        has2["FMarkerID"] = guid;
                        int maxid = Convert.ToInt32(Dal.DBUtil.SelectDataTable("select isnull(max(ID),0) as maxId from Map_MarkerProperty").Rows[0]["maxId"].ToString()) + 1;
                        has2["FAliasName"] = "Marker" + maxid.ToString();
                        has2["FType"] = "10";
                        has2["FMarker"] = "[{\"lng\":" + lnglat[0] + ",\"lat\":" + lnglat[1] + "}]";
                        has2["FMID"] = jyzId;
                        Bll.Map_MarkerBll.InsertMarker(has1);
                        Bll.Map_MarkerBll.InsertMarkerProperty(has2);
                    }

                    ShowNotify("成功！");
                    string url = Url.Action("JYZ_Diagram", "CompressorStation", new { type = "edit", jyzId = jyzId });
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

        #region 修改加压站
        [MyAuth(MenuPower = "CoreJYZEdit")]
        public ActionResult JYZ_edit(Guid ID)
        {
            BASE_JIAYAZHAN jyz = db.BASE_JIAYAZHAN.Find(ID);
            if (jyz == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (GetUserType().Equals("3"))  //如果登录用户是客户
                {
                    ViewBag.JKLJ = true;
                    ViewBag.KHReadOnly = true;
                }
                else
                {
                    ViewBag.JKLJ = false;
                    ViewBag.KHReadOnly = false;
                }
                ViewBag.ddlCompanyDataSource = sys_DeptDal.GetCompanyList("");
                ViewBag.ddlProvinceDataSource = sys_dictDal.SearchDDL(" and FDictID=108");
                ViewBag.ddlCityDataSource = sys_dictDal.SearchDDL(" and FDictID=120 and FParentValue='" + jyz.FProvince + "'");

                ViewBag.ddlCompanySelect = jyz.FCompanyNumber.ToString().Trim();
                ViewBag.ddlProvinceSelect = jyz.FProvince.ToString().Trim();
                ViewBag.ddlCitySelect = jyz.FCity.ToString().Trim();
                ViewBag.tbSelectedCustomer = jyz.Panda_Customer == null ? "" : jyz.Panda_Customer.Name;
                ViewBag.Panda_Customer = jyz.Panda_Customer;
                int customerid = jyz.Panda_Customer == null ? 0 : jyz.Panda_Customer.ID;
                ViewBag.hidSelectedCustomer = customerid.ToString();
                string FMapTempID = jyz.Panda_Customer == null ? "" : jyz.Panda_Customer.FMapTempID.ToString();
                ViewBag.FMapTempID = FMapTempID;
            }
            return View(jyz);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreJYZEdit")]
        public ActionResult btnBasicEdit_Click()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int FCustomerID = Convert.ToInt32(Request["tbxCustomerID"]);
                    Hashtable hasData = new Hashtable();
                    hasData["ID"] = Request["tbxID"];
                    hasData["FCode"] = Request["tbxFCode"];
                    hasData["FName"] = Request["tbxFName"];
                    hasData["FCustomerID"] = FCustomerID;
                    hasData["FCompanyNumber"] = Request["ddlCompany"];
                    hasData["FProvince"] = Request["ddlProvince"];
                    hasData["FCity"] = Request["ddlCity"];
                    hasData["FLngLat"] = Request["tbxLngLat"];
                    hasData["FAddress"] = Request["tbSelectedAddress"];
                    hasData["FXXAddress"] = Request["tbxAddress"];
                    hasData["FUpdUser"] = GetIdentityName();
                    hasData["FUpdDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    hasData["FJKLink"] = Request["tbxFJKLink"];
                    hasData["FNote"] = Request["tbxFNote"];
                    BASE_JIAYAZHANDal.Update(hasData);

                    if (FCustomerID > 0)
                    {
                        if (Request["tbxFMarkerID"].ToString() != "")
                        {
                            //string FMapTempID = db.Panda_Customer.Where(x => x.ID == FCustomerID).FirstOrDefault().FMapTempID.ToString();
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

        #region 加压站机组模块

        #region 加压站机组列表页面
        //
        // GET: /BasicInfo/Customer/
        [MyAuth(MenuPower = "CoreJYZJZView")]
        public ActionResult JYZ_JZ(Guid id)
        {
            ViewBag.CoreJYZJZNew = CheckPower("CoreJYZJZNew");
            ViewBag.CoreJYZJZDelete = CheckPower("CoreJYZJZDelete");
            ViewBag.CoreJYZJZEdit = CheckPower("CoreJYZJZEdit");
            ViewBag.jyzId = id.ToString();
            Hashtable table = BASE_JIAYAZHAN_JZDal.SearchJYZ(0, 20, "a.FCreateDate", "DESC", " and jyzId='" + id + "'");
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
            ViewBag.GridJZDataSource = table["data"];
            ViewBag.GridJZRecordCount = Int32.Parse(table["total"].ToString());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GridJZ_PageIndexChanged(JArray GridJZ_fields, int GridJZ_pageIndex, int gridPageSize, Guid jyzId, string searchMessage)
        {
            var GridJZ = UIHelper.Grid("GridJZ");

            string sql = string.Empty;
            if (!searchMessage.Equals(""))
            {
                sql = sql + " and jyzJZName like '%" + searchMessage + "%'";
            }
            sql = sql + " and jyzId = '" + jyzId + "'";
            Hashtable table = BASE_JIAYAZHAN_JZDal.SearchJYZ(GridJZ_pageIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
            GridJZ.DataSource(table["data"], GridJZ_fields);
            GridJZ.RecordCount(Int32.Parse(table["total"].ToString()));

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreJYZJZDelete")]
        public ActionResult GridJZ_Delete(JArray selectedRows, JArray GridJZ_fields, int gridIndex, int gridPageSize, Guid jyzId)
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
            BASE_JIAYAZHAN_JZDal.DeleteList(hasData);

            UpdateGridJZ(GridJZ_fields, gridIndex, gridPageSize, jyzId);
            return UIHelper.Result();
        }


        private void UpdateGridJZ(JArray GridJZ_fields, int gridIndex, int gridPageSize, Guid jyzId)
        {
            var GridJZ = UIHelper.Grid("GridJZ");
            string sql = string.Empty;
            sql = sql + " and jyzId = '" + jyzId + "'";
            Hashtable table = BASE_JIAYAZHAN_JZDal.SearchJYZ(gridIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
            GridJZ.DataSource(table["data"], GridJZ_fields);
            GridJZ.RecordCount(Int32.Parse(table["total"].ToString()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyCustomJZPostBack(string type, JArray gridFields, JObject typeParams, int gridIndex, int gridPageSize, Guid jyzId)
        {
            var GridJZ = UIHelper.Grid("GridJZ");
            string sql = string.Empty;
            sql = sql + " and jyzId = '" + jyzId + "'";
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
                sql = " and jyzJZName like '%" + triggerValue + "%'";
            }

            Hashtable table = BASE_JIAYAZHAN_JZDal.SearchJYZ(gridIndex, gridPageSize, "a.FCreateDate", "DESC", sql);
            GridJZ.DataSource(table["data"], gridFields);
            GridJZ.RecordCount(Int32.Parse(table["total"].ToString()));
            GridJZ.PageSize(gridPageSize);

            return UIHelper.Result();
        }
        #endregion

        #region 加压站机组新增页面
        [MyAuth(MenuPower = "CoreJYZJZNew")]
        public ActionResult JYZ_JZ_new(Guid jyzId)
        {
            ViewBag.ddlDqmsDataSource = sys_dictDal.SearchDDL(" and FDictID=138");   //读取模式
            ViewBag.ddlTypeDataSource = sys_dictDal.SearchDDL(" and FDictID=132");     //类型
            ViewBag.jyzId = jyzId.ToString();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreJYZJZNew")]
        public ActionResult btnJZCreate_Click()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (BASE_JIAYAZHAN_JZDal.Exist(" and DTUCode='" + Request["tbSelectedDTU"] + "'").Rows.Count == 0)
                    {
                        string jz_id = Guid.NewGuid().ToString();
                        Hashtable hasData = new Hashtable();
                        hasData["ID"] = jz_id;
                        hasData["DTUCode"] = Request["tbSelectedDTU"];
                        hasData["jyzId"] = Request["jyzId"];
                        hasData["FType"] = Request["ddlType"];
                        hasData["jyzJZName"] = Request["tbxName"];
                        hasData["jyzJZAddressList"] = Request["hidDZB"] == "" ? "0" : Request["hidDZB"];
                        hasData["jyzJZCollectPeriod"] = Request["txt_cjzq"];//采集周期
                        hasData["jyzJZCollectLength"] = Request["txt_cjcd"];//采集长度
                        hasData["jyzJZReadMode"] = Request["ddlDqms"];//读取模式
                        hasData["FCreateUser"] = GetIdentityName();
                        hasData["FCreateDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        hasData["FIsDelete"] = 0;
                        hasData["FSort"] = Request["txtFSort"];
                        BASE_JIAYAZHAN_JZDal.Insert(hasData);

                        ShowNotify("成功！");
                        // 关闭本窗体（触发窗体的关闭事件）
                        PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                    }
                    else
                    {
                        ShowNotify("加压站编号重复，请更换！");
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

        #region 加压站机组修改页面
        [MyAuth(MenuPower = "CoreJYZJZEdit")]
        public ActionResult JYZ_JZ_edit(Guid jzId)
        {
            BASE_JIAYAZHAN_JZ jz = db.BASE_JIAYAZHAN_JZ.Find(jzId);
            if (jz == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.ddlTypeDataSource = sys_dictDal.SearchDDL(" and FDictID=132");     //类型
                ViewBag.ddlDqmsDataSource = sys_dictDal.SearchDDL(" and FDictID=138");   //读取模式
                ViewBag.ddlDqmsSelect = jz.jyzJZReadMode.Trim().ToString();
                ViewBag.ddlTypeSelect = jz.FType.Trim().ToString();
            }
            return View(jz);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreJYZJZEdit")]
        public ActionResult btnJZEdit_Click()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (BASE_JIAYAZHAN_JZDal.Exist(" and DTUCode='" + Request["tbSelectedDTU"] + "' and DTUCode<>'" + Request["tbOldDTU"] + "'").Rows.Count == 0)
                    {
                        Hashtable hasData = new Hashtable();
                        hasData["ID"] = Request["tbxID"];
                        hasData["DTUCode"] = Request["tbSelectedDTU"];
                        hasData["jyzJZName"] = Request["tbxName"];
                        hasData["FType"] = Request["ddlType"];
                        hasData["jyzJZAddressList"] = Request["hidDZB"] == "" ? "0" : Request["hidDZB"];
                        hasData["jyzJZCollectPeriod"] = Request["txt_cjzq"];//采集周期
                        hasData["jyzJZCollectLength"] = Request["txt_cjcd"];//采集长度
                        hasData["jyzJZReadMode"] = Request["ddlDqms"];//读取模式
                        hasData["FUpdUser"] = GetIdentityName();
                        hasData["FUpdDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        hasData["FSort"] = Request["txtFSort"];
                        BASE_JIAYAZHAN_JZDal.Update(hasData);

                        ShowNotify("成功！");
                        // 关闭本窗体（触发窗体的关闭事件）
                        PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                    }
                    else
                    {
                        ShowNotify("加压站编号重复，请更换！");
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
            return UIHelper.Result();
        }
    }
}