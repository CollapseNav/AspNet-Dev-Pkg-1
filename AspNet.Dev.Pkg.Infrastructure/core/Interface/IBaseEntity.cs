using System;
using Microsoft.AspNetCore.Identity;

namespace AspNet.Dev.Pkg.Infrastructure.Interface
{
    public interface IBaseEntity
    {
        void Init();
        void Update();
        void SoftDelete();
        void Init(IdentityUser<Guid> user = null);
        void Update(IdentityUser<Guid> user = null);
        void SoftDelete(IdentityUser<Guid> user = null);


        IBaseEntity Entity();
    }
}
