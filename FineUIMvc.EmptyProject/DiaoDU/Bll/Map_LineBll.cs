using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;

namespace Bll
{
    public class Map_LineBll
    {
        public static DataTable Search(string strWhere)
        {
            return Dal.Map_LineDal.Search(strWhere);
        }
        public static string InsertLine(Hashtable has)
        {
            return Dal.Map_LineDal.InsertLine(has);
        }
        public static string InsertLineProperty(Hashtable has)
        {
            return Dal.Map_LineDal.InsertLineProperty(has);
        }
        public static void UpdateLine(Hashtable has)
        {
            Dal.Map_LineDal.UpdateLine(has);
        }
        public static void UpdateLineProperty(Hashtable has)
        {
            Dal.Map_LineDal.UpdateLineProperty(has);
        }
    }
}