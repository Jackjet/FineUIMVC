using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineUIMvc.PumpMVC.Models
{
     [Table("AddressScheme")]
    public class AddressScheme
    {
         public AddressScheme()
        {
            this.Panda_PumpJZ = new HashSet<Panda_PumpJZ>();
            this.BASE_JIAYAZHAN_JZ = new HashSet<BASE_JIAYAZHAN_JZ>();
            this.BASE_SHUICHANG_JZ = new HashSet<BASE_SHUICHANG_JZ>();
            this.BASE_LIULIANG = new HashSet<BASE_LIULIANG>();
            this.BASE_FAMEN = new HashSet<BASE_FAMEN>();
            this.BASE_YALI = new HashSet<BASE_YALI>();
            this.BASE_YUZHIBENGZHAN = new HashSet<BASE_YUZHIBENGZHAN>();
        }

         [Key]
        public int ID { get; set; }
         public string FNumber { get; set; }
         public string FName { get; set; }
         public Nullable<int> FStartAddress { get; set; }
         public Nullable<int> FAddressLength { get; set; }
         public string FNote { get; set; }
         public Nullable<DateTime> FCreateDate { get; set; }
         public Nullable<int> FOrderBy { get; set; }
         public Nullable<int> FCreateUser { get; set; }
         public Nullable<int> FUpdUser { get; set; }
         public Nullable<System.DateTime> FUpdDate { get; set; }
         public Nullable<int> FDelUser { get; set; }
         public Nullable<System.DateTime> FDelDate { get; set; }
         public int FIsDelete { get; set; }
         public string FType { get; set; }

         public virtual ICollection<Panda_PumpJZ> Panda_PumpJZ { get; set; }
         public virtual ICollection<BASE_SHUICHANG_JZ> BASE_SHUICHANG_JZ { get; set; }
         public virtual ICollection<BASE_JIAYAZHAN_JZ> BASE_JIAYAZHAN_JZ { get; set; }
         public virtual ICollection<BASE_TIAOFENG> BASE_TIAOFENG { get; set; }
         public virtual ICollection<BASE_LIULIANG> BASE_LIULIANG { get; set; }
         public virtual ICollection<BASE_FAMEN> BASE_FAMEN { get; set; }
         public virtual ICollection<BASE_YALI> BASE_YALI { get; set; }
         public virtual ICollection<BASE_YUZHIBENGZHAN> BASE_YUZHIBENGZHAN { get; set; }
    }
}