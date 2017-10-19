using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineUIMvc.PumpMVC.Models
{
    [Table("sys_Roles")]
    public class Role : IKeyID
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Remark { get; set; }
        public int RType { get; set; }
       // public int FCustomerID { get; set; }
        public virtual Panda_Customer Panda_Customer { get; set; }


        //public virtual ICollection<User> Users { get; set; }


        //public virtual ICollection<Power> Powers { get; set; }

        
    }
}