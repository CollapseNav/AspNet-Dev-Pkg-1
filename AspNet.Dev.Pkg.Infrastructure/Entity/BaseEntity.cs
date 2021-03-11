using System;
using System.ComponentModel.DataAnnotations;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using Microsoft.AspNetCore.Identity;

namespace AspNet.Dev.Pkg.Infrastructure.Entity
{
    public class BaseEntity : IBaseEntity
    {
        [Key]
        public Guid? Id { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? LastModifierId { get; set; }
        public virtual void Init()
        {
            Id = Guid.NewGuid();
            IsDeleted ??= false;
            CreationTime = DateTime.Now;
            LastModificationTime = DateTime.Now;
        }
        public virtual void Init(IdentityUser<Guid> user = null)
        {
            if (user != null)
                CreatorId = user.Id;
            Init();
        }

        public virtual void SoftDelete()
        {
            IsDeleted = true;
        }
        public virtual void SoftDelete(IdentityUser<Guid> user = null)
        {
            if (user == null)
                LastModifierId = user.Id;
            SoftDelete();
        }

        public virtual IBaseEntity Entity()
        {
            if (IsDeleted ?? false) return null;
            return this;
        }
        public virtual void Update()
        {
            LastModificationTime = DateTime.Now;
        }
        public virtual void Update(IdentityUser<Guid> user = null)
        {
            if (user != null)
                CreatorId = user.Id;
            Update();
        }
    }
}
