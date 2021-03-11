
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AspNet.Dev.Pkg.Infrastructure.Interface;
using AspNet.Dev.Pkg.Infrastructure.Unit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace AspNet.Dev.Pkg.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class, IBaseEntity, new()
    {
        protected readonly DbContext _db;

        protected readonly DbSet<T> dbSet;
        protected IdentityUser<Guid> CurrentUser = null;

        /// <summary>
        /// 初始化上下文
        /// </summary>
        public Repository(DbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }

        public virtual void SetCurrentUser(IdentityUser<Guid> user)
        {
            if (CurrentUser == null)
                CurrentUser = user;
        }

        /// <summary>
        /// 添加数据(单个)
        /// </summary>
        /// <param name="entity">新的数据</param>
        public virtual async Task AddAsync(T entity)
        {
            entity.Init(CurrentUser);
            await dbSet.AddAsync(entity);
        }

        /// <summary>
        /// 添加数据(集合)
        /// </summary>
        /// <param name="entityList">新的数据集合</param>
        public virtual async Task AddRangeAsync(ICollection<T> entityList)
        {
            foreach (var entity in entityList)
                entity.Init(CurrentUser);
            await dbSet.AddRangeAsync(entityList);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity">需要删除的数据</param>
        public virtual async Task DeleteAsync(T entity)
        {
            entity.SoftDelete(CurrentUser);
            await UpdateAsync(entity);
        }

        /// <summary>
        /// 有条件地删除数据
        /// </summary>
        /// <param name="exp">筛选条件</param>
        public virtual async Task<int> DeleteAsync(Expression<Func<T, bool>> exp)
        {
            return await dbSet.Where(exp).DeleteAsync();
        }

        /// <summary>
        /// 根据id删除数据
        /// </summary>
        /// <param name="id">主键ID</param>
        public virtual async Task DeleteByIDAsync(Guid id)
        {
            var entity = await FindByIDAsync(id);
            await DeleteAsync(entity);
        }
        /// <summary>
        /// 根据id删除数据
        /// </summary>
        /// <param name="id">主键ID</param>
        public virtual async Task<int> DeleteByIDsAsync(ICollection<Guid> id)
        {
            foreach (var item in id)
                await DeleteByIDAsync(item);
            return id.Count;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        public virtual async Task UpdateAsync(T entity)
        {
            entity.Update(CurrentUser);
            var entry = _db.Entry(entity);
            entry.State = EntityState.Modified;
            await Task.FromResult("");
        }

        /// <summary>
        /// 实现按需要只更新部分更新
        /// <para>如：Update(u =>u.Id==1,u =>new User{Name="ok"});</para>
        /// </summary>
        public virtual async Task<int> UpdateAsync(Expression<Func<T, bool>> where, Expression<Func<T, T>> entity)
        {
            return await dbSet.Where(where).UpdateAsync(entity);
        }
        /// <summary>
        /// 查询单个数据
        /// </summary>
        public virtual async Task<T> FindSingleAsync(Expression<Func<T, bool>> exp = null)
        {
            return await FindQuery(exp).FirstOrDefaultAsync();
        }
        /// <summary>
        /// 根据id查询数据
        /// </summary>
        /// <param name="id">主键ID</param>
        public virtual async Task<T> FindByIDAsync(Guid id)
        {
            var model = await _db.FindAsync<T>(id);
            return (T)model.Entity();
        }

        /// <summary>
        /// 查询所有符合条件的数据
        /// </summary>
        /// <param name="exp">筛选条件
        /// PS:若使用默认的NULL，则返回所有数据
        /// </param>
        public virtual IQueryable<T> FindQuery(Expression<Func<T, bool>> exp = null)
        {
            if (exp != null)
            {
                var query = dbSet.Where(exp);
                if (!query.Any())
                    return null;
                return query.AsNoTracking().AsQueryable();
            }
            return dbSet.AsQueryable();
        }

        /// <summary>
        /// 查询所有符合条件的数据
        /// </summary>
        /// <param name="exp">筛选条件
        /// PS:若使用默认的NULL，则返回所有数据
        /// </param>
        public virtual IQueryable<T> FindQuery(IQueryable<T> exp)
        {
            if (exp != null)
                _db.Filter<T>(q => exp);
            return dbSet.AsNoTracking().AsQueryable();
        }


        /// <summary>
        /// 分页查找所有符合条件的数据
        /// </summary>
        /// <param name="isAsc">是否正序</param>
        /// <param name="exp">筛选条件</param>
        /// <param name="pageindex">分页页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="orderBy">排序规则</param>
        public virtual async Task<PageData<T>> FindPageAsync<TKey>(Expression<Func<T, bool>> exp = null, Expression<Func<T, TKey>> orderBy = null, int pageindex = 1, int pageSize = 10, bool isAsc = true)
        {
            int total = await CountAsync(exp);
            pageindex = pageindex > 0 ? pageindex : 1;
            var skipSize = pageSize * (pageindex - 1);
            List<T> data = new();

            if (isAsc)
            {
                data = orderBy == null
                ? await FindQuery(exp).Skip(skipSize).Take(pageSize).ToListAsync()
                : await FindQuery(exp).OrderBy(orderBy).Skip(skipSize).Take(pageSize).ToListAsync();
            }
            else
            {
                data = orderBy == null
                ? await FindQuery(exp).Skip(skipSize).Take(pageSize).ToListAsync()
                : await FindQuery(exp).OrderByDescending(orderBy).Skip(skipSize).Take(pageSize).ToListAsync();
            }

            return new PageData<T>
            {
                Total = total,
                Data = data
            };
        }
        /// <summary>
        /// 分页查找所有符合条件的数据
        /// </summary>
        public virtual async Task<PageData<T>> FindPageAsync<TKey>(Expression<Func<T, bool>> exp = null, Expression<Func<T, TKey>> orderBy = null, PageRequest page = null, bool isAsc = true)
        {
            int total = await CountAsync(exp);
            if (page == null)
                page = new PageRequest();
            List<T> data = new();

            if (isAsc)
            {
                data = orderBy == null
                ? await FindQuery(exp).Skip(page.Skip).Take(page.Max).ToListAsync()
                : await FindQuery(exp).OrderBy(orderBy).Skip(page.Skip).Take(page.Max).ToListAsync();
            }
            else
            {
                data = orderBy == null
                ? await FindQuery(exp).Skip(page.Skip).Take(page.Max).ToListAsync()
                : await FindQuery(exp).OrderByDescending(orderBy).Skip(page.Skip).Take(page.Max).ToListAsync();
            }

            return new PageData<T>
            {
                Total = total,
                Data = data
            };
        }

        public virtual async Task<PageData<T>> FindPageAsync(IQueryable<T> exp = null, int pageindex = 1, int pageSize = 10)
        {
            int total = await CountAsync(exp);
            pageindex = pageindex > 0 ? pageindex : 1;
            var skipSize = pageSize * (pageindex - 1);

            return new PageData<T>
            {
                Total = total,
                Data = await FindQuery(exp).Skip(skipSize).Take(pageSize).ToListAsync()
            };
        }
        public virtual async Task<PageData<T>> FindPageAsync(IQueryable<T> exp = null, PageRequest page = null)
        {
            int total = await CountAsync(exp);
            if (page == null)
                page = new PageRequest();
            return new PageData<T>
            {
                Total = total,
                Data = await FindQuery(exp).Skip(page.Skip).Take(page.Max).ToListAsync()
            };
        }

        /// <summary>
        /// 判断是否有符合条件的数据
        /// </summary>
        public virtual async Task<bool> IsExistAsync(Expression<Func<T, bool>> exp)
        {
            return await dbSet.AnyAsync(exp);
        }

        /// <summary>
        /// 计算符合条件的数据数量
        /// </summary>
        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> exp = null)
        {
            return await FindQuery(exp).CountAsync();
        }
        /// <summary>
        /// 计算符合条件的数据数量
        /// </summary>
        public virtual async Task<int> CountAsync(IQueryable<T> exp = null)
        {
            return await FindQuery(exp).CountAsync();
        }

        /// <summary>
        /// 保存修改
        /// </summary>
        public virtual async Task<int> SaveAsync()
        {
            return await _db.SaveChangesAsync();
        }
        /// <summary>
        /// 运行sql语句
        /// </summary>
        public virtual async Task<int> ExeSqlAsync(string sql) => await _db.Database.ExecuteSqlRawAsync(sql);

        public virtual DbContext GetContext() => _db;

        public virtual DbSet<T> GetDbSet() => dbSet;
    }
}
