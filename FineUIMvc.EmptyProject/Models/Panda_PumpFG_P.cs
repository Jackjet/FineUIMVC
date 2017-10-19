using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineUIMvc.PumpMVC.Models
{
    [Table("Panda_PumpFG_P")]
    public class Panda_PumpFG_P
    {
        /// <summary>
        /// 泵房id
        /// </summary>
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 泵房编号
        /// </summary>
        public int GroupID { get; set; }
        /// <summary>
        /// 泵房名
        /// </summary>
        public string PumpID { get; set; }
    }
}