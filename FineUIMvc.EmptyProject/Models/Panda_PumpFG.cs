using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineUIMvc.PumpMVC.Models
{
    [Table("Panda_PumpFG")]
    public class Panda_PumpFG
    {
        /// <summary>
        /// 泵房id
        /// </summary>
        [Key]
         public int ID { get; set; }
        /// <summary>
        /// 泵房组名称
        /// </summary>
        public string G_Name { get; set; }
        /// <summary>
        /// 泵房分组类型（客户/分公司/管理员分组）
        /// </summary>
        public int G_Type { get; set; }
        //public int UserID { get; set; }

        public Nullable<int> FCreateUser { get; set; }
        public Nullable<System.DateTime> FCreateDate { get; set; }
        public Nullable<int> FUpdUser { get; set; }
        public Nullable<System.DateTime> FUpdDate { get; set; }
        public Nullable<int> FDelUser { get; set; }
        public Nullable<System.DateTime> FDelDate { get; set; }
        public int FIsDelete { get; set; }
    }
}