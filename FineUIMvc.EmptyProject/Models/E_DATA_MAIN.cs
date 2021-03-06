﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineUIMvc.PumpMVC.Models
{
    [Table("E_DATA_MAIN")]
    public class E_DATA_MAIN
    {
        [Key]
        public Guid ID { get; set; }
        public string FDTUCode { get; set; }
        public int FOnLine { get; set; }
        public Nullable<DateTime> FUpdateDate { get; set; }
        public Nullable<DateTime> TempTime { get; set; }
        public Guid BaseID { get; set; }
        public Nullable<int> FTotalDL { get; set; }
        public Nullable<int> FTotalOutLL { get; set; }
        public Nullable<int> FTotalInLL { get; set; }
        public string F41001 { get; set; }
        public string F41002 { get; set; }
        public string F41003 { get; set; }
        public string F41004 { get; set; }
        public string F41005 { get; set; }
        public Nullable<decimal> F41006 { get; set; }
        public Nullable<decimal> F41007 { get; set; }
        public string F41008 { get; set; }
        public string F41009 { get; set; }
        public string F41010 { get; set; }
        public string F41011 { get; set; }
        public string F41012 { get; set; }
        public string F41013 { get; set; }
        public Nullable<decimal> F41014 { get; set; }
        public Nullable<decimal> F41015 { get; set; }
        public Nullable<decimal> F41016 { get; set; }
        public Nullable<decimal> F41017 { get; set; }
        public Nullable<decimal> F41018 { get; set; }
        public Nullable<decimal> F41019 { get; set; }
        public Nullable<decimal> F41020 { get; set; }
        public Nullable<decimal> F41021 { get; set; }
        public Nullable<decimal> F41022 { get; set; }
        public Nullable<decimal> F41023 { get; set; }
        public Nullable<decimal> F41024 { get; set; }
        public Nullable<decimal> F41025 { get; set; }
        public Nullable<decimal> F41026 { get; set; }
        public Nullable<decimal> F41027 { get; set; }
        public Nullable<decimal> F41028 { get; set; }
        public Nullable<decimal> F41029 { get; set; }
        public Nullable<decimal> F41030 { get; set; }
        public Nullable<decimal> F41031 { get; set; }
        public Nullable<decimal> F41032 { get; set; }
        public Nullable<decimal> F41033 { get; set; }
        public Nullable<decimal> F41034 { get; set; }
        public string F41035 { get; set; }
        public string F41036 { get; set; }
        public string F41037 { get; set; }
        public string F41038 { get; set; }
        public string F41039 { get; set; }
        public string F41040 { get; set; }
        public Nullable<decimal> F41041 { get; set; }
        public string F41042 { get; set; }
        public string F41043 { get; set; }
        public Nullable<decimal> F41044 { get; set; }
        public Nullable<decimal> F41045 { get; set; }
        public Nullable<decimal> F41046 { get; set; }
        public Nullable<decimal> F41047 { get; set; }
        public Nullable<decimal> F41048 { get; set; }
        public Nullable<decimal> F41049 { get; set; }
        public Nullable<decimal> F41050 { get; set; }
        public Nullable<decimal> F41051 { get; set; }
        public Nullable<decimal> F41052 { get; set; }
        public Nullable<decimal> F41053 { get; set; }
        public Nullable<decimal> F41054 { get; set; }
        public Nullable<decimal> F41055 { get; set; }
        public Nullable<decimal> F41056 { get; set; }
        public Nullable<decimal> F41057 { get; set; }
        public Nullable<decimal> F41058 { get; set; }
        public Nullable<decimal> F41059 { get; set; }
        public Nullable<decimal> F41060 { get; set; }
        public Nullable<decimal> F41061 { get; set; }
        public Nullable<decimal> F41062 { get; set; }
        public Nullable<decimal> F41063 { get; set; }
        public Nullable<decimal> F41064 { get; set; }
        public Nullable<decimal> F41065 { get; set; }
        public Nullable<decimal> F41066 { get; set; }
        public Nullable<decimal> F41068 { get; set; }
        public Nullable<decimal> F41069 { get; set; }
        public Nullable<decimal> F41070 { get; set; }
        public Nullable<decimal> F41071 { get; set; }
        public Nullable<decimal> F41072 { get; set; }
        public Nullable<decimal> F41073 { get; set; }
        public Nullable<decimal> F41074 { get; set; }
        public Nullable<decimal> F41075 { get; set; }
        public Nullable<decimal> F41076 { get; set; }
        public Nullable<decimal> F41077 { get; set; }
        public Nullable<decimal> F41078 { get; set; }
        public Nullable<decimal> F41079 { get; set; }
        public Nullable<decimal> F41080 { get; set; }
        public Nullable<decimal> F41081 { get; set; }
        public Nullable<decimal> F41082 { get; set; }
        public Nullable<decimal> F41083 { get; set; }
        public Nullable<decimal> F41084 { get; set; }
        public Nullable<decimal> F41085 { get; set; }
        public Nullable<decimal> F41086 { get; set; }
        public Nullable<decimal> F41087 { get; set; }
        public Nullable<decimal> F41088 { get; set; }
        public Nullable<decimal> F41089 { get; set; }
        public Nullable<decimal> F41090 { get; set; }
        public Nullable<decimal> F41091 { get; set; }
        public Nullable<decimal> F41092 { get; set; }
        public string F41093 { get; set; }
        public string F41094 { get; set; }
        public string F41095 { get; set; }
        public string F41096 { get; set; }
        public string F41097 { get; set; }
        public string F41098 { get; set; }
        public string F41099 { get; set; }
        public Nullable<decimal> F41100 { get; set; }
        public Nullable<decimal> F41101 { get; set; }
        public string F41102 { get; set; }
        public string F41103 { get; set; }
        public string F41104 { get; set; }
        public string F41105 { get; set; }
        public string F41106 { get; set; }
        public Nullable<decimal> F41107 { get; set; }
        public string F41108 { get; set; }
        public Nullable<decimal> F41109 { get; set; }
        public string F41110 { get; set; }
        public Nullable<decimal> F41111 { get; set; }
        public string F41112 { get; set; }
        public Nullable<decimal> F41113 { get; set; }
        public string F41114 { get; set; }
        public Nullable<decimal> F41115 { get; set; }
        public string F41116 { get; set; }
        public Nullable<decimal> F41117 { get; set; }
        public string F41118 { get; set; }
        public Nullable<decimal> F41119 { get; set; }
        public string F41120 { get; set; }
        public Nullable<decimal> F41121 { get; set; }
        public string F41122 { get; set; }
        public Nullable<decimal> F41123 { get; set; }
        public string F41124 { get; set; }
        public Nullable<decimal> F41125 { get; set; }
        public string F41126 { get; set; }
        public Nullable<decimal> F41127 { get; set; }
        public string F41128 { get; set; }
        public Nullable<decimal> F41129 { get; set; }
        public string F41130 { get; set; }
        public Nullable<decimal> F41131 { get; set; }
        public string F41132 { get; set; }
        public Nullable<decimal> F41133 { get; set; }
        public string F41134 { get; set; }
        public Nullable<decimal> F41135 { get; set; }
        public string F41136 { get; set; }
        public Nullable<decimal> F41137 { get; set; }
        public string F41138 { get; set; }
        public string F41139 { get; set; }
        public Nullable<decimal> F41140 { get; set; }
        public Nullable<decimal> F41141 { get; set; }
        public string F41142 { get; set; }
        public string F41701 { get; set; }
        public Nullable<decimal> F41702 { get; set; }
        public string F41703 { get; set; }
        public string F41704 { get; set; }
        public string F41705 { get; set; }
        public string F41706 { get; set; }
        public string F41707 { get; set; }
        public string F41708 { get; set; }
        public Nullable<decimal> F41709 { get; set; }
        public string F41710 { get; set; }
        public Nullable<decimal> F41711 { get; set; }
        public string F41712 { get; set; }
        public Nullable<decimal> F41713 { get; set; }
        public Nullable<decimal> F41714 { get; set; }
        public Nullable<decimal> F41715 { get; set; }
        public Nullable<decimal> F41716 { get; set; }
        public string F41717 { get; set; }
        public string F41718 { get; set; }
        public string F41719 { get; set; }
        public string F41720 { get; set; }
        public string F41721 { get; set; }
        public string F41722 { get; set; }
        public string F41723 { get; set; }
        public string F41724 { get; set; }
        public string F41725 { get; set; }
        public string F41726 { get; set; }
        public string F41727 { get; set; }
        public Nullable<decimal> F41728 { get; set; }
        public Nullable<decimal> F41729 { get; set; }
        public Nullable<decimal> F41730 { get; set; }
        public Nullable<decimal> F41731 { get; set; }
        public Nullable<decimal> F41732 { get; set; }
        public Nullable<decimal> F41733 { get; set; }
        public Nullable<decimal> F41734 { get; set; }
        public Nullable<decimal> F41735 { get; set; }

    }
}