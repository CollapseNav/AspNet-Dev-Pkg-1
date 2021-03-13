using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AspNet.Dev.Pkg.Infrastructure.Dto;
using AspNet.Dev.Pkg.Infrastructure.Unit;

namespace AspNet.Dev.Pkg.Infrastructure.Interface
{
    public interface ICrudApplication<T, GetT, CreateT> : IBaseApplication<T, CreateT> where T : IBaseEntity where GetT : IBaseGet where CreateT : BaseCreate
    {
        IQueryable<T> GetQuery(GetT input);
        Task<ICollection<T>> FindAllAsync(GetT input);
        Task<PageData<T>> FindPageAsync(GetT input, PageRequest page);
    }
    public interface IQCrudApplication<T, GetT, CreateT> : ICrudApplication<T, GetT, CreateT> where T : IBaseEntity where GetT : IBaseGet<T> where CreateT : BaseCreate
    {
    }
}
