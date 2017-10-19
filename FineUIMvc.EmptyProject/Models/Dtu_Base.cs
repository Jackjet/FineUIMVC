using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineUIMvc.PumpMVC.Models
{
    [Table("Dtu_Base")]
    public class Dtu_Base
    {
        //public Dtu_Base()
        //{
        //    this.Panda_PumpJZ = new HashSet<Panda_PumpJZ>();
        //}

        [Key]
        public int B_ID { get; set; }
        public string B_Number { get; set; }
        public string B_Factory { get; set; }
        public string B_Type { get; set; }
        public string B_Mode { get; set; }
        public string B_IsUsed { get; set; }
        //public virtual ICollection<Panda_PumpJZ> Panda_PumpJZ { get; set; }
    }
}