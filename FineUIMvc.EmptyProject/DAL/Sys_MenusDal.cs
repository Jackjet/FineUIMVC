using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;


namespace FineUIMvc.PumpMVC.DAL
{
    public class Sys_MenusDal
    {
        public static string strWhere(params string[] key)
        {

            string rewhere = string.Empty;

            for (int i = 0; i < key.Length; i++)
            {
                if (key[0] != "")
                {


                    string[] F = key[i].Split('=');

                    if (!string.IsNullOrWhiteSpace(F[1]))
                    {

                        switch (F[0])
                        {
                            case "a.ID":
                                rewhere = rewhere + " And (" + F[0] + " = '" + F[1] + "')";

                                break;

                            case "FName":
                                rewhere = rewhere + " And (" + F[0] + " like '%" + F[1] + "%')";

                                break;

                            case "ID":
                                rewhere = rewhere + " And " + F[0] + " = '" + F[1] + "'";
                                break;

                            case "a.ParentID":
                                rewhere = rewhere + " And " + F[0] + " = '" + F[1] + "'";
                                break;

                        }


                    }
                    else
                    {
                        if (F[0] == "ParentID")
                        {

                            rewhere = rewhere + " And ISNULL(" + F[0] + ",0)=0 ";

                        }


                    }
                }

            }


            return rewhere;


        }


        public static Hashtable Search(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            string where = strWhere;
            string sql = @"SELECT a.ID,a.Name,a.ImageUrl,a.NavigateUrl,a.Remark,a.SortIndex,b.ID AS PowerID,b.Name AS PowerName, 
                                  b.GroupName AS PowerGroupName,b.Title AS PowerTitle,b.Remark AS PowerRemark,a.ParentID AS ParentID,
                                  count(c.id) as have_child
                             FROM sys_Menus a
                        left join sys_Powers b ON a.ViewPowerID = b.ID 
                        left join sys_Menus c on a.id=c.ParentID where 1=1 "+ where +
                      @" group by a.ID,a.Name,a.ImageUrl,a.NavigateUrl,a.Remark,a.SortIndex,b.ID,b.Name, 
                                  b.GroupName,b.Title,b.Remark,a.ParentID 
                         order by SortIndex ";
            //return publicDal.HashSearch(index, size, sortField, sortOrder, where, sql);
            DataTable dt = publicDal.TableSearch(sql);
            // ArrayList data = DBUtil.DataTable2ArrayList(dt);

            Hashtable result = new Hashtable();
            result["data"] = dt;
            result["total"] = dt.Rows.Count;
            return result;
        }
        public static Hashtable SearchForTopMenus(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            string where = strWhere;
            string sql = @"SELECT a.ID,a.Name,a.ImageUrl,pct.NavigateUrl,a.Remark,a.SortIndex,b.ID AS PowerID,b.Name AS PowerName, 
                                  b.GroupName AS PowerGroupName,b.Title AS PowerTitle,b.Remark AS PowerRemark,a.ParentID AS ParentID
                             FROM sys_TopMenus a
                        left join sys_Powers b ON a.ViewPowerID = b.ID 
						left join Panda_CustomerTopMenu pct  on pct.TopMenuID=a.ID
						left join [dbo].[Panda_Customer] pc on pct.CustomerID=pc.ID
						left join [dbo].[Panda_UserInfo] pu on pu.FCustomerID=pc.id
                     where 1=1 " + where +
                      @" group by a.ID,a.Name,a.ImageUrl,a.NavigateUrl,a.Remark,a.SortIndex,b.ID,b.Name, 
                                  b.GroupName,b.Title,b.Remark,a.ParentID , pct.NavigateUrl
                         order by SortIndex ";
            //return publicDal.HashSearch(index, size, sortField, sortOrder, where, sql);
            DataTable dt = publicDal.TableSearch(sql);
            // ArrayList data = DBUtil.DataTable2ArrayList(dt);

            Hashtable result = new Hashtable();
            result["data"] = dt;
            result["total"] = dt.Rows.Count;
            return result;
        }


        //        public static Hashtable SearchRolePower(int index, int size, string sortField, string sortOrder, params string[] key)
        //        {
        //            string where = strWhere(key);
        //            string sql = @"SELECT * FROM sys_Menus A, Henry_RolePowers  B WHERE A.ID=B.PowerID  " + where;
        //            if (String.IsNullOrEmpty(sortField) == false)
        //            {
        //                if (sortOrder != "DESC") sortOrder = "ASC";
        //                sql += " order by " + sortField + " " + sortOrder;
        //            }

        //            DataTable dt = DBUtil.SelectDataTablePager(sql, index, size);
        //            // ArrayList data = DBUtil.DataTable2ArrayList(dt);

        //            Int64 count = DBUtil.ExecuteScalar(sql);

        //            Hashtable result = new Hashtable();
        //            result["data"] = dt;
        //            result["total"] = count;

        //            return result;
        //        }

        //        public static Hashtable SearchGroupName(int index, int size, string sortField, string sortOrder, params string[] key)
        //        {

        //            string sql = @"SELECT DISTINCT [GroupName] AS [GroupName] FROM [dbo].[sys_Menus] ";

        //            if (String.IsNullOrEmpty(sortField) == false)
        //            {
        //                if (sortOrder != "DESC") sortOrder = "ASC";
        //                sql += " order by " + sortField + " " + sortOrder;
        //            }

        //            // string sql = @"Select * FROM (SELECT DISTINCT [FGroupName] AS [FGroupName]FROM [dbo].[Henry_Powers]  ) A LEFT JOIN [dbo].[Henry_Powers] B ON A.FGroupName=B.FGroupName  ";

        //            DataTable dt = DBUtil.SelectDataTablePager(sql, index, size);


        //            // ArrayList data = DBUtil.DataTable2ArrayList(dt);

        //            Int64 count = DBUtil.ExecuteScalar(sql);

        //            Hashtable result = new Hashtable();
        //            result["data"] = dt;
        //            result["total"] = count;

        //            return result;
        //        }

        //        public static Hashtable SearchGroupList(int index, int size, string sortField, string sortOrder, params string[] key)
        //        {

        //            //string sql = @"SELECT DISTINCT [FGroupName] AS [FGroupName] FROM [dbo].[Henry_Powers] ";
        //            string where = strWhere(key);
        //            string sql = @"Select * FROM (SELECT DISTINCT [GroupName] AS [GroupName]FROM [dbo].[sys_Menus]  ) A LEFT JOIN [dbo].[sys_Menus] B ON A.GroupName=B.GroupName  where 1=1  " + where;

        //            DataTable dt = DBUtil.SelectDataTablePager(sql, index, size);


        //            // ArrayList data = DBUtil.DataTable2ArrayList(dt);

        //            Int64 count = DBUtil.ExecuteScalar(sql);

        //            Hashtable result = new Hashtable();
        //            result["data"] = dt;
        //            result["total"] = count;

        //            return result;
        //        }
        //        public static Hashtable Get(string id)
        //        {
        //            string sql = "select * from sys_Menus where ID = '" + id + "'";
        //            ArrayList data = DBUtil.Select(sql);
        //            return data.Count > 0 ? (Hashtable)data[0] : null;
        //        }


        //        public static string Insert(Hashtable has)
        //        {
        //            string id = (has["ID"] == null || has["ID"].ToString() == "") ? Guid.NewGuid().ToString() : has["ID"].ToString();
        //            //has["FID"] = id;

        //            string columns = "";
        //            string values = "";
        //            foreach (DictionaryEntry de in has)
        //            {
        //                columns += "" + de.Key + ",";
        //                values += "@" + de.Key + ",";
        //            }
        //            string sql = string.Format("insert into sys_Menus ( {0} ) values( {1} )", columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1));

        //            DBUtil.Execute(sql, has);
        //            return id;
        //        }

        //        public static string InsertRolePowers(Hashtable has)
        //        {
        //            string id = (has["ID"] == null || has["ID"].ToString() == "") ? Guid.NewGuid().ToString() : has["ID"].ToString();
        //            //has["FID"] = id;

        //            string columns = "";
        //            string values = "";
        //            foreach (DictionaryEntry de in has)
        //            {
        //                columns += "" + de.Key + ",";
        //                values += "@" + de.Key + ",";
        //            }



        //            string sql = string.Format("insert into sys_Menus ( {0} ) values( {1} )", columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1));

        //            DBUtil.Execute(sql, has);
        //            return id;
        //        }

        //        public static void Delete(int id)
        //        {
        //            Hashtable has = new Hashtable();
        //            has["ID"] = id;
        //            DBUtil.Execute("delete from sys_Menus where ID = @ID", has);
        //        }
        //        public static void Update(Hashtable has)
        //        {
        //            string set = "";
        //            string where = "";
        //            foreach (DictionaryEntry de in has)
        //            {
        //                if (de.Key.ToString() != "ID")
        //                {
        //                    set += "" + de.Key + "= @" + de.Key + ",";
        //                }
        //                else
        //                {
        //                    where += "" + de.Key + "= @" + de.Key + "";
        //                }
        //            }
        //            string sql = string.Format("update sys_Menus  set {0}  where {1}", set.Substring(0, set.Length - 1), where);

        //            DBUtil.Execute(sql, has);
        //        }

        //        /// <summary>
        //        /// 角色权限划分
        //        /// </summary>
        //        public static void RoleRightAssign(string roleid, string rights)
        //        {
        //            //删除角色所有权限
        //            String sql = "delete  from sys_Menus where roleid='" + roleid + "'";
        //            DataTable dt = DBUtil.SelectDataTable(sql);

        //            if (rights != "")
        //            {
        //                String[] ids = rights.Split(',');
        //                for (int i = 0, l = ids.Length; i < l; i++)
        //                {
        //                    string guid = Guid.NewGuid().ToString();
        //                    string id = ids[i];

        //                    sql = "insert into sys_Menus(id,roleid,rightid) values('" + guid + "','" + roleid + "','" + id + "')";
        //                    DBUtil.Execute(sql);
        //                }
        //            }
        //        }



        //        //获得RoleUserCount
        //        public static Int64 GetPowerRolesCount(string id)
        //        {
        //            string sql = "select * from sys_RolePowers where PowerID = '" + id + "'";
        //            Int64 count = DBUtil.ExecuteScalar(sql);
        //            return count;
        //        }


        //        public static string InsertRoleUsers(Hashtable has)
        //        {
        //            string id = (has["FID"] == null || has["FID"].ToString() == "") ? Guid.NewGuid().ToString() : has["FID"].ToString();
        //            //has["FID"] = id;

        //            string columns = "";
        //            string values = "";
        //            foreach (DictionaryEntry de in has)
        //            {
        //                columns += "" + de.Key + ",";
        //                values += "@" + de.Key + ",";
        //            }
        //            string sql = string.Format("insert into [sys_RoleUsers] ( {0} ) values( {1} )", columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1));

        //            DBUtil.Execute(sql, has);
        //            return id;
        //        }
        //        public static void DeleteRoleUsers(string ids, string RoleID)
        //        {
        //            Hashtable has = new Hashtable();

        //            has["RoleID"] = RoleID;
        //            DBUtil.Execute("delete from sys_RoleUsers where UserID in(" + ids + ") AND RoleID=@RoleID ", has);
        //        }
    }



}