using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AspNet.Dev.Pkg.Infrastructure.Dto;
using AspNet.Dev.Pkg.Infrastructure.Unit;

namespace AspNet.Dev.Pkg.Infrastructure.Interface
{
    public interface IQueryApplication<T, Return, GetT, CreateT> : IBaseApplication<T, Return, CreateT>
    where T : IBaseEntity
    where Return : BaseReturn
    where GetT : IBaseGet<T>
    where CreateT : BaseCreate
    {
        IQueryable<T> GetQuery(GetT input);
        Task<ICollection<Return>> FindQueryAsync(GetT input);
        Task<PageData<Return>> FindPageAsync(GetT input, PageRequest page);
    }

    public interface IQueryApplication<T, GetT, CreateT> : IBaseApplication<T, CreateT>
    where T : IBaseEntity
    where GetT : IBaseGet<T>
    where CreateT : BaseCreate
    {
        IQueryable<T> GetQuery(GetT input);
        Task<ICollection<T>> FindQueryAsync(GetT input);
        Task<PageData<T>> FindPageAsync(GetT input, PageRequest page);
    }
}
