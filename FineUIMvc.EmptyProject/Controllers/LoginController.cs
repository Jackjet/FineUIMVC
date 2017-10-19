using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Collections;
using FineUIMvc.PumpMVC.DAL;
using FineUIMvc.PumpMVC.Models;

namespace FineUIMvc.PumpMVC.Controllers
{
    public class LoginController : BaseController
    {
        DBController db = new DBController();
        // GET: Login
        public ActionResult Index()
        {
            string host = Request.Url.Host.ToString();
            LoginPage LoginPage = db.LoginPage.ToList().Where(x => x.Host == host).FirstOrDefault();

            if (LoginPage != null)
            {
                return Redirect("~/LoginContent/" + LoginPage.FileName + "/loginDo.html");
            }
            return Redirect("~/LoginContent/default/loginDo.html");
        }


        [HttpPost]
        public ActionResult btnLogin_Click(string FName, string FPassword)
        {
            string msg = string.Empty;
            Hashtable user = Panda_UserInfoDal.GetLogin(FName, FPassword);
            if (user != null)
            {
                if (PasswordUtil.ComparePasswords(user["UserPassword"].ToString(), FPassword))
                {
                    if (user["UserEnabledisable"].ToString().Equals("False"))
                    {
                        msg = "用户未启用，请联系管理员！";
                    }
                    else
                    {
                        LoginSuccess(user);
                        return Json(new { result = "1", msg = "登陆成功" });
                    }
                }
                else
                {
                    msg = "用户名或密码错误！";
                }
            }
            else
            {
                msg = "用户名或密码错误！";
            }
            return Json(new { result = "0", msg = msg }); ;
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