#region License
//===============================================================================
// CscWfms   人力管理系统数据访问层
// 上海新致软件公司
//===============================================================================

// Function :
// CreateUser:  wuwei.wen
// CreateTime:  2008-09-10

//===============================================================================
#endregion

using System;
using System.Data;
using System.Data.SqlClient;

namespace Dal
{
	/// <summary>
	/// Class1 的摘要说明。
	/// </summary>
	public class SqlOperate : System.IDisposable
	{

		#region 成员定义
		private YM.Data.SqlScope mySS;
		#endregion

		#region 类构造函数
		public SqlOperate()
		{
			if(mySS != null)mySS = null;		
			mySS = new YM.Data.SqlScope(System.Configuration.ConfigurationSettings.AppSettings["connectionStr"]);
						
		}
		#endregion

		#region 数据更新与删除操作ExecuteNonQuery
		public void ExecuteNonQuery(string strSQL,params object[] ParObj)
		{
			try
			{
				using(mySS.EnterQuery())
				{
					this.mySS.ExecuteNonQuery(strSQL,ParObj);
				}
				
			}
			catch(Exception e)
			{
				//日志写入操作***
				throw new Exception(e.Message + @"ERROR FILE:SqlOperate  Method:ExecuteNonQuery()  SQL:" + strSQL);
			}
			return;
		}
		#endregion

		#region 数据查询操作

        public int ExecuteScalar(string strSQL, params object[] ParObj)
        {
   
            int count = 0;
            try
            {
                using (mySS.EnterQuery())
                {
                    count = Convert.ToInt16(this.mySS.ExecuteScalar(strSQL, ParObj));
                }
            }
            catch (Exception e)
            {
                //日志写入操作***
                throw new Exception(e.Message + @"ERROR FILE:SqlOperate  Method:ExecuteDataSet()  SQL:" + strSQL);
            }
            return count;

        }
        
		public DataSet ExecuteDataSet(string strSQL,params object[] ParObj)
		{
			DataSet ds = new DataSet();
			try
			{
				using(mySS.EnterQuery())
				{
					ds = this.mySS.ExecuteDataSet(strSQL,ParObj);
				}
			}
			catch(Exception e)
			{
				//日志写入操作***
				throw new Exception(e.Message + @"ERROR FILE:SqlOperate  Method:ExecuteDataSet()  SQL:" + strSQL);
			}
			return ds;
					
		}
		
		public DataTable ExecuteDataTable(string strSQL,params object[] ParObj)
		{
			DataTable dt = new DataTable();
			try
			{
				using(mySS.EnterQuery())
				{
					dt = this.mySS.ExecuteDataTable(strSQL,ParObj);
				}
			}
			catch(Exception e)
			{
				//日志写入操作***
				throw new Exception(e.Message + @"ERROR FILE:SqlOperate  Method:ExecuteDataTable()  SQL:" + strSQL);
				
			}
			
			return dt;
			
		}

		public System.Data.SqlClient.SqlDataReader ExecuteReader(string strSQL,params object[] ParObj)
		{
			SqlDataReader dr;
			try
			{
				using(mySS.EnterQuery())
				{
					dr = this.mySS.ExecuteReader(strSQL,ParObj);
				}
			}
			catch(Exception e)
			{
				//日志写入操作***
				throw new Exception(e.Message + @"ERROR FILE:SqlOperate  Method:ExecuteReader()  SQL:" + strSQL);
				
			}

			return dr;
			
		}

		#endregion

		#region 分页操作
		public DataTable DoPaging(YM.Web.UI.WebControls.Pager Pager, string strSql,params object[] ParObj)
		{
			
			DataTable  dt = new DataTable();
			try
			{
				using(mySS.EnterQuery())
				{				
					//取得COUNT数据
					int i = strSql.ToUpper().IndexOf("FROM ");
					string strSqlCount = strSql.Substring(0,i+5);
					strSqlCount = strSql.Replace(strSqlCount,@"Select Count(*) as A From ");
					//System.Text.RegularExpressions.Regex.Replace(strSql,@"^(select)(^(select))*from","Select Count(*) as A From");
					strSqlCount = System.Text.RegularExpressions.Regex.Replace(strSqlCount,@"order by.*","");	
					
					object obj = this.mySS.ExecuteScalar(strSqlCount,ParObj);
					Pager.RecordCount = Convert.ToInt16(obj);
					int intCurPage = Pager.CurrentPageIndex;
					if (intCurPage > Pager.PageCount)
					{
						intCurPage--;
						Pager.CurrentPageIndex--;
					}

					//返回查询结果
					DataSet  dst = new DataSet();					
					SqlDataAdapter da = this.mySS.CreateAdapter(strSql,ParObj);
					int StartRecord = 0;
					if (intCurPage > 1)
					{
						StartRecord = (intCurPage - 1) * Pager.PageSize;
					}
					da.Fill(dst, StartRecord, Pager.PageSize, "temp");
					dt = dst.Tables[0];
					
				}
			}
			catch(Exception e)
			{
				//日志写入操作***
				throw new Exception(e.Message + @"ERROR FILE:SqlOperate  Method:DoPaging()  SQL:" + strSql);
			}
			
			return dt;
			
		}
        public DataTable DoPaging(string strSql, int index, int size)
        {
            DataTable dt = new DataTable();
            try
            {
                using (mySS.EnterQuery())
                {
                    int intCurPage = index;
                    //返回查询结果
                    DataSet dst = new DataSet();
                    SqlDataAdapter da = this.mySS.CreateAdapter(strSql);
                    int StartRecord = 0;
                    if (intCurPage > 1)
                    {
                        StartRecord = (intCurPage - 1) * size;
                    }
                    da.Fill(dst, StartRecord, size, "temp");
                    dt = dst.Tables[0];

                }
            }
            catch (Exception e)
            {
                //日志写入操作***
                throw new Exception(e.Message + @"ERROR FILE:SqlOperate  Method:DoPaging()  SQL:" + strSql);
            }

            return dt;

        }
        public DataTable DoPaging(string strSql, int index, int size, out int RecordCount)
        {
            DataTable dt = new DataTable();
            try
            {
                using (mySS.EnterQuery())
                {
                    //取得COUNT数据
                    int i = strSql.ToUpper().IndexOf("FROM ");
                    string strSqlCount = strSql.Substring(0, i + 5);
                    strSqlCount = strSql.Replace(strSqlCount, @"Select Count(*) as A From ");
                    //System.Text.RegularExpressions.Regex.Replace(strSql,@"^(select)(^(select))*from","Select Count(*) as A From");
                    strSqlCount = System.Text.RegularExpressions.Regex.Replace(strSqlCount, @"order by.*", "");

                    object obj = this.mySS.ExecuteScalar(strSqlCount);
                    RecordCount = Convert.ToInt16(obj);


                    int intCurPage = index;
                    //返回查询结果
                    DataSet dst = new DataSet();
                    SqlDataAdapter da = this.mySS.CreateAdapter(strSql);
                    int StartRecord = 0;
                    if (intCurPage > 1)
                    {
                        StartRecord = (intCurPage - 1) * size;
                    }
                    da.Fill(dst, StartRecord, size, "temp");
                    dt = dst.Tables[0];

                }
            }
            catch (Exception e)
            {
                //日志写入操作***
                throw new Exception(e.Message + @"ERROR FILE:SqlOperate  Method:DoPaging()  SQL:" + strSql);
            }

            return dt;

        }
		#endregion

		#region IDisposable 成员(接口实现)
		public void Dispose()
		{
			// TODO:  添加 SqlOperate.Dispose 实现
			mySS.Dispose();
			mySS = null;
		}
		#endregion
	}
}
