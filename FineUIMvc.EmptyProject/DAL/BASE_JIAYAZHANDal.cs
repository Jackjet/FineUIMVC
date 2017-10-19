using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using FineUIMvc;

namespace FineUIMvc.PumpMVC.DAL
{
    public class BASE_JIAYAZHANDal
    {
        #region const
        private const string strCRM_JYZList = @" select a.id,a.FName, b.Name as CustomerName,a.FCode,c.Fengongsi,d.FName+e.FName as ProvinceCity 
                                                 from BASE_JIAYAZHAN a
                                            left join Panda_Customer b on a.FCustomerID=b.ID
                                            left join A_U_DEP c on a.FCompanyNumber=c.U8number
                                            left join sys_dictItems d on a.FProvince=d.FValue and d.FDictID=108
                                            left join sys_dictItems e on a.FCity=e.FValue and e.FDictID=120
                                                where a.FIsDelete=0 ";

        private const string strCRM_JYZAdd = @"INSERT INTO BASE_JIAYAZHAN ( {0} ) VALUES( {1} )";

        private const string strCRM_JYZUpd = @"UPDATE BASE_JIAYAZHAN SET {0} WHERE {1}";

        private const string strCRM_JYZDel = @"DELETE FROM BASE_JIAYAZHAN WHERE id = @id";
        #endregion

        public static DataTable Exist(string strWhere)
        {
            string where = strWhere;

            string sql = @"select top 1 * from BASE_JIAYAZHAN where FIsDelete=0 ";

            if (!where.Equals(""))
            {
                sql = sql + where;
            }
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }


        public static DataTable SearchJYZ(string strWhere)
        {
            string where = strWhere;

            string sql = strCRM_JYZList;

            if (!where.Equals(""))
            {
                sql = sql + where;
            }
            sql = sql + " ORDER BY a.FCreateDate desc ";
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }

        public static Hashtable SearchJYZ(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, strCRM_JYZList);
        }

        public static void Insert(Hashtable has)
        {
            publicDal.Insert(has, strCRM_JYZAdd);
        }

        public static void Update(Hashtable has)
        {
            publicDal.Update(has, strCRM_JYZUpd, "ID");
        }
        public static void DeleteList(Hashtable has)
        {
            publicDal.DeleteList(has, strCRM_JYZUpd, "ID");
        }
    }
}