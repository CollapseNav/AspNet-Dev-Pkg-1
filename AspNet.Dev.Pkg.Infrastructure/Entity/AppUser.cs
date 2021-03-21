using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Elong.Ids.Models
{
    public enum UserType
    {
        Student, Parent, Teacher, Others
    }

    [Table("Account")]
    public class AppUser : IdentityUser<Guid>
    {
        [NotMapped]
        public string Pwd { get; set; }
        /// <summary>
        /// 电子号
        /// </summary>
        public string UserAccount { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        [MaxLength(50)]
        public string CardNo { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }
        /// <summary>
        /// 学号或者工号
        /// </summary>
        [MaxLength(50)]
        public string SchoolNo { get; set; }
        // public UserType UserType { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool? IsDisable { get; set; }
        /// <summary>
        /// 是否报名注册学生
        /// </summary>
        /// <value></value>
        public bool? IsSignUp { get; set; }
        public Guid? TypeId { get; set; }
        public string TypeCode { get; set; }
        /// <summary>
        /// 最近一次登录的时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? LastModifierId { get; set; }

        public void Init()
        {
            Id = Guid.NewGuid();
            CreationTime = DateTime.Now;
            IsDeleted = false;
            IsDisable = false;
            LockoutEnabled = true;
            if (!TypeId.HasValue)
                throw new Exception();
        }
    }
}
