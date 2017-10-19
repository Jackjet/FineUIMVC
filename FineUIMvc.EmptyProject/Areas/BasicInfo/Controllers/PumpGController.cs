using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FineUIMvc.PumpMVC.Controllers;
using FineUIMvc.PumpMVC.DAL;
using FineUIMvc.PumpMVC.Models;
using System.Collections;
using System.Data;
using Newtonsoft.Json.Linq;

namespace FineUIMvc.PumpMVC.Areas.BasicInfo.Controllers
{
    [Authorize]
    public class PumpGController : BaseController
    {
        private DBController db = new DBController();

        #region 泵房分组列表
        //
        // GET: /BasicInfo/PumpG/
          [MyAuth(MenuPower = "CorePumpGView")]
        public ActionResult Index()
        {
            ViewBag.CorePumpGNew = CheckPower("CorePumpGNew");
            ViewBag.CorePumpGDelete = CheckPower("CorePumpGDelete");
            ViewBag.CorePumpGEdit = CheckPower("CorePumpGEdit");

            if (GetUserType().Equals("1"))  //如果登录用户是管理员
            {
                ViewBag.Hidden = false;
            }
            else if (GetUserType().Equals("2"))  //如果登录用户是分公司
            {
                ViewBag.Hidden = true;
            }
            else if (GetUserType().Equals("3"))  //如果登录用户是客户
            {
                ViewBag.Hidden = true;
            }
            else                                //如果登录用户是其他用户（进不去这个页面）
            {
                ViewBag.Hidden = true;
            }

            DataTable dt = new DataTable();
            int count = 0;
            BindGrid1(0, 20, "", "", out dt, out count);
            ViewBag.Grid1DataSource = dt;
            ViewBag.Grid1RecordCount = count;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FZ_Grid1_DoPostBack(JArray Grid1_fields, int Grid1_pageIndex,
            string ttbSearchMessage, string ttbSearchCustomer, string ttbSearchCompany,
            int ddlGridPageSize, string actionType, JArray deleteIds)
        {
            var ttbSearchMessageUI = UIHelper.TwinTriggerBox("ttbSearchMessage");
            var ttbSearchCustomerUI = UIHelper.TwinTriggerBox("ttbSearchCustomer");
            var ttbSearchCompanyUI = UIHelper.TwinTriggerBox("ttbSearchCompany");
            string search = string.Empty;
            if (actionType == "trigger1")
            {
                ttbSearchMessageUI.Text(String.Empty);
                ttbSearchMessageUI.ShowTrigger1(false);
                ttbSearchMessage = String.Empty;

                ttbSearchCustomerUI.Text(String.Empty);
                ttbSearchCustomerUI.ShowTrigger1(false);
                ttbSearchCustomer = String.Empty;

                ttbSearchCompanyUI.Text(String.Empty);
                ttbSearchCompanyUI.ShowTrigger1(false);
                ttbSearchCompany = String.Empty;
            }
            else if (actionType == "trigger2")
            {
                ttbSearchMessageUI.ShowTrigger1(true);
                ttbSearchCustomerUI.Text(String.Empty);
                ttbSearchCustomerUI.ShowTrigger1(false);
                ttbSearchCustomer = String.Empty;
                ttbSearchCompanyUI.Text(String.Empty);
                ttbSearchCompanyUI.ShowTrigger1(false);
                ttbSearchCompany = String.Empty;
                search = ttbSearchMessage;
            }
            else if (actionType == "trigger3")
            {
                ttbSearchCustomerUI.ShowTrigger1(true);
                ttbSearchMessageUI.Text(String.Empty);
                ttbSearchMessageUI.ShowTrigger1(false);
                ttbSearchMessage = String.Empty;
                ttbSearchCompanyUI.Text(String.Empty);
                ttbSearchCompanyUI.ShowTrigger1(false);
                ttbSearchCompany = String.Empty;
                search = ttbSearchCustomer;
            }
            else if (actionType == "trigger4")
            {
                ttbSearchCompanyUI.ShowTrigger1(true);
                ttbSearchCustomerUI.Text(String.Empty);
                ttbSearchCustomerUI.ShowTrigger1(false);
                ttbSearchCustomer = String.Empty;
                ttbSearchMessageUI.Text(String.Empty);
                ttbSearchMessageUI.ShowTrigger1(false);
                ttbSearchMessage = String.Empty;
                search = ttbSearchCompany;
            }
            else if (actionType == "delete")
            {
                string values = "";
                foreach (int ID in deleteIds)
                {
                    values = values + ID.ToString() + ",";
                }
                values = values.Substring(0, values.LastIndexOf(','));
                Hashtable hasData = new Hashtable();
                hasData["ID"] = values;
                hasData["FIsDelete"] = 1;
                hasData["FDelDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ssss"));
                hasData["FDelUser"] = GetIdentityName();
                Panda_PumpFGDal.DeleteGroupList(hasData);
            }

            var grid1UI = UIHelper.Grid("Grid1");
            DataTable dt2 = new DataTable();
            int count = 0;
            BindGrid1(Grid1_pageIndex, ddlGridPageSize, search,actionType, out dt2, out count);
            grid1UI.DataSource(dt2, Grid1_fields);
            grid1UI.RecordCount(count);
            grid1UI.PageSize(ddlGridPageSize);
            return UIHelper.Result();
        }

        private void BindGrid1(int pageIndex, int pageSize, string selectTest,string actionType, out DataTable table, out int count)
        {
            string sql = string.Empty;
            if (!selectTest.Equals(""))
            {
                if (actionType == "trigger2")
                {
                    sql = sql + " and a.G_Name like '%" + selectTest + "%'";
                }
                else if (actionType == "trigger3")
                {
                    sql = sql + " and e.Name like '%" + selectTest + "%'";
                }
                else if (actionType == "trigger4")
                {
                    sql = sql + " and c.Fengongsi like '%" + selectTest + "%'";
                }
            }

            if (GetUserType().Equals("2"))
            {
                sql = sql + " and a.FCompanyNumber='" + GetUserCompanyNumber() + "'";
            }
            else if (GetUserType().Equals("3"))
            {
                sql = sql + " and a.FCustomerID=" + GetUserCustomer();
            }

            Hashtable has = Panda_PumpFGDal.SearchPumpFG(pageIndex, pageSize, "G_Type", "asc", sql);
            count = Int32.Parse(has["total"].ToString());
            table = (DataTable)has["data"];
        }
        #endregion

        #region 新增分组
        [MyAuth(MenuPower = "CorePumpGNew")]
        public ActionResult FZ_new()
        {
            ViewBag.ddlCompanyDataSource = sys_DeptDal.GetCompanyList("");
            if (GetUserType().Equals("2"))  //如果登录用户是分公司
            {
                ViewBag.CustomerName = "";
                ViewBag.CustomerID = "0";
                ViewBag.KHReadOnly = true;
                ViewBag.KHHidden = true;
                ViewBag.FGSReadOnly = true;
                ViewBag.FGSHidden = false;
                ViewBag.FGSSelectValue = GetUserCompanyNumber();
                ViewBag.SelectValue = "1";
            }
            else if (GetUserType().Equals("3"))  //如果登录用户是客户
            {
                ViewBag.CustomerName = GetUserCustomerName();
                ViewBag.CustomerID = GetUserCustomer();
                ViewBag.KHReadOnly = true;
                ViewBag.KHHidden = false;
                ViewBag.FGSReadOnly = true;
                ViewBag.FGSHidden = true;
                ViewBag.FGSSelectValue = "0";
                ViewBag.SelectValue = "2";
            }
            else
            {
                ViewBag.CustomerName = "";
                ViewBag.CustomerID = "0";
                ViewBag.KHReadOnly = false;
                ViewBag.KHHidden = false;
                ViewBag.FGSReadOnly = true;
                ViewBag.FGSHidden = true;
                ViewBag.FGSSelectValue = "0";
                ViewBag.SelectValue = "2";
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult btnCreate_Click()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Hashtable hasData = new Hashtable();
                    hasData["FCreateUser"] = Convert.ToInt32(GetIdentityName());
                    hasData["FCreateDate"] = DateTime.Now;
                    hasData["FIsDelete"] = 0;
                    hasData["G_Name"] = Request["tbxName"];
                    hasData["G_Type"] = Convert.ToInt32(Request["ddlType"]);
                    hasData["FCustomerID"] = Convert.ToInt32(Request["tbxCustomerID"]);
                    hasData["FCompanyNumber"] = Request["ddlCompany"];
                    Panda_PumpFGDal.Insert(hasData);
                    ShowNotify("添加成功！");
                    // 关闭本窗体（触发窗体的关闭事件）
                    PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                }
                catch
                {
                    ShowNotify("添加失败！");
                }

            }

            return UIHelper.Result();
        }
        #endregion

        #region 修改分组
        [MyAuth(MenuPower = "CorePumpGEdit")]
        public ActionResult FZ_edit(int groupId)
        {
            Panda_PumpFG fg = db.Panda_PumpFG.Find(groupId);
            if (fg == null)
            {
                return HttpNotFound();
            }
            else
            {
            }

            return View(fg);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult btnEdit_Click()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int FCustomerID = Convert.ToInt32(Request["tbxCustomerID"]);
                    Hashtable hasData = new Hashtable();
                    hasData["ID"] = Request["tbxID"];
                    hasData["G_Name"] = Request["tbxName"];
                    hasData["FUpdUser"] = GetIdentityName();
                    hasData["FUpdDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Panda_PumpFGDal.Update(hasData);

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

        #region 分组泵房
        [MyAuth(MenuPower = "CorePumpGNew")]
        public ActionResult FZ_pump(string groupId)
        {
            ViewBag.CorePumpGNew = CheckPower("CorePumpGNew");
            ViewBag.CorePumpGDelete = CheckPower("CorePumpGDelete");
            DataTable dt = new DataTable();
            int count = 0;
            BindGridGPump(0, 20, "",Convert.ToInt32(groupId), out dt, out count);
            ViewBag.GridGPumpDataSource = dt;
            ViewBag.GridGPumpRecordCount = count;
            ViewBag.groupId = groupId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GridGPump_DoPostBack(JArray GridGPump_fields, int GridGPump_pageIndex,
            string ttbSearchMessage, int ddlGridPageSize, string actionType, JArray deleteIds, int groupId)
        {
            var ttbSearchMessageUI = UIHelper.TwinTriggerBox("ttbSearchMessage");
            if (actionType == "trigger1")
            {
                ttbSearchMessageUI.Text(String.Empty);
                ttbSearchMessageUI.ShowTrigger1(false);
                ttbSearchMessage = String.Empty;
            }
            else if (actionType == "trigger2")
            {
                ttbSearchMessageUI.ShowTrigger1(true);
            }
            else if (actionType == "delete")
            {
                // 在操作之前进行权限检查
                if (!CheckPower("CorePumpGDelete"))
                {
                    CheckPowerFailWithAlert();
                    return UIHelper.Result();
                }

                //foreach (int ID in deleteIds)
                //{
                //    Panda_PumpFG_P item = db.Panda_PumpFG_P.Find(ID);
                //    db.Panda_PumpFG_P.Remove(item);
                //}
                //db.SaveChanges();
                string values = "";
                foreach (int ID in deleteIds)
                {
                    values = values + ID.ToString() + ",";
                }
                values = values.Substring(0, values.LastIndexOf(','));
                Panda_PumpFGDal.DeleteGroupPumpList(values);
                
            }

            var grid1UI = UIHelper.Grid("GridGPump");
            DataTable dt2 = new DataTable();
            int count = 0;
            BindGridGPump(GridGPump_pageIndex, ddlGridPageSize, ttbSearchMessage,groupId,  out dt2, out count);
            grid1UI.DataSource(dt2, GridGPump_fields);
            grid1UI.RecordCount(count);
            grid1UI.PageSize(ddlGridPageSize);
            return UIHelper.Result();
        }

        private void BindGridGPump(int pageIndex, int pageSize, string selectTest,int groupId, out DataTable table, out int count)
        {
            string sql = string.Empty;
            sql = sql + " and a.GroupID=" + groupId;
            if (!selectTest.Equals(""))
            {
                sql = sql + " and (b.PName like '%" + selectTest + "%' or b.PCustomPName like '%" + selectTest + "%')";
            }

            if (GetUserType().Equals("2"))
            {
                sql = sql + " and b.PCompanyNumber='" + GetUserCompanyNumber() + "'";
            }
            else if (GetUserType().Equals("3"))
            {
                sql = sql + " and b.FCustomerID=" + GetUserCustomer();
            }

            Hashtable has = Panda_PumpFGDal.SearchGroupPump(pageIndex, pageSize, "b.FCreateDate", "asc", sql);
            count = Int32.Parse(has["total"].ToString());
            table = (DataTable)has["data"];
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CorePumpGNew")]
        public ActionResult GridGPump_Insert(JArray GridGPump_fields, int gridIndex, int gridPageSize, int groupId, string idList)
        {
            string[] id = idList.Split(',');
            Panda_PumpFG_P pump = new Panda_PumpFG_P();
            pump.GroupID = groupId;
            for (int i = 0; i < id.Length; i++)
            {
                pump.PumpID = id[i];
                db.Panda_PumpFG_P.Add(pump);
                db.SaveChanges();
            }

            var grid1 = UIHelper.Grid("GridGPump");
            string sql = string.Empty;

            sql = sql + " and a.GroupID = " + groupId;
            Hashtable has = Panda_PumpFGDal.SearchGroupPump(gridIndex, gridPageSize, "b.FCreateDate", "asc", sql);
            grid1.DataSource(has["data"], GridGPump_fields);
            grid1.RecordCount(Int32.Parse(has["total"].ToString()));
            return UIHelper.Result();
        }
        #endregion
    }
}