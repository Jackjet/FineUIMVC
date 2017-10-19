using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FineUIMvc.PumpMVC.DAL
{
    public class WebPortalDal
    {
        private const string str_WebPortalList = @"select ID,Name,[Address] from [dbo].[WebPortal] ";
        private const string str_WebP_UserList = @"select B.ID, b.Name,b.[Address],case when a.[Type]=4 then c.UserName when a.[Type]=2 then d.Fengongsi when a.[Type]=3 then e.Name end as UserName,
                                                   case when a.[Type]=4 then '用户' when a.[Type]=2 then '分公司' when a.[Type]=3 then '客户' end as [Type]   from [dbo].[WebP_User] a 
                                                   left join [dbo].[WebPortal] b on a.WId=b.ID
                                                   left join [dbo].[Panda_UserInfo] c on a.UserID=c.ID and a.[Type]=4
                                                   left join [dbo].[A_U_DEP]d on a.DepID=d.U8number and a.[Type]=2
                                                   left join [dbo].[Panda_Customer] e on a.CustomerID=e.ID and a.[Type]=3 where 1=1  ";
        private const string str_WebP_UserAdd = @"INSERT INTO WebP_User ( {0} ) VALUES( {1} )";
        private const string str_WebPortalAdd = @"INSERT INTO WebPortal ( {0} ) VALUES( {1} )";
        private const string str_WebP_UserDelete = @"delete from WebP_User where ID={0}";
        private const string strWebPortalUpd = @"UPDATE WebPortal SET {0} WHERE {1}";
        public static DataTable SearchTable(string strWhere)
        {
            string where = strWhere;

            string sql = str_WebPortalList;

            if (!where.Equals(""))
            {
                sql = sql + where + " order by a.Name";
            }

            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }
        public static Hashtable Search(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_WebPortalList);
        }
        public static Hashtable SearchWebP_User(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_WebP_UserList);
        }
        public static DataTable SearchWebP_UserTable(string strWhere)
        {
            string where = strWhere;

            string sql = str_WebP_UserList;

            if (!where.Equals(""))
            {
                sql = sql + where;
            }

            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }
        public static DataTable Exist(string name,int type)
        {
            string sql = string.Empty;
            if (type==4)
            {
                sql = @"select top 1 * from WebP_User where  [type]=" + type + "  and UserID=" + name + "";
            }
            if (type == 3)
            {
                sql = @"select top 1 * from WebP_User where[type]=" + type + "  and CustomerID=" + name + " ";
            }
            if (type == 2)
            {
                sql = @"select top 1 * from WebP_User where [type]=" + type + "  and DepID='" + name + "' ";
            }
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }
        public static DataTable WebPortalExist(string strWhere)
        {
            string where = strWhere;

            string sql = @"select top 1 * from WebPortal where 1=1 ";

            if (!where.Equals(""))
            {
                sql = sql + where;
            }
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }
        public static void Insert(Hashtable has)
        {
            publicDal.Insert(has, str_WebP_UserAdd);
        }
        public static int InsertWebPortal(Hashtable has)
        {
            return publicDal.InsertGetID(has, str_WebPortalAdd);
        }
        public static void DeletWebP_UserList(int ID)
        {
            string strDel = string.Format(str_WebP_UserDelete, ID);
            publicDal.Delete(strDel);
        }
        public static void DeleteList(Hashtable has)
        {
            publicDal.DeleteList(has, strWebPortalUpd, "ID");
        }


        public static void Update(Hashtable hasData)
        {
            publicDal.Update(hasData, strWebPortalUpd, "ID");
        }
    }
}