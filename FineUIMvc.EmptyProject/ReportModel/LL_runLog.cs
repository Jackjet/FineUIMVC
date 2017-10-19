using FineUIMvc.PumpMVC.Common.ExcelExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FineUIMvc.PumpMVC.ReportModel
{
    [ExcelExport("流量日志")]
    public class LL_runLog
    {
        public Guid BaseID { get; set; }

        [ExcelExport("测点编号")]
        public string FDTUCode { get; set; }
    
        [ExcelExport("测点名称")]
        public string FName { get; set; }

        [ExcelExport("采集时间")]
        public DateTime TempTime { get; set; }



        [ExcelExport("抄表标识")]
        public string C { get; set; }
        [ExcelExport("抄表标识")]
        public string E { get; set; }
        [ExcelExport("	正累积流量(m³)")]
        public int P01 { get; set; }
        [ExcelExport("负累积流量(m³)")]
        public int P02 { get; set; }
        [ExcelExport("瞬时流量(m³/h)")]
        public decimal A01 { get; set; }

        [ExcelExport("	水表电池电压(V)")]
        public decimal A02 { get; set; }
        [ExcelExport("进水压力(MPa)")]
        public decimal A03 { get; set; }
        [ExcelExport("出水压力(MPa)")]
        public decimal F4002 { get; set; }
        [ExcelExport("压力设定(MPa)")]
        public decimal F4003 { get; set; }
        [ExcelExport("阀门开度(%)")]
        public int F4004 { get; set; }
    }
}