using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using FineUIMvc;

namespace FineUIMvc.PumpMVC.DAL
{
    public class Panda_PGroupDal
    {
        #region const
        private const string strCRM_PGroupList = @"SELECT GroupID,GroupName FROM Panda_PGroup where FIsDelete=0 ";

        private const string strCRM_PGroupAdd = @"INSERT INTO Panda_PGroup ( {0} ) VALUES( {1} )";

        private const string strCRM_PGroupUpd = @"UPDATE Panda_PGroup SET {0} WHERE {1}";

        private const string strCRM_PGroupDel = @"DELETE FROM Panda_PGroup WHERE ID = @ID";

        private const string strCRM_GroupPumpList = @"select a.ID,a.PumpID,a.GroupID,b.PName,b.PCustomPName,b.PCode from Panda_GroupPump a left join Panda_Pump b on a.PumpID=b.ID where 1=1 ";

        private const string strCRM_GroupPumpAdd = @"INSERT INTO Panda_GroupPump ( {0} ) VALUES( {1} )";

        private const string strCRM_GroupPumpDel = @"DELETE FROM Panda_GroupPump WHERE ID = @ID";

        private const string strCRM_GroupPumpDelList = @"DELETE FROM Panda_GroupPump ";
        #endregion

        public static DataTable SearchPGroup(string strWhere)
        {
            string where = strWhere;

            string sql = strCRM_PGroupList;

            if (!where.Equals(""))
            {
                sql = sql  + where;
            }
            sql = sql + " ORDER BY GroupName ";
            DataTable dt = publicDal.TableSearch(sql);

            return dt;
        }

        public static Hashtable SearchPGroup(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, strCRM_PGroupList);
        }

        public static Hashtable SearchGroupPump(int index, int size, string sortField, string sortOrder, string strWhere)
        {
            return publicDal.HashSearch(index, size, sortField, sortOrder, strWhere, strCRM_GroupPumpList);
        }
        public static DataTable SearchGroupPump(string strWhere)
        {
            string where = strWhere;
            string sql = strCRM_GroupPumpList;
            if (!where.Equals(""))
            {
                sql = sql + where;
            }

            return publicDal.TableSearch(sql);
        }

        public static void Update(Hashtable has)
        {
            publicDal.Update(has, strCRM_PGroupUpd, "GroupID");
        }

    }
}