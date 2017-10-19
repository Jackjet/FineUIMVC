using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using FineUIMvc;

namespace FineUIMvc.PumpMVC.DAL
{
    public class Panda_PumpDal
    {
        private const string str_PumpList = @"select a.ID,a.PCode,a.PName,a.PCustomPName,a.FCustomerID,b.Name as CustomerName,c.Fengongsi,d.FName+e.FName as ProvinceCity 
                                                from Panda_Pump a
                                           left join Panda_Customer b on a.FCustomerID=b.ID
                                           left join A_U_DEP c on a.PCompanyNumber=c.U8number
                                           left join sys_dictItems d on a.PProvince=d.FValue and d.FDictID=108
                                           left join sys_dictItems e on a.PCity=e.FValue and e.FDictID=120
                                               where a.FIsDelete=0 ";

        private const string str_PumpUpd = @"UPDATE Panda_Pump SET {0} WHERE {1}";

        private const string str_PumpAdd = @"INSERT INTO Panda_Pump ( {0} ) VALUES( {1} );select @@identity";

        public static Hashtable Search(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_PumpList);
        }

        public static void Insert(Hashtable has)
        {
           publicDal.Insert(has, str_PumpAdd);
        }

        public static void Update(Hashtable has)
        {
            publicDal.Update(has, str_PumpUpd, "ID");
        }
        public static void DeleteList(Hashtable has)
        {
            publicDal.DeleteList(has, str_PumpUpd, "ID");
        }
    }
}