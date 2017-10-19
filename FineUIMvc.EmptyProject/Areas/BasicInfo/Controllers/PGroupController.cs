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

namespace FineUIMvc.PumpMVC.Areas.BasicInfo.Controllers
{
    [Authorize]
    public class PGroupController : BaseController
    {
        private DBController db = new DBController();

        #region 泵房组列表
        //
        // GET: /BasicInfo/PGroup/
         [MyAuth(MenuPower = "CorePumpGroupView")]
        public ActionResult Index()
        {
            ViewBag.CorePumpGroupNew = CheckPower("CorePumpGroupNew");
            ViewBag.CorePumpGroupDelete = CheckPower("CorePumpGroupDelete");
            string sql = string.Empty;
            DataTable dt = Panda_PGroupDal.SearchPGroup("");
            ViewBag.Grid1DataSource = dt;
            if(dt.Rows.Count>0)
            {
                ViewBag.Grid1SelectedRowID = dt.Rows[0]["GroupID"].ToString();
                sql = sql + " and a.GroupID = " + dt.Rows[0]["GroupID"].ToString();
                Hashtable table = Panda_PGroupDal.SearchGroupPump(0, 20, "PCode", "DESC", sql);
                ViewBag.Grid2DataSource = table["data"];
                ViewBag.Grid2RecordCount = Int32.Parse(table["total"].ToString());
            }
             else
            {
                ViewBag.Grid2DataSource = null;
                ViewBag.Grid2RecordCount = 0;
            }
     
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grid1_RowSelect(string selectedRowId, int gridIndex, int gridPageSize, JArray Grid2_fields)
        {
            string sql = string.Empty;

            sql = sql + " and a.GroupID = " + selectedRowId;
            Hashtable table = Panda_PGroupDal.SearchGroupPump(gridIndex, gridPageSize, "PCode", "DESC", sql);
            UIHelper.Grid("Grid2").DataSource(table["data"], Grid2_fields);
            UIHelper.Grid("Grid2").RecordCount(Int32.Parse(table["total"].ToString()));
            return UIHelper.Result();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grid2_PageIndexChanged(string selectedRowId, JArray Grid2_fields, int Grid2_pageIndex, int gridPageSize)
        {
            var grid2 = UIHelper.Grid("Grid2");

            if (selectedRowId.Equals("-1"))
            {
                ViewBag.Grid2DataSource = null;
                ViewBag.Grid2RecordCount = 0;
                grid2.DataSource(null, Grid2_fields);
                grid2.RecordCount(0);
            }
            else
            {
                string sql = string.Empty;

                sql = sql + " and a.GroupID = " + selectedRowId;
                Hashtable table = Panda_PGroupDal.SearchGroupPump(Grid2_pageIndex, gridPageSize, "PCode", "DESC", sql);
                grid2.DataSource(table["data"], Grid2_fields);
                grid2.RecordCount(Int32.Parse(table["total"].ToString()));
            }

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CorePumpGroupDelete")]
        public ActionResult Grid1_Delete(JArray selectedRows, JArray Grid1_fields, JArray Grid2_fields, int gridIndex, int gridPageSize)
        {
            foreach (string rowId in selectedRows)
            {
                Hashtable hasData = new Hashtable();
                hasData["GroupID"] = rowId;
                hasData["FIsDelete"] = 1;
                hasData["FDelDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ssss"));
                hasData["FDelUser"] = GetIdentityName();
                Panda_PGroupDal.Update(hasData);
            }
            //db.SaveChanges();

            DataTable dt = Panda_PGroupDal.SearchPGroup("");

            var grid1 = UIHelper.Grid("Grid1");
            grid1.DataSource(dt, Grid1_fields);
            grid1.SelectedRowIDArray(new string[] { "0" });

            UpdateGrid(Grid2_fields, gridIndex, gridPageSize, dt.Rows[0]["GroupID"].ToString());

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CorePumpGroupDelete")]
        public ActionResult Grid2_Delete(JArray selectedRows, JArray Grid2_fields, int gridIndex, int gridPageSize, string Grid1_selectedRows)
        {
            foreach (string rowId in selectedRows)
            {
                Panda_GroupPump item = db.Panda_GroupPump.Find(Convert.ToInt32(rowId));
                db.Panda_GroupPump.Remove(item);
            }
            db.SaveChanges();

            UpdateGrid(Grid2_fields, gridIndex, gridPageSize, Grid1_selectedRows);

            return UIHelper.Result();
        }

        private void UpdateGrid(JArray Grid2_fields, int gridIndex, int gridPageSize, string Grid1_selectedRows)
        {
            var grid2 = UIHelper.Grid("Grid2");
            string sql = string.Empty;

            sql = sql + " and a.GroupID = " + Grid1_selectedRows;
            Hashtable table = Panda_PGroupDal.SearchGroupPump(gridIndex, gridPageSize, "PCode", "DESC", sql);
            grid2.DataSource(table["data"], Grid2_fields);
            grid2.RecordCount(Int32.Parse(table["total"].ToString()));
        }

        private void UpdateGrid(JArray Grid1_fields)
        {
            var grid1 = UIHelper.Grid("Grid1");
            string sql = string.Empty;
            DataTable table = Panda_PGroupDal.SearchPGroup("");
            grid1.DataSource(table, Grid1_fields);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ddlGridPageSize_SelectedIndexChanged(string ddlGridPageSize, string ddlGridPageSize_text, int gridIndex, string selectedRowId, JArray Grid2_fields)
        {
            var grid2 = UIHelper.Grid("Grid2");
            string sql = string.Empty;

            sql = sql + " and a.GroupID = " + selectedRowId;
            Hashtable table = Panda_PGroupDal.SearchGroupPump(gridIndex, int.Parse(ddlGridPageSize), "PCode", "DESC", sql);
            grid2.DataSource(table["data"], Grid2_fields);
            grid2.RecordCount(Int32.Parse(table["total"].ToString()));
            grid2.PageSize(int.Parse(ddlGridPageSize));
            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Window1_Close(JArray Grid2_fields, int gridIndex, int gridPageSize, string Grid1_selectedRows)
        {
            UpdateGrid(Grid2_fields, gridIndex, gridPageSize, Grid1_selectedRows);
            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Window2_Close(JArray Grid1_fields)
        {
            UpdateGrid(Grid1_fields);
            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CorePumpGroupNew")]
        public ActionResult Grid2_Insert(JArray Grid2_fields, int gridIndex, int gridPageSize, int Grid1_selectedRows, string idList)
        {
            string[] id = idList.Split(',');
            Panda_GroupPump pump = new Panda_GroupPump();
            pump.GroupID = Grid1_selectedRows;
            for (int i = 0; i < id.Length;i++ )
            {
                pump.PumpID = id[i];
                db.Panda_GroupPump.Add(pump);
                db.SaveChanges();
            }

            var grid2 = UIHelper.Grid("Grid2");
            string sql = string.Empty;

            sql = sql + " and a.GroupID = " + Grid1_selectedRows;
            Hashtable table = Panda_PGroupDal.SearchGroupPump(gridIndex, gridPageSize, "PCode", "DESC", sql);
            grid2.DataSource(table["data"], Grid2_fields);
            grid2.RecordCount(Int32.Parse(table["total"].ToString()));
            return UIHelper.Result();
        }

        #endregion

        #region 新增泵房组
        [MyAuth(MenuPower = "CorePumpGroupNew")]
        public ActionResult PGroup_new()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CorePumpGroupNew")]
        public ActionResult btnGroupNew_Click()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Panda_PGroup group = new Panda_PGroup();
                    group.FCreateUser = Convert.ToInt32(GetIdentityName());
                    group.FCreateDate = DateTime.Now;
                    group.FIsDelete = 0;
                    group.GroupName = Request["tbxFName"];
                    db.Panda_PGroup.Add(group);
                    db.SaveChanges();

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

        #region 修改泵房组
        [MyAuth(MenuPower = "CorePumpGroupEdit")]
        public ActionResult PGroup_edit()
        {
            return View();
        }
        #endregion
    }
}