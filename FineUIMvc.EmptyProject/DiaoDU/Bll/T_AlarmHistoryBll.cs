using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace Bll
{
    public class T_AlarmHistoryBll
    {
        public static Hashtable Search(string key, int index, int size, string sortField, string sortOrder)
        {
            return Dal.T_AlarmHistoryDal.Search(key, index, size, sortField, sortOrder);
        }
        public static Hashtable Get(string id)
        {
            return Dal.T_AlarmHistoryDal.Get(id);
        }
        public static string Insert(Hashtable has)
        {
            return Dal.T_AlarmHistoryDal.Insert(has);
        }
        public static void Delete(string userId)
        {
            Dal.T_AlarmHistoryDal.Delete(userId);
        }
        public static void Update(Hashtable has)
        {
            Dal.T_AlarmHistoryDal.Update(has);
        }
    }
}