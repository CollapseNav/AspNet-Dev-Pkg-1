using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using AspNet.Dev.Pkg.Infrastructure.Util;
using Microsoft.AspNetCore.Identity;

namespace AspNet.Dev.Pkg.Infrastructure.Entity
{
    public class BaseEntity<TKey> : BaseEntity, IBaseEntity<TKey>
    {
        [Key]
        public new virtual TKey Id { get; set; }
        public new virtual TKey CreatorId { get; set; }
        public new virtual TKey LastModifierId { get; set; }
    }
    public class BaseEntity : IBaseEntity<Guid?>
    {
        [Key]
        public virtual Guid? Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreationTime { get; set; }
        public virtual Guid? CreatorId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public virtual Guid? LastModifierId { get; set; }

        public virtual void Init()
        {
            Id = Guid.NewGuid();
            CreationTime = DateTime.Now;
            LastModificationTime = DateTime.Now;
            InitModifyId();
        }

        private void InitModifyId()
        {
            var curUser = CurrentUser.User;
            LastModifierId = curUser?.Id;
        }

        public virtual void SoftDelete()
        {
            IsDeleted = true;
            InitModifyId();
        }

        public virtual IBaseEntity Entity()
        {
            if (IsDeleted) return null;
            return this;
        }
        public virtual void Update()
        {
            LastModificationTime = DateTime.Now;
            InitModifyId();
        }
    }
}
