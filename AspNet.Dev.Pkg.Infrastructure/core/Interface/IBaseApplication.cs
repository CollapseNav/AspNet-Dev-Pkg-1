
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AspNet.Dev.Pkg.Infrastructure.Dto;
using AspNet.Dev.Pkg.Infrastructure.Entity;
using AspNet.Dev.Pkg.Infrastructure.Unit;
using Microsoft.AspNetCore.Identity;

namespace AspNet.Dev.Pkg.Infrastructure.Interface
{
    public interface IBaseApplication<T, CreateT> where T : IBaseEntity where CreateT : BaseCreate
    {
        IdentityUser<Guid> GetCurrentUser();
        void SetCurrentUser(IdentityUser<Guid> user);
        IQueryable<T> FindQuery(Expression<Func<T, bool>> exp);
        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> exp);
        Task<PageData<T>> FindPageAsync(PageRequest page);
        Task<int> SaveChangesAsync();
        Task<T> AddAsync(CreateT entity);
        Task<int> AddRangeAsync(ICollection<CreateT> entitys);
        Task<T> FindAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task<int> DeleteRangeAsync(Guid[] ids);
        Task UpdateAsync(Guid id, CreateT entity);
    }
}
