using FineUIMvc.PumpMVC.Controllers;
using FineUIMvc.PumpMVC.DAL;
using FineUIMvc.PumpMVC.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FineUIMvc.PumpMVC.Areas.BasicInfo.Controllers
{
    public class AddressSchemeController : BaseController
    {
        DBController DB = new DBController();
        //
        // GET: /BasicInfo/PGroup/
        [MyAuth(MenuPower = "CoreAddressView")]
        public ActionResult Index()
        {
            ViewBag.CoreAddressAdd = CheckPower("CoreAddressAdd");
            ViewBag.CoreAddressEdit = CheckPower("CoreAddressEdit");
            ViewBag.CoreAddressDelete = CheckPower("CoreAddressDelete");
            LoadData();

            return View();
        }
        private void LoadData()
        {
            List<TreeNode> nodes = new List<TreeNode>();

            TreeNode node1 = new TreeNode();
            node1.Selectable = false;
            node1.Text = "泵站";
            nodes.Add(node1);

            TreeNode node2 = new TreeNode();
            node2.Text = "阀门";
            node2.Selectable = false;
            nodes.Add(node2);

            TreeNode node3 = new TreeNode();
            node3.Text = "流量";
            node3.Selectable = false;
            nodes.Add(node3);

            TreeNode node4 = new TreeNode();
            node4.Text = "水厂";
            node4.Selectable = false;
            nodes.Add(node4);

            TreeNode node5 = new TreeNode();
            node5.Text = "水源";
            node5.Selectable = false;
            nodes.Add(node5);

            TreeNode node6 = new TreeNode();
            node6.Text = "大表";
            node6.Selectable = false;
            nodes.Add(node6);

            TreeNode node7 = new TreeNode();
            node7.Text = "压力";
            node7.Selectable = false;
            nodes.Add(node7);

            TreeNode node8 = new TreeNode();
            node8.Text = "调峰";
            node8.Selectable = false;
            nodes.Add(node8);

            TreeNode node9 = new TreeNode();
            node9.Text = "水质";
            node9.Selectable = false;
            nodes.Add(node9);
            Hashtable hashtable = AddressSchemeDal.Search(0, 1000, "FOrderBy", "ASC", " ");
            // 模拟从数据库返回数据表
            DataTable table = (DataTable)hashtable["data"]; ;

            DataSet ds = new DataSet();
            ds.Tables.Add(table);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                TreeNode node = new TreeNode();
                node.Text = row["FName"].ToString();
                node.NodeID = row["ID"].ToString();
                node.Target = "treemainframe";
                node.NavigateUrl = "~/BasicInfo/AddressScheme/AddressSchemeEntry?FSchemeID=" + row["ID"].ToString();
                if (row["FType"].ToString() == "1")
                {
                    node1.Nodes.Add(node);
                }
                else if (row["FType"].ToString() == "2")
                {
                    node2.Nodes.Add(node);
                }
                else if (row["FType"].ToString() == "3")
                {
                    node3.Nodes.Add(node);
                }
                else if (row["FType"].ToString() == "4")
                {
                    node4.Nodes.Add(node);
                }
                else if (row["FType"].ToString() == "5")
                {
                    node5.Nodes.Add(node);
                }
                else if (row["FType"].ToString() == "6")
                {
                    node6.Nodes.Add(node);
                }
                else if (row["FType"].ToString() == "7")
                {
                    node7.Nodes.Add(node);
                }
                else if (row["FType"].ToString() == "8")
                {
                    node8.Nodes.Add(node);
                }
                else if (row["FType"].ToString() == "9")
                {
                    node9.Nodes.Add(node);
                }
            }

            // 视图数据
            ViewBag.Tree1Nodes = nodes.ToArray();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Window1_Close()
        {

            return RedirectToAction("Index", "AddressScheme");
        }
        private void UpdateGrid(JArray Grid1_fields)
        {
            var grid1 = UIHelper.Tree("Tree1");
            List<TreeNode> nodes = new List<TreeNode>();

            TreeNode node1 = new TreeNode();
            node1.Selectable = false;
            node1.Text = "泵站";
            nodes.Add(node1);

            TreeNode node2 = new TreeNode();
            node2.Text = "阀门";
            node2.Selectable = false;
            nodes.Add(node2);

            TreeNode node3 = new TreeNode();
            node3.Text = "流量";
            node3.Selectable = false;
            nodes.Add(node3);

            TreeNode node4 = new TreeNode();
            node4.Text = "水厂";
            node4.Selectable = false;
            nodes.Add(node4);

            TreeNode node5 = new TreeNode();
            node5.Text = "水源";
            node5.Selectable = false;
            nodes.Add(node5);

            TreeNode node6 = new TreeNode();
            node6.Text = "大表";
            node6.Selectable = false;
            nodes.Add(node6);

            TreeNode node7 = new TreeNode();
            node7.Text = "压力";
            node7.Selectable = false;
            nodes.Add(node7);

            TreeNode node8 = new TreeNode();
            node8.Text = "调峰";
            node8.Selectable = false;
            nodes.Add(node8);

            TreeNode node9 = new TreeNode();
            node9.Text = "水质";
            node9.Selectable = false;
            nodes.Add(node9);
            Hashtable hashtable = AddressSchemeDal.Search(0, 1000, "FOrderBy", "ASC", " ");
            // 模拟从数据库返回数据表
            DataTable table = (DataTable)hashtable["data"]; ;

            DataSet ds = new DataSet();
            ds.Tables.Add(table);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                TreeNode node = new TreeNode();
                node.Text = row["FName"].ToString();
                node.NodeID = row["ID"].ToString();
                node.Target = "treemainframe";
                node.NavigateUrl = "~/BasicInfo/AddressScheme/AddressSchemeEntry?FSchemeID=" + row["ID"].ToString();
                if (row["FType"].ToString() == "1")
                {
                    node1.Nodes.Add(node);
                }
                else if (row["FType"].ToString() == "2")
                {
                    node2.Nodes.Add(node);
                }
                else if (row["FType"].ToString() == "3")
                {
                    node3.Nodes.Add(node);
                }
                else if (row["FType"].ToString() == "4")
                {
                    node4.Nodes.Add(node);
                }
                else if (row["FType"].ToString() == "5")
                {
                    node5.Nodes.Add(node);
                }
                else if (row["FType"].ToString() == "6")
                {
                    node6.Nodes.Add(node);
                }
                else if (row["FType"].ToString() == "7")
                {
                    node7.Nodes.Add(node);
                }
                else if (row["FType"].ToString() == "8")
                {
                    node8.Nodes.Add(node);
                }
                else if (row["FType"].ToString() == "9")
                {
                    node9.Nodes.Add(node);
                }
            }

            // 视图数据
            ViewBag.Tree1Nodes = nodes.ToArray();
            grid1.LoadData(ViewBag.Tree1Nodes);
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
                AddressSchemeDal.DeleteList(hasData);
            }

            return RedirectToAction("Index", "AddressScheme");
        }
        public ActionResult AddressSchemeEntry(string FSchemeID)
        {
            Hashtable table = AddressSchemeEntryDal.Search(0, 1000, "id", "ASC", " AND FSchemeID= " + Convert.ToInt32(FSchemeID));
            ViewBag.Grid1DataSource = table["data"];
            ViewBag.Grid1RecordCount = Int32.Parse(table["total"].ToString());
            ViewBag.FSchemeID = FSchemeID;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grid1_AfterEdit(JArray Grid1_fields, JArray Grid1_modifiedData, int Grid1_pageIndex, string FSchemeID, int gridPageSize, string searchMessage)
        {

            foreach (JObject modifiedRow in Grid1_modifiedData)
            {
                string status = modifiedRow.Value<string>("status");
                int rowId = Convert.ToInt32(modifiedRow.Value<string>("id"));

                if (status == "modified")
                {
                    Hashtable hasData = new Hashtable();
                    hasData["ID"] = rowId;
                    Dictionary<string, object> rowDict = modifiedRow.Value<JObject>("values").ToObject<Dictionary<string, object>>();
                    if (rowDict.ContainsKey("FPLCAddress"))
                    {
                        hasData["FPLCAddress"] = rowDict["FPLCAddress"];
                    }
                    if (rowDict.ContainsKey("FWorR"))
                    {
                        hasData["FWorR"] = rowDict["FWorR"];
                    }
                    if (rowDict.ContainsKey("FUnit"))
                    {
                        hasData["FUnit"] = rowDict["FUnit"];
                    }
                    if (rowDict.ContainsKey("FRate"))
                    {
                        hasData["FRate"] = rowDict["FRate"];
                    }
                    if (rowDict.ContainsKey("FNote"))
                    {
                        hasData["FNote"] = rowDict["FNote"];
                    }

                    AddressSchemeEntryDal.Update(hasData);
                }
            }

            var grid1 = UIHelper.Grid("Grid1");
            string sql = string.Empty;
            if (!searchMessage.Equals(""))
            {
                sql = sql + " and FName like '%" + searchMessage + "%'";
            }
            sql = sql + " AND FSchemeID= " + Convert.ToInt32(FSchemeID);
            Hashtable table = AddressSchemeEntryDal.Search(Grid1_pageIndex, gridPageSize, "id", "ASC", sql);
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
        public ActionResult Grid1_PageIndexChanged(JArray Grid1_fields, int Grid1_pageIndex, string FSchemeID, int gridPageSize, string searchMessage)
        {
            var grid1 = UIHelper.Grid("Grid1");
            string sql = string.Empty;
            if (!searchMessage.Equals(""))
            {
                sql = sql + " and FName like '%" + searchMessage + "%'";
            }
            sql = sql + " AND FSchemeID= " + Convert.ToInt32(FSchemeID);
            Hashtable table = AddressSchemeEntryDal.Search(Grid1_pageIndex, gridPageSize, "id", "ASC", sql);
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
        public ActionResult MyCustomPostBack(string type, JArray gridFields, JObject typeParams, int gridIndex, int gridPageSize, string FSchemeID)
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
            sql = sql + " AND FSchemeID= " + Convert.ToInt32(FSchemeID);
            Hashtable table = AddressSchemeEntryDal.Search(gridIndex, gridPageSize, "id", "ASC", sql);
            Grid1.PageSize(gridPageSize);
            Grid1.DataSource(table["data"], gridFields);
            Grid1.RecordCount(Int32.Parse(table["total"].ToString()));

            return UIHelper.Result();
        }

        public ActionResult AddressScheme_new()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreAddressAdd")]
        public ActionResult btnCreate_Click()
        {
            try
            {
                if (ModelState.IsValid)
                {

                    Hashtable hasData = new Hashtable();
                    hasData["FNumber"] = Request["tbxFNumber"];
                    hasData["FName"] = Request["tbxFName"];
                    hasData["FStartAddress"] = Request["tbxFStartAddress"];
                    hasData["FAddressLength"] = Convert.ToInt32(Request["tbxFAddressLength"]);
                    hasData["FNote"] = Request["tbxFNote"];
                    hasData["FOrderBy"] = Convert.ToInt32(Request["tbxFOrderBy"]);
                    hasData["FType"] = Request["ddlFType"];
                    hasData["FCreateUser"] = GetIdentityName();
                    hasData["FCreateDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    hasData["FIsDelete"] = 0;
                    int id = AddressSchemeDal.Insert(hasData);
                    if (id > 0)
                    {
                        DataTable table = AddressSchemeDal.SearchALL("", hasData["FType"].ToString());
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            Hashtable hashtable = new Hashtable();
                            hashtable["FSchemeID"] = id;
                            hashtable["FName"] = table.Rows[i]["FName"].ToString();
                            hashtable["FPLCAddress"] = table.Rows[i]["FPLCAddress"].ToString();
                            hashtable["FDBAddress"] = table.Rows[i]["FDBAddress"].ToString();
                            hashtable["FWorR"] = table.Rows[i]["FWorR"].ToString();
                            hashtable["FNote"] = table.Rows[i]["FNote"].ToString();
                            hashtable["FUnit"] = table.Rows[i]["FUnit"].ToString();
                            hashtable["FRate"] = table.Rows[i]["FRate"].ToString();
                            hashtable["FCreateDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            AddressSchemeEntryDal.Insert(hashtable);
                        }
                    }
                    ShowNotify("成功！");
                    // 关闭本窗体（触发窗体的关闭事件）'
                    PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                }
            }
            catch
            {
                ShowNotify("失败！");
            }
            return UIHelper.Result();
        }

        public ActionResult AddressScheme_edit(int ID)
        {
            AddressScheme model = DB.AddressScheme.Find(ID);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreAddressEdit")]
        public ActionResult btnEdit_Click()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Hashtable hasData = new Hashtable();
                    hasData["ID"] = Request["tbxID"];
                    hasData["FNumber"] = Request["tbxFNumber"];
                    hasData["FName"] = Request["tbxFName"];
                    hasData["FStartAddress"] = Request["tbxFStartAddress"];
                    hasData["FAddressLength"] = Convert.ToInt32(Request["tbxFAddressLength"]);
                    hasData["FNote"] = Request["tbxFNote"];
                    hasData["FOrderBy"] = Convert.ToInt32(Request["tbxFOrderBy"]);
                    hasData["FType"] = Request["ddlFType"];
                    hasData["FUpdUser"] = GetIdentityName();
                    hasData["FUpdDate"] = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    AddressSchemeDal.Update(hasData);
                    ShowNotify("成功！");
                    // 关闭本窗体（触发窗体的关闭事件）'
                    PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                }
            }
            catch
            {
                ShowNotify("失败！");
            }
            return UIHelper.Result();
        }
    }
}