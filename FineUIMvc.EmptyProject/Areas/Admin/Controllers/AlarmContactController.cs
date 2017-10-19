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

namespace FineUIMvc.PumpMVC.Areas.Admin.Controllers
{
    [Authorize]
    public class AlarmContactController : BaseController
    {
        private DBController db = new DBController();

        #region 列表
        //
        // GET: /Admin/AlarmContact/
        public ActionResult Index()
        {
            ViewBag.CoreAlarmContactNew = CheckPower("CoreAlarmContactNew");
            ViewBag.CoreAlarmContactEdit = CheckPower("CoreAlarmContactEdit");
            ViewBag.CoreAlarmContactDelete = CheckPower("CoreAlarmContactDelete");
            LoadData();
            return View();
        }
        private void LoadData()
        {
            List<TreeNode> nodes = new List<TreeNode>();

            TreeNode node1 = new TreeNode();
            node1.NodeID = "1";
            node1.Text = "泵站";
            node1.Expanded=true;
            node1.Selectable = false;
            nodes.Add(node1);

            TreeNode node2 = new TreeNode();
            node2.NodeID = "2";
            node2.Text = "阀门";
            node2.Expanded = true;
            node2.Selectable = false;
            nodes.Add(node2);

            TreeNode node3 = new TreeNode();
            node3.NodeID = "3";
            node3.Text = "流量";
            node3.Expanded = true;
            node3.Selectable = false;
            nodes.Add(node3);

            TreeNode node4 = new TreeNode();
            node4.NodeID = "4";
            node4.Text = "水厂";
            node4.Expanded = true;
            node4.Selectable = false;
            nodes.Add(node4);

            TreeNode node5 = new TreeNode();
            node5.NodeID = "5";
            node5.Text = "水源";
            node5.Expanded = true;
            node5.Selectable = false;
            nodes.Add(node5);

            TreeNode node6 = new TreeNode();
            node6.NodeID = "6";
            node6.Text = "大表";
            node6.Expanded = true;
            node6.Selectable = false;
            nodes.Add(node6);

            TreeNode node7 = new TreeNode();
            node7.NodeID = "7";
            node7.Text = "压力";
            node7.Expanded = true;
            node7.Selectable = false;
            nodes.Add(node7);

            TreeNode node8 = new TreeNode();
            node8.NodeID = "8";
            node8.Text = "调峰";
            node8.Expanded = true;
            node8.Selectable = false;
            nodes.Add(node8);

            TreeNode node9 = new TreeNode();
            node9.NodeID = "9";
            node9.Text = "水质";
            node9.Expanded = true;
            node9.Selectable = false;
            nodes.Add(node9);
            TreeNode node10 = new TreeNode();
            node10.NodeID = "10";
            node10.Text = "加压站";
            node10.Expanded = true;
            node10.Selectable = false;
            nodes.Add(node10);
            TreeNode node11 = new TreeNode();
            node11.NodeID = "11";
            node11.Text = "预置泵站";
            node11.Expanded = true;
            node11.Selectable = false;
            nodes.Add(node11);
            string sql = string.Empty;

            if (GetUserType().Equals("3"))  //如果登录用户是客户
            {
                ViewBag.CustomerName = GetUserCustomerName();
                ViewBag.CustomerID = GetUserCustomer();
                ViewBag.ReadOnly = true;
                sql = " and FCustomerID=" + GetUserCustomer();
            }
            else
            {
                ViewBag.CustomerName = "";
                ViewBag.CustomerID = "0";
                ViewBag.ReadOnly = false;
            }

            Hashtable hashtable = AlarmContactDal.Search(0, 1000, "FCreateDate", "ASC", sql);
            // 模拟从数据库返回数据表
            DataTable table = (DataTable)hashtable["data"];

            DataSet ds = new DataSet();
            ds.Tables.Add(table);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                TreeNode node = new TreeNode();
                node.Text = row["FName"].ToString();
                node.NodeID = row["ID"].ToString();
                switch(row["FType"].ToString())
                {
                    case "1": node1.Nodes.Add(node); break;
                    case "2": node2.Nodes.Add(node); break;
                    case "3": node3.Nodes.Add(node); break;
                    case "4": node4.Nodes.Add(node); break;
                    case "5": node5.Nodes.Add(node); break;
                    case "6": node6.Nodes.Add(node); break;
                    case "7": node7.Nodes.Add(node); break;
                    case "8": node8.Nodes.Add(node); break;
                    case "9": node9.Nodes.Add(node); break;
                    case "10": node10.Nodes.Add(node); break;
                    case "11": node11.Nodes.Add(node); break;
                }
            }

            ViewBag.SelectNode = table.Rows.Count>0? table.Rows[0]["ID"].ToString():"-1";

            // 视图数据
            ViewBag.Tree1Nodes = nodes.ToArray();

            if(table.Rows.Count>0)
            {
                Hashtable tb = MessageSettingDal.Search(0, 20, "Contacts", "DESC", " and GroupID='" + table.Rows[0]["ID"].ToString() + "'");
                ViewBag.Grid1DataSource = tb["data"];
                ViewBag.Grid1RecordCount = Int32.Parse(tb["total"].ToString());
            }
            else
            {
                ViewBag.Grid1DataSource = null;
                ViewBag.Grid1RecordCount = 0;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Tree1_NodeClick(JArray Grid1_fields, string nodeId)
        {
            var Grid1 = UIHelper.Grid("Grid1");
            Hashtable table = MessageSettingDal.Search(0, 20, "Contacts", "DESC", " and GroupID='" + nodeId + "'");
            Grid1.DataSource(table["data"], Grid1_fields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));
            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult btnGetSelectedNode_Click(JObject selectedNode)
        {
            if (selectedNode.Count > 0)
            {
                int ID = Convert.ToInt32(selectedNode.Value<string>("id"));
                Hashtable hasData = new Hashtable();
                hasData["ID"] = ID;
                hasData["FIsDelete"] = 1;
                hasData["FUpdDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ssss"));
                hasData["FUpdUser"] = GetIdentityName();
                AlarmContactDal.DeleteList(hasData);
            }

            return RedirectToAction("Index", "AlarmContact");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grid1_PageIndexChanged(JArray Grid1_fields, int Grid1_pageIndex, int gridPageSize, string GroupID)
        {
            var Grid1 = UIHelper.Grid("Grid1");

            string sql = " and GroupID='" + GroupID + "'";

            Hashtable table = MessageSettingDal.Search(Grid1_pageIndex, gridPageSize, "Contacts", "DESC", sql);
            Grid1.DataSource(table["data"], Grid1_fields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));

            return UIHelper.Result();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreAlarmContactEdit")]
        public ActionResult btnSubmit_Click(JArray Grid1_fields, JArray Grid1_modifiedData, string GroupID)
        {
            Hashtable table = MessageSettingDal.Search(0, 20, "Contacts", "DESC", " and GroupID='" + GroupID + "'");
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
                    DataRow rowData = CreateNewData(modifiedRow, source, GroupID);
                    source.Rows.Add(rowData);
                }
                else if (status == "deleted")
                {
                    DeleteRowByID(source, Convert.ToInt32(rowId));
                }
            }

            UIHelper.Grid("Grid1").DataSource(source, Grid1_fields);

            ShowNotify("数据保存成功！（表格数据已重新绑定）");

            return UIHelper.Result();
        }

        #region UpdateDataRow
        [MyAuth(MenuPower = "CoreAlarmContactNew")]
        private DataRow CreateNewData(JObject modifiedRow, DataTable source, string GroupID)
        {
            DataRow rowData = source.NewRow();

            UpdateDataRow(modifiedRow, rowData);

            MessageSetting model = new MessageSetting();
            model.Contacts = rowData["Contacts"].ToString();
            model.ContactPhone = rowData["ContactPhone"].ToString();
            model.Type = Convert.ToInt32(rowData["Type"] == DBNull.Value ? "0" : rowData["Type"]);
            model.ContactSource = 0;
            model.GroupID = Convert.ToInt32(GroupID);
            model.FIsDelete = 0;
            model.FCreateUser = Convert.ToInt32(GetIdentityName());
            model.FCreateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ssss"));
            db.MessageSetting.Add(model);
            db.SaveChanges();
            rowData["ID"] = model.ID;
            return rowData;
        }


        private void UpdateDataRow(JObject modifiedRow, int rowId, DataTable source)
        {
            DataRow rowData = FindRowByID(source, rowId);
            UpdateDataRow(modifiedRow, rowData);

            Hashtable hasData = new Hashtable();
            hasData["ID"] = rowData["ID"];
            hasData["Contacts"] = rowData["Contacts"];
            hasData["ContactPhone"] = rowData["ContactPhone"];
            hasData["Type"] = Convert.ToInt32(rowData["Type"] == DBNull.Value ? "0" : rowData["Type"]);
            hasData["FUpdUser"] = GetIdentityName();
            hasData["FUpdDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ssss"));
            MessageSettingDal.Update(hasData);
        }

        private void UpdateDataRow(JObject modifiedRow, DataRow rowData)
        {
            Dictionary<string, object> rowDict = modifiedRow.Value<JObject>("values").ToObject<Dictionary<string, object>>();

            UpdateDataRow("Contacts", rowDict, rowData);
            UpdateDataRow("ContactPhone", rowDict, rowData);
            UpdateDataRow("Type", rowDict, rowData);
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
                if (Convert.ToInt32(row["ID"]) == rowId)
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
                Hashtable hasData = new Hashtable();
                hasData["ID"] = rowID;
                hasData["FIsDelete"] = 1;
                hasData["FDelDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ssss"));
                hasData["FDelUser"] = GetIdentityName();
                MessageSettingDal.Update(hasData);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyCustomPostBack(string type, JArray gridFields, JObject typeParams, int gridIndex, int gridPageSize, string GroupID)
        {
            var Tree1 = UIHelper.Tree("Tree1");
            List<TreeNode> nodes = new List<TreeNode>();

            TreeNode node1 = new TreeNode();
            node1.NodeID = "1";
            node1.Text = "泵站";
            node1.Expanded = true;
            node1.Selectable = false;
            nodes.Add(node1);

            TreeNode node2 = new TreeNode();
            node2.NodeID = "2";
            node2.Text = "阀门";
            node2.Expanded = true;
            node2.Selectable = false;
            nodes.Add(node2);

            TreeNode node3 = new TreeNode();
            node3.NodeID = "3";
            node3.Text = "流量";
            node3.Expanded = true;
            node3.Selectable = false;
            nodes.Add(node3);

            TreeNode node4 = new TreeNode();
            node4.NodeID = "4";
            node4.Text = "水厂";
            node4.Expanded = true;
            node4.Selectable = false;
            nodes.Add(node4);

            TreeNode node5 = new TreeNode();
            node5.NodeID = "5";
            node5.Text = "水源";
            node5.Expanded = true;
            node5.Selectable = false;
            nodes.Add(node5);

            TreeNode node6 = new TreeNode();
            node6.NodeID = "6";
            node6.Text = "大表";
            node6.Expanded = true;
            node6.Selectable = false;
            nodes.Add(node6);

            TreeNode node7 = new TreeNode();
            node7.NodeID = "7";
            node7.Text = "压力";
            node7.Expanded = true;
            node7.Selectable = false;
            nodes.Add(node7);

            TreeNode node8 = new TreeNode();
            node8.NodeID = "8";
            node8.Text = "调峰";
            node8.Expanded = true;
            node8.Selectable = false;
            nodes.Add(node8);

            TreeNode node9 = new TreeNode();
            node9.NodeID = "9";
            node9.Text = "水质";
            node9.Expanded = true;
            node9.Selectable = false;
            nodes.Add(node9);

            TreeNode node10 = new TreeNode();
            node10.NodeID = "10";
            node10.Text = "加压站";
            node10.Expanded = true;
            node10.Selectable = false;
            nodes.Add(node10);
            TreeNode node11 = new TreeNode();
            node11.NodeID = "11";
            node11.Text = "预置泵站";
            node11.Expanded = true;
            node11.Selectable = false;
            nodes.Add(node11);

            string sql1 = string.Empty;

            if (GetUserType().Equals("3"))  //如果登录用户是客户
            {
                sql1 = " and FCustomerID=" + GetUserCustomer();
            }
            else
            {
                if (type == "selectCustomer")
                {
                    var FCustomerID = typeParams.Value<string>("FCustomerID");
                    sql1 = " and FCustomerID=" + FCustomerID;
                }
            }
            Hashtable hashtable = AlarmContactDal.Search(0, 1000, "FCreateDate", "ASC", sql1);
            // 模拟从数据库返回数据表
            DataTable table = (DataTable)hashtable["data"];

            DataSet ds = new DataSet();
            ds.Tables.Add(table);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                TreeNode node = new TreeNode();
                node.Text = row["FName"].ToString();
                node.NodeID = row["ID"].ToString();
                node.Leaf = true;
                switch (row["FType"].ToString())
                {
                    case "1": node1.Nodes.Add(node); break;
                    case "2": node2.Nodes.Add(node); break;
                    case "3": node3.Nodes.Add(node); break;
                    case "4": node4.Nodes.Add(node); break;
                    case "5": node5.Nodes.Add(node); break;
                    case "6": node6.Nodes.Add(node); break;
                    case "7": node7.Nodes.Add(node); break;
                    case "8": node8.Nodes.Add(node); break;
                    case "9": node9.Nodes.Add(node); break;
                    case "10": node10.Nodes.Add(node); break;
                    case "11": node11.Nodes.Add(node); break;
                }
            }

            // 视图数据
            ViewBag.Tree1Nodes = nodes.ToArray();
            Tree1.LoadData(ViewBag.Tree1Nodes);
            Tree1.SelectedNodeID(table.Rows.Count>0?table.Rows[0]["ID"].ToString():"-1");
            var Grid1 = UIHelper.Grid("Grid1");
            string sql = "";
            if(table.Rows.Count>0)
            {
                sql = " and GroupID='" + table.Rows[0]["ID"].ToString() + "'";
                
            }
            else
            {
                sql = " and GroupID='-1'";
            }
            Hashtable tb = MessageSettingDal.Search(gridIndex, gridPageSize, "Contacts", "DESC", sql);
            Grid1.DataSource(tb["data"], gridFields);
            Grid1.RecordCount(Int32.Parse(tb["total"].ToString()));
            Grid1.PageSize(gridPageSize);
            return UIHelper.Result();
        }

        #endregion
        #endregion

        #region 新增组
        [MyAuth(MenuPower = "CoreAlarmContactNew")]
        public ActionResult ContactGroup_new(string C_Id, string C_Name)
        {
            ViewBag.CustomerID = C_Id;
            ViewBag.CustomerName = C_Name;
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
                    Hashtable has = new Hashtable();
                    has["FCreateUser"] = Convert.ToInt32(GetIdentityName());
                    has["FCreateDate"] = DateTime.Now;
                    has["FIsDelete"] = 0;
                    has["FName"] = Request["tbxName"];
                    has["FType"] = Convert.ToInt32(Request["ddlFType"]);
                    has["FCustomerID"] = Convert.ToInt32(Request["tbxCustomerID"]);
                    has["FNote"] = Request["tbxFNote"];
                    AlarmContactDal.Insert(has);
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

        #region 修改组
        [MyAuth(MenuPower = "CoreAlarmContactEdit")]
        public ActionResult ContactGroup_edit(int GroupID)
        {
            Alarm_Contact_Group fg = db.Alarm_Contact_Group.Find(GroupID);
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
                    Hashtable has = new Hashtable();
                    has["ID"] = Request["tbxID"];
                    has["FName"] = Request["tbxName"];
                    has["FType"] = Convert.ToInt32(Request["ddlFType"]);
                    has["FCustomerID"] = Convert.ToInt32(Request["tbxCustomerID"]);
                    has["FNote"] = Request["tbxFNote"];
                    has["FUpdUser"] = GetIdentityName();
                    has["FUpdDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    AlarmContactDal.Update(has);

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