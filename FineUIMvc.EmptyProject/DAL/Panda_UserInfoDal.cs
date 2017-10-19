using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using FineUIMvc;

namespace FineUIMvc.PumpMVC.DAL
{
    public class Panda_UserInfoDal
    {
        private const string str_UserInfoList = @"select a.ID,b.Name as FCompanyName,UserName,UserTel,UserMail,c.FName as EnableName,d.FName as UserTypeName,a.FCreateDate,
                                                         FCustomerName=case when a.FCustomerID>0 then e.Name when a.FCompanyNumber <>'' and a.FCompanyNumber is not null then b.Name end
                                                         from Panda_UserInfo	a
                                               left join Depts b on a.FCompanyNumber=b.Number
                                               left join sys_dictItems c on a.UserEnabledisable=c.FValue and c.FDictID=54
                                               left join sys_dictItems d on a.UserType=d.FValue and d.FDictID=53
                                               left join Panda_Customer e on a.FCustomerID=e.ID 
                                                   where a.FIsDelete=0 ";

        private const string strGetSelRole_User = @"select a.*,c.Name as FCompanyName,UserName,UserTel,UserMail,d.FName as EnableName,e.FName as UserTypeName,b.FCreateDate,
                                                              FCustomerName=case when b.FCustomerID>0 then f.Name when b.FCompanyNumber <>'' and b.FCompanyNumber is not null then c.Name end
                                                              from sys_RoleUsers a
                                                    left join Panda_UserInfo b on a.UserID=b.ID
                                                    left join Depts c on b.FCompanyNumber=c.Number
                                                    left join sys_dictItems d on b.UserEnabledisable=d.FValue and d.FDictID=54
                                                    left join sys_dictItems e on b.UserType=e.FValue and e.FDictID=53
                                                    left join Panda_Customer f on b.FCustomerID=f.ID 
                                                        where b.FIsDelete=0 ";

        private const string str_UserInfoUpd = @"UPDATE Panda_UserInfo SET {0} WHERE {1}";

        private const string str_UserInfoAdd = @"INSERT INTO Panda_UserInfo ( {0} ) VALUES( {1} )";

        public static Hashtable Search(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_UserInfoList);
        }

        public static DataTable Exist(string strWhere)
        {
            string where = strWhere;

            string sql = @"select top 1 * from Panda_UserInfo where FIsDelete=0 ";

            if (!where.Equals(""))
            {
                sql = sql + where;
            }
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }

        //public bool IsCustomer()
        //{

        //}

        public static Hashtable SearchRoleUsers(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, strGetSelRole_User);
        }

        public static Hashtable Get(string id)
        {
            string sql = @"select a.*,b.Name as CustomerName,b.FMapTempID from Panda_UserInfo a
            left join Panda_Customer b on a.FCustomerID=b.ID where a.ID = '" + id + "'";
            return publicDal.has_Select(sql);
        }

        public static void Insert(Hashtable has)
        {
            publicDal.Insert(has, str_UserInfoAdd);
        }

        public static void Update(Hashtable has)
        {
            publicDal.Update(has, str_UserInfoUpd, "ID");
        }
        public static void DeleteList(Hashtable has)
        {
            publicDal.DeleteList(has, str_UserInfoUpd, "ID");
        }

        public static Hashtable GetLogin(string username, string password)
        {
            string sql = @"select top 1 ID,UserName UserName,UserPwd UserPassword, UserEnabledisable 
                           from Panda_UserInfo WHERE UserName = '" + username + "'";
            return publicDal.has_Select(sql);
        }

        public static ArrayList GetRole(string UserID)
        {
            string sql = "select RoleID from sys_RoleUsers where UserID = '" + UserID + "'";
            return publicDal.arr_Select(sql);
        }
    }
}