using FineUIMvc.PumpMVC.Common.ExcelExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FineUIMvc.PumpMVC.ReportModel
{
    [ExcelExport("调峰日志")]
    public class TF_runLog
    {
        public Guid BaseID { get; set; }

        [ExcelExport("调峰编号")]
        public string FDTUCode { get; set; }

        [ExcelExport("	调峰名称")]
        public string FName { get; set; }
        [ExcelExport("采集时间")]
        public DateTime TempTime { get; set; }



        [ExcelExport("	进水压力(MPa)")]
        public decimal F40001 { get; set; }
        [ExcelExport("	出水压力(MPa) ")]
        public decimal F40002 { get; set; }
        [ExcelExport("进累积流量(m³)")]
        public int FTotalInLL { get; set; }
        [ExcelExport("出累积流量(m³)")]
        public int FTotalOutLL { get; set; }
        [ExcelExport("蓄水流量(m³/h)")]
        public decimal F40003 { get; set; }
        [ExcelExport("水箱容积(m³)")]
        public decimal F40004 { get; set; }

        [ExcelExport("出水流量(m³/h)")]
        public decimal F40005 { get; set; }
        [ExcelExport("出水流量(m³/h)")]
        public decimal F40006 { get; set; }
        [ExcelExport("储水总量(m³)")]
        public decimal F40007 { get; set; }
        [ExcelExport("调峰水量(m³)")]
        public decimal F40008 { get; set; }

        [ExcelExport("调节比例(%)")]
        public string F40009 { get; set; }
        [ExcelExport("储水比例(%)")]
        public decimal F40010 { get; set; }
        [ExcelExport("	调峰能力(min)")]
        public string F40011 { get; set; }
        [ExcelExport("	滞留时间(h)	")]
        public decimal F40012 { get; set; }
        [ExcelExport("水箱液位(m)")]
        public decimal F40013 { get; set; }
        [ExcelExport("	日调峰量(m³)")]
        public decimal F40014 { get; set; }



        public string F40015 { get; set; }

        [ExcelExport("调控状态")]
        public string F40015Name
        {
            get
            {
                if (F40015 == "1")
                {
                    return "打开";
                }
                else
                {
                    return "关闭";
                }
            }
        }

        public string F40016 { get; set; }
        [ExcelExport("流量阀远程控制")]
        public string F40016Name
        {
            get
            {
                if (F40016 == "1")
                {
                    return "启用";
                }
                else
                {
                    return "停用";
                }
            }
        }
    }
}