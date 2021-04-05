using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using AspNet.Dev.Pkg.Infrastructure.Unit;
using AspNet.Dev.Pkg.Infrastructure.Util;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Dev.Pkg.Infrastructure.Application
{
    public class QueryApplication<T, GetT, CreateT> : CrudApplication<T, GetT, CreateT>, IQueryApplication<T, GetT, CreateT>
    where T : class, IBaseEntity
    where GetT : IBaseGet<T>
    where CreateT : IBaseCreate
    {
        public QueryApplication(IRepository<T> re) : base(re)
        { }

        public new virtual IQueryable<T> GetQuery(GetT input) => input.GetExpression(Repository.WhereIf(true, item => true));
        // public virtual async Task<ICollection<T>> FindQueryAsync(GetT input)
        // {
        //     return await Repository.FindQuery(GetQuery(input)).ToListAsync();
        // }
        // public virtual async Task<PageData<T>> FindPageAsync(GetT input, PageRequest page)
        // {
        //     return await Repository.FindPageAsync(GetQuery(input), page);
        // }
    }
    public class QueryApplication<T, Return, GetT, CreateT> : QueryApplication<T, GetT, CreateT>, IQueryApplication<T, Return, GetT, CreateT>
    where T : class, IBaseEntity
    where Return : IBaseReturn
    where GetT : IBaseGet<T>
    where CreateT : IBaseCreate
    {
        public QueryApplication(IRepository<T> re) : base(re) { }

        public new virtual IQueryable<T> GetQuery(GetT input) => input.GetExpression(Repository.WhereIf(true, item => true));

        public new async Task<PageData<Return>> FindPageAsync(GetT input, PageRequest page) => PageData<Return>.GenPageData<T>(await Repository.FindPageAsync(GetQuery(input), page));

        public new async Task<ICollection<Return>> FindQueryAsync(GetT input) => _mapper.Map<ICollection<Return>>(await Repository.FindQuery(GetQuery(input)).ToListAsync());

        public new async Task<Return> AddAsync(CreateT entity) => _mapper.Map<Return>(await Repository.AddAsync(_mapper.Map<T>(entity)));

        public new async Task<Return> FindAsync(Guid id) => _mapper.Map<Return>(await Repository.FindByIDAsync(id));
    }
}
