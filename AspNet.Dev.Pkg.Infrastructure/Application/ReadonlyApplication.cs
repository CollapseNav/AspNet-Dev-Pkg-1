using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using AspNet.Dev.Pkg.Infrastructure.Unit;
using AspNet.Dev.Pkg.Infrastructure.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Dev.Pkg.Infrastructure.Application
{
    public class ReadonlyApplication<T, GetT> : Application, IReadonlyApplication<T, GetT>, IApplication
    where T : class, IBaseEntity
    where GetT : IBaseGet<T>
    {
        protected IReadonlyRepository<T> Repository;
        public ReadonlyApplication() : base()
        {
            Repository = ServiceGet.GetProvider()?.GetService<IReadonlyRepository<T>>();
        }
        public virtual async Task<T> FindAsync(Guid id) => await Repository.FindAsync(id);
        public virtual async Task<Target> FindAsync<Target>(Guid id) => _mapper.Map<Target>(await FindAsync(id));
        public virtual IQueryable<T> GetQuery(GetT input) => input.GetExpression(Repository.WhereIf(true, item => true));
        public virtual async Task<IReadOnlyCollection<T>> FindQueryAsync(GetT input) => await Repository.FindQuery(GetQuery(input)).ToListAsync();
        public virtual async Task<IReadOnlyCollection<T>> FindByIdsAsync(ICollection<Guid> ids) => await Repository.FindQuery(item => ids.Contains(item.Id.Value)).ToListAsync();
        public virtual async Task<PageData<T>> FindPageAsync(GetT input, PageRequest page) => await Repository.FindPageAsync(GetQuery(input), page);
        public virtual async Task<IReadOnlyCollection<Target>> FindQueryAsync<Target>(GetT input) => _mapper.Map<IReadOnlyCollection<Target>>(await FindQueryAsync(input));
        public virtual async Task<PageData<Target>> FindPageAsync<Target>(GetT input, PageRequest page) => PageData<Target>.GenPageData<T>(await FindPageAsync(input, page));
    }


    public class ReadonlyApplication<T, GetT, ReturnT> : ReadonlyApplication<T, GetT>, IReadonlyApplication<T, GetT, ReturnT>, IApplication
    where T : class, IBaseEntity
    where GetT : IBaseGet<T>
    where ReturnT : IBaseReturn
    {
        public ReadonlyApplication() : base() { }
        public virtual new async Task<ReturnT> FindAsync(Guid id) => (await Repository.FindAsync(id)).Map<ReturnT>();
        public virtual new async Task<IReadOnlyCollection<ReturnT>> FindQueryAsync(GetT input) => (await Repository.FindQuery(GetQuery(input)).ToListAsync()).Maps<ReturnT>() as IReadOnlyCollection<ReturnT>;
        public virtual new async Task<PageData<ReturnT>> FindPageAsync(GetT input, PageRequest page) => (await Repository.FindPageAsync(GetQuery(input), page)).Map<PageData<ReturnT>>();
    }
}
