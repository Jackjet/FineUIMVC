using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace Bll
{
    public class T_CustomerBll
    {
        public static Hashtable Search(string key, int index, int size, string sortField, string sortOrder)
        {
            return Dal.T_CustomerDal.Search(key, index, size, sortField, sortOrder);
        }
        public static Hashtable Get(string id)
        {
            return Dal.T_CustomerDal.Get(id);
        }
        public static string Insert(Hashtable has)
        {
            return Dal.T_CustomerDal.Insert(has);
        }
        public static void Delete(string userId)
        {
            Dal.T_CustomerDal.Delete(userId);
        }
        public static void Update(Hashtable has)
        {
            Dal.T_CustomerDal.Update(has);
        }
        
    }
}