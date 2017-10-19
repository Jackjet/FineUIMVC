using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineUIMvc.PumpMVC.Models
{
    [Table("BASE_SHUICHANG")]
    public class BASE_SHUICHANG
    {
        [Key]
        public Guid id { get; set; }
        public string FCode { get; set; }
        //public int FCustomerID { get; set; }
        public string FName { get; set; }
        public string FLngLat { get; set; }
        public string FMapAddress { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string FType { get; set; }
        /// <summary>
        /// 设计供水量
        /// </summary>
        public decimal FWater { get; set; }
        /// <summary>
        /// 设计供水压力
        /// </summary>
        public decimal FWaterPa { get; set; }
        /// <summary>
        /// 进水口数量
        /// </summary>
        public int FEnterWNum { get; set; }
        /// <summary>
        /// 出水口数量
        /// </summary>
        public int FExitWNum { get; set; }
        /// <summary>
        /// 蓄水容积
        /// </summary>
        public decimal FWaterM3 { get; set; }
        /// <summary>
        /// 转压方式
        /// </summary>
        public string FRotaryPa { get; set; }
        /// <summary>
        /// 进口管径
        /// </summary>
        public string FInDiameter { get; set; }
        /// <summary>
        /// 出口管径
        /// </summary>
        public string FOutDiameter { get; set; }
        /// <summary>
        /// 监控地址
        /// </summary>
        public string FJKLink { get; set; }
        public string FNote { get; set; }
        public int FCreateUser { get; set; }
        public Nullable<DateTime> FCreateDate { get; set; }
        public Nullable<int> FUpdUser { get; set; }
        public Nullable<DateTime> FUpdDate { get; set; }
        public Nullable<int> FDelUser { get; set; }
        public Nullable<DateTime> FDelDate { get; set; }
        public int FIsDelete { get; set; }
        public Guid? FMarkerID { get; set; }
        public virtual Panda_Customer Panda_Customer { get; set; }

    }
}