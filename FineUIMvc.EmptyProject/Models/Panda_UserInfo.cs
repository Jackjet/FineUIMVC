using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FineUIMvc.PumpMVC.Models
{
      [Table("Panda_UserInfo")]
    public class Panda_UserInfo
    {
          [Key]
          public int ID { get; set; }
          /// <summary>
          /// 分公司编码
          /// </summary>
          public string FCompanyNumber { get; set; }
          /// <summary>
          /// 用户名
          /// </summary>
          public string UserName { get; set; }
          /// <summary>
          /// 密码
          /// </summary>
          public string UserPwd { get; set; }
          /// <summary>
          /// 性别
          /// </summary>
          public string UserSex { get; set; }
          /// <summary>
          /// 生日
          /// </summary>
          public Nullable<System.DateTime> UserBirthday { get; set; }
          /// <summary>
          /// 电话
          /// </summary>
          public string UserTel { get; set; }
          /// <summary>
          /// 手机
          /// </summary>
          public string UserMob { get; set; }
          /// <summary>
          /// 邮箱
          /// </summary>
          public string UserMail { get; set; }
          /// <summary>
          /// 是否禁用
          /// </summary>
          public string UserEnabledisable { get; set; }
          /// <summary>
          /// 备注
          /// </summary>
          public string UserRemark { get; set; }
          /// <summary>
          /// 泵房组id
          /// </summary>
          //public int UserPumpGroup { get; set; }
          /// <summary>
          /// 客户id
          /// </summary>
          //public int FCustomerID { get; set; }
          /// <summary>
          /// 用户类型
          /// </summary>
          public string UserType { get; set; }
          /// <summary>
          /// 是否其他用户
          /// </summary>
          public string IsOther { get; set; }
          public Nullable<int> FCreateUser { get; set; }
          public Nullable<System.DateTime> FCreateDate { get; set; }
          public Nullable<int> FUpdUser { get; set; }
          public Nullable<System.DateTime> FUpdDate { get; set; }
          public Nullable<int> FDelUser { get; set; }
          public Nullable<System.DateTime> FDelDate { get; set; }
          public int FIsDelete { get; set; }

          public virtual Panda_PGroup Panda_PGroup { get; set; }
          public virtual Panda_Customer Panda_Customer { get; set; }
    }
}