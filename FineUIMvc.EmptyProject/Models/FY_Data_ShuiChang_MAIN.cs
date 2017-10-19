using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FineUIMvc.PumpMVC.Models
{
    [Table("FY_Data_ShuiChang_MAIN")]
    public class FY_Data_ShuiChang_MAIN
    {
        [Key]
        public Guid id { get; set; }
        public string FDTUCode { get; set; }
        public int FOnLine { get; set; }
        public Guid BaseID { get; set; }
        public string F40001 { get; set; }
        public string F40002 { get; set; }
        public string F40003 { get; set; }
        public string F40004 { get; set; }
        public string F40005 { get; set; }
        public string F40006 { get; set; }
        public string F40007 { get; set; }
        public string F40008 { get; set; }
        public string F40009 { get; set; }
        public string F40010 { get; set; }

        public Nullable<DateTime> FUpdateDate { get; set; }
        public Nullable<DateTime> TempTime { get; set; }
        public string Repeat { get; set; }
    }
}