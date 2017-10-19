using FineUIMvc.PumpMVC.Common.ExcelExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FineUIMvc.PumpMVC.ReportModel
{
    [ExcelExport("压力日志")]
    public class YL_runLog
    {
        public Guid BaseID { get; set; }

        [ExcelExport("测点编号")]
        public string FDTUCode { get; set; }
        [ExcelExport("	测点地址")]
        public string FMapAddress { get; set; }
        [ExcelExport("测点编号")]
        public string FName { get; set; }

        [ExcelExport("	压力上限(Mpa)")]
        public string FMpaUp { get; set; }
        [ExcelExport("	压力下限(Mpa)")]
        public string FMpaDown { get; set; }
        [ExcelExport("	压力(Mpa)")]
        public decimal FMpa { get; set; }
        [ExcelExport("流量(m³)")]
        public decimal FLL { get; set; }
        [ExcelExport("电池电压(V)")]
        public string FBatt { get; set; }

        [ExcelExport("采集时间")]
        public DateTime TempTime { get; set; }

        public string Repeat { get; set; }
    }
}