using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AspNet.Dev.Pkg.Infrastructure.Dto;
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
            if (provider != null)
            {
                _mapper = provider.GetService<IMapper>();
                _conn = provider.GetService<ConnectionMultiplexer>();
                if (_conn != null)
                    _cache = _conn.GetDatabase();
            }
        }
        public IdentityUser<Guid> GetCurrentUser()
        {
            return CurrentUser;
        }
    }
    public class BaseApplication<T, Return, CreateT> : BaseApplication<T, CreateT>, IBaseApplication<T, Return, CreateT>
    where T : class, IBaseEntity
    where Return : BaseReturn
    where CreateT : BaseCreate
    {
        public BaseApplication(IRepository<T> re) : base(re)
        {
        }

        public new async Task<Return> AddAsync(CreateT entity)
        {
            T model = _mapper.Map<T>(entity);
            await Repository.AddAsync(model);
            return _mapper.Map<Return>(model);
        }

        public new async Task<Return> FindAsync(Guid id)
        {
            return _mapper.Map<Return>(await Repository.FindByIDAsync(id));
        }
    }
    public class BaseApplication<T, CreateT> : BaseApplication, IBaseApplication<T, CreateT> where T : class, IBaseEntity where CreateT : BaseCreate
    {
        protected string AppKey = string.Empty;
        protected IRepository<T> Repository;
        public BaseApplication(IRepository<T> re)
        {
            Repository = re;
        }


        public virtual async Task<int> SaveChangesAsync()
        {
            return await Repository.SaveAsync();
        }
        public virtual int SaveChanges()
        {
            return Repository.Save();
        }

        public virtual async Task<T> AddAsync(CreateT entity)
        {
            T model = _mapper.Map<T>(entity);
            await Repository.AddAsync(model);
            return model;
        }

        public virtual async Task<int> AddRangeAsync(ICollection<CreateT> entitys)
        {

            ICollection<T> models = _mapper.Map<ICollection<T>>(entitys);
            await Repository.AddRangeAsync(models);
            return models.Count;
        }

        public virtual async Task<T> FindAsync(Guid id)
        {
            return await Repository.FindByIDAsync(id);
        }


        public virtual async Task DeleteAsync(Guid id)
        {
            await Repository.DeleteByIDAsync(id);
        }

        public virtual async Task<int> DeleteRangeAsync(ICollection<Guid> ids)
        {
            int num = await Repository.DeleteByIDsAsync(ids);
            return num;
        }

        public virtual async Task UpdateAsync(Guid id, CreateT entity)
        {
            await Repository.UpdateAsync(_mapper.Map(entity, await FindAsync(id)));
        }

        public DbContext GetDbContext() => Repository.GetContext();
    }
}
