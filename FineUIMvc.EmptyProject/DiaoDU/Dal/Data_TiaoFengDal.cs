using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.Collections;

namespace Dal
{
    public class Data_TiaoFengDal
    {
        public static DataTable Search(string strWhere)
        {
            String sql = @"select a.id as baseId,FCustomerID,FDTUCode,FName,FSchemeID,b.*
                             from BASE_TIAOFENG a,DATA_TIAOFENG_MAIN b where a.id=b.BASEID";
            sql = sql + strWhere ;
            DataTable data = DBUtil.SelectDataTable(sql);
            return data;
        }
    }
}