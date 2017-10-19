using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace Bll
{
    public class BaseToMarkerbll
    {
        public static Hashtable Search(string sql, int index, int size, string sortField, string sortOrder)
        {
            return Dal.BaseToMarkerDal.Search(sql, index, size, sortField, sortOrder);
        }
    }
}