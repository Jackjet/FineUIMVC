using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FineUIMvc.PumpMVC.Controllers;
using FineUIMvc.PumpMVC.DAL;
using FineUIMvc.PumpMVC.Models;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Data;
using System.Data.Entity;

namespace FineUIMvc.PumpMVC.Areas.Admin.Controllers
{
    [Authorize]
    public class AlarmParmController : BaseController
    {
        private DBController db = new DBController();
        //
        // GET: /Admin/AlarmParm/
         [MyAuth(MenuPower = "CoreAdmin")]
        public ActionResult Index()
        {
            Hashtable table = Alarm_ParamDal.Search(0, 1000, "FMarkerType", "ASC", " and FMarkerType = '1'");
            ViewBag.Grid1DataSource = table["data"];
            ViewBag.Grid1RecordCount = Int32.Parse(table["total"].ToString());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grid1_PageIndexChanged(JArray Grid1_fields, int Grid1_pageIndex, int gridPageSize, string searchType, string searchMessage)
        {
            var grid1 = UIHelper.Grid("Grid1");
            string sql = string.Empty;
            if (!searchType.Equals(""))
            {
                sql = sql + " and FMarkerType = '" + searchType + "'";
            }
            if (!searchMessage.Equals(""))
            {
                sql = sql + " and FMsg like '%" + searchMessage + "%'";
            }
            Hashtable table = Alarm_ParamDal.Search(Grid1_pageIndex, gridPageSize, "FMarkerType", "ASC", sql);
            var recordCount = Int32.Parse(table["total"].ToString());

            // 1.设置总项数（数据库分页回发时，如果总记录数不变，可以不设置RecordCount）
            grid1.RecordCount(recordCount);
            grid1.PageSize(gridPageSize);
            // 2.获取当前分页数据
            var dataSource = table["data"];
            grid1.DataSource(dataSource, Grid1_fields);

            return UIHelper.Result();
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
                sql = " and FMsg like '%" + triggerValue + "%'";
            }
            else if (type == "searchType")
            {
                var triggerValue = typeParams.Value<string>("searchType");
                sql = sql + " and FMarkerType = '" + triggerValue + "'";
            }

            Hashtable table = Alarm_ParamDal.Search(gridIndex, gridPageSize, "FMarkerType", "ASC", sql);
            Grid1.PageSize(gridPageSize);
            Grid1.DataSource(table["data"], gridFields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult btnSubmit_Click(JArray Grid1_fields, JArray Grid1_modifiedData, int Grid1_pageIndex, int gridPageSize, string searchType, string searchMessage)
        {
            Hashtable table = Alarm_ParamDal.Search(0, 1000, "FMarkerType", "ASC", "");
            DataTable source = (DataTable)table["data"];

            foreach (JObject modifiedRow in Grid1_modifiedData)
            {
                string status = modifiedRow.Value<string>("status");
                string rowId = modifiedRow.Value<string>("id");
                if (status == "modified")
                {
                    UpdateDataRow(modifiedRow, Convert.ToInt32(rowId), source);
                }
                else if (status == "newadded")
                {
                    DataRow rowData = CreateNewData(modifiedRow, source);
                    source.Rows.Add(rowData);
                }
                else if (status == "deleted")
                {
                    DeleteRowByID(source, Convert.ToInt32(rowId));
                }
            }

            var grid1 = UIHelper.Grid("Grid1");
            string sql = string.Empty;
            if (!searchType.Equals(""))
            {
                sql = sql + " and FMarkerType = '" + searchType + "'";
            }
            if (!searchMessage.Equals(""))
            {
                sql = sql + " and FMsg like '%" + searchMessage + "%'";
            }
            Hashtable table1 = Alarm_ParamDal.Search(Grid1_pageIndex, gridPageSize, "FMarkerType", "ASC", sql);
            var recordCount = Int32.Parse(table1["total"].ToString());

            // 1.设置总项数（数据库分页回发时，如果总记录数不变，可以不设置RecordCount）
            grid1.RecordCount(recordCount);
            grid1.PageSize(gridPageSize);
            // 2.获取当前分页数据
            var dataSource = table1["data"];
            grid1.DataSource(dataSource, Grid1_fields);

            ShowNotify("数据保存成功！");

            return UIHelper.Result();
        }

        private void UpdateDataRow(JObject modifiedRow, int rowId, DataTable source)
        {
            DataRow rowData = FindRowByID(source, rowId);
            UpdateDataRow(modifiedRow, rowData);

            Hashtable hasData = new Hashtable();
            hasData["id"] = rowData["ID"];
            hasData["FMarkerType"] = rowData["FMarkerType"];
            hasData["FKey"] = rowData["FKey"];
            hasData["FMsg"] = rowData["FMsg"];
            hasData["FLev"] = rowData["FLev"];
            Alarm_ParamDal.Update(hasData);
        }

        private DataRow CreateNewData(JObject modifiedRow, DataTable source)
        {
            DataRow rowData = source.NewRow();

            UpdateDataRow(modifiedRow, rowData);

            Alarm_Param model = new Alarm_Param();
            model.FMarkerType = Convert.ToInt32(rowData["FMarkerType"].ToString());
            model.FKey = rowData["FKey"].ToString();
            model.FMsg = rowData["FMsg"].ToString();
            model.FLev = Convert.ToInt32(rowData["FLev"].ToString());
            db.Alarm_Param.Add(model);
            db.SaveChanges();
            rowData["id"] = model.id;
            return rowData;
        }

        private void UpdateDataRow(JObject modifiedRow, DataRow rowData)
        {
            Dictionary<string, object> rowDict = modifiedRow.Value<JObject>("values").ToObject<Dictionary<string, object>>();

            UpdateDataRow("FMarkerType", rowDict, rowData);
            UpdateDataRow("FKey", rowDict, rowData);
            UpdateDataRow("FMsg", rowDict, rowData);
            UpdateDataRow("FLev", rowDict, rowData);
        }

        private void UpdateDataRow(string columnName, Dictionary<string, object> rowDict, DataRow rowData)
        {
            if (rowDict.ContainsKey(columnName))
            {
                rowData[columnName] = rowDict[columnName];
            }
        }
        private DataRow FindRowByID(DataTable table, int rowId)
        {
            foreach (DataRow row in table.Rows)
            {
                if (Convert.ToInt32(row["id"]) == rowId)
                {
                    return row;
                }
            }
            return null;
        }
        [MyAuth(MenuPower = "CoreAlarmContactDelete")]
        private void DeleteRowByID(DataTable table, int rowID)
        {
            DataRow found = FindRowByID(table, rowID);
            if (found != null)
            {
                table.Rows.Remove(found);
                Alarm_Param model = db.Alarm_Param.Where(x => x.id == rowID).FirstOrDefault();
                db.Alarm_Param.Remove(model);
                db.SaveChanges();
            }
        }

	}
}