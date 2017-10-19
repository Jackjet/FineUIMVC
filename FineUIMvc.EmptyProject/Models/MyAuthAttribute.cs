using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Collections;
using FineUIMvc.PumpMVC.DAL;

namespace FineUIMvc.PumpMVC.Models
{
    public class MyAuthAttribute : AuthorizeAttribute
    {
        public string MenuPower { get; set; }

        // 只需重载此方法，模拟自定义的角色授权机制
        //protected override bool AuthorizeCore(HttpContextBase httpContext)
        //{
        //    if (!httpContext.User.Identity.IsAuthenticated)//判断用户是否通过验证
        //        return false;
        //    //string[] StrRoles = Roles.Split(',');//通过逗号来分割允许进入的用户角色
        //    //string[] strMenuPowers = MenuPower.Split(',');
        //   if(CheckPower(httpContext))
        //   {
        //       return true;
        //   }
        //    else
        //   {
        //       return false;
        //   }
        //    //if (string.IsNullOrWhiteSpace(Roles))//如果只要求用户登录，即可访问的话
        //    //    return true;
        //    //bool isAccess = JudgeAuthorize(httpContext.User.Identity.Name, StrRoles);
        //    //if (StrRoles.Length > 0 && isAccess) //先判断是否有设用户权限，如果没有不允许访问
        //    //    return false;
        //}

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            HttpContextBase httpContext = filterContext.RequestContext.HttpContext;
            if (!httpContext.User.Identity.IsAuthenticated)//判断用户是否通过验证
            {
                filterContext.Result = new RedirectResult("/Login");
            }
            else
            {
                if (!CheckPower(httpContext))
                {
                    //权限验证未通过清楚Cooike,
                    FormsAuthentication.SignOut();
                    //权限验证未通过结束会话
                    HttpContext.Current.Session.Abandon();
                    filterContext.Result = new RedirectResult("/PowerError");
                }

            }
        }

        ///// <summary>
        ///// 根据用户名判断用户是否有对应的权限
        ///// </summary>
        ///// <param name="UserName"></param>
        ///// <param name="StrRoles"></param>
        ///// <returns></returns>
        //private bool JudgeAuthorize(string UserName, string[] StrRoles)
        //{
        //    string UserAuth = GetRole(UserName);  //从数据库中读取用户的权限
        //    return StrRoles.Contains(UserAuth,    //将用户的权限跟权限列表中做比较
        //    StringComparer.OrdinalIgnoreCase);  //忽略大小写
        //}

        //// 返回用户对应的角色， 在实际中， 可以从SQL数据库中读取用户的角色信息
        //private string GetRole(string name)
        //{
        //    switch (name)
        //    {
        //        case "aaa": return "User";
        //        case "bbb": return "Admin";
        //        case "4308": return "God";
        //        default: return "Fool";
        //    }
        //}
        #region 权限检查

        /// <summary>
        /// 检查当前用户是否拥有当前页面的浏览权限
        /// 页面需要先定义ViewPower属性，以确定页面与某个浏览权限的对应关系
        /// </summary>
        /// <returns></returns>
        protected bool CheckPowerView(HttpContextBase httpContext)
        {
            return CheckPower(httpContext);
        }

        /// <summary>
        /// 检查当前用户是否拥有某个权限
        /// </summary>
        /// <param name="powerType"></param>
        /// <returns></returns>
        protected bool CheckPower(HttpContextBase httpContext)
        {
            // 如果权限名为空，则放行
            if (String.IsNullOrEmpty(MenuPower))
            {
                return true;
            }

            // 当前登陆用户的权限列表
            List<string> rolePowerNames = GetRolePowerNames(httpContext);
            if (rolePowerNames.Contains(MenuPower))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 当前登录用户名
        /// </summary>
        /// <returns></returns>
        public string GetIdentityName(HttpContextBase httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                return httpContext.User.Identity.Name;
            }
            return String.Empty;
        }

        /// <summary>
        /// 获取当前登录用户拥有的全部权限列表
        /// </summary>
        /// <param name="roleIDs"></param>
        /// <returns></returns>
        protected List<string> GetRolePowerNames(HttpContextBase httpContext)
        {
            // 将用户拥有的权限列表保存在Session中，这样就避免每个请求多次查询数据库
            if (HttpContext.Current.Session["UserPowerList"] == null)
            {
                List<string> rolePowerNames = new List<string>();

                // 超级管理员拥有所有权限
                if (GetIdentityName(httpContext) == "admin")
                {
                    ArrayList PowerNames = Sys_PowersDal.GetPowerNameList();
                    foreach (Hashtable a in PowerNames)
                    {
                        rolePowerNames.Add(a["Name"].ToString());
                    }
                }
                else
                {
                    if (httpContext.User.Identity.IsAuthenticated)
                    {
                        FormsAuthenticationTicket ticket = ((FormsIdentity)httpContext.User.Identity).Ticket;
                        string userData = ticket.UserData;
                        if (!String.IsNullOrEmpty(userData))
                        {
                            ArrayList PowerNames = Sys_PowersDal.GetPowerNameList(userData);
                            if (PowerNames!=null)
                            {
                                foreach (Hashtable a in PowerNames)
                                {
                                    rolePowerNames.Add(a["Name"].ToString());
                                }
                            }
                        }
                    }
                }

                HttpContext.Current.Session["UserPowerList"] = rolePowerNames;
            }
            return (List<string>)HttpContext.Current.Session["UserPowerList"];
        }

        #endregion

    }
}