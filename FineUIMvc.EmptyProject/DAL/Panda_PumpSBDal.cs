using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using FineUIMvc;

namespace FineUIMvc.PumpMVC.DAL
{
    public class Panda_PumpSBDal
    {
        private const string str_PumpSBList = @" ";

        private const string str_PumpSBUpd = @"UPDATE Panda_PumpSB SET {0} WHERE {1}";
        private const string str_PumpSBDel = @"UPDATE Panda_PumpSB SET {0} WHERE {1}";

        private const string str_PumpSBAdd = @"INSERT INTO Panda_PumpSB ( {0} ) VALUES( {1} )";

        public static Hashtable Search(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_PumpSBList);
        }

        public static void Insert(Hashtable has, string id)
        {
            //publicDal.InsertUpd(has, str_PumpSBAdd, id);
        }

        public static void Update(Hashtable has, int id)
        {
            publicDal.Update(has, str_PumpSBUpd, "ID");
        }
        public static void DeleteList(Hashtable has)
        {
            publicDal.DeleteList(has, str_PumpSBDel, "ID");
        }
    }
}