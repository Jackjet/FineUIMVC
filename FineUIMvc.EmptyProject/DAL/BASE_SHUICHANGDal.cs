using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using FineUIMvc;

namespace FineUIMvc.PumpMVC.DAL
{
    public class BASE_SHUICHANGDal
    {
        #region const
        private const string strCRM_SCList = @"select a.id,a.FCode,b.Name as CustomerName,FMapAddress,a.FName,FType
                                                 from BASE_SHUICHANG a
                                            left join Panda_Customer b on a.FCustomerID=b.ID
                                                where a.FIsDelete=0 ";

        private const string strCRM_SCAdd = @"INSERT INTO BASE_SHUICHANG ( {0} ) VALUES( {1} )";

        private const string strCRM_SCUpd = @"UPDATE BASE_SHUICHANG SET {0} WHERE {1}";

        private const string strCRM_SCDel = @"DELETE FROM BASE_SHUICHANG WHERE id = @id";
        #endregion

        public static DataTable Exist(string strWhere)
        {
            string where = strWhere;

            string sql = @"select top 1 * from BASE_SHUICHANG where FIsDelete=0 ";

            if (!where.Equals(""))
            {
                sql = sql + where;
            }
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }


        public static DataTable Search(string strWhere)
        {
            string where = strWhere;

            string sql = strCRM_SCList;

            if (!where.Equals(""))
            {
                sql = sql + where;
            }
            sql = sql + " ORDER BY a.FCreateDate desc ";
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }

        public static Hashtable Search(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, strCRM_SCList);
        }

        public static void Insert(Hashtable has)
        {
            publicDal.Insert(has, strCRM_SCAdd);
        }

        public static void Update(Hashtable has)
        {
            publicDal.Update(has, strCRM_SCUpd, "id");
        }
        public static void DeleteList(Hashtable has)
        {
            publicDal.DeleteList(has, strCRM_SCUpd, "id");
        }
    }
}