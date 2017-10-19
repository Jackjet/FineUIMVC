using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using System.Linq;
using System.Data;
using FineUIMvc.PumpMVC.Models;
using FineUIMvc.PumpMVC.DAL;

namespace FineUIMvc.PumpMVC.Controllers
{
    public class MenuHelper
    {
        private static List<Menus> _menus;
        private static List<Menus> _topMenus;

        public static List<Menus> Menus
        {
            get
            {
                if (_menus == null)
                {
                    InitMenus();
                }
                return _menus;
            }
        }

        public static List<Menus> TopMenus(string userName)
        {
            _topMenus = new List<Menus>();


            Hashtable table = Sys_MenusDal.SearchForTopMenus(0, 100, "SortIndex", "ASC", "and  pu.ID=" + Convert.ToInt32(userName));

            DataTable dt = (DataTable)table["data"];

            _topMenus = ModelConvertHelper<Menus>.ConvertToModel(dt).ToList(); //PageBase.DB.Menus.Include(m => m.ViewPower).OrderBy(m => m.SortIndex).ToList();
            return _topMenus;
        }
        public static void Reload()
        {
            _menus = null;
            _topMenus = null;
        }

        private static void InitMenus()
        {
            _menus = new List<Menus>();

            Hashtable table = Sys_MenusDal.Search(0, 100, "SortIndex", "ASC", " and a.ParentID=0");

            DataTable dt = (DataTable)table["data"];

            List<Menus> dbMenus = ModelConvertHelper<Menus>.ConvertToModel(dt).ToList(); //PageBase.DB.Menus.Include(m => m.ViewPower).OrderBy(m => m.SortIndex).ToList();

            ResolveMenuCollection(dbMenus, "0", 0);
        }
        private static void InitTopMenus()
        {
           

        }


        private static int ResolveMenuCollection(List<Menus> dbMenus, string _ParentID, int level)
        {
            int count = 0;

            Hashtable table = Sys_MenusDal.Search(0, 100, "SortIndex", "ASC", " and a.ParentID=" + _ParentID);
            DataTable dt = (DataTable)table["data"];

            List<Menus> dbClildMenus = ModelConvertHelper<Menus>.ConvertToModel(dt).ToList();


            foreach (var menu in dbClildMenus)
            {
                count++;

                _menus.Add(menu);
                menu.TreeLevel = level;
                menu.IsTreeLeaf = true;
                menu.Enabled = true;

                level++;
                int childCount = ResolveMenuCollection(dbMenus, menu.ID.ToString(), level);
                if (childCount != 0)
                {
                    menu.IsTreeLeaf = false;
                }
                level--;
            }

            return count;
        }
    }
}
