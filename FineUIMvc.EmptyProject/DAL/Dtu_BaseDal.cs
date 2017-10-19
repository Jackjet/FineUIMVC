using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using FineUIMvc;

namespace FineUIMvc.PumpMVC.DAL
{
    public class Dtu_BaseDal
    {
        private const string str_Dtu_BaseList = @"select a.*,b.FName as ModeName,c.FName as	IsUsedName
                                                    from Dtu_Base a
                                               left join sys_dictItems b on a.B_Mode=b.FValue and b.FDictID=130
                                               left join sys_dictItems c on a.B_IsUsed=c.FValue and c.FDictID=131
                                                   where 1=1 ";

        private const string str_Dtu_BaseUpd = @"UPDATE Dtu_Base SET {0} WHERE {1}";

        private const string str_Dtu_BaseAdd = @"INSERT INTO Dtu_Base ( {0} ) VALUES( {1} )";

        public static Hashtable Search(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, str_Dtu_BaseList);
        }

        public static void Insert(Hashtable has)
        {
            publicDal.Insert(has, str_Dtu_BaseAdd);
        }

        public static void Update(Hashtable has)
        {
            publicDal.Update(has, str_Dtu_BaseUpd, "B_ID");
        }
        public static void DeleteList(Hashtable has)
        {
            publicDal.DeleteList(has, str_Dtu_BaseUpd, "B_ID");
        }
    }
}