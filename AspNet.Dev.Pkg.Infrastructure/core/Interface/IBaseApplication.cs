using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNet.Dev.Pkg.Infrastructure.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Dev.Pkg.Infrastructure.Interface
{
    public interface IBaseApplication
    {
        IdentityUser<Guid> GetCurrentUser();
    }
    public interface IBaseApplication<T, Return, CreateT> : IBaseApplication<T, CreateT>
    where T : IBaseEntity
    where Return : BaseReturn
    where CreateT : BaseCreate
    {
        new Task<Return> AddAsync(CreateT entity);
        new Task<Return> FindAsync(Guid id);
    }
    public interface IBaseApplication<T, CreateT> : IBaseApplication where T : IBaseEntity where CreateT : BaseCreate
    {
        Task<int> SaveChangesAsync();
        int SaveChanges();
        Task<T> AddAsync(CreateT entity);
        Task<int> AddRangeAsync(ICollection<CreateT> entitys);
        Task<T> FindAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task<int> DeleteRangeAsync(ICollection<Guid> ids);
        Task UpdateAsync(Guid id, CreateT entity);
        DbContext GetDbContext();
    }
}
