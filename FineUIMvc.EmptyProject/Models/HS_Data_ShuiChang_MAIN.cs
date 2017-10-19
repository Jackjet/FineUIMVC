using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineUIMvc.PumpMVC.Models
{
     [Table("HS_Data_ShuiChang_MAIN")]
    public class HS_Data_ShuiChang_MAIN
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
        public string F40011 { get; set; }
        public string F40012 { get; set; }
        public string F40013 { get; set; }
        public string F40014 { get; set; }
        public string F40015 { get; set; }
        public string F40016 { get; set; }
        public string F40017 { get; set; }
        public string F40018 { get; set; }
        public string F40019 { get; set; }
        public string F40020 { get; set; }
        public string F40021 { get; set; }
        public string F40022 { get; set; }
        public string F40023 { get; set; }
        public string F40024 { get; set; }
        public string F40025 { get; set; }
        public string F40026 { get; set; }
        public string F40027 { get; set; }
        public string F40028 { get; set; }
        public string F40029 { get; set; }
        public string F40030 { get; set; }
        public string F40031 { get; set; }
        public string F40032 { get; set; }
        public Nullable<DateTime> FUpdateDate { get; set; }
        public Nullable<DateTime> TempTime { get; set; }
        public string Repeat { get; set; }

    }
}