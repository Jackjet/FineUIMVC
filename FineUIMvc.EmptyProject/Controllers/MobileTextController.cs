using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FineUIMvc.PumpMVC.Models;
using FineUIMvc.PumpMVC.DAL;
using System.Data;
using System.Collections;

namespace FineUIMvc.PumpMVC.Controllers
{
    public class MobileTextController : BaseController
    {
        DBController db = new DBController();

        //
        // GET: /MobileText/
        public ActionResult Index()
        {
            Hashtable user = Panda_UserInfoDal.GetLogin("panda", "1");
            LoginSuccess(user);

            Panda_PumpJZ model = db.Panda_PumpJZ.Where(x => x.DTUCode.Equals("03170519236")).FirstOrDefault();
            E_DATA_MAIN data = db.E_DATA_MAIN.Where(x => x.BaseID.Equals(model.ID)).FirstOrDefault();
            DataTable dt = publicDal.TableSearch(@"select TempTime,  RIGHT(a.FDTUCode,3)+'('+RIGHT(a.FName,1)+')' FName,b.FMpa from BASE_YALI a,DATA_YALI_MAIN b 
                                                   where a.id=b.BASEID and a.FDTUCode in('03160926018','03160926076','03160926027','03160926028','03160926015','03160926026') 
                                                order by FName");

            ViewBag.txt_name = model.PumpJZName;
            ViewBag.txt_time = data.TempTime.ToString();
            ViewBag.txt_jinchuWat = data.F41006.ToString() + "/" + data.F41007.ToString();
            ViewBag.txt_jinchuShun = data.F41024.ToString() + "/" + data.F41025.ToString();
            ViewBag.txt_jinchuXian = data.F41109.ToString() + "/" + data.F41107.ToString() + "/" + data.F41113.ToString() + "/" + data.F41111.ToString();
            ViewBag.txt_state = data.F41003 == "0" ? "手动" : data.F41703 == "1" ? "自动" : "远程-自动";
            ViewBag.Lable1 = dt.Rows[0]["FName"].ToString();
            ViewBag.Text1 = dt.Rows[0]["FMpa"].ToString();
            ViewBag.Time1 = dt.Rows[0]["TempTime"].ToString();
            ViewBag.Lable2 = dt.Rows[1]["FName"].ToString();
            ViewBag.Text2 = dt.Rows[1]["FMpa"].ToString();
            ViewBag.Time2 = dt.Rows[1]["TempTime"].ToString();
            ViewBag.Lable3 = dt.Rows[2]["FName"].ToString();
            ViewBag.Text3 = dt.Rows[2]["FMpa"].ToString();
            ViewBag.Time3 = dt.Rows[2]["TempTime"].ToString();
            ViewBag.Lable4 = dt.Rows[3]["FName"].ToString();
            ViewBag.Text4 = dt.Rows[3]["FMpa"].ToString();
            ViewBag.Time4 = dt.Rows[3]["TempTime"].ToString();
            ViewBag.Lable5 = dt.Rows[4]["FName"].ToString();
            ViewBag.Text5 = dt.Rows[4]["FMpa"].ToString();
            ViewBag.Time5 = dt.Rows[4]["TempTime"].ToString();
            ViewBag.Lable6 = dt.Rows[5]["FName"].ToString();
            ViewBag.Text6 = dt.Rows[5]["FMpa"].ToString();
            ViewBag.Time6 = dt.Rows[5]["TempTime"].ToString();
            return View();
        }
        private void LoginSuccess(Hashtable user)
        {
            RegisterOnlineUser(user);
            Session["UserPowerList"] = null;

            // 用户所属的角色字符串，以逗号分隔
            string roleIDs = String.Empty;
            ArrayList userrole = Panda_UserInfoDal.GetRole(user["ID"].ToString());

            if (userrole != null)
            {
                ArrayList List = new ArrayList();
                foreach (Hashtable a in userrole)
                {

                    List.Add(a["RoleID"].ToString());
                }
                roleIDs = string.Join(",", (string[])List.ToArray(typeof(string)));
            }

            bool isPersistent = false;
            DateTime expiration = DateTime.Now.AddMinutes(120);
            CreateFormsAuthenticationTicket(user["ID"].ToString(), roleIDs, isPersistent, expiration);

            // 重定向到登陆后首页
        }

	}
}