using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;

namespace FineUIMvc.PumpMVC.DAL
{
    public class sys_rolesDal
    {
        #region const
        private const string str_Sys_RolesList = @"select *,TypeName=case when RType=0 then '普通角色'
                                                                          when RType=1 then '客户角色' end,b.Name as FCustomerName
                                                     from sys_Roles a
                                                left join Panda_Customer b on a.FCustomerID=b.ID where 1=1 ";

        private const string str_Sys_RolesUserList = @"select * from sys_Roles a
                                                   inner join sys_RoleUsers b on a.ID=b.RoleID";

        private const string str_Sys_RolesAdd = @"INSERT INTO sys_Roles ( {0} ) VALUES( {1} )";

        private const string str_Sys_RolesUpd = @"UPDATE sys_Roles SET {0} WHERE {1}";
        #endregion

        public static DataTable Exist(string strWhere)
        {
            string where = strWhere;

            string sql = @"select top 1 * from sys_Roles where 1=1 ";

            if (!where.Equals(""))
            {
                sql = sql + where;
            }
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }

        public static Hashtable Search(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            string where = strWhere;

            string sql = str_Sys_RolesList;

            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, sql);
        }

        public static DataTable SearchTable(string strWhere)
        {
            string where = strWhere;

            string sql = str_Sys_RolesList;

            if (!where.Equals(""))
            {
                sql = sql + where + " order by a.Name";
            }

            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }
        public static DataTable SearchRoleUser(string strWhere)
        {
            string where = strWhere;

            string sql = str_Sys_RolesUserList;

            if (!where.Equals(""))
            {
                sql = sql + " where " + where;
            }

            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }

        public static Int64 GetRoleUserCount(string id)
        {
            string sql = "select count(*) as countS from sys_RoleUsers where RoleID = '" + id + "'";
            DataTable dt = publicDal.TableSearch(sql);
            return Convert.ToInt64(dt.Rows[0][0].ToString());
        }
        //public static void Delete(string id)
        //{
        //    Hashtable has = new Hashtable();
        //    has["ID"] = id;
        //    DBUtil.Execute("delete from sys_Roles where ID = @ID", has);
        //}
        //public static void DeleteList(string id)
        //{
        //    Hashtable has = new Hashtable();
        //    has["ID"] = id;
        //    DBUtil.Execute("delete from sys_Roles where ID in(@ID)", has);
        //}

        public static void Insert(Hashtable has)
        {
            publicDal.Insert(has, str_Sys_RolesAdd);
        }

        public static void Update(Hashtable has)
        {
            publicDal.Update(has, str_Sys_RolesUpd, "ID");
        }

        public static void DeleteRoleUsersList(string ids, string RoleID)
        {
            Hashtable has = new Hashtable();

            has["ID"] = ids;
            publicDal.DeleteList(has, "delete from  sys_RoleUsers where  RoleID=" + RoleID + " and UserID in ({0})");
        }

        //public static void DeleteRoleUsers(string UserGH, string RoleID)
        //{
        //    Hashtable has = new Hashtable();
        //    has["UserGH"] = UserGH;
        //    has["RoleID"] = RoleID;
        //    DBUtil.Execute("delete from sys_RoleUsers where UserGH =@UserGH AND RoleID=@RoleID ", has);
        //}

        public static void InsertRoleUsers(Hashtable has)
        {
            publicDal.Insert(has, "insert into sys_RoleUsers ( {0} ) values( {1} )");
        }
    }
}