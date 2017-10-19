using FineUIMvc.PumpMVC.Common.ExcelExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FineUIMvc.PumpMVC.ReportModel
{
    [ExcelExport("运行日志")]
    public class RunDayLog
    {
        public Guid pumpJZId { get; set; }    
        public Guid pumpID { get; set; }
        [ExcelExport("采集时间")]
        public DateTime FUpdateDate { get; set; }
        [ExcelExport("机组编号")]
        public string DTUCode { get; set; }
        [ExcelExport("所属泵房")]
        public string PumpName { get; set; }
        [ExcelExport("分区")]
        public string PumpJZArea { get; set; }
        [ExcelExport("系统状态")]
        public string F41003 { get; set; }
        [ExcelExport("进/出水压力(Mpa)")]
        public string InOutWaPa { get; set; }
        [ExcelExport("	压力给定")]
        public decimal F41702 { get; set; }
        [ExcelExport("泵运行状态")]
        public string PActiveState { get; set; }
        [ExcelExport("1#出累计流量(m3)")]
        public int FTotalOutLL { get; set; }
        [ExcelExport("1#出累计流量(m3)")]
        public int FTotalDL { get; set; }
        [ExcelExport("1#变频器频率(HZ)")]
        public decimal F41014 { get; set; }
    }
}