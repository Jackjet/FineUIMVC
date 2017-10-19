using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FineUIMvc.PumpMVC.Models
{
     [Table("PumpVideoQuipment")]
    public class Panda_PumpVQ
    {

        /// <summary>
        /// 泵房机组ID
        /// </summary>
        [Key]
        public int ID { get; set; }
        public Guid PumpId { get; set; }
        /// <summary>
        /// DTU编号
        /// </summary>
        //public int DTUCode { get; set; }
        /// <summary>
        /// 泵房机组名称
        /// </summary>
        //public string PumpJZName { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string QuipmentType { get; set; }
        /// <summary>
        /// 主泵泵数
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 辅泵泵数
        /// </summary>
        public string Brand { get; set; }
        /// <summary>
        /// 排水泵数
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 是否报警
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 所在区域
        /// </summary>
        public string PassWord { get; set; }
        public string IP { get; set; }
        public Nullable<int> FOrderBy { get; set; }
        public Nullable<int> Port { get; set; }
        /// <summary>
        /// 地址表
        /// </summary>
        //public int PumpJZAddressList { get; set; }
        public string Mark { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateOn { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateOn { get; set; }

        public bool IsActive { get; set; }

        public string Rtmp { get; set; }

        public string Hls { get; set; }
    }
}