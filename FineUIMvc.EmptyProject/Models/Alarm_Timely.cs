using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineUIMvc.PumpMVC.Models
{
    [Table("Alarm_Timely")]
    public class Alarm_Timely
    {
        [Key]
        public int id { get; set; }
        public int ParamID { get; set; }
        public Guid BaseID { get; set; }
        public int FMarkerType { get; set; }
        public string FKey { get; set; }
        public string FMsg { get; set; }
        public string FSetMsg { get; set; }
        public int FLev { get; set; }
        public int FStatus { get; set; }
        public int FIsPhone { get; set; }
        public Nullable<DateTime> FAlarmTime { get; set; }

    }
}