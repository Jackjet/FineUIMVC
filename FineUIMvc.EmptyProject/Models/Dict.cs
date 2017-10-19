using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineUIMvc.PumpMVC.Models
{
    [Table("sys_dict")]
    public class sys_dict
    {

        /// <summary>
        /// 
        /// </summary>        

        [Key]
        public int FDictID { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        public string FName { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        public string FDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        public string FNote { get; set; }


    }
}