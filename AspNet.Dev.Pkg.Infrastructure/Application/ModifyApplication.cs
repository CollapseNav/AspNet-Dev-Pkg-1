using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using AspNet.Dev.Pkg.Infrastructure.Util;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Dev.Pkg.Infrastructure.Application
{
    public class ModifyApplication<T, CreateT> : Application, IModifyApplication<T, CreateT>
    where T : class, IBaseEntity
    where CreateT : IBaseCreate
    {
        protected IModifyRepository<T> Repository;
        public ModifyApplication()
        {
            Repository = ServiceGet.GetProvider()?.GetService<IModifyRepository<T>>();
        }
        public virtual async Task<int> SaveChangesAsync() => await Repository.SaveAsync();
        public virtual int SaveChanges() => Repository.Save();
        public virtual async Task<T> AddAsync(CreateT entity) => await Repository.AddAsync(_mapper.Map<T>(entity));
        public virtual async Task<int> AddRangeAsync(ICollection<CreateT> entitys) => await Repository.AddRangeAsync(entitys.Maps<T>().ToList());
        public virtual async Task DeleteAsync(Guid id) => await Repository.DeleteAsync(id);
        public virtual async Task<int> DeleteRangeAsync(ICollection<Guid> ids) => await Repository.DeleteAsync(ids);
        public virtual async Task UpdateAsync(Guid id, CreateT entity)
        {
            var model = entity.Map<T>();
            model.Id = id;
            await Repository.UpdateAsync(model);
        }
        public DbContext GetDbContext() => Repository.GetContext();
        public virtual async Task<Target> AddAsync<Target>(CreateT entity) => _mapper.Map<Target>(await AddAsync(entity));

    }
    public class ModifyApplication<T, CreateT, Return> : ModifyApplication<T, CreateT>, IModifyApplication<T, CreateT, Return>
    where T : class, IBaseEntity
    where Return : IBaseReturn
    where CreateT : IBaseCreate
    {
        public ModifyApplication() : base() { }
        public virtual new async Task<Return> AddAsync(CreateT entity) => _mapper.Map<Return>(await Repository.AddAsync(_mapper.Map<T>(entity)));

    }
}
