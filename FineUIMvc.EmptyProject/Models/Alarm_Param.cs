using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineUIMvc.PumpMVC.Models
{
    [Table("Alarm_Param")]
    public class Alarm_Param
    {
        [Key]
        public int id { get; set; }
        public int FMarkerType { get; set; }
        public string FKey { get; set; }
        public string FMsg { get; set; }
        public int FLev { get; set; }
    }
}