using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using FineUIMvc;

namespace FineUIMvc.PumpMVC.DAL
{
    public class BASE_JIAYAZHAN_JZDal
    {
        #region const
        private const string strCRM_JYZList = @"select a.ID,DTUCode,jyzJZName,c.FName as jyzName,d.Name as CustomerName,
                                                        e.FName as AddressName,c.FCustomerID
                                                   from BASE_JIAYAZHAN_JZ a
                                              left join BASE_JIAYAZHAN c on a.jyzId=c.ID
                                              left join Panda_Customer d on c.FCustomerID=d.ID
                                              left join AddressScheme e on a.jyzJZAddressList=e.ID
                                                  where a.FIsDelete=0 ";

        private const string strCRM_JYZAdd = @"INSERT INTO BASE_JIAYAZHAN_JZ ( {0} ) VALUES( {1} )";

        private const string strCRM_JYZUpd = @"UPDATE BASE_JIAYAZHAN_JZ SET {0} WHERE {1}";

        private const string strCRM_JYZDel = @"DELETE FROM BASE_JIAYAZHAN_JZ WHERE id = @id";
        #endregion

        public static DataTable Exist(string strWhere)
        {
            string where = strWhere;

            string sql = @"select top 1 * from BASE_JIAYAZHAN_JZ where FIsDelete=0 ";

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