using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using FineUIMvc;

namespace FineUIMvc.PumpMVC.DAL
{
    public class AddressSchemeDal
    {
        private const string str_ASList = @"select * from AddressScheme where FIsDelete=0";

        private const string str_ASUpd = @"UPDATE AddressScheme SET {0} WHERE {1}";

        private const string str_ASAdd = @"INSERT INTO AddressScheme ( {0} ) VALUES( {1} );select @@identity";
        private const string str_ASEList = @"select * from [AddressSchemeEntry] where FSchemeID in (select top 1 ID from AddressScheme  where FIsDelete=0 and FIsBaseScheme=1 and FType={0}) ";
        public static Hashtable Search(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_ASList);
        }
        public static DataTable SearchALL(string strWhere,string FType)
        {
            return publicDal.TableSearch(strWhere, string.Format(str_ASEList, FType));
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