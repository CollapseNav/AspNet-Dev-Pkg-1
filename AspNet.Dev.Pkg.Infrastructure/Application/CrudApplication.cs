using AspNet.Dev.Pkg.Infrastructure.Interface;
using AspNet.Dev.Pkg.Infrastructure.Util;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using AspNet.Dev.Pkg.Infrastructure.Unit;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Dev.Pkg.Infrastructure.Application
{
    public class CrudApplication<T, GetT, CreateT> : Application, ICrudApplication<T, GetT, CreateT>
    where T : class, IBaseEntity
    where GetT : IBaseGet<T>
    where CreateT : IBaseCreate
    {
        protected IModifyApplication<T, CreateT> Write;
        protected IReadonlyApplication<T, GetT> Read;
        public CrudApplication() : base()
        {
            var provider = ServiceGet.GetProvider();
            Write = ServiceGet.GetProvider()?.GetService<IModifyApplication<T, CreateT>>();
            Read = ServiceGet.GetProvider()?.GetService<IReadonlyApplication<T, GetT>>();
        }

        public Task<Target> AddAsync<Target>(CreateT entity) => Write.AddAsync<Target>(entity);

        public Task<T> AddAsync(CreateT entity) => Write.AddAsync(entity);

        public Task<int> AddRangeAsync(ICollection<CreateT> entitys) => Write.AddRangeAsync(entitys);


        public Task DeleteAsync(Guid id) => Write.DeleteAsync(id);


        public Task<int> DeleteRangeAsync(ICollection<Guid> ids) => Write.DeleteRangeAsync(ids);


        public Task<T> FindAsync(Guid id) => Read.FindAsync(id);


        public Task<Target> FindAsync<Target>(Guid id) => Read.FindAsync<Target>(id);


        public Task<IReadOnlyCollection<T>> FindByIdsAsync(ICollection<Guid> ids) => Read.FindByIdsAsync(ids);


        public Task<PageData<T>> FindPageAsync(GetT input, PageRequest page) => Read.FindPageAsync(input, page);


        public Task<PageData<Target>> FindPageAsync<Target>(GetT input, PageRequest page) => Read.FindPageAsync<Target>(input, page);


        public Task<IReadOnlyCollection<T>> FindQueryAsync(GetT input) => Read.FindQueryAsync(input);


        public Task<IReadOnlyCollection<Target>> FindQueryAsync<Target>(GetT input) => Read.FindQueryAsync<Target>(input);

        public DbContext GetDbContext() => Write.GetDbContext();

        public IQueryable<T> GetQuery(GetT input) => Read.GetQuery(input);

        public int SaveChanges() => Write.SaveChanges();

        public Task<int> SaveChangesAsync() => Write.SaveChangesAsync();

        public Task UpdateAsync(Guid id, CreateT entity) => Write.UpdateAsync(id, entity);

    }
}
