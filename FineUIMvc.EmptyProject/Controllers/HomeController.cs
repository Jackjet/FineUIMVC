using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Text;
using FineUIMvc.PumpMVC.Models;
using FineUIMvc.PumpMVC.DAL;
using System.Data;

namespace FineUIMvc.PumpMVC.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        DBController db = new DBController();
        public ActionResult Index()
        {
            string host = Request.Url.Host.ToString();
            LoginPage LoginPage = db.LoginPage.ToList().Where(x => x.Host == host).FirstOrDefault();
            if (LoginPage != null)
            {
                ViewBag.Message = LoginPage.Title;
                ViewBag.Logo = "/LoginContent/" + LoginPage.FileName + "/img/pandaLogo.png";
            }
            else
            {
                ViewBag.Message = "熊猫智慧水务";
                ViewBag.Logo = "/LoginContent/default/img/pandaLogo.png";
            }
            LoadData();
            var dt = WebPortalDal.SearchWebP_UserTable(" and a.[Type]=4 and  UserID=" + Convert.ToInt32(User.Identity.Name));
            if (dt.Rows.Count != 0)
            {
                ViewBag.Url = dt.Rows[0]["Address"].ToString();
            }
            else
            {
                if (GetUserType().Equals("2"))  //如果登录用户是分公司
                {
                    var dt1 = WebPortalDal.SearchWebP_UserTable(" and a.[Type]=2 and  a.DepID='" +GetUserCompanyNumber()+"'");
                    if (dt1.Rows.Count != 0)
                    {
                        ViewBag.Url = dt1.Rows[0]["Address"].ToString();
                    }
                    else
                    {
                        ViewBag.Url = "~/Admin/Default";
                    }
                }
                else if (GetUserType().Equals("3"))  //如果登录用户是客户
                {
                    var dt2 = WebPortalDal.SearchWebP_UserTable(" and a.[Type]=3 and  a.CustomerID='" + GetUserCustomer()+"'");
                    if (dt2.Rows.Count != 0)
                    {
                        ViewBag.Url = dt2.Rows[0]["Address"].ToString();
                    }
                    else
                    {
                        ViewBag.Url = "~/Admin/Default";
                    }
                }
                else
                {
                    ViewBag.Url = "~/Admin/Default";
                }
            }
            return View();
        }

        #region LoadData

        private string _cookieMenuStyle = "tree_minimode";
        private bool _cookieShowOnlyBase = false;
        private string _cookieMenuMode = "normal";
        private string _cookieLang = "zh_CN";
        private string _cookieSearchText = "";
        // 示例数
        private int _examplesCount = 0;

        private void LoadData()
        {
            HttpCookie cookie = null;

            // 从Cookie中读取 - 左侧菜单类型
            cookie = Request.Cookies["MenuStyle_Mvc"];
            if (cookie != null)
            {
                _cookieMenuStyle = cookie.Value;
            }

            // 从Cookie中读取 - 显示模式
            cookie = Request.Cookies["MenuMode_Mvc"];
            if (cookie != null)
            {
                _cookieMenuMode = cookie.Value;
            }

            // 从Cookie中读取 - 语言
            cookie = Request.Cookies["Language_Mvc"];
            if (cookie != null)
            {
                _cookieLang = cookie.Value;
            }


            // 从Cookie中读取 - 搜索文本
            cookie = Request.Cookies["SearchText_Mvc"];
            if (cookie != null)
            {
                _cookieSearchText = HttpUtility.UrlDecode(cookie.Value);
            }

            ViewBag.LoginName = Panda_UserInfoDal.Get(GetIdentityName())["UserName"].ToString();
            ViewBag.CookieMenuStyle = _cookieMenuStyle;
            ViewBag.CookieShowOnlyBase = _cookieShowOnlyBase;
            ViewBag.CookieIsBase = Constants.IS_BASE;
            ViewBag.CookieMenuMode = _cookieMenuMode;
            ViewBag.CookieLang = _cookieLang;
            ViewBag.CookieSearchText = _cookieSearchText;

            ViewBag.ProductVersion = "v" + GlobalConfig.ProductVersion;
            ViewBag.OnlineUserCount = 1;

            LoadTreeMenuData();

            // 加载完树菜单数据后，计算标题栏文本
            if (_cookieShowOnlyBase)
            {
                ViewBag.TreeMenuTitle = String.Format("系统导航（{0}）", _examplesCount);
            }
            else
            {
                ViewBag.TreeMenuTitle = String.Format("系统导航（{0}）", _examplesCount);
            }
            LoadTopTree();
        }
        /// <summary>
        /// 根据用户获取网页顶部导航菜单
        /// </summary>
        private void LoadTopTree()
        {
            List<Menus> ResolveUserMenu = ResolveUserTopMenuList();
            ViewBag.Menus = ResolveUserMenu;
        }

        private List<Menus> ResolveUserTopMenuList()
        {
            // 当前登陆用户的权限列表
            List<string> rolePowerNames = GetRolePowerNames();

            // 当前用户所属角色可用的菜单列表
            List<Menus> menus = new List<Menus>();
            FormsAuthenticationTicket ticket = ((FormsIdentity)User.Identity).Ticket;
            foreach (Menus menu in MenuHelper.TopMenus(ticket.Name))
            {
                // 如果此菜单不属于任何模块，或者此用户所属角色拥有对此模块的权限
                if (menu.PowerName == null || rolePowerNames.Contains(menu.PowerName))
                {
                    menus.Add(menu);
                }
            }
            return menus;
        }

        private void LoadTreeMenuData()
        {
            IList<TreeNode> nodes = new List<TreeNode>();
            List<Menus> ResolveUserMenu = ResolveUserMenuList();
            ResolveXmlNodeList(nodes, ResolveUserMenu.Where(x => x.ParentID == 0).ToList(), ResolveUserMenu);

            // 视图数据
            ViewBag.TreeMenuNodes = nodes.ToArray();
        }

        // 获取用户可用的菜单列表
        private List<Menus> ResolveUserMenuList()
        {
            // 当前登陆用户的权限列表
            List<string> rolePowerNames = GetRolePowerNames();

            // 当前用户所属角色可用的菜单列表
            List<Menus> menus = new List<Menus>();

            foreach (Menus menu in MenuHelper.Menus)
            {
                // 如果此菜单不属于任何模块，或者此用户所属角色拥有对此模块的权限
                if (menu.PowerName == null || rolePowerNames.Contains(menu.PowerName))
                {
                    menus.Add(menu);
                }
            }

            return menus;
        }

        private int ResolveXmlNodeList(IList<TreeNode> nodes, List<Menus> menusChild, List<Menus> menus)
        {
            // nodes 中渲染到页面上的节点个数
            int nodeVisibleCount = 0;

            foreach (Menus xmlNode in menusChild)
            {
                TreeNode node = new TreeNode();

                // 是否叶子节点
                bool isLeaf = xmlNode.have_child == 0;

                bool currentNodeIsVisible = true;

                string nodeText = "";

                nodeText = xmlNode.Name;

                int childVisibleCount = 0;
                if (isLeaf)
                {
                }
                else
                {
                    // 递归
                    DBController db = new DBController();
                    List<Menus> menu = menus.Where(x => x.ParentID == xmlNode.ID).ToList();
                    childVisibleCount = ResolveXmlNodeList(node.Nodes, menu, menus);

                    if (childVisibleCount == 0)
                    {
                        currentNodeIsVisible = false;
                    }
                    else
                    {
                        // 存在搜索文本
                        if (!String.IsNullOrEmpty(_cookieSearchText))
                        {
                            // 展开节点
                            node.Expanded = true;
                        }
                    }
                }

                if (currentNodeIsVisible)
                {
                    string name = xmlNode.Name;
                    string value = xmlNode.ID.ToString();

                    node.Text = name;
                    node.SetPropertyValue(name, value);
                    node.NavigateUrl = string.IsNullOrWhiteSpace(xmlNode.NavigateUrl) ? "" : xmlNode.NavigateUrl;
                    //node.NavigateUrl = "http://fineui.com/version_mvc/index.html?from=demo";
                    node.IconUrl = xmlNode.ImageUrl;
                    node.ToolTip = xmlNode.Name;
                    nodes.Add(node);

                    // 本子节点显示
                    nodeVisibleCount++;

                    // 示例数只计算叶子节点
                    if (isLeaf)
                    {
                        _examplesCount++;
                    }

                }

            }

            return nodeVisibleCount;
        }


        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult onSignOutClick()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            FormsAuthentication.RedirectToLoginPage();
            return UIHelper.Result();
        }


        // GET: Themes
        public ActionResult Themes()
        {
            return View();
        }

        public ActionResult IPMAC()
        {
            return View();
        }

        public ActionResult MobileTextsss()
        {
            Panda_PumpJZ model = db.Panda_PumpJZ.Where(x => x.DTUCode.Equals("03170519236")).FirstOrDefault();
            E_DATA_MAIN data = db.E_DATA_MAIN.Where(x => x.BaseID.Equals(model.ID)).FirstOrDefault();
            DataTable dt = publicDal.TableSearch(@"select RIGHT(a.FDTUCode,3)+'('+RIGHT(a.FName,1)+')' FName,b.FMpa from BASE_YALI a,DATA_YALI_MAIN b 
                                                   where a.id=b.BASEID and a.FDTUCode in('03160926018','03160926076','03160926027','03160926028','03160926015','03160926026') 
                                                order by FName");

            ViewBag.txt_name = model.PumpJZName;
            ViewBag.txt_time = data.TempTime.ToString();
            ViewBag.txt_jinchuWat = data.F41006.ToString() + "/" + data.F41007.ToString();
            ViewBag.txt_jinchuShun = data.F41024.ToString() + "/" + data.F41025.ToString();
            ViewBag.txt_jinchuXian = data.F41109.ToString() + "/" + data.F41107.ToString() + "/" + data.F41113.ToString() + "/" + data.F41111.ToString();

            ViewBag.Lable1 = dt.Rows[0]["FName"].ToString();
            ViewBag.Text1 = dt.Rows[0]["FMpa"].ToString();
            ViewBag.Lable2 = dt.Rows[1]["FName"].ToString();
            ViewBag.Text2 = dt.Rows[1]["FMpa"].ToString();
            ViewBag.Lable3 = dt.Rows[2]["FName"].ToString();
            ViewBag.Text3 = dt.Rows[2]["FMpa"].ToString();
            ViewBag.Lable4 = dt.Rows[3]["FName"].ToString();
            ViewBag.Text4 = dt.Rows[3]["FMpa"].ToString();
            ViewBag.Lable5 = dt.Rows[4]["FName"].ToString();
            ViewBag.Text5 = dt.Rows[4]["FMpa"].ToString();
            ViewBag.Lable6 = dt.Rows[5]["FName"].ToString();
            ViewBag.Text6 = dt.Rows[5]["FMpa"].ToString();

            return View();
        }
    }
}