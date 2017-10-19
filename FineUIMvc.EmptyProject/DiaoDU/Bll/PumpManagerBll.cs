using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace Bll
{
    public class PumpManagerBll 
    {
        public static Hashtable Search(string key, int index, int size, string sortField, string sortOrder)
        {
            return Dal.PumpManagerDal.Search(key, index, size, sortField, sortOrder);
        }
        public static Hashtable Get(string id)
        {
            return Dal.PumpManagerDal.Get(id);
        }
        public static string Insert(Hashtable has)
        {
            return Dal.PumpManagerDal.Insert(has);
        }
        public static void Delete(string userId)
        {
            Dal.PumpManagerDal.Delete(userId);
        }
        public static void Update(Hashtable has)
        {
            Dal.PumpManagerDal.Update(has);
        }
    }
}