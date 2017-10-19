using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using FineUIMvc;

namespace FineUIMvc.PumpMVC.DAL
{
    public class Panda_PumpDADal
    {
        private const string str_PumpDAList = @" select a.*,b.FName as TypeName,status='已完成','<img class=pumpimg width=20px height=20px src='+FilePath+'/>' as img
                                                   from BaseDA a
                                              left join sys_dictItems b on a.FileType2=b.FValue and b.FDictID=135 
                                                  where 1=1 ";

        private const string str_PumpDADelete = @"delete from BaseDA";

        public static DataTable Search(string strWhere)
        {
            return publicDal.TableSearch(str_PumpDAList + strWhere + " order by UpDateTime desc");
        }

        public static void Delete(string strWhere)
        {
            string sql = string.Empty;
            sql = str_PumpDADelete;
            if(!strWhere.Equals(""))
            {
                sql = sql + " where " + strWhere;
            }
            publicDal.Delete(sql);
        }
    }
}