using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AspNet.Dev.Pkg.Infrastructure.Unit;
using System;

namespace AspNet.Dev.Pkg.Infrastructure.Interface
{
    public interface IReadonlyApplication<T, GetT> : IApplication
    where T : IBaseEntity
    where GetT : IBaseGet<T>
    {
        Task<T> FindAsync(Guid id);
        Task<Target> FindAsync<Target>(Guid id);
        IQueryable<T> GetQuery(GetT input);
        Task<IReadOnlyCollection<T>> FindQueryAsync(GetT input);
        Task<IReadOnlyCollection<T>> FindByIdsAsync(ICollection<Guid> ids);
        Task<IReadOnlyCollection<Target>> FindQueryAsync<Target>(GetT input);
        Task<PageData<T>> FindPageAsync(GetT input, PageRequest page);
        Task<PageData<Target>> FindPageAsync<Target>(GetT input, PageRequest page);
    }
    public interface IReadonlyApplication<T, GetT, ReturnT> : IReadonlyApplication<T, GetT>, IApplication
    where T : IBaseEntity
    where GetT : IBaseGet<T>
    where ReturnT : IBaseReturn
    {
        new Task<T> FindAsync(Guid id);
        new Task<IReadOnlyCollection<ReturnT>> FindQueryAsync(GetT input);
        new Task<PageData<ReturnT>> FindPageAsync(GetT input, PageRequest page);
    }
}
