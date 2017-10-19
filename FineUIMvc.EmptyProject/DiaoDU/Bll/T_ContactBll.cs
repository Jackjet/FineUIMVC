using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace Bll
{
    public class T_ContactBll
    {
        public static Hashtable Search(string key, int index, int size, string sortField, string sortOrder)
        {
            return Dal.T_ContactDal.Search(key, index, size, sortField, sortOrder);
        }
        public static Hashtable Get(string id)
        {
            return Dal.T_ContactDal.Get(id);
        }
        public static string Insert(Hashtable has)
        {
            return Dal.T_ContactDal.Insert(has);
        }
        public static void Delete(string userId)
        {
            Dal.T_ContactDal.Delete(userId);
        }
        public static void Update(Hashtable has)
        {
            Dal.T_ContactDal.Update(has);
        }
    }
}