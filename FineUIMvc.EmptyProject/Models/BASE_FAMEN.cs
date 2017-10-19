using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FineUIMvc.PumpMVC.Models
{
    [Table("BASE_FAMEN")]
    public class BASE_FAMEN
    {
        [Key]
        public Guid id { get; set; }
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
        public string FDeviceType { get; set; }
        /// <summary>
        /// 操作方式
        /// </summary>
        public string FOperationMode { get; set; }
        /// <summary>
        /// 通讯方式
        /// </summary>
        public string FCommunicationMode { get; set; }
        /// <summary>
        /// 阀门功能
        /// </summary>
        public string FValveFunction { get; set; }
        /// <summary>
        /// 排水方式
        /// </summary>
        public string FDrainageMethod { get; set; }

        /// <summary>
        /// 阀门转向
        /// </summary>
        public string FValveSteering { get; set; }
        /// <summary>
        /// 埋设形式
        /// </summary>
        public string FBuriedForm { get; set; }
        /// <summary>
        /// 地面标高
        /// </summary>
        public decimal FGroundElevation { get; set; }
        /// <summary>
        /// 所在井室
        /// </summary>
        public string FWellRoom { get; set; }
        /// <summary>
        /// 所在管线
        /// </summary>
        public string FLocationPipeline { get; set; }      
        /// <summary>
        /// 设备状态
        /// </summary>
        public string FEquipmentState { get; set; }
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
        public int FCreateUser { get; set; }
        public Nullable<DateTime> FCreateDate { get; set; }
        public Nullable<int> FUpdateUser { get; set; }
        public Nullable<DateTime> FUpdateDate { get; set; }
        public int FIsDelete { get; set; }
        public Guid? FMarkerID { get; set; }
        public virtual Panda_Customer Panda_Customer { get; set; }
        public virtual AddressScheme AddressScheme { get; set; }
    }
}