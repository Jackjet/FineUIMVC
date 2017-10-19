using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineUIMvc.PumpMVC.Models
{
    [Table("Alarm_Contact_Group")]
    public class Alarm_Contact_Group
    {
        [Key]
        public int ID { get; set; }
        public string FName { get; set; }
        //public int FCustomerID { get; set; }
        public int FType { get; set; }
        public string FNote { get; set; }
        public int FCreateUser { get; set; }
        public Nullable<DateTime> FCreateDate { get; set; }
        public Nullable<int> FUpdUser { get; set; }
        public Nullable<DateTime> FUpdDate { get; set; }
        public Nullable<int> FDelUser { get; set; }
        public Nullable<DateTime> FDelDate { get; set; }
        public int FIsDelete { get; set; }
        public virtual Panda_Customer Panda_Customer { get; set; }

    }
}