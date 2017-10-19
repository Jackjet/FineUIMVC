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
    public class UserController : BaseController
    {
        private DBController db = new DBController();

        #region 用户列表页面
        [MyAuth(MenuPower = "CoreUserView")]
        public ActionResult Index()
        {
            ViewBag.CoreUserNew = CheckPower("CoreUserNew");
            ViewBag.CoreUserDelete = CheckPower("CoreUserDelete");
            ViewBag.CoreUserEdit = CheckPower("CoreUserEdit");
            string sql = string.Empty;
            if (GetUserType().Equals("3"))  //如果登录用户是客户
            {
                sql = sql + " and a.FCustomerID=" + GetUserCustomer();
            }
            Hashtable table = Panda_UserInfoDal.Search(0, 20, "FCreateDate", "DESC", sql);
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
            if (GetUserType().Equals("3"))  //如果登录用户是客户
            {
                sql = sql + " and a.FCustomerID=" + GetUserCustomer();
            }
            if (!searchMessage.Equals(""))
            {
                sql = sql + " and UserName like '%" + searchMessage + "%'";
            }
            Hashtable table = Panda_UserInfoDal.Search(Grid1_pageIndex, gridPageSize, "FCreateDate", "DESC", sql);
            Grid1.DataSource(table["data"], Grid1_fields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreUserDelete")]
        public ActionResult Grid1_Delete(JArray selectedRows, JArray Grid1_fields, int gridIndex, int gridPageSize)
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
            Panda_UserInfoDal.DeleteList(hasData);

            UpdateGrid(Grid1_fields, gridIndex, gridPageSize);
            return UIHelper.Result();
        }

        private void UpdateGrid(JArray Grid1_fields, int gridIndex, int gridPageSize)
        {
            var Grid1 = UIHelper.Grid("Grid1");
            string sql = string.Empty;
            if (GetUserType().Equals("3"))  //如果登录用户是客户
            {
                sql = sql + " and a.FCustomerID=" + GetUserCustomer();
            }
            Hashtable table = Panda_UserInfoDal.Search(gridIndex, gridPageSize, "FCreateDate", "DESC", sql);
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
                sql = " and UserName like '%" + triggerValue + "%'";
            }
            if (GetUserType().Equals("3"))  //如果登录用户是客户
            {
                sql = sql + " and a.FCustomerID=" + GetUserCustomer();
            }
            Hashtable table = Panda_UserInfoDal.Search(gridIndex, gridPageSize, "FCreateDate", "DESC", sql);
            Grid1.DataSource(table["data"], gridFields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));
            Grid1.PageSize(gridPageSize);
            return UIHelper.Result();
        }
        #endregion

        #region 新增用户页面
        [MyAuth(MenuPower = "CoreUserNew")]
        public ActionResult User_new()
        {
            ViewBag.ddlCompanyDataSource = sys_DeptDal.GetCompanyList("");
            ViewBag.ddlSexDataSource = sys_dictDal.SearchDDL(" and FDictID=151"); // 性别
            ViewBag.ddlEnableDataSource = sys_dictDal.SearchDDL(" and FDictID=54");//是否启用
            ViewBag.ddlTypeDataSource = sys_dictDal.SearchDDL(" and FDictID=53"); // 用户类型
            if (GetUserType().Equals("3"))  //如果登录用户是客户
            {
                ViewBag.CustomerName = GetUserCustomerName();
                ViewBag.CustomerID = GetUserCustomer();
                ViewBag.ReadOnly = true;
                ViewBag.SelectValue = "3";
                ViewBag.Hidden = false;
            }
            else
            {
                ViewBag.CustomerName = "";
                ViewBag.CustomerID = "0";
                ViewBag.ReadOnly = false;
                ViewBag.SelectValue = "1";
                ViewBag.Hidden = true;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreUserNew")]
        public ActionResult btnCreate_Click()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Panda_UserInfoDal.Exist(" and UserName='" + Request["tbxName"] + "'").Rows.Count == 0)
                    {
                        Hashtable hasData = new Hashtable();
                        hasData["FCompanyNumber"] = Request["ddlCompany"];
                        hasData["UserName"] = Request["tbxName"];
                        hasData["UserPwd"] = PasswordUtil.CreateDbPassword(Request["tbxPassword"]);
                        hasData["UserSex"] = Request["ddlSex"];
                        hasData["UserBirthday"] = Request["dpBirthday"];
                        //hasData["UserTel"] = Request["ddlCompany"];
                        hasData["UserMail"] = Request["tbxEmail"];
                        hasData["UserEnabledisable"] = Request["ddlEnable"];
                        hasData["UserRemark"] = Request["tbxRemark"];
                        hasData["UserPumpGroup"] = Request["tbxPumpGroupID"];
                        hasData["FCustomerID"] = Request["tbxCustomerID"];
                        hasData["UserType"] = Request["ddlType"];
                        hasData["IsOther"] = Request["ddlType"] == "4" ? "1" : "0";
                        hasData["FIsDelete"] = 0;
                        hasData["FCreateUser"] = GetIdentityName();
                        hasData["FCreateDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ssss"));
                        Panda_UserInfoDal.Insert(hasData);

                        ShowNotify("添加成功！");
                        // 关闭本窗体（触发窗体的关闭事件）
                        PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                    }
                    else
                    {
                        ShowNotify("用户名重复，请更换！");
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

        #region 编辑用户页面
        [MyAuth(MenuPower = "CoreUserEdit")]
        public ActionResult User_edit(int userId)
        {
            Panda_UserInfo user = db.Panda_UserInfo.Find(userId);
            if (user == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.ddlCompanyDataSource = sys_DeptDal.GetCompanyList("");
                ViewBag.ddlSexDataSource = sys_dictDal.SearchDDL(" and FDictID=151"); // 性别
                ViewBag.ddlTypeDataSource = sys_dictDal.SearchDDL(" and FDictID=53"); // 用户类型
                ViewBag.ddlEnableDataSource = sys_dictDal.SearchDDL(" and FDictID=54");//是否启用
                string sql = string.Empty;
                if (GetUserType().Equals("3"))  //如果登录用户是客户
                {
                    ViewBag.ReadOnly = true;
                }
                else
                {
                    ViewBag.ReadOnly = false;
                }
                ViewBag.ddlCompanySelect = user.FCompanyNumber;
                ViewBag.ddlSexSelect = user.UserSex.ToString();
                ViewBag.ddlTypeSelect = user.UserType.ToString();
                ViewBag.ddlEnableSelect = user.UserEnabledisable.ToString();

                ViewBag.tbSelectedCustomer = user.Panda_Customer == null ? "" : user.Panda_Customer.Name;
                ViewBag.tbSelectedPumpGroup = user.Panda_PGroup == null ? "" : user.Panda_PGroup.GroupName;
                int customerid = user.Panda_Customer == null ? 0 : user.Panda_Customer.ID;
                int groupid = user.Panda_PGroup == null ? 0 : user.Panda_PGroup.GroupID;
                ViewBag.hidSelectedCustomer = customerid.ToString();
                ViewBag.hidSelectedPumpGroup = groupid.ToString();
                ViewBag.oldUserPwd = user.UserPwd;
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreUserEdit")]
        public ActionResult btnEdit_Click([Bind(Include = "ID,FCompanyNumber,UserName,UserPwd,UserSex,UserBirthday,UserMail,UserEnabledisable,UserRemark,UserPumpGroup,FCustomerID,UserType,IsOther")] Panda_Customer cus)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Panda_UserInfoDal.Exist(" and UserName='" + Request["tbxName"] + "' and UserName<>'" + Request["tbxOldName"] + "'").Rows.Count == 0)
                    {
                        Hashtable hasData = new Hashtable();
                        hasData["ID"] = Request["tbxID"];
                        hasData["FCompanyNumber"] = Request["ddlCompany"];
                        hasData["UserName"] = Request["tbxName"];
                        if (!Request["tbxPassword"].Equals(Request["tbxoldPwd"]))
                        {
                            hasData["UserPwd"] = PasswordUtil.CreateDbPassword(Request["tbxPassword"]);
                        }
                        hasData["UserSex"] = Request["ddlSex"];
                        hasData["UserBirthday"] = Request["dpBirthday"];
                        //hasData["UserTel"] = Request["ddlCompany"];
                        hasData["UserMail"] = Request["tbxEmail"];
                        hasData["UserEnabledisable"] = Request["ddlEnable"];
                        hasData["UserRemark"] = Request["tbxRemark"];
                        hasData["UserPumpGroup"] = Request["tbxPumpGroupID"];
                        hasData["FCustomerID"] = Request["tbxCustomerID"];
                        hasData["UserType"] = Request["ddlType"];
                        hasData["IsOther"] = Request["ddlType"] == "4" ? "1" : "0";
                        hasData["FUpdUser"] = GetIdentityName();
                        hasData["FUpdDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ssss"));
                        Panda_UserInfoDal.Update(hasData);
                        ShowNotify("修改成功！");
                        // 关闭本窗体（触发窗体的关闭事件）
                        PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                    }
                    else
                    {
                        ShowNotify("用户名重复，请更换！");
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
    }
}