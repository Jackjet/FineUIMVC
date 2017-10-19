using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace Bll
{
    public class Sys_UserBll
    {
        public static Hashtable Search(string key, int index, int size, string sortField, string sortOrder)
        {
            return Dal.Sys_UserDal.Search(key, index, size, sortField, sortOrder);
        }
        public static Hashtable Get(string id)
        {
            return Dal.Sys_UserDal.Get(id);
        }
        public static string Insert(Hashtable has)
        {
            return Dal.Sys_UserDal.Insert(has);
        }
        public static void Delete(string userId)
        {
            Dal.Sys_UserDal.Delete(userId);
        }
        public static void Update(Hashtable has)
        {
            Dal.Sys_UserDal.Update(has);
        }
    
    }
}