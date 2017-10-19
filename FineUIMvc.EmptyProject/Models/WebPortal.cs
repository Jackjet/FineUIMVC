using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FineUIMvc.PumpMVC.Models
{
    [Table("WebPortal")]
    public class WebPortal
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 门户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 门户网址
        /// </summary>
        public string Address { get; set; }
        public int? FCreateUser { get; set; }
        public Nullable<DateTime> FCreateDate { get; set; }
        public Nullable<int> FUpdateUser { get; set; }
        public Nullable<DateTime> FUpdateDate { get; set; }
        public int? FIsDelete { get; set; }
    }
}