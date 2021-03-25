using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AspNet.Dev.Pkg.Infrastructure.Unit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Dev.Pkg.Infrastructure.Interface
{
    public interface IRepository<T> where T : class, IBaseEntity
    {
        void SetCurrentUser(IdentityUser<Guid> user);
        Task<T> AddAsync(T entity);
        Task<int> AddRangeAsync(ICollection<T> entityList);
        Task<bool> DeleteAsync(T entity, bool isTrue = false);
        Task<int> DeleteAsync(Expression<Func<T, bool>> exp, bool isTrue = false);
        Task<bool> DeleteByIDAsync(Guid id, bool isTrue = false);
        Task<int> DeleteByIDsAsync(ICollection<Guid> id, bool isTrue = false);
        Task UpdateAsync(T entity);
        Task<int> UpdateAsync(Expression<Func<T, bool>> where, Expression<Func<T, T>> entity);
        Task<T> FindSingleAsync(Expression<Func<T, bool>> exp = null);
        Task<T> FindByIDAsync(Guid id);
        IQueryable<T> FindQuery(Expression<Func<T, bool>> exp = null);
        IQueryable<T> FindQuery(IQueryable<T> exp);
        Task<PageData<T>> FindPageAsync<TKey>(Expression<Func<T, bool>> exp = null, Expression<Func<T, TKey>> orderBy = null, int pageindex = 1, int pageSize = 10, bool isAsc = true);
        Task<PageData<T>> FindPageAsync<TKey>(Expression<Func<T, bool>> exp = null, Expression<Func<T, TKey>> orderBy = null, PageRequest page = null, bool isAsc = true);
        Task<PageData<T>> FindPageAsync(IQueryable<T> exp = null, int pageindex = 1, int pageSize = 10);
        Task<PageData<T>> FindPageAsync(IQueryable<T> exp = null, PageRequest page = null);
        Task<bool> IsExistAsync(Expression<Func<T, bool>> exp);
        Task<int> CountAsync(Expression<Func<T, bool>> exp = null);
        Task<int> CountAsync(IQueryable<T> exp = null);
        Task<int> SaveAsync();
        int Save();
        Task<int> ExeSqlAsync(string sql);
        DbContext GetContext();
        DbSet<T> GetDbSet();
    }
}
