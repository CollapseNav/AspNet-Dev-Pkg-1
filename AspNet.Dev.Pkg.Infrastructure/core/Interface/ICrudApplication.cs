using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AspNet.Dev.Pkg.Infrastructure.Dto;
using AspNet.Dev.Pkg.Infrastructure.Unit;

namespace AspNet.Dev.Pkg.Infrastructure.Interface
{
    public interface ICrudApplication<T, Return, GetT, CreateT> : IBaseApplication<T, Return, CreateT>
    where T : IBaseEntity
    where Return : IBaseReturn
    where GetT : IBaseGet
    where CreateT : IBaseCreate
    {
        IQueryable<T> GetQuery(GetT input);
        Task<ICollection<Return>> FindQueryAsync(GetT input);
        Task<PageData<Return>> FindPageAsync(GetT input, PageRequest page);
    }



    public interface ICrudApplication<T, GetT, CreateT> : IBaseApplication<T, CreateT>
    where T : IBaseEntity
    where GetT : IBaseGet
    where CreateT : IBaseCreate
    {
        IQueryable<T> GetQuery(GetT input);
        Task<ICollection<T>> FindQueryAsync(GetT input);
        Task<ICollection<Target>> FindQueryAsync<Target>(GetT input);
        Task<PageData<T>> FindPageAsync(GetT input, PageRequest page);
        Task<PageData<Target>> FindPageAsync<Target>(GetT input, PageRequest page);
    }
}
