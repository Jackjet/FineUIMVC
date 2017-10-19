using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using FineUIMvc;

namespace FineUIMvc.PumpMVC.DAL
{
    public class tBillCodeRulesDal
    {
        #region const
        private const string strCRM_RuleList = @"SELECT ID,SortCode,BillKey,BillTitle,IsAuto,FormatDesc,Length,TableName,CodeColumnName,DateColumnName,YMD,NO
                                                       FROM tBillCodeRules";

        private const string strCRM_RuleUpd = @"UPDATE tBillCodeRules SET {0} WHERE {1}";
        #endregion

        public static DataTable Search(string strWhere)
        {
            string where = strWhere;

            string sql = strCRM_RuleList;

            if (!where.Equals(""))
            {
                sql = sql + " where " + where;
            }

            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }

        public static void Update(Hashtable has)
        {
            publicDal.Update(has, strCRM_RuleUpd, "ID");
        }
    }
}