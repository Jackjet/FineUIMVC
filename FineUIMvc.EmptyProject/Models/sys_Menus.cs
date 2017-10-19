using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineUIMvc.PumpMVC.Models
{
    [Table("sys_Menus")]
    public class sys_Menus
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }

        [StringLength(200)]
        public string ImageUrl { get; set; }

        [StringLength(200)]
        public string NavigateUrl { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        [Required]
        public int SortIndex { get; set; }


        public int ParentID { get; set; }

        public int ViewPowerID { get; set; }
    }
}