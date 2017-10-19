using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;

namespace Bll
{
    public class Data_TiaoFengBll
    {
        public static DataTable Search(string strWhere)
        {
            return Dal.Data_TiaoFengDal.Search(strWhere);
        }
    }
}