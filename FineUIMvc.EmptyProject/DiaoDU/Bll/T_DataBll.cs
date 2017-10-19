using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace Bll
{
    public class T_DataBll
    {
        public static Hashtable Search(string key, int index, int size, string sortField, string sortOrder)
        {
            return Dal.T_DataDal.Search(key, index, size, sortField, sortOrder);
        }
        public static Hashtable Get(string id)
        {
            return Dal.T_DataDal.Get(id);
        }
        public static string Insert(Hashtable has)
        {
            return Dal.T_DataDal.Insert(has);
        }
        public static void Delete(string userId)
        {
            Dal.T_DataDal.Delete(userId);
        }
        public static void Update(Hashtable has)
        {
            Dal.T_DataDal.Update(has);
        }
    }
}