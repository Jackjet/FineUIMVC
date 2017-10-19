using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace Bll
{
    public class Bas_ProvinceBll
    {
        public static Hashtable Search(string key, int index, int size, string sortField, string sortOrder)
        {
            return Dal.Bas_ProvinceDal.Search(key, index, size, sortField, sortOrder);
        }
        public static Hashtable Get(string id)
        {
            return Dal.Bas_ProvinceDal.Get(id);
        }
        public static string Insert(Hashtable has)
        {
            return Dal.Bas_ProvinceDal.Insert(has);
        }
        public static void Delete(string userId)
        {
            Dal.Bas_ProvinceDal.Delete(userId);
        }
        public static void Update(Hashtable has)
        {
            Dal.Bas_ProvinceDal.Update(has);
        }
    }
}