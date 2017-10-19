using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineUIMvc.PumpMVC.Models
{
      [Table("LunXunParam")]
    public class LunXunParam
    {
        [Key]
        public int ID { get; set; }
        public int FField { get; set; }
        public Guid BaseID { get; set; }
        public int UserID { get; set; }
    }
}