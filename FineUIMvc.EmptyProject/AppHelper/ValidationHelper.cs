using FineUIMvc.PumpMVC.AppModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FineUIMvc.PumpMVC.AppHelper
{
    public class ValidationHelper
    {
        #region 用户登陆接口校验
        public static LoginInfo Login(AcceptLoginInfo usr)
        {
            LoginInfo model = new LoginInfo();
            model.Status = 1;

            if (string.IsNullOrWhiteSpace(usr.UserName))
            {
                model.Message = "用户名不能为空。";
                model.ErrorCode = "003";
                model.Status = 0;
                model.TokenID = "";
                return model;
            }

            if (string.IsNullOrWhiteSpace(usr.Md5))
            {
                model.Message = "密码不能为空。";
                model.ErrorCode = "104";
                model.Status = 0;
                model.TokenID = "";
                return model;
            }
            return model;

        }
        #endregion
    }
}