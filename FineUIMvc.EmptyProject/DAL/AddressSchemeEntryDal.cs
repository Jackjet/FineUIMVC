using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FineUIMvc.PumpMVC.DAL
{
    public class AddressSchemeEntryDal
    {
        private const string str_ASList = @"select * from AddressSchemeEntry where 1=1";

        private const string str_ASUpd = @"UPDATE AddressSchemeEntry SET {0} WHERE {1}";

        private const string str_ASAdd = @"INSERT INTO AddressSchemeEntry ( {0} ) VALUES( {1} )";

        public static Hashtable Search(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_ASList);
        }

      
        public static void Insert(Hashtable has)
        {
            publicDal.Insert(has, str_ASAdd);
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