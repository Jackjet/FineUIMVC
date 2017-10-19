using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FineUIMvc.PumpMVC.DAL
{
    public class Panda_PumpVideoQuipmentDal
    {
        private const string str_PumpJZList = @" SELECT ID ,PumpId,c.FName,[Type] ,Brand ,Number,IP,Port,UserName ,[PassWord] ,Mark
                                                 FROM PumpVideoQuipment a left join sys_dictItems c 
                                                 on a.QuipmentType=c.FValue and c.FDictID=134 where a.IsActive=0 ";

        private const string str_PumpVQUpd = @"UPDATE PumpVideoQuipment SET {0} WHERE {1}";

        private const string str_PumpJZAdd = @"INSERT INTO PumpVideoQuipment ( {0} ) VALUES( {1} )";

        public static Hashtable Search(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_PumpJZList);
        }

        public static void Insert(Hashtable has)
        {
            publicDal.Insert(has, str_PumpJZAdd);
        }

        public static void Update(Hashtable has)
        {
            publicDal.Update(has, str_PumpVQUpd, "ID");
        }
        public static void DeleteList(Hashtable has)
        {
            publicDal.DeleteList(has, str_PumpVQUpd, "ID");
        }
    }
}