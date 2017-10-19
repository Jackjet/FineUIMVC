using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;

namespace Bll
{
    public class Map_MarkerBll
    {
        public static DataTable Search(string strWhere)
        {
            return Dal.Map_MarkerDal.Search(strWhere);
        }
        public static string InsertMarker(Hashtable has)
        {
            return Dal.Map_MarkerDal.InsertMarker(has);
        }
        public static string InsertMarkerProperty(Hashtable has)
        {
            return Dal.Map_MarkerDal.InsertMarkerProperty(has);
        }
        public static void UpdateMarker(Hashtable has)
        {
            Dal.Map_MarkerDal.UpdateMarker(has);
        }
        public static void UpdateMarkerProperty(Hashtable has)
        {
            Dal.Map_MarkerDal.UpdateMarkerProperty(has);
        }
        public static void DeleteMarker(int FType, string idList)
        {
            Dal.Map_MarkerDal.DeleteMarker(FType, idList);
        }
    }
}