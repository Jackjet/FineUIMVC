using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineUIMvc.PumpMVC.Models
{
    /// <summary>
    /// 加压站机组
    /// </summary>
    [Table("BASE_JIAYAZHAN_JZ")]
    public class BASE_JIAYAZHAN_JZ
    {
        [Key]
        public Guid ID { get; set; }
        public Guid jyzId { get; set; }
        public string DTUCode { get; set; }
        public string jyzJZName { get; set; }
        public string FType { get; set; }
        public int FSort { get; set; }
        public int jyzJZCollectPeriod { get; set; }
        public int jyzJZCollectLength { get; set; }
        public string jyzJZReadMode { get; set; }
        public int FCreateUser { get; set; }
        public Nullable<DateTime> FCreateDate { get; set; }
        public Nullable<int> FUpdUser { get; set; }
        public Nullable<DateTime> FUpdDate { get; set; }
        public Nullable<int> FDelUser { get; set; }
        public Nullable<DateTime> FDelDate { get; set; }
        public int FIsDelete { get; set; }
        public virtual AddressScheme AddressScheme { get; set; }

    }
}