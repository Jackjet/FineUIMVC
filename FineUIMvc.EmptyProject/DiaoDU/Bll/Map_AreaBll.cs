using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;

namespace Bll
{
    public class Map_AreaBll
    {
        public static DataTable Search(string strWhere)
        {
            return Dal.Map_AreaDal.Search(strWhere);
        }
        public static DataTable SearchOverlay(string strWhere)
        {
            return Dal.Map_AreaDal.SearchOverlay(strWhere);
        }
        public static string InsertArea(Hashtable has)
        {
            return Dal.Map_AreaDal.InsertArea(has);
        }
        public static string InsertAreaProperty(Hashtable has)
        {
            return Dal.Map_AreaDal.InsertAreaProperty(has);
        }
        public static string InsertAreaOverlay(Hashtable has)
        {
            return Dal.Map_AreaDal.InsertAreaOverlay(has);
        }
        public static void UpdateArea(Hashtable has)
        {
            Dal.Map_AreaDal.UpdateArea(has);
        }
        public static void UpdateAreaProperty(Hashtable has)
        {
            Dal.Map_AreaDal.UpdateAreaProperty(has);
        }
        public static void DeleteAreaOverlay(string FMapOverlayID, string FMapTempID)
        {
            Dal.Map_AreaDal.DeleteAreaOverlay(FMapOverlayID, FMapTempID);
        }
    }
}