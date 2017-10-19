using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineUIMvc.PumpMVC.Models
{
    public class A_U_DEP 
    {
     
      //  public int ID { get; set; }
        [Key]
        public  string U8number { get; set; }

        [Required, StringLength(30)]
        public string Daqu { get; set; }

        [StringLength(30)]
        public string Fengongsi { get; set; }

        [StringLength(5)]
        public string U8Name { get; set; }


        public virtual ICollection<Dept> Dept { get; set; }
    }
}