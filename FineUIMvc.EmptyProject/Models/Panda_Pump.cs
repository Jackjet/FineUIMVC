using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineUIMvc.PumpMVC.Models
{
    [Table("Panda_Pump")]
    public class Panda_Pump
    {
        /// <summary>
        /// 泵房id
        /// </summary>
        [Key]
        public Guid ID { get; set; }
        /// <summary>
        /// 泵房编号
        /// </summary>
        public string PCode { get; set; }
        /// <summary>
        /// 泵房名
        /// </summary>
        public string PName { get; set; }
        /// <summary>
        /// 自定义泵房名
        /// </summary>
        public string PCustomPName { get; set; }
        /// <summary>
        /// 泵房类型
        /// </summary>
        public string PType { get; set; }
        /// <summary>
        /// 是否重点监控
        /// </summary>
        public string IsFocusMonitoring { get; set; }
        /// <summary>
        /// 客户id
        /// </summary>
        //public int FCustomerID { get; set; }
        /// <summary>
        /// 分公司编号
        /// </summary>
        public string PCompanyNumber { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string PProvince { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string PCity { get; set; }
        /// <summary>
        /// 坐标
        /// </summary>
        public string PLngLat { get; set; }
        /// <summary>
        /// 坐标地址
        /// </summary>
        public string PAddress { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string PXXAddress { get; set; }
        public Nullable<int> FCreateUser { get; set; }
        public Nullable<System.DateTime> FCreateDate { get; set; }
        public Nullable<int> FUpdUser { get; set; }
        public Nullable<System.DateTime> FUpdDate { get; set; }
        public Nullable<int> FDelUser { get; set; }
        public Nullable<System.DateTime> FDelDate { get; set; }
        public int FIsDelete { get; set; }
        public Nullable<int> TankIsSharing { get; set; }
        public Nullable<int> PumpSoaking { get; set; }
        public Nullable<int> Warning { get; set; }
        public Nullable<int> WaterQualityDetection { get; set; }
        public Nullable<int> ControlValve { get; set; }
        public Nullable<int> WaterTankSterilizer { get; set; }
        public Nullable<int> PumpLocation { get; set; }
        public string MasterControlPLCIP { get; set; }
        public Guid? FMarkerID { get; set; }
        //安装日期
        public Nullable<System.DateTime> InstallDate { get; set; }
        //验收日期
        public Nullable<System.DateTime> AcceptanceDate { get; set; }
        //供水楼层
        public Nullable<int> WaterFloor { get; set; }
        //换药周期
        public Nullable<int> DressingCycle { get; set; }
        public virtual Panda_Customer Panda_Customer { get; set; }
    }
}