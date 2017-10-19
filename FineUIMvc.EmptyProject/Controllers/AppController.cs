using FineUIMvc.PumpMVC.AppHelper;
using FineUIMvc.PumpMVC.AppModel;
using FineUIMvc.PumpMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace FineUIMvc.PumpMVC.Controllers
{
    public class AppController : ApiController
    {
        [HttpPost]
        public LoginInfo GetToken(AcceptLoginInfo usr)
        {
            var model = ValidationHelper.Login(usr);
            if (model.Status != 1)

                return model;
            using (DBController db = new DBController())
            {
                Panda_UserInfo dbItem = db.Panda_UserInfo.FirstOrDefault(u => u.UserName == usr.UserName && u.UserPwd == usr.Md5);

                if (dbItem != null)
                {
                    model.Message = string.Empty; ;
                    model.ErrorCode = string.Empty;
                    model.Status = 1;
                    model.TokenID = Guid.NewGuid().ToString();


                    var delItem = db.Tocken.FirstOrDefault(o => o.UserName == usr.UserName);

                    if (delItem != null)
                        db.Tocken.Remove(delItem);

                    Tocken tk = new Tocken();
                    tk.TockenID = model.TokenID;
                    tk.UpdateTime = DateTime.Now;
                    tk.UserName = usr.UserName;

                    db.Tocken.Add(tk);
                    db.SaveChanges();


                    model.Message = "登录成功。";
                    model.ErrorCode = "";
                    model.Status = 1;
                    model.UserID = dbItem.ID.ToString();
                }
                else
                {
                    model.Message = "用户名或密码错误。";
                    model.ErrorCode = "103";
                    model.Status = 0;
                    model.TokenID = "";
                    model.UserID = "";

                }
            }

            return model;
        }
        public void GetToken111()
        {

            using (DBController db = new DBController())
            {

                Tocken tk = new Tocken();
                tk.TockenID = Guid.NewGuid().ToString();
                tk.UpdateTime = DateTime.Now;
                tk.UserName = "lxm2";
                db.Tocken.Add(tk);
                db.SaveChanges();
                int a = 1;

            }
        }
        //public void GetToken222()
        //{

        //    using (DBController db = new DBController())
        //    {

        //        Tocken tk = new Tocken();
        //        tk.TockenID = Guid.NewGuid().ToString();
        //        tk.UpdateTime = DateTime.Now;
        //        tk.UserName = "lxm";

        //        db.Tocken.Add(tk);
        //        db.SaveChanges();
        //        int a = 1;

        //    }
        //}
    }
}
