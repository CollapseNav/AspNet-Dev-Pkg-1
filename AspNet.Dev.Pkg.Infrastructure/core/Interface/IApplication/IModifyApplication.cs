using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Dev.Pkg.Infrastructure.Interface
{
    public interface IModifyApplication<T, CreateT> : IApplication
    where T : IBaseEntity
    where CreateT : IBaseCreate
    {
        Task<int> SaveChangesAsync();
        int SaveChanges();
        Task<T> AddAsync(CreateT entity);
        Task<Target> AddAsync<Target>(CreateT entity);
        Task<int> AddRangeAsync(ICollection<CreateT> entitys);
        Task DeleteAsync(Guid id);
        Task<int> DeleteRangeAsync(ICollection<Guid> ids);
        Task UpdateAsync(Guid id, CreateT entity);
        DbContext GetDbContext();
    }
    public interface IModifyApplication<T, CreateT, ReturnT> : IModifyApplication<T, CreateT>
    where T : IBaseEntity
    where CreateT : IBaseCreate
    where ReturnT : IBaseReturn
    {
        new Task<ReturnT> AddAsync(CreateT entity);
    }
}
