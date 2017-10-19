using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using FineUIMvc;

namespace FineUIMvc.PumpMVC.DAL
{
    public class AlarmContactDal
    {
        private const string str_ASList = @"select * from Alarm_Contact_Group where FIsDelete=0 ";

        private const string str_ASUpd = @"UPDATE Alarm_Contact_Group SET {0} WHERE {1}";

        private const string str_ASAdd = @"INSERT INTO Alarm_Contact_Group ( {0} ) VALUES( {1} );select @@identity";
        private const string str_ASEList = @"select a.ID, a.FName,(dbo.fun_RemoveHtml
                                                              ((SELECT Contacts + ','
                                                                  FROM Alarm_Contact_Contact c where c.Type=1 and c.GroupID=a.ID FOR xml path))) AS Contacts,a.FNote from Alarm_Contact_Group a
                                              where a.FIsDelete=0 ";
        private const string str_ASEList_count = @"select count(*) as countS from Alarm_Contact_Group a
                                              where a.FIsDelete=0 ";
        public static Hashtable Search(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_ASList);
        }
        public static Hashtable SearchGroupCon(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_ASEList, str_ASEList_count);
        }
        public static int Insert(Hashtable has)
        {
            return publicDal.InsertGetID(has, str_ASAdd);
        }

        public static void Update(Hashtable has)
        {
            publicDal.Update(has, str_ASUpd, "ID");
        }
        public static void DeleteList(Hashtable has)
        {
            publicDal.DeleteList(has, str_ASUpd, "ID");
        }
    }
}