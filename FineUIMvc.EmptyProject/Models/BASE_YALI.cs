using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineUIMvc.PumpMVC.Models
{
    /// <summary>
    /// 压力
    /// </summary>
     [Table("BASE_YALI")]
    public class BASE_YALI
    {
        [Key]
        public Guid id { get; set; }
        //public int FCustomerID { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string FDTUCode { get; set; }
        /// <summary>
        /// 安装坐标
        /// </summary>
        public string FLngLat { get; set; }
        /// <summary>
        /// 安装地址
        /// </summary>
        public string FMapAddress { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string FName { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public string FBrand { get; set; }
        /// <summary>
        /// 材质
        /// </summary>
        public string FMaterial { get; set; }
        /// <summary>
        /// 口径
        /// </summary>
        public string FCaliber { get; set; }
        /// <summary>
        /// 设备类型
        /// </summary>
        public string FEQuiType { get; set; }
        /// <summary>
        /// 安装方式
        /// </summary>
        public string FInstallMode { get; set; }
        /// <summary>
        /// 通讯方式
        /// </summary>
        public string FCommunicationMode { get; set; }
        /// <summary>
        /// 抄表方式
        /// </summary>
        public string FReadMeterMode { get; set; }
        /// <summary>
        /// 埋设形式
        /// </summary>
        public string FBuriedMode { get; set; }
        /// <summary>
        /// 地面标高
        /// </summary>
        public decimal FGroundHeigh { get; set; }
        /// <summary>
        /// 所在井室
        /// </summary>
        public string FWell { get; set; }
        /// <summary>
        /// 所在管线
        /// </summary>
        public string FPipeline { get; set; }
        /// <summary>
        /// 进口阀门
        /// </summary>
        public string FEnterValve { get; set; }
        /// <summary>
        /// 出口阀门
        /// </summary>
        public string FExitValve { get; set; }
        /// <summary>
        /// 设备状态
        /// </summary>
        public string FEQuiState { get; set; }
        /// <summary>
        /// 保养周期
        /// </summary>
        public int FBYCycle { get; set; }
        /// <summary>
        /// 更换周期
        /// </summary>
        public int FGHCycle { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string FNote { get; set; }
        /// <summary>
        /// 压力上限
        /// </summary>
        public string FMpaUp { get; set; }
         /// <summary>
         /// 压力下限
         /// </summary>
        public string FMpaDown { get; set; }
        public int FCreateUser { get; set; }
        public Nullable<DateTime> FCreateDate { get; set; }
        public Nullable<int> FUpdUser { get; set; }
        public Nullable<DateTime> FUpdDate { get; set; }
        public Nullable<int> FDelUser { get; set; }
        public Nullable<DateTime> FDelDate { get; set; }
        public int FIsDelete { get; set; }
        public Guid? FMarkerID { get; set; }
        public virtual Panda_Customer Panda_Customer { get; set; }
        public virtual AddressScheme AddressScheme { get; set; }
    }
}