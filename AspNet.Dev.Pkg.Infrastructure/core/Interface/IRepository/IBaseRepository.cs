using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AspNet.Dev.Pkg.Infrastructure.Interface
{
    public interface IModifyRepository<T> : IRepository<T>, IRepository where T : class, IBaseEntity
    {
        Task<T> AddAsync(T entity);
        Task<int> AddRangeAsync(ICollection<T> entityList);

        Task<bool> DeleteAsync(T entity, bool isTrue = false);
        Task<bool> DeleteAsync(Guid id, bool isTrue = false);
        Task<int> DeleteAsync(Expression<Func<T, bool>> exp, bool isTrue = false);
        Task<int> DeleteAsync(ICollection<Guid> id, bool isTrue = false);

        Task UpdateAsync(T entity);
        Task<int> UpdateAsync(Expression<Func<T, bool>> where, Expression<Func<T, T>> entity);
    }
}
