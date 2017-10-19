using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineUIMvc.PumpMVC.Models
{
    [Table("Panda_PumpJZ")]
    public class Panda_PumpJZ
    {
        /// <summary>
        /// 泵房机组ID
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        public Guid PumpId { get; set; }
        /// <summary>
        /// DTU编号
        /// </summary>
        public string DTUCode { get; set; }
        /// <summary>
        /// 泵房机组名称
        /// </summary>
        public string PumpJZName { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string MachineType { get; set; }
        /// <summary>
        /// 主泵泵数
        /// </summary>
        public int RunPumpNum { get; set; }
        /// <summary>
        /// 辅泵泵数
        /// </summary>
        public int Auxiliarypumpcount { get; set; }
        /// <summary>
        /// 排水泵数
        /// </summary>
        public int DrainPumpNum { get; set; }
        /// <summary>
        /// 是否报警
        /// </summary>
        public int IsAlarm { get; set; }
        /// <summary>
        /// 所在区域
        /// </summary>
        public string PumpJZArea { get; set; }
        /// <summary>
        /// 采集周期
        /// </summary>
        public int PumpJZCollectPeriod { get; set; }
        /// <summary>
        /// 采集长度
        /// </summary>
        public int PumpJZCollectLength { get; set; }
        /// <summary>
        /// 读取模式
        /// </summary>
        public string PumpJZReadMode { get; set; }
        /// <summary>
        /// 水箱容积
        /// </summary>
        public string PumpJZTankVolume { get; set; }
        /// <summary>
        /// 水箱尺寸
        /// </summary>
        public string PumpJZTankLength { get; set; }
        /// <summary>
        /// 进口管径
        /// </summary>
        public string PumpJZInletDiameter { get; set; }
        /// <summary>
        /// 出口管径
        /// </summary>
        public string PumpJZOutletDiameter { get; set; }
        /// <summary>
        /// 成套品牌
        /// </summary>
        public string PumpJZBrandSet { get; set; }
        /// <summary>
        /// 水泵品牌
        /// </summary>
        public string PumpJZPumpBrand { get; set; }
        /// <summary>
        /// 主泵流量
        /// </summary>
        public string PumpJZMainPumpFlow { get; set; }
        /// <summary>
        /// 主泵扬程
        /// </summary>
        public string PumpJZMainPumpLift { get; set; }
        /// <summary>
        /// 辅泵流量
        /// </summary>
        public string PumpJZAuxiliPumpFlow { get; set; }
        /// <summary>
        /// 辅泵扬程
        /// </summary>
        public string PumpJZAuxiliPumpLift { get; set; }
        /// <summary>
        /// 主泵功率
        /// </summary>
        public string PumpJZMainPumpPower { get; set; }
        /// <summary>
        /// 辅泵功率
        /// </summary>
        public string PumpJZAuxiliPumpPower { get; set; }
        /// <summary>
        /// 有无减压阀
        /// </summary>
        public int PumpJZPressReliValve { get; set; }
        /// <summary>
        /// 是否调峰功能
        /// </summary>
        public int PumpJZPeak { get; set; }
        /// <summary>
        /// 止回阀类型
        /// </summary>
        public string PumpJZCheckValve { get; set; }
        /// <summary>
        /// 进水压力上限
        /// </summary>
        public decimal PumpJZInPUpper { get; set; }
        /// <summary>
        /// 进水压力下限
        /// </summary>
        public decimal PumpJZInPLower { get; set; }
        /// <summary>
        /// 出水压力上限
        /// </summary>
        public decimal PumpJZOutPUpper { get; set; }
        /// <summary>
        /// 出水压力下限
        /// </summary>
        public decimal PumpJZOutPLower { get; set; }
        /// <summary>
        /// 余氯值上限
        /// </summary>
        public decimal PumpJZReChlorUpper { get; set; }
        /// <summary>
        /// 余氯值下限
        /// </summary>
        public decimal PumpJZReChlorLower { get; set; }
        /// <summary>
        /// 浊度值上限
        /// </summary>
        public decimal PumpJZTurbidUpper { get; set; }
        /// <summary>
        /// 浊度值下限
        /// </summary>
        public decimal PumpJZTurbidLower { get; set; }
        /// <summary>
        /// PH值上限
        /// </summary>
        public decimal PumpJZPHUpper { get; set; }
        /// <summary>
        /// PH值下限
        /// </summary>
        public decimal PumpJZPHLower { get; set; }
        /// <summary>
        /// 水箱上
        /// </summary>
        public decimal PumpJZTankUpper { get; set; }
        /// <summary>
        /// 水箱下
        /// </summary>
        public decimal PumpJZTankLower { get; set; }

        public Nullable<int> FCreateUser { get; set; }
        public Nullable<System.DateTime> FCreateDate { get; set; }
        public Nullable<int> FUpdUser { get; set; }
        public Nullable<System.DateTime> FUpdDate { get; set; }
        public Nullable<int> FDelUser { get; set; }
        public Nullable<System.DateTime> FDelDate { get; set; }
        public int FIsDelete { get; set; }
        //public virtual Dtu_Base Dtu_Base { get; set; }
        public virtual AddressScheme AddressScheme { get; set; }

    }
}