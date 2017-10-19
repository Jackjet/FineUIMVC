using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FineUIMvc.PumpMVC.Models
{
  
    [Table("BASE_SHUICHANG_JZ")]
    public class BASE_SHUICHANG_JZ
    {
        /// <summary>
        /// 水厂机组ID
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        public Guid ShuiChangId { get; set; }
        /// <summary>
        /// DTU编号
        /// </summary>
        public string FDTUCode { get; set; }
        /// <summary>
        /// 泵房机组名称
        /// </summary>
        public string FName { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string MachineType { get; set; }
    
   
        /// <summary>
        /// 采集周期
        /// </summary>
        public int CollectPeriod { get; set; }
        /// <summary>
        /// 采集长度
        /// </summary>
        public int CollectLength { get; set; }
        /// <summary>
        /// 读取模式
        /// </summary>
        /// <summary>
        /// 读取模式
        /// </summary>
        public string ReadMode { get; set; }

        public Nullable<int> FCreateUser { get; set; }
        public Nullable<System.DateTime> FCreateDate { get; set; }
        public Nullable<int> FUpdUser { get; set; }
        public Nullable<System.DateTime> FUpdDate { get; set; }
        public int FIsDelete { get; set; }
        //public virtual Dtu_Base Dtu_Base { get; set; }
        public virtual AddressScheme AddressScheme { get; set; }

    }
}