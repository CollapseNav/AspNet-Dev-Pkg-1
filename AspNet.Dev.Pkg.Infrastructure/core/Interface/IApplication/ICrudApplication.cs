using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Dev.Pkg.Infrastructure.Unit;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Dev.Pkg.Infrastructure.Interface
{
    public interface ICrudApplication<T, GetT, CreateT> : IApplication
    where T : IBaseEntity
    where GetT : IBaseGet<T>
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


        Task<T> FindAsync(Guid id);
        Task<Target> FindAsync<Target>(Guid id);
        IQueryable<T> GetQuery(GetT input);
        Task<IReadOnlyCollection<T>> FindQueryAsync(GetT input);
        Task<IReadOnlyCollection<T>> FindByIdsAsync(ICollection<Guid> ids);
        Task<IReadOnlyCollection<Target>> FindQueryAsync<Target>(GetT input);
        Task<PageData<T>> FindPageAsync(GetT input, PageRequest page);
        Task<PageData<Target>> FindPageAsync<Target>(GetT input, PageRequest page);
    }
}
