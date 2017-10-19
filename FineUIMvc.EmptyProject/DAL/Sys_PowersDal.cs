using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using FineUIMvc;


namespace FineUIMvc.PumpMVC.DAL
{
    public class Sys_PowersDal
    {

        public static Hashtable Search(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            string where = strWhere;
            string sql = @"select  *  from sys_Powers WHERE 1=1 " + where;
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, sql);
        }

        public static Hashtable SearchRolePower(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            string where = strWhere;
            string sql = @"SELECT * FROM sys_Powers A, sys_RolePowers  B WHERE A.ID=B.PowerID  " + where;
            if (String.IsNullOrEmpty(sortField) == false)
            {
                if (sortOrder != "DESC") sortOrder = "ASC";
                sql += " order by " + sortField + " " + sortOrder;
            }

            DataTable dt = publicDal.TableSearch(sql);

            Hashtable result = new Hashtable();
            result["data"] = dt;
            result["total"] = 0;

            return result;
        }

        public static Hashtable SearchGroupName(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            string where = strWhere;
            string sql = @"SELECT DISTINCT [GroupName] AS [GroupName] FROM [dbo].[sys_Powers] ";
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

            Hashtable result = new Hashtable();
            result["data"] = dt;
            result["total"] = 0;

            return result;
        }

        public static Hashtable SearchGroupList(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            string where = strWhere;
            string sql = @"Select * FROM (SELECT DISTINCT GroupName FROM sys_Powers  ) A LEFT JOIN sys_Powers B ON A.GroupName=B.GroupName  where 1=1  " + where;

            DataTable dt = publicDal.TableSearch(sql);

            Hashtable result = new Hashtable();
            result["data"] = dt;
            result["total"] = 0;

            return result;
        }
        public static Hashtable Get(string id)
        {
            string sql = "select * from sys_Powers where ID = '" + id + "'";
            return publicDal.has_Select(sql);
        }

        public static ArrayList GetPowerNameList(string roleIDs)
        {
            string sql = "select Name from sys_Powers a,sys_RolePowers b where a.ID=B.PowerID AND B.RoleID  IN(" + roleIDs + ")";
            return publicDal.arr_Select(sql);
        }

        public static ArrayList GetPowerNameList()
        {
            string sql = "select Name from sys_Powers";
            return publicDal.arr_Select(sql);
        }


        public static Hashtable Get(string FName, string Type)
        {
            string sql = "select ID from sys_Powers where Name = '" + FName + "'";
            return publicDal.has_Select(sql);
        }

        public static DataTable SearchRolePowerTable(string strWhere)
        {
            string where = strWhere;

            string sql = "select * from sys_RolePowers ";

            if (!where.Equals(""))
            {
                sql = sql + " where " + where + " order by RoleID";
            }

            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }
        
     //   public static void Insert(Hashtable has)
     //   {
     //       string columns = "";
     //       string values = "";
     //       foreach (DictionaryEntry de in has)
     //       {
     //           columns += "" + de.Key + ",";
     //           values += "@" + de.Key + ",";
     //       }
     //       string sql = string.Format("insert into sys_Powers ( {0} ) values( {1} )", columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1));

     //       DBUtil.Execute(sql, has);
     //   }

     //   public static void InsertRolePowers(Hashtable has)
     //   {
     //       string columns = "";
     //       string values = "";
     //       foreach (DictionaryEntry de in has)
     //       {
     //           columns += "" + de.Key + ",";
     //           values += "@" + de.Key + ",";
     //       }

     //       string sql = string.Format("insert into sys_RolePowers ( {0} ) values( {1} )", columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1));

     //       DBUtil.Execute(sql, has);
     //   }

        public static void InsertRolePowers(Hashtable has)
        {
            //string columns = "";
            //string values = "";
            //foreach (DictionaryEntry de in has)
            //{
            //    columns += "" + de.Key + ",";
            //    values += "@" + de.Key + ",";
            //}

            //string sql = string.Format("insert into sys_RolePowers ( {0} ) values( {1} )", columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1));

            //DBUtil.Execute(sql, has);
            publicDal.Insert(has, "insert into sys_RolePowers ( {0} ) values( {1} )");
        }

        public void DeleteRowerPower(int id)
        {
            publicDal.Delete("delete from sys_RolePowers where RoleID = " + id);
        }
     //   public static void Update(Hashtable has)
     //   {
     //       string set = "";
     //       string where = "";
     //       foreach (DictionaryEntry de in has)
     //       {
     //           if (de.Key.ToString() != "ID")
     //           {
     //               set += "" + de.Key + "= @" + de.Key + ",";
     //           }
     //           else
     //           {
     //               where += "" + de.Key + "= @" + de.Key + "";
     //           }
     //       }
     //       string sql = string.Format("update sys_Powers  set {0}  where {1}", set.Substring(0, set.Length - 1), where);

     //       DBUtil.Execute(sql, has);
     //   }

     //   /// <summary>
     //   /// 角色权限划分
     //   /// </summary>
     //   public static void RoleRightAssign(string roleid, string rights)
     //   {
     //       //删除角色所有权限
     //       String sql = "delete  from sys_Powers where roleid='" + roleid + "'";
     //       DataTable dt = DBUtil.SelectDataTable(sql);

     //       if (rights != "")
     //       {
     //           String[] ids = rights.Split(',');
     //           for (int i = 0, l = ids.Length; i < l; i++)
     //           {
     //               string guid = Guid.NewGuid().ToString();
     //               string id = ids[i];

     //               sql = "insert into sys_role_right(id,roleid,rightid) values('" + guid + "','" + roleid + "','" + id + "')";
     //               DBUtil.Execute(sql);
     //           }
     //       }
     //   }



     //   //获得RoleUserCount
     //   public static Int64 GetPowerRolesCount(string id)
     //   {
     //       string sql = "select * from sys_RolePowers where PowerID = '" + id + "'";
     //       Int64 count = DBUtil.ExecuteScalar(sql);
     //       return count;
     //   }


     //   public static string InsertRoleUsers(Hashtable has)
     //   {
     //       string id = (has["ID"] == null || has["ID"].ToString() == "") ? Guid.NewGuid().ToString() : has["ID"].ToString();
     //       //has["FID"] = id;

     //       string columns = "";
     //       string values = "";
     //       foreach (DictionaryEntry de in has)
     //       {
     //           columns += "" + de.Key + ",";
     //           values += "@" + de.Key + ",";
     //       }
     //       string sql = string.Format("insert into [sys_RoleUsers] ( {0} ) values( {1} )", columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1));

     //       DBUtil.Execute(sql, has);
     //       return id;
     //   }
     //   public static void DeleteRoleUsers(string ids,string RoleID)
     //   {
     //       Hashtable has = new Hashtable();
           
     //       has["RoleID"] = RoleID;
     //       DBUtil.Execute("delete from sys_RoleUsers where UserGH in(" + ids + ") AND RoleID=@RoleID ", has);
     //   }


    }



}