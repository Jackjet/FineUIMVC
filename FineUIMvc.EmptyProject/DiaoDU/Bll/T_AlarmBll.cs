using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace Bll
{
    public class T_AlarmBll
    {
        //public static Hashtable Search(string key, int index, int size, string sortField, string sortOrder)
        //{
        //    return Dal.T_AlarmDal.Search(key, index, size, sortField, sortOrder);
        //}
        //public static Hashtable Get(string id)
        //{
        //    return Dal.T_AlarmDal.Get(id);
        //}
        //public static string Insert(Hashtable has)
        //{
        //    return Dal.T_AlarmDal.Insert(has);
        //}
        //public static void Delete(string userId)
        //{
        //    Dal.T_AlarmDal.Delete(userId);
        //}
        //public static void Update(Hashtable has)
        //{
        //    Dal.T_AlarmDal.Update(has);
        //}
        public static DataTable SearchAlarm(string strWhere, int id)
        {
            return Dal.T_AlarmDal.SearchAlarm(strWhere, id);
        }
    }
}