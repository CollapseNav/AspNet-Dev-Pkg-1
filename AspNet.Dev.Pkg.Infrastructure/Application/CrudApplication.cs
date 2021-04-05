using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using AspNet.Dev.Pkg.Infrastructure.Unit;
using AspNet.Dev.Pkg.Infrastructure.Util;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Dev.Pkg.Infrastructure.Application
{
    public class CrudApplication<T, Return, GetT, CreateT> : BaseApplication<T, Return, CreateT>, ICrudApplication<T, Return, GetT, CreateT>
    where T : class, IBaseEntity
    where Return : IBaseReturn
    where GetT : IBaseGet
    where CreateT : IBaseCreate
    {
        public CrudApplication(IRepository<T> re) : base(re) { }

        public async Task<PageData<Return>> FindPageAsync(GetT input, PageRequest page) => PageData<Return>.GenPageData<T>(await Repository.FindPageAsync(GetQuery(input), page));

        public async Task<ICollection<Return>> FindQueryAsync(GetT input) => _mapper.Map<ICollection<Return>>(await Repository.FindQuery(GetQuery(input)).ToListAsync());

        public IQueryable<T> GetQuery(GetT input) => Repository.WhereIf(true, item => true);

    }
    public class CrudApplication<T, GetT, CreateT> : BaseApplication<T, CreateT>, ICrudApplication<T, GetT, CreateT>
    where T : class, IBaseEntity
    where GetT : IBaseGet
    where CreateT : IBaseCreate
    {
        public CrudApplication(IRepository<T> re) : base(re)
        { }

        public virtual IQueryable<T> GetQuery(GetT input) => Repository.WhereIf(true, item => true);

        public virtual async Task<ICollection<T>> FindQueryAsync(GetT input) => await Repository.FindQuery(GetQuery(input)).ToListAsync();

        public virtual async Task<PageData<T>> FindPageAsync(GetT input, PageRequest page) => await Repository.FindPageAsync(GetQuery(input), page);

        public virtual async Task<ICollection<Target>> FindQueryAsync<Target>(GetT input) => _mapper.Map<ICollection<Target>>(await FindQueryAsync(input));

        public virtual async Task<PageData<Target>> FindPageAsync<Target>(GetT input, PageRequest page) => PageData<Target>.GenPageData<T>(await FindPageAsync(input, page));
    }
}
