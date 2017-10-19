using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;

namespace Bll
{
    public class Data_YaLiBll
    {
        public static Hashtable SearchYALI(string strWhere, int index, int size, string sortField, string sortOrder)
        {
            return Dal.Data_YaLiDal.SearchYALI(strWhere, index, size, sortField, sortOrder);
        }
        public static Hashtable SearchYALI_Year(string strWhere, int index, int size, string sortField, string sortOrder, int year)
        {
            return Dal.Data_YaLiDal.SearchYALI_Year(strWhere, index, size, sortField, sortOrder, year);
        }
        public static string InsertYALI(Hashtable has)
        {
            return Dal.Data_YaLiDal.InsertYALI(has);
        }
        public static string InsertYALI_MAIN(Hashtable has)
        {
            return Dal.Data_YaLiDal.InsertYALI_MAIN(has);
        }
        public static void UpdateYALI(Hashtable has)
        {
            Dal.Data_YaLiDal.UpdateYALI(has);
        }
        public static void UpdateYALI_MAIN(Hashtable has)
        {
            Dal.Data_YaLiDal.UpdateYALI_MAIN(has);
        }
        public static DataTable SearchInsertAlarm(string strWhere, string BaseID)
        {
            return Dal.Data_YaLiDal.SearchInsertAlarm(strWhere, BaseID);
        }
     
    }
}