using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FineUIMvc.PumpMVC.DAL
{
    public class TockenDal
    {
        private const string str_DATAMAINUpd = @"UPDATE Tocken SET {0} WHERE {1}";
        private const string str_DATAMAINAdd = @"INSERT INTO Tocken ( {0} ) VALUES( {1} )";
        public static void Insert(Hashtable has)
        {
            publicDal.Insert(has, str_DATAMAINAdd);
        }

        public static void Update(Hashtable has)
        {
            publicDal.Update(has, str_DATAMAINUpd, "BASEID");
        }
    }
}