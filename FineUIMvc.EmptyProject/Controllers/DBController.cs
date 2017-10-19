using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Collections;
using System.Text.RegularExpressions;
using FineUIMvc.PumpMVC.Models;

namespace FineUIMvc.PumpMVC.Controllers
{
    public class DBController : DbContext
    {
        public DBController()
            : base("name=DBContext")
        {

        }
        public DbSet<A_U_DEP> A_U_DEP { get; set; }
        public DbSet<Menus> Menus { get; set; }
        public DbSet<sys_Menus> sys_Menus { get; set; }
        public DbSet<Power> powers { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<sys_dict> sys_dict { get; set; }
        public DbSet<sys_dictItems> sys_dictItems { get; set; }
        public DbSet<Panda_Customer> Panda_Customer { get; set; }             //客户
        public DbSet<Panda_Pump> Panda_Pump { get; set; }                     //泵房
        public DbSet<Panda_PGroup> Panda_PGroup { get; set; }                 //泵房组
        public DbSet<Panda_GroupPump> Panda_GroupPump { get; set; }           //组所含泵房
        public DbSet<Panda_UserInfo> Panda_UserInfo { get; set; }             //用户信息
        public DbSet<Panda_PumpJZ> Panda_PumpJZ { get; set; }                 //泵房机组
        public DbSet<BASE_SHUICHANG_JZ> BASE_SHUICHANG_JZ { get; set; }       //水厂机组
        public DbSet<Panda_PumpVQ> Panda_PumpVQ { get; set; }                 //视频设备
        public DbSet<Dtu_Base> Dtu_Base { get; set; }                         //DTU
        public DbSet<AddressScheme> AddressScheme { get; set; }               //地址表
        public DbSet<MessageSetting> MessageSetting { get; set; }             //联系人
        public DbSet<Panda_PumpDA> Panda_PumpDA { get; set; }
        public DbSet<Panda_CustomerTopMenu> Panda_CustomerTopMenu { get; set; }    //设备档案
        public DbSet<E_DATA_MAIN> E_DATA_MAIN { get; set; }   //泵房机组实时数据

        public DbSet<Panda_PumpFG> Panda_PumpFG { get; set; }                 //泵房分组
        public DbSet<Panda_PumpFG_P> Panda_PumpFG_P { get; set; }           //分组所含泵房
        public DbSet<BASE_LIULIANG> BASE_LIULIANG { get; set; }           //流量计
        public DbSet<BASE_FAMEN> BASE_FAMEN { get; set; }           //阀门
        public DbSet<BASE_SHUICHANG> BASE_SHUICHANG { get; set; }           //水厂
        public DbSet<BASE_JIAYAZHAN> BASE_JIAYAZHAN { get; set; } //加压站
        public DbSet<BASE_JIAYAZHAN_JZ> BASE_JIAYAZHAN_JZ { get; set; } //加压站机组
        public DbSet<BASE_YALI> BASE_YALI { get; set; }           //流量计
        public DbSet<BASE_TIAOFENG> BASE_TIAOFENG { get; set; }           //调峰
        public DbSet<BASE_YUZHIBENGZHAN> BASE_YUZHIBENGZHAN { get; set; }           //预置泵站
        public DbSet<LoginPage> LoginPage { get; set; }           //登录页
        public DbSet<WebPortal> WebPortal { get; set; }           //门户
        public DbSet<Alarm_Timely> Alarm_Timely { get; set; }

        public DbSet<LunXunParam> LunXunParam { get; set; }   //轮询设置

        public DbSet<Alarm_Contact_Group> Alarm_Contact_Group { get; set; }   //报警联系人组
        public DbSet<Alarm_Param> Alarm_Param { get; set; }   //报警参数
        public DbSet<Tocken> Tocken { get; set; }   //令牌
        public DbSet<HS_Data_ShuiChang_MAIN> HS_Data_ShuiChang_MAIN { get; set; }  //含山水厂
        public DbSet<FY_Data_ShuiChang_MAIN> FY_Data_ShuiChang_MAIN { get; set; }
        public DbSet<FY_Data_ShuiChang> FY_Data_ShuiChang { get; set; }//富源水厂
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Panda_Customer>()
                .HasMany(r => r.Panda_UserInfo)
                .WithOptional(u => u.Panda_Customer)
                .Map(x => x.MapKey("FCustomerID"));
            modelBuilder.Entity<Panda_Customer>()
                .HasMany(r => r.Panda_Pump)
                .WithOptional(u => u.Panda_Customer)
                .Map(x => x.MapKey("FCustomerID"));
            modelBuilder.Entity<Panda_PGroup>()
                .HasMany(r => r.Panda_UserInfo)
                .WithOptional(u => u.Panda_PGroup)
                .Map(x => x.MapKey("UserPumpGroup"));
            //modelBuilder.Entity<Dtu_Base>()
            //    .HasMany(r => r.Panda_PumpJZ)
            //    .WithOptional(u => u.Dtu_Base)
            //    .Map(x => x.MapKey("DTUCode"));
            modelBuilder.Entity<AddressScheme>()
                .HasMany(r => r.Panda_PumpJZ)
                .WithOptional(u => u.AddressScheme)
                .Map(x => x.MapKey("PumpJZAddressList"));
            modelBuilder.Entity<AddressScheme>()
              .HasMany(r => r.BASE_JIAYAZHAN_JZ)
              .WithOptional(u => u.AddressScheme)
              .Map(x => x.MapKey("jyzJZAddressList"));
            modelBuilder.Entity<AddressScheme>()
              .HasMany(r => r.BASE_SHUICHANG_JZ)
              .WithOptional(u => u.AddressScheme)
              .Map(x => x.MapKey("AddressList"));
            modelBuilder.Entity<AddressScheme>()
              .HasMany(r => r.BASE_TIAOFENG)
              .WithOptional(u => u.AddressScheme)
              .Map(x => x.MapKey("FSchemeID"));
            modelBuilder.Entity<AddressScheme>()
              .HasMany(r => r.BASE_LIULIANG)
              .WithOptional(u => u.AddressScheme)
              .Map(x => x.MapKey("FSchemeID"));
            modelBuilder.Entity<AddressScheme>()
              .HasMany(r => r.BASE_FAMEN)
              .WithOptional(u => u.AddressScheme)
              .Map(x => x.MapKey("FSchemeID"));
            modelBuilder.Entity<AddressScheme>()
              .HasMany(r => r.BASE_YALI)
              .WithOptional(u => u.AddressScheme)
              .Map(x => x.MapKey("FSchemeID"));
            modelBuilder.Entity<AddressScheme>()
             .HasMany(r => r.BASE_YUZHIBENGZHAN)
             .WithOptional(u => u.AddressScheme)
             .Map(x => x.MapKey("FSchemeID"));
            modelBuilder.Entity<Panda_Customer>()
                .HasMany(r => r.Role)
                .WithOptional(u => u.Panda_Customer)
                .Map(x => x.MapKey("FCustomerID"));
            modelBuilder.Entity<Panda_Customer>()
               .HasMany(r => r.BASE_LIULIANG)
               .WithOptional(u => u.Panda_Customer)
               .Map(x => x.MapKey("FCustomerID"));
            modelBuilder.Entity<Panda_Customer>()
              .HasMany(r => r.BASE_FAMEN)
              .WithOptional(u => u.Panda_Customer)
              .Map(x => x.MapKey("FCustomerID"));
            modelBuilder.Entity<Panda_Customer>()
               .HasMany(r => r.BASE_SHUICHANG)
               .WithOptional(u => u.Panda_Customer)
               .Map(x => x.MapKey("FCustomerID"));
            modelBuilder.Entity<Panda_Customer>()
               .HasMany(r => r.Alarm_Contact_Group)
               .WithOptional(u => u.Panda_Customer)
               .Map(x => x.MapKey("FCustomerID"));
            modelBuilder.Entity<Panda_Customer>()
              .HasMany(r => r.BASE_JIAYAZHAN)
              .WithOptional(u => u.Panda_Customer)
              .Map(x => x.MapKey("FCustomerID"));
            modelBuilder.Entity<Panda_Customer>()
             .HasMany(r => r.BASE_YALI)
             .WithOptional(u => u.Panda_Customer)
             .Map(x => x.MapKey("FCustomerID"));
            modelBuilder.Entity<Panda_Customer>()
             .HasMany(r => r.BASE_TIAOFENG)
             .WithOptional(u => u.Panda_Customer)
             .Map(x => x.MapKey("FCustomerID"));
            modelBuilder.Entity<Panda_Customer>()
           .HasMany(r => r.BASE_YUZHIBENGZHAN)
           .WithOptional(u => u.Panda_Customer)
           .Map(x => x.MapKey("FCustomerID"));
        }

        public DataTable SqlQueryForDataTatable(string sql, SqlParameter[] parameters = null)
        {
            DBController db = new DBController();
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(db.Database.Connection.ConnectionString))
            {
                connection.ConnectionString = db.Database.Connection.ConnectionString;
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                if (null != parameters)
                {
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                connection.Close();
            }
            return dt;
        }

        public Int64 ExecuteScalar(string sql)
        {
            int i = 0;
            string sqlSubstr = string.Empty;
            if (sql.Contains(" with "))
            {
                i = sql.IndexOf(")select");
                sqlSubstr = sql.Substring(0, i + 1);
                sql = sql.Substring(i + 1, sql.Length - i - 1);
            }
            i = sql.ToUpper().IndexOf("FROM ");
            string strSqlCount = sql.Substring(0, i + 5);
            strSqlCount = sqlSubstr + sql.Replace(strSqlCount, @"Select Count(*) as A From ");
            strSqlCount = System.Text.RegularExpressions.Regex.Replace(strSqlCount, @"order by.*", "");

            DataTable dt = SqlQueryForDataTatable(strSqlCount);

            Int64 count = Convert.ToInt64(dt.Rows[0]["A"].ToString());

            return count;
        }

        public ArrayList Select(string sql)
        {
            return Select(sql, null);
        }

        public ArrayList Select(string sql, Hashtable args)
        {
            DataTable data = new DataTable();
            DBController db = new DBController();
            using (SqlConnection connection = new SqlConnection(db.Database.Connection.ConnectionString))
            {

                SqlCommand cmd = new SqlCommand(sql, connection);
                if (args != null) SetArgs(sql, args, cmd);

                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                adapter.Fill(data);

            }

            return DataTableArrayList(data);
        }

        public static ArrayList DataTableArrayList(DataTable data)
        {
            ArrayList array = new ArrayList();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                DataRow row = data.Rows[i];

                Hashtable record = new Hashtable();
                for (int j = 0; j < data.Columns.Count; j++)
                {
                    object cellValue = row[j];
                    if (cellValue.GetType() == typeof(DBNull))
                    {
                        cellValue = null;
                    }
                    record[data.Columns[j].ColumnName] = cellValue;
                }
                array.Add(record);
            }
            return array;
        }

        public static void SetArgs(string sql, Hashtable args, IDbCommand cmd)
        {
            MatchCollection ms = Regex.Matches(sql, @"@\w+");
            foreach (Match m in ms)
            {
                string key = m.Value;

                Object value = args[key];
                if (value == null)
                {
                    value = args[key.Substring(1)];
                }
                if (value == null) value = DBNull.Value;

                cmd.Parameters.Add(new SqlParameter(key, value));
            }
            cmd.CommandText = sql;
        }

        public void Execute(string sql)
        {
            Execute(sql, null);
        }

        public void Execute(string sql, Hashtable args)
        {
            DBController db = new DBController();
            using (SqlConnection connection = new SqlConnection(db.Database.Connection.ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                if (args != null) SetArgs(sql, args, cmd);
                cmd.ExecuteNonQuery();
                connection.Close();
                connection.Dispose();
            }
        }
        public int ExecuteID(string sql, Hashtable args)
        {
            DBController db = new DBController();
            int add_id = 0;
            using (SqlConnection connection = new SqlConnection(db.Database.Connection.ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                if (args != null) SetArgs(sql, args, cmd);
                add_id = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Parameters.Clear();
                connection.Close();
            }
            return add_id;
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public static int ExecuteSqlTran(List<String> SQLStringList)
        {
            DBController db = new DBController();
            using (SqlConnection conn = new SqlConnection(db.Database.Connection.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    int count = 0;
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n];
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            count += cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    return count;
                }
                catch
                {
                    tx.Rollback();
                    return 0;
                }
            }
        }
    }
}