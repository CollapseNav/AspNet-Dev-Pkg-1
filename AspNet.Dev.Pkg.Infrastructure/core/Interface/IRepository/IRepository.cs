using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Dev.Pkg.Infrastructure.Interface
{
    public interface IRepository
    {

    }
    public interface IRepository<T> : IRepository where T : class, IBaseEntity
    {
        IQueryable<T> FindQuery(Expression<Func<T, bool>> exp);
        IQueryable<T> FindQuery(IQueryable<T> query);
        Task<bool> IsExistAsync(Expression<Func<T, bool>> exp);
        Task<int> CountAsync(Expression<Func<T, bool>> exp);
        Task<int> CountAsync(IQueryable<T> query);
        Task<int> SaveAsync();
        int Save();
        Task<int> ExeSqlAsync(string sql);
        DbContext GetContext();
        DbSet<T> GetDbSet();
    }
}
