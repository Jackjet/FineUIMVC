using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using FineUIMvc;

namespace FineUIMvc.PumpMVC.DAL
{
    public class MessageSettingDal
    {
        private const string str_MSList = @"select a.*,c.Name as FCustomerName from Alarm_Contact_Contact a
                                         left join Alarm_Contact_Group b on a.GroupID=b.ID
                                         left join Panda_Customer c on b.FCustomerID=c.ID
                                             where a.FIsDelete=0 ";

        private const string str_MSUpd = @"UPDATE Alarm_Contact_Contact SET {0} WHERE {1}";

        private const string str_MSAdd = @"INSERT INTO Alarm_Contact_Contact ( {0} ) VALUES( {1} )";

        public static Hashtable Search(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_MSList);
        }

        public static void Insert(Hashtable has)
        {
            publicDal.Insert(has, str_MSAdd);
        }

        public static void Update(Hashtable has)
        {
            publicDal.Update(has, str_MSUpd, "ID");
        }
        public static void DeleteList(Hashtable has)
        {
            publicDal.DeleteList(has, str_MSUpd, "ID");
        }
    }
}