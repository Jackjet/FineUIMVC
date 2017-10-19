using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineUIMvc.PumpMVC.Models
{
     [Table("Panda_PGroup")]
    public class Panda_PGroup
    {

         public Panda_PGroup()
        {
            this.Panda_UserInfo = new HashSet<Panda_UserInfo>();
        }

        /// <summary>
        /// 泵房id
        /// </summary>
        [Key]
         public int GroupID { get; set; }
        /// <summary>
        /// 泵房组名称
        /// </summary>
        public string GroupName { get; set; }

        public Nullable<int> FCreateUser { get; set; }
        public Nullable<System.DateTime> FCreateDate { get; set; }
        public Nullable<int> FUpdUser { get; set; }
        public Nullable<System.DateTime> FUpdDate { get; set; }
        public Nullable<int> FDelUser { get; set; }
        public Nullable<System.DateTime> FDelDate { get; set; }
        public int FIsDelete { get; set; }
        public virtual ICollection<Panda_UserInfo> Panda_UserInfo { get; set; }
        //public virtual Panda_UserInfo Panda_UserInfo { get; set; }

    }
}