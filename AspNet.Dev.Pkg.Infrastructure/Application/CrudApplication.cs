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
    public class CrudApplication<T, GetT, CreateT> : BaseApplication<T, CreateT>, ICrudApplication<T, GetT, CreateT> where T : class, IBaseEntity where GetT : BaseGet where CreateT : BaseCreate
    {
        public CrudApplication(IRepository<T> re) : base(re)
        { }

        public virtual IQueryable<T> GetQuery(GetT input)
        {
            return Repository
            .WhereIf(true, item => true)
            ;
        }
        public virtual async Task<ICollection<T>> FindAllAsync(GetT input)
        {
            return await Repository.FindQuery(GetQuery(input)).ToListAsync();
        }
        public virtual async Task<PageData<T>> FindPageAsync(GetT input, PageRequest page)
        {
            return await Repository.FindPageAsync(GetQuery(input), page);
        }
    }
    public class QCrudApplication<T, GetT, CreateT> : BaseApplication<T, CreateT>, IQCrudApplication<T, GetT, CreateT> where T : class, IBaseEntity where GetT : BaseGet<T> where CreateT : BaseCreate
    {
        public QCrudApplication(IRepository<T> re) : base(re)
        { }

        public virtual IQueryable<T> GetQuery(GetT input)
        {
            return input.GetExpression(Repository
            .WhereIf(true, item => true)
            );
        }
        public virtual async Task<ICollection<T>> FindAllAsync(GetT input)
        {
            return await Repository.FindQuery(GetQuery(input)).ToListAsync();
        }
        public virtual async Task<PageData<T>> FindPageAsync(GetT input, PageRequest page)
        {
            return await Repository.FindPageAsync(GetQuery(input), page);
        }
    }
}
