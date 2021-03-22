using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Dev.Pkg.Infrastructure.Dto;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using AspNet.Dev.Pkg.Infrastructure.Unit;
using AspNet.Dev.Pkg.Infrastructure.Util;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Dev.Pkg.Infrastructure.Application
{
    public class CrudApplication<T, Return, GetT, CreateT> : BaseApplication<T, Return, CreateT>, ICrudApplication<T, Return, GetT, CreateT>
    where T : class, IBaseEntity
    where Return : BaseReturn
    where GetT : IBaseGet
    where CreateT : BaseCreate
    {
        public CrudApplication(IRepository<T> re) : base(re) { }

        public async Task<PageData<Return>> FindPageAsync(GetT input, PageRequest page)
        {
            var result = await Repository.FindPageAsync(GetQuery(input), page);
            return new PageData<Return>
            {
                Total = result.Total,
                Data = _mapper.Map<ICollection<Return>>(result.Data)
            };
        }

        public async Task<ICollection<Return>> FindQueryAsync(GetT input)
        {
            return _mapper.Map<ICollection<Return>>(await Repository.FindQuery(GetQuery(input)).ToListAsync());
        }

        public IQueryable<T> GetQuery(GetT input)
        {
            return Repository
            .WhereIf(true, item => true)
            ;
        }
    }
    public class CrudApplication<T, GetT, CreateT> : BaseApplication<T, CreateT>, ICrudApplication<T, GetT, CreateT> where T : class, IBaseEntity where GetT : IBaseGet where CreateT : BaseCreate
    {
        public CrudApplication(IRepository<T> re) : base(re)
        { }

        public virtual IQueryable<T> GetQuery(GetT input)
        {
            return Repository
            .WhereIf(true, item => true)
            ;
        }
        public virtual async Task<ICollection<T>> FindQueryAsync(GetT input)
        {
            return await Repository.FindQuery(GetQuery(input)).ToListAsync();
        }
        public virtual async Task<PageData<T>> FindPageAsync(GetT input, PageRequest page)
        {
            return await Repository.FindPageAsync(GetQuery(input), page);
        }
    }
    public class QueryApplication<T, GetT, CreateT> : BaseApplication<T, CreateT>, IQueryApplication<T, GetT, CreateT> where T : class, IBaseEntity where GetT : IBaseGet<T> where CreateT : BaseCreate
    {
        public QueryApplication(IRepository<T> re) : base(re)
        { }

        public virtual IQueryable<T> GetQuery(GetT input)
        {
            return input.GetExpression(Repository
            .WhereIf(true, item => true)
            );
        }
        public virtual async Task<ICollection<T>> FindQueryAsync(GetT input)
        {
            return await Repository.FindQuery(GetQuery(input)).ToListAsync();
        }
        public virtual async Task<PageData<T>> FindPageAsync(GetT input, PageRequest page)
        {
            return await Repository.FindPageAsync(GetQuery(input), page);
        }
    }
    public class QueryApplication<T, Return, GetT, CreateT> : BaseApplication<T, Return, CreateT>, IQueryApplication<T, Return, GetT, CreateT>
    where T : class, IBaseEntity
    where Return : BaseReturn
    where GetT : IBaseGet<T>
    where CreateT : BaseCreate
    {
        public QueryApplication(IRepository<T> re) : base(re) { }

        public virtual IQueryable<T> GetQuery(GetT input)
        {
            return input.GetExpression(Repository
            .WhereIf(true, item => true)
            );
        }
        public async Task<PageData<Return>> FindPageAsync(GetT input, PageRequest page)
        {
            var result = await Repository.FindPageAsync(GetQuery(input), page);
            return new PageData<Return>
            {
                Total = result.Total,
                Data = _mapper.Map<ICollection<Return>>(result.Data)
            };
        }

        public async Task<ICollection<Return>> FindQueryAsync(GetT input)
        {
            return _mapper.Map<ICollection<Return>>(await Repository.FindQuery(GetQuery(input)).ToListAsync());
        }
    }
}
