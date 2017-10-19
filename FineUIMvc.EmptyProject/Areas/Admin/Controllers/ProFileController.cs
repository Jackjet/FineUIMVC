using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Newtonsoft.Json.Linq;
using System.Collections;
using FineUIMvc.PumpMVC.DAL;
using FineUIMvc.PumpMVC.Models;
using FineUIMvc.PumpMVC.Controllers;

namespace FineUIMvc.PumpMVC.Areas.Admin.Controllers
{
    [Authorize]
    public class ProFileController : BaseController
    {
        private DBController db = new DBController();
        //
        // GET: /Admin/ProFile/
        [MyAuth(MenuPower = "CoreUserChangePassword")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult btnSave_OnClick()
        {
            // 检查当前密码是否正确
            string oldPass = Request["tbxOldPassword"];
            string newPass = Request["tbxNewPassword"];
            string confirmNewPass = Request["tbxConfirmNewPassword"];

            if (newPass != confirmNewPass)
            {
                UIHelper.TextBox("tbxConfirmNewPassword").MarkInvalid("确认密码和新密码不一致！");
            }

            if (!PasswordUtil.ComparePasswords(Panda_UserInfoDal.Get(GetIdentityName())["UserPwd"].ToString(), oldPass))
            {
                UIHelper.TextBox("tbxOldPassword").MarkInvalid("当前密码不正确！");
            }

            try
            {
                //string userGH = GetIdentityName();
                //User users = db.users.Where(x => x.Name.Equals(userGH)).FirstOrDefault();
                //users.Password = PasswordUtil.CreateDbPassword(newPass);
                //db.SaveChanges();
                Hashtable hasData = new Hashtable();
                hasData["ID"] = GetIdentityName();
                hasData["UserPwd"] = PasswordUtil.CreateDbPassword(newPass);
                Panda_UserInfoDal.Update(hasData);
                ShowNotify("修改密码成功！");
            }
            catch
            {
                ShowNotify("修改密码失败,请重新操作！");
            }

            return UIHelper.Result();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
	}
}