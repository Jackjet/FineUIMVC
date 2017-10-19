using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FineUIMvc.PumpMVC.Models
{
    public class Panda_CustomerTopMenu
    {
        [Key]
        public int ID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<int> TopMenuID { get; set; }
        public string NavigateUrl { get; set; }
        public Nullable<int> FCreateUser { get; set; }
        public Nullable<DateTime> FCreateDate { get; set; }
        public Nullable<int> FUpdateUser { get; set; }
        public Nullable<DateTime> FUpdateDate { get; set; }
        public Nullable<int> FIsDelete { get; set; }
    }
}