using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace Bll
{
    public class T_DataMainBll
    {
        public static Hashtable Search(string key, int index, int size, string sortField, string sortOrder)
        {
            return Dal.T_DataMainDal.Search(key, index, size, sortField, sortOrder);
        }
        public static Hashtable Get(string id)
        {
            return Dal.T_DataMainDal.Get(id);
        }
        public static string Insert(Hashtable has)
        {
            return Dal.T_DataMainDal.Insert(has);
        }
        public static void Delete(string userId)
        {
            Dal.T_DataMainDal.Delete(userId);
        }
        public static void Update(Hashtable has)
        {
            Dal.T_DataMainDal.Update(has);
        }
    }
}