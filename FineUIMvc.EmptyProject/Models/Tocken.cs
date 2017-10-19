using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FineUIMvc.PumpMVC.Models
{
    [Table("Tocken")]
    public class Tocken
    {
        [Key]
        public int ID { get; set; }
        public string TockenID { get; set; }
        public System.DateTime UpdateTime { get; set; }
        public string UserName { get; set; }
        public string aaa { get; set; }
    }
}