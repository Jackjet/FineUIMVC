using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FineUIMvc.PumpMVC.DAL
{
    public class BASE_SHUICHANG_JZDal
    {
        #region const
        private const string strCRM_SCList = @"select *,B.FName as MachineTypeName ,c.FName as ReadModeName from BASE_SHUICHANG_JZ a 
                                              left join sys_dictItems b on a.MachineType=b.FValue and b.FDictID=132   
                                             left join sys_dictItems c on a.ReadMode=c.FValue and c.FDictID=138    
                                             where a.FIsDelete=0  ";

        private const string strCRM_SCAdd = @"INSERT INTO BASE_SHUICHANG_JZ ( {0} ) VALUES( {1} )";

        private const string strCRM_SCUpd = @"UPDATE BASE_SHUICHANG_JZ SET {0} WHERE {1}";

        private const string strCRM_SCDel = @"DELETE FROM BASE_SHUICHANG_JZ WHERE id = @id";
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
            publicDal.Update(has, strCRM_SCUpd, "ID");
        }
        public static void DeleteList(Hashtable has)
        {
            publicDal.DeleteList(has, strCRM_SCUpd, "ID");
        }
    }
}