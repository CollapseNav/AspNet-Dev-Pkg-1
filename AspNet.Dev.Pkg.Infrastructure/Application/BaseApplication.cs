using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using AspNet.Dev.Pkg.Infrastructure.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace AspNet.Dev.Pkg.Infrastructure.Application
{
    public class BaseApplication : IBaseApplication
    {
        protected readonly ConnectionMultiplexer _conn;
        protected readonly IDatabase _cache;
        protected readonly IMapper _mapper;
        protected IdentityUser<Guid> CurrentUser = null;
        public BaseApplication()
        {
            var provider = ServiceGet.GetProvider();
            _mapper = provider?.GetService<IMapper>();
            _conn = provider?.GetService<ConnectionMultiplexer>();
            _cache = _conn?.GetDatabase();
        }
        public IdentityUser<Guid> GetCurrentUser() => CurrentUser;
    }
    public class BaseApplication<T, CreateT> : BaseApplication, IBaseApplication<T, CreateT>
    where T : class, IBaseEntity
    where CreateT : IBaseCreate
    {
        protected string AppKey = string.Empty;
        protected IRepository<T> Repository;
        public BaseApplication(IRepository<T> re) => Repository = re;

        public virtual async Task<int> SaveChangesAsync() => await Repository.SaveAsync();
        public virtual int SaveChanges() => Repository.Save();

        public virtual async Task<T> AddAsync(CreateT entity) => await Repository.AddAsync(_mapper.Map<T>(entity));

        public virtual async Task<int> AddRangeAsync(ICollection<CreateT> entitys) => await Repository.AddRangeAsync(_mapper.Map<ICollection<T>>(entitys));

        public virtual async Task<T> FindAsync(Guid id) => await Repository.FindByIDAsync(id);


        public virtual async Task DeleteAsync(Guid id) => await Repository.DeleteByIDAsync(id);

        public virtual async Task<int> DeleteRangeAsync(ICollection<Guid> ids) => await Repository.DeleteByIDsAsync(ids);

        public virtual async Task UpdateAsync(Guid id, CreateT entity)
        {
            await Repository.UpdateAsync(_mapper.Map(entity, await FindAsync(id)));
        }

        public DbContext GetDbContext() => Repository.GetContext();

        public virtual async Task<Target> AddAsync<Target>(CreateT entity) => _mapper.Map<Target>(await AddAsync(entity));

        public virtual async Task<Target> FindAsync<Target>(Guid id) => _mapper.Map<Target>(await FindAsync(id));
    }
    public class BaseApplication<T, Return, CreateT> : BaseApplication<T, CreateT>, IBaseApplication<T, Return, CreateT>
    where T : class, IBaseEntity
    where Return : IBaseReturn
    where CreateT : IBaseCreate
    {
        public BaseApplication(IRepository<T> re) : base(re) { }

        public new async Task<Return> AddAsync(CreateT entity) => _mapper.Map<Return>(await Repository.AddAsync(_mapper.Map<T>(entity)));

        public new async Task<Return> FindAsync(Guid id) => _mapper.Map<Return>(await Repository.FindByIDAsync(id));
    }
}
