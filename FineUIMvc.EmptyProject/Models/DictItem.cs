using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineUIMvc.PumpMVC.Models
{
    [Table("sys_dictItems")]
    public partial class sys_dictItems
     {
        [Key]
        public int FID { get; set; }
        public int FDictID { get; set; }
        public string FValue { get; set; }
        public string FName { get; set; }
        public string FParentValue { get; set; }
        public string FDescription { get; set; }
    }
}