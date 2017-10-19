using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineUIMvc.PumpMVC.Models
{
    [Table("BaseDA")]
    public class Panda_PumpDA
    {
        [Key]
        public int ID { get; set; }
        public string BaseId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public string FileType2 { get; set; }
        public string uploadPageType { get; set; }
        public string FileSize { get; set; }
        public DateTime UpDateTime { get; set; }
        public string FPageSource { get; set; }

    }
}