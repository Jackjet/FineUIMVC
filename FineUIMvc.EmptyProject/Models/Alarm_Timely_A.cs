using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FineUIMvc.PumpMVC.Models
{
    public class Alarm_Timely_A
    {
        public int id { get; set; }
        public int ParamID { get; set; }
        public Guid PumpID { get; set; }
        public Guid BaseID { get; set; }
        public int FCustomerID { get; set; }
        public string PCompanyNumber { get; set; }
        public string FName { get; set; }
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