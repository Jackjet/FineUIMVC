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

namespace FineUIMvc.PumpMVC.Areas.BasicInfo.Controllers
{
    [Authorize]
    public class CustomerController : BaseController
    {
        private DBController db = new DBController();

        #region Customers客户列表页面
        //
        // GET: /BasicInfo/Customer/
        [MyAuth(MenuPower = "CoreCustomerView")]
        public ActionResult Index()
        {
            ViewBag.CoreCustomerNew = CheckPower("CoreCustomerNew");
            ViewBag.CoreCustomerDelete = CheckPower("CoreCustomerDelete");
            ViewBag.CoreCustomerEdit = CheckPower("CoreCustomerEdit");
            Hashtable table = Panda_CustomerDal.Search(0, 20, "FCreateDate", "DESC", "");
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
                sql = sql + " and Name like '%" + searchMessage + "%'";
            }
            Hashtable table = Panda_CustomerDal.Search(Grid1_pageIndex, gridPageSize, "FCreateDate", "DESC", sql);
            Grid1.DataSource(table["data"], Grid1_fields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreCustomerDelete")]
        public ActionResult Grid1_Delete(JArray selectedRows, JArray Grid1_fields, int gridIndex, int gridPageSize)
        {
            //foreach (string rowId in selectedRows)
            //{
            //    Panda_Customer item = db.Panda_Customer.Find(Convert.ToInt32(rowId));
            //    db.Panda_Customer.Remove(item);
            //}
            //db.SaveChanges();

            //UpdateGrid(Grid1_fields, gridIndex, gridPageSize);

            //return UIHelper.Result();
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
            Panda_CustomerDal.DeleteList(hasData);

            UpdateGrid(Grid1_fields, gridIndex, gridPageSize);
            return UIHelper.Result();
        }

        private void UpdateGrid(JArray Grid1_fields, int gridIndex, int gridPageSize)
        {
            var Grid1 = UIHelper.Grid("Grid1");
            string sql = string.Empty;

            //sql = sql + " and FDictID = " + Grid1_selectedRows;
            Hashtable table = Panda_CustomerDal.Search(gridIndex, gridPageSize, "FCreateDate", "DESC", sql);
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
                sql = " and Name like '%" + triggerValue + "%'";
            }

            Hashtable table = Panda_CustomerDal.Search(gridIndex, gridPageSize, "FCreateDate", "DESC", sql);
            Grid1.DataSource(table["data"], gridFields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));
            Grid1.PageSize(gridPageSize);
            return UIHelper.Result();
        }
        #endregion

        #region 新增客户页面
        [MyAuth(MenuPower = "CoreCustomerNew")]
        public ActionResult Customer_new()
        {
            ViewBag.ddlCompanyDataSource = sys_DeptDal.GetCompanyList("");
            ViewBag.ddlTypeDataSource = sys_dictDal.SearchDDL(" and FDictID=50");
            ViewBag.ddlLevelDataSource = sys_dictDal.SearchDDL(" and FDictID=51");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreCustomerNew")]
        public ActionResult btnCreate_Click()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Panda_CustomerDal.Exist(" and Name='" + Request["tbxName"] + "'").Rows.Count == 0)
                    {
                        string guid = Guid.NewGuid().ToString();
                        Hashtable hasData = new Hashtable();
                        hasData["Name"] = Request["tbxName"];
                        hasData["FContactName"] = Request["tbxContactName"];
                        hasData["FContactPhone"] = Request["tbxContactPhone"];
                        hasData["FBGAddress"] = Request["tbxAddress"];
                        hasData["FAddress"] = Request["tbSelectedAddress"];
                        hasData["FCompanyNumber"] = Request["ddlCompany"];
                        hasData["CustomerType"] = Request["ddlType"];
                        hasData["CustomerLevel"] = Request["ddlGrade"];
                        hasData["MessageCount"] = Request["tbxMsgNumber"];
                        hasData["FLngLat"] = Request["tbxLngLat"];
                        hasData["Guid"] = Guid.NewGuid().ToString();
                        hasData["FIsDelete"] = 0;
                        hasData["FCreateUser"] = GetIdentityName();
                        hasData["FCreateDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ssss"));
                        hasData["FMapTempID"] = guid;
                        Panda_CustomerDal.Insert(hasData);

                        Hashtable has1 = new Hashtable();
                        Hashtable has2 = new Hashtable();
                        has1["id"] = guid;
                        has1["FMapTempName"] = Request["tbxName"];
                        has2["FMapTempID"] = guid;
                        int maxid = Convert.ToInt32(Dal.DBUtil.SelectDataTable("select isnull(max(ID),0) as maxId from Map_TempProperty").Rows[0]["maxId"].ToString()) + 1;
                        has2["FAliasName"] = "Template" + maxid.ToString();
                        has2["FCenter"] = Request["tbxLngLat"];
                        Bll.Map_TemplateBll.InsertTemplate(has1);
                        Bll.Map_TemplateBll.InsertTempProperty(has2);

                        ShowNotify("添加成功！");
                        // 关闭本窗体（触发窗体的关闭事件）
                        PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                    }
                    else
                    {
                        ShowNotify("客户名重复，请更换！");
                    }
                }
                catch
                {
                    ShowNotify("添加失败！");
                }

            }

            return UIHelper.Result();
        }

        #endregion

        #region 客户修改页面
        [MyAuth(MenuPower = "CoreCustomerEdit")]
        public ActionResult Customer_edit(int customerId)
        {
            Panda_Customer cus = db.Panda_Customer.Find(customerId);
            if (cus == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.ddlCompanyDataSource = sys_DeptDal.GetCompanyList("");
                ViewBag.ddlTypeDataSource = sys_dictDal.SearchDDL(" and FDictID=50");
                ViewBag.ddlLevelDataSource = sys_dictDal.SearchDDL(" and FDictID=51");
                ViewBag.ddlCompanySelect = cus.FCompanyNumber;
                ViewBag.ddlTypeSelect = cus.CustomerType.ToString();
                ViewBag.ddlLevelSelect = cus.CustomerLevel.ToString();
            }

            return View(cus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreCustomerEdit")]
        public ActionResult btnEdit_Click([Bind(Include = "ID,Name,FContactName,FContactPhone,FBGAddress,FAddress,FCompanyNumber,CustomerType,CustomerLevel,MessageCount,FLngLat")] Panda_Customer cus)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Panda_CustomerDal.Exist(" and Name='" + Request["tbxName"] + "' and Name<>'" + Request["tbxOldName"] + "'").Rows.Count == 0)
                    {
                        Hashtable hasData = new Hashtable();
                        hasData["ID"] = Request["tbxID"];
                        hasData["Name"] = Request["tbxName"];
                        hasData["FContactName"] = Request["tbxContactName"];
                        hasData["FContactPhone"] = Request["tbxContactPhone"];
                        hasData["FBGAddress"] = Request["tbxAddress"];
                        hasData["FAddress"] = Request["tbSelectedAddress"];
                        hasData["FCompanyNumber"] = Request["ddlCompany"];
                        hasData["CustomerType"] = Request["ddlType"];
                        hasData["CustomerLevel"] = Request["ddlGrade"];
                        hasData["MessageCount"] = Request["tbxMsgNumber"];
                        hasData["FLngLat"] = Request["tbxLngLat"];
                        hasData["FUpdUser"] = GetIdentityName();
                        hasData["FUpdDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ssss"));
                        Panda_CustomerDal.Update(hasData);

                        if(Request["tbxFMapTempID"].ToString()!="")
                        {
                            Hashtable has1 = new Hashtable();
                            has1["id"] = Request["tbxFMapTempID"];
                            has1["FMapTempName"] = Request["tbxName"];
                            Bll.Map_TemplateBll.UpdateTemp(has1);
                            Hashtable has2 = new Hashtable();
                            has2["FMapTempID"] = Request["tbxFMapTempID"];
                            has2["FCenter"] = Request["tbxLngLat"];
                            Bll.Map_TemplateBll.UpdateTempProperty(has2);
                        }
                        
                        ShowNotify("修改成功！");
                        // 关闭本窗体（触发窗体的关闭事件）
                        PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                    }
                    else
                    {
                        ShowNotify("客户名重复，请更换！");
                    }
                }
            }
            catch
            {
                ShowNotify("修改失败！");
            }

            return UIHelper.Result();
        }
        #endregion

        #region 客户公共信息系统Url地址修改页面
        [MyAuth(MenuPower = "CoreCustomerEdit")]
        public ActionResult Customer_editUrl(int customerId)
        {
            ViewBag.CoreCustomerNew = CheckPower("CoreCustomerNew");
            ViewBag.CoreCustomerDelete = CheckPower("CoreCustomerDelete");
            ViewBag.CoreCustomerEdit = CheckPower("CoreCustomerEdit");
            string sql = string.Empty;
            sql = "and  pct.CustomerID=" + customerId;
            Hashtable table = Panda_CustomerDal.SearchUrl(0, 20, "FCreateDate", "DESC", sql);
            ViewBag.Grid1DataSource = table["data"];
            ViewBag.Grid1RecordCount = Int32.Parse(table["total"].ToString());
            ViewBag.customerID = customerId.ToString();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyUrlCustomPostBack(string type, JArray gridFields, JObject typeParams, int gridIndex, int gridPageSize, string customerID)
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
                sql = " and Name like '%" + triggerValue + "%'";
            }
            sql = "and  pct.CustomerID=" + customerID;
            Hashtable table = Panda_CustomerDal.SearchUrl(gridIndex, gridPageSize, "FCreateDate", "DESC", sql);
            Grid1.DataSource(table["data"], gridFields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));
            Grid1.PageSize(gridPageSize);
            return UIHelper.Result();
        }
        /// <summary>
        /// 新增客户公共信息系统Url地址
        /// </summary>
        /// <returns></returns>
        [MyAuth(MenuPower = "CoreCustomerNew")]
        public ActionResult CustomerUrl_new(int customerID)
        {
            ViewBag.ddlTypeDataSource = Panda_CustomerDal.SearchMenusDDL("");
            ViewBag.customerID = customerID.ToString();
            return View();
        }
    
        /// <summary>
        /// 新增客户公共信息系统Url地址
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreCustomerNew")]
        public ActionResult btnUrlCreate_Click()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Panda_CustomerDal.ExistUrl(" and TopMenuID='" + Request["ddlTopMenuID"] + "' and CustomerID='" + Request["customerID"] + "'").Rows.Count == 0)
                    {
                        Hashtable hasData = new Hashtable();
                        hasData["TopMenuID"] = Request["ddlTopMenuID"];
                        hasData["NavigateUrl"] = Request["tbxNavigateUrl"];
                        hasData["CustomerID"] = Request["customerID"];
                        hasData["FIsDelete"] = 0;
                        hasData["FCreateUser"] = GetIdentityName();
                        hasData["FCreateDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ssss"));
                        Panda_CustomerDal.InsertUrl(hasData);
                        ShowNotify("添加成功！");
                        // 关闭本窗体（触发窗体的关闭事件）
                        PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                    }
                    else
                    {
                        ShowNotify("该系统网址已添加！");
                    }
                }
                catch
                {
                    ShowNotify("添加失败！");
                }

            }

            return UIHelper.Result();
        }
        /// <summary>
        /// 修改客户公共信息系统Url地址
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [MyAuth(MenuPower = "CoreCustomerEdit")]
        public ActionResult CustomerUrl_edit(int ID)
        {
            Panda_CustomerTopMenu cus = db.Panda_CustomerTopMenu.Find(ID);
            if (cus == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.ddlTypeDataSource = Panda_CustomerDal.SearchMenusDDL("");
                ViewBag.ddlTypeSelect = cus.TopMenuID.Value.ToString();
            }

            return View(cus);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreCustomerEdit")]
        public ActionResult btnUrlEdit_Click([Bind(Include = "ID,TopMenuID,NavitateUrl")] Panda_CustomerTopMenu cus)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Hashtable hasData = new Hashtable();
                    hasData["ID"] = Request["tbxID"];
                    hasData["TopMenuID"] = Request["ddlTopMenuID"];
                    hasData["NavigateUrl"] = Request["tbxNavigateUrl"];
                    hasData["FUpdateUser"] = GetIdentityName();
                    hasData["FUpdateDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ssss"));
                    Panda_CustomerDal.UpdateUrl(hasData);
                    ShowNotify("修改成功！");
                    // 关闭本窗体（触发窗体的关闭事件）
                    PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                }
            }
            catch
            {
                ShowNotify("修改失败！");
            }

            return UIHelper.Result();
        }

        #endregion

        #region 管网页面
        public ActionResult Customer_GuanW(int customerId)
        {
            Panda_Customer cus = db.Panda_Customer.Find(customerId);
            if (cus == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.mapTempID = cus.FMapTempID.ToString();
            }
            return View();
        }
        #endregion
    }
}