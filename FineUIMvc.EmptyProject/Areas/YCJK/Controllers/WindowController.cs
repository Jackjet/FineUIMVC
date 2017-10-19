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

namespace FineUIMvc.PumpMVC.Areas.YCJK.Controllers
{
    [Authorize]
    public class WindowController : BaseController
    {
        //
       [MyAuth(MenuPower = "CorePumpView")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult pumpWindow()
        {
            return View();
        }
        public ActionResult pumpJZWindow()
        {
            return View();
        }
        public ActionResult dataItemShowHide()
        {
            return View();
        }
        public ActionResult WeiKaiTong()
        {
            return View();
        }
        
        public void getPumpList()
        {
            string strwhere = "";
            string _PName = Request["PName"];
            if (_PName != "" && _PName != null)
            {
                strwhere = strwhere + " and (PName like '%" + _PName + "%' or PCustomPName like '%" + _PName + "%' or PCode like '%" + _PName + "%')";
            }
            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);
            strwhere = strwhere + getPowerConst("pump");
            Hashtable table = Panda_PumpDal.Search(pageIndex, pageSize, "a.FCreateDate", "DESC", strwhere);
            string json = PluSoft.Utils.JSON.Encode(table);
            Response.Write(json);
        }

        public void getPumpJZList()
        {
            string strwhere = "";
            string pumpID = Request["pumpID"];   //搜索泵房id
            string _JZName = Request["JZName"];
            if (pumpID != null && pumpID != "")
            {
                strwhere = strwhere + " and c.id='" + pumpID + "'";
            }
            if (_JZName != "" && _JZName != null)
            {
                strwhere = strwhere + " and (a.PumpJZName like '%" + _JZName + "%' or a.DTUCode like '%" + _JZName + "%')";
            }
            strwhere = strwhere + getPowerConst("pumpJZ");
            int pageIndex = Convert.ToInt32(Request["pageIndex"]);
            int pageSize = Convert.ToInt32(Request["pageSize"]);
            Hashtable table = Panda_PumpJZDal.Search(pageIndex, pageSize, "a.FCreateDate", "DESC", strwhere);
            string json = PluSoft.Utils.JSON.Encode(table);
            Response.Write(json);
        }
	}
}