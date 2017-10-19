using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineUIMvc.PumpMVC.Models
{
    [Table("Panda_Customer")]
    public class Panda_Customer
    {
        public Panda_Customer()
        {
            this.Panda_UserInfo = new HashSet<Panda_UserInfo>();
            this.Panda_Pump = new HashSet<Panda_Pump>();
            this.Role = new HashSet<Role>();
            this.BASE_LIULIANG = new HashSet<BASE_LIULIANG>();
            this.BASE_FAMEN = new HashSet<BASE_FAMEN>();
            this.BASE_SHUICHANG = new HashSet<BASE_SHUICHANG>();
            this.BASE_JIAYAZHAN = new HashSet<BASE_JIAYAZHAN>();
            this.BASE_YALI = new HashSet<BASE_YALI>();
            this.BASE_YUZHIBENGZHAN = new HashSet<BASE_YUZHIBENGZHAN>();
        }

        [Key]
        public int ID { get; set; }
        public string Guid { get; set; }
        public string Name { get; set; }
        public string FContactName { get; set; }
        public string FContactPhone { get; set; }
        public string FAddress { get; set; }
        public string FBGAddress { get; set; }
        public string FLngLat { get; set; }
        public string FCompanyNumber { get; set; }
        public int CustomerType { get; set; }
        public int CustomerLevel { get; set; }
        public int MessageCount { get; set; }
        public int FCreateUser { get; set; }
        public Nullable<DateTime> FCreateDate { get; set; }
        public Nullable<int> FUpdUser { get; set; }
        public Nullable<DateTime> FUpdDate { get; set; }
        public Nullable<int> FDelUser { get; set; }
        public Nullable<DateTime> FDelDate { get; set; }
        public int FIsDelete { get; set; }

        public Guid? FMapTempID { get; set; }

        //public virtual Panda_UserInfo Panda_UserInfo { get; set; }
        public virtual ICollection<Panda_UserInfo> Panda_UserInfo { get; set; }
        public virtual ICollection<Panda_Pump> Panda_Pump { get; set; }
        public virtual ICollection<Role> Role { get; set; }
        public virtual ICollection<BASE_LIULIANG> BASE_LIULIANG { get; set; }
        public virtual ICollection<BASE_SHUICHANG> BASE_SHUICHANG { get; set; }
        public virtual ICollection<BASE_FAMEN> BASE_FAMEN { get; set; }
        public virtual ICollection<Alarm_Contact_Group> Alarm_Contact_Group { get; set; }
        public virtual ICollection<BASE_JIAYAZHAN> BASE_JIAYAZHAN { get; set; }
        public virtual ICollection<BASE_YALI> BASE_YALI { get; set; }
        public virtual ICollection<BASE_TIAOFENG> BASE_TIAOFENG { get; set; }
        public virtual ICollection<BASE_YUZHIBENGZHAN> BASE_YUZHIBENGZHAN { get; set; }
    }
}