using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using FineUIMvc;


namespace FineUIMvc.PumpMVC.DAL
{
    public class Sys_UserDal
    {
        private const string strGetSelDept_User = @"SELECT LoginType=CASE WHEN LoginType='0' THEN 'OA' WHEN LoginType='1' THEN 'CRM' WHEN LoginType='2' THEN 'OA/CRM' ELSE '' END,a.ID,Gender,a.Name as WorkCode,ChineseName,b.ID as DeptID,b.Number as DeptCode,b.Name as DeptName,c.ID as CompanyID, c.Name as CompanyName,c.Number as FCompanyNumber 
                                                      FROM Users a,Depts b,Depts c
                                                     WHERE a.DeptID=b.ID and b.FCompanyNumber=c.Number and Deleted=0 ";

        private const string strGetSel_User = @"SELECT LoginType=CASE WHEN LoginType='0' THEN 'OA' WHEN LoginType='1' THEN 'CRM' WHEN LoginType='2' THEN 'OA/CRM' ELSE '' END,a.ID,Gender,a.Name,ChineseName,b.ID as DeptID,b.Number as DeptCode,b.Name as DeptName,a.Enabled
                                                      FROM Users a,Depts b
                                                     WHERE a.DeptID=b.ID and Deleted=0 ";

        private const string strGetSelRole_User = @"select a.*,c.Name as FCompanyName,UserName,UserTel,UserMail,d.FName as EnableName,e.FName as UserTypeName,b.FCreateDate,
                                                              FCustomerName=case when b.FCustomerID>0 then f.Name when b.FCompanyNumber <>'' and b.FCompanyNumber is not null then c.Name end
                                                              from sys_RoleUsers a
                                                    left join Panda_UserInfo b on a.UserID=b.ID
                                                    left join Depts c on b.FCompanyNumber=c.Number
                                                    left join sys_dictItems d on b.UserEnabledisable=d.FValue and d.FDictID=54
                                                    left join sys_dictItems e on b.UserType=e.FValue and e.FDictID=53
                                                    left join Panda_Customer f on b.FCustomerID=f.ID 
                                                        where b.FIsDelete=0 ";

//        public static Hashtable SearchUserPageList(int index, int size, string sortField, string sortOrder, string strWhere)
//        {
//            string where = strWhere;

//            string sql = strGetSel_User;

//            if (!where.Equals(""))
//            {
//                sql = sql + where;
//            }

//            if (String.IsNullOrEmpty(sortField) == false)
//            {
//                if (sortOrder != "DESC") sortOrder = "ASC";
//                sql += " order by " + sortField + " " + sortOrder + " ";
//                int StartRecord = 0;
//                StartRecord = index * size;
//                sql += " offset " + StartRecord + " rows fetch next " + size + " rows only";
//            }
//            DataTable dt = DBUtil.SelectDataTable(sql);
//            Int64 count = DBUtil.ExecuteScalar(sql);

//            Hashtable result = new Hashtable();
//            result["data"] = dt;
//            result["total"] = count;

//            return result;
//        }

//        public static DataTable GetSelDept_User(string strWhere)
//        {
//            string where = strWhere;

//            string sql = strGetSelDept_User;

//            if (!where.Equals(""))
//            {
//                sql = sql  + where;
//            }

//            DataTable data = DBUtil.SelectDataTable(sql);
//            return data;
//        }

//        public static Hashtable SearchRoleUsers(int index, int size, string sortField, string sortOrder, string strWhere)
//        {
//            string where = strWhere;
////            string sql = @" select a.*,b.ChineseName,c.Name,c.Number from sys_RoleUsers a 
////                        inner join  Users b on a.UserGH=b.Name
////                        inner join  Depts c on b.DeptID=c.ID " + where;

//            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, strGetSelRole_User);
//        }

//        public static Hashtable GetOA(string username, string password)
//        {
//            string sql = @"select top 1 B.Name UserName,WORKCODE,LASTNAME,A.PASSWORD Password,B.Password crmPassword , Enabled 
//                           from oadbserver.ecology.dbo.hrmresource A,Users B 
//                           WHERE A.WORKCODE=B.Name And A.WORKCODE = '" + username + "'";
//            //DBUtilConn.CONN_DEF = "connectionSH";
//            ArrayList data = DBUtil.Select(sql);
//            return data.Count > 0 ? (Hashtable)data[0] : null;
//        }
//        public static DataTable GetType(string username, string password)
//        {
//            DataTable dt = new DataTable();
//            string sql = @"select top 1 Name UserName,Password crmPassword , Enabled ,LoginType
//                           from Users WHERE Name = '" + username + "'";
//            dt = DBUtil.SelectDataTable(sql);
//            //ArrayList data = DBUtil.Select(sql);
//            //return data.Count > 0 ? (Hashtable)data[0] : null;
//            return dt;
//        }
//        public static Hashtable GetLogin(string username, string password)
//        {
//            string sql = @"select top 1 Name UserName,Password UserPassword, Enabled 
//                           from Users WHERE Name = '" + username + "'";
//            return publicDal.has_Select(sql);
//        }

        //public static Hashtable Get(string id)
        //{
        //    string sql = "select * from Users where Name = '" + id + "'";
        //    return publicDal.has_Select(sql);
        //}

        //public static ArrayList GetRole(string UserID)
        //{
        //    string sql = "select RoleID from sys_RoleUsers where UserGH = '" + UserID + "'";
        //    return publicDal.arr_Select(sql);
        //}
//        public static DataTable GetTables(string username)
//        {
//            DataTable dt = new DataTable();
//            string sql = "select * from Users where Name = '" + username + "'";
//            dt = DBUtil.SelectDataTable(sql);
//            //ArrayList data = DBUtil.Select(sql);
//            //return data.Count > 0 ? (Hashtable)data[0] : null;
//            return dt;
//        }
//        public static string Insert(Hashtable has)
//        {
//            string id = (has["id"] == null || has["id"].ToString() == "") ? Guid.NewGuid().ToString() : has["id"].ToString();
//            has["id"] = id;

//            string columns = "";
//            string values = "";
//            foreach (DictionaryEntry de in has)
//            {
//                columns += "" + de.Key + ",";
//                values += "@" + de.Key + ",";
//            }
//            string sql = string.Format("insert into Users ( {0} ) values( {1} )", columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1));

//            DBUtil.Execute(sql, has);
//            return id;
//        }
//        public static void Delete(string id)
//        {
//            Hashtable has = new Hashtable();
//            has["id"] = id;
//            DBUtil.Execute("delete from Users where id = @id", has);
//        }
        public static void Update(Hashtable has)
        {
            //string set = "";
            //string where = "";
            //foreach (DictionaryEntry de in has)
            //{
            //    if (de.Key.ToString() != "Name")
            //    {
            //        set += "" + de.Key + "= @" + de.Key + ",";
            //    }
            //    else
            //    {
            //        where += "" + de.Key + "= @" + de.Key + "";
            //    }
            //}
            //string sql = string.Format("update Users  set {0}  where {1}", set.Substring(0, set.Length - 1), where);

            //DBUtil.Execute(sql, has);
            publicDal.Update(has, "update Users  set {0}  where {1}", "Name");
        }

//        public static void UpdList(Hashtable has)
//        {
//            PublicClass.UpdList(has, "update Users  set {0}  where {1}", "Name");
//        }



//        /// <summary>
//        /// 用户角色划分
//        /// </summary>
//        public static void UserRoleAssign(string userid, string roles)
//        {
//            //删除角色所有权限
//            String sql = "delete  from sys_user_role where userid='" + userid + "'";
//            DataTable dt = DBUtil.SelectDataTable(sql);

//            if (roles != "")
//            {
//                String[] ids = roles.Split(',');
//                for (int i = 0, l = ids.Length; i < l; i++)
//                {
//                    string guid = Guid.NewGuid().ToString();
//                    string id = ids[i];

//                    sql = "insert into sys_user_role(id,userid,roleid) values('" + guid + "','" + userid + "','" + id + "')";
//                    DBUtil.Execute(sql);
//                }
//            }
//        }
    }
}