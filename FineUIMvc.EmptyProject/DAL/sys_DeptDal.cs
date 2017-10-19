using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace FineUIMvc.PumpMVC.DAL
{
    public class sys_DeptDal
    {
        #region const

        //部门
        private const string strDeptList = @"SELECT ID,Number,Name,ParentID,FCompanyNumber FROM dbo.Depts ";
        //分公司
        private const string strCompanyList = @"select * from A_U_DEP ";

        private const string strDept_CompanyList = @"select a.ID,a.Number,a.Name,a.FCompanyNumber,a.ParentID,b.Name as ParentName 
                                                       from Depts a inner join Depts b on a.FCompanyNumber=b.Number 
                                                      where a.FDeleted=0 and a.Number<>a.FCompanyNumber ";

        #endregion

        public static DataTable GetDeptList(string sortField, string sortOrder, string strWhere)
        {
            string where = strWhere;

            string sql = strDeptList;

            if (!where.Equals(""))
            {
                sql = sql + " where " + where;
            }

            if (String.IsNullOrEmpty(sortField) == false)
            {
                if (sortOrder != "DESC") sortOrder = "ASC";
                sql += " order by " + sortField + " " + sortOrder;
            }

            DataTable dt = publicDal.TableSearch(sql);
            return dt;
        }

        public static DataTable GetCompanyList(string strWhere)
        {
            string where = strWhere;

            string sql = strCompanyList;

            if (!where.Equals(""))
            {
                sql = sql +" where "+ where;
            }

            DataTable dt = publicDal.TableSearch(sql);
            return dt;
        }

        public static DataTable GetDeptCompanyList(string sortField, string sortOrder, string strWhere)
        {
            string where = strWhere;

            string sql = strDept_CompanyList;

            if (!where.Equals(""))
            {
                sql = sql + where;
            }

            if (String.IsNullOrEmpty(sortField) == false)
            {
                if (sortOrder != "DESC") sortOrder = "ASC";
                sql += " order by " + sortField + " " + sortOrder;
            }

            DataTable dt = publicDal.TableSearch(sql);
            return dt;
        }
    }
}