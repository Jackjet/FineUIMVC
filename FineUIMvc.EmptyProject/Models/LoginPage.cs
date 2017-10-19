using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FineUIMvc.PumpMVC.Models
{
    [Table("LoginPage")]
    public class LoginPage
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 域名
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 文件夹名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// LOG文字
        /// </summary>
        public string Title { get; set; }
        public int FCreateUser { get; set; }
        public Nullable<DateTime> FCreateDate { get; set; }
        public Nullable<int> FUpdateUser { get; set; }
        public Nullable<DateTime> FUpdateDate { get; set; }
        public int FIsDelete { get; set; }

    }
}