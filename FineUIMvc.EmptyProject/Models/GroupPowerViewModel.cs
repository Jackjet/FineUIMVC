using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace FineUIMvc.PumpMVC.Models
{
    public class GroupPowerViewModel
    {
        [Display(Name = "分组名称")]
        public string GroupName { get; set; }

        [Display(Name = "权限列表")]
        public JArray Powers { get; set; }
    }
}