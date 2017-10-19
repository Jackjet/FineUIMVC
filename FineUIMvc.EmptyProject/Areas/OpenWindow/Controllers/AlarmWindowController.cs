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
using System.Data.Entity;
using System.Text;
using System.Net;

namespace FineUIMvc.PumpMVC.Areas.OpenWindow.Controllers
{
    [Authorize]
    public class AlarmWindowController : BaseController
    {
        //
        // GET: /OpenWindow/AlarmWindow/
        [MyAuth(MenuPower = "CoreAlarmParmView")]
        public ActionResult Index(string typeId, string baseId)
        {
            ViewBag.baseID = baseId;
            ViewBag.typeId = typeId;
            DataTable dt = Alarm_ParamDal.SearchAlarmParm("", Convert.ToInt32(typeId), baseId);
            ViewBag.GridAlarmParaDataSource = dt;

            DataRow[] d = dt.Select("selectItem=true");
            int[] ls = new int[d.Length];
            int j = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (dt.Rows[i]["selectItem"].Equals("true"))
                {
                    ls[j] = i;
                    j++;
                }
            }
            ViewBag.GridAlarmParaSelectedRow = ls;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuth(MenuPower = "CoreAlarmParmSet")]
        public ActionResult btnAlarmSet_Click(JArray GridAlarmPara_fields, JArray GridAlarmPara_modifiedData, JArray selected, string baseId, string cboTongBu, int typeId)
        {
            try
            {
                string select = string.Empty;
                foreach (JArray item in selected)
                {
                    select = select + "," + item[0].ToString();
                }

                select = select.Substring(1, select.Length - 1);
                Hashtable has = new Hashtable();
                has["ID"] = select;
                //根据选中项找出Alarm_Timely已有的作为多余项执行删除操作
                Alarm_ParamDal.DeleteTimelyList(has, typeId, baseId);

                //根据选中项找出Alarm_Timely没有的ParamID执行插入操作
                Insert(select, typeId, baseId, GridAlarmPara_modifiedData);
                //根据选中项找出Alarm_Timely有的ParamID执行修改操作
                Update(select, typeId, baseId, GridAlarmPara_modifiedData);

                if (cboTongBu.Equals("true"))
                {
                    switch (typeId)
                    {
                        case 1: PumpTongBu(baseId, 1); break;
                    }
                }

                ShowNotify("保存成功！");
                // 关闭本窗体（触发窗体的关闭事件）
                PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
            }
            catch
            {
                ShowNotify("一项或多项保存失败！");
            }

            return UIHelper.Result();
        }

        public void Insert(string select, int typeId, string baseId, JArray GridAlarmPara_modifiedData)
        {
            DataTable dt_not = Alarm_ParamDal.SearchNotParmInsert(select, typeId, baseId);
            Hashtable hasInsert = new Hashtable();
            if (dt_not.Rows.Count > 0)
            {
                for (int i = 0; i < dt_not.Rows.Count; i++)
                {
                    string id = dt_not.Rows[i][0].ToString();
                    hasInsert["ParamID"] = dt_not.Rows[i]["id"].ToString();
                    hasInsert["BaseID"] = baseId;
                    hasInsert["FStatus"] = 0;
                    hasInsert["FIsPhone"] = 0;
                    hasInsert["FMarkerType"] = dt_not.Rows[i]["FMarkerType"].ToString();
                    hasInsert["FKey"] = dt_not.Rows[i]["FKey"].ToString();
                    hasInsert["FMsg"] = dt_not.Rows[i]["FMsg"].ToString();
                    hasInsert["FLev"] = dt_not.Rows[i]["FLev"].ToString();
                    hasInsert["FSetMsg"] = dt_not.Rows[i]["FMsg"].ToString();
                    foreach (JObject item in GridAlarmPara_modifiedData)
                    {
                        Dictionary<string, object> rowDict = item.Value<JObject>("values").ToObject<Dictionary<string, object>>();
                        if (id.Equals(item.Value<string>("id")))
                        {
                            if (rowDict.Keys.Contains("FLev"))
                            {
                                hasInsert["FLev"] = rowDict["FLev"];
                            }
                            if (rowDict.Keys.Contains("FSetMsg"))
                            {
                                hasInsert["FSetMsg"] = rowDict["FSetMsg"];
                            }

                            continue;
                        }
                    }
                    Alarm_ParamDal.InsertTimely(hasInsert);
                }
            }
        }

        public void Update(string select, int typeId, string baseId, JArray GridAlarmPara_modifiedData)
        {
            DataTable dt_have = Alarm_ParamDal.SearchHaveParmUpdate(select, typeId, baseId);
            Hashtable hasUpdate = new Hashtable();
            if (dt_have.Rows.Count > 0)
            {
                for (int i = 0; i < dt_have.Rows.Count; i++)
                {
                    string id = dt_have.Rows[i][0].ToString();
                    foreach (JObject item in GridAlarmPara_modifiedData)
                    {
                        Dictionary<string, object> rowDict = item.Value<JObject>("values").ToObject<Dictionary<string, object>>();
                        if (id.Equals(item.Value<string>("id")))
                        {
                            if (rowDict.Keys.Contains("FLev"))
                            {
                                hasUpdate["FLev"] = rowDict["FLev"];
                            }
                            if (rowDict.Keys.Contains("FSetMsg"))
                            {
                                hasUpdate["FSetMsg"] = rowDict["FSetMsg"];
                            }
                            hasUpdate["id"] = Convert.ToInt32(dt_have.Rows[i]["TimelyID"].ToString());
                            Alarm_ParamDal.UpdateTimely(hasUpdate);
                            continue;
                        }
                    }
                }
            }
        }

        public void PumpTongBu(string baseId, int typeId)
        {
            DataTable dt_pump = Alarm_ParamDal.SearchPump_JZID(baseId);
            for (int i = 0; i < dt_pump.Rows.Count; i++)
            {
                Alarm_ParamDal.TongBu(baseId, dt_pump.Rows[i]["ID"].ToString(), typeId);
            }
        }
    }
}