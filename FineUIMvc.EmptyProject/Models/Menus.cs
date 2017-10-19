using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineUIMvc.PumpMVC.Models
{
    public class Menus : ICustomTree, IKeyID, ICloneable
    {
        [Key]
        public int ID { get; set; }
        [NotMapped]
        public string SFD { set; get; }

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
        public int have_child { get; set; }
        public int ViewPowerID { get; set; }

        public virtual Menus Parent { get; set; }

        public virtual ICollection<Menus> Children { get; set; }



        public virtual Power ViewPower { get; set; }




        /// <summary>
        /// 菜单在树形结构中的层级（从0开始）
        /// </summary>
        [NotMapped]
        public int TreeLevel { get; set; }

        /// <summary>
        /// 是否可用（默认true）,在模拟树的下拉列表中使用
        /// </summary>
        [NotMapped]
        public bool Enabled { get; set; }

        /// <summary>
        /// 是否叶子节点（默认true）
        /// </summary>
        [NotMapped]
        public bool IsTreeLeaf { get; set; }

        public int PowerID { get; set; }
        public string PowerName { get; set; }
        public string PowerGroupName { get; set; }
        public string PowerTitle { get; set; }
        public string PowerRemark { get; set; }

        public object Clone()
        {
            Menus menu = new Menus { 
                ID = ID,
                Name = Name,
                ImageUrl = ImageUrl,
                NavigateUrl = NavigateUrl,
                Remark = Remark,
                SortIndex = SortIndex,
                TreeLevel = TreeLevel,
                Enabled = Enabled,
                IsTreeLeaf = IsTreeLeaf
            };
            return menu;
        }
    }
}