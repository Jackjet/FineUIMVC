using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineUIMvc.PumpMVC.Models
{
    /// <summary>
    /// 加压站
    /// </summary>
    [Table("BASE_JIAYAZHAN")]
    public class BASE_JIAYAZHAN
    {
        [Key]
        public Guid ID { get; set; }
        /// <summary>
        /// 加压站编号
        /// </summary>
        public string FCode { get; set; }
        /// <summary>
        /// 加压站名称
        /// </summary>
        public string FName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FCompanyNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FProvince { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FCity { get; set; }
        /// <summary>
        /// 坐标
        /// </summary>
        public string FLngLat { get; set; }
        /// <summary>
        /// 坐标地址
        /// </summary>
        public string FAddress { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string FXXAddress { get; set; }
        /// <summary>
        /// 监控网址
        /// </summary>
        public string FJKLink { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
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