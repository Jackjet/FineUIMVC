using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Data.OleDb;
using System.Collections;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using System.Data.Common;

namespace Dal
{
    public class DBUtil
    {

        //SqlServer
        static String dbType = "SqlServer";
        public static String connectionString = ConfigurationSettings.AppSettings["connectionStr"];

        //Oracle
        //static String dbType = "Oracle";
        //public static String connectionString = "Provider=OraOLEDB.Oracle.1;Data Source=XE;User Id=plus;Password=sa";

        //MySql
        //static String dbType = "MySql";
        //public static String connectionString = "server=localhost; user id=root; password=; database=plusoft_test;";

        public static DbConnection getConn()
        {
            DbConnection conn = null;
            if (conn == null)
            {
                //if (dbType == "MySql")
                //{
                //    conn = new MySqlConnection(connectionString);
                //}
                //else if (dbType == "Oracle")
                //{
                //    conn = new OleDbConnection(connectionString);
                //}
                //else if (dbType == "SqlServer")
                //{
                conn = new SqlConnection(connectionString);
                //}
                conn.Open();
            }
            return conn;
        }
        public static void BeginConn()
        {
            getConn();
        }
        public static void EndConn(DbConnection conn)
        {
            if (conn != null)
            {
                conn.Close();
                conn = null;
            }
        }

        public static DataTable SelectDataTable(string sql)
        {
            return SelectDataTable(sql, null);
        }
        public static DataTable SelectDataTable(string sql, Hashtable args)
        {
            DataTable data = new DataTable();

            DbConnection con = getConn();

            //if (dbType == "MySql")
            //{
            //    MySqlCommand cmd = new MySqlCommand(sql, (MySqlConnection)con);
            //    if (args != null) SetArgs(sql, args, cmd);

            //    MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connectionString);
            //    adapter.Fill(data);
            //}
            //else if (dbType == "Oracle")
            //{
            //    OleDbCommand cmd = new OleDbCommand(sql, (OleDbConnection)con);
            //    if (args != null) SetArgs(sql, args, cmd);

            //    OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connectionString);
            //    adapter.Fill(data);
            //}
            //else if (dbType == "SqlServer")
            //{
            SqlCommand cmd = new SqlCommand(sql, (SqlConnection)con);
            if (args != null) SetArgs(sql, args, cmd);

            SqlDataAdapter adapter = new SqlDataAdapter(sql, connectionString);
            adapter.Fill(data);
            //}

            EndConn(con);

            return data;
        }
        public static DataTable SelectDataTablePager(string sql, int index, int size)
        {
            DataTable data = new DataTable();

            DbConnection con = getConn();

            //if (dbType == "MySql")
            //{
            //    MySqlCommand cmd = new MySqlCommand(sql, (MySqlConnection)con);


            //    MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connectionString);
            //    adapter.Fill(data);
            //}
            //else if (dbType == "Oracle")
            //{
            //    OleDbCommand cmd = new OleDbCommand(sql, (OleDbConnection)con);


            //    OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connectionString);
            //    adapter.Fill(data);
            //}
            //else if (dbType == "SqlServer")
            //{
            SqlCommand cmd = new SqlCommand(sql, (SqlConnection)con);
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connectionString);

            //分页
            DataSet dst = new DataSet();
            int StartRecord = 0;
            //if (index > 1)
            //{
            //    StartRecord = (index - 1) * size;
            //}

            StartRecord = index * size;

            adapter.Fill(dst, StartRecord, size, "temp");
            data = dst.Tables[0];
            //}
            EndConn(con);

            return data;
        }
        public static int ExecuteScalar(string sql)
        {
            int i = sql.ToUpper().IndexOf("FROM ");
            string strSqlCount = sql.Substring(0, i + 5);
            strSqlCount = sql.Replace(strSqlCount, @"Select Count(*) as A From ");
            strSqlCount = System.Text.RegularExpressions.Regex.Replace(strSqlCount, @"order by.*", "");

            DbConnection con = getConn();
            int count = 0;
            SqlCommand cmd = new SqlCommand(strSqlCount, (SqlConnection)con);
            count = Convert.ToInt16(cmd.ExecuteScalar());
            return count;
        }


        public static Int64 ExecuteScalar64(string sql)
        {
            int i = sql.ToUpper().IndexOf("FROM ");
            string strSqlCount = sql.Substring(0, i + 5);
            strSqlCount = sql.Replace(strSqlCount, @"Select Count(*) as A From ");
            strSqlCount = System.Text.RegularExpressions.Regex.Replace(strSqlCount, @"order by.*", "");

            DbConnection con = getConn();
            Int64 count = 0;
            SqlCommand cmd = new SqlCommand(strSqlCount, (SqlConnection)con);
            count = Convert.ToInt64(cmd.ExecuteScalar());
            return count;
        }


        public static ArrayList Select(string sql)
        {
            return Select(sql, null);
        }
        public static ArrayList Select(string sql, Hashtable args)
        {
            DataTable data = new DataTable();

            DbConnection con = getConn();

            if (dbType == "MySql")
            {
                MySqlCommand cmd = new MySqlCommand(sql, (MySqlConnection)con);
                if (args != null) SetArgs(sql, args, cmd);

                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connectionString);
                adapter.Fill(data);
            }
            else if (dbType == "Oracle")
            {
                OleDbCommand cmd = new OleDbCommand(sql, (OleDbConnection)con);
                if (args != null) SetArgs(sql, args, cmd);

                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connectionString);
                adapter.Fill(data);
            }
            else if (dbType == "SqlServer")
            {
                SqlCommand cmd = new SqlCommand(sql, (SqlConnection)con);
                if (args != null) SetArgs(sql, args, cmd);

                SqlDataAdapter adapter = new SqlDataAdapter(sql, connectionString);
                adapter.Fill(data);
            }

            EndConn(con);

            return DataTable2ArrayList(data);
        }
        public static void Execute(string sql)
        {
            Execute(sql, null);
        }
        public static void Execute(string sql, Hashtable args)
        {
            DbConnection con = getConn();

            if (dbType == "MySql")
            {
                MySqlCommand cmd = new MySqlCommand(sql, (MySqlConnection)con);
                if (args != null) SetArgs(sql, args, cmd);
                cmd.ExecuteNonQuery();
            }
            else if (dbType == "Oracle")
            {
                OleDbCommand cmd = new OleDbCommand(sql, (OleDbConnection)con);
                if (args != null) SetArgs(sql, args, cmd);
                cmd.ExecuteNonQuery();
            }
            else if (dbType == "SqlServer")
            {
                SqlCommand cmd = new SqlCommand(sql, (SqlConnection)con);
                if (args != null) SetArgs(sql, args, cmd);
                cmd.ExecuteNonQuery();
            }

            EndConn(con);
        }



        public static void SetArgs(string sql, Hashtable args, IDbCommand cmd)
        {
            if (dbType == "MySql")
            {
                MatchCollection ms = Regex.Matches(sql, @"@\w+");
                foreach (Match m in ms)
                {
                    string key = m.Value;
                    string newKey = "?" + key.Substring(1);
                    sql = sql.Replace(key, newKey);

                    Object value = args[key];
                    if (value == null)
                    {
                        value = args[key.Substring(1)];
                    }

                    cmd.Parameters.Add(new MySqlParameter(newKey, value));
                }
                cmd.CommandText = sql;
            }
            else if (dbType == "Oracle")
            {
                MatchCollection ms = Regex.Matches(sql, @"@\w+");
                int i = 1;
                foreach (Match m in ms)
                {
                    string key = m.Value;
                    string newKey = "@P" + i++;
                    sql = sql.Replace(key, "?");

                    Object value = args[key];
                    if (value == null)
                    {
                        value = args[key.Substring(1)];
                    }

                    cmd.Parameters.Add(new OleDbParameter(newKey, value));
                }
                cmd.CommandText = sql;
            }
            else if (dbType == "SqlServer")
            {
                MatchCollection ms = Regex.Matches(sql, @"@\w+");
                int i = 1;
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
        }
        public static ArrayList DataTable2ArrayList(DataTable data)
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

        /// <summary> 
        /// 将DataRow类型的列值分配到SqlParameter参数数组. 
        /// </summary> 
        /// <param name="commandParameters">要分配值的SqlParameter参数数组</param> 
        /// <param name="dataRow">将要分配给存储过程参数的DataRow</param> 
        private static void AssignParameterValues(SqlParameter[] commandParameters, DataRow dataRow)
        {
            if ((commandParameters == null) || (dataRow == null))
            {
                return;
            }

            int i = 0;
            // 设置参数值 
            foreach (SqlParameter commandParameter in commandParameters)
            {
                // 创建参数名称,如果不存在,只抛出一个异常. 
                if (commandParameter.ParameterName == null ||
                    commandParameter.ParameterName.Length <= 1)
                    throw new Exception(
                        string.Format("请提供参数{0}一个有效的名称{1}.", i, commandParameter.ParameterName));
                // 从dataRow的表中获取为参数数组中数组名称的列的索引. 
                // 如果存在和参数名称相同的列,则将列值赋给当前名称的参数. 
                if (dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) != -1)
                    commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];
                i++;
            }
        }

        /// <summary> 
        /// 将一个对象数组分配给SqlParameter参数数组. 
        /// </summary> 
        /// <param name="commandParameters">要分配值的SqlParameter参数数组</param> 
        /// <param name="parameterValues">将要分配给存储过程参数的对象数组</param> 
        private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                return;
            }

            // 确保对象数组个数与参数个数匹配,如果不匹配,抛出一个异常. 
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("参数值个数与参数不匹配.");
            }

            // 给参数赋值 
            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                // If the current array value derives from IDbDataParameter, then assign its Value property 
                if (parameterValues[i] is IDbDataParameter)
                {
                    IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];
                    if (paramInstance.Value == null)
                    {
                        commandParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[i].Value = paramInstance.Value;
                    }
                }
                else if (parameterValues[i] == null)
                {
                    commandParameters[i].Value = DBNull.Value;
                }
                else
                {
                    commandParameters[i].Value = parameterValues[i];
                }
            }
        }

        /// <summary> 
        /// 预处理用户提供的命令,数据库连接/事务/命令类型/参数 
        /// </summary> 
        /// <param name="command">要处理的SqlCommand</param> 
        /// <param name="connection">数据库连接</param> 
        /// <param name="transaction">一个有效的事务或者是null值</param> 
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param> 
        /// <param name="commandText">存储过程名或都T-SQL命令文本</param> 
        /// <param name="commandParameters">和命令相关联的SqlParameter参数数组,如果没有参数为'null'</param> 
        /// <param name="mustCloseConnection"><c>true</c> 如果连接是打开的,则为true,其它情况下为false.</param> 
        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, out bool mustCloseConnection)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            // If the provided connection is not open, we will open it 
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }

            // 给命令分配一个数据库连接. 
            command.Connection = connection;

            // 设置命令文本(存储过程名或SQL语句) 
            command.CommandText = commandText;

            // 分配事务 
            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = transaction;
            }

            // 设置命令类型. 
            command.CommandType = commandType;

            // 分配命令参数 
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }
        /// <summary> 
        /// 将SqlParameter参数数组(参数值)分配给SqlCommand命令. 
        /// 这个方法将给任何一个参数分配DBNull.Value; 
        /// 该操作将阻止默认值的使用. 
        /// </summary> 
        /// <param name="command">命令名</param> 
        /// <param name="commandParameters">SqlParameters数组</param> 
        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (SqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // 检查未分配值的输出参数,将其分配以DBNull.Value. 
                        if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        #region ExecuteReader 数据阅读器

        /// <summary> 
        /// 枚举,标识数据库连接是由SqlHelper提供还是由调用者提供 
        /// </summary> 
        private enum SqlConnectionOwnership
        {
            /// <summary>由SqlHelper提供连接</summary> 
            Internal,
            /// <summary>由调用者提供连接</summary> 
            External
        }

        /// <summary> 
        /// 执行指定数据库连接对象的数据阅读器. 
        /// </summary> 
        /// <remarks> 
        /// 如果是SqlHelper打开连接,当连接关闭DataReader也将关闭. 
        /// 如果是调用都打开连接,DataReader由调用都管理. 
        /// </remarks> 
        /// <param name="connection">一个有效的数据库连接对象</param> 
        /// <param name="transaction">一个有效的事务,或者为 'null'</param> 
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="commandText">存储过程名或T-SQL语句</param> 
        /// <param name="commandParameters">SqlParameters参数数组,如果没有参数则为'null'</param> 
        /// <param name="connectionOwnership">标识数据库连接对象是由调用者提供还是由SqlHelper提供</param> 
        /// <returns>返回包含结果集的SqlDataReader</returns> 
        private static SqlDataReader ExecuteReader(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlConnectionOwnership connectionOwnership)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            bool mustCloseConnection = false;
            // 创建命令 
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 30;
            try
            {
                PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

                // 创建数据阅读器 
                SqlDataReader dataReader;

                if (connectionOwnership == SqlConnectionOwnership.External)
                {
                    dataReader = cmd.ExecuteReader();
                }
                else
                {
                    dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }

                // 清除参数,以便再次使用.. 
                // HACK: There is a problem here, the output parameter values are fletched 
                // when the reader is closed, so if the parameters are detached from the command 
                // then the SqlReader can磘 set its values. 
                // When this happen, the parameters can磘 be used again in other command. 
                bool canClear = true;
                foreach (SqlParameter commandParameter in cmd.Parameters)
                {
                    if (commandParameter.Direction != ParameterDirection.Input)
                        canClear = false;
                }

                if (canClear)
                {
                    cmd.Parameters.Clear();
                }

                return dataReader;
            }
            catch
            {
                if (mustCloseConnection)
                    connection.Close();
                throw;
            }
        }

        /// <summary> 
        /// 执行指定数据库连接字符串的数据阅读器. 
        /// </summary> 
        /// <remarks> 
        /// 示例:  
        ///  SqlDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders"); 
        /// </remarks> 
        /// <param name="connectionString">一个有效的数据库连接字符串</param> 
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="commandText">存储过程名或T-SQL语句</param> 
        /// <returns>返回包含结果集的SqlDataReader</returns> 
        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteReader(connectionString, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary> 
        /// 执行指定数据库连接字符串的数据阅读器,指定参数. 
        /// </summary> 
        /// <remarks> 
        /// 示例:  
        ///  SqlDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24)); 
        /// </remarks> 
        /// <param name="connectionString">一个有效的数据库连接字符串</param> 
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="commandText">存储过程名或T-SQL语句</param> 
        /// <param name="commandParameters">SqlParamter参数数组(new SqlParameter("@prodid", 24))</param> 
        /// <returns>返回包含结果集的SqlDataReader</returns> 
        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                return ExecuteReader(connection, null, commandType, commandText, commandParameters, SqlConnectionOwnership.Internal);
            }
            catch
            {
                // If we fail to return the SqlDatReader, we need to close the connection ourselves 
                if (connection != null) connection.Close();
                throw;
            }

        }

        /// <summary> 
        /// 执行指定数据库连接字符串的数据阅读器,指定参数值. 
        /// </summary> 
        /// <remarks> 
        /// 此方法不提供访问存储过程输出参数和返回值参数. 
        /// 示例:  
        ///  SqlDataReader dr = ExecuteReader(connString, "GetOrders", 24, 36); 
        /// </remarks> 
        /// <param name="connectionString">一个有效的数据库连接字符串</param> 
        /// <param name="spName">存储过程名</param> 
        /// <param name="parameterValues">分配给存储过程输入参数的对象数组</param> 
        /// <returns>返回包含结果集的SqlDataReader</returns> 
        public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary> 
        /// 执行指定数据库连接对象的数据阅读器. 
        /// </summary> 
        /// <remarks> 
        /// 示例:  
        ///  SqlDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders"); 
        /// </remarks> 
        /// <param name="connection">一个有效的数据库连接对象</param> 
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="commandText">存储过程名或T-SQL语句</param> 
        /// <returns>返回包含结果集的SqlDataReader</returns> 
        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteReader(connection, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary> 
        /// [调用者方式]执行指定数据库连接对象的数据阅读器,指定参数. 
        /// </summary> 
        /// <remarks> 
        /// 示例:  
        ///  SqlDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24)); 
        /// </remarks> 
        /// <param name="connection">一个有效的数据库连接对象</param> 
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="commandText">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="commandParameters">SqlParamter参数数组</param> 
        /// <returns>返回包含结果集的SqlDataReader</returns> 
        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            return ExecuteReader(connection, (SqlTransaction)null, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
        }

        /// <summary> 
        /// [调用者方式]执行指定数据库连接对象的数据阅读器,指定参数值. 
        /// </summary> 
        /// <remarks> 
        /// 此方法不提供访问存储过程输出参数和返回值参数. 
        /// 示例:  
        ///  SqlDataReader dr = ExecuteReader(conn, "GetOrders", 24, 36); 
        /// </remarks> 
        /// <param name="connection">一个有效的数据库连接对象</param> 
        /// <param name="spName">T存储过程名</param> 
        /// <param name="parameterValues">分配给存储过程输入参数的对象数组</param> 
        /// <returns>返回包含结果集的SqlDataReader</returns> 
        public static SqlDataReader ExecuteReader(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteReader(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary> 
        /// [调用者方式]执行指定数据库事务的数据阅读器,指定参数值. 
        /// </summary> 
        /// <remarks> 
        /// 示例:  
        ///  SqlDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders"); 
        /// </remarks> 
        /// <param name="transaction">一个有效的连接事务</param> 
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="commandText">存储过程名称或T-SQL语句</param> 
        /// <returns>返回包含结果集的SqlDataReader</returns> 
        public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteReader(transaction, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary> 
        /// [调用者方式]执行指定数据库事务的数据阅读器,指定参数. 
        /// </summary> 
        /// <remarks> 
        /// 示例:  
        ///   SqlDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24)); 
        /// </remarks> 
        /// <param name="transaction">一个有效的连接事务</param> 
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="commandText">存储过程名称或T-SQL语句</param> 
        /// <param name="commandParameters">分配给命令的SqlParamter参数数组</param> 
        /// <returns>返回包含结果集的SqlDataReader</returns> 
        public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
        }

        #endregion ExecuteReader数据阅读器


        #region MakeInParam 参数列表
        /// <summary>

        /// </summary>
        /// <param name="ParamName">参数名</param>
        /// <param name="DbType">参数类型</param>
        /// <param name="Size">大小</param>
        /// <param name="Value">值</param>
        /// <returns></returns>

        public static SqlParameter MakeInParam(string ParamName, SqlDbType DbType, int Size, object Value)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }
        public static SqlParameter MakeParam(string ParamName, SqlDbType DbType, Int32 Size, ParameterDirection Direction, object Value)
        {
            SqlParameter param;
            if (Size > 0)
                param = new SqlParameter(ParamName, DbType, Size);
            else
                param = new SqlParameter(ParamName, DbType);
            param.Direction = Direction;
            if (!(Direction == ParameterDirection.Output && Value == null))
                param.Value = Value;
            return param;
        }

        #endregion
        #region MakeOutParam 参数列表
        /// <summary> 
        /// 创建输出参数 
        /// </summary> 
        /// <param name="ParamName">参数名</param> 
        /// <param name="DbType">参数类型</param> 
        /// <param name="Size">参数大小</param> 
        /// <returns>新参数对象</returns> 
        public static SqlParameter MakeOutParam(string ParamName, SqlDbType DbType, int Size)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Output, null);
        }

        #endregion

        public static string runProcedure(string ProName, SqlParameter[] ParamList, out string Msg)
        {
            var ParamListLenght = ParamList.Length;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataReader dr = null;

            try
            {
                dr = ExecuteReader(connection, CommandType.StoredProcedure, ProName, ParamList);

                while (dr.Read())
                {

                }
                dr.Close();
                dr.Dispose();
                Msg = ParamList[ParamListLenght - 1].Value.ToString();
                return "";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

        }
    }

    /// <summary> 
    /// SqlHelperParameterCache提供缓存存储过程参数,并能够在运行时从存储过程中探索参数. 
    /// </summary> 
    public sealed class SqlHelperParameterCache
    {
        #region 私有方法,字段,构造函数
        // 私有构造函数,妨止类被实例化. 
        private SqlHelperParameterCache() { }

        // 这个方法要注意 
        private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

        /// <summary> 
        /// 探索运行时的存储过程,返回SqlParameter参数数组. 
        /// 初始化参数值为 DBNull.Value. 
        /// </summary> 
        /// <param name="connection">一个有效的数据库连接</param> 
        /// <param name="spName">存储过程名称</param> 
        /// <param name="includeReturnValueParameter">是否包含返回值参数</param> 
        /// <returns>返回SqlParameter参数数组</returns> 
        private static SqlParameter[] DiscoverSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            SqlCommand cmd = new SqlCommand(spName, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            connection.Open();
            // 检索cmd指定的存储过程的参数信息,并填充到cmd的Parameters参数集中. 
            SqlCommandBuilder.DeriveParameters(cmd);
            connection.Close();
            // 如果不包含返回值参数,将参数集中的每一个参数删除. 
            if (!includeReturnValueParameter)
            {
                cmd.Parameters.RemoveAt(0);
            }

            // 创建参数数组 
            SqlParameter[] discoveredParameters = new SqlParameter[cmd.Parameters.Count];
            // 将cmd的Parameters参数集复制到discoveredParameters数组. 
            cmd.Parameters.CopyTo(discoveredParameters, 0);

            // 初始化参数值为 DBNull.Value. 
            foreach (SqlParameter discoveredParameter in discoveredParameters)
            {
                discoveredParameter.Value = DBNull.Value;
            }
            return discoveredParameters;
        }

        /// <summary> 
        /// SqlParameter参数数组的深层拷贝. 
        /// </summary> 
        /// <param name="originalParameters">原始参数数组</param> 
        /// <returns>返回一个同样的参数数组</returns> 
        private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
        {
            SqlParameter[] clonedParameters = new SqlParameter[originalParameters.Length];

            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
            }

            return clonedParameters;
        }

        #endregion 私有方法,字段,构造函数结束

        #region 缓存方法

        /// <summary> 
        /// 追加参数数组到缓存. 
        /// </summary> 
        /// <param name="connectionString">一个有效的数据库连接字符串</param> 
        /// <param name="commandText">存储过程名或SQL语句</param> 
        /// <param name="commandParameters">要缓存的参数数组</param> 
        public static void CacheParameterSet(string connectionString, string commandText, params SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            string hashKey = connectionString + ":" + commandText;

            paramCache[hashKey] = commandParameters;
        }

        /// <summary> 
        /// 从缓存中获取参数数组. 
        /// </summary> 
        /// <param name="connectionString">一个有效的数据库连接字符</param> 
        /// <param name="commandText">存储过程名或SQL语句</param> 
        /// <returns>参数数组</returns> 
        public static SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            string hashKey = connectionString + ":" + commandText;

            SqlParameter[] cachedParameters = paramCache[hashKey] as SqlParameter[];
            if (cachedParameters == null)
            {
                return null;
            }
            else
            {
                return CloneParameters(cachedParameters);
            }
        }

        #endregion 缓存方法结束

        #region 检索指定的存储过程的参数集

        /// <summary> 
        /// 返回指定的存储过程的参数集 
        /// </summary> 
        /// <remarks> 
        /// 这个方法将查询数据库,并将信息存储到缓存. 
        /// </remarks> 
        /// <param name="connectionString">一个有效的数据库连接字符</param> 
        /// <param name="spName">存储过程名</param> 
        /// <returns>返回SqlParameter参数数组</returns> 
        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName)
        {
            return GetSpParameterSet(connectionString, spName, false);
        }

        /// <summary> 
        /// 返回指定的存储过程的参数集 
        /// </summary> 
        /// <remarks> 
        /// 这个方法将查询数据库,并将信息存储到缓存. 
        /// </remarks> 
        /// <param name="connectionString">一个有效的数据库连接字符.</param> 
        /// <param name="spName">存储过程名</param> 
        /// <param name="includeReturnValueParameter">是否包含返回值参数</param> 
        /// <returns>返回SqlParameter参数数组</returns> 
        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return GetSpParameterSetInternal(connection, spName, includeReturnValueParameter);
            }
        }

        /// <summary> 
        /// [内部]返回指定的存储过程的参数集(使用连接对象). 
        /// </summary> 
        /// <remarks> 
        /// 这个方法将查询数据库,并将信息存储到缓存. 
        /// </remarks> 
        /// <param name="connection">一个有效的数据库连接字符</param> 
        /// <param name="spName">存储过程名</param> 
        /// <returns>返回SqlParameter参数数组</returns> 
        internal static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName)
        {
            return GetSpParameterSet(connection, spName, false);
        }

        /// <summary> 
        /// [内部]返回指定的存储过程的参数集(使用连接对象) 
        /// </summary> 
        /// <remarks> 
        /// 这个方法将查询数据库,并将信息存储到缓存. 
        /// </remarks> 
        /// <param name="connection">一个有效的数据库连接对象</param> 
        /// <param name="spName">存储过程名</param> 
        /// <param name="includeReturnValueParameter"> 
        /// 是否包含返回值参数 
        /// </param> 
        /// <returns>返回SqlParameter参数数组</returns> 
        internal static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            using (SqlConnection clonedConnection = (SqlConnection)((ICloneable)connection).Clone())
            {
                return GetSpParameterSetInternal(clonedConnection, spName, includeReturnValueParameter);
            }
        }

        /// <summary> 
        /// [私有]返回指定的存储过程的参数集(使用连接对象) 
        /// </summary> 
        /// <param name="connection">一个有效的数据库连接对象</param> 
        /// <param name="spName">存储过程名</param> 
        /// <param name="includeReturnValueParameter">是否包含返回值参数</param> 
        /// <returns>返回SqlParameter参数数组</returns> 
        private static SqlParameter[] GetSpParameterSetInternal(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            string hashKey = connection.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");

            SqlParameter[] cachedParameters;

            cachedParameters = paramCache[hashKey] as SqlParameter[];
            if (cachedParameters == null)
            {
                SqlParameter[] spParameters = DiscoverSpParameterSet(connection, spName, includeReturnValueParameter);
                paramCache[hashKey] = spParameters;
                cachedParameters = spParameters;
            }

            return CloneParameters(cachedParameters);
        }

        #endregion 参数集检索结束

    }
}
