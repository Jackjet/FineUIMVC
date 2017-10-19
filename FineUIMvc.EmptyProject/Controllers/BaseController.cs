using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.Web.Security;
using FineUIMvc.PumpMVC.DAL;
using FineUIMvc.PumpMVC.Models;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Text;

namespace FineUIMvc.PumpMVC.Controllers
{
    public class BaseController : Controller
    {
        #region 只读静态变量

        // Session key
        private static readonly string SK_ONLINE_UPDATE_TIME = "OnlineUpdateTime";
        //private static readonly string SK_USER_ROLE_ID = "UserRoleId";

        private static readonly string CHECK_POWER_FAIL_PAGE_MESSAGE = "您无权访问此页面！";
        private static readonly string CHECK_POWER_FAIL_ACTION_MESSAGE = "您无权进行此操作！";



        #endregion

        #region 浏览权限

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public virtual string ViewPower
        {
            get
            {
                return String.Empty;
            }
        }

        #endregion

        #region OnActionExecuting

        /// <summary>
        /// 动作方法调用之前执行
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            // 如果用户已经登录，更新在线记录
            if (User.Identity.IsAuthenticated)
            {
               Hashtable has= Panda_UserInfoDal.Get(GetIdentityName());
                if(has!=null)
                {
                    UpdateOnlineUser();
                }
            }
        }


        #endregion

        #region 通知对话框

        /// <summary>
        /// 显示通知对话框
        /// </summary>
        /// <param name="message"></param>
        public virtual void ShowNotify(string message)
        {
            ShowNotify(message, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 显示通知对话框
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageIcon"></param>
        public virtual void ShowNotify(string message, MessageBoxIcon messageIcon)
        {
            ShowNotify(message, messageIcon, Target.Top);
        }

        /// <summary>
        /// 显示通知对话框
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageIcon"></param>
        /// <param name="target"></param>
        public virtual void ShowNotify(string message, MessageBoxIcon messageIcon, Target target)
        {
            Notify n = new Notify();
            n.Target = target;
            n.Message = message;
            n.MessageBoxIcon = messageIcon;
            n.PositionX = Position.Center;
            n.PositionY = Position.Top;
            n.DisplayMilliseconds = 3000;
            n.ShowHeader = false;

            n.Show();
        }

        #endregion

        #region 在线用户相关
        protected void UpdateOnlineUser()
        {
            DateTime now = DateTime.Now;
            object lastUpdateTime = Session[SK_ONLINE_UPDATE_TIME];
            if (lastUpdateTime == null || (Convert.ToDateTime(lastUpdateTime).Subtract(now).TotalMinutes > 5))
            {
                // 记录本次更新时间
                Session[SK_ONLINE_UPDATE_TIME] = now;
                if (!User.Identity.Name.Equals(""))
                {
                    Hashtable hasOnline = new Hashtable();
                    hasOnline["UserID"] = GetIdentityName();
                    hasOnline["UpdateTime"] = now;
                    Sys_OnlineDal.Update(hasOnline);
                }
            }
        }
        protected void RegisterOnlineUser(Hashtable user)
        {
            Hashtable hasOnline = new Hashtable();
            hasOnline = Sys_OnlineDal.Get(user["ID"].ToString());
            Hashtable hasInsertOnline = new Hashtable();
            DateTime now = DateTime.Now;
            // 如果不存在，就创建一条新的记录
            hasInsertOnline["UserID"] = user["ID"].ToString();
            hasInsertOnline["IPAdddress"] = Request.UserHostAddress;
            hasInsertOnline["LoginTime"] = now;
            hasInsertOnline["UpdateTime"] = now;
            if (hasOnline == null)
            {
                Sys_OnlineDal.Insert(hasInsertOnline);
            }

            Sys_OnlineDal.Update(hasInsertOnline);

            // 记录本次更新时间
            Session[SK_ONLINE_UPDATE_TIME] = now;

        }
        /// <summary>
        /// 在线人数
        /// </summary>
        /// <returns></returns>
        protected int GetOnlineCount()
        {
            DateTime lastM = DateTime.Now.AddMinutes(-15);
            Hashtable hasonlineCount = Sys_OnlineDal.Search(1, 1, "UpdateTime", "ASC", " and UpdateTime>'" + lastM + "'");
            return Convert.ToInt32(hasonlineCount["total"].ToString());
        }

        #endregion
        #region 当前登录用户信息

        // http://blog.163.com/zjlovety@126/blog/static/224186242010070024282/
        // http://www.cnblogs.com/gaoshuai/articles/1863231.html
        /// <summary>
        /// 当前登录用户的角色列表
        /// </summary>
        /// <returns></returns>
        protected List<int> GetIdentityRoleIDs()
        {
            List<int> roleIDs = new List<int>();

            if (User.Identity.IsAuthenticated)
            {
                FormsAuthenticationTicket ticket = ((FormsIdentity)User.Identity).Ticket;
                string userData = ticket.UserData;

                foreach (string roleID in userData.Split(','))
                {
                    if (!String.IsNullOrEmpty(roleID))
                    {
                        roleIDs.Add(Convert.ToInt32(roleID));
                    }
                }
            }

            return roleIDs;
        }

        public string strGetIdentityRoleIDs()
        {
            string roleIDs = "";

            if (User.Identity.IsAuthenticated)
            {
                FormsAuthenticationTicket ticket = ((FormsIdentity)User.Identity).Ticket;
                roleIDs = ticket.UserData;
            }

            return roleIDs;
        }

        /// <summary>
        /// 当前登录用户ID
        /// </summary>
        /// <returns></returns>
        public string GetIdentityName()
        {
            if (User.Identity.IsAuthenticated)
            {
                Session["UserID"] = User.Identity.Name;
                return User.Identity.Name;
            }
            return String.Empty;
        }

        /// <summary>
        /// 当前登录用户名
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Panda_UserInfoDal.Get(GetIdentityName())["UserName"].ToString();
            }
            return String.Empty;
        }

        /// <summary>
        /// 当前登录用户类型
        /// </summary>
        /// <returns></returns>
        public string GetUserType()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Panda_UserInfoDal.Get(GetIdentityName())["UserType"].ToString();
            }
            return String.Empty;
        }
        /// <summary>
        /// 当前登录用户客户ID
        /// </summary>
        /// <returns></returns>
        public string GetUserCustomer()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Panda_UserInfoDal.Get(GetIdentityName())["FCustomerID"].ToString();
            }
            return String.Empty;
        }
        /// <summary>
        /// 当前登录用户客户地图模板ID
        /// </summary>
        /// <returns></returns>
        public string GetCustomerMapTempID()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Panda_UserInfoDal.Get(GetIdentityName())["FMapTempID"].ToString();
            }
            return String.Empty;
        }

        /// <summary>
        /// 当前登录用户客户名
        /// </summary>
        /// <returns></returns>
        public string GetUserCustomerName()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Panda_UserInfoDal.Get(GetIdentityName())["CustomerName"].ToString();
            }
            return String.Empty;
        }

        /// <summary>
        /// 当前登录用户分公司Number
        /// </summary>
        /// <returns></returns>
        public string GetUserCompanyNumber()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Panda_UserInfoDal.Get(GetIdentityName())["FCompanyNumber"].ToString();
            }
            return String.Empty;
        }

        /// <summary>
        /// 当前登录用户泵房组
        /// </summary>
        /// <returns></returns>
        public string GetUserPumpGroup()
        {
            string pumpList = string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                string group= Panda_UserInfoDal.Get(GetIdentityName())["UserPumpGroup"].ToString();
                DataTable dt= Panda_PGroupDal.SearchGroupPump(" and a.GroupID='"+group+"'");
                for(int i=0;i<dt.Rows.Count;i++)
                {
                    pumpList = pumpList + "'"+ dt.Rows[i]["PumpID"] + "',";
                }
                return pumpList.Substring(0, pumpList.LastIndexOf(','));
            }
            return String.Empty;
        }

        //protected Hashtable GetUser(string UserID)
        //{
        //    return Sys_UserDal.Get(UserID);
        //}

        /// <summary>
        /// 创建表单验证的票证并存储在客户端Cookie中
        /// </summary>
        /// <param name="userName">当前登录用户名</param>
        /// <param name="roleIDs">当前登录用户的角色ID列表</param>
        /// <param name="isPersistent">是否跨浏览器会话保存票证</param>
        /// <param name="expiration">过期时间</param>
        protected void CreateFormsAuthenticationTicket(string userName, string roleIDs, bool isPersistent, DateTime expiration)
        {
            // 创建Forms身份验证票据
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                userName,                       // 与票证关联的用户名
                DateTime.Now,                   // 票证发出时间
                expiration,                     // 票证过期时间
                isPersistent,                   // 如果票证将存储在持久性 Cookie 中（跨浏览器会话保存），则为 true；否则为 false。
                roleIDs                         // 存储在票证中的用户特定的数据
             );

            // 对Forms身份验证票据进行加密，然后保存到客户端Cookie中
            string hashTicket = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashTicket);
            cookie.HttpOnly = true;
            // 1. 关闭浏览器即删除（Session Cookie）：DateTime.MinValue
            // 2. 指定时间后删除：大于 DateTime.Now 的某个值
            // 3. 删除Cookie：小于 DateTime.Now 的某个值
            if (isPersistent)
            {
                cookie.Expires = expiration;
            }
            else
            {
                cookie.Expires = DateTime.MinValue;
            }
            Response.Cookies.Add(cookie);
        }

        #endregion

        #region 权限检查

        /// <summary>
        /// 检查当前用户是否拥有当前页面的浏览权限
        /// 页面需要先定义ViewPower属性，以确定页面与某个浏览权限的对应关系
        /// </summary>
        /// <returns></returns>
        protected bool CheckPowerView()
        {
            return CheckPower(ViewPower);
        }

        /// <summary>
        /// 检查当前用户是否拥有某个权限
        /// </summary>
        /// <param name="powerType"></param>
        /// <returns></returns>
        protected bool CheckPower(string powerName)
        {
            // 如果权限名为空，则放行
            if (String.IsNullOrEmpty(powerName))
            {
                return true;
            }

            // 当前登陆用户的权限列表
            List<string> rolePowerNames = GetRolePowerNames();
            if (rolePowerNames.Contains(powerName))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取当前登录用户拥有的全部权限列表
        /// </summary>
        /// <param name="roleIDs"></param>
        /// <returns></returns>
        protected List<string> GetRolePowerNames()
        {
            // 将用户拥有的权限列表保存在Session中，这样就避免每个请求多次查询数据库
            if (Session["UserPowerList"] == null)
            {
                List<string> rolePowerNames = new List<string>();

                // 超级管理员拥有所有权限
                if (GetIdentityName() == "admin")
                {
                    ArrayList PowerNames = Sys_PowersDal.GetPowerNameList();
                    foreach (Hashtable a in PowerNames)
                    {
                        rolePowerNames.Add(a["Name"].ToString());
                    }
                }
                else
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        FormsAuthenticationTicket ticket = ((FormsIdentity)User.Identity).Ticket;
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

                Session["UserPowerList"] = rolePowerNames;
            }
            return (List<string>)Session["UserPowerList"];
        }


        protected bool CheckPumpControlPower(string controlName)
        {
            bool flag = false;
            string control = controlName.Replace("F", "").Replace("V", "");
            switch (control)
            {
                //case "41701": flag = CheckPower(control); break;                  //变更设备控制方式（0-本地控制，2、远程）
                //case "41702": flag = CheckPower(control); break;                  //设定出水压力
                case "41703": flag = CheckPower("pumpQT"); break;                 //控制泵1启停
                case "41704": flag = CheckPower("pumpQT"); break;                 //控制泵2启停
                case "41705": flag = CheckPower("pumpQT"); break;                 //控制泵3启停
                case "41706": flag = CheckPower("pumpQT"); break;                 //控制泵4启停
                case "41707": flag = CheckPower("pumpQT"); break;                 //控制泵5启停
                case "41708": flag = CheckPower("pumpQT"); break;                 //控制泵6启停
                case "41709": flag = CheckPower("fakaidu"); break;                //设置1#电动阀开度
                case "41710": flag = CheckPower("fakaidu"); break;                //复位1#电动阀故障
                case "41711": flag = CheckPower("fakaidu"); break;                //设置2#电动阀开度
                case "41712": flag = CheckPower("fakaidu"); break;                //复位2#电动阀故障
                //case "40311": flag = CheckPower(control); break;                  //变频器故障复位
                //case "41713": flag = CheckPower(control); break;                  //定时切换
                //case "41714": flag = CheckPower(control); break;                  //加泵延时
                //case "41717": flag = CheckPower(control); break;                  //开门或关门（0-无操作，1-开，2-关）
                //case "41718": flag = CheckPower(control); break;                  //开灯或关灯（0-无操作，1-开，2-关）
                case "41715": flag = CheckPower("fengshanwendu"); break;          //控制柜风扇启动温度
                case "41716": flag = CheckPower("fengshanwendu"); break;          //控制柜风扇停止温度
                //case "41720": flag = CheckPower(control); break;                  //启停水箱消毒仪
                //case "41108": flag = CheckPower(control); break;                  //设定运行时段方案
                //case "40320": flag = CheckPower(control); break;                  //视频预置监控点
                //case "40321": flag = CheckPower(control); break;                  //数据上传周期
                //case "41719": flag = CheckPower(control); break;                  //控制排水泵启停（0-无操作，1-开，2-关）
                case "41728": flag = CheckPower("jinchushuiyali"); break;                //进水压力下限
                case "41729": flag = CheckPower("jinchushuiyali"); break;                //进水压力上限
                case "41730": flag = CheckPower("jinchushuiyali"); break;                //出水压力下限
                case "41731": flag = CheckPower("jinchushuiyali"); break;                //出水压力上限
                default: flag = CheckPower(control); break;
            }

            return flag;
        }
        #endregion

        #region 权限相关

        #region 控制控件
        protected void CheckPowerFailWithPage()
        {
            Response.Write(CHECK_POWER_FAIL_PAGE_MESSAGE);
            Response.End();
        }

        protected void CheckPowerFailWithButton(Button btn)
        {
            btn.Enabled = false;
            btn.ToolTip = CHECK_POWER_FAIL_ACTION_MESSAGE;
        }

        protected void CheckPowerFailWithMenuButton(MenuButton btn)
        {
            btn.Enabled = false;
        }

        protected void CheckPowerFailWithLinkButtonField(Grid grid, string columnID)
        {
            //LinkButtonField btn = grid.FindColumn(columnID) as LinkButtonField;
            //btn.Enabled = false;
            //btn.ToolTip = CHECK_POWER_FAIL_ACTION_MESSAGE;
        }

        protected void CheckPowerFailWithWindowField(Grid grid, string columnID)
        {
            //WindowField btn = grid.FindColumn(columnID) as WindowField;
            //btn.Enabled = false;
            //btn.ToolTip = CHECK_POWER_FAIL_ACTION_MESSAGE;
        }

        protected void CheckPowerFailWithAlert()
        {
            PageContext.RegisterStartupScript(Alert.GetShowInTopReference(CHECK_POWER_FAIL_ACTION_MESSAGE));
        }

        protected void CheckPowerWithButton(string powerName, Button btn)
        {
            if (!CheckPower(powerName))
            {
                CheckPowerFailWithButton(btn);
            }
        }

        protected void CheckPowerWithMenuButton(string powerName, MenuButton btn)
        {
            if (!CheckPower(powerName))
            {
                CheckPowerFailWithMenuButton(btn);
            }
        }

        protected void CheckPowerWithLinkButtonField(string powerName, Grid grid, string columnID)
        {
            if (!CheckPower(powerName))
            {
                CheckPowerFailWithLinkButtonField(grid, columnID);
            }
        }

        protected void CheckPowerWithWindowField(string powerName, Grid grid, string columnID)
        {
            if (!CheckPower(powerName))
            {
                CheckPowerFailWithWindowField(grid, columnID);
            }
        }
        #endregion

        #region 控制列表显示数据sql
        public string getPowerConst(string tableName)
        {
            string sql = string.Empty;
            string userType = GetUserType();  //登录用户类型
            switch (tableName)
            {
                case "pump": 
                    switch(userType)
                    {
                        case "1": ; break;
                        case "2": sql = sql + " and a.PCompanyNumber=" + GetUserCompanyNumber(); break;
                        case "3": sql = sql + " and a.FCustomerID=" + GetUserCustomer(); break;
                        case "4": sql = sql + " and a.ID in(" + GetUserPumpGroup()+")"; break;
                    }; break;
                case "pumpJZ":
                    switch (userType)
                    {
                        case "1": ; break;
                        case "2": sql = sql + " and c.PCompanyNumber=" + GetUserCompanyNumber(); break;
                        case "3": sql = sql + " and c.FCustomerID=" + GetUserCustomer(); break;
                        case "4": sql = sql + " and c.ID in(" + GetUserPumpGroup() + ")"; break;
                    }; break;
                case "water":
                    switch (userType)
                    {
                        case "1": ; break;
                        case "3": sql = sql + " and a.FCustomerID=" + GetUserCustomer(); break;
                    }; break;
                case "flow":
                    switch (userType)
                    {
                        case "1": ; break;
                        case "3": sql = sql + " and a.FCustomerID=" + GetUserCustomer(); break;
                    }; break;
                case "famen":
                    switch (userType)
                    {
                        case "1": ; break;
                        case "3": sql = sql + " and a.FCustomerID=" + GetUserCustomer(); break;
                    }; break;
                case "jiayazhan":
                   switch (userType)
                    {
                        case "1": ; break;
                        case "3": sql = sql + " and a.FCustomerID=" + GetUserCustomer(); break;
                    }; break;
                case "pressure":
                    switch (userType)
                    {
                        case "1": ; break;
                        case "3": sql = sql + " and a.FCustomerID=" + GetUserCustomer(); break;
                    }; break;
                case "tiaofeng":
                   switch (userType)
                    {
                        case "1": ; break;
                        case "3": sql = sql + " and a.FCustomerID=" + GetUserCustomer(); break;
                    }; break;
            }
            return sql;
        }
        #endregion

        #endregion

        #region 模拟树的下拉列表

        protected List<T> ResolveDDL<T>(List<T> mys) where T : ICustomTree, ICloneable, IKeyID, new()
        {
            return ResolveDDL<T>(mys, -1, true);
        }

        protected List<T> ResolveDDL<T>(List<T> mys, int currentId) where T : ICustomTree, ICloneable, IKeyID, new()
        {
            return ResolveDDL<T>(mys, currentId, true);
        }


        // 将一个树型结构放在一个下列列表中可供选择
        protected List<T> ResolveDDL<T>(List<T> source, int currentID, bool addRootNode) where T : ICustomTree, ICloneable, IKeyID, new()
        {
            List<T> result = new List<T>();

            if (addRootNode)
            {
                // 添加根节点
                T root = new T();
                root.Name = "--根节点--";
                root.ID = -1;
                root.TreeLevel = 0;
                root.Enabled = true;
                result.Add(root);
            }

            foreach (T item in source)
            {
                T newT = (T)item.Clone();
                result.Add(newT);

                // 所有节点的TreeLevel加一
                if (addRootNode)
                {
                    newT.TreeLevel++;
                }
            }

            // currentId==-1表示当前节点不存在
            if (currentID != -1)
            {
                // 本节点不可点击（也就是说当前节点不可能是当前节点的父节点）
                // 并且本节点的所有子节点也不可点击，你想如果当前节点跑到子节点的子节点，那么这些子节点就从树上消失了
                bool startChileNode = false;
                int startTreeLevel = 0;
                foreach (T my in result)
                {
                    if (my.ID == currentID)
                    {
                        startTreeLevel = my.TreeLevel;
                        my.Enabled = false;
                        startChileNode = true;
                    }
                    else
                    {
                        if (startChileNode)
                        {
                            if (my.TreeLevel > startTreeLevel)
                            {
                                my.Enabled = false;
                            }
                            else
                            {
                                startChileNode = false;
                            }
                        }
                    }
                }
            }

            return result;
        }

        #endregion

        #region 单据编码
        public static string GetBillNo(string _TableName)
        {

            string reStr = string.Empty;
            string str0 = "00000000000000000";

            DataTable dt = tBillCodeRulesDal.Search("TableName = '" + _TableName + "'");

            DataRow dr = dt.Rows[0];

            int strLength = Convert.ToInt32(dr["Length"]);

            string strYMD = dr["YMD"].ToString();

            DateTime date = DateTime.Now;

            string currYMD = date.ToString("yyMMdd");

            string strSortCode = dr["SortCode"].ToString();

            string strLast = string.Empty;

            Hashtable hasData = new Hashtable();
            hasData["ID"] = Convert.ToInt32(dr["ID"]);

            if (strYMD.CompareTo(currYMD) != 0)
            {
                hasData["YMD"] = currYMD;
                hasData["NO"] = 2;
                strLast = str0.Substring(0, strLength - strSortCode.Length - currYMD.Length - 1);
                reStr = strSortCode + currYMD + strLast + 1;
            }
            else
            {
                strLast = str0.Substring(0, strLength - strSortCode.Length - currYMD.Length - ((Int32)dr["NO"] + 1).ToString().Length);
                reStr = strSortCode + currYMD + strLast + (Int32)dr["NO"];
                hasData["NO"] = (Int32)dr["NO"] + 1;
            }
            tBillCodeRulesDal.Update(hasData);

            return reStr;
        }
        #endregion

        public static StringBuilder successMsg(string msg, string IsSuccess, int count, string data)
        {
            StringBuilder str = new StringBuilder();
            return str.AppendFormat("{0}\"success\":" + IsSuccess + ",\"msg\":\"" + msg + "\",\"count\":{1},\"obj\":{2}{3}", "{", count, data, "}");
        }

        public static StringBuilder successMsg(string msg, string IsSuccess,string data)
        {
            StringBuilder str = new StringBuilder();
            return str.AppendFormat("{0}\"success\":" + IsSuccess + ",\"msg\":\"" + msg + "\",\"obj\":{1}{2}", "{",data, "}");
        }
        public static StringBuilder successMsg(string msg, string IsSuccess)
        {
            StringBuilder str = new StringBuilder();
            return str.AppendFormat("{0}\"success\":" + IsSuccess + ",\"msg\":\"" + msg + "\",\"obj\":[]{1}", "{", "}");
        }

        #region Dispose

        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }

        #endregion
    }
}