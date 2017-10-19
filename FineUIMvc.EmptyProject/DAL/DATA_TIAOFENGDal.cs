using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using FineUIMvc;

namespace FineUIMvc.PumpMVC.DAL
{
    public class DATA_TIAOFENGDal
    {
        private const string str_DATAMAINUpd = @"UPDATE DATA_TIAOFENG_MAIN SET {0} WHERE {1}";
        private const string str_DATAMAINAdd = @"INSERT INTO DATA_TIAOFENG_MAIN ( {0} ) VALUES( {1} )";
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