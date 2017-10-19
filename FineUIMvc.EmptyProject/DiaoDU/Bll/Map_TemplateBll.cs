using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;

namespace Bll
{
    public class Map_TemplateBll
    {
        public static DataTable Search(string strWhere)
        {
            return Dal.Map_TemplateDal.Search(strWhere);
        }
        public static string InsertTemplate(Hashtable has)
        {
            return Dal.Map_TemplateDal.InsertTemplate(has);
        }
        public static string InsertTempProperty(Hashtable has)
        {
            return Dal.Map_TemplateDal.InsertTempProperty(has);
        }
        public static void UpdateTemp(Hashtable has)
        {
            Dal.Map_TemplateDal.UpdateTemp(has);
        }
        public static void UpdateTempProperty(Hashtable has)
        {
            Dal.Map_TemplateDal.UpdateTempProperty(has);
        }
    }
}