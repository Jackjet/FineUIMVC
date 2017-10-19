using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;


namespace FineUIMvc.PumpMVC.DAL
{
    public class Sys_OnlineDal
    {
        public static Hashtable Search(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            string where = strWhere;
            string sql = @" select  a.ID,a.IPAdddress,a.LoginTime,a.UpdateTime,a.UserID,b.UserName from sys_Onlines a 
                         left join Panda_UserInfo b on a.UserID=b.ID WHERE 1=1 " + where;

            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, sql);
        }

        public static Hashtable Get(string id)
        {
            string sql = "select * from sys_Onlines where UserId = '" + id + "'";
            return publicDal.has_Select(sql);
        }

        public static void Insert(Hashtable has)
        {
            publicDal.Insert(has, "insert into sys_Onlines ( {0} ) values( {1} )");
        }
        //public static void Delete(string id)
        //{
        //    Hashtable has = new Hashtable();
        //    has["id"] = id;
        //    DBUtil.Execute("delete from sys_Onlines where id = @id", has);
        //}
        public static void Update(Hashtable has)
        {
            publicDal.Update(has, "update sys_Onlines  set {0}  where {1}", "UserID");
        }

    }
}