using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using FineUIMvc;
using FineUIMvc.PumpMVC.Controllers;
using FineUIMvc.PumpMVC.DAL;
using FineUIMvc.PumpMVC.Models;
using System.Collections;
using Newtonsoft.Json.Linq;

namespace FineUIMvc.PumpMVC.Areas.Admin.Controllers
{
    [Authorize]
    public class Role_PowerController : BaseController
    {
        private DBController db = new DBController();
        private Dictionary<string, bool> _currentRolePowers = new Dictionary<string, bool>();
        //
        // GET: /Admin/Role_Power/
        [MyAuth(MenuPower = "CoreRolePowerView")]
        public ActionResult Index()
        {
            ViewBag.CoreRolePowerEdit = CheckPower("CoreRolePowerEdit");
            string sql = string.Empty;
            if(GetUserType().Equals("3"))  //如果登录用户是客户
            {
                sql = sql + " and RType=1 and FCustomerID=" + GetUserCustomer();
                ViewBag.Hidden = true;
            }
            else
            {
                sql = sql + " and RType=0 ";
                ViewBag.Hidden = false;
            }
            DataTable dt = sys_rolesDal.SearchTable(sql);
            if(dt.Rows.Count==0)
            {
                ViewBag.Grid1DataSource = null;
                ViewBag.Grid2DataSource = null;
                return View();
            }
            else
            {
                string grid1SelectedRowID = dt.Rows[0]["ID"].ToString();
                ViewBag.Grid1DataSource = dt;
                ViewBag.Grid1SelectedRowID = grid1SelectedRowID;
                return View(RolePower_LoadData(grid1SelectedRowID));
            }
        }

        private List<GroupPowerViewModel> RolePower_LoadData(string grid1SelectedRowID)
        {
            // 当前选中角色拥有的权限列表
            ViewBag.RolePowerIds = RolePower_GetRolePowerIds(grid1SelectedRowID);
            return RolePower_GetData();
        }

        private string RolePower_GetRolePowerIds(string grid1SelectedRowID)
        {
            //// 当前选中角色拥有的权限列表
            DataTable dt = Sys_PowersDal.SearchRolePowerTable(" RoleID=" + grid1SelectedRowID);
            var q = from z in dt.AsEnumerable()
                    select new
                    {
                        RoleID = z.Field<int>("RoleID"),
                        PowerID = z.Field<int>("PowerID")
                    };
            return new JArray(q.Select(x=>x.PowerID)).ToString(Newtonsoft.Json.Formatting.None);
        }

        private List<GroupPowerViewModel> RolePower_GetData()
        {
            string sql1 = string.Empty;
            string sql2 = string.Empty;
            if (GetUserType().Equals("3"))  //如果登录用户是客户
            {
                sql1 = sql1 + " IsCustomerLook=1";
                sql2 = sql2 + " and b.IsCustomerLook=1";
            }
            Hashtable tableGroup = Sys_PowersDal.SearchGroupName(0, 0, "GroupName", "ASC", sql1);
            DataTable dtGroup = (DataTable)tableGroup["data"];

            List<GroupPowerViewModel> groupPowers = new List<GroupPowerViewModel>();
            for (int i = 0; i < dtGroup.Rows.Count; i++)
            {
                var groupPower = new GroupPowerViewModel();
                groupPower.GroupName = dtGroup.Rows[i][0].ToString();
                Hashtable table = Sys_PowersDal.SearchGroupList(0, 0, "Name", "DESC", " and a.GroupName='" + dtGroup.Rows[i][0].ToString() + "'" + sql2);
                  DataTable dt = (DataTable)table["data"];
                JArray ja = new JArray();
                for (int j=0; j < dt.Rows.Count; j++)
                {
                    JObject jo = new JObject();
                    jo.Add("id", dt.Rows[j]["ID"].ToString());
                    jo.Add("name", dt.Rows[j]["Name"].ToString());
                    jo.Add("title", dt.Rows[j]["Title"].ToString());
                    ja.Add(jo);
                }
                groupPower.Powers = ja;

                groupPowers.Add(groupPower);
            }
            return groupPowers;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grid1_RowSelect(string selectedRowId, JArray Grid2_fields)
        {
            // 当前选中角色拥有的权限列表
            _currentRolePowers.Clear();
            Hashtable table = Sys_PowersDal.SearchRolePower(0, 0, "Name", "DESC", " and RoleID=" + selectedRowId);
            DataTable dt = (DataTable)table["data"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                string powerName = dt.Rows[i]["Name"].ToString(); ;
                if (!_currentRolePowers.ContainsKey(powerName))
                {
                    _currentRolePowers.Add(powerName, true);
                }
            }

            Hashtable tableGroup = Sys_PowersDal.SearchGroupName(0, 0, "GroupName", "ASC", "");
            DataTable dtGroup = (DataTable)tableGroup["data"];
            UIHelper.Grid("Grid2").DataSource(dtGroup, Grid2_fields);
            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreRolePowerEdit")]
        public ActionResult RolePower_Grid2_DoPostBack(JArray Grid1_fields, JArray Grid2_fields, string Grid2_sortField, string Grid2_sortDirection,
            string actionType, string ttbSearchCustomer, int selectedRoleId, JArray selectedPowerIds)
        {
            // 保存角色权限时，不需要重新加载表格数据
             if (actionType == "ttbSearchCustomer")
            {
                string sql1 = "";
                if(!ttbSearchCustomer.Equals(""))
                {
                    sql1 = " and RType=1 and b.Name='" + ttbSearchCustomer + "'";
                }
                else
                {
                    sql1 = " and RType=0";
                }
                DataTable dt1 = sys_rolesDal.SearchTable(sql1);
                if (dt1.Rows.Count > 0)
                {
                    selectedRoleId = Convert.ToInt32(dt1.Rows[0]["ID"].ToString());
                }
                else
                {
                    selectedRoleId = 0;
                }

                var grid1UI = UIHelper.Grid("Grid1");
                grid1UI.DataSource(dt1, Grid1_fields);
                grid1UI.SelectedRowIndexArray(0);
                PageContext.RegisterStartupScript("updateRolePowers(" + RolePower_GetRolePowerIds(selectedRoleId.ToString()) + ");");
                return UIHelper.Result();
            }
            else if (actionType == "saveall")
            {
                if (selectedRoleId==0)
                {
                    Alert.ShowInTop("请选择角色再进行权限设置！");
                }   
                else
                {
                    //// 在操作之前进行权限检查
                    if (!CheckPower("CoreRolePowerEdit"))
                    {
                        CheckPowerFailWithAlert();
                        return UIHelper.Result();
                    }

                    // 当前角色新的权限列表
                    int[] newPowerIDs = selectedPowerIds.ToObject<int[]>();

                    Sys_PowersDal dal = new Sys_PowersDal();
                    dal.DeleteRowerPower(selectedRoleId);

                    foreach (var item in newPowerIDs)
                    {
                        Hashtable hasData = new Hashtable();

                        hasData["RoleID"] = selectedRoleId;//tbxName.Text;
                        hasData["PowerID"] = item;//tbxGroupName.Text;

                        Sys_PowersDal.InsertRolePowers(hasData);
                    }

                    Alert.ShowInTop("当前角色的权限更新成功！");
                }
                
            }
            else
            {
                var grid2UI = UIHelper.Grid("Grid2");
             
                grid2UI.DataSource(RolePower_GetData(), Grid2_fields);

                // 更新当前角色的权限
                PageContext.RegisterStartupScript("updateRolePowers(" + RolePower_GetRolePowerIds(selectedRoleId.ToString()) + ");");
            }

            return UIHelper.Result();
        }
	}
}