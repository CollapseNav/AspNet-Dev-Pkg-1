using System;
using Microsoft.AspNetCore.Identity;

namespace AspNet.Dev.Pkg.Infrastructure.Interface
{
    public interface IBaseEntity<TKey> : IBaseEntity
    {
        new TKey Id { get; set; }
        new TKey CreatorId { get; set; }
        new TKey LastModifierId { get; set; }

    }
    public interface IBaseEntity
    {
        Guid? Id { get; set; }
        Guid? CreatorId { get; set; }
        Guid? LastModifierId { get; set; }
        bool IsDeleted { get; set; }
        DateTime? CreationTime { get; set; }
        DateTime? LastModificationTime { get; set; }
        void Init();
        void SoftDelete();
        void Update();
        IBaseEntity Entity();
    }
}
