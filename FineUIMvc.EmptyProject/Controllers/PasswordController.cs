using System;
using System.Security.Cryptography;
using System.Text;


namespace FineUIMvc.PumpMVC.Controllers
{
	/// <summary>
	/// ������������û����룬���Ƚ������Ƿ�һ�µ���
	/// </summary>
	public class PasswordUtil
	{
        #region field & constructor

        //private static readonly Log _log = new Log(typeof(PasswordUtil));

		private const int saltLength = 4;

        public PasswordUtil() { }

        #endregion

        /// <summary>
        /// �Ա��û����������Ƿ�ͼ��ܺ�����һ��
        /// </summary>
        /// <param name="dbPassword">���ݿ��е�����ܺ������</param>
        /// <param name="userPassword">�û���������</param>
        /// <returns></returns>
		
        public static bool ComparePasswords(string dbPassword,string userPassword)
		{
			
            byte[] dbPwd = Convert.FromBase64String(dbPassword);

			byte[] hashedPwd = HashString(userPassword);

			if(dbPwd.Length ==0 || hashedPwd.Length ==0 || dbPwd.Length !=hashedPwd.Length + saltLength)
			{
				return false;
			}

			byte[] saltValue = new byte[saltLength];
			//	int saltOffset = dbPwd.Length - hashedPwd.Length;
			int saltOffset = hashedPwd.Length;
			for (int i = 0; i < saltLength; i++)
				saltValue[i] = dbPwd[saltOffset + i];

			byte[] saltedPassword = CreateSaltedPassword(saltValue, hashedPwd);
		
			// compare the values
            /*return CompareByteArray(dbPwd, saltedPassword);
            */

            if (dbPassword.ToUpper().Equals(CreateDbPassword(userPassword)))
            {
              return true;
          }
          else {
              return false;
          }
			
		}





        /// <summary>
        /// �����û������ݿ�����
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
		public static string CreateDbPassword(string userPassword)
		{
			
            
            /*byte[] unsaltedPassword = HashString(userPassword);

			//Create a salt value
			byte[] saltValue = new byte[saltLength];
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
			rng.GetBytes(saltValue);
			
			byte[] saltedPassword = CreateSaltedPassword(saltValue, unsaltedPassword);

			return Convert.ToBase64String(saltedPassword);*/

            string source = userPassword;
            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMd5Hash(md5Hash, source);

                return hash.ToUpper();
            }



		}

		#region ˽�к���


        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
		/// <summary>
		/// ��һ���ַ�����ϣ��
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		private static byte[] HashString(string str)
		{
			byte[] pwd = System.Text.Encoding.UTF8.GetBytes(str);

			SHA1 sha1 = SHA1.Create();
			byte[] saltedPassword = sha1.ComputeHash(pwd);
			return saltedPassword;
		}
		private static bool CompareByteArray(byte[] array1, byte[] array2)
		{
			
           /* if (array1.Length != array2.Length)
				return false;
			for (int i = 0; i < array1.Length; i++)
			{
				if (array1[i] != array2[i])
					return false;
			}
            */

			return true;
		}
		// create a salted password given the salt value
		private static byte[] CreateSaltedPassword(byte[] saltValue, byte[] unsaltedPassword)
		{
			// add the salt to the hash
			byte[] rawSalted  = new byte[unsaltedPassword.Length + saltValue.Length]; 
			unsaltedPassword.CopyTo(rawSalted,0);
			saltValue.CopyTo(rawSalted,unsaltedPassword.Length);
			
			//Create the salted hash			
			SHA1 sha1 = SHA1.Create();
			byte[] saltedPassword = sha1.ComputeHash(rawSalted);

			// add the salt value to the salted hash
			byte[] dbPassword  = new byte[saltedPassword.Length + saltValue.Length];
			saltedPassword.CopyTo(dbPassword,0);
			saltValue.CopyTo(dbPassword,saltedPassword.Length);

			return dbPassword;
		}
		#endregion

	}
}
