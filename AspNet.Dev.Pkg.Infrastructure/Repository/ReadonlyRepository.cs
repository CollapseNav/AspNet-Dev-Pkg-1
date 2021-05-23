
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using AspNet.Dev.Pkg.Infrastructure.Unit;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Dev.Pkg.Infrastructure.Repository
{
    public class ReadonlyRepository<T> : Repository<T>, IReadonlyRepository<T>, IRepository<T>, IRepository
    where T : class, IBaseEntity, new()
    {
        public ReadonlyRepository(DbContext db) : base(db) { }

        public virtual async Task<T> FindAsync(Guid id)
        {
            return await dbSet.AsNoTracking().FirstOrDefaultAsync(item => item.Id.Value == id);
        }


        /// <summary>
        /// 查询数据
        /// </summary>
        public virtual async Task<IReadOnlyCollection<T>> FindAsync(Expression<Func<T, bool>> exp)
        {
            return await FindQuery(exp).ToListAsync();
        }

        public virtual async Task<IReadOnlyCollection<T>> FindAsync(ICollection<Guid> ids)
        {
            return await FindAsync(item => ids.Contains(item.Id.Value));
        }


        /// <summary>
        /// 查询所有符合条件的数据
        /// </summary>
        /// <param name="exp">筛选条件
        /// PS:若使用默认的NULL，则返回所有数据
        /// </param>
        public override IQueryable<T> FindQuery(Expression<Func<T, bool>> exp)
        {
            var query = base.FindQuery(exp).AsNoTracking();
            return query;
        }

        public override IQueryable<T> FindQuery(IQueryable<T> query)
        {
            return base.FindQuery(query).AsNoTracking();
        }

        /// <summary>
        /// 分页查找所有符合条件的数据
        /// </summary>
        /// <param name="isAsc">是否正序</param>
        /// <param name="exp">筛选条件</param>
        /// <param name="pageindex">分页页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="orderBy">排序规则</param>
        public virtual async Task<PageData<T>> FindPageAsync<TKey>(Expression<Func<T, bool>> exp = null, int pageindex = 1, int pageSize = 10, Expression<Func<T, TKey>> orderBy = null, bool isAsc = true)
        {
            var query = FindQuery(exp);
            int total = await query.CountAsync();
            var page = new PageRequest { Index = pageindex, Max = pageSize };
            if (orderBy != null)
                query = isAsc ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy); ;

            return new PageData<T>
            {
                Total = total,
                Data = await query.Skip(page.Skip).Take(page.Max).ToListAsync()
            };
        }

        /// <summary>
        /// 分页查找所有符合条件的数据
        /// </summary>
        public virtual async Task<PageData<T>> FindPageAsync<TKey>(Expression<Func<T, bool>> exp = null, PageRequest page = null, Expression<Func<T, TKey>> orderBy = null, bool isAsc = true)
        {
            if (page == null)
                page = new PageRequest();
            var query = FindQuery(exp);
            int total = await query.CountAsync();

            if (orderBy != null)
                query = isAsc ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

            return new PageData<T>
            {
                Total = total,
                Data = await query.Skip(page.Skip).Take(page.Max).ToListAsync()
            };
        }

        public virtual async Task<PageData<T>> FindPageAsync(IQueryable<T> query = null, int pageindex = 1, int pageSize = 10)
        {
            query = FindQuery(query);
            int total = await query.CountAsync();
            var page = new PageRequest { Index = pageindex, Max = pageSize };

            return new PageData<T>
            {
                Total = total,
                Data = await query.Skip(page.Skip).Take(page.Max).ToListAsync()
            };
        }
        public virtual async Task<PageData<T>> FindPageAsync(IQueryable<T> query = null, PageRequest page = null)
        {
            query = FindQuery(query);
            int total = await query.CountAsync();
            if (page == null)
                page = new PageRequest();
            return new PageData<T>
            {
                Total = total,
                Data = await query.Skip(page.Skip).Take(page.Max).ToListAsync()
            };
        }



    }
}
