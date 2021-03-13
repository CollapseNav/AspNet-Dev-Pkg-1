using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AspNet.Dev.Pkg.Infrastructure.Dto;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using AspNet.Dev.Pkg.Infrastructure.Unit;
using AspNet.Dev.Pkg.Infrastructure.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Microsoft.AspNetCore.Http;

namespace AspNet.Dev.Pkg.Infrastructure.Application
{
    public class BaseApplication<T, CreateT> : IBaseApplication<T, CreateT> where T : class, IBaseEntity where CreateT : BaseCreate
    {
        protected string AppKey = string.Empty;
        protected IRepository<T> Repository;
        protected readonly ConnectionMultiplexer _conn;
        protected readonly IDatabase _cache;
        protected readonly IMapper _mapper;
        protected IdentityUser<Guid> CurrentUser = null;
        public BaseApplication(IRepository<T> re)
        {
            Repository = re;
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

        public void SetCurrentUser(IdentityUser<Guid> user)
        {
            if (CurrentUser == null)
            {
                CurrentUser = user;
                Repository.SetCurrentUser(user);
            }
        }

        public virtual IQueryable<T> FindQuery(Expression<Func<T, bool>> exp)
        {
            var query = Repository.FindQuery(exp);
            return query;
        }

        public virtual async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> exp)
        {
            return await Repository.FindQuery(exp).ToListAsync();
        }

        public virtual async Task<PageData<T>> FindPageAsync(PageRequest page)
        {
            return await Repository.FindPageAsync(FindQuery(null), page);
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
            model.Init();
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
