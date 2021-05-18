using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AspNet.Dev.Pkg.Infrastructure.Unit;

namespace AspNet.Dev.Pkg.Infrastructure.Interface
{
    public interface IReadonlyRepository<T> : IRepository<T>, IRepository where T : class, IBaseEntity
    {
        Task<IReadOnlyCollection<T>> FindAsync(Expression<Func<T, bool>> exp);
        Task<IReadOnlyCollection<T>> FindAsync(ICollection<Guid> ids);
        Task<T> FindAsync(Guid id);
        Task<PageData<T>> FindPageAsync<TKey>(Expression<Func<T, bool>> exp = null, PageRequest page = null, Expression<Func<T, TKey>> orderBy = null, bool isAsc = true);
        Task<PageData<T>> FindPageAsync<TKey>(Expression<Func<T, bool>> exp = null, int pageindex = 1, int pageSize = 10, Expression<Func<T, TKey>> orderBy = null, bool isAsc = true);
        Task<PageData<T>> FindPageAsync(IQueryable<T> query = null, int pageindex = 1, int pageSize = 10);
        Task<PageData<T>> FindPageAsync(IQueryable<T> query = null, PageRequest page = null);
    }
}
