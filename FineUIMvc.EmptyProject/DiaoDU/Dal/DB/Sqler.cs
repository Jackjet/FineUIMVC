using System;

using System.Configuration;
using System.Security.Cryptography;
using System.IO;
using System.Text;

using System.Data;

using System.Collections;

namespace Dal
{
	public class Sqler
	{
		/// <summary>
		/// 数据库连接字符串，静态变量
		/// </summary>
		public static string DbConnString;
		/// <summary>
		/// 解密数据库连接字符串
		/// </summary>
		public static void InitDbConnString()
		{
			DbConnString = ConfigurationSettings.AppSettings["connectionStr"];
		}
		/// <summary>
		/// 实例化SqlScope
		/// </summary>
		public static YM.Data.SqlScope Instance()
		{
			InitDbConnString();	
			return new YM.Data.SqlScope(DbConnString);
		}

		/// <summary>
		/// 解密数据库连接字符串
		/// </summary>
		public static void InitDbConnString(string str)
		{
			DbConnString = str;
		}
		/// <summary>
		/// 实例化SqlScope
		/// </summary>
		public static YM.Data.SqlScope Instance(string str)
		{
			InitDbConnString(str);	
			return new YM.Data.SqlScope(DbConnString);
		}


		/// <summary>
		/// 转换嵌入Sql语句查询参数中的通配符
		/// </summary>
		public static string ConvertLikeParam(string param)
		{
			//只有在用Lile语句时才需要替换下列通配符
			//如不是用SqlParameter方式赋值，还需替换'为''
			param = param.Replace("[", "[[]");
			param = param.Replace("_", "[_]");
			param = param.Replace("%", "[%]");
			//^
			param = param.Replace("'", "''");

			return param;
		}

	}
}
