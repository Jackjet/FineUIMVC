using FineUIMvc.PumpMVC.Controllers;
using FineUIMvc.PumpMVC.DAL;
using FineUIMvc.PumpMVC.Models;
using FineUIMvc.PumpMVC.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FineUIMvc.PumpMVC.Areas.OpenWindow.Controllers
{
    public class BaseDAController : BaseController
    {
        private DBController db = new DBController();
        #region 泵房设备档案
        [MyAuth(MenuPower = "CorePumpDAView")]
        public ActionResult PumpArchives(string baseId,string pageType)
        {
            ViewBag.CorePumpDAUpLoad = CheckPower("CorePumpDAUpLoad");
            ViewBag.CorePumpDADown = CheckPower("CorePumpDADown");
            ViewBag.CorePumpDADelete = CheckPower("CorePumpDADelete");
            ViewBag.baseId = baseId.ToString();
            ViewBag.pageType = pageType; 
            DataTable table1 = Panda_PumpDADal.Search(" and uploadPageType='pumpattach' and baseId = '" + baseId + "' and FPageSource='" + pageType+"'");
            DataTable table2 = Panda_PumpDADal.Search(" and uploadPageType='pumppic' and baseId = '" + baseId + "' and FPageSource='" + pageType + "'");
            ViewBag.ddlFile1TypeDataSource = sys_dictDal.SearchDDL(" and FDictID=135");     //泵房设备档案-文件类型
            ViewBag.ddlFile2TypeDataSource = sys_dictDal.SearchDDL(" and FDictID=136");     //泵房设备档案-图片类型
            for (int i = 0; i < table2.Rows.Count; i++)
            {
                table2.Rows[i]["img"] = "<img src='" + Url.Content(table2.Rows[i]["FilePath"].ToString()) + "'/>";
            }
            ViewBag.Grid1DataSource = table1;
            ViewBag.Grid2DataSource = table2;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RebindGrid1(JArray Grid1_fields, string baseId, string pageType)
        {
            var Grid1 = UIHelper.Grid("Grid1");
            string sql = string.Empty;
            sql = sql + " and uploadPageType='pumpattach' and  baseId = '" + baseId + "' and FPageSource='" + pageType + "'";
            DataTable table = Panda_PumpDADal.Search(sql);
            Grid1.DataSource(table, Grid1_fields);

            return UIHelper.Result();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RebindGrid2(JArray Grid2_fields, string baseId, string pageType)
        {
            var Grid2 = UIHelper.Grid("Grid2");
            string sql = string.Empty;
            sql = sql + " and uploadPageType='pumppic' and baseId = '" + baseId + "' and FPageSource='" + pageType + "'";
            DataTable table = Panda_PumpDADal.Search(sql);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                table.Rows[i]["img"] = "<img src='" + Url.Content(table.Rows[i]["FilePath"].ToString()) + "'/>";
            }
            Grid2.DataSource(table, Grid2_fields);
            PageContext.RegisterStartupScript("$(\".imgClass img\").addClass(\"pumpimg\");");
            return UIHelper.Result();
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="selectedRows"></param>
        /// <returns></returns>
        [MyAuth(MenuPower = "CorePumpDADown")]
        public ActionResult Grid_DownLoad(JArray selectedRows)
        {
            int id = Convert.ToInt32(selectedRows[0]);
            var q = db.Panda_PumpDA.Where(x => x.ID == id);
            string fileName = q.FirstOrDefault().FileName;
            fileName = HttpUtility.UrlEncode(fileName, Encoding.GetEncoding("UTF-8"));
            return File(Server.MapPath(q.FirstOrDefault().FilePath), Utilities.MimeType(fileName), fileName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CorePumpDADelete")]
        public ActionResult Grid_Delete(JArray selectedRows, JArray Grid_fields, string type, int baseId, string pageType)
        {
            try
            {
                int id = Convert.ToInt32(selectedRows[0]);
                var q = db.Panda_PumpDA.Where(x => x.ID == id);
                int count = q.Count();
                if (count > 0)
                {
                    if (System.IO.File.Exists(Server.MapPath(q.FirstOrDefault().FilePath)))
                    {
                        System.IO.File.Delete(Server.MapPath(q.FirstOrDefault().FilePath));
                    }
                    Panda_PumpDADal.Delete(" ID=" + id);
                    string sql = " and uploadPageType='" + type + "' and BaseId = '" + baseId + "' and FPageSource='" + pageType + "'";
                    DataTable table = Panda_PumpDADal.Search(sql);

                    if (type.Equals("pumpattach"))
                    {
                        var Grid1 = UIHelper.Grid("Grid1");
                        Grid1.DataSource(table, Grid_fields);
                    }
                    else if (type.Equals("pumppic"))
                    {
                        var Grid2 = UIHelper.Grid("Grid2");
                        Grid2.DataSource(table, Grid_fields);
                    }
                    ShowNotify("删除成功！");
                }
                else
                {
                    ShowNotify("文件不存在！");
                }


            }
            catch
            {
                ShowNotify("删除失败！");
            }

            return UIHelper.Result();
        }
        #endregion
	}
}