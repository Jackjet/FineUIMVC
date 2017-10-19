using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineUIMvc.PumpMVC.Models
{
      [Table("Alarm_Contact_Contact")]
    /// <summary>
    /// 报警联系人
    /// </summary>
    public class MessageSetting
    {
         [Key]
        public int ID { get; set; }
        /// <summary>
        /// 是否报警
        /// </summary>
         public int Type { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
         public string Contacts { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
         public string ContactPhone { get; set; }
        /// <summary>
        /// 联系人类型（报警联系人或。。。）    0：报警联系人
        /// </summary>
         public int ContactSource { get; set; }
         /// <summary>
         /// 组ID
         /// </summary>
         public int GroupID { get; set; }
         public Nullable<DateTime> FCreateDate { get; set; }
         public Nullable<int> FCreateUser { get; set; }
         public Nullable<int> FUpdUser { get; set; }
         public Nullable<System.DateTime> FUpdDate { get; set; }
         public Nullable<int> FDelUser { get; set; }
         public Nullable<System.DateTime> FDelDate { get; set; }
         public int FIsDelete { get; set; }

    }
}