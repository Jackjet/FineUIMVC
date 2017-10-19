using FineUIMvc.PumpMVC.Common.ExcelExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FineUIMvc.PumpMVC.ReportModel
{
    [ExcelExport("阀门日志")]
    public class FM_runLog
    {
        public Guid BaseID { get; set; }

        [ExcelExport("测点地址")]
        public string FDTUCode { get; set; }
        [ExcelExport("地址")]
        public string FMapAddress { get; set; }
        [ExcelExport("阀门名称")]
        public string FName { get; set; }

        [ExcelExport("采集时间")]
        public DateTime TempTime { get; set; }
        [ExcelExport("进水压力(MPa)")]
        public decimal F40001 { get; set; }
        [ExcelExport("出水压力(MPa)")]
        public decimal F40002 { get; set; }
        [ExcelExport("	压力给定(MPa)")]
        public decimal F40003 { get; set; }
        [ExcelExport("	阀门开度(%)")]
        public decimal F40004 { get; set; }

        [ExcelExport("	瞬时流量(m³/h)")]
        public decimal F40005 { get; set; }
        [ExcelExport("累计流量(m³/h)")]
        public int FTotalLL { get; set; }
     
    }
}