using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using FineUIMvc;

namespace FineUIMvc.PumpMVC.DAL
{
    public class Panda_PumpFGDal
    {
        #region const
        private const string strCRM_PumpFGList = @"SELECT a.ID,G_Name,TypeName=case when a.G_Type=0 then '管理员分组'when a.G_Type=1 then '分公司分组'when a.G_Type=2 then '客户分组' end,
                                                            e.Name as FCustomerName, c.Fengongsi
                                                      FROM Panda_PumpFG a
                                                 left join A_U_DEP c on a.FCompanyNumber=c.U8number
                                                 left join Panda_Customer e on a.FCustomerID=e.ID
                                                     where a.FIsDelete=0 ";

        private const string strCRM_PumpFGAdd = @"INSERT INTO Panda_PumpFG ( {0} ) VALUES( {1} )";

        private const string strCRM_PumpFGUpd = @"UPDATE Panda_PumpFG SET {0} WHERE {1}";

        private const string strCRM_PumpFGDel = @"DELETE FROM Panda_PumpFG WHERE ID = @ID";

        private const string strCRM_GroupPumpList = @"select a.ID,a.GroupID,b.PName,b.PCustomPName,b.PCode from Panda_PumpFG_P a left join Panda_Pump b on a.PumpID=b.ID where 1=1 ";

        private const string strCRM_GroupPumpAdd = @"INSERT INTO Panda_PumpFG_P ( {0} ) VALUES( {1} )";

        private const string strCRM_GroupPumpDel = @"DELETE FROM Panda_PumpFG_P WHERE ID = @ID";

        private const string strCRM_GroupPumpDelList = @"DELETE FROM Panda_PumpFG_P ";
        #endregion

        public static DataTable SearchPumpFG(string strWhere)
        {
            string where = strWhere;

            string sql = strCRM_PumpFGList;

            if (!where.Equals(""))
            {
                sql = sql + where;
            }
            sql = sql + " ORDER BY G_Name ";
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }

        public static Hashtable SearchPumpFG(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, strCRM_PumpFGList);
        }

        public static Hashtable SearchGroupPump(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, strCRM_GroupPumpList);
        }

        public static void Insert(Hashtable has)
        {
            publicDal.Insert(has, strCRM_PumpFGAdd);
        }

        public static void Update(Hashtable has)
        {
            publicDal.Update(has, strCRM_PumpFGUpd, "ID");
        }

        public static void DeleteGroupList(Hashtable has)
        {
            publicDal.DeleteList(has, strCRM_PumpFGUpd, "ID");
        }
        public static void DeleteGroupPumpList(string ids)
        {
            Hashtable has = new Hashtable();

            has["ID"] = ids;
            publicDal.DeleteList(has, "delete from  Panda_PumpFG_P where ID in ({0})");
        }
    }
}