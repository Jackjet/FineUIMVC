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
		/// ���ݿ������ַ�������̬����
		/// </summary>
		public static string DbConnString;
		/// <summary>
		/// �������ݿ������ַ���
		/// </summary>
		public static void InitDbConnString()
		{
			DbConnString = ConfigurationSettings.AppSettings["connectionStr"];
		}
		/// <summary>
		/// ʵ����SqlScope
		/// </summary>
		public static YM.Data.SqlScope Instance()
		{
			InitDbConnString();	
			return new YM.Data.SqlScope(DbConnString);
		}

		/// <summary>
		/// �������ݿ������ַ���
		/// </summary>
		public static void InitDbConnString(string str)
		{
			DbConnString = str;
		}
		/// <summary>
		/// ʵ����SqlScope
		/// </summary>
		public static YM.Data.SqlScope Instance(string str)
		{
			InitDbConnString(str);	
			return new YM.Data.SqlScope(DbConnString);
		}


		/// <summary>
		/// ת��Ƕ��Sql����ѯ�����е�ͨ���
		/// </summary>
		public static string ConvertLikeParam(string param)
		{
			//ֻ������Lile���ʱ����Ҫ�滻����ͨ���
			//�粻����SqlParameter��ʽ��ֵ�������滻'Ϊ''
			param = param.Replace("[", "[[]");
			param = param.Replace("_", "[_]");
			param = param.Replace("%", "[%]");
			//^
			param = param.Replace("'", "''");

			return param;
		}

	}
}
